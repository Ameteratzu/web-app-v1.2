import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

import { AutonomousCommunityService } from '../../services/autonomous-community.service';
import { FireStatusService } from '../../services/fire-status.service';
import { FireService } from '../../services/fire.service';
import { MenuItemActiveService } from '../../services/menu-item-active.service';
import { MunicipalityService } from '../../services/municipality.service';
import { ProvinceService } from '../../services/province.service';
import { SeverityLevelService } from '../../services/severity-level.service';
import { TerritoryService } from '../../services/territory.service';

import { ApiResponse } from '../../types/api-response.type';
import { AutonomousCommunity } from '../../types/autonomous-community.type';
import { FireStatus } from '../../types/fire-status.type';
import { Fire } from '../../types/fire.type';
import { Municipality } from '../../types/municipality.type';
import { Province } from '../../types/province.type';
import { SeverityLevel } from '../../types/severity-level.type';
import { Territory } from '../../types/territory.type';

import { FireNationalCreateComponent } from '../fire-national-create/fire-national-create.component';

@Component({
  selector: 'app-fire',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './fire.component.html',
  styleUrl: './fire.component.css',
})
export class FireComponent {
  public menuItemActiveService = inject(MenuItemActiveService);
  public territoryService = inject(TerritoryService);
  public autonomousCommunityService = inject(AutonomousCommunityService);
  public provinceService = inject(ProvinceService);
  public municipalityService = inject(MunicipalityService);
  public fireStatusService = inject(FireStatusService);
  public severityLevelService = inject(SeverityLevelService);
  public fireService = inject(FireService);

  public territories = signal<Territory[]>([]);
  public autonomousCommunities = signal<AutonomousCommunity[]>([]);
  public provinces = signal<Province[]>([]);
  public municipalities = signal<Municipality[]>([]);
  public fireStatus = signal<FireStatus[]>([]);
  public severityLevels = signal<SeverityLevel[]>([]);
  public fires = <ApiResponse<Fire[]>>{};

  public matDialog = inject(MatDialog);

  public formData: FormGroup;

  async ngOnInit() {
    this.formData = new FormGroup({
      name: new FormControl(),
      territory: new FormControl(),
      autonomousCommunity: new FormControl(),
      province: new FormControl(),
      municipality: new FormControl(),
      fireStatus: new FormControl(),
      episode: new FormControl(),
      severityLevel: new FormControl(),
      affectedArea: new FormControl(),
      move: new FormControl(),
      enter: new FormControl(),
      start: new FormControl(),
      end: new FormControl(),
    });

    this.menuItemActiveService.set.emit('/fire');

    const territories = await this.territoryService.get();
    this.territories.set(territories);

    const autonomousCommunities = await this.autonomousCommunityService.get();
    this.autonomousCommunities.set(autonomousCommunities);

    const fireStatus = await this.fireStatusService.get();
    this.fireStatus.set(fireStatus);

    const severityLevels = await this.severityLevelService.get();
    this.severityLevels.set(severityLevels);

    const fires = await this.fireService.get();
    this.fires = fires;

    console.log(fires.data);
  }

  openModal() {
    this.matDialog.open(FireNationalCreateComponent, {
      width: '1000px',
      maxWidth: '1000px',
      disableClose: true,
    });
  }

  async loadProvinces(event: any) {
    const ac_id = event.target.value;
    console.log(ac_id);
    const provinces = await this.provinceService.get(ac_id);
    this.provinces.set(provinces);
  }

  async loadMunicipalities(event: any) {
    const province_id = event.target.value;
    const municipalities = await this.municipalityService.get(province_id);
    this.municipalities.set(municipalities);
  }

  async onSubmit() {
    const data = this.formData.value;
    const fires = await this.fireService.get(data);
    this.fires = fires;
  }
}
