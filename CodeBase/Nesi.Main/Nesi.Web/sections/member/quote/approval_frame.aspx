<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="quote_approval_frame" Title="Approve Quote" Codebehind="approval_frame.aspx.cs" %>


<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>











<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<asp:ScriptManager ID="sm" runat="server">
	</asp:ScriptManager>
<asp:UpdatePanel ID="up" runat="server">
<ContentTemplate>
	<script type="text/javascript" language="javascript">
function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 100) + 'px';

 }
	
 </script>
	
				<div id="lblerror" Runat="Server" style="font-family: Arial, Helvetica, sans-serif; font-size: 10pt; font-weight: bold; color: #FF0000"></div>
				<table style="width:100%; font-family: Arial;">
					<tr>
						<td class="style7" colspan="3">
							Because this is a large quote, we will be implementing the process requiring 
							your approval at this stage to reduce shop time spent on quoting jobs that we 
							will never get, but also to improve our chances on quotes that we should win.&nbsp; 
							So please fill out the information the information here and then review the 
							schedule that is produced.<br />
							<br />
							<strong>Initial Filter Questions:</strong></td>
					</tr>
					<tr>
						<td class="style7" colspan="3">
							<div ID="filter_results" runat="server">
							</div>
						</td>
					</tr>
					<tr>
						<td>
							<b>Started By:</b></td>
						<td>
							<dx:ASPxLabel ID="lbl_pop1_started_by" runat="server" Font-Names="Arial" 
								Text="Unknown">
							</dx:ASPxLabel>
						</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td>
							<b>Expected Quote Value:</b></td>
						<td>
							<dx:ASPxLabel ID="lbl_pop1_value" runat="server" Font-Names="Arial" 
								Text="Unknown">
							</dx:ASPxLabel>
						</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td nowrap="nowrap" style="font-weight: 700">
							Quote Due Date:</td>
						<td>
							<dx:ASPxLabel ID="lbl_pop1_due" runat="server" Font-Names="Arial" 
								Text="Unknown">
							</dx:ASPxLabel>
						</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td nowrap="nowrap">
							<b>Expected Work Completion Date:</b></td>
						<td>
							<dx:ASPxLabel ID="lbl_pop1_completion" runat="server" Font-Names="Arial" 
								Text="Unknown">
							</dx:ASPxLabel>
						</td>
						<td>
							<dx:ASPxLabel ID="lbl_how_many_bodies_will" runat="server" Font-Names="Arial">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td nowrap="nowrap">
							<b>Allowable Hours to Quote:</b></td>
						<td>
							<dx:ASPxTextBox ID="txt_hours" runat="server" ClientInstanceName="txt_hours" 
								Font-Names="Arial" Width="50px">
							</dx:ASPxTextBox>
						</td>
						<td width="100%">
							<dx:ASPxLabel ID="lbl_typical_quote_time" runat="server" Font-Names="Arial">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td>
							<strong>Point Person:</strong></td>
						<td>
							<dx:ASPxComboBox ID="ddl_rt5" runat="server" DataSourceID="SqlDataSource5" 
								EnableCallbackMode="True" Font-Names="Arial" TextField="member_name" 
								ValueField="member_id" ValueType="System.Int32" Width="150px" AnimationType="None" 
								IncrementalFilteringMode="Contains">
							</dx:ASPxComboBox>
						</td>
						<td>
							<dx:ASPxLabel ID="lbl_point_person_busy" runat="server" Font-Names="Arial">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td>
							<strong>Estimator:</strong></td>
						<td>
							<dx:ASPxComboBox ID="ddl_rt6" runat="server" DataSourceID="SqlDataSource6" 
								EnableCallbackMode="True" Font-Names="Arial" TextField="member_name" 
								ValueField="member_id" ValueType="System.Int32" Width="150px" AnimationType="None" 
								IncrementalFilteringMode="Contains">
							</dx:ASPxComboBox>
						</td>
						<td>
							<dx:ASPxLabel ID="lbl_hit_rate_of_estimator" runat="server" Font-Names="Arial">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr id="tr_rt1" runat="server">
						<td>
							<b>Review Team Member 1:</b></td>
						<td>
							<dx:ASPxComboBox ID="ddl_rt1" runat="server" DataSourceID="SqlDataSource5" 
								EnableCallbackMode="True" Font-Names="Arial" TextField="member_name" 
								ValueField="member_id" ValueType="System.Int32" Width="150px" AnimationType="None" 
								IncrementalFilteringMode="Contains">
							</dx:ASPxComboBox>
						</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr id="tr_rt2" runat="server">
						<td>
							<b>Review Team Member 2:</b></td>
						<td>
							<dx:ASPxComboBox ID="ddl_rt2" runat="server" DataSourceID="SqlDataSource5" 
								EnableCallbackMode="True" Font-Names="Arial" Width="150px" TextField="member_name" 
								ValueField="member_id" ValueType="System.Int32" AnimationType="None" 
								IncrementalFilteringMode="Contains">
							</dx:ASPxComboBox>
						</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr id="tr_rt3" runat="server">
						<td>
							<b>Review Team Member 3:</b></td>
						<td>
							<dx:ASPxComboBox ID="ddl_rt3" runat="server" DataSourceID="SqlDataSource5" 
								EnableCallbackMode="True" Font-Names="Arial" Width="150px" TextField="member_name" 
								ValueField="member_id" ValueType="System.Int32" AnimationType="None" 
								IncrementalFilteringMode="Contains">
							</dx:ASPxComboBox>
						</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr  id="tr_rt4" runat="server">
						<td>
							<b>Review Team Member 4:</b></td>
						<td>
							<dx:ASPxComboBox ID="ddl_rt4" runat="server" DataSourceID="SqlDataSource5" 
								EnableCallbackMode="True" Font-Names="Arial" Width="150px" TextField="member_name" 
								ValueField="member_id" ValueType="System.Int32" AnimationType="None" 
								IncrementalFilteringMode="Contains">
							</dx:ASPxComboBox>
						</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td>
							<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" >
							</asp:SqlDataSource>
						</td>
						<td>
							<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" >
								<SelectParameters>
									<asp:ControlParameter ControlID="hdn_cid" Name="cid" PropertyName="Value" />
								</SelectParameters>
							</asp:SqlDataSource>
						</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxButton ID="btnapprovestage1_pop" runat="server" 
								OnClick="btnapprovestage1_Click" Text="Allow Quote to Proceed" Width="200px" Font-Names="Arial">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="btncancelstage1_pop" runat="server" Font-Names="Arial" 
								OnClick="btncancelstage1_pop_Click" Text="STOP THIS QUOTE!" Width="200px" AutoPostBack="False">
								<ClientSideEvents Click="function(s, e) {
	pop_close.Show();
}" />
							</dx:ASPxButton>
						</td>
						<td>
							&nbsp;</td>
					</tr>
	</table>
	<dx:ASPxPopupControl ID="pop_close" runat="server" 
		HeaderText="Kill this quote!" Height="300px" Width="400px" 
		ClientInstanceName="pop_close" CloseAction="CloseButton" Font-Names="Arial" 
		Modal="True" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" 
		PopupVerticalAlign="WindowCenter" onwindowcallback="pop_close_WindowCallback">
		<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_close=='close')
{

pop_close.Hide();
window.parent.popstage1.Hide();
window.parent.window.location.href = window.parent.window.location.href;
}
}" />
		<HeaderStyle BackColor="#99FF66" Font-Size="14pt" />
		<ModalBackgroundStyle Opacity="0">
		</ModalBackgroundStyle>
		<ContentCollection>
			<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
				<table style="width:100%;">
					<tr>
						<td colspan="3">
							You are about to close a quote and the following people will get a notification 
							...</td>
					</tr>
					<tr>
						<td colspan="3">
							<div id="pop_close_list" runat="server"></div></td>
					</tr>
					<tr>
						<td colspan="3" style="font-weight: 700">
							Why are you closing this quote?</td>
					</tr>
					<tr>
						<td colspan="3">
							<dx:ASPxMemo ID="mem_close_notes" runat="server" BackColor="#FFFFCC" 
								ClientInstanceName="mem_close_notes" Height="71px" Width="100%">
							</dx:ASPxMemo>
						</td>
					</tr>
					<tr>
						<td>
							&nbsp;</td>
						<td>
							&nbsp;</td>
						<td align="right" style="text-align: right">
							&nbsp;</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Close" 
								AutoPostBack="False" Font-Names="Arial">
								<ClientSideEvents Click="function(s, e) {
	pop_close.PerformCallback();
}" />
							</dx:ASPxButton>
						</td>
						<td>
							&nbsp;</td>
						<td align="right">
							<dx:ASPxButton ID="ASPxButton3" runat="server" Text="Cancel" 
								AutoPostBack="False" Font-Names="Arial">
								<ClientSideEvents Click="function(s, e) {
	pop_close.Hide();
}" />
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
	<br />
	<br />
			</ContentTemplate>
	</asp:UpdatePanel>
<asp:HiddenField ID="hdn_cid" runat="server" />


<asp:HiddenField ID="hdn_quote_id" runat="server" />
	<br />
</asp:Content>

<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">

	.style7
	{
		height: 18px;
	}
	.dxbButton
{
	color: #000000;
	font: normal 12px Tahoma, Geneva, sans-serif;
	vertical-align: middle;
	border: 1px solid #7F7F7F;
	
	padding: 1px;
	cursor: pointer;
}
	</style>
	</asp:Content>


