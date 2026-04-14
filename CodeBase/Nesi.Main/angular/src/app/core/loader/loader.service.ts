import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';

import { LoaderState } from './loader';
import { Router, ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../configuration';

@Injectable()

export class LoaderService {

    private loaderSubject = new Subject<LoaderState>();

    loaderState = this.loaderSubject.asObservable();

    constructor(
    ) { }

    show() {
        this.loaderSubject.next(<LoaderState>{show: true});
    }

    topShow() {
    //  CONFIG.LOG('topshow', 'loading bar showing');
      this.loaderSubject.next(<LoaderState>{show: true});
    }

    hide() {
    //  CONFIG.LOG('hide', 'loading bar hide');
        this.loaderSubject.next(<LoaderState>{show: false});
    }


}
