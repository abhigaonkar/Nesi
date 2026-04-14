<%@ Page Title="" Language="C#" Theme="" AutoEventWireup="true" Inherits="dashboard_modules_sales_pipline" Codebehind="sales_pipeline.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
	<link type="text/css" href="/css/dashboard.css" rel="Stylesheet" />
	<link type="text/css" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/themes/base/jquery-ui.css" rel="Stylesheet">
	<style type="text/css">
		.style1
		{
			font-family: Arial, Helvetica, sans-serif;
			font-size: x-small;
		}
	</style>
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
				<div class="tile clickable" id="bm_tile_salespipeline" runat="server">
					<div class='title' id="bm_salespipline_title" runat="server">Sales Pipeline</div>
					<div class='content' id="bm_salespipline_content" runat="server" align="center">
						<table cellpadding="0" cellspacing="0" width="100%" data-level="salespipeline">
							<tr>
								<td class='subtitle' style='color:#000;'>Based on Quote % Chance, Within the Next Month<br /><br /></td>
							</tr>
							<tr>
								<td class='subtitle'>90%+</td>
							</tr>
							<tr>
								<td class='gauge' data-type="0" data-level="90plus">
									<div ID="gc_90plus" class="val click" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>
							<tr>
								<td class='subtitle'>60% - 90%</td>
							</tr>
							<tr>
								<td class='gauge' data-type="0" data-level="60to90">
									<div ID="gc_60to90" class="val click" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>

							<tr>
								<td class='subtitle'>30% - 60%</td>
							</tr>
							<tr>
								<td class='gauge' data-type="0" data-level="30to60">
									<div ID="gc_30to60" class="val n" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>

							<tr>
								<td class='subtitle'>WO $ This Month</td>
							</tr>
							<tr>
								<td class='gauge' data-type="0" data-level="WOThisMonth">
									<div ID="gc_wos_thismonth" class="val n" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>

							<tr>
								<td class='subtitle'>WO $ Next Month</td>
							</tr>
							<tr>
								<td class='gauge' data-type="0" data-level="WONextMonth">
									<div ID="gc_wos_nextmonth" class="val n" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>

							<tr>
								<td class='subtitle'>WO $ Following Month</td>
							</tr>
							<tr>
								<td class='gauge' data-type="0" data-level="WOFollowingMonth">
									<div ID="gc_wos_followingmonth" class="val n" runat="server" data-is_customizable="true"></div>
								</td>
							</tr>
						</table>
					</div>
				</div>
    </form>
</body>
</html>