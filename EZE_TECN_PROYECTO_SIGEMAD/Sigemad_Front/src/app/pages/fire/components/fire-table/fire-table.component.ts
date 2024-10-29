import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { ApiResponse } from '../../../../types/api-response.type';
import { Fire } from '../../../../types/fire.type';
import { FireTableToolbarComponent } from '../fire-table-toolbar/fire-table-toolbar.component';

@Component({
  selector: 'app-fire-table',
  standalone: true,
  imports: [CommonModule, FireTableToolbarComponent],
  templateUrl: './fire-table.component.html',
  styleUrl: './fire-table.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FireTableComponent {
  @Input() fires: ApiResponse<Fire[]>;
}
