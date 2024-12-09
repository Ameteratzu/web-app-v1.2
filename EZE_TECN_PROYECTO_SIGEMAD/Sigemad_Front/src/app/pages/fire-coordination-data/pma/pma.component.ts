import { Component, EventEmitter, inject, Input, OnInit, Output, signal, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { DireccionesService } from '../../../services/direcciones.service';
import { CoordinationAddress } from '../../../types/coordination-address';
import { MatSelectModule } from '@angular/material/select';
import moment from 'moment';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { CoordinationAddressService } from '../../../services/coordination-address.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ProvinceService } from '../../../services/province.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { Province } from '../../../types/province.type';
import { Municipality } from '../../../types/municipality.type';
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { Geometry } from 'ol/geom';
import Feature from 'ol/Feature';
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
  selector: 'app-pma',
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
  templateUrl: './pma.component.html',
  styleUrl: './pma.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class PmaComponent {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;

  public direcionesServices = inject(DireccionesService);
  public coordinationServices = inject(CoordinationAddressService);
  public toast = inject(MatSnackBar);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private provinceService = inject(ProvinceService);
  private municipalityService = inject(MunicipalityService);

  public displayedColumns: string[] = ['fechaHora', 'procendenciaDestino', 'descripcion', 'fichero', 'opciones'];

  formData!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public isCreate = signal<number>(-1);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    const coordinationAddress = await this.direcionesServices.getAllDirecciones();
    this.coordinationAddress.set(coordinationAddress);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    this.formData = this.fb.group({
      provincia: ['', Validators.required],
      municipio: ['', Validators.required],
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [''],
      lugar: ['', Validators.required],
      observaciones: [''],
    });

    this.formData.get('municipio')?.disable();

    if (this.editData) {
      console.log('Información recibida en el hijo:', this.editData);
      if (this.coordinationServices.dataPma().length === 0) {
        this.coordinationServices.dataPma.set(this.editData);
      }
    }
    this.spinner.hide();
  }

  onSubmit() {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        this.coordinationServices.dataPma.set([data, ...this.coordinationServices.dataPma()]);
      } else {
        this.editarItem(this.isCreate());
      }

      this.formData.reset();
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    if (this.coordinationServices.dataPma().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  formatDate(date: Date | string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  async loadMunicipalities(event: any) {
    const province_id = event.value.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formData.get('municipio')?.enable();
  }

  openModalMap() {
    if (!this.formData.value.municipio) {
      return;
    }
    const municipioSelected = this.municipalities().find((item) => item.id == this.formData.value.municipio.id);

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

    dialogRef.componentInstance.save.subscribe((features: Feature<Geometry>[]) => {
      //this.featuresCoords = features;
      console.info('features', features);
    });
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.coordinationServices.dataPma.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.coordinationServices.dataPma.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);
    this.formData.patchValue(this.coordinationServices.dataPma()[index]);
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
    this.save.emit({ save: true, delete: false, close: false, update: false });
  }
}
