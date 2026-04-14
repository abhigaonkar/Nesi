import { Component, OnInit, OnDestroy } from '@angular/core';
import { CONFIG } from '../../configuration';
import { DashMessage } from '../../models/layout/dashMessage';
import { TokenService } from '../../services/authentication/tokenService';
import { BannerService } from '../../services/layout/banner.service';
import { Subscription } from 'rxjs/Subscription';
import * as DATETIME from '../../services/helper/datetime';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromMessage from '../../actions/layout/growlMessage';
import { SignalRService } from '../../services/authentication/signalR.service';
import { CoreService } from '../../services/shared/core.service';

@Component({
  selector: 'nesi-dashMessage',
  templateUrl: './dashMessage.component.html',
  styleUrls: ['./dashMessage.component.css']
})
export class DashMessageComponent implements OnInit, OnDestroy {
  selectBusinessUnitId: number;
  msgText: string;
  subMsg: Subscription;
  msgs: DashMessage[];
  msg = '';

  constructor(
    private ts: TokenService,
    private bs: BannerService,
    private store: Store<fromRoot.State>,
    private ss: SignalRService,
    public cs: CoreService,
  ) {

  }

  ngOnInit() {
    this.selectBusinessUnitId = this.ts.currentUser.businessUnitId;
    this.loadText();
  }

  loadText() {
    this.subMsg = this.bs.loadDashMessageList(this.selectBusinessUnitId)
      .subscribe((res: DashMessage[]) => this.fillText(res));
  }
  fillText(msgs: DashMessage[]) {
    this.msgText = '';
    msgs.forEach(m => {
      this.msgText += DATETIME.ToyyyyMMdd(m.date) + ' ' + m.name + '\n' + m.text + '\n\n';
    });
  }

  ngOnDestroy(): void {
    this.subMsg.unsubscribe();
  }

  change(event: any) {
    this.loadText();
  }

  save() {
    const msg: DashMessage = {
      id: 0,
      name: '',
      date: null,
      text: this.msg,
      memberId: this.ts.currentUser.id,
      businessUnitId: this.selectBusinessUnitId
    }
    this.bs.saveDashMessage(msg).subscribe(
      (res: Boolean) => {
        if (res) {
          this.loadText();
          this.msg = '';
          CONFIG.LOG(this.selectBusinessUnitId, 'dashmessage save buid');
          const name = this.cs.getVisibleBusinessUnitById(this.selectBusinessUnitId).ddl_name;
          CONFIG.LOG(name, 'dashmessage save name');
          this.store.dispatch(new fromMessage.PushSuccessMessage('Save Message successed.'));
          this.ss.FireSignalREventFromBusinessUnit(
            name,
            this.ts.currentUser.id);
        } else {
          this.store.dispatch(new fromMessage.PushErrorMessage('Save Message failed.'));
        }
      },
      (err) => {
        this.store.dispatch(new fromMessage.PushErrorMessage('Save Message failed.'));
      }
    )
  }
}
