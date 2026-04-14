import { Component, OnInit, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'nesi-asyncFieldErrorDisplay',
  templateUrl: './asyncFieldErrorDisplay.component.html',
  styleUrls: ['./asyncFieldErrorDisplay.component.css']
})
export class AsyncFieldErrorDisplayComponent implements OnInit {

  @Input() errorMsg: string;
  @Input() control: FormControl;
  @Input() field = 'data';
  @Input() show_check = false;
  
  constructor() { }

  ngOnInit() {
  }

}
