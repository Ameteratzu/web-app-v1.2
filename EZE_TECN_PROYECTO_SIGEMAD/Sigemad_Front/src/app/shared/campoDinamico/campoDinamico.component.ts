// Enum de tipos de campo
enum TipoCampo {
  Text = 'Text',
  Number = 'Number',
  Checkbox = 'Checkbox',
  Date = 'Date',
  DateTime = 'Datetime',
  Select = 'Select',
}

// Modelo de campo
interface Campo {
  campo: string;
  esObligatorio: boolean;
  id: number;
  idImpactoClasificado: number;
  label: string;
  options: { id: number; description: string }[];
  tipoCampo: TipoCampo;
  initValue?: any;
}

import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';

import { MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
//import { Campo } from '../../types/Campo.type';
import { MapCreateComponent } from '../mapCreate/map-create.component';

import { FlexLayoutModule } from '@angular/flex-layout';

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';

import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { NgxSpinnerModule } from 'ngx-spinner';

const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-campo-dinamico',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatTableModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatDialogModule,
    MatCheckboxModule,
    NgxSpinnerModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
  templateUrl: './campoDinamico.component.html',
  styleUrl: './campoDinamico.component.css',
})
export class CampoDinamico implements OnInit {
  @Input() fields: Campo[] = [];
  @Output() formGroupChange = new EventEmitter<FormGroup>();

  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);

  formGroup: FormGroup = this.fb.group({});

  constructor(
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {}

  async ngOnInit(): Promise<void> {
    await this.createForm();
  }

  async createForm() {
    const group: { [key: string]: any } = {};
    this.fields.forEach((field) => {
      console.info('field.campo', field.campo, field.esObligatorio);
      //group[field.campo] = field.esObligatorio ? [field.initValue, Validators.required] : [field.campo == TipoCampo.Checkbox ? false : null];

      group[field.campo] = [field.initValue, field.esObligatorio ? [Validators.required] : []];
    });
    this.formGroup = this.fb.group(group);

    // Emitir cambios del formulario
    this.formGroupChange.emit(this.formGroup);
    this.formGroup.valueChanges.subscribe(() => {
      this.formGroupChange.emit(this.formGroup);
    });
  }

  ngOnChanges(changes: any): void {
    if (changes['fields']) {
      this.createForm();
      this.cdr.detectChanges(); // Forzar detección de cambios
    }
  }

  ngOnDestroy() {}

  public openModalMapCreate(section: string = '') {
    let mapModalRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
    });

    mapModalRef.componentInstance.section = section;
  }
}
