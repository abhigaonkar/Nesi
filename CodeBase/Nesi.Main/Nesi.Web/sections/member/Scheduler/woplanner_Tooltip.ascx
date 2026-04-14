<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_woplanner_Tooltip" Codebehind="woplanner_Tooltip.ascx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>






<style type="text/css">
	.style2
	{
		font-weight: bold;
	}
	.dxtcLeftAlignCell
	{
		text-align: left;
	}
	.newStyle1
	{
		text-align: left;
	}
	</style>
<script type="text/javascript" id="dxss_cat">
    // <![CDATA[
	ASPxClientAppointmentToolTip = ASPx.CreateClass(ASPxClientToolTipBase,
	{
		Update: function (data) {
			var apt = data.GetAppointment();
			this.apt = apt;
			if (!apt.updated) {
				//			this.scheduler.RefreshClientAppointmentProperties(apt, AppointmentPropertyNames.Normal, _aspxCreateDelegate(this.OnAppointmentRefresh, this));
				//			this.controls.lblInterval.SetText("Loading...");		
	//			cb.SetVisible(false);
				this.controls.lbltype.SetText(apt.cptype);
				this.controls.lblInterval.SetText(apt.cpinterval);
				
//				cb.PerformCallback(apt.cpaptid);

			}
			else {

							this.controls.lblInterval.SetText("stuff");
			}





			//			this.controls.lblhoursexp.SetText(apt.cphoursexp);
			//			this.controls.lblhourslabel.SetText(apt.cplblhourslabel);



		},
		OnAppointmentRefresh: function (apt) {
			apt.updated = true;

		}
	});
	
	
    // ]]> 
</script>
			
				 <table style="width: 400px;">
					 <tr>
						 <td style="padding: 5px 3px 3px 3px; background-color: #008000" valign="middle">
				<dx:ASPxLabel runat="server" ClientInstanceName="lbltype" ID="lbltype" 
								 style="font-family: Arial, Helvetica, sans-serif" BackColor="Transparent" 
								 Font-Bold="True" Font-Names="Arial" ForeColor="White" Height="20px" 
								 Width="100%"></dx:ASPxLabel>

						 </td>
					 </tr>
					 <tr>
						 <td>
				<dx:ASPxLabel runat="server" ClientInstanceName="lblInterval" EnableClientSideAPI="True" 
								 CssClass="style2" Font-Bold="True" Font-Names="Arial" ID="lblInterval" 
								 style="font-family: Arial, Helvetica, sans-serif"></dx:ASPxLabel>

						 </td>
					 </tr>
					 <tr>
						 <td>
			 				&nbsp;</td>
					 </tr>
				 </table>
				 


			 
