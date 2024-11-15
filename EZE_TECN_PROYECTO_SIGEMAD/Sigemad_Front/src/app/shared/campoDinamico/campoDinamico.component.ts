import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';

import { MatDialog, MatDialogRef } from '@angular/material/dialog';

import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MultiSelectModule } from 'primeng/multiselect';

import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Subscription } from 'rxjs';
import { Campo } from '../../types/Campo.type';
import { MapCreateComponent } from '../mapCreate/map-create.component';

@Component({
  selector: 'app-campo-dinamico',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CalendarModule,
    DropdownModule,
    InputTextModule,
    InputTextareaModule,
    MultiSelectModule,
    CheckboxModule,
  ],
  templateUrl: './campoDinamico.component.html',
  styleUrl: './campoDinamico.component.css',
})
export class CampoDinamico implements OnInit {
  @Input() campos: Campo[] = [];
  @Output() formData = new EventEmitter<any>();

  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);

  private formSubscription: Subscription;

  form: FormGroup;
  async ngOnInit() {}

  ngOnChanges(changes: any) {
    this.form = new FormGroup({});

    if (this.formSubscription) {
      this.formSubscription.unsubscribe();
    }

    this.formSubscription = this.form.valueChanges.subscribe((valores) => {
      this.formData.emit(this.form.value);
    });

    this.campos.forEach((campo) => {
      const validators = campo.esObligatorio ? [Validators.required] : [];
      this.form.addControl(
        campo.campo,
        new FormControl(campo?.initValue || null)
      );
    });
  }

  ngOnDestroy() {
    if (this.formSubscription) {
      this.formSubscription.unsubscribe();
    }
  }

  public openModalMapCreate(section: string = '') {
    let mapModalRef = this.matDialog.open(MapCreateComponent, {
      width: '1000px',
      maxWidth: '1000px',
    });

    mapModalRef.componentInstance.section = section;
  }

  getComponente(tipoCampo: string) {
    switch (tipoCampo) {
      case 'Checkbox':
        return 'checkbox';
      case 'Text':
        return 'text';
      case 'Number':
        return 'number';
      case 'Date':
        return 'calendar';
      case 'Datetime':
        return 'calendarHour';
      case 'Select':
        return 'Select';
      case 'GEOMETRY':
        return 'map';
      default:
        return 'text';
    }
  }

  onChangeForm() {
    this.formData.emit(this.form.value);
  }
}
