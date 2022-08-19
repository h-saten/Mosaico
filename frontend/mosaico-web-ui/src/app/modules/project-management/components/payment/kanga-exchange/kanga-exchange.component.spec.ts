import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KangaExchangeComponent } from './kanga-exchange.component';

describe('KangaExchangeComponent', () => {
  let component: KangaExchangeComponent;
  let fixture: ComponentFixture<KangaExchangeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ KangaExchangeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(KangaExchangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
