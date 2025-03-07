import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Geometry } from 'ol/geom';
import Feature from 'ol/Feature';
// PCD
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { LocalFiltrosOpePuertos } from '@services/ope/local-filtro-ope-puertos.service';
import { OpePuertosService } from '@services/ope/ope-puertos.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { MY_DATE_FORMATS } from '../../../../../../types/date-formats';
import { UtilsService } from '@shared/services/utils.service';
import { ProvinceService } from '@services/province.service';
import { MunicipalityService } from '@services/municipality.service';
import { CountryService } from '@services/country.service';
import { Province } from '@type/province.type';
import { Municipality } from '@type/municipality.type';
import { Countries } from '@type/country.type';
import { MapCreateComponent } from '@shared/mapCreate/map-create.component';
import { TerritoryService } from '@services/territory.service';
import { AutonomousCommunityService } from '@services/autonomous-community.service';
// FIN PCD

@Component({
  selector: 'ope-puerto-create-edit',
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
    NgxSpinnerModule,
    TooltipDirective,
    DragDropModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
  templateUrl: './ope-puerto-create-edit-form.component.html',
  styleUrl: './ope-puerto-create-edit-form.component.scss',
})
export class OpePuertoCreateEdit implements OnInit {
  COUNTRIES_ID = {
    PORTUGAL: 1,
    SPAIN: 60,
    FRANCE: 65,
  };

  constructor(
    private filtrosOpePuertosService: LocalFiltrosOpePuertos,
    private opePuertosService: OpePuertosService,
    public dialogRef: MatDialogRef<OpePuertoCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,
    private router: Router,

    private territoryService: TerritoryService,
    private countryService: CountryService,
    private autonomousCommunityService: AutonomousCommunityService,
    private provinceService: ProvinceService,
    private municipioService: MunicipalityService,

    @Inject(MAT_DIALOG_DATA) public data: { opePuerto: any }
  ) {}

  public filteredCountries = signal<Countries[]>([]);

  public listadoTerritorio = signal<any[]>([]);
  public listadoPaises = signal<any[]>([]);
  public listadoCCAA = signal<any[]>([]);
  public listadoProvincia = signal<any[]>([]);
  public listadoMunicipio = signal<any[]>([]);

  public listaPaisesExtranjeros = signal<Countries[]>([]);
  public listaPaisesNacionales = signal<Countries[]>([]);

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  //PCD
  public snackBar = inject(MatSnackBar);
  public utilsService = inject(UtilsService);
  // FIN PCD

  async ngOnInit() {
    this.spinner.show();
    this.formData = new FormGroup(
      {
        nombre: new FormControl('', Validators.required),
        territory: new FormControl(1),
        country: new FormControl(this.COUNTRIES_ID.SPAIN),
        CCAA: new FormControl('', Validators.required),
        province: new FormControl('', Validators.required),
        municipality: new FormControl('', Validators.required),
        fechaValidezDesde: new FormControl(moment().format('YYYY-MM-DDTHH:mm'), [Validators.required, FechaValidator.validarFecha]),
        fechaValidezHasta: new FormControl(moment().format('YYYY-MM-DDTHH:mm'), [Validators.required, FechaValidator.validarFecha]),
        capacidad: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
      },
      {
        validators: [FechaValidator.validarFechaFinPosteriorFechaInicio('fechaValidezDesde', 'fechaValidezHasta')],
      }
    );

    if (!this.data.opePuerto?.id) {
      this.formData.get('municipality')?.disable();
    }

    if (this.data.opePuerto?.id) {
      //this.loadMunicipalities({ value: this.data.opePuerto.idProvincia });
      this.formData.patchValue({
        id: this.data.opePuerto.id,
        nombre: this.data.opePuerto.nombre,
        province: this.data.opePuerto.idProvincia,
        municipality: this.data.opePuerto.idMunicipio,
        fechaInicioFaseSalida: moment(this.data.opePuerto.fechaInicioFaseSalida).format('YYYY-MM-DD HH:mm'),
        fechaFinFaseSalida: moment(this.data.opePuerto.fechaFinFaseSalida).format('YYYY-MM-DD HH:mm'),
        fechaInicioFaseRetorno: moment(this.data.opePuerto.fechaInicioFaseRetorno).format('YYYY-MM-DD HH:mm'),
        fechaFinFaseRetorno: moment(this.data.opePuerto.fechaFinFaseRetorno).format('YYYY-MM-DD HH:mm'),
      });
    }

    const countriesExtranjeros = await this.countryService.getExtranjeros();
    this.listaPaisesExtranjeros.set(countriesExtranjeros);
    const countriesNacionales = await this.countryService.getNacionales();
    this.listaPaisesNacionales.set(countriesNacionales);

    this.filteredCountries.set(countriesNacionales);

    const territories = await this.territoryService.get();
    this.listadoTerritorio.set(territories);

    await this.loadCommunities();

    this.spinner.hide();
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      //const municipio = this.municipalities().find((item) => item.id === data.municipality);

      if (this.data.opePuerto?.id) {
        data.id = this.data.opePuerto.id;
        await this.opePuertosService
          .update(data)
          .then((response) => {
            // PCD
            this.snackBar
              .open('Datos modificados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.closeModal({ refresh: true });
                this.spinner.hide();
              });
            // FIN PCD
          })
          .catch((error) => {
            console.error('Error', error);
          });
      } else {
        await this.opePuertosService
          .post(data)
          .then((response) => {
            this.snackBar
              .open('Datos creados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.closeModal({ refresh: true });
                this.spinner.hide();
              });
          })
          .catch((error) => {
            console.log(error);
          });
      }
    } else {
      this.formData.markAllAsTouched();
    }
  }

  closeModal(params?: any) {
    this.dialogRef.close(params);
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  async loadCommunities(country?: any) {
    if (country === '9999') {
      this.listadoCCAA.set([]);
      return;
    }
    const autonomousCommunities = await this.autonomousCommunityService.getByCountry(country ?? this.formData.value.country);
    this.listadoCCAA.set(autonomousCommunities);
  }

  // para meter en uns servicio común
  async changeTerritory(event: any) {
    this.formData.patchValue({
      country: event.value == 1 ? this.COUNTRIES_ID.SPAIN : '',
      autonomousCommunity: '',
      province: '',
      municipality: '',
    });
    this.loadCommunities(event.value.id == 1 ? this.COUNTRIES_ID.SPAIN : '9999');
    if (event.value == 1) {
      this.filteredCountries.set(this.listaPaisesNacionales());
    }
    if (event.value == 2) {
      this.filteredCountries.set(this.listaPaisesExtranjeros());
    }
    if (event.value == 3) {
      this.filteredCountries.set([]);
    }
  }

  // para meter en uns servicio común
  async loadProvinces(event: any) {
    const ac_id = event.value.id;
    const provinces = await this.provinceService.get(ac_id);
    this.listadoProvincia.set(provinces);
  }

  async loadMunicipios(event: any) {
    const provinciaId = event.value.id;
    const municipios = await this.municipioService.get(provinciaId);
    this.listadoMunicipio.set(municipios);
  }
}
