import { Component, OnInit, Input } from '@angular/core';
import { WorkOrderService } from '../../../../services/pages/workorder.service';

@Component({
  selector: 'nesi-workorder-bucket-open-item',
  templateUrl: './workorder-bucket-open-item.component.html',
  styleUrls: ['./workorder-bucket-open-item.component.css']
})
export class WorkorderBucketOpenItemComponent implements OnInit {

  @Input() item: any;
  @Input() business_unit_id: number;

  public open_invocie_preview = false;
  constructor(
    private wos: WorkOrderService,

  ) { }

  ngOnInit() {
  }

  open_wo(event) {
    this.wos.openUrl(this.open_invocie_preview ? 'preview' : 'id', this.item.business_unit_id, 'WorkOrder', this.item.woprog_id, event);
  }

  get backgroundClass() {
    if (this.item.woprog_hold) {
      return 'workorder_bucket_item_onhold';
    }
    if (this.item.woprog_hold) {
      return 'workorder_bucket_item_onhold';
    }
    if (this.item.parent_woprog_id > 1) {
      this.open_invocie_preview = this.business_unit_id !== this.item.business_unit_id;
      return this.item.business_unit_id === this.business_unit_id ? 'workorder_bucket_item_isachild' : 'workorder_bucket_item_otherbranch';
    }
    if (this.item.woprog_associate_woprog_id) {
      return 'workorder_bucket_item_associate';
    }
    if (this.item.woprog_quoteid > 0) {
      return 'workorder_bucket_item_quote';
    }
  }


  get toolTip() {
    return `${this.item.woprog_bvwo} - ${this.item.woprog_customername}<hr><table cellspacing='0' cellpadding='1' width='100%'>
	<tr>
		<td width='150'><b>Status</b></td>
		<td>${this.item.woprog_status}</td>
	</tr>
	<tr>
		<td width='150'><b>Business Unit</b></td>
		<td>${this.item.ddl_name}</td>
	</tr>
	<tr>
		<td width='150'><b>Customer Name</b></td>
		<td>${this.item.woprog_customername}</td>
	</tr>
	<tr>
		<td><b>Days Since Scanned</b></td>
		<td>${this.item.days_since}</td>
	</tr>
  <tr>
	<td><b>Days in this bucket</b></td>
	<td>${this.item.last_updated_dt}</td>
</tr>
<tr>
	<td><b>Last Status Changed by</b></td>
	<td>${this.item.last_updated_by}</td>
</tr>
	<tr>
		<td><b>Still to be Billed</b></td>
		<td>$${this.item.woprog_stilltobebilled}</td>
	</tr>
	<tr>
		<td><b>PO:</b></td>
		<td>${this.item.woprog_custpo}</td>
	</tr>
	<tr>
		<td><b>Project Manager</b></td>
		<td>${this.item.pm_name}</td>
	</tr>
	<tr>
		<td><b></b></td>
		<td></td>
	</tr>
   <tr>
		<td><b>On Hold</b></td>
		<td>${this.item.woprog_hold}</td>
  </tr>${this.polist_string}</table>
<b>Work Order Description</b>
<div style='background-color:black'>${this.item.woprog_description}</div>`;
  }


  get polist_string() {
    if (!this.item.po_list || this.item.po_list.length === 0) {
      return '';
    } else {

      let str = '';
      this.item.po_list.forEach(x => {
        str += `<div><a style='color:#fff;' href='javascript:load_po(${x.id});'>${x.bvpo} - ${x.vendor_name}</a></div>`;
      });
      return `
      <tr>
		<td colspan='2' valign='top'><br/><b>Open Purchase Orders</b></td>
	</tr>
	<tr>
		<td colspan='2' style='background-color:#047;padding:5px;'>${str}</td></tr>`;
    }
  }
}
