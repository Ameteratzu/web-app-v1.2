import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, signal, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AlertService } from '@shared/alert/alert.service';
import { LocalFiltrosOpeDatosFronteras } from '@services/ope/datos/local-filtro-ope-datos-fronteras.service';
import { OpeDatosFronterasService } from '@services/ope/datos/ope-datos-fronteras.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { FORMATO_FECHA } from '../../../../../../types/date-formats';
import { UtilsService } from '@shared/services/utils.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { OpeFrontera } from '@type/ope/administracion/ope-frontera.type';
import { OpeDatoFrontera } from '@type/ope/datos/ope-dato-frontera.type';
import { MatToolbarModule } from '@angular/material/toolbar';

interface FormType {
  id?: string;
  //opeFrontera: { id: string; nombre: string };
  fechaHoraInicioIntervalo: Date;
  fechaHoraFinIntervalo: Date;
  numeroVehiculos: number;
  afluencia: string;
}

@Component({
  selector: 'ope-dato-frontera-create-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
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
    DragDropModule,
    MatTableModule,
    MatToolbarModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-dato-frontera-create-edit-form.component.html',
  styleUrl: './ope-dato-frontera-create-edit-form.component.scss',
})
export class OpeDatoFronteraCreateEdit implements OnInit {
  constructor(
    //private filtrosOpeDatosFronterasService: LocalFiltrosOpeDatosFronteras,
    private opeDatosFronterasService: OpeDatosFronterasService,
    public dialogRef: MatDialogRef<OpeDatoFronteraCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,

    //@Inject(MAT_DIALOG_DATA) public data: { opeDatoFrontera: any }
    @Inject(MAT_DIALOG_DATA) public data: { opeFrontera: OpeFrontera; opeDatoFrontera: OpeDatoFrontera }
  ) {}

  public formData!: FormGroup;

  public dataSource = new MatTableDataSource<any>([]);
  public opeDatosFronterasRelacionados = signal<OpeDatoFrontera[]>([]);

  private spinner = inject(NgxSpinnerService);

  public snackBar = inject(MatSnackBar);
  public utilsService = inject(UtilsService);

  public displayedColumns: string[] = ['fechaHoraInicioIntervalo', 'fechaHoraFinIntervalo', 'numeroVehiculos', 'afluencia', 'opciones'];

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        //opeFrontera: new FormControl('', Validators.required),
        fechaHoraInicioIntervalo: new FormControl(moment().format('YYYY-MM-DDTHH:mm'), [Validators.required, FechaValidator.validarFecha]),
        fechaHoraFinIntervalo: new FormControl(moment().format('YYYY-MM-DDTHH:mm'), [Validators.required, FechaValidator.validarFecha]),
        numeroVehiculos: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
        afluencia: new FormControl('', Validators.required),
      },
      {
        validators: [FechaValidator.validarFechaFinPosteriorFechaInicio('fechaHoraInicioIntervalo', 'fechaHoraFinIntervalo')],
      }
    );

    if (this.data.opeDatoFrontera?.id) {
      this.formData.patchValue({
        id: this.data.opeDatoFrontera.id,
        fechaHoraInicioIntervalo: moment(this.data.opeDatoFrontera.fechaHoraInicioIntervalo).format('YYYY-MM-DDTHH:mm'),
        fechaHoraFinIntervalo: moment(this.data.opeDatoFrontera.fechaHoraFinIntervalo).format('YYYY-MM-DDTHH:mm'),
        numeroVehiculos: this.data.opeDatoFrontera.numeroVehiculos,
        afluencia: this.data.opeDatoFrontera.afluencia,
      });
    }

    const { between, fechaHoraInicioIntervalo, fechaHoraFinIntervalo } = this.formData.value;
    const opeDatosFronterasRelacionados = await this.opeDatosFronterasService.get({
      IdComparativoFecha: between,
      fechaHoraInicioIntervalo: moment(fechaHoraInicioIntervalo).format('YYYY-MM-DD'),
      fechaHoraFinIntervalo: moment(fechaHoraFinIntervalo).format('YYYY-MM-DD'),
    });
    this.opeDatosFronterasRelacionados.set(opeDatosFronterasRelacionados.data);
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      //
      data.opeFrontera = this.data.opeFrontera?.id;
      //

      if (this.data.opeDatoFrontera?.id) {
        data.id = this.data.opeDatoFrontera.id;
        await this.opeDatosFronterasService
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
        await this.opeDatosFronterasService
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

  getFechaFormateada(date: any) {
    return moment(date).format('DD/MM/YY HH:mm');
  }

  async onFechaHoraChange(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const selectedDateTime = input.value;

    const { between, fechaHoraInicioIntervalo, fechaHoraFinIntervalo } = this.formData.value;
    const opeDatosFronterasRelacionados = await this.opeDatosFronterasService.get({
      IdComparativoFecha: between,
      fechaHoraInicioIntervalo: moment(fechaHoraInicioIntervalo).format('YYYY-MM-DD'),
      fechaHoraFinIntervalo: moment(fechaHoraFinIntervalo).format('YYYY-MM-DD'),
    });
    this.opeDatosFronterasRelacionados.set(opeDatosFronterasRelacionados.data);
  }
}
