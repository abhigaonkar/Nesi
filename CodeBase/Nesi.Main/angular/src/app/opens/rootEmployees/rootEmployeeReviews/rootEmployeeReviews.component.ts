import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  selector: 'nesi-rootEmployeeReviews',
  templateUrl: './rootEmployeeReviews.component.html',
  styleUrls: ['./rootEmployeeReviews.component.css']
})
export class RootEmployeeReviewsComponent  extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
 
}
