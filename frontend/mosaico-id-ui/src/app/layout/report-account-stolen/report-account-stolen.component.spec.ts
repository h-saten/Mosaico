import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportAccountStolenComponent } from './report-account-stolen.component';

describe('ReportAccountStolenComponent', () => {
  let component: ReportAccountStolenComponent;
  let fixture: ComponentFixture<ReportAccountStolenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportAccountStolenComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportAccountStolenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
