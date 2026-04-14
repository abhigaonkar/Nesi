<%@ Page Title="Offer Gridview" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" EnableTheming="true" Theme="NETheme01" Inherits="sections_hr_offer_gridview" Codebehind="offer_gridview.aspx.cs" %>	



<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>


	



	
<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>


	
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
 function bind_tooltips()
			{
			$(".opt1").each(function()
				{
				$(this).tip();
				});
			}
		$(document).ready(function()
			{
	//		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
			});

function EndReqHandler()
	{
	bind_tooltips();
	
	}
	</script>

<table width = "100%">
<tr>
<td>
	<uc1:layout_control ID="lc" runat="server" GridviewID="gv_mo" />
	<dx:ASPxGridView ID="gv_mo" runat="server" AutoGenerateColumns="False" 
		ClientInstanceName="gv_mo" Font-Names="Arial" 
		KeyFieldName="Member_ID" Width="100%" oncustomcallback="gv_mo_CustomCallback" 
		oncustomjsproperties="gv_mo_CustomJSProperties" SettingsPager-PageSize="50">
		<Columns>
			<dx:GridViewCommandColumn Caption=" " ShowClearFilterButton="True" Visible="False" VisibleIndex="0">
            </dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="ID" FieldName="Member_ID" ReadOnly="True" 
				VisibleIndex="1">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Employee" FieldName="member_fullname" 
				VisibleIndex="2">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Cursor="pointer" 
						oninit="ASPxHyperLink1_Init" style="font-family: Arial, Helvetica, sans-serif" 
						Text='<%# Eval("member_fullname") %>' />
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Reports To" FieldName="reports_to" 
				VisibleIndex="3">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" 
				VisibleIndex="4">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Active Offer ID" FieldName="active_offer_id" 
				VisibleIndex="5" Visible="False">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Days Until Review is Due" 
				FieldName="review_days" VisibleIndex="9">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink3" runat="server" Cursor="pointer" 
						oninit="ASPxHyperLink3_Init" style="font-family: Arial, Helvetica, sans-serif" 
						Text='<%# Eval("review_days") %>' />
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Review Status" FieldName="r_status" 
				VisibleIndex="10">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Latest Offer Status" 
				FieldName="mo_status" VisibleIndex="8">
                  <DataItemTemplate>
							<dx:ASPxHyperLink ID="ASPxHyperLink4" runat="server" Text=<%# Eval("mo_status") %>
							NavigateUrl=<%# (Eval("mo_status").ToString()=="In Development")?string.Format(&quot;javascript:boing('member_offer.aspx?id={0}','mo',1100,800);&quot;,Eval(&quot;mo_id&quot;)):string.Format(&quot;javascript:boing('offer_print_off.aspx?moid={0}','mo',800,900);&quot;,Eval(&quot;mo_id&quot;)) %> 
								Theme="NETheme01" ondatabound="ASPxHyperLink4_Init" />
					
					</DataItemTemplate>
				
			</dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="mo_days" Caption="Days Until Expiry" VisibleIndex="6">
	<PropertiesTextEdit DisplayFormatString="{0}">
	</PropertiesTextEdit>
	<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" Cursor="pointer" 
						oninit="ASPxHyperLink2_Init" style="font-family: Arial, Helvetica, sans-serif" 
						Text='<%# Eval("mo_days") %>' />
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Next Offer ID" FieldName="mo_id" 
				VisibleIndex="7" Visible="False">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Next Review Date" 
				FieldName="nr_date" VisibleIndex="11">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="HR Status" FieldName="hr_status" 
				VisibleIndex="12">
			</dx:GridViewDataTextColumn>
		    <dx:GridViewDataTextColumn Caption="mo_reports_to" FieldName="mo_reports_to" Visible="False" VisibleIndex="13">
            </dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
	
	    <Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" ShowHeaderFilterButton="True" />
	</dx:ASPxGridView>
	<br />

</td>
</tr>
</table>



</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	</asp:Content>

