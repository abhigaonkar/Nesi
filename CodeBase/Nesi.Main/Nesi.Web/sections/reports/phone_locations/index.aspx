<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="phone_locations" Title="Phone Locations" Codebehind="index.aspx.cs" %>

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
	<script type="text/javascript" language="javascript">
function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 30) + 'px';
	
 }
 </script>

	<lc:LayoutControl runat="server" id="layout" __is_private="True" />
            <dx:ASPxGridView ID="gv_gps" runat="server" 
		AutoGenerateColumns="False" ClientInstanceName="gv_gps" 
		KeyFieldName="locationid_id" 
		OnCustomCallback="gv_MasterContacts_CustomCallback" 
		OnCustomJSProperties="gv_MasterContacts_CustomJSProperties" Font-Names="Arial" 
		Font-Size="9pt" Width="1000px" DataSourceID="SqlDataSource1" 
		onhtmleditformcreated="gv_gps_HtmlEditFormCreated">
                <Templates>
                    <TitlePanel>
                        &nbsp;
                    </TitlePanel>
                	<EditForm>
						<dx:ASPxLabel ID="_title" runat="server" Font-Names="Arial" Font-Size="14pt" 
							Text="ASPxLabel" Width="100%">
						</dx:ASPxLabel>
						<br />
						<br />
						<iframe id="editframe" runat="server" width = "900" frameborder="0" 
							name="editframe" height="700"></iframe>
						<br />
					</EditForm>
                </Templates>
                <GroupSummary>
					<dx:ASPxSummaryItem DisplayFormat="Contacts:{0}" FieldName="Contact_ID" 
						ShowInColumn="Customer Name" SummaryType="Count" />
					<dx:ASPxSummaryItem DisplayFormat="Contacts:{0}" FieldName="Contact_ID" 
						ShowInColumn="Vendor Name" SummaryType="Count" />
				</GroupSummary>
                <Columns>
					<dx:GridViewCommandColumn VisibleIndex="0">
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn FieldName="locationid_id" ReadOnly="True" 
						VisibleIndex="1">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="latitude_db" 
						VisibleIndex="2">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="longitude_db" 
						VisibleIndex="3">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="timestamp_dt" VisibleIndex="4">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="imei_ds" 
						VisibleIndex="5">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataComboBoxColumn FieldName="phone" VisibleIndex="6">
						<PropertiesComboBox DataSourceID="Sqlmembers" 
							TextField="get_name(member.Member_ID)" ValueField="id">
						</PropertiesComboBox>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataTextColumn FieldName="softwareversion_ds" VisibleIndex="7">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="simserial_ds" 
						VisibleIndex="8">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="subscribeid_ds" 
						VisibleIndex="9">
					</dx:GridViewDataTextColumn>
					
					<dx:GridViewDataTextColumn FieldName="androidid_ds" 
						VisibleIndex="10">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="battery_db" 
						VisibleIndex="11" MinWidth="10">
					</dx:GridViewDataTextColumn>
				</Columns>
                <SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control" 
					AutoFilterRowInputDelay="6000" EnableCustomizationWindow="True"/>
                <Styles>
                    <Header Font-Bold="True" BackColor="#FFCC66">
					</Header>
                    <Cell Wrap="False">
                    </Cell>
					<EditFormColumnCaption Font-Bold="True">
					</EditFormColumnCaption>
                	<TitlePanel BackColor="Silver">
						<Border BorderColor="Silver" />
					</TitlePanel>
                </Styles>
                <StylesPopup EditForm-Header-BackColor="#0066FF" EditForm-Header-ForeColor="White" EditForm-Header-Font-Bold="true" EditForm-Content-Paddings-Padding="20px"/>
                <SettingsPager AlwaysShowPager="True" NumericButtonCount="5" PageSize="100" 
					Position="TopAndBottom">
                    <AllButton Text="All">
                    </AllButton>
                </SettingsPager>
                <SettingsEditing Mode="PopupEditForm"  />
                <SettingsText CommandUpdate="Save" PopupEditFormCaption="Show Location" />
                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" 
					ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowTitlePanel="True" 
					ShowFilterBar="Visible" ShowHeaderFilterButton="True" />

                <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides"
                    EditForm-Height="700px" EditForm-HorizontalAlign="WindowCenter" 
					EditForm-Modal="True" EditForm-VerticalAlign="WindowCenter" 
					EditForm-Width="1100px" />

            	<Border BorderColor="Silver" />

            </dx:ASPxGridView>
            <asp:SqlDataSource ID="Sqlmembers" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand='SELECT
get_name(member.Member_ID) ,Concat(member.Member_NECellAreaCode,member.Member_NECellPhoneFirst,member.Member_NECellPhoneLast) id 

FROM
member
WHERE
member.Member_NECellAreaCode and member.member_status = &#039;Active&#039; 
order by get_name(member.member_id)
'>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="server=72.14.168.138;User Id=root;password=NEgps@13;Persist Security Info=True;database=test" 
		ProviderName="MySql.Data.MySqlClient" 
		
		SelectCommand="SELECT locationid_id, latitude_db, longitude_db, timestamp_dt, imei_ds, right(phonenumber_ds,10) phone, softwareversion_ds, simserial_ds, subscribeid_ds, androidid_ds, battery_db FROM ne_location
where phonenumber_ds &lt;&gt; '' and phonenumber_ds is not null">
	</asp:SqlDataSource>
            </asp:SqlDataSource>
            
    <br />




</asp:Content>

<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style2
		{
			background-color: #FF0000;
		}
	</style>
</asp:Content>


