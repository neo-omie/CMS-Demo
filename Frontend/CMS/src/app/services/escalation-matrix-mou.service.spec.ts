import { TestBed } from '@angular/core/testing';

import { EscalationMatrixMouService } from './escalation-matrix-mou.service';

describe('EscalationMatrixMouService', () => {
  let service: EscalationMatrixMouService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EscalationMatrixMouService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
