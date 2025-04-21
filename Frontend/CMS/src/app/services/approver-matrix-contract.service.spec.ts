import { TestBed } from '@angular/core/testing';

import { ApproverMatrixContractService } from './approver-matrix-contract.service';

describe('ApproverMatrixContractService', () => {
  let service: ApproverMatrixContractService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApproverMatrixContractService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
