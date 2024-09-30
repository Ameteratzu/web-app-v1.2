import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuItemActiveService } from '../../services/menu-item-active.service';

@Component({
  selector: 'app-fire-national-data',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './fire-national-data.component.html',
  styleUrl: './fire-national-data.component.css'
})
export class FireNationalDataComponent {
  public menuItemActiveService = inject(MenuItemActiveService);

  public items = [
    {
      datetime: '12/08/2024 00:00',
      info: 'Parte',
      proc_dest: 'MITECO',
      ent_sal: 'Entrada',
      medium: 'E-mail',
      number: '',
      technical: 'sacop1'
    }
  ];

  ngOnInit() {
    this.menuItemActiveService.set.emit('/fire');
  }
}
