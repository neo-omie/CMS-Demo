import { TestBed } from '@angular/core/testing';

import { ContractTypeMasterService } from './contract-type-master.service';

describe('ContractTypeMasterService', () => {
  let service: ContractTypeMasterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContractTypeMasterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
