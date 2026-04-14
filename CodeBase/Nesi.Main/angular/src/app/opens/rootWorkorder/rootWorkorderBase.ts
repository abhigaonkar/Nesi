import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CoreService } from '../../services/shared/core.service';
import { Subscription } from 'rxjs';

export class RootWorkorderBase implements OnInit {
    sub: Subscription;
    workorder: any;
    entity: any;
    constructor(
        public route: ActivatedRoute,
    ) { }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            const str_wo_id = String(params['str_wo_id']);
            const id = String(params['id']);
            this.workorder = { id: id, str_wo_id: str_wo_id };
        });

    }

}
