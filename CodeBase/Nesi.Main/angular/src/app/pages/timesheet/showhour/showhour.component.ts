import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nesi-showhour',
  templateUrl: './showhour.component.html',
  styleUrls: ['./showhour.component.css']
})
export class ShowhourComponent implements OnInit {
  @Input() hour: number;
  @Input() label: string;

  constructor() { }

  ngOnInit() {
  }

}
