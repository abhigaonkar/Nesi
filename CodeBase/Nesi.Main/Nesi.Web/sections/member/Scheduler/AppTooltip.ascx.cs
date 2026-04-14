using System;
using System.Data;
using System.Web.UI;
using DevExpress.Web.ASPxScheduler;
using nesi.core;

public partial class sections_member_scheduler_AppTooltip : ASPxSchedulerToolTipBase
{
	public override bool ToolTipShowStem { get { return false; } }
	public override string ClassName { get { return "ASPxClientAppointmentToolTip"; } }
	protected override void OnLoad(EventArgs e)
	{
	//	base.OnLoad(e);
if (!IsPostBack)
{

}
else
{

}
	}
	protected override Control[] GetChildControls()
	{
		var controls = new Control[] { lblInterval, lblwodesc, lblcustomer, lblhours,lblhoursexp,lbltype };
	
		return controls;
	
	}


	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var x = e.Parameter;
		cb.JSProperties["cp_open"] = "0";
	
			try
			{
				if (x != "" & x != "0")
				{
					var apt = new NeAppointment(Convert.ToInt32(x));
                    if (apt.Status>99 && apt.Status<106)
                    {
                        tbl_details.Visible = false;
                        return;
                    }
                    tbl_details.Visible = true;
					hdn_cid.Value = apt.business_unit_id.ToString();
					lblstart.Text = apt.StartTime.ToString("HH:mm");
					lblend.Text = apt.EndTime.ToString("HH:mm");
					lblInterval.Text = apt.StartTime.ToString("MMM dd  h:mm tt") + " to " + apt.EndTime.ToString("h:mm tt");
					if (apt.member_id > 100000000)
					{
						lblresource.Text = Toolbox.doSQL_string(@"Select membertype_name from membertype where membertype_id =@v0 " , apt.membertype_id);
					}
					else if (apt.member_id != 0)
					{
						lblresource.Text = Toolbox.doSQL_string("Select member_fullname from member where member_id =@v0 " ,apt.member_id);
					}
                    if (apt.setby != null && apt.setby != 0)
					{
						lblsetby.Text = Toolbox.doSQL_string(@"Select member_fullname from member where member_id =@v0 " , apt.setby);
					}
					cb.JSProperties["cp_open"] = "1";

					if (apt.woprog_id != 0)
					{
						var wo = new NeWOProg(Convert.ToInt32(apt.woprog_id));
						var addr = new NEAddress(wo.woprog_Address_ID);
						hl.Text = wo.OrderNumber;
						hl.NavigateUrl = string.Format("javascript:boing('/sections/workorder/index.aspx?woprog_id={0}&business_unit_id={1}', 'workorder', 1200,750)", wo.woprog_id, wo.business_unit_id);
						lbladdress.Text = addr.Addr1 + ", " + addr.City;
                        lbllocation.Text = wo.woprog_Location_in_plant;
                        lblcustomer.Text = wo.CustomerName;
						lblwodesc.Text = wo.Description;
						lbltruck.Text = Toolbox.doSQL_string(@"Select ifnull((Select concat('Truck ',assets_no) from assets where assets_id = @v0),'Not Set')", apt.assetid);
						lblfuturehours.Text = Toolbox.doSQL_double(@"Select ifnull((Select SUM(TIMESTAMPDIFF(MINUTE,appointments.StartDate,appointments.EndDate)/60) from appointments  where appointments.woprog_id =@v0 and appointments.startdate>=curdate()),0)", new object[] { wo.woprog_id }).ToString("N1");
						lblhours.Text = Toolbox.doSQL_double(@"Select ifnull((Select sum(membertime.NumberOfHours) from membertime  where MemberTime_WOProg_id =@v0 and WOType = 'WO'),0)", new object[] { wo.woprog_id }).ToString("N1");
						var dt1 = Toolbox.doSQL_dt(@"Select member_fullname from member,appointments  where date(appointments.startdate) =@v0 and appointments.woprog_id =@v1  and appointments.status = 0  AND member.member_status = 'Active' and appointments.member_id = member.member_id and member.member_id !=@v2 ", new object[] { apt.StartTime.ToString("yyyy-MM-dd"),apt.woprog_id,apt.member_id });
						if (dt1.Rows.Count > 0)
						{
							foreach (DataRow dr1 in dt1.Rows)
							{
								lbl_whoelse.Text += dr1[0] + System.Environment.NewLine;
							}
						}
						lblhoursexp.Text = wo.woprog_exp_labor.ToString("N0");
						hdn_aptid.Value = x;
					}
					else if (apt.quote_id != 0)
					{
						var quote = new quote(Convert.ToInt32(apt.quote_id));
						var addr = new NEAddress(quote.address_id);
						hl.Text = quote.QuoteID + " R" + quote.Revision;
						//boing("/sections/member/quote/index.aspx?a=g&quote_id=" + id + "&revision=" + rev, "quote", 1035, 800);
						hl.NavigateUrl = string.Format("javascript:boing('/sections/member/quote/index.aspx?a=g&quote_id={0}&revision={1}', 'quote', 1035,800)", quote.QuoteID, quote.Revision);
						lbladdress.Text = addr.Addr1 + ", " + addr.City;
						lblcustomer.Text = quote.txtCustomerName;
						lblwodesc.Text = quote.txtJobDescription;
						lbltruck.Text = "";
						lblfuturehours.Text = Toolbox.doSQL_double(@"Select ifnull((Select SUM(TIMESTAMPDIFF(MINUTE,appointments.StartDate,appointments.EndDate)/60) from appointments  where appointments.quote_id =@v0 and appointments.startdate>=curdate()),0)", new object[] { quote.QuoteID }).ToString("N1");
						lblhours.Text = Toolbox.doSQL_double(@"Select Get_QuotedHours(@v0,@v1)",new object[] { quote.QuoteID,quote.Revision } ).ToString("N1");
						lblhoursexp.Text = "0";
						hdn_aptid.Value = x;


					}
				}
                else
                {
                    tbl_details.Visible = false;
                }
			}
			catch { }
		
	}
}