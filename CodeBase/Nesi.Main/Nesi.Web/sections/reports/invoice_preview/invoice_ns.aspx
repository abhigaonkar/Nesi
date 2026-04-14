<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="invoice_ns.aspx.cs" Inherits="Nesi.Web.sections.reports.invoice_preview.invoice_ns" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link type="text/css" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/themes/base/jquery-ui.css" rel="Stylesheet" />
    <title runat="server" id="report_title">Invoice</title>
</head>
<body>
    <script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
    <script type="text/javascript">		
		function toolbar_click(s,e)
			{		
					popupemail.Show();			
			}		
	</script>
    <form id="form1" runat="server" >
        <asp:ScriptManager ID ="sm" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID ="up" runat="server">
            <ContentTemplate>
        <div style="margin:2VH;text-align:center;">       					
		<dx:ASPxButton ID="email_invoice" Image-Url="/images/icon/icon[email].gif" Name="Email" Text="Email" ToolTip="Email this Invoice" runat="server" Enabled="False" >			 
	         <ClientSideEvents Click="toolbar_click" /> 		
		     </dx:ASPxButton> 
        
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
        </div>
        <div style="margin:5vh">
	    <iframe id="iframe_invoice" runat="server" frameborder="0" height="700" name="iframe_invoice" width="100%"></iframe>
</div>
                </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
