import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart } from 'chart.js/auto';
import Map from 'ol/Map';
import View from 'ol/View';
import { Draw, Modify, Snap } from 'ol/interaction';
import { DrawEvent }  from 'ol/interaction/Draw';
import { XYZ, OSM, Vector as VectorSource } from 'ol/source';
import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import { get } from 'ol/proj';
import { MenuItemActiveService } from '../../services/menu-item-active.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

  public chart: Chart;

  public draw: Draw;
  public source: VectorSource;
  public map: Map;
  public snap: Snap;

  public menuItemActiveService = inject(MenuItemActiveService);

  public events = [
    { date: '13/06/2024 05:50', type: 'Terremoto', description: 'ALBORÁN SUR. Magnitud: 3mblg' },
    { date: '12/06/2024 10:25', type: 'Incendio forestal', description: 'Vilanova (Orense). Estado: Activo' },
    { date: '12/06/2024 09:15', type: 'Incendio forestal', description: 'Estado: Extinguido' },
    { date: '10/06/2024 15:45', type: 'Terremoto', description: 'S TETUAN.MAC. Magnitud: 3.6 mblg' },
  ];

  ngOnInit() {
    this.menuItemActiveService.set.emit('/dashboard');

    // Map
    const raster = new TileLayer({
      source: new XYZ({
        url: 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}',
        maxZoom: 19
      })
    });

    this.source = new VectorSource();
    const vector = new VectorLayer({
      source: this.source,
      style: {
        'fill-color': 'rgba(255, 255, 255, 0.2)',
        'stroke-color': '#ffcc33',
        'stroke-width': 2,
      },
    });

    const extent = get('EPSG:3857')!.getExtent().slice();

    extent[0] += extent[0];
    extent[2] += extent[2];

    this.map = new Map({
      layers: [raster, vector],
      target: 'map',
      view: new View({
        center: [-400000, 4900000],
        zoom: 6.2,
        extent,
      }),
    });

    this.addInteractions();

    // Graph
    const data = {
      labels: [
        'Día 6',
        'Día 5',
        'Día 4',
        'Día 3',
        'Día 2',
        'Día 1',
        'Hoy'
      ],
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

    this.chart = new Chart('chart', {
      type: 'bar',
      data,
      options: {
        plugins: {
          legend: {
            display: true,
            align: 'center',
            position: 'bottom',
          },
        },
        scales: {
          y: {
            suggestedMin: 0,
            suggestedMax: 7,
          },
        },
      },
    });
  }

  addInteractions() {
    this.draw = new Draw({
      source: this.source,
      type: 'Polygon',
    });

    this.draw.on('drawstart', (drawEvent:DrawEvent) => {
      const features = this.source.getFeatures();
      const last = features[features.length - 1];
      this.source.removeFeature(last);
    });

    this.draw.on('drawend', (drawEvent:DrawEvent) => {
      console.log(this.map, this.draw);
    });

    this.map.addInteraction(this.draw);
    this.snap = new Snap({ source: this.source });
    this.map.addInteraction(this.snap);
  }

}
