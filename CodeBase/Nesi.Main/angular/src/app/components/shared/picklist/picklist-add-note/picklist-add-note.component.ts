import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormMessageBase } from 'app/core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../../reducers';
import { CONFIG } from 'app/configuration';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'nesi-picklist-add-note',
  templateUrl: './picklist-add-note.component.html',
  styleUrls: ['./picklist-add-note.component.css']
})
export class PicklistAddNoteComponent extends FormMessageBase implements OnInit {

  @Input()
  notes: string;

  @Input()
  id: string;


  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.shared.pickList.quoteNote);
  }

  createForm() {
    this.userform = this.fb.group({
      'note': ['', Validators.required],
      'id': '',
      'is_new': '',
    });
  
  }

  ngOnInit() {
    this.initFormvalue = {
      'note': '',
      'id': this.id,
      'is_new': true,
    }
    this.submitReset();
  }

}
