import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, Renderer2, signal, ViewChild } from '@angular/core';

import { ActivatedRoute } from '@angular/router';

import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

import moment from 'moment';

import { EventService } from '../../../services/event.service';
import { EventStatusService } from '../../../services/eventStatus.service';
import { FireStatusService } from '../../../services/fire-status.service';
import { FireService } from '../../../services/fire.service';
import { MenuItemActiveService } from '../../../services/menu-item-active.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';

import { Event } from '../../../types/event.type';

import { EventStatus } from '../../../types/eventStatus.type';
import { FireStatus } from '../../../types/fire-status.type';
import { Fire } from '../../../types/fire.type';
import { Municipality } from '../../../types/municipality.type';
import { Province } from '../../../types/province.type';

import { FormFieldComponent } from '../../../shared/Inputs/field.component';
import { FireDetail } from '../../../types/fire-detail.type';
import { FireCoordinationData } from '../../fire-coordination-data/fire-coordination-data.component';
import { FireDocumentation } from '../../fire-documentation/fire-documentation.component';
import { FireCreateComponent } from '../../fire-evolution-create/fire-evolution-create.component';
import { FireOtherInformationComponent } from '../../fire-other-information/fire-other-information.component';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { AlertService } from '../../../shared/alert/alert.service';
import { TooltipDirective } from '../../../shared/directive/tooltip/tooltip.directive';
  
@Component({
  selector: 'app-fire-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    FormFieldComponent,
    MatFormFieldModule,
    MatInputModule,
    MatGridListModule,
    FlexLayoutModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatDialogModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    NgxSpinnerModule,
    MatTooltipModule,
    TooltipDirective
  ],
  providers: [],
  templateUrl: './fire-edit.component.html',
  styleUrl: './fire-edit.component.scss',
})
export class FireEditComponent implements OnInit {
  @ViewChild(MatSort) sort!: MatSort;

  public activedRoute = inject(ActivatedRoute);
  public matDialog = inject(MatDialog);
  public menuItemActiveService = inject(MenuItemActiveService);
  public fireService = inject(FireService);
  public provinceService = inject(ProvinceService);
  public municipalityService = inject(MunicipalityService);
  public eventService = inject(EventService);
  public eventStatusService = inject(EventStatusService);
  public fireStatusService = inject(FireStatusService);
  public route = inject(ActivatedRoute);
  public routenav = inject(Router);
  private spinner = inject(NgxSpinnerService);
  public alertService = inject(AlertService);
  public renderer = inject(Renderer2);

  public fire = <Fire>{};
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public events = signal<Event[]>([]);
  public eventsStatus = signal<EventStatus[]>([]);
  public fireStatus = signal<FireStatus[]>([]);

  public formData!: FormGroup;

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = [
    'numero',
    'fechaHora',
    'registro',
    'origen',
    'tipoRegistro',
    'tecnico',
    'opciones',
  ];

  public fire_id = Number(this.route.snapshot.paramMap.get('id'));

  async ngOnInit() {
    this.menuItemActiveService.set.emit('/fire');
    this.formData = new FormGroup({
      id: new FormControl(),
      denomination: new FormControl({ value: '', disabled: true }),
      territory: new FormControl(),
      province: new FormControl(),
      municipality: new FormControl(),
      startDate: new FormControl({ value: '', disabled: true }),
      event: new FormControl(),
      generalNote: new FormControl(),
      idEstado: new FormControl(),
      ubicacion: new FormControl({ value: '', disabled: true }),
      suceso: new FormControl({ value: '', disabled: true }),
      estado: new FormControl({ value: '', disabled: true }),
    });

    this.dataSource.data = [];

    const fire = await this.fireService.getById(this.fire_id);
    this.fire = fire;

    await this.cargarRegistros();

    this.formData.patchValue({
      id: this.fire.id,
      territory: this.fire.idTerritorio,
      denomination: this.fire.denominacion,
      province: this.fire.idProvincia,
      municipality: this.fire.id,
      startDate: moment(this.fire.fechaInicio).format('YYYY-MM-DD'),
      event: this.fire.idClaseSuceso,
      generalNote: this.fire.notaGeneral,
      idEstado: this.fire.idEstadoSuceso,
      ubicacion: this.fire.ubicacion,
      suceso: this.fire.claseSuceso?.descripcion,
      estado: this.fire.estadoSuceso?.descripcion,
    });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  async cargarRegistros(){
    this.spinner.show();
    const details = await this.fireService.details(Number(this.fire_id));
    this.dataSource.data = details;
    this.spinner.hide();
    return
  }

  async loadMunicipalities(event: any) {
    const province_id = event.target.value;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  goModalEvolution(fireDetail?: FireDetail) {
    const dialogRef = this.matDialog.open(FireCreateComponent, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Datos Evolución',
        idIncendio: Number(this.route.snapshot.paramMap.get('id')),
        fireDetail,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
          this.cargarRegistros();
      }
    });
  }

  goModalCoordination(fireDetail?: FireDetail) {
    const dialogRef = this.matDialog.open(FireCoordinationData, {
      width: '90vw',
      maxWidth: 'none',
      height: '700px',
      disableClose: true,
      data: {
        title: 'Nuevo - Datos de dirección y coordinación de la emergencia',
        idIncendio: Number(this.route.snapshot.paramMap.get('id')),
        fireDetail,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
          this.cargarRegistros();
      }
    });
  }

  goModalOtherInformation(fireDetail?: FireDetail) {
    const dialogRef = this.matDialog.open(FireOtherInformationComponent, {
      width: '90vw',
      maxWidth: 'none',
      //height: '90vh',
      disableClose: true,
      data: {
        title: 'Nuevo - Otra Información',
        fire: this.fire,
        fireDetail,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
      }
    });
  }

  goModalDocumentation(fireDetail?: FireDetail) {
    const dialogRef = this.matDialog.open(FireDocumentation, {
      width: '90vw',
      maxWidth: 'none',
      //height: '90vh',
      disableClose: true,
      data: {
        title: 'Nuevo - Documentación',
        fire: this.fire,
        fireDetail,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
      }
    });
  }

  goModalEdit(fireDetail: FireDetail) {

    const modalActions: { [key: string]: (detail: FireDetail) => void } = {
      'Documentación': this.goModalDocumentation.bind(this),
      'Otra Información': this.goModalOtherInformation.bind(this),
      'Dirección y coordinación': this.goModalCoordination.bind(this),
      'Datos de evolución': this.goModalEvolution.bind(this),
    };
  
    const action = modalActions[fireDetail.tipoRegistro];
    if (action) {
      action(fireDetail);
    }
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YY HH:mm');
  }

  volver(){
    this.routenav.navigate([`/fire`])
  }

  async deleteFire(){
    this.alertService
      .showAlert({
        title: "¿Estás seguro?",
        text: "¡No podrás revertir esto!",
        icon: "warning",
        showCancelButton: true,
        cancelButtonColor: "#d33",
        confirmButtonText: "¡Sí, eliminar!",
      })
      .then(async (result) => {
        if (result.isConfirmed) {
          this.spinner.show();
          const toolbar = document.querySelector('mat-toolbar');
          this.renderer.setStyle(toolbar, 'z-index', '1');

          await this.fireService.delete(this.fire_id);
          setTimeout(() => {
            this.renderer.setStyle(toolbar, 'z-index', '5');
            this.spinner.hide();
            this.alertService.showAlert({
              title: 'Eliminado!',
              icon: 'success',
            }).then((result) => {
              this.routenav.navigate([`/fire`])
            });
          }, 2000);

          
        }else{
          this.spinner.hide();
        }
         
      });

  }
}
