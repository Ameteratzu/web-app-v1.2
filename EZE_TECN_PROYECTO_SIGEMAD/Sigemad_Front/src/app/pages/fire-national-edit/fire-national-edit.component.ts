import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';

import { MapCreateComponent } from '../map-create/map-create.component';

import { MenuItemActiveService } from '../../services/menu-item-active.service';
import { FireService } from '../../services/fire.service';
import { ProvinceService } from '../../services/province.service';
import { MunicipalityService } from '../../services/municipality.service';
import { EventService } from '../../services/event.service';

import { Fire } from '../../types/fire.type';
import { Province } from '../../types/province.type';
import { Municipality } from '../../types/municipality.type';
import { Event } from '../../types/event.type';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

@Component({
  selector: 'app-fire-national-edit',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, FormsModule
  ],
  templateUrl: './fire-national-edit.component.html',
  styleUrl: './fire-national-edit.component.css'
})
export class FireNationalEditComponent {
  public route = inject(ActivatedRoute);

  public matDialog = inject(MatDialog);

  public menuItemActiveService = inject(MenuItemActiveService);
  public fireService = inject(FireService);
  public provinceService = inject(ProvinceService);
  public municipalityService = inject(MunicipalityService);
  public eventService = inject(EventService);

  public fire = <Fire>({});
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public events = signal<Event[]>([]);

  public formData: FormGroup;

  public error:boolean = false;

  public items = [
    {
      id: 1,
      datetime: '12/08/2024 00:00',
      record: 'Entrada',
      origin: 'MITECO',
      record_type: 'Datos de evolución',
      technical: 'sacop1'
    },
    {
      id: 2,
      datetime: '12/08/2024 00:00',
      record: 'Salida',
      origin: 'DOPCE',
      record_type: 'Datos de evolución',
      technical: 'sacop1'
    },
    {
      id: 3,
      datetime: '12/08/2024 00:00',
      record: 'Salida',
      origin: 'DOPCE',
      record_type: 'Otra infromación',
      technical: 'sacop1'
    },
    {
      id: 4,
      datetime: '12/08/2024 00:00',
      record: 'Interna',
      origin: 'DGPCyE',
      record_type: 'Dirección y coordinación',
      technical: 'sacop1'
    },
    {
      id: 5,
      datetime: '12/08/2024 00:00',
      record: 'Entrada',
      origin: 'MITECO',
      record_type: 'Datos de evolución',
      technical: 'sacop1'
    },
    {
      id: 6,
      datetime: '12/08/2024 00:00',
      record: 'Interna',
      origin: 'DGPCyE',
      record_type: 'Actuaciones relevantes',
      technical: 'sacop1'
    },
    {
      id: 7,
      datetime: '12/08/2024 00:00',
      record: 'Entrada',
      origin: 'DOPCE',
      record_type: 'Documentación',
      technical: 'sacop1'
    },
    {
      id: 8,
      datetime: '12/08/2024 00:00',
      record: 'Salida',
      origin: 'MITECO',
      record_type: 'Otra información',
      technical: 'sacop1'
    },
    {
      id: 9,
      datetime: '12/08/2024 00:00',
      record: 'Entrada',
      origin: 'DOPCE',
      record_type: 'Documentación',
      technical: 'sacop1'
    },
    {
      id: 10,
      datetime: '12/08/2024 00:00',
      record: 'Salida',
      origin: 'MITECO',
      record_type: 'Otra información',
      technical: 'sacop1'
    },
  ];

  async ngOnInit() {
    localStorage.removeItem('coordinates');
    
    this.menuItemActiveService.set.emit('/fire');

    this.formData = new FormGroup({
      id: new FormControl(),
      name: new FormControl(),
      territory: new FormControl(),
      province: new FormControl(),
      municipality: new FormControl(),
      start: new FormControl(),
      event: new FormControl(),
      note: new FormControl(),
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

    const municipalities = await this.municipalityService.get(this.fire.idProvincia);
    this.municipalities.set(municipalities);

    const events = await this.eventService.get();
    this.events.set(events);

    this.formData.patchValue({
      id: this.fire.id,
      territory: this.fire.idTerritorio,
      name: this.fire.denominacion,
      province: this.fire.idProvincia,
      municipality: this.fire.idMunicipio,
      start: this.fire.fechaInicio,
      event: this.fire.idClaseSuceso,
      note: this.fire.comentarios,
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
    data.coordinates = JSON.parse(localStorage.getItem('coordinates') || '{}');

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
}
