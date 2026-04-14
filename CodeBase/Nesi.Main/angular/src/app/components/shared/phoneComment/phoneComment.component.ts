import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { PhoneLog } from '../../../models/Shared/phoneLog';
import { CoreService } from '../../../services/shared/core.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as MessageHelper from '../../../services/helper/MessageHelper';
import { CONFIG } from '../../../configuration';

@Component({
  selector: 'nesi-phoneComment',
  templateUrl: './phoneComment.component.html',
  styleUrls: ['./phoneComment.component.css']
})
export class PhoneCommentComponent implements OnInit, OnChanges {


  items: PhoneLog[];

  @Input()
  date: Date;

  @Input()
  userId: number;

  constructor(
    public cs: CoreService,
    private store: Store<fromRoot.State>,
  ) { }

  ngOnInit() {

  }

  ngOnChanges(changes: SimpleChanges): void {
    CONFIG.LOG(changes, 'phonecomment on changes')
    if (this.userId && this.userId > 0 && this.date) {
      this.cs.GetPhoneLog(this.userId, this.date)
        .subscribe(
        (res: PhoneLog[]) => {
          this.items = res;
        }
        );
    }
  }

  EditComplete(event: any) {
    const row: PhoneLog = event.data;
    this.cs.UpdatePhoneLog(row.phone_log_id, this.userId, row.notes)
      .subscribe(
      (res: string) => {
        MessageHelper.pushResponseMessage(this.store, res);
      },
      (err: string) => {
        MessageHelper.pushResponseMessage(this.store, err);
      }
      );
  }
}
