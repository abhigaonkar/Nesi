import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-buttonAddResize',
  templateUrl: './buttonAddResize.component.html',
  styleUrls: ['./buttonAddResize.component.css']
})
export class QuoteButtonAddResizeComponent implements OnInit {
  @Output()
  add = new EventEmitter();
  @Output()
  addfromQuote = new EventEmitter();


  @Output()
  resize = new EventEmitter();
  val = false;
  @Input()
  edit_disabled: boolean;
  @Input()
  type: number;
  @Input() quote_id: number;
  @Input() revision: number;
  displayType = 0;
  fromQuote_checked = false;
  fromPredfined_checked = false;

  constructor() { }

  ngOnInit() {
  }

  handleChange(event) {
    this.resize.emit(event);
  }

  fromQuote_checked_change() {
    if (this.fromQuote_checked) {
      this.displayType = 1;
      this.fromPredfined_checked = false;
    } else {
      this.displayType = 0;
      this.fromPredfined_checked = false;
    }
  }
  fromPredifined_checked_change() {
    if (this.fromPredfined_checked) {
      this.displayType = 2;
      this.fromQuote_checked = false;
    } else {
      this.displayType = 0;
      this.fromQuote_checked = false;
    }
  }
}
