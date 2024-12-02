import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
import moment from 'moment';
import { FireOtherInformationService } from '../../services/fire-other-information.service';
import { MediaService } from '../../services/media.service';
import { OriginDestinationService } from '../../services/origin-destination.service';
import { FireDetail } from '../../types/fire-detail.type';
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
  fecha: Date;
  hora: any;
  procendenciaDestino: { id: string; descripcion: string }[];
  medio: { id: string; descripcion: string };
  asunto: string;
  observaciones: string;
}

@Component({
  selector: 'app-fire-create',
  standalone: true,
  templateUrl: './fire-other-information.component.html',
  styleUrls: ['./fire-other-information.component.scss'],
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
    MatIconModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class FireOtherInformationComponent implements OnInit {
  constructor(
    private originDestinationService: OriginDestinationService,
    private mediaService: MediaService,
    private dialogRef: MatDialogRef<FireOtherInformationComponent>,
    private otherInformationService: FireOtherInformationService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  private fb = inject(FormBuilder);
  dataProps = inject(MAT_DIALOG_DATA) as {
    title: string;
    fire: any;
    fireDetail: FireDetail;
  };

  formData!: FormGroup;

  readonly sections = [{ id: 1, label: 'Otra información' }];
  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  ////////////////////////////////
  public listadoProcedenciaDestino = signal<OriginDestination[]>([]);
  public listadoMedios = signal<Media[]>([]);
  public dataOtherInformation = signal<FormType[]>([]);
  public isCreate = signal<number>(-1);

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = [
    'fecha',
    'procendenciaDestino',
    'medio',
    'asunto',
    'opciones',
  ];

  async ngOnInit() {
    this.formData = this.fb.group({
      fecha: ['', Validators.required],
      hora: ['', Validators.required],
      procendenciaDestino: ['', Validators.required],
      medio: ['', Validators.required],
      asunto: ['', Validators.required],
      observaciones: ['', Validators.required],
    });

    const procedenciasDestino = await this.originDestinationService.get();
    this.listadoProcedenciaDestino.set(procedenciasDestino);

    const medios = await this.mediaService.get();
    this.listadoMedios.set(medios);

    this.isToEditDocumentation();
  }

  async isToEditDocumentation() {
    if (!this.dataProps?.fireDetail?.id) {
      return;
    }
    const dataOtraInformacion: any = await this.otherInformationService.getById(
      Number(this.dataProps.fireDetail.id)
    );

    const newData = dataOtraInformacion?.lista?.map((otraInformacion: any) => ({
      id: otraInformacion.id,
      asunto: otraInformacion.asunto,
      fecha: moment(otraInformacion.fechaHora).format('YYYY-MM-DD'),
      hora: moment(otraInformacion.fechaHora).format('HH:mm'),
      medio: otraInformacion.medio,
      observaciones: otraInformacion.observaciones,
      procendenciaDestino: otraInformacion.procedenciaDestinos,
    }));

    this.dataOtherInformation.set(newData);
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  onSubmit() {
    console.info('submit');
    if (this.formData.valid) {
      const data = this.formData.value;
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
        fechaHora: this.getFechaHora(item.fecha, item.hora),
        idMedio: item.medio?.id ?? null,
        asunto: item.asunto,
        observaciones: item.observaciones,
        idsProcedenciasDestinos: item.procendenciaDestino.map(
          (procendenciaDestino) => procendenciaDestino.id
        ),
      };
    });
    const objToSave = {
      idOtraInformacion: null,
      idIncendio: this.dataProps?.fire?.id,
      lista: arrayToSave,
    };

    try {
      const resp: { idOtraInformacion: string | number } | any =
        await this.otherInformationService.post(objToSave);
      if (resp!.idOtraInformacion > 0) {
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

    const medioSelected = () =>
      this.listadoMedios().find(
        (medio) =>
          medio.id === Number(this.dataOtherInformation()[index].medio.id)
      );

    /*
    const procedenciasSelecteds = () => {
      const idsABuscar = this.dataOtherInformation()[
        index
      ].procendenciaDestino.map((obj: any) => Number(obj.id));
      return this.listadoProcedenciaDestino().filter((procedencia) => {
        return idsABuscar.includes(Number(procedencia.id));
      });
    };
      */

    this.formData.patchValue({
      ...this.dataOtherInformation()[index],
      medio: medioSelected(),
      procendenciaDestino: [],
    });

    //this.formData.patchValue(this.dataOtherInformation()[index]);
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

  getDescripcionProcedenciaDestion(procedenciaDestino: any[]) {
    return procedenciaDestino?.map((obj) => obj?.descripcion).join(', ');
  }

  getFechaHora(fecha: Date, hora: string): string {
    const [horas, minutos] = hora.split(':').map(Number);
    const fechaHora = new Date(fecha);
    fechaHora.setHours(horas, minutos, 0, 0);

    return moment(fechaHora).format('MM/DD/YY HH:mm');
    //return fechaHora.toISOString();
  }
}
