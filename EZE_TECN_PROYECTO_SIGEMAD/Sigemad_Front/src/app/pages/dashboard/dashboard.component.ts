import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import Map from 'ol/Map';
import View from 'ol/View';
import { ImageStatic, OSM, TileWMS, WMTS, XYZ } from 'ol/source';
import ImageLayer from 'ol/layer/Image';
import { fromLonLat, get, toLonLat, transformExtent } from 'ol/proj';
import LayerGroup from 'ol/layer/Group';
import TileLayer from 'ol/layer/Tile';
import WMTSTileGrid from 'ol/tilegrid/WMTS';
import { get as getProjection } from 'ol/proj';
import { getTopLeft } from 'ol/extent';
import { Control, defaults as defaultControls, FullScreen, ScaleLine, ZoomToExtent } from 'ol/control';
import LayerSwitcher from 'ol-ext/control/LayerSwitcher';
import proj4 from 'proj4';

import { MenuItemActiveService } from '../../services/menu-item-active.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { environment } from '../../../environments/environment';

// Define the projection for UTM zone 30N (EPSG:25830)
const utm30n = "+proj=utm +zone=30 +ellps=GRS80 +units=m +no_defs";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatFormFieldModule, MatGridListModule, MatCardModule, MatDividerModule, MatIconModule, MatButtonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  private map!: Map;
  private view!: View;

  public menuItemActiveService = inject(MenuItemActiveService);

  public events = [
    { date: '13/06/2024 05:50', type: 'Terremoto', description: 'ALBORÁN SUR. Magnitud: 3mblg' },
    { date: '12/06/2024 10:25', type: 'Incendio forestal', description: 'Vilanova (Orense). Estado: Activo' },
    { date: '12/06/2024 09:15', type: 'Incendio forestal', description: 'Estado: Extinguido' },
    { date: '10/06/2024 15:45', type: 'Terremoto', description: 'S TETUAN.MAC. Magnitud: 3.6 mblg' },
  ];

  // Añadir nuevas propiedades para el popup
  public selectedFeature: any = null;
  public popupPosition = { x: 0, y: 0 };

  private isShowingPeninsula = true;  // Para cambiar zoom a Canarias o Península

  ngOnInit() {
    this.menuItemActiveService.set.emit('/dashboard');

    this.configuremap();

    // Graph
    const data = {
      labels: ['Día 6', 'Día 5', 'Día 4', 'Día 3', 'Día 2', 'Día 1', 'Hoy'],
      datasets: [
        {
          label: 'Periodo anterior',
          data: [1, 0, 2, 4, 1, 0, 2],
          backgroundColor: '#E66E2A',
          fill: '#E66E2A',
        },
        {
          label: 'Periodo actual',
          data: [3, 2, 5, 4, 3, 6, 3],
          backgroundColor: '#10A0E0',
          fill: '#10A0E0',
        },
      ],
    };

  }

  configuremap() {

    const baseLayers = this.getBaseLayers();

    const layersGroupIncendios = this.getIncendiosLayers();

    this.view = new View({
      center: [-319201, 4834489],
      zoom: 6.5,
      extent: [-4500000, 3000000, 2500000, 6500000]
    });

    this.map = new Map({
      controls: defaultControls({
        zoom: true, zoomOptions: {
          zoomInTipLabel: 'Acercar',
          zoomOutTipLabel: 'Alejar'
        }
      }).extend([
        new FullScreen({ tipLabel: 'Pantalla completa' }),
      ]),
      target: 'map',
      layers: [baseLayers, layersGroupIncendios],
      view: this.view,
    });

    this.map.addControl(new LayerSwitcher({
      mouseover: true,
      show_progress: true,
    }));

    this.map.addControl(new ScaleLine());

    this.addZoomCanariasPeninsula();

    this.map.on('pointermove', (event) => {
      const coordinate = event.coordinate;
      const [lon, lat] = toLonLat(coordinate);
      const [x, y] = proj4('EPSG:4326', utm30n, [lon, lat]);
      const cursorCoordinatesElement = document.getElementById('cursor-coordinates');
      if (cursorCoordinatesElement) {
        cursorCoordinatesElement.innerText = `X: ${x.toFixed(2)}, Y: ${y.toFixed(2)}`;
      }
    });

    this.addPopup(layersGroupIncendios);
  }

  getBaseLayers() {
    const baseLayers = new LayerGroup({
      properties: { 'title': 'Capas base', openInLayerSwitcher: true },
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
                156543.03392804097, 78271.51696402048, 39135.75848201024,
                19567.87924100512, 9783.93962050256, 4891.96981025128,
                2445.98490512564, 1222.99245256282, 611.49622628141,
                305.748113140705, 152.8740565703525, 76.43702828517625,
                38.21851414258813, 19.109257071294063, 9.554628535647032,
                4.777314267823516, 2.388657133911758, 1.194328566955879,
                0.5971642834779395, 0.29858214173896974, 0.14929107086948487
              ],
              matrixIds: ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20']
            }),
            attributions: '© Instituto Geográfico Nacional de España (IGN)'
          }),
          properties: { 'title': 'IGN base', baseLayer: true, },
          visible: true
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
          visible: false
        }),
      ]
    });
    function preventGroupLayerToggle(event: any) {
      // Evitar que se oculten todas las capas base
      if (event.target === baseLayers) {
        baseLayers.setVisible(true);
      }
    }
    baseLayers.on('change:visible', preventGroupLayerToggle);
    return baseLayers;
  }

  getIncendiosLayers() {
    const wmsIncendiosActivos = new TileWMS({
      url: environment.urlGeoserver + 'wms?version=1.1.0',
      params: {
        'LAYERS': 'incendios_activos',
        'TILED': true,
      },
      serverType: 'geoserver',
      transition: 0,
    });
    const layerIncenciosActivos = new TileLayer({
      source: wmsIncendiosActivos,
      properties: { 'title': 'Activos' }
    });

    const wmsIncendiosControlados = new TileWMS({
      url: environment.urlGeoserver + 'wms?version=1.1.0',
      params: {
        'LAYERS': 'incendios_controlados',
        'TILED': true,
      },
      serverType: 'geoserver',
      transition: 0,
    });
    const layerIncenciosControlados = new TileLayer({
      source: wmsIncendiosControlados,
      properties: { 'title': 'Controlados' }
    });

    const wmsIncendiosEstabilizados = new TileWMS({
      url: environment.urlGeoserver + 'wms?version=1.1.0',
      params: {
        'LAYERS': 'incendios_estabilizados',
        'TILED': true,
      },
      serverType: 'geoserver',
      transition: 0,
    });
    const layerIncenciosEstabilizados = new TileLayer({
      source: wmsIncendiosEstabilizados,
      properties: { 'title': 'Estabilizados' },
    });

    const wmsIncendiosExtinguidos = new TileWMS({
      url: environment.urlGeoserver + 'wms?version=1.1.0',
      params: {
        'LAYERS': 'incendios_extinguidos',
        'TILED': true,
      },
      serverType: 'geoserver',
      transition: 0,
    });
    const layerIncenciosExtinguidos = new TileLayer({
      source: wmsIncendiosExtinguidos,
      properties: { 'title': 'Extinguidos' },
      visible: false
    });

    // capas de riesgo de incendios
    const lastDate = this.getFormattedLastDate();
    const peninsulaExtent = transformExtent(
      [-9.500000000000007, 35.04941937438704, 4.347283247337459, 44.050000000000026], // Ajustamos las coordenadas para mejor cobertura
      'EPSG:4326',
      'EPSG:3857'
    );
    const layerRiesgoPeninsula = new ImageLayer({
      source: new ImageStatic({
        url: `https://www.aemet.es/es/api-eltiempo/incendios/imagen/riesgo/p_fc024_RIESGO_${lastDate}_1.png`,
        imageExtent: peninsulaExtent,
        projection: 'EPSG:3857'
      }),
      properties: { 'title': 'AEMET riesgo península' },
      opacity: 0.25
    });

    const canariasExtent = transformExtent(
      [-18.500000000000018, 27.498279015149162, -12.949039024665785, 29.549999999999997],
      'EPSG:4326',
      'EPSG:3857'
    );
    const layerRiesgoCanarias = new ImageLayer({
      source: new ImageStatic({
        url: `https://www.aemet.es/es/api-eltiempo/incendios/imagen/riesgo/c_fc024_RIESGO_${lastDate}_1.png`,
        imageExtent: canariasExtent,
        projection: 'EPSG:3857'
      }),
      properties: { 'title': 'AEMET riesgo Canarias' },
      opacity: 0.25
    });

    // const wmsIncendiosLimitesMunicipios = new TileWMS({
    //   url: environment.urlGeoserver + 'wms?version=1.1.0',
    //   params: {
    //     'LAYERS': 'incendios_limites_municipio',
    //     'TILED': true,
    //   },
    //   serverType: 'geoserver',
    //   transition: 0,
    // });
    // const layerIncenciosLimitesMunicipio = new TileLayer({
    //   source: wmsIncendiosLimitesMunicipios,
    //   properties: { 'title': 'Limites Municipios' }
    // });    

    return new LayerGroup({
      properties: { 'title': 'Incendios', 'openInLayerSwitcher': true },
      //layers: [layerIncendiosEvolutivo, layerIncendiosInicial, layerIncenciosCentroideMunicipio, layerIncenciosLimitesMunicipio]
      layers: [layerRiesgoCanarias, layerRiesgoPeninsula, layerIncenciosExtinguidos, layerIncenciosEstabilizados, layerIncenciosControlados, layerIncenciosActivos]
    });
  }

  getFormattedLastDate(): string {
    const date = new Date();
    date.setDate(date.getDate() - 1);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    return `${day}${month}${year}`;
  }

  addPopup(layersGroupIncendios: LayerGroup) {
    this.map.on('click', async (evt) => {
      const coordinate = evt.coordinate;
      const pixel = this.map.getEventPixel(evt.originalEvent);

      // Obtener información de las capas WMS
      const viewResolution = this.view.getResolution();
      const projection = this.view.getProjection();

      // Cerrar popup existente
      this.closePopup();

      // Obtener las capas del grupo de incendios
      const layers = layersGroupIncendios.getLayers().getArray().map(layer => ({
        source: (layer as TileLayer).getSource(),
        type: 'Incendio ' + layer.get('title').slice(0, -1)
      }));

      for (const layer of layers) {
        if (!viewResolution) continue;
        if (!(layer.source instanceof TileWMS)) continue;

        const url = layer.source.getFeatureInfoUrl(
          coordinate,
          viewResolution,
          projection,
          { 'INFO_FORMAT': 'application/json', 'FEATURE_COUNT': 1 }
        );

        if (url) {
          try {
            const response = await fetch(url);
            const data = await response.json();

            if (data.features && data.features.length > 0) {
              const feature = data.features[0];
              this.selectedFeature = {
                type: layer.type,
                date: feature.properties.fechaInicio,
                location: feature.properties.municipio,
                status: feature.properties.estado,
              };

              // Posicionar el popup
              this.popupPosition = {
                x: pixel[0],
                y: pixel[1] + 10 // Pequeño offset vertical
              };

              break; // Mostrar solo el primer feature encontrado
            }
          } catch (error) {
            console.error('Error al obtener información del feature:', error);
          }
        }
      }
    });
  }

  addZoomCanariasPeninsula() {
    const toggleViewControl = this.createCustomControl('Alternar vista', () => {
      if (this.isShowingPeninsula) {
        // Zoom a Canarias
        this.map.getView().animate({
          center: [-1741235, 3291683.5],
          zoom: 8,
          duration: 1000
        });
      } else {
        // Zoom a Península
        this.map.getView().animate({
          center: [-319201, 4834489],
          zoom: 6.5,
          duration: 1000
        });
      }
      this.isShowingPeninsula = !this.isShowingPeninsula;

      // Actualizar el título del botón
      const button = document.querySelector('.ol-custom-control') as HTMLButtonElement;
      if (button) {
        button.title = this.isShowingPeninsula ? 'Ver Canarias' : 'Ver Península y Baleares';
      }
    }, 'sync', 2);

    this.map.addControl(toggleViewControl);
  }

  private createCustomControl(label: string, callback: () => void, icon: string, index: number): Control {
    const button = document.createElement('button');
    button.innerHTML = `<span class="material-icons">${icon}</span>`;
    button.title = 'Ver Canarias';
    button.className = 'ol-custom-control';

    const element = document.createElement('div');
    element.className = 'ol-unselectable ol-control custom-controls';
    element.style.top = `${index * 40}px`;
    element.style.left = '9px';
    element.appendChild(button);

    button.addEventListener('click', callback);

    return new Control({ element });
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

  closePopup(): void {
    this.selectedFeature = null;
  }
}
