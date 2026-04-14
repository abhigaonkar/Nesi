import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterQuoteEditorComponent } from './editor.component';

describe('EditorComponent', () => {
  let component: MasterQuoteEditorComponent;
  let fixture: ComponentFixture<MasterQuoteEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterQuoteEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterQuoteEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
