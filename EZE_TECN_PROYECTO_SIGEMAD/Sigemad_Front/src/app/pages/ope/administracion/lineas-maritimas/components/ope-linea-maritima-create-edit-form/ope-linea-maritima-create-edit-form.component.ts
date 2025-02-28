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
import { FormFieldComponent } from '@shared/Inputs/field.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { OpeLineasMaritimasService } from '@services/ope/ope-lineas-maritimas.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { LocalFiltrosOpeLineasMaritimas } from '@services/ope/local-filtro-ope-lineas-maritimas.service';
import { UtilsService } from '@shared/services/utils.service';
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
  selector: 'ope-lineaMaritima-create-edit',
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
  templateUrl: './ope-linea-maritima-create-edit-form.component.html',
  styleUrl: './ope-linea-maritima-create-edit-form.component.scss',
})
export class OpeLineaMaritimaCreateEdit implements OnInit {
  constructor(
    private filtrosOpeLineasMaritimasService: LocalFiltrosOpeLineasMaritimas,
    private opeLineasMaritimasService: OpeLineasMaritimasService,
    public dialogRef: MatDialogRef<OpeLineaMaritimaCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,
    private router: Router,

    @Inject(MAT_DIALOG_DATA) public data: { opeLineaMaritima: any }
  ) {}

  //public filtrosIncendioService = inject(LocalFiltrosIncendio);

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  //PCD
  public snackBar = inject(MatSnackBar);
  public utilsService = inject(UtilsService);
  // FIN PCD

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        nombre: new FormControl('', Validators.required),
        origen: new FormControl('', Validators.required),
        destino: new FormControl('', Validators.required),
        fechaValidezDesde: new FormControl(moment().format('YYYY-MM-DDTHH:mm'), [Validators.required, FechaValidator.validarFecha]),
        fechaValidezHasta: new FormControl(moment().format('YYYY-MM-DDTHH:mm'), [Validators.required, FechaValidator.validarFecha]),
        numeroRotaciones: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
        numeroPasajeros: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
        numeroTurismos: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
        numeroAutocares: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
        numeroCamiones: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
        numeroTotalVehiculos: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
      },
      {
        validators: [FechaValidator.validarFechaFinPosteriorFechaInicio('fechaValidezDesde', 'fechaValidezHasta')],
      }
    );

    if (this.data.opeLineaMaritima?.id) {
      this.formData.patchValue({
        id: this.data.opeLineaMaritima.id,
        nombre: this.data.opeLineaMaritima.nombre,
        origen: this.data.opeLineaMaritima.nombre,
        destino: this.data.opeLineaMaritima.nombre,
        fechaValidezDesde: moment(this.data.opeLineaMaritima.fechaInicioFaseSalida).format('YYYY-MM-DD HH:mm'),
        fechaValidezHasta: moment(this.data.opeLineaMaritima.fechaFinFaseSalida).format('YYYY-MM-DD HH:mm'),
        numeroRotaciones: this.data.opeLineaMaritima.numeroRotaciones,
        numeroPasajeros: this.data.opeLineaMaritima.numeroPasajeros,
        numeroTurismos: this.data.opeLineaMaritima.numeroTurismos,
        numeroAutocares: this.data.opeLineaMaritima.numeroAutocares,
        numeroCamiones: this.data.opeLineaMaritima.numeroCamiones,
        numeroTotalVehiculos: this.data.opeLineaMaritima.numeroTotalVehiculos,
      });
    }
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      if (this.data.opeLineaMaritima?.id) {
        data.id = this.data.opeLineaMaritima.id;
        await this.opeLineasMaritimasService
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
        await this.opeLineasMaritimasService
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
