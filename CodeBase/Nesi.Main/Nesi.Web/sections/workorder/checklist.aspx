<%@ Page Title="" Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_workorder_checklist" Theme="NETheme01" Codebehind="checklist.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content0" ContentPlaceHolderID="header_placeholder" Runat="Server">
</asp:Content>
<ASP:CONTENT ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</ASP:CONTENT>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <dx:ASPxGridView ID="gv_checklist" runat="server" AutoGenerateColumns="False" DataSourceID="sds_jobtag" KeyFieldName="id" Width="500px" SettingsEditing-Mode="Batch">
		<Columns>
			<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" VisibleIndex="1" Width="35px" Visible="False">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Checklist Item" FieldName="checklist_text" VisibleIndex="2">
			</dx:GridViewDataTextColumn>
			<dx:GridViewCommandColumn ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0" Width="50px" >
			</dx:GridViewCommandColumn>
		    <dx:GridViewDataCheckColumn Caption="Status" FieldName="status" VisibleIndex="3">
            </dx:GridViewDataCheckColumn>
		</Columns>

<SettingsEditing Mode="Batch"></SettingsEditing>

		<SettingsDataSecurity AllowDelete="False" />
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="sds_jobtag" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
         
        ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
        SelectCommand="SELECT * FROM wo_checklist" 
        UpdateCommand="UPDATE wo_checklist SET checklist_text = ?checklist_text, status=?status WHERE id = ?id"
        InsertCommand="INSERT INTO wo_checklist (checklist_text,status) VALUES (@checklist_text, 1)">
		<InsertParameters>
			<asp:ControlParameter ControlID="hid_member_id" Name="who" PropertyName="Value" />
			<asp:Parameter Name="checklist_text" />
          
		</InsertParameters>
	</asp:SqlDataSource>
	<asp:HiddenField ID="hid_member_id" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
