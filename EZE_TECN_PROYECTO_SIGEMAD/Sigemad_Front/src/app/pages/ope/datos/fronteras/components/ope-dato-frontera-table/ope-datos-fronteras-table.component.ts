import { CommonModule } from '@angular/common';
import { Component, effect, EventEmitter, inject, Input, OnChanges, Output, Renderer2, signal, SimpleChanges, ViewChild } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeDatoFrontera } from '@type/ope/datos/ope-dato-frontera.type';
import { OpeDatosFronterasService } from '@services/ope/datos/ope-datos-fronteras.service';
import { OpeDatoFronteraCreateEdit } from '../ope-dato-frontera-create-edit-form/ope-dato-frontera-create-edit-form.component';
import { OpeFrontera } from '@type/ope/administracion/ope-frontera.type';

@Component({
  selector: 'app-ope-datos-fronteras-table',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatPaginatorModule, MatTableModule, CommonModule],
  templateUrl: './ope-datos-fronteras-table.component.html',
  styleUrl: './ope-datos-fronteras-table.component.scss',
})
export class OpeDatosFronterasTableComponent implements OnChanges {
  @Input() opeDatosFronteras: OpeDatoFrontera[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpeDatoFrontera>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opeDatosFronterasService = inject(OpeDatosFronterasService);
  public routenav = inject(Router);

  public displayedColumns: string[] = [
    'opeFrontera',
    'fechaHoraInicioIntervalo',
    'fechaHoraFinIntervalo',
    'numeroVehiculos',
    'afluencia',
    'opciones',
  ];

  cabeceraDatosMapa: string = '';
  opeFronterasOrdenadas: OpeFrontera[] = [];
  public displayedColumnsMapaOpeDatosFronterasRelacionadosPorFechaHora: string[] = [
    'enlacesEdicion',
    'fechaHoraInicioIntervalo',
    'fechaHoraFinIntervalo',
    'datos',
    'total',
  ];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opeDatosFronteras'] && this.opeDatosFronteras) {
      this.dataSource.data = this.opeDatosFronteras;
      // PCD
      if (this.paginator) {
        this.paginator.length = this.getMapaOpeDatosFronterasRelacionadosPorFechaHora()?.length ?? 0;
      }
      // FIN PCD
    }
  }

  ngAfterViewInit(): void {
    //this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  goToEdit(frontera: OpeDatoFrontera) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditDatoFrontera(opeDatoFrontera: OpeDatoFrontera) {}

  goModal() {
    const dialogRef = this.dialog.open(OpeDatoFronteraCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      data: {
        title: 'Nuevo - DatoFrontera',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
      }
    });
  }

  getFechaFormateada(fecha: any) {
    return moment(fecha).format('DD/MM/yyyy HH:mm');
  }

  goModalEdit(opeDatoFrontera: OpeDatoFrontera) {
    const dialogRef = this.dialog.open(OpeDatoFronteraCreateEdit, {
      width: '75vw',
      maxWidth: 'none',
      data: {
        //title: 'Modificar - DatoFrontera.',
        opeFrontera: opeDatoFrontera.opeFrontera,
        opeDatoFrontera: opeDatoFrontera,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);
      }
    });
  }

  async deleteOpeDatoFrontera(idOpeDatoFrontera: number) {
    this.alertService
      .showAlert({
        title: '¿Estás seguro de eliminar el registro?',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
        cancelButtonText: 'Cancelar',
        customClass: {
          title: 'sweetAlert-fsize20',
        },
      })

      .then(async (result) => {
        if (result.isConfirmed) {
          this.spinner.show();
          const toolbar = document.querySelector('mat-toolbar');
          this.renderer.setStyle(toolbar, 'z-index', '1');

          await this.opeDatosFronterasService.delete(idOpeDatoFrontera);
          setTimeout(() => {
            //PCD
            this.snackBar
              .open('Datos eliminados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.routenav.navigate(['/ope-nuevo-datos-fronteras']).then(() => {
                  window.location.href = '/ope-nuevo-datos-fronteras';
                });
                this.spinner.hide();
              });
            // FIN PCD
          }, 2000);
        } else {
          this.spinner.hide();
        }
      });
  }

  getMapaOpeDatosFronterasRelacionadosPorFechaHora() {
    const mapaOpeDatosFronterasRelacionadosPorFechaHora: Map<string, OpeDatoFrontera[]> = new Map();

    this.dataSource.data.forEach((opeDatoFrontera: OpeDatoFrontera) => {
      const clave = `${moment(opeDatoFrontera.fechaHoraInicioIntervalo).format('YYYY-MM-DD HH:mm')} - ${moment(opeDatoFrontera.fechaHoraFinIntervalo).format('YYYY-MM-DD HH:mm')}`;

      if (!mapaOpeDatosFronterasRelacionadosPorFechaHora.has(clave)) {
        mapaOpeDatosFronterasRelacionadosPorFechaHora.set(clave, []);
      }
      mapaOpeDatosFronterasRelacionadosPorFechaHora.get(clave)?.push(opeDatoFrontera);
    });

    const mapaArray = Array.from(mapaOpeDatosFronterasRelacionadosPorFechaHora, ([clave, valores]) => ({
      clave,
      valores,
    }));

    if (mapaArray && mapaArray.length > 0) {
      this.opeFronterasOrdenadas = this.getTodasFronterasOrdenadas(mapaArray);
      this.cabeceraDatosMapa = this.getCabeceraDatosMapa(this.opeFronterasOrdenadas);
    }

    return mapaArray;
  }

  getFechaHoraInicioIntervaloMapa(clave: string): string {
    const fechaHoraInicioIntervalo = clave.split(' - ')[0];
    return this.getFechaFormateada(fechaHoraInicioIntervalo);
  }

  getFechaHoraFinIntervaloMapa(clave: string): string {
    const fechaHoraFinIntervalo = clave.split(' - ')[1];
    return this.getFechaFormateada(fechaHoraFinIntervalo);
  }

  getInicialNombreFrontera(nombre: string): string {
    if (nombre && nombre.length > 0) {
      return nombre.charAt(0).toUpperCase();
    }
    return ''; // Retorna una cadena vacía si el nombre es vacío o nulo
  }

  getCabeceraDatosMapa(opeFronterasOrdenadas: OpeFrontera[]): string {
    let cadenaHTML = '';
    if (opeFronterasOrdenadas && Array.isArray(opeFronterasOrdenadas)) {
      cadenaHTML += '<table class="estiloDatosTabla" style="width: 100%"><tr>';
      opeFronterasOrdenadas.forEach((opeFrontera) => {
        cadenaHTML += '<td class="estiloDatosTD">TURISMOS<br/>' + opeFrontera.nombre.toUpperCase() + '</td>';
        cadenaHTML += '<td class="estiloDatosTD">AUTOCARES<br/> ' + opeFrontera.nombre.toUpperCase() + '</td>';
        cadenaHTML += '<td class="estiloDatosTD">TOTAL<br/>' + opeFrontera.nombre.toUpperCase() + '</td>';
      });
      cadenaHTML += '</tr></table>';
    }
    return cadenaHTML;
  }

  getDatosTablaMapa(valores: OpeDatoFrontera[]): string {
    let cadenaHTML = '';
    if (valores && Array.isArray(valores) && this.opeFronterasOrdenadas && this.opeFronterasOrdenadas.length > 0) {
      cadenaHTML += '<table class="estiloDatosTabla"><tr>';

      // Iteramos sobre las fronteras ordenadas
      this.opeFronterasOrdenadas.forEach((opeFronteraOrdenada) => {
        // Comprobamos si hay algún dato en la tabla de valores para la frontera ordenada
        const opeDatoFrontera = valores.find((dato) => dato.opeFrontera.id === opeFronteraOrdenada.id);

        // Si encontramos el dato, mostramos el valor de los vehículos, si no, mostramos valores vacíos
        if (opeDatoFrontera) {
          cadenaHTML += '<td class="estiloDatosTD">' + opeDatoFrontera.numeroVehiculos + '</td>';
          cadenaHTML += '<td class="estiloDatosTD">' + opeDatoFrontera.numeroVehiculos + '</td>';
          cadenaHTML += '<td class="estiloDatosTD">' + opeDatoFrontera.numeroVehiculos + '</td>';
        } else {
          cadenaHTML += '<td class="estiloDatosTD"> </td>'; // Espacio vacío para el caso de no encontrar datos
          cadenaHTML += '<td class="estiloDatosTD"> </td>';
          cadenaHTML += '<td class="estiloDatosTD"> </td>';
        }
      });

      cadenaHTML += '</tr></table>';
    }

    return cadenaHTML;
  }

  getTotalMapa(valores: OpeDatoFrontera[]): number {
    if (valores && Array.isArray(valores)) {
      return valores.reduce((total, opeDatoFrontera) => {
        return total + (opeDatoFrontera.numeroVehiculos || 0);
      }, 0);
    }

    return 0;
  }

  getTodasFronterasOrdenadas(mapaArray?: Array<{ clave: string; valores: OpeDatoFrontera[] }>): OpeFrontera[] {
    if (!mapaArray || !Array.isArray(mapaArray) || mapaArray.length === 0) {
      return [];
    }

    const opeFronterasMap: Map<number, OpeFrontera> = new Map();

    mapaArray.forEach((item) => {
      if (!item || !Array.isArray(item.valores) || item.valores.length === 0) {
        return;
      }

      item.valores.forEach((opeDatoFrontera) => {
        if (!opeDatoFrontera?.opeFrontera?.id || !opeDatoFrontera.opeFrontera.nombre) {
          return;
        }
        opeFronterasMap.set(opeDatoFrontera.opeFrontera.id, opeDatoFrontera.opeFrontera);
      });
    });

    return Array.from(opeFronterasMap.values()).sort((a, b) => (a.nombre ?? '').localeCompare(b.nombre ?? ''));
  }
}
