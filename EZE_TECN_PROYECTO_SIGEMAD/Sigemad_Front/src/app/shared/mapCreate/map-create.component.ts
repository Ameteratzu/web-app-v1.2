import 'ol/ol.css';
import "ol-ext/dist/ol-ext.css"

import { Component, EventEmitter, inject, Input, Output, signal, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import Feature from 'ol/Feature';
import Point from 'ol/geom/Point';
import { Draw, Snap } from 'ol/interaction';
import { DrawEvent } from 'ol/interaction/Draw';
import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import Map from 'ol/Map';
import { OSM, Vector as VectorSource, XYZ } from 'ol/source';
import View from 'ol/View';
import LayerGroup from 'ol/layer/Group';
import { defaults as defaultControls, FullScreen, ScaleLine } from 'ol/control';
import LayerSwitcher from 'ol-ext/control/LayerSwitcher';
import TileWMS from 'ol/source/TileWMS';

import { MunicipalityService } from '../../services/municipality.service';

import { CommonModule } from '@angular/common';
import { Geometry, Polygon } from 'ol/geom';
import { fromLonLat, toLonLat } from 'ol/proj';
import { Municipality } from '../../types/municipality.type';

import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import Icon from 'ol/style/Icon';
import Style from 'ol/style/Style';
import proj4 from 'proj4';
import { environment } from '../../../environments/environment';

// Define the projection for UTM zone 30N (EPSG:25830)
const utm30n = "+proj=utm +zone=30 +ellps=GRS80 +units=m +no_defs";

@Component({
  selector: 'app-map-create',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, FlexLayoutModule],
  templateUrl: './map-create.component.html',
  styleUrls: ['./map-create.component.css'],
})
export class MapCreateComponent implements OnInit {
  @Input() municipio: any;
  @Input() listaMunicipios: any;
  @Input() onlyView: any = null;

  @Output() save = new EventEmitter<Feature<Geometry>[]>();

  public source!: VectorSource;
  public map!: Map;
  public view!: View;
  public draw!: Draw;
  public snap!: Snap;
  public vector!: VectorLayer;
  public coords: any;

  public data = inject(MAT_DIALOG_DATA);
  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);

  public municipalityService = inject(MunicipalityService);

  public municipalities = signal<Municipality[]>([]);

  public municipioSelected = signal(this.data?.municipio || {});

  public length!: number;
  public latitude!: number;

  public section: string = '';

  public coordinates: string = 'Lon: 0.0000, Lat: 0.0000'; 
  public cursorPosition = { x: 0, y: 0 }; 

  async ngOnInit() {
    const { municipio, listaMunicipios, defaultPolygon, onlyView } = this.data;

    this.configureMap(municipio, defaultPolygon, onlyView);
  }

  configureMap(municipio: any, defaultPolygon: any, onlyView: any) {

    let defaultPolygonMercator;

    if (defaultPolygon) {
      defaultPolygonMercator = defaultPolygon.map((coord: any) => fromLonLat(coord));
    }

    let baseLayers = new LayerGroup({
      properties: { 'title': 'Capas base', openInLayerSwitcher: true },
      layers: [
        new TileLayer({
          source: new OSM(),
          properties: { 'title': 'OpenStreetMap', baseLayer: true },
          visible: true

        }),
        new TileLayer({
          source: new XYZ({
            url: 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}'
          }),
          properties: { 'title': 'Satélite', baseLayer: true, },
          visible: false
        })
      ]
    });    

    this.source = new VectorSource();

    this.vector = new VectorLayer({
      source: this.source,
      style: {
        'stroke-color': 'rgb(252, 5, 5)', 
        'stroke-width': 5,
      },
      properties: { 'title': 'Área afectada' }
    });

    const wmsNucleosPoblacion = new TileLayer({
      source: new TileWMS({
        url: environment.urlGeoserver,
        params: {
          'LAYERS': 'nucleos_poblacion',
          'TILED': true,
        },
        serverType: 'geoserver', 
        transition: 0,
      }),
      properties: { 'title': 'Núcleos de población' }
    });

    const wmsLimitesMunicipio = new TileLayer({
      source: new TileWMS({
        url: environment.urlGeoserver,
        params: {
          'LAYERS': 'limites_municipio',
          'TILED': true,
        },
        serverType: 'geoserver', 
        transition: 0,
      }),
      properties: { 'title': 'Límites municipio' }
    });    

    this.view = new View({
      center: fromLonLat(municipio.geoPosicion.coordinates),
      zoom: 12,
      projection: 'EPSG:3857',
    })

    this.map = new Map({
      controls: defaultControls({ 
        zoom: true, 
        zoomOptions: { 
          zoomInTipLabel: 'Acercar', 
          zoomOutTipLabel: 'Alejar' 
        } 
      }).extend([
        new FullScreen(({tipLabel: 'Pantalla completa'})),
      ]),
      target: 'map',
      layers: [baseLayers, wmsLimitesMunicipio, wmsNucleosPoblacion, this.vector], 
      view: this.view
    });

    this.map.addControl(new LayerSwitcher());
    this.map.addControl(new ScaleLine());

    const point = new Point(fromLonLat(municipio.geoPosicion.coordinates));

    const pointFeature = new Feature({
      geometry: point,
    });

    if (defaultPolygon) {
      const polygonFeature = new Feature({
        geometry: new Polygon([defaultPolygonMercator]),
      });

      this.source.addFeature(polygonFeature);
    }

    const pointLayer = new VectorLayer({
      source: new VectorSource({
        features: [pointFeature],
      }),
      properties: { 'title': 'Municipio' }
    });

    pointLayer.setStyle(
      new Style({
        image: new Icon({
          anchor: [1, 1],
          src: 'https://cdn-icons-png.flaticon.com/512/684/684908.png', 
          scale: 0.05,
        }),
      })
    );

    this.map.addLayer(pointLayer);

    if (!onlyView) {
      this.addInteractions();
    }

    this.map.on('pointermove', (event) => {
      const coords = toLonLat(event.coordinate);
      const [lon, lat] = [coords[0].toFixed(4), coords[1].toFixed(4)];

      this.coordinates = `Lon: ${lon}, Lat: ${lat}`;

      const pixel = this.map.getEventPixel(event.originalEvent);

      this.cursorPosition = {
        x: pixel[0],
        y: pixel[1] + 40,
      };
    });

    this.map.on('pointermove', (event) => {
      const coordinate = event.coordinate;
      const [lon, lat] = toLonLat(coordinate);
      const [x, y] = proj4('EPSG:4326', utm30n, [lon, lat]);
      const cursorCoordinatesElement = document.getElementById('cursor-coordinates');
      if (cursorCoordinatesElement) {
        cursorCoordinatesElement.innerText = `X: ${x.toFixed(2)}, Y: ${y.toFixed(2)}`;
      }
    });
  }

  changeMunicipio(event: any) {
    //console.info('event', event);
  }

  addInteractions() {
    this.draw = new Draw({
      source: this.source,
      type: 'Polygon',
    });

    this.draw.on('drawstart', (drawEvent: DrawEvent) => {
      this.coords = null;
      const features = this.source.getFeatures();
      const last = features[features.length - 1];
      this.source.removeFeature(last);
    });

    this.draw.on('drawend', (drawEvent: DrawEvent) => {
      const coords = [];

      for (let coord of drawEvent.target.sketchCoords_[0]) {
        coords.push(toLonLat(coord));
      }

      coords.push(coords[0]);

      this.coords = coords;
    });

    this.map.addInteraction(this.draw);
    this.snap = new Snap({ source: this.source });
    this.map.addInteraction(this.snap);
  }

  savePolygon() {
    if (this.coords) {
      localStorage.setItem('coordinates' + this.section, this.coords);
      localStorage.setItem('polygon' + this.section, '1');
      this.save.emit(this.coords);
      this.closeModal();
    }
  }

  closeModal() {
    this.matDialogRef.close();
  }

  searchCoordinates() {
    const utmX = (document.getElementById('utm-x') as HTMLInputElement).value;
    const utmY = (document.getElementById('utm-y') as HTMLInputElement).value;

    if (utmX && utmY) {
      const [lon, lat] = proj4(utm30n, 'EPSG:4326', [parseFloat(utmX), parseFloat(utmY)]);
      const coordinate = fromLonLat([lon, lat]);
      this.view.setCenter(coordinate);
      this.view.setZoom(13); 
    }
  }
}
