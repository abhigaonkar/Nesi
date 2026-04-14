import { Component, OnInit } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { Store } from '@ngrx/store';
import { WindowRef } from '../../../services/shared/windowRef';
import * as fromRoot from '../../../reducers';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { Subscription } from 'rxjs/Subscription';
import * as fromCurrentUser from '../../../actions/layout/currentUser';
import { getRebootTime } from '../../../reducers/layout/currentUser';
import { SignalRService } from '../../../services/authentication/signalR.service';
import { LabelValueString } from '../../../models/Shared/labelValueInt';

@Component({
  selector: 'app-release',
  templateUrl: './release.component.html',
  styleUrls: ['./release.component.css']
})
export class ReleaseComponent extends FormMessageBase implements OnInit {
  timeOptions: LabelValueInt[] = [
    {
      label: ' 1 minutes',
      value: 1
    },
    {
      label: ' 2 minutes',
      value: 2
    },
    {
      label: ' 3 minutes',
      value: 3
    },
    {
      label: ' 5 minutes',
      value: 5
    },

    {
      label: '10 minutes',
      value: 10
    },

    {
      label: '15 minutes',
      value: 15
    },

    {
      label: '20 minutes',
      value: 20
    },
    {
      label: '30 minutes',
      value: 30
    },
    {
      label: '1 hour',
      value: 60
    },
    {
      label: '2 hours',
      value: 120
    },
  ];

  sites: LabelValueString[] = [
    {
      label: 'devbeta',
      value: 'devbetanesica'
    },
    {
      label: 'beta',
      value: 'betanesica'
    },
    {
      label: 'live',
      value: 'wwwnesica'
    },
  ];

  rebootTime$: any;
  subRebootTime: Subscription;
  seconds = 0;
  interval: any;

  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    protected signalr: SignalRService,

  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.releaseSystem.rebootTime);
    this.rebootTime$ = this.store.select(fromRoot.getCurrentuser.getRebootTime);
  }

  createForm() {
    this.userform = this.fb.group({
      'hostName': '',
      'minutes': '',
      'shutDownTime': '',
    });

    const host = window.location.hostname.replace(/\./g, '').replace('www','');
    this.initFormvalue = {
      'hostName': host,
      'minutes': 1,
      'shutDownTime': 1,
    }
  }

  getRebootTime() {
    const host = window.location.hostname.replace(/\./g, '').replace('www','');
    this.cs.getData<number>(CONFIG.apiURL.currentUser.rebootTime + host)
      .subscribe(
      (res: number) => {
        if (!res) { res = 0; }
        CONFIG.LOG(res, 'reboot time in getreboottime in release');
        // this.store.dispatch(new fromCurrentUser.SetRebootTime(res));
        this.signalr.releaseUserId = this.ts.currentUser.id;
        this.signalr.FireSignalRAllNameValue(CONFIG.SignalR.Events.reboot.name, String(res));
      }
      );
  }

  submitSuccess() {
    this.getRebootTime();
  }

  ngOnInit() {
    // this.subRebootTime = this.rebootTime$.subscribe(
    //   (res) => {
    //     CONFIG.LOG(res, 'sub reboot time in release');
    //     if (res !== 0 && this.seconds !== res) {
    //       this.seconds = res;
    //       if (this.seconds > 0) {
    //         this.interval = window.setInterval(() => {
    //           this.seconds--;
    //           if (this.seconds < 0) { window.clearInterval(this.interval); }
    //         }, 1000);
    //       } else {
    //         this.interval = window.setInterval(() => {
    //           this.seconds++;
    //           if (this.seconds > 0) { window.clearInterval(this.interval); }
    //         }, 1000);
    //       }

    //     }
    //   }
    // );
    // this.getRebootTime();
  }
}
