import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilteredContractsComponent } from './filtered-contracts.component';

describe('FilteredContractsComponent', () => {
  let component: FilteredContractsComponent;
  let fixture: ComponentFixture<FilteredContractsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FilteredContractsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FilteredContractsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
