import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  selector: 'app-rootEmployeeReview',
  templateUrl: './rootEmployeeReview.component.html',
  styleUrls: ['./rootEmployeeReview.component.css']
})
export class RootEmployeeReviewComponent  extends RootEmployeeBase implements OnInit {
  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
 
}
