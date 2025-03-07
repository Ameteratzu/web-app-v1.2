import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
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

@Component({
  selector: 'app-ope-datos-fronteras-table',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatPaginatorModule, MatTableModule, CommonModule, TooltipDirective],
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
    'nombre',
    'fechaInicioFaseSalida',
    'fechaFinFaseSalida',
    'fechaInicioFaseRetorno',
    'fechaFinFaseRetorno',
    'opciones',
  ];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opeDatosFronteras'] && this.opeDatosFronteras) {
      this.dataSource.data = this.opeDatosFronteras;
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
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
        title: 'Modificar - DatoFrontera.',
        opeDatoFrontera: opeDatoFrontera,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);
      }
    });
  }

  //
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
                this.routenav.navigate(['/ope-administracion-fronteras']).then(() => {
                  window.location.href = '/ope-administracion-fronteras';
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
