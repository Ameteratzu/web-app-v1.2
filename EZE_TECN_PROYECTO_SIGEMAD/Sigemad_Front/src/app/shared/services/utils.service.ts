import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root', // Hace que esté disponible en toda la aplicación
})
export class UtilsService {
  allowOnlyNumbers(event: KeyboardEvent) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode === 8 || charCode === 9 || charCode === 13 || charCode === 27) {
      return;
    }
    if (charCode < 48 || charCode > 57) {
      event.preventDefault();
    }
  }

  allowOnlyNumbersAndDecimal(event: KeyboardEvent) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode !== 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
      event.preventDefault();
    }
  }
}
