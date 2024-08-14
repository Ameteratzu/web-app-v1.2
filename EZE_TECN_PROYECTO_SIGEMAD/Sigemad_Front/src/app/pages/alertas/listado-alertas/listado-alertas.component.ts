import { Component, OnInit } from '@angular/core';
import { Alerta } from '../../../models/alerta';
import { AlertasService } from '../../../services/AlertasService';

@Component({
  selector: 'app-listado-alertas',
  templateUrl: './listado-alertas.component.html',
  styleUrl: './listado-alertas.component.css'
})
export class ListadoAlertasComponent implements OnInit{

  alertas: Alerta[] =[];
  cargando = false;

  constructor( private alertasService: AlertasService) { }

  ngOnInit(): void {
    this.cargando = true;
    this.alertasService.getAlertas()
        .subscribe( resp => {
          console.log(resp);
          this.alertas = resp;
          this.cargando = false;
        });   

  }

}
