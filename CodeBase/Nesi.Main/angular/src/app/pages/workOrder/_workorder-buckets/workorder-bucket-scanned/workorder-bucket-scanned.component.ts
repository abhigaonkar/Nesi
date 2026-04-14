import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CONFIG } from '../../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { TokenService } from '../../../../services/authentication/tokenService';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';

@Component({
    selector: 'nesi-workorder-bucket-scanned',
    templateUrl: './workorder-bucket-scanned.component.html',
    styleUrls: ['./workorder-bucket-scanned.component.css']
})
export class WorkorderBucketScannedComponent extends FormMessageBase implements OnInit {

    _list: any;
    @Input() height = 200;
    @Input() business_unit_id: number;

    @Input() set list(value: any[]) {
        this._list = value;
    }

    @Output() loading = new EventEmitter();
    @Output() loaded = new EventEmitter();


    get list(): any[] {
        return this._list || [];
    }

    constructor(
        public cs: CoreService,
        protected store: Store<fromRoot.State>,
        public ts: TokenService,
    ) {
        super(store, cs);
    }

    ngOnInit() {
        this.Init(CONFIG.apiURL.page.workOrder.scanned_file_updloaded);
        this.cs.getString(CONFIG.apiURL.page.workOrder.scanned_file_path)
            .subscribe(
                (res) => {
                    this.fullPath = res + '\\' + this.ts.currentAuthData.guid;
                }
            );
    }

    createForm() {

    }

    getUrl() {
        return this.postUrl + '/' + this.business_unit_id.toString();
    }

    onUploaded(event: any) {
        this.submitting = true;
        this.loading.emit();
        super.onUploaded(event);
        if (this.uploadedFiles && this.uploadedFiles.length > 0) {
            this.cs.postDataExtra(this.getUrl(), { fullpath: this.fullPath, files: this.uploadedFiles.map(x => x.name) })
                .subscribe(res => {
                    if (this.PushResponseMessage(res.data)) {
                        this.list = res.extra;
                    }
                    this.submitting = false;
                    this.loaded.emit();
                },
                    (err) => {
                        this.submitting = false;
                        this.loaded.emit();
                    });
        }
    }

}
