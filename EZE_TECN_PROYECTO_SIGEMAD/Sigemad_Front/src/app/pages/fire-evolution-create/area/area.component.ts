import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  inject,
  Output,
  signal,
  ViewChild,
} from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import {
  FormBuilder,
  FormGroup,
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
import { EvolutionService } from '../../../services/evolution.service';
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
  @Output() save = new EventEmitter<boolean>();
  public evolutionService = inject(EvolutionService);
  public toast = inject(MatSnackBar);
  private provinceService = inject(ProvinceService);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private municipalityService = inject(MunicipalityService);
  private minorService = inject(MinorEntityService);

  public polygon = signal<any>([]);

  public displayedColumns: string[] = [
    'fechaHora',
    'procendenciaDestino',
    'descripcion',
    'fichero',
    'opciones',
  ];

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

    // const minor = await this.minorService.get();
    // this.minors.set(minor);

    this.formData = this.fb.group({
      fechaHora: [new Date(), Validators.required],
      idProvincia: [null, Validators.required],
      idMunicipio: [null, Validators.required],
      idEntidadMenor: [null],
      observaciones: [''],
      fichero: ['', Validators.required],
    });
    this.formData.get('idMunicipio')?.disable();
    this.formData.get('fichero')?.disable();
    this.formData.get('idEntidadMenor')?.disable();

    /*
    if (this.editData) {
      if (this.coordinationServices.dataPma().length === 0) {
        this.coordinationServices.dataPma.set(this.editData);
        this.polygon.set(this.editData.geoPosicion?.coordinates[0]);
      }
    }
    */
  }

  async loadMunicipalities(event: any) {
    const province_id = event?.value?.id ?? event.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formData.get('idMunicipio')?.enable();
  }

  onSubmit() {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        data.geoPosicion = {
          type: 'Polygon',
          coordinates: [this.polygon()],
        };
        this.evolutionService.dataAffectedArea.set([
          data,
          ...this.evolutionService.dataAffectedArea(),
        ]);
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

  async seleccionarItem(index: number) {
    this.isCreate.set(index);

    const provinciaSeleccionada = () =>
      this.provinces().find(
        (provincia) =>
          provincia.id ===
          Number(this.evolutionService.dataAffectedArea()[index].idProvincia.id)
      );

    await this.loadMunicipalities(provinciaSeleccionada());

    const municipioSeleccionado = () =>
      this.municipalities().find(
        (municipio) =>
          municipio.id ===
          Number(this.evolutionService.dataAffectedArea()[index].idMunicipio.id)
      );

    this.formData.patchValue({
      ...this.evolutionService.dataAffectedArea()[index],
      idProvincia: provinciaSeleccionada(),
      idMunicipio: municipioSeleccionado(),
    });
    this.polygon.set(
      this.evolutionService.dataAffectedArea()[index]?.geoPosicion
        ?.coordinates[0]
    );
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

  openModalMap() {
    if (!this.formData.value.idMunicipio) {
      return;
    }
    const municipioSelected = this.municipalities().find(
      (item) => item.id == this.formData.value.idMunicipio.id
    );

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

    dialogRef.componentInstance.save.subscribe(
      (features: Feature<Geometry>[]) => {
        this.polygon.set(features);
      }
    );
  }
}
