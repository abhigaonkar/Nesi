import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromCurrentUser from '../../../actions/layout/currentUser';
import { TimerBase } from '../../shared/Bases/TimerBase';
import { CONFIG } from '../../../configuration';
import { Subscription } from 'rxjs';
import { Observable } from 'rxjs/Observable';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'bar-timer',
  templateUrl: './Timer.component.html',
  styleUrls: ['./Timer.component.css']
})
export class TimerComponent extends TimerBase implements OnInit {

  subRebootTime: Subscription;
  rebootTime$: Observable<number>;
  countdown = 0;
  interval: any;
  start = false;

  constructor(
    private store: Store<fromRoot.State>,
  ) {
    super();
    this.rebootTime$ = this.store.select(fromRoot.getCurrentuser.getRebootTime);
  }

  clearTimer() {
    window.clearInterval(this.interval);
    this.store.dispatch(new fromCurrentUser.SetRebootTime(0));
    this.start = false;
    this.TimeEnd.emit();
  }

  ngOnInit() {
    this.subRebootTime = this.rebootTime$.subscribe(
      (res) => {
        if (res > 0) {
          this.countdown = res;
          CONFIG.LOG(this.countdown, 'countdown reboot time in timer');
          if (!this.interval && !this.start) {
            this.start = true;
            this.interval = window.setInterval(
              () => {
                this.countdown--;
                if (this.countdown <= 0) {
                  this.clearTimer();
                }
              }, 1000
            );
          }
        }
      }
    );
  }

}
