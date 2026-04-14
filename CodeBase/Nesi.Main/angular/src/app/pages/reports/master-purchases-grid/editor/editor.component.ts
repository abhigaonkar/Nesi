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
  // tslint:disable-next-line:component-selector
  selector: 'master-purchases-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})
export class MasterPurchasesGridEditorComponent extends FormMessageBase implements OnInit {
  public _masterPurchase: any;
  @ViewChild(OverlayPanel) overlayPanel: OverlayPanel;

  // tslint:disable-next-line:no-input-rename
  @Input('editLink') editLink = '';
  // tslint:disable-next-line:no-input-rename
  @Input('apStatusList') apStatusList = [];
  @Output() onExit = new EventEmitter();

  @Input()
  set masterPurchase(value: any) {
    if (!value) {
      return;
    }

    this._masterPurchase = value;

    this.userform.reset({
      poprog_hasproblem_notes: this._masterPurchase.poprog_hasproblem_notes,
      poprog_apstatus: this._masterPurchase.poprog_apstatus,
      poprog_id: this._masterPurchase.poprog_id, // used for post back to server
      copy: this._masterPurchase
    });

  }

  constructor(private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService) {
    super(store, cs);
    this.createForm();
  }

  ngOnInit() {
    this.postUrl = this.editLink;

    this.onChange.subscribe(
      this.success
    );
  }

  createForm() {
    this.userform = this.fb.group({
      poprog_hasproblem_notes: '',
      poprog_apstatus: '',
      poprog_id: '',
      copy: ''
    });
  }

  exit() {
    this.onExit.emit();
  }

  success(event) {
    console.dir(event);
    event.post.copy.poprog_hasproblem_notes = event.result.poprog_hasproblem_notes;
    event.post.copy.poprog_apstatus = event.result.poprog_apstatus;
  }

}
