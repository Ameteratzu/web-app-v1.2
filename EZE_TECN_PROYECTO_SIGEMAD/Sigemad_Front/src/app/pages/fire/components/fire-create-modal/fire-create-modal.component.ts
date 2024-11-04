import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { CountryService } from '../../../../services/country.service';
import { EventService } from '../../../../services/event.service';
import { FireService } from '../../../../services/fire.service';
import { MunicipalityService } from '../../../../services/municipality.service';
import { ProvinceService } from '../../../../services/province.service';
import { TerritoryService } from '../../../../services/territory.service';
import { Countries } from '../../../../types/country.type';
import { Event } from '../../../../types/event.type';
import { Municipality } from '../../../../types/municipality.type';
import { Province } from '../../../../types/province.type';
import { Territory } from '../../../../types/territory.type';
import { MapCreateComponent } from '../../../map-create/map-create.component';

@Component({
  selector: 'app-fire-create-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    DropdownModule,
    InputTextModule,
    CalendarModule,
    InputTextareaModule,
    InputTextareaModule,
    CheckboxModule,
  ],
  templateUrl: './fire-create-modal.component.html',
  styleUrl: './fire-create-modal.component.css',
})
export class FireCreateModalComponent implements OnInit {
  public error: boolean = false;
  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);
  public showInputForeign: boolean = false;
  public territoryService = inject(TerritoryService);
  public provinceService = inject(ProvinceService);
  public municipalityService = inject(MunicipalityService);
  public eventService = inject(EventService);
  public countryServices = inject(CountryService);
  public fireService = inject(FireService);

  public territories = signal<Territory[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public events = signal<Event[]>([]);
  public countries = signal<Countries[]>([]);

  public length: number;
  public latitude: number;

  public municipalityName: string = '';

  public formData: FormGroup;

  async ngOnInit() {
    localStorage.removeItem('coordinates');
    localStorage.removeItem('polygon');

    this.formData = new FormGroup({
      territory: new FormControl(),
      event: new FormControl(),
      province: new FormControl(),
      municipality: new FormControl(),
      denomination: new FormControl(),
      startDate: new FormControl(),
      generalNote: new FormControl(),
      //name: new FormControl(),
      //Foreign
      country: new FormControl(),
      ubication: new FormControl(),
      limitSpain: new FormControl(),
    });

    this.formData.patchValue({
      territory: 1,
    });

    const territories = await this.territoryService.getForCreate();
    this.territories.set(territories);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    const events = await this.eventService.get();
    this.events.set(events);

    const countries = await this.countryServices.get();
    this.countries.set(countries);
  }

  onChange(event: any) {
    if (event.value == 1) {
      this.showInputForeign = false;
    }
    if (event.value == 2) {
      this.showInputForeign = true;
    }
    if (event.value == 3) {
      //TODO
    }
  }

  async loadMunicipalities(event: any) {
    const province_id = event.value;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
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
          denomination: municipality.descripcion,
        });
      }
    }
  }

  openModalMapCreate() {
    this.matDialog.open(MapCreateComponent, {
      width: '1000px',
      maxWidth: '1000px',
    });
  }

  closeModal() {
    this.matDialogRef.close();
  }
}
