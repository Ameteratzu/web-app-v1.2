import { Component, effect, EnvironmentInjector, EventEmitter, inject, Input, OnInit, Output, runInInjectionContext, signal } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { InputOutputService } from '../../../services/input-output.service';
import { MediaService } from '../../../services/media.service';
import { OriginDestinationService } from '../../../services/origin-destination.service';
import { FireStatusService } from '../../../services/fire-status.service';
import { EvolutionService } from '../../../services/evolution.service';
import { PhasesService } from '../../../services/phases.service';
import { SituationsEquivalentService } from '../../../services/situations-equivalent.service';
import { InputOutput } from '../../../types/input-output.type';
import { Media } from '../../../types/media.type';
import { OriginDestination } from '../../../types/origin-destination.type';
import { FireStatus } from '../../../types/fire-status.type';
import { Phases } from '../../../types/phases.type';
import { SituationsEquivalent } from '../../../types/situations-equivalent.type';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import moment from 'moment';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EvolucionIncendio } from '../../../types/evolution-record.type';
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
  selector: 'app-records',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    FlexLayoutModule,
    MatGridListModule,
    MatButtonModule,
    MatButtonModule,
    MatSort,
    MatTableModule,
    MatIconModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    MatSelectModule,
  ],
  templateUrl: './records.component.html',
  styleUrl: './records.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class RecordsComponent implements OnInit {
  formDataSignal = signal({
    inputOutput: '',
    startDate: new Date(),
    media: 1,
    originDestination: [],
    datetimeUpdate: new Date(),
    observations_1: '',
    forecast: '',
    status: 1,
    end_date: new Date(),
    emergencyPlanActivated: '',
    phases: '',
    nivel: '',
    operativa: '',
    afectada: null,
  });

  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;

  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  public inputOutputService = inject(InputOutputService);
  public mediaService = inject(MediaService);
  public originDestinationService = inject(OriginDestinationService);
  public fireStatusService = inject(FireStatusService);
  public evolutionSevice = inject(EvolutionService);
  public phasesService = inject(PhasesService);
  public situationEquivalentService = inject(SituationsEquivalentService);

  private spinner = inject(NgxSpinnerService);
  public toast = inject(MatSnackBar);

  formData!: FormGroup;
  private environmentInjector = inject(EnvironmentInjector);

  public inputOutputs = signal<InputOutput[]>([]);
  public medias = signal<Media[]>([]);
  public originDestinations = signal<OriginDestination[]>([]);
  public status = signal<FireStatus[]>([]);
  public phases = signal<Phases[]>([]);
  public situationEquivalent = signal<SituationsEquivalent[]>([]);
  public isCreate = signal<number>(-1);

  async ngOnInit() {
    const inputOutputs = await this.inputOutputService.get();
    this.inputOutputs.set(inputOutputs);

    const medias = await this.mediaService.get();
    this.medias.set(medias);

    const originDestinations = await this.originDestinationService.get();
    this.originDestinations.set(originDestinations);

    const status = await this.fireStatusService.get();
    this.status.set(status);

    const phases = await this.phasesService.get();
    this.phases.set(phases);

    const situationsEquivalent = await this.situationEquivalentService.get();
    this.situationEquivalent.set(situationsEquivalent);

    this.formData = this.fb.group({
      inputOutput: [this.formDataSignal().inputOutput, Validators.required],
      startDate: [this.formDataSignal().startDate, Validators.required],
      media: [this.formDataSignal().media, Validators.required],
      originDestination: [this.formDataSignal().originDestination, Validators.required],
      datetimeUpdate: [this.formDataSignal().datetimeUpdate, Validators.required],
      observations_1: [this.formDataSignal().observations_1],
      forecast: [this.formDataSignal().forecast],
      status: [this.formDataSignal().status, Validators.required],
      end_date: [this.formDataSignal().end_date, Validators.required],
      emergencyPlanActivated: [this.formDataSignal().emergencyPlanActivated],
      phases: [this.formDataSignal().phases],
      nivel: [this.formDataSignal().nivel],
      operativa: [this.formDataSignal().operativa],
      afectada: [this.formDataSignal().afectada, Validators.required],
    });

    runInInjectionContext(this.environmentInjector, () => {
      effect(() => {
        const data = this.formDataSignal();
        this.formData.patchValue(data, { emitEvent: false });
      });

      this.formData.valueChanges.subscribe((values) => {
        this.formDataSignal.set({ ...this.formDataSignal(), ...values });
      });
    });

    this.formData.get('emergencyPlanActivated')?.disable();
    this.formData.get('phases')?.disable();
    this.formData.get('nivel')?.disable();
    this.formData.get('operativa')?.disable();

    if (this.editData) {
      console.log('Información recibida en el hijo:', this.editData);
      if (this.evolutionSevice.dataRecords().length === 0) {
        this.updateFormWithJson(this.editData);
      }
    }
    this.spinner.hide();
  }

  updateFormWithJson(json: any) {
    this.formDataSignal.set({
      inputOutput: json.registro?.entradaSalida?.id || '',
      startDate: json.registro?.fechaHoraEvolucion ? new Date(json.registro.fechaHoraEvolucion) : new Date(),
      media: json.registro?.medio?.id || '',
      originDestination: json.registro?.procedenciaDestinos?.join(', ') || '',
      datetimeUpdate: json.datoPrincipal?.fechaHora ? new Date(json.datoPrincipal.fechaHora) : new Date(),
      observations_1: json.datoPrincipal?.observaciones || '',
      forecast: json.datoPrincipal?.prevision || '',
      status: json.parametro?.estadoIncendio?.id || '',
      end_date: json.parametro?.fechaFinal ? new Date(json.parametro.fechaFinal) : new Date(),
      emergencyPlanActivated: json.parametro?.planEmergenciaActivado || '',
      phases: json.parametro?.fase?.id || '',
      nivel: json.parametro?.situacionEquivalente?.id || '',
      operativa: json.parametro?.situacionOperativa?.id || '',
      afectada: json.parametro?.superficieAfectadaHectarea || null,
    });
  }

  async sendDataToEndpoint() {
    this.spinner.show();
    if (this.formData.valid) {
      const formValues = this.formData.value;

      const newRecord: EvolucionIncendio = {
        idEvolucion: null,
        idIncendio: this.data.idIncendio,
        registro: {
          fechaHoraEvolucion: formValues.startDate.toISOString(),
          idEntradaSalida: formValues.inputOutput,
          idMedio: formValues.media,
          registroProcedenciasDestinos: formValues.originDestination.map((procendenciaDestino: any) => procendenciaDestino.id),
        },
        datoPrincipal: {
          fechaHora: formValues.datetimeUpdate.toISOString(),
          observaciones: formValues.observations_1,
          prevision: formValues.forecast,
        },
        parametro: {
          idEstadoIncendio: formValues.status,
          fechaFinal: formValues.end_date.toISOString(),
          superficieAfectadaHectarea: formValues.afectada,
          planEmergenciaActivado: '',
          idFase: 1,
          idSituacionOperativa: 1,
          idSituacionEquivalente: 1,
        },
      };

      this.evolutionSevice.dataRecords.update((records) => [newRecord, ...records]);

      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      this.formData.markAllAsTouched();
      this.spinner.hide();
    }
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YY');
  }

  allowOnlyNumbersAndDecimal(event: KeyboardEvent) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode !== 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
      event.preventDefault();
    }
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  closeModal() {
    this.save.emit({ save: false, delete: false, close: true, update: false });
  }

  delete() {
    this.save.emit({ save: false, delete: true, close: false, update: false });
  }
}
