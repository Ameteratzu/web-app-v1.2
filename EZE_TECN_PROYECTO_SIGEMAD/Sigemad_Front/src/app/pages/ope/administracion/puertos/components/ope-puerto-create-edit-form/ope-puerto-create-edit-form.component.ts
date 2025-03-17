import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, signal, SimpleChanges } from '@angular/core';
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
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { LocalFiltrosOpePuertos } from '@services/ope/administracion/local-filtro-ope-puertos.service';
import { OpePuertosService } from '@services/ope/administracion/ope-puertos.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { UtilsService } from '@shared/services/utils.service';
import { ProvinceService } from '@services/province.service';
import { MunicipalityService } from '@services/municipality.service';
import { CountryService } from '@services/country.service';
import { Province } from '@type/province.type';
import { Municipality } from '@type/municipality.type';
import { Countries } from '@type/country.type';
import { TerritoryService } from '@services/territory.service';
import { AutonomousCommunityService } from '@services/autonomous-community.service';
import { Territory } from '@type/territory.type';
import { AutonomousCommunity } from '@type/autonomous-community.type';
import { COUNTRIES_ID, FECHA_MAXIMA_DATEPICKER, FECHA_MINIMA_DATEPICKER } from '@type/constants';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { FORMATO_FECHA } from '@type/date-formats';
import { OpeFase } from '@type/ope/administracion/ope-fase.type';
import { OpeFasesService } from '@services/ope/administracion/ope-fases.service';

@Component({
  selector: 'ope-puerto-create-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
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
    DragDropModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-puerto-create-edit-form.component.html',
  styleUrl: './ope-puerto-create-edit-form.component.scss',
})
export class OpePuertoCreateEdit implements OnInit {
  constructor(
    private filtrosOpePuertosService: LocalFiltrosOpePuertos,
    private opePuertosService: OpePuertosService,
    private opeFasesService: OpeFasesService,
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

  public territories = signal<Territory[]>([]);
  public autonomousCommunities = signal<AutonomousCommunity[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

  public listaPaisesExtranjeros = signal<Countries[]>([]);
  public listaPaisesNacionales = signal<Countries[]>([]);

  public opeFases = signal<OpeFase[]>([]);

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  //PCD
  public snackBar = inject(MatSnackBar);
  public utilsService = inject(UtilsService);
  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;
  // FIN PCD

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        nombre: new FormControl('', Validators.required),
        opeFase: new FormControl('', Validators.required),
        territory: new FormControl(1),
        country: new FormControl(COUNTRIES_ID.SPAIN),
        autonomousCommunity: new FormControl(''),
        CCAA: new FormControl(''),
        //province: new FormControl(''),
        provincia: new FormControl(''),
        municipality: new FormControl('', Validators.required),
        coordenadaUTM_X: new FormControl('', Validators.required),
        coordenadaUTM_Y: new FormControl('', Validators.required),
        fechaValidezDesde: new FormControl(new Date(), [Validators.required, FechaValidator.validarFecha]),
        fechaValidezHasta: new FormControl(new Date(), [Validators.required, FechaValidator.validarFecha]),
        capacidad: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
      },
      {
        validators: [FechaValidator.validarFechaFinPosteriorFechaInicio('fechaValidezDesde', 'fechaValidezHasta')],
      }
    );

    if (!this.data.opePuerto?.id) {
      this.formData.get('municipality')?.disable();
      this.formData.get('provincia')?.disable();
    }

    if (this.data.opePuerto?.id) {
      //this.loadMunicipalities({ value: this.data.opePuerto.idProvincia });
      this.loadProvinces({ value: this.data.opePuerto.idCcaa });
      this.loadMunicipios({ value: this.data.opePuerto.idProvincia });
      this.formData.patchValue({
        id: this.data.opePuerto.id,
        nombre: this.data.opePuerto.nombre,
        opeFase: this.data.opePuerto.idOpeFase,
        autonomousCommunity: this.data.opePuerto.idCcaa,
        provincia: this.data.opePuerto.idProvincia,
        municipality: this.data.opePuerto.idMunicipio,
        coordenadaUTM_X: this.data.opePuerto.coordenadaUTM_X,
        coordenadaUTM_Y: this.data.opePuerto.coordenadaUTM_Y,
        fechaValidezDesde: moment(this.data.opePuerto.fechaValidezDesde).format('YYYY-MM-DD'),
        fechaValidezHasta: moment(this.data.opePuerto.fechaValidezHasta).format('YYYY-MM-DD'),
        capacidad: this.data.opePuerto.capacidad,
      });
    }

    const opeFases = await this.opeFasesService.get();
    this.opeFases.set(opeFases);

    const countriesExtranjeros = await this.countryService.getExtranjeros();
    this.listaPaisesExtranjeros.set(countriesExtranjeros);
    const countriesNacionales = await this.countryService.getNacionales();
    this.listaPaisesNacionales.set(countriesNacionales);

    this.filteredCountries.set(countriesNacionales);

    const territories = await this.territoryService.get();
    this.territories.set(territories);

    await this.loadCommunities();
  }

  /*
  ngOnChanges(changes: SimpleChanges): void {
    if ('refreshFilterForm' in changes) {
      //this.onSubmit();
      alert('aa');
    }
  }
  */

  async onSubmit() {
    alert(this.formData);
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

  // para meter en uns servicio común
  async changeTerritory(event: any) {
    this.formData.patchValue({
      country: event.value == 1 ? COUNTRIES_ID.SPAIN : '',
      autonomousCommunity: '',
      provincia: '',
      municipality: '',
    });
    this.loadCommunities(event.value == 1 ? COUNTRIES_ID.SPAIN : '9999');
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

  async loadCommunities(country?: any) {
    if (country === '9999') {
      this.autonomousCommunities.set([]);
      this.formData.get('autonomousCommunity')?.disable();
      this.formData.get('provincia')?.disable();
      this.formData.get('municipality')?.disable();
      return;
    }
    const autonomousCommunities = await this.autonomousCommunityService.getByCountry(country ?? this.formData.value.country);
    this.autonomousCommunities.set(autonomousCommunities);
    this.formData.get('autonomousCommunity')?.enable();
  }

  async loadProvinces(event: any) {
    const ac_id = event.value;
    const provinces = await this.provinceService.get(ac_id);
    this.provinces.set(provinces);
    this.formData.get('provincia')?.enable();
  }

  async loadMunicipios(event: any) {
    const provinciaId = event.value;
    const municipios = await this.municipioService.get(provinciaId);
    this.municipalities.set(municipios);
    this.formData.get('municipality')?.enable();
  }
}
