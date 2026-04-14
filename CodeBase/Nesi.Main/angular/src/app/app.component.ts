import { Component, OnInit, OnDestroy } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Observable } from 'rxjs/Observable';
import { GrowlMessage } from './models/layout/growlMessage';
import { Store } from '@ngrx/store';
import * as fromRoot from './reducers'
import * as fromMessage from './actions/layout/growlMessage';
import { CONFIG } from './configuration';
import { Profile } from './models/layout/profile';
import { Subscription } from 'rxjs/Subscription';
import { LayoutProfileService } from './services/layout/layoutProfile.services';
import { NavigationEnd, ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { FormControlName, FormControlDirective } from '@angular/forms';




@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations: [
    trigger('overlayState', [
      state('hidden', style({
        opacity: 0
      })),
      state('visible', style({
        opacity: 1
      })),
      transition('visible => hidden', animate('400ms ease-in')),
      transition('hidden => visible', animate('400ms ease-out'))
    ])
  ]
})
export class AppComponent implements OnInit, OnDestroy {

  messages$: Observable<GrowlMessage[]>;
  messageLifeTime: number;
  MessageLifeTime$: Observable<Number>;
  subLayoutProfiles: Subscription;

  constructor(
    private store: Store<fromRoot.State>,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private titleService: Title

  ) {
    this.messages$ = store.select(fromRoot.getMessages);
    this.MessageLifeTime$ = store.select(fromRoot.getLayoutProfiles.MessageLifeTime);
  }

  ngOnInit(): void {
    this.subLayoutProfiles = this.MessageLifeTime$.subscribe((value: number) => {
      if (value) {
        this.messageLifeTime = value;
      } else {
        this.messageLifeTime = CONFIG.messageLifeTime;
      }
    });

    //  console.log(this.messageLifeTime);
    this.router
      .events
      .filter(event => event instanceof NavigationEnd)
      .map(() => {
        let child = this.activatedRoute.firstChild;
        while (child) {
          if (child.firstChild) {
            child = child.firstChild;
          } else if (child.snapshot.data && child.snapshot.data['title']) {
            return child.snapshot.data['title'];
          } else {
            return null;
          }
        }
        return null;
      }).subscribe((title: any) => {
        if (title) {
          this.titleService.setTitle(title);
        }
      });
  }

  ngOnDestroy(): void {
  }

  isMobile() {
    return window.innerWidth <= 640;
  }
}
