export enum NesiMenuType {
  Nesi1 = 0,
  MainMenu = 1,
  RouterLink = 2,
  Command = 3,
  Url = 4,
  BlankPage = 5
}


export interface NesiMenuItem {
  id: number;
  label: string;
  icon: string;
  command: any;
  routerLink: any;
  url: string;
  openurl: string;
  badge: number;
  badgeStyleClass: string;
  type: NesiMenuType;
  target: string;
  items: NesiMenuItem[];
}

