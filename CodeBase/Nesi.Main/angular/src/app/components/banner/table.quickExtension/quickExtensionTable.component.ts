import { Component, OnInit, Input } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';
import { QuickExtension } from '../../../models/layout/quickExtension';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'bar-quickExtensionTable',
  templateUrl: './quickExtensionTable.component.html',
  styleUrls: ['./quickExtensionTable.component.css']
})
export class QuickExtensionTableComponent implements OnInit {

@Input()
  quickExtensions: QuickExtension[];
  @Input()
  showAll = false;
  @Input()
  recentNumber = 10;

  constructor() { }

  ngOnInit() {
  }
  get QuickExtensionsList(): QuickExtension[] {
    return this.showAll ? this.quickExtensions : this.quickExtensions.slice(0, this.recentNumber);
  }
}
