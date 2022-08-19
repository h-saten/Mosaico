import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AirdropParticipantsComponent } from './airdrop-participants.component';

describe('AirdropParticipantsComponent', () => {
  let component: AirdropParticipantsComponent;
  let fixture: ComponentFixture<AirdropParticipantsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AirdropParticipantsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AirdropParticipantsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
