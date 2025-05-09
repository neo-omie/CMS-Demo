import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApproverMatrixContractTableComponent } from './approver-matrix-contract-table.component';

describe('ApproverMatrixContractTableComponent', () => {
  let component: ApproverMatrixContractTableComponent;
  let fixture: ComponentFixture<ApproverMatrixContractTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApproverMatrixContractTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApproverMatrixContractTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
