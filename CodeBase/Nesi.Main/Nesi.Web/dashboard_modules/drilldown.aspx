<%@ Page Language="C#" AutoEventWireup="true" Theme="" Inherits="dashboard_modules_drilldown_revenue" Codebehind="drilldown.aspx.cs" %>
<%@ OutputCache Duration="60" VaryByParam="business_unit_id" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>





<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<link type="text/css" href="/css/dashboard.css" rel="Stylesheet" />
    <title></title>
</head>
<body>
	<script type='text/javascript' src='/js/jquery-1.3.2.min.js'></script>
	<script type="text/javascript" src="/js/dashboard.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
    <form id="form1" runat="server">
    <div>
		<dx:ASPxCallbackPanel ID="cbp_drilldown" runat="server" ClientInstanceName="drilldown" CssClass="drilldown" Width="100%" oncallback="drilldown_Callback">
			<PanelCollection>
				<dx:PanelContent ID="cbp_drilldown_content" runat="server" SupportsDisabledAttribute="True">

					<dx:ASPxPanel ID="p_rev_mtd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Revenue Month to Date</div>
								<div class='detail' id="rev_mtd" runat="server">
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_rev_fytd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Revenue Fiscal Year to Date</div>
								<div class='detail' id="rev_fytd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_rev_btm" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Revenue Budget this Month</div>
								<div class='detail' id="rev_btm" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_rev_ftm" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Revenue Fiscal this Month</div>
								<div class='detail' id="rev_ftm" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_lab_mtd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Labor Month to Date</div>
								<div class='detail' id="lab_mtd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_lab_fytd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Labor Fiscal Year to Date</div>
								<div class='detail' id="lab_fytd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_lab_btm" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Labor Budget this Month</div>
								<div class='detail' id="lab_btm" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_lab_ftm" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent7" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Labor Fiscal this Month</div>
								<div class='detail' id="lab_ftm" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_mat_mtd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent8" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Material Month to Date</div>
								<div class='detail' id="mat_mtd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_mat_fytd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent9" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Material Fiscal Year to Date</div>
								<div class='detail' id="mat_fytd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_mat_btm" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent10" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Material Budget this Month</div>
								<div class='detail' id="mat_btm" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_mat_ftm" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Material Fiscal this Month</div>
								<div class='detail' id="mat_ftm" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_margin_mtd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent12" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Gross Margin Month to Date</div>
								<div class='detail' id="margin_mtd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_margin_fytd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent13" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Fiscal Year to Date</div>
								<div class='detail' id="margin_fytd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_inc_mtd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent14" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Income Month to Date</div>
								<div class='detail' id="inc_mtd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_inc_fytd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent15" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Income Fiscal Year to Date</div>
								<div class='detail' id="inc_fytd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_inc_btm" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent16" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Income Budget this Month</div>
								<div class='detail' id="inc_btm" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_inc_ftm" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent17" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Income Fiscal This Month</div>
								<div class='detail' id="inc_ftm" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_cust_fytd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent18" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Customer Fiscal Year to Date</div>
								<div class='detail' id="cust_fytd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_cust_bytd" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent19" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Customer Budget Year to Date</div>
								<div class='detail' id="cust_bytd" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_worstcustomers" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent20" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Worst Customers</div>
								<div class='detail' id="worstcustomers" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_90plus" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent21" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Quotes with 90%+ Chance</div>
								<div class='detail' id="d90plus" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_60to90" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent22" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Quotes with 60 to 90% Chance</div>
								<div class='detail' id="d60to90" runat="server">
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_30to60" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent23" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Quotes with 60 to 60% Chance</div>
								<div class='detail' id="d30to60" runat="server">
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_WOThisMonth" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent25" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Work Orders This Month</div>
								<div class='detail' id="dWOThisMonth" runat="server">
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_WONextMonth" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent26" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Work Orders Next Month</div>
								<div class='detail' id="dWONextMonth" runat="server">
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_WOFollowingMonth" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent27" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Work Orders Following Month</div>
								<div class='detail' id="dWOFollowingMonth" runat="server">
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="p_inventory" runat="server" ClientVisible="False" CssClass="panel">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent24" runat="server" SupportsDisabledAttribute="True">
								<div class='title'>Inventory Details</div>
								<div class='detail' id="dinventory" runat="server">
								
								</div>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
				</dx:PanelContent>
			</PanelCollection>
		</dx:ASPxCallbackPanel>
    </div>
    </form>
</body>
</html>
