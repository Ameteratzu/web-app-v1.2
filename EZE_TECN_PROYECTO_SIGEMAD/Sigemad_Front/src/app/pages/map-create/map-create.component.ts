import { Component, inject, signal } from '@angular/core';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';

import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import { XYZ, OSM, Vector as VectorSource } from 'ol/source';
import { get, transform } from 'ol/proj';
import Map from 'ol/Map';
import View from 'ol/View';
import { Draw, Modify, Snap } from 'ol/interaction';
import { DrawEvent }  from 'ol/interaction/Draw';
import Point from 'ol/geom/Point';
import Feature from 'ol/Feature';

import { MunicipalityService } from '../../services/municipality.service';

import { Municipality } from '../../types/municipality.type';

@Component({
  selector: 'app-map-create',
  standalone: true,
  imports: [],
  templateUrl: './map-create.component.html',
  styleUrl: './map-create.component.css'
})

export class MapCreateComponent {

  public source: VectorSource;
  public map: Map;
  public draw: Draw;
  public snap: Snap;
  public vector: VectorLayer;
  public coords:any;

  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);

  public municipalityService = inject(MunicipalityService);

  public municipalities = signal<Municipality[]>([]);

  public length:number;
  public latitude:number;

  async ngOnInit() {
    // Map
    this.length = Number(localStorage.getItem('length'));
    this.latitude = Number(localStorage.getItem('latitude'));

    this.source = new VectorSource();
    this.vector = new VectorLayer({
      source: this.source,
      style: {
        'fill-color': 'rgba(255, 255, 255, 0.2)',
        'stroke-color': '#ffcc33',
        'stroke-width': 2,
      },
    });

    this.map = new Map({
      layers: [
        new TileLayer({
          source: new OSM()
        }),
        this.vector
      ],
      target: 'map',
      view: new View({
        center: transform([-3, 40], 'EPSG:4326', 'EPSG:4326'),
        zoom: 6.4,
        projection: 'EPSG:4326'
      }),
    });

    const point = new Point([this.length, this.latitude]);

    const feature = new Feature({
      geometry: point
    });

    const vectorLayer = new VectorLayer({
      source: new VectorSource({
        features: [feature]
      })
    });

    this.map.addLayer(vectorLayer);

    this.addInteractions();
  }

  addInteractions() {
    this.draw = new Draw({
      source: this.source,
      type: 'Polygon',
    });

    this.draw.on('drawstart', (drawEvent:DrawEvent) => {
      this.coords = null;
      const features = this.source.getFeatures();
      const last = features[features.length - 1];
      this.source.removeFeature(last);
    });

    this.draw.on('drawend', (drawEvent:DrawEvent) => {
      const coords = [];

      for (let coord of drawEvent.target.sketchCoords_[0]) {
        coords.push(coord);
      }

      coords.push(coords[0]);

      this.coords = JSON.stringify(coords);
    });

    this.map.addInteraction(this.draw);
    this.snap = new Snap({ source: this.source });
    this.map.addInteraction(this.snap);
  }

  savePolygon() {
    if (this.coords) {
      localStorage.setItem('coordinates', this.coords);
      localStorage.setItem('polygon', '1');
      this.closeModal();
    }
  }

  closeModal() {
    this.matDialogRef.close();
  }
}
