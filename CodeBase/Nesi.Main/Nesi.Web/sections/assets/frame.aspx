<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_assets_frame" Title="Assets" Codebehind="frame.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>





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
	<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
		Width="100%" Theme="NETheme01" >
		<TabPages>
			<dx:TabPage Text="Assets">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					<iframe width="100%" frameborder="0" id="frame" height="1060" src="index.aspx" allowtransparency="true" runat="server" style="background-color:Transparent" ></iframe>

						</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="History / Log" >
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<iframe ID="I1" runat="server" allowtransparency="true" frameborder="0" 
							height="1060" name="I1" src="Asset_history.aspx" 
							style="background-color:Transparent" width="100%"></iframe>
						</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
            <dx:TabPage Text="Calibration" >
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						  <iframe width="100%" frameborder="0" id="Iframe1" height="1060" src="Calibration.aspx" allowtransparency="true" runat="server" style="background-color:Transparent" ></iframe>
						</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
	</dx:ASPxPageControl>
</asp:Content>


