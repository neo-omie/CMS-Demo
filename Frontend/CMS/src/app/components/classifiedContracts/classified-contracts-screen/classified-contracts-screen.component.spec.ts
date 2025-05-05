import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassifiedContractsScreenComponent } from './classified-contracts-screen.component';

describe('ClassifiedContractsScreenComponent', () => {
  let component: ClassifiedContractsScreenComponent;
  let fixture: ComponentFixture<ClassifiedContractsScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClassifiedContractsScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClassifiedContractsScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
