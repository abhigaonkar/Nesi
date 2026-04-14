import { FileInfo } from './file';

export interface DirectoryInfo {
  name: string;
  fullname: string;
  subDirectories: DirectoryInfo[];
  files: FileInfo[];
}
