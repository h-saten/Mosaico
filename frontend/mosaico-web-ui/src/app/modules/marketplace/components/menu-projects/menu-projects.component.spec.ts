import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuProjectsComponent } from './menu-projects.component';

describe('MenuProjectsComponent', () => {
  let component: MenuProjectsComponent;
  let fixture: ComponentFixture<MenuProjectsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MenuProjectsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
