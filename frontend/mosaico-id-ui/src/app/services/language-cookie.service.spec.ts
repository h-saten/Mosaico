import { TestBed } from '@angular/core/testing';

import { LanguageCookieService } from './language-cookie.service';

describe('LanguageCookieService', () => {
  let service: LanguageCookieService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LanguageCookieService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
