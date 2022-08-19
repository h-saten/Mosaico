import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewCompanyMemberComponent } from './new-company-member.component';

describe('NewCompanyMemberComponent', () => {
  let component: NewCompanyMemberComponent;
  let fixture: ComponentFixture<NewCompanyMemberComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewCompanyMemberComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewCompanyMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
