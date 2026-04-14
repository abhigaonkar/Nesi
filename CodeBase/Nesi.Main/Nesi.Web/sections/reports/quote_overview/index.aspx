<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits		= 'sections_reports_quote_overview_index' 
			Title			= 'Quote Overview' 
 Codebehind="index.aspx.cs" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				<div id='divSide' runat='server'>
					<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
				</div>
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
				<div id='divMenu' runat='server'></div>
</asp:Content>
	<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphMasterBody">
		<div id="report" runat="server">
		</div>
	</asp:Content>