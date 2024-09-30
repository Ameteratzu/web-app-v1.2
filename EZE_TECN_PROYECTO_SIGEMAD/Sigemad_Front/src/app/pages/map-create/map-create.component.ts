import { Component, inject } from '@angular/core';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';

import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import { XYZ, OSM, Vector as VectorSource } from 'ol/source';
import { get } from 'ol/proj';
import Map from 'ol/Map';
import View from 'ol/View';
import { Draw, Modify, Snap } from 'ol/interaction';
import { DrawEvent }  from 'ol/interaction/Draw';

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

  ngOnInit() {
    // Map
    const raster = new TileLayer({
      source: new XYZ({
        url: 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}',
        maxZoom: 19
      })
    });

    this.source = new VectorSource();
    this.vector = new VectorLayer({
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
      layers: [raster, this.vector],
      target: 'map',
      view: new View({
        center: [-400000, 4900000],
        zoom: 6.2,
        extent,
      }),
    });

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
        coords.push(this.coord3857To4326(coord));
      }

      this.coords = JSON.stringify(coords);
    });

    this.map.addInteraction(this.draw);
    this.snap = new Snap({ source: this.source });
    this.map.addInteraction(this.snap);
  }

  savePolygon() {
    if (this.coords) {
      localStorage.setItem('coordinates', this.coords);
      this.closeModal();
    }
  }

  closeModal() {
    this.matDialogRef.close();
  }

  coord3857To4326(coord:any) {    
    const e_value = 2.7182818284;
    const X = 20037508.34;
    
    const lat3857 = coord[0];
    const long3857 = coord[1];
    
    const long4326 = (long3857 * 180) / X;
    
    let lat4326 = lat3857/(X / 180);
    const exponent = (Math.PI / 180) * lat4326;
    
    lat4326 = Math.atan(Math.pow(e_value, exponent));
    lat4326 = lat4326 / (Math.PI / 360);
    lat4326 = lat4326 - 90;

    return [lat4326, long4326];  
  }

}
