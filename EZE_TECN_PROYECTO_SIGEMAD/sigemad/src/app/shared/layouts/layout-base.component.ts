import { Component, computed, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar'
import { MatButtonModule } from '@angular/material/button'
import { MatIconModule } from '@angular/material/icon'
import { MatSidenavModule } from '@angular/material/sidenav'
import { CustomSidenavComponent } from '../../components/custom-sidenav/custom-sidenav.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet, 
    MatToolbarModule, 
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    CustomSidenavComponent
  ],
    
  templateUrl: './layout-base.component.html',
  styleUrl: './layout-base.component.scss',
})
export class LayoutBaseComponent {
  title = 'sigemad';

  collapsed = signal(false);
  sidenavWidth = computed (() => this.collapsed() ? '65px' : '300px');
}
