<%@ Page Language="C#" MasterPageFile="../../../IntraDefault.master" AutoEventWireup="true" Inherits="PayrollSubmittal" Title="Payroll Submittal" Codebehind="index.aspx.cs" %>

<%@ register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="summary.ascx" tagname="summary" tagprefix="uc" %>
<%@ Register src="approval.ascx" tagname="approval" tagprefix="uc" %>
<asp:content ID="Content1" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    <asp:sqldatasource ID="Users" runat="server"></asp:sqldatasource>
</div>
</asp:content>
<asp:content ID="Content2" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server"></div>
</asp:content>
<asp:content contentplaceholderid="header_placeholder" runat="server" id="header">
	<script type="text/javascript" src="/js/functions.js?get=new4"></script>
	<script type="text/javascript" src="/js/payroll_approval.js?get=new3"></script>
</asp:content>
<asp:content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
		<dx:aspxcallbackpanel id="cbp_payroll" clientinstancename="cbp_payroll" runat="server" oncallback="cbp_payroll_Callback">
				<panelcollection>
					<dx:panelcontent>
						<div align="right">
							<table cellpadding="0" cellspacing="0">
								<tr>
									<td>
										<dx:aspxcombobox ToolTip="Sorted by tax entity, then name" runat="server"  theme="NETheme01" id="combo_branch_selector" clientinstancename="combo_branch_selector" caption="Active Branch" datasourceid="sds_branches" autopostback="false" textfield="text" valuefield="id" valuetype="System.Int32">
											<clientsideevents selectedindexchanged="approval.change_branch" />
										</dx:aspxcombobox>
									</td>
									<td>
										<dx:aspxcheckbox id="cb_showall" runat="server" clientvisible="false" clientinstancename="cb_showall" text="Show All?" autopostback="false">
											<clientsideevents checkedchanged="function(s,e){if(!s.GetChecked()){combo_branch_selector.SetSelectedIndex(0);}approval.change_branch(s,e);}" />
										</dx:aspxcheckbox>
									</td>
								</tr>
							</table>
							
							
							<asp:sqldatasource id="sds_branches" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" selectcommand="CALL DS_PAYROLL_APPROVAL_BRANCHES(@member_id, @start_date, @end_date, @override)">
								<selectparameters>
									<asp:parameter name="@member_id" />
									<asp:parameter name="@start_date" />
									<asp:parameter name="@end_date" />
									<asp:parameter name="@override" defaultvalue="0" />
								</selectparameters>
							</asp:sqldatasource>
						</div>
						<dx:aspxpanel id="panel_payroll_closed" runat="server" width="100%" height="550px" styles-panel-horizontalalign="Center">
							<panelcollection>
								<dx:panelcontent>
									<div id="div_payroll_closed" runat="server" align="center"></div>
								</dx:panelcontent>
							</panelcollection>
						</dx:aspxpanel>
						<dx:aspxpanel id="panel_youredone_inprogress" runat="server" width="100%" visible="false">
							<panelcollection>
								<dx:panelcontent>
									<div align="center">Payroll for this branch is still being processed, but you either don't have any employees in this branch, or you have already submitted your employees.</div>
								</dx:panelcontent>
							</panelcollection>
						</dx:aspxpanel>
						<dx:aspxpanel id="panel_branchdone_inprogress" runat="server" width="100%" visible="false" styles-panel-horizontalalign="Center">
							<panelcollection>
								<dx:panelcontent>
										<div align="center">Payroll for this branch is sent, but payroll is still being processed in other branches.</div>
								</dx:panelcontent>
							</panelcollection>
						</dx:aspxpanel>
						<dx:aspxpagecontrol id="pc" clientinstancename="pc" runat="server" activetabindex="0" width="100%" visible="false" theme="NETheme01" OnCallback="pc_Callback">
							<tabpages>		
								<dx:tabpage name="approval" text="Approval">
									<contentcollection>
										<dx:contentcontrol runat="server">
											<uc:approval id="uc_approval" runat="server" />
										</dx:contentcontrol>
									</contentcollection>
								</dx:tabpage>
								<dx:tabpage name="summary" text="Summary">
									<contentcollection>
										<dx:contentcontrol runat="server">
											<uc:summary id="uc_summary" runat="server" />
										</dx:contentcontrol>
									</contentcollection>
								</dx:tabpage>
							</tabpages>
							<clientsideevents activetabchanged="approval.tab_changed" />
						</dx:aspxpagecontrol>
					</dx:panelcontent>
				</panelcollection>
		</dx:aspxcallbackpanel>
</asp:content>

