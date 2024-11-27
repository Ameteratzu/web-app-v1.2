import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
import { MatChipsModule, MatChipListboxChange } from '@angular/material/chips';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout'; 
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { DireccionesService } from '../../services/direcciones.service'
import { CoordinationAddress } from '../../types/coordination-address';
import { MatSelectModule } from '@angular/material/select';
import moment from 'moment';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { CoordinationAddressService } from '../../services/coordination-address.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";

const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL', 
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

interface FormType {
  id?: string,
  autoridadQueDirige: string,
  idIncendio?: number
  fechaInicio: Date,
  fechaFin: Date,
  idTipoDireccionEmergencia: number,
}

@Component({
  selector: 'app-fire-coordination-data',
  standalone: true,
  imports: [
    ReactiveFormsModule, 
    MatFormFieldModule, 
    MatDatepickerModule, 
    MatNativeDateModule, 
    MatChipsModule,
    CommonModule,
    MatInputModule,
    FlexLayoutModule,
    MatGridListModule,
    MatButtonModule,
    MatSelectModule,
    MatTableModule,
    MatIconModule,
    NgxSpinnerModule
  ],
  templateUrl: './fire-coordination-data.component.html',
  styleUrl: './fire-coordination-data.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class FireCoordinationData implements OnInit {

  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };

  public direcionesServices = inject(DireccionesService);
  public coordinationServices = inject(CoordinationAddressService);
  public toast = inject(MatSnackBar);
  
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);

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

  formData!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public dataCoordinationAddress = signal<FormType[]>([]);
  public isCreate = signal<number>(-1);

  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    console.log("🚀 ~ FireCoordinationData ~ data:", this.data)
    const coordinationAddress = await this.direcionesServices.getAllDirecciones();
    this.coordinationAddress.set(coordinationAddress);
    
    this.formData = this.fb.group({
      idTipoDireccionEmergencia : ['', Validators.required],
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [new Date(), Validators.required],
      autoridadQueDirige: ['', Validators.required],
    });

  }

  onSubmit(){
    console.info("submit")
    if (this.formData.valid) {
      const data = this.formData.value;
      if(this.isCreate() == -1){
        
        this.dataCoordinationAddress.set([data, ...this.dataCoordinationAddress()]);  
      }else{
        this.editarItem(this.isCreate())
      }
      
      this.formData.reset()
    }else {
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    this.spinner.show();
    const transformedData = this.getFormattedData();
    const result = await this.coordinationServices.postAddress(transformedData);
    this.closeModal();
    this.showToast();
    this.spinner.hide();
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000, // Duración en milisegundos
      horizontalPosition: 'right', // 'start' | 'center' | 'end' | 'left' | 'right'
      verticalPosition: 'top', // 'top' | 'bottom'
    });
  }

  getFormattedData(): any {
    return {
      // idDireccionCoordinacionEmergencia: 0, 
      idIncendio: this.data.idIncendio, 
      direcciones: this.dataCoordinationAddress().map((item: any) => ({
        // id: item.id || 0, 
        idTipoDireccionEmergencia: Number(item.idTipoDireccionEmergencia.id),
        autoridadQueDirige: item.autoridadQueDirige,
        fechaInicio: this.formatDate(item.fechaInicio),
        fechaFin: this.formatDate(item.fechaFin),
      })),
    };
  }

  formatDate(date: Date | string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
    console.log('Seleccionado:', this.selectedOption);
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataCoordinationAddress.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1)
    this.formData.reset()
    
  }

  eliminarItem(index: number) {
    this.dataCoordinationAddress.update((data) => {
      data.splice(index, 1); 
      return [...data]; 
    });
  }

  seleccionarItem(index: number){
    this.isCreate.set(index)
    this.formData.patchValue(this.dataCoordinationAddress()[index]);
  }

  getFormatdate(date: any){
    return moment(date).format('DD/MM/YY')
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal(){
    this.matDialog.closeAll();
  }

}
