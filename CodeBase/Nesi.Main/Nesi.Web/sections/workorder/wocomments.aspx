<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_workorder_wocomments" MasterPageFile="~/nonframe.master" Codebehind="wocomments.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content runat="server" ContentPlaceHolderID="header_placeholder">
	<script type="text/javascript">
		function opener_update()
			{
			if(opener != undefined && opener != null)
				{
				try
					{
					opener.update_header();
					}
				catch(err){}
				}
			}
	$(window).bind('beforeunload', function(){
		opener_update();
	});
		$(document).ready(function()
			{
			page_obj.update_panel_progress.bind();
			});
	</script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphMasterBody">
	<asp:ScriptManager runat="server" ID="sm"></asp:ScriptManager>
	<asp:UpdatePanel runat="server" ID="up">
		<ContentTemplate>
    <div>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="600px" Height="450px" HeaderText="Comments">
            <PanelCollection>
                <dx:PanelContent runat="server">
                    <div class="chatcontent" id="divComments" runat="server" style="OVERFLOW: auto; POSITION: Relative; HEIGHT: 167px; width: 600px; left: 7px; top: 7px;">
                    </div>
                    <br />
                    <asp:Label ID="lblChatHistory" runat="server" Text="Add a New Comment to Chat History"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtChat" runat="server" Rows="5" TextMode="MultiLine" Width="500px"></asp:TextBox>
                    <asp:Button ID="btnChatAdd" runat="server" Text="Add" OnClick="btnChatAdd_Click" UseSubmitBehavior="false"  />
                    <br />
                    <asp:Label ID="lblTXComment" runat="server" Font-Names="Arial" Text="Comments Entered Through Time Sheet"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtComments" runat="server" Rows="5" TextMode="MultiLine" Width="500px"></asp:TextBox>
                    <asp:Button ID="btnUpdateTS" runat="server" OnClick="btnUpdateTS_Click" Text="Save" />
					<asp:HiddenField runat="server" ID="hid_ts_comment_id" Value=""/>
                    <br />
                    <br />
                    <asp:CheckBox ID="chkPrint" runat="server" Font-Names="Arial" Text="Print Time Sheet Comments on Invoice" />
                    &nbsp;
                    <asp:Label ID="Label4" runat="server" Font-Names="Arial" Text="Label" 
						Visible="False"></asp:Label>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br />
        <asp:HiddenField ID="hidWOBVWO" runat="server" />
        <asp:HiddenField ID="hidCompanyID" runat="server" />
        <asp:HiddenField ID="hidWOProgID" runat="server" />
    
    </div>
			</ContentTemplate>
		</asp:UpdatePanel>
</asp:Content>