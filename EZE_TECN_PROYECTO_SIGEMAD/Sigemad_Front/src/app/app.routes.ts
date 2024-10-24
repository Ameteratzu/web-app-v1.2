import { Routes } from '@angular/router';

import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { FireComponent } from './pages/fire/fire.component';
import { FireNationalEditComponent } from './pages/fire-national-edit/fire-national-edit.component';

export const routes: Routes = [
	{ path: 'dashboard', component: DashboardComponent },
	{ path: 'fire', component: FireComponent },
	{ path: 'fire-national-edit/:id', component: FireNationalEditComponent },
];
