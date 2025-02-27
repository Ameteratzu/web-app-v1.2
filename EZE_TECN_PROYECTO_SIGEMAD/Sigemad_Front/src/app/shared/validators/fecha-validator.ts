// fecha-validator.ts
import { AbstractControl, ValidationErrors } from '@angular/forms';

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

  static validarFechaFinPosteriorFechaInicio(fechaInicioKey: string, fechaFinKey: string) {
    return (form: AbstractControl): ValidationErrors | null => {
      const fechaInicio = form.get(fechaInicioKey)?.value;
      const fechaFin = form.get(fechaFinKey)?.value;

      if (!fechaInicio || !fechaFin) {
        return null; // No validamos si alguna fecha está vacía
      }

      const inicio = new Date(fechaInicio);
      const fin = new Date(fechaFin);

      if (fin <= inicio) {
        // Asigna el error directamente al FormControl correspondiente
        form.get(fechaFinKey)?.setErrors({ fechaFinInvalida: true });
        return { fechaFinInvalida: true };
      }

      // Si no hay error, se limpia cualquier error previamente establecido
      form.get(fechaFinKey)?.setErrors(null);
      return null;
    };
  }
}
