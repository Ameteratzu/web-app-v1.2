import { Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { CommentsComponent } from './pages/comments/comments.component';
import { FireComponent } from './pages/fire/fire.component';
import { FireEditComponent } from './pages/fire/fire-edit/fire-edit.component';

export const routes: Routes = [

    { path: '*', redirectTo: 'dashboard', pathMatch: 'full' },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'fire', component: FireComponent },
    { path: 'earthquakes', component: CommentsComponent },
    { path: 'adverse-weather', component: CommentsComponent },
    { path: 'volcanic-phenomena', component: CommentsComponent },
    { path: 'floods', component: CommentsComponent },
    { path: 'chemical-risk', component: CommentsComponent },
    { path: 'dangerous-goods', component: CommentsComponent },
    { path: 'nuclear-radiological-risk', component: CommentsComponent },
    { path: 'other-risks', component: CommentsComponent },
    { path: 'ope', component: CommentsComponent },
    { path: 'documentation', component: CommentsComponent },
    { path: 'incidents', component: CommentsComponent },
    { path: 'config', component: CommentsComponent },
    { path: 'users', component: CommentsComponent },
    { path: 'catalogs', component: CommentsComponent },
    { path: 'search', component: CommentsComponent },
    { path: 'episodes', component: CommentsComponent },
    { path: 'fire-national-edit/:id', component: FireEditComponent },
];

