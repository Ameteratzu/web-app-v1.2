import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  MatDialog,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { DireccionesService } from '../../../../services/direcciones.service';
import { ProvinceService } from '../../../../services/province.service';
import { MunicipalityService } from '../../../../services/municipality.service';
import { PlanesService } from '../../../../services/planes.service';
import Feature from 'ol/Feature';
import { Geometry } from 'ol/geom';
import { MapCreateComponent } from '../../../../shared/mapCreate/map-create.component';
import { FormFieldComponent } from '../../../../shared/Inputs/field.component';

@Component({
  selector: 'app-fire-direction-coordination',
  standalone: true,
  imports: [
    FormFieldComponent,
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
  ],
  providers: [],
  templateUrl: './fire-direction-coordination.component.html',
  styleUrl: './fire-direction-coordination.component.css',
})
export class FireDirectionCoordinationComponent implements OnInit {
  /** Formularios */
  public formularioDireccion: FormGroup = new FormGroup({
    direccion: new FormControl(''),
    autoridad: new FormControl(''),
    fechaInicio: new FormControl(''),
    fechaFin: new FormControl(''),
  });
  public formularioCECOPI: FormGroup;
  public formularioPMA: FormGroup;
  public formularioEmergencia: FormGroup;

  /**Signals */
  public isCreate = signal<boolean>(true);
  public direccionesInsertadas = signal<any[]>([]);
  public CECOPIInsertadas = signal<any[]>([]);
  public PMAInsertadas = signal<any[]>([]);
  public emergenciasInsertadas = signal<any[]>([]);
  /** IndexEdits */
  public indexDireccion = signal<number>(0);
  public indexCECOPI = signal<number>(0);
  public indexPMA = signal<number>(0);
  public indexEmergencia = signal<number>(0);

  /**Listas */

  public listaDirecciones = signal<any[]>([]);
  public listaProvincia = signal<any[]>([]);
  public listaMunicipio = signal<any[]>([]);
  public listaTipoPlan = signal<any[]>([]);
  public listaNombrePlan = signal<any[]>([]);

  /** Servicios */
  public direccionesService = inject(DireccionesService);
  public provinceService = inject(ProvinceService);
  public municipalitiesService = inject(MunicipalityService);
  public planesService = inject(PlanesService);

  public matDialog = inject(MatDialog);
  public matDialogRef = inject(MatDialogRef);

  async ngOnInit() {
    this.formularioDireccion = new FormGroup({
      direccion: new FormControl(''),
      autoridad: new FormControl(''),
      fechaInicio: new FormControl(''),
      fechaFin: new FormControl(''),
    });

    this.formularioCECOPI = new FormGroup({
      fechaInicio: new FormControl(''),
      fechaFin: new FormControl(''),
      lugar: new FormControl(''),
      provincia: new FormControl(''),
      municipio: new FormControl(''),
      observaciones: new FormControl(''),
      coordenadas: new FormControl(''),
    });

    this.formularioPMA = new FormGroup({
      fechaInicio: new FormControl(''),
      fechaFin: new FormControl(''),
      lugar: new FormControl(''),
      provincia: new FormControl(''),
      municipio: new FormControl(''),
      observaciones: new FormControl(''),
      coordenadas: new FormControl(''),
    });

    this.formularioEmergencia = new FormGroup({
      tipoPlan: new FormControl(''),
      //nombre: new FormControl('', Validators.required),
      nombre: new FormControl(''),
      autoridadActiva: new FormControl(''),
      //documento: new FormControl('', Validators.required),
      documento: new FormControl(''),
      fechaInicio: new FormControl(''),
      fechaFin: new FormControl(''),
      observaciones: new FormControl(''),
    });

    const direcciones = await this.direccionesService.getAllDirecciones();
    this.listaDirecciones.set(direcciones);

    const provincias = await this.provinceService.get();
    this.listaProvincia.set(provincias);

    const planes = await this.planesService.getAllPlanes();
    this.listaTipoPlan.set(planes);
  }

  async getMunicipiosByPronvincia(event: any) {
    const idProvincia = event.target.value;
    const municipalities = await this.municipalitiesService.get(idProvincia);
    this.listaMunicipio.set(municipalities);
  }

  onSubmit() {
    //TODO
  }

  onSubmitDireccion() {
    if (this.formularioDireccion.valid) {
      if (this.indexDireccion() == 0) {
        this.direccionesInsertadas().splice(
          this.indexDireccion(),
          1,
          this.formularioDireccion.value
        );
      } else {
        this.direccionesInsertadas.set([
          ...this.direccionesInsertadas(),
          this.formularioDireccion.value,
        ]);
      }

      this.clearFormDireccion();
    } else {
      this.formularioDireccion.markAllAsTouched();
    }
  }

  onEditDireccion(index: number) {
    this.indexDireccion.set(index);
    const itemUpdate = this.direccionesInsertadas()[index];
    this.formularioDireccion.patchValue(itemUpdate);
  }

  onRemoveDireccion(index: number) {
    const newList = this.direccionesInsertadas().filter(
      (item, i) => i != index
    );
    this.direccionesInsertadas.set(newList);
  }

  onSubmitCECOPI() {
    if (this.formularioCECOPI.valid) {
      if (this.indexCECOPI() == 0) {
        this.CECOPIInsertadas().splice(
          this.indexCECOPI(),
          1,
          this.formularioCECOPI.value
        );
      } else {
        this.CECOPIInsertadas.set([
          ...this.CECOPIInsertadas(),
          this.formularioCECOPI.value,
        ]);
      }
      this.clearFormCECOPI();
    } else {
      this.formularioCECOPI.markAllAsTouched();
    }
  }

  onEditCECOPIInsertadas(index: number) {
    this.indexCECOPI.set(index);
    const itemUpdated = this.CECOPIInsertadas()[index];
    this.formularioCECOPI.patchValue(itemUpdated);
  }

  onRemoveCECOPIInsertadas(index: number) {
    const newList = this.CECOPIInsertadas().filter((item, i) => i != index);
    this.CECOPIInsertadas.set(newList);
  }

  onSubmitPMA() {
    if (this.formularioPMA.valid) {
      if (this.indexPMA() == 0) {
        this.PMAInsertadas().splice(
          this.indexPMA(),
          1,
          this.formularioPMA.value
        );
      } else {
        this.PMAInsertadas.set([
          ...this.PMAInsertadas(),
          this.formularioPMA.value,
        ]);
      }
      this.clearFormPMA();
    } else {
      this.formularioPMA.markAllAsTouched();
    }
  }

  onEditPMAInsertadas(index: number) {
    this.indexPMA.set(index);
    const itemUpdated = this.PMAInsertadas()[index];
    this.formularioPMA.patchValue(itemUpdated);
  }

  onRemovePMAInsertadas(index: number) {
    const newList = this.PMAInsertadas().filter((item, i) => i != index);
    this.PMAInsertadas.set(newList);
  }

  onSubmitEmergencia() {
    if (this.formularioEmergencia.valid) {
      if (this.indexEmergencia() == 0) {
        this.emergenciasInsertadas().splice(
          this.indexEmergencia(),
          1,
          this.formularioEmergencia.value
        );
      } else {
        this.emergenciasInsertadas.set([
          ...this.emergenciasInsertadas(),
          this.formularioEmergencia.value,
        ]);
      }

      this.clearFormEmergencia();
    } else {
      this.formularioEmergencia.markAllAsTouched();
    }
  }

  onEditEmergenciaInsertada(index: number) {
    this.indexEmergencia.set(index);
    const itemUpdated = this.emergenciasInsertadas()[index];

    this.formularioEmergencia.patchValue(itemUpdated);
  }

  onRemoveEmergenciaInsertada(index: number) {
    const newList = this.emergenciasInsertadas().filter(
      (item, i) => i != index
    );
    this.emergenciasInsertadas.set(newList);
  }

  getValid(formulario: any, campo: string, isSelect: boolean = false): string {
    if (formulario.controls[campo].touched) {
      if (formulario.controls[campo].valid) {
        return `${isSelect ? 'form-select' : 'form-control'} is-valid`;
      }
      {
        return `${isSelect ? 'form-select' : 'form-control'} is-invalid`;
      }
    }
    return 'form-control';
  }

  esRequerido(formulario: any, campo: string): boolean {
    if (formulario.controls[campo].touched) {
      return true;
    }
    return false;
  }

  scrollToSection(sectionId: string) {
    const element = document.getElementById(sectionId);

    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  }

  clearFormDireccion() {
    this.formularioDireccion.reset();
  }
  clearFormCECOPI() {
    this.formularioCECOPI.reset();
  }
  clearFormPMA() {
    this.formularioPMA.reset();
  }
  clearFormEmergencia() {
    this.formularioEmergencia.reset();
  }

  openModalMapCreate(formulario: any) {
    const municipio = this.listaMunicipio().find(
      (item) => item.id == formulario.value.municipio
    );
    if (!municipio) return;

    const dialogRef = this.matDialog.open(MapCreateComponent, {
      width: '780px',
      maxWidth: '780px',
      height: '780px',
      maxHeight: '780px',
      data: { municipio: municipio, listaMunicipios: this.listaMunicipio() },
    });

    dialogRef.componentInstance.save.subscribe(
      (features: Feature<Geometry>[]) => {
        features;
      }
    );
  }

  closeModal() {
    this.matDialogRef.close();
  }

  getDescripcion(lista: any[], id: any) {
    return lista.find((item: any) => item.id == id).descripcion;
  }

  getFormDireccion(atributo: string): any {
    return this.formularioDireccion.controls[atributo];
  }

  getFormCECOPI(atributo: string): any {
    return this.formularioCECOPI.controls[atributo];
  }

  getFormPMA(atributo: string): any {
    return this.formularioPMA.controls[atributo];
  }
  getFormEmergencia(atributo: string): any {
    return this.formularioEmergencia.controls[atributo];
  }
}
