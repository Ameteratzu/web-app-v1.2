import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output, signal, SimpleChanges } from '@angular/core';

import { DateAdapter, MAT_DATE_FORMATS, NativeDateAdapter } from '@angular/material/core';

import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MenuItemActiveService } from '@services/menu-item-active.service';
import { ApiResponse } from '@type/api-response.type';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import moment from 'moment';
import { OpeDatosFronterasService } from '@services/ope/datos/ope-datos-fronteras.service';
import { OpeDatoFronteraCreateEdit } from '../ope-dato-frontera-create-edit-form/ope-dato-frontera-create-edit-form.component';
import { ComparativeDateService } from '@services/comparative-date.service';
import { ComparativeDate } from '@type/comparative-date.type';
import { FORMATO_FECHA } from '@type/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { OpeDatoFrontera } from '@type/ope/datos/ope-dato-frontera.type';
import { LocalFiltrosOpeDatosFronteras } from '@services/ope/datos/local-filtro-ope-datos-fronteras.service';
import { OpeFrontera } from '@type/ope/administracion/ope-frontera.type';
import { OpeFronterasService } from '@services/ope/administracion/ope-fronteras.service';

@Component({
  selector: 'app-ope-dato-frontera-filter-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
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
    MatDialogModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: FORMATO_FECHA },
  ],
  templateUrl: './ope-datos-fronteras-filter-form.component.html',
  styleUrl: './ope-datos-fronteras-filter-form.component.scss',
})
export class OpeDatoFronteraFilterFormComponent implements OnInit {
  @Input() opeDatosFronteras: ApiResponse<OpeDatoFrontera[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opeDatosFronterasChange = new EventEmitter<ApiResponse<OpeDatoFrontera[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public filtrosOpeDatosFronterasService = inject(LocalFiltrosOpeDatosFronteras);
  public opeDatosFronterasService = inject(OpeDatosFronterasService);
  public comparativeDateService = inject(ComparativeDateService);

  public comparativeDates = signal<ComparativeDate[]>([]);

  public menuItemActiveService = inject(MenuItemActiveService);
  private dialog = inject(MatDialog);
  public formData!: FormGroup;

  public opeFronterasService = inject(OpeFronterasService);
  public opeFronteras = signal<OpeFrontera[]>([]);

  myForm!: FormGroup;

  showFilters = false;

  public showDateEnd = signal<boolean>(true);

  async ngOnInit() {
    const fb = new FormBuilder();
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
    });

    const { nombre, fechaInicioFaseSalida, fechaFinFaseSalida, fechaInicioFaseRetorno, fechaFinFaseRetorno, between } = this.filtros();

    this.formData = new FormGroup({
      nombre: new FormControl(nombre ?? ''),
      between: new FormControl(between ?? 1),
      fechaInicioFaseSalida: new FormControl(fechaInicioFaseSalida ?? moment().subtract(4, 'days').toDate()),
      fechaFinFaseSalida: new FormControl(fechaFinFaseSalida ?? moment().toDate()),
      fechaInicioFaseRetorno: new FormControl(fechaInicioFaseRetorno ?? moment().subtract(4, 'days').toDate()),
      fechaFinFaseRetorno: new FormControl(fechaFinFaseRetorno ?? moment().toDate()),
    });

    //this.clearFormFilter();
    this.menuItemActiveService.set.emit('/ope-fronteras');

    const comparativeDates = await this.comparativeDateService.get();
    this.comparativeDates.set(comparativeDates);

    const opeFronteras = await this.opeFronterasService.get();
    this.opeFronteras.set(opeFronteras.data);

    this.onSubmit();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ('refreshFilterForm' in changes) {
      this.onSubmit();
    }
  }

  toggleAccordion(panel: MatExpansionPanel) {
    panel.toggle();
  }

  async onSubmit() {
    if (!this.formData) {
      return;
    }

    this.opeDatosFronterasChange.emit({
      count: 0,
      page: 1,
      pageSize: 10,
      data: [],
      pageCount: 0,
    });
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    //const { nombre } = this.formData.value;

    const opeDatosFronteras = await this.opeDatosFronterasService.get({
      //idOpeFrontera: nombre,
    });

    this.filtrosOpeDatosFronterasService.setFilters(this.formData.value);
    this.opeDatosFronteras = opeDatosFronteras;
    this.opeDatosFronterasChange.emit(this.opeDatosFronteras);
    this.isLoadingChange.emit(false);
    this.isLoading = false;
  }

  clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      /*
      between: 1,
      fechaInicioFaseSalida: moment().subtract(4, 'days').toDate(),
      fechaFinFaseSalida: moment().toDate(),
      fechaInicioFaseRetorno: moment().subtract(4, 'days').toDate(),
      fechaFinFaseRetorno: moment().toDate(),
      nombre: '',
      */
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  goModal(opeFrontera: OpeFrontera) {
    const dialogRef = this.dialog.open(OpeDatoFronteraCreateEdit, {
      width: '75vw',
      maxWidth: 'none',
      data: {
        //title: 'Nuevo - Datos Frontera' + opeFrontera.nombre,
        opeFrontera: opeFrontera,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
        this.onSubmit();
      }
    });
  }

  changeBetween(event: any) {
    this.showDateEnd.set(event.value === 1 || event.value === 5 ? true : false);
  }
}
