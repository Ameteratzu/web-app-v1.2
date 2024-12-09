import { Component, EventEmitter, inject, Input, OnInit, Output, signal, ViewChild } from '@angular/core';
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
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { Geometry } from 'ol/geom';
import Feature from 'ol/Feature';
import { SavePayloadModal } from '../../../types/save-payload-modal';

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
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
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
    const coordinationAddress = await this.direcionesServices.getAllDirecciones();
    this.coordinationAddress.set(coordinationAddress);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);
    
    this.formDataCecopi = this.fb.group({
      provincia : ['', Validators.required],
      municipio : ['', Validators.required],
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [''],
      lugar : ['', Validators.required],
      observaciones : [''],
    });

    this.formDataCecopi.get('municipio')?.disable();

    if (this.editData) {
      console.log('Información recibida en el hijo:', this.editData);
      if(this.coordinationServices.dataCecopi().length === 0){
        this.coordinationServices.dataCecopi.set(this.editData);
      }
    }
    this.spinner.hide();
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
    if (this.coordinationServices.dataCecopi().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false  }); 
    }else{
      if (this.editData){
        this.save.emit({ save: false, delete: false, close: false, update: true  });
      } 
    }
  }


  async loadMunicipalities(event: any) {
    const province_id = event.value.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
    this.formDataCecopi.get('municipio')?.enable();
  }
  
  openModalMap() {
    if (!this.formDataCecopi.value.municipio) {
      return;
    }
    const municipioSelected = this.municipalities().find(
      (item) => item.id == this.formDataCecopi.value.municipio.id
    );

    if (!municipioSelected) {
      return;
    }

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      height: '780px',
      maxHeight: '780px',
      data: {
        municipio: municipioSelected,
        listaMunicipios: this.municipalities(),
      },
    });

    dialogRef.componentInstance.save.subscribe(
      (features: Feature<Geometry>[]) => {
        //this.featuresCoords = features;
        console.info('features', features);
      }
    );
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
    this.save.emit({ save: false, delete: false, close: true, update: false  }); 
  }

  delete(){
    this.save.emit({ save: true, delete: false, close: false, update: false  }); 
  }

}
