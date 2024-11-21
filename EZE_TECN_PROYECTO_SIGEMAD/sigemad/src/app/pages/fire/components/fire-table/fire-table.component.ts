import { Component, Input, ViewChild, OnChanges, SimpleChanges, inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Fire } from '../../../../types/fire.type';
import { Router } from '@angular/router';
import moment from 'moment';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-fire-table',
  standalone: true,
  templateUrl: './fire-table.component.html',
  styleUrls: ['./fire-table.component.scss'],
  imports: [MatPaginatorModule, MatTableModule],
})
export class FireTableComponent implements OnChanges {
  @Input() fires: Fire[] = []; // Cambiar el tipo a Fire[]

  public dataSource = new MatTableDataSource<Fire>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);

  public displayedColumns: string[] = [
    'denominacion',
    'fechaInicio',
    'estado',
    'ngp',
    'maxNgp',
    'ubicacion',
    'ultimoRegistro',
    'opciones',
  ]; // Define las columnas que se mostrarán

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['fires'] && this.fires) {
      this.dataSource.data = this.fires; // Asigna el array Fire[] al dataSource
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }


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