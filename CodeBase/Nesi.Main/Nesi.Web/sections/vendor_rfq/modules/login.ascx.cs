using System;
using DevExpress.Web;
using nesi.core;

public partial class sections_vendor_rfq_modules_login : System.Web.UI.UserControl
	{
    protected void Page_Load(object sender, EventArgs e)
        {
        i_logo.ImageUrl = @"~\\images\\Logos\\nesi-logo-blue-invert.png";

        }
	protected void cbp_login_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var cb		= (ASPxCallbackPanel) sender;
		if(t_passcode.Text != "")
			{
			var passcode					= t_passcode.Text;
			var rv					= new rfq_vendor();
			var vendor_id					= 0;
			var rfq_header_id				= 0;
			var valid						= rv.validate(passcode, out rfq_header_id, out vendor_id);
			if(valid)
				{
				// Load vendor row
				rv							= new rfq_vendor(vendor_id, rfq_header_id);
				if(rv.date_opened == null) // Set the opened date of the rfq 
					{
					rv.set_vendor_opened();
					}
				if(rv.date_verified != null)
					{
					lb_error.Text	= "Thank you for verifying all information in this RFQ already. <br/>This keycode is no longer valid.";
					}
				else
					{
					var rf				= new VendorRFQ(rv.rfq_header_id);
					    i_logo.ImageUrl = @"~\\images\\Logos\\" + new NeBusinessUnit(rf.business_unit_id).logo_file;
                    if (rf.status == "Sent" || rf.status == "Open")
						{
						if(DateTime.Now.Date <= rf.date_close.Date) // Total length of allowed days this rfq is to be used
							{
							Session["rfq_passcode"]		= passcode;
							Session["rfq_vendor_id"]	= vendor_id;
							Session["rfq_header_id"]	= rfq_header_id;
							Response.RedirectLocation	= "index.aspx?review";
							}
						else // Expired
							{
							lb_error.Text = "This keycode has expired";
							}
						}
					else
						{
						lb_error.Text	= "We're sorry, this RFQ is no longer available for quoting.";
						}
					}
				}
			else
				{
				lb_error.Text = "Not a valid keycode";				
				}
			}
		}
	}
