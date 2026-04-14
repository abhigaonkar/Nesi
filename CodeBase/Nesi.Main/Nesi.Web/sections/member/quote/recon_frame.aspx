<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_member_quote_recon_frame" Codebehind="recon_frame.aspx.cs" %>

<%@ Register src="modules/recon.ascx" tagname="recon" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Quote Recon</title>
	<link type="text/css" href="/css/base/ui.all.css" rel="stylesheet" />
</head>
<body>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script language="javascript">
	function noError(){return true;}
window.onerror = noError;
	</script>
    <form id="form1" runat="server">
    <div>
		<uc1:recon ID="recon1" runat="server" />
    
    </div>
    </form>
</body>
</html>
