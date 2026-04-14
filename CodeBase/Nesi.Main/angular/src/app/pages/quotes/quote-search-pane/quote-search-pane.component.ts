import { LazyLoadEvent } from 'primeng/primeng';
import { Component, OnInit, Input } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { LabelValueString } from '../../../models/Shared/labelValueInt';
import { QuoteFormBase } from '../_base/quoteFormBase';
import { WindowRef } from '../../../services/shared/windowRef';
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-search-pane',
  templateUrl: './quote-search-pane.component.html',
  styleUrls: ['./quote-search-pane.component.css']
})
export class QuoteSearchPaneComponent extends QuoteFormBase implements OnInit {
  @Input()
  isDialog = false;

  businessUnits: LabelValueInt[];
  users: LabelValueInt[];
  status: LabelValueString[];
  searchResults: any[] = [];
  totalResultRecords = 0;
  page = 0;
  pageSize = 100;
  totalPage = 0;
  afterSubmited = false;

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private fb: FormBuilder,
    private ts: TokenService,
    protected winRef: WindowRef,
  ) {
    super(winRef, store, cs);
    super.Init(CONFIG.apiURL.page.quotes.search);
    this.doSearch = true;
  }

  createForm() {
    this.userform = this.fb.group({
      'quote_id': ['', Validators.pattern(CONFIG.SQL_PATTERN)],
      'quote_by': 0,
      'quote_businessUnit': this.ts.currentUser.businessUnitId,
      'date_before': '',
      'date_after': '',
      'job_description': ['', Validators.pattern(CONFIG.SQL_PATTERN)],
      'customer_name': ['', Validators.pattern(CONFIG.SQL_PATTERN)],
      'status': 0,
      'page': 0,
      'pageSize': this.pageSize,
    });
  }

  formValidateBefore() {
    this.setDateFieldNullToEmptyString('date_before');
    this.setDateFieldNullToEmptyString('date_after');
  }

  ngOnInit() {
    this.cs.getObject<any>(CONFIG.apiURL.page.quotes.searchInit)
      .subscribe(
      (res: any) => {
        this.businessUnits = res.businessUnitList;
        this.users = res.userListInBusinessUnit;
        this.status = res.status;
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }

  loadUserList(event: any) {
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.quotes.searchUserList + event.value)
      .subscribe(
      (res: LabelValueInt[]) => {
        this.users = res;
        this.userform.get('quote_by').setValue(0);
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }

  searchSuccess(res: any) {
    this.totalResultRecords = res.totalRecorders;
    this.searchResults = res.data;
    // this.page = res.page;
    // this.pageSize = res.pageSize;
    // this.totalPage = Math.ceil(this.totalResultRecords / this.pageSize);
    this.afterSubmited = true;
  }

  // loadSearchLazy(event: LazyLoadEvent) {
  //   if (this.totalResultRecords === 0) { return; }
  //   this.page = event.first / this.pageSize;

  //   super.LOG(event, 'loadsearchlazy');
  //   this.userform.get('page').setValue(this.page);
  //   this.cs.postObject<any>(this.postUrl, this.userform.value)
  //     .subscribe(
  //     (res: any) => {
  //       this.searchSuccess(res);
  //     }
  //     );
  // }


}
