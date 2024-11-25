import { Component, Input, ViewChild, OnChanges, SimpleChanges, inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Fire } from '../../../../types/fire.type';
import { Router } from '@angular/router';
import moment from 'moment';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { FireCreateComponent } from '../../../fire-evolution-create/fire-evolution-create.component';
import { FireOtherInformationComponent } from '../../../fire-other-information/fire-other-information.component';

@Component({
  selector: 'app-fire-table',
  standalone: true,
  templateUrl: './fire-table.component.html',
  styleUrls: ['./fire-table.component.scss'],
  imports: [MatPaginatorModule, MatTableModule, MatDialogModule],
})
export class FireTableComponent implements OnChanges {
  @Input() fires: Fire[] = []; 

  public dataSource = new MatTableDataSource<Fire>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  public displayedColumns: string[] = [
    'denominacion',
    'fechaInicio',
    'estado',
    'ngp',
    'maxNgp',
    'ubicacion',
    'ultimoRegistro',
    'opciones',
  ]; 

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
    this.router.navigate([`/fire-national-edit/${fire.id}`]);
  }


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

  goModalOtherInformation() {
    const dialogRef = this.dialog.open(FireOtherInformationComponent, {
      width: '90vw', 
      maxWidth: 'none', 
      //height: '90vh', 
      data: {
        title: 'Nuevo - Otra Información', 
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
    const { fechaInicio, fechaModificacion } = fire;
    return fechaModificacion
      ? moment(fechaModificacion).format('DD/MM/yyyy hh:mm')
      : moment(fire.fechaInicio).format('DD/MM/yyyy hh:mm');
  }
}