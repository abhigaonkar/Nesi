<%@ Page Language="C#" AutoEventWireup="true" Theme="" Inherits="dashboard_modules_margin" Codebehind="margin.aspx.cs" %>
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
				<div class="tile" id="bm_tile_margin" runat="server">
					<div class='title' id="bm_margin_title" runat="server">Active WO Margins</div>
					<div class='content' id="bm_margin_content" runat="server" align="center">
						<table cellpadding="0" cellspacing="0" width="100%" data-level="margin">
							<tr>
								<td class='subtitle'>Total</td>
							</tr>
							<tr>
								<td class='gauge' data-type="0" data-level="margin_total">
									<div ID="gc_total_margin" class="val" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>
							<tr>
								<td class='subtitle'>T&amp;M</td>
							</tr>
							<tr>
								<td class='gauge' data-type="0" data-level="margin_tm">
									<div ID="gc_total_TM" class="val" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>
							<tr>
								<td class='subtitle'>Quoted</td>
							</tr>
							<tr>
								<td class='gauge' data-type="0" data-level="margin_quoted">
									<div ID="gc_total_quoted" class="val n" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>
						</table>
					</div>
				</div>
    </form>
</body>
</html>
