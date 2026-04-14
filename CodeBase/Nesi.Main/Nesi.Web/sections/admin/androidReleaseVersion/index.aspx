<%@ Page Title="Nesi App Versions" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true"  EnableTheming="True" Inherits="sections_admin_androidReleaseVersion_index" Codebehind="index.aspx.cs" %>	

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
	
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
        ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
        SelectCommand="SELECT id, code_version, dt, isLive FROM it.nesi_app_versions"
        UpdateCommand="update it.nesi_app_versions 
        set code_version = ?code_version ,
        isLive = ?isLive
        where id = ?id" 
        InsertCommand="INSERT INTO it.nesi_app_versions 
        (code_version, isLive) 
        VALUES (?code_version,
         ?isLive)" 
        >
        <UpdateParameters>
			<asp:Parameter Name="id" />
			<asp:Parameter Name="code_version" />
			<asp:Parameter Name="isLive" />
		</UpdateParameters>
        <InsertParameters>
			<asp:Parameter Name="code_version" />
			<asp:Parameter Name="isLive" />
        </InsertParameters>
	</asp:SqlDataSource>


    <dx:ASPxGridView ID="ASPxGridView1" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" KeyFieldName="id" Theme="NETheme01">
        <Columns>
            <dx1:GridViewCommandColumn ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0">
            </dx1:GridViewCommandColumn>
            <dx1:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="1">
                <EditFormSettings Visible="False" />
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn FieldName="code_version" VisibleIndex="2">
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataDateColumn FieldName="dt" VisibleIndex="4" ReadOnly="True">
                <PropertiesDateEdit DisplayFormatString="">
                </PropertiesDateEdit>
                <EditFormSettings Visible="False" />
            </dx1:GridViewDataDateColumn>
            <dx1:GridViewDataCheckColumn FieldName="isLive" VisibleIndex="3">
            </dx1:GridViewDataCheckColumn>
        </Columns>

        <Styles>
            <EditForm BackColor="WhiteSmoke">
            </EditForm>
        </Styles>

    </dx:ASPxGridView>
	
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
		</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">

	
    

	
</asp:Content>