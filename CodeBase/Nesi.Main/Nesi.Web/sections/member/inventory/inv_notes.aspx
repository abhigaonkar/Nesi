<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_member_inventory_inv_notes" Theme="NETheme01" Codebehind="inv_notes.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<div style="margin:10px;">
		<textarea id='note' style="width:512px;height:150px;"></textarea><br />
		<button type="button" onclick="if($('#note').val().trim() == ''){alert('Please enter a note');}else{cb.PerformCallback($('#note').val());}">Save Note</button>
		<br />
		<dx:ASPxCallback ID="cb" ClientInstanceName="cb" runat="server" oncallback="cb_Callback">
			<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;, &quot;Saving Note&quot;)
}" CallbackComplete="function(s, e) {
	please_wait(&quot;stop&quot;);
	gv_notes.Refresh();
	$('#note').val('');
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;)
}" />
		</dx:ASPxCallback>
	</div>
	<dx:ASPxGridView ID="gv_notes" ClientInstanceName="gv_notes" runat="server" AutoGenerateColumns="False" DataSourceID="ds_notes" KeyFieldName="id" Theme="Default" Width="100%" EnableTheming="True">
		<Columns>
			<dx:GridViewCommandColumn VisibleIndex="0" ShowClearFilterButton="true" >
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" VisibleIndex="1">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Date" FieldName="dt" VisibleIndex="2" Width="150px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="Member" FieldName="member" VisibleIndex="3" Width="200px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Note" FieldName="note" VisibleIndex="4">
			</dx:GridViewDataTextColumn>
		</Columns>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
		<Styles>
			<Header HorizontalAlign="Center">
			</Header>
		</Styles>
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="ds_notes" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, dt, MEMBER_NAME(member_id) member, note FROM inventory_note WHERE master_id = @master_id AND business_unit_id = @business_unit_id ">
		<SelectParameters>
			<asp:QueryStringParameter Name="@master_id" QueryStringField="id" />
			<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
		</SelectParameters>
	</asp:SqlDataSource>
	<br />
</asp:Content>

