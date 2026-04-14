import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromCurrentUser from '../../../actions/layout/currentUser';
// tslint:disable-next-line:import-blacklist
import { Subscription } from 'rxjs';
import { Observable } from 'rxjs/Observable';

export class TimerBase implements OnInit {

  ticks = 0;
  seconds = 0;
  minutesDisplay = 0;
  hoursDisplay = 0;
  secondsDisplay = 0;

  sub: Subscription;

  public set Seconds(value: number) {
    if (this.sub) {
      this.sub.unsubscribe();
      this.sub = null;
    }
    this.seconds = value;
    this.ticks = 0;
    this.startTimer();
  }

  @Output()
  TimeEnd = new EventEmitter();
  @Output()
  TimeTick = new EventEmitter();

  clearTimer() { }

  constructor(

  ) {
  }

  ngOnInit() {

  }

  public startTimer() {

    if (this.seconds <= 0) { return; }

    const timer = Observable.timer(1000, 1000);
    this.sub = timer.subscribe(
      t => {
        this.ticks = this.seconds - t;
        if (this.ticks <= 0) {
          this.clearTimer();
          this.TimeEnd.emit();

          if (this.sub) {
            this.sub.unsubscribe();
          }
          return;
        }
        this.secondsDisplay = this.getSeconds(this.ticks);
        this.minutesDisplay = this.getMinutes(this.ticks);
        this.hoursDisplay = this.getHours(this.ticks);
        this.TimeTick.emit(this.timeString);
      }
    );
  }

  protected getSeconds(ticks: number) {
    return this.pad(ticks % 60);
  }

  protected getMinutes(ticks: number) {
    return this.pad((Math.floor(ticks / 60)) % 60);
  }

  protected getHours(ticks: number) {
    return this.pad(Math.floor((ticks / 60) / 60));
  }

  protected pad(digit: any) {
    return digit <= 9 ? '0' + digit : digit;
  }

  public get timeString(): string {
    return this.getTimeString(this.ticks);
  }

  public getTimeString(ticks: number, showHours = false) {
    const secondsDisplay = this.getSeconds(ticks);
    const minutesDisplay = this.getMinutes(ticks);
    const hoursDisplay = this.getHours(ticks);
    // tslint:disable-next-line:max-line-length
    return `${hoursDisplay && (showHours || hoursDisplay > 0) ? hoursDisplay + ':' : ''}${(minutesDisplay) && (minutesDisplay <= 59) ? minutesDisplay : '00'}:${(secondsDisplay) && (secondsDisplay <= 59) ? secondsDisplay : '00'}`

  }
}
