import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { OpePuntoControlCarretera } from '../../../../../../types/ope/ope-punto-control-carretera.type';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpePuntoControlCarreteraCreateEdit } from '../ope-punto-control-carretera-create-edit-form/ope-punto-control-carretera-create-edit-form.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpePuntosControlCarreterasService } from '@services/ope/ope-puntos-control-carreteras.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-ope-puntos-control-carreteras-table',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatPaginatorModule, MatTableModule, CommonModule, TooltipDirective],
  templateUrl: './ope-puntos-control-carreteras-table.component.html',
  styleUrl: './ope-puntos-control-carreteras-table.component.scss',
})
export class OpePuntosControlCarreterasTableComponent implements OnChanges {
  @Input() opePuntosControlCarreteras: OpePuntoControlCarretera[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpePuntoControlCarretera>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opePuntosControlCarreterasService = inject(OpePuntosControlCarreterasService);
  public routenav = inject(Router);

  public displayedColumns: string[] = [
    'nombre',
    'fechaInicioFaseSalida',
    'fechaFinFaseSalida',
    'fechaInicioFaseRetorno',
    'fechaFinFaseRetorno',
    'opciones',
  ];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opePuntosControlCarreteras'] && this.opePuntosControlCarreteras) {
      this.dataSource.data = this.opePuntosControlCarreteras;
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  goToEdit(puntoControlCarretera: OpePuntoControlCarretera) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditPuntoControlCarretera(opePuntoControlCarretera: OpePuntoControlCarretera) {}

  goModal() {
    const dialogRef = this.dialog.open(OpePuntoControlCarreteraCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      data: {
        title: 'Nuevo - PuntoControlCarretera',
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

  goModalEdit(opePuntoControlCarretera: OpePuntoControlCarretera) {
    const dialogRef = this.dialog.open(OpePuntoControlCarreteraCreateEdit, {
      width: '75vw',
      maxWidth: 'none',
      data: {
        title: 'Modificar - PuntoControlCarretera.',
        opePuntoControlCarretera: opePuntoControlCarretera,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);
      }
    });
  }

  //
  async deleteOpePuntoControlCarretera(idOpePuntoControlCarretera: number) {
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

          await this.opePuntosControlCarreterasService.delete(idOpePuntoControlCarretera);
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
                this.routenav.navigate(['/ope-administracion-puntosControlCarreteras']).then(() => {
                  window.location.href = '/ope-administracion-puntosControlCarreteras';
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
  //
}
