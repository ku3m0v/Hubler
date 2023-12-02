import { TestBed } from '@angular/core/testing';

import { NonperishableService } from './nonperishable.service';

describe('NonperishableService', () => {
  let service: NonperishableService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NonperishableService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
