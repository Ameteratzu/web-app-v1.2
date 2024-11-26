import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FireCoordinationDataComponent } from './fire-coordination-data.component';

describe('FireCoordinationDataComponent', () => {
  let component: FireCoordinationDataComponent;
  let fixture: ComponentFixture<FireCoordinationDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FireCoordinationDataComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FireCoordinationDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
