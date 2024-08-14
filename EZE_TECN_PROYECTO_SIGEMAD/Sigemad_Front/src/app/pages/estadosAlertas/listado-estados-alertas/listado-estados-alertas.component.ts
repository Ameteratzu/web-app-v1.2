import { Component } from '@angular/core';
import { EstadoAlerta } from '../../../models/estadoAlerta';
import { EstadosAlertasService } from '../../../services/EstadosAlertasService';


@Component({
  selector: 'app-listado-estados-alertas',
  templateUrl: './listado-estados-alertas.component.html',
  styleUrl: './listado-estados-alertas.component.css'
})
export class ListadoEstadosAlertasComponent {
  estadosalertas: EstadoAlerta[] =[];
  cargando = false;

  constructor( private estadosAlertasService: EstadosAlertasService) { }

  ngOnInit(): void {
    this.cargando = true;
    this.estadosAlertasService.getEstadosAlertas()
        .subscribe( resp => {
          console.log(resp);
          this.estadosalertas = resp;
          this.cargando = false;
        });   

  }
}
