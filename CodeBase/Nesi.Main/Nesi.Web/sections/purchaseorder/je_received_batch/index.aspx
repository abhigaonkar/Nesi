<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true"  Inherits="sections_purchaseorder_je_received_batch_index " Title="AP Batch JE" EnableTheming="true"  Codebehind="index.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <br />
    <dx:ASPxComboBox ID="ddl_company" Theme="NETheme01" runat="server" ValueType="System.Int32" AutoPostBack="True" ValueField="id" TextField="ddl_name" DataSourceID="Sql_companies" OnSelectedIndexChanged="ddl_company_SelectedIndexChanged"></dx:ASPxComboBox>
    <asp:SqlDataSource ID="Sql_companies" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" ></asp:SqlDataSource>
    <br />
    <uc1:layout_control ID="layout" runat="server" />
   		<dx:ASPxGridView ID="gv" runat="server" autogeneratecolumns="False" 
				ClientInstanceName="gv" Font-Names="Arial" OnCustomCallback="gv_CustomCallback" 
				OnCustomJSProperties="gv_CustomJSProperties" 
				width="100%" SettingsPager-PageSize="50" Theme="NETheme01">
				
				<Columns>
                    <dx:GridViewDataTextColumn FieldName="trans_no" Caption="Transaction Number" VisibleIndex="0" >
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="transaction_date" Caption="Transaction Date" VisibleIndex="1">

                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="gl_account" Caption="Account No"  VisibleIndex="3" >
                       
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Memo" FieldName="memo" VisibleIndex="4">
                       
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Debit" FieldName="debit" VisibleIndex="5">
                        <PropertiesTextEdit DisplayFormatString="N2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Credit" FieldName="credit" VisibleIndex="6" CellStyle-VerticalAlign="NotSet">
                        <PropertiesTextEdit DisplayFormatString="N2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                   
                    <dx:GridViewDataCheckColumn Caption="Already In BV" FieldName="in_bv" VisibleIndex="7">
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataCheckColumn Caption="Different Currency" FieldName="diff_currency" VisibleIndex="8">
                    </dx:GridViewDataCheckColumn>
                   
                </Columns>
				<SettingsBehavior AutoExpandAllGroups="True" ColumnResizeMode="Control" 
					EnableRowHotTrack="True" EnableCustomizationWindow="True"/>
				<Styles>
					<Cell Wrap="False">
					</Cell>
				    <Footer Font-Bold="True">
                    </Footer>
                    <GroupFooter Font-Bold="True">
                    </GroupFooter>
				    <BatchEditCell BackColor="#FFFFCC">
                    </BatchEditCell>
				</Styles>
				<SettingsPager PageSize="50" Mode="ShowAllRecords">
				</SettingsPager>
           

				           

				<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
					ShowGroupPanel="True" ShowHeaderFilterButton="True" GroupFormat="{1}" ShowFooter="True" ShowGroupedColumns="True" ShowGroupFooter="VisibleIfExpanded" />

        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" 
					CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Height="250px" 
					CustomizationWindow-HorizontalOffset="5" CustomizationWindow-VerticalOffset="5">


			<CustomizationWindow Height="250px" HorizontalAlign="LeftSides" 
				HorizontalOffset="5" VerticalAlign="TopSides" VerticalOffset="5" />
				</SettingsPopup>


			</dx:ASPxGridView>
    <p>
        <br />
    </p>
    <p>
    </p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

