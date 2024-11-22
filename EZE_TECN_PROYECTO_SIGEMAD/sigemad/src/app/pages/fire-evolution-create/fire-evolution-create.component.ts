import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

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
    MatButtonModule
  ],
})
export class FireCreateComponent implements OnInit {
  private fb = inject(FormBuilder); 
  data = inject(MAT_DIALOG_DATA) as { title: string };

  formData!: FormGroup;

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

  ngOnInit() {
    this.formData = this.fb.group({
      registroFechaHora: ['', Validators.required],
      registroEntradaSalida: ['', Validators.required],
      registroMedio: ['', Validators.required],
      datosFechaHoraActualizacion: ['', Validators.required],
      datosTipoRegistro: ['', Validators.required],
      datosObservaciones: [''],
      datosPrevision: [''],
      parametrosEstado: ['', Validators.required],
      parametrosPlanEmergencia: ['', Validators.required],
    });
  }

  trackByFn(index: number, item: string): string {
    return item;
  }
}