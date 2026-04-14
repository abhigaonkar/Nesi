<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_on_call_report" Title="On Call Schedule Report" Codebehind="on_call_report.aspx.cs" %>

<%@ Register Assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

   
	
	<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<link type="text/css" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/themes/base/jquery-ui.css" rel="Stylesheet" />
	<title runat="server" id="report_title">On Call Preview</title>
	<style type="text/css">
		.style3
		{
			font-family: Arial, Helvetica, sans-serif;
		}
		.style4
		{
			font-size: small;
		}
	</style>
</head>
<body>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript">
		function SetIndex(index)
		{
			
			if (cbx == null)
				return;
			if (index == 4) {
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
		//setTimeout("resize_report()", 2000);
	</script>
	<form id="form1" runat="server">
   <center>
		<div id="script_reg" runat="server">
		</div>
		<asp:ScriptManager ID="sm" runat="server">
		</asp:ScriptManager>

    <table __designer:mapid="1b2" style="width:100%;">
        <tr __designer:mapid="1b3">
            <td __designer:mapid="1b4" nowrap="nowrap">From Date:</td>
            <td __designer:mapid="1b5">
                <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server">
                </dx:ASPxDateEdit>
            </td>
            <td __designer:mapid="1b6" width="100%">&nbsp;</td>
        </tr>
        <tr __designer:mapid="1b7">
            <td __designer:mapid="1b8">To Date:</td>
            <td __designer:mapid="1b9">
                <dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server">
                </dx:ASPxDateEdit>
            </td>
            <td __designer:mapid="1ba">&nbsp;</td>
        </tr>
        <tr __designer:mapid="1bb">
            <td __designer:mapid="1bc">Business Units:</td>
            <td __designer:mapid="1bd"><dx:ASPxCheckBoxList ID="cl_companies" runat="server" 
					DataSourceID="SqlDataSource1" RepeatColumns="3" TextField="name" 
					Theme="NETheme01" ValueField="business_unit_id" ClientInstanceName="cl_companies" Width="750px">
				</dx:ASPxCheckBoxList>
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
					SelectCommand="select id business_unit_id, name from business_unit  where active='T' and active = 'T' and id not in(8,11,48,26) order by name ">
				</asp:SqlDataSource></td>
            <td __designer:mapid="1be">&nbsp;</td>
        </tr>
       
        <tr __designer:mapid="1bb">
            <td __designer:mapid="1bc" colspan="2" align="left">
                <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="Refresh">
                </dx:ASPxButton>
            </td>
            <td __designer:mapid="1be">&nbsp;</td>
        </tr>
       
    </table>
   
	<dx:ReportToolbar ID="ReportToolbar1" runat="server" ClientInstanceName="tb" 
			ReportViewer="<%# InvoicePreviewer %>" ShowDefaultButtons="False" Width="1000px" ReportViewerID="InvoicePreviewer" 
			>
			<Paddings Padding="0px" />
<Paddings Padding="0px"></Paddings>
			<Items>
				<dx:ReportToolbarButton ItemKind="Search" />
				<dx:ReportToolbarSeparator />
				<dx:ReportToolbarButton ItemKind="PrintReport"  />
				<dx:ReportToolbarButton ItemKind="PrintPage"  />
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
				<dx:ReportToolbarButton ItemKind="SaveToDisk"  />
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
				<dx:ReportToolbarButton ImageUrl="~/images/icon/icon[email].gif" Name="Email" Text="Email" ToolTip="Email this Invoice"  />
				
			</Items>
			<ClientSideEvents ItemClick="function(s, e) {

			
switch(e.item.name)
	{
	case &quot;Email&quot;:
		popupemail.Show();
	break;
	
	}
}" />

<ClientSideEvents ItemClick="function(s, e) {

		
switch(e.item.name)
	{
	case &quot;Email&quot;:
		popupemail.Show();
	break;
	
	}
}"></ClientSideEvents>

			<Styles>
				<LabelStyle>
					<Margins MarginLeft="3px" MarginRight="3px" />
<Margins MarginLeft="3px" MarginRight="3px"></Margins>
				</LabelStyle>
			</Styles>
		</dx:ReportToolbar>
		<span class="style3"><span class="style4">For Chrome users open </span>
		<a href="chrome://plugins/"><span class="style4">chrome://plugins/</span></a><span 
			class="style4"> in your browser and Disable <strong>Chrome PDF</strong> 
		Viewer and Enable <strong>Adobe Reader</strong></span></span><br />
		<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"  Modal="True" Text="">
			<Image Url="../../../images/loading_panel.gif">
			</Image>
		    <Border BorderStyle="None" />
		</dx:ASPxLoadingPanel>
		<dx:ReportViewer ID="InvoicePreviewer" runat="server"  
			>
			<settingsloadingpanel delay="0" text="" />
			<loadingpanelimage url="~/images/loading_panel.gif">
			</loadingpanelimage>
		</dx:ReportViewer>
	</center>
		<dx:ASPxPopupControl ID="popupemail" runat="server" ClientInstanceName="popupemail" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Email" Theme="NETheme01" ModalBackgroundStyle-Opacity="0">
		<ContentCollection>
			<dx:PopupControlContentControl runat="server">
				
					
							<table width="600" cellpadding="2">
								<tbody>
									<tr>
										<td style="width: 80px">
											<span style="font-family: Arial">To:</span>
										</td>
										<td colspan="2">
											<dx:ASPxTextBox runat="server" Width="277px" ID="txtEmailAddress" 
												Theme="NETheme01">
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td style="width: 80px">
											<span style="font-family: Arial">Cc:</span>
										</td>
										<td colspan="2">
											<dx:ASPxTextBox runat="server" Width="277px" ID="txtEmailCC" Theme="NETheme01">
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td style="width: 80px">
											<span style="font-family: Arial">Subject:</span>
										</td>
										<td colspan="2">
											<dx:ASPxTextBox runat="server" Width="100%" ID="txtSubject" Theme="NETheme01">
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td style="width: 80px">
											<span style="font-family: Arial">Body:</span>
										</td>
										<td colspan="2">
											<dx:ASPxMemo runat="server" Height="100px" Width="100%" 
												ClientInstanceName="memoBody" ID="memoBody" Theme="NETheme01">
												<BackgroundImage HorizontalPosition="center" VerticalPosition="center" />
												<BackgroundImage HorizontalPosition="center" VerticalPosition="center"></BackgroundImage>
											</dx:ASPxMemo>
										</td>
									</tr>
									<tr>
										<td style="width: 80px">
											<dx:ASPxButton runat="server" 
											Text="Send" 
											 
												ID="btnSendEmail" OnClick="btnSendEmail_Click" Theme="NETheme01">
												<ClientSideEvents Click="function(s, e) {
	loadingPanel.Show();
}"></ClientSideEvents>
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
								</tbody>
							</table>
					
					
			</dx:PopupControlContentControl>
		</ContentCollection>
		
	</dx:ASPxPopupControl>
   
</form>
</body>
</html>
   
