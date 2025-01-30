import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, inject, Renderer2 } from '@angular/core';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FireDetail } from '../../types/fire-detail.type';
import { EmergencyNationalComponent } from './emergency-national/emergency-national.component';
import { ZagepComponent } from './zagep/zagep.component';
import { CecodComponent } from './cecod/cecod.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { ActivationPlanComponent } from './activation-plan/activation-plan.component';
import { ActionsRelevantService } from '../../services/actions-relevant.service';
import { AlertService } from '../../shared/alert/alert.service';
import { _isNumberValue } from '@angular/cdk/coercion';
import moment from 'moment';

@Component({
  selector: 'app-fire-actions-relevant',
  standalone: true,
  imports: [
    NgxSpinnerModule,
    FlexLayoutModule,
    MatChipsModule,
    CommonModule,
    EmergencyNationalComponent,
    ZagepComponent,
    CecodComponent,
    NotificationsComponent,
    ActivationPlanComponent
  ],
  animations: [
    trigger('fadeInOut', [
      state('void', style({ opacity: 0, transform: 'translateY(20px)' })),
      transition(':enter', [animate('900ms ease-out')]),
      transition(':leave', [animate('0ms ease-in')]),
    ]),
  ],
  templateUrl: './fire-actions-relevant.component.html',
  styleUrl: './fire-actions-relevant.component.scss',
})
export class FireActionsRelevantComponent {
  private dialogRef = inject(MatDialogRef<FireActionsRelevantComponent>);
  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public actionsRelevantSevice = inject(ActionsRelevantService);
  public alertService = inject(AlertService);

  selectedOption: MatChipListboxChange = { source: null as any, value: 2 };

  data = inject(MAT_DIALOG_DATA) as {
    title: string;
    idIncendio: number;
    fireDetail?: FireDetail;
    valoresDefecto?: number;
    fire?: any;
  };

  editData: any;
  isDataReady = false;
  idReturn = null;
  isEdit = false;
  estado: number | undefined;

  readonly sections = [
    { id: 1, label: 'Movilización de medios' },
    { id: 2, label: 'Convocatoria CECOD' },
    { id: 3, label: 'Activación de planes' },
    { id: 4, label: 'Notificaciones oficiales' },
    { id: 5, label: 'Activación de sistemas' },
    { id: 6, label: 'Declaración ZAGEP' },
    { id: 7, label: 'Emergencia nacional' },
  ];

  dataMaestros: any = {};

  async ngOnInit() {
    console.log('🚀 ~ FireCreateComponent ~ ngOnInit ~ this.data.fire:', this.data.fire);
    this.spinner.show();
    await this.loadData();
    this.isToEdit();
  }

  async loadData() {
    const tipoNotificaciones = await this.actionsRelevantSevice.getTipoNotificacion();
    const tipoPlanes = await this.actionsRelevantSevice.getAllPlanes();
    console.log("🚀 ~ FireActionsRelevantComponent ~ loadData ~ tipoPlanes:", tipoPlanes)

    this.dataMaestros = {
      tipoNotificaciones,
      tipoPlanes
    };

    console.log('🚀 ~ loadData ~ this.dataMaestros:', this.dataMaestros);

    return this.dataMaestros;
  }

  async isToEdit() {
    if (this.data.fireDetail?.id) {
      const dataCordinacion: any = await this.actionsRelevantSevice.getById(Number(this.data.fireDetail?.id));
      this.editData = dataCordinacion;
    }
    this.isDataReady = true;
  }

  async onSaveFromChild(value: { save: boolean; delete: boolean; close: boolean; update: boolean }) {
    const keyWithTrue = (Object.keys(value) as Array<keyof typeof value>).find((key) => value[key]);
    this.isEdit = false;

    if (keyWithTrue) {
      switch (keyWithTrue) {
        case 'save':
          this.save();
          break;
        case 'delete':
          this.delete();
          break;
        case 'close':
          this.spinner.hide();
          //this.evolutionSevice.clearData();
          this.closeModal(true);
          break;
        case 'update':
          this.isEdit = true;
          this.save();
          break;
        default:
          console.error('Clave inesperada');
      }
    } else {
      console.log('Ninguna clave tiene valor true');
    }
  }

  async save() {
    console.log('🚀 ~ FireActionsRelevantComponent ~ save ~ save:', 'save');
    this.spinner.show();
    const toolbar = document.querySelector('mat-toolbar');
    this.renderer.setStyle(toolbar, 'z-index', '1');
    await this.processData();

    this.actionsRelevantSevice.clearData();

    setTimeout(() => {
      this.renderer.setStyle(toolbar, 'z-index', '5');
      this.alertService
        .showAlert({
          title: 'Buen trabajo!',
          text: 'Registro subido correctamente!',
          icon: 'success',
        })
        .then(async (result) => {
          this.isDataReady = false;
          const dataActuaciones: any = await this.actionsRelevantSevice.getById(Number(this.idReturn));
          this.editData = dataActuaciones;
          this.isDataReady = true;
          this.spinner.hide();
        });
    }, 2000);
  }

  async processData(): Promise<void> {
    if (this.actionsRelevantSevice.dataEmergencia().length > 0) {
      this.editData ? (this.idReturn = this.editData.id) : 0;
      this.idReturn ? (this.actionsRelevantSevice.dataEmergencia()[0].idActuacionRelevante = this.idReturn) : 0;
      const result: any = await this.actionsRelevantSevice.postData(this.actionsRelevantSevice.dataEmergencia()[0]);
      this.idReturn = result.idActuacionRelevante;
    }

    if (this.actionsRelevantSevice.dataPlanes().length > 0) {
      console.log("🚀 ~ FireActionsRelevantComponent ~ processData ~ this.actionsRelevantSevice.dataPlanes():", this.actionsRelevantSevice.dataPlanes())

      const arrayToSave = this.actionsRelevantSevice.dataPlanes().map((item, index) => {
        return {
          id: item.id ?? null,
          idTipoPlan: _isNumberValue(item.idTipoPlan) ? item.idTipoPlan : item.idTipoPlan.id,
          nombrePlan: item.nombrePlan,
          nombrePlanPersonalizado: item.nombrePlanPersonalizado,
          fechaInicio: this.formatDate(item.fechaInicio),
          fechaFin: this.formatDate(item.fechaFin),
          autoridad: item.autoridad,
          observaciones: item.observaciones,
          archivo: item.file,
        };
      });
  
      const objToSave = {
        detallesDocumentaciones: arrayToSave,
      };
  
      const formData = new FormData();
      formData.append('IdActuacionRelevante', this.actionsRelevantSevice.dataPlanes()[0].idActuacionRelevante ?? 0);
      formData.append('idSuceso', this.data.idIncendio.toString());

      objToSave.detallesDocumentaciones.forEach((detalle, index) => {
        formData.append(`detalles[${index}].Id`, (detalle.id ?? '0').toString());
        formData.append(`detalles[${index}].IdTipoPlan`, (detalle.idTipoPlan).toString());
        formData.append(`detalles[${index}].FechaInicio`, detalle.fechaInicio);
        formData.append(`detalles[${index}].FechaFin`, detalle.fechaFin);
        formData.append(`detalles[${index}].Autoridad`, detalle.autoridad ?? '');
        formData.append(`detalles[${index}].TipoPlanPersonalizado`, detalle.nombrePlanPersonalizado ?? '');
        formData.append(`detalles[${index}].IdPlanEmergencia`, '3');
        formData.append(`detalles[${index}].PlanEmergenciaPersonalizado`, detalle.nombrePlan ?? '');
        formData.append(`detalles[${index}].Observaciones`, detalle.observaciones ?? '');
        formData.append(`detalles[${index}].Archivo`, detalle.archivo);
      });

      const resp: { idActuacionRelevante: string | number } | any = await this.actionsRelevantSevice.postPlanes(formData);
      console.log("🚀 ~ FireActionsRelevantComponent ~ processData ~ resp:", resp)
      
      this.idReturn = resp.idActuacionRelevante;
    }

    if (this.actionsRelevantSevice.dataCecod().length > 0) {
      await this.handleDataProcessing(
        this.actionsRelevantSevice.dataCecod(),
        (item) => ({
          id: item.id ?? 0,
          fechaInicio: this.formatDate(item.fechaInicio),
          fechaFin: item.fechaFin ? this.formatDate(item.fechaFin) : null,
          lugar: item.lugar,
          convocados: item.convocados,
          participantes: item.participantes,
          observaciones: item.observaciones,
        }),
        this.actionsRelevantSevice.postDataCecod.bind(this.actionsRelevantSevice),
        'detalles'
      );
    }

    if (this.actionsRelevantSevice.dataNotificaciones().length > 0) {
    
      await this.handleDataProcessing(
        this.actionsRelevantSevice.dataNotificaciones(),
        (item) => ({
          id: item.id ?? 0,
          idTipoNotificacion: _isNumberValue(item.idTipoNotificacion) ? item.idTipoNotificacion : item.idTipoNotificacion.id,
          fechaHoraNotificacion: this.formatDate(item.fechaHoraNotificacion),
          organosNotificados: item.organosNotificados,
          ucpm: item.ucpm,
          organismoInternacional: item.organismoInternacional,
          otrosPaises: item.otrosPaises,
          observaciones: item.observaciones,
        }),
        this.actionsRelevantSevice.postDataNotificaciones.bind(this.actionsRelevantSevice),
        'detalles'
      );
    }

    if (this.actionsRelevantSevice.dataZagep().length > 0) {
      await this.handleDataProcessing(
        this.actionsRelevantSevice.dataZagep(),
        (item) => ({
          id: item.id ?? 0,
          fechaSolicitud: this.formatDate(item.fechaSolicitud),
          denominacion: item.denominacion,
          observaciones: item.observaciones,
        }),
        this.actionsRelevantSevice.postDataZagep.bind(this.actionsRelevantSevice),
        'detalles'
      );
    }
  }

  async handleDataProcessing<T>(data: T[], formatter: (item: T) => any, postService: (body: any) => Promise<any>, key: string): Promise<void> {
    if (data.length > 0) {
      const formattedData = data.map(formatter);

      const body = {
        idSuceso: this.data.idIncendio,
        idActuacionRelevante: this.data?.fireDetail?.id ? this.data?.fireDetail?.id : this.idReturn,
        [key]: formattedData,
      };

      const result = await postService(body);
      console.log('🚀 ~ result:', result);
      this.idReturn = result.idActuacionRelevante;
    }
  }

  formatDate(date: Date | string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
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

  async delete() {
    const toolbar = document.querySelector('mat-toolbar');
    this.renderer.setStyle(toolbar, 'z-index', '1');
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
          try {
            await this.actionsRelevantSevice.deleteActions(Number(this.data?.fireDetail?.id));
            this.actionsRelevantSevice.clearData();
            setTimeout(() => {
              this.renderer.setStyle(toolbar, 'z-index', '5');
              this.spinner.hide();
            }, 2000);

            this.alertService
              .showAlert({
                title: 'Eliminado!',
                icon: 'success',
              })
              .then((result) => {
                this.closeModal(true);
              });
          } catch (error) {
            this.alertService
              .showAlert({
                title: 'No hemos podido eliminar la evolución',
                icon: 'error',
              })
              .then((result) => {
                this.closeModal();
              });
          }
        } else {
          this.spinner.hide();
        }
      });
  }

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal(value?: any) {
    this.dialogRef.close(value);
  }
}
