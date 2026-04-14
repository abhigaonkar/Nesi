using System;
using System.Web.UI;
using DevExpress.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using nesi.core;

public partial class home : Page
	{
	private NeMember myMember;
	private const int _page_id = 1; // from Page table in DB
    private Toolbox _tools;
    int x;
	NameValueCollection _q;
	bool is_developer		= false;

	protected void Page_Load(object sender, EventArgs e)
	{

	    //This page should never run. Alert jordan
	    Toolbox.RedirectToN2(Response, Request, _page_id);

        x = 0;
		_tools		= new Toolbox();
   		myMember	= Toolbox.do_handle_authentication(_page_id);
		((IntraDefault)this.Master).page_name		= NePage.get_page_name(_page_id);

			_q								= Request.QueryString;
		is_developer			= Toolbox.Contains(myMember.id, new int[]{8,711,1295,1359, 2024 , 2025 });

		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		todolist1.Visible = !myMember.isContact;
		string[] apt_ids;

		if (Request.QueryString["a"] != null)
		{
			var apt_id = Convert.ToInt32(Request.QueryString["apt_id"]);
			NeAppointment apt;
			switch (Request.QueryString["a"])
			{
					
				#region process schedule confirmation
				case "schedule_approve": Response.Clear();
					
					apt = new NeAppointment(apt_id);
					if (apt.Status == 97)
					{
						apt.Status = 0;
						apt.Id = apt_id;
						apt.Save();
						Response.Write(@"
<script>
	alert('Schedule Confirmed');
	location.href	= '/home.aspx';
</script>");
					}
					Response.End();
					break;
				case "schedule_delete": Response.Clear();
					apt = new NeAppointment(apt_id);
					if (apt.Status == 97)
					{
						_tools.getSQL_void("Delete from appointments where id= " + apt_id + " limit 1");
						Response.Write(@"
<script>
	alert('Schedule Deleted');
	location.href	= '/home.aspx';
</script>");
					}
					Response.End();
					break;

				
				case "schedule_approve_all":
					Response.Clear();
					apt_ids = Request.QueryString["all_apt_id"].Split('|');
					foreach (var a_id in apt_ids)
					{
						apt = new NeAppointment(Convert.ToInt32(a_id));
						if (apt.Status == 97)
						{
							apt.Status = 0;
							apt.Id = Convert.ToInt32(a_id);
							apt.Save();
						}
					}
					Response.Write(@"
<script>
	alert('Schedules Confirmed');
	location.href	= '/home.aspx';
</script>");
					Response.End();
					break;
				case "schedule_delete_all":
					Response.Clear();
					apt_ids = Request.QueryString["all_apt_id"].Split('|');
					foreach (var a_id in apt_ids)
					{
						apt = new NeAppointment(Convert.ToInt32(a_id));
						if (apt.Status == 97)
						{
							_tools.getSQL_void("Delete from appointments where id= " + a_id + " limit 1");
						}
					}
						Response.Write(@"
<script>
	alert('Schedules Deleted');
	location.href	= '/home.aspx';
</script>");
						Response.End();
					break;

				#endregion email-process
			}
		}
		if (!is_developer)
		{
            if (new List<int>(new int[]{4,5,11,12,17,25,29,34,38,39,52}).Contains(myMember.MemberTypeID))
			{
				logo.Visible                = false;
				rp_devwatch.Visible = false;
				pnl_mywoprocess.Visible     = true;
				pnl_myquotes.Visible        = true;
				pnl_wowaitingpm.Visible     = true;
				pnl_branchquotes.Visible    = true;
				pnl_branchwoprocess.Visible = true;
				pnl_branchpo.Visible        = (myMember.MemberTypeID == 5) || (myMember.MemberTypeID == 29);

				if(pnl_mywoprocess.Visible)
					{
						grid_woStatus.DataSource = _tools.getSQL_datatable(@"SELECT woprog_status.woprog_status_status as status, 
Count(woprog.WOProg_ID) as wocount,datediff(curdate(),ifnull(MIN(woprog.woprog_opendatetime),curdate())) as Age
					FROM woprog_status Left Join woprog ON woprog_status.woprog_status_status = woprog.WOProg_Status
and woprog.WOProg_Status <>  'Invoiced' AND woprog.WOProg_Status <> 'Open' AND
woprog.WOProg_PM_MemberID = @v0 AND woprog.business_unit_id = @v1 GROUP BY woprog_status.woprog_status_status  order by woprog_status.woprog_status_id",
							new object[]
							{
								myMember.id,myMember.business_unit_id
							});
					grid_woStatus.DataBind();
					}
				if(pnl_branchwoprocess.Visible)
					{
					grid_woStatus_branch.DataSource = _tools.getSQL_datatable(@" 
SELECT 
	a.woprog_status_status status, 
	COUNT(b.WOProg_ID) wocount,
	DATEDIFF(CURDATE(),IFNULL(MIN(b.woprog_opendatetime),CURDATE())) Age
FROM 
	woprog_status a
LEFT JOIN 
	woprog b ON 
		a.woprog_status_status = b.woprog_status AND 
		b.business_unit_id = @v0 
WHERE 
	b.woprog_status NOT IN ('Invoiced','Open','Deleted') 
GROUP BY 
	status 
order by 
	b.woprog_status", new object[] { myMember.business_unit_id });
					grid_woStatus_branch.DataBind();
					}
				if(pnl_myquotes.Visible)
					{
					grid_quotes.DataSource = _tools.getSQL_datatable(@" 
SELECT 
	quote_status.status as status, 
	Count(quote_master.quote_ID) as wocount,
	datediff(curdate(),ifnull(MIN(quote_master.last_print_date),curdate())) as
Age FROM quote_status Left Join quote_master ON quote_status.id = quote_master.Status_id and quote_status.id <= 5 
AND quote_master.quoted_by = @v0 GROUP BY quote_status.id ",new object[] {
						myMember.id
					});
					grid_quotes.DataBind();
					}
				if(pnl_branchquotes.Visible)
					{
					grid_comp_quotes.DataSource = _tools.getSQL_datatable(@" 
SELECT 
	member.member_fullname as pm, 
	Count(quote_master.quote_id) AS quotecount, 
	datediff(curdate(),ifnull(MIN(quote_master.last_print_date),curdate())) AS Age 
FROM 
	quote_status 
LEFT JOIN 
	quote_master ON quote_status.id = quote_master.status_id AND quote_master.business_unit_id = @v0 and quote_status.id = 4 
INNER JOIN 
	member ON quote_master.quoted_by = member.Member_ID 
GROUP BY 
	quoted_by,quote_status.id 

order by 
	member.member_fullname", new object[] {

						myMember.business_unit_id

					});
					grid_comp_quotes.DataBind();
					}
				if (pnl_wowaitingpm.Visible)
				{
					grid_comp_wo.DataSource = _tools.getSQL_datatable(@"SELECT member_fullname as pm, 
Count(DISTINCT woprog.woprog_id) AS wocount, datediff(curdate(),ifnull(MIN(woprog.woprog_opendatetime),curdate())) AS Age
FROM woprog_status LEFT JOIN woprog ON woprog_status.woprog_status_status= woprog.WOProg_Status
AND woprog.business_unit_id = @v0 and woprog_status.woprog_status_status = 'Waiting PM Approval'  INNER JOIN member ON woprog.WOProg_PM_MemberID = member.Member_ID" +
					                                                  " GROUP BY woprog.WOProg_PM_MemberID,woprog_status.woprog_status_status ",
						new object[]
						{
							myMember.business_unit_id
						}
				);
					grid_comp_wo.DataBind();
					}
				if(pnl_branchpo.Visible)
					{
					grid_pos.DataSource = _tools.getSQL_datatable(@" 
SELECT 
	a.status_type as status, 
	Count(b.poprog_id) as wocount,
	datediff(curdate(),ifnull(MIN(b.poprog_cutdate),curdate())) as Age 
FROM 
	poprog_status a
Left Join poprog_header b ON a.poprog_status_id = b.poprog_status and a.poprog_status_id NOT IN (4,6,7,8)
WHERE 
	b.business_unit_id = 7 AND
	b.nesi_cut_po = false AND
	b.poprog_order_description NOT LIKE '%NESI%'
GROUP BY 
	a.poprog_status_id 
",null);
					grid_pos.DataBind();
					}

			}
			else
			{
				if (myMember.isContact)
				{
					logo.Visible = true;
					_tools.add_css("/css/customer_portal/home.css");
					//img_quotebutton.Visible = true;
					//img_wobutton.Visible = true;
					if (myMember.AuthenticatedForPage(12))
					{
						wo_button.Attributes["onclick"] = string.Format(@"boing('/sections/workorder/index.aspx?woprog_id=0&business_unit_id={0}','wo',1200,800)", myMember.business_unit_id);
					}
					else
					{
						wo_button.Visible = false;
					}
					if (myMember.AuthenticatedForPage(111))
					{
						quote_button.Attributes["onclick"] = string.Format(@"boing('/sections/reports/CustView_Quotes/index.aspx?new_quote=true','quote',1000,800)");
					}
					else
					{
						quote_button.Visible = false;
					}
					if (myMember.AuthenticatedForPage(147))
					{
					faq_button.Attributes["onclick"] = string.Format(@"location.href = '/sections/training/faq.aspx'");
					}
					else
					{
						faq_button.Visible = false;
					}
 		
				}
				else
				{
					//img_quotebutton.Visible = false;
					//img_wobutton.Visible = false;

				}

			}


		}
		else if(is_developer)
		{

			logo.Visible = false;
			rp_devwatch.Visible = true;
			pnl_mywoprocess.Visible = false;
			pnl_myquotes.Visible = false;
			pnl_branchpo.Visible = false;
			pnl_branchquotes.Visible = false;
			pnl_wowaitingpm.Visible = false;
			pnl_branchwoprocess.Visible = false;

		}
	}


    protected void ASPxProgressBar1_DataBound(object sender, EventArgs e)  // work order panels
    {
        var progressBar = (ASPxProgressBar)sender;
        
        var templateContainer = (GridViewDataItemTemplateContainer)progressBar.NamingContainer;
        var rowKeyValue = templateContainer.KeyValue;
       if (progressBar.Position>progressBar.Maximum)
	   {
		   progressBar.Position = progressBar.Maximum;
	   }
        if (progressBar.Position >7)
            progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Red;
        else if (progressBar.Position <=7 && progressBar.Position >3)
            progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Yellow;
        else if (progressBar.Position <= 3)
			progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Lime;
    }

    protected void ASPxProgressBar2_DataBound(object sender, EventArgs e)  // quotes panels
    {
        var hi_level = 8;
        var low_level = 4;
        var progressBar = (ASPxProgressBar)sender;

        switch (x)
        {
            case 0:
                hi_level = 8;
                low_level = 4;
                break;
            case 1:
                hi_level = 8;
                low_level = 4;
                break;

            case 2:
                hi_level = 8;
                low_level = 4;
                break;

            case 3:
                hi_level = 8;
                low_level = 4;
                break;

            case 4:
                hi_level = 8;
                low_level = 4;
                break;

        }
        

        var templateContainer = (GridViewDataItemTemplateContainer)progressBar.NamingContainer;
        var rowKeyValue = templateContainer.KeyValue;
		if (progressBar.Position > progressBar.Maximum)
		{
			progressBar.Position = progressBar.Maximum;
		}
        if (progressBar.Position > hi_level)
            progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Red;
        else if (progressBar.Position <= hi_level && progressBar.Position > low_level)
            progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Yellow;
        else if (progressBar.Position <= low_level)
			progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Lime;
        x++;
    }

    protected void ASPxProgressBar3_DataBound(object sender, EventArgs e)  // quotes panels
    {
        var hi_level = 12;
        var low_level = 8;
        var progressBar = (ASPxProgressBar)sender;

		if (progressBar.Position > progressBar.Maximum)
		{
			progressBar.Position = progressBar.Maximum;
		}

        var templateContainer = (GridViewDataItemTemplateContainer)progressBar.NamingContainer;
        var rowKeyValue = templateContainer.KeyValue;

        if (progressBar.Position > hi_level)
            progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Red;
        else if (progressBar.Position <= hi_level && progressBar.Position > low_level)
            progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Yellow;
        else if (progressBar.Position <= low_level)
			progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Lime;
        x++;
    }

    protected void grid_woStatus_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.Name == "Age")
        {
                if (Toolbox.ReturnZeroIfNull_int(e.CellValue) > 5)
                {
                    e.Cell.ForeColor = System.Drawing.Color.Red;
                    e.Cell.Font.Bold = true;
                }
        }
    }
    protected void grid_quotes_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.Name == "Age2")
        {
            if (Toolbox.ReturnZeroIfNull_int(e.CellValue) > 60)
            {
                e.Cell.ForeColor = System.Drawing.Color.Red;
                e.Cell.Font.Bold = true;
            }
        }
    }
    protected void ASPxProgressBar4_DataBound(object sender, EventArgs e)
    {
        var hi_level = 20;
        var low_level = 10;
        var progressBar = (ASPxProgressBar)sender;

		if (progressBar.Position > progressBar.Maximum)
		{
			progressBar.Position = progressBar.Maximum;
		}

        var templateContainer = (GridViewDataItemTemplateContainer)progressBar.NamingContainer;
        var rowKeyValue = templateContainer.KeyValue;

        if (progressBar.Position > hi_level)
            progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Red;
        else if (progressBar.Position <= hi_level && progressBar.Position > low_level)
            progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Yellow;
        else if (progressBar.Position <= low_level)
			progressBar.IndicatorStyle.BackColor = System.Drawing.Color.Lime;
        x++;
    }
	protected void grid_woStatus_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (myMember.MemberTypeID == 4)
		{
			if (e.KeyValue.ToString() == "Waiting PM Approval")
			{
				e.Row.Cells[0].Font.Bold = true;
			}
		}

		if (e.KeyValue.ToString() == "Questions For PM")
			{
				e.Row.Cells[0].Font.Bold = true;
			}
	

	}

	protected void gv_watch_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if(e.DataColumn.Name == "count" && e.VisibleIndex > 0)
			{
			var val			= Toolbox.ReturnZeroIfNull_int(e.CellValue);
			if(val > 0)
				{
				e.Cell.Style["background-color"]		= "#fcc";
				e.Cell.Style["color"]					= "#000";
				e.Cell.Style["font-weight"]				= "bold";
				}
			if(val == 0)
				{
				e.Cell.Style["background-color"]		= "#cfc";
				e.Cell.Style["color"]					= "#000";
				e.Cell.Style["font-weight"]				= "normal";
				}
			}
		}
protected void  gv_slowpages_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
{
	if (e.DataColumn.Caption == "Seconds Spent Week Before")
	{

		e.Cell.Text = _tools.getSQL_string(@"Select concat(sum(timediff(ifnull(render_end,request_end), request_start)), 
' (',avg(timediff(ifnull(render_end,request_end), request_start)),')') t2 from log_page log_page2 
where log_page2.url =@v0 and timediff(ifnull(render_end,request_end), request_start)>3" +
		                                   " and dt>=(curdate()-interval 14 day) and dt<(curdate()-interval 7 day) ", new object[] { e.KeyValue});
		
	}
}

}
