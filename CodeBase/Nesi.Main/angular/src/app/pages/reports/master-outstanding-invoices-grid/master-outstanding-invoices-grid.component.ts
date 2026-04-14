import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';

@Component({
  selector: 'nesi-master-outstanding-invoices-grid',
  templateUrl: './master-outstanding-invoices-grid.component.html',
  styleUrls: ['./master-outstanding-invoices-grid.component.css']
})
export class MasterOutstandingInvoicesGridComponent extends FormMessageBase implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;
  public userform: FormGroup;
  public displayEdit = false;
  public entity: any;

  public editLink = CONFIG.apiURL.page.reports.masterOutstandingInvoicesGrid.edit;
  public editNoteLink = '';
  public addNoteLink = '';
  
  public customerStatusList = [];
  public invoiceStatusList = [];

  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService) {
    super(store, cs);
    this.createForm();
    this.postUrl = CONFIG.apiURL.page.reports.masterOutstandingInvoicesGrid.confirmPaidDate;
  }

  ngOnInit() {
    this.onChange.subscribe(
      () => { this.loadDetail(); this.dt.RemoveSelectedItems(); }
    );

    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.reports.masterOutstandingInvoicesGrid.customerStatus)
    .subscribe(
      (res) => {
        this.customerStatusList = res;
      }
    );

    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.reports.masterOutstandingInvoicesGrid.invoiceStatus)
    .subscribe(
      (res) => {
        this.invoiceStatusList = res;
      }
    );
  }

  createForm() {
    this.userform = this.fb.group({
      confirmedPaidDate: new Date(),
      woprogIDs: ''
    });
  }

  loadDetail() {
    this.dt.after_onRefresh();
  }

  confirmPaidDate() {
    const confirmedDate = this.userform.get('confirmedPaidDate').value;
    const list = this.dt.getSelectedItems();
    console.dir(list);
    console.log(confirmedDate);

    if (list != null && list.length > 0) {
      const items = [];
      for (const item of list) {
        items.push((<any>item).woprog_id);
      }

      this.userform.get('woprogIDs').setValue(items);
      this.onSubmit();
    }
  }

  masterOutstandingInvoicesGridColumnCallback(rowData: any, rowIndex: number, columnName) {
    if (columnName === 'wOProg_InvoicedNetTotal') {
      return 'boldRight';
    }

    return;
  }

  masterOutstandingInvoicesGridCallback(rowData: any, rowIndex: number) {
    if (rowData['woprog_invoice_collection_status'] === '2') {
      return 'colorFFCCCC';
    }

    if (rowData['woprog_invoice_collection_status'] === '5') {
      return 'color99CCFF';
    }

    if (rowData['woprog_invoice_collection_status'] === '7') {
      return 'colorFFFF00';
    }

    if (rowData['woprog_invoice_collection_status'] === '3') {
      return 'color99FF99';
    }
    return ' ';
  }

  // Editing part
  // Open editor
  openEdit(event) {
    this.entity = event;
    this.displayEdit = true;
  }

  // Close editor
  exitFromEditor() {
    this.entity = null;
    this.displayEdit = false;
  }


}
