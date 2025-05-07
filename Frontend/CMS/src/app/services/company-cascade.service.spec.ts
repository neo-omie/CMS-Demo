import { TestBed } from '@angular/core/testing';

import { CompanyCascadeService } from './company-cascade.service';

describe('CompanyCascadeService', () => {
  let service: CompanyCascadeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CompanyCascadeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
