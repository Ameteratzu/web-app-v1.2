import { CommonModule } from '@angular/common';
import { Component, Input, signal } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { Capacidad, GenericMaster } from '../../../../types/actions-relevant.type';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';

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
  selector: 'app-step6',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatSelectModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
  ],
  templateUrl: './step6.component.html',
  styleUrl: './step6.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class Step6Component {
  @Input() formGroup!: FormGroup;
  @Input() dataMaestros: any;
  public capacidad = signal<Capacidad[]>([]);

  async ngOnInit() {
    this.capacidad.set(this.dataMaestros.capacidades);
    console.log('🚀 ~ Step5Component ~ ngOnInit ~  this.capacidad:', this.capacidad());
  }

  getForm(controlName: string): FormControl {
    return this.formGroup.get(controlName) as FormControl;
  }
}
