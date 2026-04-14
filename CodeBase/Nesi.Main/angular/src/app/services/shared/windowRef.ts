import { Injectable } from '@angular/core';
import { CONFIG } from '../../configuration';
import { TokenService } from '../authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';
@Injectable()
export class WindowRef {
  // encryptString: string;

  constructor(
    private ts: TokenService,
    private cs: CoreService,
  ) {


  }


  public invokeSignIn() {
    CONFIG.LOG('Sign-in call noop');
    // if (CONFIG.ISDEBUG()) {
    //   return;
    // }
    // try {
    //   this.cs.superGet(this.generateNesi1Url(CONFIG.Nesi1URL.signIn)).subscribe(
    //     (res) => {
    //       CONFIG.LOG(res, 'signIn call');
    //     }
    //   );
    // } catch (e) {
    //   CONFIG.LOG(e);
    // }
  }

  newTab(url: string) {
    return this.getNativeWindow().open(url);
  }

  newWindow(url: string) {
    return this.getNativeWindow().open(url, '_blank');
  }

  getNativeWindow() {
    return window;
  }

  public openInventory(masterId: number) {
    const url = this.generateNesi1Url(CONFIG.Nesi1URL.inventory + masterId.toString());
    return this.boing(url, 'inventory' + masterId.toString());
  }

  public generateNesi1Url(url: string) {
    let urlReturn;
    if (url.indexOf('?') < 0 && url.toLowerCase().indexOf('%3f') < 0) {
      url += '?is_n1=true';
    }

    if (!url.startsWith(CONFIG.Nesi1URL.host())) {
      if (!url.startsWith('/')) {
        url = '/' + url;
      }
      url = CONFIG.Nesi1URL.host() + url;
    }
    urlReturn = url;
    urlReturn += (!url.includes('is_n1') ? '&is_n1=true' : '');
    return urlReturn;
  }

  open(url: string, target: string) {
    return this.getNativeWindow().open(this.generateNesi1Url(url), target);
  }

  boingFvr(id: number) {
    const url = this.generateNesi1Url(CONFIG.Nesi1URL.Fvr + id.toString());
    return this.boing(url, 'fvrs_' + id.toString());
  }


  boingNesi1(url: string, title: string, target: string = null, newtab = false) {

    let width = 1600;
    let height = 960;
    if (target) {
      try {
        const values = target.split(',');
        width = Number(values[0]);
        height = Number(values[1]);
      } catch (e) {
        width = 1600;
        height = 960;
      }
    }
    if (!url.includes('.aspx') || url.startsWith('#')) {
      return this.boing(url, title, width, height, newtab);
    }

    if (url.indexOf('http') === -1) {
      url = this.generateNesi1Url(url);
    }
    return this.boing(url, title, width, height, newtab);
  }

  boing(url: string, title: string, b_width: number = 1600, b_height: number = 960, newtab = false) {
    const navWindow = this.getNativeWindow();

    const s_width = navWindow.screen.width;
    const s_height = navWindow.screen.height;
    const b_leftpos = (s_width - b_width) / 2;
    const b_toppos = (s_height - b_height) / 2 - 40;
    if (!b_height) {
      b_height = s_height;
    }
    if (!b_width) {
      b_width = s_width;
    }

    let b_settings = 'screenX=' + b_leftpos;
    b_settings += ', screenY=' + b_toppos;
    b_settings += ', left=' + b_leftpos;
    b_settings += ', top=' + b_toppos;
    b_settings += ', toolbar=0';
    b_settings += ', directories=0';
    b_settings += ', status=0';
    b_settings += ', menubar=0';
    b_settings += ', scrollbars=0';
    b_settings += ', width=' + b_width;
    b_settings += ', height=' + b_height;
    if (!newtab) {
      return navWindow.open(url, title, b_settings);
    } else {
      return navWindow.open(url, '_blank');
    }

  }

  refreshPage(f: boolean = false) {
    setTimeout(() => {
      window.location.reload(f);
    }, 200);
  }
}
