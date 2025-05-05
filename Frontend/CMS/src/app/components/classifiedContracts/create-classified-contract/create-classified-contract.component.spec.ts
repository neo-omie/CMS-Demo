import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateClassifiedContractComponent } from './create-classified-contract.component';

describe('CreateClassifiedContractComponent', () => {
  let component: CreateClassifiedContractComponent;
  let fixture: ComponentFixture<CreateClassifiedContractComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateClassifiedContractComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateClassifiedContractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
