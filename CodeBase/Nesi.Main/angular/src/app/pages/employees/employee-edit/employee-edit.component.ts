import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.css']
})
export class EmployeeEditComponent implements OnInit {
  submitting = false;
  _employee: any;

  @Input() set employee(value: any) {
    if (value) {
      this._employee = value;
      this.getbase_profile();
    }
  }

  get employee(): any {
    return this._employee;
  }

  @Output() close = new EventEmitter();

  _base_profile: any;

  set base_profile(value: any) {
    this._base_profile = value;
  }

  get base_profile() {
    return this._base_profile || {};
  }

  get tab_enabled(): boolean[] {
    // debug purpose
    // return [true, true, true, true, true, true, true, true, true, true, true, true];
    return (this._base_profile && this._base_profile.tab_enabled) || [];
  }

  @Input() tabIndex = 0;
  public currentIndex = 0;

  constructor(
    private cs: CoreService,
  ) { }

  getCurrentLoad(index) {
    if (index === 0) {
      return 0;
    }
    let prevDisabled = 0;
    let prevEnabled = 0;
    for (let i = 0; i < this.tab_enabled.length; i++) {
      if (!this.tab_enabled[i]) {
        prevDisabled++;
      } else {
        prevEnabled++;
      }
      if (prevEnabled === index + 1) {
        return index + prevDisabled;
      }
    }
    return index + prevDisabled;
  }

  ngOnInit() {
  }

  getbase_profile(callback: Function = null) {
    this.submitting = true;
    this.cs.getObject(CONFIG.apiURL.page.employee.edit.profile + this.employee.memberid)
      .subscribe(
        (res) => {
          this.base_profile = res;

          if (callback) {
            callback();
          }
          this.submitting = false;
        }
      )
  }

  tabChanged(event) {
    this.tabIndex = event.index;
    this.currentIndex = this.getCurrentLoad(this.tabIndex);
  }

  terminated() {
    this.submitting = true;
    this.getbase_profile(() => {
      this.currentIndex = -1;
      this.submitting = false;
      setTimeout(() => {
        this.tabIndex = 7 - this.tab_enabled.slice(0, 7).filter(x => x === false).length;
        this.currentIndex = 7;
      }, 500);
    });
  }
}
