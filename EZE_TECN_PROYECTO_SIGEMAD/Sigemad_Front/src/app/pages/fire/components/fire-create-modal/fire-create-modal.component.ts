import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  MatDialog,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
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
import { LocalFiltrosIncendio } from '../../../../services/local-filtro-incendio.service';
import { MapCreateComponent } from '../../../../shared/mapCreate/map-create.component';
import { FormFieldComponent } from '../../../../shared/Inputs/field.component';

@Component({
  selector: 'app-fire-create-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
    ToastModule,
    FormFieldComponent
  ],
  providers: [MessageService],
  templateUrl: './fire-create-modal.component.html',
  styleUrl: './fire-create-modal.component.css',
})
export class FireCreateModalComponent implements OnInit {
  featuresCoords: Feature<Geometry>[] = [];

  public error: boolean = false;

  public filtrosIncendioService = inject(LocalFiltrosIncendio);
  
  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);
  public showInputForeign: boolean = false;
  public territoryService = inject(TerritoryService);
  public provinceService = inject(ProvinceService);
  public municipalityService = inject(MunicipalityService);
  public eventService = inject(EventService);
  public countryServices = inject(CountryService);
  public fireService = inject(FireService);
  public classValidate = signal<string>('needs-validation');

  public messageService = inject(MessageService);

  public territories = signal<Territory[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public events = signal<Event[]>([]);
  public countries = signal<Countries[]>([]);

  public length: number;
  public latitude: number;
  public municipalityName: string = '';

  public formData: FormGroup;

  public today: string;

  //MAP
  public coordinates = signal<any>({});
  public polygon = signal<any>({});

  async ngOnInit() {
    localStorage.removeItem('coordinates');
    localStorage.removeItem('polygon');

    this.today = new Date().toISOString().split('T')[0];

    this.formData = new FormGroup({
      territory: new FormControl(''),
      event: new FormControl(''),
      province: new FormControl(''),
      municipality: new FormControl(''),
      denomination: new FormControl(''),
      startDate: new FormControl(''),
      generalNote: new FormControl(),
      //name: new FormControl(),
      //Foreign
      country: new FormControl(''),
      ubication: new FormControl(''),
      limitSpain: new FormControl(false),
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
    if (event.target.value == 1) {
      //this.clearValidatosToForeign();
      this.showInputForeign = false;
    }
    if (event.target.value == 2) {
      //this.addValidatorsToForeign();
      this.showInputForeign = true;
    }
    if (event.target.value == 3) {
      //TODO
    }
  }
  /*
  addValidatorsToForeign() {
    this.formData.get('country')?.setValidators([Validators.required]);
    this.formData.get('ubication')?.setValidators([Validators.required]);

    this.formData.get('country')?.updateValueAndValidity();
    this.formData.get('ubication')?.updateValueAndValidity();
  }

  clearValidatosToForeign() {
    this.formData.get('country')?.clearValidators();
    this.formData.get('ubication')?.clearValidators();

    this.formData.get('country')?.updateValueAndValidity();
    this.formData.get('ubication')?.updateValueAndValidity();
  }
  */

  async loadMunicipalities(event: any) {
    const province_id = event.target.value;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.error = false;

      this.classValidate.set('needs-validation');
      const data = this.formData.value;

      const municipio = this.municipalities().find(
        (item) => item.id === data.municipality
      );

      data.geoposition = {
        type: 'Point',
        coordinates: [
          municipio?.geoPosicion.coordinates[0],
          municipio?.geoPosicion.coordinates[1],
        ],
      };

      /*
      if (this.formData.valid) {
        this.classValidate.set('needs-validation');
      }
      */

      await this.fireService
        .post(data)
        .then((response) => {
          this.messageService.add({
            severity: 'success',
            summary: 'Creado',
            detail: 'Incendio creado correctamente',
          });
          this.filtrosIncendioService.setFilters({});
          new Promise((resolve) => setTimeout(resolve, 2000)).then(
            () => (window.location.href = '/fire')
          );
        })
        .catch((error) => {
          console.log(error);
          this.error = true;
        });
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async setMunicipalityId(event: any) {
    const municipality_id = event.target.value;
    localStorage.setItem('municipality', municipality_id);

    for (let municipality of this.municipalities()) {
      if (municipality.id == Number(localStorage.getItem('municipality'))) {
        this.municipalityName = municipality.descripcion;

        this.formData.patchValue({
          denomination: municipality.descripcion,
        });
      }
    }
  }

  openModalMapCreate() {
    let x;
    if (this.showInputForeign) {
      const paises = this.countries().find(
        (item) => item.id == this.formData.value.country
      );
      x = paises;
    } else {
      const municipio = this.municipalities().find(
        (item) => item.id == this.formData.value.municipality
      );
      x = municipio;
    }

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      height: '780px',
      maxHeight: '780px',
      data: { municipio: x, listaMunicipios: this.municipalities() },
    });

    dialogRef.componentInstance.save.subscribe(
      (features: Feature<Geometry>[]) => {
        this.featuresCoords = features;
      }
    );
  }

  closeModal() {
    this.matDialogRef.close();
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }
}
