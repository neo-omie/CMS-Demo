import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllClassifiedContractViewAddModalComponent } from './all-classified-contract-view-add-modal.component';

describe('AllClassifiedContractViewAddModalComponent', () => {
  let component: AllClassifiedContractViewAddModalComponent;
  let fixture: ComponentFixture<AllClassifiedContractViewAddModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllClassifiedContractViewAddModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllClassifiedContractViewAddModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
