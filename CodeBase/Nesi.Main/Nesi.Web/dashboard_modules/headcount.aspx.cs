using System;
using System.Text;
using DevExpress.XtraCharts;
using System.Data;
using System.Collections.Specialized;
using nesi.core;

public partial class dashboard_modules_headcount : System.Web.UI.Page
{
    public Toolbox _tools = new Toolbox();
    NeMember current_user = new NeMember();
    protected void Page_Load(object sender, EventArgs e)
    {
        _tools = new Toolbox();
        _tools.dont_cache_page();
        current_user = Toolbox.do_handle_authentication(32);
        var _q = Request.QueryString;
        var business_unit_id = Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);

        if (business_unit_id == 0)
        {
            Response.Clear();
            Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Business unit not set!</div>");
            Response.End();
        }

        var _chart_wo_ds = Toolbox.doSQL_dt("call get_daily_snapshot_over_year_for_bu(@v0,@v1)", new object[] {current_user.id32, business_unit_id });
        var wo_series = chart_wo.Series[0];
        wo_series.DataSource = _chart_wo_ds;
        wo_series.ArgumentDataMember = "ddd";
        wo_series.ArgumentScaleType = ScaleType.Auto;
        
        wo_series.ValueScaleType = ScaleType.Numerical;

        wo_series.Visible = _chart_wo_ds.Rows.Count != 0;
        wo_series.ValueDataMembers.AddRange(new string[] { "headcount" });
     
        wo_series = chart_wo.Series[1];
        wo_series.DataSource = _chart_wo_ds;
        wo_series.ArgumentDataMember = "ddd";
        wo_series.ArgumentScaleType = ScaleType.Auto;

        wo_series.ValueScaleType = ScaleType.Numerical;

        wo_series.Visible = _chart_wo_ds.Rows.Count != 0;
        wo_series.ValueDataMembers.AddRange(new string[] { "truck_count" });
    }



}