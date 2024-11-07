import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
  signal,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
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

@Component({
  selector: 'app-fire-filter-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    InputTextModule,
    DropdownModule,
    CalendarModule,
  ],
  templateUrl: './fire-filter-form.component.html',
  styleUrl: './fire-filter-form.component.css',
})
export class FireFilterFormComponent implements OnInit {
  @Input() fires: ApiResponse<Fire[]>;
  @Output() firesChange = new EventEmitter<ApiResponse<Fire[]>>();

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

  public formData: FormGroup;

  async ngOnInit() {
    this.formData = new FormGroup({
      name: new FormControl(),
      territory: new FormControl(),
      autonomousCommunity: new FormControl(),
      province: new FormControl(),
      country: new FormControl(),
      municipality: new FormControl(),
      fireStatus: new FormControl(),
      episode: new FormControl(),
      severityLevel: new FormControl(),
      affectedArea: new FormControl(),
      move: new FormControl(),
      //enter: new FormControl(),
      start: new FormControl(),
      end: new FormControl(),
      between: new FormControl(),
      eventStatus: new FormControl(),
    });

    this.formData.patchValue({
      between: 1,
      move: 1,
      territory: 1, //pre seleccionamos Nacional
      country: 60, // pre seleccionamos España
      start: moment().subtract(4, 'days').toDate(),
      end: moment().toDate(),
    });

    this.menuItemActiveService.set.emit('/fire');

    const superficiesFiltro =
      await this.superficiesService.getSuperficiesFiltro();
    this.superficiesFiltro.set(superficiesFiltro);

    const territories = await this.territoryService.get();
    this.territories.set(territories);

    //const autonomousCommunities = await this.autonomousCommunityService.get();
    //this.autonomousCommunities.set(autonomousCommunities);
    //getByCountry;

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
  }

  async changeTerritory(event: any) {
    this.formData.patchValue({
      country: event.value == 1 ? 60 : null,
      autonomousCommunity: null,
      province: null,
      municipality: null,
    });

    if (event.value == 1) {
      const countries = await this.countryService.get();
      this.countries.set(countries);
      this.loadCommunities();
      this.disabledCountry.set(false);
    }
    if (event.value == 2) {
      const countries = await this.countryService.get();
      this.countries.set(countries);
      this.loadCommunities();
      this.autonomousCommunities.set([]);
      this.disabledCountry.set(false);
    }
    if (event.value == 3) {
      this.disabledCountry.set(true);
      this.countries.set([]);
    }

    this.provinces.set([]);
  }

  async loadCommunities() {
    const autonomousCommunities =
      await this.autonomousCommunityService.getByCountry(
        this.formData.value.country
      );
    this.autonomousCommunities.set(autonomousCommunities);
  }

  async loadProvinces(event: any) {
    const ac_id = event.value.id;
    const provinces = await this.provinceService.get(ac_id);
    this.provinces.set(provinces);
  }

  async loadMunicipalities(event: any) {
    const province_id = event.value.id;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
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
    });
    this.fires = fires;
    this.firesChange.emit(this.fires);
  }

  clearFormFilter() {
    this.formData.patchValue({
      between: 1,
      move: 1,
      territory: 1, //pre seleccionamos Nacional
      country: 60, // pre seleccionamos España
      start: moment().subtract(4, 'days').toDate(),
      end: moment().toDate(),
      autonomousCommunity: null,
      province: null,
      municipality: null,
      fireStatus: null,
      episode: null,
      affectedArea: null,
      severityLevel: null,
      name: '',
    });
  }
}
