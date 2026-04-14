<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" EnableTheming = "True"  Theme="NETheme01" Inherits="corporations" Title="Corporations" Codebehind="index.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
                <div id='divMenu' runat='server'></div>
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server'>
    <style type="text/css">
	    .gv_override {
            background-color:transparent !important;
            
	    }
    </style>

    <asp:SqlDataSource ID="sql_corps" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select business_unit_id id,name, company_business_number from business_unit  order by name">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Sqlcompany" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select business_unit_id,name from business_unit   order by name"></asp:SqlDataSource>
     <asp:SqlDataSource ID="Sqlgroups" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select id,company_group from business_unit _groups  order by company_group"></asp:SqlDataSource>

       <asp:SqlDataSource ID="Sqlowner" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
        SelectCommand="
select (business_unit.id + 1000000) id,
        business_unit.name name
        from business_unit 
        union

select
member.member_id id,
 member.member_fullname name 
from member where member_status='Active' ">


    </asp:SqlDataSource>
    <dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv" DataSourceID="sql_corps" KeyFieldName="id" Width="100%">
        <Columns>
            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" Width="30px">
                <CellStyle HorizontalAlign="Left" VerticalAlign="Top">
                </CellStyle>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="1" Width="25px" Caption="IS" Visible="False">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="name" VisibleIndex="2" Width="100%" Caption="Company Name">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="company_business_number" VisibleIndex="3" Width="100px" Caption="Tax ID">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Owned By" VisibleIndex="4" Width="150px">
                <EditFormSettings Visible="False" />
                <DataItemTemplate>
                    <asp:SqlDataSource ID="Sqlowners" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
                        ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" OnDataBinding="sql_companies_DataBinding"
                        SelectCommand="


select
company_owner.id id,
company_owner.member_id member_id,
company_owner.parent_business_unit_id,
if (company_owner.member_id=0,company_owner.parent_business_unit_id+1000000,company_owner.member_id) uni_id
from business_unit _owner

where company_owner.business_unit_id = ?id" 
                        DeleteCommand="delete from business_unit _owner where id = ?id" 
                        InsertCommand="insert into company_owner (business_unit_id,member_id,parent_business_unit_id) values(?id,if(?uni_id&lt;1000000,?uni_id,0),if(?uni_id&gt;1000000,(?uni_id-1000000),0))">
                        
                        <InsertParameters>
                        <asp:Parameter Name="uni_id" />
                        
                        </InsertParameters>
                        
                    </asp:SqlDataSource>
                    <dx:ASPxGridView ID="gv_owners" runat="server" AutoGenerateColumns="False" KeyFieldName="id" Theme="NETheme01" Width="100%" DataSourceID="Sqlowners" SettingsBehavior-ConfirmDelete="True">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowDeleteButton="True" ShowNewButton="True" VisibleIndex="0" Width="30px" ShowNewButtonInHeader="True">
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn Caption="id" FieldName="id" Visible="False" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataComboBoxColumn Caption="Name"  VisibleIndex="1" Width="100%" FieldName="uni_id">
                                <PropertiesComboBox DataSourceID="Sqlowner" TextField="name" ValueField="id"  ValueType="System.Int32">
                                </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>

                        </Columns>
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsPager Visible="False">
                        </SettingsPager>
                        <SettingsEditing Mode="PopupEditForm">
                        </SettingsEditing>
                        <Settings ColumnMinWidth="10" ShowColumnHeaders="False" />
                        <SettingsText EmptyDataRow=" " />
                        <SettingsPopup>
                            <EditForm HorizontalAlign="WindowCenter" Modal="True" VerticalAlign="WindowCenter" />
                        </SettingsPopup>
                        <SettingsCommandButton>
                            <NewButton>
                                <Image Url="~/images/icon/icon[add].gif">
                                </Image>
                            </NewButton>

                            <UpdateButton ButtonType="Link" Text="Save">
                            </UpdateButton>
                            <CancelButton ButtonType="Link" Text="Cancel">
                            </CancelButton>
                            <DeleteButton>
                                <Image Url="~/images/icon/icon[delete].gif">
                                </Image>
                            </DeleteButton>
                        </SettingsCommandButton>
                    </dx:ASPxGridView>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Operating Business" VisibleIndex="5" Width="150px" Visible="false">
                <EditFormSettings Visible="False" />
                <DataItemTemplate>
                    <asp:SqlDataSource ID="sql_companies" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
                        SelectCommand="Select business_unit.id,name from business_unit  inner join company_owner on company_owner.business_unit_id = business_unit.id where company_owner.parent_business_unit_id= ?id" OnDataBinding="sql_companies_DataBinding" DeleteCommand="delete from business_unit _owner where parent_business_unit_id=?id and business_unit_id=?business_unit_id" InsertCommand="insert into company_owner (business_unit_id,parent_business_unit_id) values(?business_unit_id,?id)">
                              <DeleteParameters>
                            <asp:Parameter Name="business_unit_id" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="business_unit_id" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <dx:ASPxGridView ID="gv_companies" runat="server"  Theme="NETheme01" Width="100%" DataSourceID="sql_companies" style="margin-top: 0px" AutoGenerateColumns="False" KeyFieldName="business_unit_id">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowDeleteButton="True" ShowNewButtonInHeader="True" VisibleIndex="0" Width="30px" ShowNewButton="True">
                                <CellStyle Wrap="False" HorizontalAlign="Left">
                                </CellStyle>
                            </dx:GridViewCommandColumn>
                            
                            <dx:GridViewDataComboBoxColumn Caption="Name" FieldName="business_unit_id" VisibleIndex="1" Width="100%" >
                                <PropertiesComboBox DataSourceID="Sqlcompany" TextField="name" 
                                    ValueField="business_unit_id" ValueType="System.Int32" EnableCallbackMode="True">
                                </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                        </Columns>
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsPager Mode="ShowAllRecords" Visible="False">
                        </SettingsPager>
                        <SettingsEditing Mode="PopupEditForm">
                        </SettingsEditing>
                        <Settings ColumnMinWidth="10" ShowColumnHeaders="False" />
                        <SettingsText EmptyDataRow=" " />
                        <SettingsPopup>
                            <EditForm HorizontalAlign="WindowCenter" Modal="True" VerticalAlign="WindowCenter" />
                        </SettingsPopup>
                        <SettingsCommandButton>
                            
                            <UpdateButton ButtonType="Link" Text="Save">
                            </UpdateButton>
                            <CancelButton ButtonType="Link" Text="Cancel">
                            </CancelButton>
                            
                        </SettingsCommandButton>
                        <Styles>
                            <Row BackColor="Transparent" CssClass="gv_override">
                            </Row>
                            <AlternatingRow BackColor="Transparent" CssClass="gv_override">
                            </AlternatingRow>
                        </Styles>
                    </dx:ASPxGridView>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        
            <dx:GridViewDataTextColumn Caption="In Group" VisibleIndex="6" Width="150px">
                <EditFormSettings Visible="False" />
                <DataItemTemplate>
                    <asp:SqlDataSource ID="sql_company_group" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
                        DeleteCommand="delete from business_unit _group_link where company_group_link.business_unit_id=?id and company_group_link.company_group_id=1" 
                        ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
                        SelectCommand="  SELECT
company_group_link.id,
company_groups.company_group,
company_group_link.company_group_id
FROM
company_groups
INNER JOIN company_group_link ON company_group_link.company_group_id = company_groups.id
INNER join business_unit ON company_group_link.business_unit_id = business_unit.id
where business_unit.id=?id" InsertCommand="insert into company_group_link (business_unit_id,company_group_id) values(?id,?company_group_id)" OnDataBinding="sql_company_group_DataBinding" >
                        <InsertParameters>
                            <asp:Parameter Name="company_group_id" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                   
                             <dx:ASPxGridView ID="gv_groups" runat="server"  Theme="NETheme01" Width="100%" DataSourceID="sql_company_group" style="margin-top: 0px" AutoGenerateColumns="False" KeyFieldName="id">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowDeleteButton="True" ShowNewButtonInHeader="True" VisibleIndex="0" Width="30px" ShowNewButton="True">
                                <CellStyle Wrap="False" HorizontalAlign="Left">
                                </CellStyle>
                            </dx:GridViewCommandColumn>
                            
                            <dx:GridViewDataComboBoxColumn Caption="Name" FieldName="company_group_id" VisibleIndex="1" Width="100%" >
                                <PropertiesComboBox DataSourceID="Sqlgroups" TextField="company_group" 
                                    ValueField="id" ValueType="System.Int32" EnableCallbackMode="True">
                                </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataTextColumn FieldName="company_group_id" Visible="False" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsPager Mode="ShowAllRecords" Visible="False">
                        </SettingsPager>
                        <SettingsEditing Mode="PopupEditForm">
                        </SettingsEditing>
                        <Settings ColumnMinWidth="10" ShowColumnHeaders="False" />
                        <SettingsText EmptyDataRow=" " />
                        <SettingsPopup>
                            <EditForm HorizontalAlign="WindowCenter" Modal="True" VerticalAlign="WindowCenter" />
                        </SettingsPopup>
                        <SettingsCommandButton>
                            
                            <UpdateButton ButtonType="Link" Text="Save">
                            </UpdateButton>
                            <CancelButton ButtonType="Link" Text="Cancel">
                            </CancelButton>
                            
                        </SettingsCommandButton>
                        <Styles>
                            <Row BackColor="Transparent" CssClass="gv_override">
                            </Row>
                            <AlternatingRow BackColor="Transparent" CssClass="gv_override">
                            </AlternatingRow>
                        </Styles>
                    </dx:ASPxGridView>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        
        </Columns>
        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
        <SettingsPager Mode="ShowAllRecords">
        </SettingsPager>
        <SettingsEditing Mode="PopupEditForm">
        </SettingsEditing>
        <Settings ColumnMinWidth="20" ShowFilterBar="Visible" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" />
        <SettingsCommandButton>
            
            <UpdateButton ButtonType="Link" Text="Save">
            </UpdateButton>
            <CancelButton ButtonType="Link" Text="Cancel">
            </CancelButton>
            
            
        </SettingsCommandButton>
        <SettingsSearchPanel Visible="True" />
        <Styles>
            <Cell VerticalAlign="Top">
            </Cell>
        </Styles>
    </dx:ASPxGridView>
	
</asp:Content>
