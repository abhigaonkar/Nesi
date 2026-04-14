import { Component, OnInit, Input } from '@angular/core';
import { WorkOrderService } from '../../../../services/pages/workorder.service';

@Component({
  selector: 'nesi-workorder-bucket-scanned-item',
  templateUrl: './workorder-bucket-scanned-item.component.html',
  styleUrls: ['./workorder-bucket-scanned-item.component.css']
})
export class WorkorderBucketScannedItemComponent implements OnInit {
  @Input() item: any;
  @Input() business_unit_id: number;

  constructor(
    private wos: WorkOrderService,
  ) { }

  ngOnInit() {
  }

  get toolTip() {
    return `File Information<hr><table>
    <tr><td>File Name:</td><td>${this.item.file_name}</td></tr>
    <tr><td>Size:</td><td>${this.item.size} kb</td></tr>
    <tr><td>Created:</td><td>${this.item.created_time}</td></tr>
    <tr><td>Last Accessed:</td><td>${this.item.last_access_time}</td></tr>
    </table>`;
  }

  open_file(event) {
    this.wos.open_wo_file(this.item.file_name, this.business_unit_id.toString(), event);
  }
}
