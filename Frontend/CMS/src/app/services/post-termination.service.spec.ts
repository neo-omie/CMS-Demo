import { TestBed } from '@angular/core/testing';

import { PostTerminationService } from './post-termination.service';

describe('PostTerminationService', () => {
  let service: PostTerminationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PostTerminationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
