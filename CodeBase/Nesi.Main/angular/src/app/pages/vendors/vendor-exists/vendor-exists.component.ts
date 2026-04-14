import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { LabelValueInt } from '../../../models/Shared/labelValueString';

@Component({
  selector: 'nesi-vendor-exists',
  templateUrl: './vendor-exists.component.html',
  styleUrls: ['./vendor-exists.component.css']
})
export class VendorExistsComponent implements OnInit {
  @Input() list: LabelValueInt[];
  @Input() query: string;
  @Output() openVendor = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  get_text(item) {
    if (this.query) {
      const regEx = new RegExp(this.query, 'ig');
      return item.label.replace(regEx, '<b>' + this.query + '</b>');
    } else {
      return item.label;
    }
  }
}
