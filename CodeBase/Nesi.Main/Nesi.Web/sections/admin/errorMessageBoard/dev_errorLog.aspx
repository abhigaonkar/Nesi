<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_admin_errorMessageBoard_dev_errorLog" 
   EnableTheming="true" Theme ="DevDashboard01"  Codebehind="dev_errorLog.aspx.cs" %>

<%@ Register src="../../../modules/crash_log.ascx" tagname="crash_log" tagprefix="uc2" %>
<%@ Register Src="~/modules/auto_bingo.ascx" TagPrefix="uc2" TagName="auto_bingo" %>
<%@ Register Src="~/modules/slow_page.ascx" TagPrefix="uc2" TagName="slow_page" %>





<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MessageBoard</title>

</head>
<body>
    <form id="form1" runat="server" >

        

        <div style="padding: 7px;" >
            <uc2:crash_log ID="crash_log2" runat="server" />
        </div>

    </form>
     
</body>
</html>
