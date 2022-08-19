import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyVotingComponent } from './company-voting.component';

describe('CompanyVotingComponent', () => {
  let component: CompanyVotingComponent;
  let fixture: ComponentFixture<CompanyVotingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyVotingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyVotingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
