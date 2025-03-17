import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, signal, SimpleChanges } from '@angular/core';
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
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';
import { AlertService } from '@shared/alert/alert.service';
import { LocalFiltrosOpeLineasMaritimas } from '@services/ope/administracion/local-filtro-ope-lineas-maritimas.service';
import { OpeLineasMaritimasService } from '@services/ope/administracion/ope-lineas-maritimas.service';
import moment from 'moment';
import { FechaValidator } from '@shared/validators/fecha-validator';
import { UtilsService } from '@shared/services/utils.service';
import { ProvinceService } from '@services/province.service';
import { MunicipalityService } from '@services/municipality.service';
import { CountryService } from '@services/country.service';
import { Province } from '@type/province.type';
import { Municipality } from '@type/municipality.type';
import { Countries } from '@type/country.type';
import { TerritoryService } from '@services/territory.service';
import { AutonomousCommunityService } from '@services/autonomous-community.service';
import { Territory } from '@type/territory.type';
import { AutonomousCommunity } from '@type/autonomous-community.type';
import { COUNTRIES_ID, FECHA_MAXIMA_DATEPICKER, FECHA_MINIMA_DATEPICKER } from '@type/constants';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { FORMATO_FECHA } from '@type/date-formats';
import { OpeFase } from '@type/ope/administracion/ope-fase.type';
import { OpeFasesService } from '@services/ope/administracion/ope-fases.service';
import { OpePuerto } from '@type/ope/administracion/ope-puerto.type';
import { OpePuertosService } from '@services/ope/administracion/ope-puertos.service';

@Component({
  selector: 'ope-linea-maritima-create-edit',
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
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-linea-maritima-create-edit-form.component.html',
  styleUrl: './ope-linea-maritima-create-edit-form.component.scss',
})
export class OpeLineaMaritimaCreateEdit implements OnInit {
  constructor(
    private filtrosOpeLineasMaritimasService: LocalFiltrosOpeLineasMaritimas,
    private opeLineasMaritimasService: OpeLineasMaritimasService,
    private opePuertosService: OpePuertosService,
    private opeFasesService: OpeFasesService,
    public dialogRef: MatDialogRef<OpeLineaMaritimaCreateEdit>,
    private matDialog: MatDialog,
    public alertService: AlertService,
    private router: Router,

    @Inject(MAT_DIALOG_DATA) public data: { opeLineaMaritima: any }
  ) {}

  public opePuertosOrigen = signal<OpePuerto[]>([]);
  public opePuertosDestino = signal<OpePuerto[]>([]);
  public opeFases = signal<OpeFase[]>([]);

  public formData!: FormGroup;

  public today: string = new Date().toISOString().split('T')[0];

  private spinner = inject(NgxSpinnerService);

  //PCD
  public snackBar = inject(MatSnackBar);
  public utilsService = inject(UtilsService);
  public fechaMinimaDatePicker = FECHA_MINIMA_DATEPICKER;
  public fechaMaximaDatePicker = FECHA_MAXIMA_DATEPICKER;
  // FIN PCD

  async ngOnInit() {
    this.formData = new FormGroup(
      {
        nombre: new FormControl('', Validators.required),
        opePuertoOrigen: new FormControl('', Validators.required),
        opePuertoDestino: new FormControl('', Validators.required),
        opeFase: new FormControl('', Validators.required),
        fechaValidezDesde: new FormControl(new Date(), [Validators.required, FechaValidator.validarFecha]),
        fechaValidezHasta: new FormControl(new Date(), [Validators.required, FechaValidator.validarFecha]),
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
      //this.loadMunicipalities({ value: this.data.opeLineaMaritima.idProvincia });
      //this.loadProvinces({ value: this.data.opeLineaMaritima.idCcaa });
      //this.loadMunicipios({ value: this.data.opeLineaMaritima.idProvincia });
      this.formData.patchValue({
        id: this.data.opeLineaMaritima.id,
        nombre: this.data.opeLineaMaritima.nombre,
        opePuertoOrigen: this.data.opeLineaMaritima.idOpePuertoOrigen,
        opePuertoDestino: this.data.opeLineaMaritima.idOpePuertoDestino,
        opeFase: this.data.opeLineaMaritima.idOpeFase,
        fechaValidezDesde: moment(this.data.opeLineaMaritima.fechaValidezDesde).format('YYYY-MM-DD'),
        fechaValidezHasta: moment(this.data.opeLineaMaritima.fechaValidezHasta).format('YYYY-MM-DD'),
        numeroRotaciones: this.data.opeLineaMaritima.numeroRotaciones,
        numeroPasajeros: this.data.opeLineaMaritima.numeroPasajeros,
        numeroTurismos: this.data.opeLineaMaritima.numeroTurismos,
        numeroAutocares: this.data.opeLineaMaritima.numeroAutocares,
        numeroCamiones: this.data.opeLineaMaritima.numeroCamiones,
        numeroTotalVehiculos: this.data.opeLineaMaritima.numeroTotalVehiculos,
      });
    }

    const opePuertos = await this.opePuertosService.get();
    this.opePuertosOrigen.set(opePuertos.data);
    this.opePuertosDestino.set(opePuertos.data);

    const opeFases = await this.opeFasesService.get();
    this.opeFases.set(opeFases);
  }

  async onSubmit() {
    if (this.formData.valid) {
      this.spinner.show();
      const data = this.formData.value;

      //const municipio = this.municipalities().find((item) => item.id === data.municipality);
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
