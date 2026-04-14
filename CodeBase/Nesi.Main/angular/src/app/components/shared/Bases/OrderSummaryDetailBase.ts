
import { Component, OnInit, Input } from '@angular/core';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { MessageBase } from 'app/core/messageBaseComponent';
import { MessagespageBase } from 'app/pages/messagesPage/_base/messagespage.base';
import { SelectItem } from 'primeng/primeng';

export class OrderSummaryDetailBase extends MessageBase {
    selected_business_unit: any;
    items: any[];
    loading = false;
    columnOptions: SelectItem[];
    cols: string[];

    url: string;

    constructor(
        public store: Store<fromRoot.State>,
        public cs: CoreService,
        public ts: TokenService,
    ) {
        super(store);
    }

    // tslint:disable-next-line:use-life-cycle-interface
    ngOnInit() {
        this.selected_business_unit = this.ts.currentUser.businessUnitId;
        this.init();
        this.cols = this.columnOptions.map(x => x.value);
        this.loadDetail();
    }

    init() {

    }

    detailLoaded() {

    }
    showCol(header: string) {
        return this.cols.indexOf(header) > -1;
    }

    loadDetail() {
        this.loading = true;
        this.cs.getObject<any[]>(this.url + this.selected_business_unit)
            .subscribe(
            (res) => {
                this.items = res;
                this.loading = false;
                this.detailLoaded();
            }
            );
    }

    get host() {
        return CONFIG.Nesi1URL.host();
    }
}
