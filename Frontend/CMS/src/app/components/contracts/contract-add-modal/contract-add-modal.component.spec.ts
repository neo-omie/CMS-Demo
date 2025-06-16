import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractAddModalComponent } from './contract-add-modal.component';

describe('ContractAddModalComponent', () => {
  let component: ContractAddModalComponent;
  let fixture: ComponentFixture<ContractAddModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContractAddModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContractAddModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
