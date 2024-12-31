import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output, signal, SimpleChanges, ViewChild } from '@angular/core';

import { DateAdapter, MAT_DATE_FORMATS, NativeDateAdapter } from '@angular/material/core';

import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import moment from 'moment';
import { AutonomousCommunityService } from '../../../../services/autonomous-community.service';
import { ComparativeDateService } from '../../../../services/comparative-date.service';
import { CountryService } from '../../../../services/country.service';
import { EventStatusService } from '../../../../services/eventStatus.service';
import { FireStatusService } from '../../../../services/fire-status.service';
import { FireService } from '../../../../services/fire.service';
import { LocalFiltrosIncendio } from '../../../../services/local-filtro-incendio.service';
import { MenuItemActiveService } from '../../../../services/menu-item-active.service';
import { MoveService } from '../../../../services/move.service';
import { MunicipalityService } from '../../../../services/municipality.service';
import { ProvinceService } from '../../../../services/province.service';
import { SeverityLevelService } from '../../../../services/severity-level.service';
import { SuperficiesService } from '../../../../services/superficies.service';
import { TerritoryService } from '../../../../services/territory.service';
import { FormFieldComponent } from '../../../../shared/Inputs/field.component';
import { ApiResponse } from '../../../../types/api-response.type';
import { AutonomousCommunity } from '../../../../types/autonomous-community.type';
import { ComparativeDate } from '../../../../types/comparative-date.type';
import { Countries } from '../../../../types/country.type';
import { EventStatus } from '../../../../types/eventStatus.type';
import { FireStatus } from '../../../../types/fire-status.type';
import { Fire } from '../../../../types/fire.type';
import { Move } from '../../../../types/move.type';
import { Municipality } from '../../../../types/municipality.type';
import { Province } from '../../../../types/province.type';
import { SeverityLevel } from '../../../../types/severity-level.type';
import { Territory } from '../../../../types/territory.type';
import { FireCreateEdit } from '../../../fire/components/fire-create-edit-form/fire-create-edit-form.component';

import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EventService } from '../../../../services/event.service';
import { SucesosRelacionadosService } from '../../../../services/sucesos-relacionados.service';

const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-fire-related-event-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    FormFieldComponent,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatTableModule,
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatDialogModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
  templateUrl: './fire-related-event-form.component.html',
  styleUrl: './fire-related-event-form.component.scss',
})
export class FireRelatedEventForm implements OnInit {
  @Input() fire: any;
  /*
  @Input() fires: ApiResponse<Fire[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() firesChange = new EventEmitter<ApiResponse<Fire[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();
  */
  @ViewChild(MatSort) sort!: MatSort;

  COUNTRIES_ID = {
    PORTUGAL: 1,
    SPAIN: 60,
    FRANCE: 65,
  };

  public autonomousCommunityService = inject(AutonomousCommunityService);
  public municipioService = inject(MunicipalityService);
  public filtrosIncendioService = inject(LocalFiltrosIncendio);
  public menuItemActiveService = inject(MenuItemActiveService);
  public superficiesService = inject(SuperficiesService);
  public territoryService = inject(TerritoryService);
  public fireStatusService = inject(FireStatusService);
  public severityLevelService = inject(SeverityLevelService);
  public fireService = inject(FireService);
  public eventStatusService = inject(EventStatusService);
  public moveService = inject(MoveService);
  public comparativeDateService = inject(ComparativeDateService);
  public sucesosRelacionadosService = inject(SucesosRelacionadosService);

  public dataFindedEvents = signal<any[]>([]);
  public dataFindedRelationsEvents = signal<any[]>([]);

  public comparativeDates = signal<ComparativeDate[]>([]);
  public listaSucesosRelacionados = signal<any>({});
  public listaSucesos = signal<any>({});

  public fireStatus = signal<FireStatus[]>([]);
  public severityLevels = signal<SeverityLevel[]>([]);
  public filteredCountries = signal<Countries[]>([]);
  public listaPaisesExtranjeros = signal<Countries[]>([]);
  public listaPaisesNacionales = signal<Countries[]>([]);
  public superficiesFiltro = signal<any[]>([]);

  public eventStatus = signal<EventStatus[]>([]);
  public moves = signal<Move[]>([]);
  public showDateEnd = signal<boolean>(true);
  //public provinces = signal<Province[]>([]);
  //public autonomousCommunities = signal<AutonomousCommunity[]>([]);

  public listadoClaseSuceso = signal<any[]>([]);
  public listadoTerritorio = signal<any[]>([]);
  public listadoPaises = signal<any[]>([]);
  public listadoCCAA = signal<any[]>([]);
  public listadoProvincia = signal<any[]>([]);
  public listadoMunicipio = signal<any[]>([]);

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = ['fecha', 'eventType', 'status', 'denominacion', 'opciones'];
  public displayedColumnsRelations: string[] = ['fechaCreacion', 'fechaModificacion', 'observaciones', 'idSucesoAsociado', 'opciones'];

  public formData!: FormGroup;

  myForm!: FormGroup;

  public provinceService = inject(ProvinceService);
  public countryService = inject(CountryService);
  public eventService = inject(EventService);

  async ngOnInit() {
    const fb = new FormBuilder();
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
    });

    this.formData = new FormGroup({
      name: new FormControl(''),
      claseSuceco: new FormControl(''),
      territory: new FormControl(1),
      country: new FormControl(this.COUNTRIES_ID.SPAIN),
      CCAA: new FormControl(''),
      province: new FormControl(''),
      minicipality: new FormControl(''),
      move: new FormControl(1),
      between: new FormControl(1),
      fechaInicio: new FormControl(moment().subtract(4, 'days').toDate()),
      fechaFin: new FormControl(moment().toDate()),
    });

    const estadoSuceso = await this.eventService.get();
    this.listadoClaseSuceso.set(estadoSuceso);
    const countriesExtranjeros = await this.countryService.getExtranjeros();
    this.listaPaisesExtranjeros.set(countriesExtranjeros);
    const countriesNacionales = await this.countryService.getNacionales();
    this.listaPaisesNacionales.set(countriesNacionales);

    this.filteredCountries.set(countriesNacionales);

    this.menuItemActiveService.set.emit('/fire');

    const superficiesFiltro = await this.superficiesService.getSuperficiesFiltro();
    this.superficiesFiltro.set(superficiesFiltro);

    const territories = await this.territoryService.get();
    this.listadoTerritorio.set(territories);

    const fireStatus = await this.fireStatusService.get();
    this.fireStatus.set(fireStatus);

    const severityLevels = await this.severityLevelService.get();
    this.severityLevels.set(severityLevels);

    const eventStatus = await this.eventStatusService.get();
    this.eventStatus.set(eventStatus);

    const moves = await this.moveService.get();
    this.moves.set(moves);

    const comparativeDates = await this.comparativeDateService.get();
    this.comparativeDates.set(comparativeDates);

    const listadoSucesosRelacionados = await this.sucesosRelacionadosService.get(this.fire.idSuceso);

    this.listaSucesosRelacionados.set({ data: listadoSucesosRelacionados });

    this.loadCommunities();

    this.onSubmit();
  }

  /*
  ngOnChanges(changes: SimpleChanges): void {
    if ('refreshFilterForm' in changes ) {
      this.onSubmit()
    }
  }
  */

  getCountryByTerritory(country: any, territory: any) {
    if (territory == 1) {
      return country;
    }
    if (territory == 2) {
      if (country == this.COUNTRIES_ID.SPAIN) {
        return null;
      }
    }
  }

  async changeTerritory(event: any) {
    this.formData.patchValue({
      country: event.value.id == 1 ? this.COUNTRIES_ID.SPAIN : '',
      autonomousCommunity: '',
      province: '',
      municipality: '',
    });
    this.loadCommunities(event.value.id == 1 ? this.COUNTRIES_ID.SPAIN : '9999');
    if (event.value.id == 1) {
      this.filteredCountries.set(this.listaPaisesNacionales());
    }
    if (event.value.id == 2) {
      this.filteredCountries.set(this.listaPaisesExtranjeros());
    }
    if (event.value.id == 3) {
      this.filteredCountries.set([]);
    }
  }

  async loadCommunities(country?: any) {
    if (country === '9999') {
      this.listadoCCAA.set([]);
      return;
    }
    const autonomousCommunities = await this.autonomousCommunityService.getByCountry(country ?? this.formData.value.country);
    this.listadoCCAA.set(autonomousCommunities);
  }

  async loadProvinces(event: any) {
    const ac_id = event.value.id;
    const provinces = await this.provinceService.get(ac_id);
    this.listadoProvincia.set(provinces);
  }

  async loadMunicipios(event: any) {
    const provinciaId = event.value.id;
    const municipios = await this.municipioService.get(provinciaId);
    this.listadoMunicipio.set(municipios);
  }

  changeBetween(event: any) {
    this.showDateEnd.set(event.value === 1 || event.value === 5 ? true : false);
  }

  async onSubmit() {
    const { name, claseSuceco, territory, country, CCAA, province, minicipality, move, between, fechaInicio, fechaFin } = this.formData.value;

    const listadoSucesos: any = await this.sucesosRelacionadosService.getListaSuceso({
      Denominacion: name,
      IdClaseSuceso: claseSuceco,
      IdTerritorio: territory,
      IdPais: country,
      IdSuceso: this.fire.idSuceso,
      IdCcaa: CCAA,
      IdProvincia: province,
      IdMunicipio: minicipality,
      IdMovimiento: move,
      IdComparativoFecha: between,
      FechaInicio: moment(fechaInicio).format('YYYY-MM-DD'),
      FechaFin: moment(fechaFin).format('YYYY-MM-DD'),
      //Sort: '',
      Page: 0,
      //PageSize: 0,
      search: name,
    });
    
    const dataFiltrada = listadoSucesos.data.filter(
      (listadoSuceso: any) => !this.listaSucesosRelacionados().data.some((sucesoRelacionado: any) => sucesoRelacionado.idSucesoAsociado === listadoSuceso.idSuceso)
    );

    this.listaSucesos.set({data:dataFiltrada});
  }

  async agregarItem(i: any) {
    await this.sucesosRelacionadosService.post(this.fire.idSuceso, {
      idSucesoAsociado: this.listaSucesos()?.data[i]?.idSuceso,
      observaciones: '',
    });

    const listadoSucesosRelacionados = await this.sucesosRelacionadosService.get(this.fire.idSuceso);
    this.listaSucesosRelacionados.set({ data: listadoSucesosRelacionados });

    this.onSubmit()
  }

  //seleccionarItem(i: any) {}
  async eliminarItem(i: any) {
    await this.sucesosRelacionadosService.delete( {
      idSucesoPrincipal: this.fire.idSuceso,
      idSucesoAsociado: this.listaSucesosRelacionados()?.data[i]?.idSucesoAsociado,
    });
    const listadoSucesosRelacionados = await this.sucesosRelacionadosService.get(this.fire.idSuceso);
    this.listaSucesosRelacionados.set({ data: listadoSucesosRelacionados });
    this.onSubmit()
  }

  clearFormFilter() {
    this.formData.reset();
    this.formData.patchValue({
      between: 1,
      move: 1,
      territory: 1,
      country: this.COUNTRIES_ID.SPAIN,
      fechaInicio: moment().subtract(4, 'days').toDate(),
      fechaFin: moment().toDate(),
      autonomousCommunity: '',
      province: '',
      municipality: '',
      fireStatus: '',
      episode: '',
      affectedArea: '',
      severityLevel: '',
      name: '',
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  getFormatdate(date: any) {
    return date ? moment(date).format('DD/MM/YY HH:mm') : '';
  }

  getDescripcionProcedenciaDestion(procedenciaDestino: any[]) {
    return procedenciaDestino.map((obj) => obj.descripcion).join(', ');
  }
}
