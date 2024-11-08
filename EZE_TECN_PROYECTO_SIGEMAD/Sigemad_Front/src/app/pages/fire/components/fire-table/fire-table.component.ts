import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  Input,
} from '@angular/core';
import { Router } from '@angular/router';
import moment from 'moment';
import { ApiResponse } from '../../../../types/api-response.type';
import { Fire } from '../../../../types/fire.type';
import { FireTableToolbarComponent } from '../fire-table-toolbar/fire-table-toolbar.component';

@Component({
  selector: 'app-fire-table',
  standalone: true,
  imports: [CommonModule, FireTableToolbarComponent],
  templateUrl: './fire-table.component.html',
  styleUrl: './fire-table.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FireTableComponent {
  @Input() fires: ApiResponse<Fire[]>;

  public router = inject(Router);

  goToEdit(fire: Fire) {
    this.router.navigate([`/fire-national-edit/${fire.id}`]);
  }

  getUbicacion(fire: Fire) {
    let label = '';
    switch (fire.idTerritorio) {
      case 1:
        label = `${fire?.municipio?.descripcion}`;
        break;
      case 2:
        label = `${fire?.municipio?.descripcion}`;
        break;
      case 3:
        label = `Transfronterizo`;
        break;

      default:
        break;
    }
    return label;
  }

  getLastUpdated(fire: Fire) {
    const { fechaInicio, fechaModificacion } = fire;
    return fechaModificacion
      ? moment(fechaModificacion).format('DD/MM/yyyy hh:mm')
      : moment(fire.fechaInicio).format('DD/MM/yyyy hh:mm');
  }
}
