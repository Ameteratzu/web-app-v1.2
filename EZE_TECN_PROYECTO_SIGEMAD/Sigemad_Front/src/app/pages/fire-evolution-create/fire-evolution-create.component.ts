import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RecordsComponent } from './records/records.component';
import { ConsequencesComponent } from './consequences/consequences.component';
import { InterventionComponent } from './intervention/intervention.component';
import { AreaComponent } from './area/area.component';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

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
    AreaComponent
  ],
  animations: [
    trigger('fadeInOut', [
      state('void', style({ opacity: 0, transform: 'translateY(20px)' })), 
      transition(':enter', [animate('900ms ease-out')]), 
      transition(':leave', [animate('0ms ease-in')])   
    ])
  ]
})
export class FireCreateComponent implements OnInit {

  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  
  readonly sections = [
    { id: 1, label: 'Registro' },
    { id: 2, label: 'Datos principales' },
    { id: 3, label: 'Parámetros' },
    { id: 4, label: 'Área afectada' },
    { id: 5, label: 'Consecuencias/Actuac.' },
    { id: 6, label: 'Intervención de medios' },
  ];


  async ngOnInit() {
  }

  async onSaveFromChild() {
    console.log(`Guardar desde el componente:`);

    // if (this.coordinationServices.dataCoordinationAddress().length > 0){
    //   for (const item of this.coordinationServices.dataCoordinationAddress()) {
    //     const body = await this.getFormattedDataAdress(item)
    //     const result = await this.coordinationServices.postAddress(body);
    //   }
    // }

    // if (this.coordinationServices.dataCecopi().length > 0){
    //   for (const item of this.coordinationServices.dataCecopi()) {
    //     const body = await this.getFormattedDataCecopi(item)
    //     const result = await this.coordinationServices.postCecopi(body);
    //   }
    // }
    // console.log("🚀 ~ FireCoordinationData ~ onSaveFromChild ~ this.coordinationServices.dataPma():", this.coordinationServices.dataPma())
    // if (this.coordinationServices.dataPma().length > 0){
      
    //   for (const item of this.coordinationServices.dataPma()) {
    //     const body = await this.getFormattedDataPma(item)
    //     const result = await this.coordinationServices.postPma(body);
    //   }
    // }

    // this.coordinationServices.clearData();
    // this.closeModal();
    // this.spinner.hide();
    // this.showToast();


  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

}