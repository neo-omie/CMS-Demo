import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EscalationMatrixContractModalComponent } from './escalation-matrix-contract-modal.component';

describe('EscalationMatrixContractModalComponent', () => {
  let component: EscalationMatrixContractModalComponent;
  let fixture: ComponentFixture<EscalationMatrixContractModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EscalationMatrixContractModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EscalationMatrixContractModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
