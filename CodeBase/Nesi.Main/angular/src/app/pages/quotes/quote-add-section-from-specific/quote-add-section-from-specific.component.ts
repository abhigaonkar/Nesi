import { Component, OnInit, Input } from '@angular/core';
import { FormMessageBase } from 'app/core/formMessageBaseComponent';
import * as fromRoot from '../../../reducers';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';
import { Store } from '@ngrx/store';
import { CONFIG } from 'app/configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { Observable } from 'rxjs/Observable';
import { ConfirmationService } from 'primeng/primeng';

@Component({
  selector: 'nesi-quote-add-section-from-specific',
  templateUrl: './quote-add-section-from-specific.component.html',
  styleUrls: ['./quote-add-section-from-specific.component.css']
})
export class QuoteAddSectionFromSpecificComponent extends FormMessageBase implements OnInit {
  @Input() quote_id: number;
  @Input() revision: number;
  @Input() type: number;
  @Input() can_edit = true;

  buId: number;
  dialogDisplay = false;
  label: string;
  checkedAll = false;
  getListUrl: string;
  has_row = false;
  edit_mode = false;
  items: any[];

  constructor(private fb: FormBuilder,
    private ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    private cf: ConfirmationService,
  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.quotes.saveNewIds + '$quote_id/$revision/$type');
    this.getListUrl = CONFIG.apiURL.page.shared.pickList.specificSections + '$buId/$type';
  }

  get rows(): FormArray {
    return <FormArray>this.userform.get('rows');
  }

  createForm() {
    this.userform = this.fb.group({
      'rows': this.fb.array([]),
    });
  }

  ngOnInit() {
    this.buId = this.ts.currentUser.businessUnitId;
  }

  addRow() {
    this.items.push({ label: '', value: 0, is_checked: false });
    this.setRows(this.items);
  }

  deleteRow(row, index) {
    if (row.get('value').value === 0) {
      this.items = this.items.filter((x, i) => index !== i);
      this.setRows(this.items);
    } else {
      this.cf.confirm({
        message: 'Would you like to delete this specific section from this business unit?',
        accept: () => {
          this.cs.deleteString(CONFIG.apiURL.page.shared.pickList.specificSections + row.get('value').value)
            .subscribe(
            res => {
              if (this.PushResponseMessage(res)) {
                this.getList();
              }
            },
            (err:any)=>{
              this.PushErrorMessage(err);
            }
            );
        }
      });
    }
  }

  saveRow(row, index) {
    if (!row.get('label').value) {
      this.PushWarnMessage('Please input some text.');
      return;
    }
    if (row.get('value').value === 0) {
      this.cs.postString(CONFIG.apiURL.page.shared.pickList.specificSections + this.buId + '/' + this.type.toString(), { data: row.get('label').value })
        .subscribe(
        res => {
          if (this.PushResponseMessage(res)) {
            this.getList();
          }
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
        );
    } else {
      this.cs.patchString(CONFIG.apiURL.page.shared.pickList.specificSections + row.get('value').value, { data: row.get('label').value })
        .subscribe(
        res => {
          if (this.PushResponseMessage(res)) {
            this.getList();
          }
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
        );
    }
  }

  selectBusinessUnitChanged(event) {
    this.ClearMessage();
    if (event.selectBusinessUnit) {
      this.label = event.selectBusinessUnit.name;
    }
    this.dialogDisplay = true;
    this.getList();
  }

  getList() {
    const url = this.replaceURL(this.getListUrl);
    this.submitting = true;
    this.cs.getObject<any>(url)
      .subscribe(
      (res: any) => {
        this.submitting = false;
        this.can_edit = res.can_edit;
        this.items = res.list;
        this.setRows(this.items);
      },
      (err:any)=>{
        this.PushErrorMessage(err);
        this.submitting = false;
      }
      );
  }


  replaceURL(url: string): string {
    if (this.quote_id) {
      url = url.replace('$quote_id', this.quote_id.toString());
    }
    if (this.revision) {
      url = url.replace('$revision', this.revision.toString());
    }
    if (this.buId) {
      url = url.replace('$buId', this.buId.toString());
    }
    if (this.type) {
      url = url.replace('$type', this.type.toString());
    }
    return url;
  }

  setRows(res: any[]) {
    res.forEach(item => {
      item.is_checked = false;
      item.label = decodeURIComponent(item.label)
    });
    const rs = res.map(item => this.fb.group(item));
    const rowsFormArray = this.fb.array(rs);
    this.userform.setControl('rows', rowsFormArray);

  }


  checkAll(event: any) {
    const chk = event;
    for (let i = 0; i < this.rows.length; i++) {
      this.rows.controls[i].get('is_checked').setValue(chk);
    }
  }

  submitBefore() {
    if (this.userform.get('rows')) {
      this.submitedValue = this.userform.get('rows').value;
      if (this.submitedValue) {
        this.submitedValue = this.submitedValue.filter(x => x.is_checked);
      }
    } else {
      this.submitedValue = this.userform.value;
    }
    this.postUrl = this.replaceURL(this.postUrl);
  }

  submitValidate(): boolean {
    if (this.userform.get('rows')) {
      if (this.submitedValue.length === 0) {
        super.PushWarnMessage('No item has been selected.');
        return false;
      }
    }
    return true;
  }


  submitSuccess() {
    this.dialogDisplay = false;
    this.submitting = false;
  }

}
