import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { SavePayloadModal } from '../../../types/save-payload-modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActionsRelevantService } from '../../../services/actions-relevant.service';
import { EmergenciaNacional } from '../../../types/actions-relevant.type';

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
  selector: 'app-emergency-national',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule,
    ReactiveFormsModule,
    FlexLayoutModule
  ],
  templateUrl: './emergency-national.component.html',
  styleUrl: './emergency-national.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class EmergencyNationalComponent {
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };
  @Output() save = new EventEmitter<SavePayloadModal>();
  @Input() editData: any;
  @Input() esUltimo: boolean | undefined;
  @Input() estadoIncendio: any;
  @Input() fire: any;
  private fb = inject(FormBuilder);
  public actionsRelevantSevice = inject(ActionsRelevantService);
  formData!: FormGroup;

  formDataSignal = signal({
    autoridad: '',
    descripcionSolicitud: '',
    fechaHoraSolicitud: new Date(),
    fechaHoraDeclaracion: new Date(),
    descripcionDeclaracion: '',
    fechaHoraDireccion: new Date(),
    observaciones: '',
  });

  private spinner = inject(NgxSpinnerService);

  async ngOnInit() {
    this.formData = this.fb.group({
      autoridad: [this.formDataSignal().autoridad, Validators.required],
      descripcionSolicitud: [this.formDataSignal().descripcionSolicitud, Validators.required],
      fechaHoraSolicitud: [this.formDataSignal().fechaHoraSolicitud, Validators.required],
      fechaHoraDeclaracion: [this.formDataSignal().fechaHoraDeclaracion, Validators.required],
      descripcionDeclaracion: [this.formDataSignal().descripcionDeclaracion, Validators.required],
      fechaHoraDireccion: [this.formDataSignal().fechaHoraDireccion, Validators.required],
      observaciones: [this.formDataSignal().observaciones],
    });
    this.formData.get('end_date')?.disable();

    // runInInjectionContext(this.environmentInjector, () => {
    //   effect(() => {
    //     const { end_date, ...rest } = this.formDataSignal();
    //     this.formData.patchValue(rest, { emitEvent: false });
    //   });

    // });

    // this.formData.get('phases')?.disable();
    // this.formData.get('nivel')?.disable();
    // this.formData.get('operativa')?.disable();

    // if (this.editData) {
    //   if (this.evolutionSevice.dataRecords().length === 0) {
    //     this.updateFormWithJson(this.editData);
    //   }
    // } else {
    //   this.updateEndDate(this.estadoIncendio);
    // }
    this.spinner.hide();
  }

  async sendDataToEndpoint() {
    this.spinner.show();
    if (this.formData.valid) {
      const formValues = this.formData.value;

      const newRecord: EmergenciaNacional = {
        idActuacionRelevante: null,
        idSuceso: this.data.idIncendio,
        emergenciaNacional: {
          autoridad: formValues.autoridad,
          descripcionSolicitud: formValues.descripcionSolicitud,
          fechaHoraSolicitud: formValues.fechaHoraSolicitud.toISOString(),
          fechaHoraDeclaracion: formValues.fechaHoraDeclaracion.toISOString(),
          descripcionDeclaracion: formValues.descripcionDeclaracion,
          fechaHoraDireccion: formValues.fechaHoraDireccion.toISOString(),
          observaciones: formValues.observaciones,
        }
      };

      this.actionsRelevantSevice.dataEmergencia.update((records) => [newRecord, ...records]);

      this.save.emit({ save: true, delete: false, close: false, update: false });
    } else {
      this.formData.markAllAsTouched();
      this.spinner.hide();
    }
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
