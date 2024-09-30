import { Component, inject } from '@angular/core';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { FireNationalCreateComponent } from '../fire-national-create/fire-national-create.component';

@Component({
  selector: 'app-fire-foreign-create',
  standalone: true,
  imports: [],
  templateUrl: './fire-foreign-create.component.html',
  styleUrl: './fire-foreign-create.component.css'
})
export class FireForeignCreateComponent {
  matDialogRef = inject(MatDialogRef);
  matDialog = inject(MatDialog);

  closeModal() {
    this.matDialogRef.close();
  }

  onChange(event:any) {
    if (event.target.value == 'Nacional') {
      this.closeModal();
    }

    this.matDialog.open(FireNationalCreateComponent, {
      width: '1300px',
      maxWidth: '1300px',
    });
  }
}
