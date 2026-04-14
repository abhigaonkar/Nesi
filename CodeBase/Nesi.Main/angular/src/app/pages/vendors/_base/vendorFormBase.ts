
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';

export class VendorFormBase extends FormMessageBase {
    _vendor_id: number;
    _profile: any;
    @Output() loading = new EventEmitter();
    @Output() loaded = new EventEmitter();
    @Output() close = new EventEmitter();

    @Input() edit_disabled = false;

    @Input() set vendor_id(value: number) {
        if (value) {
            this._vendor_id = value;
            this.loadData();
        }
    }
    get vendor_id(): number {
        return this._vendor_id;
    }


    profileUrl: string;
    get profile(): any {
        return this._profile || { entity: { isnull: true }, isnull: true };
    }
    set profile(value: any) {
        if (value && !value.isnull) {
            this._profile = value;
        }
    }

    get entity(): any {
        return this.profile.entity;
    }

    constructor(
        protected store: Store<fromRoot.State>,
        public cs: CoreService

    ) {
        super(store, cs);
    }

    InitVendor(post_url: string, profile_url: string = null) {
        super.Init(post_url);
        this.profileUrl = profile_url || post_url;
    }


    AfterProfileLoaded() {

    }

    // tslint:disable-next-line:use-life-cycle-interface
    ngOnInit() {
    }

    loadData(forceLoad = false) {
        if (this.profileUrl && this.profileUrl !== 'N/A') {
            if (!forceLoad && !this.vendor_id) {
                return;
            }
            this.submitting = true;
            this.loading.emit();
            this.cs.getObject<any>(this.getUrl(this.profileUrl))
                .subscribe(
                    (res) => {
                        this.profile = res;
                        this.submitting = false;
                        this.loadProfile();
                        this.loaded.emit();
                    }
                );
        } else if (this.postUrl) {
            this.profile = { entity: this.entity };
            this.loadProfile();
        }
    }

    createForm() {
    }


    loadProfile() {
        if (this.userform && this.profile.entity && !this.profile.entity.isnull) {
            this.userform.patchValue(this.profile.entity);

        }

        this.disableFields();
        this.AfterProfileLoaded();
        this.submitReset();
    }

    getUrl(path: string): string {
        if (!this.vendor_id) {
            return path;
        }
        const o = path.replace('$vendor_id', this.vendor_id.toString());
        return o;
    }

    formValidateBefore() {
        if (this.userform.get('vendor_id')) {
            this.userform.get('vendor_id').setValue(this.vendor_id);
        }
    }
}
