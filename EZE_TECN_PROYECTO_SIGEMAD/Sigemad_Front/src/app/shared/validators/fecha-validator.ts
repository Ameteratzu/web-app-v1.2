// fecha-validator.ts
import { AbstractControl } from '@angular/forms';

export class FechaValidator {
  static validarFecha(control: AbstractControl): { [key: string]: boolean } | null {
    const valor = control.value;
    const fecha = new Date(valor);

    if (fecha < new Date('2000-01-01T00:00') || fecha > new Date('2099-12-31T23:59')) {
      return { fechaInvalida: true };
    }

    // Si todo es válido, devolvemos null
    return null;
  }
}
