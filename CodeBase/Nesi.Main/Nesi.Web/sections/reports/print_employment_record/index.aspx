<%@ page language="C#" autoeventwireup="true" inherits="print_employment_record" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	namespace="DevExpress.Web" tagprefix="dx" %>



<%@ register assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Untitled Page</title>
</head>
<body>
	<form id="form1" runat="server">
		<div style="width: 99%; " align="center">
	
			<dx:reporttoolbar id="ReportToolbar1" runat="server" reportviewer="<%# ReportViewer1 %>"
				showdefaultbuttons="False" Width="100%">
				<items>
				
					<dx:reporttoolbarbutton itemkind="PrintReport" name="PrintReport" />
					<dx:reporttoolbarbutton itemkind="PrintPage" />
					<dx:reporttoolbarseparator />
					<dx:reporttoolbarbutton enabled="False" itemkind="FirstPage" />
					<dx:reporttoolbarbutton enabled="False" itemkind="PreviousPage" />
					<dx:reporttoolbarlabel itemkind="PageLabel" />
					<dx:reporttoolbarcombobox itemkind="PageNumber" width="65px">
					</dx:reporttoolbarcombobox>
					<dx:reporttoolbarlabel itemkind="OfLabel" />
					<dx:reporttoolbartextbox itemkind="PageCount" />
					<dx:reporttoolbarbutton itemkind="NextPage" />
					<dx:reporttoolbarbutton itemkind="LastPage" />
					<dx:reporttoolbarseparator />
					<dx:reporttoolbarbutton itemkind="SaveToDisk" />
					<dx:reporttoolbarbutton itemkind="SaveToWindow" />
					<dx:reporttoolbarcombobox itemkind="SaveFormat" width="70px">
						<elements>
							<dx:listelement value="pdf" />
						</elements>
					</dx:reporttoolbarcombobox>
				</items>
				<styles>
					<labelstyle>
						<margins marginleft="3px" marginright="3px" />
					</labelstyle>
				</styles>
				
			</dx:reporttoolbar>
			<dx:reportviewer id="ReportViewer1" clientinstancename="ReportViewer1" runat="server" Width="99%">
				<paddings paddingleft="30px" />
				<border bordercolor="Gray" borderstyle="Solid" borderwidth="1px" />
			</dx:reportviewer>
			
		</div>
	</form>
</body>
</html>
