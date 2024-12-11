import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, inject, Renderer2, ViewChild } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CoordinationAddressService } from '../../services/coordination-address.service';
import { AlertService } from '../../shared/alert/alert.service';
import { FireDetail } from '../../types/fire-detail.type';
import { AddressComponent } from './address/address.component';
import { CecopiComponent } from './cecopi/cecopi.component';
import { PmaComponent } from './pma/pma.component';

@Component({
  selector: 'app-fire-coordination-data',
  standalone: true,
  imports: [
    MatChipsModule,
    CommonModule,
    FlexLayoutModule,
    MatGridListModule,
    MatIconModule,
    NgxSpinnerModule,
    AddressComponent,
    CecopiComponent,
    PmaComponent,
    MatButtonModule,
  ],
  templateUrl: './fire-coordination-data.component.html',
  styleUrl: './fire-coordination-data.component.scss',
  animations: [
    trigger('fadeInOut', [
      state('void', style({ opacity: 0, transform: 'translateY(20px)' })),
      transition(':enter', [animate('900ms ease-out')]),
      transition(':leave', [animate('0ms ease-in')]),
    ]),
  ],
})
export class FireCoordinationData {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as {
    title: string;
    idIncendio: number;
    fireDetail?: FireDetail;
  };

  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  public coordinationServices = inject(CoordinationAddressService);
  public toast = inject(MatSnackBar);
  public renderer = inject(Renderer2);
  public router = inject(Router);
  public alertService = inject(AlertService);

  private dialogRef = inject(MatDialogRef<FireCoordinationData>);
  readonly sections = [
    { id: 1, label: 'Dirección' },
    { id: 2, label: 'Coordinación CECOPI' },
    { id: 3, label: 'Coordinación PMA' },
  ];

  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  public displayedColumns: string[] = ['fechaHora', 'procendenciaDestino', 'descripcion', 'fichero', 'opciones'];

  editDataDir: any;
  editDataCecopi: any;
  editDataPma: any;
  isDataReady = false;
  idReturn = null;
  isEdit = false;

  async isToEditDocumentation() {
    if (!this.data?.fireDetail?.id) {
      this.isDataReady = true;
      return;
    }

    const dataCordinacion: any = await this.coordinationServices.getById(Number(this.data.fireDetail.id));

    this.editDataDir = dataCordinacion.direcciones;
    this.editDataCecopi = dataCordinacion.coordinacionesCecopi;
    this.editDataPma = dataCordinacion.coordinacionesPMA;
    this.isDataReady = true;
  }

  async ngOnInit() {
    this.spinner.show();
    this.isToEditDocumentation();
  }

  onSelectionChange(event: MatChipListboxChange): void {
    this.spinner.show();
    this.selectedOption = event;
  }

  async onSaveFromChild(value: { save: boolean; delete: boolean; close: boolean; update: boolean }) {
    const keyWithTrue = (Object.keys(value) as Array<keyof typeof value>).find((key) => value[key]);
    console.log('🚀 ~ FireCoordinationData ~ onSaveFromChild ~ keyWithTrue:', keyWithTrue);
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
          this.coordinationServices.clearData();
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
    await this.processData();

    this.coordinationServices.clearData();

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
          const dataCordinacion: any = await this.coordinationServices.getById(Number(this.idReturn));

          this.editDataDir = dataCordinacion.direcciones;
          this.editDataCecopi = dataCordinacion.coordinacionesCecopi;
          this.editDataPma = dataCordinacion.coordinacionesPMA;
          this.isDataReady = true;
          this.spinner.hide();
        });
    }, 2000);
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
          console.log('🚀 ~ FireCoordinationData ~ .then ~ this.data?.fireDetail?.id:', this.data?.fireDetail?.id);
          await this.coordinationServices.delete(Number(this.data?.fireDetail?.id));
          this.coordinationServices.clearData();
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
        } else {
          this.spinner.hide();
        }
      });
  }

  async processData(): Promise<void> {
    await this.handleDataProcessing(
      this.coordinationServices.dataCoordinationAddress(),
      (item) => ({
        id: item.id ? item.id : 0,
        idTipoDireccionEmergencia: Number(item.tipoDireccionEmergencia.id),
        autoridadQueDirige: item.autoridadQueDirige,
        fechaInicio: this.formatDate(item.fechaInicio),
        fechaFin: item.fechaFin ? this.formatDate(item.fechaFin) : '',
      }),
      this.coordinationServices.postAddress.bind(this.coordinationServices),
      'direcciones'
    );

    await this.handleDataProcessing(
      this.coordinationServices.dataCecopi(),
      (item) => ({
        id: item.id ? item.id : 0,
        idProvincia: Number(item.provincia.id),
        idMunicipio: Number(item.municipio.id),
        fechaInicio: this.formatDate(item.fechaInicio),
        lugar: String(item.lugar),
        fechaFin: item.fechaFin ? this.formatDate(item.fechaFin) : '',
        GeoPosicion: this.isGeoPosicionValid(item) ? item.geoPosicion : null,
        observaciones: item.observaciones,
      }),
      this.coordinationServices.postCecopi.bind(this.coordinationServices),
      'coordinaciones'
    );

    await this.handleDataProcessing(
      this.coordinationServices.dataPma(),
      (item) => ({
        id: item.id ? item.id : 0,
        idProvincia: Number(item.provincia.id),
        idMunicipio: Number(item.municipio.id),
        fechaInicio: this.formatDate(item.fechaInicio),
        lugar: String(item.lugar),
        fechaFin: item.fechaFin ? this.formatDate(item.fechaFin) : '',
        GeoPosicion: this.isGeoPosicionValid(item) ? item.geoPosicion : null,
        observaciones: item.observaciones,
      }),
      this.coordinationServices.postPma.bind(this.coordinationServices),
      'coordinaciones'
    );
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

  async handleDataProcessing<T>(data: T[], formatter: (item: T) => any, postService: (body: any) => Promise<any>, key: string): Promise<void> {
    //if (data.length > 0 || this.isEdit ) {
    if (data.length > 0) {
      const formattedData = data.map(formatter);

      const body = {
        idIncendio: this.data.idIncendio,
        idDireccionCoordinacionEmergencia: this.data?.fireDetail?.id ? this.data?.fireDetail?.id : this.idReturn,
        [key]: formattedData,
      };

      const result = await postService(body);
      this.idReturn = result.idDireccionCoordinacionEmergencia;
    }
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

  closeModal(value: boolean) {
    this.dialogRef.close(value);
  }
}
