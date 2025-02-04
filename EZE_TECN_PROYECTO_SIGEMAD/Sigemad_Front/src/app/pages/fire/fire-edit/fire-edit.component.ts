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
import { EvolutionService } from '../../../services/evolution.service';
import { Evolution } from '../../../types/evolution.type';
import { FireActionsRelevantComponent } from '../../fire-actions-relevant/fire-actions-relevant.component';

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
  public evolutionService = inject(EvolutionService);
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

    const municipalities = await this.municipalityService.get(this.fire.idProvincia);
    this.municipalities.set(municipalities);

    await this.cargarRegistros();

    this.formData.patchValue({
      id: this.fire.id,
      territory: this.fire.idTerritorio,
      denomination: this.fire.denominacion,
      province: this.fire.idProvincia,
      municipality: this.fire.municipio,
      startDate: moment(this.fire.fechaInicio).format('DD/MM/YYYY'),
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
    this.dataSource.data = details;
    this.spinner.hide();

    // PCD
    this.listadoEvoluciones = [];

    if (this.dataSource.data != null && this.dataSource.data.length > 0) {
      for (const actualizacion of this.dataSource.data) {
        if (actualizacion.tipoRegistro === 'Datos de evolución') {
          const evolucion: any = await this.evolutionService.getById(actualizacion.id);
          if (evolucion != null) {
            this.listadoEvoluciones.push(evolucion);
          }
        }
      }
    }

    this.cargarFilaEvolucion();
    this.cargarNivelSituacionOperativaEquivalente();
    this.cargarFilaAfectaciones();
    this.cargarFilaMediosExtincionOrdinarios();
    this.cargarFilaEMediosExtincionExtraordinariosNacionales();
    this.cargarFilaEMediosExtincionExtraordinariosInternacionales();

    // Obtenemos el rango completo de fechas
    const menorFechaHistorico = this.obtenerMenorFechaHistorico([
      this.filaEvolucion,
      this.filaNivelSituacionOperativaEquivalente,
      this.filaAfectaciones,
      this.filaMediosExtincionOrdinarios,
      this.filaMediosExtincionExtraordinariosNacionales,
      this.filaMediosExtincionExtraordinariosInternacionales,
    ]);

    const mayorFechaHistorico = this.obtenerMayorFechaHistorico([
      this.filaEvolucion,
      this.filaNivelSituacionOperativaEquivalente,
      this.filaAfectaciones,
      this.filaMediosExtincionOrdinarios,
      this.filaMediosExtincionExtraordinariosNacionales,
      this.filaMediosExtincionExtraordinariosInternacionales,
    ]);

    if (menorFechaHistorico && mayorFechaHistorico) {
      this.cargarFilaDias(menorFechaHistorico, mayorFechaHistorico);
    } else {
      console.error('Error: No se encontraron fechas válidas.');
    }

    // FIN PCD
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
    const dialogRef = this.matDialog.open(FireOtherInformationComponent, {
      width: '90vw',
      maxWidth: 'none',
      //height: '90vh',
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
      //height: '90vh',
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
      Documentación: this.goModalDocumentation.bind(this),
      'Otra Información': this.goModalOtherInformation.bind(this),
      'Dirección y coordinación': this.goModalCoordination.bind(this),
      'Datos de evolución': this.goModalEvolution.bind(this),
      'Sucesos Relacionados': this.goModalRelatedEvent.bind(this),
      'Actuaciones Relevantes': this.goModalRelevantActions.bind(this),
    };

    const action = modalActions[fireDetail.tipoRegistro];
    if (action) {
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
      .showAlert({
        title: '¿Estás seguro?',
        text: '¡No podrás revertir esto!',
        icon: 'warning',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
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
        defaultPolygon: this.fire.geoPosicion.coordinates[0],
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

  // PCD
  // Mapas para las filas del gráfico
  listadoEvoluciones: any[] = [];
  filaDias: Map<string, { hora: string; estado: string }[]> = new Map();
  filaEvolucion: Map<string, { hora: string; estado: string }[]> = new Map();
  filaNivelSituacionOperativaEquivalente: Map<string, { hora: string; estado: string }[]> = new Map();
  filaAfectaciones: Map<string, { hora: string; estado: string }[]> = new Map();
  filaMediosExtincionOrdinarios: Map<string, { hora: string; estado: string }[]> = new Map();
  filaMediosExtincionExtraordinariosNacionales: Map<string, { hora: string; estado: string }[]> = new Map();
  filaMediosExtincionExtraordinariosInternacionales: Map<string, { hora: string; estado: string }[]> = new Map();

  // Fila Dias
  async cargarFilaDias(menorFechaHistorico: string, mayorFechaHistorico: string) {
    if (!menorFechaHistorico.trim() || !mayorFechaHistorico.trim()) {
      return;
    }

    this.filaDias.clear();

    const [d1, m1, y1] = menorFechaHistorico.split('-').map(Number);
    const [d2, m2, y2] = mayorFechaHistorico.split('-').map(Number);

    const fechaInicio = new Date(y1, m1 - 1, d1);
    const fechaFin = new Date(y2, m2 - 1, d2);

    for (let fecha = new Date(fechaInicio); fecha <= fechaFin; fecha.setDate(fecha.getDate() + 1)) {
      const day = String(fecha.getDate()).padStart(2, '0');
      const month = String(fecha.getMonth() + 1).padStart(2, '0');
      const year = fecha.getFullYear();

      const fechaFormateada = `${day}-${month}-${year}`;

      if (!this.filaDias.has(fechaFormateada)) {
        this.filaDias.set(fechaFormateada, []);
      }
    }

    // Convertimos las claves del Map a un array para el *ngFor
    this.filaDiasArray = Array.from(this.filaDias.keys());
  }

  filaDiasArray: string[] = [];

  getDiasContinuos(): string[] {
    const fechas = this.listadoEvoluciones.map((evolucion: any) => this.fechaADDMMYY(evolucion.datoPrincipal?.fechaHora)); // Suponiendo que cada evolución tiene una propiedad 'fecha'

    if (fechas.length === 0) {
      return [];
    }

    const diasContinuos: string[] = [];
    const diasContinuos2: string[] = [];
    const [dayInicio, monthInicio, yearInicio] = fechas[0].split('-').map(Number);
    const [dayFin, monthFin, yearFin] = fechas[fechas.length - 1].split('-').map(Number);

    // No restamos 1 al mes, para que el primer día sea el correcto
    const fechaInicio = new Date(yearInicio, monthInicio - 1, dayInicio);
    const fechaFin = new Date(yearFin, monthFin - 1, dayFin);

    // Iterar desde fechaInicio hasta fechaFin
    for (let fecha = new Date(fechaInicio); fecha <= fechaFin; fecha.setDate(fecha.getDate() + 1)) {
      const day = String(fecha.getDate()).padStart(2, '0'); // Asegura que el día tenga dos dígitos
      const month = String(fecha.getMonth() + 1).padStart(2, '0'); // Asegura que el mes tenga dos dígitos
      const year = fecha.getFullYear();

      const fechaFormateadaFinal = `${day}-${month}-${year}`; // Formato final DD-MM-YYYY

      // Solo agregar la fecha si no está en filaEvolucion
      if (!this.filaEvolucion.has(fechaFormateadaFinal)) {
        this.filaEvolucion.set(fechaFormateadaFinal, []); // Añadir la fecha como entrada vacía
      }

      diasContinuos.push(fechaFormateadaFinal);
    }

    return diasContinuos;
  }
  // Fin Dias

  /********************* */
  /* FILA EVOLUCION  */
  /********************* */
  async cargarFilaEvolucion() {
    this.filaEvolucion.clear();

    /*
    const listadoEvoluciones = [];
    const listadoActualizaciones = this.dataSource.data;

    if (listadoActualizaciones != null && listadoActualizaciones.length > 0) {
      for (const actualizacion of listadoActualizaciones) {
        if (actualizacion.tipoRegistro === 'Datos de evolución') {
          const evolucion: any = await this.evolutionService.getById(actualizacion.id);
          if (evolucion != null) {
            listadoEvoluciones.push(evolucion);
          }
        }
      }
    }
      */

    if (this.listadoEvoluciones != null && this.listadoEvoluciones.length > 0) {
      this.listadoEvoluciones.sort((a, b) => {
        const fechaA = new Date(a.datoPrincipal?.fechaHora);
        const fechaB = new Date(b.datoPrincipal?.fechaHora);
        return fechaA.getTime() - fechaB.getTime();
      });
      for (const evolucion of this.listadoEvoluciones) {
        const fechaEvolucion: string = this.fechaADDMMYY(evolucion.datoPrincipal?.fechaHora);
        const horaEvolucion: string = this.fechaAHHMM(evolucion.datoPrincipal?.fechaHora);
        const estadoEvolucion: string = evolucion.parametro.estadoIncendio?.descripcion;
        if (estadoEvolucion) {
          this.actualizarFilaEvolucion(fechaEvolucion, horaEvolucion, estadoEvolucion);
        }
      }
    }
  }

  actualizarFilaEvolucion(fechaEvolucion: string, horaEvolucion: string, estadoEvolucion: string) {
    // Verificar si ya existe la fecha en el mapa
    if (this.filaEvolucion.has(fechaEvolucion)) {
      // Si ya existe, obtener el array de entradas para esa fecha
      const entradas = this.filaEvolucion.get(fechaEvolucion) || []; // Si es undefined, se asigna un array vacío

      // Añadir un nuevo estado a las entradas
      entradas.push({ hora: horaEvolucion, estado: estadoEvolucion });

      // Actualizar la entrada en el mapa con el nuevo valor
      this.filaEvolucion.set(fechaEvolucion, entradas);
    } else {
      // Si no existe la fecha, crear una nueva entrada
      this.filaEvolucion.set(fechaEvolucion, [{ hora: horaEvolucion, estado: estadoEvolucion }]);
    }
  }

  /*
  getBackgroundColorFilaEvolucion(fecha: string, horaIndex: number): string {
    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaEvolucion.keys()).sort((a, b) => new Date(a).getTime() - new Date(b).getTime());

    // Obtener todas las entradas del día dado
    const entradas = this.filaEvolucion.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = this.getColorPorEstadoFilaEvolucion(entrada.estado);
          return color;
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaEvolucion.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        const ultimaEntrada = entradasAnteriores[entradasAnteriores.length - 1];
        color = this.getColorPorEstadoFilaEvolucion(ultimaEntrada.estado);
        break;
      }
    }

    return color;
  }
    */

  // test
  getBackgroundColorFilaEvolucion(fecha: string, horaIndex: number): string {
    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaEvolucionDiasCompletos.keys()).sort((a, b) => new Date(a).getTime() - new Date(b).getTime());

    // Obtener todas las entradas del día dado
    const entradas = this.filaEvolucionDiasCompletos.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = this.getColorPorEstadoFilaEvolucion(entrada.estado);
          return color;
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaEvolucionDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        const ultimaEntrada = entradasAnteriores[entradasAnteriores.length - 1];
        color = this.getColorPorEstadoFilaEvolucion(ultimaEntrada.estado);
        break;
      }
    }

    return color;
  }

  // test

  getColorPorEstadoFilaEvolucion(estado: string): string {
    estado = estado.toLowerCase();
    switch (estado) {
      case 'activo': // activo
        return 'red';
      case 'estabilizado':
        return '#FF8000';
      case 'controlado':
        return '#F9D705';
      case 'extinguido':
        return 'green';
      default:
        return 'transparent';
    }
  }

  get filaEvolucionDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      // Si la fecha existe en filaNivelSituacionOperativaEquivalente, la usamos
      if (this.filaEvolucion.has(fecha)) {
        completo.set(fecha, this.filaEvolucion.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }

  getEstadoFilaEvolucion(fecha: string, horaIndex: number): string | null {
    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaEvolucionDiasCompletos.keys()).sort((a, b) => new Date(a).getTime() - new Date(b).getTime());

    // Obtener todas las entradas del día dado
    const entradas = this.filaEvolucionDiasCompletos.get(fecha);

    if (entradas) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el estado correspondiente
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el `horaIndex` está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          return entrada.estado;
        }
      }
    }

    // Si no se encontró un estado en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaEvolucionDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        return entradasAnteriores[entradasAnteriores.length - 1].estado;
      }
    }

    return null; // Si no hay estado disponible
  }

  /********************* */
  /* FIN FILA EVOLUCION  */
  /********************* */

  /********************************************* */
  /*   Fila NivelSituacionOperativaEquivalente   */
  /********************************************* */
  async cargarNivelSituacionOperativaEquivalente() {
    this.filaNivelSituacionOperativaEquivalente.clear();

    if (this.listadoEvoluciones.length > 0) {
      this.listadoEvoluciones.sort((a, b) => {
        const fechaA = new Date(a.datoPrincipal?.fechaHora);
        const fechaB = new Date(b.datoPrincipal?.fechaHora);
        return fechaA.getTime() - fechaB.getTime();
      });
      for (const evolucion of this.listadoEvoluciones) {
        const fechaEvolucion: string = this.fechaADDMMYY(evolucion.datoPrincipal?.fechaHora);
        const horaEvolucion: string = this.fechaAHHMM(evolucion.datoPrincipal?.fechaHora);
        const nivelSituacionOperativaEquivalenteEvolucion: string = evolucion.parametro?.situacionEquivalente?.descripcion;

        if (nivelSituacionOperativaEquivalenteEvolucion) {
          this.actualizarFilaSituacionOperativaEquivalente(fechaEvolucion, horaEvolucion, nivelSituacionOperativaEquivalenteEvolucion);
        }
      }
    }
  }

  actualizarFilaSituacionOperativaEquivalente(fechaEvolucion: string, horaEvolucion: string, nivelSituacionOperativaEquivalenteEvolucion: string) {
    // Verificar si ya existe la fecha en el mapa
    if (this.filaNivelSituacionOperativaEquivalente.has(fechaEvolucion)) {
      // Si ya existe, obtener el array de entradas para esa fecha
      const entradas = this.filaNivelSituacionOperativaEquivalente.get(fechaEvolucion) || []; // Si es undefined, se asigna un array vacío

      // Añadir un nuevo estado a las entradas
      entradas.push({ hora: horaEvolucion, estado: nivelSituacionOperativaEquivalenteEvolucion });

      // Actualizar la entrada en el mapa con el nuevo valor
      this.filaNivelSituacionOperativaEquivalente.set(fechaEvolucion, entradas);
    } else {
      // Si no existe la fecha, crear una nueva entrada
      this.filaNivelSituacionOperativaEquivalente.set(fechaEvolucion, [{ hora: horaEvolucion, estado: nivelSituacionOperativaEquivalenteEvolucion }]);
    }
  }

  /*
  getBackgroundColorFilaNivelSituacionOperativaEquivalente(fecha: string, horaIndex: number): string {
    console.log(fecha + ' ' + horaIndex);

    // Obtener todas las fechas ordenadas

    const fechas = Array.from(this.filaNivelSituacionOperativaEquivalente.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Asegurar continuidad temporal agregando días intermedios
    const [dayInicio, monthInicio, yearInicio] = fechas[0].split('-').map(Number);
    const [dayFin, monthFin, yearFin] = fechas[fechas.length - 1].split('-').map(Number);

    const fechaInicio = new Date(yearInicio, monthInicio - 1, dayInicio);
    const fechaFin = new Date(yearFin, monthFin - 1, dayFin);

    for (let fechaIter = new Date(fechaInicio); fechaIter <= fechaFin; fechaIter.setDate(fechaIter.getDate() + 1)) {
      const dia = String(fechaIter.getDate()).padStart(2, '0');
      const mes = String(fechaIter.getMonth() + 1).padStart(2, '0');
      const anio = fechaIter.getFullYear();
      const fechaFormateada = `${dia}-${mes}-${anio}`;

      if (!this.filaNivelSituacionOperativaEquivalente.has(fechaFormateada)) {
        this.filaNivelSituacionOperativaEquivalente.set(fechaFormateada, []); // Añadir día sin entradas
      }
    }

    // Obtener todas las entradas del día dado
    const entradas = this.filaNivelSituacionOperativaEquivalente.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = this.getColorPorEstadoFilaNivelSituacionOperativaEquivalente(entrada.estado);
          return color;
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaNivelSituacionOperativaEquivalente.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        const ultimaEntrada = entradasAnteriores[entradasAnteriores.length - 1];
        color = this.getColorPorEstadoFilaNivelSituacionOperativaEquivalente(ultimaEntrada.estado);
        break;
      }
    }

    return color;
  }
    */

  // test
  getBackgroundColorFilaNivelSituacionOperativaEquivalente(fecha: string, horaIndex: number): string {

    // Usamos filaNivelSituacionOperativaEquivalenteDiasCompletos para garantizar que todas las fechas estén
    const fechas = Array.from(this.filaNivelSituacionOperativaEquivalenteDiasCompletos.keys());

    // Obtener todas las entradas del día dado
    const entradas = this.filaNivelSituacionOperativaEquivalenteDiasCompletos.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas del día por hora
      entradas.sort((a, b) => parseInt(a.hora.split(':')[0], 10) - parseInt(b.hora.split(':')[0], 10));

      // Buscar el color según el `horaIndex`
      for (let i = 0; i < entradas.length; i++) {
        const entrada = entradas[i];
        const horaSolo = parseInt(entrada.hora.split(':')[0], 10);

        if (horaSolo <= horaIndex && (i === entradas.length - 1 || horaIndex < parseInt(entradas[i + 1].hora.split(':')[0], 10))) {
          return this.getColorPorEstadoFilaNivelSituacionOperativaEquivalente(entrada.estado);
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaNivelSituacionOperativaEquivalenteDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        return this.getColorPorEstadoFilaNivelSituacionOperativaEquivalente(entradasAnteriores[entradasAnteriores.length - 1].estado);
      }
    }

    return color;
  }

  // test

  getColorPorEstadoFilaNivelSituacionOperativaEquivalente(estado: string): string {
    switch (estado) {
      case '0':
        return 'lightgreen';
      case '1':
        return 'yellow';
      case '2':
        return 'orange';
      case '3':
        return 'brown';
      default:
        return 'transparent';
    }
  }

  get filaNivelSituacionOperativaEquivalenteDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      // Si la fecha existe en filaNivelSituacionOperativaEquivalente, la usamos
      if (this.filaNivelSituacionOperativaEquivalente.has(fecha)) {
        completo.set(fecha, this.filaNivelSituacionOperativaEquivalente.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }
  /********************************************* */
  /* FIN Fila NivelSituacionOperativaEquivalente */
  /********************************************* */

  /*************************/
  /* Fin Fila Afectaciones */
  /*************************/
  cargarFilaAfectaciones() {
    this.filaAfectaciones.set('29-01-2025', [
      { hora: '02:15', estado: 'PERSONAS' },
      { hora: '16:20', estado: 'CARRETERAS' },
    ]);
    this.filaAfectaciones.set('28-01-2025', [{ hora: '12:15', estado: 'MEDIOAMBIENTE' }]);
  }

  /*
  getBackgroundIconFilaAfectaciones(fecha: string, horaIndex: number): string {
    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaAfectaciones.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Asegurar continuidad temporal agregando días intermedios
    const [dayInicio, monthInicio, yearInicio] = fechas[0].split('-').map(Number);
    const [dayFin, monthFin, yearFin] = fechas[fechas.length - 1].split('-').map(Number);

    const fechaInicio = new Date(yearInicio, monthInicio - 1, dayInicio);
    const fechaFin = new Date(yearFin, monthFin - 1, dayFin);

    for (let fechaIter = new Date(fechaInicio); fechaIter <= fechaFin; fechaIter.setDate(fechaIter.getDate() + 1)) {
      const dia = String(fechaIter.getDate()).padStart(2, '0');
      const mes = String(fechaIter.getMonth() + 1).padStart(2, '0');
      const anio = fechaIter.getFullYear();
      const fechaFormateada = `${dia}-${mes}-${anio}`;

      if (!this.filaAfectaciones.has(fechaFormateada)) {
        this.filaAfectaciones.set(fechaFormateada, []); // Añadir día sin entradas
      }
    }

    // Obtener todas las entradas del día dado
    const entradas = this.filaAfectaciones.get(fecha);

    let iconUrl = ''; // Valor predeterminado (sin icono)

    if (entradas && entradas.length > 0) {
      // Buscar una coincidencia exacta de hora
      for (const entrada of entradas) {
        const [horaExacta] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        if (horaExacta === horaIndex) {
          iconUrl = this.getIconUrlPorEstado(entrada.estado);
          return `url('${iconUrl}')`;
        }
      }
    }

    // Si no hay coincidencia exacta, devolver 'none' (sin icono)
    return 'none';
  }
  */

  //
  getBackgroundIconFilaAfectaciones(fecha: string, horaIndex: number): string {
    // Obtener todas las entradas del día dado desde filaAfectacionesDiasCompletos
    const entradas = this.filaAfectacionesDiasCompletos.get(fecha);

    let iconUrl = ''; // Valor predeterminado (sin icono)

    if (entradas && entradas.length > 0) {
      // Buscar una coincidencia exacta de hora
      for (const entrada of entradas) {
        const [horaExacta] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        if (horaExacta === horaIndex) {
          iconUrl = this.getIconUrlPorEstado(entrada.estado);
          return `url('${iconUrl}')`;
        }
      }
    }

    // Si no hay coincidencia exacta, devolver 'none' (sin icono)
    return 'none';
  }
  //

  getIconUrlPorEstado(estado: string): string {
    switch (estado) {
      case 'PERSONAS':
        return '/assets/assets/img/persona.png';
      case 'CARRETERAS':
        return '/assets/assets/img/vialidad.png';
      case 'MEDIOAMBIENTE':
        return '/assets/assets/img/hoja.png';
      case 'VARIOS':
        return '/assets/assets/img/varios_3.png';
      default:
        return '/assets/img/logo-color.png';
    }
  }

  get filaAfectacionesDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      // Si la fecha existe en filaNivelSituacionOperativaEquivalente, la usamos
      if (this.filaAfectaciones.has(fecha)) {
        completo.set(fecha, this.filaAfectaciones.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }
  /*************************/
  /* Fin Fila Afectaciones */
  /*************************/

  /************************************** */
  /*   Fila Medios extinción ordinarios   */
  /************************************** */
  cargarFilaMediosExtincionOrdinarios() {
    this.filaMediosExtincionOrdinarios.set('28-01-2025', [
      { hora: '12:15', estado: 'AVION' },
      { hora: '16:20', estado: 'BRIGADAS' },
    ]);
    this.filaMediosExtincionOrdinarios.set('29-01-2025', [{ hora: '12:15', estado: 'BRIGADAS' }]);
  }

  /*
  getBackgroundColorFilaMediosExtincionOrdinarios(fecha: string, horaIndex: number): string {
    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaMediosExtincionOrdinarios.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Asegurar continuidad temporal agregando días intermedios
    if (fechas.length > 0) {
      const [dayInicio, monthInicio, yearInicio] = fechas[0].split('-').map(Number);
      const [dayFin, monthFin, yearFin] = fechas[fechas.length - 1].split('-').map(Number);

      const fechaInicio = new Date(yearInicio, monthInicio - 1, dayInicio);
      const fechaFin = new Date(yearFin, monthFin - 1, dayFin);

      for (let fechaIter = new Date(fechaInicio); fechaIter <= fechaFin; fechaIter.setDate(fechaIter.getDate() + 1)) {
        const dia = String(fechaIter.getDate()).padStart(2, '0');
        const mes = String(fechaIter.getMonth() + 1).padStart(2, '0');
        const anio = fechaIter.getFullYear();
        const fechaFormateada = `${dia}-${mes}-${anio}`;

        if (!this.filaMediosExtincionOrdinarios.has(fechaFormateada)) {
          this.filaMediosExtincionOrdinarios.set(fechaFormateada, []); // Agregar día vacío
        }
      }
    }

    // Obtener todas las entradas del día dado
    const entradas = this.filaMediosExtincionOrdinarios.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = '#00BFBF'; // Color específico
          return color;
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaMediosExtincionOrdinarios.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        const ultimaEntrada = entradasAnteriores[entradasAnteriores.length - 1];
        color = '#00BFBF'; // Color específico
        break;
      }
    }

    return color;
  }
    */

  //
  getBackgroundColorFilaMediosExtincionOrdinarios(fecha: string, horaIndex: number): string {
    // Obtener todas las entradas del día dado desde filaMediosExtincionOrdinariosDiasCompletos
    const entradas = this.filaMediosExtincionOrdinariosDiasCompletos.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = '#00BFBF'; // Color específico
          return color;
        }
      }
    }

    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaMediosExtincionOrdinariosDiasCompletos.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaMediosExtincionOrdinariosDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        color = '#00BFBF'; // Color específico
        break;
      }
    }

    return color;
  }
  //

  get filaMediosExtincionOrdinariosDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      if (this.filaMediosExtincionOrdinarios.has(fecha)) {
        completo.set(fecha, this.filaMediosExtincionOrdinarios.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }

  /************************************** */
  /* Fin Fila Medios extinción ordinarios */
  /************************************** */

  /****************************************************** */
  /* Fin Fila Medios extinción extraordinarios nacionales */
  /****************************************************** */
  cargarFilaEMediosExtincionExtraordinariosNacionales() {
    this.filaMediosExtincionExtraordinariosNacionales.set('29-01-2025', [
      { hora: '08:15', estado: 'AVION' },
      { hora: '16:20', estado: 'BRIGADAS' },
    ]);
    this.filaMediosExtincionExtraordinariosNacionales.set('28-01-2025', [{ hora: '14:15', estado: 'BRIGADAS' }]);
  }

  /*
  getBackgroundColorFilaMediosExtincionExtraordinariosNacionales(fecha: string, horaIndex: number): string {
    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaMediosExtincionExtraordinariosNacionales.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Asegurar continuidad temporal agregando días intermedios
    if (fechas.length > 0) {
      const [dayInicio, monthInicio, yearInicio] = fechas[0].split('-').map(Number);
      const [dayFin, monthFin, yearFin] = fechas[fechas.length - 1].split('-').map(Number);

      const fechaInicio = new Date(yearInicio, monthInicio - 1, dayInicio);
      const fechaFin = new Date(yearFin, monthFin - 1, dayFin);

      for (let fechaIter = new Date(fechaInicio); fechaIter <= fechaFin; fechaIter.setDate(fechaIter.getDate() + 1)) {
        const dia = String(fechaIter.getDate()).padStart(2, '0');
        const mes = String(fechaIter.getMonth() + 1).padStart(2, '0');
        const anio = fechaIter.getFullYear();
        const fechaFormateada = `${dia}-${mes}-${anio}`;

        if (!this.filaMediosExtincionExtraordinariosNacionales.has(fechaFormateada)) {
          this.filaMediosExtincionExtraordinariosNacionales.set(fechaFormateada, []); // Agregar día vacío
        }
      }
    }

    // Obtener todas las entradas del día dado
    const entradas = this.filaMediosExtincionExtraordinariosNacionales.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = '#00BFBF'; // Color específico
          return color;
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaMediosExtincionExtraordinariosNacionales.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        const ultimaEntrada = entradasAnteriores[entradasAnteriores.length - 1];
        color = '#00BFBF'; // Color específico
        break;
      }
    }

    return color;
  }
    */

  //
  getBackgroundColorFilaMediosExtincionExtraordinariosNacionales(fecha: string, horaIndex: number): string {
    // Obtener todas las fechas ordenadas desde el getter
    const fechas = Array.from(this.filaMediosExtincionOrdinariosNacionalesDiasCompletos.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Obtener todas las entradas del día dado
    const entradas = this.filaMediosExtincionOrdinariosNacionalesDiasCompletos.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = '#00BFBF'; // Color específico
          return color;
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaMediosExtincionOrdinariosNacionalesDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        color = '#00BFBF'; // Color específico
        break;
      }
    }

    return color;
  }

  //

  get filaMediosExtincionOrdinariosNacionalesDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      if (this.filaMediosExtincionExtraordinariosNacionales.has(fecha)) {
        completo.set(fecha, this.filaMediosExtincionExtraordinariosNacionales.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }

  /****************************************************** */
  /* Fin Fila Medios extinción extraordinarios nacionales */
  /****************************************************** */

  /*************************************************************/
  /*   Fila Medios extinción extraordinarios internacionales   */
  /*************************************************************/
  cargarFilaEMediosExtincionExtraordinariosInternacionales() {
    this.filaMediosExtincionExtraordinariosInternacionales.set('29-01-2025', [
      { hora: '06:15', estado: 'AVION' },
      { hora: '16:20', estado: 'BRIGADAS' },
    ]);
    this.filaMediosExtincionExtraordinariosInternacionales.set('28-01-2025', [{ hora: '04:15', estado: 'BRIGADAS' }]);
  }

  /*
  getBackgroundColorFilaMediosExtincionExtraordinariosInternacionales(fecha: string, horaIndex: number): string {
    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaMediosExtincionExtraordinariosInternacionales.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Asegurar continuidad temporal agregando días intermedios
    if (fechas.length > 0) {
      const [dayInicio, monthInicio, yearInicio] = fechas[0].split('-').map(Number);
      const [dayFin, monthFin, yearFin] = fechas[fechas.length - 1].split('-').map(Number);

      const fechaInicio = new Date(yearInicio, monthInicio - 1, dayInicio);
      const fechaFin = new Date(yearFin, monthFin - 1, dayFin);

      for (let fechaIter = new Date(fechaInicio); fechaIter <= fechaFin; fechaIter.setDate(fechaIter.getDate() + 1)) {
        const dia = String(fechaIter.getDate()).padStart(2, '0');
        const mes = String(fechaIter.getMonth() + 1).padStart(2, '0');
        const anio = fechaIter.getFullYear();
        const fechaFormateada = `${dia}-${mes}-${anio}`;

        if (!this.filaMediosExtincionExtraordinariosInternacionales.has(fechaFormateada)) {
          this.filaMediosExtincionExtraordinariosInternacionales.set(fechaFormateada, []); // Agregar día vacío
        }
      }
    }

    // Obtener todas las entradas del día dado
    const entradas = this.filaMediosExtincionExtraordinariosInternacionales.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = '#00BFBF'; // Color específico
          return color;
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaMediosExtincionExtraordinariosInternacionales.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        const ultimaEntrada = entradasAnteriores[entradasAnteriores.length - 1];
        color = '#00BFBF'; // Color específico
        break;
      }
    }

    return color;
  }
    */

  //
  getBackgroundColorFilaMediosExtincionExtraordinariosInternacionales(fecha: string, horaIndex: number): string {
    // Obtener todas las fechas ordenadas desde el getter
    const fechas = Array.from(this.filaMediosExtincionOrdinariosInternacionalesDiasCompletos.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Obtener todas las entradas del día dado
    const entradas = this.filaMediosExtincionOrdinariosInternacionalesDiasCompletos.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = '#00BFBF'; // Color específico
          return color;
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaMediosExtincionOrdinariosInternacionalesDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        color = '#00BFBF'; // Color específico
        break;
      }
    }

    return color;
  }
  //

  get filaMediosExtincionOrdinariosInternacionalesDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      if (this.filaMediosExtincionExtraordinariosInternacionales.has(fecha)) {
        completo.set(fecha, this.filaMediosExtincionExtraordinariosInternacionales.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }

  /*************************************************************/
  /* Fin Fila Medios extinción extraordinarios internacionales */
  /*************************************************************/

  fechaADDMMYY(fechaString: string): string {
    const fecha = new Date(fechaString);
    const dia = String(fecha.getDate()).padStart(2, '0'); // Asegura que el día tenga dos dígitos
    const mes = String(fecha.getMonth() + 1).padStart(2, '0'); // Los meses son 0-indexed, por eso sumamos 1
    const anio = fecha.getFullYear();
    const fechaFormateada = `${dia}-${mes}-${anio}`;
    console.log(dia);
    return fechaFormateada;
  }

  fechaAHHMM(fechaString: string): string {
    const fecha = new Date(fechaString);

    const horas = String(fecha.getHours()).padStart(2, '0');
    const minutos = String(fecha.getMinutes()).padStart(2, '0');

    const fechaFormateada = `${horas}:${minutos}`;
    return fechaFormateada;
  }

  obtenerMenorFechaHistorico(mapas: Map<string, { hora: string; estado: string }[]>[]): string | null {
    let menor: Date | null = null;

    for (const mapa of mapas) {
      for (const clave of mapa.keys()) {
        const partes = clave.split('-'); // Suponiendo formato DD-MM-YYYY
        if (partes.length !== 3) continue;

        const fecha = new Date(
          Number(partes[2]), // Año
          Number(partes[1]) - 1, // Mes (base 0 en JavaScript)
          Number(partes[0]) // Día
        );

        if (!menor || fecha < menor) {
          menor = fecha;
        }
      }
    }

    if (!menor) return null;

    // Formatear salida a DD-MM-YYYY
    return menor.getDate().toString().padStart(2, '0') + '-' + (menor.getMonth() + 1).toString().padStart(2, '0') + '-' + menor.getFullYear();
  }

  obtenerMayorFechaHistorico(mapas: Map<string, { hora: string; estado: string }[]>[]): string | null {
    let mayor: Date | null = null;

    for (const mapa of mapas) {
      for (const clave of mapa.keys()) {
        const partes = clave.split('-'); // Suponiendo formato DD-MM-YYYY
        if (partes.length !== 3) continue;

        const fecha = new Date(
          Number(partes[2]), // Año
          Number(partes[1]) - 1, // Mes (base 0 en JavaScript)
          Number(partes[0]) // Día
        );

        if (!mayor || fecha > mayor) {
          mayor = fecha;
        }
      }
    }

    if (!mayor) return null;

    // Formatear salida a DD-MM-YYYY
    return mayor.getDate().toString().padStart(2, '0') + '-' + (mayor.getMonth() + 1).toString().padStart(2, '0') + '-' + mayor.getFullYear();
  }

  // FIN PCD
}
