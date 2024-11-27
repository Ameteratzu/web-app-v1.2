import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
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
import { MatChipsModule, MatChipListboxChange } from '@angular/material/chips';
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

interface FormTypePma{
  id?: string,
  autoridadQueDirige: string,
  idIncendio?: number
  fechaInicio: Date,
  fechaFin: Date,
  idTipoDireccionEmergencia: number,
}

@Component({
  selector: 'app-pma',
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
  templateUrl: './pma.component.html',
  styleUrl: './pma.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class PmaComponent {

  @ViewChild(MatSort) sort!: MatSort;
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

  formData!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public dataPma = signal<FormTypePma[]>([]);
  public isCreate = signal<number>(-1);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);

  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    const coordinationAddress = await this.direcionesServices.getAllDirecciones();
    this.coordinationAddress.set(coordinationAddress);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);
    

    this.formData = this.fb.group({
      idProvincia : ['', Validators.required],
      idMunicipio : ['', Validators.required],
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [new Date(), Validators.required],
      lugar : ['', Validators.required],
      observaciones : ['', Validators.required],
    });

    this.formData.get('idMunicipio')?.disable();
  }

  onSubmit(){
    if (this.formData.valid) {
      const data = this.formData.value;
      if(this.isCreate() == -1){
        
        this.dataPma.set([data, ...this.dataPma()]);  
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
    const result = await this.coordinationServices.postPma(transformedData);
    this.closeModal();
    this.showToast();
    this.spinner.hide();
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000, 
      horizontalPosition: 'right', 
      verticalPosition: 'top', 
    });
  }

  getFormattedData(): any {
    return {
      idIncendio: this.data.idIncendio, 
      coordinaciones: this.dataPma().map((item: any) => ({
        idProvincia: Number(item.idProvincia.id),
        idMunicipio: Number(item.idMunicipio.id),
        fechaInicio: this.formatDate(item.fechaInicio),
        lugar: String(item.lugar),
        fechaFin: this.formatDate(item.fechaFin),
        GeoPosicion:{"type":"Point","coordinates":[null,null]}
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

  async loadMunicipalities(event: any) {
    const province_id = event.value.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formData.get('idMunicipio')?.enable();
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.dataPma.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1)
    this.formData.reset()
  }

  eliminarItem(index: number) {
    this.dataPma.update((data) => {
      data.splice(index, 1); 
      return [...data]; 
    });
  }

  seleccionarItem(index: number){
    this.isCreate.set(index)
    this.formData.patchValue(this.dataPma()[index]);
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
