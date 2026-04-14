using System;
using System.Linq;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_member_inventory_user_controls_part_transfer : System.Web.UI.UserControl
	{
	Toolbox _tools;
	public NeMember current_user { get; set; }
	private const int _page_id			= 43; // from Page table in DB
    public const string _page_name			= "Part_Transfer";
	public NeBusinessUnit WorkingBusinessUnit  { get; set; }
	public NeBusinessUnit WarehouseBusinessUnit  { get; set; }
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	

	protected void Page_Init(object sender, EventArgs e)
		{
	
		_tools								= new Toolbox();
		layout.DataBind();
		layout.__page_name = _page_name;
			layout.used_gv				= gv_transfer;
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		
		WorkingBusinessUnit						= new NeBusinessUnit(Session["working_business_unit_id"]);
		WarehouseBusinessUnit					= new NeBusinessUnit(Session["working_warehouse_bu_id"]);
//		hdn_cid.Value = WorkingBusinessUnit.id.ToString();
		if(!Page.IsPostBack && !Page.IsCallback)
			{
				Session["set_ddls_value"] = null;
                Session["gv_move_stock"] = null;
			}

       
        fill_gv_transfer();


        panel_export.Visible = true;
			
		}
	protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Visible)
			{
				NeGridLayouts gl;
				h.Set("gridview_id", "gv_transfer");
				ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
				ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
				if (!IsPostBack)
				{

					gl = new NeGridLayouts(current_user.id, _page_name);
					if (gl.GridLayout_Layout != "")
					{
						if (gv_transfer.Visible)
						{
							gv_transfer.LoadClientLayout(gl.GridLayout_Layout);
						}
						else
						{
							gv_transfer.LoadClientLayout(gl.GridLayout_Layout);
						}
					}
					else
					{
						gl = new NeGridLayouts();
						gl.GridLayout_Layout = gv_transfer.SaveClientLayout();
						gl.member_id = current_user.id;
						gl.GridLayout_Name = "Default";
						gl.GridLayout_Gridid = _page_name;
						gl.SaveGridLayout();
					}
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
					dde_filter.Text = gl.GridLayout_Name;
				}
			}
		}
	protected void Load_Layout(object sender, ASPxGridViewCustomCallbackEventArgs e)
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

	public void fill_gv_transfer()
	{
		if ((WorkingBusinessUnit != null)&& (Session["gv_move_stock"]==null))
		{
            //hdn_cid.Value = WorkingBusinessUnit.id.ToString();
            //sql.SelectParameters["?c_id"].DefaultValue = hdn_cid.Value.ToString();

            Session["gv_move_stock"] = _tools.getSQL_datatable(@"SELECT
		concat(a.master_id,'-',h.location_master_id) id,
		a.master_id,
		CONCAT(d.description, ' ', IF(cc.country = 'USA', i.abbr, j.abbr)) description,
		c.tag tag,
		hh.name location,
		h.location_master_id,
		h.min min_qty,
		h.max max_qty,
		h.qty,
		IFNULL(h.qty * (b.dollar_balance / b.onhand_qty), 0) dollar_balance ,
		hh.type_id intext,
		hh.id loc_master_id
	FROM
		inventory_item_master a
	LEFT JOIN
		inventory_branch b ON
			a.master_id = b.master_id AND
			b.business_unit_id = @v0
	LEFT join 
		business_unit cc on cc.id = @v0 
	LEFT JOIN
		inventory_tag c ON
			a.tag_id = c.tag_id
	LEFT JOIN
		inventory_description d ON
			a.master_id = d.master_id
	LEFT JOIN
		inventory_location h ON
			a.master_id = h.master_id AND
			h.business_unit_id = cc.id
	LEFT JOIN
		inventory_location_master hh ON
			hh.id = h.location_master_id AND
			hh.business_unit_id = cc.id
	LEFT JOIN
		inventory_sold_as i ON
			c.usa_sold_as = i.id
	LEFT JOIN
		inventory_sold_as j ON
			c.canadian_sold_as = j.id

	WHERE
		a.active = TRUE AND
		c.is_exclude = FALSE AND
		a.master_id NOT IN (666,777) AND
    h.location_master_id IS NOT NULL AND
    h.location_master_id != 0 AND
	h.qty != 0
	ORDER BY master_id", new object[] { Session["working_warehouse_bu_id"] });

			
		}
        gv_transfer.DataSource = Session["gv_move_stock"];
        gv_transfer.DataBind();
	}

	protected void gv_transfer_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters != "")
		{
			if (e.Parameters.Equals("set_min_to_zero"))
			{
				for (var x = 0; x < (gv_transfer.VisibleRowCount >= gv_transfer.SettingsPager.PageSize ? gv_transfer.SettingsPager.PageSize : gv_transfer.VisibleRowCount); x++)
				{
					var master_id = Convert.ToInt32(gv_transfer.GetRowValues(x, "master_id"));
					var _from = Convert.ToInt32(gv_transfer.GetRowValues(x, "location_master_id"));
					var ib = new branch(master_id, WarehouseBusinessUnit.id);
					var il = new location(_from, WarehouseBusinessUnit.id, master_id);
					
					il.min = 0;
					il.member_id = current_user.id;
					il.save();
				}
                Session["gv_move_stock"] = null;
                fill_gv_transfer();
			}
			else if (e.Parameters.Equals("set_max_to_zero"))
			{
				for (var x = 0; x < (gv_transfer.VisibleRowCount >= gv_transfer.SettingsPager.PageSize ? gv_transfer.SettingsPager.PageSize : gv_transfer.VisibleRowCount); x++)
				{
					var master_id = Convert.ToInt32(gv_transfer.GetRowValues(x, "master_id"));
					var _from = Convert.ToInt32(gv_transfer.GetRowValues(x, "location_master_id"));
					var ib = new branch(master_id, WarehouseBusinessUnit.id);
					var il = new location(_from, WarehouseBusinessUnit.id, master_id);
					
					il.max = 0;
					il.member_id = current_user.id;
					il.save();
				}
                Session["gv_move_stock"] = null;
                fill_gv_transfer();

            }
            else if (e.Parameters.Contains('T'))
			{
			//	Session["set_ddls_value"] = e.Parameters.Split('|').GetValue(1).ToString();
				for (var x = 0; x < (gv_transfer.VisibleRowCount>=gv_transfer.SettingsPager.PageSize?gv_transfer.SettingsPager.PageSize:gv_transfer.VisibleRowCount); x++)
				{
					
					var c = (ASPxComboBox)gv_transfer.FindRowCellTemplateControl(x, (GridViewDataColumn)gv_transfer.Columns["To Location"], "to_" + x);
					c.Value = Convert.ToInt32(e.Parameters.Split('|').GetValue(1).ToString());
				}
		//		int _to = Convert.ToInt32(e.Parameters.Split('|').GetValue(1));
				//foreach (gv_transfer.FindRowCellTemplateControlByKey
		//		int x = gv_transfer.VisibleRowCount-1;
		//		while (x>0)
		//		{
		//			ASPxComboBox cb = (ASPxComboBox)gv_transfer.FindRowCellTemplateControl(x, gv_transfer.Columns["_to"] as GridViewDataColumn, "ddl" + gv_transfer.GetRowValues(x, "id"));
		//			cb.Value = _to;
		//			x--;

				//}
			}
			else if (e.Parameters.Contains("move"))
			{
                foreach (string _row in e.Parameters.Split('~'))
                {if (_row != "")
                    {
                        var txt_to = _row.Split('|').GetValue(2).ToString();
                        var txt_qty = _row.Split('|').GetValue(1).ToString();
                        var v_index = Convert.ToInt32(_row.Split('|').GetValue(3));
                        var _from = Convert.ToInt32(gv_transfer.GetRowValues(v_index, "location_master_id"));
                        var master_id = Convert.ToInt32(gv_transfer.GetRowValues(v_index, "master_id"));

                        if ((txt_qty != null) && (txt_to != null) && (_from != 0))
                        {
                            var il = new location();
                            il.xfer_using_master_id(Convert.ToInt32(master_id), WarehouseBusinessUnit.id, _from, Convert.ToInt32(txt_to), Convert.ToDouble(txt_qty), current_user);

                        }
                    }
                }
                Session["gv_move_stock"] = null;
                fill_gv_transfer();
              //  gv_transfer.JSProperties["cp_refresh"] = "1";
			}
            else if (e.Parameters.Contains("refresh"))
            {
                Session["gv_move_stock"] = null;
                fill_gv_transfer();
            }
			else if (e.Parameters.Contains("ALL"))
			{
				for (var x = 0; x < (gv_transfer.VisibleRowCount >= gv_transfer.SettingsPager.PageSize ? gv_transfer.SettingsPager.PageSize : gv_transfer.VisibleRowCount); x++)
				{
					var t = (ASPxTextBox)gv_transfer.FindRowCellTemplateControl(x, (GridViewDataColumn)gv_transfer.Columns["Qty to Move"], "qty_" + x);
					var c = (ASPxComboBox)gv_transfer.FindRowCellTemplateControl(x, (GridViewDataColumn)gv_transfer.Columns["To Location"], "to_" + x);
					if ((t != null && t.Text != "0" && t.Text!="" && c!=null && c.Value.ToString()!="0" && c.Value.ToString()!=""))
					{
						var master_id = Convert.ToInt32(gv_transfer.GetRowValues(x, "master_id"));
						var _from = Convert.ToInt32(gv_transfer.GetRowValues(x, "location_master_id"));
						var il = new location();
					//	il.xfer_using_master_id(Convert.ToInt32(master_id), Convert.ToInt32(hdn_cid.Value), _from, Convert.ToInt32(c.Value), Convert.ToDouble(t.Text), current_user);
						
					}

				}
                Session["gv_move_stock"] = null;
                fill_gv_transfer();


            }
        }
	}
	
	protected void cb_to_Init(object sender, EventArgs e)
	{
		var cc = sender as ASPxComboBox;
		var container = cc.NamingContainer as GridViewDataItemTemplateContainer;
		if ((Session["set_ddls_value"] == null)||(Session["set_ddls_value"].ToString() == "null"))
		{
			cc.SelectedIndex = -1;
		}
		else
		{
			cc.Value = Convert.ToInt32(Session["set_ddls_value"]);
		}
		cc.ID = string.Format("to_{0}", container.VisibleIndex);
		cc.ClientInstanceName = string.Format("to_{0}", container.VisibleIndex);
	}
	protected void ddl_to_Init(object sender, EventArgs e)
	{

	}

	protected void txt_qty_to_move_Init(object sender, EventArgs e)
	{
		var tb = (ASPxTextBox)sender;
		var container = tb.NamingContainer as GridViewDataItemTemplateContainer;
		tb.ID = string.Format("qty_{0}", container.VisibleIndex);
		tb.ClientInstanceName = string.Format("qty_{0}", container.VisibleIndex);
        if (Convert.ToDouble(gv_transfer.GetRowValues(container.VisibleIndex,"qty"))<=0)
        {
            tb.ClientEnabled = false;
        }
	}
	protected void btn_move_one_Init(object sender, EventArgs e)
	{
		var b = (ASPxButton)sender;
		var container = b.NamingContainer as GridViewDataItemTemplateContainer;
		b.ID = string.Format("b_{0}", container.VisibleIndex);
		b.ClientInstanceName = string.Format("b_{0}", container.VisibleIndex);
		b.ClientSideEvents.Click = "function(s, e) {{ move_all(" + container.VisibleIndex + "); }}";
        if (Convert.ToDouble(gv_transfer.GetRowValues(container.VisibleIndex, "qty")) <= 0)
        {
            b.ClientVisible = false;
        }
    }
   

    protected void gv_transfer_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
}