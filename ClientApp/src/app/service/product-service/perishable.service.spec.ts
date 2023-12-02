import { TestBed } from '@angular/core/testing';

import { PerishableService } from './perishable.service';

describe('PerishableService', () => {
  let service: PerishableService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PerishableService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
