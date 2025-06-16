import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WithdrawalContractMailBodyModalComponent } from './withdrawal-contract-mail-body-modal.component';

describe('WithdrawalContractMailBodyModalComponent', () => {
  let component: WithdrawalContractMailBodyModalComponent;
  let fixture: ComponentFixture<WithdrawalContractMailBodyModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WithdrawalContractMailBodyModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WithdrawalContractMailBodyModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
