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
import { Step4Component } from './step4/step4.component';
import { Step5Component } from './step5/step5.component';
import { Step6Component } from './step6/step6.component';
import { Step7Component } from './step7/step7.component';
import { Step8Component } from './step8/step8.component';
import {
  ActuacionRelevante,
  Movilizacion,
  PasoAportacion,
  PasoDespliegue,
  PasoIntervencion,
  PasoLlegadaBase,
  PasoOfrecimiento,
  PasoSolicitud,
  PasoTramitacion,
} from '../../../types/mobilization.type';

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
    Step3Component,
    Step4Component,
    Step5Component,
    Step6Component,
    Step7Component,
    Step8Component,
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
  public pasoActual = signal<number>(1); // Inicialmente en Paso 1

  displayedColumns: string[] = ['solicitante', 'situacion', 'ultimaActualizacion', 'opciones'];
  // Definir datos estáticos
  dataSource = new MatTableDataSource([
    {
      solicitante: 'Delegación del gobierno',
      situacion: 'Emergencia activada',
      ultimaActualizacion: '20/08/2024',
    },
  ]);
  formData!: FormGroup;

  public isCreate = signal<number>(-1);
  public tiposGestion = signal<GenericMaster[]>([]);
  public pasoSolicitud!: PasoSolicitud;
  public pasoTramitacion!: PasoTramitacion;
  public pasoOfrecimiento!: PasoOfrecimiento;
  public pasoAportacion!: PasoAportacion;
  public pasoDespliegue!: PasoDespliegue;
  public pasoIntervencion!: PasoIntervencion;
  public pasoLlegada!: PasoLlegadaBase;
  public movilizacionSeleccionada?: Movilizacion;

  async ngOnInit() {
    console.log('🚀 ~ MobilizationComponent ~ sendDataToEndpoint ~ this.pasoSolicitud:', this.pasoSolicitud);
    this.tiposGestion.set(this.dataMaestros.tiposGestion);

    this.formData = this.fb.group({
      idTipoNotificacion: [null, Validators.required],
      paso1: this.fb.group({
        IdProcedenciaMedio: [null, Validators.required],
        AutoridadSolicitante: ['', Validators.required],
        FechaHoraSolicitud: [new Date(), Validators.required],
        Descripcion: [''],
        Observaciones: [''],
      }),
      paso2: this.fb.group({
        IdDestinoMedio: [null, Validators.required],
        TitularMedio: [''],
        FechaHoraTramitacion: [new Date(), Validators.required],
        PublicadoCECIS: [false],
        Descripcion2: [''],
        Observaciones2: [''],
      }),
      paso3: this.fb.group({
        TitularMedio3: [''],
        GestionCECOD: [false],
        FechaHoraOfrecimiento: [new Date(), Validators.required],
        Descripcion3: [''],
        FechaHoraDisponibilidad: [null],
        Observaciones3: [''],
      }),
      paso5: this.fb.group({
        IdCapacidad: [null, Validators.required],
        MedioNoCatalogado: [''],
        IdTipoAdministracion: [null],
        TitularMedio5: [''],
        FechaHoraAportacion: [new Date(), null],
        Descripcion5: [''],
      }),
      paso6: this.fb.group({
        IdCapacidad: [null, Validators.required],
        MedioNoCatalogado: [''],
        FechaHoraDespliegue: [new Date(), null],
        FechaHoraInicioIntervencion: [null],
        Descripcion6: [''],
        Observaciones6: [''],
      }),
      paso7: this.fb.group({
        IdCapacidad: [null, Validators.required],
        MedioNoCatalogado: [''],
        FechaHoraInicioIntervencion: [new Date(), null],
        Observaciones7: [''],
      }),
      paso8: this.fb.group({
        IdCapacidad: [null, Validators.required],
        MedioNoCatalogado: [''],
        FechaHoraLlegada: [new Date(), null],
        Observaciones8: [''],
      }),
    });

    if (this.editData) {
      if (this.movilizacionService.dataMovilizacion().length === 0) {
        this.movilizacionService.dataMovilizacion.set(this.editData.notificacionesEmergencias);
      }
    }
    this.spinner.hide();
  }

  async onSubmit(formDirective: FormGroupDirective): Promise<void> {
    const pasoActual = this.formData.get('idTipoNotificacion')?.value.id;
    if (pasoActual === undefined || pasoActual === null) {
      console.error('No se ha seleccionado un paso válido.');
      return;
    }

    const actuaciones = this.getOrCreateActuacion();
    const movilizaciones = actuaciones[0].Movilizaciones;

    switch (pasoActual) {
      case 1:
        if (!this.procesarPaso1()) return;
        this.agregarNuevaMovilizacion(movilizaciones, this.pasoSolicitud);
        break;
      case 2:
        if (!this.procesarPaso2()) return;
        if (!this.movilizacionSeleccionada) {
          console.error('No se ha seleccionado una movilización para agregar el Paso 2.');
          return;
        }
        this.movilizacionSeleccionada.Pasos.push(this.pasoTramitacion);
        break;
      case 3:
        if (!this.procesarPaso3()) return;
        if (!this.movilizacionSeleccionada) {
          console.error('No se ha seleccionado una movilización para agregar el Paso 3.');
          return;
        }
        this.movilizacionSeleccionada.Pasos.push(this.pasoOfrecimiento);
        break;
      case 5:
        if (!this.procesarPaso5()) return;
        if (!this.movilizacionSeleccionada) {
          console.error('No se ha seleccionado una movilización para agregar el Paso 5.');
          return;
        }
        this.movilizacionSeleccionada.Pasos.push(this.pasoAportacion);
        break;
      case 6:
        if (!this.procesarPaso6()) return;
        if (!this.movilizacionSeleccionada) {
          console.error('No se ha seleccionado una movilización para agregar el Paso 6.');
          return;
        }
        this.movilizacionSeleccionada.Pasos.push(this.pasoDespliegue);
        break;
      case 7:
        if (!this.procesarPaso7()) return;
        if (!this.movilizacionSeleccionada) {
          console.error('No se ha seleccionado una movilización para agregar el Paso 7.');
          return;
        }
        this.movilizacionSeleccionada.Pasos.push(this.pasoIntervencion);
        break;
      case 8:
        if (!this.procesarPaso8()) return;
        if (!this.movilizacionSeleccionada) {
          console.error('No se ha seleccionado una movilización para agregar el Paso 58');
          return;
        }
        this.movilizacionSeleccionada.Pasos.push(this.pasoLlegada);
        break;
      default:
        console.error('Paso actual desconocido.');
        return;
    }

    this.onReset(formDirective);

    const actuacionRelevante: ActuacionRelevante = {
      IdActuacionRelevante: 0,
      IdSuceso: this.data.idIncendio,
      Movilizaciones: movilizaciones,
    };
    this.movilizacionService.dataMovilizacion.set([actuacionRelevante]);
    console.log('Datos actualizados:', this.movilizacionService.dataMovilizacion());
  }

  private getOrCreateActuacion(): ActuacionRelevante[] {
    let actuaciones = this.movilizacionService.dataMovilizacion();
    if (!actuaciones.length) {
      actuaciones = [
        {
          IdActuacionRelevante: 0,
          IdSuceso: this.data.idIncendio,
          Movilizaciones: [],
        },
      ];
    }
    return actuaciones;
  }

  private agregarNuevaMovilizacion(movilizaciones: Movilizacion[], paso: PasoSolicitud): void {
    const nuevaMovilizacion: Movilizacion = {
      Id: movilizaciones.length,
      Solicitante: paso?.AutoridadSolicitante || 'Solicitud de movilización',
      Pasos: [paso],
    };
    movilizaciones.push(nuevaMovilizacion);
  }

  onReset(formDirective: FormGroupDirective): void {
    const defaultFormValues = {
      idTipoNotificacion: null,
      paso1: {
        IdProcedenciaMedio: null,
        AutoridadSolicitante: '',
        FechaHoraSolicitud: new Date(),
        Descripcion: '',
        Observaciones: '',
      },
      paso2: {
        IdDestinoMedio: null,
        TitularMedio: '',
        FechaHoraTramitacion: new Date(),
        PublicadoCECIS: false,
        Descripcion2: '',
        Observaciones2: '',
      },
      paso3: {
        TitularMedio3: '',
        GestionCECOD: false,
        FechaHoraOfrecimiento: new Date(),
        Descripcion3: '',
        FechaHoraDisponibilidad: null,
        Observaciones3: '',
      },
    };

    this.formData.reset(defaultFormValues);
    formDirective.resetForm(defaultFormValues);
    this.loadTipo(0);
  }

  private procesarPaso1(): boolean {
    const pasoValido =
      (this.formData.get('idTipoNotificacion')?.valid ?? false) &&
      (this.formData.get('paso1.IdProcedenciaMedio')?.valid ?? false) &&
      (this.formData.get('paso1.AutoridadSolicitante')?.valid ?? false) &&
      (this.formData.get('paso1.FechaHoraSolicitud')?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoSolicitud = {
      Id: 0,
      TipoPaso: 1,
      IdProcedenciaMedio: this.formData.value.paso1.IdProcedenciaMedio?.id ?? 0,
      AutoridadSolicitante: this.formData.value.paso1.AutoridadSolicitante,
      FechaHoraSolicitud: new Date(this.formData.value.paso1.FechaHoraSolicitud).toISOString(),
      Descripcion: this.formData.value.paso1.Descripcion || '',
      Observaciones: this.formData.value.paso1.Observaciones || '',
    };

    return true;
  }

  private procesarPaso2(): boolean {
    const pasoValido =
      (this.formData.get('paso2.IdDestinoMedio')?.valid ?? false) && (this.formData.get('paso2.FechaHoraTramitacion')?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoTramitacion = {
      Id: 0,
      TipoPaso: 2,
      IdDestinoMedio: this.formData.value.paso2.IdDestinoMedio?.id ?? 0,
      TitularMedio: this.formData.value.paso2.TitularMedio || '',
      FechaHoraTramitacion: new Date(this.formData.value.paso2.FechaHoraTramitacion).toISOString(),
      PublicadoCECIS: this.formData.value.paso2.PublicadoCECIS ?? false,
      Descripcion: this.formData.value.paso2.Descripcion2 || '',
      Observaciones: this.formData.value.paso2.Observaciones2 || '',
    };

    return true;
  }

  private procesarPaso3(): boolean {
    const pasoValido =
      (this.formData.get('paso3.TitularMedio3')?.valid ?? false) && (this.formData.get('paso3.FechaHoraOfrecimiento')?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoOfrecimiento = {
      Id: 0,
      TipoPaso: 3,
      TitularMedio: this.formData.value.paso3.TitularMedio3 || '',
      FechaHoraOfrecimiento: new Date(this.formData.value.paso3.FechaHoraOfrecimiento).toISOString(),
      FechaHoraDisponibilidad: this.formData.value.paso3.FechaHoraDisponibilidad
        ? new Date(this.formData.value.paso3.FechaHoraDisponibilidad).toISOString()
        : '',
      GestionCECOD: this.formData.value.paso3.GestionCECOD ?? false,
      Descripcion: this.formData.value.paso3.Descripcion3 || '',
      Observaciones: this.formData.value.paso3.Observaciones3 || '',
    };

    return true;
  }

  private procesarPaso5(): boolean {
    const pasoValido = (this.formData.get('paso5.IdCapacidad')?.valid ?? false) && (this.formData.get('paso5.FechaHoraAportacion')?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoAportacion = {
      Id: 0,
      TipoPaso: 5,
      IdCapacidad: this.formData.value.paso5.IdCapacidad?.id ?? 0,
      MedioNoCatalogado: this.formData.value.paso5.MedioNoCatalogado || '',
      IdTipoAdministracion: this.formData.value.paso5.IdTipoAdministracion?.id ?? 0,
      TitularMedio: this.formData.value.paso5.TitularMedio5 || '',
      FechaHoraAportacion: new Date(this.formData.value.paso5.FechaHoraAportacion).toISOString(),
      Descripcion: this.formData.value.paso5.Descripcion5 || '',
    };

    return true;
  }

  private procesarPaso6(): boolean {
    const pasoValido = (this.formData.get('paso6.IdCapacidad')?.valid ?? false) && (this.formData.get('paso6.FechaHoraDespliegue')?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoDespliegue = {
      Id: 0,
      TipoPaso: 6,
      IdCapacidad: this.formData.value.paso6.IdCapacidad?.id ?? 0,
      MedioNoCatalogado: this.formData.value.paso6.MedioNoCatalogado || '',
      FechaHoraDespliegue: new Date(this.formData.value.paso6.FechaHoraDespliegue).toISOString(),
      FechaHoraInicioIntervencion: new Date(this.formData.value.paso6.FechaHoraInicioIntervencion).toISOString(),
      Descripcion: this.formData.value.paso5.Descripcion6 || '',
      Observaciones: this.formData.value.paso5.Observaciones6 || '',
    };
    return true;
  }

  private procesarPaso7(): boolean {
    const pasoValido =
      (this.formData.get('paso7.IdCapacidad')?.valid ?? false) && (this.formData.get('paso7.FechaHoraInicioIntervencion')?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoIntervencion = {
      Id: 0,
      TipoPaso: 7,
      IdCapacidad: this.formData.value.paso7.IdCapacidad?.id ?? 0,
      MedioNoCatalogado: this.formData.value.paso7.MedioNoCatalogado || '',
      FechaHoraInicioIntervencion: new Date(this.formData.value.paso7.FechaHoraInicioIntervencion).toISOString(),
      Observaciones: this.formData.value.paso7.Observaciones7 || '',
      Descripcion: '',
    };

    return true;
  }

  private procesarPaso8(): boolean {
    const pasoValido = (this.formData.get('paso8.IdCapacidad')?.valid ?? false) && (this.formData.get('paso8.FechaHoraLlegada')?.valid ?? false);

    if (!pasoValido) {
      this.formData.markAllAsTouched();
      return false;
    }

    this.pasoLlegada = {
      Id: 0,
      TipoPaso: 8,
      IdCapacidad: this.formData.value.paso8.IdCapacidad?.id ?? 0,
      MedioNoCatalogado: this.formData.value.paso8.MedioNoCatalogado || '',
      FechaHoraLlegada: new Date(this.formData.value.paso8.FechaHoraLlegada).toISOString(),
      Observaciones: this.formData.value.paso8.Observaciones7 || '',
      Descripcion: '',
    };

    return true;
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

  cargarPaso(movilizacion: Movilizacion) {
    this.movilizacionSeleccionada = movilizacion;
    const pasoActual = this.getMaxTipoPaso(this.movilizacionService.dataMovilizacion());
    this.loadTipo(pasoActual);
  }

  getMaxTipoPaso(data: ActuacionRelevante[]): number {
    const allPasos = data.flatMap((actuacion) => (actuacion.Movilizaciones || []).flatMap((movilizacion) => movilizacion.Pasos || []));

    if (allPasos.length === 0) {
      return 0;
    }
    return allPasos.reduce((max, paso) => (paso.TipoPaso > max ? paso.TipoPaso : max), 0);
  }

  getAllMovilizaciones(): Movilizacion[] {
    return this.movilizacionService.dataMovilizacion()?.flatMap((actuacion) => actuacion.Movilizaciones) || [];
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

  eliminarItem(index: number): void {
    const actuaciones = this.movilizacionService.dataMovilizacion();

    if (actuaciones && actuaciones.length > 0) {
      const actuacion = actuaciones[0];
      const movilizaciones = actuacion.Movilizaciones;

      if (index >= 0 && index < movilizaciones.length) {
        const idToDelete = movilizaciones[index].Id;
        console.log('Eliminando la movilización con id:', idToDelete);
        movilizaciones.splice(index, 1);
        actuacion.Movilizaciones = movilizaciones;
        this.movilizacionService.dataMovilizacion.set([actuacion]);
      } else {
        console.error('Índice fuera del rango de movilizaciones');
      }
    } else {
      console.error('No se encontró ninguna actuación en dataMovilizacion');
    }
  }

  async seleccionarItem(index: number) {
    this.isCreate.set(index);
    const data = this.movilizacionService.dataMovilizacion()[index];
  }

  get hasMovilizaciones(): boolean {
    const data = this.movilizacionService.dataMovilizacion();
    return data && data.length > 0 && data[0].Movilizaciones && data[0].Movilizaciones.length > 0;
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

  async loadTipo(id?: any) {
    this.spinner.show();
    id === 8 ? this.formData.get('idTipoNotificacion')?.disable() : this.formData.get('idTipoNotificacion')?.enable();
    const tipos = await ((await id) ? this.movilizacionService.getTipoGestion(id) : this.movilizacionService.getTipoGestion());
    this.tiposGestion.set(tipos);

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

  getFormGroup(controlName: string): FormGroup {
    return this.formData.get(controlName) as FormGroup;
  }
}
