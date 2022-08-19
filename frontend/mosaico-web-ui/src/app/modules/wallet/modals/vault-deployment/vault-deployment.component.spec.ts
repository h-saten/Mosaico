import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VaultDeploymentComponent } from './vault-deployment.component';

describe('VaultDeploymentComponent', () => {
  let component: VaultDeploymentComponent;
  let fixture: ComponentFixture<VaultDeploymentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VaultDeploymentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VaultDeploymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
