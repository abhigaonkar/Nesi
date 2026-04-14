import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { CONFIG } from '../../../../configuration';

@Component({
  selector: 'nesi-workorder-main-buttons-menu',
  templateUrl: './workorder-main-buttons-menu.component.html',
  styleUrls: ['./workorder-main-buttons-menu.component.css']
})
export class WorkorderMainButtonsMenuComponent implements OnInit {

  public items: MenuItem[];

  @Input() set buttons(value: any) {
    if (value) {
      this.populate_menu_items(value);
    }
  }
  @Output() button_click = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  populate_menu_items(_buttons: any) {
    this.items = [];
    this.items.push(this.setButton(_buttons.button_back));
    const edit_items: MenuItem[] = [];
    edit_items.push(this.setButton(_buttons.button_invoicePreview));
    edit_items.push(this.setButton(_buttons.button_print));
    edit_items.push({ separator: true });
    edit_items.push(this.setButton(_buttons.button_saveGeneral));
    edit_items.push(this.setButton(_buttons.button_delete));
    if (edit_items.filter(x => x && x.visible).length > 0) {
      const edit_button = {
        label: 'Edit',
        icon: 'fa-pencil',
        items: edit_items
      }
      this.items.push(edit_button);
    }

    const sendto_items = [];
    sendto_items.push(this.setButton(_buttons.button_send.button_sendToWaitCustPO));
    sendto_items.push(this.setButton(_buttons.button_send.button_sendPMForQuestions));
    sendto_items.push(this.setButton(_buttons.button_send.button_sendToPMApproval));
    sendto_items.push(this.setButton(_buttons.button_send.button_sendToRework));
    sendto_items.push(this.setButton(_buttons.button_send.button_sendToWaitingToBeInvoiced));
    if (sendto_items.filter(x => x && x.visible).length > 0) {
      const sendto_button = {
        label: 'Send To',
        icon: 'fa-arrows',
        items: sendto_items
      }
      this.items.push(sendto_button);
    }

    this.items.push(this.setButton(_buttons.button_approvedByPM));
    this.items.push(this.setButton(_buttons.button_signOff));
  }


  setButton(button: MenuItem): MenuItem {
    button.command = (e) => {
      this.button_click.emit(e);
    }
    return button;
  }

}
