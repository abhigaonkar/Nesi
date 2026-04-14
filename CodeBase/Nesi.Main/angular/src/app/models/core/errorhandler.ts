export interface Errorhandler {
  id: number;
  memeber_ID: number;
  dt: Date;
  error_desc: string;
  error_short: string;
  level: number;
  host_url: string;
  full_stacktrace: string;
  user_ip: string;
  origin: string;
  is_global: boolean;
  error_on_page: string;
  error_on_line: string;
}
