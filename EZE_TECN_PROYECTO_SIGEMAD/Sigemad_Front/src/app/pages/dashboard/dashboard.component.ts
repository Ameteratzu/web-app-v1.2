import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import Map from 'ol/Map';
import View from 'ol/View';
import { OSM, TileWMS, XYZ } from 'ol/source';
import { fromLonLat, toLonLat } from 'ol/proj';
import LayerGroup from 'ol/layer/Group';
import TileLayer from 'ol/layer/Tile';
import { defaults as defaultControls, FullScreen, ScaleLine,ZoomToExtent } from 'ol/control';
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

    const baseLayers = new LayerGroup({
      properties: { 'title': 'Capas base', openInLayerSwitcher: true },
      layers: [
        new TileLayer({
          source: new TileWMS({
            url: 'https://www.ign.es/wms-inspire/mapa-raster?',
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
          visible: true
        }),
        new TileLayer({
          source: new OSM(),
          properties: { 'title': 'OpenStreetMap', baseLayer: true },
          visible: false
        }),
      ]
    });

    const wmsIncenciosEvolutivo = new TileWMS({
      url: environment.urlGeoserver + 'wms?version=1.1.0',
      params: {
        'LAYERS': 'incendios_evolutivo',
        'TILED': true,
      },
      serverType: 'geoserver',
      transition: 0,
    });
    const layerIncendiosEvolutivo = new TileLayer({
      source: wmsIncenciosEvolutivo,
      properties: { 'title': 'Evolutivo' }
    });

    const wmsIncenciosInicial = new TileWMS({
      url: environment.urlGeoserver + 'wms?version=1.1.0',
      params: {
        'LAYERS': 'incendios_inicial',
        'TILED': true,
      },
      serverType: 'geoserver',
      transition: 0,
    });
    const layerIncendiosInicial = new TileLayer({
      source: wmsIncenciosInicial,
      properties: { 'title': 'Inicial' }
    });    

    const wmsIncendiosCentroideMunicipios = new TileWMS({
      url: environment.urlGeoserver + 'wms?version=1.1.0',
      params: {
        'LAYERS': 'incendios_centroide_municipio',
        'TILED': true,
      },
      serverType: 'geoserver',
      transition: 0,
    });
    const layerIncenciosCentroideMunicipio = new TileLayer({
      source: wmsIncendiosCentroideMunicipios,
      properties: { 'title': 'Municipios' }
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

    const wmsLayersGroupIncendios = new LayerGroup({
      properties: { 'title': 'Incendios', 'openInLayerSwitcher': true },
      //layers: [layerIncendiosEvolutivo, layerIncendiosInicial, layerIncenciosCentroideMunicipio, layerIncenciosLimitesMunicipio]
      layers: [layerIncendiosEvolutivo, layerIncendiosInicial, layerIncenciosCentroideMunicipio]
    });

    this.view = new View({
      center: [-400000, 4900000],
      zoom: 6,
      extent: [-4500000, 3000000,  2500000, 6500000]
    })

    this.map = new Map({
      controls: defaultControls({ 
        zoom: true, zoomOptions: { 
          zoomInTipLabel: 'Acercar', 
          zoomOutTipLabel: 'Alejar' } 
      }).extend([
        new FullScreen({tipLabel: 'Pantalla completa'}),
      ]),
      target: 'map',
      layers: [baseLayers, wmsLayersGroupIncendios],
      view: this.view,
    });

    this.map.addControl(new LayerSwitcher({
      mouseover: true,
      show_progress: true,
    }));

    this.map.addControl(new ScaleLine());

    this.map.addControl(new ZoomToExtent({
      extent: [-2032613, 3138198, -1449857, 3445169],
      tipLabel: 'Islas Canarias',
    }));

    this.map.on('pointermove', (event) => {
      const coordinate = event.coordinate;
      const [lon, lat] = toLonLat(coordinate);
      const [x, y] = proj4('EPSG:4326', utm30n, [lon, lat]);
      const cursorCoordinatesElement = document.getElementById('cursor-coordinates');
      if (cursorCoordinatesElement) {
        cursorCoordinatesElement.innerText = `X: ${x.toFixed(2)}, Y: ${y.toFixed(2)}`;
      }
    });

    // Añadir el manejador de eventos click
    this.map.on('click', async (evt) => {
      const coordinate = evt.coordinate;
      const pixel = this.map.getEventPixel(evt.originalEvent);

      // Obtener información de las capas WMS
      const viewResolution = this.view.getResolution();
      const projection = this.view.getProjection();

      // Cerrar popup existente
      this.closePopup();

      // Consultar cada capa WMS
      const layers = [
        //{ source: wmsIncenciosEvolutivo, type: 'Incendio Evolutivo' },
        //{ source: wmsIncenciosInicial, type: 'Incendio Inicial' },
        { source: wmsIncendiosCentroideMunicipios, type: 'Incendio' }
      ];

      for (const layer of layers) {
        if (!viewResolution) continue;

        const url = layer.source.getFeatureInfoUrl(
          coordinate,
          viewResolution,
          projection,
          {'INFO_FORMAT': 'application/json', 'FEATURE_COUNT': 1}
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

  // Añadir método para cerrar el popup
  closePopup(): void {
    this.selectedFeature = null;
  }
}
