import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { InputOutput } from '../types/input-output.type';
import { Media } from '../types/media.type';
import { OriginDestination } from '../types/origin-destination.type';
import { FireStatus } from '../types/fire-status.type';
import { SituationsEquivalent } from '../types/situations-equivalent.type';
import { TypesPlans } from '../types/types-plans.type';
import { SituationPlan } from '../types/situation-plan.type';

 
@Injectable({
  providedIn: 'root'
})
export class MasterDataEvolutionsService {

  private http = inject(HttpClient);
  
    getInputOutput() {
      const endpoint = '/entradas-salidas';
      return firstValueFrom(this.http.get<InputOutput[]>(endpoint).pipe((response) => response));
    }

    getMedia() {
      const endpoint = '/medios';
      return firstValueFrom(this.http.get<Media[]>(endpoint).pipe((response) => response));
    }

    getOriginDestination() {
      const endpoint = '/procedencias-destinos';
      return firstValueFrom(this.http.get<OriginDestination[]>(endpoint).pipe((response) => response));
    }

    getFireStatus() {
      const endpoint = '/estados-incendios';
      return firstValueFrom(this.http.get<FireStatus[]>(endpoint).pipe((response) => response));
    }

    getSituationEquivalent() {
      const endpoint = '/situaciones-equivalentes';
      return firstValueFrom(this.http.get<SituationsEquivalent[]>(endpoint).pipe((response) => response));
    }

    getTypesPlans() {
      const endpoint = '/planes-emergencias';
      return firstValueFrom(this.http.get<TypesPlans[]>(endpoint).pipe((response) => response));
    }

    getPhases(plan_id: number | string) {
      const endpoint = `/fases-emergencia?idPlanEmergencia=${plan_id}`;
      return firstValueFrom(this.http.get<TypesPlans[]>(endpoint).pipe((response) => response));
    }

    getSituationsPlans(plan_id: number | string, phase_id: number | string) {
      const endpoint = `/plan-situacion-emergencia?idPlanEmergencia=${plan_id}&idFaseEmergencia=${phase_id}`;
      return firstValueFrom(this.http.get<SituationPlan[]>(endpoint).pipe((response) => response));
    }

}
