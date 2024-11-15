import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
  signal,
  SimpleChanges,
} from '@angular/core';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

import { AutonomousCommunityService } from '../../../../services/autonomous-community.service';

import moment from 'moment';
import { ComparativeDateService } from '../../../../services/comparative-date.service';
import { CountryService } from '../../../../services/country.service';
import { EventStatusService } from '../../../../services/eventStatus.service';
import { FireStatusService } from '../../../../services/fire-status.service';
import { FireService } from '../../../../services/fire.service';
import { MenuItemActiveService } from '../../../../services/menu-item-active.service';
import { MoveService } from '../../../../services/move.service';
import { MunicipalityService } from '../../../../services/municipality.service';
import { ProvinceService } from '../../../../services/province.service';
import { SeverityLevelService } from '../../../../services/severity-level.service';
import { SuperficiesService } from '../../../../services/superficies.service';
import { TerritoryService } from '../../../../services/territory.service';
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
import { LocalFiltrosIncendio } from '../../../../services/local-filtro-incendio.service';
import { FormFieldComponent } from '../../../../shared/Inputs/field.component';

@Component({
  selector: 'app-fire-filter-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    FormFieldComponent
  ],
  templateUrl: './fire-filter-form.component.html',
  styleUrl: './fire-filter-form.component.css',
})
export class FireFilterFormComponent implements OnInit {
  @Input() fires: ApiResponse<Fire[]>;
  @Input() filtros: any;
  @Output() firesChange = new EventEmitter<ApiResponse<Fire[]>>();

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
  public municipalityService = inject(MunicipalityService);
  public fireStatusService = inject(FireStatusService);
  public severityLevelService = inject(SeverityLevelService);
  public fireService = inject(FireService);

  public comparativeDateService = inject(ComparativeDateService);
  public moveService = inject(MoveService);

  public superficiesFiltro = signal<any[]>([]);
  public territories = signal<Territory[]>([]);
  public autonomousCommunities = signal<AutonomousCommunity[]>([]);
  public provinces = signal<Province[]>([]);
  public countries = signal<Countries[]>([]);
  public eventStatus = signal<EventStatus[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public fireStatus = signal<FireStatus[]>([]);
  public severityLevels = signal<SeverityLevel[]>([]);

  public showDateEnd = signal<boolean>(true);

  public moves = signal<Move[]>([]);
  public comparativeDates = signal<ComparativeDate[]>([]);

  public disabledCountry = signal<boolean>(false);
  public disabledAutonomousCommunity = signal<boolean>(false);
  public disabledProvince = signal<boolean>(false);

  public filterCountries = signal<any[]>([]);

  public formData: FormGroup;

  async ngOnInit() {
    const {
      severityLevel,
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
      start,
      end,
      municipality,
      episode,
      provincia
    } = this.filtros();

    this.formData = new FormGroup({
      name: new FormControl(name ?? ''),
      territory: new FormControl(territory ?? ''),
      autonomousCommunity: new FormControl(autonomousCommunity ?? ''),
      province: new FormControl(province ?? ''),
      country: new FormControl(country ?? ''),
      municipality: new FormControl(municipality ?? ''),
      fireStatus: new FormControl(initFireStatus ?? ''),
      episode: new FormControl(episode ?? ''),
      severityLevel: new FormControl(severityLevel ?? ''),
      affectedArea: new FormControl(affectedArea ?? ''),
      move: new FormControl(move ?? ''),
      //enter: new FormControl(?? ''),
      start: new FormControl(start ?? ''),
      end: new FormControl(end ?? ''),
      between: new FormControl(between ?? ''),
      eventStatus: new FormControl(initEventStatus ?? ''),
      CCAA: new FormControl(CCAA ?? ''),
      provincia: new FormControl(provincia ?? ''),
    });

    this.clearFormFilter();
    this.menuItemActiveService.set.emit('/fire');

    const superficiesFiltro =
      await this.superficiesService.getSuperficiesFiltro();
    this.superficiesFiltro.set(superficiesFiltro);

    const territories = await this.territoryService.get();
    this.territories.set(territories);

    const fireStatus = await this.fireStatusService.get();
    this.fireStatus.set(fireStatus);

    const severityLevels = await this.severityLevelService.get();
    this.severityLevels.set(severityLevels);

    const eventStatus = await this.eventStatusService.get();
    this.eventStatus.set(eventStatus);

    const countries = await this.countryService.get();
    this.countries.set(countries);

    const moves = await this.moveService.get();
    this.moves.set(moves);

    const comparativeDates = await this.comparativeDateService.get();
    this.comparativeDates.set(comparativeDates);

    this.loadCommunities();
    this.getCountriesByTerritory();

    this.onSubmit()
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['filtros']) {
      console.log('El filtro ha cambiado:', changes['filtro'].currentValue);
      
    }
  }

  getCountryByTerritory (country: any, territory: any ){
    if(territory == 1){
      return country
    }
    if(territory == 2){
      if(country == this.COUNTRIES_ID.SPAIN){
        return null
      }
    }
  }

  async changeTerritory(event: any) {
    this.formData.patchValue({
      country: event.target.value == 1 ? this.COUNTRIES_ID.SPAIN : '',
      autonomousCommunity: '',
      province: '',
      municipality: '',
    });

    if (event.target.value == 1) {
      const countries = await this.countryService.get();
      this.countries.set(countries);
      this.loadCommunities();
      this.disabledCountry.set(true);
      this.disabledAutonomousCommunity.set(false);
      this.disabledProvince.set(false);
      this.formData.patchValue({
        country: this.COUNTRIES_ID.SPAIN,
      });
    }
    if (event.target.value == 2) {
      const countries = await this.countryService.get();
      this.countries.set(countries);
      //this.loadCommunities();
      this.autonomousCommunities.set([]);
      this.disabledCountry.set(false);
      this.disabledAutonomousCommunity.set(false);
      this.disabledProvince.set(false);
      this.formData.patchValue({
        country: '',
      });
    }
    if (event.target.value == 3) {
      this.disabledCountry.set(true);
      this.disabledAutonomousCommunity.set(true);
      this.disabledProvince.set(true);
      this.countries.set([]);
      this.formData.patchValue({
        country: '',
      });
    }

    this.provinces.set([]);
    this.getCountriesByTerritory();
  }

  getCountriesByTerritory() {
    let original = [...this.countries()];
    let newCountries = [...this.countries()];

    if (this.formData.value.territory != 2) {
      this.filterCountries.set(newCountries);
    }
    if (this.formData.value.territory == 2) {
      const indexSpain = newCountries.findIndex(
        (country) => country.id == this.COUNTRIES_ID.SPAIN
      );
      newCountries.splice(indexSpain, 1);
      const indexPortugal = newCountries.findIndex(
        (country) => country.id == this.COUNTRIES_ID.PORTUGAL
      );
      newCountries.splice(indexPortugal, 1);
      const indexFrance = newCountries.findIndex(
        (country) => country.id == this.COUNTRIES_ID.FRANCE
      );
      newCountries.splice(indexFrance, 1);

      const portugal = original.find(
        (country) => country.id === this.COUNTRIES_ID.PORTUGAL
      );
      const france = original.find(
        (country) => country.id === this.COUNTRIES_ID.FRANCE
      );

      if (france) {
        newCountries.unshift(france);
      }
      if (portugal) {
        newCountries.unshift(portugal);
      }

      this.filterCountries.set(newCountries);
    }
  }

  async loadCommunities() {
    const autonomousCommunities =
      await this.autonomousCommunityService.getByCountry(
        this.formData.value.country
      );
    this.autonomousCommunities.set(autonomousCommunities);
  }

  async loadProvinces(event: any) {
    const ac_id = event.target.value;
    const provinces = await this.provinceService.get(ac_id);
    this.provinces.set(provinces);
  }

  changeBetween(event: any) {
    this.showDateEnd.set(event.value === 1 || event.value === 5 ? true : false);
  }

  async onSubmit() {
    const {
      territory,
      country,
      autonomousCommunity,
      province,
      fireStatus,
      eventStatus,
      severityLevel,
      affectedArea,
      move,
      between,
      start,
      end,
      name
    } = this.formData.value;

    const fires = await this.fireService.get({
      IdTerritorio: territory,
      IdPais: country,
      IdCcaa: autonomousCommunity,
      IdProvincia: province,
      IdEstadoSuceso: fireStatus,
      IdEstadoIncendio: eventStatus,
      IdNivelGravedad: severityLevel,
      IdSuperficieAfectada: affectedArea,
      IdMovimiento: move,
      IdComparativoFecha: between,
      FechaInicio: moment(start).format('YYYY-MM-DD'),
      FechaFin: moment(end).format('YYYY-MM-DD'),
      denominacion: name
    });
    this.filtrosIncendioService.setFilters(this.formData.value);
    this.fires = fires;
    this.firesChange.emit(this.fires);
    
  }

  clearFormFilter() {
    this.formData.patchValue({
      between: 1,
      move: 1,
      territory: 1, //pre seleccionamos Nacional
      country: this.COUNTRIES_ID.SPAIN, // pre seleccionamos España
      start: moment().subtract(4, 'days').toDate(),
      end: moment().toDate(),
      autonomousCommunity: '',
      province: '',
      municipality: '',
      fireStatus: '',
      episode: '',
      affectedArea: '',
      severityLevel: '',
      name: '',
    });
    this.getCountriesByTerritory();
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }
}
