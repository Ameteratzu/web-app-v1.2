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
import { EvolutionService } from '../../../services/evolution.service';
import { ImpactTypeService } from '../../../services/impact-type.service';
import { ImpactService } from '../../../services/impact.service';
import { MinorEntityService } from '../../../services/minor-entity.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { CoordinationAddress } from '../../../types/coordination-address';
import { MinorEntity } from '../../../types/minor-entity.type';
import { Municipality } from '../../../types/municipality.type';
import { Province } from '../../../types/province.type';

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
  ],
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
  @Input() municipio: any;

  public evolutionService = inject(EvolutionService);
  public toast = inject(MatSnackBar);
  private provinceService = inject(ProvinceService);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private municipalityService = inject(MunicipalityService);
  private minorService = inject(MinorEntityService);
  private tiposImpactoService = inject(ImpactTypeService);
  private impactosService = inject(ImpactService);

  public polygon = signal<any>([]);

  public displayedColumns: string[] = ['fechaHora', 'tipo', 'denominacion', 'numero', 'opciones'];

  formData!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public isCreate = signal<number>(-1);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public listadoTipoImpacto = signal<string[]>([]);
  public listadoImpacto = signal<any[]>([]);
  public minors = signal<MinorEntity[]>([]);
  public dataSource = new MatTableDataSource<any>([]);

  //Array con toda la data indexada
  public listadoData = signal<any[]>([]);

  public listadoGrupos = signal<any[]>([]);
  public listadoDenominaciones = signal<any[]>([]);

  async ngOnInit() {
    const listadoTipoImpacto: any = await this.tiposImpactoService.get();
    this.listadoTipoImpacto.set(listadoTipoImpacto);

    const dataIndexada: any = await this.impactosService.get();
    console.info('dataIndexada', dataIndexada);
    this.listadoData.set(dataIndexada);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    this.formData = this.fb.group({
      tipo: [null, Validators.required],
      grupo: [null, Validators.required],
      denominacion: [null, Validators.required],
      numero: [null, Validators.required],
      localizacion: [null, Validators.required],
      observacion: [''],
    });

    this.formData.get('grupo')?.disable();
    this.formData.get('denominacion')?.disable();

    this.spinner.hide();
    console.info('municipio', this.municipio);
  }

  async loadMunicipalities(event: any) {
    const province_id = event.value.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formData.get('idMunicipio')?.enable();
  }

  async loadGrupos(event: any) {
    const indexTipoImpacto = this.listadoTipoImpacto().findIndex((item: string) => item === event.value);

    this.listadoGrupos.set(this.listadoData()[indexTipoImpacto].grupos);
    this.listadoDenominaciones.set([]);

    this.formData.get('grupo')?.enable();
    this.formData.get('denominacion')?.disable();
  }

  async loadDenominacion(event: any) {
    console.info('loadGrupo-', event.value);
    this.listadoDenominaciones.set(event.value.subgrupos);
    this.formData.get('denominacion')?.enable();
  }

  openModalMap() {
    console.info('openmap');
    if (!this.formData.value.municipio) {
      return;
    }
    const municipioSelected = this.municipalities().find((item) => item.id == this.formData.value.municipio);

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
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        this.evolutionService.dataAffectedArea.set([data, ...this.evolutionService.dataAffectedArea()]);
      } else {
        this.editarItem(this.isCreate());
      }

      this.formData.reset();
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    this.spinner.show();
    if (this.evolutionService.dataAffectedArea().length > 0) {
      this.save.emit(true);
    } else {
      this.spinner.hide();
      this.showToast();
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
    const dataEditada = this.formData.value;
    this.evolutionService.dataAffectedArea.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.evolutionService.dataAffectedArea.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);
    this.formData.patchValue(this.evolutionService.dataAffectedArea()[index]);
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YY');
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal() {
    this.save.emit(false);
  }
}
