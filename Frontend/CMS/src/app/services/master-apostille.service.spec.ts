import { TestBed } from '@angular/core/testing';

import { MasterApostilleService } from './master-apostille.service';

describe('MasterApostilleService', () => {
  let service: MasterApostilleService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MasterApostilleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
