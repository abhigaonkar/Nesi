import { Component, OnInit } from '@angular/core';
import { PickListSearchbase } from '../_base/picklist-search-base';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CONFIG } from 'app/configuration';
import { Store } from "@ngrx/store";
import { CoreService } from "app/services/shared/core.service";
import * as fromRoot from '../../../../reducers';
import { FormBuilder } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';


@Component({
  selector: 'nesi-picklist-search-RFQ',
  templateUrl: './picklist-search-RFQ.component.html',
  styleUrls: ['./picklist-search-RFQ.component.css']
})
export class PicklistSearchRFQComponent extends PickListSearchbase implements OnInit {
  placeholder="Search vendor RFQ";
  
  constructor(
    protected fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(fb,store, cs);
    
    super.Init('');
  }

  createForm() {

  }

  
    ngOnInit() {
    }
  
    
    select(event: any) {
  
    }
  
  }
