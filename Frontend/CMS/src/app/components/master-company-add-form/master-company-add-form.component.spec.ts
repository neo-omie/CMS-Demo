import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterCompanyAddFormComponent } from './master-company-add-form.component';

describe('MasterCompanyAddFormComponent', () => {
  let component: MasterCompanyAddFormComponent;
  let fixture: ComponentFixture<MasterCompanyAddFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MasterCompanyAddFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MasterCompanyAddFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
