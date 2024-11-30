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

  async onSaveFromChild(value: boolean) {
    if(value){

      await this.processData();

      this.evolutionSevice.clearData();
      this.closeModal();
      this.spinner.hide();
      this.showToast();

    }else{
      this.evolutionSevice.clearData();
      this.closeModal();
    }

  }

  async processData(): Promise<void> {

    await this.handleDataProcessing(
      this.evolutionSevice.dataAffectedArea(),
      (item) => ({
        fechaHora: this.formatDate(item.fechaHora), // Formatea la fecha de cada área
        idProvincia: item.idProvincia.id,
        idMunicipio: item.idMunicipio.id,
        idEntidadMenor: item.idEntidadMenor.id,
        observaciones: item.observaciones,
        GeoPosicion: { type: 'Point', coordinates: [null, null] },
      }),
      this.evolutionSevice.postAreas.bind(this.evolutionSevice),
      'areasAfectadas'
    );

  
    // await this.handleDataProcessing(
    //   this.coordinationServices.dataCecopi(),
    //   (item) => ({
    //     idProvincia: Number(item.idProvincia.id),
    //     idMunicipio: Number(item.idMunicipio.id),
    //     fechaInicio: this.formatDate(item.fechaInicio),
    //     lugar: String(item.lugar),
    //     fechaFin: this.formatDate(item.fechaFin),
    //     GeoPosicion: { type: 'Point', coordinates: [null, null] },
    //   }),
    //   this.coordinationServices.postCecopi.bind(this.coordinationServices),
    //   'coordinaciones'
    // );
  
  
  }
     


  // async onSaveFromChild() {
  //   console.log(`Guardar desde el componente:`);
  //   console.log("🚀 ~ FireCreateComponent ~ onSaveFromChild ~ this.coordinationServices.dataCoordinationAddress():", this.evolutionSevice.dataRecords())
  //   const result = await this.evolutionSevice.postData(this.evolutionSevice.dataRecords()[0]);


  //   if (this.evolutionSevice.dataAffectedArea().length > 0){
   
  //     for (const item of this.evolutionSevice.dataAffectedArea()) {
  //       const body = await this.getFormattedDataAreas(item)
  //       const result = await this.evolutionSevice.postAreas(body);
  //     }
  //   }

  //   this.evolutionSevice.clearData();
  //   this.closeModal();
  //   this.spinner.hide();
  //   this.showToast();
  // }

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