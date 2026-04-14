<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="Holidays" Title="Statutory Holiday Management" Theme="BlueStyle" Codebehind="Holidays.aspx.cs" %>
<%@ MasterType VirtualPath="~/IntraDefault.master" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
   <div id="divMenu" runat="server">
   </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<script type ="text/javascript">
</script>
    <asp:ScriptManager ID="sm" runat="server">
	</asp:ScriptManager>
				<asp:UpdatePanel ID="up_new" runat="server">
					<ContentTemplate>
    <dx:ASPxGridView ID="gv_holidays" runat="server" AutoGenerateColumns="False" DataSourceID="sds_holidays" KeyFieldName="Holidays_ID" Width="750px" onrowdeleting="gv_holidays_RowDeleting" Theme="NETheme01">
        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
		<Columns>
			<dx:GridViewCommandColumn ButtonType="Button" Caption="Action" VisibleIndex="5" Width="5%" ShowDeleteButton="true" ShowClearFilterButton="true">
				
				
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="ID" FieldName="Holidays_ID" ReadOnly="True" Visible="False" VisibleIndex="0">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Date" FieldName="Holidays_Date" VisibleIndex="2" Width="10%">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="Holiday" FieldName="Holidays_Name" VisibleIndex="1" Width="50%">
				<CellStyle Font-Bold="True" HorizontalAlign="Left">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Canadian" FieldName="is_canada" VisibleIndex="3" Width="5%">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="American" FieldName="is_america" VisibleIndex="4" Width="5%">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior EnableRowHotTrack="True" />
		<SettingsPager PageSize="15">
		</SettingsPager>
		<Settings ShowTitlePanel="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
		<Styles>
			<TitlePanel>
				<Paddings Padding="0px" />
			</TitlePanel>
		</Styles>
		<Templates>
			<TitlePanel>
						<table cellpadding="2" cellspacing="0" style="width:100%;">
							<tr style="font-weight:bold;font-size:11px;color:#000;">
								<td align="center">
									Name</td>
								<td align="center">
									Date</td>
								<td align="center">
									Canadian?</td>
								<td align="center">
									American?</td>
								<td>
									&nbsp;</td>
							</tr>
							<tr>
								<td align="center" class="style6">
									<dx:ASPxTextBox ID="name" runat="server" NullText="Holiday's Name" Width="170px">
									</dx:ASPxTextBox>
								</td>
								<td align="center" class="style6">
									<dx:ASPxDateEdit ID="date" runat="server" DisplayFormatString="yyyy-MM-dd" NullText="Select a Date" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style6">
									<dx:ASPxCheckBox ID="cb_canadian" runat="server">
									</dx:ASPxCheckBox>
								</td>
								<td align="center" class="style6">
									<dx:ASPxCheckBox ID="cb_american" runat="server">
									</dx:ASPxCheckBox>
								</td>
								<td class="style6">
									<dx:ASPxButton ID="bt_save" runat="server" onclick="bt_save_Click" Text="Save">
										<Image Url="~/images/icon/icon[save].gif">
										</Image>
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
			</TitlePanel>
		</Templates>
	</dx:ASPxGridView>
    <asp:SqlDataSource ID="sds_holidays" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT Holidays_ID, DATE_FORMAT(Holidays_Date, '%Y-%m-%d') Holidays_Date, Holidays_Name, if(Holidays_Canada, 'Yes', 'No') is_canada, IF(Holidays_America = 1, 'Yes', 'No') is_america from Holidays WHERE Holidays_Active = 1 ORDER BY Holidays_Date DESC"></asp:SqlDataSource>
					</ContentTemplate>
				</asp:UpdatePanel>
    <br />
    
       
    
  
</asp:Content>
<asp:Content ID="Content5" runat="server" contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style6 { height: 31px; }
	</style>
</asp:Content>



