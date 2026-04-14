<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_AppTooltip" Codebehind="AppTooltip.ascx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>






<style type="text/css">
	.style1
	{
		height: 19px;
		text-align: left;
	}
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
	.style3
	{
		height: 19px;
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
				cb.SetVisible(true);
				this.controls.lbltype.SetText(apt.cptype);
				this.controls.lblInterval.SetText(apt.cpinterval);
				
				cb.PerformCallback(apt.cpaptid);

			}
			else {

				//			this.controls.lblInterval.SetText("stuff");
			}





			//			this.controls.lblhoursexp.SetText(apt.cphoursexp);
			//			this.controls.lblhourslabel.SetText(apt.cplblhourslabel);



		},
		OnAppointmentRefresh: function (apt) {
			apt.updated = true;

		}
	});
	
	function show_panel()
	{
	    $("#div_loading").show();
	}
	function hide_panel() {
	    $("#div_loading").hide();
	}
    // ]]> 
</script>
<table style="width: 400px;background-color:antiquewhite">
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
					</table>
			<div id="div_loading" style="background-position: 50% 30%;  background-image: url('../../../images/loading_panel.gif'); background-repeat: no-repeat; background-color: transparent; height:100%;width:100%;position:absolute;"> </div>
				 <table style="width: 400px;background-color:antiquewhite;">
					 
					 <tr>
						 <td>
			                 
			 <dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
				 oncallback="cb_Callback" Width="100%" SettingsLoadingPanel-Enabled="true" 
	>
<SettingsLoadingPanel Text="wait.." Delay="0"></SettingsLoadingPanel>

			 	 <ClientSideEvents EndCallback="function(s, e) {
                      hide_panel();
if(s.cp_open=='1')
{
	s.SetVisible(true);
}
}" BeginCallback="function(s, e) {
	show_panel();
}" />

                 <Images>
                     <LoadingPanel Url="~/images/loading_panel.gif">
                     </LoadingPanel>
                 </Images>
                 <LoadingPanelImage Url="~/images/loading_panel.gif">
                 </LoadingPanelImage>

<Styles>
<LoadingPanel BackColor="Transparent">
<Border BorderStyle="None"></Border>
</LoadingPanel>

<LoadingDiv BackColor="Transparent">
<Border BorderStyle="None"></Border>
</LoadingDiv>
</Styles>

			 	 <LoadingPanelStyle BackColor="Transparent">
					 <Border BorderStyle="None" />
				 </LoadingPanelStyle>
				 <LoadingDivStyle BackColor="Transparent">
					 <Border BorderStyle="None" />
				 </LoadingDivStyle>
			 	<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table id="tbl_details" runat="server"  style="font-family: Arial; font-size: small;" width="100%">
		<tr>
			<td class="style1" width="60px">
				<dx:ASPxHyperLink ID="hl" runat="server"></dx:ASPxHyperLink>
			</td>
			<td width="100%" align="left">
				<dx:ASPxLabel ID="lblcustomer" runat="server" ClientInstanceName="lblcustomer" 
					style="font-family: Arial, Helvetica, sans-serif" Font-Bold="True" 
					Font-Names="Arial" Width="100%" Wrap="True">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td class="style1" width="60px">
				&nbsp;</td>
			<td align="left" width="100%">
				<dx:ASPxLabel ID="lbladdress" runat="server" ClientInstanceName="lbladdress" 
					Font-Bold="True" Font-Names="Arial" 
					style="font-family: Arial, Helvetica, sans-serif" Width="100%" Wrap="True">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td class="dxtcLeftAlignCell" width="60px">
			
			</td>
			<td width="100%" align="left" style="text-align: left">
				<dx:ASPxLabel ID="lblwodesc" runat="server" ClientInstanceName="lblwodesc" 
					style="font-family: Arial, Helvetica, sans-serif; font-size: x-small;" 
					Width="100%" Wrap="True" Font-Bold="False" Font-Names="Arial">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td style="font-size: 12px; font-family: Arial, Helvetica, sans-serif;" 
				class="style3" width="60px" nowrap="nowrap">
				Location:</td>
			<td width="100%" align="left" class="style3">
				<dx:ASPxLabel ID="lbllocation" runat="server" ClientInstanceName="lblresource" Font-Bold="True" Font-Names="Arial" style="font-family: Arial, Helvetica, sans-serif" Width="100%" Wrap="True">
                </dx:ASPxLabel>
			</td>
		</tr>
		<tr>
            <td class="style3" nowrap="nowrap" style="font-size: 12px; font-family: Arial, Helvetica, sans-serif;" width="60px">Resource:</td>
            <td align="left" class="style3" width="100%">
                <dx:ASPxLabel ID="lblresource" runat="server" ClientInstanceName="lblresource" Font-Bold="True" Font-Names="Arial" style="font-family: Arial, Helvetica, sans-serif" Width="150px" Wrap="True">
                </dx:ASPxLabel>
            </td>
        </tr>
		<tr>
			<td class="style3" nowrap="nowrap" 
				style="font-size: 12px; font-family: Arial, Helvetica, sans-serif;" 
				width="60px">
				Hours Spent:</td>
			<td align="left" class="style3" width="100%">
				<dx:ASPxLabel ID="lblhours" runat="server" ClientInstanceName="lblhours" 
					Font-Bold="True" Font-Names="Arial" 
					style="font-family: Arial, Helvetica, sans-serif" Width="150px" Wrap="True">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td style="font-size: 12px; font-family: Arial, Helvetica, sans-serif;" 
				width="60px" nowrap="nowrap">
				Original Hours Expected:</td>
			<td width="100%" align="left">
				<dx:ASPxLabel ID="lblhoursexp" runat="server" ClientInstanceName="lblhoursexp" 
					style="font-family: Arial, Helvetica, sans-serif" Width="150px" Wrap="True" 
					Font-Bold="True" Font-Names="Arial">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap" 
				style="font-size: 12px; font-family: Arial, Helvetica, sans-serif; white-space: nowrap;" 
				width="60px">
				Future Scheduled Hours:</td>
			<td align="left" width="100%">
				<dx:ASPxLabel ID="lblfuturehours" runat="server" 
					ClientInstanceName="lblfuturehours" Font-Bold="True" Font-Names="Arial" 
					style="font-family: Arial, Helvetica, sans-serif" Width="150px" Wrap="True">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td class="dxtcLeftAlignCell" 
				style="font-size: 12px; font-family: Arial, Helvetica, sans-serif;" 
				width="60px">
				Start Time:</td>
			<td align="left" width="100%">
				<dx:ASPxLabel ID="lblstart" runat="server" ClientInstanceName="lblstart" 
					Font-Bold="True" Font-Names="Arial" 
					style="font-family: Arial, Helvetica, sans-serif" Width="150px" Wrap="True">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td class="dxtcLeftAlignCell" 
				style="font-size: 12px; font-family: Arial, Helvetica, sans-serif;" 
				width="60px">
				End Time:</td>
			<td align="left" width="100%">
				<dx:ASPxLabel ID="lblend" runat="server" ClientInstanceName="lblend" 
					Font-Bold="True" Font-Names="Arial" 
					style="font-family: Arial, Helvetica, sans-serif" Width="150px" Wrap="True">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td 
				style="font-size: 12px; font-family: Arial, Helvetica, sans-serif;" 
				width="60px">
				Truck:</td>
			<td align="left" width="100%">
				<dx:ASPxLabel ID="lbltruck" runat="server" ClientInstanceName="lbltruck" 
					Font-Bold="True" Font-Names="Arial" 
					style="font-family: Arial, Helvetica, sans-serif" Width="150px" Wrap="True">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td style="font-size: 12px; font-family: Arial, Helvetica, sans-serif;" 
				width="60px">
				Set By:</td>
			<td align="left" width="100%">
				<dx:ASPxLabel ID="lblsetby" runat="server" ClientInstanceName="lblsetby" 
					Font-Bold="True" Font-Names="Arial" 
					style="font-family: Arial, Helvetica, sans-serif" Width="150px" Wrap="True">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap" 
				style="font-size: 12px; font-family: Arial, Helvetica, sans-serif;" 
				valign="top" width="160px">
				Who Else On This Job/Day?</td>
			<td align="left" valign="top" width="100%">
				<dx:ASPxLabel ID="lbl_whoelse" runat="server" Font-Names="Arial">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td class="dxtcLeftAlignCell" 
				style="font-size: 12px; font-family: Arial, Helvetica, sans-serif;" 
				width="60px">
				<asp:HiddenField ID="hdn_cid" runat="server" />
				<asp:HiddenField ID="hdn_aptid" runat="server" />
				</td>
			<td align="right" width="100%">
				
				&nbsp;</td>
		</tr>
	</table>
					</dx:PanelContent>
</PanelCollection>
			 </dx:ASPxCallbackPanel>
			
						 </td>
					 </tr>
				 </table>
				 


			 
