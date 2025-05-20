import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EscalationMatrixContractScreenComponent } from './escalation-matrix-contract-screen.component';

describe('EscalationMatrixContractScreenComponent', () => {
  let component: EscalationMatrixContractScreenComponent;
  let fixture: ComponentFixture<EscalationMatrixContractScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EscalationMatrixContractScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EscalationMatrixContractScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
