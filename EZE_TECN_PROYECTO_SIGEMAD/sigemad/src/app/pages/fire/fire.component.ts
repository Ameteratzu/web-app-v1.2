import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';

import { FireService } from '../../services/fire.service';
import { LocalFiltrosIncendio } from '../../services/local-filtro-incendio.service';
import { ApiResponse } from '../../types/api-response.type';
import { Fire } from '../../types/fire.type';
import { FireFilterFormComponent } from './components/fire-filter-form/fire-filter-form.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
// import { FireTableComponent } from './components/fire-table/fire-table.component';

import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { FireCreateEdit } from './components/fire-create-edit-form/fire-create-edit-form.component';

@Component({
  selector: 'app-fire',
  standalone: true,
  imports: [
    CommonModule,
    FireFilterFormComponent,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDialogModule,
  ],
  templateUrl: './fire.component.html',
  styleUrl: './fire.component.scss',
})
export class FireComponent implements OnInit {
  public filtros = signal<any>({});

  public fires = <ApiResponse<Fire[]>>{};

  constructor(
    private matDialog: MatDialog,
    //private matDialogRef: MatDialogRef<FireCreateEdit>,
    private fireService: FireService,
    private filtrosIncendioService: LocalFiltrosIncendio
  ) {}

  async ngOnInit() {
    const fires = await this.fireService.get();
    this.fires = fires;
    console.info('fires', fires.data[0]);
    this.filtros.set(this.filtrosIncendioService.getFilters());
  }

  openModalCrete(idFire: number) {
    const dialogRef = this.matDialog.open(FireCreateEdit, {
      width: '780px',
      maxWidth: '780px',
      data: {
        fire: idFire ? this.fires?.data?.[0] : {},
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('El modal fue cerrado', result);
    });
    /*
    dialogRef.componentInstance.save.subscribe(
      (features: Feature<Geometry>[]) => {
        //this.featuresCoords = features;
      }
    );
    */
  }
}
