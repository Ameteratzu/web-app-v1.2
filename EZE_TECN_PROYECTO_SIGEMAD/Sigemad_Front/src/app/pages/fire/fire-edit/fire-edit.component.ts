import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { MessageService } from 'primeng/api';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';

//import { FireEvolutionCreateComponent } from '../../fire-evolution-create/fire-evolution-create.component';

import { EventService } from '../../../services/event.service';
import { EventStatusService } from '../../../services/eventStatus.service';
import { FireStatusService } from '../../../services/fire-status.service';
import { FireService } from '../../../services/fire.service';
import { MenuItemActiveService } from '../../../services/menu-item-active.service';
import { MunicipalityService } from '../../../services/municipality.service';
import { ProvinceService } from '../../../services/province.service';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import moment from 'moment';
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';
import { ToastModule } from 'primeng/toast';
import { Event } from '../../../types/event.type';
import { FireDetail } from '../../../types/fire-detail.type';
import { FireStatus } from '../../../types/fire-status.type';
import { Fire } from '../../../types/fire.type';
import { Municipality } from '../../../types/municipality.type';
import { Province } from '../../../types/province.type';
import { EventStatus } from '../../../types/eventStatus.type';
import { FireDirectionCoordinationComponent } from '../components/fire-direction-coordination/fire-direction-coordination.component';
import { MapCreateComponent } from '../../../shared/mapCreate/map-create.component';
import { FireEvolutionCreateComponent } from '../../fire-evolution-create/fire-evolution-create.component';
import { FormFieldComponent } from '../../../shared/Inputs/field.component';

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
    ToastModule,
    FormFieldComponent
  ],
  providers: [MessageService],
  templateUrl: './fire-edit.component.html',
  styleUrl: './fire-edit.component.css',
})
export class FireEditComponent {
  featuresCoords: Feature<Geometry>[] = [];

  classValidate = signal<string>('needs-validation');

  public messageService = inject(MessageService);

  public router = inject(Router);

  public route = inject(ActivatedRoute);

  public matDialog = inject(MatDialog);

  public menuItemActiveService = inject(MenuItemActiveService);
  public fireService = inject(FireService);
  public provinceService = inject(ProvinceService);
  public municipalityService = inject(MunicipalityService);
  public eventService = inject(EventService);
  public eventStatusService = inject(EventStatusService);
  
  public fireStatusService = inject(FireStatusService);

  public fire = <Fire>{};
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public events = signal<Event[]>([]);
  public eventsStatus = signal<EventStatus[]>([]);
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

    const fire_id = Number(this.route.snapshot.paramMap.get('id'));

    const fire = await this.fireService.getById(fire_id);
    this.fire = fire;
    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    const municipalities = await this.municipalityService.get(
      this.fire.idProvincia
    );
    this.municipalities.set(municipalities);

    const events = await this.eventService.get();
    this.events.set(events);

    const eventsStatus = await this.eventStatusService.get();
    this.eventsStatus.set(eventsStatus);

    const fireStatus = await this.fireStatusService.get();
    this.fireStatus.set(fireStatus);

    const details = await this.fireService.details(Number(fire_id));
    this.logs.set(details);

    this.formData.patchValue({
      id: this.fire.id,
      territory: this.fire.idTerritorio,
      denomination: this.fire.denominacion,
      province: this.fire.idProvincia,
      municipality: this.fire.idMunicipio,
      startDate: moment(this.fire.fechaInicio).format('YYYY-MM-DD'),
      event: this.fire.idClaseSuceso,
      generalNote: this.fire.notaGeneral,
      idEstado: this.fire.idEstadoSuceso,
    });
    //this.openModalEvolution()
  }

  async loadMunicipalities(event: any) {
    const province_id = event.target.value;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
  }

  async onSubmit() {
    if (this.formData.invalid) {
      this.formData.markAllAsTouched();
      return 
    }
    
    this.error = false;
    const data = this.formData.value;

    

    if (this.featuresCoords.length) {
      data.coordinates = this.featuresCoords;
    } else {
      const municipio = this.municipalities().find(
        (item) => item.id === this.formData.value.municipality
      );

      data.coordinates = [
        municipio?.geoPosicion?.coordinates[1],
        municipio?.geoPosicion?.coordinates[0],
      ];
    }

    await this.fireService
      .update(data)
      .then((response) => {
        this.messageService.add({
          severity: 'success',
          summary: 'Modificado',
          detail: 'Incendio modificado correctamente',
        });

        new Promise((resolve) => setTimeout(resolve, 2000)).then(() =>
          this.router.navigate([`/fire`])
        );
      })
      .catch((error) => {
        this.error = true;
      });
  }

  async confirmDelete() {
    if (confirm('¿Está seguro que desea eliminar este incendio?')) {
      const fire_id = Number(this.route.snapshot.paramMap.get('id'));

      await this.fireService
        .delete(fire_id)
        .then((response) => {
          this.messageService.add({
            severity: 'success',
            summary: 'Eliminado',
            detail: 'Incendio eliminado correctamente',
          });
          new Promise((resolve) => setTimeout(resolve, 2000)).then(
            () => (window.location.href = '/fire')
          );
        })
        .catch((error) => {
          this.error = true;
        });
    }
  }

  openModalMapEdit() {
    const municipio = this.municipalities().find(
      (item) => item.id === this.formData.value.municipality
    );

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      height: '780px',
      maxHeight: '780px',
      data: { municipio: municipio, listaMunicipios: this.municipalities() },
    });

    dialogRef.componentInstance.save.subscribe(
      (features: Feature<Geometry>[]) => {
        this.featuresCoords = features;
        console.info('this.featuresCoords', this.featuresCoords);
      }
    );
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

  openModalDireccion() {
    let evolutionModalRef = this.matDialog.open(FireDirectionCoordinationComponent, {
      width: '1220px',
      maxWidth: '1220px',
      height: '720px',
      disableClose: true,
    });

    
    
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

  back() {
    this.router.navigate([`/fire`]);
  }

  getEstadoDesc(fire: Fire) {
    const desc = fire?.estadoSuceso?.descripcion
      ? fire?.estadoSuceso?.descripcion
      : '';
    return;
    desc;
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }
}
