import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, inject, Renderer2, ViewChild } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatChipListboxChange, MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CoordinationAddressService } from '../../services/coordination-address.service';
import { FireDetail } from '../../types/fire-detail.type';
import { Fire } from '../../types/fire.type';
import { AddressComponent } from './address/address.component';
import { CecopiComponent } from './cecopi/cecopi.component';
import { PmaComponent } from './pma/pma.component';

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
    PmaComponent,
  ],
  templateUrl: './fire-coordination-data.component.html',
  styleUrl: './fire-coordination-data.component.scss',
  animations: [
    trigger('fadeInOut', [
      state('void', style({ opacity: 0, transform: 'translateY(20px)' })),
      transition(':enter', [animate('900ms ease-out')]),
      transition(':leave', [animate('0ms ease-in')]),
    ]),
  ],
})
export class FireCoordinationData {
  @ViewChild(MatSort) sort!: MatSort;
  data = inject(MAT_DIALOG_DATA) as {
    title: string;
    idIncendio: number;
    fire: Fire;
    fireDetail: FireDetail;
  };

  public matDialog = inject(MatDialog);
  private spinner = inject(NgxSpinnerService);
  public coordinationServices = inject(CoordinationAddressService);
  public toast = inject(MatSnackBar);
  public renderer = inject(Renderer2);

  readonly sections = [
    { id: 1, label: 'Dirección' },
    { id: 2, label: 'Coordinación CECOPI' },
    { id: 3, label: 'Coordinación PMA' },
  ];

  selectedOption: MatChipListboxChange = { source: null as any, value: 1 };

  public displayedColumns: string[] = ['fechaHora', 'procendenciaDestino', 'descripcion', 'fichero', 'opciones'];

  editDataDir: any;
  editDataCecopi: any;
  editDataPma: any;
  isDataReady = false;
  idReturn = null;

  async isToEditDocumentation() {
    if (!this.data?.fireDetail?.id) {
      this.isDataReady = true;
      return;
    }
    const dataOtraInformacion: any = await this.coordinationServices.getById(Number(this.data.fireDetail.id));

    this.editDataDir = dataOtraInformacion.direcciones;
    this.editDataCecopi = dataOtraInformacion.coordinacionesCecopi;
    this.editDataPma = dataOtraInformacion.coordinacionesPMA;
    this.isDataReady = true;
  }

  async ngOnInit() {
    this.isToEditDocumentation();
  }

  onSelectionChange(event: MatChipListboxChange): void {
    this.selectedOption = event;
  }

  async onSaveFromChild(value: boolean) {
    this.spinner.show();
    const toolbar = document.querySelector('mat-toolbar');
    this.renderer.setStyle(toolbar, 'z-index', '1');
    console.info('value', value);
    if (value) {
      const x = await this.processData();
      console.log(x);

      this.coordinationServices.clearData();
      this.closeModal();

      this.showToast();
      setTimeout(() => {
        window.location.href = `fire-national-edit/${this.data?.idIncendio ?? 1}`;
        this.renderer.setStyle(toolbar, 'z-index', '5');
        this.spinner.hide();
      }, 2000);
    } else {
      this.spinner.hide();
      this.coordinationServices.clearData();
      this.closeModal();
    }
  }

  async processData(): Promise<void> {
    await this.handleDataProcessing(
      this.coordinationServices.dataCoordinationAddress(),
      (item) => ({
        id: item.id ? item.id : 0,
        idTipoDireccionEmergencia: Number(item.tipoDireccionEmergencia.id),
        autoridadQueDirige: item.autoridadQueDirige,
        fechaInicio: this.formatDate(item.fechaInicio),
        fechaFin: item.fechaFin ? this.formatDate(item.fechaFin) : '',
      }),
      this.coordinationServices.postAddress.bind(this.coordinationServices),
      'direcciones'
    );

    await this.handleDataProcessing(
      this.coordinationServices.dataCecopi(),
      (item) => {
        console.info('item', item);
        return {
          id: item.id ? item.id : 0,
          idProvincia: Number(item.provincia.id),
          idMunicipio: Number(item.municipio.id),
          fechaInicio: this.formatDate(item.fechaInicio),
          lugar: String(item.lugar),
          fechaFin: item.fechaFin ? this.formatDate(item.fechaFin) : '',
          GeoPosicion: item.geoPosicion,
          observaciones: item.observaciones,
        };
      },
      this.coordinationServices.postCecopi.bind(this.coordinationServices),
      'coordinaciones'
    );

    await this.handleDataProcessing(
      this.coordinationServices.dataPma(),
      (item) => ({
        id: item.id ? item.id : 0,
        idProvincia: Number(item.provincia.id),
        idMunicipio: Number(item.municipio.id),
        fechaInicio: this.formatDate(item.fechaInicio),
        lugar: String(item.lugar),
        fechaFin: item.fechaFin ? this.formatDate(item.fechaFin) : '',
        GeoPosicion: item.geoPosicion,
        observaciones: item.observaciones,
      }),
      this.coordinationServices.postPma.bind(this.coordinationServices),
      'coordinaciones'
    );
  }

  async handleDataProcessing<T>(data: T[], formatter: (item: T) => any, postService: (body: any) => Promise<any>, key: string): Promise<void> {
    if (data.length > 0) {
      const formattedData = data.map(formatter);

      const body = {
        idIncendio: this.data.idIncendio,
        idDireccionCoordinacionEmergencia: this.data?.fireDetail?.id ? this.data?.fireDetail?.id : this.idReturn,
        [key]: formattedData,
      };
      console.info('body', body);
      const result = await postService(body);
      this.idReturn = result.idDireccionCoordinacionEmergencia;
    }
  }

  formatDate(date: Date | string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  trackByFn(index: number, item: any): number {
    return item.id;
  }

  closeModal() {
    this.matDialog.closeAll();
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
}
