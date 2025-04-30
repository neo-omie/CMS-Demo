import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractsScreenComponent } from './contracts-screen.component';

describe('ContractsScreenComponent', () => {
  let component: ContractsScreenComponent;
  let fixture: ComponentFixture<ContractsScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContractsScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContractsScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
