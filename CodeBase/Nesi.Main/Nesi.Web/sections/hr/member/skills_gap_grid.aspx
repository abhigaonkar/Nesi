<%@ Page Title="Skills Gap Grid" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_hr_master_skills_gap_grid" Codebehind="skills_gap_grid.aspx.cs" %>	

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register src="../../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>

	
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	
	<p>
		<br />
	</p>
	<p>
	</p>
	
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

	<script type="text/javascript">
 function bind_tooltips()
			{
			$(".opt1").each(function()
				{
				$(this).tip();
				});
			}
		$(document).ready(function()
			{
	//		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
			});

function EndReqHandler()
	{
	bind_tooltips();
	
	}
	</script>

<table width = "100%">
<tr>
<td>
			<lc:LayoutControl runat="server" id="layout" __is_private="True" 
		GridviewID="cb" />
			<br />
			<dx:ASPxCheckBox ID="chk_mt" runat="server" AutoPostBack="True" 
				ClientInstanceName="chk_mt" oncheckedchanged="chk_mt_CheckedChanged" 
				Text="Compare to Existing Agreements" Theme="NETheme01">
			</dx:ASPxCheckBox>
			<br />
	<dx:ASPxGridView ID="cb" runat="server" AutoGenerateColumns="False" 
		Width="100%" 
		onhtmlrowprepared="ASPxGridView1_HtmlRowPrepared" 
		ClientInstanceName="cb" 
		oncustomcallback="ASPxGridView1_CustomCallback" 
		oncustomjsproperties="ASPxGridView1_CustomJSProperties" Theme="NETheme01" 
				KeyFieldName="x" oncustomgroupdisplaytext="cb_CustomGroupDisplayText">
		<GroupSummary>
				<dx:ASPxSummaryItem DisplayFormat="Missing: {0} Certificates  " FieldName="expired_count" 
											 SummaryType="Sum" />
				<dx:ASPxSummaryItem DisplayFormat="Almost Expired: {0}  " FieldName="almost_expired_count" 
											 SummaryType="Sum" />
		</GroupSummary>
		<Columns>
			<dx:GridViewCommandColumn Caption=" Issue" ShowSelectCheckbox="True" 
				VisibleIndex="0" Width="140px">
				<FooterTemplate>
					<table style="width:100%;">
						<tr>
							<td>
								<dx:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False" 
									oninit="ASPxButton3_Init" Text="Issue Cert" Theme="NETheme01" Wrap="False">
									<ClientSideEvents Click="function(s, e) {
	pop_issue.Show();
}" />
								</dx:ASPxButton>
							</td>
						</tr>
					</table>
				</FooterTemplate>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="Branch" FieldName="branch" VisibleIndex="1" 
				Width="50px" MinWidth="10">
				<Settings HeaderFilterMode="CheckedList" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Membertype" FieldName="mt" VisibleIndex="2" 
				Width="75px" MinWidth="10">
				<Settings HeaderFilterMode="CheckedList" />
				<FooterCellStyle BackColor="#FF99CC">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Member" FieldName="name" VisibleIndex="3" 
				Width="75px" MinWidth="10">
				<Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Core Responsibility" FieldName="cr" 
				VisibleIndex="4" MinWidth="20" Width="100%">
				<Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
			</dx:GridViewDataTextColumn>

			<dx:GridViewDataTextColumn Caption="Expires (d)" FieldName="cert_exp" 
				VisibleIndex="7" Width="75px" MinWidth="10" Visible="False">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Certificate" FieldName="cert_req" 
				VisibleIndex="5" Width="100px" MinWidth="10" Visible="False">
				
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="crid" FieldName="crid" Visible="False" 
				VisibleIndex="20">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataCheckColumn Caption="Cert Is Internal" 
				FieldName="cert_is_internal" VisibleIndex="18" MinWidth="10" Width="50px" 
				Visible="False">
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataDateColumn Caption="Cert Date" FieldName="cert_date" 
				VisibleIndex="6" Width="80px" MinWidth="10">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="Cert Score" FieldName="cert_score" 
				VisibleIndex="8" Width="60px" MinWidth="10" Visible="False">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Cert Exp Date" FieldName="cert_exp_date" 
				VisibleIndex="9" Width="75px" MinWidth="10" Visible="False">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="training_id" FieldName="training_id" 
				Visible="False" VisibleIndex="17">
			</dx:GridViewDataTextColumn>
			
			<dx:GridViewDataDateColumn Caption="Enrolled Training Date" FieldName="training_date" 
				VisibleIndex="16" Width="120px" MinWidth="10">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="How to Acquire" FieldName="acquire" 
				MinWidth="10" VisibleIndex="10" Width="50%" Visible="False">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Possible Training Date" 
				FieldName="pos_training_date" MinWidth="10" VisibleIndex="12" Width="150px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<DataItemTemplate>
				<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" oninit="banc_Init" 
				Text='<%# Eval("pos_training_date") %>' Visible='<%# Eval("pos_training_date") != System.DBNull.Value %>' 
				Cursor="pointer"  Theme="NETheme01" /></dx:ASPxHyperLink>
				</DataItemTemplate>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="Training Name" 
				FieldName="training_name" VisibleIndex="11" Width="200px" MinWidth="10">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Training Location" 
				FieldName="cert_location" MinWidth="10" VisibleIndex="13" Width="150px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Max Enroll" FieldName="max_fill" 
				MinWidth="10" VisibleIndex="14" Width="60px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Enrolled" FieldName="enrolled" 
				MinWidth="10" VisibleIndex="15" Width="50px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Priority" FieldName="_mt_cr_priority" 
				VisibleIndex="19" Width="20px">
				<Settings HeaderFilterMode="CheckedList" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Is Expired" FieldName="is_expired" 
				Visible="False" VisibleIndex="21" Width="10px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="almost_expired_count" FieldName="almost_expired_count" 
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="22">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="expired_count" FieldName="expired_count" 
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="23">
										</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="certificate_id" FieldName="cert_id" 
				Visible="False" VisibleIndex="24">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="member_id" FieldName="member_id" 
				Visible="False" VisibleIndex="25">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="business_unit_id" FieldName="business_unit_id" 
				Visible="false" VisibleIndex="26">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ShowFilterBar="Auto" ShowFilterRow="True" ShowFilterRowMenu="True" 
			ShowHeaderFilterButton="True" ShowGroupPanel="True" ShowFooter="True" 
			ColumnMinWidth="10" ShowTitlePanel="True" />
		<Styles>
			<TitlePanel BackColor="White">
			</TitlePanel>
		</Styles>
		<Templates>
		
			<TitlePanel>
				<table style="width:100%; color: #000000; font-family: Arial, Helvetica, sans-serif; font-size: 10pt;" cellpadding="5">
					<tr>
						<td>
							Certificate Status</td>
						<td>
							&nbsp;</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td bgcolor="#FFFF99" nowrap="nowrap" 
							style="font-family: arial, Helvetica, sans-serif; font-size: 12px; color: #000000; font-weight: bold;">
							Yellow row indicates a certificate is within 90 days of expiring</td>
						<td width="100%">
							&nbsp;</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td bgcolor="#FFCCCC" 
							style="font-family: arial, Helvetica, sans-serif; font-size: 12px; color: #000000; font-weight: bold;">
							Red row indicates you need the certificate</td>
						<td>
							&nbsp;</td>
						<td>
							&nbsp;</td>
					</tr>
				</table>
			
			</TitlePanel>
		</Templates>
	</dx:ASPxGridView>

			<br />
			<dx:ASPxPopupControl ID="pop_schedule" runat="server" ClientInstanceName="pop_schedule" 
				HeaderText="Schedule Training" Modal="True" PopupAnimationType="None" 
				PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
				Width="350px" Theme="NETheme01" onwindowcallback="pop_schedule_WindowCallback">
				<HeaderStyle Font-Names="Arial" />
				<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxCallbackPanel ID="cbb0" runat="server" ClientInstanceName="cbb0" 
		OnCallback="cbb0_Callback" Width="100%">
		<ClientSideEvents EndCallback="function(s, e) {
	if(txtcrid.GetText()=='0')
{
pop_schedule.Hide();
location.href = location.href;
}
}" />
<ClientSideEvents EndCallback="function(s, e) {
	if(txtcrid.GetText()==&#39;0&#39;)
{
pop_schedule.Hide();
location.href = location.href;
}
}"></ClientSideEvents>
		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<table style="width:100%;">
					<tr>
						<td class="style5">
							Certification:</td>
						<td width="100%">
							<dx:ASPxLabel ID="lblcert0" runat="server" ClientInstanceName="lblcert0" 
								Text="Certification" Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style5">
							Employee:</td>
						<td>
							<dx:ASPxLabel ID="lblmember0" runat="server" ClientInstanceName="lblmember0" 
								Text="Employee" Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style5">
							Training Date:</td>
						<td>
							<dx:ASPxComboBox ID="ddl_training" runat="server" 
								ClientInstanceName="ddl_training" DataSourceID="SqlDataSource1" 
								EnableTheming="True" TextField="_name" Theme="NETheme01" ValueField="_id" 
								ValueType="System.Int32" OnCallback="ddl_training_Callback1">
							</dx:ASPxComboBox>
						</td>
					</tr>
					<tr>
						<td class="style5">
							Location:</td>
						<td>
							<dx:ASPxLabel ID="lbllocation" runat="server" ClientInstanceName="lbllocation" 
								Text="Location" Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style5">
							Already Enrolled:</td>
						<td>
							<dx:ASPxLabel ID="lbl_enrolled" runat="server" 
								ClientInstanceName="lbl_enrolled" Text="0/0" Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style5">
							&nbsp;</td>
						<td>
							<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
cap_training_schedule.id _id,
Concat(date(cap_training_schedule.date), '-', training_header.`name`) _name,
cap_training_schedule.date
FROM
training_header
INNER JOIN cap_training_schedule ON training_header.id = cap_training_schedule.training_header_id
INNER JOIN certificate_training_link ON certificate_training_link.training_header_id = training_header.id
WHERE
certificate_training_link.certificate_id = ?certid and cap_training_schedule.date &gt;curdate()

">
								<SelectParameters>
									<asp:ControlParameter ControlID="txtcrid0" Name="?certid" PropertyName="Text" />
								</SelectParameters>
							</asp:SqlDataSource>
						</td>
					</tr>
					<tr>
						<td class="style5">
							&nbsp;</td>
						<td>
							<dx:ASPxTextBox ID="txtcrid0" runat="server" ClientInstanceName="txtcrid0" 
								ClientVisible="False" Width="170px" Theme="NETheme01">
							</dx:ASPxTextBox>
							<dx:ASPxTextBox ID="txtmemid0" runat="server" ClientInstanceName="txtmemid0" 
								ClientVisible="False" Width="170px" Theme="NETheme01">
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td>
							&nbsp;</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxButton ID="btnadd0" runat="server" AutoPostBack="False" 
								ClientInstanceName="btnadd0" Text="I'd Like To Attend" Theme="NETheme01" Wrap="False">
								<ClientSideEvents Click="function(s, e) {
	cbb0.PerformCallback();
}" />
<ClientSideEvents Click="function(s, e) {
	pop_schedule.PerformCallback(ddl_training.GetValue()+'|'+txtmemid0.GetText());
}"></ClientSideEvents>
							</dx:ASPxButton>
						</td>
						<td align="right">
							<dx:ASPxButton ID="C0" runat="server" AutoPostBack="False" 
								ClientInstanceName="btncancel0" Text="Cancel" Theme="NETheme01">
								<ClientSideEvents Click="function(s, e) {
	pop_schedule.Hide();
}" />
<ClientSideEvents Click="function(s, e) {
	pop_schedule.Hide();
}"></ClientSideEvents>
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
	<br />
					</dx:PopupControlContentControl>
</ContentCollection>
			</dx:ASPxPopupControl>
			<br />
			<dx:ASPxPopupControl ID="pop_issue" runat="server" ClientInstanceName="pop_issue" 
				HeaderText="Issue Certification" Modal="True" PopupAnimationType="None" 
				PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
				Width="350px" Theme="NETheme01" onpopupwindowcommand="pop_PopupWindowCommand">
				<HeaderStyle Font-Names="Arial" />
				<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxCallbackPanel ID="cbb" runat="server" ClientInstanceName="cbb" 
		OnCallback="cbb_Callback" Width="100%">
		<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_close=='close')
{
pop_issue.Hide();
location.href = location.href;

}
}" />

		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<table style="width:100%;">
					<tr>
						<td class="style5">
							Date:</td>
						<td>
							<dx:ASPxDateEdit ID="dtecert" runat="server" ClientInstanceName="dtecert" 
								CssClass="current_payroll" Theme="NETheme01">
							</dx:ASPxDateEdit>
						</td>
					</tr>
					<tr>
						<td class="style5">
							External ID:</td>
						<td>
							<dx:ASPxTextBox ID="extid" runat="server" ClientInstanceName="extid" 
								CssClass="current_payroll" Width="170px" Theme="NETheme01">
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td class="style5">
							&nbsp;</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style5">
							Score:</td>
						<td>
							<dx:ASPxTextBox ID="txtscore" runat="server" ClientInstanceName="txtscore" 
								CssClass="current_payroll" Theme="NETheme01" Width="170px">
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td class="style5">
							Notes:</td>
						<td>
							<dx:ASPxMemo ID="memnotes" runat="server" ClientInstanceName="memnotes" 
								CssClass="current_payroll" Height="71px" Width="100%" Theme="NETheme01">
							</dx:ASPxMemo>
						</td>
					</tr>
					<tr>
						<td>
							&nbsp;</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxButton ID="btnadd" runat="server" AutoPostBack="False" 
								ClientInstanceName="btnadd" Text="Add" Theme="NETheme01">
								<ClientSideEvents Click="function(s, e) {
	cbb.PerformCallback();
}" />
<ClientSideEvents Click="function(s, e) {
	cbb.PerformCallback();
}"></ClientSideEvents>
							</dx:ASPxButton>
						</td>
						<td align="right">
							<dx:ASPxButton ID="C" runat="server" AutoPostBack="False" 
								ClientInstanceName="btncancel" Text="Cancel" Theme="NETheme01">
								<ClientSideEvents Click="function(s, e) {
	pop_issue.Hide();
}" />
<ClientSideEvents Click="function(s, e) {
	pop_issue.Hide();
}"></ClientSideEvents>
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
	<br />
					</dx:PopupControlContentControl>
</ContentCollection>
			</dx:ASPxPopupControl>
			<br />

</td>
</tr>
</table>



</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style5
		{
			font-family: Arial;
			
		}
	</style>
	</asp:Content>

