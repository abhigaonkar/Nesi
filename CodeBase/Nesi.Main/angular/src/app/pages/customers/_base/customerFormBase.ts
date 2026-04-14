
import { Component, OnInit, Input } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';

export class CustomerFormBase extends FormMessageBase {
    _customer_id: number;
    _address_id: number;

    @Input() customer_base_profile: any;

    @Input() edit_disabled = false;

    @Input() set customer_id(value: number) {
        if (value) {
            this._customer_id = value;
            this.loadData();
        }
    }
    get customer_id(): number {
        return this._customer_id;
    }

    @Input() set address_id(value: number) {
        if (value) {
            this._address_id = value;
            this.loadData();
        }
    }

    get address_id(): number {
        return this._address_id;
    }

    profileUrl: string;
    profile: any;

    constructor(
        protected store: Store<fromRoot.State>,
        public cs: CoreService

    ) {
        super(store, cs);
    }

    InitCustomer(post_url: string, profile_url: string = null) {
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
        if (this.profileUrl && this.profileUrl !== 'N/A' && this.customer_id) {
            if (this.profileUrl.indexOf('$address_id') > -1 && !this._address_id) {
                return;
            }
            this.submitting = true;
            this.cs.getObject<any>(this.getUrl(this.profileUrl))
                .subscribe(
                (res) => {
                    this.profile = res;
                    this.submitting = false;
                    this.AfterProfileLoaded();
                    this.submitReset();
                    if (this.customer_base_profile) {
                        if (!this.customer_base_profile.is_edit_allowed) {
                            this.edit_disabled = true;
                            this.userform.disable();
                        }
                    }
                }
                );
        }
    }

    createForm() {
    }

    getUrl(path: string): string {
        if (!this.customer_id) {
            return path;
        }
        let o = path.replace('$customer_id', this.customer_id.toString());
        if (this.address_id) {
            o = o.replace('$address_id', this.address_id.toString());
        }
        return o;
    }

    formValidateBefore() {
        if (this.userform.get('customer_id')) {
            this.userform.get('customer_id').setValue(this.customer_id);
        }
        if (this.userform.get('address_id')) {
            this.userform.get('address_id').setValue(this.address_id);
        }
    }
}
