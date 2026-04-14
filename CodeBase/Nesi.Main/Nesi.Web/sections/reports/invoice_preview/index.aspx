<%@ Page Language="C#" AutoEventWireup="true" EnableTheming="True" Inherits="sections_reports_invoice_preview_index" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<link type="text/css" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/themes/base/jquery-ui.css" rel="Stylesheet" />
	<title runat="server" id="report_title">Invoice Preview</title>
</head>
<body>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript">
		$(document).ready(function(s,e)
			{
			page_obj.update_panel_progress.bind();
			});
		function SetIndex(index)
			{
			var cbx = tb.getTemplateControl("BrokenOut");
			if (cbx == null)
				return;
			if (index == 4)
				{
				index = 3;
				}
			cbx.SetSelectedIndex(index);
			}
		function resize_report()
			{
			var myElement = $('#InvoicePreviewer_Div');
			var sWidth = myElement.width() + 100;
			var sHeight = myElement.height() + 50;
			window.resizeTo(sWidth, sHeight);
			}
		function toolbar_click(s,e)
			{
			var cbx = tb.getTemplateControl("BrokenOut");
			switch(e.item.name)
				{
				case "Email":
					popupemail.Show();
					break;
				case "default_invoice":
					cb_default_invoice.PerformCallback(cbx.GetValue());
					break;
				}
			}

		//setTimeout("resize_report()", 2000);
	</script>
	<form id="form1" runat="server">
			<div id="script_reg" runat="server">
			</div>
			<asp:ScriptManager ID="sm" runat="server">
			</asp:ScriptManager>
			<dx:ReportToolbar ID="ReportToolbar1" runat="server" ClientInstanceName="tb"
				ReportViewer="<%# InvoicePreviewer %>" ShowDefaultButtons="False" Width="100%">
				<Paddings Padding="0px" />
				<Paddings Padding="0px"></Paddings>
				<Items>
					<dx:ReportToolbarButton ItemKind="Search" />
					<dx:ReportToolbarSeparator />
					<dx:ReportToolbarButton ItemKind="PrintReport" Enabled="False" />
					<dx:ReportToolbarButton ItemKind="PrintPage" Enabled="False" />
					<dx:ReportToolbarSeparator />
					<dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
					<dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
					<dx:ReportToolbarLabel ItemKind="PageLabel" />
					<dx:ReportToolbarComboBox ItemKind="PageNumber" Width="40px">
					</dx:ReportToolbarComboBox>
					<dx:ReportToolbarLabel ItemKind="OfLabel" />
					<dx:ReportToolbarTextBox ItemKind="PageCount" />
					<dx:ReportToolbarButton ItemKind="NextPage" />
					<dx:ReportToolbarButton ItemKind="LastPage" />
					<dx:ReportToolbarSeparator />
					<dx:ReportToolbarButton ItemKind="SaveToDisk" Enabled="False" />
					<dx:ReportToolbarButton ItemKind="SaveToWindow" Enabled="False" />
					<dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="50px">
						<Elements>
							<dx:ListElement Value="pdf" />
						</Elements>
					</dx:ReportToolbarComboBox>
					<dx:ReportToolbarButton ImageUrl="/images/icon/icon[email].gif" Name="Email" Text="Email" ToolTip="Email this Invoice" Enabled="False" />
					<dx:ReportToolbarComboBox Name="BrokenOut" Width="100px">
						<Elements>
							<dx:ListElement Text="Default with Grouping" Value="0" />
							<dx:ListElement Text="All items broken out, no material prices" Value="1" />
							<dx:ListElement Text="All items broken out, material prices shown" Value="2" />
							<dx:ListElement Text="Labour Total and Material Total ONLY" Value="4" />
						</Elements>
					</dx:ReportToolbarComboBox>
			     </Items>
				<ClientSideEvents ItemClick="toolbar_click" />
				<Styles>
					<LabelStyle>
						<Margins MarginLeft="3px" MarginRight="3px" />
					</LabelStyle>
				</Styles>
			</dx:ReportToolbar>
		<center>
			<dx:ReportViewer ID="InvoicePreviewer" runat="server" >
				<SettingsLoadingPanel Delay="0" Text="" />
				<LoadingPanelImage Url="/images/loading_panel.gif">
				</LoadingPanelImage>
			</dx:ReportViewer>
		</center>
		
				<asp:UpdatePanel runat="server" ID="up">
					<ContentTemplate>
				
		<dx:ASPxPopupControl ID="popupemail" runat="server" ClientInstanceName="popupemail" AllowDragging="true" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" HeaderText="Send Email" Theme="NETheme01">
			<ContentCollection>
				<dx:PopupControlContentControl runat="server">
					<table width="600" cellpadding="2">
						<tbody>
							<tr>
								<td style="width: 80px">To:</td>
								<td colspan="2">
									<dx:ASPxTextBox runat="server" Width="277px" ID="txtEmailAddress" Theme="NETheme01">
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td style="width: 80px">Cc:</td>
								<td colspan="2">
									<dx:ASPxTextBox runat="server" Width="277px" ID="txtEmailCC" Theme="NETheme01">
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td style="width: 80px">Subject:</td>
								<td colspan="2">
									<dx:ASPxTextBox runat="server" Width="100%" ID="txtSubject" Theme="NETheme01">
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td style="width: 80px">Body:</td>
								<td colspan="2">
									<dx:ASPxMemo runat="server" Height="100px" Width="100%"
										ClientInstanceName="memoBody" ID="memoBody" Theme="NETheme01">
										<BackgroundImage HorizontalPosition="center" VerticalPosition="center" />
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td style="width: 80px">
									<dx:ASPxButton runat="server"
										Text="Send"
										ID="btnSendEmail" OnClick="btnSendEmail_Click" Theme="NETheme01">
									</dx:ASPxButton>
								</td>
								<td style="width: 50px">
									<dx:ASPxCheckBox runat="server" Wrap="False" Text="CC to me..."
										ID="chksendtome" Theme="NETheme01">
									</dx:ASPxCheckBox>
								</td>
								<td style="width: 750px">
									<dx:ASPxTextBox runat="server" Width="250px" ID="txtmyemail" Theme="NETheme01">
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td colspan="3">
									<dx:ASPxListBox runat="server" CallbackPageSize="6"
										DataSourceID="SqlDataSource1" Width="100%" ID="ASPxListBox1" Theme="NETheme01">
										<Columns>
											<dx:ListBoxColumn FieldName="Date"></dx:ListBoxColumn>
											<dx:ListBoxColumn FieldName="Member"></dx:ListBoxColumn>
											<dx:ListBoxColumn FieldName="Event"></dx:ListBoxColumn>
										</Columns>
									</dx:ASPxListBox>
									<asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
SelectCommand="SELECT 
	collection_history.date AS `Date`,
    member_fullname AS Member, 
    collection_history.action AS Event, 
    collection_history.woprog_id 
FROM member 
INNER JOIN collection_history ON member.Member_ID = collection_history.member_id where woprog_id = ?woprog_id ORDER BY `Date`

 DESC" ID="SqlDataSource1">
                                         <SelectParameters>
                                            <asp:QueryStringParameter Name="woprog_id" QueryStringField="id" />
                                        </SelectParameters>
									</asp:SqlDataSource>
								</td>
							</tr>
						</tbody>
					</table>
				</dx:PopupControlContentControl>
			</ContentCollection>

		</dx:ASPxPopupControl>
			</ContentTemplate>

		</asp:UpdatePanel>
		<dx:ASPxCallback ID="cb_default_invoice" runat="server" ClientInstanceName="cb_default_invoice" OnCallback="cb_default_invoice_Callback">
			<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}"
				CallbackComplete="function(s, e) {
	if(e.result != &quot;SUCCESS&quot;)
		{
		alert(e.result);
		}
	else
		{
		alert(&quot;Successfully saved the default invoice type for this work order&quot;);
		}
}"
				EndCallback="function(s, e) {
	please_wait(&quot;end&quot;);
}" />
		</dx:ASPxCallback>
	</form>
</body>
</html>
