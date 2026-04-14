<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_workorder_modules_project_schedule" Codebehind="project_schedule.ascx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>Untitled Page</title>
<link rel="stylesheet" type="text/css" href="jsgantt/jsgantt.css"/>
<script language="javascript" src="jsgantt/jsgantt.js"></script>
</head>
<body>
<form id="form1" runat="server">
<div>
<table style="width: 384px">
<tr>
<td style="width: 185px">
</td>
<td style="width: 167px">
</td>
<td style="width: 65px">
</td>
</tr>
<tr>
<td style="width: 185px; height: 24px;">
</td>
<td style="width: 167px; height: 24px;">
<asp:DropDownList ID="cmbproject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbproject_SelectedIndexChanged"
Width="165px">
</asp:DropDownList></td>
<td style="width: 65px; height: 24px;">
</td>
</tr>
<tr>
<td style="width: 185px">
</td>
<td style="width: 167px">
</td>
<td style="width: 65px">
</td>
</tr>
</table>
</div>
<div style="position:relative" class="gantt" id="GanttChartDIV">
</div>
<script>
	var g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');
	g.setShowRes(1); // Show/Hide Responsible (0/1)
	g.setShowDur(0); // Show/Hide Duration (0/1)
	g.setShowComp(1); // Show/Hide % Complete(0/1)
	//g.setCaptionType('Resource'); // Set to Show Caption (None,Caption,Resource,Duration,Complete)
	if (g) {
		// You can also use the XML file parser
		JSGantt.parseXML('Tasks.xml', g)//Read data from Tasks.xml
		g.Draw();
		g.DrawDependencies();
	}
	else {
		alert("not defined");
	}
</script>
</form>
</body>
</html>