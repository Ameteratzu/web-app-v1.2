import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleAlertaComponent } from './detalle-alerta.component';

describe('DetalleAlertaComponent', () => {
  let component: DetalleAlertaComponent;
  let fixture: ComponentFixture<DetalleAlertaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DetalleAlertaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DetalleAlertaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
