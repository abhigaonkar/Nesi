import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-employee-days-off-list',
  templateUrl: './employee-days-off-list.component.html',
  styleUrls: ['./employee-days-off-list.component.css']
})
export class EmployeeDaysOffListComponent implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  _memberId: number;
  constructor() { }

  ngOnInit() {
  }


  public loadGrid(memberid: number = null, refresh_button = false) {
    if (memberid) {
      this._memberId = memberid;
    }
    this.dt.reportQueryParam = [{ coulumnname: 'member_id', value: this._memberId }];
    this.dt.refreshCache = true;
    if (refresh_button) {
      this.dt.loadReport();
    } else {
      this.dt.showReport('EmployeeDaysOffGrid');
    }
  }

}
