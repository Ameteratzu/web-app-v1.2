import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, inject, Renderer2 } from '@angular/core';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FireDetail } from '../../types/fire-detail.type';
import { EmergencyNationalComponent } from "./emergency-national/emergency-national.component";
import { ZagepComponent } from "./zagep/zagep.component";
import { ActionsRelevantService } from '../../services/actions-relevant.service';
import { AlertService } from '../../shared/alert/alert.service';


@Component({
  selector: 'app-fire-actions-relevant',
  standalone: true,
  imports: [NgxSpinnerModule, FlexLayoutModule, MatChipsModule, CommonModule, EmergencyNationalComponent, ZagepComponent],
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

  selectedOption: MatChipListboxChange = { source: null as any, value: 7 };

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

  async ngOnInit() {
    console.log('🚀 ~ FireCreateComponent ~ ngOnInit ~ this.data.fire:', this.data.fire);
    this.spinner.show();
    this.isToEditDocumentation();
  }

  async isToEditDocumentation() {
    if (!this.data?.fireDetail?.id) {
      if (this.data?.valoresDefecto) {
        //const dataCordinacion: any = await this.evolutionSevice.getById(Number(this.data?.valoresDefecto));
       // this.estado = dataCordinacion.parametro?.estadoIncendio.id;
      }
      this.isDataReady = true;
      return;
    }

    //const dataCordinacion: any = await this.evolutionSevice.getById(Number(this.data.fireDetail.id));

    //this.editData = dataCordinacion;
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
          //this.delete();
          break;
        case 'close':
          this.spinner.hide();
          //this.evolutionSevice.clearData();
          this.closeModal(true);
          break;
        case 'update':
          this.isEdit = true;
          //this.save();
          break;
        default:
          console.error('Clave inesperada');
      }
    } else {
      console.log('Ninguna clave tiene valor true');
    }
  }

  async save() {
    this.spinner.show();
    const toolbar = document.querySelector('mat-toolbar');
    this.renderer.setStyle(toolbar, 'z-index', '1');
 
    if (this.actionsRelevantSevice.dataEmergencia().length > 0) {
      await this.processData();
    }

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
      console.log("🚀 ~ FireActionsRelevantComponent ~ processData ~  this.idReturn:",  this.idReturn)
    }
  }

  onSelectionChange(event: MatChipListboxChange): void {
    this.spinner.show();
    this.selectedOption = event;
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal(value?: any) {
    this.dialogRef.close(value);
  }



}
