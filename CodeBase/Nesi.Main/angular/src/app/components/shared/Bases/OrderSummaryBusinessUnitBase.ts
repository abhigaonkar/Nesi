import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { WorkOrderService } from 'app/services/pages/workorder.service';

export class OrderSummaryBusinessUnitBase {
  @Input() businessUnit_id: number;
  @Input() businessUnit_name: string;
  @Input() urls: any;
  @Input() set summaryList(value: any[]) {
    if (value) {
      this.summary = value.find(x => x.businessUnit_id === this.businessUnit_id);
    } else {
      this.summary = null;
    }
  }
  @Input() onloading: boolean;

  @Output() loading = new EventEmitter();
  @Output() loaded = new EventEmitter();

  summary: any;

  constructor(
    public cs: CoreService,
    public ws: WorkOrderService,
  ) { }


  // tslint:disable-next-line:use-life-cycle-interface
  ngOnInit() {
    // this.loadItem();
  }

  get isLg() {
    return this.cs.isLg;
  }

  get isSm() {
    return this.cs.isSm;
  }
  // loadItem() {
  //   if (!this.summary) {
  //     this.onloading = true;
  //     this.loading.emit(this.businessUnit_id);
  //     this.cs.getObject<any>(this.urls.businessUnit_summary + this.businessUnit_id.toString())
  //       .subscribe(
  //       (res) => {
  //         this.summary = res;
  //         this.onloading = false;
  //         this.loaded.emit(this.summary);
  //       }
  //       );
  //   }
  // }

  openUrl(type, buId, from) {
    CONFIG.LOG(type, 'type in open url work order summary');
    CONFIG.LOG(buId, 'buId in open url work order summary');

    this.ws.openUrl(type, buId, from);
  }
}
