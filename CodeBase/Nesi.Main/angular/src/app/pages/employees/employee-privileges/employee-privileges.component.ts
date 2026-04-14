import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { MessageBase } from '../../../core/messageBaseComponent';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';

@Component({
  selector: 'nesi-employee-privileges',
  templateUrl: './employee-privileges.component.html',
  styleUrls: ['./employee-privileges.component.css']
})
export class EmployeePrivilegesComponent extends MessageBase implements OnInit {

 
  @Output() loading = new EventEmitter();
  @Output() loaded = new EventEmitter();

  memberid: number;
  _employee: any;

  @Input() set employee(value: any) {
    if (value) {
      this._employee = value;
      this.memberid = this.employee.memberid || this.employee.member_id;
    }
  }

  get employee(): any {
    return this._employee || { isnull: true };
  }


  constructor(
    private cs: CoreService,
    protected store: Store<fromRoot.State>,
  ) {
    super(store);

  }
  ngOnInit() {
  }

}
