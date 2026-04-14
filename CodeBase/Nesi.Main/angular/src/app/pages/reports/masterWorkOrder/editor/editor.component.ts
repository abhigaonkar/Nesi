import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'masterWorkOrder-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})
export class EditorComponent extends FormMessageBase implements OnInit {
  public _masterWorkOrder: any;
  @Input()
  set masterWorkOrder(value: any) {
    if (!value) {
      return;
    }

    this._masterWorkOrder = value;
    // console.dir(this._masterWorkOrder);

    this.userform.reset({
      woprog_id: this._masterWorkOrder.woprog_id,
      expected_enddate: this._masterWorkOrder.expected_enddate,
      expected_startdate: this._masterWorkOrder.expected_startdate,
      acting_ram: this._masterWorkOrder.acting_ram,
      description: this._masterWorkOrder.description,
      expected_sales: this._masterWorkOrder.expected_sales,
      notes: this._masterWorkOrder.notes,
      exp_hours: this._masterWorkOrder.exp_hours,
      copy: this._masterWorkOrder,

      rd: this._masterWorkOrder.rd,
      sc: this._masterWorkOrder.sc,
      hold: this._masterWorkOrder.hold
    });
  }
  get masterWorkOrder(): any {
    return this._masterWorkOrder;
  }

  @Output() onExit = new EventEmitter();

  @Input('optionList') optionList = [];
  @Input('editLink') editLink = '';

  @Input('showExpectedStartDate') showExpectedStartDate = false;

  constructor(private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService) {
    super(store, cs);
    this.createForm();
  }

  ngOnInit() {
    this.postUrl = this.editLink;
    // console.dir(this._masterWorkOrder);

    this.onChange.subscribe(
      this.success
    );
  }

  createForm() {
    this.userform = this.fb.group({
      woprog_id: '',
      description: '',
      expected_enddate: '',
      expected_startdate: '',
      acting_ram: '',
      notes: '',
      expected_sales: 0,
      copy: '',
      rd: { value: false, disabled: true },
      hold: { value: false, disabled: true },
      sc: { value: false, disabled: true },
      exp_hours: 0
    });
  }

  exit() {
    this.onExit.emit();
  }

  success(event) {
    // console.dir(event);

    if (event.result.okay) {
      event.post.copy.expected_startdate = event.result.expected_startdate;
      event.post.copy.expected_enddate = event.result.expected_enddate;
      event.post.copy.acting_ram = event.result.acting_ram;
      event.post.copy.description = event.result.description;
      event.post.copy.expected_sales = event.result.expected_sales;
      event.post.copy.notes = event.result.notes;
      event.post.copy.exp_hours = event.result.exp_hours;
    }
  }
}
