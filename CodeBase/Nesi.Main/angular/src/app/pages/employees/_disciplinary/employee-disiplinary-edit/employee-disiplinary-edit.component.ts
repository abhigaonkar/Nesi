import { DatatableComponent } from '../../../../components/nesi-datatable/components/datatable/datatable.component';
import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { EmployeeFormBase } from '../../_base/employeeFormBase';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { TokenService } from '../../../../services/authentication/tokenService';
import { CONFIG } from '../../../../configuration';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { WindowRef } from '../../../../services/shared/windowRef';
import { EmployeeService } from '../../_base/employeeService';

@Component({
  selector: 'nesi-employee-disiplinary-edit',
  templateUrl: './employee-disiplinary-edit.component.html',
  styleUrls: ['./employee-disiplinary-edit.component.css']
})
export class EmployeeDisiplinaryEditComponent extends EmployeeFormBase implements OnInit {
  _id: number;

  @Input() set id(value: number) {
    if (value && value > 0) {
      this._id = value;
      this.header_label = "Update";
      this.loadData();
    } else {
      this.header_label = "Add";
    }
  }

  get id() {
    return this._id || 0;
  }

  header_label = "Add";

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public ts: TokenService,
    private fb: FormBuilder,
    private win: WindowRef,
    public es: EmployeeService,
  ) {
    super(store, cs, es);
    super.InitEmployee(
      CONFIG.apiURL.page.employee.disciplinary.save,
      CONFIG.apiURL.page.employee.disciplinary.get,
    );
  }

  ngOnInit() {
    this.cs.getString(CONFIG.apiURL.page.employee.disciplinary.filePath)
    .subscribe(
    (res) => {
      this.fullPath = res + '\\' + this.ts.currentAuthData.guid;
    },
    (err:any)=>
    {
      this.PushErrorMessage(err);
    }
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'membernote_member_id': ['', Validators.required],
      'date': ['', [Validators.required]],
      'comments': ['', Validators.required],
      'file_id': '',
      'file_name': '',
      'membernote_id': '',
    });

  }

  getUrl(url): string {
    return super.getUrl(url).replace('@id', this.id.toString());
  }

  formValidateBefore() {
    this.userform.get('membernote_member_id').setValue(this.memberid);
  }

  submitSuccess() {
    this.userform.patchValue(this.extraData && this.extraData.entity);
    this.formSubmitAttempt = false;
    this.id = 0;
  }


  onUploaded(event: any) {
    super.onUploaded(event);
    if (this.uploadedFiles && this.uploadedFiles.length > 0) {
      this.userform.get('file_name').setValue(this.uploadedFiles[0].name);
    }
    super.LOG(this.userform.get('file_name').value, 'file uploaded new message');

  }

}