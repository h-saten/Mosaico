import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlockchainLinksComponent } from './blockchain-links.component';

describe('BlockchainLinksComponent', () => {
  let component: BlockchainLinksComponent;
  let fixture: ComponentFixture<BlockchainLinksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BlockchainLinksComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BlockchainLinksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
