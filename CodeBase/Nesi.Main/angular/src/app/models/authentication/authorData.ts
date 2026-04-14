// State Data

export interface AuthData {
  access_token: string;
  refresh_token: string;
  guid: string;
  expires_in: number;
  token_type: string;
  userId: string;
  isContact: string;
  isDeveloper: string;
  issued: Date;
  expires: Date;
  expireSecond: number;
}
