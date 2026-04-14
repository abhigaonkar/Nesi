<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_workorder_modules_analysis_test" Codebehind="analysis_test.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/sections/workorder/modules/analysis.ascx" TagPrefix="uc1" TagName="analysis" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript">
		$("document").ready(function () {
			page_obj.update_panel_progress.bind();
		}
		);
	</script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding:20px;">
			<asp:ScriptManager runat="server" ID="sm">
			</asp:ScriptManager>
			<asp:UpdatePanel runat="server" ID="up">
			<ContentTemplate>
			<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" Width="100%" ActiveTabIndex="0">
				<TabPages>
					<dx:TabPage Text="Quoted">
						<ContentCollection>
							<dx:ContentControl runat="server">
								<uc1:analysis runat="server" ID="analysisq" />
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="T&amp;M">
						<ContentCollection>
							<dx:ContentControl runat="server">
								<uc1:analysis runat="server" ID="analysist" />
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
				</TabPages>

			</dx:ASPxPageControl>
			</ContentTemplate>
			</asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
