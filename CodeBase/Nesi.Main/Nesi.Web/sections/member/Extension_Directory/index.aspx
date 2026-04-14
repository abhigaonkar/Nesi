<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="Extension_Directory" Title="Employee Extensions" EnableTheming="true" Theme="NETheme01" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
	<table width="100%">
		<tr>
			<td align="left">
				<table>
					<tr>
						<td>
						</td>
						<td>
							<dx:ASPxButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel">
								<ClientSideEvents Click="function(s, e) {
								window.opener.location.href = window.opener.location.href;
	window.close();
}" />
							</dx:ASPxButton>
						</td>
						<td>
							<asp:Label runat="server" ID="error_msg" ForeColor="Red"></asp:Label>
						</td>
					</tr>
				</table>
			</td>
			<td>
			</td>
			<td align="left">
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<dx:ASPxGridView ID="gv_extensions" runat="server" AutoGenerateColumns="False" KeyFieldName="member_id" Width="100%" Font-Names="Arial" OnDataBound="ASPxGridView1_DataBound" Theme="Default" DataSourceID="ds_extensions" onhtmlcommandcellprepared="ASPxGridView1_HtmlCommandCellPrepared" onhtmlrowprepared="ASPxGridView1_HtmlRowPrepared">
					<Styles>
						<Header HorizontalAlign="Center" ImageSpacing="5px" SortingImageSpacing="5px">
						</Header>
						<DetailCell Wrap="False">
						</DetailCell>
						<Cell Wrap="False">
						</Cell>
					</Styles>
					<SettingsPager Mode="ShowAllRecords">
					</SettingsPager>
					<Columns>
						<dx:GridViewDataTextColumn Caption="Member" FieldName="Member" VisibleIndex="2">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="Company" VisibleIndex="3">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Title" FieldName="Title" VisibleIndex="4">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Ext" FieldName="Ext" VisibleIndex="5">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Memberid" FieldName="member_id" Visible="False" VisibleIndex="6">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Select for Quick View" FieldName="_selected" VisibleIndex="1" Width="50px">
							<DataItemTemplate>
								<dx:ASPxCheckBox ID="cb" runat="server" ValueType="System.String" Value='<%# Bind("_selected") %>'>
								</dx:ASPxCheckBox>
							</DataItemTemplate>
							<CellStyle HorizontalAlign="Center"></CellStyle>
						</dx:GridViewDataTextColumn>
					</Columns>
					<Settings ShowFilterRow="True" />
					<StylesEditors>
						<CalendarHeader Spacing="1px">
						</CalendarHeader>
						<ProgressBar Height="25px">
						</ProgressBar>
					</StylesEditors>
				</dx:ASPxGridView>
				<br />
				<asp:SqlDataSource ID="ds_extensions" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT
	a.member_id,
	b.membertype_name AS Title,
	c.name AS Company,
	a.member_fullname AS Member,
	a.member_phoneextension AS Ext,
	if(ifnull(d.quick_extensions_id,0)=0,'False','True') AS _selected,
	d.Quick_Extensions_Mymember_ID
FROM
	member a
LEFT JOIN 
	membertype b ON a.Member_MemberType_ID = b.MemberType_ID
LEFT join 
	business_unit c ON a.business_unit_id = c.id
LEFT JOIN 
	quick_extensions d ON a.Member_ID = d.Quick_Extensions_Member_ID and d.Quick_Extensions_Mymember_ID = @member_id
WHERE
	a.member_status = 'Active' and 
	a.member_phoneextension != '000' and
	a.member_phoneextension != '' 
   and 
(find_in_set (c.id,get_visible_business_units_group_concat(@member_id))
    or
c.is_backoffice = 1                
                    )
ORDER BY
                    c.name, 
	a.Member_FirstName
">
					<SelectParameters>
						<asp:Parameter Name="@member_id" />
					</SelectParameters>
				</asp:SqlDataSource>
			</td>
		</tr>
	</table>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
