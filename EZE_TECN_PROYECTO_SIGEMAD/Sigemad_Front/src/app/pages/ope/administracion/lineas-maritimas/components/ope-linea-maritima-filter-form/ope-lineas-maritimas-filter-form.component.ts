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
import { ApiResponse } from '../../../../../../types/api-response.type';
import { OpeLineaMaritima } from '../../../../../../types/ope/administracion/ope-linea-maritima.type';
import { FormFieldComponent } from '@shared/Inputs/field.component';
import moment from 'moment';
import { OpeLineasMaritimasService } from '@services/ope/administracion/ope-lineas-maritimas.service';
import { OpeLineaMaritimaCreateEdit } from '../ope-linea-maritima-create-edit-form/ope-linea-maritima-create-edit-form.component';
import { ComparativeDateService } from '@services/comparative-date.service';
import { ComparativeDate } from '../../../../../../types/comparative-date.type';
import { FORMATO_FECHA } from '../../../../../../types/date-formats';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { LocalFiltrosOpeLineasMaritimas } from '@services/ope/administracion/local-filtro-ope-lineas-maritimas.service';

@Component({
  selector: 'app-ope-linea-maritima-filter-form',
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
  templateUrl: './ope-lineas-maritimas-filter-form.component.html',
  styleUrl: './ope-lineas-maritimas-filter-form.component.scss',
})
export class OpeLineaMaritimaFilterFormComponent implements OnInit {
  @Input() opeLineasMaritimas: ApiResponse<OpeLineaMaritima[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() opeLineasMaritimasChange = new EventEmitter<ApiResponse<OpeLineaMaritima[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public filtrosOpeLineasMaritimasService = inject(LocalFiltrosOpeLineasMaritimas);
  public opeLineasMaritimasService = inject(OpeLineasMaritimasService);
  public comparativeDateService = inject(ComparativeDateService);

  public comparativeDates = signal<ComparativeDate[]>([]);

  public menuItemActiveService = inject(MenuItemActiveService);
  private dialog = inject(MatDialog);
  public formData!: FormGroup;

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
    this.menuItemActiveService.set.emit('/ope-lineasMaritimas');

    const comparativeDates = await this.comparativeDateService.get();
    this.comparativeDates.set(comparativeDates);

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

    this.opeLineasMaritimasChange.emit({
      count: 0,
      page: 1,
      pageSize: 10,
      data: [],
      pageCount: 0,
    });
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const { between, fechaInicioFaseSalida, fechaFinFaseSalida, fechaInicioFaseRetorno, fechaFinFaseRetorno, nombre } = this.formData.value;

    const opeLineasMaritimas = await this.opeLineasMaritimasService.get({
      IdComparativoFecha: between,
      fechaInicioFaseSalida: moment(fechaInicioFaseSalida).format('YYYY-MM-DD'),
      fechaFinFaseSalida: moment(fechaFinFaseSalida).format('YYYY-MM-DD'),
      fechaInicioFaseRetorno: moment(fechaInicioFaseRetorno).format('YYYY-MM-DD'),
      fechaFinFaseRetorno: moment(fechaFinFaseRetorno).format('YYYY-MM-DD'),
      nombre: nombre,
    });
    this.filtrosOpeLineasMaritimasService.setFilters(this.formData.value);
    this.opeLineasMaritimas = opeLineasMaritimas;
    this.opeLineasMaritimasChange.emit(this.opeLineasMaritimas);
    this.isLoadingChange.emit(false);
    this.isLoading = false;
  }

  clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      between: 1,
      fechaInicioFaseSalida: moment().subtract(4, 'days').toDate(),
      fechaFinFaseSalida: moment().toDate(),
      fechaInicioFaseRetorno: moment().subtract(4, 'days').toDate(),
      fechaFinFaseRetorno: moment().toDate(),
      nombre: '',
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  goModal() {
    const dialogRef = this.dialog.open(OpeLineaMaritimaCreateEdit, {
      width: '75vw',
      maxWidth: 'none',
      data: {
        title: 'Nuevo - Datos LineaMaritima',
        fire: {},
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
