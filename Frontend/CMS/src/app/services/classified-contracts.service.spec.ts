import { TestBed } from '@angular/core/testing';

import { ClassifiedContractsService } from './classified-contracts.service';

describe('ClassifiedContractsService', () => {
  let service: ClassifiedContractsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClassifiedContractsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
