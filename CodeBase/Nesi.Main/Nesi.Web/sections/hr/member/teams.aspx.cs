using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class member_teams : Page
	{
	NeMember current_user;
	private const int _page_id = 172; // from Page table in DB
	private const string _page_name = "Teams";
	private Toolbox _tools = new Toolbox();

	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		

        if (!IsPostBack)
			{
		
			}
		}
	protected void Page_Load(object sender, EventArgs e)
	    {
	    ddlbranch.DataSource = _tools.getSQL_datatable(@"Select id, ddl_name name from business_unit  where find_in_set(id, @v0 ) ", new object[] { new Current_User().visible_business_units });
        ddlbranch.DataBind();
			if (!IsPostBack)
			{
				ddlbranch.Value = current_user.business_unit_id;
			}
			fill_grid();
		}
	protected void fill_grid()
	{
		var tbl = new Table();
		tbl.Style["padding"] = "0px";
		tbl.Style["BorderStyle"] = "None";
		var row = new TableRow();
		pnl.Controls.Clear();
		row.Style["padding"] = "0px";
		row.Style["BorderStyle"] = "None";


		var dt = _tools.getSQL_datatable(@"Select member_id, member_fullname,membertype_name from member,membertype  where membertype.membertype_id = member.member_membertype_id and business_unit_id =@v0 and membertype.is_team_leader=1 and member_status='Active'", new object[] { ddlbranch.Value.ToString() });
		var pm_list = "";
	
		
		foreach (DataRow dr in dt.Rows)
		{
			var cell1 = new TableCell();
			cell1.Style["padding"] = "0px";
			pm_list += dr[0] + ",";
			var lb = new ASPxListBox();
			var dt1 = _tools.getSQL_datatable(@"Select member_id,concat(member_fullname,'-',membertype_name) _name from member,membertype  where membertype.membertype_id = member.member_membertype_id and member_status='Active' and member.reports_to =@v0 order by member_fullname", new object[] { dr[0] });

	//		lb.DataSource = _tools.getSQL_datatable(@"Select member_id,member_fullname from member  where member_status='Active' and scheduled_by =@v0", new object[] { dr[0] });
			lb.TextField = "_name";
			lb.ItemStyle.Border.BorderStyle = BorderStyle.None;
			lb.ValueField = "member_id";
			lb.Font.Name = "Arial";
			lb.Height = Unit.Pixel(300);
			lb.Width = Unit.Percentage(100);
			lb.Border.BorderStyle = BorderStyle.None;
			lb.CssClass = "listBoxRight";
			lb.ItemStyle.CssClass = "lbItem";
			
			lb.ID = "lb@" + dr[0];
			lb.ClientInstanceName = "lb@" + dr[0];
			var lbc = new ListBoxColumn("member_id");
			lbc.Visible = false;
			lb.Columns.Add(lbc);
			lb.Columns.Add(new ListBoxColumn("_name", dr[1].ToString()));
			if (dt1.Rows.Count > 0)
			{
				lb.DataSource = dt1;
				lb.DataBind();
	//			lb.Columns[0].Visible = false;			
			}
			else
			{

			}
			
			
			

			cell1.Controls.Add(lb);
			row.Cells.Add(cell1);

			
			
		}
		tbl.Rows.Add(row);
		pnl.Controls.Add(tbl);
		if (pm_list.Length > 0)
		{
			pm_list=pm_list.TrimEnd(',');
		}
		else
		{
			pm_list = "9999";
		}
			lbAvailable.DataSource = _tools.getSQL_datatable(@"SELECT
member.Member_ID,
member.member_fullname,
membertype.membertype_name 
FROM
member,membertype
WHERE
member.member_membertype_id=membertype.membertype_id and 
member.Member_Status = 'Active' AND
not find_in_set(member.reports_to ,@v0) AND
not find_in_set(member.member_id ,@v0) AND
member.business_unit_id = @v1 and membertype.is_scheduled = 1", new object[] { pm_list, ddlbranch.Value });
		
		lbAvailable.DataBind();
		
	}


	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var w = e.Parameter.Split('|');
		if (w[0] == "0")
		{
			try
			{
				var mid = Convert.ToInt32(w.GetValue(2));
				var pid = Convert.ToInt32(w.GetValue(1).ToString().Split('@').GetValue(1));
				if (pid != 0 && mid != 0)
				{
					_tools.getSQL_void(@"update member set scheduled_by=@v0, reports_to=@v0 where member_id =@v1  limit 1", new object[] {
						pid,mid
					});
					fill_grid();
				}
			}
			catch { }
		}
		else
		{
			var lb_name = w[1];
			var _index = w[2];
			var _fromtable = w[3];
			var _index_start = _fromtable.IndexOf('@')+1;
			if (_index_start!=0)
			{
			var _index_end = _fromtable.LastIndexOf('_');
			var _frompid = _fromtable.Substring(_index_start, _index_end - _index_start);
			
				var dt1 = _tools.getSQL_datatable(@"Select member_id,member_fullname from member  where member_status='Active' and reports_to =@v0 order by member_fullname", new object[] { _frompid });

				var _member_id = Convert.ToInt32(dt1.Rows[Convert.ToInt32(_index)][0]);
				if (lb_name.Contains("@"))  // if dragging to another table
				{
					var pid = Convert.ToInt32(w.GetValue(1).ToString().Split('@').GetValue(1));
					
					if(pid == _member_id)
						{
						throw new Exception("You cannot have users report to themselves");
						}
					_tools.getSQL_void(@"update member set scheduled_by=@v0, reports_to=@v0 where member_id =@v1  limit 1", new object[] {
						pid, _member_id
					});
					fill_grid();

				}
				else  // if just returning back to the pool
				{
				    var offerExists = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member_offers WHERE memberid = @v0 AND status = 'Accepted'", new object[] { _member_id }) > 0;
				    if (offerExists)
				    {
				        _tools.getSQL_void(@"update member a set scheduled_by=0, 
reports_to = (Select reports_to from member_offers 
where memberid = a.member_id and status='Accepted' order by id desc limit 1) where member_id =@v0 limit 1", new object[] {
				            _member_id});
				    }
				    else
				    {
				        throw new Exception("Cannot remove employee from team - employee does not have an active employment offer.");
				    }
				    fill_grid();
                }
			}

		}


		
	}
	
}