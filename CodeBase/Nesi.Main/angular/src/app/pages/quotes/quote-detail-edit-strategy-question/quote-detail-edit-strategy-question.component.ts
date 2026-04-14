import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from 'app/core/formMessageBaseComponent';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../reducers';
import { CONFIG } from 'app/configuration';
import { DataExtra } from 'app/models/core/dataExtra';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-detail-edit-strategy-question',
  templateUrl: './quote-detail-edit-strategy-question.component.html',
  styleUrls: ['./quote-detail-edit-strategy-question.component.css']
})
export class QuoteDetailEditStrategyQuestionComponent extends FormMessageBase implements OnInit {

  @Input()
  type: string;
  @Input()
  quote_id: string;
  @Output()
  clear = new EventEmitter();

  loaded = false;
  score: number;
  is_readonly = false;

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.quotes.processQuestionHistory);

  }

  get rows(): FormArray {
    return <FormArray>this.userform.get('rows');
  }


  createRow(): FormGroup {
    return this.fb.group({
      'id': '',
      'question': '',
      'is_checked': '',
      'notes': ''
    });
  }

  createForm() {
    this.userform = this.fb.group(
      {
        rows: this.fb.array([])
      });
  }

  ngOnInit() {
  }

  submitBefore() {
    this.postUrl = this.Url;
    this.submitedValue = this.userform.get('rows').value;
  }

  clearClick() {
    this.cs.deleteString(this.Url)
      .subscribe(
      res => {
        this.PushShortResponseMessage(res);
        this.clear.emit({});
      },
      (err:any)=>{
        this.PushErrorMessage(err);
        this.clear.emit({});
      }
      )
  }

  get Url() {
    return CONFIG.apiURL.page.quotes.processQuestionHistory + this.quote_id + '/' + this.type;
  }

  public loadQuestions(readonly: boolean) {
    this.is_readonly = readonly;
    this.cs.getList<any>(this.Url)
      .subscribe(
      (res) => {
        CONFIG.LOG(res, 'get questions in startegy question');
        //  this.userform.get('rows').patchValue(res);
        const rs = res.map(item => this.fb.group(item));
        const rowsFormArray = this.fb.array(rs);
        this.userform.setControl('rows', rowsFormArray);
        if (this.is_readonly) {
          this.rows.controls.forEach(x => {
            x.get('is_checked').disable();
          });
        }
        this.calculateScore();
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }

      );
  }

  calculateScore() {

    let sum = 0;
    let yes = 0;
    for (let i = 0; i < this.rows.length; i++) {
      const is_chk = this.rows.controls[i].get('is_checked').value;
      const ifyes = this.rows.controls[i].get('ifyes').value;
      const ifno = this.rows.controls[i].get('ifno').value;
      CONFIG.LOG(ifyes, 'if yes value in calculate score');
      CONFIG.LOG(ifno, 'if no value in calculate score');
      if (is_chk) {
        yes += ifyes || 0;
      } else {
        yes += ifno || 0;
      }
      sum += ifyes || 0;
    }

    this.score = Math.round(yes * 1000 / sum) / 10;
  }

  // getScore(id, checked) {
  //   const obj = {
  //     name: this.type,
  //     id: id,
  //     value: checked,
  //     field: 'is_checked'
  //   }
  //   this.updateField(obj);
  // }

  // updateNotes(id, note) {
  //   const obj = {
  //     name: this.type,
  //     id: id,
  //     value: note,
  //     field: 'notes'
  //   }
  //   this.updateField(obj);
  // }


  // updateField(obj) {
  //   this.cs.postDataExtra(CONFIG.apiURL.page.quotes.processQuestionHistoryUpdate + this.quote_id, obj)
  //     .subscribe(
  //     (res: DataExtra) => {
  //       if (obj.field !== '') {
  //         this.PushShortResponseMessage(res.data);
  //       }
  //       this.score = res.extra;
  //     }
  //     );
  // }
}
