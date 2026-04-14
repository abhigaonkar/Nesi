import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';
import { TreeNode } from 'primeng/primeng';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-edit-storage',
  templateUrl: './quote-edit-storage.component.html',
  styleUrls: ['./quote-edit-storage.component.css']
})
export class QuoteEditStorageComponent extends QuoteEditFormBase implements OnInit {
  QuoteFileDirectory: TreeNode[];

  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    private route: ActivatedRoute,
  ) {
    super(winRef, store, cs, ts);
    super.Init(CONFIG.apiURL.page.quotes.new);
  }
  createForm() {

  }

  initQ() {
    if (!this.QuoteFileDirectory && this._q && this._q.quote_id) {
      this.cs.getList<TreeNode>(CONFIG.apiURL.core.fileManager.Quote + this._q.quote_id + '/' + this._q.revision)
        .subscribe(
        (res) => {
          this.QuoteFileDirectory = res;
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        });
    }
  }
  ngOnInit() {
  }

}
