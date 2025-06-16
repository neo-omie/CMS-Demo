import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractViewModalComponent } from './contract-view-modal.component';

describe('ContractViewModalComponent', () => {
  let component: ContractViewModalComponent;
  let fixture: ComponentFixture<ContractViewModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContractViewModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContractViewModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
