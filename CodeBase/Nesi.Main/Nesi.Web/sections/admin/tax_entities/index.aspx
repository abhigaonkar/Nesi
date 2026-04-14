<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_tax_entities_index"  Title="Tax Entities" EnableTheming="True" Codebehind="index.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<%@ Register src="../../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
    <div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
    <div id="divSide" runat="server">
	</div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
   
    <script language="javascript">

        function custom_button_click(s,e)
        {
            if (e.buttonID=="gl")
            {
                boing("gl_structure.aspx?te_id=" + s.GetRowKey(e.visibleIndex), "gl_structure", 1000, 800);
            }
            else if (e.buttonID == "SyncBV") {
                boing("sync_bv_data.aspx?te_id=" + s.GetRowKey(e.visibleIndex), "Sync BV Data", 1000, 800);
            }
            else if (e.buttonID == "files") {
                pop_files.Show();
                pop_files.PerformCallback(s.GetRowKey(e.visibleIndex));
            }
            else
            {
                boing("business_units.aspx?te_id=" + s.GetRowKey(e.visibleIndex), "business_units", 1000, 800);

            }

        }


    </script>

	<dx:ASPxCallbackPanel ID="cp" runat="server" ClientInstanceName="cp" 
		oncallback="cp_Callback">
		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<table>
					<tr>
						<td class="auto-style1">
							
							<lc:LayoutControl ID="layout" runat="server" is_private="True" 
								GridviewID="gv_te" />
							
						    <asp:SqlDataSource ID="sql_te" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" InsertCommand="insert into tax_entity (public_name,ddl_name,nesi_nickname,tax_id_no,is_test,dsn,is_active,yearend_month,tax1,tax2,tax3,tax4,sync_customers,sync_vendors,default_mileage_rate) values(@public_name,@ddl_name,@nesi_nickname,@tax_id_no,@is_test,@dsn,@is_active,@yearend_month,@tax1,@tax2,@tax3,@tax4,@sync_customers,@sync_vendors,@default_mileage_rate)" 
                                ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, public_name, ddl_name, nesi_nickname, tax_id_no, is_test, dsn, is_active, YearEnd_Month, tax1, tax2, tax3, tax4,sync_customers,sync_vendors,default_mileage_rate FROM tax_entity" UpdateCommand="update tax_entity set public_name=@public_name, ddl_name=@ddl_name, nesi_nickname=@nesi_nickname, tax_id_no=@tax_id_no, is_test=@is_test, dsn=@dsn, is_active=@is_active, yearend_month=@yearend_month, tax1=@tax1,  tax2=@tax2,  tax3=@tax3,  tax4=@tax4,  sync_customers=@sync_customers, sync_vendors=@sync_vendors, default_mileage_rate=@default_mileage_rate  where id = @id" DeleteCommand="Delete from tax_entity where id = ?id ">
                                <DeleteParameters>
                                    <asp:Parameter Name="@id" />

                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="id" />
                                     <asp:Parameter Name="public_name" />
                                    <asp:Parameter Name="ddl_name" />
                                    <asp:Parameter Name="nesi_nickname" />
                                    <asp:Parameter Name="tax_id_no" />
                                    <asp:Parameter Name="is_test" />
                                    <asp:Parameter Name="dsn" />
                                    <asp:Parameter Name="is_active" />
                                    <asp:Parameter Name="yearend_month" />
                                    <asp:Parameter Name="tax1" />
                                     <asp:Parameter Name="tax2" />
                                     <asp:Parameter Name="tax3" />
                                     <asp:Parameter Name="tax4" />
                                   
                                    <asp:Parameter Name="sync_customers" />
                                    <asp:Parameter Name="sync_vendors" />
                                    <asp:Parameter Name="default_mileage_rate" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="public_name" />
                                    <asp:Parameter Name="ddl_name" />
                                    <asp:Parameter Name="nesi_nickname" />
                                    <asp:Parameter Name="tax_id_no" />
                                    <asp:Parameter Name="is_test" />
                                    <asp:Parameter Name="dsn" />
                                    <asp:Parameter Name="is_active" />
                                    <asp:Parameter Name="yearend_month" />
                                     <asp:Parameter Name="tax1" />
                                     <asp:Parameter Name="tax2" />
                                     <asp:Parameter Name="tax3" />
                                     <asp:Parameter Name="tax4" />
                                   
                                    <asp:Parameter Name="sync_customers" />
                                    <asp:Parameter Name="sync_vendors" />
                                    <asp:Parameter Name="default_mileage_rate" />
                                </UpdateParameters>
                            </asp:SqlDataSource>
							
						    <asp:SqlDataSource ID="sql_tax" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select * from tax where is_active = 1"></asp:SqlDataSource>
							
						    <br />
							
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxGridView ID="gv_te" runat="server" AutoGenerateColumns="False" 
								ClientInstanceName="gv_te" DataSourceID="sql_te" 
								KeyFieldName="id" OnCustomCallback="gv_te_CustomCallback" 
								OnCustomJSProperties="gv_te_CustomJSProperties" Width="100%" 
								
								Font-Names="Arial"  Theme="NETheme01" 
								 SettingsBehavior-ConfirmDelete="True" SettingsBehavior-ColumnResizeMode="Control" OnRowUpdating="gv_te_RowUpdating" >


<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control" ConfirmDelete="True"></SettingsBehavior>


<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowGroupPanel="True" ></Settings>



								<ClientSideEvents CustomButtonClick="function(s, e) {
	custom_button_click(s,e);
}" />


                                <SettingsCommandButton>

<UpdateButton Text="Update">
<Image Url="~/images/icon/icon[save].gif"></Image>
</UpdateButton>

<CancelButton>
<Image Url="~/images/icon/icon[cancel].gif"></Image>
</CancelButton>

	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px">
<Image Height="16px" Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
                                    </EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"  >
<Image Height="16px" Width="16px" Url="~/images/icon/icon[add].gif"></Image>
                                    </NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px">
<Image Height="16px" Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
                                    </DeleteButton>
</SettingsCommandButton>
								<Columns>
									<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
										ShowInCustomizationForm="True" VisibleIndex="0" Width="150px" ShowNewButtonInHeader="True" ShowCancelButton="True" ShowUpdateButton="True" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"   ShowNewButton="true">
										
										
										
										
										<CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="segments" Image-ToolTip="Business Units">
                                                <Image Url="~/images/icon/icon[company].gif">
                                                </Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewCommandColumn>
									<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" 
										ShowInCustomizationForm="True" VisibleIndex="1" Width="30px">
										<EditFormSettings Visible="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Public Name" FieldName="public_name"  ReadOnly="true"
										ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
										
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="DDL Name" FieldName="ddl_name" 
										ShowInCustomizationForm="True" VisibleIndex="3" Width="100px">
										
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Nesi Nickname" FieldName="nesi_nickname" 
										ShowInCustomizationForm="True" VisibleIndex="4" Width="100px">
										
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Tax ID No" FieldName="tax_id_no" 
										ShowInCustomizationForm="True" VisibleIndex="5" Width="50px" ReadOnly="true">
										
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="DSN" 
										FieldName="dsn" ShowInCustomizationForm="True" VisibleIndex="6" 
										Width="100px">
										
									</dx:GridViewDataTextColumn>
									
									
								    <dx:GridViewDataCheckColumn Caption="IS Test" FieldName="is_test" ShowInCustomizationForm="True" VisibleIndex="7" Width="30px">
                                    </dx:GridViewDataCheckColumn>
                                    <dx:GridViewDataCheckColumn Caption="Is Active" FieldName="is_active" ShowInCustomizationForm="True" VisibleIndex="8" Width="30px" ReadOnly="true">
                                    </dx:GridViewDataCheckColumn>
									
									
								    <dx:GridViewDataComboBoxColumn Caption="Tax 1" FieldName="tax1" ShowInCustomizationForm="True" VisibleIndex="9" Width="30px" ReadOnly="true">
                                        <PropertiesComboBox DataSourceID="sql_tax" TextField="tax_name" ValueField="tax_id">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>

                                     <dx:GridViewDataComboBoxColumn Caption="Tax 2" FieldName="tax2" ShowInCustomizationForm="True" VisibleIndex="10" Width="30px" ReadOnly="true">
                                        <PropertiesComboBox DataSourceID="sql_tax" TextField="tax_name" ValueField="tax_id">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
									
                                     <dx:GridViewDataComboBoxColumn Caption="Tax 3" FieldName="tax3" ShowInCustomizationForm="True" VisibleIndex="11" Width="30px" ReadOnly="true">
                                        <PropertiesComboBox DataSourceID="sql_tax" TextField="tax_name" ValueField="tax_id">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
									
                                     <dx:GridViewDataComboBoxColumn Caption="Tax 4" FieldName="tax4" ShowInCustomizationForm="True" VisibleIndex="12" Width="30px" ReadOnly="true">
                                        <PropertiesComboBox DataSourceID="sql_tax" TextField="tax_name" ValueField="tax_id">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
								    <dx:GridViewDataCheckColumn Caption="Sync Customers" FieldName="sync_customers" 
								                               ShowInCustomizationForm="True" VisibleIndex="13" Width="50px" ReadOnly="true">
								    </dx:GridViewDataCheckColumn>
								    
								    <dx:GridViewDataCheckColumn Caption="Sync Vendors" FieldName="sync_vendors" 
								                               ShowInCustomizationForm="True" VisibleIndex="15" Width="50px" ReadOnly="true">
								    </dx:GridViewDataCheckColumn>
								    
								    <dx:GridViewDataTextColumn Caption="Year End Month" FieldName="YearEnd_Month" 
								                               ShowInCustomizationForm="True" VisibleIndex="16" Width="50px" ReadOnly="true">
										
								    </dx:GridViewDataTextColumn>
								    <dx:GridViewDataTextColumn Caption="default_mileage_rate" FieldName="default_mileage_rate" 
								                               ShowInCustomizationForm="True" VisibleIndex="17" Width="50px">
										
								    </dx:GridViewDataTextColumn>
									
								</Columns>


								<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" 
									ConfirmDelete="True" />


								<SettingsPager Mode="ShowAllRecords">
                                </SettingsPager>
								<SettingsEditing  Mode="Inline" />


								<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" 
									ShowHeaderFilterButton="True" />



								<SettingsDataSecurity AllowDelete="False" AllowInsert="False" />



								<SettingsText CommandClearFilter="Clear"/>
							    <SettingsCommandButton>
                                    <UpdateButton Text="Update">
                                        <Image Url="~/images/icon/icon[save].gif">
                                        </Image>
                                    </UpdateButton>
                                    <CancelButton>
                                        <Image Url="~/images/icon/icon[cancel].gif">
                                        </Image>
                                    </CancelButton>
                                    
                                </SettingsCommandButton>
							    
							</dx:ASPxGridView>
						    <br />
                            <dx:ASPxPopupControl ID="pop_files" runat="server" HeaderText="Files" Height="600px" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Theme="NETheme01" Width="900px" ClientInstanceName="pop_files" CloseAction="CloseButton" OnWindowCallback="pop_files_WindowCallback">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <div id="div_files" runat="server"></div>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
						</td>
						
					</tr>
					
				</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
   
</asp:Content>




<asp:Content ID="Content5" runat="server" contentplaceholderid="header_placeholder">
    <style type="text/css">
        .auto-style1 {
            height: 141px;
        }
    </style>
</asp:Content>





