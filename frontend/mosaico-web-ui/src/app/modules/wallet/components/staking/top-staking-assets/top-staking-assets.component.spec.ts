import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopStakingAssetsComponent } from './top-staking-assets.component';

describe('TopStakingAssetsComponent', () => {
  let component: TopStakingAssetsComponent;
  let fixture: ComponentFixture<TopStakingAssetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TopStakingAssetsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TopStakingAssetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
