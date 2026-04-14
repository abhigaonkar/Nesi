<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_reports_rates_sheet_index" Theme="NETheme01" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>







<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<link type="text/css" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/themes/base/jquery-ui.css" rel="Stylesheet" />
	<title runat="server" id="report_title">Chargeout Rates</title>
</head>
<body>
	<form id="form1" runat="server">
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript">
		function preventEnter(s,e)
			{
			if(e.htmlEvent.keyCode == 13)
				{
				ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
				}
			}
		
	</script>
	<center>
		<div id="script_reg" runat="server">
		</div>
		<asp:ScriptManager ID="sm" runat="server">
		</asp:ScriptManager>
	</center>
	<div class="dxtcLeftAlignCell_BlackGlass">
		<dx:ASPxComboBox ID="ASPxComboBox1" runat="server" AutoPostBack="True" 
			 TextField="ddl_name" ValueField="id" 
			ValueType="System.Int32" 
			onselectedindexchanged="ASPxComboBox1_SelectedIndexChanged">
		</dx:ASPxComboBox>
	</div>
	
		<dx:ReportToolbar ID="ReportToolbar1" runat="server" ClientInstanceName="tb" ReportViewer="<%# RatesSheetPreviewer %>" ShowDefaultButtons="False" Width="850px">
			<Paddings Padding="0px" />
			<Items>
				<dx:ReportToolbarButton ItemKind="Search" />
				<dx:ReportToolbarSeparator />
				<dx:ReportToolbarButton ItemKind="PrintReport"  />
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
				<dx:ReportToolbarButton ItemKind="SaveToDisk" Enabled="True" />
				<dx:ReportToolbarButton ItemKind="SaveToWindow" Enabled="False" />
				<dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="50px">
					<Elements>
						<dx:ListElement Value="pdf" />
						<dx:ListElement Value="xls" />
						<dx:ListElement Value="xlsx" />
						<dx:ListElement Value="rtf" />
						<dx:ListElement Value="mht" />
						<dx:ListElement Value="html" />
						<dx:ListElement Value="txt" />
						<dx:ListElement Value="csv" />
						<dx:ListElement Value="png" />
					</Elements>
				</dx:ReportToolbarComboBox>
				<dx:ReportToolbarButton ImageUrl="~/images/icon/icon[email].gif" Name="Email" 
					Text="Email" ToolTip="Email this Rates Sheet"  />
			</Items>
			<ClientSideEvents ItemClick="function(s, e) {

			var cbx = tb.getTemplateControl(&quot;BrokenOut&quot;);
switch(e.item.name)
	{
	case &quot;Email&quot;:
		popupemail.Show();
	break;
	
	}
}" />
			<Styles>
				<LabelStyle>
					<Margins MarginLeft="3px" MarginRight="3px" />
				</LabelStyle>
			</Styles>
		</dx:ReportToolbar>
		<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" 
			ClientInstanceName="loadingPanel" 
			CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" 
			Modal="True" Text="Sending Rates Sheet Via Email">
			<Image Url="~/App_Themes/BlackGlass/Web/Loading.gif">
			</Image>
		</dx:ASPxLoadingPanel>
		<dx:ReportViewer ID="RatesSheetPreviewer" runat="server">
		</dx:ReportViewer>
	</center>
	<dx:ASPxPopupControl ID="popupemail" runat="server" ClientInstanceName="popupemail" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="" Theme="NETheme01">
		<ContentCollection>
			<dx:PopupControlContentControl runat="server">
					<ContentPaddings PaddingLeft="9px" PaddingTop="6px" PaddingRight="9px" PaddingBottom="6px"></ContentPaddings>
					
					<ClientSideEvents Init="function(s, e) {
	loadingPanel.Hide();
}"></ClientSideEvents>
					<PanelCollection>
						<dx:PanelContent runat="server">
							<table width="600" cellpadding="2">
								<tbody>
									<tr>
										<td style="width: 80px">
											<span style="font-family: Arial">To:</span>
										</td>
										<td colspan="2">
											<dx:ASPxTextBox runat="server" Width="277px" ID="txtEmailAddress">
												<ClientSideEvents KeyDown="preventEnter" />
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td style="width: 80px">
											<span style="font-family: Arial">Cc:</span>
										</td>
										<td colspan="2">
											<dx:ASPxTextBox runat="server" Width="277px" ID="txtEmailCC">
												<ClientSideEvents KeyDown="preventEnter" />
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td style="width: 80px">
											<span style="font-family: Arial">Subject:</span>
										</td>
										<td colspan="2">
											<dx:ASPxTextBox runat="server" Width="100%" ID="txtSubject">
												<ClientSideEvents KeyDown="preventEnter" />
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td style="width: 80px">
											<span style="font-family: Arial">Body:</span>
										</td>
										<td colspan="2">
											<dx:ASPxMemo runat="server" Height="80px" Width="100%" ClientInstanceName="memoBody" ID="memoBody">
												<BackgroundImage HorizontalPosition="center" VerticalPosition="center" />
												<BackgroundImage HorizontalPosition="center" VerticalPosition="center"></BackgroundImage>
											</dx:ASPxMemo>
										</td>
									</tr>
									<tr>
										<td style="width: 80px">
											<dx:ASPxButton runat="server"  Text="Send" CssPostfix="BlackGlass"  ID="btnSendEmail" OnClick="btnSendEmail_Click">
												<ClientSideEvents Click="function(s, e) {
	loadingPanel.Show();
}"></ClientSideEvents>
											</dx:ASPxButton>
										</td>
										<td style="width: 50px">
											<dx:ASPxCheckBox runat="server" Wrap="False" Text="CC to me..." ID="chksendtome">
											</dx:ASPxCheckBox>
										</td>
										<td style="width: 750px">
											<dx:ASPxTextBox runat="server" Width="250px" ID="txtmyemail">
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td colspan="3">
											<asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT collection_history.date AS `Date`, member_fullname AS Member, collection_history.action AS Event, collection_history.woprog_id FROM member INNER JOIN collection_history ON member.Member_ID = collection_history.member_id ORDER BY `Date` DESC" ID="SqlDataSource1"></asp:SqlDataSource>
										</td>
									</tr>
								</tbody>
							</table>
						</dx:PanelContent>
					</PanelCollection>
					
				
			</dx:PopupControlContentControl>
		</ContentCollection>
		
	</dx:ASPxPopupControl>
	
	</form>
</body>
</html>
