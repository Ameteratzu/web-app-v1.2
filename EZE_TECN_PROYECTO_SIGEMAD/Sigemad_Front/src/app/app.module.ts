import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';

import { HomeComponent } from './pages/home/home.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ListadoAlertasComponent } from './pages/alertas/listado-alertas/listado-alertas.component';
import { ListadoEstadosAlertasComponent } from './pages/estadosAlertas/listado-estados-alertas/listado-estados-alertas.component';
import { DetalleAlertaComponent } from './pages/alertas/detalle-alerta/detalle-alerta.component';
import { LoginComponent } from './auth/pages/login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    ListadoAlertasComponent,
    ListadoEstadosAlertasComponent,
    DetalleAlertaComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
