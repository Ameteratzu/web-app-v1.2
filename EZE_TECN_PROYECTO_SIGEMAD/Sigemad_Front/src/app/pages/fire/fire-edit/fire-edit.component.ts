import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';

import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';

import { FireEvolutionCreateComponent } from '../../fire-evolution-create/fire-evolution-create.component';
import { MapCreateComponent } from '../../map-create/map-create.component';

import { EventService } from '../../../services/event.service';
import { FireStatusService } from '../../../services/fire-status.service';
import { FireService } from '../../../services/fire.service';
import { MenuItemActiveService } from '../../../services/menu-item-active.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';

import { Event } from '../../../types/event.type';
import { FireDetail } from '../../../types/fire-detail.type';
import { FireStatus } from '../../../types/fire-status.type';
import { Fire } from '../../../types/fire.type';
import { Municipality } from '../../../types/municipality.type';
import { Province } from '../../../types/province.type';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

@Component({
  selector: 'app-fire-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    InputTextModule,
    DropdownModule,
    CalendarModule,
    InputTextareaModule,
  ],
  templateUrl: './fire-edit.component.html',
  styleUrl: './fire-edit.component.css',
})
export class FireEditComponent {
  public route = inject(ActivatedRoute);

  public matDialog = inject(MatDialog);

  public menuItemActiveService = inject(MenuItemActiveService);
  public fireService = inject(FireService);
  public provinceService = inject(ProvinceService);
  public municipalityService = inject(MunicipalityService);
  public eventService = inject(EventService);
  public fireStatusService = inject(FireStatusService);

  public fire = <Fire>{};
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public events = signal<Event[]>([]);
  public fireStatus = signal<FireStatus[]>([]);
  public logs = signal<FireDetail[]>([]);

  public formData: FormGroup;

  public error: boolean = false;

  public showUpdateLog: boolean = true;
  public showDetailsUpdate: boolean = false;

  public details = [
    {
      reg: '10',
      datetime: '19/08/2024 19:45',
      scope: 'Personas',
      type: 'Evacuados',
      implication: 'Santa María (134)',
    },
    {
      reg: '10',
      datetime: '19/08/2024 19:25',
      scope: 'Viabilidad',
      type: 'Meteorológica',
      implication: 'CN-21 (Corte PK 2,300-3,100)',
    },
    {
      reg: '9',
      datetime: '19/08/2024 19:15',
      scope: 'Medios estatales',
      type: 'Extraordinario',
      implication: 'UME (Aprobación - Salida)',
    },
    {
      reg: '8',
      datetime: '19/08/2024 19:12',
      scope: 'Medios estatales',
      type: 'Extraordinario',
      implication: 'UME (Aprobación - Entrada)',
    },
    {
      reg: '7',
      datetime: '19/08/2024 18:15',
      scope: 'Dirección',
      type: 'Entrada',
      implication: 'COCOPI (Inicio)',
    },
    {
      reg: '6',
      datetime: '19/08/2024 18:10',
      scope: 'Medios estatales',
      type: 'Extraordinario',
      implication: 'UME (Solicitud - Salida)',
    },
  ];

  async ngOnInit() {
    localStorage.removeItem('coordinates');

    this.menuItemActiveService.set.emit('/fire');

    this.formData = new FormGroup({
      id: new FormControl(),
      denomination: new FormControl(),
      territory: new FormControl(),
      province: new FormControl(),
      municipality: new FormControl(),
      startDate: new FormControl(),
      event: new FormControl(),
      generalNote: new FormControl(),
      idEstado: new FormControl(),
    });

    const fire_id = this.route.snapshot.paramMap.get('id');

    const fires = await this.fireService.get();

    for (let fire of fires.data) {
      if (fire.id == Number(fire_id)) {
        this.fire = fire;
      }
    }

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    const municipalities = await this.municipalityService.get(
      this.fire.incendioNacional.idProvincia
    );
    this.municipalities.set(municipalities);

    const events = await this.eventService.get();
    this.events.set(events);

    const fireStatus = await this.fireStatusService.get();
    this.fireStatus.set(fireStatus);

    const details = await this.fireService.details(Number(fire_id));
    this.logs.set(details);

    this.formData.patchValue({
      id: this.fire.id,
      territory: this.fire.idTerritorio,
      denomination: this.fire.denominacion,
      province: this.fire.incendioNacional.idProvincia,
      municipality: this.fire.incendioNacional.idMunicipio,
      startDate: new Date(this.fire.fechaInicio),
      event: this.fire.idClaseSuceso,
      generalNote: this.fire.notaGeneral,
      idEstado: this.fire.idEstadoSuceso,
    });
  }

  async loadMunicipalities(event: any) {
    const province_id = event.target.value;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
  }

  async onSubmit() {
    this.error = false;
    const data = this.formData.value;
    data.coordinates = JSON.parse(
      localStorage.getItem('coordinates') ||
        JSON.stringify(this.fire.geoPosicion.coordinates)
    );

    await this.fireService
      .update(data)
      .then((response) => {
        window.location.href = window.location.href;
      })
      .catch((error) => {
        this.error = true;
      });
  }

  async confirmDelete() {
    if (confirm('¿Está seguro que desea eliminar este incendio?')) {
      const fire_id = Number(this.route.snapshot.paramMap.get('id'));
      await this.fireService.delete(fire_id);
      window.location.href = '/fire';
    }
  }

  openModalMapEdit() {
    this.matDialog.open(MapCreateComponent, {
      width: '1000px',
      maxWidth: '1000px',
    });
  }

  openModalEvolution() {
    let evolutionModalRef = this.matDialog.open(FireEvolutionCreateComponent, {
      width: '1220px',
      maxWidth: '1220px',
      height: '720px',
      disableClose: true,
    });

    evolutionModalRef.componentInstance.fire_id = Number(
      this.route.snapshot.paramMap.get('id')
    );
  }

  showTable(table: string) {
    this.showUpdateLog = false;
    this.showDetailsUpdate = false;

    if (table == 'showUpdateLog') {
      this.showUpdateLog = true;
    }

    if (table == 'showDetailsUpdate') {
      this.showDetailsUpdate = true;
    }
  }
}
