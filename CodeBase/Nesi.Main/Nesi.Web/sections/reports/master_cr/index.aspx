<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="master_cr" Title="Master Responsibilites Grid" EnableTheming="True" Theme="NETheme01" Codebehind="index.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript">
		function q(id, rev)
			{
			boing("/sections/member/quote/index.aspx?a=g&quote_id="+id+"&revision="+rev, "quote", 1035, 800);
			}
    </script>
	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
	<ASP:UPDATEPROGRESS ID="UPDATEPROGRESS1" runat="server" DisplayAfter="100" AssociatedUpdatePanelID="UpdatePanel1">
        <PROGRESSTEMPLATE>
        <div id="Layer1" style="position:fixed; left: 0px; top: 0px; width: 100%; padding-top: 200px; text-align: center;" class="update_progress">
          <img id="Img1" src="/images/loading_panel.gif" alt="progressing" />
        	<br />
			
        </div>
        </PROGRESSTEMPLATE>
    </ASP:UPDATEPROGRESS>
	<lc:LayoutControl ID="layout" runat="server" is_private="True" 
				ShowExcelExport="True" GridviewID="gv_cr" />
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<dx:ASPxGridView ID="gv_cr" runat="server" AutoGenerateColumns="False" 
				ClientInstanceName="gv_cr" oncustomcallback="gv_cr_CustomCallback" 
				oncustomjsproperties="gv_cr_CustomJSProperties" 
				onhtmldatacellprepared="gv_cr_HtmlDataCellPrepared" Width="100%">
				<Columns>
					<dx:GridViewDataTextColumn Caption="crid" FieldName="crid" Visible="False" 
						VisibleIndex="6">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="name" 
						VisibleIndex="0" Width="50px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Responsibility" 
						FieldName="core_responsibility" VisibleIndex="1">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Membertype" FieldName="title" 
						VisibleIndex="2" Width="80px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Name" FieldName="should_be" 
						VisibleIndex="3" Width="100px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Current Offer Title" 
						FieldName="offered_title" VisibleIndex="4" Width="150px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Name on Offer" FieldName="offered_cr" 
						VisibleIndex="5" Width="150px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
				</Columns>
				<SettingsBehavior ColumnResizeMode="Control" />
				<SettingsPager PageSize="200">
				</SettingsPager>
			</dx:ASPxGridView>
			<dx:ASPxCallbackPanel ID="cb_chkprivate" runat="server" 
				ClientInstanceName="cb_chkprivate" oncallback="cb_chkprivate_Callback" 
				Width="200px">
				<LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle">
				</LoadingPanelStyle>
				<PanelCollection>
					<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
					</dx:PanelContent>
				</PanelCollection>
			</dx:ASPxCallbackPanel>
<br />
		</ContentTemplate>
	</asp:UpdatePanel>
            </asp:Content>

<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	</asp:Content>


