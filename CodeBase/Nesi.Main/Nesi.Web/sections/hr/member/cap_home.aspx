<%@ Page Title="CAP Home" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_hr_master_cap_home" Codebehind="cap_home.aspx.cs" %>	



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
//	bind_tooltips();

}

function closeit(stuff) {

	

}
	</script>

<table width = "100%">
<tr>
<td>

			<table style="width:100%;">
				<tr>
					<td nowrap="nowrap" style="font-family: 'Segoe UI'; font-size: 12px">
						&nbsp;</td>
					<td>
						&nbsp;</td>
					<td width="100%" colspan="2">
						&nbsp;</td>
				</tr>
				<tr>
					<td nowrap="nowrap" style="font-family: 'Segoe UI'; font-size: 12px">
						<dx:ASPxComboBox ID="ddl_member" runat="server" AutoPostBack="True" 
							DataSourceID="SqlDataSource2" EnableCallbackMode="True" 
							IncrementalFilteringMode="Contains" TextField="_name" Theme="NETheme01" 
							ValueField="id" ValueType="System.Int32" 
							onselectedindexchanged="ddl_member_SelectedIndexChanged">
						</dx:ASPxComboBox>
					</td>
					<td>
						&nbsp;</td>
					<td width="100%" style="width: 0%">
						<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							SelectCommand="Select a.member_id id, concat(b.name, ' - ' ,a.member_fullname) _name from member a inner join business_unit b on a.business_unit_id= b.id where a.member_status='Active' and find_in_set(business_unit_id, @visibleBU) order by b.name,a.member_fullname ">
						    <SelectParameters>
						        <asp:SessionParameter Name="@visibleBU" SessionField="visibleBU" Type="String" />
						    </SelectParameters>
                        </asp:SqlDataSource>
					</td>
					<td width="100%" align="right" style="width: 50%">
						&nbsp;</td>
				</tr>
				<tr>
					<td class="style6">
						</td>
					<td class="style6">
						</td>
					<td colspan="2" class="style6">
						</td>
				</tr>
			</table>
			<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
				Theme="NETheme01" Width="100%">
				<TabPages>
					<dx:TabPage Text="Certification Status">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								<table style="width:100%;">
									<tr>
										<td nowrap="nowrap">
											Comparing current certificates with those needed for</td>
										<td>
											<dx:ASPxComboBox ID="ddlmt" runat="server" AutoPostBack="True" 
												ClientInstanceName="ddlmt" OnSelectedIndexChanged="ddlmt_SelectedIndexChanged" 
												TextField="_name" Theme="NETheme01" ValueField="id" ValueType="System.Int32" 
												Width="250px">
											</dx:ASPxComboBox>
										</td>
										<td width="100%">
											&nbsp;</td>
									</tr>
								</table>
								<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
									ClientInstanceName="gv" KeyFieldName="x" OnCustomCallback="gv_CustomCallback" 
									OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared" 
									OnHtmlRowPrepared="gv_HtmlRowPrepared" Theme="NETheme01" Width="100%">
									<ClientSideEvents EndCallback="function(s, e) {

if ((s.cp_alert!=null)&amp;&amp;(s.cp_alert!=''))
{

alert(s.cp_alert);
s.cp_alert='';
}

	if (s.cp_refresh=='1')
{
s.cp_refresh ='0'
gv_training.Refresh();

}

}" />
									<GroupSummary>
										<dx:ASPxSummaryItem DisplayFormat="Missing: {0} Certificates  " FieldName="expired_count" 
											ShowInColumn="Core Responsibility" SummaryType="Sum" />
										<dx:ASPxSummaryItem DisplayFormat="Almost Expired: {0}  " FieldName="almost_expired_count" 
											 SummaryType="Sum" />
									</GroupSummary>
									<Columns>
										<dx:GridViewDataTextColumn Caption="Core Responsibility" FieldName="cr" 
											ShowInCustomizationForm="True" VisibleIndex="0" Width="300px">
											<CellStyle Wrap="True">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Certificate" FieldName="cert" 
											ShowInCustomizationForm="True" VisibleIndex="1" Width="300px">
											<CellStyle Wrap="True">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Expires In (days)" FieldName="expires" 
											ShowInCustomizationForm="True" VisibleIndex="2" Width="100px">
											<HeaderStyle HorizontalAlign="Center" />
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="x" FieldName="x" 
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="10">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="certid" ShowInCustomizationForm="True" 
											Visible="False" VisibleIndex="9">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="How to Acquire" FieldName="acquire" 
											ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
											<DataItemTemplate>
												<dx:ASPxPanel ID="div_acquire" runat="server" Theme="NETheme01" Width="100%">
												</dx:ASPxPanel>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="expired_count" FieldName="expired_count" 
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="8">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="memberid" FieldName="member_id" 
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="business_unit_id" FieldName="business_unit_id" 
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="7">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="membertype_id" FieldName="membertype_id" 
											ShowInCustomizationForm="True" VisibleIndex="4" Visible="False">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="almost_expired_count" FieldName="almost_expired_count" 
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="11">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Priority" FieldName="cr_priority" 
											ShowInCustomizationForm="True" SortIndex="0" SortOrder="Ascending" 
											Visible="False" VisibleIndex="5" Width="25px">
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsBehavior AllowDragDrop="False" ColumnResizeMode="Control" />
									<SettingsPager Mode="ShowAllRecords">
									</SettingsPager>
									<Settings ShowTitlePanel="True" />
									<SettingsText Title="Current Certificate Status" />
									<Styles>
										<GroupRow Font-Bold="True" Wrap="True">
										</GroupRow>
									</Styles>
									<Templates>
										<TitlePanel>
											<table cellpadding="5" style="width: 100%;">
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
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Certificate History">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								<dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
									DataSourceID="SqlDataSource3" KeyFieldName="id" Theme="NETheme01" Width="100%">
									<Columns>
										<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" ShowClearFilterButton="true"
											VisibleIndex="0">
											
										</dx:GridViewCommandColumn>
										<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" 
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
											<EditFormSettings Visible="False" />
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataDateColumn FieldName="Date" ShowInCustomizationForm="True" 
											SortIndex="0" SortOrder="Descending" VisibleIndex="2" Width="120px">
											<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
												EditFormatString="yyyy-MM-dd">
											</PropertiesDateEdit>
										</dx:GridViewDataDateColumn>
										<dx:GridViewDataTextColumn FieldName="Certificate" 
											ShowInCustomizationForm="True" VisibleIndex="3" Width="200px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="Notes" ShowInCustomizationForm="True" 
											VisibleIndex="4" Width="50%">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="Cert Notes" 
											ShowInCustomizationForm="True" VisibleIndex="5" Width="50%">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="How To Acquire" 
											ShowInCustomizationForm="True" VisibleIndex="6" Width="200px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Expires In (days)" FieldName="Expires In" 
											ShowInCustomizationForm="True" VisibleIndex="7" Width="60px">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsPager PageSize="25">
									</SettingsPager>
									<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
										ShowHeaderFilterButton="True" />
								</dx:ASPxGridView>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Training Log">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								<lc:LayoutControl ID="LayoutControl" runat="server" />
								<dx:ASPxGridView ID="gv_training" runat="server" AutoGenerateColumns="False" 
									ClientInstanceName="gv_training" DataSourceID="SqlDataSource1" 
									KeyFieldName="id" 
									OnCommandButtonInitialize="gv_training_CommandButtonInitialize" 
									OnRowDeleting="gv_training_RowDeleting" Theme="NETheme01" 
									OnCustomCallback="gv_training_CustomCallback" 
									OnCustomJSProperties="gv_training_CustomJSProperties" Width="100%">
									<ClientSideEvents EndCallback="function(s, e) {
	if((s.cp_gv_refresh!=null) &amp;&amp; (s.cp_gv_refresh=='1'))
{
gv.PerformCallback('refresh');
s.cp_gv_refresh='0';
please_wait('stop');
}


}" />
									<Columns>
										<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
											VisibleIndex="0" Width="60px" ShowDeleteButton="True" ShowClearFilterButton="true">
											
											
										</dx:GridViewCommandColumn>
										<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataDateColumn Caption="Date" FieldName="date" 
											ShowInCustomizationForm="True" SortIndex="0" SortOrder="Descending" 
											VisibleIndex="3" Width="110px">
											<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
												EditFormatString="yyyy-MM-dd">
											</PropertiesDateEdit>
										</dx:GridViewDataDateColumn>
										<dx:GridViewDataTimeEditColumn Caption="Start Time" FieldName="start_time" 
											ShowInCustomizationForm="True" VisibleIndex="4" Width="90px">
											<PropertiesTimeEdit DisplayFormatString="">
											</PropertiesTimeEdit>
										</dx:GridViewDataTimeEditColumn>
										<dx:GridViewDataTimeEditColumn Caption="End Time" FieldName="end_time" 
											ShowInCustomizationForm="True" VisibleIndex="5" Width="90px">
											<PropertiesTimeEdit DisplayFormatString="">
											</PropertiesTimeEdit>
										</dx:GridViewDataTimeEditColumn>
										<dx:GridViewDataTextColumn Caption="Location" FieldName="location" 
											ShowInCustomizationForm="True" VisibleIndex="6" Width="200px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Training Name" FieldName="name" 
											ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
											ShowInCustomizationForm="True" VisibleIndex="7" Width="200px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Days Until" FieldName="days_until" 
											ShowInCustomizationForm="True" VisibleIndex="8" Width="50px">
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
									<SettingsPager PageSize="25">
									</SettingsPager>
									<Settings ColumnMinWidth="10" ShowFilterBar="Visible" ShowFilterRow="True" 
										ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
										ShowHeaderFilterButton="True" />
									<SettingsText Title="My Training History" />
								</dx:ASPxGridView>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
				</TabPages>
			</dx:ASPxPageControl>
			<br />
			<asp:SqlDataSource runat="server" 
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
training_header_history.id,
training_header.`name`,
training_header_history.member_id,
training_header_history.business_unit_id,
training_header_history.membertype_id,
training_header_history.date,
training_header_history.notes,
training_header_history.start_time,
training_header_history.end_time,
training_header_history.cap_training_schedule_id,
cap_training_schedule.location,
DateDiff(date(training_header_history.date),curdate()) days_until
FROM
          training_header_history
INNER JOIN training_header ON training_header_history.training_header_id = training_header.id
LEFT JOIN cap_training_schedule ON cap_training_schedule.training_header_id = training_header.id
WHERE
          training_header_history.member_id = ?mid" ID="SqlDataSource1">
				<SelectParameters>
					<asp:ControlParameter ControlID="ddl_member" PropertyName="Value" Name="mid">
					</asp:ControlParameter>
				</SelectParameters>
			</asp:SqlDataSource>
			<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
certificate_history.id,
certificate_history.date `Date`,
certificates.certificate_name Certificate,
certificate_history.notes Notes,
certificates.notes `Cert Notes`,
certificates.how_to_acquire `How To Acquire`,
ifnull(datediff((certificate_history.date + interval (certificates.expires) day ), curdate()),0) `Expires In`
FROM
certificate_history
INNER JOIN certificates ON certificate_history.certificate_id = certificates.id
WHERE
certificates.`status` = 'Active' AND
certificate_history.member_id = ?mid
">
				<SelectParameters>
					<asp:ControlParameter ControlID="ddl_member" Name="mid" PropertyName="Value" />
				</SelectParameters>
			</asp:SqlDataSource>

			<br />

			<br />
			<br />
			<asp:HiddenField ID="hdnmid" runat="server" />
			<br />
			<dx:ASPxPopupControl ID="pop_t" runat="server" AppearAfter="0" 
				ClientInstanceName="pop_t" HeaderText="Training Details" Height="600px" 
				Modal="True" onwindowcallback="pop_t_WindowCallback" PopupAnimationType="None" 
				PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
				Theme="NETheme01" Width="500px">
				
				<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_alert!=null &amp;&amp; s.cp_alert!='')
{
alert(s.cp_alert);
s.cp_alert='';
pop_t.Hide();
gv.PerformCallback('refresh');
please_wait('stop');
}
}" />
				
				<ContentCollection>
<dx:PopupControlContentControl runat="server">
	<table cellpadding="3px" style="width:100%;">
		<tr>
			<td>
				<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ID:" Theme="NETheme01">
				</dx:ASPxLabel>
			</td>
			<td colspan="2" width="100%">
				<dx:ASPxLabel ID="lbl_pop_t_id" runat="server" 
					ClientInstanceName="lbl_pop_t_id" Text="0">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Training Name:" 
					Theme="NETheme01">
				</dx:ASPxLabel>
			</td>
			<td colspan="2">
				<dx:ASPxTextBox ID="txt_training_name" runat="server" ClientEnabled="False" 
					Width="100%">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxLabel ID="ASPxLabel3" runat="server" 
					Text="Date (or Due Date for Online courses):" Theme="NETheme01" Width="150px">
				</dx:ASPxLabel>
			</td>
			<td colspan="2">
				<dx:ASPxTextBox ID="txt_training_date" runat="server" ClientEnabled="False" 
					Width="100%">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Start Time:" 
					Theme="NETheme01">
				</dx:ASPxLabel>
			</td>
			<td colspan="2">
				<dx:ASPxTextBox ID="txt_training_start" runat="server" ClientEnabled="False" 
					Width="100%">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="End Time:" Theme="NETheme01">
				</dx:ASPxLabel>
			</td>
			<td colspan="2">
				<dx:ASPxTextBox ID="txt_training_end" runat="server" ClientEnabled="False" 
					Width="100%">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td valign="top">
				<dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Location:" Theme="NETheme01">
				</dx:ASPxLabel>
			</td>
			<td colspan="2">
				<dx:ASPxMemo ID="mem_location" runat="server" ClientEnabled="False" 
					Height="100px" Width="100%">
				</dx:ASPxMemo>
			</td>
		</tr>
		<tr>
			<td valign="top">
				<dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Training Notes:" 
					Theme="NETheme01">
				</dx:ASPxLabel>
			</td>
			<td colspan="2">
				<dx:ASPxMemo ID="mem_training_notes" runat="server" ClientEnabled="False" 
					Height="200px" Width="100%">
				</dx:ASPxMemo>
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap" valign="top">
				&nbsp;</td>
			<td colspan="2">
				&nbsp;</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td>
				&nbsp;</td>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td>
				&nbsp;</td>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td align="left" valign="middle">
				<dx:ASPxButton ID="btn_pop_cancel" runat="server" 
					ClientInstanceName="Cancel Training" ClientVisible="False" 
					Text="Cancel Training" Theme="NETheme01" AutoPostBack="False">
					<ClientSideEvents Click="function(s, e) {
	pop_t.PerformCallback('kill|' + lbl_pop_t_id.GetText());
	

}" />
				</dx:ASPxButton>
			</td>
			<td align="center" valign="middle">
				&nbsp;</td>
			<td align="right" valign="middle">
				<dx:ASPxButton ID="btn_pop_action" runat="server" 
					ClientInstanceName="btn_pop_action" Text="Email Myself" Theme="NETheme01" 
					AutoPostBack="False">
					<ClientSideEvents Click="function(s, e) {
	pop_t.PerformCallback('go|' + lbl_pop_t_id.GetText() + '|' + s.GetText());
	
}" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
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
		.style6
		{
			height: 18px;
		}
	</style>
	</asp:Content>

