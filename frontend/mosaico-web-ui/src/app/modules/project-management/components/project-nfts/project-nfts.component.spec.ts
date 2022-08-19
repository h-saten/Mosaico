import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectNftsComponent } from './project-nfts.component';

describe('ProjectNftsComponent', () => {
  let component: ProjectNftsComponent;
  let fixture: ComponentFixture<ProjectNftsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectNftsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectNftsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
