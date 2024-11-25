import { Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';

import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';


import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
import { OriginDestinationService } from '../../services/origin-destination.service';
import { OriginDestination } from '../../types/origin-destination.type';
import { MediaService } from '../../services/media.service';
import { Media } from '../../types/media.type';
import moment from 'moment';

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
  id?: string
  fechaHora: Date,
  tipoDocumento: string,
  fechaHoraSolicitud: Date,
  fichero: any,
  procendenciaDestino: string,
  medio: string,
  asunto: string,
  observaciones: string,
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
    MatTableModule
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ]
})
export class FireDocumentation implements OnInit {

  constructor(
    private originDestinationService: OriginDestinationService,
    private mediaService: MediaService,
    private dialogRef: MatDialogRef<FireDocumentation>
  ){}

  @ViewChild(MatSort) sort!: MatSort;
  
  private fb = inject(FormBuilder);
  data = inject(MAT_DIALOG_DATA) as { title: string };

  formData!: FormGroup;

  entradaSalidaOptions = ['Entrada', 'Salida'];

  tipoRegistroOptions = ['Tipo 1', 'Tipo 2'];
  estadoOptions = ['Estado 1', 'Estado 2'];
  planEmergenciaOptions = ['Sí', 'No'];
  readonly sections: string[] = ['Documentación'];

  selectOptions = ['Opción 1', 'Opción 2', 'Opción 3'];
  anotherSelectOptions = ['Otro 1', 'Otro 2', 'Otro 3'];

  ////////////////////////////////
  public listadoProcedenciaDestino = signal<OriginDestination[]>([]);
  public listadoTipoDocumento = signal<OriginDestination[]>([]);
  public listadoMedios = signal<Media[]>([]);
  public dataOtherInformation = signal<FormType[]>([]);
  public isCreate = signal<number>(-1);

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = [
    'fechaHora',
    'procendenciaDestino',
    'descripcion',
    'fichero',
    'opciones',
  ]; 

  async ngOnInit() {
    this.formData = this.fb.group({
      fechaHora: ['', Validators.required],
      fechaHoraSolicitud: ['', Validators.required],
      tipoDocumento: ['', Validators.required],
      procendenciaDestino: ['', Validators.required],
      medio: ['', Validators.required],
      asunto: ['', Validators.required],
      observaciones: ['', Validators.required],
    });

    this.dataSource.data=[
      {
        fechaHora: 'fechaHora',
        procendenciaDestino: 'procendenciaDestino',
        medio: 'medio',
        asunto: 'asunto',
        observaciones: 'observaciones',
      },
      {
        fechaHora: 'fechaHora',
        procendenciaDestino: 'procendenciaDestino',
        medio: 'medio',
        asunto: 'asunto',
        observaciones: 'observaciones',
      },
    ]

    const procedenciasDestino = await this.originDestinationService.get();
    this.listadoProcedenciaDestino.set(procedenciasDestino);

    const medios = await this.mediaService.get();
    this.listadoMedios.set(medios);
    
  }

  trackByFn(index: number, item: string): string {
    return item;
  }

  onSubmit(){
    console.info("submit")
    if (this.formData.valid) {

      const data = this.formData.value;
      if(this.isCreate() == -1){
        this.dataOtherInformation.set([data, ...this.dataOtherInformation()]);  
      }else{
        this.editarItem(this.isCreate())
      }
      
      this.formData.reset()
    }else {
      this.formData.markAllAsTouched();
    }

  }

  seleccionarItem(index: number){
    this.isCreate.set(index)
    this.formData.patchValue(this.dataOtherInformation()[index]);
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataOtherInformation.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1)
    this.formData.reset()
    
  }

  
  eliminarItem(index: number) {
    this.dataOtherInformation.update((data) => {
      data.splice(index, 1); 
      return [...data]; 
    });
  }

  getFormatdate(date: any){
    return moment(date).format('DD/MM/YY')
  }

  closeModal(){
    this.dialogRef.close();
  }

  fileName: string | null = null;

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.fileName = input.files[0].name;
    }
  }
}
