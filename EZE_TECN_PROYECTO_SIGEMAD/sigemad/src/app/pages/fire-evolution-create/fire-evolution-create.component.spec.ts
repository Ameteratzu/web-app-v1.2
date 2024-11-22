import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FireEvolutionCreateComponent } from './fire-evolution-create.component';

describe('FireEvolutionCreateComponent', () => {
  let component: FireEvolutionCreateComponent;
  let fixture: ComponentFixture<FireEvolutionCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FireEvolutionCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FireEvolutionCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
