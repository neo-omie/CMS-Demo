import { TestBed } from '@angular/core/testing';

import { MasterEmployeeService } from './master-employee.service';

describe('MasterEmployeeService', () => {
  let service: MasterEmployeeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MasterEmployeeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
