import { TestBed } from '@angular/core/testing';

import { AccessBuyGuard } from './access-buy.guard';

describe('AccessBuyGuard', () => {
  let guard: AccessBuyGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AccessBuyGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
