import { Component, Input, OnInit } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule, AbstractControl  } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'sg-field',
  templateUrl: './field.component.html',
  styleUrls: ['./field.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class FormFieldComponent implements OnInit {
  @Input() label!: string;
  @Input() type: string = 'text';
  @Input() isRequired: boolean = false;
  @Input() options: string[] = [];
  @Input() formControl!: AbstractControl;

  ngOnInit() {
    if (this.isRequired && this.formControl instanceof FormControl) {
      this.formControl.addValidators(Validators.required); // Solo agregar validación si es FormControl
      this.formControl.updateValueAndValidity();
    }
  }

  get showError(): boolean {
    if (this.formControl instanceof FormControl) {
      return this.formControl.invalid && (this.formControl.dirty || this.formControl.touched);
    }
    return false;
  }
}