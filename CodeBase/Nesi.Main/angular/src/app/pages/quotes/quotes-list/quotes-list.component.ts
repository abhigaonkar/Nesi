import { Component, OnInit } from '@angular/core';
import { WindowRef } from '../../../services/shared/windowRef';
import { MessageBase } from '../../../core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { QuoteBase } from '../_base/quoteBase';
import { LabelValueInt } from 'app/models/Shared/labelValueString';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quotes-list',
  templateUrl: './quotes-list.component.html',
  styleUrls: ['./quotes-list.component.css']
})
export class QuotesListComponent extends QuoteBase implements OnInit {

  header: string[];
  accordionView = false;
  body: any[];
  sum: string[];
  todonowList: any[];
  businessUnit_list: LabelValueInt[];
  selected_businessUnits: number;
  all_member_list: any[];
  member_list: any[];
  selected_members: number[];
  headerLabels = [
    'STAGE 1 GO/NO GO',
    'STAGE 4 GO/NO GO',
    'TO BE QUOTED',
    'FINAL REVIEW',
    'TO BE DELIVERED',
    'TO BE VERIFIED',
    'WAITING ACCEPTANCE',
    'FOLLOW UP TODAY',
  ];
  loading = false;

  use_quote_process = false;

  constructor(
    public ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
  ) {
    super(winRef, store);
  }

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    this.loading = true;

    this.cs.getObject(CONFIG.apiURL.page.quotes.profile)
      .subscribe(
      (res: any) => {
        this.header = res.summary.header;
        this.body = res.summary.body;
        this.sum = res.summary.sum;
        this.todonowList = res.todonow_list;
        this.businessUnit_list = res.businessUnit_list;
        this.selected_businessUnits = this.ts.currentUser.businessUnitId;
        this.all_member_list = res.member_list;
        this.getMember_list(null);
        this.selected_members = [this.ts.currentUser.id];
        this.loading = false;
        this.use_quote_process = res.use_quote_process;
      }, (err:any)=>{
        this.PushErrorMessage(err);
        this.loading=false;
      }
      );
  }

  getMember_list(event: any) {
    this.member_list = this.all_member_list.filter(x =>
      this.selected_businessUnits === x.bu_id);
    if (this.selected_businessUnits === this.ts.currentUser.businessUnitId) {
      this.selected_members = [this.ts.currentUser.id];
    } else {
      this.selected_members = this.member_list.map(x => x.value);
    }
  }

  getQuoteLevel(quote: any): string {
    return quote.is_level_3 ? 'Level 3' : quote.is_level_2 ? 'Level 2' : 'Level 1';
  }

  filter() {
    this.loading = true;
    this.body = [];
    this.todonowList = [];
    this.header = [];
    this.sum = [];
    this.cs.postObject<any>(CONFIG.apiURL.page.quotes.profileFilter, this.selected_members)
      .subscribe(res => {
        this.header = res.header;
        this.body = res.body;
        this.sum = res.sum;
        this.todonowList = res.todonow_list;
        this.loading = false;
      },
      (err:any)=>{
        this.PushErrorMessage(err);
        this.loading=false;
      }
      );
  }

  is_use_quote_process(index: number) {
    return this.use_quote_process || (index !== 0 && index !== 1 && index !== 3);
  }
}
