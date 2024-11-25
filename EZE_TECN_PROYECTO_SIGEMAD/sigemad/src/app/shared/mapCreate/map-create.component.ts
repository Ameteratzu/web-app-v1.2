import {
  Component,
  EventEmitter,
  inject,
  Input,
  Output,
  signal,
} from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';

import Feature from 'ol/Feature';
import Map from 'ol/Map';
import View from 'ol/View';
import Point from 'ol/geom/Point';
import { Draw, Snap } from 'ol/interaction';
import { DrawEvent } from 'ol/interaction/Draw';
import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import { OSM, Vector as VectorSource } from 'ol/source';

import { MunicipalityService } from '../../services/municipality.service';

import { CommonModule } from '@angular/common';
import { Geometry } from 'ol/geom';
import { fromLonLat } from 'ol/proj';
import { Municipality } from '../../types/municipality.type';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  NativeDateAdapter,
} from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-map-create',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
  ],
  templateUrl: './map-create.component.html',
  styleUrl: './map-create.component.css',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MAT_DATE_FORMATS },
  ],
})
export class MapCreateComponent {
  @Input() municipio: any;
  @Input() listaMunicipios: any;
  @Output() save = new EventEmitter<Feature<Geometry>[]>();

  public source: VectorSource | undefined;
  public map: Map | undefined;
  public draw: Draw | undefined;
  public snap: Snap | undefined;
  public vector: VectorLayer | undefined;

  public coords: any;
  
  public data = inject(MAT_DIALOG_DATA);
  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);

  public municipalityService = inject(MunicipalityService);

  public municipalities = signal<Municipality[]>([]);

  public municipioSelected = signal(this.data?.municipio || {});

  public section: string = '';

  async ngOnInit() {

    const { municipio, listaMunicipios } = this.data;
    console.info('+++', municipio, listaMunicipios);

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
          source: new OSM(),
        }),
        this.vector,
      ],
      target: 'map',
      view: new View({
        center: fromLonLat([
          municipio.geoPosicion.coordinates[0],
          municipio.geoPosicion.coordinates[1],
        ]),
        //center: fromLonLat([-2.9704191830794, 43.0277066101594]),
        zoom: 12,
        //projection: 'EPSG:4326',
      }),
    });

    const point = new Point([
      municipio.geoPosicion.coordinates[0],
      municipio.geoPosicion.coordinates[1],
    ]);

    const feature = new Feature({
      geometry: point,
    });

    const vectorLayer = new VectorLayer({
      source: new VectorSource({
        features: [feature],
      }),
    });

    this.map.addLayer(vectorLayer);

    this.addInteractions();
  }

  changeMunicipio(event: any) {
    console.info('event', event);
  }

  addInteractions() {
    this.draw = new Draw({
      source: this.source,
      type: 'Polygon',
    });

    this.draw.on('drawstart', (drawEvent: DrawEvent) => {
      this.coords = null;
      const features = this.source?.getFeatures();
      const last = features?.[features.length - 1];
      if (last) {
        this.source?.removeFeature(last);
      }
    });

    this.draw.on('drawend', (drawEvent: DrawEvent) => {
      const coords = [];

      for (let coord of drawEvent.target.sketchCoords_[0]) {
        coords.push(coord);
      }

      coords.push(coords[0]);

      this.coords = JSON.stringify(coords);
    });

    this.map?.addInteraction(this.draw);
    this.snap = new Snap({ source: this.source });
    this.map?.addInteraction(this.snap);
  }

  savePolygon() {
    if (this.coords) {
      this.save.emit(this.coords);
      this.closeModal();
    }
  }

  closeModal() {
    this.matDialogRef.close();
  }
}
