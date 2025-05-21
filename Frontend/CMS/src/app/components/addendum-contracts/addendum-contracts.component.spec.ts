import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddendumContractsComponent } from './addendum-contracts.component';

describe('AddendumContractsComponent', () => {
  let component: AddendumContractsComponent;
  let fixture: ComponentFixture<AddendumContractsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddendumContractsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddendumContractsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
