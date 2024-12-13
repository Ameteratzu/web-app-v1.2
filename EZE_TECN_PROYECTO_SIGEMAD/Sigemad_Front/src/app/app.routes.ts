import { Routes } from '@angular/router';
import { CommentsComponent } from './pages/comments/comments.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { FireEditComponent } from './pages/fire/fire-edit/fire-edit.component';
import { FireComponent } from './pages/fire/fire.component';
import { Login } from './pages/login/login.component';
import { LayoutBaseComponent } from './shared/layouts/layout-base.component';

export const routes: Routes = [
  { path: 'login', component: Login },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: '',
    component: LayoutBaseComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent, data: { breadcrumb: 'Inicio' } },
      {
        path: 'fire',
        component: FireComponent,
        data: { breadcrumb: 'Incendio' },
        children: [
          { path: 'fire-national-edit/:id', component: FireEditComponent, data: { breadcrumb: 'Editar Incendio' } }
        ]
      },
      { path: 'earthquakes', component: CommentsComponent , data: { breadcrumb: 'earthquakes' }},
      { path: 'adverse-weather', component: CommentsComponent , data: { breadcrumb: 'adverse-weather' }},
      { path: 'volcanic-phenomena', component: CommentsComponent , data: { breadcrumb: 'volcanic-phenomena' }},
      { path: 'floods', component: CommentsComponent , data: { breadcrumb: 'floods' }},
      { path: 'chemical-risk', component: CommentsComponent , data: { breadcrumb: 'chemical-risk' }},
      { path: 'dangerous-goods', component: CommentsComponent , data: { breadcrumb: 'dangerous-goods' }},
      { path: 'nuclear-radiological-risk', component: CommentsComponent , data: { breadcrumb: 'nuclear-radiological-risk' }},
      { path: 'other-risks', component: CommentsComponent , data: { breadcrumb: 'other-risks' }},
      { path: 'ope', component: CommentsComponent , data: { breadcrumb: 'ope' }},
      { path: 'documentation', component: CommentsComponent , data: { breadcrumb: 'documentation' }},
      { path: 'incidents', component: CommentsComponent , data: { breadcrumb: 'incidents' }},
      { path: 'config', component: CommentsComponent , data: { breadcrumb: 'config' }},
      { path: 'users', component: CommentsComponent , data: { breadcrumb: 'users' }},
      { path: 'catalogs', component: CommentsComponent , data: { breadcrumb: 'catalogs' }},
      { path: 'search', component: CommentsComponent , data: { breadcrumb: 'search' }},
      { path: 'episodes', component: CommentsComponent , data: { breadcrumb: 'episodes' }},
    ],
  },
  { path: '**', redirectTo: 'login' },
];
