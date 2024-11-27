import { Component, inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatChipsModule, MatChipListboxChange } from '@angular/material/chips';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout'; 
import { MatGridListModule } from '@angular/material/grid-list';
import { MatSort } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { AddressComponent } from './address/address.component';
import { CecopiComponent } from './cecopi/cecopi.component';
import { PmaComponent } from './pma/pma.component';
import { trigger, state, style, transition, animate } from '@angular/animations';


@Component({
  selector: 'app-fire-coordination-data',
  standalone: true,
  imports: [
    MatChipsModule,
    CommonModule,
    FlexLayoutModule,
    MatGridListModule,
    MatIconModule,
    NgxSpinnerModule,
    AddressComponent,
    CecopiComponent,
    PmaComponent
  ],
  templateUrl: './fire-coordination-data.component.html',
  styleUrl: './fire-coordination-data.component.scss',
  animations: [
    trigger('fadeInOut', [
      state('void', style({ opacity: 0, transform: 'translateY(20px)' })), 
      transition(':enter', [animate('900ms ease-out')]), 
      transition(':leave', [animate('0ms ease-in')])   
    ])
  ]
})
export class FireCoordinationData {

  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as { title: string; idIncendio: number };

  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  
  readonly sections = [
    { id: 1, label: 'Dirección' },
    { id: 2, label: 'Coordinación CECOPI' },
    { id: 3, label: 'Coordinación PMA' },
  ];

  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  public displayedColumns: string[] = [
    'fechaHora',
    'procendenciaDestino',
    'descripcion',
    'fichero',
    'opciones',
  ]; 

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

}
