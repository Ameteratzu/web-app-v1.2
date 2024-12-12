import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { FireService } from '../../services/fire.service';
import { LocalFiltrosIncendio } from '../../services/local-filtro-incendio.service';
import { ApiResponse } from '../../types/api-response.type';
import { Fire } from '../../types/fire.type';
import { FireFilterFormComponent } from './components/fire-filter-form/fire-filter-form.component';
import { FireTableComponent } from './components/fire-table/fire-table.component';

@Component({
  selector: 'app-fire',
  standalone: true,
  imports: [CommonModule, FireFilterFormComponent, FireTableComponent, RouterOutlet],
  templateUrl: './fire.component.html',
  styleUrl: './fire.component.scss',
})
export class FireComponent implements OnInit {
  public filtros = signal<any>({});

  public isLoading = true
  public refreshFilterForm = true

  public fires: ApiResponse<Fire[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  public fireService = inject(FireService);
  public filtrosIncendioService = inject(LocalFiltrosIncendio);

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosIncendioService.getFilters());
  }
}
