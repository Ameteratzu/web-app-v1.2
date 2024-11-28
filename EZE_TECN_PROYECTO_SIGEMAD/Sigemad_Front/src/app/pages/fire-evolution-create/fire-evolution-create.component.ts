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
import { EvolutionService } from '../../services/evolution.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService, NgxSpinnerModule } from 'ngx-spinner';

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
    NgxSpinnerModule
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
  public evolutionSevice = inject(EvolutionService);
  public matDialog = inject(MatDialog);
  public toast = inject(MatSnackBar);
  private spinner = inject(NgxSpinnerService);
  
  readonly sections = [
    { id: 1, label: 'Registro / Parámetros' },
    { id: 2, label: 'Área afectada' },
    { id: 3, label: 'Consecuencias / Actuac.' },
    { id: 4, label: 'Intervención de medios' },
  ];


  async ngOnInit() {
  }

  async onSaveFromChild() {
    console.log(`Guardar desde el componente:`);
    console.log("🚀 ~ FireCreateComponent ~ onSaveFromChild ~ this.coordinationServices.dataCoordinationAddress():", this.evolutionSevice.dataRecords())
    const result = await this.evolutionSevice.postData(this.evolutionSevice.dataRecords()[0]);


    if (this.evolutionSevice.dataAffectedArea().length > 0){
   
      for (const item of this.evolutionSevice.dataAffectedArea()) {
        const body = await this.getFormattedDataAreas(item)
        const result = await this.evolutionSevice.postAreas(body);
      }
    }

    this.evolutionSevice.clearData();
    this.closeModal();
    this.spinner.hide();
    this.showToast();

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

  getFormattedDataAreas(data: any): any {
    return {
      idIncendio: this.data.idIncendio, 
      areasAfectadas: [{
        fechaHora: this.formatDate(data.fechaFin),
        idProvincia: data.idProvincia,
        idMunicipio: data.idMunicipio,
        idEntidadMenor: data.idEntidadMenor,
        observaciones: data.observaciones,
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

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
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