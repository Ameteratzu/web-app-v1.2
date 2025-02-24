import { CommonModule } from '@angular/common';
import { Component, effect, EnvironmentInjector, EventEmitter, inject, Input, OnInit, Output, runInInjectionContext, signal } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { MatTableModule } from '@angular/material/table';
import moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';
import { EvolutionService } from '../../../services/evolution.service';
import { MasterDataEvolutionsService } from '../../../services/master-data-evolutions.service';
import { EvolucionIncendio } from '../../../types/evolution-record.type';
import { FireStatus } from '../../../types/fire-status.type';
import { InputOutput } from '../../../types/input-output.type';
import { Media } from '../../../types/media.type';
import { OriginDestination } from '../../../types/origin-destination.type';
import { Phases } from '../../../types/phases.type';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { SituationPlan } from '../../../types/situation-plan.type';
import { SituationsEquivalent } from '../../../types/situations-equivalent.type';
import { TypesPlans } from '../../../types/types-plans.type';

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
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() estadoIncendio: any;
  @Input() fire: any;

  formDataSignal = signal({
    inputOutput: 1,
    startDate: '',
    media: 1,
    originDestination: <OriginDestination[]>[],
    datetimeUpdate: new Date(),
    observations_1: '',
    forecast: '',
    status: 1,
    end_date: null as Date | null,
    emergencyPlanActivated: '',
    phases: '',
    nivel: '',
    operativa: '',
    afectada: null,
  });

  private fb = inject(FormBuilder);
  public matDialog = inject(MatDialog);
  public masterData = inject(MasterDataEvolutionsService);
  public evolutionSevice = inject(EvolutionService);
  private spinner = inject(NgxSpinnerService);
  public toast = inject(MatSnackBar);

  formData!: FormGroup;
  private environmentInjector = inject(EnvironmentInjector);

  public inputOutputs = signal<InputOutput[]>([]);
  public medias = signal<Media[]>([]);
  public originDestinations = signal<OriginDestination[]>([]);
  public status = signal<FireStatus[]>([]);
  public typesPlans = signal<TypesPlans[]>([]);
  public situationEquivalent = signal<SituationsEquivalent[]>([]);
  public isCreate = signal<number>(-1);
  public phases = signal<Phases[]>([]);
  public niveles = signal<SituationPlan[]>([]);
  public operativas = signal<SituationPlan[]>([]);

  async ngOnInit() {
    const inputOutputs = await this.masterData.getInputOutput();
    this.inputOutputs.set(inputOutputs);

    const medias = await this.masterData.getMedia();
    this.medias.set(medias);

    const originDestinations = await this.masterData.getOriginDestination();
    this.originDestinations.set(originDestinations);

    const status = await this.masterData.getFireStatus();
    this.status.set(status);

    const typesPlans = await this.masterData.getTypesPlans(this.fire.provincia.idCcaa);
    this.typesPlans.set(typesPlans);

    const situationEquivalente = await this.masterData.getSituationEquivalent();
    this.situationEquivalent.set(situationEquivalente);

    this.estadoIncendio ? (this.formDataSignal().status = this.estadoIncendio) : 0;

    this.formData = await this.fb.group({
      inputOutput: [this.formDataSignal().inputOutput, Validators.required],
      startDate: [this.formDataSignal().startDate, Validators.required],
      media: [this.formDataSignal().media, Validators.required],
      originDestination: [this.formDataSignal().originDestination, Validators.required],
      datetimeUpdate: [this.formDataSignal().datetimeUpdate, Validators.required],
      observations_1: [this.formDataSignal().observations_1],
      forecast: [this.formDataSignal().forecast],
      status: [this.formDataSignal().status, Validators.required],
      end_date: [this.formDataSignal().end_date],
      emergencyPlanActivated: [this.formDataSignal().emergencyPlanActivated],
      phases: [this.formDataSignal().phases],
      nivel: [this.formDataSignal().nivel],
      operativa: [this.formDataSignal().operativa],
      afectada: [this.formDataSignal().afectada, Validators.min(0)],
    });
    this.formData.get('end_date')?.disable();

    runInInjectionContext(this.environmentInjector, () => {
      effect(() => {
        const { end_date, ...rest } = this.formDataSignal();
        this.formData.patchValue(rest, { emitEvent: false });
      });
    });

    this.formData.get('phases')?.disable();
    this.formData.get('nivel')?.disable();
    this.formData.get('operativa')?.disable();

    if (this.editData) {
      if (this.evolutionSevice.dataRecords().length === 0) {
        this.updateFormWithJson(this.editData);
      }
    } else {
      this.formDataSignal.set({
        ...this.formDataSignal(),
        startDate: this.getCurrentDateTimeString(),
      });
      const startDateControl = this.formData.get('startDate');
      startDateControl?.patchValue(this.getCurrentDateTimeString());
      this.updateEndDate(this.estadoIncendio);
    }
    this.spinner.hide();
  }

  async updateFormWithJson(json: any) {
    const procedenciasSelecteds = () => {
      const idsABuscar = json.registro?.procedenciaDestinos?.map((obj: any) => Number(obj.id)) || [];
      return this.originDestinations().filter((procedencia: OriginDestination) => idsABuscar.includes(procedencia.id));
    };

    const rawDate: string = json.registro?.fechaHoraEvolucion || '';
    let dateValue: Date = new Date();
    if (rawDate) {
      dateValue = new Date(rawDate);
    }

    const formattedDate = dateValue.toISOString().substring(0, 16);

    this.formDataSignal.set({
      inputOutput: json.registro?.entradaSalida?.id || '',
      startDate: formattedDate,
      media: json.registro?.medio?.id || '',
      originDestination: procedenciasSelecteds(),
      datetimeUpdate: json.datoPrincipal?.fechaHora ? new Date(json.datoPrincipal.fechaHora) : new Date(),
      observations_1: json.datoPrincipal?.observaciones || '',
      forecast: json.datoPrincipal?.prevision || '',
      status: json.parametro?.estadoIncendio?.id || '',
      end_date: json.parametro?.fechaFinal ? new Date(json.parametro.fechaFinal) : null,
      emergencyPlanActivated: json.parametro?.planEmergencia?.id || '',
      phases: json.parametro?.faseEmergencia?.id || '',
      nivel: json.parametro?.planSituacion?.id || '',
      operativa: json.parametro?.situacionEquivalente?.id || '',
      afectada: json.parametro?.superficieAfectadaHectarea || null,
    });

    await this.updateEndDate(this.formDataSignal().status);
    this.formData.patchValue({
      end_date: json.parametro?.fechaFinal ? new Date(json.parametro.fechaFinal) : null,
    });
    this.loadPhases(null, json.parametro?.planEmergencia?.id);
    this.loadLevels();
  }

  updateEndDate(statusValue: number) {
    const endDateControl = this.formData.get('end_date');
    if (!endDateControl) return;

    if (statusValue === 2) {
      endDateControl.enable();
      endDateControl.setValidators([Validators.required]);
      endDateControl.patchValue(new Date());
    } else {
      endDateControl.disable();
      endDateControl.clearValidators();
      endDateControl.patchValue(null);
    }
    endDateControl.updateValueAndValidity();
    return true;
  }

  async sendDataToEndpoint() {
    this.spinner.show();
    if (this.formData.valid) {
      const formValues = this.formData.value;

      const newRecord: EvolucionIncendio = {
        idEvolucion: null,
        idSuceso: this.data.idIncendio,
        registro: {
          fechaHoraEvolucion: new Date(formValues.startDate).toISOString(),
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
          fechaFinal: formValues.end_date ? formValues.end_date.toISOString() : '',
          superficieAfectadaHectarea: formValues.afectada,
          idPlanEmergencia: formValues.emergencyPlanActivated,
          idFaseEmergencia: formValues.phases,
          idSituacionEquivalente: Number(formValues.operativa),
          idPlanSituacion: formValues.nivel,
        },
      };

      this.evolutionSevice.dataRecords.update((records) => [newRecord, ...records]);

      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      this.formData.markAllAsTouched();
      this.spinner.hide();
    }
  }

  async loadLevels() {
    const phases_id = this.editData.parametro?.faseEmergencia?.id;
    const plan_id = this.editData.parametro?.planEmergencia?.id;
    let situationsPlans: any[] = [];
    if (plan_id) {
      situationsPlans = await this.masterData.getSituationsPlans(plan_id, phases_id);
    }
    this.niveles.set(situationsPlans);
    this.formData.get('nivel')?.setValue(this.editData.parametro?.planSituacion?.id);
    return true;
  }

  async loadPhases(event: any, id?: string) {
    let id_plan;

    if (!event) {
      id_plan = id;
    } else {
      id_plan = event.value;
    }

    this.spinner.show();
    if (id_plan) {
      const phases = await this.masterData.getPhases(id_plan);
      this.phases.set(phases);
      this.formData.get('phases')?.enable();
      this.spinner.hide();
      return phases;
    }
    this.spinner.hide();
    return [];
  }

  async loadSituationPlans(event: any) {
    this.spinner.show();
    const phases_id = event.value;
    const plan_id = this.formData.get('emergencyPlanActivated')?.value;
    let situationsPlans: any = [];
    if (plan_id) {
      situationsPlans = await this.masterData.getSituationsPlans(plan_id, phases_id);
    }

    this.niveles.set(situationsPlans);
    this.formData.get('nivel')?.enable();
    this.formData.get('operativa')?.enable();

    this.spinner.hide();
  }

  selectStatus(event: any) {
    const status_id = event.value;
    this.updateEndDate(status_id);
  }

  async loadSituacionEquivalente(event: any) {
    this.spinner.show();
    let arr: SituationPlan[] = [];
    const nivelSelect = this.niveles().find((situacion) => situacion.id === event.value);
    const found = this.situationEquivalent().find((situacion) => situacion.descripcion === String(nivelSelect?.situacionEquivalente));

    this.formData.get('operativa')?.setValue(found?.id);
    this.spinner.hide();
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

  private getCurrentDateTimeString(): string {
    const now = new Date();
    return now.toISOString().substring(0, 16);
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
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
