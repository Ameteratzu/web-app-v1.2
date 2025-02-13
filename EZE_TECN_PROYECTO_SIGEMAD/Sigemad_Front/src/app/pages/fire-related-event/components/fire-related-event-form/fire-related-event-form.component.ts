import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output, signal, ViewChild } from '@angular/core';

import { DateAdapter, MAT_DATE_FORMATS, NativeDateAdapter } from '@angular/material/core';

import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { MatDialogModule } from '@angular/material/dialog';
import moment from 'moment';
import { AutonomousCommunityService } from '../../../../services/autonomous-community.service';
import { ComparativeDateService } from '../../../../services/comparative-date.service';
import { CountryService } from '../../../../services/country.service';
import { EventStatusService } from '../../../../services/eventStatus.service';
import { FireService } from '../../../../services/fire.service';
import { LocalFiltrosIncendio } from '../../../../services/local-filtro-incendio.service';
import { MasterDataEvolutionsService } from '../../../../services/master-data-evolutions.service';
import { MenuItemActiveService } from '../../../../services/menu-item-active.service';
import { MoveService } from '../../../../services/move.service';
import { MunicipalityService } from '../../../../services/municipality.service';
import { ProvinceService } from '../../../../services/province.service';
import { SeverityLevelService } from '../../../../services/severity-level.service';
import { SuperficiesService } from '../../../../services/superficies.service';
import { TerritoryService } from '../../../../services/territory.service';
import { FormFieldComponent } from '../../../../shared/Inputs/field.component';
import { ComparativeDate } from '../../../../types/comparative-date.type';
import { Countries } from '../../../../types/country.type';
import { EventStatus } from '../../../../types/eventStatus.type';
import { FireStatus } from '../../../../types/fire-status.type';
import { Move } from '../../../../types/move.type';
import { SeverityLevel } from '../../../../types/severity-level.type';

import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EventService } from '../../../../services/event.service';
import { SucesosRelacionadosService } from '../../../../services/sucesos-relacionados.service';

import { AlertService } from '../../../../shared/alert/alert.service';
import { MatSnackBar } from '@angular/material/snack-bar';

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
    MatCheckboxModule,
    NgxSpinnerModule,
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
  @Input() fireDetail: any;
  @Output() closeModal = new EventEmitter<void>();

  @ViewChild(MatSort) sort!: MatSort;

  COUNTRIES_ID = {
    PORTUGAL: 1,
    SPAIN: 60,
    FRANCE: 65,
  };

  private spinner = inject(NgxSpinnerService);
  private alertService = inject(AlertService);

  // PCD
  public snackBar = inject(MatSnackBar);
  // FIN PCD

  public autonomousCommunityService = inject(AutonomousCommunityService);
  public municipioService = inject(MunicipalityService);
  public filtrosIncendioService = inject(LocalFiltrosIncendio);
  public menuItemActiveService = inject(MenuItemActiveService);
  public superficiesService = inject(SuperficiesService);
  public territoryService = inject(TerritoryService);
  public masterData = inject(MasterDataEvolutionsService);
  public severityLevelService = inject(SeverityLevelService);
  public fireService = inject(FireService);
  public eventStatusService = inject(EventStatusService);
  public moveService = inject(MoveService);
  public comparativeDateService = inject(ComparativeDateService);
  public sucesosRelacionadosService = inject(SucesosRelacionadosService);

  public dataFindedEvents = signal<any[]>([]);
  public dataFindedRelationsEvents = signal<any[]>([]);

  public comparativeDates = signal<ComparativeDate[]>([]);
  public listaSucesosRelacionados = signal<any>({ data: {} });

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

  public listadoClaseSuceso = signal<any[]>([]);
  public listadoTerritorio = signal<any[]>([]);
  public listadoPaises = signal<any[]>([]);
  public listadoCCAA = signal<any[]>([]);
  public listadoProvincia = signal<any[]>([]);
  public listadoMunicipio = signal<any[]>([]);

  public dataSource = new MatTableDataSource<any>([]);

  public displayedColumns: string[] = ['fecha', 'eventType', 'status', 'denominacion', 'opciones'];
  public displayedColumnsRelations: string[] = ['fecha', 'eventType', 'status', 'denominacion', 'opciones'];

  public isSaving = signal<boolean>(false);

  public formData!: FormGroup;

  myForm!: FormGroup;

  public provinceService = inject(ProvinceService);
  public countryService = inject(CountryService);
  public eventService = inject(EventService);

  async ngOnInit() {
    this.spinner.show();
    try {
      const fb = new FormBuilder();
      this.myForm = fb.group({
        selectField: ['', Validators.required],
        inputField1: ['', Validators.required],
        inputField2: ['', Validators.required],
      });

      this.formData = new FormGroup({
        name: new FormControl(''),
        claseSuceco: new FormControl(1),
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

      const fireStatus = await this.masterData.getFireStatus();
      this.fireStatus.set(fireStatus);

      const severityLevels = await this.severityLevelService.get();
      this.severityLevels.set(severityLevels);

      const eventStatus = await this.eventStatusService.get();
      this.eventStatus.set(eventStatus);

      const moves = await this.moveService.get();
      this.moves.set(moves);

      const comparativeDates = await this.comparativeDateService.get();
      this.comparativeDates.set(comparativeDates);

      if (this.fireDetail) {
        const listadoSucesosRelacionados = await this.sucesosRelacionadosService.get(this.fireDetail.id);
        this.listaSucesosRelacionados.set({ data: listadoSucesosRelacionados });
      }

      await this.loadCommunities();

      this.spinner.hide();

      await this.onSubmit();
    } catch (error) {
      console.error('error', error);
      this.spinner.hide();
    }
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
    this.loadCommunities(event.value.id == 1 ? this.COUNTRIES_ID.SPAIN : '9999');
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
    this.spinner.show();
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
      Page: 0,
      //PageSize: 0,
      //Sort: '',
      search: name,
    });

    const dataFiltrada = listadoSucesos.data.filter(
      (listadoSuceso: any) =>
        !this.listaSucesosRelacionados()?.data?.sucesosAsociados?.some((sucesoRelacionado: any) => sucesoRelacionado.id === listadoSuceso.id)
    );

    this.listaSucesos.set({ data: dataFiltrada });
    this.spinner.hide();
  }

  async guardarAgregar() {
    this.spinner.show();
    if (this.isSaving()) {
      this.spinner.hide();
      return;
    }
    this.isSaving.set(true);

    const idsSucesosAsociados = this.listaSucesosRelacionados()?.data?.sucesosAsociados?.map((item: any) => item.id);

    if (idsSucesosAsociados?.length === 0) {
      //this.spinner.hide();
      /*
      this.alertService
        .showAlert({
          title: 'Advertencia!',
          text: 'Debe meter almenos un Suceso Relacionado',
          icon: 'error',
        })
        .then((result) => {
          this.isSaving.set(false);
          return;
        });
        */

      // PCD
      this.snackBar
        .open('Debe introducir al menos un suceso!', '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-rojo'],
        })
        .afterDismissed()
        .subscribe(() => {
          this.isSaving.set(false);
          this.spinner.hide();
          return;
        });
      // FIN PCD
    } else {
      try {
        const respSucesosRelacionados: any = await this.sucesosRelacionadosService.post({
          idsSucesosAsociados,
          idSucesoRelacionado: this.fireDetail?.id ?? 0,
          idSuceso: this.fire.idSuceso,
        });

        const listadoSucesosRelacionados = await this.sucesosRelacionadosService.get(respSucesosRelacionados.idSucesoRelacionado);

        this.listaSucesosRelacionados.set({ data: listadoSucesosRelacionados });
        //this.spinner.hide();
        await this.onSubmit();

        /*
        this.alertService
          .showAlert({
            title: 'Buen trabajo!',
            text: 'Registro actualizado correctamente!',
            icon: 'success',
          })
          .then((result) => {
            this.closeModal.emit();
            this.isSaving.set(false);
          });
          */

        this.snackBar
          .open('Registro guardado correctamente!', '', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
            panelClass: ['snackbar-verde'],
          })
          .afterDismissed()
          .subscribe(() => {
            this.closeModal.emit();
            this.isSaving.set(false);
            this.spinner.hide();
          });
        // FIN PCD
      } catch (error) {
        console.error('error', error);
        this.alertService
          .showAlert({
            title: 'Ha ocurrido un error!',
            text: 'Contacte a soporte técnico!',
            icon: 'error',
          })
          .then((result) => {
            this.closeModal.emit();

            this.isSaving.set(false);
          });
      }
    }
  }

  async handleSeleccionarItem(i: any) {
    const newLista: any = this.listaSucesos();
    newLista.data[i].selected = !newLista.data[i].selected;
    this.listaSucesos.set(newLista);
  }

  agregarItem() {
    this.spinner.show();
    const newData = this.listaSucesosRelacionados();

    const dataPush = this.listaSucesos()?.data?.filter((item: any) => item.selected);

    if (newData?.data?.sucesosAsociados) {
      newData.data.sucesosAsociados = [...newData.data.sucesosAsociados, ...dataPush];
    } else {
      newData.data.sucesosAsociados = [...dataPush];
    }

    const newDataOptions = this.listaSucesos()?.data?.filter((item: any) => !item.selected);

    this.listaSucesos.set({ data: newDataOptions });
    this.listaSucesosRelacionados.set({ ...newData });
    this.spinner.hide();
  }

  async eliminarItem(i: any) {
    this.spinner.show();
    const newListaSucesos = [...this.listaSucesos().data, { ...this.listaSucesosRelacionados().data.sucesosAsociados[i], selected: false }];
    this.listaSucesos.set({ data: newListaSucesos });
    /*
    try {
      await this.sucesosRelacionadosService.delete(this.listaSucesosRelacionados().data.sucesosAsociados[i].id);
    } catch (error) {}
     */

    const newAsociados: any = this.listaSucesosRelacionados().data.sucesosAsociados.filter(
      (asociado: any) => asociado.id !== this.listaSucesosRelacionados().data.sucesosAsociados[i].id
    );

    this.listaSucesosRelacionados.set({
      data: {
        ...this.listaSucesosRelacionados().data,
        sucesosAsociados: newAsociados,
      },
    });
    this.spinner.hide();
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
