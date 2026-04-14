import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CoreService } from '../../../services/shared/core.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { Subscription } from 'rxjs-compat/Subscription';
import { CONFIG } from '../../../configuration';
import { LoadedRouterConfig } from '@angular/router/src/config';
import { MessageBase } from '../../../core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { DataExtra } from '../../../models/core/dataExtra';

@Component({
  selector: 'nesi-workorder-scanned-file',
  templateUrl: './workorder-scanned-file.component.html',
  styleUrls: ['./workorder-scanned-file.component.css']
})
export class WorkorderScannedFileComponent extends MessageBase implements OnInit {

  @Output() loaded = new EventEmitter();
  @Output() submitted = new EventEmitter();
  @Output() deleted = new EventEmitter();

  private _profile: any;
  public business_unit_id = 0;
  public file_name: string;
  public woprog_id: number;
  public confirmDisplayDialog = false;

  public set profile(value: any) {
    this._profile = value;
  }

  public get profile() {
    return this._profile || { isnull: true, nameList: [] };
  }

  public selected_id: number;
  private sub: Subscription;
  public submitting = false;

  constructor(
    public cs: CoreService,
    public route: ActivatedRoute,
    public router: Router,
    public titleService: Title,
    protected store: Store<fromRoot.State>,

  ) {
    super(store);
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.business_unit_id = +params['business_unit_id'];
      this.file_name = params['file_name'];
      this.woprog_id = +params['woprog_id'];
      this.titleService.setTitle('Workorder Scanned File ' + this.file_name || this.woprog_id.toString());
      this.loadProfile();
    });
  }

  get url() {
    if (this.file_name) {
      return CONFIG.apiURL.page.workOrder.scanned_file + this.business_unit_id.toString() + '?filename=' + this.file_name;
    } else if (this.woprog_id) {
      return CONFIG.apiURL.page.workOrder.renname_file + this.business_unit_id.toString() + '/' + this.woprog_id;
    }
  }

  loadProfile() {
    this.submitting = true;
    this.cs.getObject<any>(this.url)
      .subscribe((res) => {
        this.profile = res;
        this.submitting = false;
        this.loaded.emit({ pdf_path: this.profile.pdf_path });
      });
  }

  back() {
    if (this.file_name) {
      this.router.navigate(['/home/12/workorder/buckets/' + this.business_unit_id]);
    } else if (this.woprog_id) {
      this.router.navigate(['/home/12/workorder/edit/' + this.business_unit_id + '/' + this.woprog_id]);
    }
  }

  submit() {
    this.submitting = true;
    this.cs.postDataExtra(this.url, { data: this.selected_id })
      .subscribe((res) => {
        if (this.PushResponseMessage(res.data)) {
          this.submitted.emit();
          this.back();
        }
        this.submitting = false;
      });
  }


  confirmDelete() {
    this.confirmDisplayDialog = true;
  }

  delete() {
    this.submitting = true;
    this.cs.deleteDataExtra(this.url)
      .subscribe((res) => {
        if (this.PushResponseMessage(res.data)) {
          this.deleted.emit();
          this.back();
        }
        this.submitting = false;
      });
  }

  get wo_title(): string {
    return `Work Order: ${this.profile.orderNumber || ''} - ${this.profile.customerName || ''}`;
  }

}
