<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_journal_entries" Codebehind="journal_entries.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>






<p>
	<dx:ASPxMenu ID="menu" runat="server" ClientInstanceName="menu" 
		ClientVisible="False" Theme="NETheme01">
		<Items>
			<dx:MenuItem Text="Print">
			</dx:MenuItem>
			<dx:MenuItem Text="Note">
			</dx:MenuItem>
		</Items>
	</dx:ASPxMenu>
</p>






<dx:ASPxGridView ID="gv_je" runat="server" AutoGenerateColumns="False" 
	ClientInstanceName="gv_je" KeyFieldName="id" 
	Theme="NETheme01" Width="1000px">
	<ClientSideEvents BatchEditStartEditing="function(s, e) {
	menu.Show();
}" />
	<Columns>
		<dx:GridViewCommandColumn ShowDeleteButton="True" ShowNewButtonInHeader="True" 
			VisibleIndex="0">
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="div" 
			VisibleIndex="1" Caption="Div">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="acctno" 
			VisibleIndex="2" Caption="Acct. No.">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
			VisibleIndex="3">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Memo" FieldName="memo" VisibleIndex="4">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Debit Amount" FieldName="Debit Amount" 
			VisibleIndex="5">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Credit Amount" FieldName="Credit Amount" 
			VisibleIndex="6">
		</dx:GridViewDataTextColumn>
	</Columns>
	<SettingsPager Visible="False">
	</SettingsPager>
	<SettingsEditing Mode="Batch">
	</SettingsEditing>
	<SettingsText CommandBatchEditCancel="Clear" CommandBatchEditUpdate="Post" />
</dx:ASPxGridView>





	<asp:HiddenField ID="hdn_tasks_woid" runat="server" />

