import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAffiliationOverviewComponent } from './user-affiliation-overview.component';

describe('UserAffiliationOverviewComponent', () => {
  let component: UserAffiliationOverviewComponent;
  let fixture: ComponentFixture<UserAffiliationOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserAffiliationOverviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAffiliationOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
