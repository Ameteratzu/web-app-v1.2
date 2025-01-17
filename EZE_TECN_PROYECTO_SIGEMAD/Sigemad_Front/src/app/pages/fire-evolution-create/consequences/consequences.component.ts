import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output, signal, ViewChild } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import moment from 'moment';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';
import { ConsequenceService } from '../../../services/consequence.service';
import { EvolutionService } from '../../../services/evolution.service';
import { ImpactTypeService } from '../../../services/impact-type.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { CoordinationAddress } from '../../../types/coordination-address';
import { MinorEntity } from '../../../types/minor-entity.type';
import { Municipality } from '../../../types/municipality.type';
import { Province } from '../../../types/province.type';

import { CampoDinamico } from '../../../shared/campoDinamico/campoDinamico.component';
const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-consequences',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    CommonModule,
    MatInputModule,
    FlexLayoutModule,
    MatGridListModule,
    MatButtonModule,
    MatSelectModule,
    MatTableModule,
    MatIconModule,
    NgxSpinnerModule,
    CampoDinamico,
  ],
  /*declarations: [
    // otros componentes
    CampoDinamico,
  ],*/
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
  templateUrl: './consequences.component.html',
  styleUrl: './consequences.component.scss',
})
export class ConsequencesComponent {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number; municipio: any };
  @Output() save = new EventEmitter<any>();
  @Input() dataProp: any;

  public evolutionService = inject(EvolutionService);
  public toast = inject(MatSnackBar);
  private provinceService = inject(ProvinceService);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private municipalityService = inject(MunicipalityService);
  private tiposImpactoService = inject(ImpactTypeService);
  private consecuenciaService = inject(ConsequenceService);

  public polygon = signal<any>([]);

  public displayedColumns: string[] = ['fechaHora', 'tipo', 'denominacion', 'numero', 'opciones'];

  formData!: FormGroup;
  formDataComplementarios!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public isCreate = signal<number>(-1);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public listadoTipoImpacto = signal<string[]>([]);
  public listadoImpacto = signal<any[]>([]);
  public minors = signal<MinorEntity[]>([]);
  public dataSource = new MatTableDataSource<any>([]);

  public listadoGrupos = signal<any[]>([]);
  public listadoDenominaciones = signal<any[]>([]);
  public listadoCamposComplementarios = signal<any[]>([]);

  async ngOnInit() {
    const { fire } = this.dataProp;
    const { municipio } = fire;
    const listadoTipoImpacto: any = await this.tiposImpactoService.get();
    this.listadoTipoImpacto.set(listadoTipoImpacto);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    this.formData = this.fb.group({
      tipo: [null, Validators.required],
      grupo: [null, Validators.required],
      denominacion: [null, Validators.required],
      numero: [null, Validators.required],
      localizacion: [municipio.descripcion, Validators.required], //MUNICIPIO DEL INCENDIO
      observacion: [''],
    });

    this.formData.get('localizacion')?.disable();
    this.formData.get('grupo')?.disable();
    this.formData.get('denominacion')?.disable();

    this.spinner.hide();
    console.info('dataProp', this.dataProp);
  }

  onFormGroupChange(formCamposComplementario: any) {
    this.formDataComplementarios = formCamposComplementario;
  }

  async cargarMunicipio(event: any) {
    const province_id = event.value.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formData.get('idMunicipio')?.enable();
  }

  async loadGrupos(event: any) {
    const dataGrupo: any = await this.consecuenciaService.getGrupo(event.value);

    this.listadoGrupos.set(dataGrupo);
    this.listadoDenominaciones.set([]);

    this.formData.get('grupo')?.enable();
    this.formData.get('denominacion')?.disable();
  }

  async loadDenominacion(event: any) {
    const { tipo } = this.formData.value;
    const dataDenominaciones: any = await this.consecuenciaService.getDenominaciones(tipo, event.value);
    this.listadoDenominaciones.set(dataDenominaciones);
    this.formData.patchValue({ denominacion: '' });
    this.formData.get('denominacion')?.enable();
  }

  async loadCamposImpacto(event: any) {
    const dataCamposComplementarios: any = await this.consecuenciaService.getCamposImpacto(event.value.id);
    this.listadoCamposComplementarios.set(dataCamposComplementarios);
    console.info('dataCamposComplementarios'), dataCamposComplementarios;
  }

  openModalMap() {
    const municipioSelected = this.dataProp?.fire?.municipio;
    if (!municipioSelected) {
      return;
    }

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      //height: '780px',
      //maxHeight: '780px',
      data: {
        municipio: municipioSelected,
        listaMunicipios: this.municipalities(),
        defaultPolygon: this.polygon(),
      },
    });

    dialogRef.componentInstance.save.subscribe((features: Feature<Geometry>[]) => {
      this.polygon.set(features);
    });
  }
  onSubmit() {
    this.formDataComplementarios.markAllAsTouched();
    if (this.formData.valid && this.formDataComplementarios?.valid) {
      const data = {
        IdImpactoClasificado: '1',
        ...this.formData.value,
        ...this.formDataComplementarios.value,
      };
      if (this.isCreate() == -1) {
        this.evolutionService.dataConse.set([data, ...this.evolutionService.dataConse()]);
      } else {
        this.editarItem(this.isCreate());
      }

      this.listadoCamposComplementarios.set([]);
      this.listadoDenominaciones.set([]);
      this.listadoGrupos.set([]);

      this.formData.reset();
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    if (this.evolutionService.dataConse().length > 0 && this.isCreate() == 1) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.isCreate() == -1) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }

  editarItem(index: number) {
    const dataEditada = {
      ...this.formData.value,
      ...this.formDataComplementarios.value,
    };
    this.evolutionService.dataConse.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.evolutionService.dataConse.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  async seleccionarItem(index: number) {
    this.isCreate.set(index);
    const data = this.evolutionService.dataConse()[index];
    console.info('data', data);
    await this.loadGrupos({ value: data.tipo });
    await this.loadDenominacion({ value: data.grupo });
    await this.loadCamposImpacto({ value: data.denominacion });

    const newData = this.listadoCamposComplementarios().map((item: any) => ({
      ...item,
      initValue: data[item.campo],
    }));
    this.listadoCamposComplementarios.set(newData);
    this.formData.patchValue(data);
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YYYY');
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal() {
    this.save.emit({ save: false, delete: false, close: true, update: false });
  }

  delete() {
    this.save.emit({ save: false, delete: true, close: false, update: false });
  }
}
