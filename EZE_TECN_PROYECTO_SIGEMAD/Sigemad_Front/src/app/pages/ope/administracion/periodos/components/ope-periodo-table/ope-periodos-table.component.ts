import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { OpePeriodo } from '../../../../../../types/ope-periodo.type';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpePeriodoCreateEdit } from '../ope-periodo-create-edit-form/ope-periodo-create-edit-form.component';
import { TooltipDirective } from '../../../../../../shared/directive/tooltip/tooltip.directive';
import { AlertService } from '../../../../../../shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpePeriodosService } from '../../../../../../services/ope-periodos.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-ope-periodos-table',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatPaginatorModule, MatTableModule, CommonModule, TooltipDirective],
  templateUrl: './ope-periodos-table.component.html',
  styleUrl: './ope-periodos-table.component.scss',
})
export class PeriodosTableComponent implements OnChanges {
  @Input() opePeriodos: OpePeriodo[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpePeriodo>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opePeriodosService = inject(OpePeriodosService);
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
    if (changes['opePeriodos'] && this.opePeriodos) {
      this.dataSource.data = this.opePeriodos;
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  goToEdit(periodo: OpePeriodo) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditPeriodo(opePeriodo: OpePeriodo) {}

  goModal() {
    const dialogRef = this.dialog.open(OpePeriodoCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      data: {
        title: 'Nuevo - Periodo',
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

  goModalEdit(opePeriodo: OpePeriodo) {
    const dialogRef = this.dialog.open(OpePeriodoCreateEdit, {
      width: '75vw',
      maxWidth: 'none',
      data: {
        title: 'Modificar - Periodo.',
        opePeriodo: opePeriodo,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);
      }
    });
  }

  //
  async deleteOpePeriodo(idOpePeriodo: number) {
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

          await this.opePeriodosService.delete(idOpePeriodo);
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
                this.routenav.navigate(['/ope-administracion-periodos']).then(() => {
                  window.location.href = '/ope-administracion-periodos';
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
