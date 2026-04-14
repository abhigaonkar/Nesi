import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { ConfirmationService } from 'primeng/primeng';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from '../../../core/messageBaseComponent';
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-disciplinary',
  templateUrl: './employee-disciplinary.component.html',
  styleUrls: ['./employee-disciplinary.component.css']
})
export class EmployeeDisciplinaryComponent extends MessageBase implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;

  memberid: number;
  _employee: any;
  selected_id = -1;

  @Input() set employee(value: any) {
    if (value) {
      this._employee = value;
      this.memberid = this.employee.memberid || this.employee.member_id;
      this.loadGrid(this.memberid);
    }
  }

  get employee(): any {
    return this._employee || {};
  }


  constructor(
    private cs: CoreService,
    private cf: ConfirmationService,
    protected store: Store<fromRoot.State>,
  ) {
    super(store);
  }

  ngOnInit() {
  }


  public loadGrid(mid: number = null, refresh_button = false) {
    this.dt.reportQueryParam = [{ coulumnname: 'member_id', value: mid || this.memberid }];
    this.dt.refreshCache = true;

    if (refresh_button) {
      this.dt.loadReport();
    } else {
      this.dt.showReport('EmployeeDiscplinaryGrid');
    }
  }

  edit(entity) {
    this.selected_id = entity.membernote_id;
  }

  delete(entity) {
    this.cf.confirm({
      message: 'Are you sure that you want to delete this record?',
      accept: () => {
        this.cs.postDataExtra(CONFIG.apiURL.page.employee.disciplinary.delete + '/' + entity.membernote_id, entity)
          .subscribe(
            (res) => {
              if (this.PushResponseMessage(res.data)) {
                this.loadGrid();
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
