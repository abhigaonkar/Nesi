import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CoreService } from '../../services/shared/core.service';
import { Subscription } from 'rxjs';

export class RootEmployeeBase implements OnInit {
    sub: Subscription;
    employee: any;
    entity: any;
    constructor(
        public route: ActivatedRoute,
    ) { }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            const memberid = String(params['member_id']);
            const id = String(params['id'] || 0);
            this.employee = { memberid: memberid };
            this.entity = { memberid: memberid, id: id };
        });

    }

}
