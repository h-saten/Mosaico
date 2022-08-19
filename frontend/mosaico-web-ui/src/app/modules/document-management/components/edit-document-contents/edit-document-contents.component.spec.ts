import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDocumentContentsComponent } from './edit-document-contents.component';

describe('EditDocumentContentsComponent', () => {
  let component: EditDocumentContentsComponent;
  let fixture: ComponentFixture<EditDocumentContentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EditDocumentContentsComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditDocumentContentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
