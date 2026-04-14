<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_admin_errorMessageBoard_dev_autoBingo" 
   EnableTheming="true" Theme ="DevDashboard01"  Codebehind="dev_autoBingo.aspx.cs" %>

<%@ Register Src="~/modules/auto_bingo.ascx" TagPrefix="uc2" TagName="auto_bingo" %>





<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MessageBoard</title>
      <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>

</head>
<body>
<form id="form1" runat="server" >

        

    <div style="padding: 7px;" >
            <uc2:auto_bingo runat="server" ID="auto_bingo1" />
    </div>

</form>
    	
</body>
</html>
