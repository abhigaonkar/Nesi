import { TokenService } from 'app/services/authentication/tokenService';
import { Router } from '@angular/router';
import { WindowRef } from 'app/services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';
import { Injectable } from '@angular/core';

@Injectable()
export class VendorsService {
    constructor(
        private ts: TokenService,
        private router: Router,
        private winRef: WindowRef,
        private cs: CoreService,
    ) {

    }

    public openVendor(vendor_id: number) {
        const url = '/#/opens/11/vendors/' + vendor_id.toString();
        if (url) {
            this.winRef.boing(url, vendor_id.toString(), 1600, 960);
        }
    }

    public boingUrl(vendor_id: number) {
        const url = '';
        if (url) {
            this.winRef.boingNesi1(url, vendor_id.toString(), '_blank');
        }
    }
}
