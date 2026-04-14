import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DirectoryInfo } from '../../../models/component/fileManager/directory';
import { Tree, TreeModule, TreeNode } from 'primeng/primeng';
import { CONFIG } from '../../../configuration';
import { FileInfo } from '../../../models/component/fileManager/file';
import { CoreService } from '../../../services/shared/core.service';
import { DatePipe } from '@angular/common';
import { ContextMenuModule, MenuItem } from 'primeng/primeng';
import { Observable } from 'rxjs/Observable';
import * as MessageHelper from '../../../services/helper/MessageHelper';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { ConfirmDialogModule, ConfirmationService } from 'primeng/primeng';
import { saveAs as importedSaveAs } from 'file-saver';


@Component({
  selector: 'nesi-fileManager',
  templateUrl: './fileManager.component.html',
  styleUrls: ['./fileManager.component.css']
})
export class FileManagerComponent implements OnInit {
  @Input() disabled:boolean;
  dir: TreeNode[];
  selectedNode: TreeNode;
  uploadedFiles = [];
  uploadURL: string;

  files: FileInfo[];
  selectedFiles: FileInfo[];
  set Files(value: FileInfo[]) {
    if (!value) {
      return;
    }
    value.forEach(x => {
      x.lastModified = new Date(x.lastModified);
    });
    this.files = value;
  }



  @Input()
  set Dir(value: TreeNode[]) {
    if (value && value[0]) {
      this.dir = value;
      this.uploadURL = CONFIG.apiURL.host()
        + CONFIG.apiURL.core.fileManager.uploadFiles + encodeURI(value[0].data);
      this.selectedNode = value[0];
      this.loadFolderFiles(value[0].data, value[0].label);

      this.expandAll();
    }
  }
  InputDisplay = false;
  InputValue: string;
  InputType: string;

  cmItems: MenuItem[];
  selectedDestinationNode: TreeNode;
  selectDestinationDisplay = false;

  filesOperationType = 'Copy';

  constructor(
    public cs: CoreService,
    private store: Store<fromRoot.State>,
    private cfs: ConfirmationService
  ) { }

  ngOnInit() {
    this.cmItems = [
      { label: 'New', icon: 'fa-plus', command: (event) => this.CreateDirectory(event) },
      //   { label: 'Rename', icon: 'fa-tag', command: (event) => this.RenameDirectory(event) },
      { label: 'Delete', icon: 'fa-times', command: (event) => this.DeleteDirectory(event) }
    ];
  }

  CreateDirectory(event) {
    this.InputValue = null;
    this.InputDisplay = true;
    this.InputType = 'CreateDirectory';


  }
  DeleteDirectory(event) {
    if (!this.selectedNode.parent) {
      MessageHelper.pushResponseMessage(this.store, 'The root folder can not be deleted.');
      return;
    }
    this.cfs.confirm({
      message: 'Are you sure that you want to delete this folder?',
      accept: () => {
        this.cs.postString(CONFIG.apiURL.core.fileManager.deleteDirectory, {
          name: this.selectedNode.label,
          fullname: this.selectedNode.data
        }).subscribe((res: string) => {
          this.refreshDir();
          MessageHelper.pushResponseMessage(this.store, res);
        });
      }
    })
  }
  RenameDirectory(event) {
    this.InputValue = this.selectedNode.label;
    this.InputDisplay = true;
    this.InputType = 'RenameDirectory';
  }

  InputDialogConfirm() {
    this.InputDisplay = false;
    if (!this.InputValue) {
      return;
    }
    let resp: Observable<string>;
    switch (this.InputType) {
      case 'CreateDirectory': {
        resp = this.cs.postString(CONFIG.apiURL.core.fileManager.createDirectory, {
          name: this.InputValue,
          fullname: this.selectedNode.data + '\\' + this.InputValue
        });
        break;
      }
      case 'RenameDirectory': {
        resp = this.cs.postString(CONFIG.apiURL.core.fileManager.renameDirectory, [{
          name: this.selectedNode.label,
          fullname: this.selectedNode.data
        },
        {
          name: this.InputValue,
          fullname: this.selectedNode.data
        }
        ]);
        break;
      }
    }
    resp.subscribe(
      (res: string) => {
        this.refreshDir();
        MessageHelper.pushResponseMessage(this.store, res);
      }
    );
  }

  refreshDir() {
    const d = this.dir[0];
    this.cs.postList<TreeNode>(CONFIG.apiURL.core.fileManager.listDirectory,
      {
        name: d.label,
        fullName: d.data,
      })
      .subscribe(
      (res: TreeNode[]) => {
        this.Dir = res;
      }
      );
  }

  FileEditComplete(event) {
    const row: FileInfo = event.data;
    this.cs.postString(CONFIG.apiURL.core.fileManager.renameFile, {
      name: row.name,
      fullName: row.fullName,
    }).subscribe(
      (res: string) => {
        this.loadFolderFiles(this.selectedNode.data, this.selectedNode.label);
        MessageHelper.pushResponseMessage(this.store, res);
      });
  }

  copyFiles() {
    this.selectedDestinationNode = null;
    this.filesOperationType = 'Copy';
    this.selectDestinationDisplay = true;

  }

  moveFiles() {
    this.selectedDestinationNode = null;
    this.filesOperationType = 'Move';
    this.selectDestinationDisplay = true;
  }
  selectDestinationDialogConfirm() {
    this.selectDestinationDisplay = false;
    let url: string;
    switch (this.filesOperationType) {
      case 'Copy': {
        url = CONFIG.apiURL.core.fileManager.copyFiles;
        break;
      }
      case 'Move': {
        url = CONFIG.apiURL.core.fileManager.moveFiles;
        break;
      }
    }

    const files = [];
    this.selectedFiles.forEach(
      (x: FileInfo) => {
        files.push({
          from: x.fullName,
          to: this.selectedDestinationNode.data + '\\' + x.name
        });
      }
    )
    this.cs.postString(url, files).subscribe(
      (res: string) => {
        this.loadFolderFiles(this.selectedNode.data, this.selectedNode.label);
        MessageHelper.pushResponseMessage(this.store, res);
      }
    );

  }

  onSelect(event: any) {
    this.uploadedFiles = [];
    for (const file of event.files) {
      this.uploadedFiles.push(file);
    }

  }

  deleteFiles() {
    CONFIG.LOG(this.selectedFiles, 'on delete files timesheet project files');
    if (!(this.selectedFiles && this.selectedFiles.length > 0)) {
      return;
    }
    this.cs.postString(CONFIG.apiURL.core.fileManager.deleteFiles, JSON.stringify(this.selectedFiles))
      .subscribe(
      (res: any) => {
        this.loadFolderFiles(this.selectedNode.data, this.selectedNode.label);
        this.selectedFiles = null;
      }
      );

  }

  downLoadFiles(event: any) {
    this.selectedFiles.forEach(
      (file: FileInfo) => {
        this.cs.downloadFile(file).subscribe(
          (blob) => {
            importedSaveAs(blob, file.name);
          }
        );
      });
  }
  onUploaded(event: any) {
    this.loadFolderFiles(this.selectedNode.data, this.selectedNode.label);
  }

  nodeSelect(event: any) {
    if (!event.node) {
      return;
    }
    CONFIG.LOG(event.node.data, 'tree node selected filemanager');
    this.loadFolderFiles(event.node.data, event.node.label);
    this.selectedFiles = null;
  }

  loadFolderFiles(path: string, name: string) {
    this.cs.postList(CONFIG.apiURL.core.fileManager.getFiles,
      {
        name: name,
        data: path,
      }
    ).subscribe((res: FileInfo[]) => {
      this.Files = res;
    });
    this.uploadURL = CONFIG.apiURL.host()
      + CONFIG.apiURL.core.fileManager.uploadFiles + encodeURI(path);
  }
  nodeUnselect(event) {
    CONFIG.LOG(event.node.data, 'tree node unselected filemanager');

  }
  onBefoeUpload(event) {
    // before upload, we need set http request credential.
    this.cs.SetTokenBeforeUpload(event);
  }

  public expandAll() {
    this.dir.forEach(node => {
      this.expandRecursive(node, true);
    }
    );
  }
  private expandRecursive(node: TreeNode, isExpand: boolean) {
    node.expanded = isExpand;
    if (node.children) {
      node.children.forEach(childNode => {
        this.expandRecursive(childNode, isExpand);
      });
    } else {
      node.children = null;
    }
  }
}
