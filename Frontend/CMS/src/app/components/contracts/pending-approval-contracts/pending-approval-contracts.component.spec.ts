import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingApprovalContractsComponent } from './pending-approval-contracts.component';

describe('PendingApprovalContractsComponent', () => {
  let component: PendingApprovalContractsComponent;
  let fixture: ComponentFixture<PendingApprovalContractsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PendingApprovalContractsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PendingApprovalContractsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
