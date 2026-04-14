using System;
using System.Data;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Web.Script.Serialization;
using System.Text;
using nesi.core;

public partial class sections_vendor_rfq_index : System.Web.UI.Page
	{
    protected void Page_Load(object sender, EventArgs e)
		{
		var _tools				= new Toolbox();
		_tools.add_css("style.css");
		var _f		= Request.Form;
		var _q		= Request.QueryString;
		var jSON	= new JavaScriptSerializer();
		if(!isAuthenticated())
			{
			mod_login.Visible		= true;
			if(!string.IsNullOrEmpty(_q["passcode"]) && !isAuthenticated())
				{
				var cbp_login	= (ASPxCallbackPanel) mod_login.FindControl("cbp_login");
				var t_passcode		= (ASPxTextBox) cbp_login.FindControl("t_passcode");
				t_passcode.Text				= _q["passcode"];
				}
			return;
			}
		switch(Request.HttpMethod)
			{
			#region POST
			case "POST":
				if(!string.IsNullOrEmpty(_f["a"]))
					{
					Response.Clear();
					switch(_f["a"])
						{
						case "handle_mass_save":
							var items	= jSON.Deserialize<System.Collections.Generic.IList<item>>(_f["li"]);
							for(var i = 0; i < items.Count; i++)
								{
								var li		= new rfq_lineitem(Convert.ToInt32(items[i].id));
								li.vendor_price		= Convert.ToDouble(items[i].cost);
								li.qty				= Convert.ToDouble(items[i].qty);
								li.vendor_code		= items[i].code;
								li.lead_time		= Convert.ToInt32(items[i].lead);
								li.save();
								}
							Response.Write("SUCCESS");
						break;
						}
					Response.End();
					}
			break;
			#endregion POST
			#region GET
			case "GET":
				if(!string.IsNullOrEmpty(_q["a"]))
					{
					Response.Clear();
					switch(_q["a"])
						{
						case "handle_note":
							try
								{
								var note				= _q["note"];
								var id					= Convert.ToInt32(_q["id"]);
								var rv			= new rfq_lineitem(id);
								rv.vendor_note			= note;
								rv.save();
								Response.Write("SUCCESS");
								}
							catch (Exception ee)
								{
								_tools.current_user	= new NeMember(711);
								_tools.catch_error(ee);
								throw new Exception("We are sorry, we're having technical difficulties. Our support team has been alerted of this failure.");
								}
						break;
						case "handle_rowsave":
							try 
								{	        
								var cost			= Convert.ToDouble(_q["cost"]);
								var qty			= Convert.ToDouble(_q["qty"]);
								var code			= _q["code"];
								var id				= Convert.ToInt32(_q["id"]);
								var lead			= Convert.ToInt32(_q["lead"]);
								var li		= new rfq_lineitem(id);
								li.vendor_price		= cost;
								li.qty				= qty;
								li.vendor_code		= code;
								li.lead_time		= lead;
								li.save();
								Response.Write("SUCCESS");
								}
							catch (Exception ee)
								{
								_tools.current_user	= new NeMember(711);
								_tools.catch_error(ee);
								throw new Exception("We are sorry, we're having technical difficulties. Our support team has been alerted of this failure.");
								}
						break;
						case "handle_verify":
							try 
								{
								var rf				= new VendorRFQ((int) Session["rfq_header_id"]);
								var em_purchaser		= new NeEMail();
								em_purchaser.To				= new NeMember(rf.member_id).NEEmail;
								var ven				= new NEVendor((int) Session["rfq_vendor_id"]);
								var rfven			= new rfq_vendor(ven.ID, rf.id);
								em_purchaser.From			= "administrator@thatsnew.com";
								em_purchaser.Subject		= string.Format("The information on RFQ #{0} for {1}, has been verified", rf.id, _tools.value_from(ven.Vendor_Name, false));
								em_purchaser.Body			= "";
								em_purchaser.isHTML			= false;
								em_purchaser.Send();
								rfven.set_vendor_verified();
								try
									{
									// Send Vendor a copy of what they quoted
									var this_contact		= new NEContact(rfven.contact_id);
									var em_vendor			= new NeEMail();
									em_vendor.To				= this_contact.Contact_Email;
									em_vendor.From				= new NeMember(rf.member_id).NEEmail;
									em_vendor.Subject			= string.Format("Thank you for quoting on RFQ #{0}", rf.id);
									var this_body		= new StringBuilder();
									this_body.Append(string.Format(@"
<div style='font-family:Arial, Helvetica, sans-serif;font-size:13px;'>Thank you for quoting on RFQ #{0}.
<br/>
Below is a report of the information you provided.
<hr/></div>
<table width='100%' cellpadding='2' cellspacing='0' style='font-family:Arial, Helvetica, sans-serif;font-size:13px;border:solid 1px #ccc;'>
	<thead>
		<tr style='background-color:#000;color:#fff;'>
			<th align='center'>Our Part #</th>
			<th align='center'>Description</th>
			<th align='center'>Your Part #</th>
			<th align='center'>Amount Requested</th>
			<th align='center'>Package Quantity</th>
			<th align='center'>Lead Time (days)</th>
			<th align='center'>Your Price</th>
		</tr>
	</thead>
	<tbody>", rf.id));
									var part_list			= Toolbox.doSQL_dt(@" SELECT a.id, a.vendor_code, a.master_id, FULL_PART_DESCRIPTION(a.master_id, true, c.country) description, IF(IFNULL(a.qty, 0) = 0, 1, a.qty) qty, rp.qty rfq_qty, a.lead_time lead_time, vendor_price FROM rfq_lineitem a LEFT JOIN rfq_header b ON a.rfq_header_id = b.id LEFT join business_unit c ON b.business_unit_id = c.id LEFT JOIN rfq_part_list rp ON a.master_id = rp.master_id AND a.rfq_header_id = rp.rfq_header_id WHERE a.rfq_header_id = @v0  AND a.vendor_id = @v1 ", new object[] {  rf.id, Session["rfq_vendor_id"] } );
									foreach(DataRow _p in part_list.Rows)
										{
										var this_part_number			= _p["master_id"];
										object this_vendor_code			= Toolbox.do_value_from(_p["vendor_code"], true);
										var this_description			= Toolbox.do_value_from(_p["description"], true);
										var this_qty					= Convert.ToDouble(_p["qty"]);
										var this_rfq_qty				= Convert.ToDouble(_p["rfq_qty"]);
										var this_lead_time				= Convert.ToInt32(_p["lead_time"]);
										var this_price				= Convert.ToDouble(_p["vendor_price"]);

										this_body.Append(string.Format(@"
		<tr>
			<td align='center' style='border: solid 1px #ccc;'>{0}</td>
			<td align='center' style='border: solid 1px #ccc;'>{1}</td>
			<td align='center' style='border: solid 1px #ccc;'>{2}</td>
			<td align='center' style='border: solid 1px #ccc;'>{4}</td>
			<td align='center' style='border: solid 1px #ccc;'>{3}</td>
			<td align='center' style='border: solid 1px #ccc;'>{5}</td>
			<td align='center' style='border: solid 1px #ccc;'>{6:C5}</td>
		</tr>
",
										this_part_number, 
										this_vendor_code, 
										this_description, 
										this_qty, 
										this_rfq_qty, 
										this_lead_time, 
										this_price
										));
										}
									this_body.Append("</tbody></table>");
									em_vendor.Body						= this_body.ToString();
									em_vendor.isHTML					= true;
									em_vendor.Send();
									}
								catch (Exception ee)
									{
									_tools.catch_error(ee);
									}
								Response.Write("SUCCESS");
								Session.Remove("rfq_passcode");
								Session.Remove("rfq_header_id");
								Session.Remove("rfq_vendor_id");
								}
							catch (Exception ee)
								{
								_tools.current_user	= new NeMember(711);
								_tools.catch_error(ee);
								throw new Exception("We are sorry, we're having technical difficulties. Our support team has been alerted of this failure.");
								}
						break;
						}
					Response.End();
					}
			break;
			#endregion GET
			}
		if(_q.Count == 0 || !string.IsNullOrEmpty(_q["passcode"]))
			{
			Response.Redirect("./index.aspx?review");
			}
		else if(string.IsNullOrEmpty(_q["passcode"]))
			{
			switch(_q[0])
				{
				#region review
				case "review":
					mod_menu.Visible			= true;
					mod_review.Visible			= true;
					var vendor_id				= (int) Session["rfq_vendor_id"];
					var rfq				= new VendorRFQ((int) Session["rfq_header_id"]);
					var business_unit_id				= rfq.business_unit_id;
					var _vendor			= new NEVendor(vendor_id);
					var rv				= new rfq_vendor(vendor_id, rfq.id);
					var _contact			= new NEContact(rv.contact_id);
					var cbp_menu	= (ASPxCallbackPanel) mod_menu.Controls[0];
					//ASPxLabel lbl_welcome		= (ASPxLabel) cbp_menu.FindControl("lbl_vendor_name");
					var lbl_rfq_info		= (ASPxLabel) cbp_menu.FindControl("lbl_rfq_info");
					var lbl_rfq_number	= (ASPxLabel) cbp_menu.FindControl("lbl_rfq_number");
					var lbl_branch_info	= (ASPxLabel) cbp_menu.FindControl("lbl_branch_info");
					//lbl_welcome.Text			= _vendor.Vendor_Name;
					lbl_rfq_number.Text			= "RFQ #: "+rfq.id.ToString().PadLeft(5, '0')+"<br/>"+"<div style='margin-left:15px;font-size:12px;'>Expires: "+rfq.date_close.ToString("yyyy-MM-dd")+"</div>";
					var co				= new NeBusinessUnit(business_unit_id);
					lbl_branch_info.Text		= string.Format(@"
<b>{0}</b>
{1}<br/>
{2}, {3} {4}<br/>
{5}
", co.name, co.address, co.city, co.provstate, co.postal, co.PhoneNumber);
					lbl_rfq_info.Text			= string.Format(@"
<b>{0}</b>
{1}<br/>
{2}, {3} {4}<br/>
<strong style='color:#f00;'>ATTN: {5}</strong>
", _vendor.Vendor_Name, _tools.value_from(_vendor.Address.Addr1, false), _vendor.Address.City, _vendor.Address.Prov, _vendor.Address.Postal, _contact.Contact_Name);
				break;
				#endregion review
				#region logoff
				case "logoff":
					Session.Remove("rfq_passcode");
					Session.Remove("rfq_header_id");
					Session.Remove("rfq_vendor_id");
					Response.Redirect("./index.aspx", true);
				break;
				#endregion logoff
				}
			}
		}
	protected bool isAuthenticated()
		{
		return (Session["rfq_passcode"] != null);
		}
	protected class item
		{
		public string id { get; set; }
		public string code { get; set; }
		public string qty { get; set; }
		public string cost { get; set; }
		public string lead { get; set; }
		}
	}
