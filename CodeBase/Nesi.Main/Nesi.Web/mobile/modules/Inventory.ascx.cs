using System;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using nesi.core;

public partial class mobile_modules_inventory : System.Web.UI.UserControl
	{
		Toolbox _tools;
		NeMember current_user;
		JavaScriptSerializer jSON = new JavaScriptSerializer();
	protected void Page_Load(object sender, EventArgs e)
		{

			_tools = new Toolbox();
			current_user = Toolbox.do_handle_authentication(1);
			var _q = Request.QueryString;
			var parameter = Session["inventory_master_id"]!=null?Session["inventory_master_id"].ToString():null;
			if (parameter != null)
			{
				hdnmasterid.Value = parameter;
				load_general_tab();
			}
			else
			{
				ASPxPageControl1.TabPages[0].ClientVisible = false;
			}
	

		}

	protected void load_general_tab()
	{
		try
		{
			var inventory = new inventory();
			inventory.Load(hdnmasterid.Value, current_user.business_unit_id);
			lblmasterid.Text = inventory.master_id;
			lbldesc.Text = inventory.description_int;
			lblstock.Text = _tools.getSQL_double(@"SELECT IFNULL(SUM(a.qty), 0) FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = @v0  AND a.master_id = @v1  and b.type_id = 1", new object[] {  current_user.business_unit.warehouse_bu_id, inventory.master_id } ).ToString();

			lbllocation.Text = Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location_master b LEFT JOIN inventory_location a ON b.id = a.location_master_id AND a.master_id = @v1  WHERE b.business_unit_id = @v0  AND b.id = @v2  ORDER BY qty DESC", new object[] {  current_user.business_unit.warehouse_bu_id, Session["inventory_master_id"], current_user.member_default_location } ).ToString();
			ASPxPageControl1.TabPages[0].ClientVisible = true;
		}
		catch { }
				
				

	}

	protected void txtbc_TextChanged(object sender, EventArgs e)
	{
		if (txtbc.Text.Substring(0, 4) == "001-")
		{

			hdnmasterid.Value = txtbc.Text.Substring(4, (txtbc.Text.Length-4));
			Session["inventory_master_id"] = hdnmasterid.Value;
			load_general_tab();
		}
		else if (txtbc.Text.Length == 5)
		{
			hdnmasterid.Value = txtbc.Text;
			Session["inventory_master_id"] = hdnmasterid.Value;
			load_general_tab();

		}
		else
		{
			hdnmasterid.Value = "0";

		}

	}
	protected void ddlwo_SelectedIndexChanged(object sender, EventArgs e)
	{
		
		
	}
	
}