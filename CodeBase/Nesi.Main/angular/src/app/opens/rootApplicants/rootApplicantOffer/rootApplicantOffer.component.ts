import { Component, OnInit } from '@angular/core';
import { RootApplicantBase } from '../rootApplicantBase';
import { ActivatedRoute } from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-rootApplicantOffer',
  templateUrl: './rootApplicantOffer.component.html',
  styleUrls: ['./rootApplicantOffer.component.css']
})
export class RootApplicantOfferComponent extends RootApplicantBase implements OnInit {

  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
}

