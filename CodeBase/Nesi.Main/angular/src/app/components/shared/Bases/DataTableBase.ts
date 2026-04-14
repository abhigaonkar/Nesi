import { Component, OnInit, Input } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from 'app/core/messageBaseComponent';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import * as DATE from '../../../services/helper/datetime';
import { LabelValueInt } from 'app/models/Shared/labelValueString';

export abstract class DataTableBase extends MessageBase {
    items: any[];
    pageSize = 20;
    page = 0;
    totalPage = 0;
    totalRecorders = 0;
    sortField = '';
    sortOrder = 1;
    loading = false;
    text = '';

    abstract get url(): string;

    abstract get postData(): any;

    constructor(
        protected store: Store<fromRoot.State>,
        protected cs: CoreService,
    ) {
        super(store);
    }

    init() {

    }

    afterSearch() { }


    // tslint:disable-next-line:use-life-cycle-interface
    ngOnInit() {
        this.init();
        this.doSearch();
    }

    doSearch() {
        this.loading = true;
        this.cs.postObject<any>(this.url, this.postData)
            .subscribe(
            (res) => {
                this.items = res.data;
                this.totalRecorders = res.totalRecorders;
                this.totalPage = Math.ceil(this.totalRecorders / this.pageSize);
                this.loading = false;
                this.afterSearch();
            }
            );
    }

    loadLazy(event) {
        if (this.totalRecorders === 0) { return; }
        this.sortField = event.sortField;
        this.sortOrder = event.sortOrder;
        this.page = event.first / this.pageSize;
        this.doSearch();
    }

    applyRowStyle(row: any) {
    }

    GroupBy(field: string): any[] {
        return this.items.reduce((prev, cur) => {
            if (!prev[cur[field]]) {
                prev[cur[field]] = [cur];
            } else {
                prev[cur[field]].push(cur);
            }
            return prev;
        }, {});
    }

    getFieldList(field: string) {
        if (this.items) {
            const o = this.items.map(x => x[field]);
            const list: LabelValueInt[] = [];
            o.forEach(x => {
                if (list.findIndex(y => y.value === x) === -1) {
                    list.push({ label: x, value: x });
                }
            });
            return list;
        }
    }
}
