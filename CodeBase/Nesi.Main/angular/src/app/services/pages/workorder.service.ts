import { Injectable } from '@angular/core';
import { TokenService } from 'app/services/authentication/tokenService';
import { Router } from '@angular/router';
import { WindowRef } from 'app/services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from '../../configuration';


@Injectable()
export class WorkOrderService {

    public is_n2 = true; // CONFIG.ISDEV() || CONFIG.ISDEBUG();
    public is_usingOldWorkorders=true;

    constructor(
        private ts: TokenService,
        private router: Router,
        private winRef: WindowRef,
        private cs: CoreService,
    ) { }


    public open_wo_file(file: string, bu: string, event = null) {
        let url = '';
        if (this.is_n2) {
            url = '/home/12/workorder/scanned_file/' + bu + '/' + file.substr(0, file.length - 4);
        } else {
            url = `/wo_prog_frame.aspx?action=justscan&scanfile=${file}&business_unit_id=${bu}&navpanes=0`;
        }
        this.open_with_event(url, 'wofile_' + bu.toString() + '_' + file.toString(), '12', '/home/1/12/default', event);
    }


    public open_with_event(url: string, title: string, selfid: string, self: string, event = null) {
        if (!url) { return; }
        if (event) {
            if (event.ctrlKey || event.button === 1) {
                this.winRef.boingNesi1(url, title, '', true);
            } else if (event.shiftKey) {
                this.winRef.boingNesi1(url, title, '', false);
            } else {
                if (url.toLowerCase().includes('.aspx')) {
                    if (url.startsWith('[B]')) {
                        url = url.substr(3);
                        this.winRef.boingNesi1(url, title, '', false);
                    } else {
                        this.ts.nesi1IframeUrlParent = selfid;
                        this.ts.nesi1IframeUrl = url;
                        this.router.navigate([self]);
                    }
                } else {
                    this.router.navigate([url]);
                }
            }
        } else {
            if (url.toLowerCase().includes('.aspx')) {
                if (url.startsWith('[B]')) {
                    url = url.substr(3);
                    this.winRef.boingNesi1(url, title, '', false);
                } else {
                    this.ts.nesi1IframeUrlParent = selfid;
                    this.ts.nesi1IframeUrl = url;
                    this.router.navigate([self]);
                }
            } else {
                this.router.navigate([url]);
            }
        }
    }
    public getUrl(type: string, buId: number, from: string, id) {
        let url = '';
        const bu: string = buId.toString();
        if (from === 'WorkOrder') {
            switch (type) {
                case 'add':
                    if (this.cs.isSm) {
                        url = '/sections/workorder/mobile_wo/index.aspx?id=0&is_mobile=1&business_unit_id=' + bu;
                    } else {
                        url = '/sections/workorder/index.aspx?woprog_id=0&business_unit_id=' + bu;
                    }
                    break;
                case 'businessUnit_name':
                    if (this.is_n2) {
                        url = '/home/12/workorder/buckets/' + bu;
                    } else {
                        url = '/wo_prog_edit.aspx?business_unit_id=' + bu;
                    }
                    break;
                case 'open':
                    if (this.is_n2) {
                        url = '/home/51/reports/77/master_workorder/' + bu + '/Open';
                    } else {
                        url = '/sections/reports/master_workorders/index.aspx?status=Open&business_unit_id=' + bu;
                    }
                    break;
                case 'being_processed':
                    if (this.is_n2) {
                        url = '/home/51/reports/77/master_workorder/' + bu + '/In_Progress';
                    } else {
                        url = '/sections/reports/master_workorders/index.aspx?status=In%20Progress&business_unit_id=' + bu;
                    }
                    break;
                case 'rework':
                    if (this.is_n2) {
                        url = '/home/51/reports/77/master_workorder/' + bu + '/Rework';
                    } else {
                        url = '/sections/reports/master_workorders/index.aspx?status=Rework&business_unit_id=' + bu;
                    }
                    break;
                case 'questions':
                    if (this.is_n2) {
                        url = '/home/51/reports/77/master_workorder/' + bu + '/Questions_For_PM';
                    } else {
                        url = '/sections/reports/master_workorders/index.aspx?status=Questions%20For%20PM&business_unit_id=' + bu;
                    }
                    break;
                case 'pm_approval':
                    if (this.is_n2) {
                        url = '/home/51/reports/77/master_workorder/' + bu + '/Waiting_PM_Approval';
                    } else {
                        url = '/sections/reports/master_workorders/index.aspx?status=Waiting%20PM%20Approval&business_unit_id=' + bu;
                    }
                    break;
                case 'bm_approval':
                    if (this.is_n2) {
                        url = '/home/51/reports/77/master_workorder/' + bu + '/Waiting_BM_Approval';
                    } else {
                        url = '/sections/reports/master_workorders/index.aspx?status=Waiting%20BM%20Approval&business_unit_id=' + bu;
                    }
                    break;
                case 'id':
                    if (this.is_n2 && !this.is_usingOldWorkorders ) {
                        url = '/home/12/workorder/edit/' + bu + '/' + id;
                    } else if (this.cs.isSm) {
                        url = '/sections/workorder/mobile_wo/index.aspx?id=' + id + '&is_mobile=1';
                    } else {
                        url='/wo_prog_frame.aspx?action=show&woprog_id=' + id + '&business_unit_id=' + bu + '&is_n1=true';
                        //url = '/sections/workorder/index.aspx?woprog_id=' + id + '&is_n1=true&business_unit_id=' + bu + '&fromwo=';
                    }
                    break;
                case 'id_file':
                    url = '/home/12/workorder/edit_withfile/' + bu + '/' + id;
                    break;
                case 'preview':
                    url = `[B]/sections/reports/invoice_preview/frame.aspx?id=${id}&is_child_approval=true&is_signoff=false`;
                    break;
                default:
                    break;
            }
        } else if (from === 'PurchaseOrder') {
            switch (type) {
                case 'add':
                    url = '/sections/purchaseorder/po_prog_add.aspx?action=add&woprog_id=0&poprogid=0&business_unit_id=' + bu;
                    break;
                case 'businessUnit_name':
                    url = '/sections/purchaseorder/po_prog_edit.aspx?business_unit_id=' + bu;
                    break;
            }
        }
        return url;
    }

    public openUrl(type: string, buId: number, from: string, id = 0, event = null) {
        const url = this.getUrl(type, buId, from, id);
        const title = from.toLowerCase() + '_' + buId.toString() + '_' + id.toString();
        if (from === 'WorkOrder') {
            this.open_with_event(url, title, '12', '/home/1/12/default', event);
        } else if (from === 'PurchaseOrder') {
            this.open_with_event(url, title, '92', '/home/1/92/default', event);
        }
    }

    public boingUrl(type: string, buId: number, from: string, id = 0) {
        const url = this.getUrl(type, buId, from, id);
        if (url) {
            this.winRef.boingNesi1(url, from.toLowerCase() + '_' + buId.toString() + '_' + id.toString(), '_blank');
        }
    }
}
