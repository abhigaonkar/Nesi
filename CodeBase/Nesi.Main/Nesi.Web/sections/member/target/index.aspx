
<%@ Page Language="C#" MasterPageFile="../../../IntraDefault.master" AutoEventWireup="true"  Inherits="target" EnableTheming="true" Theme="NETheme01" Title="MessageBoard Targets Admin" Codebehind="index.aspx.cs" %>



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
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<script type ="text/javascript">


</script>
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <ASP:UPDATEPANEL id="UpdatePanel1" runat="server">
    <contenttemplate>
    <ASP:UPDATEPROGRESS ID="UPDATEPROGRESS1" runat="server" DisplayAfter="250">
        <PROGRESSTEMPLATE>
        <div id="Layer1" style="position:absolute; z-index:1; left: 50%; top: 50%;">
          <img id="Img1" src="/images/loading_panel.gif" alt="progressing" />
        </div>
        </PROGRESSTEMPLATE>
    </ASP:UPDATEPROGRESS>
       
       
                    <br />
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" KeyFieldName="target_ID"
                        Width="100%" OnRowDeleting="ASPxGridView1_RowDeleting" OnRowInserting="ASPxGridView1_RowInserting" OnRowUpdating="ASPxGridView1_RowUpdating" SettingsPager-Mode="ShowAllRecords">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" Width="70px">
                                
                                
                                
                                
                                
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="target_ID" ReadOnly="True" Visible="False"
                                VisibleIndex="1">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            
                            <dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="business_unit_id" VisibleIndex="1">
                                <PropertiesComboBox DataSourceID="SqlDataSource1" TextField="name" ValueField="id"
                                    ValueType="System.Int32">
                                </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                            
                            <dx:GridViewDataComboBoxColumn Caption="Target Name" 
								FieldName="target_target_type_id" ShowInCustomizationForm="True" 
								VisibleIndex="1">
								<PropertiesComboBox DataSourceID="SqlDataSource2" TextField="target_type_Name" 
									ValueField="target_type_ID" ValueType="System.Int32">
								</PropertiesComboBox>
							</dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataDateColumn Caption="Start" FieldName="target_StartDate"
                                VisibleIndex="4">
                                <PropertiesDateEdit DisplayFormatInEditMode="True" EditFormatString="yyyy-MM-dd">
                                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesDateEdit>
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn Caption="End" FieldName="target_EndDate"
                                VisibleIndex="5">
                                <PropertiesDateEdit DisplayFormatInEditMode="True" EditFormatString="yyyy-MM-dd">
                                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesDateEdit>
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn Caption="Target Value" FieldName="target_value" VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataCheckColumn Caption="Active?" FieldName="target_active" VisibleIndex="3">
                                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0" ConvertEmptyStringToNull="False">
                                </PropertiesCheckEdit>
                            </dx:GridViewDataCheckColumn>
                           
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterBar="Visible" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
                        <SettingsBehavior EnableRowHotTrack="True" ConfirmDelete="True" />
                        <SettingsEditing Mode="Inline" />
                    </dx:ASPxGridView>
                    &nbsp;
                    
        &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp;
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
            ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" >
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
            ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT target_type_ID, target_type_Name FROM target_type">
        </asp:SqlDataSource>
        &nbsp;
    </contenttemplate>
    </ASP:UPDATEPANEL>
    </ASP:CONTENT>
