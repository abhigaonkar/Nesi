<%@ Page Language="C#" AutoEventWireup="true" Inherits="pdf" Codebehind="pdf.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        
        <embed src="/images/NoScan.pdf" width="800" height="1500" scale="tofit" runat="server" id="pdfObj" alt="pdf" pluginspage="http://www.adobe.com/products/acrobat/readstep2.html"/>
      
    </form>
</body>
</html>
