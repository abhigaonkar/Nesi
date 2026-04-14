<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_admin_errorMessageBoard_dev_dashboard" 
   EnableTheming="true" Theme ="DevDashboard01"  Codebehind="dev_dashboard.aspx.cs" %>

<%@ Register src="../../../modules/crash_log.ascx" tagname="crash_log" tagprefix="uc2" %>
<%@ Register Src="~/modules/auto_bingo.ascx" TagPrefix="uc2" TagName="auto_bingo" %>
<%@ Register Src="~/modules/slow_page.ascx" TagPrefix="uc2" TagName="slow_page" %>
<%@ Register Src="~/modules/invoiceService_runTimes.ascx" TagPrefix="uc1" TagName="invoiceService_runTimes" %>





<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MessageBoard</title>
     
</head>
<body>
    <form id="form1" runat="server" >
            <div style="padding: 7px;" >
            <div >
                <uc2:slow_page runat="server" ID="slow_page" />
            </div>
            <div >
                <uc1:invoiceService_runTimes runat="server" ID="invoiceService_runTimes" />
            </div>
        </div>
    </form>
     
</body>
</html>
