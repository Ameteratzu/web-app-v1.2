import { Component, EventEmitter, inject, Input, Output, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { environment } from '../../../environments/environment';

import Feature from 'ol/Feature';
import Point from 'ol/geom/Point';
import { Draw, Snap, Select } from 'ol/interaction';
import { DrawEvent } from 'ol/interaction/Draw';
import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import Map from 'ol/Map';
import { OSM, Vector as VectorSource, WMTS, XYZ } from 'ol/source';
import View from 'ol/View';
import LayerGroup from 'ol/layer/Group';
import { defaults as defaultControls, FullScreen, ScaleLine } from 'ol/control';
import LayerSwitcher from 'ol-ext/control/LayerSwitcher';
import TileWMS from 'ol/source/TileWMS';
import Icon from 'ol/style/Icon';
import Style from 'ol/style/Style';
import { Geometry, Polygon } from 'ol/geom';
import WKT from 'ol/format/WKT';
import WMTSTileGrid from 'ol/tilegrid/WMTS';
import { get as getProjection } from 'ol/proj';
import { getTopLeft } from 'ol/extent';
import Bar from 'ol-ext/control/Bar';
import Toggle from 'ol-ext/control/Toggle';
import Overlay from 'ol/Overlay';

import proj4 from 'proj4';
import { fromLonLat, toLonLat } from 'ol/proj';

import { MunicipalityService } from '../../services/municipality.service';
import { Municipality } from '../../types/municipality.type';

import 'ol/ol.css';
import 'ol-ext/dist/ol-ext.css';

import { DragDropModule } from '@angular/cdk/drag-drop';

// Define the projection for UTM zone 30N (EPSG:25830)
const utm30n = '+proj=utm +zone=30 +ellps=GRS80 +units=m +no_defs';

@Component({
  selector: 'app-map-create',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, FlexLayoutModule, DragDropModule],
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
      properties: { title: 'Capas base', openInLayerSwitcher: true },
      layers: [
        new TileLayer({
          source: new WMTS({
            url: 'https://www.ign.es/wmts/ign-base?',
            layer: 'IGNBaseTodo',
            matrixSet: 'GoogleMapsCompatible',
            format: 'image/png',
            style: 'default',
            tileGrid: new WMTSTileGrid({
              origin: getTopLeft(getProjection('EPSG:3857')?.getExtent() || [0, 0, 0, 0]),
              resolutions: [
                156543.03392804097, 78271.51696402048, 39135.75848201024, 19567.87924100512, 9783.93962050256, 4891.96981025128, 2445.98490512564,
                1222.99245256282, 611.49622628141, 305.748113140705, 152.8740565703525, 76.43702828517625, 38.21851414258813, 19.109257071294063,
                9.554628535647032, 4.777314267823516, 2.388657133911758, 1.194328566955879, 0.5971642834779395, 0.29858214173896974,
                0.14929107086948487,
              ],
              matrixIds: ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20'],
            }),
            attributions: '© Instituto Geográfico Nacional de España (IGN)',
          }),
          properties: { title: 'IGN callejero', baseLayer: true },
          visible: true,
        }),
        new TileLayer({
          source: new TileWMS({
            url: 'https://www.ign.es/wms-inspire/pnoa-ma?',
            params: { LAYERS: 'OI.OrthoimageCoverage', FORMAT: 'image/jpeg' },
            attributions: '© PNOA - IGN España',
          }),
          properties: { title: 'IGN satélite', baseLayer: true },
          visible: false,
        }),
        new TileLayer({
          source: new XYZ({
            url: 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}',
            attributions: 'Tiles © <a href="https://www.esri.com/">Esri</a> - Source: Esri, Maxar, Earthstar Geographics',
          }),
          properties: { title: 'Satélite', baseLayer: true },
          visible: false,
        }),
        new TileLayer({
          source: new OSM(),
          properties: { title: 'OpenStreetMap', baseLayer: true },
          visible: false,
        }),
      ],
    });
    // Evitar que se oculten todas las capas base
    function preventGroupLayerToggle(event: any) {
      if (event.target === baseLayers) {
        baseLayers.setVisible(true);
      }
    }
    baseLayers.on('change:visible', preventGroupLayerToggle);

    // capas wms administrativas
    const wmsNucleosPoblacion = new TileWMS({
      url: environment.urlGeoserver + 'wms?version=1.1.0',
      params: {
        LAYERS: 'nucleos_poblacion',
        TILED: true,
      },
      serverType: 'geoserver',
      transition: 0,
    });
    const wmsLayersGroup = new LayerGroup({
      properties: { title: 'Límites administrativos', openInLayerSwitcher: true },

      layers: [
        new TileLayer({
          source: wmsNucleosPoblacion,
          properties: { title: 'Núcleos de población' },
        }),
        new TileLayer({
          source: new TileWMS({
            url: environment.urlGeoserver + 'wms?version=1.1.0',
            params: {
              LAYERS: 'municipio_limites',
              TILED: true,
            },
            serverType: 'geoserver',
            transition: 0,
          }),
          properties: { title: 'Límites municipio' },
        }),
      ],
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
        'stroke-color': 'rgb(255, 128, 0)',
        'stroke-width': 5,
      },
      properties: { title: 'Área afectada' },
    });

    this.view = new View({
      center: fromLonLat(municipio.geoPosicion.coordinates),
      zoom: 12,
      projection: 'EPSG:3857',
    });

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
      properties: { title: 'Municipio' },
    });

    layerMunicipio.setStyle(
      new Style({
        image: new Icon({
          anchor: [1, 1],
          src: '/assets/img/centroide.png',
          scale: 0.07,
        }),
      })
    );

    const layersGroupIncendio = new LayerGroup({
      properties: { title: 'Incendios', openInLayerSwitcher: true },
      layers: [layerMunicipio, this.layerAreasAfectadas],
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
          zoomOutTipLabel: 'Alejar',
        },
      }).extend([new FullScreen({ tipLabel: 'Pantalla completa' })]),
      target: 'map',
      layers: [baseLayers, wmsLayersGroup],
      view: this.view,
      overlays: [this.popup],
    });

    this.map.addControl(
      new LayerSwitcher({
        mouseover: true,
        show_progress: true,
      })
    );

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
      const url = wmsNucleosPoblacion.getFeatureInfoUrl(coordinate, viewResolution!, 'EPSG:3857', { INFO_FORMAT: 'application/json' });

      if (url) {
        fetch(url)
          .then((response) => response.json())
          .then((data) => {
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
      toggleOne: true,
    });

    mainBar.addControl(drawBar);

    this.drawPoint = new Draw({
      type: 'Point',
      source: this.source,
    });

    let tgPoint = new Toggle({
      title: 'Dibujar punto',
      html: '<img src="/assets/img/location-dot-solid.svg" alt="Toggle Icon" style="width: 24px; height: 24px;">',
      interaction: this.drawPoint,
    });

    this.drawPoint.on('drawend', (e) => {
      console.log('draw', e.feature);

      let format = new WKT();
      let wkt = format.writeFeature(e.feature, {
        dataProjection: 'EPSG:4326',
        featureProjection: 'EPSG:3857',
      });
      console.log('json:', wkt);
    });

    //drawBar.addControl(tgPoint);

    this.drawPolygon = new Draw({
      type: 'Polygon',
      source: this.source,
    });

    let tgPolygon = new Toggle({
      title: 'Dibujar polígono',
      html: '<img src="/assets/img/draw-polygon.svg" alt="Toggle Icon" style="width: 24px; height: 24px;">',
      interaction: this.drawPolygon,
    });

    this.drawPolygon.on('drawstart', (drawEvent: DrawEvent) => {
      this.coords = null;
      const features = this.source.getFeatures();
      const last = features[features.length - 1];
      this.source.removeFeature(last);
    });

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
      html: '<img src="/assets/img/hand-pointer.svg" alt="Toggle Icon" style="width: 24px; height: 24px;">',
      title: 'Seleccionar',
      interaction: new Select({ hitTolerance: 2 }),
    });
    drawBar.addControl(tgSelect);

    const tgDelete = new Toggle({
      html: '<img src="/assets/img/trash.svg" alt="Toggle Icon" style="width: 24px; height: 24px;">',
      title: 'Borrar',
      onToggle: () => {
        if (this.select) {
          const selectedFeatures = this.select.getFeatures();
          selectedFeatures.forEach((feature) => {
            this.source.removeFeature(feature);
          });
          selectedFeatures.clear();
        }
      },
    });
    drawBar.addControl(tgDelete);

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
