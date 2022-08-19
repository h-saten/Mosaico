import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAirdropComponent } from './create-airdrop.component';

describe('CreateAirdropComponent', () => {
  let component: CreateAirdropComponent;
  let fixture: ComponentFixture<CreateAirdropComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateAirdropComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateAirdropComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
