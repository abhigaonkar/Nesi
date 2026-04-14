import { Component, OnInit, Input, EventEmitter, Output, ViewChild } from '@angular/core';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';
import { FormBuilder } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { Store } from '@ngrx/store';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import * as fromRoot from '../../../reducers';
import { CONFIG } from '../../../configuration';
import { FormMessageBase } from 'app/core/formMessageBaseComponent';
import { QuoteDetailEditStrategyQuestionComponent } from 'app/pages/quotes/quote-detail-edit-strategy-question/quote-detail-edit-strategy-question.component';
import { DomSanitizer } from '@angular/platform-browser';
import { DataExtra } from 'app/models/core/dataExtra';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-detail-edit-strategy-item',
  templateUrl: './quote-detail-edit-strategy-item.component.html',
  styleUrls: ['./quote-detail-edit-strategy-item.component.css']
})
export class QuoteDetailEditStrategyItemComponent extends FormMessageBase implements OnInit {
  item: any;
  @Input()
  set Item(value: any) {
    // CONFIG.LOG(value, 'value in quote detail edit strategy item');
    if (!this.item) { this.item = value; }
  };
  @Input()
  quote_id: string;
  @Input()
  buid: string;

  @Output() onbuttonClick = new EventEmitter();
  @Output() assigntoChanged = new EventEmitter();
  @Output() quoestionDone = new EventEmitter();
  @ViewChild(QuoteDetailEditStrategyQuestionComponent)
  question: QuoteDetailEditStrategyQuestionComponent;

  showStage1Kill = false;
  dialogType: string;
  dialogDisplay = false;
  reason = '';

  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
  ) {
    super(store, cs);
    // super.Init(CONFIG.apiURL.page.quotes.new);
  }
  createForm() {
    this.userform = this.fb.group({
    });

    this.initFormvalue = {
    }
  }


  ngOnInit() {
  }


  quoestion_Done(event) {
    this.dialogDisplay = false;
    this.item.is_checked = true;
    this.quoestionDone.emit(this.item);
  }



  valueChange(item: any, field: string, index: number = -1) {

    let event;
    if (index < 0) {
      event = {
        name: item.name,
        field: field,
        value: item[field],
        index: index
      };
    } else {
      event = {
        name: item.name,
        field: field,
        value: item[field][index],
        index: index
      };
    }
    CONFIG.LOG(event, 'event in valuechange strategy item');
    CONFIG.LOG(item.completed_type, 'item.compelted_type in valuechange strategy item');
    if (field === 'button_label') {
      switch (item.completed_type) {
        case 'button':
          this.postValueChange(event, field, item);
          break;
        case 'dialog':
          this.question.loadQuestions(item.is_checked && !item.button_enabled);
          this.dialogDisplay = true;
          break;
        case 'review':
          this.dialogDisplay = true;
          break;
        case 'check_value':
          this.postValueChange(event, field, item);
          break;
        case 'print':
          this.postValueChange(event, field, item);
          break;
        case 'two_buttons':
          this.postValueChange(event, field, item);
          break;
        default:
          break;
      }
    } else {
      this.postValueChange(event, field, item);
    }

  }

  buttonDisabled(item): boolean {
    return !(item.completed_type !== 'dialog' && item.completed_type !== 'review' && (item.button_enabled && !item.is_checked))
      && !((item.completed_type === 'dialog' || item.completed_type === 'review') && (item.button_enabled || item.is_checked))
      && !(item.completed_type === 'print');
  }

  postValueChange(event, field, item) {
    this.cs.postDataExtra(CONFIG.apiURL.page.quotes.strategyUpdateItem + this.quote_id.toString(), event)
      .subscribe(
      (res: DataExtra) => {
        this.PushShortResponseMessage(res.data);
        if (field === 'assign_to') {
          this.assigntoChanged.emit(item);
        }
        if (field === 'button_label') {
          if (item.completed_type === 'print') {
            const rev = res.extra;
            this.printQuote(this.quote_id.toString(), rev);
          } else if (item.completed_type === 'two_buttons') {
            this.openWO();
            this.onbuttonClick.emit(item);
          } else {
            this.item.is_checked = true;
            this.onbuttonClick.emit(item);
          }
        }
        if (field === 'check_value') {
          const extra = String(res.extra).split('|');
          if (extra[0] === 'A') {
            this.onbuttonClick.emit(item);
          } else {
            item.check_text[event.index] = extra[1];
          }
        }
      },
      (err:any)=>{
          this.PushErrorMessage(err); 
        }
      );
  }

  protected openWO() {
    const url = CONFIG.Nesi1URL.quoteWorkorder.replace('@quoteid', this.quote_id).replace('@buid', this.buid);
    this.winRef.boingNesi1(url, 'wo_quote_' + this.quote_id);
  }

  protected printQuote(id: string, rev: string) {
    const url = CONFIG.Nesi1URL.printQuote.replace('@quoteid', id + rev);
    this.winRef.boingNesi1(url, 'print_quote_' + id);
  }


  killQuote() {
    this.cs.postString(CONFIG.apiURL.page.quotes.strategyKillQuote + this.quote_id, { data: this.reason })
      .subscribe(
      (res) => {
        this.PushShortResponseMessage(res);
        setTimeout(() => window.close(), 1000);
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }
}
