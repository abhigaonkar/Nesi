using System;
using System.Data;
using System.Text;
using System.Web.UI;
using MySql.Data.MySqlClient;
using nesi.core;

public partial class sections_workorder_wocomments : Page
	{
	NeMember _currentUser = new NeMember();
	string _strprint = "0";
	private const int page_id = 1; // from Page table in DB

	protected void Page_Load(object _sender, EventArgs _e)
		{
		using (var conn = Toolbox.connect())
			{
			_currentUser = Toolbox.do_handle_authentication(page_id);
			var woprogid = Request.QueryString["woid"];
			var wo = new NeWOProg(Convert.ToInt32(woprogid));
			hidCompanyID.Value = wo.business_unit_id.ToString();
			hidWOBVWO.Value = wo.OrderNumber;
			hidWOProgID.Value = woprogid;
			if (IsPostBack) return;
			get_comments(conn, woprogid);
			get_time_sheet_comments(conn);
			}
		}

	private void get_comments(MySqlConnection _conn, string _str_wo_prog_id)
		{
		var cnt = 0;
		var layer = new StringBuilder();
		layer.Append("<div id='content' style='min-height:180px; PADDING-RIGHT: 5px; PADDING-LEFT: 5px; PADDING-BOTTOM: 5px; PADDING-TOP: 5px;'>");
		var comments = Toolbox.doSQL_dt(_conn, @"
SELECT 
	member_fullname name, 
	woprogcomment_datetime, 
	woprogcomment_text
FROM 
	woprogcomment,member,woprog
WHERE  
	woprogcomment_woprog_id = @v0 AND 
	woprogcomment_woprog_id=woprog_id AND 
	woprogcomment_member_id=member_id AND 
	woprogcomment_deleted='F'"  , new object[] {_str_wo_prog_id});

		foreach (DataRow dr_comments in comments.Rows)
			{
			cnt++;
			layer.AppendFormat("<span class='chatcontentname'>{0} - {1}</span><br>{2}<hr>", dr_comments["name"], dr_comments["woprogcomment_datetime"], Server.HtmlEncode(dr_comments["woprogcomment_text"].ToString()).Replace("\n", "<br />"));
			}


		if (cnt == 0)
			{
			layer.Append("No Comments Entered");
			}
		layer.Append(@"
            </div>
            ");
		divComments.InnerHtml = layer.ToString();
		}

	private void get_time_sheet_comments(MySqlConnection _conn)
		{
		var timecomment = Toolbox.doSQL_dt(_conn, @"SELECT wocomment_id, workorder_id, comments, CAST(printcomments AS UNSIGNED) printcomments, member_id_audit, created_date, business_unit_id FROM vwwocomments  WHERE woprog_id =@v0 AND member_id = 0 and comments != '' ", new object[] { hidWOBVWO.Value,hidCompanyID.Value });
		if (timecomment.Rows.Count != 0)
			{
			foreach (DataRow dr_time in timecomment.Rows)
				{
				chkPrint.Enabled = true;
				txtComments.Enabled = true;
				var printComments = Convert.ToBoolean(dr_time["PrintComments"]);
				chkPrint.Checked = printComments;
				var comment_id = dr_time["wocomment_id"].ToString();
				hid_ts_comment_id.Value = comment_id;
				txtComments.Enabled = true;
				txtComments.Text = dr_time["Comments"].ToString();
				return;
				}
			}
		else
			{
			txtComments.Text	= "";
			}
		}

	protected void btnChatAdd_Click(object _sender, EventArgs _e)
		{
		using (var conn = Toolbox.connect())
			{
			var commtext = txtChat.Text.Trim().Replace("'", "''");
			if (commtext != "")
				{
			
				Toolbox.doSQL_void(@"INSERT INTO woprogcomment (woprogcomment_woprog_id,woprogcomment_member_id,woprogcomment_text,woprogcomment_x,woprogcomment_y,woprogcomment_datetime) VALUES (@v0 ,@v1 ,@v2 ,5,5,NOW())", new object[] {  hidWOProgID.Value, _currentUser.id, commtext } );
				txtChat.Text = "";
				}
			_strprint = chkPrint.Checked ? "1" : "0";
			ScriptManager.RegisterStartupScript(this, this.GetType(), "Update", "window.opener.location.href = window.opener.location.href; HideProgress();", true);
			get_comments(conn, hidWOProgID.Value);
        }
		}

	protected void btnUpdateTS_Click(object _sender, EventArgs _e)
		{
		using (var conn = Toolbox.connect())
			{
			_strprint = chkPrint.Checked ? "1" : "0";
			var thiscommentid = "0"; ;
			var timecomment = Toolbox.doSQL_dt(conn, @"SELECT wocomment_id, workorder_id, comments, printcomments, member_id_audit, created_date, business_unit_id FROM vwwocomments  WHERE woprog_id =@v0 and membeR_id = 0 and comments = ''", new object[] { hidWOBVWO.Value,hidCompanyID.Value });
			if (hid_ts_comment_id.Value == "")
				{
				foreach (DataRow dr_time in timecomment.Rows)
					{
					chkPrint.Enabled = true;
					txtComments.Enabled = true;

					thiscommentid = dr_time["WOComment_ID"].ToString();
					break;
					}
				}
			else
				{
				thiscommentid = hid_ts_comment_id.Value;
				}

			if (thiscommentid != "0")
				{
					Toolbox.doSQL_void(conn, @"UPDATE wocomment SET comments =@v0  ,printcomments =@v1, wocomment_member_id = 0  ,modified_date = NOW() WHERE wocomment_id = @v2 ", new object[] {  txtComments.Text, Convert.ToInt16(_strprint), thiscommentid } );
				}
			else
				{
					Toolbox.doSQL_void(conn, @"
INSERT INTO wocomment 
	(
	workorder_id, 
	comments, 
	personal, 
	printcomments, 
	member_id_audit,
	created_date, 
	modified_date, 
	wocomment_member_id, 
	business_unit_id,
	woprog_id
	) 
VALUES 
	(
	@v0 , 
	@v1 ,
	0,
	@v2 , 
	0 , 
	NOW(),
	NOW(), 
	0, 
	@v3 ,
	@v4 
	)", 
new object[]
	{
	hidWOBVWO.Value,
	txtComments.Text,
	_strprint,
	hidCompanyID.Value,
	hidWOProgID.Value
	} );
				}
			get_time_sheet_comments(conn);
			}	
		}
	}
