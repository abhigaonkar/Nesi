import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CoreService } from '../../../services/shared/core.service';
import { Subscription } from 'rxjs';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-rootEmployeeOffer',
  templateUrl: './rootEmployeeOffer.component.html',
  styleUrls: ['./rootEmployeeOffer.component.css']
})
export class RootEmployeeOfferComponent extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
  }

}
