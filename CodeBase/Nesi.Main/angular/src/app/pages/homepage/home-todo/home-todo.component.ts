import { Component, OnInit } from '@angular/core';
import { WindowRef } from 'app/services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CONFIG } from 'app/configuration';
import { HomePageBase } from 'app/pages/homepage/home-base';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'nesi-home-todo',
  templateUrl: './home-todo.component.html',
  styleUrls: ['./home-todo.component.css']
})
export class HomeTodoComponent extends HomePageBase implements OnInit {

  public selected_item: any;
  public display = false;
  public submitting = true;
  public n2_type: string;
  public is_employee_termination = false;
  public is_employee_userinfo = false;
  public is_employee_offer = false;
  public is_employee_it = false;
  public is_quote = false;
  public is_customer = false;
  public n2_params: string[];

  public employee: any;
  public customer_id: number;
  public qutoe_id: number;
  public revision: number;

  public offerToPass: any = {};

  constructor(
    private winRef: WindowRef,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    public title: Title
  ) {
    super(cs, store);
  }

  get Url(): string {
    return CONFIG.apiURL.page.homepage.todo;
  }

  openTodo(event: any) {
    // const item = event.data;
    this.selected_item = event.data;
    if (this.selected_item.link.includes('redir.aspx')) {
      this.winRef.boingNesi1(this.selected_item.link, this.selected_item.link2);
      return;
    }

    let n2_page = this.selected_item.n2_page;
    if (n2_page) {
      const n2_params = this.selected_item.n2_params.split(',');
      this.submitting = false;
      this.is_employee_termination = false;
      this.is_employee_userinfo = false;
      this.is_employee_offer = false;
      this.is_employee_it = false;
      this.is_quote = false;
      this.is_customer = false;
      n2_page = String(n2_page);
      this.selected_item.link = '/#/opens/' + n2_page;
      if (n2_page.includes('127/employees')) {
        this.employee = { memberid: n2_params[0] };
        if (n2_page.includes('userinfo')) {
          this.is_employee_userinfo = true;
        }
        if (n2_page.includes('termination')) {
          this.is_employee_termination = true;
        }
        if (n2_page.includes('it')) {
          this.is_employee_it = true;
        }
        if (n2_page.includes('offer')) {
          this.is_employee_offer = true;
          this.employee.id = n2_params[1];
          this.loadOffer_ForBugFix_1808();
        }
      }
      if (n2_page.includes('137/applicants')) {
        this.employee = { applicant_id: n2_params[0], is_applicant: true };
        if (n2_page.includes('offer')) {
          this.is_employee_offer = true;
          this.employee.id = n2_params[1];
        }
      }
      if (n2_page.includes('10/customers')) {
        this.customer_id = +n2_params[0];
        this.is_customer = true;
      }
      if (n2_page.includes('65/quotes')) {
        this.qutoe_id = +n2_params[0];
        this.revision = +n2_params[1];
        this.is_quote = true;
      }
    }

    if (n2_page.includes('offer')) {
      //
      // For bug 1808: Employee offer link freezes Spark Ops
      // Need to load offer first, so delay to show in here until getting offer info ....
      //
      return;
    }

    this.display = true;
    // this.winRef.boingNesi1(item.link, 'todo_' + item.link2);
  }

  Refresh() {
    super.Refresh();
    this.title.setTitle('Home Page');
  }

  //
  // Case by case handle: for offer stuff.
  //
  private loadOffer_ForBugFix_1808() {
    let o: string = CONFIG.apiURL.page.employee.employment.List + this.employee.memberid;
    this.cs.getList(o).subscribe(
      (res) => {
        //
        // start to run
        //

        if(!res) {
          // If manually deleted.
          super.PushErrorMessage('No offer list was found.');
          return;
        }

        let found = false;
        for(let i = 0; i< res.length; i++) {
          if( (<any>(res[i])).id == this.employee.id) {
            this.offerToPass = res[i];
            this.display = true;
            found = true;
            break;
          }
        }

        if(!found) {
          // If manually deleted.
          super.PushErrorMessage('No offer was found.');
        }

        //
        // end of running
        //
      },

      // not okay
      (err: any) => {
        super.PushErrorMessage(err);
      }
    );
  }

}
