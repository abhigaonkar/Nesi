import { Component, OnInit, Inject, forwardRef, OnDestroy, Input } from '@angular/core';
import { Router } from '@angular/router';
import { HomeComponent } from '../../pages/home/home.component';
import { TokenService } from '../../services/authentication/tokenService';
import { OnlineUser } from '../../models/authentication/onlineUser';
import { BannerService } from '../../services/layout/banner.service';
import { MenuService } from '../../services/layout/menuService';
import * as fromRoot from '../../reducers';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import * as fromNesiMenu from '../../actions/layout/nesiMenu';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import { Fvr } from '../../models/layout/fvr';
import { Subscription } from 'rxjs/Subscription';
import { CONFIG } from '../../configuration';
import { ToDo } from '../../models/layout/todo';
import { Message } from '../../models/banner/message';
import { Ticket } from '../../models/layout/ticket';
import { WindowRef } from '../../services/shared/windowRef';
import { SwitchUser } from '../../models/banner/switchUser';
import { QuickExtension } from '../../models/layout/quickExtension';
import { WhoDoIAsk } from '../../models/layout/whoDoIAsk';
import { UpcomingVacations } from '../../models/layout/upcomingVacations';
import { AverageDaysToInvoice } from '../../models/layout/averageDaysToInvoice';
import { AuthorizeService } from '../../services/authentication/authorize.Service';
import { DeviceService } from '../../services/authentication/device';
import { CoreService } from '../../services/shared/core.service';
import { MessageBase } from '../../core/messageBaseComponent';
import { getRebootTime } from '../../reducers/layout/currentUser';
import { SignalRService } from '../../services/authentication/signalR.service';
import * as fromMessage from '../../actions/layout/growlMessage';
import { LoadTODOList } from '../../actions/layout/currentUser';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CookieService } from 'ng2-cookies';
import { config } from '../../../../node_modules/rxjs';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrls: ['./banner.component.css']
})
export class BannerComponent extends MessageBase implements OnInit, OnDestroy {
  @Input()
  showAll = false;

  public visible = true;
  models: any[];
  bannerItems: any[];
  displayQEView = false;
  displayActiveUsers = false;
  displayChangePassword = false;
  displayWaitingForMeTickets = false;
  displayFvrs = false;
  displayVacations = false;
  displayWhoDoIAsk = false;
  displayQuickExtensions = false;
  displayUpcomingVacations = false;
  allOnlineUsers$: Observable<OnlineUser[]>;
  allOnlineUsers: OnlineUser[];
  ticketsWaitingForMe: OnlineUser[];
  todo$: Observable<ToDo[]>;
  message$: Observable<Message[]>;
  messageList: Message[];
  ticket$: Observable<Ticket[]>;
  ticketList: Ticket[];
  quickExtensionsList: QuickExtension[];
  whoDoIAskList: WhoDoIAsk[];
  upcomingVacationsList: UpcomingVacations[];
  averageDaysToInvoice: AverageDaysToInvoice;
  alternateLogo : string;
  toggleMessageIcon: boolean;
  toggleTasksIcon: boolean;
  toggleTicketIcon: boolean;
  toggleOnlineUserIcon: boolean;
  showHelpWiki = true;

  todoList: ToDo[];
  onlineUserCount: number;
  fvrs$: Observable<Fvr[]>;
  fvrs: Fvr[];
  user: OnlineUser;
  logo: string;
  subOnlineUser: Subscription;
  subToDo: Subscription;
  subMessage: Subscription;
  subFvr: Subscription;
  subTicket: Subscription;
  pageId: number;

  appliedRelease = false;

  isContact = false;
  currentUserId: number;
  switchUserList: LabelValueInt[];
  switchUserLoaded = false;

  ToggleMessageIcon$: Observable<string>;
  ToggleTasksIcon$: Observable<string>;
  ToggleTicketIcon$: Observable<string>;
  ToggleOnlineUserIcon$: Observable<string>;
  PageId$: Observable<number>;
  force_beta = false;

  rebootTime = 0;


  private subscriptionTOU: Subscription;
  private subscriptionTTicket: Subscription;
  private subscriptionTMessage: Subscription;
  private subscriptionTtask: Subscription;
  private subscriptionPageId: Subscription;

  constructor(
    public bs: BannerService,
    public as: AuthorizeService,
    public ts: TokenService,
    private router: Router,
    protected store: Store<fromRoot.State>,
    private winRef: WindowRef,
    private device: DeviceService,
    public cs: CoreService,
    private ms: MenuService,
    private signalr: SignalRService,
    private cookie: CookieService,

    @Inject(forwardRef(() => HomeComponent)) public app: HomeComponent
  ) {
    super(store);
    // tslint:disable-next-line:triple-equals
    this.isContact = this.ts.isContact;
    //  console.log(this.isContact);
    if (!this.isContact) {
      this.allOnlineUsers$ = this.store.select(fromRoot.getCurrentuser.getActiveUserList);
      this.fvrs$ = this.store.select(fromRoot.getCurrentuser.getFVRList);
      this.todo$ = this.store.select(fromRoot.getCurrentuser.getTODOList);
      this.message$ = this.store.select(fromRoot.getCurrentuser.getMessageList);
      this.ticket$ = this.store.select(fromRoot.getCurrentuser.getTicketList);
      this.ToggleOnlineUserIcon$ = store.select(fromRoot.getLayoutProfiles.ToggleOnlineUserIcon);
      this.ToggleTasksIcon$ = store.select(fromRoot.getLayoutProfiles.ToggleTasksIcon);
      this.ToggleTicketIcon$ = store.select(fromRoot.getLayoutProfiles.ToggleTicketIcon);
      this.ToggleMessageIcon$ = store.select(fromRoot.getLayoutProfiles.ToggleMessageIcon);
      this.PageId$ = store.select(fromRoot.getCurrentuser.getCurrentPageId);
    }

    this.PageId$ = store.select(fromRoot.getCurrentuser.getCurrentPageId);
  }

  doSearch(searchText: string) {
    this.searchMenu(searchText);
  }

  searchMenu(searchText: string) {
    this.store.dispatch(new fromNesiMenu.SearchMenu(searchText));
  }

  switchUser(event: any) {
    // console.log(event);
    const memberId = event.selectedId;
    this.bs.SwtichToUser(memberId).subscribe(
      (payload: boolean) => {
        if (payload) {
          this.cookie.set('ismasq', '1');
          this.reSignIn();
        }
      });
  }

  cancelSwitchUser(event: any) {
    this.bs.CancelSwitchToUser().subscribe(
      (payload: boolean) => {
        if (payload) {
          this.cookie.set('ismasq', '0');
          this.reSignIn();
        }
      });
  }

  private reSignIn() {
    this.as.validateToken().subscribe((res) => {
      this.ts.handleOnlineUser(res);
      this.ts.clearSessionStorage();
      localStorage.setItem(CONFIG.authentication.signOutCheck, '1');
      this.router.navigate(['/signin']);
    });
  }

  ngOnInit() {
    this.logo = this.ts.currentUser.logo == 'spc-ne-logo.png'? 'spark_Logo.png':this.ts.currentUser.logo;
    CONFIG.LOG(this.ts.currentUser.logo);
    this.currentUserId = this.ts.currentUser.id;
    CONFIG.LOG(this.ts.currentUser.fullName, 'banner init');
    CONFIG.LOG(this.ts.currentUser.companyName, 'banner init');
    this.getRebootTime();

    if (!this.isContact) {
      this.bs.reload();
      this.force_beta = CONFIG.ISBETA() || CONFIG.ISDEV();
      this.subscriptionPageId = this.PageId$
        .subscribe((value: number) => this.pageId = value);

      // setInterval(() => this.bs.reload(), 5 * 60 * 1000);
      this.subscriptionTOU = this.ToggleOnlineUserIcon$
        .subscribe((value: string) => {
          this.toggleOnlineUserIcon = value === '1';
        });

      this.subscriptionTTicket = this.ToggleTicketIcon$
        .subscribe((value: string) => {
          this.toggleTicketIcon = value === '1';
        });
      this.subscriptionTMessage = this.ToggleMessageIcon$
        .subscribe((value: string) => {
          this.toggleMessageIcon = value === '1';
        });
      this.subscriptionTtask = this.ToggleTasksIcon$
        .subscribe((value: string) => {
          this.toggleTasksIcon = value === '1';
        });
      this.subOnlineUser = this.allOnlineUsers$.subscribe(res => {
        this.allOnlineUsers = res;
        this.onlineUserCount = this.allOnlineUsers.length;
      });
      /*TODO come from webAPI   */
      this.subToDo = this.todo$.subscribe(res => {
        // console.log()
        this.todoList = res;
      });
      this.subMessage = this.message$.subscribe(res => {
        //  console.log()
        this.messageList = res;
      });
      this.subFvr = this.fvrs$.subscribe(res => {
        //  console.log()
        this.fvrs = res;
      });
      this.subTicket = this.ticket$.subscribe(res => {
        //  console.log()
        this.ticketList = res;
      });
      this.bs.loadSwtichUserList()
        .subscribe((res: LabelValueInt[]) => {
          this.switchUserList = res;
          this.switchUserLoaded = true;
        });
      this.bs.loadQuickExtensions().subscribe((res: QuickExtension[]) => this.quickExtensionsList = res);
      this.bs.loadWhoDoIAsk().subscribe((res: WhoDoIAsk[]) => this.whoDoIAskList = res);
      this.bs.loadUpcomingVacations().subscribe((res: UpcomingVacations[]) => this.upcomingVacationsList = res);
      this.bs.loadAverageDaysToInvoice().subscribe((res: AverageDaysToInvoice) => {
        this.averageDaysToInvoice = res;
        //   console.log(this.averageDaysToInvoice);
      });

    } else {
      this.switchUserLoaded = true;
    }

    if (this.pageId === 0) {
      this.showHelpWiki = false;
    }
  }
  ngOnDestroy(): void {
    if (!this.isContact) {
      this.subOnlineUser.unsubscribe();
      this.subToDo.unsubscribe();
      this.subMessage.unsubscribe();
      this.subFvr.unsubscribe();
      this.subTicket.unsubscribe();
      this.subscriptionTOU.unsubscribe();
      this.subscriptionTtask.unsubscribe();
      this.subscriptionTTicket.unsubscribe();
      this.subscriptionTMessage.unsubscribe();
      this.subscriptionPageId.unsubscribe();
    }
  }

  userClick($event): void {

    if (!this.isContact) {
      this.app.onTopbarMenuButtonClick($event);
    } else {
      this.winRef.boingNesi1(CONFIG.Nesi1URL.contact.contactDisplay + this.ts.currentUser.id, 'topmenu', null);
    }
  }

  signOut(): void {
    this.router.navigate(['/signout']);
  }


  changePassword(): void {
    this.displayChangePassword = true;
    this.app.hideTopbarMenu();

  }


  showActiveUsers(): void {
    this.displayActiveUsers = true;
    this.app.hideTopbarMenu();

  }

  onTasksIconMouseOver() {
    CONFIG.LOG('mouseover', 'tasksIcon banner');
    this.store.dispatch(new fromCurrentUser.LoadTODOList());
  }
  onOnlineUserMouseOver() {
    CONFIG.LOG('mouseover', 'onlineuser banner');
    this.store.dispatch(new fromCurrentUser.LoadActiveUserList());
  }
  showTickets(): void {
    this.winRef.boingNesi1(CONFIG.Nesi1URL.Tickets, 'tickets', null);
    // this.router.navigate(['/home/1/170']);
  }
  showsetDefaultPage(): void {
    CONFIG.LOG(this.pageId, 'setting default page to');

    if (this.pageId) {

      this.bs.saveDefaultPage(this.pageId).subscribe((response) => {
        this.store.dispatch(new fromMessage.PushSuccessMessage('Default page set to ' + response));
      });
    }
  }


  showTicketsList(): void {
    this.store.dispatch(new fromCurrentUser.LoadTicketList());
    if (this.device.isMobile) {
      this.router.navigate([CONFIG.Nesi1URL.TicketsMobile]);
    } else {
      this.winRef.boingNesi1(CONFIG.Nesi1URL.TicketsList, 'ticketlist', null);
    }

  }

  showToDo(): void {
    this.store.dispatch(new fromCurrentUser.LoadTODOList());
    this.router.navigate(['/home/1/1']);
  }

  showMessage(): void {
    this.store.dispatch(new fromCurrentUser.LoadMESSAGEList());
    this.router.navigate(['/home/1/29']);
  }
  get userName(): string {
    // CONFIG.LOG(this.ts.currentUser.name, 'banner.getusername');
    return this.ts.currentUser == null || this.ts.currentUser.name == null ? '' : this.ts.currentUser.name;
  }
  get userPhoto(): string {
    return this.ts.currentUser == null || this.ts.currentUser.photo == null ? 'assets/images/noimage/men.jpg' : this.ts.currentUser.photo;
  }

  showEditQuickExtensions() {
    this.winRef.boingNesi1(CONFIG.Nesi1URL.QuickExtension, 'quickextension', null);
  }
  showWikiHelp() {

    CONFIG.LOG(this.pageId, 'helpWiki PageID');
    if (this.pageId !== 0) {
      this.winRef.boingNesi1(CONFIG.Nesi1URL.WikiHelp + '&page_id=' + this.pageId, 'wiki', null);
    }
  }
  showQuickExtensions() {
    this.bs.loadQuickExtensions().subscribe((res: QuickExtension[]) => this.quickExtensionsList = res);
  }

  get displayMessageIcon() {
    return window.innerWidth > 380;
  }


  getRebootTime() {
    const host = window.location.hostname.replace(/\./g, '');
    this.cs.getData<number>(CONFIG.apiURL.currentUser.rebootTime + host)
      .subscribe(
      (res: number) => {
        if (!res) { res = 0; }
        CONFIG.LOG(res, 'reboot time in getreboottime in banner');
        if (res < 0) {
          if (!this.ms.checkMenuPrivilege(166) || this.pageId !== 166) {
            this.router.navigate(['/signout']);
          } else {
            this.store.dispatch(new fromCurrentUser.SetRebootTime(-res));
          }
          // this.rebootTime = 0;
        } else if (res > 0) {
          this.rebootTime = res;
          this.store.dispatch(new fromCurrentUser.SetRebootTime(this.rebootTime));
        }
      }
      );
  }

  applyRelease() {
    CONFIG.LOG('apply release', 'banner');
    this.getRebootTime();
    this.appliedRelease = true;
  }

  rebootEnd() {
    CONFIG.LOG(this.pageId, 'pageid in reboot end in banner');
    const releaseUserId = this.signalr.releaseUserId;
    if (!this.appliedRelease && releaseUserId && releaseUserId === this.ts.currentUser.id
      && this.ms.checkMenuPrivilege(166) && this.pageId === 166) {
      window.setTimeout(() => {
        this.applyRelease();
      }, 5000);
    } else if (this.appliedRelease || !this.ms.checkMenuPrivilege(166) || this.pageId !== 166) {
      this.router.navigate(['/signout']);
    } else {
      this.getRebootTime();
    }
  }


  get widthIsMobile() {
    return window.innerWidth < 800;
  }
}
