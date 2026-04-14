import { Component, OnInit, EventEmitter, Output, Input, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';
import {
  OverlayPanel
} from 'primeng/primeng';
import { DatePipe } from '@angular/common'

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'master-outstanding-invoice-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})
export class MasterOutstandingInvoiceEditorComponent extends FormMessageBase implements OnInit {
  public _masterOutstandingInvoice: any;
  public noteDisplay = false;
  public _existingNote: any = {};
  public _editMode = 'Add';
  list = [];

  // tslint:disable-next-line:no-input-rename
  @Input('editLink') editLink = '';
  // tslint:disable-next-line:no-input-rename
  @Input('editLink') editNoteLink = '';
  // tslint:disable-next-line:no-input-rename
  @Input('addNoteLink') addNoteLink = '';

  // tslint:disable-next-line:no-input-rename
  @Input('customerStatusList') customerStatusList = [];
  // tslint:disable-next-line:no-input-rename
  @Input('invoiceStatusList') invoiceStatusList = [];

  @Input()
  set masterOutstandingInvoice(value: any) {
    if (!value) {
      return;
    }

    this._masterOutstandingInvoice = value;

    this.userform.reset({
      customer_collection_status: this._masterOutstandingInvoice.customer_collection_status,
      exp_pay: this.checkDate(this._masterOutstandingInvoice.exp_pay), // exp_pay=10/8/2017 12:00:00 AM
      woprog_ExpectedCheckRun: this.checkDate(this._masterOutstandingInvoice.woprog_ExpectedCheckRun), // woprog_ExpectedCheckRun=
      // tslint:disable-next-line:max-line-length
      woprog_invoice_collection_status: this._masterOutstandingInvoice.woprog_invoice_collection_status, // woprog_invoice_collection_status=0
      wOProg_CustPO: this._masterOutstandingInvoice.wOProg_CustPO, // wOProg_CustPO=Test PO#
      mat_cost: this._masterOutstandingInvoice.mat_cost, // mat_cost=50
      lab_cost: this._masterOutstandingInvoice.lab_cost, // lab_cost=0
      terms: this._masterOutstandingInvoice.terms, // terms=30
      cust_terms: this._masterOutstandingInvoice.cust_terms, // cust_terms=50
      woprog_id: this._masterOutstandingInvoice.woprog_id, // woprog_id=65920,
      wOProg_InvoiceNo: this._masterOutstandingInvoice.wOProg_InvoiceNo, //wOProg_InvoiceNo=0000101811
      wOProg_CustomerName: this._masterOutstandingInvoice.wOProg_CustomerName, //wOProg_CustomerName=Test Customer
      wOProg_InvoicedNetTotal: this._masterOutstandingInvoice.wOProg_InvoicedNetTotal, //wOProg_InvoicedNetTotal=1001
      balance: this._masterOutstandingInvoice.balance, // balance=0
      note_date: this.formatDate(this.checkDate(this._masterOutstandingInvoice.note_date)), // note_date=5/8/2018 2:09:13 PM
      phone: this._masterOutstandingInvoice.phone // phone=231 250 6745
    });

    // Get notes
    this.submitting = true;
    this.cs.getObject(CONFIG.apiURL.page.reports.masterOutstandingInvoicesGrid.invoiceNotes + this._masterOutstandingInvoice.woprog_id.toString())
      .subscribe(
        (res: any) => {
          if (res) {
            this.list = res;
            this.submitting = false;
          }
        },
        (error: any) => {
          this.submitting = false;
        }
      );

  }

  @Output() onExit = new EventEmitter();

  constructor(private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public datepipe: DatePipe) {
    super(store, cs);
    this.createForm();
  }

  ngOnInit() {
    this.postUrl = CONFIG.apiURL.page.reports.masterOutstandingInvoicesGrid.edit;

    this.onChange.subscribe(
      (v) => {
        this._masterOutstandingInvoice.woprog_ExpectedCheckRun = this.checkDate(v.result.woprog_ExpectedCheckRun);
        this._masterOutstandingInvoice.woprog_invoice_collection_status = v.result.woprog_invoice_collection_status;
        this._masterOutstandingInvoice.customer_collection_status = v.result.customer_collection_status;
        this._masterOutstandingInvoice.wOProg_CustPO = v.result.wOProg_CustPO;

        // Update notes
        this.submitting = true;
        this.cs.getObject(CONFIG.apiURL.page.reports.masterOutstandingInvoicesGrid.invoiceNotes + this._masterOutstandingInvoice.woprog_id.toString())
          .subscribe(
            (res: any) => {
              if (res) {
                this.list = res;
                this.submitting = false;
              }
            },
            (error: any) => {
              this.submitting = false;
            }
          );

      }
    );
  }

  createForm() {
    this.userform = this.fb.group({
      customer_collection_status: '',
      exp_pay: '',
      woprog_ExpectedCheckRun: '',
      woprog_invoice_collection_status: '',
      wOProg_CustPO: '',
      mat_cost: '',
      lab_cost: '',
      terms: '',
      cust_terms: '',
      woprog_id: '',
      wOProg_InvoiceNo: '',
      wOProg_CustomerName: '',
      wOProg_InvoicedNetTotal: '',
      balance: '',
      note_date: '',
      phone: ''
    });
  }

  exit() {
    this.onExit.emit();
  }

  addNewNote() {
    this._editMode = "Add";
    this._existingNote = { note: '', noteId: 0 };
    this.noteDisplay = true;
  }

  editNote(data) {
    this._editMode = "Edit";
    this._existingNote = data;
    this.noteDisplay = true;
  }


  exitFromNoteEditor(refresh: boolean) {
    this.noteDisplay = false;

    if (refresh) {
      this.submitting = true;
      this.cs.getObject(CONFIG.apiURL.page.reports.masterOutstandingInvoicesGrid.invoiceNotes + this._masterOutstandingInvoice.woprog_id.toString())
        .subscribe(
          (res: any) => {
            if (res) {
              this.list = res;
              this.submitting = false;
            }
          },
          (error: any) => {
            this.submitting = false;
          }
        );
    }
  }


  checkDate(d: any) {
    if (d === null || d === '' || d.toString() === '1901-01-01' || d.toString() === '1901-01-01 00:00:00'
      || d.toString() === '0001-01-01' || d.toString() === '0001-01-01 00:00:00') {
      return null;
    } else {
      try {
        // Supposed the format is "yyyy-mm-dd"
        // check this https://stackoverflow.com/questions/7556591/javascript-date-object-always-one-day-off
        const d2 = d.replace(/-/g, '\/');
        const d1 = new Date(d2);
        return d1;
      } catch {
        // if not follow "yyyy-mm-dd"
        return new Date(d);
      }
    }
  }

  formatDate(d: Date) {
    if (d === null) {
      return d;
    }

    return this.datepipe.transform(d, 'yyyy-MM-dd HH:mm:ss');
  }
}
