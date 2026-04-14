
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { EmployeeService } from './employeeService';
import { TokenService } from '../../../services/authentication/tokenService';

export class EmployeeFormBase extends FormMessageBase {
    _is_applicant: boolean;
    _memberid: number;
    _applicant_id: number;
    _employee: any;
    _profile: any;
    @Output() loading = new EventEmitter();
    @Output() loaded = new EventEmitter();
    @Output() close = new EventEmitter();

    @Input() set employee(value: any) {
        if (value && !value.isnull) {
            this._employee = value;
            this._memberid = this.employee.memberid || this.employee.member_id;
            this._applicant_id = this.employee.applicantid || this.employee.applicant_id;
            this._is_applicant = this.employee.isapplicant || this.employee.is_applicant;
            this.es.memberid = this._memberid;
            this.loadData();
        }
    };

    get employee() {
        return this._employee || { isnull: true };
    }

    get memberid(): number {
        return this._memberid || 0;
    }

    get applicant_id(): number {
        return this._applicant_id || 0;
    }
    get is_applicant(): boolean {
        return this._is_applicant || false;
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
        public es: EmployeeService,
    ) {
        super(store, cs);
    }

    InitApplicant(post_url: string, profile_url: string = null) {
        this.InitEmployee(post_url, profile_url);
    }


    InitEmployee(post_url: string, profile_url: string = null) {
        super.Init(post_url);
        this.profileUrl = profile_url || post_url;
    }


    AfterProfileLoaded() {
    }

    // tslint:disable-next-line:use-life-cycle-interface
    ngOnInit() {
        //  this.loadData();
    }

    loadData(mid: number = 0) {
        if (mid > 0) {
            this._memberid = mid;
        }
        if (this.profileUrl && this.profileUrl !== 'N/A' && (this.memberid || this.applicant_id || this.applicant_id === 0)) {
            this.submitting = true;
            this.loading.emit();
            this.cs.getObject<any>(this.getUrl(this.profileUrl))
                .subscribe(
                    (res) => {
                        this.profile = res;
                        this.es.is_backoffice = this.profile.is_backoffice;
                        this.es.profile = this.profile;
                        this.submitting = false;
                        this.loadProfile();
                        this.loaded.emit();
                    },
                    (err:any) => {
                        if (String(err).includes('404 Not Found')) {
                            this.PushErrorMessage('You do not have permission to access this page.');
                            this.submitting = false;
                            this.close.emit();
                        }
                        else
                        {
                        this.PushErrorMessage(err);
                        this.submitting = false;
                        this.close.emit();
                        }

                    }
                );
        } else if (this.postUrl) {
            this.profile = { entity: this.employee };
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
        if (!(this.memberid || this.applicant_id || this.applicant_id === 0)) {
            return path;
        }
        if (!path) {
            path = this.postUrl;
        }
        const o = path.replace('$member_id', this.memberid.toString())
            .replace('$applicant_id', this.applicant_id.toString());
        return o;
    }

    formValidateBefore() {
        if (this.userform.get('member_id')) {
            this.userform.get('member_id').setValue(this.memberid);
        }
        if (this.userform.get('applicant_id')) {
            this.userform.get('applicant_id').setValue(this.applicant_id);
        }
    }
}
