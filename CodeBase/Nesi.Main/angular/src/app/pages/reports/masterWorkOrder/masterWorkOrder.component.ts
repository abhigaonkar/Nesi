import { Component, OnInit, ViewChild } from '@angular/core';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { TokenService } from 'app/services/authentication/tokenService';
import { DatatableComponent } from 'app/components/nesi-datatable/components/datatable/datatable.component';
import { CONFIG } from 'app/configuration';
import { CoreService } from 'app/services/shared/core.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-master-work-order',
  templateUrl: './masterWorkOrder.component.html'
})

export class MasterWorkOrderComponent implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;
  optionList = [];
  public displayEdit = false;
  public entity: any;
  public link = CONFIG.apiURL.page.reports.masterWorkOrder.edit;

  public checked = true;

  public buid = 0;
  public status: string;

  constructor(
    public ts: TokenService,
    public cs: CoreService,
    public router: ActivatedRoute,
  ) {
  }

  ngOnInit() {
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.reports.masterWorkOrder.activeRam)
      .subscribe(
        (res) => {
          this.optionList = res;
        }
      );
    this.router.params.subscribe(params => {
      this.buid = params['bu_id'];
      this.status = params['status'];
      this.pass_params();
    });
  }

  pass_params() {
    if (this.buid > 0 && this.status) {
      this.dt.passed_params_apply = true;
      this.dt.passed_filterObject = { status: { value: this.status.replace(/\_/g, ' '), matchMode: 'startsWith' } };
      this.dt.passed_business_unit_string = '[S]' + this.buid.toString();
    }
  }

  loadDetail() {
    this.dt.after_onRefresh();
  }
  activeRamUpdate(event) {
    // console.dir(event);
    // tslint:disable-next-line:max-line-length
    this.cs.postString(CONFIG.apiURL.page.reports.masterWorkOrder.updateActiveRam, { id: event.data.woprog_id, value: event.data.acting_ram })
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
