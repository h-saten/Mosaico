import { TestBed } from '@angular/core/testing';

import { StonlyService } from './stonly.service';

describe('StonlyService', () => {
  let service: StonlyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StonlyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
