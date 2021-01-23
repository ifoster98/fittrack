import { TestBed } from '@angular/core/testing';
import { FittrackService } from './fittrack.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('FittrackService', () => {
  let service: FittrackService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ]
    });
    service = TestBed.inject(FittrackService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
