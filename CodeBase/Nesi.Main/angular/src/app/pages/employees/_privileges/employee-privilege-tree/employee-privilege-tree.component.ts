import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../../reducers';
import { CONFIG } from 'app/configuration';
import { EmployeeService } from '../../_base/employeeService';
import { FormBuilder } from '@angular/forms';
import { EmployeeFormBase } from '../../_base/employeeFormBase';
import { TreeNode } from 'primeng/primeng';
import { TokenService } from '../../../../services/authentication/tokenService';


@Component({
  selector: 'nesi-employee-privilege-tree',
  templateUrl: './employee-privilege-tree.component.html',
  styleUrls: ['./employee-privilege-tree.component.css']
})
export class EmployeePrivilegeTreeComponent extends EmployeeFormBase implements OnInit {
  showtip = false;
  tooltipX = '';
  tooltipY = '';
  currentNode = { business_unit: '', members: [] };
  onlyMemberType = false;
  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    public fb: FormBuilder,
    public ts: TokenService,
  ) {
    super(store, cs, es);

    this.InitEmployee(CONFIG.apiURL.page.employee.privilege.list);
  }

  getUrl(url) {
    return super.getUrl(url).replace('$buid', this.profile.selected_business_unit_id || 0)
      .replace('$memberType', this.onlyMemberType ? '1' : '0');
  }


  ngOnInit(): void {
    this.loaded.subscribe(
      (v) => {
        this.checkReportNodes();
      },
      (err:any)=>
      {
        this.PushErrorMessage(err);
      }
    );
  }

  checkReportNodes()  {
    if ( !(this.profile && this.profile.report && this.profile.report.length && this.profile.report.length > 0) ) {
      return;
    }

    this.profile.report.forEach( node => {
      if (node.children) {
        let selectedChildren = 0;
        node.children.forEach( child => {
          if (child.is_checked) {
            selectedChildren ++;
          }
        });
        if (selectedChildren > 0) {
          node.is_checked = true;
        }
      }
    });
  }

  createForm() {

  }

  remove_parent(nodes: any[]) {
    if (nodes) {
      nodes.forEach(x => {
        x.parent = null;
        this.remove_parent(x.children);
      });
    }
    return nodes;
  }

  // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Cyclic_object_value
  getCircularReplacer = () => {
    const seen = new WeakSet();
    return (key, value) => {
      if (typeof value === 'object' && value !== null) {
        if (seen.has(value)) {
          return;
        }
        seen.add(value);
      }
      return value;
    };
  };

  saveReport() {
    this.submitting = true;
    const copyOfNodes = JSON.parse(JSON.stringify(this.profile.report, this.getCircularReplacer()));
    const postData = this.remove_parent(copyOfNodes);
    this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.employee.privilege.report), postData)
      .subscribe(
        (res) => {
          this.PushResponseMessage(res.data);
          this.submitting = false;
        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
          this.submitting = false;
        }
      );
  }

  saveGlobal() {
    this.submitting = true;
    this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.employee.privilege.global), this.profile.global)
      .subscribe(
        (res) => {
          this.PushResponseMessage(res.data);
          this.submitting = false;
        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
          this.submitting = false;
        }
      );
  }

  savePage() {
    this.submitting = true;
    const postData = this.remove_parent(this.profile.page);
    this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.employee.privilege.list), postData)
      .subscribe(
        (res) => {
          this.PushResponseMessage(res.data);
          this.submitting = false;
        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
          this.submitting = false;
        }
      );
  }


  check_children(node, change_true = true) {
    if (node.children) {
      node.children.forEach(x => {
        if (node.is_checked) {
          if (change_true) {
            x.is_checked = true;
          }
        } else {
          x.is_checked = false;
        }
        this.check_children(x, change_true);
        if (node.is_checked) {
          node.expanded = true;
        }
      });
    }
  }

  checkParent(node, change_true = true) {
    if (node && node.parent && node.parent.children) {
      // Current user clicked one node now, and also this node has parents, we will check or /uncheck the parent node based on how many
      // children nodes are selected.
      let selectedChildren = 0;
      node.parent.children.forEach( child => {
        if (child.is_checked) {
          selectedChildren ++;
        }
       });

       if (selectedChildren > 0) {
         node.parent.is_checked = true;
       } else {
         if ( change_true ) {
          node.parent.is_checked = false;
         }
       }
       // console.log(`${selectedChildren} children were selected.`);
    }
  }

  show_tip() {
    this.showtip = true;
  }

  expandAll(tree: any[]) {
    tree.forEach(node => {
      this.expandRecursive(node, true);
    });
  }

  collapseAll(tree: any[]) {
    tree.forEach(node => {
      this.expandRecursive(node, false);
    });
  }

  private expandRecursive(node: any, isExpand: boolean) {
    node.expanded = isExpand;
    if (node.children) {
      node.children.forEach(childNode => {
        this.expandRecursive(childNode, isExpand);
      });
    }
  }

  moveTip(event: any, node: any) {
    this.show_tip();
    this.currentNode = node;
    this.tooltipX = (event.clientX + 20) + 'px';
    this.tooltipY = (event.clientY + 20) + 'px';
  }


}
