import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { OpeAreaDescanso } from '../../../../../../types/ope/administracion/ope-area-descanso.type';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpeAreaDescansoCreateEdit } from '../ope-area-descanso-create-edit-form/ope-area-descanso-create-edit-form.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpeAreasDescansoService } from '@services/ope/administracion/ope-areas-descanso.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-ope-areas-descanso-table',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatPaginatorModule, MatTableModule, CommonModule, TooltipDirective],
  templateUrl: './ope-areas-descanso-table.component.html',
  styleUrl: './ope-areas-descanso-table.component.scss',
})
export class OpeAreasDescansoTableComponent implements OnChanges {
  @Input() opeAreasDescanso: OpeAreaDescanso[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpeAreaDescanso>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opeAreasDescansoService = inject(OpeAreasDescansoService);
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
    if (changes['opeAreasDescanso'] && this.opeAreasDescanso) {
      this.dataSource.data = this.opeAreasDescanso;
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  goToEdit(areaDescanso: OpeAreaDescanso) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditAreaDescanso(opeAreaDescanso: OpeAreaDescanso) {}

  goModal() {
    const dialogRef = this.dialog.open(OpeAreaDescansoCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      data: {
        title: 'Nuevo - AreaDescanso',
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

  goModalEdit(opeAreaDescanso: OpeAreaDescanso) {
    const dialogRef = this.dialog.open(OpeAreaDescansoCreateEdit, {
      width: '75vw',
      maxWidth: 'none',
      data: {
        title: 'Modificar - AreaDescanso.',
        opeAreaDescanso: opeAreaDescanso,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);
      }
    });
  }

  //
  async deleteOpeAreaDescanso(idOpeAreaDescanso: number) {
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

          await this.opeAreasDescansoService.delete(idOpeAreaDescanso);
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
                this.routenav.navigate(['/ope-administracion-areasDescanso']).then(() => {
                  window.location.href = '/ope-administracion-areasDescanso';
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
