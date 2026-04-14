using System;
using System.Data;
using nesi.core;

public partial class markup_popup : System.Web.UI.Page
	{
	NeMember myMember;
	Toolbox _Tools = new Toolbox();
	const int _page_id = 89;
	double base_cost = 0;
	double qty_cost = 0;
	double qty = 0;
	protected void Page_Load(object sender, EventArgs e)
		{
		myMember = Toolbox.do_handle_authentication(_page_id);
        ddl_bu.DataSource = _Tools.getSQL_datatable("Select id, ddl_name from business_unit where find_in_set(id,'" + new Current_User().visible_business_units + "') order by ddl_name", null);
        ddl_bu.DataBind();

		if (!IsPostBack)
			{
            ddl_bu.Value = myMember.business_unit_id;
			lblMargin.Text = "0.0%";
			lblQtySell.Text = "$0.00";
			lblSell.Text = "$0.00";
			txtCost.Text = "";
			txtQty.Text = "";
			txtQtyCost.Text = "";
			txtQtySell.Text = "";
			lblqtytotalsell.Text = "$ 0.00";
			}
		else
			{
			double.TryParse(txtCost.Text, out base_cost);
			double.TryParse(txtQtyCost.Text, out qty_cost);
			double.TryParse(txtQty.Text, out qty);
			}
		}

	protected void txtCost_TextChanged2(object sender, EventArgs e)
		{
		try
			{
			var temp = _Tools.Round(shared.GetSellPrice(base_cost, 0, "QTY", 1, Convert.ToInt32(ddl_bu.Value)), 2);
			var margin = _Tools.Round((temp - base_cost) / temp, 2);
			lblMargin.Text = margin.ToString();
			lblSell.Text = temp.ToString("$#,###.00");
			}
		catch
			{
			lblSell.Text = "$0.00";
			}

		}
	protected void txtQtyCost_TextChanged(object sender, EventArgs e)
		{
		try
			{
			var temp = _Tools.Round(shared.GetSellPrice(qty_cost, 0, "QTY", 1, Convert.ToInt32(ddl_bu.Value)), 2);
			txtQtySell.Text = temp.ToString("$#,###.00");

			temp = _Tools.Round(shared.GetSellPrice(qty_cost, 0, "QTY", qty, Convert.ToInt32(ddl_bu.Value)), 2);
			lblQtySell.Text = temp.ToString("$#,###.00");
			lblqtytotalsell.Text = Convert.ToDouble(temp * qty).ToString("$#,####.00");
			txtQty.Focus();
			}
		catch
			{
			lblQtySell.Text = "$0.00";
			lblqtytotalsell.Text = "$0.00";
			txtQtyCost.Focus();

			}
		}
	protected void txtQty_TextChanged(object sender, EventArgs e)
		{
		try
			{
			var temp = _Tools.Round(shared.GetSellPrice(qty_cost, 0, "QTY", qty, Convert.ToInt32(ddl_bu.Value)), 2);
			lblQtySell.Text = temp.ToString("$#,###.00");
			lblqtytotalsell.Text = (temp * qty).ToString("$#,####.00");
			}
		catch
			{
			lblQtySell.Text = "$0.00";
			lblqtytotalsell.Text = "$0.00";
			}

		}
	}
