import { Component, EventEmitter, inject, Input, Output, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import Feature from 'ol/Feature';
import Point from 'ol/geom/Point';
import { Draw, Snap } from 'ol/interaction';
import { DrawEvent } from 'ol/interaction/Draw';
import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import Map from 'ol/Map';
import { OSM, Vector as VectorSource } from 'ol/source';
import View from 'ol/View';

import { MunicipalityService } from '../../services/municipality.service';

import { CommonModule } from '@angular/common';
import { Geometry, Polygon } from 'ol/geom';
import { fromLonLat, toLonLat } from 'ol/proj';
import { Municipality } from '../../types/municipality.type';

import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';

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

  async ngOnInit() {
    const { municipio, listaMunicipios, defaultPolygon, onlyView } = this.data;

    let defaultPolygonMercator;

    if (defaultPolygon) {
      defaultPolygonMercator = defaultPolygon.map((coord: any) => fromLonLat(coord));
    }

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
      controls: [],
      view: new View({
        center: fromLonLat(municipio.geoPosicion.coordinates),
        zoom: 12,
        projection: 'EPSG:3857',
      }),
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

    const pointLayer = new VectorLayer({
      source: new VectorSource({
        features: [pointFeature],
      }),
    });

    this.map.addLayer(pointLayer);
    if (!onlyView) {
      this.addInteractions();
    }
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
}
