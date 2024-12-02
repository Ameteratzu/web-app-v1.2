import { CommonModule } from '@angular/common';
import {
  Component,
  Inject,
  inject,
  Input,
  OnInit,
  signal,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

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

import { FormFieldComponent } from '../../../../shared/Inputs/field.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
import { EventStatus } from '../../../../types/eventStatus.type';
import { EventStatusService } from '../../../../services/eventStatus.service';
import moment from 'moment';
import { Router } from '@angular/router';
import { MapCreateComponent } from '../../../../shared/mapCreate/map-create.component';
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";

const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'LL', // Definir el formato de entrada
  },
  display: {
    dateInput: 'LL', // Definir cómo mostrar la fecha
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'fire-create-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
    FormFieldComponent,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NgxSpinnerModule
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
  templateUrl: './fire-create-edit-form.component.html',
  styleUrl: './fire-create-edit-form.component.scss',
})
export class FireCreateEdit implements OnInit {

  constructor(
    private filtrosIncendioService: LocalFiltrosIncendio,
    private territoryService: TerritoryService,
    private provinceService: ProvinceService,
    private municipalityService: MunicipalityService,
    private eventService: EventService,
    private countryServices: CountryService,
    private fireService: FireService,
    public eventStatusService: EventStatusService,
    public dialogRef: MatDialogRef<FireCreateEdit>,
    private matDialog: MatDialog,

    private router: Router,

    @Inject(MAT_DIALOG_DATA) public data: { fire: any }
  ) {}

  //public filtrosIncendioService = inject(LocalFiltrosIncendio);

  public showInputForeign: boolean = false;

  public territories = signal<Territory[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public listClassEvent = signal<Event[]>([]);
  public countries = signal<Countries[]>([]);
  public listEventStatus = signal<EventStatus[]>([]);

  public length: number = 0;
  public latitude: number = 0;
  public municipalityName: string = '';

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  //MAP
  public coordinates = signal<any>({});
  public polygon = signal<any>({});
  private spinner = inject(NgxSpinnerService);

  async ngOnInit() {
    
    this.formData = new FormGroup({
      territory: new FormControl('', Validators.required),
      classEvent: new FormControl('', Validators.required),
      province: new FormControl('', Validators.required),
      municipality: new FormControl('', Validators.required),
      denomination: new FormControl('', Validators.required),
      startDate: new FormControl('', Validators.required),
      generalNote: new FormControl('', Validators.required),
      eventStatus: new FormControl('', Validators.required),
      
      //Foreign No se utiliza actualmente
      country: new FormControl(''),
      ubication: new FormControl(''),
      limitSpain: new FormControl(false),
    });

    if (!this.data.fire?.id) {
      this.formData.get('territory')?.disable();
      this.formData.patchValue({
        territory: 1,
        classEvent: 1,
        eventStatus: 1,
      });

      this.formData.get('municipality')?.disable();
    }

    if (this.data.fire?.id) {
      this.loadMunicipalities({ value: this.data.fire.idProvincia });

      this.formData.patchValue({
        id: this.data.fire.id,
        territory: this.data.fire.idTerritorio,
        denomination: this.data.fire.denominacion,
        province: this.data.fire.idProvincia,
        municipality: this.data.fire.idMunicipio,
        startDate: moment(this.data.fire.fechaInicio).format('YYYY-MM-DD'),
        generalNote: this.data.fire.notaGeneral,
        classEvent: this.data.fire.idClaseSuceso,
        eventStatus: this.data.fire.idEstadoSuceso,
      });
    }

    const territories = await this.territoryService.getForCreate();
    this.territories.set(territories);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    const events = await this.eventService.get();
    this.listClassEvent.set(events);

    const countries = await this.countryServices.get();
    this.countries.set(countries);

    const listEventStatus = await this.eventStatusService.get();
    this.listEventStatus.set(listEventStatus);
  }

  changeTerritory(event: any) {
    if (event.value == 1) {
      //this.clearValidatosToForeign();
      this.showInputForeign = false;
    }
    if (event.value == 2) {
      //this.addValidatorsToForeign();
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
    this.formData.get('municipality')?.enable();
  }

  async onSubmit() {
    debugger
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      const municipio = this.municipalities().find(
        (item) => item.id === data.municipality
      );

      //Coordenadas del municipio
      data.geoposition = {
        type: 'Point',
        coordinates: [
          municipio?.geoPosicion.coordinates[0],
          municipio?.geoPosicion.coordinates[1],
        ],
      };

      if (this.data.fire?.id) {
        data.id = this.data.fire.id;
        await this.fireService
          .update(data)
          .then((response) => {
            //TODO toast
            /*
          this.messageService.add({
            severity: 'success',
            summary: 'Modificado',
            detail: 'Incendio modificado correctamente',
          });
          */
            new Promise((resolve) => setTimeout(resolve, 2000)).then(
              () => {
                this.spinner.hide();
                 //this.router.navigate([`/fire`])
                 (window.location.href = '/fire')
              }
               
            );
          })
          .catch((error) => {
            console.error('Error', error);
          });
      } else {
        await this.fireService
          .post(data)
          .then((response) => {
            //TODO toast
            this.filtrosIncendioService.setFilters({});
            new Promise((resolve) => setTimeout(resolve, 2000)).then(
              () =>
                //this.router.navigate([`/fire`])
                (window.location.href = '/fire')
            );
          })
          .catch((error) => {
            console.log(error);
          });
      }
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async setMunicipalityId(event: any) {
    const municipality_id = event.value;
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

  openModalMap() {
    if (!this.formData.value.municipality) {
      return;
    }
    const municipioSelected = this.municipalities().find(
      (item) => item.id == this.formData.value.municipality
    );

    if (!municipioSelected) {
      return;
    }

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      height: '780px',
      maxHeight: '780px',
      data: {
        municipio: municipioSelected,
        listaMunicipios: this.municipalities(),
      },
    });

    dialogRef.componentInstance.save.subscribe(
      (features: Feature<Geometry>[]) => {
        //this.featuresCoords = features;
        console.info('features', features);
      }
    );
  }

  closeModal() {
    debugger
    this.dialogRef.close();
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }
}
