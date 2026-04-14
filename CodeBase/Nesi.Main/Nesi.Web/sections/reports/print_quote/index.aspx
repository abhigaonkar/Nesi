<%@ page language="C#" autoeventwireup="true" inherits="print_quote" EnableTheming="True" Codebehind="index.aspx.cs" %>
<%@ register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ register assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Untitled Page</title>
</head>
<body>
	<form id="form1" runat="server">
		<div style="text-align: center;">
			<dx:aspxpopupcontrol id="popupemail" runat="server" clientinstancename="popupemail" PopupAnimationType="Slide"
				closeaction="CloseButton" modal="True" popuphorizontalalign="WindowCenter" popupverticalalign="WindowCenter" headertext="" Theme="NETheme01">
				<contentcollection>
					<dx:popupcontrolcontentcontrol runat="server">
				
									<table width="600" cellpadding="2">
										<tbody>
											<tr>
												<td style="width: 80px"><span style="font-family: Arial">To:</span></td>
												<td colspan="2">
													<dx:aspxtextbox id="txtEmailAddress" runat="server" enabled="False" width="277px">
													</dx:aspxtextbox>
												</td>
											</tr>
											<tr>
												<td style="width: 80px"><span style="font-family: Arial">Cc:</span></td>
												<td colspan="2">
													<dx:aspxtextbox runat="server" width="277px" id="txtEmailCC"></dx:aspxtextbox>

												</td>
											</tr>
											<tr>
												<td style="width: 80px"><span style="font-family: Arial">Subject:</span></td>
												<td colspan="2">
													<dx:aspxtextbox runat="server" width="100%" id="txtSubject"></dx:aspxtextbox>

												</td>
											</tr>
											<tr>
												<td style="width: 80px"><span style="font-family: Arial">Body:</span></td>
												<td colspan="2">
													<dx:aspxmemo runat="server" height="80px" width="100%" clientinstancename="memoBody" id="memoBody">
														<backgroundimage horizontalposition="center" verticalposition="center" />
													</dx:aspxmemo>

												</td>
											</tr>
											<tr>
												<td style="width: 80px">
													<dx:aspxbutton runat="server" text="Send" id="btnSendEmail" UseSubmitBehavior="false" onclick="btnSendEmail_Click">
														<clientsideevents click="function(s, e) {
	                                                        loadingPanel.Show();
                                                        if(window.top && window.top.location)
                                                            setTimeout(function (){window.top.location.reload();}, 18000);
                                                            
                                                        }"></clientsideevents>
													</dx:aspxbutton>

												</td>
												<td style="width: 50px">
													<dx:aspxcheckbox runat="server" wrap="False" text="CC to me..." id="chksendtome"></dx:aspxcheckbox>

												</td>
												<td style="width: 750px">
													<dx:aspxtextbox runat="server" width="250px" id="txtmyemail"></dx:aspxtextbox>

												</td>
											</tr>
											<tr>
												<td colspan="3">
													<dx:aspxlistbox runat="server" callbackpagesize="6" datasourceid="SqlDataSource1" width="100%" id="ASPxListBox1">
														<columns>
															<dx:listboxcolumn fieldname="Date"></dx:listboxcolumn>
															<dx:listboxcolumn fieldname="Member"></dx:listboxcolumn>
															<dx:listboxcolumn fieldname="Event"></dx:listboxcolumn>
														</columns>
													</dx:aspxlistbox>

													<asp:sqldatasource runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" selectcommand="SELECT b.create_datetime AS `Date`,
 a.member_fullname AS Member, 
 b.event AS Event,
 b.quote_id,
 b.revision 
 FROM member a

 INNER JOIN quote_history b ON a.Member_ID = b.created_by 
 where CONCAT(b.quote_id, b.revision) = @quote_id 
 ORDER BY `Date` DESC " ID="SqlDataSource1"
>

													    <SelectParameters>
                                                            <asp:QueryStringParameter Name="@quote_id" QueryStringField="quoteid" />
                                                        </SelectParameters>

													</asp:sqldatasource>


												</td>
											</tr>
										</tbody>
									</table>

					</dx:popupcontrolcontentcontrol>
				</contentcollection>
				<headerstyle backcolor="Transparent" />
                <ModalBackgroundStyle Opacity="0" />
			</dx:aspxpopupcontrol>
			<dx:aspxloadingpanel id="loadingPanel" runat="server" clientinstancename="loadingPanel"
			modal="True" text="Sending Quote via Email&hellip;">
		</dx:aspxloadingpanel>
			<dx:reporttoolbar id="ReportToolbar1" runat="server" Width="100%" reportviewer="<%# ReportViewer1 %>"
				showdefaultbuttons="False">
				<items>
					<dx:reporttoolbarbutton name="Email" text="Email" />
					<dx:reporttoolbarbutton itemkind="Search" />
					<dx:reporttoolbarseparator />
					<dx:reporttoolbarbutton itemkind="PrintReport" name="PrintReport" />
					<dx:reporttoolbarbutton itemkind="PrintPage" />
					<dx:reporttoolbarseparator />
					<dx:reporttoolbarbutton enabled="False" itemkind="FirstPage" />
					<dx:reporttoolbarbutton enabled="False" itemkind="PreviousPage" />
					<dx:reporttoolbarlabel itemkind="PageLabel" />
					<dx:reporttoolbarcombobox itemkind="PageNumber" width="65px">
					</dx:reporttoolbarcombobox>
					<dx:reporttoolbarlabel itemkind="OfLabel" />
					<dx:reporttoolbartextbox itemkind="PageCount" />
					<dx:reporttoolbarbutton itemkind="NextPage" />
					<dx:reporttoolbarbutton itemkind="LastPage" />
					<dx:reporttoolbarseparator />
					<dx:reporttoolbarbutton itemkind="SaveToDisk" />
					<dx:reporttoolbarbutton itemkind="SaveToWindow" />
				</items>
				<styles>
					<labelstyle>
						<margins marginleft="3px" marginright="3px" />
					</labelstyle>
				</styles>
				<clientsideevents itemclick="function(s, e) {

	if (e.item.name == 'Email')
	popupemail.Show();
}" />
			</dx:reporttoolbar>
			<center>
			<dx:reportviewer id="ReportViewer1" clientinstancename="ReportViewer1" runat="server">
			</dx:reportviewer>
			</center>
		</div>
	</form>
</body>
</html>
