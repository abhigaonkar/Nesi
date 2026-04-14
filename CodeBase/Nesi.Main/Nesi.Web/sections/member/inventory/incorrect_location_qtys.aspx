<%@ Page Language="C#" AutoEventWireup="true"   Inherits="sections_member_inventory_incorrect_location_qtys" Theme="NETheme01" EnableTheming="True" MasterPageFile="../../../IntraDefault.master" Title="Incorrect Location Qtys" Codebehind="incorrect_location_qtys.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterLeft" runat="Server">
    <div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterMenu" runat="Server">
    <div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterBody" runat="server">
    
	
	<script type="text/javascript">
	   
	  
      

	</script>
      <div>
	

          <br />
          <br />
          <span class="auto-style1">** You can only edit these if the &#39;inventory qty adjustment&#39; is OPEN for your branch and you have the inventory admin </span><span class="auto-style1" style="font-size: 9.0pt; line-height: 115%; mso-fareast-font-family: Calibri; mso-fareast-theme-font: minor-latin; mso-ansi-language: EN-US; mso-fareast-language: EN-US; mso-bidi-language: AR-SA">privilege</span><br />
	

        <uc1:layout_control ID="layout" runat="server" GridviewID="gv" />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
            SelectCommand="SELECT
inventory_incorrect_levels.id,
inventory_incorrect_levels.location_id,
inventory_incorrect_levels.member_id AS EnteredBy_id,
EnteredBy.member_fullname AS EnteredBy_name,
inventory_incorrect_levels.date AS date_entered,
inventory_incorrect_levels.cleared_date,
inventory_incorrect_levels.cleared_by AS ClearedBy_id,
ClearedBy.member_fullname AS ClearedBy_name,
inventory_incorrect_levels.reported_qty,
inventory_incorrect_levels.notes,
inventory_incorrect_levels.master_id,
Location.`name` AS location_name,
inventory_incorrect_levels.incorrect_qty,
inventory_description.description,
            inventory_location.qty QtyinStock
FROM
inventory_incorrect_levels
LEFT JOIN member AS EnteredBy ON EnteredBy.Member_ID = inventory_incorrect_levels.member_id
LEFT JOIN member AS ClearedBy ON ClearedBy.Member_ID = inventory_incorrect_levels.cleared_by
INNER JOIN inventory_location_master AS Location ON inventory_incorrect_levels.location_id = Location.id
INNER JOIN inventory_description ON inventory_incorrect_levels.master_id = inventory_description.master_id 
            INNER JOIN inventory_location ON Location.id = inventory_location.location_master_id AND inventory_incorrect_levels.master_id = inventory_location.master_id 
where Location.business_unit_id = @cid order by inventory_incorrect_levels.id desc ">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdn_company" Name="cid" PropertyName="Value" />
            </SelectParameters>
          </asp:SqlDataSource>
        <asp:HiddenField ID="hdn_company" runat="server" />
        <dx:ASPxGridView ID="gv" runat="server" Width="100%" ClientInstanceName="gv" DataSourceID="SqlDataSource1" OnCustomCallback="gv_CustomCallback" OnCustomJSProperties="gv_CustomJSProperties" AutoGenerateColumns="False" KeyFieldName="id" OnCustomButtonCallback="gv_CustomButtonCallback" OnCustomButtonInitialize="gv_CustomButtonInitialize">
            <ClientSideEvents CustomButtonClick="function(s, e) {
	if (confirm('Are you sure you want to override this Qty?'))
{
e.processOnServer=true;
}
                else
                {
                e.processOnServer=false;
                }
}" EndCallback="function(s, e) {
	if (s.cp_alert!=null &amp;&amp; s.cp_alert!='')
{
alert(s.cp_alert);
s.cp_alert = '';
}
 please_wait('stop');            
}" />
            <Columns>
                <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" VisibleIndex="1">
                    <EditFormSettings Visible="False"/>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="location_id" Visible="False" VisibleIndex="2">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EnteredBy_id" Visible="False" VisibleIndex="3">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Reported By" FieldName="EnteredBy_name" VisibleIndex="10">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="Date" FieldName="date_entered" VisibleIndex="4" SortIndex="0" SortOrder="Ascending">
                    <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn Caption="Cleared Date" FieldName="cleared_date" VisibleIndex="11">
                    <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm">
                    </PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Center">
                    </CellStyle>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="ClearedBy_id" Visible="False" VisibleIndex="12">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Cleared By" FieldName="ClearedBy_name" VisibleIndex="13">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Reported Qty" FieldName="reported_qty" VisibleIndex="9">
                    <CellStyle HorizontalAlign="Center">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" VisibleIndex="14">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Part No" FieldName="master_id" VisibleIndex="6">
                    <DataItemTemplate>
                    <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
								NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/member/inventory/index.aspx?a=get&tab=PL&id={0}', 'inventory', 1200,800)&quot;, Eval(&quot;master_id&quot;)) %>" 
								Text='<%# Eval("master_id") %>'>
							</dx:ASPxHyperLink>
                        </DataItemTemplate>
                     <CellStyle HorizontalAlign="Center">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Location" FieldName="location_name" VisibleIndex="5">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="System Qty" FieldName="QtyinStock" VisibleIndex="8">
                    <DataItemTemplate>
                        <dx:ASPxTextBox ID="tb_qty" runat="server" onkeydown="only_numeric(event)" HorizontalAlign="Center" Text='<%# Eval("QtyinStock") %>' Width="60px">
                        </dx:ASPxTextBox>
                    </DataItemTemplate>
                    <CellStyle HorizontalAlign="Center">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Description" FieldName="description" VisibleIndex="7">
                </dx:GridViewDataTextColumn>
                <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0">
                    <CustomButtons>
                        <dx:GridViewCommandColumnCustomButton ID="approve">
                            <Image Url="~/images/icon/icon[save].gif">
                            </Image>
                        </dx:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dx:GridViewCommandColumn>
            </Columns>
            <Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
            <SettingsSearchPanel Visible="True" />
        </dx:ASPxGridView>
        <br />
	

</div>
</asp:Content>
<asp:Content ID="Content2" runat="server" 
	contentplaceholderid="header_placeholder">
    <style type="text/css">
		.focused_row{
           background-color:orange !important;
		}
	    .auto-style1 {
            font-family: Calibri;
        }
	</style>


  

</asp:Content>

