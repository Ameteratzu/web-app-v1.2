import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, Renderer2 } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EvolutionService } from '../../services/evolution.service';
import { AlertService } from '../../shared/alert/alert.service';
import { FireDetail } from '../../types/fire-detail.type';
import { AreaComponent } from './area/area.component';
import { ConsequencesComponent } from './consequences/consequences.component';
import { InterventionComponent } from './intervention/intervention.component';
import { RecordsComponent } from './records/records.component';

@Component({
  selector: 'app-fire-create',
  standalone: true,
  templateUrl: './fire-evolution-create.component.html',
  styleUrls: ['./fire-evolution-create.component.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatChipsModule,
    FlexLayoutModule,
    RecordsComponent,
    ConsequencesComponent,
    InterventionComponent,
    AreaComponent,
    NgxSpinnerModule,
  ],
  animations: [
    trigger('fadeInOut', [
      state('void', style({ opacity: 0, transform: 'translateY(20px)' })),
      transition(':enter', [animate('900ms ease-out')]),
      transition(':leave', [animate('0ms ease-in')]),
    ]),
  ],
})
export class FireCreateComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<FireCreateComponent>);
  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };
  data = inject(MAT_DIALOG_DATA) as {
    title: string;
    idIncendio: number;
    fireDetail?: FireDetail;
    valoresDefecto?: number;
    fire?: any;
  };

  public evolutionSevice = inject(EvolutionService);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public router = inject(Router);
  public alertService = inject(AlertService);

  readonly sections = [
    { id: 1, label: 'Registro / Parámetros' },
    { id: 2, label: 'Área afectada' },
    { id: 3, label: 'Consecuencias / Actuac.' },
    { id: 4, label: 'Intervención de medios' },
  ];

  editData: any;
  isDataReady = false;
  idReturn = null;
  isEdit = false;
  estado: number | undefined;

  async isToEditDocumentation() {    
    if (!this.data?.fireDetail?.id) {
      if (this.data?.valoresDefecto) {
        const dataCordinacion: any = await this.evolutionSevice.getById(Number(this.data?.valoresDefecto));
        this.estado = dataCordinacion.parametro?.estadoIncendio.id;
      }
      this.isDataReady = true;
      return;
    }

    const dataCordinacion: any = await this.evolutionSevice.getById(Number(this.data.fireDetail.id));

    this.editData = dataCordinacion;
    this.isDataReady = true;
  }

  async ngOnInit() {
    console.log('🚀 ~ FireCreateComponent ~ ngOnInit ~ this.data.fire:', this.data.fire);
    this.spinner.show();
    this.isToEditDocumentation();
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
          console.info("delete02")
          this.delete();
          break;
        case 'close':
          this.spinner.hide();
          this.evolutionSevice.clearData();
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
    this.spinner.show();
    const toolbar = document.querySelector('mat-toolbar');
    this.renderer.setStyle(toolbar, 'z-index', '1');
    console.log('🚀 ~ FireCreateComponent ~ save ~ this.evolutionSevice.dataRecords():', this.evolutionSevice.dataRecords());
    // if (this.evolutionSevice.dataRecords().length === 0) {
    //   this.alertService.showAlert({
    //     title: 'Falta información',
    //     text: 'Debe ingresar datos en Registro/Párametros',
    //     icon: 'warning',
    //     confirmButtonText: 'OK',
    //   });
    //   this.spinner.hide();
    //   return;
    // }

    await this.processData();

    this.evolutionSevice.clearData();

    setTimeout(() => {
      this.renderer.setStyle(toolbar, 'z-index', '5');
      this.alertService
        .showAlert({
          title: 'Buen trabajo!',
          text: 'Registro subido correctamente!',
          icon: 'success',
        })
        .then(async (result) => {
          //this.isDataReady = false;
          //const dataCordinacion: any = await this.evolutionSevice.getById(Number(this.idReturn));
          //this.editData = dataCordinacion;
          //this.isDataReady = true;
          this.spinner.hide();
          this.closeModal(true)
        });
    }, 2000);
  }

  async processData(): Promise<void> {
    if (this.evolutionSevice.dataRecords().length > 0) {
      this.editData ? (this.idReturn = this.editData.id) : 0;
      this.idReturn ? (this.evolutionSevice.dataRecords()[0].idEvolucion = this.idReturn) : 0;
      const result: any = await this.evolutionSevice.postData(this.evolutionSevice.dataRecords()[0]);
      this.idReturn = result.id;
    }
    if (this.evolutionSevice.dataAffectedArea().length > 0) {
      await this.handleDataProcessing(
        this.evolutionSevice.dataAffectedArea(),
        (item) => ({
          id: item.id ?? 0,
          fechaHora: this.formatDate(item.fechaHora),
          idProvincia: item.provincia.id ?? item.provincia,
          idMunicipio: item.municipio.id ?? item.municipio,
          idEntidadMenor: item.entidadMenor?.id ?? item.entidadMenor ?? null,
          observaciones: item.observaciones,
          GeoPosicion: this.isGeoPosicionValid(item) ? item.geoPosicion : null,
        }),
        this.evolutionSevice.postAreas.bind(this.evolutionSevice),
        'areasAfectadas'
      );
    }

    if (this.evolutionSevice.dataConse().length > 0) {
      /*
      await this.handleDataProcessing(
        this.evolutionSevice.dataConse(),
        (item) => ({
          id: item.id ?? 0,
          fechaHora: this.formatDate(item.fechaHora),
          idProvincia: item.provincia.id ?? item.provincia,
          idMunicipio: item.municipio.id ?? item.municipio,
          idEntidadMenor: item.entidadMenor?.id ?? item.entidadMenor ?? null,
          observaciones: item.observaciones,
          GeoPosicion: this.isGeoPosicionValid(item) ? item.geoPosicion : null,
        }),
        this.evolutionSevice.postAreas.bind(this.evolutionSevice),
        'areasAfectadas'
      );
      */

      const dataSave = {
        idSuceso: this.data.idIncendio,
        idEvolucion: this.data?.fireDetail?.id ? this.data?.fireDetail?.id : this.idReturn,
        Impactos: this.evolutionSevice.dataConse(),
      };
      this.evolutionSevice.postConse(dataSave);
    }
  }

  async handleDataProcessing<T>(data: T[], formatter: (item: T) => any, postService: (body: any) => Promise<any>, key: string): Promise<void> {
    if (data.length > 0) {
      const formattedData = data.map(formatter);

      const body = {
        idSuceso: this.data.idIncendio,
        idEvolucion: this.data?.fireDetail?.id ? this.data?.fireDetail?.id : this.idReturn,
        [key]: formattedData,
      };

      const result = await postService(body);
      console.log('🚀 ~ result:', result);
      this.idReturn = result.idEvolucion;
    }
  }

  isGeoPosicionValid(data: any): boolean {
    const geoPosicion = data?.geoPosicion;

    if (!geoPosicion || geoPosicion.type !== 'Polygon' || !Array.isArray(geoPosicion.coordinates)) {
      return false;
    }

    return geoPosicion.coordinates.every(
      (polygon: any[]) =>
        Array.isArray(polygon) &&
        polygon.length > 0 &&
        polygon.every((point) => Array.isArray(point) && point.length === 2 && point.every((coord) => typeof coord === 'number'))
    );
  }

  formatDate(date: Date | string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  onSelectionChange(event: MatChipListboxChange): void {
    this.spinner.show();
    this.selectedOption = event;
  }

  closeModal(value: boolean) {
    this.dialogRef.close(value);
  }

  async delete() {
    console.info("delete03")
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
            await this.evolutionSevice.deleteConse(Number(this.data?.fireDetail?.id));  
            this.evolutionSevice.clearData();
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
                this.closeModal(true);
              });
          }
          
          
          
        } else {
          console.info("delete0200")
          this.spinner.hide();
        }
      });
  }
}
