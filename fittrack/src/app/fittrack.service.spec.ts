import { TestBed } from '@angular/core/testing';

import { FittrackService } from './fittrack.service';

describe('FittrackService', () => {
  let service: FittrackService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FittrackService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
