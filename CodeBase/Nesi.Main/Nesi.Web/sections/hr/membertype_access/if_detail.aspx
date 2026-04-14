<%@ Page Language="C#" MasterPageFile="~/nonframe.master" AutoEventWireup="true" Inherits="sections_hr_membertype_access_if_detail" Title="Membertype Access Detail" Codebehind="if_detail.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<asp:Content ID="header_override" ContentPlaceHolderID="header_placeholder" runat="server">
	<style type="text/css">
		.priv
			{
			color:		green;
			}
		.page
			{
			color:		blue;
			}
		.style1
		{
			font-family: Arial, Helvetica, sans-serif;
			font-size: small;
		}
		.style2
		{
			font-family: Arial, Helvetica, sans-serif;
			font-size: x-small;
		}
		.style3
		{
			font-family: Arial, Helvetica, sans-serif;
		}
	</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
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
	var key		= $(obj).attr("data-key");
	var text	= $(obj).text();
	popup1.PerformWindowCallback(popup1.GetWindow(0), key+"|"+text);
	popup1.Show();
	e.stopPropagation();
	}
</script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" class="style2">
							<tr>
								<td colspan="3" style="height: 35px" valign="top">
									<dx:ASPxLabel ID="lbltitle" runat="server" Text="ASPxLabel" Width="450px" 
										Font-Bold="True" Font-Size="15px" CssClass="style3" Font-Names="Arial">
									</dx:ASPxLabel>
									<br />
									&nbsp;&nbsp;
									<dx:ASPxLabel ID="lbldescription" runat="server" Text="ASPxLabel" Width="450px" 
										Font-Names="Arial">
									</dx:ASPxLabel>
								</td>
							</tr>
							<tr>
								<td style="white-space:nowrap" valign="top" width="47%">
									&nbsp;<table>
										<tr>
											<td style="width: 100px">
												<dx:ASPxCheckBox ID="chkEnabled" runat="server" CheckState="Unchecked" 
													ClientInstanceName="chkEnabled" Text="Enabled" ValueChecked="1" 
													ValueType="System.Int32" ValueUnchecked="0" Font-Names="Arial">
													<ClientSideEvents CheckedChanged="function(s, e) {
	btnApply.SetEnabled(1);
}" />
												</dx:ASPxCheckBox>
											</td>
											<td style="width: 100px">
												<dx:ASPxButton ID="btnApply" runat="server" ClientEnabled="False" ClientInstanceName="btnApply" OnClick="btnApply_Click" Text="Apply">
												</dx:ASPxButton>
											</td>
										</tr>
									</table>
								</td>
								
								<td style="white-space:nowrap" valign="top" width="0%">
									&nbsp;</td>
								
								<td width="47%">
									&nbsp;</td>
							</tr>
							<tr>
								<td>
									<dx:ASPxCheckBox ID="ck_all_active" runat="server" CheckState="Unchecked" 
										ClientInstanceName="ck_all_active" Text="Toggle all active member types" 
										Font-Names="Arial">
										<ClientSideEvents CheckedChanged="function(s, e) {
	tree_memlist.PerformCallback(s.GetChecked());
}" />
									</dx:ASPxCheckBox>
								</td>
								<td width="0%">
									&nbsp;</td>
								<td>
									&nbsp;</td>
							</tr>
							<tr>
								<td class="style1">
									Available Membertype List </td>
								<td class="style1" width="0%">
									&nbsp;</td>
								<td class="style1">
									Enabled for this Page / Privilege</td>
							</tr>
							<tr>
								<td rowspan="2" valign="top">
									<dx:ASPxTreeList ID="tree_memlist" runat="server" AutoGenerateColumns="False" ClientInstanceName="tree_memlist" Font-Names="Arial" Font-Size="8pt" Height="100%" OnCustomCallback="tree_CustomCallback" Width="100%">
										<Columns>
											<dx:TreeListTextColumn Caption="Member Type" FieldName="MemberType_Name" 
												Name="MemberType" ShowInCustomizationForm="True" VisibleIndex="0">
												<PropertiesTextEdit EncodeHtml="False">
												</PropertiesTextEdit>
											</dx:TreeListTextColumn>
										</Columns>
										<SettingsBehavior FocusNodeOnExpandButtonClick="False" />
										<SettingsSelection Enabled="True" />
										<Border BorderColor="#E0E0E0" BorderStyle="Solid" />
									</dx:ASPxTreeList>
								</td>
								
								<td valign="top" align="center" width="0%">
									<dx:ASPxButton ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="&gt;&gt;" ToolTip="Add Selected Members to This Page / Privilege" Width="35px">
									</dx:ASPxButton>
									<br />
									<br />
									<dx:ASPxButton ID="btn_delete" runat="server" OnClick="btn_delete_Click" Text="&lt;&lt;" ToolTip="Remove Selected Members from this Page/Privilege" Width="35px">
									</dx:ASPxButton>
								</td>
								
								<td rowspan="2" valign="top">
									<dx:ASPxTreeList ID="tree_priv_list" runat="server" AutoGenerateColumns="False" ClientInstanceName="tree_priv_list" Font-Names="Arial" Font-Size="8pt" Height="100%" OnCustomCallback="tree_CustomCallback" Width="100%">
										<Columns>
											<dx:TreeListTextColumn Caption="Member Type" FieldName="MemberType_Name" 
												Name="MemberType" VisibleIndex="0">
												<PropertiesTextEdit EncodeHtml="False">
												</PropertiesTextEdit>
											</dx:TreeListTextColumn>
										</Columns>
										<SettingsBehavior FocusNodeOnExpandButtonClick="False"  />
										<SettingsSelection Enabled="True" />
										<Border BorderColor="#E0E0E0" BorderStyle="Solid" />
									</dx:ASPxTreeList>
								</td>
							</tr>
						</table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


