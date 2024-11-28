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
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };

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

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  async onSaveFromChild() {
    console.log(`Guardar desde el componente:`);

    if (this.coordinationServices.dataCoordinationAddress().length > 0){
      for (const item of this.coordinationServices.dataCoordinationAddress()) {
        const body = await this.getFormattedDataAdress(item)
        const result = await this.coordinationServices.postAddress(body);
      }
    }

    if (this.coordinationServices.dataCecopi().length > 0){
      for (const item of this.coordinationServices.dataCecopi()) {
        const body = await this.getFormattedDataCecopi(item)
        const result = await this.coordinationServices.postCecopi(body);
      }
    }
    console.log("🚀 ~ FireCoordinationData ~ onSaveFromChild ~ this.coordinationServices.dataPma():", this.coordinationServices.dataPma())
    if (this.coordinationServices.dataPma().length > 0){
      
      for (const item of this.coordinationServices.dataPma()) {
        const body = await this.getFormattedDataPma(item)
        const result = await this.coordinationServices.postPma(body);
      }
    }

    this.coordinationServices.clearData();
    this.closeModal();
    this.spinner.hide();
    this.showToast();


  }

  getFormattedDataAdress(data: any): any {
    return {
      idIncendio: this.data.idIncendio, 
      direcciones: [{
        idTipoDireccionEmergencia: Number(data.idTipoDireccionEmergencia.id),
        autoridadQueDirige: data.autoridadQueDirige,
        fechaInicio: this.formatDate(data.fechaInicio),
        fechaFin: this.formatDate(data.fechaFin),
      }],
    };
  }


  getFormattedDataCecopi(data: any): any {
    return {
      idIncendio: this.data.idIncendio, 
      coordinaciones:[{
        idProvincia: Number(data.idProvincia.id),
        idMunicipio: Number(data.idMunicipio.id),
        fechaInicio: this.formatDate(data.fechaInicio),
        lugar: String(data.lugar),
        fechaFin: this.formatDate(data.fechaFin),
        GeoPosicion:{"type":"Point","coordinates":[null,null]}
      }],
    };
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
