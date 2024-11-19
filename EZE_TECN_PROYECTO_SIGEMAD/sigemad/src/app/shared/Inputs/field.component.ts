import { ChangeDetectionStrategy, Component, forwardRef, Input, OnInit } from '@angular/core';
import {
  FormControl,
  Validators,
  ReactiveFormsModule,
  AbstractControl,
  NG_VALUE_ACCESSOR,
  ControlValueAccessor,
} from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'sg-field',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './field.component.html',
  styleUrls: ['./field.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormFieldComponent),
      multi: true
    }
  ]
})
export class FormFieldComponent implements OnInit, ControlValueAccessor  {
  @Input() label!: string;
  @Input() type: string = 'text';
  @Input() isRequired: boolean = false;
  @Input() disabled: boolean = false;
  @Input() options: {
    id: string | number;
    descripcion: string;
  }[] = [];
  @Input() change: Function = () => {};
  @Input() formControl!: any;

  private onChange: (value: any) => void = () => {};
  private onTouched: () => void = () => {};

  value: string | null = null;

  
  ngOnInit() {
    //console.info("formControl", this.formControl)
    if (this.isRequired && this.formControl instanceof FormControl) {
      this.formControl.addValidators(Validators.required);
      this.formControl.updateValueAndValidity();
    }
  }

  get showError(): boolean {
    if (this.formControl instanceof FormControl) {
      return (
        this.formControl.invalid &&
        (this.formControl.dirty || this.formControl.touched)
      );
    }
    return false;
  }

   // Implementación de ControlValueAccessor
   writeValue(value: any): void {
    if (value !== undefined) {
      this.value = value;
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    // Puedes manejar el estado deshabilitado aquí si es necesario
  }

  onSelectChange(value: string): void {
    this.value = value;
    this.onChange(value); // Propagar el valor cambiado
  }
}
