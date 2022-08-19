import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTransactionsComponent } from './project-transactions.component';

describe('ProjectTransactionsComponent', () => {
  let component: ProjectTransactionsComponent;
  let fixture: ComponentFixture<ProjectTransactionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectTransactionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTransactionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
