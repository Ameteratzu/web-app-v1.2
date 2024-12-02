import { Component, inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatChipsModule, MatChipListboxChange } from '@angular/material/chips';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout'; 
import { MatGridListModule } from '@angular/material/grid-list';
import { MatSort } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { AddressComponent } from './address/address.component';
import { CecopiComponent } from './cecopi/cecopi.component';
import { PmaComponent } from './pma/pma.component';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { CoordinationAddressService } from '../../services/coordination-address.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FireDetail } from '../../types/fire-detail.type';
import moment from 'moment';

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
    PmaComponent
  ],
  templateUrl: './fire-coordination-data.component.html',
  styleUrl: './fire-coordination-data.component.scss',
  animations: [
    trigger('fadeInOut', [
      state('void', style({ opacity: 0, transform: 'translateY(20px)' })), 
      transition(':enter', [animate('900ms ease-out')]), 
      transition(':leave', [animate('0ms ease-in')])   
    ])
  ]
})
export class FireCoordinationData {

  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { 
    title: string; 
    idIncendio: number;
    fireDetail: FireDetail; 
  };

  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  public coordinationServices = inject(CoordinationAddressService);
  public toast = inject(MatSnackBar);

  
  readonly sections = [
    { id: 1, label: 'Dirección' },
    { id: 2, label: 'Coordinación CECOPI' },
    { id: 3, label: 'Coordinación PMA' },
  ];

  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  public displayedColumns: string[] = [
    'fechaHora',
    'procendenciaDestino',
    'descripcion',
    'fichero',
    'opciones',
  ]; 

  editData: any;
  isDataReady = false; 

  async isToEditDocumentation() {
    if (!this.data?.fireDetail?.id) {
      this.isDataReady = true;
      return;
    }
    const dataOtraInformacion: any = await this.coordinationServices.getById(
      Number(this.data.fireDetail.id)
    );

    this.editData = dataOtraInformacion.direcciones;
    this.isDataReady = true;
  }

  async ngOnInit() {
    this.isToEditDocumentation();
  }

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  async onSaveFromChild(value: boolean) {
    if(value){

      await this.processData();

      this.coordinationServices.clearData();
      this.closeModal();
      this.spinner.hide();
      this.showToast();

    }else{
      this.coordinationServices.clearData();
      this.closeModal();
    }

  }

  async processData(): Promise<void> {
    await this.handleDataProcessing(
      this.coordinationServices.dataCoordinationAddress(),
      (item) => ({
        idTipoDireccionEmergencia: Number(item.idTipoDireccionEmergencia.id),
        autoridadQueDirige: item.autoridadQueDirige,
        fechaInicio: item.fechaInicio ? this.formatDate(item.fechaInicio) : '',
        fechaFin: this.formatDate(item.fechaFin),
      }),
      this.coordinationServices.postAddress.bind(this.coordinationServices),
      'direcciones'
    );
  
    await this.handleDataProcessing(
      this.coordinationServices.dataCecopi(),
      (item) => ({
        idProvincia: Number(item.idProvincia.id),
        idMunicipio: Number(item.idMunicipio.id),
        fechaInicio: this.formatDate(item.fechaInicio),
        lugar: String(item.lugar),
        fechaFin: item.fechaFin ? this.formatDate(item.fechaFin): '',
        GeoPosicion: { type: 'Point', coordinates: [null, null] },
      }),
      this.coordinationServices.postCecopi.bind(this.coordinationServices),
      'coordinaciones'
    );
  
    await this.handleDataProcessing(
      this.coordinationServices.dataPma(),
      (item) => ({
        idProvincia: Number(item.idProvincia.id),
        idMunicipio: Number(item.idMunicipio.id),
        fechaInicio: this.formatDate(item.fechaInicio),
        lugar: String(item.lugar),
        fechaFin: item.fechaFin ? this.formatDate(item.fechaFin): '',
        GeoPosicion: { type: 'Point', coordinates: [null, null] },
      }),
      this.coordinationServices.postPma.bind(this.coordinationServices),
      'coordinaciones'
    );
  }

  async handleDataProcessing<T>(
    data: T[],
    formatter: (item: T) => any,
    postService: (body: any) => Promise<any>,
    key: string
  ): Promise<void> {
    if (data.length > 0) {
      const formattedData = data.map(formatter);
  
      const body = {
        idIncendio: this.data.idIncendio,
        [key]: formattedData, 
      };
  
      const result = await postService(body);
    }
  }

  getFormattedDataPma(data: any): any {
    return {
      idIncendio: this.data.idIncendio, 
      coordinaciones: [{
        idProvincia: Number(data.idProvincia.id),
        idMunicipio: Number(data.idMunicipio.id),
        fechaInicio: this.formatDate(data.fechaInicio),
        lugar: String(data.lugar),
        fechaFin: this.formatDate(data.fechaFin),
        GeoPosicion:{"type":"Point","coordinates":[null,null]}
      }],
    };
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

  closeModal(){
    this.matDialog.closeAll();
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000, 
      horizontalPosition: 'right', 
      verticalPosition: 'top', 
    });
  }

}
