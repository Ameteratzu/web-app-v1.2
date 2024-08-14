import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListadoEstadosAlertasComponent } from './listado-estados-alertas.component';

describe('ListadoEstadosAlertasComponent', () => {
  let component: ListadoEstadosAlertasComponent;
  let fixture: ComponentFixture<ListadoEstadosAlertasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListadoEstadosAlertasComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListadoEstadosAlertasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
