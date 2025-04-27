import { TestBed } from '@angular/core/testing';

import { MasterDepartmentService } from './master-department.service';

describe('MasterDepartmentService', () => {
  let service: MasterDepartmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MasterDepartmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
