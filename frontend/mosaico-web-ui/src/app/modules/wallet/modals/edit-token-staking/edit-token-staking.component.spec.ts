import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditTokenStakingComponent } from './edit-token-staking.component';

describe('EditTokenStakingComponent', () => {
  let component: EditTokenStakingComponent;
  let fixture: ComponentFixture<EditTokenStakingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditTokenStakingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditTokenStakingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
