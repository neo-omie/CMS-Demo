import { TestBed } from '@angular/core/testing';

import { MasterDocumentService } from './master-document.service';

describe('MasterDocumentService', () => {
  let service: MasterDocumentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MasterDocumentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
