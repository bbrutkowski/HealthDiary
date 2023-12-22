import { TestBed } from '@angular/core/testing';

import { RequestHelperBaseService } from './request-helper-base.service';

describe('RequestHelperBaseService', () => {
  let service: RequestHelperBaseService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RequestHelperBaseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
