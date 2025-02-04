import { Component, EventEmitter, inject, Input, Output, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import moment from 'moment';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ActionsRelevantService } from '../../../services/actions-relevant.service';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { GenericMaster } from '../../../types/actions-relevant.type';
import { _isNumberValue } from '@angular/cdk/coercion';
import { Step1Component } from './step1/step1.component';
import { Step2Component } from './step2/step2.component';
import { Step3Component } from './step3/step3.component';

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
  selector: 'app-mobilization',
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
    NgxSpinnerModule,
    Step1Component,
    Step2Component,
    Step3Component
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
  templateUrl: './mobilization.component.html',
  styleUrl: './mobilization.component.scss',
})
export class MobilizationComponent {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() fire: any;
  @Input() dataMaestros: any;

  public movilizacionService = inject(ActionsRelevantService);
  public toast = inject(MatSnackBar);
  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);

  displayedColumns: string[] = ['solicitante', 'situacion', 'ultimaActualizacion', 'opciones'];
  // Definir datos estáticos
  dataSource = new MatTableDataSource([
    {
      solicitante: 'Delegación del gobierno',
      situacion: 'Emergencia activada',
      ultimaActualizacion: '20/08/2024',
    }
  ]);
  formData!: FormGroup;

  public isCreate = signal<number>(-1);
  public tiposGestion = signal<GenericMaster[]>([]);
  // public dataSource = new MatTableDataSource<any>([]);

  async ngOnInit() {
    this.tiposGestion.set(this.dataMaestros.tiposGestion);

    this.formData = this.fb.group({
      //Paso1
      idTipoNotificacion: [null, Validators.required],
      IdProcedenciaMedio: [null, Validators.required],
      AutoridadSolicitante: ['', Validators.required],
      FechaHoraSolicitud: [new Date(), Validators.required],
      Descripcion: [''],
      Observaciones: [''],
      //Paso2
      IdDestinoMedio: [null, Validators.required],
      TitularMedio: [''],
      FechaHoraTramitacion: [new Date(), Validators.required],
      PublicadoCECIS: [false],
      Descripcion2: [''],
      Observaciones2: [''],
      //Paso 3
      TitularMedio3: [''],
      GestionCECOD: [false],
      FechaHoraOfrecimiento: [new Date(), Validators.required],
      Descripcion3: [''],
      FechaHoraDisponibilidad: [null],
      Observaciones3: [''],
      
    });


    if (this.editData) {
      console.log('🚀 ~ CecodComponent ~ ngOnInit ~ this.editData:', this.editData);
      console.log('🚀 ~ CecodComponent ~ ngOnInit ~ this.cecodService.dataCecod():', this.movilizacionService.dataMovilizacion());
      if (this.movilizacionService.dataMovilizacion().length === 0) {
        this.movilizacionService.dataMovilizacion.set(this.editData.notificacionesEmergencias);
      }
    }
    this.spinner.hide();
  }

  async onSubmit(formDirective: FormGroupDirective) {
    if (this.formData.valid) {
      const data = this.formData.value;
      if (this.isCreate() == -1) {
        this.movilizacionService.dataMovilizacion.set([data, ...this.movilizacionService.dataMovilizacion()]);
      } else {
        this.editarItem(this.isCreate());
      }

      formDirective.resetForm({
        fechaHora: new Date(),
      });
      this.formData.reset();
    } else {
      this.formData.markAllAsTouched();
    }
  }

  async sendDataToEndpoint() {
    if (this.movilizacionService.dataMovilizacion().length > 0 && !this.editData) {
      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      if (this.editData) {
        this.save.emit({ save: false, delete: false, close: false, update: true });
      }
    }
  }

  editarItem(index: number) {
    const dataEditada = this.formData.value;
    this.movilizacionService.dataMovilizacion.update((data) => {
      data[index] = { ...data[index], ...dataEditada };
      return [...data];
    });
    this.isCreate.set(-1);
    this.formData.reset();
  }

  eliminarItem(index: number) {
    this.movilizacionService.dataMovilizacion.update((data) => {
      data.splice(index, 1);
      return [...data];
    });
  }

  async seleccionarItem(index: number) {
    this.isCreate.set(index);
    const data = this.movilizacionService.dataMovilizacion()[index];
    // console.log("🚀 ~ NotificationsComponent ~ seleccionarItem ~ data:", data.idTipoNotificacion)
    // // if (typeof data.idTipoNotificacion === 'number') {
    //   var ob = this.tiposNotificaciones().find((tipo) =>
    //     typeof data.idTipoNotificacion === 'number'
    //       ? tipo.id === data.idTipoNotificacion
    //       : tipo.id === data.idTipoNotificacion.id
    //   );

    // this.formData.get('idTipoNotificacion')?.setValue(ob);

    // }else{
    //   this.formData.get('idTipoNotificacion')?.setValue(data.idTipoNotificacion);
    // }

    // this.formData.get('fechaHoraNotificacion')?.setValue(data.fechaHoraNotificacion);
    // this.formData.get('organosNotificados')?.setValue(data.organosNotificados);
    // this.formData.get('ucpm')?.setValue(data.ucpm);
    // this.formData.get('organismoInternacional')?.setValue(data.organismoInternacional);
    // this.formData.get('otrosPaises')?.setValue(data.otrosPaises);
    // this.formData.get('observaciones')?.setValue(data.observaciones);
  }

  getFormatdate(date: any) {
    if (date) {
      return moment(date).format('DD/MM/YYYY');
    } else {
      return 'Sin fecha selecionada.';
    }
  }

  getTipoNotificacion(value: any) {
    var tipo: any;

    if (_isNumberValue(value)) {
      tipo = this.tiposGestion().find((tipo) => tipo.id === value) || null;
    } else {
      tipo = this.tiposGestion().find((tipo) => tipo.id === value.id) || null;
    }

    return tipo.descripcion;
  }

  async loadTipo(id: any) {
    console.log('🚀 ~ MobilizationComponent ~ loadTipo ~ id:', id);
    this.spinner.show();
    id === 8 ? this.formData.get('idTipoNotificacion')?.disable() : this.formData.get('idTipoNotificacion')?.enable();
    const tipos = await this.movilizacionService.getTipoGestion(id);
    this.tiposGestion.set(tipos);
    this.formData.get('idTipoNotificacion')?.setValue(null);
    // this.formData.patchValue({
    //   nombrePlan: planes[0]?.descripcion ?? '',
    //   nombrePlanPersonalizado: ''
    // });

    this.spinner.hide();
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal() {
    this.save.emit({ save: false, delete: false, close: true, update: false });
  }

  delete() {
    this.save.emit({ save: false, delete: true, close: false, update: false });
  }

  isInteger(value: any): boolean {
    return Number.isInteger(value);
  }
}
