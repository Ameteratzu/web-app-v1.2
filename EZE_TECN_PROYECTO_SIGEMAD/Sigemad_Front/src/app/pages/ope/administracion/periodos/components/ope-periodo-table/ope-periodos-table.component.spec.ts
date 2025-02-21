import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PeriodosTableComponent } from './ope-periodos-table.component';

describe('PeriodosTableComponent', () => {
  let component: PeriodosTableComponent;
  let fixture: ComponentFixture<PeriodosTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PeriodosTableComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PeriodosTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
