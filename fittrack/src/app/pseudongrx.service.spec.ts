import { TestBed } from '@angular/core/testing';

import { PseudongrxService } from './pseudongrx.service';

describe('PseudongrxService', () => {
  let service: PseudongrxService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PseudongrxService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
