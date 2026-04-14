<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_employee_access_index" Title="Member Access Report" Codebehind="index.aspx.cs" %>






<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script>
	function bind_tooltips()
			{
			$(".opt1").each(function()
				{
				$(this).tip();
				});
			}
		$(document).ready(function()
			{
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
			});

function EndReqHandler()
	{
	bind_tooltips();
	
	}
	</script>

    &nbsp;
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></td>
                        <td style="color: #000000">
                        </td>
                    </tr>
                    <tr style="color: #000000">
                        <td style="white-space: nowrap">
                            <asp:Label ID="lblCompanyList" runat="server" Text=" Only apply mouseover popups for this Business Unit ->"></asp:Label>
                            <asp:DropDownList ID="ddlcompany" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlcompany_SelectedIndexChanged">
                            </asp:DropDownList></td>
                        <td style="white-space: nowrap">
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap">
                            <asp:DropDownList ID="ddlview" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlview_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMemberType" runat="server" AutoPostBack="true" DataTextField="MemberTypeName"
                                DataValueField="MemberTypeID" OnSelectedIndexChanged="ddlMemberType_SelectedIndexChanged"
                                Visible="false">
                            </asp:DropDownList><span id="divmembertype" runat="server"> </span>
                        </td>
                        <td style="white-space: nowrap">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap">
                            <asp:Label ID="lblPrivileges" runat="server" Text="Exceptions to the standard Member Type privilege set up are highlighted.."></asp:Label></td>
                        <td style="width: 100%">
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap">
                            &nbsp;</td>
                        <td>
                        </td>
                    </tr>
                </table>
                <div id="Div1" runat="server">
                </div>
              <div id="divSiteMap" runat="server">  &nbsp; &nbsp; &nbsp; &nbsp;
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hidMemberID" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div id="Layer1" style="position: absolute; z-index: 999; left: 50%; top: 100px;">
                <img id="Img1" alt="progressing" src="/images/loading_panel.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

