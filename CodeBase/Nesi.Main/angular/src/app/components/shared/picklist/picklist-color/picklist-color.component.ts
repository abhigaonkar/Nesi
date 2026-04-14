import { Component, OnInit, Input } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-color',
  templateUrl: './picklist-color.component.html',
  styleUrls: ['./picklist-color.component.css']
})
export class PicklistColorComponent implements OnInit {

  @Input()
  color: string;

  constructor() { }

  ngOnInit() {
  }

}
