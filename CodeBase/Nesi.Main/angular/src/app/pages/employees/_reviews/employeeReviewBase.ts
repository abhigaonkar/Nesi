import { Component, OnInit, Input } from '@angular/core';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { MessageBase } from '../../../core/messageBaseComponent';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { Validators, FormBuilder } from '@angular/forms';
import { EmployeeService } from '../_base/employeeService';
import { ConfirmationService } from 'primeng/primeng';
import { DataExtra } from '../../../models/core/dataExtra';
import { EmployeeFormBase } from '../_base/employeeFormBase';

export class EmployeeReviewBase extends EmployeeFormBase {

    _review: any;

    @Input() set review(value: any) {
        if (value) {
            this._review = value;
            this._memberid = this._review.memberid;
            this.loadData();
        }
    }

    get review(): any {
        return this._review || {};
    }

    set review_id(value: number) {
        this.review.id = value;
    }
    get review_id(): number {
        return this.review.id || 0;
    }

    constructor(
        protected store: Store<fromRoot.State>,
        public cs: CoreService,
        public es: EmployeeService,

    ) {
        super(store, cs, es);
    }

    createForm() {
    }

    getUrl(url: string) {
        const u = super.getUrl(url);
        return u &&
            u.replace('$review_id', this.review_id.toString());
    }

    formValidateBefore() {
        super.formValidateBefore();
        if (this.userform.get('review_id')) {
            this.userform.get('review_id').setValue(this.review_id);
        }
    }

    submitSuccess() {
        if (this.extraData) {
            this.profile = this.extraData;
            if (this.extraData.entity) {
                this.userform.patchValue(this.extraData.entity);
                this.review_id = this.extraData.entity.review_id;
            }
        }
        this.formSubmitAttempt = false;
    }
}