<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="master_fvr_grid" Title="FVR Gridview"  EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>






    
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
	<lc:LayoutControl runat="server" id="layout" />
            <dx:aspxgridview id="gv" runat="server" autogeneratecolumns="False" Width="100%"
                clientinstancename="gv" 
		OnCustomCallback="gv_er_CustomCallback" 
		OnCustomJSProperties="gv_er_CustomJSProperties" Font-Names="Arial" 
		Font-Size="9pt" Theme="NETheme01">


<SettingsPager PageSize="50" NumericButtonCount="50" Position="TopAndBottom" >
<AllButton Text="All"></AllButton>

<NextPageButton Text="Next &gt;"></NextPageButton>

<PrevPageButton Text="&lt; Prev"></PrevPageButton>
</SettingsPager>

<Columns>
   
	<dx:GridViewDataTextColumn FieldName="ID" VisibleIndex="0">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn FieldName="Active" VisibleIndex="1">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataDateColumn FieldName="Date" VisibleIndex="2">
		<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
		</PropertiesDateEdit>
		<CellStyle Wrap="False">
		</CellStyle>
	</dx:GridViewDataDateColumn>
	<dx:GridViewDataTextColumn FieldName="Type" VisibleIndex="3">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn FieldName="business_unit" caption ="Business Unit" VisibleIndex="4">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn FieldName="Cut_By" VisibleIndex="5">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn FieldName="Employee" VisibleIndex="6">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn FieldName="filename" VisibleIndex="7">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn FieldName="Confirmed" VisibleIndex="8">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn FieldName="Upload_Required" VisibleIndex="9">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn FieldName="Uploaded_Filename" VisibleIndex="10">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn FieldName="HR_Status" VisibleIndex="11">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn FieldName="FVR_Status" VisibleIndex="12">
	</dx:GridViewDataTextColumn>
   
</Columns>
<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowFilterBar="Visible" UseFixedTableLayout="True"></Settings>

</dx:aspxgridview>
    </asp:Content>

