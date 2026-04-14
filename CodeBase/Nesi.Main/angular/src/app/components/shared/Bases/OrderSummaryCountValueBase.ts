import { Component, OnInit, Input } from '@angular/core';
import { TokenService } from 'app/services/authentication/tokenService';
import { Router } from '@angular/router';
import { WindowRef } from 'app/services/shared/windowRef';
import { WorkOrderService } from 'app/services/pages/workorder.service';
import { CoreService } from 'app/services/shared/core.service';

export class OrderSummaryCountValueBase {
    @Input() value: any;
    @Input() name: string;
    @Input() businessUnit_id: number;

    from: string;
    link_names;

    constructor(
        public ts: TokenService,
        public router: Router,
        public cs: CoreService,
        public ws: WorkOrderService,
    ) { }


    getClass(type: string) {
        if (type === 'count') {
            return {
                'bold': this.businessUnit_id == this.ts.currentUser.businessUnitId,
                'total_count': this.businessUnit_id != this.ts.currentUser.businessUnitId && this.name == 'total',
                'bu_11_total_count': this.businessUnit_id == this.ts.currentUser.businessUnitId && this.name == 'total',
                'on_hold_count': this.businessUnit_id !== this.ts.currentUser.businessUnitId && this.name == 'on_hold',
                'bu_11_on_hold_count': this.businessUnit_id == this.ts.currentUser.businessUnitId && this.name == 'on_hold',
                'a_link': this.link_names.indexOf(this.name) > -1,
            };
        } else {
            return {
                'bold': this.businessUnit_id == this.ts.currentUser.businessUnitId,
                'total_value': this.businessUnit_id != this.ts.currentUser.businessUnitId && this.name == 'total',
                'bu_11_total_value': this.businessUnit_id == this.ts.currentUser.businessUnitId && this.name == 'total',
                'on_hold_value': this.businessUnit_id !== this.ts.currentUser.businessUnitId && this.name == 'on_hold',
                'bu_11_on_hold_value': this.businessUnit_id == this.ts.currentUser.businessUnitId && this.name == 'on_hold',
                'a_link': this.link_names.indexOf(this.name) > -1,
            };
        }
    }

    link_click(event) {
        
        if (event.ctrlKey || event.shiftKey) {
            this.ws.boingUrl(this.name, this.businessUnit_id, this.from);
        } else {
            this.ws.openUrl(this.name, this.businessUnit_id, this.from);
        }
    }


}
