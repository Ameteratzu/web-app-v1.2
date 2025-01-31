import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

import { MatSnackBar } from '@angular/material/snack-bar';

import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import moment from 'moment';
import { FileSystemDirectoryEntry, FileSystemFileEntry, NgxFileDropEntry, NgxFileDropModule } from 'ngx-file-drop';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ActionsRelevantService } from '../../../services/actions-relevant.service';
import { MasterDataEvolutionsService } from '../../../services/master-data-evolutions.service';
import { TipoDocumentoService } from '../../../services/tipo-documento.service';
import { AlertService } from '../../../shared/alert/alert.service';
import { FireDetail } from '../../../types/fire-detail.type';
import { Media } from '../../../types/media.type';
import { OriginDestination } from '../../../types/origin-destination.type';
import { FireDocumentationService } from '../../../services/fire-documentation.service';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { GenericMaster } from '../../../types/actions-relevant.type';
import { FlexLayoutModule } from '@angular/flex-layout';
import { _isNumberValue } from '@angular/cdk/coercion';

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

interface FormType {
  id?: string;
  idDocumento?: string;
  fecha: Date;
  hora: any;
  procendenciaDestino: any;
  fechaSolicitud: Date;
  horaSolicitud: any;
  tipoDocumento: { id: string; descripcion: string };
  descripcion: string;
  file?: any;
}

@Component({
  selector: 'app-activation-plan',
  standalone: true,
  templateUrl: './activation-plan.component.html',
  styleUrl: './activation-plan.component.scss',
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
    NgxSpinnerModule,
    NgxFileDropModule,
    MatIconModule,
    FlexLayoutModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class ActivationPlanComponent implements OnInit {
  constructor(
    private dialogRef: MatDialogRef<ActivationPlanComponent>,
    private spinner: NgxSpinnerService,
    public alertService: AlertService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() fire: any;
  @Input() dataMaestros: any;

  file: File | null = null;
  public files: NgxFileDropEntry[] = [];
  fileFlag: boolean = false;

  private fb = inject(FormBuilder);
  dataProps = inject(MAT_DIALOG_DATA) as {
    title: string;
    fire: any;
    fireDetail: FireDetail;
  };

  formData!: FormGroup;

  readonly sections = [{ id: 1, label: 'Documentación' }];

  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  public listadoProcedenciaDestino = signal<OriginDestination[]>([]);
  public listadoTipoDocumento = signal<OriginDestination[]>([]);
  public listadoMedios = signal<Media[]>([]);
  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);
  public tiposPlanes = signal<GenericMaster[]>([]);
  public planes = signal<GenericMaster[]>([]);
  mostrarCamposAdicionales = signal<boolean>(true);

  public dataSource = new MatTableDataSource<any>([]);
  public displayedColumns: string[] = ['idTipoPlan', 'nombrePlan', 'fechaInicio', 'fechaFin', 'fichero', 'opciones'];

  public toast = inject(MatSnackBar);
  private fireDocumentationService = inject(FireDocumentationService);
  public planesService = inject(ActionsRelevantService);
  public masterData = inject(MasterDataEvolutionsService);

  async ngOnInit() {
    this.tiposPlanes.set(this.dataMaestros.tipoPlanes);
    this.formData = this.fb.group({
      idTipoPlan: [null, Validators.required],
      nombrePlan: [''],
      nombrePlanPersonalizado: [''],
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [null],
      autoridad: ['', Validators.required],
      observaciones: [''],
      file: [null, Validators.required],
    });

    this.dataSource.data = [];

    // this.isToEditDocumentation();
  }

  // async isToEditDocumentation() {
  //   if (!this.dataProps?.fireDetail?.id) {
  //     this.spinner.hide();
  //     return;
  //   }
  //   const dataDocumentacion: any = await this.fireDocumentationService.getById(Number(this.dataProps.fireDetail.id));

  //   const newData = dataDocumentacion?.detalles?.map((documento: any) => {
  //     const fecha = moment(documento.fechaHora, 'YYYY-MM-DDTHH:mm:ss').toDate();
  //     const hora = moment(documento.fechaHora).format('HH:mm');
  //     documento.archivo.name = documento.archivo.nombreOriginal;
  //     return {
  //       id: documento.id,
  //       descripcion: documento.descripcion,
  //       idSuceso: dataDocumentacion.idSuceso,
  //       idDocumento: dataDocumentacion.id,
  //       fecha,
  //       hora,
  //       fechaSolicitud: moment(documento.fechaHoraSolicitud).format('YYYY-MM-DD'),
  //       horaSolicitud: moment(documento.fechaHoraSolicitud).format('HH:mm'),
  //       procendenciaDestino: documento.procedenciaDestinos,
  //       tipoDocumento: documento.tipoDocumento,
  //       archivo: documento.archivo,
  //       file: documento.archivo,
  //     };
  //   });

  //   console.log('🚀 ~ FireDocumentation ~ newData ~ newData:', newData);
  //   this.dataOtherInformation.set(newData);
  //   this.spinner.hide();
  // }

  trackByFn(index: number, item: any): string {
    return item;
  }

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const formValue = this.formData.value;

      const data = {
        ...formValue,
        file: formValue.file,
      };

      if (this.isCreate() == -1) {
        this.planesService.dataPlanes.set([data, ...this.planesService.dataPlanes()]);
      } else {
        this.editarItem(this.isCreate());
      }

      formDirective.resetForm();
      this.formData.reset({
        fecha: moment().toDate(),
        hora: moment().format('HH:mm'),
        procendenciaDestino: [],
        tipoDocumento: null,
        file: null,
      });
      this.fileFlag = false;
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    if (this.planesService.dataPlanes().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  async loadTipo(event: any) {
    this.spinner.show();
    const tipo_id = event?.value?.id ?? event.id;
    this.mostrarCamposAdicionales.set(tipo_id !== 6);
  
    if (tipo_id !== 6) {
      const planes = await this.masterData.getTypesPlansByPlan(this.fire.provincia.idCcaa, tipo_id);
      this.formData.patchValue({
        nombrePlan: planes[0]?.descripcion ?? '',
        nombrePlanPersonalizado: ''
      });
    } else {
      this.formData.patchValue({ nombrePlan: '' });
    }
    this.spinner.hide();
  }

   getTipo(value: any) {

      var tipo: any;
      if (_isNumberValue(value)) {
        tipo = this.tiposPlanes().find((tipo) => tipo.id === value) || null;
      } else {
        tipo = this.tiposPlanes().find((tipo) => tipo.id === value.id) || null;
      }
      return tipo.descripcion;
    }

  async delete() {
    this.spinner.show();
  }

  seleccionarItem(index: number) {

    this.isCreate.set(index);
    this.formData.patchValue({
      ...this.planesService.dataPlanes()[index],
    });
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.planesService.dataPlanes.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset({
      procendenciaDestino: [],
      tipoDocumento: null,
    });
    this.file = null;
  }

  eliminarItem(index: number) {
    this.planesService.dataPlanes.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  closeModal(params?: any) {
    this.dialogRef.close(params);
  }

  public dropped(files: NgxFileDropEntry[]) {
    for (const droppedFile of files) {
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: File) => {
          this.file = file;
          this.fileFlag = true;

          this.formData.patchValue({ file });
        });
      } else {
        const fileEntry = droppedFile.fileEntry as FileSystemDirectoryEntry;
        console.log(droppedFile.relativePath, fileEntry);
      }
    }
  }

  public fileOver(event: any) {
    console.log(event);
  }

  public fileLeave(event: any) {
    console.log(event);
  }

  getDescripcionProcedenciaDestion(procedenciaDestino: any[]) {
    if (procedenciaDestino.length === 0) {
      return 'Sin información selecionada';
    } else {
      return procedenciaDestino.map((obj) => obj.descripcion).join(', ');
    }
  }

  getFechaHora(fecha: Date, hora: string, format: string = 'MM/DD/YY HH:mm'): any {
    if (hora && fecha) {
      const [horas, minutos] = hora.split(':').map(Number);
      const fechaHora = new Date(fecha);
      fechaHora.setHours(horas, minutos, 0, 0);

      return moment(fechaHora).format(format);
    }
  }

  getFechaHoraIso(fechaHora: string): any {
    if (fechaHora) {
      const [fecha, hora] = fechaHora.split(' ');
      const [mes, dia, anio] = fecha.split('/');
      const anioCompleto = `20${anio}`;
      const dateTime = new Date(`${anioCompleto}-${mes}-${dia}T${hora}:00.000Z`);

      return dateTime.toISOString();
    }
  }

  showToast({ title, txt = 'Cerrar' }: { title: string; txt?: string }) {
    this.toast.open(title, txt, {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }

  trackByIdDocumento(index: number, opcion: any): number {
    return opcion.id;
  }

  async onFileNameClick(data: any) {
    try {
      const blob = await this.fireDocumentationService.getFile(data.id);
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = data.nombreOriginal;
      document.body.appendChild(a);
      a.click();

      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    } catch (error) {
      console.error('Error al descargar el archivo:', error);
    }
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  getFormatdate(date: any) {
      if (date) {
        return moment(date).format('DD/MM/YYYY');
      } else {
        return 'Sin fecha selecionada.';
      }
    }
}
