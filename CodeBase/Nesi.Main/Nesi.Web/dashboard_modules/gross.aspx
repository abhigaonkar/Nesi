<%@ Page Language="C#" AutoEventWireup="true" Theme="" Inherits="dashboard_modules_gross" Codebehind="gross.aspx.cs" %>
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
				<div class="tile" id="bm_tile_gross" runat="server">
					<div class='title' id="bm_gross_title" runat="server">Gross Margin</div>
					<div class='content' id="bm_gross_content" runat="server" align="center">
						<table cellpadding="0" cellspacing="0" width="100%" data-level="gross">
							<tr>
								<td class='subtitle'>Month to Date</td>
							</tr>
							<tr>
								<td class='gauge' data-clickable="false" data-type="0" data-level="margin_mtd">
									<div ID="gc_gross_mtd" class="val" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>
							<tr>
								<td class='subtitle'>Fiscal Year to Date</td>
							</tr>
							<tr>
								<td class='gauge' data-clickable="false" data-type="0" data-level="margin_fytd">
									<div ID="gc_gross_fytd" class="val" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>
						</table>
					</div>
				</div>
    </form>
</body>
</html>
