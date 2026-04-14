import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { PickListNote } from '../../../../models/picklist/picklistNote';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-note',
  templateUrl: './picklist-note.component.html',
  styleUrls: ['./picklist-note.component.css']
})
export class PicklistNoteComponent implements OnInit {
  @Input()
  notes: string;
  @Input()
  partno: string;
  @Input()
  id: string;

  @Output() changed = new EventEmitter();

  tooltipX: string;
  tooltipY: string;

  addNoteDisplay = false;

  constructor(public cs: CoreService) { }

  ngOnInit() {
  }

  moveTip(event: any) {
    this.tooltipX = (event.clientX + 20) + 'px';
    this.tooltipY = (event.clientY + 20) + 'px';
  }
}
