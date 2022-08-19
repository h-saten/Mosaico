import { TestBed } from '@angular/core/testing';

import { MenuProjectsService } from './menu-projects.service';

describe('MenuProjectsService', () => {
  let service: MenuProjectsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MenuProjectsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
