import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, signal, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { LocalFiltrosOpeDatosFronteras } from '@services/ope/datos/local-filtro-ope-datos-fronteras.service';
import { OpeDatosFronterasService } from '@services/ope/datos/ope-datos-fronteras.service';
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
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

interface FormType {
  id?: string;
  opeFrontera: { id: string; nombre: string };
  fechaHoraInicioIntervalo: Date;
  fechaHoraFinIntervalo: Date;
  numeroVehiculos: number;
  afluencia: string;
}

@Component({
  selector: 'ope-dato-frontera-create-edit',
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
    MatTableModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-dato-frontera-create-edit-form.component.html',
  styleUrl: './ope-dato-frontera-create-edit-form.component.scss',
})
export class OpeDatoFronteraCreateEdit implements OnInit {
  constructor(
    private filtrosOpeDatosFronterasService: LocalFiltrosOpeDatosFronteras,
    private opeDatosFronterasService: OpeDatosFronterasService,
    public dialogRef: MatDialogRef<OpeDatoFronteraCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,
    private router: Router,

    private territoryService: TerritoryService,
    private countryService: CountryService,
    private autonomousCommunityService: AutonomousCommunityService,
    private provinceService: ProvinceService,
    private municipioService: MunicipalityService,

    @Inject(MAT_DIALOG_DATA) public data: { opeDatoFrontera: any }
  ) {}

  public filteredCountries = signal<Countries[]>([]);

  public territories = signal<Territory[]>([]);
  public autonomousCommunities = signal<AutonomousCommunity[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

  public listaPaisesExtranjeros = signal<Countries[]>([]);
  public listaPaisesNacionales = signal<Countries[]>([]);

  public formData!: FormGroup;

  public dataSource = new MatTableDataSource<any>([]);
  public dataOpeDatosFronteras = signal<FormType[]>([]);
  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  public displayedColumns: string[] = ['numeroVehiculos', 'fechaHoraInicioIntervalo', 'fechaHoraFinIntervalo', 'afluencia', 'opciones'];

  public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  //PCD
  public snackBar = inject(MatSnackBar);
  public utilsService = inject(UtilsService);
  // FIN PCD

  async ngOnInit() {
    this.formData = new FormGroup({
      frontera: new FormControl('', Validators.required),
      fechaHoraInicioIntervalo: new FormControl(moment().format('YYYY-MM-DDTHH:mm'), [Validators.required, FechaValidator.validarFecha]),
      fechaHoraFinIntervalo: new FormControl(moment().format('YYYY-MM-DDTHH:mm'), [Validators.required, FechaValidator.validarFecha]),
      numeroVehiculos: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
      afluencia: new FormControl('', Validators.required),
    });

    if (!this.data.opeDatoFrontera?.id) {
      this.formData.get('municipality')?.disable();
      this.formData.get('provincia')?.disable();
    }

    if (this.data.opeDatoFrontera?.id) {
      //this.loadMunicipalities({ value: this.data.opeDatoFrontera.idProvincia });
      this.formData.patchValue({
        id: this.data.opeDatoFrontera.id,

        frontera: this.data.opeDatoFrontera.idFrontera,
        fechaHoraInicioIntervalo: moment(this.data.opeDatoFrontera.fechaHoraInicioIntervalo).format('YYYY-MM-DD HH:mm'),
        fechaHoraFinIntervalo: moment(this.data.opeDatoFrontera.fechaHoraFinIntervalo).format('YYYY-MM-DD HH:mm'),
        numeroVehiculos: this.data.opeDatoFrontera.numeroVehiculos,
        afluencia: this.data.opeDatoFrontera.numeroVehiculos,
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

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        this.dataOpeDatosFronteras.set([data, ...this.dataOpeDatosFronteras()]);
      } else {
        this.editarItem(this.isCreate());
      }

      formDirective.resetForm();
      this.formData.reset({
        //fecha: moment().toDate(),
        //hora: moment().format('HH:mm'),
        // PCD
        fechaHoraInicioIntervalo: moment().format('YYYY-MM-DD HH:mm'),
        fechaHoraFinIntervalo: moment().format('YYYY-MM-DD HH:mm'),
        // FIN PCD
      });
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

  getFechaFormateada(date: any) {
    return moment(date).format('DD/MM/YY HH:mm');
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);

    this.formData.patchValue({
      ...this.dataOpeDatosFronteras()[index],
      //fechaHoraInicioIntervalo: fechaHoraInicioIntervaloSelected(),
    });

    //this.formData.patchValue(this.dataOtherInformation()[index]);
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOpeDatosFronteras.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.dataOpeDatosFronteras.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  //Función para guardar en base de datos
  async saveList() {
    if (this.isSaving()) {
      return;
    }
    this.isSaving.set(true);
    if (this.dataOpeDatosFronteras().length <= 0) {
      this.snackBar.open('Debe introducir algún elemento en la lista!', '', {
        duration: 3000,
        horizontalPosition: 'center',
        verticalPosition: 'bottom',
        panelClass: ['snackbar-rojo'],
      });
      this.isSaving.set(false);
      return;
    }

    const arrayToSave = this.dataOpeDatosFronteras().map((item) => {
      return {
        id: item.id ?? null,
        //fechaHora: this.getFechaHora(item.fecha, item.hora),
        // PCD

        // FIN PCD
        idOpeFrontera: item.opeFrontera?.id ?? null,
        fechaHoraInicioIntervalo: moment(item.fechaHoraInicioIntervalo).format('MM/DD/YY HH:mm'),
        fechaHoraFinIntervalo: moment(item.fechaHoraFinIntervalo).format('MM/DD/YY HH:mm'),
        numeroVehiculos: item.numeroVehiculos,
        afluencia: item.afluencia,
      };
    });
    const objToSave = {
      //IdSuceso: this.dataProps?.fire?.id,
      IdOpeFrontera: 1,
      lista: arrayToSave,
    };

    try {
      this.spinner.show();

      const resp: { idOpeDatoFrontera: string | number } | any = await this.opeDatosFronterasService.post(objToSave);
      if (resp!.idRegistroActualizacion > 0) {
        this.isSaving.set(false);
        this.snackBar
          .open('Datos guardados correctamente!', '', {
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
      } else {
        //this.showToast({ title: 'Ha ocurrido un error al guardar la lista' });
        this.spinner.hide();
      }
    } catch (error) {
      console.info({ error });
      this.spinner.hide();
    }
  }
}
