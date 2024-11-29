import { Component, EventEmitter, inject, OnInit, Output, signal, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout'; 
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { DireccionesService } from '../../../services/direcciones.service'
import { CoordinationAddress } from '../../../types/coordination-address';
import { MatSelectModule } from '@angular/material/select';
import moment from 'moment';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { CoordinationAddressService } from '../../../services/coordination-address.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { ProvinceService } from '../../../services/province.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { Province } from '../../../types/province.type';
import { Municipality } from '../../../types/municipality.type';

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

@Component({
  selector: 'app-cecopi',
  standalone: true,
  imports: [
    ReactiveFormsModule, 
    MatFormFieldModule, 
    MatDatepickerModule, 
    MatNativeDateModule, 
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
  templateUrl: './cecopi.component.html',
  styleUrl: './cecopi.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class CecopiComponent {

  @ViewChild(MatSort) sort!: MatSort;
  @Output() save = new EventEmitter<boolean>();
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };

  public direcionesServices = inject(DireccionesService);
  public coordinationServices = inject(CoordinationAddressService);
  public toast = inject(MatSnackBar);
  
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  private provinceService = inject(ProvinceService);
  private municipalityService = inject(MunicipalityService);
  
  public displayedColumns: string[] = [
    'fechaHora',
    'procendenciaDestino',
    'descripcion',
    'fichero',
    'opciones',
  ]; 

  formDataCecopi!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public isCreate = signal<number>(-1);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    console.log("🚀 ~ CecopiComponent ~ dataCecopi:", this.coordinationServices.dataCecopi())
    const coordinationAddress = await this.direcionesServices.getAllDirecciones();
    this.coordinationAddress.set(coordinationAddress);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);
    

    this.formDataCecopi = this.fb.group({
      idProvincia : ['', Validators.required],
      idMunicipio : ['', Validators.required],
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [new Date(), Validators.required],
      lugar : ['', Validators.required],
      observaciones : ['', Validators.required],
    });

    this.formDataCecopi.get('idMunicipio')?.disable();
  }

  onSubmitCecopi(){
    if (this.formDataCecopi.valid) {
      const data = this.formDataCecopi.value;
      if(this.isCreate() == -1){
        
        this.coordinationServices.dataCecopi.set([data, ...this.coordinationServices.dataCecopi()]);  
      }else{
        this.editarItemCecopi(this.isCreate())
      }
    
      this.formDataCecopi.reset()
    }else {
      this.formDataCecopi.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {

    this.spinner.show();
    if (this.coordinationServices.dataCecopi().length > 0) {
      this.save.emit(true); 
    }else{
      this.spinner.show();
      this.showToast();
    }
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000, 
      horizontalPosition: 'right', 
      verticalPosition: 'top', 
    });
  }

  async loadMunicipalities(event: any) {
    const province_id = event.value.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formDataCecopi.get('idMunicipio')?.enable();
  }

  editarItemCecopi(index: number) {
    const dataEditada = this.formDataCecopi.value;
    this.coordinationServices.dataCecopi.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1)
    this.formDataCecopi.reset()
  }

  eliminarItemCecopi(index: number) {
    this.coordinationServices.dataCecopi.update((data) => {
      data.splice(index, 1); 
      return [...data]; 
    });
  }

  seleccionarItemCecopi(index: number){
    this.isCreate.set(index)
    this.formDataCecopi.patchValue(this.coordinationServices.dataCecopi()[index]);
  }

  getFormatdate(date: any){
    return moment(date).format('DD/MM/YY')
  }

  getFormCecopi(atributo: string): any {
    return this.formDataCecopi.controls[atributo];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal(){
    this.save.emit(false); 
  }

}
