import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllClassifiedContractComponent } from './all-classified-contract.component';

describe('AllClassifiedContractComponent', () => {
  let component: AllClassifiedContractComponent;
  let fixture: ComponentFixture<AllClassifiedContractComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllClassifiedContractComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllClassifiedContractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
