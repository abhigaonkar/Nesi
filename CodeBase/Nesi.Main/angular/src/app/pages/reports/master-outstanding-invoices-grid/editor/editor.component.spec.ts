import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterOutstandingInvoiceEditorComponent } from './editor.component';

describe('EditorComponent', () => {
  let component: MasterOutstandingInvoiceEditorComponent;
  let fixture: ComponentFixture<MasterOutstandingInvoiceEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterOutstandingInvoiceEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterOutstandingInvoiceEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
