import { TestBed } from '@angular/core/testing';

import { EscalationMatrixContractService } from './escalation-matrix-contract.service';

describe('EscalationMatrixContractService', () => {
  let service: EscalationMatrixContractService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EscalationMatrixContractService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
