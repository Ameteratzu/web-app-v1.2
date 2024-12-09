import { Component, EventEmitter, inject, Input, Output, signal, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormGroupDirective } from '@angular/forms';
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
  selector: 'app-address',
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
  templateUrl: './address.component.html',
  styleUrl: './address.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class AddressComponent {

  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<boolean>();
  @Input() editData: any;

  public direcionesServices = inject(DireccionesService);
  public coordinationServices = inject(CoordinationAddressService);
  public toast = inject(MatSnackBar);

  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);

  public displayedColumns: string[] = [
    'fechaHora',
    'procendenciaDestino',
    'descripcion',
    'fichero',
    'opciones',
  ];

  formData!: FormGroup;

  public coordinationAddress = signal<CoordinationAddress[]>([]);
  public isCreate = signal<number>(-1);
  public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    const coordinationAddress = await this.direcionesServices.getAllDirecciones();
    this.coordinationAddress.set(coordinationAddress);

    this.formData = this.fb.group({
      tipoDireccionEmergencia: ['', Validators.required],
      fechaInicio: [new Date(), Validators.required],
      fechaFin: [null],
      autoridadQueDirige: ['', Validators.required],
    });

    if (this.editData) {
      console.log('Información recibida en el hijo:', this.editData);
      if (this.coordinationServices.dataCoordinationAddress().length === 0) {
        this.coordinationServices.dataCoordinationAddress.set(this.editData);
      }
    }
  }

  onSubmit(formDirective: FormGroupDirective): void {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {

        this.coordinationServices.dataCoordinationAddress.set([data, ...this.coordinationServices.dataCoordinationAddress()]);
      } else {
        this.editarItem(this.isCreate())
      }

      formDirective.resetForm();
      this.formData.reset();

    } else {
      this.formData.markAllAsTouched();
    }
  }


  async sendDataToEndpoint() {

    if (this.coordinationServices.dataCoordinationAddress().length > 0) {
      this.save.emit(true);
    } else {

      // this.showToast();
    }
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.coordinationServices.dataCoordinationAddress.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1)
    this.formData.reset()

  }

  eliminarItem(index: number) {
    this.coordinationServices.dataCoordinationAddress.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  seleccionarItem(index: number) {
    const selectedItem = this.coordinationServices.dataCoordinationAddress()[index];
    this.isCreate.set(index);

    this.formData.patchValue({
      ...selectedItem,
      tipoDireccionEmergencia: this.findOptionMatch(selectedItem.tipoDireccionEmergencia),
    });
  }


  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YY')
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal() {
    this.save.emit(false);
  }

  findOptionMatch(option: any) {
    return this.coordinationAddress().find((item) => item.id === option.id);
  }

}
