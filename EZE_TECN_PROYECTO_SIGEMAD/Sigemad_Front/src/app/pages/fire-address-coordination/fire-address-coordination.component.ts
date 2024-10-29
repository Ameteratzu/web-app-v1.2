import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MapCreateComponent } from '../map-create/map-create.component';

@Component({
  selector: 'app-fire-address-coordination',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './fire-address-coordination.component.html',
  styleUrl: './fire-address-coordination.component.css',
})
export class FireAddressCoordinationComponent {
  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);

  public formGroup: FormGroup;

  async ngOnInit() {
    this.formGroup = new FormGroup({
      address: new FormControl(),
      directing_authority: new FormControl(),
      address_start: new FormControl(),
      address_end: new FormControl(),

      cecopi_start: new FormControl(),
      cecopi_end: new FormControl(),
      cecopi_place: new FormControl(),
      cecopi_province: new FormControl(),
      cecopi_municipality: new FormControl(),
      cecopi_observations: new FormControl(),

      pma_start: new FormControl(),
      pma_end: new FormControl(),
      pma_place: new FormControl(),
      pma_province: new FormControl(),
      pma_municipality: new FormControl(),
      pma_observations: new FormControl(),

      plan_type: new FormControl(),
      plan_name: new FormControl(),
      activating_authority: new FormControl(),
      activating_start: new FormControl(),
      activating_end: new FormControl(),
      activating_observations: new FormControl(),
    });
  }

  public closeModal() {
    this.matDialogRef.close();
  }

  public submit() {
    const data = this.formGroup.value;
    console.log(data);
  }

  public openModalMapCreate(section: string = '') {
    let mapModalRef = this.matDialog.open(MapCreateComponent, {
      width: '1000px',
      maxWidth: '1000px',
    });

    mapModalRef.componentInstance.section = section;
  }
}
