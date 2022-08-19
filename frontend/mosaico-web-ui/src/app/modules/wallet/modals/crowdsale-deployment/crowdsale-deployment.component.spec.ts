import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrowdsaleDeploymentComponent } from './crowdsale-deployment.component';

describe('CrowdsaleDeploymentComponent', () => {
  let component: CrowdsaleDeploymentComponent;
  let fixture: ComponentFixture<CrowdsaleDeploymentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CrowdsaleDeploymentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CrowdsaleDeploymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
