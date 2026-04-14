<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_quote_modules_post_mortem" Codebehind="post_mortem.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>







<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>



<style type="text/css">
	.dxbButton
{
	color: #000000;
	font: normal 12px Tahoma, Geneva, sans-serif;
	vertical-align: middle;
	border: 1px solid #7F7F7F;
	
	padding: 1px;
	cursor: pointer;
}
	</style>


	<table style="width:100%; font-family: Arial; font-size: small;">
		<tr>
			<td nowrap="nowrap">
				<dx:ASPxLabel ID="lblstage_killed" runat="server" Text="ASPxLabel">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap">
				<dx:ASPxLabel ID="lbl_whokilled" runat="server" Text="ASPxLabel">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap">
				<dx:ASPxLabel ID="lbl_datekilled" runat="server" Text="ASPxLabel">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap">
				<dx:ASPxLabel ID="lbl_whykilled" runat="server" Text="ASPxLabel">
				</dx:ASPxLabel>
			</td>
		</tr>
	</table>
	<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
		ClientInstanceName="gv" DataSourceID="SqlDataSource1" KeyFieldName="id" 
		OnCustomCallback="gv_CustomCallback" Width="100%">
		<Columns>
			<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
				ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Question" FieldName="question" 
				ShowInCustomizationForm="True" VisibleIndex="0" Width="50%">
				<CellStyle VerticalAlign="Top">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Answer" FieldName="result" 
				ShowInCustomizationForm="True" VisibleIndex="1" Width="50%">
				<DataItemTemplate>
					<dx:ASPxMemo ID="mem_result" runat="server" BackColor="#FFFFCC" 
						Font-Names="Arial" Height="71px" NullText="Enter Answer Here ..." 
						oninit="mem_result_Init" Text='<%# Eval("result") %>' Width="100%" ClientEnabled="False">
					</dx:ASPxMemo>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager PageSize="5">
		</SettingsPager>
		<Settings GridLines="Horizontal" ShowColumnHeaders="False" />
		<Styles>
			<AlternatingRow BackColor="#DDDDDD">
			</AlternatingRow>
		</Styles>
	</dx:ASPxGridView>
	<br />
		<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
	oncallback="cb_Callback" Width="100%">
			<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_close=='close')
{
poppost_mortem.Hide();
window.parent.poppost_mortem.Hide();

}
}" />
		<PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxButton ID="btnclose" runat="server" AutoPostBack="False" 
		Text="Finished" ClientVisible="False">
		<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback();
}" />
	</dx:ASPxButton>
			</dx:PanelContent>
</PanelCollection>
</dx:ASPxCallbackPanel>
<p>
	<br />
</p>
	
<p>

<asp:HiddenField ID="hdn_cid" runat="server" />
	<br />
</p>

<asp:HiddenField ID="hdn_quote_id" runat="server" />
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	SelectCommand="SELECT
quote_post_mortem.id,
quote_post_mortem_questions.question,
quote_post_mortem.result
FROM
quote_post_mortem
INNER JOIN quote_post_mortem_questions ON quote_post_mortem.question_id = quote_post_mortem_questions.id where quote_id = ?qid and status='Active'">
	<SelectParameters>
		<asp:ControlParameter ControlID="hdn_quote_id" Name="qid" 
			PropertyName="Value" />
	</SelectParameters>
</asp:SqlDataSource>


