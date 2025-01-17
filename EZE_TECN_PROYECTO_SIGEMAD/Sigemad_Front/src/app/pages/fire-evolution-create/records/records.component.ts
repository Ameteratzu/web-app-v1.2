import { Component, effect, EnvironmentInjector, EventEmitter, inject, Input, OnInit, Output, runInInjectionContext, signal } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EvolutionService } from '../../../services/evolution.service';
import { InputOutput } from '../../../types/input-output.type';
import { Media } from '../../../types/media.type';
import { OriginDestination } from '../../../types/origin-destination.type';
import { FireStatus } from '../../../types/fire-status.type';
import { TypesPlans } from '../../../types/types-plans.type';
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
import { MasterDataEvolutionsService } from '../../../services/master-data-evolutions.service';
import { Phases } from '../../../types/phases.type';
import { SituationPlan } from '../../../types/situation-plan.type';

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

  formDataSignal = signal({
    inputOutput: 1,
    startDate: new Date(),
    media: 1,
    originDestination: <OriginDestination[]>[],
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

    const typesPlans = await this.masterData.getTypesPlans();
    this.typesPlans.set(typesPlans);

    const situationEquivalente = await this.masterData.getSituationEquivalent();
    this.situationEquivalent.set(situationEquivalente);

    
    this.estadoIncendio ? (this.formDataSignal().status = this.estadoIncendio) : 0;

    this.formData = this.fb.group({
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
      afectada: [this.formDataSignal().afectada],
    });

    runInInjectionContext(this.environmentInjector, () => {
      effect(() => {
        const data = this.formDataSignal();
        this.formData.patchValue(data, { emitEvent: false });
      });

      // this.formData.valueChanges.subscribe((values) => {
      //   this.formDataSignal.set({ ...this.formDataSignal(), ...values });
      // });
    });

    this.formData.get('phases')?.disable();
    this.formData.get('nivel')?.disable();
    this.formData.get('operativa')?.disable();
    this.formData.get('end_date')?.disable();

    if (this.editData) {
      if (this.evolutionSevice.dataRecords().length === 0) {
        // this.evolutionSevice.dataRecords.update((records) => [this.editData, ...records]);
        this.updateFormWithJson(this.editData);
      }
    }
    this.spinner.hide();
  }

  updateFormWithJson(json: any) {
    const procedenciasSelecteds = () => {
      const idsABuscar = json.registro?.procedenciaDestinos?.map((obj: any) => Number(obj.id)) || [];
      return this.originDestinations().filter((procedencia: OriginDestination) => idsABuscar.includes(procedencia.id));
    };

    this.formDataSignal.set({
      inputOutput: json.registro?.entradaSalida?.id || '',
      startDate: json.registro?.fechaHoraEvolucion ? new Date(json.registro.fechaHoraEvolucion) : new Date(),
      media: json.registro?.medio?.id || '',
      originDestination: procedenciasSelecteds(),
      datetimeUpdate: json.datoPrincipal?.fechaHora ? new Date(json.datoPrincipal.fechaHora) : new Date(),
      observations_1: json.datoPrincipal?.observaciones || '',
      forecast: json.datoPrincipal?.prevision || '',
      status: json.parametro?.estadoIncendio?.id || '',
      end_date: json.parametro?.fechaFinal ? new Date(json.parametro.fechaFinal) : new Date(),
      emergencyPlanActivated: json.parametro?.planEmergencia?.id || '',
      phases: json.parametro?.faseEmergencia?.id || '',
      nivel: json.parametro?.planSituacion?.id || '',
      operativa: json.parametro?.situacionEquivalente?.id || '',
      afectada: json.parametro?.superficieAfectadaHectarea || null,
    });

    this.loadPhases(null, json.parametro?.planEmergencia?.id);
    this.loadLevels();
  }

  async sendDataToEndpoint() {
    this.spinner.show();
    if (this.formData.valid) {
      const formValues = this.formData.value;
      formValues.end_date;
      const newRecord: EvolucionIncendio = {
        idEvolucion: null,
        idSuceso: this.data.idIncendio,
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

  async loadLevels(){
 
      const phases_id = this.editData.parametro?.faseEmergencia?.id;
        const plan_id = this.editData.parametro?.planEmergencia?.id
        const situationsPlans = await this.masterData.getSituationsPlans(plan_id, phases_id );
        this.niveles.set(situationsPlans);
        this.formData.get('nivel')?.setValue(this.editData.parametro?.planSituacion?.id);
      return true
  }

  async loadPhases(event: any, id?: string) {
    let id_plan;
    
    if(!event){
      id_plan = id; 
    }else{
      id_plan = event.value; 
    }

    this.spinner.show();
    const phases = await this.masterData.getPhases(id_plan);
    this.phases.set(phases);
    this.formData.get('phases')?.enable();
    this.spinner.hide();
    return phases
  }

  async loadSituationPlans(event: any) {
    this.spinner.show();
    const phases_id = event.value;
    const plan_id = this.formData.get('emergencyPlanActivated')?.value;
    const situationsPlans = await this.masterData.getSituationsPlans(plan_id, phases_id );

    this.niveles.set(situationsPlans);
    this.formData.get('nivel')?.enable();
    this.formData.get('operativa')?.enable();

    this.spinner.hide();
  }

  async loadSituacionEquivalente(event: any) {
    this.spinner.show();
    let arr: SituationPlan[] = [];
    const nivelSelect = this.niveles().find(situacion => situacion.id === event.value);
    const found = this.situationEquivalent().find(situacion => situacion.descripcion === String(nivelSelect?.situacionEquivalente));
    
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
