import { TestBed } from '@angular/core/testing';

import { CashregisterService } from './cashregister.service';

describe('CashregisterService', () => {
  let service: CashregisterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CashregisterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
