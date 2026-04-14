import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CoreService } from '../../services/shared/core.service';
import { Subscription } from 'rxjs';

export class RootApplicantBase implements OnInit {
    sub: Subscription;
    applicant: any;
    entity: any;
    constructor(
        public route: ActivatedRoute,
    ) { }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            const applicant_id = String(params['applicant_id']);
            const id = String(params['id']);
            this.applicant = { id: id, applicant_id: applicant_id, is_applicant: true };
        });

    }

}
