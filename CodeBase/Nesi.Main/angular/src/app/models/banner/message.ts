export interface Message {
  type: string;
  toName: string;
  ToId: string;
  fromName: string;
  fromId: string;
  subject: string;
  date: Date;
  status: string;
  body: string;
}
