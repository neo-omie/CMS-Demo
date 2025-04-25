import { TestBed } from '@angular/core/testing';

import { CompanyMasterService } from './company-master.service';

describe('CompanyMasterService', () => {
  let service: CompanyMasterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CompanyMasterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
