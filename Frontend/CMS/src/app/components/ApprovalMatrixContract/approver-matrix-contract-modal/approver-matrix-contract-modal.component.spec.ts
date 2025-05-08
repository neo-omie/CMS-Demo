import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApproverMatrixContractModalComponent } from './approver-matrix-contract-modal.component';

describe('ApproverMatrixContractModalComponent', () => {
  let component: ApproverMatrixContractModalComponent;
  let fixture: ComponentFixture<ApproverMatrixContractModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApproverMatrixContractModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApproverMatrixContractModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
