import { TestBed } from '@angular/core/testing';


// @ts-ignore
//FIXME TS2305: Module '"./store.service"' has no exported member 'StoreService'.
import { StoreService } from './store.service';

describe('StoreService', () => {
  let service: StoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
