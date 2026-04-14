import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-days-off-vacation-request',
  templateUrl: './employee-days-off-vacation-request.component.html',
  styleUrls: ['./employee-days-off-vacation-request.component.css']
})
export class EmployeeDaysOffVacationRequestComponent implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;
  memberid: number;
  constructor() { }

  ngOnInit() {
  }


  public loadGrid(mid: number = null, refresh_button = false) {
    if (mid) {
      this.memberid = mid;
    }
    this.dt.reportQueryParam = [{ coulumnname: 'member_id', value: this.memberid }];
    this.dt.refreshCache = true;
    if (refresh_button) {
      this.dt.loadReport();
    } else {
      this.dt.showReport('EmployeeDaysOffVacationRequestGrid');
    }
  }
}
