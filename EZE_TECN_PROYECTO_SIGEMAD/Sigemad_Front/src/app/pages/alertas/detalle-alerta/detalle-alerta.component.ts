import { Component, OnInit } from '@angular/core';
import { Alerta } from '../../../models/alerta';
import { AlertasService } from '../../../services/AlertasService';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-detalle-alerta',
  templateUrl: './detalle-alerta.component.html',
  styleUrl: './detalle-alerta.component.css'
})
export class DetalleAlertaComponent implements OnInit {

  alerta: Alerta = new Alerta();

  constructor( private activatedRoute: ActivatedRoute,
        private alertasService: AlertasService,
        private router: Router ) { }

  ngOnInit(): void {
    this.activatedRoute.params
      .pipe(
      switchMap(({ id }) => this.alertasService.getAlerta(id) )
      )
      .subscribe( ( resp ) => this.alerta = resp );
  }
}
