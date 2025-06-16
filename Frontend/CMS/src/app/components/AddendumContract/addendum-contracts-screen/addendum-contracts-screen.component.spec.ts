import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddendumContractsScreenComponent } from './addendum-contracts-screen.component';

describe('AddendumContractsScreenComponent', () => {
  let component: AddendumContractsScreenComponent;
  let fixture: ComponentFixture<AddendumContractsScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddendumContractsScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddendumContractsScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
