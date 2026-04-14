import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';
import { ConfirmationService } from 'primeng/primeng';
import { DataExtra } from 'app/models/core/dataExtra';


export class QuoteEditRowBase extends QuoteEditFormBase implements OnInit {

  @Output() openSection = new EventEmitter();
  draggedRow: any;
  dialogDisplay = false;
  draggable = true;
  draggingIndex: number;
  timeout: any;

  get rows(): FormArray {
    return <FormArray>this.userform.get('rows');
  }

  rowsLoaded = false;
  autoResize = false;
  public type = 1;

  constructor(
    protected fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    protected route: ActivatedRoute,
    protected cf: ConfirmationService,
  ) {
    super(winRef, store, cs, ts);
    super.Init(CONFIG.apiURL.page.quotes.editUpdate);
  }

  createForm() {
    this.userform = this.fb.group({
      'rows': this.fb.array([]),
    });

    this.initFormvalue = {
    }
    if (this.edit_disabled) {
      this.userform.disable();
    }
  }

  getTextLines(id: string, text: string) {
    // el.styles.remove('height');
    const el = document.getElementById(id);
    if (el) {
      if (this.autoResize) {
        el.style.removeProperty('height');
        el.style.resize = 'none';
      } else {
        el.style.resize = 'vertical';
      }
    }
    const len = text.split('\n').length;
    return this.autoResize ? len + 2 : 2;
  }

  row_highlight(row, index) {
    return this.draggedRow == row || this.draggingIndex === index;
  }
  initQ() {
    let div: any;
    if (this.type === 1) {
      div = this.q && this.q.divs1
    } else {
      div = this.q && this.q.divs2
    }

    if (div && this.rows && !this.rowsLoaded) {
      super.LOG(div, 'divs in quoteeditrowbase');
      this.rowsLoaded = true;
      const rowsFGs = div.map(item => this.fb.group(item));
      const rowsFormArray = this.fb.array(rowsFGs);
      this.userform.setControl('rows', rowsFormArray);
      // this.loadFormArrayChangeEvent('rows', this.type);
    }
  }

  textDragStart(event) {
    if (this.edit_disabled) {
      return;
    }
    CONFIG.LOG(this.draggable, 'text drag start');
    this.draggable = false;
    event.preventDefault();
    event.returnValue = false;
  }

  textFocus() {
    setTimeout(() => {
      this.draggable = false;
      CONFIG.LOG(this.draggable, 'draggable value on focus');
    }, 200);
  }

  textBlur() {
    this.draggable = true;
    CONFIG.LOG(this.draggable, 'draggable value on blur');
  }

  afterRowEdit(row_id, value) {
    let o;
    if (this.type === 1) {
      o = (this._q.divs1 as Array<any>).find(x => x.row_id === row_id);
    } else {
      o = (this._q.divs2 as Array<any>).find(x => x.row_id === row_id);
    }
    o.line_text = value;
    this.rows.controls.find(x => x.get('row_id').value === row_id).get('has_name').setValue(!!value);
  }

  openWorksheetSection(value: number) {
    CONFIG.LOG(value, 'open worksheet section id in quote-edit-scorework');
    this.openSection.emit(value);
  }

  deleteRow(row: any) {
    this.cf.confirm({
      message: `Please confirm that you want to delete this ${this.type === 1 ? 'detail' : 'note'} ?`,
      accept: () => {
        super.LOG(row, 'row data in delete row quote-edit row base');
        if (row['section_id']) {
          setTimeout(() => { this.confirmDel_assoc(row) }, 100);
        } else {
          this.postDelete(row);
        }
      },
      reject: () => {

      }
    });

  }

  confirmDel_assoc(row) {
    this.cf.confirm({
      message: 'Would you also like to delete the worksheet items & the associated section also?',
      accept: () => {
        this.postDelete(row, 1);
      },
      reject: () => {
        this.postDelete(row, 0);
      }
    });
  }

  postDelete(row, del_assoc = 0) {
    this.submitting = true;
    this.cs.postDataExtra(CONFIG.apiURL.page.quotes.deleteDetail
      + this._q.quote_id.toString() + '/' + this._q.revision.toString() + '/' + this.type.toString(),
      { id: row.row_id, value: del_assoc })
      .subscribe(
        (res) => {

          if (res.extra) {
            this.reOrder.emit(res.extra.sections);
          }
          this.submitting = false;

          // // this.PushResponseMessage(res);
          // const array = this.userform.get('rows') as FormArray;
          // let index = -1;
          // for (let i = 0; i < array.length; i++) {
          //   const group = array.controls[i] as FormGroup;
          //   if (group.get('row_id').value === row.row_id) {
          //     index = i;
          //   }
          // }
          // if (index > -1) {
          //   array.removeAt(index);
          //   if (this.type === 1) {
          //     this._q.divs1 = this._q.divs1.filter(x => x.row_id !== row.row_id);
          //   } else {
          //     this._q.divs2 = this._q.divs2.filter(x => x.row_id !== row.row_id);
          //   }
          //   this.rowUpdated.emit(null);
          // }

        },
        (err:any)=>{
          this.PushErrorMessage(err);
          this.submitting = false;
        }
      );
  }

  openRow(row: any) {
    super.LOG(row, 'row data in open row quote-edit-scorework');
    if (!row.line_text) {
      super.PushWarnMessage('Text needs to be added to this detail before it\'s useable as a section in the worksheet.');
      return;
    }
    // tslint:disable-next-line:max-line-length
    this.winRef.boingNesi1(`/sections/member/picklist/pikclist.aspx?id=${this._q.quote_id}&rev=${this._q.revision}&origin=quote&section_id=${row.section_id}`
      , 'picklist_' + this._q.quote_id);
  }

  ngOnInit() {

  }

  buildRow(item): FormGroup {
    return this.fb.group(
      {
        'count': item.count || '',
        'line_text': item.line_text || '',
        'section_id': item.section_id || '',
        'is_referenced': item.is_referenced || '',
        'current_section_total': item.current_section_total || 0,
        'row_id': item.row_id || '',
      }
    );
  }

  addRow() {
    this.submitting = true;
    this.cs.getObject<any>(CONFIG.apiURL.page.quotes.editNewId + this._q.quote_id + '/' + this._q.revision + '/' + this.type)
      .subscribe(
        (res: any) => {
          super.LOG(res, 'addrow data in quote editrow base');
          if (this.type === 1) {
            this._q.divs1 = res;
          } else {
            this._q.divs2 = res;
          }
          this.rowsLoaded = false;
          this.initQ();
          this.rowUpdated.emit(null);
          this.submitting = false;
        },
        (err:any)=>{
          this.PushErrorMessage(err);
          this.rowsLoaded = false;
          this.submitting = false;
        }
      );
  }

  resizeRows(event) {
    this.autoResize = event.checked;
  }

  dragStart(event: any, row: any) {
    if (this.edit_disabled) {
      return;
    }
    CONFIG.LOG(event, 'drap start in quote scorework');
    this.draggingIndex = -1;
    this.draggedRow = row;
  }

  dragEnd(event: any) {
    this.draggedRow = null;
  }

  drop(event: any, pos: number) {
    CONFIG.LOG(pos, 'drop position in quote scorework');
    if (this.edit_disabled) {
      return;
    }
    if (this.draggedRow) {
      const draggedIndex = this.findIndex(this.draggedRow);
      this.draggedRow = null;
      this.re_position(draggedIndex, pos);
    }
  }

  re_position(from, to) {
    if (from === to) {
      return;
    }
    this.draggingIndex = from;
    const newDiv = [];
    const oldDiv = this.type === 1 ? this.q.divs1 : this.q.divs2;
    if (oldDiv.length > 1) {
      this.submitting = true;
      for (let i = 0; i < oldDiv.length; i++) {
        if (to === i) {
          newDiv.push(oldDiv[from]);
          newDiv.push(oldDiv[i]);
        } else if (from === i) {

        } else {
          newDiv.push(oldDiv[i]);
        }
      }

      if (to === oldDiv.length) {
        newDiv.push(oldDiv[from]);
      }
      if (this.type === 1) {
        this.q.divs1 = newDiv;
      } else {
        this.q.divs2 = newDiv;
      }
      this.draggingIndex = from > to ? to : to - 1;
      if (this.timeout) {
        window.clearTimeout(this.timeout);
      }
      this.timeout = window.setTimeout(() => {
        this.timeout = null;
        this.draggingIndex = -1
      }, 3000);
      this.rowsLoaded = false;
      this.initQ();
      this.postReOrder();
    }
  }

  findIndex(row: any) {
    let index = -1;
    for (let i = 0; i < this.rows.length; i++) {
      if (this.rows.controls[i].get('row_id').value === row.get('row_id').value) {
        index = i;
        break;
      }
    }
    return index;
  }

  postReOrder() {
    this.submitting = true;
    const orders = [];
    let index = 0;
    this.rows.controls.forEach(x => {
      orders.push({
        id: x.get('row_id').value,
        order: index,
        is_checked: x.get('is_checked').value,
      });
      index++;
    });
    this.cs.postObject<DataExtra>(CONFIG.apiURL.page.quotes.reOrder
      + this._q.quote_id + '/' + this._q.revision + '/' + this.type.toString(), orders)
      .subscribe(
        (res: DataExtra) => {
          if (this.CheckResponseMessage(res.data)) {
            // if (this.type === 1) {
            //   this._q.divs1 = res.extra.div;
            // } else {
            //   this._q.divs2 = res.extra.div;
            // }
            this.reOrder.emit(res.extra.sections);
          }
          this.submitting = false;
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
      );
  }

  addedFromQuote(event: any) {
    if (event.result) {
      this.reOrder.emit(event.result.sections);
    }
  }


  check_changed(row, event) {

  }
}
