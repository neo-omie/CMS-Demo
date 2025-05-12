import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EscalationMatrixMouModalComponent } from './escalation-matrix-mou-modal.component';

describe('EscalationMatrixMouModalComponent', () => {
  let component: EscalationMatrixMouModalComponent;
  let fixture: ComponentFixture<EscalationMatrixMouModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EscalationMatrixMouModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EscalationMatrixMouModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
