import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { ListadoAlertasComponent } from './pages/alertas/listado-alertas/listado-alertas.component';
import { DetalleAlertaComponent } from './pages/alertas/detalle-alerta/detalle-alerta.component';
import { ListadoEstadosAlertasComponent } from './pages/estadosAlertas/listado-estados-alertas/listado-estados-alertas.component';


const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'listadoAlertas',
    component: ListadoAlertasComponent,
  },
  {
    path: 'listadoEstadosAlertas',
    component: ListadoEstadosAlertasComponent,
  },
  {
    path: 'alerta/:id',
    component: DetalleAlertaComponent,
  }, 
  {
    path: '**',
    redirectTo: ''
  }
]

@NgModule({
imports: [
    RouterModule.forRoot(routes)
],
exports: [
    RouterModule
]
})
export class AppRoutingModule { }
