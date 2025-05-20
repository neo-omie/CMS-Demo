import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalMatrixMouScreenComponent } from './approval-matrix-mou-screen.component';

describe('ApprovalMatrixMouScreenComponent', () => {
  let component: ApprovalMatrixMouScreenComponent;
  let fixture: ComponentFixture<ApprovalMatrixMouScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApprovalMatrixMouScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApprovalMatrixMouScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
