
import { Component, OnInit, Input } from '@angular/core';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { MessageBase } from 'app/core/messageBaseComponent';
import { MessagespageBase } from 'app/pages/messagesPage/_base/messagespage.base';
import { SelectItem } from 'primeng/primeng';
export class OrderSummaryBase extends MessageBase {

    visible_businessUnit_list: any[];
    selectedBusinessUnits: any[];
    copy_selectedBusinessUnits: any[];
    all_tax_entity_list: any[];
    selected_tax_entity_list: any[];
    sum: any;
    summaries: any[] = [];
    openned_tax_entities: number[] = [];

    urls: any;
    summaryList: any[] = [];

    constructor(
        public store: Store<fromRoot.State>,
        public cs: CoreService,
        public ts: TokenService,
    ) {
        super(store);
    }


    load_init(res: any) {

    }

    init() {

    }

    // tslint:disable-next-line:use-life-cycle-interface
    ngOnInit() {
        this.init();
        this.sum = this.init_sum;
        this.cs.getObject<any>(this.urls.profile)
            .subscribe(
                (res) => {
                    this.load_init(res);
                    this.visible_businessUnit_list = res.visible_businessUnit;
                    this.all_tax_entity_list = this.map_Tax_entity(this.visible_businessUnit_list);
                    if (res.selected_businessUnits) {
                        this.selectedBusinessUnits = res.selected_businessUnits;
                    } else {
                        this.selectedBusinessUnits = this.visible_businessUnit_list.map(x => x.value);
                    }
                    if (res.openned_tax_entities) {
                        this.openned_tax_entities = res.openned_tax_entities;
                    } else {
                        this.openned_tax_entities = [this.ts.currentUser.tax_entity_id];
                    }

                    this.copy_selectedBusinessUnits = this.selectedBusinessUnits;
                    this.selected_tax_entity_list = this.gettax_entity_list();
                    // loaditem for opened tax entities
                    this.load_opentax_entity_items();
                }
            );
    }

    load_opentax_entity_items() {
        this.summaryList = [];
        this.openned_tax_entities.forEach(x => {
            if (this.selected_tax_entity_list.findIndex(y => y.id == x) >= 0) {
                this.loadItem(-1, x);
            }
        });

    }

    map_Tax_entity(list: any[]) {
        if (!list || list.length === 0) {
            return null;
        }
        CONFIG.LOG(this.openned_tax_entities, 'openned.tax entity of tax entity openned');

        return list.map(x => {
            //   CONFIG.LOG(x.tax_entity_id, 'x.tax entity of tax entity openned');
            // CONFIG.LOG(this.openned_tax_entities.indexOf(x.tax_entity_id), 'index of tax entity openned');
            return {
                name: x.tax_entity_name, id: x.tax_entity_id,
                open: this.openned_tax_entities.indexOf(x.tax_entity_id) > -1
            }
        });
    }

    gettax_entity_list(): any[] {
        if (!this.visible_businessUnit_list || !this.copy_selectedBusinessUnits || this.copy_selectedBusinessUnits.length === 0) {
            return null;
        }
        const bulist = this.visible_businessUnit_list.filter(x => this.copy_selectedBusinessUnits.findIndex(y => y === x.value) > -1)
            .filter(function (item, i, ar) { return ar.findIndex(y => y.tax_entity_id === item.tax_entity_id) === i; });
        CONFIG.LOG(bulist.map(x => x.tax_entity_id), 'tax entity list in get tax entity');
        return this.map_Tax_entity(bulist);
    }

    getBusinessUnitByTaxEntity(tax_entity_id: number) {
        return this.visible_businessUnit_list
            .filter(x => x.tax_entity_id === tax_entity_id && this.copy_selectedBusinessUnits.indexOf(x.value) > -1);
    }

    selectChange() {
        this.sum = this.init_sum;
        this.copy_selectedBusinessUnits = this.selectedBusinessUnits;
        this.selected_tax_entity_list = this.gettax_entity_list();
        this.load_opentax_entity_items();
        this.saveLayout();
    }

    get init_sum() {
        return {};
    }

    caluculateSum() {

    }
    buLoaded(event) {
        if (!event) { return; }
        this.summaries.push(event);
        this.caluculateSum();
    }

    tabOpen(event) {
        CONFIG.LOG(event.index, 'tab open in work order summary');
        this.selected_tax_entity_list[event.index].open = true;
        if (this.openned_tax_entities.indexOf(this.selected_tax_entity_list[event.index].id) == -1) {
            this.openned_tax_entities.push(this.selected_tax_entity_list[event.index].id);
        }
        this.loadItem(event.index, 0);
        this.saveLayout();
    }

    loadItem(index: number, teid: number) {
        if (index >= 0) {
            teid = this.selected_tax_entity_list[index].id;
        }
        const buids = this.getBusinessUnitByTaxEntity(teid).map(x => x.value);
        buids.forEach(x => {
            this.summaryList = this.summaryList.filter(y => y.businessUnit_id != x);
        });
        this.cs.postList<any>(this.urls.businessUnit_summary, buids)
            .subscribe(
                (res: any[]) => {
                    for (let i = 0; i < res.length; i++) {
                        this.summaryList = this.summaryList.filter(x => x.businessUnit_id != res[i].businessUnit_id);
                        this.summaryList.push(res[i]);
                    }
                    this.caluculateSum();
                }
            );
    }

    tabClose(event) {
        CONFIG.LOG(event.index, 'tab close in work order summary');
        this.selected_tax_entity_list[event.index].open = false;
        this.summaryList = this.summaryList.filter(x => x.tax_entity_id != this.selected_tax_entity_list[event.index].id);
        this.openned_tax_entities = this.openned_tax_entities.filter(x => x != this.selected_tax_entity_list[event.index].id);
        this.caluculateSum();
        this.saveLayout();
    }

    saveLayout() {
        const data = {
            selected_businessUnits: this.selectedBusinessUnits,
            openned_tax_entities: this.openned_tax_entities
        };
        CONFIG.LOG(data, 'dat of savelayout in workorder summary');
        this.cs.postString(this.urls.saveLayout, data)
            .subscribe(
                (res) => {
                    // this.PushShortResponseMessage(res);
                }
            );
    }

}