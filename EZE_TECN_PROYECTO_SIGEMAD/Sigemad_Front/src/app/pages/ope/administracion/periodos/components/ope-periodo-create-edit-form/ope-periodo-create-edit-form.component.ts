import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';

// PCD
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormFieldComponent } from '../../../../../../shared/Inputs/field.component';
import { TooltipDirective } from '../../../../../../shared/directive/tooltip/tooltip.directive';
import { AlertService } from '../../../../../../shared/alert/alert.service';
import { LocalFiltrosOpePeriodos } from '../../../../../../services/local-filtro-ope-periodos.service';
import { OpePeriodosService } from '../../../../../../services/ope-periodos.service';
import moment from 'moment';
import { FechaValidator } from '../../../../../../shared/validators/fecha-validator';
// FIN PCD

const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'LL', // Definir el formato de entrada
  },
  display: {
    dateInput: 'LL', // Definir cómo mostrar la fecha
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'ope-periodo-create-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
    FormFieldComponent,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NgxSpinnerModule,
    TooltipDirective,
    DragDropModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
  templateUrl: './ope-periodo-create-edit-form.component.html',
  styleUrl: './ope-periodo-create-edit-form.component.scss',
})
export class OpePeriodoCreateEdit implements OnInit {
  constructor(
    private filtrosOpePeriodosService: LocalFiltrosOpePeriodos,
    private opePeriodosService: OpePeriodosService,
    public dialogRef: MatDialogRef<OpePeriodoCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,
    private router: Router,

    @Inject(MAT_DIALOG_DATA) public data: { opePeriodo: any }
  ) {}

  //public filtrosIncendioService = inject(LocalFiltrosIncendio);

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  //PCD
  public snackBar = inject(MatSnackBar);
  // FIN PCD

  async ngOnInit() {
    this.formData = new FormGroup({
      nombre: new FormControl('', Validators.required),
      fechaInicioFaseSalida: new FormControl(null, [Validators.required, FechaValidator.validarFecha]),
      fechaFinFaseSalida: new FormControl(null, [Validators.required, FechaValidator.validarFecha]),
      fechaInicioFaseRetorno: new FormControl(null, [Validators.required, FechaValidator.validarFecha]),
      fechaFinFaseRetorno: new FormControl(null, [Validators.required, FechaValidator.validarFecha]),
    });

    if (this.data.opePeriodo?.id) {
      this.formData.patchValue({
        id: this.data.opePeriodo.id,
        nombre: this.data.opePeriodo.nombre,
        fechaInicioFaseSalida: moment(this.data.opePeriodo.fechaInicioFaseSalida).format('YYYY-MM-DD HH:mm'),
        fechaFinFaseSalida: moment(this.data.opePeriodo.fechaFinFaseSalida).format('YYYY-MM-DD HH:mm'),
        fechaInicioFaseRetorno: moment(this.data.opePeriodo.fechaInicioFaseRetorno).format('YYYY-MM-DD HH:mm'),
        fechaFinFaseRetorno: moment(this.data.opePeriodo.fechaFinFaseRetorno).format('YYYY-MM-DD HH:mm'),
      });
    }
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      if (this.data.opePeriodo?.id) {
        data.id = this.data.opePeriodo.id;
        await this.opePeriodosService
          .update(data)
          .then((response) => {
            // PCD
            this.snackBar
              .open('Datos modificados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.closeModal({ refresh: true });
                this.spinner.hide();
              });
            // FIN PCD
          })
          .catch((error) => {
            console.error('Error', error);
          });
      } else {
        await this.opePeriodosService
          .post(data)
          .then((response) => {
            this.snackBar
              .open('Datos creados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.closeModal({ refresh: true });
                this.spinner.hide();
              });
          })
          .catch((error) => {
            console.log(error);
          });
      }
    } else {
      this.formData.markAllAsTouched();
    }
  }

  closeModal(params?: any) {
    this.dialogRef.close(params);
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }
}
