<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits		= 'top_twenty' 
			Title			= 'Top Twenty Customers' 
 Codebehind="top_twenty.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				<div id='divSide' runat='server'>
					<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
				</div>
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
				<div id='divMenu' runat='server'></div>
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server'>

    <table class='framework' border='0' cellpadding='0' cellspacing='0'>
							<tr>
								<td valign='top' align='center' id='detail' runat='server'>
									<asp:ScriptManager ID="sm" runat="server">
									</asp:ScriptManager>
							<asp:UpdateProgress ID="progress" runat="server" AssociatedUpdatePanelID="up">
								<ProgressTemplate>
										<div id="Layer1" style="text-align: center;" class="update_progress">
											<center>
											<div align="center" style="border:solid 2px #ddd;width:150px;background-color:#fff;padding:10px;border-radius:10px;">
												<div>Loading</div>
												<img id="Img1" src="/images/loading_panel.gif" alt="progressing" />
											</div>
											</center>
										</div>
								</ProgressTemplate>
							</asp:UpdateProgress>
									<asp:UpdatePanel ID="up" runat="server">
										<ContentTemplate>
											<div align="left">
											<script type="text/javascript">
											
	$("document").ready(function()
							{
							Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
							});
	function BeginReqHandler()
		{
		$(".update_progress").css(	{
									"height"			: "100%", 
									"z-index"			: $.maxZ(), 
									"width"				: "100%",
									"position"			: "fixed",
									"padding-top"		: "200px",
									"top"				: 0,
									"left"				:0,
									});
		}

document.getElementsByClassName = function(cl) {
var retnode = [];
var myclass = new RegExp('\\b'+cl+'\\b');
var elem = this.getElementsByTagName('*');
for (var i = 0; i < elem.length; i++) {
var classes = elem[i].className;
if (myclass.test(classes)) retnode.push(elem[i]);
}
return retnode;
}; 


function ToggleRow(id) {

var e = document.getElementsByClassName(id);
for(var i = 0, len = e.length; i<len; i++){ 

 if( e[i].style.display=='none' ){
   e[i].style.display = '';
 }else{
   e[i].style.display = 'none';
 }
}
}
</script>
											<asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Height="31px" Text="Top Twenty Customer Report by Sales - Beta Version For Test" Width="367px"></asp:Label>
    <br />
    										<asp:Label ID="lblCompany" runat="server" Text="Select Business Unit"></asp:Label>
											<asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" datatextfield="name" datavaluefield="id" Enabled="False" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
											</asp:DropDownList>
											<asp:CheckBox ID="chkProfitSel" runat="server" Text="Sort by Gross Profit" AutoPostBack="True" oncheckedchanged="ddlCompany_SelectedIndexChanged" />
											<asp:RadioButtonList ID="radio_top_choice" runat="server" AutoPostBack="True" onselectedindexchanged="ddlCompany_SelectedIndexChanged">
												<asp:ListItem Selected="True" Text="Top 20" Value="20"></asp:ListItem>
												<asp:ListItem Text="Top 50" Value="50"></asp:ListItem>
												<asp:ListItem Text="Top 100" Value="100"></asp:ListItem>
											</asp:RadioButtonList>
											<br />
    										&nbsp;<br />
											<dx:ASPxGridView ID="gv_top_customers" runat="server" autogeneratecolumns="False" OnHtmlDataCellPrepared="gv_top_customers_HtmlDataCellPrepared">
												<Columns>
													<dx:GridViewDataTextColumn Caption="Customer" FieldName="Customer_Name" VisibleIndex="1">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="YTD Sales" FieldName="YTD_Sales" VisibleIndex="2">
														<PropertiesTextEdit DisplayFormatString="C2">
														</PropertiesTextEdit>
														<HeaderStyle HorizontalAlign="Center" />
														<CellStyle HorizontalAlign="Center">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="YTD Profit" FieldName="YTD_Profit" VisibleIndex="3">
														<PropertiesTextEdit DisplayFormatString="C2">
														</PropertiesTextEdit>
														<HeaderStyle HorizontalAlign="Center" />
														<CellStyle HorizontalAlign="Center">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Last YTD Sales" VisibleIndex="4">
														<PropertiesTextEdit DisplayFormatString="C2">
														</PropertiesTextEdit>
														<CellStyle HorizontalAlign="Center">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Last YTD Profit" VisibleIndex="5">
														<PropertiesTextEdit DisplayFormatString="C2">
														</PropertiesTextEdit>
														<CellStyle HorizontalAlign="Center">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Cust No" FieldName="cust_id" VisibleIndex="0">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Overall Margin" FieldName="overall_margin" VisibleIndex="6">
														<PropertiesTextEdit DisplayFormatString="P2">
														</PropertiesTextEdit>
													</dx:GridViewDataTextColumn>
												</Columns>
												<SettingsPager Mode="ShowAllRecords" PageSize="100">
												</SettingsPager>
											</dx:ASPxGridView>
    <br />
    <br />
    										<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" style="border-top-style: dotted; border-right-style: dotted; border-left-style: dotted; border-bottom-style: dotted" Text="Print Report" Visible="False" />
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
	</table>
    <br />
    &nbsp;<br />
    
</asp:Content>

