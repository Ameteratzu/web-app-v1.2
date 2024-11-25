import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';

import { MatDialog, MatDialogRef } from '@angular/material/dialog';

import { Router } from '@angular/router';

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

import { ToastModule } from 'primeng/toast';

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

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';
import { MessageService } from 'primeng/api';

import { AreasAffectedService } from '../../services/areas-affected.service';
import { CamposImpactoService } from '../../services/campos-impacto.service';
import { CampoDinamico } from '../../shared/campoDinamico/campoDinamico.component';
import { FormFieldComponent } from '../../shared/Inputs/field.component';
import { MapCreateComponent } from '../../shared/mapCreate/map-create.component';
import { Campo } from '../../types/Campo.type';

@Component({
  selector: 'app-fire-evolution-create',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MultiSelectModule,
    CampoDinamico,
    ToastModule,
    FormFieldComponent,
  ],
  providers: [MessageService],
  templateUrl: './fire-evolution-create.component.html',
  styleUrl: './fire-evolution-create.component.css',
})
export class FireEvolutionCreateComponent {
  public activeTab: string = 'Registro';

  public router = inject(Router);
  public messageService = inject(MessageService);
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
  public formGroupAreaAfectada: FormGroup;
  public formGroupConsecuenciasActuaciones: FormGroup;
  public formGroupIntervecionMedios: FormGroup;

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
    this.formGroupAreaAfectada = new FormGroup({
      startAreaAffected: new FormControl(''),
      province_1: new FormControl(''),
      municipality_1: new FormControl(''),
      minorEntity: new FormControl(''),
      //georeferencedFile: new FormControl(''),
      observationsAreaAffected: new FormControl(''),
      coordsAreaAffected: new FormControl(''),

      areaAffectedActionUpdate: new FormControl(''),
      areaAffectedActionIndex: new FormControl(''),
    });
    this.formGroupConsecuenciasActuaciones = new FormGroup({
      consequenceActionUpdate: new FormControl(''),
      consequenceActionIndex: new FormControl(''),
      impactType: new FormControl(''),
      impactGroup: new FormControl(''),
      name: new FormControl(''),
      number: new FormControl(''),
      observations_2: new FormControl(''),
      coordsConsecuencias: new FormControl(''),
      /*
      end: new FormControl('', [Validators.required]),
      start: new FormControl('', [Validators.required]),
      injureds: new FormControl('', [Validators.required]),
      participants: new FormControl('', [Validators.required]),
      */
    });
    this.formGroupIntervecionMedios = new FormGroup({
      interveningMediaIndex: new FormControl(''),
      interveningMediaUpdate: new FormControl(''),
      mediaType: new FormControl(''),
      quantity: new FormControl(''),
      unit: new FormControl(''),
      classification: new FormControl(''),
      ownership_1: new FormControl(''),
      ownership_2: new FormControl(''),
      province_2: new FormControl(''),
      municipality_2: new FormControl(''),
      observations_3: new FormControl(''),
      coordsIntervencionMedios: new FormControl(''),
    });

    const now = new Date();
    const formattedDate = this.formatDateToDateTimeLocalInitial(now);

    this.formGroup = new FormGroup({
      // Registro
      //datetime: new FormControl('', [Validators.required]),
      //type: new FormControl('', [Validators.required]),
      startDate: new FormControl(formattedDate, [Validators.required]),
      inputOutput: new FormControl('1', [Validators.required]),
      media: new FormControl('', [Validators.required]),
      originDestination: new FormControl('', [Validators.required]),

      // Datos principales
      datetimeUpdate: new FormControl('', [Validators.required]),
      recordType: new FormControl('1', [Validators.required]),
      observations_1: new FormControl(''),
      forecast: new FormControl(''),

      // Parametros
      status: new FormControl('1', [Validators.required]),
      affectedSurface: new FormControl(''),
      end_date: new FormControl(''),
      emergencyPlanActivated: new FormControl(''),
      operativeStatus: new FormControl(''),
    });

    const impactTypes = await this.impactTypeService.get();

    this.impactTypes = impactTypes;

    this.formGroupConsecuenciasActuaciones.patchValue({
      number: 1,
    });
    this.formGroupIntervecionMedios.patchValue({
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
      (mediaType) => mediaType.id == event.target.value
    );

    let clasificacionMedio: any = mediaTypeSelected?.clasificacionMedio
      ? mediaTypeSelected.clasificacionMedio
      : null;
    let titularidadMedio: any = mediaTypeSelected?.titularidadMedio
      ? mediaTypeSelected.titularidadMedio
      : null;

    this.formGroupIntervecionMedios.patchValue({
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
    const province_id = event.target.value;
    const municipalities = await this.municipalityService.get(province_id);

    if (input == '1') {
      this.municipalities.set(municipalities);
    }

    if (input == '2') {
      this.municipalities2.set(municipalities);
    }
  }

  public openModalMapCreate(section: string = '') {
    let idMunicipio = 1;
    let listaMunicipio: any[] = [];
    if (section == 'ConsecuenciasActuaciones') {
      return;
    }
    switch (section) {
      case 'AreaAfectada':
        idMunicipio = this.formGroupAreaAfectada.value.municipality_1;
        listaMunicipio = this.municipalities();
        break;
      case 'ConsecuenciasActuaciones':
        idMunicipio =
          this.formGroupConsecuenciasActuaciones.value.municipality_1; //Esta tomando el de area afecta xq no se cual sera el de consecuencia
        listaMunicipio = this.municipalities();
        break;
      case 'IntervencionMedios':
        idMunicipio = this.formGroupIntervecionMedios.value.municipality_2;
        listaMunicipio = this.municipalities2();
        break;

      default:
        break;
    }

    const municipio = listaMunicipio.find((item) => item.id == idMunicipio);

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      height: '780px',
      maxHeight: '780px',
      data: { municipio: municipio, listaMunicipios: this.municipalities() },
    });

    dialogRef.componentInstance.section = section;

    dialogRef.componentInstance.save.subscribe(
      (features: Feature<Geometry>[]) => {
        switch (section) {
          case 'AreaAfectada':
            this.formGroupAreaAfectada.patchValue({
              coordsAreaAffected: features,
            });
            break;
          case 'ConsecuenciasActuaciones':
            this.formGroupConsecuenciasActuaciones.patchValue({
              coordsConsecuencias: features,
            });
            break;
          case 'IntervencionMedios':
            this.formGroupIntervecionMedios.patchValue({
              coordsIntervencionMedios: features,
            });
            break;

          default:
            break;
        }
        //this.featuresCoords = features; // Almacena o manipula las características aquí
      }
    );
  }

  public saveAreaAffected() {
    if (this.formGroupAreaAfectada.invalid) {
      this.formGroupAreaAfectada.markAllAsTouched();
      return;
    }
    const { value } = this.formGroupAreaAfectada;
    const newAreaAffected = {
      startAreaAffected: value.startAreaAffected,
      province_1: value.province_1,
      municipality_1: value.municipality_1,
      minorEntity: value.minorEntity,
      observationAreaAffeted: value.observationsAreaAffected,
      geoPosicion: {
        type: 'Polygon',
        coordinates: [value.coordsAreaAffected],
      },
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

    this.formGroupAreaAfectada.patchValue({
      startAreaAffected: '',
      province_1: '',
      municipality_1: '',
      minorEntity: '',
      observationsAreaAffected: '',
      areaAffectedActionUpdate: '',
      areaAffectedActionIndex: '',
      coordsAreaAffected: '{}',
    });
  }

  public showAreaAffectedDataInForm(index: number) {
    const areaAffected = this.areasAffected[index];

    this.formGroupAreaAfectada.patchValue({
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

  public saveConsequenceAction() {
    if (this.formGroupConsecuenciasActuaciones.invalid) {
      this.formGroupConsecuenciasActuaciones.markAllAsTouched();
      return;
    }

    this.dinamicDataConsecuencesActions;
    this.consequenceActionError = false;
    const fieldsRequired = this.fieldCampos().filter(
      (item: any) => item.esObligatorio
    );

    const data = this.formGroupConsecuenciasActuaciones.value;

    this.consequenceActionError = false;

    const fieldError = fieldsRequired.some((field) => {
      return this.dinamicDataConsecuencesActions()[field.campo] ? false : true;
    });

    if (fieldError) {
      this.consequenceActionError = fieldError;
      return;
    }

    const newConsequence = {
      impactType: data.impactType,
      impactGroup: data.impactGroup,
      name: data.name,
      number: data.number,
      observations_2: data.observations_2,
      coordsConsecuencias: data.coordsConsecuencias,
      geoPosicion: {
        type: 'Polygon',
        coordinates: [data.coordsConsecuencias],
      },
      ...this.dinamicDataConsecuencesActions(),
    };
    if (data.consequenceActionUpdate == '1') {
      this.consequencesActions.splice(
        data.consequenceActionIndex,
        1,
        newConsequence
      );
    } else {
      this.consequencesActions.push(newConsequence);
    }

    this.formGroupConsecuenciasActuaciones.patchValue({
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
    this.formGroupConsecuenciasActuaciones.patchValue({
      consequenceActionIndex: index,
      consequenceActionUpdate: '1',
      impactType: impactType,
      impactGroup: impactGroup,
      name: name,
      number: number,
      observations_2: observations_2,
    });

    const camposImpacto = await this.camposImpactoService.getFieldsById(
      item.impactType == 'Consecuencia' ? `1` : `2`
    );
    const newCamposImpacto = camposImpacto.map((item) => {
      return {
        ...item,
        initValue: res[item.campo],
      };
    });
    this.fieldCampos.set(newCamposImpacto);
  }

  public deleteConsequence(index: number) {
    if (confirm('Está seguro que desea eliminar?')) {
      this.consequencesActions.splice(index, 1);
    }
  }

  public saveInterveningMedia() {
    if (this.formGroupIntervecionMedios.invalid) {
      this.formGroupIntervecionMedios.markAllAsTouched();
      return;
    }

    const data = this.formGroupIntervecionMedios.value;

    const newIntervencion = {
      mediaType: data.mediaType,
      quantity: data.quantity,
      unit: data.unit,
      classification: data.classification,
      ownership_1: data.ownership_1,
      ownership_2: data.ownership_2,
      province_2: data.province_2,
      municipality_2: data.municipality_2,
      observations_3: data.observations_3,
      geoPosicion: {
        type: 'Polygon',
        coordinates: [data.coordsIntervencionMedios],
      },
    };
    if (data.interveningMediaUpdate == '1') {
      this.interveningMedias.splice(
        data.interveningMediaIndex,
        1,
        newIntervencion
      );
    } else {
      this.interveningMedias.push(newIntervencion);
    }

    this.formGroupIntervecionMedios.patchValue({
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

  public showInterveningMediaDataInForm(index: number) {
    const item = this.interveningMedias[index];

    this.formGroupIntervecionMedios.patchValue({
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

  public deleteInterveningMedia(index: number) {
    if (confirm('Está seguro que desea eliminar?')) {
      this.interveningMedias.splice(index, 1);
    }
  }

  public async submit() {
    const data = this.formGroup.value;

    if (this.formGroup.invalid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    data.fire_id = this.fire_id;

    this.errorAreaAfectada = false;
    this.errors = false;

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
        IdImpactoClasificado: impactType == 'Consecuencia' ? 1 : 2,
        observaciones: observations_2,
        numero: number,
        idImpactGroup: impactGroup,
        name: name,
        FechaHora: res.FechaHoraInicio,
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
        geoPosicion: intervening.geoPosicion,
      };

      await this.interveningMediaService.post(interveningMedia);
    }

    for (let areaAffected of this.areasAffected) {
      const bodyAreaAffected = {
        idEvolucion: this.evolution_id,
        fechaHora: areaAffected.startAreaAffected,
        idProvincia: areaAffected.province_1,
        idMunicipio: areaAffected.municipality_1,
        idEntidadMenor: areaAffected.minorEntity,
        geoPosicion: areaAffected.geoPosicion,
      };

      const x = await this.areaAffectedService.post(bodyAreaAffected);
    }

    this.messageService.add({
      severity: 'success',
      summary: 'Creado',
      detail: 'Evolución creada correctamente',
      //life: 90000,
    });
    //this.matDialogRef.close();
    //(window.location.href = '/fire-national-edit/' + this.fire_id)
    new Promise((resolve) => setTimeout(resolve, 2000)).then(() => {
      this.closeModal();
      this.router.navigate([`/fire-national-edit/${this.fire_id}`]).then(() => {
        window.location.reload();
      });
    });
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
    this.type = event.target.value;
    this.getDenominations();
  }

  async changeDenomination(event: any) {
    const camposImpacto = await this.camposImpactoService.getFieldsById(
      event.target.value
    );
    this.fieldCampos.set(camposImpacto);
  }

  public setGroup(event: any) {
    this.group = event.target.value;
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

  getProvincia(id: string | number) {
    const provincia = this.provinces().find((province) => province.id == id);
    return provincia?.descripcion;
  }
  getMunicipio(id: string | number) {
    const municipio = this.municipalities().find(
      (municipio) => municipio.id == id
    );
    return municipio?.descripcion;
  }
  getEntidadMenor(id: string | number) {
    const entidadMenor = this.minorEntities().find(
      (minorEntity) => minorEntity.id == id
    );
    return entidadMenor?.descripcion;
  }

  getForm(atributo: string): any {
    return this.formGroup.controls[atributo];
  }
  getFormAreaAfectada(atributo: string): any {
    return this.formGroupAreaAfectada.controls[atributo];
  }
  getFormConsecuencias(atributo: string): any {
    return this.formGroupConsecuenciasActuaciones.controls[atributo];
  }
  getFormIntervencionsMedio(atributo: string): any {
    return this.formGroupIntervecionMedios.controls[atributo];
  }

  private formatDateToDateTimeLocalInitial(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    const hours = '00';
    const minutes = '00';

    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }
}
