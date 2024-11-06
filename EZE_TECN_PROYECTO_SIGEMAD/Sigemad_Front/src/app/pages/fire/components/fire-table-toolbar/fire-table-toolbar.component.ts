import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  Input,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ApiResponse } from '../../../../types/api-response.type';
import { Fire } from '../../../../types/fire.type';
import { FireCreateModalComponent } from '../fire-create-modal/fire-create-modal.component';

@Component({
  selector: 'app-fire-table-toolbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './fire-table-toolbar.component.html',
  styleUrl: './fire-table-toolbar.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FireTableToolbarComponent {
  @Input() fires: ApiResponse<Fire[]>;
  public matDialog = inject(MatDialog);

  openModal() {
    this.matDialog.open(FireCreateModalComponent, {
      minWidth: '940px',
      disableClose: true,
      panelClass: 'custom-dialog-container',
    });
  }
}
