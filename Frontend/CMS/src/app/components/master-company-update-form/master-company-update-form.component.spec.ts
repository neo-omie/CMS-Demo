import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterCompanyUpdateFormComponent } from './master-company-update-form.component';

describe('MasterCompanyUpdateFormComponent', () => {
  let component: MasterCompanyUpdateFormComponent;
  let fixture: ComponentFixture<MasterCompanyUpdateFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MasterCompanyUpdateFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MasterCompanyUpdateFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
