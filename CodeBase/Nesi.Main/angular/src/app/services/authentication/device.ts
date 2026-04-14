import { Ng2DeviceService } from 'ng2-device-detector';
import { Injectable } from '@angular/core';

@Injectable()
export class DeviceService {
  deviceInfo: any;
  constructor(private deviceService: Ng2DeviceService) {
    this.deviceInfo = this.deviceService.getDeviceInfo();
  }

  get isMobile(): boolean {
    // console.log(this.deviceInfo);
    return this.deviceInfo.device !== 'unknown' && this.deviceInfo.device !== 'ipad';
  }

  get deviceType(): boolean {
    return this.deviceInfo.device;
  }

  get isChrome(): boolean {
    return String(this.deviceInfo.browser).toLowerCase().indexOf('chrome') > -1;
  }
}
