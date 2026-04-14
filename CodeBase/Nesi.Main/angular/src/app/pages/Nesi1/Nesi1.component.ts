import {
  Component, OnInit, ViewChild,
  AfterViewInit, OnDestroy, HostListener,
  ElementRef, Attribute, Input, Output, EventEmitter
} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeUrl, Title } from '@angular/platform-browser';
import { Subscription } from 'rxjs/Subscription';
import { LoaderService } from '../../core/loader/loader.service';
import { StringItem } from '../../models/basictype/stringItem';
import { MenuService } from '../../services/layout/menuService';
import * as fromRoot from '../../reducers';
import { Store } from '@ngrx/store';
import * as fromMessage from '../../actions/layout/growlMessage';
import { Observable } from 'rxjs/Observable';
import { NesiMenuItem, NesiMenuType } from '../../models/layout/nesiMenuItem';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import { CONFIG, HostContains } from '../../configuration';
import { state, style } from '@angular/animations';
import { TokenService } from '../../services/authentication/tokenService';
import { CoreService } from '../../services/shared/core.service';
import { WindowRef } from '../../services/shared/windowRef';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-Nesi1',
  templateUrl: './Nesi1.component.html',
  styleUrls: ['./Nesi1.component.css']
})
export class Nesi1Component implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild('iframe') iframe: ElementRef;
  @Input() direct_url: string;

  private sub: Subscription;
  private subIsMobile: Subscription;
  public url: string;
  public trustedUrl: SafeUrl;
  private timeout: any;
  private isMobile: boolean;
  private isMobile$: Observable<boolean>;
  pageTitle = 'Spark Ops';
  interval: any;
  pageId: string;

  private loading = false;

  @Output() onloading = new EventEmitter();
  @Output() onloaded = new EventEmitter();


  constructor(
    private ts: TokenService,
    private ms: MenuService,
    private route: ActivatedRoute,
    private router: Router,
    private loaderService: LoaderService,
    private sanitizer: DomSanitizer,
    private store: Store<fromRoot.State>,
    private titleService: Title,
    private win: WindowRef,

  ) {
    /*
    Observable.fromEvent(window, 'resize')
    .debounceTime(500)
    .subscribe((event) => {
      this.onResize();
    });
    */

    this.isMobile$ = this.store.select(fromRoot.getCurrentuser.getIsMobile);
    // adding listeners for the post messages, both newer browser and older browser
    if (window.addEventListener) {
      window.addEventListener('message', this.messageReceiver, false);
    } else {
      (<any>window).attachEvent('onmessage', this.messageReceiver);
    }

  }

  ngOnInit() {
    this.subIsMobile = this.isMobile$.subscribe(res => this.isMobile = res);
    if (!this.direct_url) {
      this.sub = this.route.params.subscribe(
        params => {
          let id = params['id'];
          const urlIn = params['url'];
          CONFIG.LOG(id, 'pageID in nesi1');
          CONFIG.LOG(urlIn, 'urlIn in nesi1');
          this.pageId = id;
          if (!id) {
            id = 0;
          }
          if (urlIn === '0') {
            this.url = this.win.generateNesi1Url(this.ts.nesi1IframeUrl);
            this.url = decodeURIComponent(this.url);
            this.trustedUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
            CONFIG.LOG(this.trustedUrl, 'set iframe url to urlin');
            this.iframeTimeout();
            return;
          }

          // this.titleService.setTitle('test');

          // todo if double menu fix it.
          if (urlIn && String(urlIn).indexOf('#/home/') >= -1 && String(urlIn).indexOf('#/home/0/') < 0) {
            CONFIG.LOG('DoubleMenu', urlIn);
            // window.location.href = urlIn;
            // window.location.reload();
            // return;
          }
          CONFIG.LOG(urlIn, 'url in url');

          if (urlIn && String(urlIn).indexOf('home.aspx') >= 0) {
            this.router.navigate(['/home/1/1/homepage/default']);
          } else if (urlIn && String(urlIn).indexOf('.aspx') >= 0) {
            // get a username and password

            const n2url = CONFIG.releaseMenus.find(x => x.mapN1Link && String(urlIn).toLowerCase().includes(x.mapN1Link.toLowerCase()));
            CONFIG.LOG(n2url, 'n2 map url');
            if (n2url && !(this.isMobile && n2url.mobileN1Link)) {
              CONFIG.LOG(n2url.routerLink, 'n2 routerlink map url');
              this.router.navigate([n2url.routerLink]);
              return;
            }
            this.url = window.location.hash;
            const index = this.url.lastIndexOf('/');
            this.url = this.url.substr(index + 1);


            CONFIG.LOG(this.url, 'aspxPage');

            for (let i = 0; i < 20; i++) {
              this.url = decodeURIComponent(this.url);
            }



            this.url = this.url.replace('?', '&');
            this.url = this.url.replace('aspx&', 'aspx?');
            CONFIG.LOG(this.url, 'url in nesi1');
            // tslint:disable-next-line:max-line-length
            let iframeURL = this.url;
            if (iframeURL.indexOf('?') !== iframeURL.lastIndexOf('?')) {
              // remove double ? from querystring
              const indexOfFirstQ = iframeURL.indexOf('?');
              const urlFirst = iframeURL.substr(0, indexOfFirstQ);
              let urlEnd = iframeURL.substr(indexOfFirstQ);
              urlEnd = urlEnd.replace('?', '&');
              iframeURL = urlFirst + urlEnd;
            }
            if (urlIn && String(urlIn).indexOf('wo_prog_edit.aspx') >= 0) {
              const tempURL = sessionStorage.getItem('nesi1IframeUrl_V1');
              if (String(tempURL).indexOf('woprog_id=') >= 0) {
                iframeURL = tempURL.replace('&action', '&actions');
              }
            }
            if (iframeURL.indexOf('#/signin') > -1) {
              return;
            }
            this.ts.nesi1IframeUrl = iframeURL;
            this.ts.nesi1IframeUrlParent = '0';
            this.router.navigate(['/home/0/0']);
          } else {
            this.getUrl(id);
          }
        }
      );
    } else {
      this.onloading.emit();
      this.url = this.win.generateNesi1Url(this.direct_url);
      this.url = decodeURIComponent(this.url);
      this.trustedUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
      CONFIG.LOG(this.trustedUrl, 'set iframe url to urlin');
      this.iframeTimeout();
      return;
    }
  }
  ngOnDestroy(): void {
    try {
      clearTimeout(this.timeout);
      this.loaderService.hide();
      this.subIsMobile.unsubscribe()
      this.sub.unsubscribe();
    } catch  {

    }
  }

  ngAfterViewInit(): void {
  }


  onResize() {

    const thisObj: any = document.getElementById('sizeCheckDiv');

    Observable.fromEvent(thisObj, 'resize')
      .debounceTime(100)
      .subscribe((event) => {
        this.onResize();
      });
    const divIframe: HTMLLinkElement = <HTMLLinkElement>document.getElementById('nesi1framediv');
    if (divIframe) {
      divIframe.style.width = '100%';
    }
  }

  getUrl(id: string): void {
    this.ms.getPageUrl(id, this.isMobile).then(
      res => this.handleGetUrlSuccess(<NesiMenuItem>res.json(), Number(id))
    );
  }

  get PageTitle(): string {
    const title: HTMLLinkElement = <HTMLLinkElement>document.getElementById('page_title');
    return (title && title.innerText) || this.pageTitle;
  }

  set PageTitle(value: string) {
    const title: HTMLLinkElement = <HTMLLinkElement>document.getElementById('page_title');
    if (title) {
      title.innerText = value;
    }
  }



  handleGetUrlSuccess(item: NesiMenuItem, id: number) {
    CONFIG.LOG(id, 'id in handle get url success');

    if (item.id && id === 0) {
      const n2url = CONFIG.releaseMenus.find(x => x.id === item.id);
      if (n2url && !(this.isMobile && n2url.mobileN1Link)) {
        this.router.navigate([n2url.routerLink]);
        return;
      }
    }

    if ((item.type === NesiMenuType.Nesi1 || item.type === NesiMenuType.RouterLink) && item.url.toLowerCase().indexOf('.aspx') < 0) {
      this.router.navigate([item.url]);
      return;
    }



    CONFIG.LOG('topshow', 'handle geturl success');
    this.loaderService.topShow();
    this.loading = true;

    const sessionUrl = this.ts.nesi1IframeUrl;
    const parentId = this.ts.nesi1IframeUrlParent;
    CONFIG.LOG(item.url, 'handleGetURLSuccess item.url');
    if (item.type === NesiMenuType.RouterLink) {
      this.router.navigate([item.url]);
      return;
    }
    if (parentId && sessionUrl && this.pageId === parentId) {
      item.url = sessionUrl;
      item.type = NesiMenuType.Nesi1;
    } else if (!sessionUrl && item.url) {
      CONFIG.LOG(item.url, 'handleGetURLSuccess null sessionURL');
      this.ts.nesi1IframeUrl = item.url;
      item.url = this.ts.nesi1IframeUrl;
    }

    if (window.location.hostname.indexOf('localhost') < 0) {
      const div: HTMLLinkElement = <HTMLLinkElement>document.getElementById('nesi1framediv');
      const h = '1200px';
      const w = '100%';
      if (div) {
        div.style.height = h;
        div.style.width = w;
      }
    }

    if (item.type === NesiMenuType.Nesi1 || item.type === NesiMenuType.BlankPage) {

      this.url = this.win.generateNesi1Url(item.url);
      this.url = decodeURIComponent(this.url);
      this.trustedUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
      CONFIG.LOG(this.trustedUrl, 'set iframe url to handleGetUrlSuccess');
      this.iframeTimeout();
    } else {
      this.router.navigate([item.url]);
      this.loading = false;
    }
    if (item) {
      this.store.dispatch(new fromCurrentUser.SetCurrentPageId(item.id));
    }

    try {
      const doc = this.iframe.nativeElement.contentDocument;
      document.title = doc.title;
    } catch { }
  }

  iframeTimeout() {
    if (!this.direct_url) {
      this.pageTitle = 'Spark Ops';
      this.PageTitle = this.pageTitle;
    }
    this.timeout = setTimeout(() => {
      // CONFIG.LOG('setTimeout topshow', 'handle geturl success');
      // CONFIG.LOG(this.PageTitle, 'iframe title in settimeout nesi1');
      if (this.PageTitle === this.pageTitle) {
        this.loaderService.topShow();
        if (!HostContains('localhost')) {
          this.interval = window.setInterval(() => {
            CONFIG.LOG(this.PageTitle, 'setinterval page title in nesi 1');
            if (this.PageTitle !== this.pageTitle) {
              CONFIG.LOG('clear interval', 'setinterval in nesi 1');
              this.loaderService.hide();
              this.loading = false;
              if (this.interval) {
                window.clearInterval(this.interval);
                this.interval = null;
              }
            }
          }, 500);
        }
      }
    }, 500);
  }

  onload(event: any, obj: any): void {

    CONFIG.LOG('iframe loaded', 'nesi 1 iframe onload');

    // this.store.dispatch(new fromMessage.PushInfoMessage('Iframe is Loaded.'));
    if (this.trustedUrl) {
      // CONFIG.LOG(this.PageTitle, 'Page title in nesi 1 iframe onload');
      // CONFIG.LOG(this.pageTitle, 'page title in nesi 1 iframe onload');
      if (! this.direct_url && this.PageTitle && this.PageTitle !== this.pageTitle) {
        CONFIG.LOG('clear timeout', 'nesi 1 iframe onload');
        if (this.timeout) {
          clearTimeout(this.timeout);
          this.timeout = null;
        }
        if (this.interval) {
          clearInterval(this.interval);
          this.interval = null;
        }
      }
      this.loaderService.hide();
      this.loading = false;
      this.onloaded.emit();
    }
    try {
      const iframeurl = obj.contentWindow.location.href;
      this.ts.nesi1IframeUrlParent = this.pageId || '0';
      this.ts.nesi1IframeUrl = iframeurl;
    } catch { }
  }

  messageReceiver(event: any): void {
    if (window.location.hostname.indexOf('localhost') < 0) {
      if (event.data.iframeHeight) {
        //  const div: HTMLLinkElement = <HTMLLinkElement>document.getElementById('nesi1framediv');

        let h = event.data.iframeHeight + 'px';
        if (this.isMobile) {
          h = (event.data.iframeHeight + 300) + 'px';
        }
        const w = event.data.iframeWidth + 'px';
        // console.log(h);
        // console.log(w);
        // if (!div) { return; }
        // div.style.height = h;
        // div.style.width = w;
        const t: HTMLLinkElement = <HTMLLinkElement>document.getElementById('page_title');
        if (t.innerText !== event.data.title) {
          // console.log(event.data.title, 'get title from iframe');
          t.innerText = event.data.title;
          if (this.timeout) {
            clearTimeout(this.timeout);
            this.timeout = null;
          }
          if (this.interval) {
            clearInterval(this.interval);
            this.interval = null;
          }
        }
      }
    }
  }
}
