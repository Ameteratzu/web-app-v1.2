
import { MatListModule } from '@angular/material/list'
import { MatIconModule, MatIconRegistry } from '@angular/material/icon'
import { CommonModule } from '@angular/common'; 
import { RouterLink, RouterModule } from '@angular/router';
import { ChangeDetectorRef, Component, inject, signal, Input, computed, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItemActiveService } from '../../services/menu-item-active.service';
import { MenuService } from '../../services/menu.service';
import { Menu } from '../../types/menu.types';
import { DomSanitizer } from '@angular/platform-browser';
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-custom-sidenav',
  standalone: true,
  imports: [CommonModule, MatListModule, MatIconModule, RouterLink, RouterModule, NgxSpinnerModule],
  templateUrl: './custom-sidenav.component.html',
  styleUrl: './custom-sidenav.component.scss'
})
export class CustomSidenavComponent {

  public menuItemActiveService = inject(MenuItemActiveService);
  public menuService = inject(MenuService);
  public changeDetectorRef = inject(ChangeDetectorRef);
  public renderer = inject(Renderer2);
  public iconRegistry = inject(MatIconRegistry);
  public sanitizer = inject(DomSanitizer);
  private spinner = inject(NgxSpinnerService);
  public router = inject(Router);
  public title = 'sigemad';
  public active: string | undefined;
  public menuBack = signal<Menu[]>([]);

  public user = {
    name: 'Manuel Ramos Gómez',
    role: 'Supervisor',
  };

  expandedMenuId: number | null = null;

  sideNavCollapsed = signal(false)
  @Input() set collapsed (val: boolean) {
    this. sideNavCollapsed.set(val);
  }

  iconMap: { [key: string]: string } = {
    '/fire': 'fire',
    '/earthquakes': 'earthquakes',
    '/adverse-weather': 'weather',
    '/volcanic-phenomena': 'volcanic',
    '/floods': 'floods',
    '/chemical-risk': 'chemical',
    '/dangerous-goods': 'dangerous',
    '/nuclear-radiological-risk': 'nuclear',
    '/other-risks': 'other',
    '/ope': 'ope',
    '/dashboard': 'dashboard',
    '/documentation': 'documentation',
    '/incidents': 'incidents',
    '/config': 'config',
    '/users': 'users',
    '/catalogs': 'catalogs',
    '/search': 'search',
    '/episodes': 'episodes'
  };



  registerIcons(): void {
    const icons = [
      { name: 'dashboard', path: '/assets/img/dashboard.svg' },
      { name: 'search', path: '/assets/img/search.svg' },
      { name: 'episodes', path: '/assets/img/episodes.svg' },
      { name: 'fire', path: '/assets/img/fire.svg' },
      { name: 'earthquakes', path: '/assets/img/earthquakes.svg' },
      { name: 'weather', path: '/assets/img/adverse-weather.svg' },
      { name: 'volcanic', path: '/assets/img/volcanic-phenomena.svg' },
      { name: 'floods', path: '/assets/img/floods.svg' },
      { name: 'chemical', path: '/assets/img/chemical-risk.svg' },
      { name: 'dangerous', path: '/assets/img/dangerous-goods.svg' },
      { name: 'nuclear', path: '/assets/img/nuclear-radiological-risk.svg' },
      { name: 'other', path: '/assets/img/other-risks.svg' },
      { name: 'ope', path: '/assets/img/ope.svg' },
      { name: 'dashboard', path: '/assets/img/dashboard.svg' },
      { name: 'documentation', path: '/assets/img/documentation.svg' },
      { name: 'incidents', path: '/assets/img/incidents.svg' },
      { name: 'config', path: '/assets/img/config.svg' },
      { name: 'users', path: '/assets/img/users.svg' },
      { name: 'catalogs', path: '/assets/img/catalogs.svg' },
    ];
  
    icons.forEach(icon => {
      this.iconRegistry.addSvgIcon(
        icon.name,
        this.sanitizer.bypassSecurityTrustResourceUrl(icon.path)
      );
    });
  }
  

  toggleSubmenu(item: any): void {
    item.ruta ? this.redirectTo(item) : "";
    if (this.expandedMenuId === item.id) {
      this.expandedMenuId = null;
    } else {
      this.expandedMenuId = item.id;
    }
  }

  getActiveStyle(item: any, isActive: boolean): { [key: string]: string } {
    if (isActive && item.colorRgb) {
      return {
        backgroundColor: `rgba(${item.colorRgb}, 0.2)`,
        borderLeft: `4px solid rgb(${item.colorRgb})`,
      };
    }
    return {};
  }

  logoSize = computed<{ width: string; height: string }>(() => {
    return this.sideNavCollapsed()
      ? { width: '50px', height: '72px' }
      : { width: '213px', height: '67px' };
  });

  logoSrc = computed(() => {
    return this.sideNavCollapsed()
      ? '/assets/img/logo2.png'
      : '/assets/img/logo.svg';
  });

  async ngOnInit() {
    this.spinner.show();
    const toolbar = document.querySelector('mat-toolbar');
    this.renderer.setStyle(toolbar, 'z-index', '1');
    this.registerIcons();
   
    this.menuItemActiveService.set.subscribe((data: string) => {
      this.active = data;
    });

    const respMenu = await this.menuService.get();
    this.menuBack.set(respMenu);
    this.spinner.hide();
    this.renderer.setStyle(toolbar, 'z-index', '5');
  }

  redirectTo(itemSelected: Menu) {
      this.router.navigate([`${itemSelected.ruta}`]);
  }
}
