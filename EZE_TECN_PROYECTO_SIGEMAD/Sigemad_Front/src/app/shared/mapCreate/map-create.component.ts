import { Component, EventEmitter, inject, Input, Output, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import{FlexLayoutModule}from'@angular/flex-layout';
import{MatButtonModule}from'@angular/material/button';
import{CommonModule}from'@angular/common';
import{environment}from'../../../environments/environment';

import Feature from 'ol/Feature';
import Point from 'ol/geom/Point';
import { Draw, Snap, Select } from 'ol/interaction';
import { DrawEvent } from 'ol/interaction/Draw';
import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import Map from 'ol/Map';
import { OSM, Vector as VectorSource, XYZ } from 'ol/source';
import View from 'ol/View';
import LayerGroup from 'ol/layer/Group';
import { defaults as defaultControls, FullScreen, ScaleLine } from 'ol/control';
import LayerSwitcher from 'ol-ext/control/LayerSwitcher';
import TileWMS from 'ol/source/TileWMS';
import Icon from 'ol/style/Icon';
import Style from 'ol/style/Style';
import { Geometry, Polygon } from 'ol/geom';
import WKT from 'ol/format/WKT';
import Bar from 'ol-ext/control/Bar';
import Toggle from 'ol-ext/control/Toggle';
import Overlay from 'ol/Overlay';

import proj4 from 'proj4';
import { fromLonLat, toLonLat } from 'ol/proj';

import { MunicipalityService } from '../../services/municipality.service';
import { Municipality } from '../../types/municipality.type';

import '@fortawesome/fontawesome-free/css/all.min.css';
import 'ol/ol.css';
import 'ol-ext/dist/ol-ext.css';

// Define the projection for UTM zone 30N (EPSG:25830)
const utm30n = "+proj=utm +zone=30 +ellps=GRS80 +units=m +no_defs";

@Component({
  selector: 'app-map-create',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, FlexLayoutModule],
  templateUrl: './map-create.component.html',
  styleUrl: './map-create.component.css',
})
export class MapCreateComponent {
  @Input() municipio: any;
  @Input() listaMunicipios: any;
  @Input() onlyView: any = null;

  @Output() save = new EventEmitter<Feature<Geometry>[]>();

  public source!: VectorSource;
  public map!: Map;
  public view!: View;
  public drawPoint!: Draw;
  public drawPolygon!: Draw;
  public snap!: Snap;
  public layerAreasAfectadas!: VectorLayer;
  public coords: any;
  public select!: Select;

  public data = inject(MAT_DIALOG_DATA);
  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);

  public municipalityService = inject(MunicipalityService);

  public municipalities = signal<Municipality[]>([]);

  public municipioSelected = signal(this.data?.municipio || {});

  public length!: number;
  public latitude!: number;

  public section: string = '';

  public coordinates: string = '';
  public cursorPosition = { x: 0, y: 0 };

  public popup!: Overlay;

  async ngOnInit() {
    const { municipio, listaMunicipios, defaultPolygon, onlyView } = this.data;

    this.configureMap(municipio, defaultPolygon, onlyView);
  }

  configureMap(municipio: any, defaultPolygon: any, onlyView: any) {
    // capas base
    const baseLayers = new LayerGroup({
      properties: { 'title': 'Capas base', openInLayerSwitcher: true },
      layers: [
        new TileLayer({
          source: new TileWMS({
            url: 'https://www.ign.es/wms-inspire/mapa-rasterizado?',
            params: { 'LAYERS': 'mtn_rasterizado', 'FORMAT': 'image/jpeg' },
            attributions: '© Instituto Geográfico Nacional de España'
          }),
          properties: { 'title': 'IGN raster', baseLayer: true, },
          visible: false 
        }),
        new TileLayer({
          source: new TileWMS({
            url: 'https://www.ign.es/wms-inspire/pnoa-ma?',
            params: { 'LAYERS': 'OI.OrthoimageCoverage', 'FORMAT': 'image/jpeg' },
            attributions: '© PNOA - IGN España'
          }),
          properties: { 'title': 'IGN satélite', baseLayer: true, },
          visible: false 
        }),
        new TileLayer({
          source: new XYZ({
            url: 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}',
            attributions: 'Tiles © <a href="https://www.esri.com/">Esri</a> - Source: Esri, Maxar, Earthstar Geographics'
          }),
          properties: { 'title': 'Satélite', baseLayer: true, },
          visible: false
        }),
        new TileLayer({
          source: new OSM(),
          properties: { 'title': 'OpenStreetMap', baseLayer: true },
          visible: true
        }),
      ]
    });

    // capas wms administrativas
    const wmsNucleosPoblacion = new TileWMS({
      url: environment.urlGeoserver + 'wms?version=1.1.0',
      params: {
        'LAYERS': 'nucleos_poblacion',
        'TILED': true,
      },
      serverType: 'geoserver',
      transition: 0,
    });
    const wmsLayersGroup = new LayerGroup({
      properties: { 'title': 'Límites administrativos', 'openInLayerSwitcher': true },

      layers: [
        new TileLayer({
          source: wmsNucleosPoblacion,
          properties: { 'title': 'Núcleos de población' }
        }),
        new TileLayer({
          source: new TileWMS({
            url: environment.urlGeoserver + 'wms?version=1.1.0',
            params: {
              'LAYERS': 'municipio_limites',
              'TILED': true,
            },
            serverType: 'geoserver',
            transition: 0,
          }),
          properties: { 'title': 'Límites municipio' }
        }),
      ]
    });    

    // capas de incendios
    let defaultPolygonMercator;

    if (defaultPolygon) {
      defaultPolygonMercator = defaultPolygon.map((coord: any) => fromLonLat(coord));
    }    

    this.source = new VectorSource();

    this.layerAreasAfectadas = new VectorLayer({
      source: this.source,
      style: {
        'stroke-color': 'rgb(252, 5, 5)',
        'stroke-width': 5,
      },
      properties: { 'title': 'Área afectada' }
    });

    this.view = new View({
      center: fromLonLat(municipio.geoPosicion.coordinates),
      zoom: 12,
      projection: 'EPSG:3857',
    })

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

    const layerMunicipio = new VectorLayer({
      source: new VectorSource({
        features: [pointFeature],
      }),
      properties: { 'title': 'Municipio' }
    });

    layerMunicipio.setStyle(
      new Style({
        image: new Icon({
          anchor: [1, 1],
          src: 'https://cdn-icons-png.flaticon.com/512/684/684908.png',
          scale: 0.05,
        }),
      })
    );

    const layersGroupIncendio = new LayerGroup({
      properties: { 'title': 'Incendios', 'openInLayerSwitcher': true },
      layers: [ layerMunicipio, this.layerAreasAfectadas ]
    });    

    // Crear el popup
    const container = document.getElementById('popup');
    this.popup = new Overlay({
      element: container!,
      autoPan: true,
      positioning: 'bottom-center',
    });

    // Crear el mapa
    this.map = new Map({
      controls: defaultControls({
        zoom: true,
        zoomOptions: {
          zoomInTipLabel: 'Acercar',
          zoomOutTipLabel: 'Alejar'
        }
      }).extend([
        new FullScreen(({ tipLabel: 'Pantalla completa' })),
      ]),
      target: 'map',
      layers: [baseLayers, wmsLayersGroup],
      view: this.view,
      overlays: [this.popup],
    });

    this.map.addControl(new LayerSwitcher({
      mouseover: true,
      show_progress: true,
    }));

    this.map.addControl(new ScaleLine());


    this.map.addLayer(layersGroupIncendio);

    if (!onlyView) {
      this.addInteractions();
    }

    this.map.on('pointermove', (event) => {
      const coordinate = event.coordinate;
      const [lon, lat] = toLonLat(coordinate);
      const [x, y] = proj4('EPSG:4326', utm30n, [lon, lat]);
      
      this.coordinates = `X: ${x.toFixed(2)}, Y: ${y.toFixed(2)}`;
    });

    // Añadir evento click
    this.map.on('singleclick', (evt) => {
      const coordinate = evt.coordinate;
      
      // Hacer la consulta WMS GetFeatureInfo
      const viewResolution = this.view.getResolution();
      const url = wmsNucleosPoblacion.getFeatureInfoUrl(
        coordinate,
        viewResolution!,
        'EPSG:3857',
        {'INFO_FORMAT': 'application/json'}
      );

      if (url) {
        fetch(url)
          .then(response => response.json())
          .then(data => {
            if (data.features.length > 0) {
              const content = document.getElementById('popup-content');
              content!.innerHTML = `
                <h4>Núcleo de población</h4>
                <p>Núcleo: ${data.features[0].properties.nombre}</p>
                <p>Población: ${data.features[0].properties.poblacion || 'N/A'}</p>
              `;
              this.popup.setPosition(coordinate);
            } else {
              this.popup.setPosition(undefined);
            }
          });
      }
    });
  }

  changeMunicipio(event: any) {
    //console.info('event', event);
  }

  addInteractions() {
    let mainBar = new Bar({});
    this.map.addControl(mainBar);

    let drawBar = new Bar({
      group: true,
      toggleOne: true
    });

    mainBar.addControl(drawBar);

    this.drawPoint = new Draw({
      type: 'Point',
      source: this.source,
    });

    let tgPoint = new Toggle({
      title: 'Dibujar punto',
      html: '<i class="fa-solid fa-location-dot"></i>',
      interaction: this.drawPoint
    });

    this.drawPoint.on('drawend', (e) => {
      console.log("draw", e.feature);

      let format = new WKT();
      let wkt = format.writeFeature(e.feature, {
        dataProjection: 'EPSG:4326',
        featureProjection: 'EPSG:3857'
      });
      console.log("json:", wkt);
    });

    //drawBar.addControl(tgPoint);

    this.drawPolygon = new Draw({
      type: 'Polygon',
      source: this.source,
    });

    let tgPolygon = new Toggle({
      title: 'Dibujar polígono',
      html: '<i class="fa-solid fa-draw-polygon"></i>',
      interaction: this.drawPolygon
    });

    // this.drawPolygon.on('drawstart', (drawEvent: DrawEvent) => {
    //   this.coords = null;
    //   const features = this.source.getFeatures();
    //   const last = features[features.length - 1];
    //   this.source.removeFeature(last);
    // });

    this.drawPolygon.on('drawend', (drawEvent: DrawEvent) => {
      const coords = [];

      for (let coord of drawEvent.target.sketchCoords_[0]) {
        coords.push(toLonLat(coord));
      }

      coords.push(coords[0]);

      this.coords = coords;
    });

    drawBar.addControl(tgPolygon);

    const tgSelect = new Toggle({
      // html: '<i class="fa fa-hand-pointer-o"></i>',
      html: '<i class="fa-solid fa-hand-pointer"></i>',
      title: "Seleccionar",
      interaction: new Select ({ hitTolerance: 2 }),
    });
    drawBar.addControl(tgSelect);    

    const tgDelete = new Toggle({
      html: '<i class="fa fa-trash"></i>',
      title: "Borrar",
      onToggle: () => {
        if (this.select) {
          const selectedFeatures = this.select.getFeatures();
          selectedFeatures.forEach(feature => {
            this.source.removeFeature(feature);
    });
          selectedFeatures.clear();
        }
    }});          
    drawBar.addControl (tgDelete);   

    this.select = new Select();
    this.map.addInteraction(this.select);    

    // this.map.addInteraction(this.drawPolygon);
    // this.snap = new Snap({ source: this.source });
    // this.map.addInteraction(this.snap);
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
