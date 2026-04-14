<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wo_tasklist.ascx.cs" Inherits="sections_workorder_modules_wo_tasklist" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<dx:ASPxCheckBox ID="enableTimesheetEditing" runat="server" Text="Time sheet comments from scope items only" AutoPostBack="True" OnCheckedChanged="enableTimesheetEditing_OnCheckedChanged">
</dx:ASPxCheckBox>
<dx:ASPxGridView ID="gv_wotasks" runat="server" AutoGenerateColumns="False" 
	ClientInstanceName="gv_wotasks" DataSourceID="SqlDataSource1" KeyFieldName="id" 
	Theme="MaterialCompact" Width="100%" OnRowDeleting="gv_wotasks_OnRowDeleting" OnCommandButtonInitialize="gv_wotasks_OnCommandButtonInitialize" EnableTheming="True">

	<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" />

<SettingsPopup>
<HeaderFilter MinHeight="140px"></HeaderFilter>
</SettingsPopup>
	<Columns>
		<dx:GridViewCommandColumn ShowNewButtonInHeader="True" 
			VisibleIndex="0" Caption=" " Width="60px" ShowClearFilterButton="True" ShowDeleteButton="True">
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" 
			VisibleIndex="1">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="woprog_id" Visible="False" 
			VisibleIndex="2">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataCheckColumn Caption="Complete?" FieldName="complete" 
			VisibleIndex="6" Width="15%">
			<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
				ValueUnchecked="0">
			</PropertiesCheckEdit>
		</dx:GridViewDataCheckColumn>
		<dx:GridViewDataSpinEditColumn Caption="Priority" FieldName="_order" 
			VisibleIndex="4" Width="10%">
			<PropertiesSpinEdit DisplayFormatString="g" MaxValue="10" MinValue="1" 
				NumberType="Integer">
			</PropertiesSpinEdit>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataSpinEditColumn>
		<dx:GridViewDataTextColumn Caption="# Time Entries" FieldName="n_timeslices" VisibleIndex="3" Width="10%">
			<EditFormSettings Visible="False" />
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataMemoColumn Caption="Scope Item" FieldName="task" VisibleIndex="5" Width="75%">
			<CellStyle Wrap="True">
			</CellStyle>
			<PropertiesMemoEdit>
				<ClientSideEvents GotFocus="function(obj,e)
					{ 
					var textArea = obj.GetInputElement();
					obj.SetHeight(15);
						if (textArea.scrollHeight + 15 > obj.GetHeight()) {
							obj.SetHeight(textArea.scrollHeight + 15);
						}
						if (textArea.scrollHeight + 15 < obj.GetHeight()) {
							obj.SetHeight(textArea.scrollHeight + 15);
						}
					}"></ClientSideEvents>
			</PropertiesMemoEdit>
		</dx:GridViewDataMemoColumn>
	</Columns>
	<SettingsPager Mode="ShowAllRecords">
	</SettingsPager>
	<SettingsEditing Mode="Batch">
	</SettingsEditing>
	<Styles>
		<Header HorizontalAlign="Center">
		</Header>
	</Styles>
</dx:ASPxGridView>





	<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select *, (select count(*) from membertime WHERE scope_id = a.id) n_timeslices from woprog_tasks a where woprog_id = ?woid order by _order, id"
		InsertCommand="INSERT INTO woprog_tasks (woprog_id, task, _order, complete) VALUES(@woid, @task, 1, 0)"
		UpdateCommand="UPDATE woprog_tasks SET task = @task, _order=@_order, complete=@complete WHERE id = @id"
		DeleteCommand="Delete from woprog_tasks WHERE id = @id AND (SELECT COUNT(*) FROM membertime WHERE scope_id = @id) = 0">
		
		<SelectParameters>
			<asp:ControlParameter ControlID="hdn_tasks_woid" Name="woid" 
				PropertyName="Value" />
		</SelectParameters>
		<InsertParameters>
		<asp:ControlParameter ControlID="hdn_tasks_woid" Name="woid" 
				PropertyName="Value" Type="Int32" />
			<asp:Parameter Name="task" />
			<asp:Parameter Name="_order" />
		</InsertParameters>
        <UpdateParameters>
		<asp:ControlParameter ControlID="hdn_tasks_woid" Name="woid" 
				PropertyName="Value" Type="Int32" />
			<asp:Parameter Name="task" />
			<asp:Parameter Name="_order" />
            <asp:Parameter Name="complete" />
		</UpdateParameters>
	
	
	</asp:SqlDataSource>
	<asp:HiddenField ID="hdn_tasks_woid" runat="server" />

