import { Component, EventEmitter, inject, Input, Output, signal, ViewChild } from '@angular/core';

import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { MinorEntityService } from '../../../services/minor-entity.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { CoordinationAddress } from '../../../types/coordination-address';
import { MinorEntity } from '../../../types/minor-entity.type';
import { Municipality } from '../../../types/municipality.type';
import { Province } from '../../../types/province.type';
import { SavePayloadModal } from '../../../types/save-payload-modal';

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
  selector: 'app-area',
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
  templateUrl: './area.component.html',
  styleUrl: './area.component.scss',
})
export class AreaComponent {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;

  public evolutionService = inject(EvolutionService);
  public toast = inject(MatSnackBar);
  private provinceService = inject(ProvinceService);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private municipalityService = inject(MunicipalityService);
  private minorService = inject(MinorEntityService);
  private cdr = inject(ChangeDetectorRef);
  public polygon = signal<any>([]);

  public displayedColumns: string[] = ['fechaHora', 'procendenciaDestino', 'descripcion', 'fichero', 'opciones'];

  formData!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public isCreate = signal<number>(-1);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public minors = signal<MinorEntity[]>([]);
  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    this.formData = this.fb.group({
      fechaHora: [new Date(), Validators.required],
      provincia: [null, Validators.required],
      municipio: [null, Validators.required],
      entidadMenor: [null],
      observaciones: [''],
      fichero: ['', Validators.required],
    });
    this.formData.get('municipio')?.disable();
    this.formData.get('fichero')?.disable();
    this.formData.get('entidadMenor')?.disable();

    if (this.editData) {
      if (this.evolutionService.dataAffectedArea().length === 0) {
        this.evolutionService.dataAffectedArea.set(this.editData.areaAfectadas);
        this.polygon.set(this.editData.geoPosicion?.coordinates[0]);
      }
    }
    this.spinner.hide();
  }

  async loadMunicipalities(event: any) {
    this.spinner.show();
    const province_id = event.value;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formData.get('municipio')?.enable();
    this.spinner.hide();
  }

  async loadMinor(event: any) {
    this.spinner.show();
    const muni_id = event.value;
    const minor = await this.minorService.get(muni_id);
    this.minors.set(minor);
    this.formData.get('entidadMenor')?.enable();
    this.spinner.hide();
  }

  onSubmit(formDirective: FormGroupDirective) {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        data.geoPosicion = {
          type: 'Polygon',
          coordinates: [this.polygon()],
        };
        this.evolutionService.dataAffectedArea.set([data, ...this.evolutionService.dataAffectedArea()]);
      } else {
        this.editarItem(this.isCreate());
      }

      formDirective.resetForm({
        fechaHora: new Date(),
      });
      this.formData.reset();
      this.formData.get('municipio')?.disable();
      this.formData.get('entidadMenor')?.disable();
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    if (this.evolutionService.dataAffectedArea().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
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

  async seleccionarItem(index: number) {
    this.isCreate.set(index);
    const data = this.evolutionService.dataAffectedArea()[index];
    this.spinner.show();

    if (data.provincia.id) {
      const municipalities = await this.municipalityService.get(data.provincia.id);
      this.municipalities.set(municipalities);
    }
    if (data.entidadMenor.id && data.municipio.id) {
      const minor = await this.minorService.get(data.municipio.id);
      this.minors.set(minor);
    }
    this.formData.get('fechaHora')?.setValue(data.fechaHora);
    this.formData.get('observaciones')?.setValue(data.observaciones);
    this.polygon.set(this.evolutionService.dataAffectedArea()[index]?.geoPosicion?.coordinates[0]);

    if (data.id) {
      this.formData.get('provincia')?.setValue(data.provincia.id);
      this.formData.get('municipio')?.setValue(data.municipio.id);
      this.formData.get('entidadMenor')?.setValue(data.entidadMenor.id);
    } else {
      this.formData.get('provincia')?.setValue(data.provincia);
      this.formData.get('municipio')?.setValue(data.municipio);
      this.formData.get('entidadMenor')?.setValue(data.entidadMenor);
    }

    this.formData.get('entidadMenor')?.enable();
    this.formData.get('municipio')?.enable();

    this.spinner.hide();
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
    this.save.emit({ save: false, delete: false, close: true, update: false });
  }

  delete() {
    this.save.emit({ save: false, delete: true, close: false, update: false });
  }

  getProvincesById(id: number): string | null {
    const data = this.provinces();
    const found = data.find((item) => item.id === id);
    return found ? found.descripcion : null;
  }

  getMunicipalitiesById(id: number): string | null {
    const data = this.municipalities();
    const found = data.find((item) => item.id === id);
    return found ? found.descripcion : null;
  }

  getMinorById(id: number): string | null {
    const data = this.minors();
    const found = data.find((item) => item.id === id);
    return found ? found.descripcion : 'Sin identidad menor';
  }

  onChangeMunicipio(event: any) {
    this.polygon.set([]);
  }

  openModalMap() {
    if (!this.formData.value.municipio) {
      return;
    }
    const municipioSelected = this.municipalities().find((item) => item.id == this.formData.value.municipio);
    console.info('municipioSelected', municipioSelected);

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

  isInteger(value: any): boolean {
    return Number.isInteger(value);
  }
}
