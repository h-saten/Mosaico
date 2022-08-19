import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StageDeploymentComponent } from './stage-deployment.component';

describe('StageDeploymentComponent', () => {
  let component: StageDeploymentComponent;
  let fixture: ComponentFixture<StageDeploymentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StageDeploymentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StageDeploymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
