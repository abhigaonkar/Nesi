<%@ Page Language="C#" AutoEventWireup="true" Theme="BlueStyle" Inherits="sections_hr_i_index" Codebehind="index.aspx.cs" %>

<%@ Register src="modules/employee_information.ascx" tagname="employee_information" tagprefix="uc" %>
<%@ Register src="modules/template.ascx" tagname="template" tagprefix="uc" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Employees</title>
	<link rel="stylesheet" href="/App_Themes/mobile/Default.css" runat="server" id="mobile_css" />
	<link rel="stylesheet" href="/css/fvr_client.css" />
</head>
<body>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" src="/js/fvr.js"></script>
	<asp:placeholder id="mobile_header_tags" runat="server" visible="false">
	<meta content="True" name="HandheldFriendly" />
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=10, minimum-scale=1 user-scalable=1" />
	<meta name="theme-color" content="#4682B4">
	</asp:placeholder>
	<script type="text/javascript">
		$("document").ready(function()
								{
								Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
								});
		function BeginReqHandler()
			{
			$(".update_progress").css(	{
										"height"			: $(".wiz").height()+"px", 
										"z-index"			: $.maxZ(), 
										"width"				: $(".wiz").width()+25+"px",
										"position"			: "absolute",
										"top"				: $(".wiz").offset().top+"px",
										"left"				: $(".wiz").offset().left+"px"
										});
			}
		function thisRefresh()
			{
			window.location.href = window.location.href;
			}
	</script>
    <form id="form1" runat="server">
	<div class="wrapper" style="min-height:100%;margin: 0;">
	<asp:ScriptManager ID="sm" EnablePageMethods="true" runat="server">
	</asp:ScriptManager>
	<asp:UpdateProgress ID="prog" AssociatedUpdatePanelID="update" runat="server">
		<ProgressTemplate>
			<div id="Layer1" style="position:fixed; left: 0px; top: 0px; width: 100%; padding-top: 200px; text-align: center;" class="update_progress">
				<center>
				<div align="center" style="border:solid 2px #ddd;width:150px;background-color:#fff;padding:10px;border-radius:10px;">
					<div>Loading</div>
					<img id="Img1" src="/images/loading_panel.gif" alt="progressing" />
				</div>
				</center>
			</div>
		</ProgressTemplate>
	</asp:UpdateProgress>
    	<asp:UpdatePanel ID="update" runat="server" class="up">
			<ContentTemplate>
				<asp:Button ID="btnrefresh" runat="server" style="display:none;" />
				<div align="center" style="width:100%;height:100%;">
					<div id="lb_as_user" style="font-size:16px;color:#999;" runat="server"></div>
					<div id="new_video" runat="server" visible="false" style="margin-top:25px">
							
					</div>
					<div id="wiz_new" runat="server" Visible="false" class="wiz">
						<div class="column menu">
							<div id="wiz_new_categories" runat="server" class="sidesteps"></div>
							<br/>
							<asp:Panel runat="server" ID="new_pnl_upload_desktop" Font-Names="Arial" Font-Size="12px" Visible="False" Height="400">
								<iframe runat="server" id="new_if_upload_desktop" name="if_upload" width="100%" height="500" frameborder="0"></iframe>
							</asp:Panel>
							<asp:Panel runat="server" ID="new_pnl_showfile_desktop" Visible="false" Font-Names="Arial" Font-Size="12px">
								<div id="new_pnl_showfile_html_desktop" runat="server"></div>
							</asp:Panel>
						</div>
						<div class="column body">
							<div class="header" runat="server" id="NEWHIRE_header"></div>
							<uc:template ID="NEWHIRE_welcome" Visible="false" template_type="NEW" template_subtype="welcome" runat="server" />
							<uc:employee_information ID="NEWHIRE_information" Visible="false" template_type="NEW" template_subtype="information" runat="server" />
							<uc:template ID="NEWHIRE_handbook" Visible="false" template_type="NEW" template_subtype="handbook" runat="server" />
							<uc:template ID="NEWHIRE_president" Visible="false" template_type="NEW" template_subtype="president" runat="server" />
							<uc:template ID="NEWHIRE_hire_acknowledgement" Visible="false" template_type="NEW" template_subtype="hire_acknowledgement" runat="server" />
							<uc:template ID="NEWHIRE_truck" Visible="false" template_type="NEW" template_subtype="truck" runat="server" />
							<uc:template ID="NEWHIRE_contacts" Visible="false" template_type="NEW" template_subtype="contacts" runat="server" />
							<uc:template ID="NEWHIRE_confidentiality" Visible="false" template_type="NEW" template_subtype="confidentiality" runat="server" />
							<uc:template ID="NEWHIRE_qualifications" Visible="false" template_type="NEW" template_subtype="qualifications" runat="server" />
							<uc:template ID="NEWHIRE_training" Visible="false" template_type="NEW" template_subtype="training" runat="server" />
							<uc:template ID="NEWHIRE_finish" Visible="false" template_type="NEW" template_subtype="finish" runat="server" />
							<asp:Panel runat="server" ID="new_pnl_upload_mobile" Font-Names="Arial" Font-Size="12px" Visible="false" Height="400">
								<br/><br/>
								<iframe runat="server" id="new_if_upload_mobile" name="if_upload" width="100%" height="500" frameborder="0"></iframe>
							</asp:Panel>
							<asp:Panel runat="server" ID="new_pnl_showfile_mobile" Visible="false" Font-Names="Arial" Font-Size="12px">
								<br/><br/>
								<div id="new_pnl_showfile_html_mobile" runat="server"></div>
							</asp:Panel>
						</div>
					</div>
					<div id="wiz_renew" runat="server" Visible="false" class="wiz">
							<div class="column menu">
								<div id="wiz_renew_categories" runat="server" class="sidesteps"></div>
								<br/>
								<asp:Panel runat="server" ID="renew_pnl_upload_desktop" Font-Names="Arial" Font-Size="12px" Visible="False" Height="400">
									<iframe runat="server" id="renew_if_upload_desktop" name="if_upload" width="100%" height="500" frameborder="0"></iframe>
								</asp:Panel>
								<asp:Panel runat="server" ID="renew_pnl_showfile_desktop" Visible="false" Font-Names="Arial" Font-Size="12px">
									<div id="renew_pnl_showfile_html_desktop" runat="server"></div>
								</asp:Panel>
							</div>
							<div class="column body">
								<div runat="server" class="header" id="RENEW_header"></div>
								<uc:template ID="RENEW_welcome" Visible="false" template_type="RENEW" template_subtype="welcome" runat="server" />
								<uc:employee_information ID="RENEW_information" Visible="false" template_type="RENEW" template_subtype="information" runat="server" />
								<uc:template ID="RENEW_emp_handbook" Visible="false" template_type="RENEW" template_subtype="emp_handbook" runat="server" />
								<uc:template ID="RENEW_survey" Visible="false" template_type="RENEW" template_subtype="survey" runat="server" />
								<uc:template ID="RENEW_message_from_president" Visible="false" template_type="RENEW" template_subtype="president" runat="server" />
								<uc:template ID="RENEW_truck" Visible="false" template_type="RENEW" template_subtype="truck" runat="server" />
								<uc:template ID="RENEW_contact_list" Visible="false" template_type="RENEW" template_subtype="contact_list" runat="server" />
								<uc:template ID="RENEW_confidentiality_agreement" Visible="false" template_type="RENEW" template_subtype="confidentiality" runat="server" />
								<uc:template ID="RENEW_qualifications_stmt" Visible="false" template_type="RENEW" template_subtype="qualifications" runat="server" />
								<uc:template ID="RENEW_trademarks" Visible="false" template_type="RENEW" template_subtype="trademarks" runat="server" />
								<uc:template ID="RENEW_code" Visible="false" template_type="RENEW" template_subtype="code" runat="server" />
								<uc:template ID="RENEW_safety" Visible="false" template_type="RENEW" template_subtype="safety" runat="server" />
								<uc:template ID="RENEW_training" Visible="false" template_type="RENEW" template_subtype="training" runat="server" />
								<uc:template ID="RENEW_finish" Visible="false" template_type="RENEW" template_subtype="finish" runat="server" />
								
							<asp:Panel runat="server" ID="renew_pnl_upload_mobile" Font-Names="Arial" Font-Size="12px" Visible="false" Height="400">
								<br/><br/>
								<iframe runat="server" id="renew_if_upload_mobile" name="if_upload" width="100%" height="500" frameborder="0"></iframe>
							</asp:Panel>
							<asp:Panel runat="server" ID="renew_pnl_showfile_mobile" Visible="false" Font-Names="Arial" Font-Size="12px">
								<br/><br/>
								<div id="renew_pnl_showfile_html_mobile" runat="server"></div>
							</asp:Panel>
						</div>
					</div>	
					<asp:Button id="bt_refresh" onclick="bt_refresh_click" runat="server" style="display:none" Text="Refresh"/>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
		</div>
    </form>
</body>
</html>
