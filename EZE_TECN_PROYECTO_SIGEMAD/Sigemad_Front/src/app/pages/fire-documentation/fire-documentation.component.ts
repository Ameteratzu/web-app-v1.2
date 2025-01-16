import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, inject, signal } from '@angular/core';
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
import { FireDocumentationService } from '../../services/fire-documentation.service';
import { MasterDataEvolutionsService } from '../../services/master-data-evolutions.service';
import { TipoDocumentoService } from '../../services/tipo-documento.service';
import { AlertService } from '../../shared/alert/alert.service';
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
  procendenciaDestino: any;
  fechaSolicitud: Date;
  horaSolicitud: any;
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
    NgxSpinnerModule,
    NgxFileDropModule,
    MatIconModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class FireDocumentation implements OnInit {
  constructor(
    private masterData: MasterDataEvolutionsService,
    private tipoDocumento: TipoDocumentoService,
    private dialogRef: MatDialogRef<FireDocumentation>,
    private fireDocumentationService: FireDocumentationService,
    private spinner: NgxSpinnerService,
    public alertService: AlertService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

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
  public dataOtherInformation = signal<FormType[]>([]);
  public isCreate = signal<number>(-1);
  public isSaving = signal<boolean>(false);

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = ['fechaHora', 'procendenciaDestino', 'descripcion', 'fichero', 'opciones'];

  public toast = inject(MatSnackBar);

  async ngOnInit() {
    console.info('moment().toISOString()', moment().toISOString(), moment().format('HH:mm'));
    this.formData = this.fb.group({
      fecha: [moment().toISOString(), Validators.required],
      hora: [moment().format('HH:mm'), Validators.required],
      fechaSolicitud: [''],
      horaSolicitud: [''],
      tipoDocumento: ['', Validators.required],
      procendenciaDestino: [''],
      descripcion: [''],
    });

    this.dataSource.data = [];

    const procedenciasDestino = await this.masterData.getOriginDestination();
    this.listadoProcedenciaDestino.set(procedenciasDestino);

    const tipoDocumentos = await this.tipoDocumento.get();
    this.listadoTipoDocumento.set(tipoDocumentos);

    this.isToEditDocumentation();
  }

  async isToEditDocumentation() {
    if (!this.dataProps.fireDetail.id) {
      return;
    }
    const dataDocumentacion: any = await this.fireDocumentationService.getById(Number(this.dataProps.fireDetail.id));

    const newData = dataDocumentacion?.detalles?.map((documento: any) => ({
      id: documento.id,
      descripcion: documento.descripcion,
      fecha: moment(documento.fechaHora).format('YYYY-MM-DD'),
      hora: moment(documento.fechaHora).format('HH:mm'),
      fechaSolicitud: moment(documento.fechaHoraSolicitud).format('YYYY-MM-DD'),
      horaSolicitud: moment(documento.fechaHoraSolicitud).format('HH:mm'),
      procendenciaDestino: documento.procedenciaDestinos,
      tipoDocumento: documento.tipoDocumento,
      archivo: documento.archivo,
    }));

    this.dataOtherInformation.set(newData);
  }

  trackByFn(index: number, item: any): string {
    return item;
  }

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid && this.fileFlag) {
      const data = { file: this.file, ...this.formData.value };

      if (this.isCreate() == -1) {
        this.dataOtherInformation.set([data, ...this.dataOtherInformation()]);
      } else {
        this.editarItem(this.isCreate());
      }

      formDirective.resetForm();
      this.formData.reset({
        fecha: moment().format('YYYY-MM-DD'),
        hora: moment().format('HH:mm'),
        procendenciaDestino: [],
        tipoDocumento: null,
      });
      //this.file = null;
    } else {
      this.fileFlag = false;
      this.formData.markAllAsTouched();
    }
  }

  //Función para guardar en base de datos
  async saveList() {
    if (this.isSaving()) {
      return;
    }
    this.isSaving.set(true);
    if (this.dataOtherInformation().length <= 0) {
      this.showToast({ title: 'Debe meter data en la lista' });
      this.isSaving.set(false);
      return;
    }

    const arrayToSave = this.dataOtherInformation().map((item, index) => {
      return {
        id: item.id ?? null,
        fechaHora: this.getFechaHora(item.fecha, item.hora),
        fechaHoraSolicitud: this.getFechaHora(item.fechaSolicitud, item.horaSolicitud),
        idTipoDocumento: item.tipoDocumento?.id,
        descripcion: item.descripcion,
        archivo: index === 0 ? this.file : null, // Solo agrega el archivo en el índice 0
        documentacionProcedenciasDestinos: item.procendenciaDestino.map((procendenciaDestino: any) => procendenciaDestino.id),
      };
    });

    const objToSave = {
      idDocumento: null,
      idSuceso: this.dataProps?.fire?.id,
      detallesDocumentaciones: arrayToSave,
    };

    const formData = new FormData();
    formData.append('idDocumento', objToSave.idDocumento ?? '0');
    formData.append('idSuceso', objToSave.idSuceso ?? '');

    // Construir `detalles` incluyendo el archivo en `detalles[0].archivo`
    objToSave.detallesDocumentaciones.forEach((detalle, index) => {
      formData.append(`detalles[${index}].fechaHora`, this.getFechaHoraIso(detalle.fechaHora));
      formData.append(`detalles[${index}].fechaHoraSolicitud`, this.getFechaHoraIso(detalle.fechaHora));
      formData.append(`detalles[${index}].idTipoDocumento`, detalle.idTipoDocumento ?? '');
      formData.append(`detalles[${index}].descripcion`, detalle.descripcion ?? '');

      detalle.documentacionProcedenciasDestinos.forEach((id: string | Blob, subIndex: any) => {
        formData.append(`detalles[${index}].idsProcedenciasDestinos[${subIndex}]`, id);
      });

      if (detalle.archivo) {
        formData.append(`detalles[${index}].archivo`, detalle.archivo);
      }
    });

    try {
      this.spinner.show();
      // const resp: { idOtraInformacion: string | number } | any = await this.fireDocumentationService.post(objToSave);
      const resp: { idOtraInformacion: string | number } | any = await this.fireDocumentationService.post(formData);

      if (resp!.idDocumentacion > 0) {
        this.isSaving.set(false);
        this.spinner.hide();

        this.alertService
          .showAlert({
            title: 'Buen trabajo!',
            text: 'Registro subido correctamente!',
            icon: 'success',
          })
          .then((result) => {
            this.closeModal({ refresh: true });
          });
      } else {
        this.showToast({ title: 'Ha ocurrido un error al guardar la lista' });
        this.spinner.hide();
      }
    } catch (error) {
      console.info({ error });
      this.spinner.hide();
    }
  }

  async delete() {
    //const toolbar = document.querySelector('mat-toolbar');
    //this.renderer.setStyle(toolbar, 'z-index', '1');
    this.spinner.show();

    this.alertService
      .showAlert({
        title: '¿Estás seguro?',
        text: '¡No podrás revertir esto!',
        icon: 'warning',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
      })
      .then(async (result) => {
        if (result.isConfirmed) {
          await this.fireDocumentationService.delete(Number(this.dataProps?.fireDetail?.id));
          //this.coordinationServices.clearData();
          //setTimeout(() => {
          //this.renderer.setStyle(toolbar, 'z-index', '5');
          this.spinner.hide();
          //}, 2000);

          this.alertService
            .showAlert({
              title: 'Eliminado!',
              icon: 'success',
            })
            .then((result) => {
              this.closeModal({ refresh: true });
            });
        } else {
          this.spinner.hide();
        }
      });
  }

  seleccionarItem(index: number) {
    this.isCreate.set(index);

    const documentoSelected = () =>
      this.listadoTipoDocumento().find((documento) => documento.id === Number(this.dataOtherInformation()[index].tipoDocumento.id));

    const procedenciasSelecteds = () => {
      const idsABuscar = this.dataOtherInformation()[index].procendenciaDestino.map((obj: any) => Number(obj.id));
      return this.listadoProcedenciaDestino().filter((procedencia) => {
        return idsABuscar.includes(Number(procedencia.id));
      });
    };

    this.formData.patchValue({
      ...this.dataOtherInformation()[index],
      tipoDocumento: documentoSelected(),
      procendenciaDestino: procedenciasSelecteds(),
    });
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOtherInformation.update((data) => {
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
    this.dataOtherInformation.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  closeModal(params?: any) {
    this.dialogRef.close(params);
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.file = input.files[0];
    }
  }

  public dropped(files: NgxFileDropEntry[]) {
    for (const droppedFile of files) {
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: any) => {
          this.file = file;
          this.fileFlag = true;
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
    return procedenciaDestino.map((obj) => obj.descripcion).join(', ');
  }

  getFechaHora(fecha: Date, hora: string): any {
    if (hora && fecha) {
      const [horas, minutos] = hora.split(':').map(Number);
      const fechaHora = new Date(fecha);
      fechaHora.setHours(horas, minutos, 0, 0);

      return moment(fechaHora).format('MM/DD/YYYY HH:mm');
    }

    //return fechaHora.toISOString();
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

      // Crear una URL para el Blob
      const url = window.URL.createObjectURL(blob);

      // Crear un enlace temporal para la descarga
      const a = document.createElement('a');
      a.href = url;
      a.download = data.nombreOriginal; // Nombre del archivo original
      document.body.appendChild(a);
      a.click();

      // Limpia el objeto URL después de la descarga
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    } catch (error) {
      console.error('Error al descargar el archivo:', error);
    }
  }
}
