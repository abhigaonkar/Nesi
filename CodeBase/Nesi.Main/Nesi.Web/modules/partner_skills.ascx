<%@ Control Language="C#" AutoEventWireup="true"  Inherits="modules_partner_skills" EnableTheming="True" Codebehind="partner_skills.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	DeleteCommand="Select from partner_skills where id = ?id" 
	InsertCommand="INSERT INTO [partner_skills] ([skill], [added_by], [date_added], [table], [table_id]) VALUES (?skill, ?mid, curdate(), ?_table, ?_tableid)" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	SelectCommand="Select * from partner_skills where `table` = ?table and table_id = ?tableid" 
	
	UpdateCommand="update partner_skills set skill=?skill, date_added=curdate(), added_by=?mid where id = ?id">
	<DeleteParameters>
		<asp:Parameter Name="id" Type="Int32" />
	</DeleteParameters>
	<InsertParameters>
						<asp:Parameter Name="skill" Type="String" />
						<asp:ControlParameter ControlID="hdn_table" Name="_table" PropertyName="Value" />
						<asp:ControlParameter ControlID="hdn_member_id" Name="mid" PropertyName="Value" />
						<asp:ControlParameter ControlID="hdn_table_id" Name="_tableid" PropertyName="Value" />
	</InsertParameters>
	<SelectParameters>
		<asp:ControlParameter ControlID="hdn_table" Name="table" PropertyName="Value" />
		<asp:ControlParameter ControlID="hdn_table_id" Name="tableid" PropertyName="Value" />
	</SelectParameters>
	<UpdateParameters>
		<asp:Parameter Name="skill" Type="String" />
		<asp:Parameter Name="id" Type="Int32" />
		<asp:ControlParameter ControlID="hdn_member_id" Name="mid" PropertyName="Value" />
	</UpdateParameters>
</asp:SqlDataSource>
<dx:ASPxGridView ID="gv" runat="server" Theme="NETheme01" 
	AutoGenerateColumns="False" DataSourceID="SqlDataSource1" KeyFieldName="id" 
	 Width="100%" ClientInstanceName="gv" oncustomcallback="gv_CustomCallback"  >
	<ClientSideEvents BatchEditStartEditing="function(s, e) {
	
edit_mode.SetVisible(true);
hl_save.SetVisible(true);
hl_cancel.SetVisible(true);



}" CustomButtonClick="function(s, e) {
	if (e.buttonID=='delete')
{
if (confirm('Are you sure you want to delete this task?'))
{
gv.PerformCallback('DELETE|'+e.visibleIndex);
}
}

}" />
	<Columns>
		<dx:GridViewCommandColumn ButtonType="Image" ShowNewButtonInHeader="True" VisibleIndex="0" 
			Width="100px">
			<CustomButtons>
													<dx:GridViewCommandColumnCustomButton ID="delete">
														<Image Url="~/images/icon/icon[delete].gif">
														</Image>
													</dx:GridViewCommandColumnCustomButton>
											
												</CustomButtons>
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" 
			VisibleIndex="1">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="table" Visible="False" VisibleIndex="2">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="table_id" Visible="False" 
			VisibleIndex="3">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Skill" FieldName="skill" VisibleIndex="4" 
			Width="100%">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataDateColumn Caption="Date Added" FieldName="date_added" 
			VisibleIndex="5" Width="110px">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
				EditFormatString="yyyy-MM-dd">
			</PropertiesDateEdit>
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataComboBoxColumn Caption="Added By" FieldName="added_by" 
			VisibleIndex="6" Width="100px">
			<PropertiesComboBox DataSourceID="sqlmember" TextField="_name" 
				ValueField="member_id" ValueType="System.Int32">
			</PropertiesComboBox>
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataComboBoxColumn>
	</Columns>
	<SettingsEditing Mode="Batch">
	</SettingsEditing>
	<SettingsCommandButton>
		<NewButton>
			<Image Url="~/images/icon/icon[add].gif">
			</Image>
		</NewButton>
		<EditButton>
			<Image Url="~/images/icon/icon[edit].gif">
			</Image>
		</EditButton>
		<DeleteButton>
			<Image Url="~/images/icon/icon[delete].gif">
			</Image>
		</DeleteButton>
	</SettingsCommandButton>

	<Templates>
											
											<StatusBar>
												<table style="width:100%;">
													<tr>
														<td style="text-decoration: blink; font-family: Arial, Helvetica, sans-serif; font-size: 14px" 
															width="100%" align="left">
															<dx:ASPxLabel ID="edit_mode" runat="server" ClientInstanceName="edit_mode" 
																ClientVisible="False" Font-Bold="True" Font-Names="Arial" ForeColor="Red" 
																Text="Edit Mode">
															</dx:ASPxLabel>
														</td>
														<td>
															<dx:ASPxHyperLink ID="hl_save" runat="server" Cursor="pointer" Text="Save" 
																Width="50px" ClientVisible="False" ClientInstanceName="hl_save">
																<ClientSideEvents Click="function(s, e){edit_mode.SetVisible(false);hl_save.SetVisible(false);hl_cancel.SetVisible(false); gv.UpdateEdit(); }" />
															</dx:ASPxHyperLink>
														</td>
														<td>
															<dx:ASPxHyperLink ID="hl_cancel" runat="server" Cursor="pointer" Text="Cancel" ClientVisible="False" ClientInstanceName="hl_cancel">
																<ClientSideEvents Click="function(s, e){  edit_mode.SetVisible(false);hl_save.SetVisible(false);hl_cancel.SetVisible(false); gv.CancelEdit();  }" />
															</dx:ASPxHyperLink>
														</td>
													</tr>
												</table>
											</StatusBar>
										</Templates>


</dx:ASPxGridView>
<asp:HiddenField ID="hdn_table" runat="server" />
<asp:HiddenField ID="hdn_table_id" runat="server" />
<asp:HiddenField ID="hdn_member_id" runat="server" />
<asp:SqlDataSource ID="sqlmember" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	SelectCommand="Select member.member_id, Concat('(',business_unit.name,') ', member.member_fullname) _name from member inner join business_unit on business_unit.id = member.business_unit_id order by _name,member_fullname">
</asp:SqlDataSource>
