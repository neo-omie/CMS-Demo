import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EscalationMatrixContractComponent } from './escalation-matrix-contract.component';

describe('EscalationMatrixContractComponent', () => {
  let component: EscalationMatrixContractComponent;
  let fixture: ComponentFixture<EscalationMatrixContractComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EscalationMatrixContractComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EscalationMatrixContractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
