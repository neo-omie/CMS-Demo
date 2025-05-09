import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApproverMatrixContractTablePaginatorComponent } from './approver-matrix-contract-table-paginator.component';

describe('ApproverMatrixContractTablePaginatorComponent', () => {
  let component: ApproverMatrixContractTablePaginatorComponent;
  let fixture: ComponentFixture<ApproverMatrixContractTablePaginatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApproverMatrixContractTablePaginatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApproverMatrixContractTablePaginatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
