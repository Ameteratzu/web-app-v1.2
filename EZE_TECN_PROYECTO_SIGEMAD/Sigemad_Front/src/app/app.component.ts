import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MenuItemActiveService } from './services/menu-item-active.service';
import { MenuService } from './services/menu.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent {
  public menuItemActiveService = inject(MenuItemActiveService);
  public menuService = inject(MenuService);
  public changeDetectorRef = inject(ChangeDetectorRef);

  public title = 'sigemad';

  public active:string;

  public menu = [
    { name: 'Panel inicial', path: '/dashboard', icon: '/assets/img/dashboard.svg', color: '204, 204, 0' },
    { name: 'Buscador', path: '/search', icon: '/assets/img/search.svg', color: '51, 153, 255' },
    { name: 'Episodios', path: '/episodes', icon: '/assets/img/episodes.svg', color: '102, 204, 0' },
  ];

  public naturals = [
    { name: 'Incendios forestales', path: '/fire', icon: '/assets/img/fire.svg', color: '187, 0, 0' },
    { name: 'Terremotos', path: '/earthquakes', icon: '/assets/img/earthquakes.svg', color: '144, 73, 0' },
    { name: 'Meteorología adversa', path: '/adverse-weather', icon: '/assets/img/adverse-weather.svg', color: '0, 0, 255' },
    { name: 'Fenómenos volcánicos', path: '/volcanic-phenomena', icon: '/assets/img/volcanic-phenomena.svg', color: '255, 128, 0' },
    { name: 'Inundaciones', path: '/floods', icon: '/assets/img/floods.svg', color: '0, 153, 153' },
  ];

  public technologicals = [
    { name: 'Riesgo químico', path: '/chemical-risk', icon: '/assets/img/chemical-risk.svg', color: '156, 161, 35' },
    { name: 'Mercancías peligrosas', path: '/dangerous-goods', icon: '/assets/img/dangerous-goods.svg', color: '249, 215, 5' },
    { name: 'Riesgo nuclear/radiológico', path: '/nuclear-radiological-risk', icon: '/assets/img/nuclear-radiological-risk.svg', color: '204, 153, 255' },
  ];

  public others = [
    { name: 'Otros riesgos', path: '/other-risks', icon: '/assets/img/other-risks.svg', color: '0, 134, 187' },
    { name: 'OPE', path: '/ope', icon: '/assets/img/ope.svg', color: '86, 130, 171' },
  ];

  public utilities = [
    { name: 'Cuadro de mando', path: '/dashboard', icon: '/assets/img/dashboard.svg', color: '102, 0, 102' },
    { name: 'Documentación', path: '/documentation', icon: '/assets/img/documentation.svg', color: '0, 153, 0' },
    { name: 'Incidencias', path: '/incidents', icon: '/assets/img/incidents.svg', color: '153, 0, 76' },
  ];

  public admins = [
    { name: 'Configuración', path: '/config', icon: '/assets/img/config.svg', color: '169, 169, 169' },
    { name: 'Usuarios', path: '/users', icon: '/assets/img/users.svg', color: '102, 51, 0' },
    { name: 'Catálogos', path: '/catalogs', icon: '/assets/img/catalogs.svg', color: '55, 3, 159' },
  ];

  public user = {
    name: 'Manuel Ramos Gómez',
    role: 'Supervisor'
  }  

  public showNaturals = false;
  public showTechnologicals = false;
  public showOthers = false;
  public showUtilities = false;
  public showAdmins = false;

  ngOnInit() {
    this.menuItemActiveService.set.subscribe((data:string) => {
      this.active = data;

      if (this.active == '/fire') {
        this.collapse('naturals');
        this.changeDetectorRef.detectChanges();
      }
    });

    this.menuService.get();
  }

  collapse(submenu:string) {
    this.showNaturals = false;
    this.showTechnologicals = false;
    this.showOthers = false;
    this.showUtilities = false;
    this.showAdmins = false;

    if (submenu == 'naturals') {
      this.showNaturals = ! this.showNaturals;
    }

    if (submenu == 'technologicals') {
      this.showTechnologicals = ! this.showTechnologicals;
    }

    if (submenu == 'others') {
      this.showOthers = ! this.showOthers;
    }

    if (submenu == 'utilities') {
      this.showUtilities = ! this.showUtilities;
    }

    if (submenu == 'admins') {
      this.showAdmins = ! this.showAdmins;
    }
  }

  redirectTo(path:string) {
    window.location.href = path;
  }
}
