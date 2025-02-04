import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepCancelComponent } from './step-cancel.component';

describe('StepCancelComponent', () => {
  let component: StepCancelComponent;
  let fixture: ComponentFixture<StepCancelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StepCancelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StepCancelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
