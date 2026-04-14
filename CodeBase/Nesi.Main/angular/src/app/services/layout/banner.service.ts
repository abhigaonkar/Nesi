import { Injectable, OnInit } from '@angular/core';
import { Response } from '@angular/http';
import { CONFIG } from '../../configuration';
import { HttpService } from '../../core/http.service';
import { Observable } from 'rxjs/Observable';
import { OnlineUser } from '../../models/authentication/onlineUser';
import { Fvr } from '../../models/layout/fvr';
import { ToDo } from '../../models/layout/todo';
import { Message } from '../../models/banner/message';
import { ServiceBase } from '../shared/serviceBase';
import { Ticket } from '../../models/layout/ticket';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromNesiMenu from '../../actions/layout/nesiMenu';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import { SwitchUser } from '../../models/banner/switchUser';
import { QuickExtension } from '../../models/layout/quickExtension';
import { WhoDoIAsk } from '../../models/layout/whoDoIAsk';
import { UpcomingVacations } from '../../models/layout/upcomingVacations';
import { AverageDaysToInvoice } from '../../models/layout/averageDaysToInvoice';
import { DashMessage } from '../../models/layout/dashMessage';
import { LabelValueInt } from 'app/models/Shared/labelValueString';

@Injectable()
export class BannerService extends ServiceBase implements OnInit {

  constructor(protected http: HttpService,
    private store: Store<fromRoot.State>,

  ) {
    super(http);
  }

  ngOnInit(): void {

  }

  reload() {

    this.store.dispatch(new fromNesiMenu.LoadMenu());
    this.store.dispatch(new fromNesiMenu.LoadMobileMenu());
    this.store.dispatch(new fromCurrentUser.LoadActiveUserList());
    this.store.dispatch(new fromCurrentUser.LoadFVRList());
    this.store.dispatch(new fromCurrentUser.LoadTODOList());
    this.store.dispatch(new fromCurrentUser.LoadMESSAGEList());
    this.store.dispatch(new fromCurrentUser.LoadTicketList());
  }

  private getActiveUsers(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.allActiveUsers);
  }



  private getSwitchUserList(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.switchUserList);
  }

  private switchToUser(id: number): Observable<Response> {
    return this.http.put(CONFIG.apiURL.layout.switchUser + id.toString(), null);
  }

  private cancelSwitchUser(): Observable<Response> {
    return this.http.delete(CONFIG.apiURL.layout.switchUser);
  }


  private getFVRs(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.fvrs);
  }

  private getTODO(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.toDo);
  }

  private getMessage(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.message);
  }

  private getTickets(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.tickets);
  }
  private getQuickExtensions(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.quickExtensions);
  }
  private getWhoDoIAsk(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.whoDoIAsk);
  }
  private getUpcomingVacations(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.ucomingVacations);
  }
  private getAverageDaysToInvoice(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.averageDaysToInvoice);
  }
  private getDashMessage(id: number): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.dashMessage + id.toString());
  }
  private postDashMessage(body: DashMessage): Observable<Response> {
    return this.http.post(CONFIG.apiURL.layout.dashMessage, body);
  }
  private getDefaultPage(id: number): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.layout.defaultPage + 'Set/' + id.toString());
  }
  saveDefaultPage(id: number): Observable<boolean> {
    return this.ConvertData(this.getDefaultPage(id));
  }

  saveDashMessage(body: DashMessage): Observable<boolean> {
    return this.ConvertData(this.postDashMessage(body));
  }
  loadDashMessageList(id: number): Observable<DashMessage[]> {
    return this.ConvertList(this.getDashMessage(id));
  }

  loadSwtichUserList(): Observable<LabelValueInt[]> {
    return this.ConvertList(this.getSwitchUserList());
  }

  SwtichToUser(id: number): Observable<boolean> {
    return this.ConvertData(this.switchToUser(id));
  }

  CancelSwitchToUser(): Observable<boolean> {
    return this.ConvertData(this.cancelSwitchUser());
  }
  loadActiveUserList(): Observable<OnlineUser[]> {
    return this.ConvertList(this.getActiveUsers());
  }

  loadFVRList(): Observable<Fvr[]> {
    return this.ConvertList<Fvr>(this.getFVRs());
  }

  loadTODOList(): Observable<ToDo[]> {
    return this.ConvertList<ToDo>(this.getTODO());
  }

  loadMESSAGEList(): Observable<Message[]> {
    return this.ConvertList<Message>(this.getMessage());
  }

  loadTicketList(): Observable<Ticket[]> {
    return this.ConvertList<Ticket>(this.getTickets());
  }
  loadQuickExtensions(): Observable<QuickExtension[]> {
    return this.ConvertList<QuickExtension>(this.getQuickExtensions());
  }
  loadWhoDoIAsk(): Observable<WhoDoIAsk[]> {
    return this.ConvertList<WhoDoIAsk>(this.getWhoDoIAsk());
  }
  loadUpcomingVacations(): Observable<UpcomingVacations[]> {
    return this.ConvertList<UpcomingVacations>(this.getUpcomingVacations());
  }
  loadAverageDaysToInvoice(): Observable<AverageDaysToInvoice> {
    return this.ConvertObject<AverageDaysToInvoice>(this.getAverageDaysToInvoice());
  }
}
