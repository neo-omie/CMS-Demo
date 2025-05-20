import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalMatrixMouModalComponent } from './approval-matrix-mou-modal.component';

describe('ApprovalMatrixMouModalComponent', () => {
  let component: ApprovalMatrixMouModalComponent;
  let fixture: ComponentFixture<ApprovalMatrixMouModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApprovalMatrixMouModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApprovalMatrixMouModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
