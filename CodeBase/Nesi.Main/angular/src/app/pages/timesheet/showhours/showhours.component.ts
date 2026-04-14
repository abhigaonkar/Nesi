import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nesi-showhours',
  templateUrl: './showhours.component.html',
  styleUrls: ['./showhours.component.css']
})
export class ShowhoursComponent implements OnInit {
  @Input() hours: number[];
  @Input() labels: string[];

  constructor() { }

  ngOnInit() {
  }

 
}
