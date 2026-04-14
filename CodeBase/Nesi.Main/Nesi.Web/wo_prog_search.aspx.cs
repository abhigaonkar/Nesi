using System;
using System.Data;
using System.Web.UI.WebControls;
using nesi.core;

public partial class wo_prog_search : System.Web.UI.Page
{
    NeMember current_user;
    Toolbox _tools = new Toolbox();
    private const int _page_id = 13; // from Page table in DB
    private const string _page_description = "Work Order Search";


    protected void Page_Load(object sender, EventArgs e)
		{
		current_user	= Toolbox.do_handle_authentication(13);
        var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
        var lbltemp = (Label)Page.Master.FindControl("lblHeading");
        lbltemp.Text = _page_description;
        if (!IsPostBack)
			{
            txtSearch.Focus();
            ddlCompany.DataSource		= Toolbox.doSQL_dt(@"Call get_visible_business_units(@v0)"  , new object[] {current_user.id });
            ddlCompany.DataTextField	= "ddl_name";
            ddlCompany.DataValueField	= "id";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem("All Business Units", "0"));
			ddlCompany.SelectedValue = current_user.business_unit_id != 11 ? current_user.business_unit_id.ToString() : "0";
			}

    }
    


    protected void btnSearch_Click(object sender, EventArgs e)
    {

        var intUpperLimit = 1;
        var intFlag = 1;
       if (chkHistory.Checked == true)
        {
            intUpperLimit = 50;
        }
        var CompanySelect = "";
        if (ddlCompany.SelectedValue == "0")
        {
            CompanySelect = " > 0";
        }
        else
        {
            CompanySelect = " = " + ddlCompany.SelectedValue;
        }

        var searchLength = txtSearch.Text.Length;
        if (searchLength > 2 && txtDescription.Text == "")
        {
            try
            {
                var number = Convert.ToInt32(txtSearch.Text);
            }
            catch
            {
                intFlag = 0;
            }
            string strSearchSQL;
            if (intFlag == 1)
            {
                if (chkHistory.Checked == true)
                {
                    strSearchSQL = @"
SELECT
	a.WOProg_ID,
	a.WOProg_BVWO,
	a.business_unit_id,
	a.WOPRog_CustomerName WOProg_CustomerName,
	b.WOProgStatus_Status AS THESTATUS,
	WOProgStatus_DateTime AS THETIME,
	c.member_fullname AS THEMEMBER,
	a.WOProg_Description,
d.name
FROM
	WOProg a
LEFT JOIN
	WOProgStatus b ON a.WOProg_ID = b.WOProgStatus_WOProg_ID
LEFT JOIN
	Member c ON b.WOProgStatus_Member_ID = c.Member_ID
LEFT join
	business_unit d ON a.business_unit_id = d.ID
WHERE
	a.business_unit_id " + CompanySelect + @"
	AND
		(
		a.WOProg_BVWO LIKE '%" + txtSearch.Text.Trim().Replace("'", "\"") + @"%' OR
        a.WOProg_ID LIKE '%" + txtSearch.Text.Trim().Replace("'", "\"") + @"%' OR
		a.WOProg_CustomerName LIKE '%" + txtSearch.Text.Trim().Replace("'", "''") + @"%'
		)
ORDER BY
	THETIME
DESC
LIMIT 0," + intUpperLimit + "";
                }
                else
                {
                    strSearchSQL = @"
SELECT
	a.WOProg_ID,
	a.WOProg_BVWO,
	a.business_unit_id,
	a.WOPRog_CustomerName WOProg_CustomerName,
	a.WOProg_Status AS THESTATUS,
	WOProg_OpenDateTime AS THETIME,
	c.member_fullname AS THEMEMBER,
	a.WOProg_Description,
d.name
FROM
	WOProg a
LEFT JOIN
	Member c ON a.WOProg_CutBy_MemberID = c.Member_ID
LEFT join
	business_unit d ON a.business_unit_id = d.ID
WHERE
	a.business_unit_id " + CompanySelect + @"
	AND
		(
		a.WOProg_BVWO LIKE '%" + txtSearch.Text.Trim().Replace("'", "\"") + @"%' OR
        a.WOProg_ID LIKE '%" + txtSearch.Text.Trim().Replace("'", "\"") + @"%' OR
		a.WOProg_CustomerName LIKE '%" + txtSearch.Text.Trim().Replace("'", "''") + @"%'
		)
ORDER BY
	THETIME
DESC";
                }
            }
            else
            {
                strSearchSQL = @"
SELECT
	a.WOProg_ID,
	a.WOProg_BVWO,
	a.business_unit_id,
	a.WOPRog_CustomerName WOProg_CustomerName,
	a.WOProg_Status AS THESTATUS,
	a.WOProg_OpenDateTime AS THETIME,
	c.member_fullname AS THEMEMBER,
	a.WOProg_Description,
d.name
FROM
	WOProg a
LEFT JOIN
	Member c ON a.WOProg_CutBy_MemberID = c.Member_ID
LEFT join
	business_unit d ON a.business_unit_id = d.ID
WHERE
	a.business_unit_id " + CompanySelect + @"
	AND
		(
		a.WOProg_CustomerName LIKE '%" + txtSearch.Text.Trim().Replace("'", "''") + @"%'
		)
ORDER BY
	THETIME
DESC";
            }
	        var dtWoSearch = Toolbox.doSQL_dt(strSearchSQL,null);

            var strResult = "";
            var WO_number = "";
            var BVWO_number = "";
            var CustID = "";
            var CustName = "";
            var Status = "";
            var LastModified = "";
            var LastModifiedBy = "";
            var Description = "";
			var name = "";

            foreach  (DataRow drSearch in dtWoSearch.Rows)
            {
                WO_number = drSearch["WOProg_ID"].ToString();
                BVWO_number = drSearch["WOProg_BVWO"].ToString();
                CustID = drSearch["business_unit_id"].ToString();
                CustName = drSearch["WOProg_CustomerName"].ToString();
				name = drSearch["name"].ToString();
                Status = drSearch["THESTATUS"].ToString();
                LastModified = drSearch["THETIME"].ToString();
                LastModifiedBy = drSearch["THEMEMBER"].ToString();
                Description = drSearch["WOProg_Description"].ToString();

                var wo_prog_url = "/wo_prog_frame.aspx?action=show&woprog_id=" + WO_number + "&business_unit_id=" + CustID + "&fromwo=search";
                wo_prog_url = Server.UrlEncode(wo_prog_url);
                wo_prog_url = "/redir.aspx?url=" + wo_prog_url;
                var wo_link			= Convert.ToInt32(WO_number) > 20000 ? "<a href='"+wo_prog_url+ "' target='_blank'>" + BVWO_number + @"</a>" : BVWO_number;
                strResult += @"
									<tr class='result'>
										<td class='wo_number'>" +wo_link+@"</a></td>
										<td class='description'>" + name + @"</td>
										<td class='cust_name'>" + CustName + @"</td>
                                        <td class='description'>&nbsp;" + Description + @"</td>
										<td class='status'>" + Status + @"</td>
										<td class='last_modified'>" + LastModified + @"&nbsp;</td>
										<td class='last_modified_by'>" + LastModifiedBy + @"&nbsp;</td>
									</tr>";
            }


            if (strResult == "")
            {
                divResults.InnerHtml = @"
								<table cellpadding='0' cellspacing='0' id='work_order_search'>
									<tr>
										<td class='noresults'>No Results Found</td>
									</tr>
								</table>";
            }
            else
            {
                divResults.InnerHtml = @"
								<table cellpadding='3' cellspacing='0' id='work_order_search'>
									<tr class='column_header'>
										<td>WO No</td>
                                        <td>Business Unit</td>
										<td>Customer Name</td>
                                        <td>Description</td>
										<td>Status</td>
										<td>Last Modified</td>
										<td>Last Modified By</td>
									</tr>" + strResult + @"
									
								</table>";
            }
        }
        else if (txtDescription.Text != "")
        {
            var addionsrch = "";
            if (txtSearch.Text != "")
            {
                try
                {
                    var number = Convert.ToInt32(txtSearch.Text);
                    addionsrch = " AND (a.WOProg_BVWO LIKE CONCAT('%',@v1,'%') OR a.WOProg_ID LIKE CONCAT('%',@v1,'%'))";
                }
                catch
                {
                    addionsrch = " AND a.WOProg_CustomerName LIKE CONCAT('%',@v1,'%')";
                }
            }

            var sql = string.Format(@"SELECT
	                                   DISTINCT(a.WOProg_ID),
	                                   a.WOProg_BVWO,
	                                   a.business_unit_id,
	                                   a.WOPRog_CustomerName WOProg_CustomerName,
	                                   a.WOProg_Status AS THESTATUS,
	                                   a.WOProg_OpenDateTime AS THETIME,
	                                   CONCAT(c.member_firstname, ' ', c.member_lastname) AS THEMEMBER,
	                                   a.WOProg_Description,
									   d.name
                                       FROM
	                                     WOProg a
                                       JOIN
                                         wo_detail_history b ON a.woprog_id = b.wo_detail_history_woprog_id
                                       LEFT JOIN
	                                     Member c ON a.WOProg_CutBy_MemberID = c.Member_ID
LEFT join
	business_unit d ON a.business_unit_id = d.ID
                                       WHERE
	                                     a.business_unit_id {0}
                                       AND
                                         b.wo_detail_history_description LIKE CONCAT('%',@v0,'%') {1}
                                       GROUP BY
                                         a.WOProg_ID", CompanySelect, addionsrch);

            var historywoids = _tools.getSQL_datatable(sql, new object[] { txtDescription.Text, txtSearch.Text.Trim() });

            if (historywoids.Rows.Count > 0)
            {
                var strResult = "";
                var WO_number = "";
                var BVWO_number = "";
                var CustID = "";
                var CustName = "";
                var Status = "";
                var LastModified = "";
                var LastModifiedBy = "";
                var Description = "";
				var name = "";
                foreach (DataRow historyrow in historywoids.Rows)
                {
                    WO_number = historyrow["WOProg_ID"].ToString();
                    BVWO_number = historyrow["WOProg_BVWO"].ToString();
                    CustID = historyrow["business_unit_id"].ToString();
                    CustName = historyrow["WOProg_CustomerName"].ToString();
                    Status = historyrow["THESTATUS"].ToString();
                    LastModified = historyrow["THETIME"].ToString();
                    LastModifiedBy = historyrow["THEMEMBER"].ToString();
                    Description = historyrow["WOProg_Description"].ToString();
					name = historyrow["name"].ToString();
                    var wo_prog_url = "/wo_prog_frame.aspx?action=show&woprog_id=" + WO_number + "&business_unit_id=" + CustID + "&fromwo=search";
                    wo_prog_url = Server.UrlEncode(wo_prog_url);
                    wo_prog_url = "/redir.aspx?url=" + wo_prog_url;

                    var wo_link			= Convert.ToInt32(WO_number) > 20000 ? "<a href='"+ wo_prog_url + "' target='_blank'>" + BVWO_number + @"</a>" : BVWO_number;
                    strResult += @"
									<tr class='result'>
										<td class='wo_number'>"+wo_link+@"</td>
<td class='comp_name'>" + name + @"</td>
										<td class='description'>" + CustName + @"</td>
                                        <td class='description'>&nbsp;" + Description + @"</td>
										<td class='status'>" + Status + @"</td>
										<td class='last_modified'>" + LastModified + @"&nbsp;</td>
										<td class='last_modified_by'>" + LastModifiedBy + @"&nbsp;</td>
									</tr>";


                }
                divResults.InnerHtml = @"
								<table cellpadding='3' cellspacing='0' id='work_order_search'>
									<tr class='column_header'>
										<td>WO No</td>
<td>Branch</td>
										<td>Customer Name</td>
                                        <td>Description</td>
										<td>Status</td>
										<td>Last Modified</td>
										<td>Last Modified By</td>
									</tr>" + strResult + @"
									
								</table>";

            }
            else
            {
                divResults.InnerHtml = @"
								<table cellpadding='0' cellspacing='0' id='work_order_search'>
									<tr>
										<td class='noresults'>No Results Found</td>
									</tr>
								</table>";
            }
        }
        else
        {
            divResults.InnerHtml = @"
							<table cellpadding='0' cellspacing='0' id='work_order_search'>
								<tr>
									<td class='noresults'>You're search is too broad, please refine and try again.</td>
								</tr>
							</table>";
        }

    }
}
