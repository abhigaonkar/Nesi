import { Injectable } from '@angular/core';
import { Profile } from '../../models/layout/profile';
@Injectable()
export class LayoutProfileHelper {
  private profiles: Array<Profile>;

  constructor() {

  }

  set Profiles(profiles: Profile[]) {
    this.profiles = profiles;
  }

  get Profiles(): Profile[] {
    return this.profiles
  }

  get MenuStyle(): string {
    return this.getValueByName('MenuStyle');
  }
  set MenuStyle(value: string) {
    this.setValue('MenuStyle', value);
  }

  get MenuMode(): string {
    return this.getValueByName('MenuMode');
  }
  set MenuMode(value: string) {
    this.setValue('MenuMode', value);
  }

  get LayoutColor(): string {
    return this.getValueByName('LayoutColor');
  }
  set LayoutColor(value: string) {
    this.setValue('LayoutColor', value);
  }
  get SignOutTime(): number {
    return Number(this.getValueByName('SignOutTime'));
  }
  set SignOutTime(value: number) {
    this.setValue('SignOutTime', String(value));
  }
  get Theme(): string {
    return this.getValueByName('Theme');
  }
  set Theme(value: string) {
    this.setValue('Theme', value);
  }

  get MessageLifeTime(): number {
    return Number(this.getValueByName('MessageLifeTime'));
  }
  set MessageLifeTime(value: number) {
    this.setValue('MessageLifeTime', String(value));
  }

  get ToggleOnlineUserIcon(): string {
    return this.getValueByName('ToggleOnlineUserIcon');
  }
  set ToggleOnlineUserIcon(value: string) {
    this.setValue('ToggleOnlineUserIcon', value);
  }
  get ToggleOnlineUserBadge(): string {
    return this.getValueByName('ToggleOnlineUserBadge');
  }
  set ToggleOnlineUserBadge(value: string) {
    this.setValue('ToggleOnlineUserBadge', value);
  }
  get ToggleTasksIcon(): string {
    return this.getValueByName('ToggleTasksIcon');
  }
  set ToggleTasksIcon(value: string) {
    this.setValue('ToggleTasksIcon', value);
  }
  get ToggleTicketIcon(): string {
    return this.getValueByName('ToggleTicketIcon');
  }
  set ToggleTicketIcon(value: string) {
    this.setValue('ToggleTicketIcon', value);
  }
  get ToggleMessageIcon(): string {
    return this.getValueByName('ToggleMessageIcon');
  }
  set ToggleMessageIcon(value: string) {
    this.setValue('ToggleMessageIcon', value);
  }

  get ToggleNotificationWhenOtherSignedIn(): string {
    return this.getValueByName('ToggleNotificationWhenOtherSignedIn');
  }
  set ToggleNotificationWhenOtherSignedIn(value: string) {
    this.setValue('ToggleNotificationWhenOtherSignedIn', value);
  }

  get ToggleNotificationWhenOtherSignedOut(): string {
    return this.getValueByName('ToggleNotificationWhenOtherSignedOut');
  }
  set ToggleNotificationWhenOtherSignedOut(value: string) {
    this.setValue('ToggleNotificationWhenOtherSignedOut', value);
  }

  private getValueByName(name: string): string {
    if (this.profiles) {
      const obj = this.profiles.find(m => m.name === name);
      return obj ? obj.value : null;
    } else {
      return null;
    }

  }

  private setValue(name: string, value: string) {
    if (this.profiles) {
      const obj = this.profiles.find(m => m.name === name);
      if (obj) { obj.value = value; }
    }
  }

}
