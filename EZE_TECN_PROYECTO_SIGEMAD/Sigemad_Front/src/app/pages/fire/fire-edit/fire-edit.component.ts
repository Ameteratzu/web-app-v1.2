import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output, Renderer2, signal, ViewChild } from '@angular/core';
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
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import moment from 'moment';
import { EventService } from '../../../services/event.service';
import { EventStatusService } from '../../../services/eventStatus.service';
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
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';
import { AlertService } from '../../../shared/alert/alert.service';
import { TooltipDirective } from '../../../shared/directive/tooltip/tooltip.directive';
import { FormFieldComponent } from '../../../shared/Inputs/field.component';
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { ModalConfirmComponent } from '../../../shared/modalConfirm/modalConfirm.component';
import { FireDetail } from '../../../types/fire-detail.type';
import { FireCoordinationData } from '../../fire-coordination-data/fire-coordination-data.component';
import { FireDocumentation } from '../../fire-documentation/fire-documentation.component';
import { FireCreateComponent } from '../../fire-evolution-create/fire-evolution-create.component';
import { FireOtherInformationComponent } from '../../fire-other-information/fire-other-information.component';
import { FireRelatedEventComponent } from '../../fire-related-event/fire-related-event.component';
import { DataSource } from '@angular/cdk/collections';
import { FireActionsRelevantComponent } from '../../fire-actions-relevant/fire-actions-relevant.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FireCreateEdit } from '../components/fire-create-edit-form/fire-create-edit-form.component';
import { FireGraficoHistoricoComponent } from './fire-grafico-historico/fire-grafico-historico/fire-grafico-historico.component';

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
    TooltipDirective,
    FireGraficoHistoricoComponent,
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

  // PCD
  public snackBar = inject(MatSnackBar);
  // FIN PCD

  public provinceService = inject(ProvinceService);
  public municipalityService = inject(MunicipalityService);
  public eventService = inject(EventService);
  public eventStatusService = inject(EventStatusService);
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

  public displayedColumns: string[] = ['numero', 'fechaHora', 'tipoRegistro', 'apartados', 'tecnico', 'opciones'];

  public fire_id = Number(this.route.snapshot.paramMap.get('id'));

  async ngOnInit() {
    await this.cargarIncendio();
  }

  async cargarIncendio() {
    this.menuItemActiveService.set.emit('/fire');
    this.formData = new FormGroup({
      id: new FormControl(),
      denomination: new FormControl({ value: '', disabled: true }),
      territory: new FormControl(),
      province: new FormControl(),
      municipality: new FormControl(),
      startDate: new FormControl({ value: '', disabled: true }),
      event: new FormControl(),
      generalNote: new FormControl({ value: '', disabled: true }),
      idEstado: new FormControl(),
      ubicacion: new FormControl({ value: '', disabled: true }),
      suceso: new FormControl({ value: '', disabled: true }),
      estado: new FormControl({ value: '', disabled: true }),
    });

    this.dataSource.data = [];

    const fire = await this.fireService.getById(this.fire_id);

    this.fire = fire;

    const municipalities = await this.municipalityService.get(this.fire.idProvincia);
    this.municipalities.set(municipalities);

    //await this.cargarRegistros();
    const details = await this.fireService.details(Number(this.fire_id));
    console.log('🚀 ~ FireEditComponent ~ cargarRegistros ~ details:', details);
    this.dataSource.data = details.data;

    this.formData.patchValue({
      id: this.fire.id,
      territory: this.fire.idTerritorio,
      denomination: this.fire.denominacion,
      province: this.fire.idProvincia,
      municipality: this.fire.municipio,
      startDate: moment(this.fire.fechaInicio).format('DD/MM/YYYY HH:mm'),
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

  async cargarRegistros() {
    this.spinner.show();
    const details = await this.fireService.details(Number(this.fire_id));
    console.log('🚀 ~ FireEditComponent ~ cargarRegistros ~ details:', details);
    this.dataSource.data = details.data;

    this.spinner.hide();

    return;
  }

  async loadMunicipalities(event: any) {
    const province_id = event.target.value;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  // PCD
  goModalEditFire() {
    const dialogRef = this.matDialog.open(FireCreateEdit, {
      width: '75vw',
      maxWidth: 'none',
      data: {
        title: 'Modificar - Incendio.',
        fire: this.fire,
      },
    });

    dialogRef.afterClosed().subscribe(async (result) => {
      if (result?.refresh) {
        await this.cargarIncendio();
      }
    });
  }
  // FIN PCD

  goModalRelatedEvent(fireDetail?: FireDetail) {
    const dialogRef = this.matDialog.open(FireRelatedEventComponent, {
      width: '90vw',
      maxWidth: 'none',
      maxHeight: '95vh',
      disableClose: true,
      data: {
        title: fireDetail ? 'Editar - Sucesos Relacionados' : 'Nuevo - Sucesos Relacionados',
        idIncendio: Number(this.route.snapshot.paramMap.get('id')),
        fire: this.fire,
        fireDetail,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.info('close', result);
      if (result) {
        this.cargarRegistros();
      }
    });
  }

  goModalRelevantActions(fireDetail?: FireDetail) {
    console.log('🚀 ~ FireEditComponent ~ goModalRelevantActions ~ fireDetail:', fireDetail);
    const dialogRef = this.matDialog.open(FireActionsRelevantComponent, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: fireDetail ? 'Editar - Actuaciones relevantes de la DGPCE' : 'Nuevo - Actuaciones relevantes de la DGPCE',
        idIncendio: Number(this.route.snapshot.paramMap.get('id')),
        fireDetail,
        fire: this.fire,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.cargarRegistros();
      }
    });
  }

  goModalEvolution(fireDetail?: FireDetail) {
    console.log("🚀 ~ FireEditComponent ~ goModalEvolution ~ fireDetail:", fireDetail)
    const resultado = this.dataSource.data.find((item) => item.esUltimoRegistro && item.tipoRegistro === 'Datos de evolución');

    const dialogRef = this.matDialog.open(FireCreateComponent, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: fireDetail ? 'Editar - Datos Evolución' : 'Nuevo - Datos Evolución',
        idIncendio: Number(this.route.snapshot.paramMap.get('id')),
        fireDetail,
        valoresDefecto: resultado ? resultado.id : null,
        fire: this.fire,
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
        title: fireDetail
          ? 'Editar - Datos de dirección y coordinación de la emergencia'
          : 'Nuevo - Datos de dirección y coordinación de la emergencia',
        idIncendio: Number(this.route.snapshot.paramMap.get('id')),
        fire: this.fire,
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
    console.log("🚀 ~ FireEditComponent ~ goModalOtherInformation ~ fireDetail:", fireDetail)
    const dialogRef = this.matDialog.open(FireOtherInformationComponent, {
      width: '90vw',
      maxWidth: 'none',
      height: '700px',
      disableClose: true,
      data: {
        title: fireDetail ? 'Editar - Otra Información' : 'Nuevo - Otra Información',
        fire: this.fire,
        fireDetail,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        if (result.refresh) {
          this.cargarRegistros();
        }
        console.log('Modal result:', result);
      }
    });
  }

  goModalDocumentation(fireDetail?: FireDetail) {
    const dialogRef = this.matDialog.open(FireDocumentation, {
      width: '90vw',
      maxWidth: 'none',
      height: '700px',
      disableClose: true,
      data: {
        title: fireDetail ? 'Editar - Documentación' : 'Nuevo - Documentación',
        fire: this.fire,
        fireDetail,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        if (result.refresh) {
          this.cargarRegistros();
        }
        console.log('Modal result:', result);
      }
    });
  }

  goModalEdit(fireDetail: FireDetail) {

    const modalActions: { [key: string]: (detail: FireDetail) => void } = {
      // PCD
      Incendio: this.goModalDocumentation.bind(this),
      // FIN PCD

      Documentación: this.goModalDocumentation.bind(this),
      'Otra Información': this.goModalOtherInformation.bind(this),
      'Dirección y Coordinación': this.goModalCoordination.bind(this),
      'Datos de evolución': this.goModalEvolution.bind(this),
      'Sucesos Relacionados': this.goModalRelatedEvent.bind(this),
      'Actuaciones Relevantes': this.goModalRelevantActions.bind(this),
    };

    const action = modalActions[fireDetail.tipoRegistro?.nombre];
    console.log("🚀 ~ FireEditComponent ~ goModalEdit ~ action:", action)
    if (action) {
      console.log("🚀 ~ FireEditComponent ~ goModalEdit ~ fireDetail:", fireDetail)
      action(fireDetail);
    }
  }

  getFormatdate(date: any) {
    return moment(date).format('DD/MM/YY HH:mm');
  }

  volver() {
    this.routenav.navigate([`/fire`]);
  }

  async deleteFire() {
    this.alertService
      /*
      .showAlert({
        title: '¿Estás seguro?',
        text: '¡No podrás revertir esto!',
        icon: 'warning',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
      })
      */

      // PCD
      .showAlert({
        title: '¿Estás seguro de eliminar el registro?',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
        cancelButtonText: 'Cancelar',
        customClass: {
          title: 'sweetAlert-fsize20',
        },
      })
      // FIN PCD

      .then(async (result) => {
        if (result.isConfirmed) {
          this.spinner.show();
          const toolbar = document.querySelector('mat-toolbar');
          this.renderer.setStyle(toolbar, 'z-index', '1');

          await this.fireService.delete(this.fire_id);
          setTimeout(() => {
            //this.renderer.setStyle(toolbar, 'z-index', '5');
            //this.spinner.hide();

            /*
            this.alertService
              .showAlert({
                title: 'Eliminado!',
                icon: 'success',
              })
              .then((result) => {
                this.routenav.navigate(['/fire']).then(() => {
                  window.location.href = '/fire';
                });
              });
              */

            //PCD
            this.snackBar
              .open('Datos eliminados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.routenav.navigate(['/fire']).then(() => {
                  window.location.href = '/fire';
                });
                this.spinner.hide();
              });
            // FIN PCD
          }, 2000);
        } else {
          this.spinner.hide();
        }
      });
  }

  openModalMap() {
    if (!this.formData.value.municipality) {
      return;
    }

    const municipioSelected = this.municipalities().find((item) => item.id == this.formData.value.municipality.id);

    if (!municipioSelected) {
      return;
    }

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      //height: '780px',
      //maxHeight: '780px',
      data: {
        municipio: municipioSelected,
        listaMunicipios: this.municipalities(),
        defaultGeometry: this.fire.geoPosicion.coordinates[0],
        onlyView: true,
      },
    });

    dialogRef.componentInstance.save.subscribe((features: Feature<Geometry>[]) => {
      //this.polygon.set(features);
    });
  }

  goModalConfirm(): void {
    this.matDialog.open(ModalConfirmComponent, {
      width: '30vw',
      maxWidth: 'none',
      //height: '90vh',
      disableClose: true,
      data: {
        fireId: this.fire.id,
      },
    });
  }

  async refrescarDatos() {
    this.spinner.show();

    await this.delay(1000);
    await this.cargarIncendio();
    this.spinner.hide();
  }

  delay(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}
