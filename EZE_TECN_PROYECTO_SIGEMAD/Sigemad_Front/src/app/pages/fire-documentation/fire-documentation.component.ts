import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
import moment from 'moment';
import { FireDocumentationService } from '../../services/fire-documentation.service';
import { OriginDestinationService } from '../../services/origin-destination.service';
import { TipoDocumentoService } from '../../services/tipo-documento.service';
import { Media } from '../../types/media.type';
import { OriginDestination } from '../../types/origin-destination.type';

const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'LL', // Definir el formato de entrada
  },
  display: {
    dateInput: 'LL', // Definir cómo mostrar la fecha
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

interface FormType {
  id?: string;
  fechaHora: Date;
  procendenciaDestino: any;
  fechaHoraSolicitud: Date;
  tipoDocumento: { id: string; descripcion: string };
  descripcion: string;
  fichero?: any;
}

@Component({
  selector: 'app-fire-create',
  standalone: true,
  templateUrl: './fire-documentation.component.html',
  styleUrls: ['./fire-documentation.component.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatTableModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class FireDocumentation implements OnInit {
  constructor(
    private originDestinationService: OriginDestinationService,
    private tipoDocumento: TipoDocumentoService,
    private dialogRef: MatDialogRef<FireDocumentation>,
    private fireDocumentationService: FireDocumentationService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  file: File | null = null;

  private fb = inject(FormBuilder);
  dataProps = inject(MAT_DIALOG_DATA) as { title: string; fire: any };

  formData!: FormGroup;

  readonly sections = [{ id: 1, label: 'Documentación' }];

  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  public listadoProcedenciaDestino = signal<OriginDestination[]>([]);
  public listadoTipoDocumento = signal<OriginDestination[]>([]);
  public listadoMedios = signal<Media[]>([]);
  public dataOtherInformation = signal<FormType[]>([]);
  public isCreate = signal<number>(-1);

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = [
    'fechaHora',
    'procendenciaDestino',
    'descripcion',
    'fichero',
    'opciones',
  ];

  async ngOnInit() {
    this.formData = this.fb.group({
      fechaHora: ['', Validators.required],
      fechaHoraSolicitud: ['', Validators.required],
      tipoDocumento: ['', Validators.required],
      procendenciaDestino: ['', Validators.required],
      descripcion: ['', Validators.required],
    });

    this.dataSource.data = [];

    const procedenciasDestino = await this.originDestinationService.get();
    this.listadoProcedenciaDestino.set(procedenciasDestino);

    const tipoDocumentos = await this.tipoDocumento.get();
    this.listadoTipoDocumento.set(tipoDocumentos);
  }

  trackByFn(index: number, item: any): string {
    return item;
  }

  onSubmit() {
    console.info('submit');
    if (this.formData.valid) {
      const data = { file: this.file, ...this.formData.value };
      if (this.isCreate() == -1) {
        this.dataOtherInformation.set([data, ...this.dataOtherInformation()]);
      } else {
        this.editarItem(this.isCreate());
      }
      this.formData.reset();
    } else {
      this.formData.markAllAsTouched();
    }
  }

  //Función para guardar en base de datos
  async saveList() {
    if (this.dataOtherInformation().length <= 0) {
      alert('Debe meter data en la tabla');
      return;
    }

    const arrayToSave = this.dataOtherInformation().map((item) => {
      return {
        id: item.id ?? null,
        fechaHora: item.fechaHora,
        fechaHoraSolicitud: item.fechaHoraSolicitud,
        idTipoDocumento: item.tipoDocumento?.id,
        descripcion: item.descripcion,
        idArchivo: null,
        documentacionProcedenciasDestinos: item.procendenciaDestino.map(
          (procendenciaDestino: any) => procendenciaDestino.id
        ),
      };
    });
    const objToSave = {
      idDocumento: null,
      idIncendio: this.dataProps?.fire?.id,
      detallesDocumentaciones: arrayToSave,
    };

    try {
      const resp: { idOtraInformacion: string | number } | any =
        await this.fireDocumentationService.post(objToSave);
      console.info('fireDocumentationServicerest', resp);
      if (resp!.idDocumentacion > 0) {
        alert('Se ha guardado la lista');
        window.location.href = `fire-national-edit/${
          this.dataProps?.fire?.id ?? 1
        }`;
      } else {
        alert('Ha ocurrido un error al guardar la lista');
      }
    } catch (error) {
      console.info({ error });
    }
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);
    this.formData.patchValue(this.dataOtherInformation()[index]);
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOtherInformation.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.dataOtherInformation.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YY');
  }

  closeModal() {
    this.dialogRef.close();
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.file = input.files[0];
    }
  }
  getDescripcionProcedenciaDestion(procedenciaDestino: any[]) {
    return procedenciaDestino.map((obj) => obj.descripcion).join(', ');
  }
}
