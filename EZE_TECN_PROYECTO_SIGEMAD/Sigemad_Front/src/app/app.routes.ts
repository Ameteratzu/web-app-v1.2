import { Routes } from '@angular/router';

import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { FireEditComponent } from './pages/fire/fire-edit/fire-edit.component';
import { FireComponent } from './pages/fire/fire.component';

export const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'fire', component: FireComponent },
  { path: 'fire-national-edit/:id', component: FireEditComponent },
];
