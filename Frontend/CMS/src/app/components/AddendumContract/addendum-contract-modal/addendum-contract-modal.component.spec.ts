import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddendumContractModalComponent } from './addendum-contract-modal.component';

describe('AddendumContractModalComponent', () => {
  let component: AddendumContractModalComponent;
  let fixture: ComponentFixture<AddendumContractModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddendumContractModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddendumContractModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
