import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterEmployeeComponent } from './master-employee.component';

describe('MasterEmployeeComponent', () => {
  let component: MasterEmployeeComponent;
  let fixture: ComponentFixture<MasterEmployeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MasterEmployeeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MasterEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
