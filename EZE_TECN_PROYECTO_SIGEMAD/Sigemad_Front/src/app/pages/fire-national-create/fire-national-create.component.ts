import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';

import { FireForeignCreateComponent } from '../fire-foreign-create/fire-foreign-create.component';
import { MapCreateComponent } from '../map-create/map-create.component';

import { EventService } from '../../services/event.service';
import { FireService } from '../../services/fire.service';
import { MunicipalityService } from '../../services/municipality.service';
import { ProvinceService } from '../../services/province.service';
import { TerritoryService } from '../../services/territory.service';

import { Event } from '../../types/event.type';
import { Municipality } from '../../types/municipality.type';
import { Province } from '../../types/province.type';
import { Territory } from '../../types/territory.type';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

@Component({
  selector: 'app-fire-national-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './fire-national-create.component.html',
  styleUrl: './fire-national-create.component.css',
})
export class FireNationalCreateComponent {
  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);

  public territoryService = inject(TerritoryService);
  public provinceService = inject(ProvinceService);
  public municipalityService = inject(MunicipalityService);
  public eventService = inject(EventService);
  public fireService = inject(FireService);

  public territories = signal<Territory[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public events = signal<Event[]>([]);

  public formData: FormGroup;

  public error: boolean = false;

  public length: number;
  public latitude: number;

  public municipalityName: string = '';

  async ngOnInit() {
    localStorage.removeItem('coordinates');
    localStorage.removeItem('polygon');

    this.formData = new FormGroup({
      territory: new FormControl(),
      province: new FormControl(),
      municipality: new FormControl(),
      name: new FormControl(),
      start: new FormControl(),
      event: new FormControl(),
      generalNote: new FormControl(),
    });

    this.formData.patchValue({
      territory: 1,
    });

    const territories = await this.territoryService.get();
    this.territories.set(territories);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    const events = await this.eventService.get();
    this.events.set(events);
  }

  closeModal() {
    this.matDialogRef.close();
  }

  onChange(event: any) {
    if (event.target.value == 2) {
      this.closeModal();
    }

    this.matDialog.open(FireForeignCreateComponent, {
      width: '1000px',
      maxWidth: '1000px',
    });
  }

  openModalMapCreate() {
    this.matDialog.open(MapCreateComponent, {
      width: '1000px',
      maxWidth: '1000px',
    });
  }

  async loadMunicipalities(event: any) {
    const province_id = event.target.value;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
  }

  async setMunicipalityId(event: any) {
    const municipality_id = event.value;
    localStorage.setItem('municipality', municipality_id);

    for (let municipality of this.municipalities()) {
      if (municipality.id == Number(localStorage.getItem('municipality'))) {
        this.latitude = Number(municipality.geoPosicion.coordinates[0]);
        this.length = Number(municipality.geoPosicion.coordinates[1]);

        const coordinates = [this.length, this.latitude];

        localStorage.setItem(
          'latitude',
          municipality.geoPosicion.coordinates[0]
        );
        localStorage.setItem('length', municipality.geoPosicion.coordinates[1]);
        localStorage.setItem('coordinates', JSON.stringify(coordinates));

        this.municipalityName = municipality.descripcion;

        this.formData.patchValue({
          name: municipality.descripcion,
        });
      }
    }
  }

  async onSubmit() {
    this.error = false;

    const data = this.formData.value;
    data.coordinates = JSON.parse(localStorage.getItem('coordinates') ?? '{}');

    if (localStorage.getItem('polygon')) {
      data.geoposition = {
        type: 'Polygon',
        coordinates: [data.coordinates],
      };
    } else {
      data.geoposition = {
        type: 'Point',
        coordinates: data.coordinates,
      };
    }

    await this.fireService
      .post(data)
      .then((response) => {
        window.location.href = '/fire';
      })
      .catch((error) => {
        this.error = true;
      });
  }
}
