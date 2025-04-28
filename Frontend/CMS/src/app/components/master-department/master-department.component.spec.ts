import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterDepartmentComponent } from './master-department.component';

describe('MasterDepartmentComponent', () => {
  let component: MasterDepartmentComponent;
  let fixture: ComponentFixture<MasterDepartmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MasterDepartmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MasterDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
