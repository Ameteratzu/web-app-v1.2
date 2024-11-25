import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { InputOutputService } from '../../services/input-output.service';
import { InputOutput } from '../../types/input-output.type';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MediaService } from '../../services/media.service';
import { Media } from '../../types/media.type';
import { OriginDestinationService } from '../../services/origin-destination.service';
import { OriginDestination } from '../../types/origin-destination.type';
import { FireStatusService } from '../../services/fire-status.service';
import { FireStatus } from '../../types/fire-status.type';

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
  selector: 'app-fire-create',
  standalone: true,
  templateUrl: './fire-evolution-create.component.html',
  styleUrls: ['./fire-evolution-create.component.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    FlexLayoutModule
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class FireCreateComponent implements OnInit {

  private fb = inject(FormBuilder); 
  data = inject(MAT_DIALOG_DATA) as { title: string };
  public inputOutputService = inject(InputOutputService);
  public mediaService = inject(MediaService);
  public originDestinationService = inject(OriginDestinationService);
  public fireStatusService = inject(FireStatusService);

  formData!: FormGroup;
  public inputOutputs = signal<InputOutput[]>([]);
  public medias = signal<Media[]>([]);
  public originDestinations = signal<OriginDestination[]>([]);
  public status = signal<FireStatus[]>([]);

  entradaSalidaOptions = ['Entrada', 'Salida'];
  mediosOptions = ['Medio 1', 'Medio 2'];
  tipoRegistroOptions = ['Tipo 1', 'Tipo 2'];
  estadoOptions = ['Estado 1', 'Estado 2'];
  planEmergenciaOptions = ['Sí', 'No'];
  readonly sections: string[] = [
    'Registro',
    'Datos principales',
    'Parámetros',
    'Área afectada',
    'Consecuencias/Actuac.',
    'Intervención de medios',
  ];

  selectOptions = ['Opción 1', 'Opción 2', 'Opción 3'];
  anotherSelectOptions = ['Otro 1', 'Otro 2', 'Otro 3'];

  async ngOnInit() {
    const inputOutputs = await this.inputOutputService.get();
    this.inputOutputs.set(inputOutputs);

    const medias = await this.mediaService.get();
    this.medias.set(medias);

    const originDestinations = await this.originDestinationService.get();
    this.originDestinations.set(originDestinations);

    const status = await this.fireStatusService.get();
    this.status.set(status);

    

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
      emergencyPlanActivated: ['', Validators.required],

      
    });

    // this.formGroup = new FormGroup({
    //   startDate: new FormControl(new Date(), [Validators.required]),
    //   inputOutput: new FormControl('', [Validators.required]),
    //   media: new FormControl('', [Validators.required]),
    //   originDestination: new FormControl('', [Validators.required]),

    //   datetimeUpdate: new FormControl('', [Validators.required]),
    //   recordType: new FormControl('', [Validators.required]),
    //   observations_1: new FormControl(''),
    //   forecast: new FormControl(''),

    //   status: new FormControl('', [Validators.required]),
    //   affectedSurface: new FormControl(''),
    //   end_date: new FormControl(''),
    //   emergencyPlanActivated: new FormControl(''),
    //   operativeStatus: new FormControl(''),
    // });
  }

  trackByFn(index: number, item: string): string {
    return item;
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }
}