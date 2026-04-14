import { Component, Input, OnChanges, Output, EventEmitter, ViewChild, ElementRef } from "@angular/core";
import { OverlayPanel } from 'primeng/primeng';
import { Store } from '@ngrx/store';
import * as fromMessage from '../../../../actions/layout/growlMessage';
import * as fromRoot from '../../../../reducers';

@Component({
  selector: 'text-area-layover',
  templateUrl: './textAreaLayover.component.html',
})
export class TextAreaLayover implements OnChanges {
  @Input('rowIndex') rowIndex;
  @Input('rowData') rowData;
  @Input('column') column;
  @Output() saveEditedNote = new EventEmitter();
  public noteValue: string;

  constructor(protected store: Store<fromRoot.State>) {

  }

  ngOnChanges() {
    this.noteValue = this.rowData[this.column.field];
  }

  /**
   * Revert the column changes and toggle overlayPanel visibility
   * @param  {} $event
   * @param  {OverlayPanel} op
   */
  cancelEditing($event, op: OverlayPanel) {
    this.noteValue = this.rowData[this.column.field];
  }


  /**
   * Emit the edited row and toggle overlayPanel visibility
   * @param  {} $event
   * @param  {OverlayPanel} op
   */
  saveEdited($event, op: OverlayPanel) {
    if (this.doValidationsPass(this.noteValue)) {
    this.rowData[this.column.field] = this.noteValue;
    this.saveEditedNote.emit(this.rowData);
      op.hide();
  }
}

  /**
   * Check notes validations
   * @param  {} valueEntered
   * @returns boolean
   */
  doValidationsPass(valueEntered): boolean {
    let isNoteValid: boolean = true;
    const validations = this.column.validations.length ? JSON.parse(this.column.validations) : {};
    if (validations.required && valueEntered.trim() == "") {
      this.store.dispatch(new fromMessage.PushErrorMessage(`${this.column.label} is required.`));
      isNoteValid = false;
    }

    if (validations.maxLength && valueEntered.length > validations.maxLength) {
      this.store.dispatch(new fromMessage.PushErrorMessage(`${this.column.label} should have atmost ${validations.maxLength} characters.`));
      isNoteValid = false;
    }
    return isNoteValid;
  }
}

