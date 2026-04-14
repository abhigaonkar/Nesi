<%@ Page Language="C#" AutoEventWireup="true" Theme="" Inherits="dashboard_modules_hourutilization" Codebehind="hourutilization.aspx.cs" %>
<%@ OutputCache Duration="60" VaryByParam="business_unit_id" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link type="text/css" href="/css/dashboard.css" rel="Stylesheet" />
	<link type="text/css" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/themes/base/jquery-ui.css" rel="Stylesheet">
</head>
<body>
	<script type='text/javascript' src='/js/jquery-1.3.2.min.js'></script>
	<script type="text/javascript" src="/js/dashboard.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript">
		$("document").ready(function()
								{
								$("body").find(".val").each(function()
									{
									var is_customizable		= $(this).attr("data-is_customizable") == "true";
									if(is_customizable)
										{
										$(this).hover(function(){customize_toggle(this, true)}, function(){customize_toggle(this, false)});
										}
									});
								});
	</script>
    <form id="form1" runat="server">
				<div class="tile" id="bm_tile_hourutilization" runat="server">
					<div class='title' id="bm_hourutilization_title" runat="server">Hour Utilization</div>
					<div class='content' id="bm_hourutilization_content" runat="server">
						<table cellpadding="0" cellspacing="0" width="100%" data-level="hourutilization">
							<tr>
								<td class='subtitle'>This Month</td>
							</tr>
							<tr>
								<td class='gauge' data-clickable="false" data-type="0" data-level="hour_mtd">
									<div ID="gc_hour_mtd" class="val" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>
							<tr>
								<td class='subtitle'>Fiscal Year to Date</td>
							</tr>
							<tr>
								<td class='gauge' data-clickable="false" data-type="0" data-level="hour_fytd">
									<div ID="gc_hour_fytd" class="val" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>
							<tr>
								<td class='subtitle'>Budget Year to Date</td>
							</tr>
							<tr>
								<td class='gauge' data-clickable="false" data-type="0" data-level="hour_bytd">
									<div ID="gc_hour_bytd" class="val n" runat="server" data-is_customizable="false"></div>
								</td>
							</tr>
						</table>
					</div>
				</div>
    </form>
</body>
</html>
