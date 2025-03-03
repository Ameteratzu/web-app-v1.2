import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { OpePuerto } from '../../../../../../types/ope/ope-puerto.type';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OpePuertoCreateEdit } from '../ope-puerto-create-edit-form/ope-puerto-create-edit-form.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { OpePuertosService } from '@services/ope/ope-puertos.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-ope-puertos-table',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatPaginatorModule, MatTableModule, CommonModule, TooltipDirective],
  templateUrl: './ope-puertos-table.component.html',
  styleUrl: './ope-puertos-table.component.scss',
})
export class OpePuertosTableComponent implements OnChanges {
  @Input() opePuertos: OpePuerto[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpePuerto>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opePuertosService = inject(OpePuertosService);
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
    if (changes['opePuertos'] && this.opePuertos) {
      this.dataSource.data = this.opePuertos;
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  goToEdit(puerto: OpePuerto) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditPuerto(opePuerto: OpePuerto) {}

  goModal() {
    const dialogRef = this.dialog.open(OpePuertoCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      data: {
        title: 'Nuevo - Puerto',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
      }
    });
  }

  getFechaFormateada(fecha: any) {
    return moment(fecha).format('DD/MM/yyyy');
  }

  goModalEdit(opePuerto: OpePuerto) {
    const dialogRef = this.dialog.open(OpePuertoCreateEdit, {
      width: '75vw',
      maxWidth: 'none',
      data: {
        title: 'Modificar - Puerto.',
        opePuerto: opePuerto,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);
      }
    });
  }

  //
  async deleteOpePuerto(idOpePuerto: number) {
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

          await this.opePuertosService.delete(idOpePuerto);
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
                this.routenav.navigate(['/ope-administracion-puertos']).then(() => {
                  window.location.href = '/ope-administracion-puertos';
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
