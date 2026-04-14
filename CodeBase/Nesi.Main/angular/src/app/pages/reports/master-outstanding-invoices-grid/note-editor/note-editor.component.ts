import { Component, OnInit, EventEmitter, Output, Input, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';
import {
  OverlayPanel
} from 'primeng/primeng';

@Component({
  selector: 'nesi-moig-note-editor',
  templateUrl: './note-editor.component.html',
  styleUrls: ['./note-editor.component.css']
})
export class NoteEditorComponent extends FormMessageBase implements OnInit {
  public _existingNote: any = {};

  // tslint:disable-next-line:no-input-rename
  @Input('editMode') editMode = '';
  // tslint:disable-next-line:no-input-rename
  @Input('invoice') invoice = '';

  @Input()
  set existingNote(value: any) {
    if (!value) {
      return;
    }

    this._existingNote = value;

    this.userform.reset({
      note: this._existingNote.noteForEditing,
      noteId: this._existingNote.id
    });
  }

  @Output() onExit = new EventEmitter<boolean>();

  constructor(private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService) {
    super(store, cs);
    this.createForm();
  }

  ngOnInit() {

  }

  createForm() {
    this.userform = this.fb.group({
      woprogId: '',
      noteId: '',
      note: ''
    });
  }

  cancel() {
    this.onExit.emit(false);
  }

  addOrUpdateNote() {
    const invoice: any = this.invoice;
    const note = this.userform.get('note').value;
    let noteId = this.userform.get('noteId').value;
    if (this.editMode === 'Add') {
      noteId = 0;
    }

    const postData = {
      note: note,
      noteId: noteId,
      woId: invoice.woprog_id
    };

    this.cs.postDataExtra(CONFIG.apiURL.page.reports.masterOutstandingInvoicesGrid.addOrUpdateNote, postData).subscribe(
      (res) => {
        super.PushResponseMessage(res.data)
        this.onExit.emit(true);
      }
    );
  }
}
