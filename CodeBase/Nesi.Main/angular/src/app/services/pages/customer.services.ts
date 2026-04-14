import { TokenService } from 'app/services/authentication/tokenService';
import { Router } from '@angular/router';
import { WindowRef } from 'app/services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';
import { Injectable } from '@angular/core';
import { CONFIG } from '../../configuration';
import { HttpService } from '../../core/http.service';

@Injectable()
export class CustomersService {
    constructor(
        private ts: TokenService,
        private router: Router,
        private winRef: WindowRef,
      private cs: CoreService,
      private http: HttpService
    ) {

    }

    public openCustomer(cust_id: number) {
        const url = '/#/opens/10/customers/' + cust_id.toString();
        if (url) {
            this.winRef.boing(url, cust_id.toString(), 1600, 960);
        }
    }

    public boingUrl(cust_id: number) {
        const url = '';
        if (url) {
            this.winRef.boingNesi1(url, cust_id.toString(), '_blank');
        }
  }

  create(model: any) {
    return this.http.post(CONFIG.apiURL.page.customerrequestadmins.create, model);
  }

  update(id: number, model: any) {
    const url = CONFIG.apiURL.page.customerrequestadmins.update.replace('$id', id.toString());
    return this.http.post(url, model);
  }

getIndustrialTypes() {
  // NEW dedicated endpoint – no payload needed
  return this.http.get(CONFIG.apiURL.page.customerrequestadmins.industrialTypes);
}

  public tableValues = [
      { value: 'cust_national_account_mgr', label: 'National Account Manager' },
      { value: 'cus_primary_sales_team_member', label: 'Primary Sales Team Member' },
      { value: 'cus_inside_sales_team_member', label: 'Inside Sales Team Member' },
      { value: 'cus_industrial_type', label: 'Industrial Type' },
      { value: 'cus_end_market_segment', label: 'End Market Segment' },
      { value: 'lead_source', label: 'Lead Source' }
    ];
  }

