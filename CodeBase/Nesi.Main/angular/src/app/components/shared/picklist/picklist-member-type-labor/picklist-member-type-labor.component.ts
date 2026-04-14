import { Component, OnInit, Input } from '@angular/core';
import { PickListMemberTypeLabor } from '../../../../models/picklist/picklistMemberTypeLabor';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-member-type-labor',
  templateUrl: './picklist-member-type-labor.component.html',
  styleUrls: ['./picklist-member-type-labor.component.css']
})
export class PicklistMemberTypeLaborComponent implements OnInit {

  @Input()
  labor: PickListMemberTypeLabor;

  constructor() { }

  ngOnInit() {
  }

}
