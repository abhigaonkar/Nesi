import { Component, OnInit } from '@angular/core';
import { MessageBase } from 'app/core/messageBaseComponent';
import { WindowRef } from 'app/services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import { CONFIG } from 'app/configuration';


export class HomePageBase extends MessageBase implements OnInit {

    loading = false;
    items: any[];

    get Url(): string {
        return '';
    }

    afterLoaded(res) {

    }

    constructor(
        public cs: CoreService,
        protected store: Store<fromRoot.State>,
    ) {
        super(store);
    }

    ngOnInit() {
        this.Refresh();
    }

    public Refresh() {
        this.loading = true;
        this.cs.getList<any>(this.Url)
            .subscribe(
            (res: any[]) => {
                this.items = res;
                this.afterLoaded(res);
                this.loading = false;

            });
    }
}
