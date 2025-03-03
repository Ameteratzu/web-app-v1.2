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
import { AlertService } from '@shared/alert/alert.service';
import { LocalFiltrosOpePuntosControlCarreteras } from '@services/ope/local-filtro-ope-puntos-control-carreteras.service';
import { OpePuntosControlCarreterasService } from '@services/ope/ope-puntos-control-carreteras.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { FORMATO_FECHA } from '../../../../../../types/date-formats';
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
import { COUNTRIES_ID } from '@type/constants';

@Component({
  selector: 'ope-punto-control-carretera-create-edit',
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
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-punto-control-carretera-create-edit-form.component.html',
  styleUrl: './ope-punto-control-carretera-create-edit-form.component.scss',
})
export class OpePuntoControlCarreteraCreateEdit implements OnInit {
  constructor(
    private filtrosOpePuntosControlCarreterasService: LocalFiltrosOpePuntosControlCarreteras,
    private opePuntosControlCarreterasService: OpePuntosControlCarreterasService,
    public dialogRef: MatDialogRef<OpePuntoControlCarreteraCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,
    private router: Router,

    private territoryService: TerritoryService,
    private countryService: CountryService,
    private autonomousCommunityService: AutonomousCommunityService,
    private provinceService: ProvinceService,
    private municipioService: MunicipalityService,

    @Inject(MAT_DIALOG_DATA) public data: { opePuntoControlCarretera: any }
  ) {}

  public filteredCountries = signal<Countries[]>([]);

  public territories = signal<Territory[]>([]);
  public autonomousCommunities = signal<AutonomousCommunity[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

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
    this.formData = new FormGroup({
      nombre: new FormControl('', Validators.required),
      autonomousCommunity: new FormControl(''),
      CCAA: new FormControl(''),
      province: new FormControl(''),
      provincia: new FormControl(''),
      municipality: new FormControl('', Validators.required),
      carreteraPK: new FormControl('', Validators.required),
      coordenadaUTM_X: new FormControl('', Validators.required),
      coordenadaUTM_Y: new FormControl('', Validators.required),
      transitoMedioVehiculos: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
      transitoAltoVehiculos: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
    });

    if (!this.data.opePuntoControlCarretera?.id) {
      this.formData.get('municipality')?.disable();
      this.formData.get('provincia')?.disable();
    }

    if (this.data.opePuntoControlCarretera?.id) {
      //this.loadMunicipalities({ value: this.data.opePuntoControlCarretera.idProvincia });
      this.formData.patchValue({
        id: this.data.opePuntoControlCarretera.id,
        nombre: this.data.opePuntoControlCarretera.nombre,
        autonomousCommunity: this.data.opePuntoControlCarretera.idCcaa,
        province: this.data.opePuntoControlCarretera.idProvincia,
        municipality: this.data.opePuntoControlCarretera.idMunicipio,
        carreteraPK: this.data.opePuntoControlCarretera.carreteraPK,
        coordenadaUTM_X: this.data.opePuntoControlCarretera.coordenadaUTM_X,
        coordenadaUTM_Y: this.data.opePuntoControlCarretera.coordenadaUTM_Y,
        transitoMedioVehiculos: this.data.opePuntoControlCarretera.transitoMedioVehiculos,
        transitoAltoVehiculos: this.data.opePuntoControlCarretera.transitoAltoVehiculos,
      });
    }

    /*
    const countriesExtranjeros = await this.countryService.getExtranjeros();
    this.listaPaisesExtranjeros.set(countriesExtranjeros);
    const countriesNacionales = await this.countryService.getNacionales();
    this.listaPaisesNacionales.set(countriesNacionales);

    this.filteredCountries.set(countriesNacionales);

    const territories = await this.territoryService.get();
    this.territories.set(territories);
    */

    await this.loadCommunities();

    //this.onSubmit();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ('refreshFilterForm' in changes) {
      //this.onSubmit();
      alert('aa');
    }
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      //const municipio = this.municipalities().find((item) => item.id === data.municipality);

      if (this.data.opePuntoControlCarretera?.id) {
        data.id = this.data.opePuntoControlCarretera.id;
        await this.opePuntosControlCarreterasService
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
        await this.opePuntosControlCarreterasService
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
    /*
    if (country === '9999') {
      alert('aa');
      this.autonomousCommunities.set([]);
      return;
    }
    */
    const autonomousCommunities = await this.autonomousCommunityService.getByCountry(COUNTRIES_ID.SPAIN.toString());
    this.autonomousCommunities.set(autonomousCommunities);
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
