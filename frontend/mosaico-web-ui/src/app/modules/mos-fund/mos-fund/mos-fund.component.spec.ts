import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MosFundComponent } from './mos-fund.component';

describe('MosFundComponent', () => {
  let component: MosFundComponent;
  let fixture: ComponentFixture<MosFundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MosFundComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MosFundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
