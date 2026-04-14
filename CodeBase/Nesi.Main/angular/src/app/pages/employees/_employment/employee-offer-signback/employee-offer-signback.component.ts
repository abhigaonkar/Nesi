import { Component, OnInit, Input } from '@angular/core';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { EmployeeService } from '../../_base/employeeService';
import { EmployeeOfferFormBase } from '../employeeOfferBase';
import { WindowRef } from '../../../../services/shared/windowRef';
import { ConfirmationService } from 'primeng/primeng';
import { DataExtra } from '../../../../models/core/dataExtra';
import { TokenService } from '../../../../services/authentication/tokenService';

@Component({
  selector: 'nesi-employee-offer-signback',
  templateUrl: './employee-offer-signback.component.html',
  styleUrls: ['./employee-offer-signback.component.css']
})
export class EmployeeOfferSignbackComponent extends EmployeeOfferFormBase implements OnInit {

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    public fb: FormBuilder,
    private win: WindowRef,
    private cf: ConfirmationService,
    private ts: TokenService,
  ) {
    super(store, cs, es);
    this.InitEmployee(CONFIG.apiURL.page.employee.offer.signBack)
  }

  ngOnInit() {
    this.cs.getString(CONFIG.apiURL.page.employee.offer.filePath)
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


  viewFile(id: number) {
    this.win.boingNesi1(CONFIG.Nesi1URL.getFile.replace('@id', id.toString()), 'file_store_' + id.toString());
  }

  deleteFile(id: number) {
    this.cf.confirm({
      message: 'Are you sure that you want to delete this file?',
      accept: () => {
        this.submitting = true;
        this.cs.deleteObject<DataExtra>(this.getUrl(this.postUrl) + '/' + id.toString())
          .subscribe((res) => {
            if (this.PushResponseMessage(res.data)) {
              this.profile = res.extra;
            }
            this.submitting = false;
          },
          (err:any)=>
          {
            this.PushErrorMessage(err);
            this.submitting = false;
          });
      }
    });

  }


  onUploaded(event: any) {
    this.submitting = true;
    super.onUploaded(event);
    if (this.uploadedFiles && this.uploadedFiles.length > 0) {
      this.cs.postDataExtra(this.getUrl(this.postUrl), { data: this.uploadedFiles[0].name })
        .subscribe(res => {
          if (this.PushResponseMessage(res.data)) {
            this.profile = res.extra;
          }
          this.submitting = false;
        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
          this.submitting = false;
        })
    }
  }

}
