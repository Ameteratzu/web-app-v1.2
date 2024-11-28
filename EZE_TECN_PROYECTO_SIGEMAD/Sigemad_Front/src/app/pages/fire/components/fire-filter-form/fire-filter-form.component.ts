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
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
  FormBuilder
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from '@angular/material/icon';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';
import { MatDatepickerModule } from '@angular/material/datepicker';

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
import { FireCreateEdit } from '../../../fire/components/fire-create-edit-form/fire-create-edit-form.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';


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
    MatDialogModule
  ],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
  templateUrl: './fire-filter-form.component.html',
  styleUrl: './fire-filter-form.component.scss',
})
export class FireFilterFormComponent implements OnInit {
  @Input() fires: ApiResponse<Fire[]> | undefined;
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
  private dialog = inject(MatDialog);

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

  public filteredCountries = signal<Countries[]>([]);
  public formData!: FormGroup;

  myForm!: FormGroup; 
  options = [
    { label: 'Opción 1', value: 'option1' },
    { label: 'Opción 2', value: 'option2' },
    { label: 'Opción 3', value: 'option3' }
  ];

  showFilters = false;

  async ngOnInit() {
    const fb = new FormBuilder(); 
    this.myForm = fb.group({
      selectField: ['', Validators.required],
      inputField1: ['', Validators.required],
      inputField2: ['', Validators.required]
    });
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
      provincia,
      fechaInicio,
      fechaFin
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
      start: new FormControl(start ?? ''),
      end: new FormControl(end ?? ''),
      between: new FormControl(between ?? ''),
      eventStatus: new FormControl(initEventStatus ?? ''),
      CCAA: new FormControl(CCAA ?? ''),
      provincia: new FormControl(provincia ?? ''),
      fechaInicio: new FormControl(fechaInicio ?? ''),
      fechaFin: new FormControl(fechaFin ?? ''),
    });

    const countries = await this.countryService.get();
    this.countries.set(countries);

    this.formData.get('country')?.valueChanges.subscribe((value) => {
      
      this.updateFilteredCountries(value || '');

    });

    this.updateFilteredCountries('');
    
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

    const countries2 = await this.countryService.get();
    this.countries.set(countries2);

    const moves = await this.moveService.get();
    this.moves.set(moves);

    const comparativeDates = await this.comparativeDateService.get();
    this.comparativeDates.set(comparativeDates);

    this.loadCommunities();
    this.getCountriesByTerritory();

    this.onSubmit()
  }

  toggleAccordion(panel: MatExpansionPanel) {
    panel.toggle(); 
  }

   private updateFilteredCountries(value: string) {
    
    const filterValue = value.toLowerCase();
    const allCountries = this.countries();
    this.filteredCountries.set(
      allCountries.filter((country) =>
        country.descripcion.toLowerCase().includes(filterValue)
      )
    );
  }

  filteredCountriesList() {
    return this.filteredCountries();
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
    console.log("🚀 ~ FireFilterFormComponent ~ changeTerritory ~ event:", event)
    this.formData.patchValue({
      country: event.value == 1 ? this.COUNTRIES_ID.SPAIN : '',
      autonomousCommunity: '',
      province: '',
      municipality: '',
    });

    if (event.value == 1) {
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
    if (event.value == 2) {
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
    if (event.value == 3) {
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
      this.filteredCountries.set(newCountries); 
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

      this.filteredCountries.set(newCountries);
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
    const ac_id = event.value;
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
      territory: 1, 
      country: "",
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

  goModal() {
    const dialogRef = this.dialog.open(FireCreateEdit, {
      width: '75vw',
      maxWidth: 'none', 
      data: {
        title: 'Nuevo - Datos Evolución', 
        fire: {}
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
      }
    });
  }
}
