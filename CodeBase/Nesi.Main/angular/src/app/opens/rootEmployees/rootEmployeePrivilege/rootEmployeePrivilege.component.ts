import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CoreService } from '../../../services/shared/core.service';
import { Subscription } from 'rxjs';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  selector: 'nesi-rootEmployeePrivilege',
  templateUrl: './rootEmployeePrivilege.component.html',
  styleUrls: ['./rootEmployeePrivilege.component.css']
})
export class RootEmployeePrivilegeComponent extends RootEmployeeBase implements OnInit {

  
  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
 
}

