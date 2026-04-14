import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from 'app/core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../reducers';
import { CONFIG } from 'app/configuration';

@Component({
  selector: 'nesi-quote-stage1',
  templateUrl: './quote-stage1.component.html',
  styleUrls: ['./quote-stage1.component.css']
})
export class QuoteStage1Component extends FormMessageBase implements OnInit {
  _quote_id: number;
  @Input() set quote_id(value: number) {
    if (value) {
      this._quote_id = value;
      this.init();
    }
  }

  @Output() Killed = new EventEmitter();
  @Output() Approved = new EventEmitter();

  model: any;
  showStage1Kill = false;
  reason: string;
  rt_names: string[] = [];

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,

  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.quotes.strategyStage1 + '$quote_id');

  }

  createForm() {
    this.userform = this.fb.group({
      'hours': ['', [Validators.required, Validators.min(0.01)]],
      'rt1': ['', [Validators.required]],
      'rt2': ['', [Validators.required]],
      'rt3': ['', [Validators.required]],
      'rt4': ['', [Validators.required]],
      'rt5': ['', [Validators.required, Validators.min(1)]],
      'rt6': ['', [Validators.required, Validators.min(1)]],
    });

  }


  ngOnInit() {

  }

  init() {
    this.cs.getObject<any>(CONFIG.apiURL.page.quotes.strategyStage1 +  this._quote_id.toString())
      .subscribe(
      (res: any) => {
        this.model = res;
        this.userform.patchValue(this.model);
        CONFIG.LOG(this.userform.value, 'init userform value in quote stage1');

        let rtname = this.GetLabelByValueFromLabelValueInt(this.model.ddl5, this.model.rt1);
        if (!rtname) { this.userform.get('rt1').setValue(''); }

        rtname = this.GetLabelByValueFromLabelValueInt(this.model.ddl5, this.model.rt2);
        if (!rtname) { this.userform.get('rt2').setValue(''); }

        rtname = this.GetLabelByValueFromLabelValueInt(this.model.ddl5, this.model.rt3);
        if (!rtname) { this.userform.get('rt3').setValue(''); }


        rtname = this.GetLabelByValueFromLabelValueInt(this.model.ddl5, this.model.rt4);
        if (!rtname) { this.userform.get('rt4').setValue(''); }


        if (Number(this.model.quote_level) !== 3) {
          this.userform.get('rt1').setValue('0');
          this.userform.get('rt2').setValue('0');
          this.userform.get('rt3').setValue('0');
          this.userform.get('rt4').setValue('0');
        }

        rtname = this.GetLabelByValueFromLabelValueInt(this.model.ddl5, this.model.rt5);
        if (!rtname) { this.userform.get('rt5').setValue(''); }


        rtname = this.GetLabelByValueFromLabelValueInt(this.model.ddl6, this.model.rt6);
        if (!rtname) { this.userform.get('rt6').setValue(''); }

        // if (rtname && this.rt_names.indexOf(rtname) < 0) { this.rt_names.push(rtname); }
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }

  submitValidate() {
    const checkRt = this.isDuplicateRt() && this.model.quote_level === 3;
    this.postUrl = this.postUrl.replace('$quote_id', this._quote_id.toString());
    if (checkRt) {
      this.PushWarnMessage('You must have 4 different members for the quote review team.');
      return false;
    } else {
      return true;
    }
  }

  isDuplicateRt(): boolean {
    const rts = [];
    const rt1 = this.userform.get('rt1').value;
    const rt2 = this.userform.get('rt2').value;
    const rt3 = this.userform.get('rt3').value;
    const rt4 = this.userform.get('rt4').value;
    rts.push(rt1);
    if (rts.indexOf(rt2) < 0) {
      rts.push(rt2);
    }
    if (rts.indexOf(rt3) < 0) {
      rts.push(rt3);
    }
    if (rts.indexOf(rt4) < 0) {
      rts.push(rt4);
    }
    return rts.length !== 4;
  }

  killQuote() {
    this.cs.postString(CONFIG.apiURL.page.quotes.strategyKillStage1 + this._quote_id.toString()
      , { data: this.reason })
      .subscribe(
      (res) => {
        this.PushResponseMessage(res);
        this.showStage1Kill = false;
        this.Killed.emit();
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );

  }

  submitSuccess() {
    this.Approved.emit();
  }
}
