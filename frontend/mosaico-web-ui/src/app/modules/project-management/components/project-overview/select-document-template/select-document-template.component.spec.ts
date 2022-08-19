import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectDocumentTemplateComponent } from './select-document-template.component';

describe('SelectDocumentTemplateComponent', () => {
  let component: SelectDocumentTemplateComponent;
  let fixture: ComponentFixture<SelectDocumentTemplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectDocumentTemplateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectDocumentTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
