import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllClassifiedContractPostTerminationModalComponent } from './all-classified-contract-post-termination-modal.component';

describe('AllClassifiedContractPostTerminationModalComponent', () => {
  let component: AllClassifiedContractPostTerminationModalComponent;
  let fixture: ComponentFixture<AllClassifiedContractPostTerminationModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllClassifiedContractPostTerminationModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllClassifiedContractPostTerminationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
