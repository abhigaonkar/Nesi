import { Component, OnInit, ViewChild } from '@angular/core';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { TokenService } from 'app/services/authentication/tokenService';
import { DatatableComponent } from 'app/components/nesi-datatable/components/datatable/datatable.component';
import { CONFIG } from 'app/configuration';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-master-work-order-v2',
  templateUrl: './masterWorkOrderV2.component.html'
})

// tslint:disable-next-line:component-class-suffix
export class MasterWorkOrderComponentV2 implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;
  optionList = [];
  public displayEdit = false;
  public entity: any;
  public link = CONFIG.apiURL.page.reports.masterWorkOrderV2.edit;

  constructor(
    public ts: TokenService,
    public cs: CoreService,
  ) {
  }

  ngOnInit() {
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.reports.masterWorkOrderV2.activeRam)
      .subscribe(
        (res) => {
          this.optionList = res;
        }
      );
  }
  loadDetail() {
    this.dt.after_onRefresh();
  }
  activeRamUpdate(event) {
    // console.dir(event);
    // tslint:disable-next-line:max-line-length
    this.cs.postString(CONFIG.apiURL.page.reports.masterWorkOrderV2.updateActiveRam, { id: event.data.woprog_id, value: event.data.acting_ram })
      .subscribe(
        (res) => { }
      );
  }

  openEdit(event) {
    this.entity = event;
    this.displayEdit = true;
  }

  exitFromEditor() {
    this.displayEdit = false;
    this.entity = null;
  }
}
