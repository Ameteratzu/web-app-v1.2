import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';

import { FireService } from '../../services/fire.service';
import { ApiResponse } from '../../types/api-response.type';
import { Fire } from '../../types/fire.type';
import { FireFilterFormComponent } from './components/fire-filter-form/fire-filter-form.component';
import { FireTableComponent } from './components/fire-table/fire-table.component';

@Component({
  selector: 'app-fire',
  standalone: true,
  imports: [CommonModule, FireFilterFormComponent, FireTableComponent],
  templateUrl: './fire.component.html',
  styleUrl: './fire.component.css',
})
export class FireComponent implements OnInit {
  public fires = <ApiResponse<Fire[]>>{};

  public fireService = inject(FireService);

  async ngOnInit() {
    const fires = await this.fireService.get();
    this.fires = fires;
  }
}
