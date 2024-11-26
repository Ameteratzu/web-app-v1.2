import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
import { MatChipsModule } from '@angular/material/chips';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout'; 
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';

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
  selector: 'app-fire-coordination-data',
  standalone: true,
  imports: [
    ReactiveFormsModule, 
    MatFormFieldModule, 
    MatDatepickerModule, 
    MatNativeDateModule, 
    MatChipsModule,
    CommonModule,
    MatInputModule,
    FlexLayoutModule,
    MatGridListModule,
    MatButtonModule
  ],
  templateUrl: './fire-coordination-data.component.html',
  styleUrl: './fire-coordination-data.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class FireCoordinationData implements OnInit {

  data = inject(MAT_DIALOG_DATA) as { title: string };
  readonly sections: string[] = [
    'Registro',
    'Datos principales',
    'Parámetros',
    'Área afectada',
    'Consecuencias/Actuac.',
    'Intervención de medios',
  ];
  formData!: FormGroup;
  private fb = inject(FormBuilder); 

  async ngOnInit() {

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

  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  trackByFn(index: number, item: string): string {
    return item;
  }

}
