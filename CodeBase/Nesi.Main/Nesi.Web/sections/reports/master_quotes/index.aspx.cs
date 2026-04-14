using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Collections.Specialized;
using NESI.Common.Models;
using nesi.core;

public partial class master_quotes : Page
{
	private NeMember current_user;
	private const string _page_name = "MasterQuotes";
	static string default_filter = "page1|visible23|t0|t1|t2|t3|t4|t5|t6|t7|t8|t9|t10|t11|f12|f12|f11|t12|f14|t13|t14|t18|t19|t20|t21|width23|25px|75px|25px|70px|100px|80px|200px|70px|100px|100px|100px|40px|50px|50px|50px|150px|25px|70px|100px|75px|75px|75px|75px";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	NameValueCollection _q;


	protected void Page_Init(object sender, EventArgs e)
	{
		_q = Request.QueryString;
		current_user = Toolbox.do_handle_authentication(OpsPage.MasterQuoteGrid);
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		layout.used_gv = gv_quotes;
		h.Set("gridview_id", "gv_quotes");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
		 fill_grid();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		var quote_id = 0;
	   
        if (!string.IsNullOrEmpty(_q["a"]) && !string.IsNullOrEmpty(_q["qid"]))
		{
			switch (_q["a"])
			{
				case "handle_note":
					int.TryParse(_q["qid"], out quote_id);
					var note = _q["note"].Trim();
					if (quote_id > 0 && note != "")
					{
						var wopn = new NeWoProgNotes(quote_id, "Q");
						wopn.woprogid = quote_id;
						var temp_note = Toolbox.MySQLNow_long() + ": " + current_user.FullName + " - " + note + "\n" + wopn.notes + "\n";
						wopn.notes = temp_note;
						wopn.memberid = current_user.id32;
						wopn.Type = "Q";
						wopn.SaveWOProgProjectNote();



                        Toolbox.QuickReponse(Response, "SUCCESS");
					}
					else
					{
						Toolbox.QuickReponse(Response, "Saving of note failed.");
					}
					break;

				case "handle_note_edit":
					int.TryParse(_q["qid"], out quote_id);
					note = _q["note"].Trim();
					if (quote_id > 0 && note != "")
					{
						var wopn = new NeWoProgNotes(quote_id, "Q");
						wopn.woprogid = quote_id;
						var temp_note = Toolbox.MySQLNow_long() + ": " + current_user.FullName + " - " + note + "\n" + wopn.notes + "\n";
						wopn.notes = temp_note;
						wopn.memberid = current_user.id32;
						wopn.Type = "Q";
						wopn.SaveWOProgProjectNote();


                        var x = gv_quotes.EditingRowVisibleIndex;
						Toolbox.QuickReponse(Response, temp_note);
					}
					else
					{
						Toolbox.QuickReponse(Response, "Saving of note failed.");
					}
					break;
			}
		}
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Master Quote Report";
		if (!IsPostBack)
		{
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				var filter_length = 128 + current_user.FullName.Length; // Length of base filter (128) + length of name
				default_filter = string.Format("page1|filter{0}|StartsWith([pm], '{1}') And [status] Not Like 'P.O Received' And [status] Not Like 'Dead Quote' And [status] Not Like 'Placeholder'|conditions4|3|5|4|3|5|1|10|5|16|7|17|7|21|9|visible27|t0|t1|t2|t3|t4|t5|t6|t7|t8|t9|t10|f12|f12|f11|t12|f14|t13|t14|t15|t16|t17|t18|f23|f20|t19|t21|t22|width27|25px|75px|25px|70px|100px|80px|200px|70px|100px|100px|100px|50px|50px|50px|150px|25px|70px|100px|50px|50px|50px|50px|e|75px|75px|75px|75px", filter_length, current_user.FullName);
				gv_quotes.LoadClientLayout(default_filter);
				gl.GridLayout_Layout = default_filter;
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_quotes.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			//dde_filter.Text = gl.GridLayout_Name;
		}
	}
	private void fill_grid()
	{
        gv_quotes.DataSource = Toolbox.doSQL_dt(@"CALL report_quote_master(@v0 )", new object[] { current_user.id });
     



    }
	protected void gv_quotes_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		if (gv_quotes.GetRowValues(e.VisibleIndex, "notes").ToString().Length > 0)
		{
			// some condition
			// hide the Edit button
			if (e.ButtonType == ColumnCommandButtonType.Edit)
			{
				e.Image.Url = "~/images/FullNotes.JPG";
				e.Image.ToolTip = gv_quotes.GetRowValues(e.VisibleIndex, "notes").ToString();
			}
		}
	}
	protected void gv_quotes_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		var status = gv_quotes.GetRowValues(e.VisibleIndex, "status").ToString();
		var field_name = e.DataColumn.FieldName;
		if (Convert.ToString(e.DataColumn.FieldName) == "date_due" &&  Toolbox.ReturnNullDateTime(e.CellValue) != null)
		{
			if ((Convert.ToDateTime(e.CellValue).Date <= DateTime.Now) && ((status == "Waiting to be Quoted") || (status == "Waiting to be Sent") || (status == "Waiting to be Verified")))
			{
				e.Cell.ForeColor = System.Drawing.Color.Red;
			}
		}
		else if (e.DataColumn.FieldName == "notes")
		{
			e.Cell.ToolTip = Toolbox.ReturnBlankIfNull_string(e.CellValue);
		}

	}
	protected void gv_quotes_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.Parameters != "")
		{
			gv.LoadClientLayout(e.Parameters);
		}
		else
		{
			gv.FilterExpression = "";
			for (var i = 0; i < gv.Columns.Count; i++)
			{
				if (gv.Columns[i] is GridViewDataColumn)
				{
					var col = (GridViewDataColumn)gv.Columns[i];
					if (col.GroupIndex > -1)
					{
						gv.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}
	}
	protected void gv_quotes_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		if (!gv_quotes.IsNewRowEditing)
		{
			var cp = (ASPxCallbackPanel)gv_quotes.FindEditFormTemplateControl("cbp_edit_note");
			var cb = (ASPxComboBox)cp.FindControl("cbchance");
			cb.Text = Toolbox.doSQL_string(@"Select quote_chance_name from quote_chance  where quote_chance_id =@v0", new object[] { gv_quotes.GetRowValues(gv_quotes.EditingRowVisibleIndex, "pct_chance_reason") });
		}
	}
	protected void gv_quotes_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			int pct_chance = Convert.ToInt16(gv_quotes.GetRowValues(e.VisibleIndex, "pct_chance"));
			if (pct_chance != 0)
			{
				if (pct_chance >= 75)
				{
					e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCFF99");
				}
				else if (pct_chance >= 50)
				{
					e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
				}
				else if (pct_chance >= 25)
				{
					e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
				}
			}
		}
	}
	protected void gv_quotes_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_quotes_PageIndexChanged(object sender, EventArgs e)
	{
	}

	protected void cbp_edit_note_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var s = sender as ASPxCallbackPanel;
		var container = ((ASPxCallbackPanel)sender).NamingContainer as GridViewEditFormTemplateContainer;
		var this_quote_id = gv_quotes.GetDataRow(container.VisibleIndex)["quote"].ToString();
		var this_quote_n = Convert.ToInt32(gv_quotes.GetDataRow(container.VisibleIndex)["quote_n"]);
		var this_revision = gv_quotes.GetDataRow(container.VisibleIndex)["rev"].ToString();
		var note = (ASPxMemo)s.FindControl("memx");
		var date_expected = (ASPxDateEdit)s.FindControl("date_expected");
		var date_expected_str = date_expected.Text == "" ? "NULL" :  date_expected.Text;
	    var date_due = (ASPxDateEdit)s.FindControl("date_due");
	    var date_expected_start = (ASPxDateEdit)s.FindControl("QO_startdate");
	    var date_due_str = date_due.Text == "" ? "NULL" : date_due.Text;
	    var date_expected_start_str = date_expected_start.Text == "" ? "NULL" : date_expected_start.Text;
        var chance_percent = (ASPxSpinEdit)s.FindControl("spin_success_pct");
		var success_percent_obj = chance_percent == null || chance_percent.Value.ToString() == "" ? 0 : chance_percent.Value;
		var notes = new NeWoProgNotes(this_quote_n, "Q");
		var cb = (ASPxComboBox)s.FindControl("cbchance");
		var chance_reason = cb.Value == null ? 0 : cb.Value;
		notes.woprog_project_notes_woprogid = Convert.ToInt32(this_quote_id);
		notes.woprog_project_notes_notes = note.Text;
		notes.woprog_project_notes_memberid = current_user.id32;
		notes.woprog_project_notes_Type = "Q";
		notes.SaveWOProgProjectNote();
		Toolbox.doSQL_void(@"UPDATE quote_master 
SET date_due = @v0 , completion_date = @v1 ,
pct_chance = @v4 , pct_chance_reason=@v5 , date_expected_start = @v6 
WHERE quote_id = @v2  AND revision = @v3 ", new object[] {   date_due_str,date_expected_str, this_quote_id, this_revision, success_percent_obj, chance_reason, date_expected_start_str });
        fill_grid();

	    }
	protected void cbchance_DataBound(object sender, EventArgs e)
	{
		if (!gv_quotes.IsNewRowEditing)
		{
			var cp = (ASPxCallbackPanel)gv_quotes.FindEditFormTemplateControl("cbp_edit_note");
			var cb = (ASPxComboBox)cp.FindControl("cbchance");
			cb.Text = Toolbox.doSQL_string(@"Select quote_chance_name from quote_chance  where quote_chance_id =@v0", new object[] { gv_quotes.GetRowValues(gv_quotes.EditingRowVisibleIndex, "pct_chance_reason") });
		}
	}
	protected void spn_chance_Init(object sender, EventArgs e)
	{
	}
	protected void cb_chkprivate_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
	    try
	        {
	        var p = e.Parameter.Split('|');
	        var do_sql = true;
	        var quote_id = 0;
	        var revision = 0;
	        if (p[0].Length > 6)
	            {
	            int.TryParse(p[0].Substring(0, 6), out quote_id);
	            int.TryParse(p[0].Substring(6, 1), out revision);
	            var action = p[2];
	            var value = p[1];
	            if (action == "note")
	                {
	                do_sql = false;
	                var notes = new NeWoProgNotes(quote_id, "Q");
	                notes.woprog_project_notes_woprogid = quote_id;
	                notes.woprog_project_notes_notes = value;
	                notes.woprog_project_notes_memberid = current_user.id32;
	                notes.woprog_project_notes_Type = "Q";
	                notes.SaveWOProgProjectNote();
                }
	            if (do_sql)
	                {
	                if ((action == "follow_up") || (action == "parallel_bid"))
	                    {
	                    value = Convert.ToBoolean(value) ? "1" : "0";
	                    }
	                Toolbox.doSQL_void(
	                    string.Format(@"UPDATE quote_master SET {0}=@v0 WHERE quote_id=@v1 AND revision = @v2 LIMIT 1",
	                        action), new object[] {value, quote_id, revision});




	                }
	            }

	        }
	    catch(Exception ee)
	        {
	        throw new Exception("Couldn't Update Quote");
	        }
	    finally
	        {
	 //       ScriptManager.RegisterStartupScript(this, GetType(), "open_", "please_wait('stop');", true);
        }
	    return;

	    }
    protected void mem_Init(object sender, EventArgs e)
	{
		//if (gv_MasterCustomers.IsCallback)
		//{
		var due = sender as ASPxMemo;
		var container = due.NamingContainer as GridViewDataItemTemplateContainer;
		var quote_id = gv_quotes.GetDataRow(container.VisibleIndex)["quote"].ToString();
		due.Attributes.Add("onclick", "note_show(this)");
		due.Attributes.Add("data-qid", quote_id);
		due.Attributes.Add("wrap", "off");
		//}
	}
	protected void memx_Init(object sender, EventArgs e)
	{
		//if (gv_MasterCustomers.IsCallback)
		//{
		var due = sender as ASPxMemo;

		//GridViewEditFormTemplateContainer container = due.NamingContainer as GridViewEditFormTemplateContainer;
		var quote_id = gv_quotes.GetDataRow(gv_quotes.EditingRowVisibleIndex)["quote"].ToString();
		due.Attributes.Add("onclick", "note_show_edit(this)");
		due.Attributes.Add("data-qid", quote_id);
		due.Attributes.Add("wrap", "off");
		//}
	}
	protected void memx_DataBound(object sender, EventArgs e)
	{
		var dtecomp = sender as ASPxMemo;
		//	GridViewEditFormTemplateContainer container = dtecomp.NamingContainer as GridViewEditFormTemplateContainer;

		if (dtecomp.Text.Length < 1)
		{
			dtecomp.Height = Unit.Pixel(20);
		}
		else
		{
			dtecomp.BackColor = System.Drawing.Color.LightYellow;
			dtecomp.Height = Unit.Pixel(40);
		}
	}
	protected void mem_PreRender(object sender, EventArgs e)
	{
		//if (!IsCallback)
		//{
		//}
	}
	protected void mem_DataBound(object sender, EventArgs e)
	{
		var dtecomp = sender as ASPxMemo;
		var container = dtecomp.NamingContainer as GridViewDataItemTemplateContainer;

		if (dtecomp.Text.Length < 1)
		{
			dtecomp.Height = Unit.Pixel(20);
		}
		else
		{
			dtecomp.BackColor = System.Drawing.Color.LightYellow;
			dtecomp.Height = Unit.Pixel(40);
		}
	}
	protected void lblhr_PreRender(object sender, EventArgs e)
	{

	}
	protected void lblhr_Init(object sender, EventArgs e)
	{
		var label = sender as ASPxLabel;
		var won = 0;
		try
		{
			if (gv_quotes.VisibleRowCount > 0)
			{
				for (var i = 0; i < gv_quotes.VisibleRowCount; i++)
				{
					if (gv_quotes.GetRowLevel(i) == gv_quotes.GroupCount)
					{
						if (gv_quotes.GetRowValues(i, new string[] { "status" }).ToString() == "P.O Received")
						{
							won++;
						}
					}
				}
				label.Text = string.Format("{0} %", Math.Round(Convert.ToDouble(won) / Convert.ToDouble(gv_quotes.VisibleRowCount) * 100, 0));
			}
		}
		catch { }
	}
	protected void cb_followup_CheckedChanged(object sender, EventArgs e)
	{
		var cb = (CheckBox)sender;
		var tc = (GridViewDataItemTemplateContainer)cb.NamingContainer;
		var dr = gv_quotes.GetDataRow(tc.VisibleIndex);
		var quote_id = dr["quote"];
		var rev = dr["rev"];
		Toolbox.doSQL_void(@"UPDATE quote_master SET follow_up = @v0  WHERE quote_id = @v1  AND revision = @v2  LIMIT 1", new object[] { cb.Checked, quote_id, rev });
	    fill_grid();

    }
    protected void dtecomp_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
    {
        var dte = (ASPxDateEdit)sender;
        var tc = (GridViewDataItemTemplateContainer)dte.NamingContainer;
        e.Properties["cpKeyValue"] = tc.KeyValue;
    }

    protected void QO_startdate_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
    {
        var dte = (ASPxDateEdit)sender;
        var tc = (GridViewDataItemTemplateContainer)dte.NamingContainer;
        e.Properties["cpKeyValue"] = tc.KeyValue;
    }
    protected void date_due_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
	{
		var dte = (ASPxDateEdit)sender;
		var tc = (GridViewDataItemTemplateContainer)dte.NamingContainer;
		e.Properties["cpKeyValue"] = tc.KeyValue;
		/*
1	Waiting to be Quoted
2	Waiting to be Sent
3	Waiting to be Verified
4	Waiting for Approval
5	Follow up today
6	Dead Quote
7	Placeholder
8	P.O Received
9	Revisioned
10	Waiting for Stage 1 Go
11	Waiting for Stage 4 Go
12	Waiting for Final Review
13	Waiting Post Mortem*/
		var status = gv_quotes.GetRowValuesByKeyValue(tc.KeyValue, "status_id").ToString();
		switch (status)
		{
			case "9":
			case "8":
			case "7":
			case "6":
			case "5":
			case "13":
			case "4":
				dte.ReadOnly = true;
				e.Properties["cpdr"] = "Quote is Locked";
				break;
			default:
				dte.ReadOnly = false;

				break;
		}

	}

	protected void spn_chance_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
	{
		var spnchance = sender as ASPxSpinEdit;
		var container = spnchance.NamingContainer as GridViewDataItemTemplateContainer;
		e.Properties["cpKeyValue"] = container.KeyValue;
	}
	protected void mem_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
	{
		var memo = sender as ASPxMemo;
		var container = memo.NamingContainer as GridViewDataItemTemplateContainer;
		e.Properties["cpKeyValue"] = container.KeyValue;
	}
	protected void memx_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
	{

		e.Properties["cpKeyValue"] = gv_quotes.GetDataRow(gv_quotes.EditingRowVisibleIndex)["quote"].ToString();
	}
	protected void cb_followup_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
	{
		var chk = sender as ASPxCheckBox;
		var container = chk.NamingContainer as GridViewDataItemTemplateContainer;
		e.Properties["cpKeyValue"] = container.KeyValue;
	}
    protected void cb_parallel_bid_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
    {
        var chk = sender as ASPxCheckBox;
        var container = chk.NamingContainer as GridViewDataItemTemplateContainer;
        e.Properties["cpKeyValue"] = container.KeyValue;
    }


}
