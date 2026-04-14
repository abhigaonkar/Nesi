<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_partner_specialties_index" Title="Partner Specialties" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="header_placeholder" runat="server">
	<link rel="Stylesheet" type="text/css" href="/css/customer.css" />
	<style type="text/css">
		.options
			{
background: -moz-linear-gradient(top,  rgba(204,204,204,0.65) 0%, rgba(204,204,204,0.64) 1%, rgba(0,0,0,0) 100%); /* FF3.6+ */
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(204,204,204,0.65)), color-stop(1%,rgba(204,204,204,0.64)), color-stop(100%,rgba(0,0,0,0))); /* Chrome,Safari4+ */
background: -webkit-linear-gradient(top,  rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* Chrome10+,Safari5.1+ */
background: -o-linear-gradient(top,  rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* Opera 11.10+ */
background: -ms-linear-gradient(top,  rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* IE10+ */
background: linear-gradient(to bottom,  rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* W3C */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#a6cccccc', endColorstr='#00000000',GradientType=0 ); /* IE6-9 */



			}
		.fleft
			{
			float: left;
			display:block;
			}
		.fleft .table
			{
			background-color: #fff;
			border:solid 1px #999;
			margin:5px;
			border-radius:3px;
			}
		.style5
		{
			font-size: xx-small;
		}
	</style>

	<script type="text/javascript" language="javascript">

	function get_customer(id,type) {


		if (type == "Customer") {

			boing("/sections/customer/index.aspx?customer_id=" + id, "customer", 1224, 968);
		}
		else {
			boing("/#/opens/11/vendors/" + id, "vendor", 1224, 968);

		}
			}




	</script>



</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
			<lc:LayoutControl runat="server" ID="layout" __is_private="True" GridviewID="gv" />

			<br />
			<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text=" * To add new partners, go to either a customer or a vendor, set them as a partner and add specialities from their specialities tab." Theme="NETheme01">
			</dx:ASPxLabel>

	<dx:ASPxGridView ID="gv" runat="server" ClientInstanceName="gv" Theme="NETheme01" 
				Width="100%" AutoGenerateColumns="False" 
				oncustomcallback="gv_CustomCallback" 
				oncustomjsproperties="gv_CustomJSProperties" KeyFieldName="id">
		<Columns>
			<dx:GridViewDataTextColumn FieldName="id" VisibleIndex="0" ReadOnly="True" 
				Visible="False">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="Type" 
				VisibleIndex="1" Width="100px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="Skill" VisibleIndex="2" Width="100%">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn FieldName="Date Added" VisibleIndex="3" 
				Width="100px" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn FieldName="Added By" VisibleIndex="4" Width="120px">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="business_unit" VisibleIndex="5" Width="100px" Caption="Business Unit">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="6" Width="200px">
				<CellStyle Wrap="False">
				</CellStyle>
				<DataItemTemplate>
							<a href="javascript:void(-1)" onclick="get_customer(<%# Eval("id") %>,'<%# Eval("Type") %>')">
								<%#Container.Text %>
							</a>
						</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="Phone" VisibleIndex="8" Width="100px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataCheckColumn Caption="Is A Partner" FieldName="is_partner" 
				VisibleIndex="7" Width="50px">
			</dx:GridViewDataCheckColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ColumnMinWidth="20" />
		<SettingsSearchPanel Visible="True" />
		<Styles>
			<Cell VerticalAlign="Top">
			</Cell>
		</Styles>
	</dx:ASPxGridView>
</asp:Content>

