import { TestBed } from '@angular/core/testing';

import { AddAddendumContractsService } from './add-addendum-contracts.service';

describe('AddAddendumContractsService', () => {
  let service: AddAddendumContractsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AddAddendumContractsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
