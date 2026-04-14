import { Component, OnInit, Input } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';
import { WhoDoIAsk } from '../../../models/layout/whoDoIAsk';

@Component({
  selector: 'bar-whoDoIAskTable',
  templateUrl: './whoDoIAskTable.component.html',
  styleUrls: ['./whoDoIAskTable.component.css']
})
export class WhoDoIAskTableComponent implements OnInit {

@Input()
  whoDoIAsk: WhoDoIAsk[];
  @Input()
  showAll = false;
  @Input()
  recentNumber = 10;

  constructor() { }

  ngOnInit() {
  }
  get WhoDoIAskList(): WhoDoIAsk[] {
    return this.showAll ? this.whoDoIAsk : this.whoDoIAsk.slice(0, this.recentNumber);
  }
}
