<%@ Control Language="C#" AutoEventWireup="true" Inherits="mobile_modules_workorder" EnableTheming="True" Codebehind="workorder.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ register src="~/mobile/modules/workorder_details.ascx" tagprefix="uc" tagname="workorder_details" %>


<meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0' name='viewport' />
<link rel="stylesheet" type="text/css" href="/mobile/css/workorder.css"></link>
<script type="text/javascript" src="/mobile/js/workorder.js"></script>
<script type="text/javascript">
	$(document).ready(function()
		{
		page_obj.update_panel_progress.bind();
		});
</script>
<asp:updatepanel id="up_details" runat="server">
		<contenttemplate>
<dx:aspxcallbackpanel ID="cbp" runat="server" oncallback="cbp_Callback" 
	ClientInstanceName="cbp" Width="100%" Height="100%">
	<PanelCollection>
		<dx:PanelContent>
			<button type="button" id="new_wo" runat="server" class="whitetext aligncenter" onclick="mobile_wo.start_workorder();">
				Start New Work Order
			</button>
			<asp:dropdownlist id="ddl_tab" runat="server" cssclass="ddl_tabs whitetext alignleft" autopostback="true" onselectedindexchanged="ddl_tab_SelectedIndexChanged">
				<asp:listitem value="0" text="All"/>
				<asp:listitem value="1" text="Today's Work Orders"/>
				<asp:listitem value="2" text="Waiting for Me"/>
				<asp:listitem value="3" text="Details" enabled="false"/>
			</asp:dropdownlist>
			<asp:button id="bt_back" runat="server" text="&lt; Back" cssclass="whitetext aligncenter" onclick="bt_back_Click" visible="false"/>
			<asp:multiview id="mv_tabs" runat="server">
				<asp:view id="tab_all" runat="server">
					<dx:aspxgridview id="gv_inprogress" border-borderwidth="0px" runat="server" autogeneratecolumns="False" datasourceid="sds_progress" keyfieldname="woprog_id" width="100%" onautofiltercelleditorinitialize="gv_inprogress_AutoFilterCellEditorInitialize" onhtmlrowcreated="gv_HtmlRowCreated" enablepaginggestures="False">
						<columns>
							<dx:gridviewdatatextcolumn caption="Work Order" fieldname="wo" showincustomizationform="True" visibleindex="1">
								<settings autofiltercondition="Contains" />
								<dataitemtemplate>
									<dx:aspxhyperlink id="hl" runat="server" cssclass="link" navigateurl='<%# string.Format("javascript:cbp.PerformCallback(\"view|{0}\")", Eval("woprog_id")) %>' text='<%# Eval("wo") %>' />
								</dataitemtemplate>
							</dx:gridviewdatatextcolumn>
						</columns>
						<settingspager pagesize="100">
						</settingspager>
						<settings showcolumnheaders="False" showfilterrow="True" />
						<styles>
							<cell border-borderwidth="0px" cssclass="title">
								<border borderwidth="0px" />
							</cell>
							<alternatingrow backcolor="WhiteSmoke"></alternatingrow>
						</styles>
						<clientsideevents begincallback="function(s,e){please_wait('start');}"  endcallback="function(s,e){please_wait('stop');}" />
						<border borderwidth="0px" />
					</dx:aspxgridview>
					<asp:sqldatasource id="sds_progress" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" selectcommand="
SELECT 
	woprog_id,
	CONCAT(trim(leading '0' from woprog_bvwo),' [',woprog_status, '] - ', woprog_customername, ' - ', woprog_description) wo 
FROM 
	woprog 
WHERE 
	woprog_id &gt; 20000 AND 
	business_unit_id = @business_unit_id and 
	woprog_status NOT IN ('Waiting to be Invoiced', 'Invoiced', 'Deleted') 
ORDER BY 
	woprog_id DESC">
						<selectparameters>
							<asp:parameter name="@business_unit_id" defaultvalue="0" />
						</selectparameters>
					</asp:sqldatasource>
				</asp:view>
				<asp:view id="tab_todays" runat="server">
					<dx:aspxgridview id="gv_signoff" runat="server" autogeneratecolumns="False" datasourceid="sds_signoff" keyfieldname="woprog_id" width="100%" onautofiltercelleditorinitialize="gv_inprogress_AutoFilterCellEditorInitialize" onhtmlrowcreated="gv_HtmlRowCreated" enablepaginggestures="False" settingstext-emptydatarow="No jobs that require customer signatures today">
						<columns>
							<dx:gridviewdatatextcolumn caption="Work Order" fieldname="wo" showincustomizationform="True" visibleindex="1">
								<settings autofiltercondition="Contains" />
								<dataitemtemplate>
									<dx:aspxhyperlink id="ASPxHyperLink1" cssclass="link" runat="server" navigateurl='<%# string.Format("javascript:cbp.PerformCallback(\"view|{0}\")", Eval("woprog_id")) %>' text='<%# Eval("wo") %>' />
								</dataitemtemplate>
							</dx:gridviewdatatextcolumn>
						</columns>
						<settingspager pagesize="100">
						</settingspager>
						<settings showcolumnheaders="False" showfilterrow="True" />

						<settingstext emptydatarow="No jobs that require customer signatures today"></settingstext>
						<styles>
							<cell border-borderwidth="0px" cssclass="title">
								<border borderwidth="0px" />
							</cell>
							<alternatingrow backcolor="WhiteSmoke"></alternatingrow>
						</styles>
					</dx:aspxgridview>
					<asp:sqldatasource id="sds_signoff" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" selectcommand="
SELECT 
	a.woprog_id,
	CONCAT(trim(leading '0' from b.woprog_bvwo),' [',b.woprog_status, '] - ', b.woprog_customername, ' - ', b.woprog_description) wo 
FROM 
	appointments a
LEFT JOIN 
	woprog b ON
		a.woprog_id = b.woprog_id 
WHERE 
	b.woprog_id &gt; 20000 AND 
	b.woprog_status NOT IN ('Waiting to be Invoiced', 'Invoiced', 'Deleted', 'Initial Prep','Open Vendor POs') AND
	
	DATE(a.startdate) = CURDATE() AND 
	a.member_id = @member_id
GROUP BY
	a.woprog_id 
ORDER BY 
	a.woprog_id DESC">
						<selectparameters>
							<asp:parameter name="@business_unit_id" defaultvalue="0" />
							<asp:parameter name="@is_bm" defaultvalue="0" />
							<asp:parameter name="@member_id" defaultvalue="0" />
						</selectparameters>
					</asp:sqldatasource>
				</asp:view>
				<asp:view id="tab_waiting" runat="server">
					<dx:aspxgridview id="gv_toapprove" runat="server" autogeneratecolumns="False" datasourceid="sds_toapprove" keyfieldname="woprog_id" width="100%" onautofiltercelleditorinitialize="gv_inprogress_AutoFilterCellEditorInitialize" onhtmlrowcreated="gv_HtmlRowCreated" enablepaginggestures="False" settingstext-emptydatarow="No works orders to approve">
						<columns>
							<dx:gridviewdatatextcolumn caption="Work Order" fieldname="wo" showincustomizationform="True" visibleindex="1">
								<settings autofiltercondition="Contains" />
								<dataitemtemplate>
									<dx:aspxhyperlink id="hl" runat="server" cssclass="link" navigateurl='<%# string.Format("javascript:cbp.PerformCallback(\"view|{0}\")", Eval("woprog_id")) %>' text='<%# Eval("wo") %>' />
								</dataitemtemplate>
							</dx:gridviewdatatextcolumn>
						</columns>
						<settingspager pagesize="100">
						</settingspager>
						<settings showcolumnheaders="False" showfilterrow="True" />

						<settingstext emptydatarow="No work orders to approve"></settingstext>
						<styles>
							<cell border-borderwidth="0px" cssclass="title">
								<border borderwidth="0px" />
							</cell>
							<alternatingrow backcolor="WhiteSmoke"></alternatingrow>
						</styles>
					</dx:aspxgridview>
					<asp:sqldatasource id="sds_toapprove" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" selectcommand="
SELECT 
	woprog_id,
	CONCAT(trim(leading '0' from woprog_bvwo),' [',woprog_status, '] - ', woprog_customername, ' - ', woprog_description) wo,
	woprog_status status
FROM 
	woprog 
WHERE 
	woprog_id &gt; 20000 AND 
	business_unit_id = @business_unit_id and 
	woprog_status IN ('Waiting PM Approval','Waiting BM Approval') AND 
	IF(@is_bm = 1, true, woprog_pm_memberid = @member_id)
ORDER BY 
	woprog_status, woprog_id DESC">
						<selectparameters>
							<asp:parameter name="@business_unit_id" defaultvalue="0" />
							<asp:parameter name="@is_bm" defaultvalue="0" />
							<asp:parameter name="@member_id" defaultvalue="0" />
						</selectparameters>
					</asp:sqldatasource>
				</asp:view>
				<asp:view id="tab_details" runat="server">
					<uc:workorder_details runat="server" id="uc_workorder_details" />
				</asp:view>
			</asp:multiview>
				<dx:ASPxPopupControl ID="popup_reworks" runat="server" ClientInstanceName="popup_reworks" Width="360px" Height="1024px" AutoUpdatePosition="True" CloseAction="CloseButton" ShowHeader="false" ShowShadow="false" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" HeaderText="Rework Reason" >
					<ContentCollection>
						<dx:PopupControlContentControl runat="server">
							<center>
								Please enter a reason why you are sending this work order to reworks.<br /><br />
								<dx:ASPxMemo ID="memo_rework_reason" ClientInstanceName="memo_rework_reason" runat="server" Height="250px" Width="100%">
								</dx:ASPxMemo><br />
								<dx:ASPxButton ID="bt_reworksubmit_popup" AutoPostBack="false" Width="100%" Font-Size="2.25em" runat="server" Text="Send">
									<ClientSideEvents Click="mobile_wo.reworks.send" />
								</dx:ASPxButton><br /><br />
								<dx:ASPxButton ID="bt_reworkcancel_popup" AutoPostBack="false" Width="100%" Font-Size="2.25em" runat="server" Text="Cancel">
									<ClientSideEvents Click="mobile_wo.reworks.hide" />
								</dx:ASPxButton>
							</center>
						</dx:PopupControlContentControl>
					</ContentCollection>
					<Border BorderWidth="0px" />
				</dx:ASPxPopupControl>
		</dx:PanelContent>
	</PanelCollection>
	<ClientSideEvents BeginCallback="mobile_wo.cbp.begin" EndCallback="mobile_wo.cbp.end" CallbackError="mobile_wo.cbp.error" />
</dx:aspxcallbackpanel>
												</contenttemplate>
										</asp:updatepanel>
