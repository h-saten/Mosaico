import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VestingTokenDynamicComponent } from './vesting-token-dynamic.component';

describe('VestingTokenDynamicComponent', () => {
  let component: VestingTokenDynamicComponent;
  let fixture: ComponentFixture<VestingTokenDynamicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VestingTokenDynamicComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VestingTokenDynamicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
