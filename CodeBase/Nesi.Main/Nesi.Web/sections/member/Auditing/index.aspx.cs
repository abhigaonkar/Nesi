using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Linq;
using System.Web.UI;
using MySql.Data.MySqlClient;
using nesi.core;

public partial class sections_member_auditing : System.Web.UI.Page
	{
	NeMember _current_user;
	private const int page_id		= 50;
	DataTable _audit_items			= new DataTable();
	string _todays_date				= Toolbox.MySQLNow_short();
	protected void Page_Init()
		{
		_current_user = Toolbox.do_handle_authentication(page_id);
		_audit_items					= Toolbox.doSQL_dt(@"SELECT audititem_ratingdesc_id id, audititem_ratingdesc_type type_id, audititem_ratingdesc_audititem_id group_id FROM audititem_ratingdesc"  , null);
		    SqlDataSource2.SelectCommand = "Select id,ddl_name name from business_unit  where id in (" +
		                                   new Current_User().visible_business_units + ")";

		    }
    protected void Page_Load(object _sender, EventArgs _e)
		{

        

		var q = Request.QueryString;
		    gv_history.DataSource = Toolbox.doSQL_dt(@"
		    SELECT
		    a.audithistory_headerid,
		    a.business_unit_id,
		    a.audithistory_header_memberid,
		    a.audithistory_header_dateofaudit,
		    a.audithistory_header_dateofentry,
		    a.audithistory_header_score,
		    d.name audithistory_type,
		        b.member_fullname auditor,
		        c.name branch
		    FROM

		        audithistory_header a
		        LEFT JOIN
		        member b on a.audithistory_header_memberid = b.member_id
		    LEFT join
		    business_unit c ON a.business_unit_id = c.id  
		    LEFT JOIN

		    audit_type d on a.audithistory_type = d.id
where a.business_unit_id in (" + new Current_User().visible_business_units + @")

		    ORDER BY

		    audithistory_header_dateofaudit DESC", null);
        gv_history.DataBind();




		var menu = new NeMenu(_current_user, page_id);

		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(_current_user);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = Toolbox.doSQL_string(@"SELECT Page_Desc FROM page WHERE Page_Id = @v0" , page_id);
		if (_current_user.AuthenticatedForPrivilege(40)) // if the user is an admin
			{
			gv_history.Columns[0].Visible = true;
			var gvc = (GridViewCommandColumn)gv_history.Columns[0];
			gvc.ShowDeleteButton = true;
			//ASPxPageControl1.TabPages[0].ClientVisible = true;
			//ASPxPageControl1.TabPages[1].ClientVisible = true;
			}
		else
			{
			if (_current_user.AuthenticatedForPrivilege(39)) // if the user is an auditor
				{
				//gv_history.Columns[0].Visible = true;
				//ASPxPageControl1.TabPages[0].ClientVisible = false;
				//ASPxPageControl1.TabPages[1].ClientVisible = true;

				}
			if (_current_user.AuthenticatedForPrivilege(38)) // if the user can see results
				{
				//gv_history.Columns[0].Visible = true;
				//ASPxPageControl1.TabPages[0].ClientVisible = false;
				//ASPxPageControl1.TabPages[1].ClientVisible = true;
				}
			}

		}


	protected void btnExport_Click(object _sender, EventArgs _e)
		{
		export.FileName = "Auditing List as of " + _todays_date;
		export.Styles.Cell.Font.Name = "Arial";
		export.Styles.Header.Font.Name = "Arial";
		export.GridView.SettingsText.Title = "Auditing List as of " + _todays_date;
		export.GridView.Settings.ShowTitlePanel = true;

		export.Styles.Title.Font.Name = "Arial";
		export.Styles.Title.Font.Size = FontUnit.XLarge;

		var c = new GridViewDataTextColumn {Caption = "Pass-Fail"};

		var n = new GridViewDataTextColumn {Caption = "Notes"};

		export.GridView.Columns.Add(c);
		export.GridView.Columns.Add(n);
		export.WriteXlsToResponse();

		}
	protected void gv_admin_RowInserting(object _sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs _e)
		{
		var gv				= (ASPxGridView) _sender;
		var combo_type		= (ASPxComboBox) gv.FindEditFormTemplateControl("combo_type");
		var combo_group	= (ASPxComboBox) gv.FindEditFormTemplateControl("combo_group");
		var memo_item			= (ASPxMemo) gv.FindEditFormTemplateControl("memo_item");
		var cb_active		= (ASPxCheckBox) gv.FindEditFormTemplateControl("cb_active");
		Toolbox.doSQL_void(@"
INSERT INTO audititem_ratingdesc 
	(
	audititem_ratingdesc_active,
	audititem_ratingdesc_audititem_id,
	audititem_ratingdesc_name,
	audititem_ratingdesc_type
	) 
VALUES 
	(
	@v0,
	@v1,
	@v2,
	@v3
	)", new object[] {
		cb_active.Checked,
		combo_group.Value,
		memo_item.Text,
		combo_type.Value
		});

		_e.Cancel = true;
		gv_admin.DataBind();
		gv_admin.CancelEdit();
		}
	protected void gv_admin_RowDeleting(object _sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs _e)
		{
		var id = _e.Keys[0].ToString();
		Toolbox.doSQL_void(@"Update audititem_ratingdesc set AuditItem_RatingDesc_Active = 0 where AuditItem_RatingDesc_id =@v0 " , id);
		_e.Cancel = true;
		gv_admin.CancelEdit();
		gv_admin.DataBind();
		}
	protected void gv_admin_RowUpdating(object _sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs _e)
		{
		var gv				    = (ASPxGridView) _sender;
		var combo_type		    = (ASPxComboBox) gv.FindEditFormTemplateControl("combo_type");
		var combo_group	        = (ASPxComboBox) gv.FindEditFormTemplateControl("combo_group");
		var memo_item			= (ASPxMemo) gv.FindEditFormTemplateControl("memo_item");
		var cb_active		    = (ASPxCheckBox) gv.FindEditFormTemplateControl("cb_active");
		Toolbox.doSQL_void(@"
UPDATE 
	audititem_ratingdesc 
SET 
	audititem_ratingdesc_name = @v0, 
	audititem_ratingdesc_active = @v1,
	audititem_ratingdesc_audititem_id = @v2,
	audititem_ratingdesc_type = @v3
WHERE 
	audititem_ratingdesc_id = @v4
LIMIT 1", new object[] {
		memo_item.Text,
		cb_active.Checked,
		combo_group.Value,
		combo_type.Value,
		_e.Keys[0]
		});
		gv_admin.DataBind();

		_e.Cancel = true;
		gv_admin.CancelEdit();
		}
	protected void gv_history_RowDeleting(object _sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs _e)
		{
		var id = _e.Keys[0].ToString();

		if (id != "0")
			{
			Toolbox.doSQL_void(@"Delete from audithistory_details where audithistory_details_headerID= @v0", id);
			Toolbox.doSQL_void(@"Delete from audithistory_header where audithistory_headerID =@v0 " , id);

			}
		gv_history.DataBind();
		gv_history.CancelEdit();
		_e.Cancel = true;

		}
	protected void gv_history_RowUpdating(object _sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs _e)
		{
		var gv = (ASPxGridView)_sender;
		do_save(gv);
		gv_history.CancelEdit();
		gv_history.DataBind();
		gv.DataBind();
		_e.Cancel = true;
		}
	protected void gv_history_RowInserting(object _sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs _e)
		{
		var gv = (ASPxGridView)_sender;
		do_save(gv);
		gv.DataBind();
		gv.CancelEdit();
		_e.Cancel = true;
		}
	private void do_save(ASPxGridView _gv)
		{

		var cb         = (ASPxCallbackPanel)_gv.FindEditFormTemplateControl("cb");
		var cbp_tree = (ASPxCallbackPanel) cb.FindControl("cbp_tree");
		var tv	= (ASPxTreeView)cbp_tree.FindControl("tree");
		var ddlcompany = (ASPxComboBox)cb.FindControl("ddlcompany");
		var ddlauditor = (ASPxComboBox)cb.FindControl("ddlauditor");
		var dtedate    = (ASPxDateEdit)cb.FindControl("dtedate");
		var btncreate  = (ASPxButton)cb.FindControl("btn_show");
		var lblid      = (HiddenField)cb.FindControl("lblid");

		var newaudit = new NeAudit_History(Convert.ToInt32(lblid.Value));

		double chkd = 0;
		double unchkd = 0;
		Toolbox.doSQL_void(@"Delete from Audithistory_details where AuditHistory_Details_Headerid = @v0" , newaudit.audithistory_headerid);
		foreach (TreeViewNode item in tv.Nodes)
			{
			foreach (TreeViewNode ndl in item.Nodes)
				{
				if (ndl.Checked)
					chkd++;
				else
					unchkd++;

				Toolbox.doSQL_void(@"Insert into Audithistory_details 
(AuditHistory_Details_Headerid,audithistory_details_audititem_ratingdesc_id,audithistory_details_result) 
values (@v0,@v1,@v2)", new object[] { newaudit.audithistory_headerid, ndl.DataItem, ndl.Checked});

				}
			}
		newaudit.audithistory_header_score = Convert.ToDouble(chkd / (chkd + unchkd) * 100);
		newaudit.savescore();

		Toolbox.doSQL_void(@"update audithistory_header
set business_unit_id = @v0 , 
audithistory_header_memberid = @v1,
audithistory_header_dateofaudit=@v2 where audithistory_headerid =@v3", new object[] {
				ddlcompany.Value, ddlauditor.Value,dtedate.Date.ToString("yyyy-MM-dd"),lblid.Value});
		_gv.DataBind();


		}
	private void populate_checklist(ASPxTreeView _tv, int _business_unit_id, bool _is_new, int? _type_id)
		{
		if(_type_id == null) return;
		
		if(_is_new)
			{
			_tv.Nodes.Clear();
			var audits = Toolbox.doSQL_dt(@"SELECT audititem_id id, audititem_name name FROM audititem where audititem_id IN (select group_id FROM audit_typegroup_link WHERE type_id = @v0 )", new object[] {  _type_id } );
			foreach (DataRow dr1 in audits.Rows)
				{
				var group_id		= (int) dr1["id"];
				var group_name		= (string) dr1["name"];
				var tn = new TreeViewNode
							{
							Text = group_name,
							DataItem = group_id.ToString()
							};
				var audits_eval = Toolbox.doSQL_dt(@" SELECT audititem_ratingdesc_id id, audititem_ratingdesc_name name FROM audititem_ratingdesc WHERE audititem_ratingdesc_active = 1 AND audititem_ratingdesc_audititem_id = @v0  AND audititem_ratingdesc_type = @v1 ", new object[] {  group_id, _type_id } );
				if (audits_eval.Rows.Count > 0)
					{
					foreach (DataRow dr2 in audits_eval.Rows)
						{
						var tn2 = new TreeViewNode
									{
									Text = (string) dr2["name"],
									DataItem = dr2["id"].ToString()
									};
						tn.Nodes.Add(tn2);

						}

					_tv.Nodes.Add(tn);
					_tv.ExpandAll();
					perform_action_on_nodes_recursive(_tv.Nodes, delegate(TreeViewNode _node) { _node.AllowCheck = _node.Nodes.Count == 0; });
					}
				}
			}
		else
			{
			
			}
		}
	protected void gv_history_HtmlEditFormCreated(object _sender, ASPxGridViewEditFormEventArgs _e)
		{
		var cb = (ASPxCallbackPanel)gv_history.FindEditFormTemplateControl("cb");
		var cbp_tree = (ASPxCallbackPanel) cb.FindControl("cbp_tree");
		var tv = (ASPxTreeView)cbp_tree.FindControl("tree");
		var ddlcompany = (ASPxComboBox)cb.FindControl("ddlcompany");
		var ddlauditor = (ASPxComboBox)cb.FindControl("ddlauditor");
		var dtedate = (ASPxDateEdit)cb.FindControl("dtedate");
		var btncreate = (ASPxButton)cb.FindControl("btn_show");
		var lblid = (HiddenField)cb.FindControl("lblid");
		var btnsave = (ASPxButton)cb.FindControl("btnUpdate");
		var btncancel = (ASPxButton)cb.FindControl("btnCancel");
		var lblscore = (ASPxLabel)cb.FindControl("lblscore");
		var ddl_audittype = (ASPxComboBox) cb.FindControl("ddl_audittype");

		if (_current_user.AuthenticatedForPrivilege(39) || _current_user.AuthenticatedForPrivilege(39)) // if the user is an auditor or admin
			{

			}
		else
			{
			ddlcompany.ClientEnabled = false;
			ddlauditor.ClientEnabled = false;
			dtedate.ClientEnabled = false;
			btnsave.ClientEnabled = false;
			btncreate.ClientEnabled = false;
			tv.ClientVisible = false;

			}

		if (gv_history.IsNewRowEditing)
			{
			//		btnsave.ClientEnabled = false;
			ddlauditor.Value				= _current_user.id;
			ddlcompany.Value				= _current_user.business_unit.id;
			populate_checklist(tv, (int) ddlcompany.Value, true, (int?) ddl_audittype.Value ?? 1);
			dtedate.Date					= DateTime.Now;
			btnsave.ClientVisible			= false;
			ddl_audittype.Value				= 1;
			ddl_audittype.ClientSideEvents.SelectedIndexChanged		= "function(s,e){cbp_tree.PerformCallback(ddlcompany.GetValue()+'|'+s.GetValue());}";
			}
		else
			{
			//			btnsave.ClientEnabled = true;
			var row_index = gv_history.EditingRowVisibleIndex;
			var id = gv_history.GetRowValues(row_index, "audithistory_headerid");
			var a = new NeAudit_History(Convert.ToInt32(id));

			lblid.Value = id.ToString();
			ddlcompany.Value = a.business_unit_id;
			var branch = new NeBusinessUnit(a.business_unit_id);
			ddlauditor.Value = a.audithistory_header_memberid;
			dtedate.Date = a.audithistory_header_audit_date;
			ddl_audittype.Value	= a.audithistory_type;
			ddl_audittype.ClientEnabled = false;
			btncreate.ClientVisible = false;
			tv.Nodes.Clear();
			//		ArrayList Groupitems = new NeAudit_History().LoadNeAudit_History_Groups(a.audithistory_headerid);

			var headgroups = Toolbox.doSQL_dt(@"SELECT audititem_id id, audititem_name name FROM audititem where audititem_id IN (select group_id FROM audit_typegroup_link WHERE type_id = @v0 )", new object[] {  a.audithistory_type } );
			if (headgroups.Rows.Count > 0)
				{
				var chkd = 0.0;
				var unchkd = 0.0;
				tv.Nodes.Clear();
				foreach (DataRow dr1 in headgroups.Rows)
					{
					var group_id		= (int) dr1["id"];
					var group_name	= (string) dr1["name"];
					var tn = new TreeViewNode
								{
									Text = group_name,
									DataItem = group_id
								};

					var audits_eval = Toolbox.doSQL_dt(@" SELECT b.audititem_ratingdesc_id, b.audititem_ratingdesc_audititem_id, b.audititem_ratingdesc_name, b.audititem_ratingdesc_value, b.audititem_ratingdesc_active FROM audithistory_details a INNER JOIN audititem_ratingdesc b ON a.audithistory_details_audititem_ratingdesc_id = b.audititem_ratingdesc_id where a.audithistory_details_headerid = @v0  AND b.audititem_ratingdesc_audititem_id = @v1 ", new object[] {  id, group_id } );
					if (audits_eval.Rows.Count > 0)
						{
						foreach (DataRow dr2 in audits_eval.Rows)
							{
							var tn2 = new TreeViewNode
										{
											Text = dr2[2].ToString(),
											DataItem = dr2[0].ToString()
										};
							tn.Nodes.Add(tn2);
							tn2.Checked = Toolbox.doSQL_bool(@"Select audithistory_details_result from audithistory_details where audithistory_details_audititem_ratingdesc_id = @v0  and audithistory_details_headerID = @v1 ", new object[] {  tn2.DataItem, lblid.Value } );
							if (tn2.Checked)
								chkd++;
							else
								unchkd++;
							}
						lblscore.Text = Convert.ToDouble(chkd / (chkd + unchkd)).ToString("P2");
						tv.Nodes.Add(tn);
						tv.ExpandAll();
						perform_action_on_nodes_recursive(tv.Nodes, delegate(TreeViewNode _node) { _node.AllowCheck = _node.Nodes.Count == 0; });
						}
					}
				tv.ClientVisible = true;
				}
			else
				{
				tv.Nodes.Clear();
				}
			}
		
		}
	public static DataTable get_ne_audit_listfrom_groups()
		{
		return Toolbox.doSQL_dt(@" SELECT distinct c.audititem_id, c.audititem_name FROM audititem_group b, audititem c  WHERE b.audititem_id = c.audititem_id ORDER BY c.audititem_name" , null);
		}
		

	protected void perform_action_on_nodes_recursive(TreeViewNodeCollection _nodes, Action<TreeViewNode> _action)
		{
		foreach (TreeViewNode node in _nodes)
			{
			_action(node);
			if (node.Nodes.Count > 0)
				perform_action_on_nodes_recursive(node.Nodes, _action);
			}
		}


	protected void cb_Callback(object _sender, CallbackEventArgsBase _e)
		{
		using(var conn = Toolbox.connect())
			{
			var cb            = (ASPxCallbackPanel)gv_history.FindEditFormTemplateControl("cb");
			var cbp_tree = (ASPxCallbackPanel) cb.FindControl("cbp_tree");
			var tv = (ASPxTreeView)cbp_tree.FindControl("tree");
			var ddlcompany    = (ASPxComboBox)cb.FindControl("ddlcompany");
			var ddlauditor    = (ASPxComboBox)cb.FindControl("ddlauditor");
			var dtedate       = (ASPxDateEdit)cb.FindControl("dtedate");
			var btncreate     = (ASPxButton)cb.FindControl("btn_show");
			var lblid         = (HiddenField)cb.FindControl("lblid");
			var ddl_audittype = (ASPxComboBox) cb.FindControl("ddl_audittype");
			var btnsave		 = (ASPxButton)cb.FindControl("btnUpdate");
			var btncancel = (ASPxButton)cb.FindControl("btnCancel");

			if (ddlcompany.Value != null && ddlauditor.Value != null && dtedate.Text != null && ddl_audittype.Value != null)
				{
				if (lblid.Value == "")  // create a new one
					{
					var id = Toolbox.doSQL_return_id(conn, @"
INSERT INTO audithistory_header 
	(
	business_unit_id,
	audithistory_header_memberid,
	audithistory_header_dateofaudit, 
	audithistory_type
	) 
VALUES 
	(
	@v0,
	@v1,
	@v2,
	@v3
	)", new object[] {
		ddlcompany.Value, 
		ddlauditor.Value, 
		dtedate.Date.ToString("yyyy-MM-dd"), 
		ddl_audittype.Value
		});
					populate_checklist(tv, (int) ddlcompany.Value, true, (int) ddl_audittype.Value);
					lblid.Value = id.ToString();
					var newaudit = new NeAudit_History(id);
					foreach (TreeViewNode item in tv.Nodes)
						{
						foreach (TreeViewNode ndl in item.Nodes)
							{
							Toolbox.doSQL_void(conn, @"
INSERT INTO Audithistory_details 
	(
	AuditHistory_Details_Headerid,
	audithistory_details_audititem_ratingdesc_id,
	audithistory_details_result
	) 
VALUES 
	(
	@v0,
	@v1,
	@v2
	)", new object[] {
		newaudit.audithistory_headerid, 
		ndl.DataItem, 
		ndl.Checked});
							}
						}
					newaudit.savescore();
					}
				else
					{
					Toolbox.doSQL_void(conn, @"
UPDATE 
	audithistory_header 
SET 
	business_unit_id = @v0,
	audithistory_header_memberid = @v1,
	audithistory_header_dateofaudit = @v2, 
	audithistory_type = @v3 
WHERE 
	audithistory_headerid = @v4", new object[] { ddlcompany.Value, ddlauditor.Value, dtedate.Date.ToString("yyyy-MM-dd"), ddl_audittype.Value, lblid.Value});
					}
				btncreate.ClientVisible = false;
				btnsave.ClientVisible = true;
				cb.JSProperties["cpcloseme"]	= "true";
				}
			else
				{
				throw new Exception("You must select a branch, and auditor, a date and a type before a new audit can be created.");
				}
			}
		}
	protected void gv_grouplink_CustomCallback(object _sender, ASPxGridViewCustomCallbackEventArgs _e)
		{
		var group_id = _e.Parameters;
		var gv		= (ASPxGridView) _sender;
		var cb		= (ASPxComboBox) gv.FindTitleTemplateControl("cb_groups");

		var type_id = gv.GetMasterRowKeyValue();
		Toolbox.doSQL_void(@"INSERT INTO audit_typegroup_link (type_id, group_id) VALUES (@v0, @v1)", new object[] { type_id, group_id});
		cb.DataBind();
		cb.Value			= null;
		cb.SelectedItem		= null;
		cb.SelectedIndex	= -1;

		gv.DataBind();
		}
	protected void gv_typeadmin_CommandButtonInitialize(object _sender, ASPxGridViewCommandButtonEventArgs _e)
		{
		// Need to see if this type is used on any audititems
		var gv					= (ASPxGridView) _sender;
		if (_e.VisibleIndex <= -1 || _e.ButtonType != ColumnCommandButtonType.Delete) return;
		var this_id						= Convert.ToInt32(gv.GetDataRow(_e.VisibleIndex)["id"]);
		var is_used					= _audit_items.Select("type_id = "+this_id).Any();
		_e.Image.Url						= !is_used
			? "/images/icon/icon[delete].gif"
			: "/images/icon/icon[delete-grey].gif";
		_e.Enabled						= !is_used;
		}
	protected void gv_grouplink_CommandButtonInitialize(object _sender, ASPxGridViewCommandButtonEventArgs _e)
		{
		// Need to see if this linked group is used on any audit items
		var gv					= (ASPxGridView) _sender;
		if (_e.VisibleIndex <= -1 || _e.ButtonType != ColumnCommandButtonType.Delete) return;
		var this_id						= Convert.ToInt32(gv.GetDataRow(_e.VisibleIndex)["group_id"]);
		var is_used					= _audit_items.Select("group_id = "+this_id).Any();
		_e.Image.Url						= !is_used
			? "/images/icon/icon[delete].gif"
			: "/images/icon/icon[delete-grey].gif";
		_e.Enabled						= !is_used;
		}
	protected void gv_typeadmin_CellEditorInitialize(object _sender, ASPxGridViewEditorEventArgs _e)
		{

		}
	protected void gv_typeadmin_InitNewRow(object _sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs _e)
		{
		var gv					= (ASPxGridView) _sender;
		var gv_link			    = (ASPxGridView) gv.FindEditFormTemplateControl("gv_grouplink");
		var div_title	        = (HtmlContainerControl) gv.FindEditFormTemplateControl("grouplink_title");
		gv_link.Visible		    = !gv.IsNewRowEditing;
		div_title.Visible	    = !gv.IsNewRowEditing;
		}
	private void bind_item_admin_groups(ref ASPxComboBox _cb, int _type_id)
		{
		var dt_groups			= Toolbox.doSQL_dt(@"SELECT a.id, c.audititem_id group_id, c.AuditItem_Name AS group_name FROM audit_typegroup_link a LEFT OUTER JOIN audit_type b ON a.type_id = b.id LEFT OUTER JOIN audititem c ON a.group_id = c.AuditItem_ID WHERE a.type_id = @v0 ", new object[] {  _type_id } );
		_cb.DataSource				= dt_groups;
		_cb.DataBind();
		}
	protected void gv_admin_HtmlEditFormCreated(object _sender, ASPxGridViewEditFormEventArgs _e)
		{
		var gv					= (ASPxGridView) _sender;
		var combo_type			= (ASPxComboBox) gv.FindEditFormTemplateControl("combo_type");
		var combo_group			= (ASPxComboBox) gv.FindEditFormTemplateControl("combo_group");
		if(!gv.IsNewRowEditing)
			{
			var type_id						= Convert.ToInt32(gv.GetDataRow(gv.EditingRowVisibleIndex)["type_id"]);
			var group_id					= Convert.ToInt32(gv.GetDataRow(gv.EditingRowVisibleIndex)["group_id"]);
			combo_group.Value				= group_id;
			combo_type.Value				= type_id;
			bind_item_admin_groups(ref combo_group, type_id);
			}
		}
	protected void combo_group_Callback(object _sender, CallbackEventArgsBase _e)
		{
		var combo_group		= (ASPxComboBox) _sender;
		var type_id						= Convert.ToInt32(_e.Parameter);
		bind_item_admin_groups(ref combo_group, type_id);
		}
	protected void cbp_tree_Callback(object _sender, CallbackEventArgsBase _e)
		{
		var cbp_tree	= (ASPxCallbackPanel) _sender;
		var tv			= (ASPxTreeView) cbp_tree.FindControl("tree");
		var paras		= _e.Parameter.Split('|');
		populate_checklist(tv,Convert.ToInt32(paras[0]), true, Convert.ToInt32(paras[1]));
		}
	protected void gv_history_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
		{
		gv_history.DataBind();
		}
}
