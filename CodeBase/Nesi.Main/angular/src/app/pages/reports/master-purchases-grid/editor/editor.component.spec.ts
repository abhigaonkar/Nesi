import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterPurchasesGridEditorComponent } from './editor.component';

describe('EditorComponent', () => {
  let component: MasterPurchasesGridEditorComponent;
  let fixture: ComponentFixture<MasterPurchasesGridEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterPurchasesGridEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterPurchasesGridEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
