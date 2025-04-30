import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractTypeMasterComponent } from './contract-type-master.component';

describe('ContractTypeMasterComponent', () => {
  let component: ContractTypeMasterComponent;
  let fixture: ComponentFixture<ContractTypeMasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContractTypeMasterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContractTypeMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
