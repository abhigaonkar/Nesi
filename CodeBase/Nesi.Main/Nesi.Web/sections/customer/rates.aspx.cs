using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Collections.Generic;
using nesi.core;

public partial class customer_rates : Page
	{
	NeMember current_user;
	protected NameValueCollection _q;
	protected int _cust_id = 0;
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(10);
		_q = Request.QueryString;
		combo_company.DataSource = shared.GetAllBVDSNs(false);
		combo_company.DataBind();
		if(Session["rates_business_unit_id"] == null)
			{
			Session["rates_business_unit_id"] = current_user.business_unit_id.ToString();
			}
		combo_company.Items.FindByValue((string) Session["rates_business_unit_id"]).Selected		= true;
		if(!string.IsNullOrEmpty(_q["customer_id"]))
			{
			_cust_id	= Toolbox.ReturnZeroIfNull_int(_q["customer_id"]);
			}
		else
			{
			Toolbox.FriendlyException(Response, "Customer Not Set", "/");
			}
		}
	protected void combo_company_SelectedIndexChanged(object sender, EventArgs e)
		{
		var cb				= (ASPxComboBox) sender;
		Session["rates_business_unit_id"] = cb.Value.ToString();
		gv_rates.DataBind();
		ScriptManager.RegisterStartupScript(this, this.GetType(), "b", "bind_dates()", true);
		}
	protected void b_per_save_Click(object sender, EventArgs e)
		{
		var b								= (ASPxButton) sender;
		var c			= (GridViewDataItemTemplateContainer) b.NamingContainer;
		var tb								= (ASPxTextBox) gv_rates.FindRowCellTemplateControlByKey(c.KeyValue, (GridViewDataColumn) gv_rates.Columns["Reg"], "t_reg");
		var val									= tb.Text;


		var t_fromdate = (HtmlInputText)gv_rates.FindRowCellTemplateControlByKey(c.KeyValue, (GridViewDataColumn)gv_rates.Columns["from_date"], "t_fromdate");
		var val_fromdate = Convert.ToDateTime(t_fromdate.Value).ToString("yyyy-MM-dd");

		var t_todate = (HtmlInputText)gv_rates.FindRowCellTemplateControlByKey(c.KeyValue, (GridViewDataColumn)gv_rates.Columns["to_date"], "t_todate");
		var val_todate = Convert.ToDateTime(t_todate.Value).ToString("yyyy-MM-dd");

		if (val_todate == null || val_todate == ""  )
		{
			throw new Exception("Invalid End Date");
		}

		var membertype_id							= Toolbox.ReturnZeroIfNull_int(c.KeyValue); // 1, 2, 3, 4, 5
		double chargeout									= 0;
		double.TryParse(tb.Text, out chargeout);
		if(chargeout <= 0)
			{
			throw new Exception("Invalid Charge Out Rate");
			}
		// Not we need to cycle through the pay types
		var paytypes						= new List<string>(new string[]{"RT", "OT", "DT", "RTSP", "OTSP", "DTSP"});
		var paytype_id								= 1;
		var multiplier						= new List<double>(new double[]{1,1.5,2,1.1,1.65,2.2});
		foreach(var paytype in paytypes)
			{
			var base_chargeout_id					= Toolbox.doSQL_int(@"SELECT id 
FROM membertype_chargeout WHERE membertype_id = @v0 AND paytype_id = @v1 AND business_unit_id = @v2", new object[] {
			membertype_id, paytype_id, Session["rates_business_unit_id"]});
			var row_id								= Toolbox.doSQL_int(@"SELECT IFNULL(MAX(id),0) 
FROM customer_rate WHERE base_chargeout_id = @v0 AND customer_id = @v1", new object[] { base_chargeout_id, _cust_id});
			var charge				= new customer_chargeout();
			charge.id								= row_id;
			charge.chargeout						= chargeout * multiplier[paytype_id - 1];
			charge.base_chargeout_id				= base_chargeout_id;
			charge.customer_id						= _cust_id;
			charge.member_id						= current_user.id;
			charge.business_unit_id						= Toolbox.ReturnZeroIfNull_int(Session["rates_business_unit_id"]);
			charge.from_date						= Convert.ToDateTime(val_fromdate).Date;
			charge.to_date							= Convert.ToDateTime(val_todate).Date;

			charge.last_updated = System.DateTime.Today;
			charge.save();
			paytype_id++;
			}
		gv_rates.DataBind();
		}
	protected void sm_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
		{
		sm.AsyncPostBackErrorMessage	= e.Exception.Message;
		}
	protected void b_save_all_Click(object sender, EventArgs e)
		{
		var b							= (ASPxButton) sender;
		var g							= gv_rates;
		var start								= g.VisibleStartIndex;
		var end									= g.VisibleRowCount;
		for (var i = start; i < end; i++)
			{
			var row_id							= Toolbox.ReturnZeroIfNull_int(g.GetRowValues(i, new string[]{g.KeyFieldName}));
			var gvdc				= (GridViewDataColumn) g.Columns["chargeout"];
			var tb						= (ASPxTextBox) g.FindRowCellTemplateControl(i, gvdc, "t_chargeout");
			var base_chargeout_id				= Toolbox.ReturnZeroIfNull_int(gv_rates.GetRowValues(i, "base_chargeout_id"));
			double chargeout					= 0;
			double.TryParse(tb.Text, out chargeout);
			if(chargeout <= 0)
				{
				throw new Exception("Invalid Charge Out Rate - Row "+i);
				}
			else
				{
				var charge		= new customer_chargeout();
				charge.id						= row_id;
				charge.chargeout				= chargeout;
				charge.base_chargeout_id		= base_chargeout_id;
				charge.customer_id				= _cust_id;
				charge.member_id				= current_user.id;
				charge.business_unit_id				= Toolbox.ReturnZeroIfNull_int(Session["rates_business_unit_id"]);
				charge.last_updated = System.DateTime.Today;
				charge.save();
				}
			}
		gv_rates.DataBind();
		}
	protected void gv_rates_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.FieldName == "REG" && e.CellValue != null && gv_rates.GetRowValues(e.VisibleIndex, "norm") != null && gv_rates.GetRowValues(e.VisibleIndex, "norm") != DBNull.Value)
		{
			var norm		= Convert.ToDouble(gv_rates.GetRowValues(e.VisibleIndex, "norm"));
            var cellval = Toolbox.ReturnZeroIfNull_double(e.CellValue);
			if (cellval < norm)
			{
				e.Cell.BackColor = System.Drawing.Color.LightPink;
			}
			else if (cellval > norm)
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}
		}
	}
}

