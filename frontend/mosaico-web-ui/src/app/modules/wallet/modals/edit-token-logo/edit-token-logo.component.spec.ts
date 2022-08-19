import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EditTokenLogoComponent } from './edit-token-logo.component';

describe('EditTokenLogoComponent', () => {
  let component: EditTokenLogoComponent;
  let fixture: ComponentFixture<EditTokenLogoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditTokenLogoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditTokenLogoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
