using System;
using System.Data;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_customer_index2 : System.Web.UI.Page
{
	public Toolbox _tools = new Toolbox();
	protected NameValueCollection _q;
	private const int _page_id = 10;
	private string _cust_id = "";

	protected void Page_Init(object sender, EventArgs e)
	{
//		myMember = Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();
	}
    protected void Page_Load(object sender, EventArgs e)
    {
		_q = Request.QueryString;
		if (!string.IsNullOrEmpty(_q["customer_id"]))
		{
			_cust_id = _q["customer_id"];
		}
		else
		{
			_cust_id = "0";
		}
		if (!IsPostBack)
		{
			Session["working _customer_list"] = null;
		
		}
		

    }
	protected void fill_top_frame()
	{
		ddl_top_selector.DataBind();
		if (Session["working _customer_list"] != null)
		{
			if (pg.PageIndex >= 0)
			{
				var dt = (DataTable)Session["working _customer_list"];
				if (dt.Rows.Count > 0)
				{
					ddl_top_selector.SelectedIndex = pg.PageIndex;
					var dr = dt.Rows[pg.PageIndex];
					var cust = new NECustomer(Convert.ToInt32(dr["Customer_ID"]));
					txtcustname.Text = cust.Customer_Name;
					hl_website.Text = cust.Address.Web;
					hl_website.NavigateUrl = string.Format(@"javascript:boing('http://{0}', 'Customer_web', 1200,800)", cust.Address.Web);

					if ((dr["Contact_id"] != DBNull.Value)&&(Convert.ToString(dr["Contact_id"]) != ""))
					{
						var contact = new NEContact(Convert.ToInt32(dr["Contact_id"]));
						txtcontact_name.Text = contact.name;
						ddltitle.Text = contact.Contact_Title;
					}
					else
					{
						clear_contact();
					}
				}

			}
		}
	}

	protected void clear_contact()
	{
		txtcontact_name.Text = "";
		ddltitle.Text = "";
	}

	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{

	}
	protected void btneditSearch_ButtonClick(object source, ButtonEditClickEventArgs e)
	{
		DataTable dt = null;
		if (ddlsearchtype.Text == "Customer")
		{
			dt = _tools.getSQL_datatable(@"SELECT customer.Customer_Name, customer.Customer_ID, contact.Contact_ID, Concat(customer.Customer_ID,'|',contact.Contact_ID) id, Concat(customer.Customer_Name,' - ',contact.contact_name) name FROM contact Right JOIN customer ON contact.Contact_Cust_ID = customer.Customer_ID  where customer.Customer_Name like CONCAT('%',@v0,'%')  order by customer.Customer_Name", new object[] { btneditSearch.Text });
		}
		else if (ddlsearchtype.Text == "Contact")
		{
			dt = _tools.getSQL_datatable(@"SELECT customer.Customer_Name, customer.Customer_ID, contact.Contact_ID, Concat(customer.Customer_ID,'|',contact.Contact_ID) id, Concat(customer.Customer_Name,' - ',contact.contact_name) name FROM contact Right JOIN customer ON contact.Contact_Cust_ID = customer.Customer_ID  where Contact.Contact_Name like CONCAT('%',@v0,'%') order by Contact.Contact_Name", new object[] { btneditSearch.Text });
		}
		Session["working _customer_list"] = dt;

		if (dt.Rows.Count > 0)
		{
			ddl_top_selector.DataSource = dt;
			ddl_top_selector.DataBind();
			pg.ItemCount = dt.Rows.Count;
			pg.DataBind();
			fill_top_frame();
			ddl_top_selector.ClientVisible = true;
		}
	}
	protected void pg_PageIndexChanged(object sender, EventArgs e)
	{
		fill_top_frame();
	}
	protected void hl_website_Init(object sender, EventArgs e)
	{
		var hl = (ASPxHyperLink)sender;
		
	}
	protected void ddl_top_selector_SelectedIndexChanged(object sender, EventArgs e)
	{
		pg.PageIndex = ddl_top_selector.SelectedIndex;
		fill_top_frame();

	}
}