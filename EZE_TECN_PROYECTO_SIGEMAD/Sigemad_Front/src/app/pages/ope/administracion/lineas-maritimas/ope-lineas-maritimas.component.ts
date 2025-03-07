import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OpeLineasMaritimasTableComponent } from './components/ope-linea-maritima-table/ope-lineas-maritimas-table.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { OpeLineaMaritimaFilterFormComponent } from './components/ope-linea-maritima-filter-form/ope-lineas-maritimas-filter-form.component';
import { LocalFiltrosOpeLineasMaritimas } from '../../../../services/ope/administracion/local-filtro-ope-lineas-maritimas.service';
import { OpeLineaMaritima } from '../../../../types/ope/administracion/ope-linea-maritima.type';
import { OpeLineasMaritimasService } from '../../../../services/ope/administracion/ope-lineas-maritimas.service';

@Component({
  selector: 'app-ope-lineasMaritimas',
  standalone: true,
  imports: [CommonModule, RouterOutlet, OpeLineasMaritimasTableComponent, OpeLineaMaritimaFilterFormComponent],
  templateUrl: './ope-lineas-maritimas.component.html',
  styleUrl: './ope-lineas-maritimas.component.scss',
})
export class OpeLineasMaritimasComponent {
  public opeLineasMaritimasService = inject(OpeLineasMaritimasService);
  public filtrosOpeLineasMaritimasService = inject(LocalFiltrosOpeLineasMaritimas);

  public filtros = signal<any>({});

  public isLoading = true;
  public refreshFilterForm = true;
  public childActivated = false;

  public opeLineasMaritimas: ApiResponse<OpeLineaMaritima[]> = {
    count: 0,
    page: 1,
    pageSize: 10,
    data: [],
    pageCount: 0,
  };

  async ngOnInit() {
    //const fires = await this.fireService.get();
    //this.fires = fires;
    this.filtros.set(this.filtrosOpeLineasMaritimasService.getFilters());
  }
}
