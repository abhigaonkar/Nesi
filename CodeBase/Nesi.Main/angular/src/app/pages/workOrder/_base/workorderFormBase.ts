
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';

export class WorkOrderFormBase extends FormMessageBase {
    _profile: any;
    _workorder: any;
    @Output() loading = new EventEmitter();
    @Output() loaded = new EventEmitter();
    @Output() close = new EventEmitter();

    @Input() set workorder(value: any) {
        if (value && value.str_wo_id) {
            this._workorder = value;
            this.loadData();
        }
    };

    get workorder(): any {
        return this._workorder || { isnull: true }
    }

    get str_wo_id(): string {
        return this.workorder.str_wo_id || '';
    }

    get woprog_id(): number {
        return this.workorder.woprog_id || parseInt(this.str_wo_id, 10);
    }

    get business_unit_id(): number {
        return this.workorder.business_unit_id || 0;
    }

    get entity() {
        return this.profile.entity || {};
    }

    set profile(value: any) {
        if (value && !value.isnull) {
            this._profile = value;
        }
    }

    get profile() {
        return this._profile || { entity: { isnull: true }, isnull: true };
    }

    profileUrl: string;



    constructor(
        protected store: Store<fromRoot.State>,
        public cs: CoreService,
    ) {
        super(store, cs);
    }

    InitApplicant(post_url: string, profile_url: string = null) {
        this.Init(post_url, profile_url);
    }


    Init(post_url: string, profile_url: string = null) {
        super.Init(post_url);
        this.profileUrl = profile_url || post_url;
    }


    AfterProfileLoaded() {
    }

    // tslint:disable-next-line:use-life-cycle-interface
    ngOnInit() {
        //  this.loadData();
    }

    loadData() {
        if (this.profileUrl && this.profileUrl !== 'N/A' && (this.str_wo_id)) {
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
            this.profile = { entity: this.workorder };
            this.loadProfile();
        }
    }

    loadProfile() {
        if (this.userform && this.profile.entity && !this.profile.entity.isnull) {
            this.userform.patchValue(this.profile.entity);
        }
        this.disableFields();
        this.AfterProfileLoaded();
        this.submitReset();
    }

    createForm() {
    }

    getUrl(path: string = null): string {
        if (!(this.str_wo_id || this.business_unit_id)) {
            return path;
        }
        if (!path) {
            path = this.postUrl;
        }
        const o = path.replace('$str_wo_id', this.str_wo_id)
            .replace('$bu_id', this.business_unit_id.toString())
            .replace('$woprog_id', this.woprog_id.toString());
        return o;
    }

    formValidateBefore() {
        if (this.userform.get('str_wo_id')) {
            this.userform.get('str_wo_id').setValue(this.str_wo_id);
        }
        if (this.userform.get('business_unit_id')) {
            this.userform.get('business_unit_id').setValue(this.business_unit_id);
        }
        if (this.userform.get('woprog_id')) {
            this.userform.get('woprog_id').setValue(this.woprog_id);
        }
    }
}
