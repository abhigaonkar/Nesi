<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_hist_chart" Codebehind="hist_chart.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<html>
<div id="div_graph" runat="server">
 
  </div>
  <body>
      <dx:ASPxHiddenField ID="hdn_data" runat="server">
	  </dx:ASPxHiddenField>
	  <dx:ASPxHiddenField ID="hdn_controls" runat="server">
	  </dx:ASPxHiddenField>
    <div id="chart_div" style="width: 900px; height: 500px;"></div>
  </body>
</html>

      
                
            

      
