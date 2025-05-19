import { TestBed } from '@angular/core/testing';

import { NoticeWithdrawalService } from './notice-withdrawal.service';

describe('NoticeWithdrawalService', () => {
  let service: NoticeWithdrawalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NoticeWithdrawalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
