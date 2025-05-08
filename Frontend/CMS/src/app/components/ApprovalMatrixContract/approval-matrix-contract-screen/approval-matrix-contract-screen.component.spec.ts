import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalMatrixContractScreenComponent } from './approval-matrix-contract-screen.component';

describe('ApprovalMatrixContractScreenComponent', () => {
  let component: ApprovalMatrixContractScreenComponent;
  let fixture: ComponentFixture<ApprovalMatrixContractScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApprovalMatrixContractScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApprovalMatrixContractScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
