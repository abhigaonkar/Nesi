

import { Component, OnInit, Input } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { EmployeeService } from '../_base/employeeService';
import { EmployeeFormBase } from '../_base/employeeFormBase';

export class EmployeeOfferFormBase extends EmployeeFormBase {

    _save_enabled = true;
    disabled_form_with_save_enabled = true;
    @Input() set save_enabled(value: boolean) {
        this._save_enabled = value;
        this.disableFields();
    }

    get save_enabled() {
        return this._save_enabled;
    }

    _offer: any;

    @Input() set offer(value: any) {
        if (value) {
            this._offer = value;
            this._memberid = this._offer.memberid;
            this.loadData();
        }
    }

    get offer(): any {
        return this._offer || {};
    }

    set offer_id(value: number) {
        this.offer.id = value;
    }
    get offer_id(): number {
        return this.offer.id || 0;
    }


    get is_applicant(): boolean {
        return this.offer.is_applicant || false;
    }

    set is_applicant(value: boolean) {
        this.offer.is_applicant = value;
    }

    get applicant_id(): number {
        return this.offer.applicant_id || 0;
    }
    set applicant_id(value: number) {
        this.offer.applicant_id = value;
    }

    constructor(
        protected store: Store<fromRoot.State>,
        public cs: CoreService,
        public es: EmployeeService,

    ) {
        super(store, cs, es);
    }

    disableFields() {
        if (this.disabled_form_with_save_enabled && this.userform) {
            if (!this.save_enabled) {
                this.userform.disable();
            } else {
                this.userform.enable();
            }
        }
    }

    getUrl(url: string) {
        const u = super.getUrl(url);
        return u &&
            u.replace('$offer_id', this.offer_id.toString())
                .replace('$is_applicant', this.is_applicant ? '1' : '0')
                .replace('$applicant_id', this.applicant_id.toString());
    }

    formValidateBefore() {
        super.formValidateBefore();
        if (this.userform.get('offer_id')) {
            this.userform.get('offer_id').setValue(this.offer_id);
        }
    }


    submitSuccess() {
        if (this.extraData && this.extraData.entity) {
            this.userform.patchValue(this.extraData.entity);
            this.offer_id = this.extraData.entity.offer_id;
        }
        this.formSubmitAttempt = false;
    }
}
