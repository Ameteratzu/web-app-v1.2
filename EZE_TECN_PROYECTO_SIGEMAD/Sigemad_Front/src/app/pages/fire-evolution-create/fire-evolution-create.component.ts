import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';

import { MatDialog, MatDialogRef } from '@angular/material/dialog';

import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MultiSelectModule } from 'primeng/multiselect';

import { FireStatusService } from '../../services/fire-status.service';

import { EvolutionService } from '../../services/evolution.service';
import { ImpactEvolutionService } from '../../services/impact-evolution.service';
import { ImpactGroupService } from '../../services/impact-group.service';
import { ImpactTypeService } from '../../services/impact-type.service';
import { ImpactService } from '../../services/impact.service';
import { InputOutputService } from '../../services/input-output.service';
import { InterveningMediaService } from '../../services/intervening-media.service';
import { MediaClassificationService } from '../../services/media-classification.service';
import { MediaOwnershipService } from '../../services/media-ownership.service';
import { MediaTypeService } from '../../services/media-type.service';
import { MediaService } from '../../services/media.service';
import { MinorEntityService } from '../../services/minor-entity.service';
import { MunicipalityService } from '../../services/municipality.service';
import { OriginDestinationService } from '../../services/origin-destination.service';
import { ProvinceService } from '../../services/province.service';
import { RecordTypeService } from '../../services/record-type.service';

import { FireStatus } from '../../types/fire-status.type';
import { Impact } from '../../types/impact.type';
import { InputOutput } from '../../types/input-output.type';
import { MediaClassification } from '../../types/media-classification.type';
import { MediaOwnership } from '../../types/media-ownership.type';
import { MediaType } from '../../types/media-type.type';
import { Media } from '../../types/media.type';
import { MinorEntity } from '../../types/minor-entity.type';
import { Municipality } from '../../types/municipality.type';
import { OriginDestination } from '../../types/origin-destination.type';
import { Province } from '../../types/province.type';
import { RecordType } from '../../types/record-type.type';

import { MapCreateComponent } from '../map-create/map-create.component';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { AreasAffectedService } from '../../services/areas-affected.service';
import { CamposImpactoService } from '../../services/campos-impacto.service';
import { CampoDinamico } from '../../shared/campoDinamico/campoDinamico.component';
import { Campo } from '../../types/Campo.type';

@Component({
  selector: 'app-fire-evolution-create',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    CalendarModule,
    DropdownModule,
    InputTextModule,
    InputTextareaModule,
    MultiSelectModule,
    CampoDinamico,
  ],
  templateUrl: './fire-evolution-create.component.html',
  styleUrl: './fire-evolution-create.component.css',
})
export class FireEvolutionCreateComponent {
  public activeTab: string = 'Registro';

  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);
  public mediaService = inject(MediaService);
  public originDestinationService = inject(OriginDestinationService);
  public fireStatusService = inject(FireStatusService);
  public provinceService = inject(ProvinceService);
  public municipalityService = inject(MunicipalityService);
  public impactTypeService = inject(ImpactTypeService);
  public impactGroupService = inject(ImpactGroupService);
  public areaAffectedService = inject(AreasAffectedService);
  public impactService = inject(ImpactService);
  public impactEvolutionService = inject(ImpactEvolutionService);
  public mediaClassificationService = inject(MediaClassificationService);
  public mediaOwnershipService = inject(MediaOwnershipService);
  public inputOutputService = inject(InputOutputService);
  public mediaTypeService = inject(MediaTypeService);
  public recordTypeService = inject(RecordTypeService);
  public minorEntityService = inject(MinorEntityService);
  public evolutionService = inject(EvolutionService);
  public interveningMediaService = inject(InterveningMediaService);
  public camposImpactoService = inject(CamposImpactoService);

  public medias = signal<Media[]>([]);
  public originDestinations = signal<OriginDestination[]>([]);
  public status = signal<FireStatus[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public municipalities2 = signal<Municipality[]>([]);
  public impacts = signal<Impact[]>([]);
  public mediaClassifications = signal<MediaClassification[]>([]);
  public mediaOwnerships = signal<MediaOwnership[]>([]);
  public inputOutputs = signal<InputOutput[]>([]);
  public mediaTypes = signal<MediaType[]>([]);
  public recordTypes = signal<RecordType[]>([]);
  public minorEntities = signal<MinorEntity[]>([]);
  //se esta creando o modificando un campo
  public isCreate = signal<boolean>(false);

  public fieldCampos = signal<Campo[]>([]);

  public disabledBenefits = signal<boolean>(false);
  public disabledOwner1 = signal<boolean>(false);

  public dinamicDataConsecuencesActions = signal<any>({});

  public impactTypes: any;
  public impactGroups: any;

  public areasAffected = [] as any;
  public consequencesActions = [] as any;

  public interveningMedias = [] as any;
  public denominations = [] as any;

  public formGroup: FormGroup;

  public fire_id: number;
  public evolution_id: any;
  public errors: any;

  public consequenceActionError: boolean = false;
  public areaAffectedActionError: boolean = false;
  public interveningMediaError: boolean = false;
  public errorAreaAfectada: boolean = false;

  public type: string = '';
  public group: string = '';

  async ngOnInit() {
    this.formGroup = new FormGroup({
      datetime: new FormControl(),
      inputOutput: new FormControl(),
      type: new FormControl(),
      media: new FormControl(),
      originDestination: new FormControl(),
      datetimeUpdate: new FormControl(),
      recordType: new FormControl(),
      observations_1: new FormControl(),
      forecast: new FormControl(),
      status: new FormControl(),
      affectedSurface: new FormControl(),
      end_date: new FormControl(),
      emergencyPlanActivated: new FormControl(),

      startAreaAffected: new FormControl(),
      province_1: new FormControl(),
      municipality_1: new FormControl(),
      minorEntity: new FormControl(),
      georeferencedFile: new FormControl(),
      observationsAreaAffected: new FormControl(),

      areaAffectedActionUpdate: new FormControl(),
      areaAffectedActionIndex: new FormControl(),

      consequenceActionUpdate: new FormControl(),
      consequenceActionIndex: new FormControl(),
      impactType: new FormControl(),
      impactGroup: new FormControl(),
      name: new FormControl(),
      number: new FormControl(),
      observations_2: new FormControl(),
      end: new FormControl(),
      start: new FormControl(),
      injureds: new FormControl(),
      participants: new FormControl(),

      interveningMediaIndex: new FormControl(),
      interveningMediaUpdate: new FormControl(),
      mediaType: new FormControl(),
      quantity: new FormControl(),
      unit: new FormControl(),
      classification: new FormControl(),
      ownership_1: new FormControl(),
      ownership_2: new FormControl(),
      province_2: new FormControl(),
      municipality_2: new FormControl(),
      observations_3: new FormControl(),

      startDate: new FormControl(),
    });

    const ID_EJEMPLO_CAMPOS_IMPACTO = `1`;

    const impactTypes = await this.impactTypeService.get();
    this.impactTypes = impactTypes;

    this.formGroup.patchValue({
      number: 1,
      injureds: 22,
      participants: 15,
      ownership_2: 'Comunidad Farol de Navarra',
    });

    localStorage.clear();

    const medias = await this.mediaService.get();
    this.medias.set(medias);

    const originDestinations = await this.originDestinationService.get();

    this.originDestinations.set(originDestinations);

    const status = await this.fireStatusService.get();
    this.status.set(status);

    const provinces = await this.provinceService.get();
    this.provinces.set(provinces);

    const impactGroups = await this.impactGroupService.get();
    this.impactGroups = impactGroups;

    const impacts = await this.impactService.get();
    this.impacts.set(impacts);

    const mediaClassifications = await this.mediaClassificationService.get();
    this.mediaClassifications.set(mediaClassifications);

    const mediaOwnerships = await this.mediaOwnershipService.get();
    this.mediaOwnerships.set(mediaOwnerships);

    const inputOutputs = await this.inputOutputService.get();
    this.inputOutputs.set(inputOutputs);

    const mediaTypes = await this.mediaTypeService.get();

    this.mediaTypes.set(mediaTypes);

    const recordTypes = await this.recordTypeService.get();
    this.recordTypes.set(recordTypes);

    const minorEntities = await this.minorEntityService.get();
    this.minorEntities.set(minorEntities);
  }

  getDataForConsecuenciasActuaciones(datos: any) {
    this.dinamicDataConsecuencesActions.set(datos);
  }

  public changeMediaTypes(event: any) {
    const mediaTypeSelected = this.mediaTypes().find(
      (mediaType) => mediaType.id === event.value
    );

    let clasificacionMedio: any = mediaTypeSelected?.clasificacionMedio
      ? mediaTypeSelected.clasificacionMedio
      : null;
    let titularidadMedio: any = mediaTypeSelected?.titularidadMedio
      ? mediaTypeSelected.titularidadMedio
      : null;

    this.formGroup.patchValue({
      classification: clasificacionMedio?.id,
      ownership_1: titularidadMedio?.id,
    });

    this.disabledBenefits.update(() => (clasificacionMedio ? true : false));
    this.disabledOwner1.update(() => (titularidadMedio ? true : false));
  }

  public changeTab(tab: string) {
    this.activeTab = tab;
  }

  public closeModal() {
    this.matDialogRef.close();
  }

  public async loadMunicipalities(event: any, input: string) {
    const province_id = event.value;
    const municipalities = await this.municipalityService.get(province_id);

    if (input == '1') {
      this.municipalities.set(municipalities);
    }

    if (input == '2') {
      this.municipalities2.set(municipalities);
    }
  }

  public openModalMapCreate(section: string = '') {
    let mapModalRef = this.matDialog.open(MapCreateComponent, {
      width: '1000px',
      maxWidth: '1000px',
    });

    mapModalRef.componentInstance.section = section;
  }

  public saveAreaAffected() {
    const { value } = this.formGroup;
    this.areaAffectedActionError = false;
    if (
      !value.startAreaAffected ||
      !value.province_1 ||
      !value.municipality_1 ||
      !value.minorEntity
    ) {
      this.areaAffectedActionError = true;
      return;
    }
    const newAreaAffected = {
      startAreaAffected: value.startAreaAffected,
      province_1: value.province_1,
      municipality_1: value.municipality_1,
      minorEntity: value.minorEntity,
      observationAreaAffeted: value.observationsAreaAffected,
      geoPosicion: '{}',
    };

    if (value.areaAffectedActionUpdate == '1') {
      this.areasAffected.splice(
        value.areaAffectedActionIndex,
        1,
        newAreaAffected
      );
    } else {
      this.areasAffected.push(newAreaAffected);
    }

    this.formGroup.patchValue({
      startAreaAffected: '',
      province_1: '',
      municipality_1: '',
      minorEntity: '',
      observationsAreaAffected: '',
      areaAffectedActionUpdate: '',
      areaAffectedActionIndex: '',
    });
  }

  public showAreaAffectedDataInForm(index: number) {
    const areaAffected = this.areasAffected[index];

    this.formGroup.patchValue({
      areaAffectedActionUpdate: '1',
      areaAffectedActionIndex: index,
      startAreaAffected: new Date(areaAffected.startAreaAffected),
      province_1: areaAffected.province_1,
      municipality_1: areaAffected.municipality_1,
      minorEntity: areaAffected.minorEntity,
      observationsAreaAffected: `${areaAffected.observationAreaAffeted}`,
    });
  }

  public deleteAreaAffected(index: number) {
    if (confirm('Está seguro que desea eliminar?')) {
      this.areasAffected.splice(index, 1);
    }
  }
  /*
  public saveConsequenceAction() {
    const data = this.formGroup.value;

    this.consequenceActionError = false;

    if (
      !data.impactType ||
      !data.impactGroup ||
      !data.name ||
      !data.number ||
      !data.observations_2 ||
      !data.start ||
      !data.end ||
      !data.injureds ||
      !data.participants
    ) {
      this.consequenceActionError = true;
      return;
    }

    if (data.consequenceActionUpdate == '1') {
      const replace = {
        impactType: data.impactType,
        impactGroup: data.impactGroup,
        name: data.name,
        number: data.number,
        observations_2: data.observations_2,
        start: data.start,
        end: data.end,
        injureds: data.injureds,
        participants: data.participants,
      };

      this.consequencesActions.splice(data.consequenceActionIndex, 1, replace);
    } else {
      this.consequencesActions.push({
        impactType: data.impactType,
        impactGroup: data.impactGroup,
        name: data.name,
        number: data.number,
        observations_2: data.observations_2,
        start: data.start,
        end: data.end,
        injureds: data.injureds,
        participants: data.participants,
      });
    }

    this.formGroup.patchValue({
      consequenceActionIndex: '',
      consequenceActionUpdate: '',
      impactType: '',
      impactGroup: '',
      name: '',
      number: '',
      observations_2: '',
      start: '',
      end: '',
      injureds: '',
      participants: '',
    });
  }
  */
  public saveConsequenceAction() {
    this.dinamicDataConsecuencesActions;
    this.consequenceActionError = false;
    const fieldsRequired = this.fieldCampos().filter(
      (item: any) => item.esObligatorio
    );

    const data = this.formGroup.value;

    this.consequenceActionError = false;

    if (
      !data.impactType ||
      !data.impactGroup ||
      !data.name ||
      !data.number ||
      !data.observations_2
    ) {
      this.consequenceActionError = true;
      return;
    }

    const fieldError = fieldsRequired.some((field) => {
      return this.dinamicDataConsecuencesActions()[field.campo] ? false : true;
    });

    if (fieldError) {
      this.consequenceActionError = true;
      return;
    }

    if (data.consequenceActionUpdate == '1') {
      const replace = {
        impactType: data.impactType,
        impactGroup: data.impactGroup,
        name: data.name,
        number: data.number,
        observations_2: data.observations_2,
        ...this.dinamicDataConsecuencesActions(),
      };

      this.consequencesActions.splice(data.consequenceActionIndex, 1, replace);
    } else {
      this.consequencesActions.push({
        impactType: data.impactType,
        impactGroup: data.impactGroup,
        name: data.name,
        number: data.number,
        observations_2: data.observations_2,
        ...this.dinamicDataConsecuencesActions(),
      });
    }

    this.formGroup.patchValue({
      consequenceActionIndex: '',
      consequenceActionUpdate: '',
      impactType: '',
      impactGroup: '',
      name: '',
      number: '',
      observations_2: '',
    });
    this.fieldCampos.set([]);
  }

  public async showConsequenceDataInForm(index: number) {
    const item = this.consequencesActions[index];
    const { impactType, impactGroup, name, number, observations_2, ...res } =
      item;
    this.formGroup.patchValue({
      consequenceActionIndex: index,
      consequenceActionUpdate: '1',
      impactType: impactType,
      impactGroup: impactGroup,
      name: name,
      number: number,
      observations_2: observations_2,
    });

    const camposImpacto = await this.camposImpactoService.getFieldsById(
      item.impactType === 'Consecuencia' ? `1` : `2`
    );
    const newCamposImpacto = camposImpacto.map((item) => {
      return {
        ...item,
        initValue: res[item.campo],
      };
    });
    this.fieldCampos.set(newCamposImpacto);
  }

  public showInterveningMediaDataInForm(index: number) {
    const item = this.interveningMedias[index];

    this.formGroup.patchValue({
      interveningMediaIndex: index,
      interveningMediaUpdate: 1,
      mediaType: item.mediaType,
      quantity: item.quantity,
      unit: item.unit,
      classification: item.classification,
      ownership_1: item.ownership_1,
      ownership_2: item.ownership_2,
      province_2: item.province_2,
      municipality_2: item.municipality_2,
      observations_3: item.observations_3,
    });
  }

  public deleteConsequence(index: number) {
    if (confirm('Está seguro que desea eliminar?')) {
      this.consequencesActions.splice(index, 1);
    }
  }

  public deleteInterveningMedia(index: number) {
    if (confirm('Está seguro que desea eliminar?')) {
      this.interveningMedias.splice(index, 1);
    }
  }

  public saveInterveningMedia() {
    this.interveningMediaError = false;
    const data = this.formGroup.value;

    if (
      !data.mediaType ||
      !data.quantity ||
      !data.unit ||
      !data.classification ||
      !data.ownership_1 ||
      !data.ownership_2 ||
      !data.province_2 ||
      !data.municipality_2 ||
      !data.observations_3
    ) {
      this.interveningMediaError = true;
      return;
    }

    if (data.interveningMediaUpdate == '1') {
      const replace = {
        mediaType: data.mediaType,
        quantity: data.quantity,
        unit: data.unit,
        classification: data.classification,
        ownership_1: data.ownership_1,
        ownership_2: data.ownership_2,
        province_2: data.province_2,
        municipality_2: data.municipality_2,
        observations_3: data.observations_3,
      };

      this.interveningMedias.splice(data.interveningMediaIndex, 1, replace);
    } else {
      this.interveningMedias.push({
        mediaType: data.mediaType,
        quantity: data.quantity,
        unit: data.unit,
        classification: data.classification,
        ownership_1: data.ownership_1,
        ownership_2: data.ownership_2,
        province_2: data.province_2,
        municipality_2: data.municipality_2,
        observations_3: data.observations_3,
      });
    }

    this.formGroup.patchValue({
      interveningMediaIndex: '',
      interveningMediaUpdate: '',
      mediaType: '',
      quantity: '',
      unit: '',
      classification: '',
      ownership_1: '',
      ownership_2: '',
      province_2: '',
      municipality_2: '',
      observations_3: '',
    });
  }

  public async submit() {
    const data = this.formGroup.value;
    data.fire_id = this.fire_id;

    this.errorAreaAfectada = false;
    this.errors = false;

    if (!localStorage.getItem('coordinatesAreaAfectada')) {
      this.errorAreaAfectada = true;
      return;
    }

    data.coordinatesAreaAfectada = JSON.parse(
      localStorage.getItem('coordinatesAreaAfectada') ?? '{}'
    );

    if (localStorage.getItem('polygonAreaAfectada')) {
      data.geoPosicionAreaAfectada = {
        type: 'Polygon',
        coordinates: [data.coordinatesAreaAfectada],
      };
    } else {
      data.geoPosicionAreaAfectada = {
        type: 'Point',
        coordinates: data.coordinatesAreaAfectada,
      };
    }

    data.coordinatesIntervencionMedios = JSON.parse(
      localStorage.getItem('coordinatesIntervencionMedios') ?? '{}'
    );

    if (localStorage.getItem('polygonIntervencionMedios')) {
      data.geoPosicion = {
        type: 'Polygon',
        coordinates: [data.coordinatesIntervencionMedios],
      };
    } else {
      data.geoPosicion = {
        type: 'Point',
        coordinates: data.coordinatesIntervencionMedios,
      };
    }

    await this.evolutionService
      .post({
        ...data,
        consequencesActions: this.consequencesActions,
        interveningMedias: this.interveningMedias,
        areasAffected: this.areasAffected,
      })
      .then((response) => {
        this.evolution_id = response;
        this.evolution_id = this.evolution_id.id;
      })
      .catch((error) => {
        this.errors = error.errors;
        const element = document.getElementById('validation-evolution-error');
        setTimeout(() => {
          element?.scrollIntoView();
        }, 1000);
      });

    if (this.errors) {
      return;
    }

    for (let consequence of this.consequencesActions) {
      const { impactType, impactGroup, name, number, observations_2, ...res } =
        consequence;
      const consequenceAction = {
        idEvolucion: this.evolution_id,
        IdImpactoClasificado: impactType === 'Consecuencia' ? 1 : 2,
        observaciones: observations_2,
        numero: number,
        idImpactGroup: impactGroup, // ???
        name: name, // ???
        ...res,
      };

      await this.impactEvolutionService.post(consequenceAction);
    }

    for (let intervening of this.interveningMedias) {
      const interveningMedia = {
        idEvolucion: this.evolution_id,
        idCaracterMedio: 1,
        idTipoIntervencionMedio: intervening.mediaType,
        idClasificacionMedio: intervening.classification,
        idTitularidadMedio: intervening.ownership_1,
        idMunicipio: intervening.municipality_2,
        cantidad: intervening.quantity,
        unidad: intervening.unit,
        titular: intervening.ownership_2,
        observaciones: intervening.observations_3,
        geoPosicion: data.geoPosicion,
      };

      await this.interveningMediaService.post(interveningMedia);
    }

    for (let areaAffected of this.areasAffected) {
      const { value } = this.formGroup;

      const bodyAreaAffected = {
        idEvolucion: this.evolution_id,
        fechaHora: areaAffected.startAreaAffected,
        idProvincia: areaAffected.province_1,
        idMunicipio: areaAffected.municipality_1,
        idEntidadMenor: areaAffected.minorEntity,
        geoPosicion: data.geoPosicion,
      };

      const x = await this.areaAffectedService.post(bodyAreaAffected);
    }

    window.location.href = '/fire-national-edit/' + this.fire_id;
  }

  public getDescriptionName(id: number) {
    return 'Consecuencias';
    return this.impacts().filter((item) => item.id == id)[0].descripcion;
  }

  public getMediaTypeDescription(id: number) {
    return this.mediaTypes().filter((item) => item.id == id)[0].descripcion;
  }

  public getClassificationDescription(id: number) {
    return this.mediaClassifications().filter((item) => item.id == id)[0]
      .descripcion;
  }

  public getOwnershipDescription(id: number) {
    return this.mediaOwnerships().filter((item) => item.id == id)[0]
      .descripcion;
  }

  public async setType(event: any) {
    this.type = event.value;
    this.getDenominations();
  }

  async changeDenomination(event: any) {
    const camposImpacto = await this.camposImpactoService.getFieldsById(
      event.value === 'Consecuencia' ? `1` : `2`
    );
    this.fieldCampos.set(camposImpacto);
  }

  public setGroup(event: any) {
    this.group = event.value;
    this.getDenominations();
  }

  public getDenominations() {
    this.denominations = [];

    if (this.type && this.group) {
      const type = this.impacts().filter(
        (item) => item.descripcion == this.type
      );

      const subgrupos = type[0].grupos.filter(
        (item: any) => item.descripcion == this.group
      )[0].subgrupos;

      for (let subgrupo of subgrupos) {
        for (let clase of subgrupo.clases) {
          for (let impacto of clase.impactos) {
            this.denominations.push({
              id: impacto.id,
              descripcion: impacto.descripcion,
            });
          }
        }
      }
    }
  }

  public getDenominationName(
    type: number,
    group: number,
    denomination: number
  ) {
    const typeArr = this.impacts().filter(
      (item) => item.descripcion == this.type
    );

    const subgrupos = typeArr[0].grupos.filter(
      (item: any) => item.descripcion == this.group
    )[0].subgrupos;

    for (let subgrupo of subgrupos) {
      for (let clase of subgrupo.clases) {
        for (let impacto of clase.impactos) {
          if (denomination == impacto.id) {
            return impacto.descripcion;
          }
        }
      }
    }
  }

  scrollToSection(sectionId: string) {
    const element = document.getElementById(sectionId);

    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  }
}
