import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nesi-home-page-qtyBar',
  templateUrl: './home-page-qtyBar.component.html',
  styleUrls: ['./home-page-qtyBar.component.css']
})
export class HomePageQtyBarComponent implements OnInit {
  @Input() max: number = 100;
  @Input() value: number;
  @Input() level_1: number;
  @Input() level_2: number;


  constructor() { }

  ngOnInit() {
  }

}
