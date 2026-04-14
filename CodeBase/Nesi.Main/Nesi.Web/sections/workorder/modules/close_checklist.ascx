<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_workorder_modules_close_checklist" Codebehind="close_checklist.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>






<dx:ASPxGridView ID="gv_checklist" runat="server" AutoGenerateColumns="False" 
	ClientInstanceName="gv_checklist" DataSourceID="SqlDataSource1" KeyFieldName="id" 
	Theme="NETheme01" Width="99%"> 
	
	<Columns>
		<dx:GridViewCommandColumn 
			VisibleIndex="0" Caption=" " Width="60px" Visible="False">
			
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" 
			VisibleIndex="1">
			
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="woprog_id" Visible="False" 
			VisibleIndex="2">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Checklist Item" FieldName="checklist_text" VisibleIndex="3">
		    <EditFormSettings Visible="False" />
            <CellStyle Wrap="True">
            </CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataCheckColumn Caption="Complete?" FieldName="complete" 
			VisibleIndex="4" >
		    <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
            </PropertiesCheckEdit>
		</dx:GridViewDataCheckColumn>
	<dx:GridViewDataTextColumn Caption="Comment" FieldName="comment" VisibleIndex="5">
		<CellStyle Wrap="True">
        </CellStyle>
		</dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Completed By" FieldName="member_fullname" VisibleIndex="6">
		<CellStyle Wrap="True">
        </CellStyle>
		</dx:GridViewDataTextColumn>
	</Columns>
	<SettingsPager Visible="False">
	</SettingsPager>
	
    <SettingsEditing Mode="Batch">
    </SettingsEditing>
	
    <SettingsText EmptyDataRow="There are no checklist items for this work order" />
	
</dx:ASPxGridView>





	<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select * from wo_checklist_history wh inner join wo_checklist wc on wh.wo_checklist_id=wc.id
        left join member on member.member_id = wh.member_id where woprog_id = ?woid"
        UpdateCommand="Update wo_checklist_history set complete=@complete, member_id=@member_id where id=@id">
		<SelectParameters>
			<asp:ControlParameter ControlID="hdn_tasks_woid" Name="woid" 
				PropertyName="Value" />
		</SelectParameters>
	<UpdateParameters>
        <asp:Parameter Name="complete" />
     <asp:ControlParameter ControlID="hdn_member_id" Name="member_id" 
				PropertyName="Value" />
	</UpdateParameters>
	
	</asp:SqlDataSource>
	<asp:HiddenField ID="hdn_tasks_woid" runat="server" />

<asp:HiddenField ID="hdn_member_id" runat="server" />

