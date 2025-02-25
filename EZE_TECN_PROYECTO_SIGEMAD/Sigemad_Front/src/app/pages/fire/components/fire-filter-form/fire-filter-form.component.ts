import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output, signal, SimpleChanges } from '@angular/core';

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
import { MasterDataEvolutionsService } from '../../../../services/master-data-evolutions.service';
import { SituationsEquivalent } from '../../../../types/situations-equivalent.type';
import { EventService } from '../../../../services/event.service';
import { Event } from '../../../../types/event.type';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MY_DATE_FORMATS } from '../../../../types/date-formats';

@Component({
  selector: 'app-fire-filter-form',
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
    MatIconModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatDialogModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
  templateUrl: './fire-filter-form.component.html',
  styleUrl: './fire-filter-form.component.scss',
})
export class FireFilterFormComponent implements OnInit {
  // PCD

  // FIN PCD

  @Input() fires: ApiResponse<Fire[]> | undefined;
  @Input() filtros: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;
  @Output() firesChange = new EventEmitter<ApiResponse<Fire[]>>();
  @Output() isLoadingChange = new EventEmitter<boolean>();
  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  COUNTRIES_ID = {
    PORTUGAL: 1,
    SPAIN: 60,
    FRANCE: 65,
  };

  public filtrosIncendioService = inject(LocalFiltrosIncendio);

  public superficiesService = inject(SuperficiesService);
  public menuItemActiveService = inject(MenuItemActiveService);
  public territoryService = inject(TerritoryService);
  public autonomousCommunityService = inject(AutonomousCommunityService);
  public provinceService = inject(ProvinceService);
  public countryService = inject(CountryService);
  public eventStatusService = inject(EventStatusService);
  public eventTypeService = inject(EventService);
  public municipalityService = inject(MunicipalityService);
  public severityLevelService = inject(SeverityLevelService);
  public fireService = inject(FireService);
  public comparativeDateService = inject(ComparativeDateService);
  public moveService = inject(MoveService);
  private dialog = inject(MatDialog);
  public masterData = inject(MasterDataEvolutionsService);

  public superficiesFiltro = signal<any[]>([]);
  public territories = signal<Territory[]>([]);
  public autonomousCommunities = signal<AutonomousCommunity[]>([]);
  public provinces = signal<Province[]>([]);
  //public countries = signal<Countries[]>([]);
  public listaPaisesExtranjeros = signal<Countries[]>([]);
  public listaPaisesNacionales = signal<Countries[]>([]);

  public eventStatus = signal<EventStatus[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public fireStatus = signal<FireStatus[]>([]);
  public situationsEquivalent = signal<SituationsEquivalent[]>([]);

  public eventTypes = signal<Event[]>([]);

  public showDateEnd = signal<boolean>(true);

  public moves = signal<Move[]>([]);
  public comparativeDates = signal<ComparativeDate[]>([]);

  public filteredCountries = signal<Countries[]>([]);
  public formData!: FormGroup;

  myForm!: FormGroup;

  showFilters = false;

  async ngOnInit() {
    const fb = new FormBuilder();
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required],
    });
    const {
      situationEquivalent,
      name,
      territory,
      country,
      autonomousCommunity,
      province,
      fireStatus: initFireStatus,
      eventStatus: initEventStatus,
      CCAA,
      affectedArea,
      move,
      between,
      //start,
      //end,
      municipality,
      episode,
      provincia,
      fechaInicio,
      fechaFin,
      eventTypes,
    } = this.filtros();

    this.formData = new FormGroup({
      name: new FormControl(name ?? ''),
      territory: new FormControl(territory ?? 1),
      autonomousCommunity: new FormControl(autonomousCommunity ?? ''),
      province: new FormControl(province ?? ''),
      country: new FormControl(country ?? this.COUNTRIES_ID.SPAIN),
      municipality: new FormControl(municipality ?? ''),
      fireStatus: new FormControl(initFireStatus ?? ''),
      episode: new FormControl(episode ?? ''),
      situationEquivalent: new FormControl(situationEquivalent ?? ''),
      affectedArea: new FormControl(affectedArea ?? ''),
      move: new FormControl(move ?? 1),
      //start: new FormControl(start ?? ''),
      //end: new FormControl(end ?? ''),
      between: new FormControl(between ?? 1),
      eventStatus: new FormControl(initEventStatus ?? ''),
      CCAA: new FormControl(CCAA ?? ''),
      provincia: new FormControl(provincia ?? ''),
      fechaInicio: new FormControl(fechaInicio ?? moment().subtract(4, 'days').toDate()),
      fechaFin: new FormControl(fechaFin ?? moment().toDate()),
      eventTypes: new FormControl(eventTypes ?? 1),
    });

    const countriesExtranjeros = await this.countryService.getExtranjeros();
    this.listaPaisesExtranjeros.set(countriesExtranjeros);
    const countriesNacionales = await this.countryService.getNacionales();
    this.listaPaisesNacionales.set(countriesNacionales);

    this.filteredCountries.set(countriesNacionales);

    //this.clearFormFilter();
    this.menuItemActiveService.set.emit('/fire');

    const superficiesFiltro = await this.superficiesService.getSuperficiesFiltro();
    this.superficiesFiltro.set(superficiesFiltro);

    const territories = await this.territoryService.get();
    this.territories.set(territories);

    const fireStatus = await this.masterData.getFireStatus();
    this.fireStatus.set(fireStatus);

    const situationsEquivalents = await this.masterData.getSituationEquivalent();
    this.situationsEquivalent.set(situationsEquivalents);

    const eventStatus = await this.eventStatusService.get();
    this.eventStatus.set(eventStatus);

    const moves = await this.moveService.get();
    this.moves.set(moves);

    const comparativeDates = await this.comparativeDateService.get();
    this.comparativeDates.set(comparativeDates);

    const eventsTypes = await this.eventTypeService.get();
    this.eventTypes.set(eventsTypes);

    this.loadCommunities();

    this.onSubmit();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ('refreshFilterForm' in changes) {
      this.onSubmit();
    }
  }

  toggleAccordion(panel: MatExpansionPanel) {
    panel.toggle();
  }

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
      country: event.value == 1 ? this.COUNTRIES_ID.SPAIN : '',
      autonomousCommunity: '',
      province: '',
      municipality: '',
    });
    this.loadCommunities(event.value == 1 ? this.COUNTRIES_ID.SPAIN : '9999');
    if (event.value == 1) {
      this.filteredCountries.set(this.listaPaisesNacionales());
    }
    if (event.value == 2) {
      this.filteredCountries.set(this.listaPaisesExtranjeros());
    }
    if (event.value == 3) {
      this.filteredCountries.set([]);
    }
  }

  async loadCommunities(country?: any) {
    if (country === '9999') {
      this.autonomousCommunities.set([]);
      return;
    }
    const autonomousCommunities = await this.autonomousCommunityService.getByCountry(country ?? this.formData.value.country);
    this.autonomousCommunities.set(autonomousCommunities);
  }

  async loadProvinces(event: any) {
    const ac_id = event.value;
    const provinces = await this.provinceService.get(ac_id);
    this.provinces.set(provinces);
  }

  changeBetween(event: any) {
    this.showDateEnd.set(event.value === 1 || event.value === 5 ? true : false);
  }

  async onSubmit() {
    this.firesChange.emit({
      count: 0,
      page: 1,
      pageSize: 10,
      data: [],
      pageCount: 0,
    });
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const {
      territory,
      country,
      autonomousCommunity,
      province,
      fireStatus,
      eventStatus,
      situationEquivalent,
      affectedArea,
      move,
      between,
      //start,
      //end,
      fechaInicio,
      fechaFin,
      name,
      eventTypes,
    } = this.formData.value;

    const fires = await this.fireService.get({
      IdTerritorio: territory,
      IdPais: country,
      IdCcaa: autonomousCommunity,
      IdProvincia: province,
      IdEstadoSuceso: eventStatus,
      IdEstadoIncendio: fireStatus,
      IdSituacionEquivalente: situationEquivalent,
      IdSuperficieAfectada: affectedArea,
      IdMovimiento: move,
      IdComparativoFecha: between,
      FechaInicio: moment(fechaInicio).format('YYYY-MM-DD'),
      FechaFin: moment(fechaFin).format('YYYY-MM-DD'),
      denominacion: name,
      search: name,
      idClaseSuceso: eventTypes,
    });
    this.filtrosIncendioService.setFilters(this.formData.value);
    this.fires = fires;
    this.firesChange.emit(this.fires);
    this.isLoadingChange.emit(false);
    this.isLoading = false;
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
      situationEquivalent: '',
      name: '',
      eventTypes: 1,
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  goModal() {
    const dialogRef = this.dialog.open(FireCreateEdit, {
      width: '75vw',
      maxWidth: 'none',
      data: {
        title: 'Nuevo - Datos Evolución',
        fire: {},
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
        this.onSubmit();
      }
    });
  }
}
