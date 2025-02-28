import { Routes } from '@angular/router';
import { CommentsComponent } from './pages/comments/comments.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { FireEditComponent } from './pages/fire/fire-edit/fire-edit.component';
import { FireComponent } from './pages/fire/fire.component';
import { Login } from './pages/login/login.component';
import { LayoutBaseComponent } from './shared/layouts/layout-base.component';
import { OpePeriodosComponent } from './pages/ope/administracion/periodos/ope-periodos.component';
import { OpePuertosComponent } from './pages/ope/administracion/puertos/ope-puertos.component';
import { OpeLineasMaritimasComponent } from './pages/ope/administracion/lineas-maritimas/ope-lineas-maritimas.component';
import { OpeFronterasComponent } from './pages/ope/administracion/fronteras/ope-fronteras.component';

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
      { path: 'dashboard', component: DashboardComponent },
      {
        path: 'fire',
        component: FireComponent,
        data: { breadcrumb: 'Incendios forestales' },
        children: [{ path: 'fire-national-edit/:id', component: FireEditComponent, data: { breadcrumb: 'Panel de seguimiento' } }],
      },
      { path: 'earthquakes', component: CommentsComponent, data: { breadcrumb: 'earthquakes' } },
      { path: 'adverse-weather', component: CommentsComponent, data: { breadcrumb: 'adverse-weather' } },
      { path: 'volcanic-phenomena', component: CommentsComponent, data: { breadcrumb: 'volcanic-phenomena' } },
      { path: 'floods', component: CommentsComponent, data: { breadcrumb: 'floods' } },
      { path: 'chemical-risk', component: CommentsComponent, data: { breadcrumb: 'chemical-risk' } },
      { path: 'dangerous-goods', component: CommentsComponent, data: { breadcrumb: 'dangerous-goods' } },
      { path: 'nuclear-radiological-risk', component: CommentsComponent, data: { breadcrumb: 'nuclear-radiological-risk' } },
      { path: 'other-risks', component: CommentsComponent, data: { breadcrumb: 'other-risks' } },
      // PCD
      // OPE - ADMINISTRACIÓN
      { path: 'ope-administracion-periodos', component: OpePeriodosComponent, data: { breadcrumb: 'Administración de periodos' } },
      { path: 'ope-administracion-puertos', component: OpePuertosComponent, data: { breadcrumb: 'Administración de puertos' } },
      {
        path: 'ope-administracion-lineas-maritimas',
        component: OpeLineasMaritimasComponent,
        data: { breadcrumb: 'Administración de líneas marítimas' },
      },
      { path: 'ope-administracion-fronteras', component: OpeFronterasComponent, data: { breadcrumb: 'Administración de fronteras' } },

      {
        path: 'ope-administracion-puntos-control-carreteras',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Puntos de Control en carreteras' },
      },
      {
        path: 'ope-administracion-areas-descanso',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Áreas de descanso y puntos de información en carreterass' },
      },
      { path: 'ope-administracion-areas-estacionamiento', component: OpePeriodosComponent, data: { breadcrumb: 'Áreas de estacionamiento' } },
      {
        path: 'ope-administracion-ocupacion-areas-estacionamiento',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Porcentajes de ocupación de áreas de estacionamiento' },
      },
      {
        path: 'ope-administracion-log',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Log' },
      },
      // OPE - NUEVO
      {
        path: 'ope-nuevo-embarques-diarios',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Embarques diarios' },
      },

      {
        path: 'ope-nuevo-embarques-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Embarques. Funcionalidades' },
      },
      {
        path: 'ope-nuevo-asistencias',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Asistencias' },
      },
      {
        path: 'ope-nuevo-asistencias-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Asistencias. Funcionalidades' },
      },
      {
        path: 'ope-nuevo-fronteras',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Fronteras' },
      },
      {
        path: 'ope-nuevo-fronteras-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Fronteras. Funcionalidades' },
      },
      {
        path: 'ope-nuevo-afluencia-puntos-control-carreteras',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Afluencia a puntos de control en carreteras' },
      },
      {
        path: 'ope-nuevo-afluencia-puntos-control-carreteras-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Afluencia a puntos de control en carreteras. Funcionalidades' },
      },
      {
        path: 'ope-nuevo-areas-descanso',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Áreas de descanso y puntos de información en carreteras' },
      },
      {
        path: 'ope-nuevo-areas-estacionamiento',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Ocupación de áreas de estacionamiento' },
      },
      {
        path: 'ope-nuevo-areas-estacionamiento-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Ocupación de áreas de estacionamiento. Funcionalidades' },
      },
      // OPE - BUSCAR
      {
        path: 'ope-buscar-embarques-diarios',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Embarques diarios' },
      },
      {
        path: 'ope-buscar-embarques-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Embarques. Funcionalidades' },
      },
      {
        path: 'ope-buscar-asistencias',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Asistencias' },
      },

      {
        path: 'ope-buscar-asistencias-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Asistencias. Funcionalidades' },
      },
      {
        path: 'ope-buscar-fronteras',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Fronteras' },
      },
      {
        path: 'ope-buscar-fronteras-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Fronteras. Funcionalidades' },
      },
      {
        path: 'ope-buscar-afluencia-puntos-control-carretera',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Afluencia a puntos de control en carretera' },
      },
      {
        path: 'ope-buscar-afluencia-puntos-control-carretera-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Afluencia a puntos de control en carreteras. Funcionalidades' },
      },
      {
        path: 'ope-buscar-areas-descanso',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Áreas de descanso y puntos de información en carreteras' },
      },
      {
        path: 'ope-buscar-ocupacion-areas-estacionamiento',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Ocupación de áreas de estacionamiento' },
      },
      {
        path: 'ope-buscar-ocupacion-areas-estacionamiento-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Ocupación de áreas de estacionamiento. Funcionalidades' },
      },
      // OPE - APBA
      {
        path: 'ope-apba-entrada-vehiculos-puertos',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Entrada de vehículos en puertos APBA. Datos' },
      },
      {
        path: 'ope-apba-entrada-vehiculos-puertos-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Entrada de vehículos en puertos APBA. Funcionalidades' },
      },
      {
        path: 'ope-apba-embarques-vehiculos-intervalos-horarios',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Embarques de vehículos en APBA por intervalos horarios. Datos' },
      },
      {
        path: 'ope-apba-embarques-vehiculos-intervalos-horarios-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Embarques de vehículos por intervalos horarios. Funcionalidades' },
      },
      // OPE - PLANIFICACIÓN
      {
        path: 'ope-planificacion-plan-flota',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Plan de flota' },
      },
      {
        path: 'ope-planificacion-plan-flota-funcionalidades',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Plan de flota. Funcionalidades' },
      },
      {
        path: 'ope-planificacion-participantes-age',
        component: OpePeriodosComponent,
        data: { breadcrumb: '	Participantes AGE' },
      },
      // OPE - INCIDENCIAS
      {
        path: 'ope-incidencias-datos-inicio',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Incidencias. Datos de inicio' },
      },
      // OPE - INFORMES
      {
        path: 'ope-informes-prueba',
        component: OpePeriodosComponent,
        data: { breadcrumb: 'Informe de prueba' },
      },
      // FIN PCD

      { path: 'documentation', component: CommentsComponent, data: { breadcrumb: 'documentation' } },
      { path: 'incidents', component: CommentsComponent, data: { breadcrumb: 'incidents' } },
      { path: 'config', component: CommentsComponent, data: { breadcrumb: 'config' } },
      { path: 'users', component: CommentsComponent, data: { breadcrumb: 'users' } },
      { path: 'catalogs', component: CommentsComponent, data: { breadcrumb: 'catalogs' } },
      { path: 'search', component: CommentsComponent, data: { breadcrumb: 'search' } },
      { path: 'episodes', component: CommentsComponent, data: { breadcrumb: 'episodes' } },
    ],
  },
  { path: '**', redirectTo: 'login' },
];
