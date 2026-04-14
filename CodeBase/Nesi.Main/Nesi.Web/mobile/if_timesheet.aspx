<%@ Page Language="C#" AutoEventWireup="true" Inherits="mobile_if_timesheet" Codebehind="if_timesheet.aspx.cs" %>

<%@ register src="~/mobile/modules/timesheet.ascx" tagprefix="uc" tagname="timesheet" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	
	<meta content="True" name="HandheldFriendly" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=10, minimum-scale=1 user-scalable=1" name="viewport" />
	<meta name="viewport" content="width=device-width" />
	<link href="/css/base/ui.all.css" rel="stylesheet" type="text/css" /> 
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<link type="text/css" media="all" rel="Stylesheet" href="/mobile/css/base.css" />
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" src="/mobile/js/base.js"></script>
	<script type="text/javascript">
	$(document).ready(function()
		{
		page_obj.update_panel_progress.bind();
		});
	</script>
	<style>
		body 
			{
			background: #fff;
			background-color: #fff;
			}
	</style>
    <title>Timesheet</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
	    <asp:scriptmanager id="sm" runat="server"></asp:scriptmanager>
		<uc:timesheet runat="server" id="timesheet" only_wo="true" />
    </div>
    </form>
</body>
</html>
