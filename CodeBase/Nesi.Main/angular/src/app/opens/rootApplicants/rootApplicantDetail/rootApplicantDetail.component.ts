import { Component, OnInit } from '@angular/core';
import { RootApplicantBase } from '../rootApplicantBase';
import { ActivatedRoute } from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-rootApplicantDetail',
  templateUrl: './rootApplicantDetail.component.html',
  styleUrls: ['./rootApplicantDetail.component.css']
})
export class RootApplicantDetailComponent extends RootApplicantBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
  }

}
