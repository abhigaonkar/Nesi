import { Component, OnInit, Input } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-fieldErrorDisplay',
  templateUrl: './fieldErrorDisplay.component.html',
  styleUrls: ['./fieldErrorDisplay.component.css']
})
export class FieldErrorDisplayComponent implements OnInit {

  @Input() errorMsg: string;
  @Input() displayError: boolean;

  constructor() { }

  get displayMsg(): string {
    return this.errorMsg;
  }

  ngOnInit() {

  }

}
