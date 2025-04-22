import { TestBed } from '@angular/core/testing';

import { ApprovalMatrixMouService } from './approval-matrix-mou.service';

describe('ApprovalMatrixMouService', () => {
  let service: ApprovalMatrixMouService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApprovalMatrixMouService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
