import { ConfirmationService } from 'primeng/primeng';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SwitchUser } from '../../../models/banner/switchUser';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { TokenService } from 'app/services/authentication/tokenService';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'bar-swtichUser',
  templateUrl: './swtichUser.component.html',
  styleUrls: ['./swtichUser.component.css']
})
export class SwtichUserComponent implements OnInit {

  // @Input()
  currentUserId: number;
  @Input()
  userName: string;
  @Input()
  businessUnitName: string;

  @Output() change = new EventEmitter();

  @Output() cancel = new EventEmitter();

  @Input()
  switchUserList: SwitchUser[] = [];

  @Input()
  swtiched = false;

  @Input()
  preShow = false;
  constructor(
    private confirmationService: ConfirmationService,
    private ts: TokenService,
  ) {
    this.currentUserId = ts.currentUser.id;
  }

  ngOnInit() {
  }

  checkFVR(str: string): string {
    if(!str) {
      //
      // 1939: bad data screw up switching users.
      // Bring the ability to skip the bad user data copying from other db (partially copying)
      //
      return '';
    }
    return str.indexOf('FVR Lockout') > 0 ? 'red' : '';
  }

  onchange(event) {
    //  event.preventDefault();
    event.originalEvent.cancelBubble = true;
    // event.selectedId = this.currentUserId;

    // tslint:disable-next-line:triple-equals
    const s = this.switchUserList.find(x => x.member_id === this.currentUserId);
    let name = '';

    if (s) {
      name = s.member_name;
    }

    this.confirmationService.confirm({
      message: 'Are you sure that you want to switch to ' + name + '?',
      accept: () => {

        this.change.emit({ selectedId: this.currentUserId });
        return true;
      },
      reject: () => {
        this.currentUserId = this.ts.currentUser.id;
        return false;
      }
    });
  }


  onclick(event) {
    // event.preventDefault();
  }

  onCancelClick(event) {
    event.preventDefault();
    event.cancelBubble = true;

    this.confirmationService.confirm({
      message: 'Are you sure that you want to stop masquerading?',
      accept: () => {

        this.cancel.emit(event);
        return true;
      },
      reject: () => {
        return false;
      }
    });
  }

}
