import { Component, EventEmitter, inject, OnInit, Output, signal } from '@angular/core';
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
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
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
    MatSelectModule
  ],
  templateUrl: './records.component.html',
  styleUrl: './records.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class RecordsComponent implements OnInit {

  @Output() save = new EventEmitter<void>();
  private fb = inject(FormBuilder); 
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
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
      inputOutput : ['', Validators.required],
      startDate: [new Date(), Validators.required],
      media: ['', Validators.required],
      originDestination: ['', Validators.required],
      datetimeUpdate: ['', Validators.required],
      observations_1: ['', Validators.required],
      forecast: ['', Validators.required],
      status: ['', Validators.required],
      end_date: [new Date(), Validators.required],
      emergencyPlanActivated: [''],
      phases: [''],
      nivel: [''],
      operativa: [''],
      afectada: [null, Validators.required],
    });

    this.formData.get('emergencyPlanActivated')?.disable();
    this.formData.get('phases')?.disable();
    this.formData.get('nivel')?.disable();
    this.formData.get('operativa')?.disable();
  }

  async sendDataToEndpoint() {
    this.spinner.show();
    if (this.formData.valid) {
      const formValues = this.formData.value;
  
      const newRecord: EvolucionIncendio = {
        idIncendio:  this.data.idIncendio,
        registro: {
          fechaHoraEvolucion: formValues.startDate.toISOString(),
          idEntradaSalida: formValues.inputOutput,
          idMedio: formValues.media
        },
        datoPrincipal: {
          fechaHora: formValues.datetimeUpdate.toISOString(),
          observaciones: formValues.observations_1,
          prevision: formValues.forecast
        },
        parametro: {
          idEstadoIncendio: formValues.status,
          fechaFinal: formValues.end_date.toISOString(),
          superficieAfectadaHectarea:  formValues.afectada, 
          planEmergenciaActivado: "",
          idFase: 1,
          idSituacionOperativa: 1, 
          idSituacionEquivalente: 1 
        },
        registroProcedenciasDestinos: [] 
      };
  
      this.evolutionSevice.dataRecords.update((records) => [newRecord, ...records]);
  
      this.save.emit();
    } else {
      this.formData.markAllAsTouched();
      this.spinner.hide();
    }
  }

  getFormatdate(date: any){
    return moment(date).format('DD/MM/YY')
  }

  allowOnlyNumbers(event: KeyboardEvent): void {
    const charCode = event.charCode;
    if (charCode < 48 || charCode > 57) {
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

  closeModal(){
    this.matDialog.closeAll();
  }

}
