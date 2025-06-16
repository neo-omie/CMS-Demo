import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractAddAddendumContractModalComponent } from './contract-add-addendum-contract-modal.component';

describe('ContractAddAddendumContractModalComponent', () => {
  let component: ContractAddAddendumContractModalComponent;
  let fixture: ComponentFixture<ContractAddAddendumContractModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContractAddAddendumContractModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContractAddAddendumContractModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
