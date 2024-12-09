import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  Output,
  signal,
  ViewChild,
} from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import {
  FormBuilder,
  FormGroup,
  FormGroupDirective,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
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
import { CoordinationAddressService } from '../../../services/coordination-address.service';
import { DireccionesService } from '../../../services/direcciones.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { CoordinationAddress } from '../../../types/coordination-address';
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
  selector: 'app-cecopi',
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
  templateUrl: './cecopi.component.html',
  styleUrl: './cecopi.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class CecopiComponent {
  @ViewChild(MatSort) sort!: MatSort;
  @Output() save = new EventEmitter<boolean>();
  @Input() editData: any;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };

  public polygon = signal<any>([]);

  public direcionesServices = inject(DireccionesService);
  public coordinationServices = inject(CoordinationAddressService);
  public toast = inject(MatSnackBar);

  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private provinceService = inject(ProvinceService);
  private municipalityService = inject(MunicipalityService);

  public displayedColumns: string[] = [
    'fechaHora',
    'procendenciaDestino',
    'descripcion',
    'fichero',
    'opciones',
  ];

  formDataCecopi!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public isCreate = signal<number>(-1);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    const coordinationAddress =
      await this.direcionesServices.getAllDirecciones();
    this.coordinationAddress.set(coordinationAddress);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    this.formDataCecopi = this.fb.group({
      provincia: ['', Validators.required],
      municipio: ['', Validators.required],
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [''],
      lugar: ['', Validators.required],
      observaciones: [''],
    });

    this.formDataCecopi.get('municipio')?.disable();

    if (this.editData) {
      console.log('Información recibida en el hijo:', this.editData);
      if (this.coordinationServices.dataCecopi().length === 0) {
        this.coordinationServices.dataCecopi.set(this.editData);
        this.polygon.set(this.editData.geoPosicion?.coordinates[0]);
      }
    }
  }

  onSubmitCecopi(formDirective: FormGroupDirective): void {
    if (this.formDataCecopi.valid) {
      const data = this.formDataCecopi.value;
      if (this.isCreate() == -1) {
        data.geoPosicion = {
          type: 'Polygon',
          coordinates: [this.polygon()],
        };
        this.coordinationServices.dataCecopi.set([
          data,
          ...this.coordinationServices.dataCecopi(),
        ]);
      } else {
        this.editarItemCecopi(this.isCreate());
      }

      formDirective.resetForm();
      this.formDataCecopi.reset({
        fechaInicio: new Date(),
        fechaFin: null,
      });
      this.formDataCecopi.get('municipio')?.disable();
    } else {
      this.formDataCecopi.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    this.spinner.show();
    if (this.coordinationServices.dataCecopi().length > 0) {
      this.save.emit(true);
    } else {
      this.spinner.show();
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

  async loadMunicipalities(event: any) {
    const province_id = event?.value?.id ?? event.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formDataCecopi.get('municipio')?.enable();
  }

  openModalMap() {
    if (!this.formDataCecopi.value.municipio) {
      return;
    }
    const municipioSelected = this.municipalities().find(
      (item) => item.id == this.formDataCecopi.value.municipio.id
    );

    if (!municipioSelected) {
      return;
    }
    console.info('this.polygon()', this.polygon());
    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      height: '780px',
      maxHeight: '780px',
      data: {
        municipio: municipioSelected,
        listaMunicipios: this.municipalities(),
        defaultPolygon: this.polygon(),
      },
    });

    dialogRef.componentInstance.save.subscribe(
      (features: Feature<Geometry>[]) => {
        this.polygon.set(features);
      }
    );
  }

  editarItemCecopi(index: number) {
    const dataEditada = this.formDataCecopi.value;
    this.coordinationServices.dataCecopi.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formDataCecopi.reset();
  }

  eliminarItemCecopi(index: number) {
    this.coordinationServices.dataCecopi.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  seleccionarItemCecopi(index: number) {
    this.isCreate.set(index);
    const selectedItem = this.coordinationServices.dataCecopi()[index];

    // Actualizar los valores en el formulario
    this.formDataCecopi.patchValue(selectedItem);

    // Habilitar los campos dependientes si tienen datos
    if (selectedItem.municipio) {
      this.formDataCecopi.get('municipio')?.enable();
    } else {
      this.formDataCecopi.get('municipio')?.disable();
    }
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YY');
  }

  getFormCecopi(atributo: string): any {
    return this.formDataCecopi.controls[atributo];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal() {
    this.save.emit(false);
  }
}
