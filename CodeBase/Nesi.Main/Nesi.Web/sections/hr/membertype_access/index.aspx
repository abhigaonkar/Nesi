<%@ Page Language="C#" MasterPageFile="~/nonframe.master" AutoEventWireup="true" Inherits="sections_hr_membertype_access_index" Theme="NETheme01" Title="Membertype Access" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<asp:Content ID="header_override" ContentPlaceHolderID="header_placeholder" runat="server">
	<style type="text/css">
		.priv
			{
			color:		green !important;
			}
		.page
			{
			color:		blue !important;
			}
	</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript">

$("document").ready(function()
						{
						Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
						Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
						//Sys.WebForms.PageRequestManager.getInstance()._events._list.endRequest[0]();
						});
function BeginReqHandler(sender, args)
	{
	please_wait("start");
	}
function EndReqHandler(sender, args)
	{
	please_wait("stop");
	if (args != undefined && args.get_error() != undefined)
		{
		var err				= args.get_error().toString().replace(/Sys.+\Exception:/g, "").trim();
		if($(".ErrorLabel").size() > 0)
			{
			$(".ErrorLabel").show().text(err);
			}
		else
			{
			alert(err);
			}
		}	
	}
function node_click(s,e)
	{
	tree.ExpandNode(e.nodeKey);
	}
function item_click(obj, e)
	{
	var id		= $(obj).attr("data-id");
	var key		= $(obj).attr("data-key");
	var type	= $(obj).attr("data-type");
	var text	= $(obj).text();
	popup1.Show();
	$("#if_detail").attr("src", "./if_detail.aspx?type="+type+"&id="+id+"&key="+key);
	e.stopPropagation();
	}
</script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
				<tr>
					<td>
				<table style="font-family:arial;font-size:12px;">
					<tr>
						<td>&nbsp;</td>
						<td>
							<dx:ASPxComboBox ID="ddl_mt" runat="server" AutoPostBack="True" 
								onselectedindexchanged="ddl_user_SelectedIndexChanged" ValueType="System.Int32" 
								ClientVisible="False">
							</dx:ASPxComboBox>
						</td>
					</tr>
					<tr>
						<td>
							<asp:SqlDataSource ID="ds_templates" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								SelectCommand="SELECT id,name FROM pageprivilege_template WHERE type = 3">
							</asp:SqlDataSource>
						</td>
						<td>
							<dx:ASPxButton ID="bt_savetouser" runat="server" onclick="bt_savetouser_Click" 
								Text="Apply Below Changes to Above USER" Visible="False" ClientVisible="False">
								<ClientSideEvents Click="function(s, e) {
	if(!confirm(&quot;Are you sure you want to save? This will overwrite all pages/privileges for this user.&quot;))
		{
		e.processOnServer = false;
		}
}" />
							</dx:ASPxButton>
						</td>
				</table>
				</td>
				</tr>
                <tr>
                    <td align="center">
						<div style="width:250px;"><dx:ASPxLabel ID="lb_notice" runat="server" ForeColor="Green" Font-Bold="true"></dx:ASPxLabel></div>
					</td>
				</tr>
                <tr>
                    <td align="center">
						<div style="width:735px;text-align:center;">
							<dx:ASPxLabel ID="lb_error" runat="server" ForeColor="Red" Font-Bold="true"></dx:ASPxLabel></div>
                    </td>
                </tr>
				<tr>
					<td align="left">
						<table ID="table_templatemanagement" runat="server" cellpadding="5" 
							cellspacing="0" style="border:solid 1px #000;width:725px;" visible="False">
							<tr>
								<td align="center" colspan="6" 
									style="background-color:#000;color:#fff;font-size:10px;padding:2px;">
									<b>Template Admin</b></td>
							</tr>
							<tr>
								<td style="background-color:#cfc">
									<dx:ASPxComboBox ID="pagepriv_templates" runat="server" AutoPostBack="True" 
										ClientInstanceName="pagepriv_templates" DataSourceID="ds_templates" 
										DropDownStyle="DropDown" 
										onselectedindexchanged="pagepriv_templates_SelectedIndexChanged" 
										TextField="name" ValueField="id" ValueType="System.Int32">
										<ClientSideEvents KeyPress="function(s, e) {
}" KeyUp="function(s, e) {
	if(s.GetValue() == null || s.GetValue() == &quot;&quot;)
		{
		s.SetText(&quot;&quot;);
		}
	else
		{
		alert(s.GetValue());
		}
}" />
									</dx:ASPxComboBox>
								</td>
								<td ID="td_savetemplate" runat="server"  
									style="background-color:#cfc">
									<dx:ASPxButton ID="bt_savetemplate" runat="server" 
										onclick="bt_savetemplate_Click" Text="Save">
									</dx:ASPxButton>
								</td>
								<td ID="td_resettemplate" runat="server" style="background-color:#cfc">
									<dx:ASPxButton ID="bt_resettemplate" runat="server" 
										onclick="bt_resettemplate_Click" Text="Reset">
									</dx:ASPxButton>
								</td>
								<td ID="td_deletetemplate" runat="server" style="background-color:#cfc">
									<dx:ASPxButton ID="bt_deletetemplate" runat="server" 
										onclick="bt_deletetemplate_Click" Text="Delete">
										<ClientSideEvents Click="function(s, e) {
	if(pagepriv_templates.GetValue() == null)
		{
		alert(&quot;Please select a template to delete&quot;);
		e.processOnServer = false;
		}
	else if(!confirm(&quot;Are you sure you want to delete this template?&quot;))
		{
		e.processOnServer = false;
		}
}" />
									</dx:ASPxButton>
								</td>
								<td ID="td_newtemplatename" runat="server" style="background-color:#aca">
									<dx:ASPxTextBox ID="tb_newtemplate" runat="server" NullText="New Template Name">
									</dx:ASPxTextBox>
								</td>
								<td ID="td_newtemplatebutton" runat="server" align="right" 
									style="background-color:#aca">
									<dx:ASPxButton ID="bt_savenewtemplate" runat="server" 
										onclick="bt_savenewtemplate_Click" Text="Save as New Template">
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
					</td>
				</tr>
                <tr id="template_row" runat="server">
                    <td align="right">
						&nbsp;</td>
                </tr>
				<tr>
					<td>
                        <dx:aspxtreelist id="tree" runat="server" autogeneratecolumns="False" 
							width="100%" ClientInstanceName="tree" EnableCallbacks="False" 
							onselectionchanged="tree_SelectionChanged" Font-Names="Arial" Font-Size="9pt">
							<Settings SuppressOuterGridLines="True" />
<SettingsBehavior ProcessFocusedNodeChangedOnServer="True" FocusNodeOnExpandButtonClick="False" AutoExpandAllNodes="False" ColumnResizeMode="Control"></SettingsBehavior>
<Columns>
    <dx:TreeListTextColumn Caption="Page / Privilege" FieldName="Page" Name="Page" 
		VisibleIndex="0" Width="200px">
        <PropertiesTextEdit EncodeHtml="False">
            <Style Font-Bold="False"></Style>
        </PropertiesTextEdit>
        <HeaderStyle Font-Bold="True" />
        <HeaderCaptionTemplate>
            <span style="color: blue"><strong>Page / <span style="color: green">Privilege</span></strong></span>
        </HeaderCaptionTemplate>
        <CellStyle Font-Bold="True">
        </CellStyle>
    </dx:TreeListTextColumn>
    <dx:TreeListTextColumn Caption="Description" FieldName="Description" Name="Description"
        VisibleIndex="3" Width="85%">
        <PropertiesTextEdit EncodeHtml="False">
        </PropertiesTextEdit>
        <HeaderStyle Font-Bold="True" />
    </dx:TreeListTextColumn>
</Columns>
                            <SettingsSelection AllowSelectAll="True" Enabled="True" />
                            <Styles>
                                <AlternatingNode BackColor="WhiteSmoke">
                                </AlternatingNode>
								<SelectedNode BackColor="#CCCCCC" ForeColor="Black">
								</SelectedNode>
                            </Styles>
                            
							<ClientSideEvents NodeClick="node_click" />
                            
</dx:aspxtreelist>
                    	<br />
                    </td>
                </tr>
            </table>
    <dx:ASPxPopupControl id="popup1" runat="server" 
				ClientInstanceName="popup1" HeaderText="" Modal="True" 
				PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
				ShowPageScrollbarWhenModal="True" Top="150" MinWidth="850px" 
				onwindowcallback="popup1_WindowCallback" RenderIFrameForPopupElements="False" 
				PopupAnimationType="None" CloseAction="CloseButton">
        <contentcollection>
<dx:PopupControlContentControl runat="server">
	<iframe id="if_detail" width="100%" height="650" frameborder="0"></iframe>
</dx:PopupControlContentControl>
</contentcollection>
        <ModalBackgroundStyle>
        </ModalBackgroundStyle>
        <HeaderStyle Font-Bold="True" Font-Size="14pt" />
    </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


