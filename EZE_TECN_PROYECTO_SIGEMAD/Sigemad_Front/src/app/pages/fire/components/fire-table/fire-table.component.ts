import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, ViewChild, inject } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';
import { TooltipDirective } from '../../../../shared/directive/tooltip/tooltip.directive';
import { Fire } from '../../../../types/fire.type';
import { FireCreateComponent } from '../../../fire-evolution-create/fire-evolution-create.component';
import { FireCreateEdit } from '../fire-create-edit-form/fire-create-edit-form.component';

@Component({
  selector: 'app-fire-table',
  standalone: true,
  templateUrl: './fire-table.component.html',
  styleUrls: ['./fire-table.component.scss'],
  imports: [MatPaginatorModule, MatTableModule, MatDialogModule, CommonModule, MatProgressSpinnerModule, TooltipDirective],
})
export class FireTableComponent implements OnChanges {
  @Input() fires: Fire[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<Fire>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  public displayedColumns: string[] = ['denominacion', 'fechaInicio', 'estado', 'ngp', 'maxNgp', 'ubicacion', 'ultimoRegistro', 'opciones'];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['fires'] && this.fires) {
      this.dataSource.data = this.fires;
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  goToEdit(fire: Fire) {
    this.router.navigate([`fire/fire-national-edit/${fire.id}`]);
  }

  goToEditFire(fire: Fire) {}

  goModal() {
    const dialogRef = this.dialog.open(FireCreateComponent, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      data: {
        title: 'Nuevo - Datos Evolución',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
      }
    });
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
    const { fechaUltimoRegistro } = fire;
    if(fechaUltimoRegistro){
      return fechaUltimoRegistro ? moment(fechaUltimoRegistro).format('DD/MM/yyyy hh:mm') : moment(fire.fechaUltimoRegistro).format('DD/MM/yyyy hh:mm');
    }else{
      return 'Sin fecha registrada.'
    }
    
  }

  getFechaInicio(fecha: any) {
    return moment(fecha).format('DD/MM/yyyy hh:mm');
  }

  goModalEdit(fire: Fire) {
    const dialogRef = this.dialog.open(FireCreateEdit, {
      width: '75vw',
      maxWidth: 'none',
      data: {
        title: 'Modificar - Incendio.',
        fire: fire,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);
      }
    });
  }
}
