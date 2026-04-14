<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_admin_WO_unlock_Index" Codebehind="Index.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
		button					{
								width:			350px;
								font-family:	Arial;
								border:			solid 1px #000; 
								margin:			5px;
								cursor:			pointer;
								}
		#server_response		{
								font-size:		20px;
								color:			#f00;
								}
    </style>
</head>
<body>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
    <script type="text/javascript">
	function remove_lock(obj)
		{
		var lock_id			= $(obj).attr('data-LOCKID');
		var DSN				= $(obj).attr('data-DSN');
		if(confirm('Are you sure you want to remove this lock?'))
			{
			location.href	= './index.aspx?ID='+lock_id+'&DSN='+DSN;
			}
		}
    </script>
    <div id="current_DSN" runat="server"></div>
    <div id="server_response" runat="server"></div>
    <div id="company_selection" runat="server"></div>
    <div id="locks" runat="server"></div>
</body>
</html>
