import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllClassifiedContractScreenComponent } from './all-classified-contract-screen.component';

describe('AllClassifiedContractScreenComponent', () => {
  let component: AllClassifiedContractScreenComponent;
  let fixture: ComponentFixture<AllClassifiedContractScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllClassifiedContractScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllClassifiedContractScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
