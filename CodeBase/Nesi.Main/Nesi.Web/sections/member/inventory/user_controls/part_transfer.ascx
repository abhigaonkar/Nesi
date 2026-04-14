<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_inventory_user_controls_part_transfer" EnableTheming="True" Codebehind="part_transfer.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


			<%@ Register src="../../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>

 <script type="text/javascript">

   

 	function GridRowAction(rowVisibleIndex) {
 		name = "qty_" + rowVisibleIndex;
 		txt_qty = eval(name);

 		name_to = "to_" + rowVisibleIndex;
 		txt_to = eval(name_to);
 		if (txt_qty.GetText() != "" && txt_to.GetValue() != "") {

 		    gv_transfer.PerformCallback("move|" + txt_qty.GetText() + "|" + txt_to.GetValue() + "|" + rowVisibleIndex);
 		}

 	}

 	function move_all(_index)
 	{
 	  
 	    if (_index==-1)
 	    {
 	        var buildup = "";
 	        for (var index = gv_transfer.GetTopVisibleIndex(); index < gv_transfer.GetVisibleRowsOnPage(); index++)
 	        {
 	            name = "qty_" + index;
 	            txt_qty = eval(name);
 	            name_to = "to_" + index;
 	            txt_to = eval(name_to);
 	            if (txt_qty.GetText() != "" && txt_to.GetValue() != "") {
 	                buildup += "move|" + txt_qty.GetText() + "|" + txt_to.GetValue() + "|" + index + "~";
 	            }
 	        }
 	        if (buildup != "") {
 	            gv_transfer.PerformCallback(buildup);
 	        }
 	    }
 	    else
 	    {
 	        GridRowAction(_index)
 	    }
  //gv_transfer.PerformCallback("refresh");
 	//    gv_transfer.Refresh();
 	 //   gv_transfer.PerformCallback("refresh");
 	
 	  
 	}

 	function reset_max()
 	{
 	    if (confirm("Are you sure you want to reset all these location max qty settings to 0?"))
 	    {
 	        gv_transfer.PerformCallback("set_max_to_zero");
 	    }

 	}
 	function reset_min() {
 	    if (confirm('Are you sure you want to reset all these location min qty settings to 0?')) {
 	       
 	        gv_transfer.PerformCallback("set_min_to_zero");
 	    }

 	}
    </script>

<div style="padding: 20px">
			
	<dx:ASPxCallbackPanel ID="pnl_transfer" runat="server" 
		ClientInstanceName="pnl_transfer" Width="100%">
		<PanelCollection>
<dx:PanelContent runat="server">
	<uc1:layout_control ID="layout" runat="server" GridviewID="gv_transfer" ShowExcelExport="True" />
	<br />
	<dx:ASPxGridView ID="gv_transfer" runat="server" AutoGenerateColumns="False" 
		ClientInstanceName="gv_transfer"  KeyFieldName="id" 
		OnCustomCallback="gv_transfer_CustomCallback" 
		OnCustomJSProperties="gv_transfer_CustomJSProperties" Theme="NETheme01" 
		Width="100%">
		<ClientSideEvents
            
             BeginCallback="function(s, e) {
                       
	        pnl_transfer.SetEnabled(false);
            please_wait('start');
  

}"
            
             EndCallback="function(s, e) {

if( (s.cp_alert!=null)&amp;&amp;(s.cp_alert!=''))
{
alert(s.cp_alert);
s.cp_alert='';
}

if ((s.cp_refresh!=null)&amp;&amp;(s.cp_refresh='1'))
{
//gv_transfer.Refresh();
s.cp_refresh = '';
            
            
}
pnl_transfer.SetEnabled(true);
please_wait('stop');

            	
}" />
		<Columns>
			<dx:GridViewCommandColumn Caption=" " ShowClearFilterButton="True" 
				ShowInCustomizationForm="True" VisibleIndex="0" Width="30px">
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn FieldName="id" ShowInCustomizationForm="True" 
				Visible="False" VisibleIndex="1">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="master_id" ReadOnly="True" 
				ShowInCustomizationForm="True" VisibleIndex="2" Width="70px">
				<Settings AutoFilterCondition="Equals" />
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="description" 
				ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
				<Settings AutoFilterCondition="Contains" />
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="tag" ShowInCustomizationForm="True" 
				Visible="False" VisibleIndex="4">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="From Location" FieldName="location" 
				ShowInCustomizationForm="True" VisibleIndex="5" Width="200px">
				<Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="location_master_id" 
				ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Min Qty" FieldName="min_qty" ShowInCustomizationForm="True" 
				VisibleIndex="7" Width="70px">
				<Settings AutoFilterCondition="LessOrEqual" />
			    <FooterTemplate>
                    <dx:ASPxButton ID="btn_reset_min" runat="server" AutoPostBack="True" ClientInstanceName="btn_reset_min" Text="Resest to 0" Theme="NETheme01" UseSubmitBehavior="False">
                        <ClientSideEvents Click="function(s, e) {
                           reset_min();
}" />
                    </dx:ASPxButton>
                </FooterTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Max Qty" Name="_max_qty" FieldName="max_qty" ShowInCustomizationForm="True" 
				VisibleIndex="8" Width="70px">
				
			    <FooterTemplate>
                    <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Reset to 0" Theme="NETheme01" UseSubmitBehavior="False">
                        <ClientSideEvents Click="function(s, e) {
                           reset_max();

}" />
                    </dx:ASPxButton>
                </FooterTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Qty at Origin" FieldName="qty" 
				ShowInCustomizationForm="True" VisibleIndex="9" Width="100px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="dollar_balance" 
				ShowInCustomizationForm="True" Visible="False" VisibleIndex="10">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="intext" ShowInCustomizationForm="True" 
				Visible="False" VisibleIndex="11">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="To Location" Name="_to" 
				ShowInCustomizationForm="True" VisibleIndex="12" Width="190px">
				<DataItemTemplate>
					<dx:ASPxComboBox ID="cb_to" runat="server" DataSourceID="sql_locations" 
						EnableCallbackMode="True" oninit="cb_to_Init" TextField="name" 
						Theme="NETheme01" ValueField="id" ValueType="System.Int32" Width="100%">
					</dx:ASPxComboBox>
				</DataItemTemplate>
				<HeaderCaptionTemplate>
					<table style="width:100%;">
						<tr>
							<td>
								<dx:ASPxComboBox ID="ddl_to" runat="server" ClientInstanceName="ddl_to" 
									DataSourceID="sql_locations" oninit="ddl_to_Init" TextField="name" 
									Theme="NETheme01" ValueField="id" ValueType="System.Int32" Width="170px">
								</dx:ASPxComboBox>
							</td>
						</tr>
						<tr>
							<td align="right">
								<dx:ASPxButton ID="btn_set_locations" runat="server" AutoPostBack="False" 
									HorizontalAlign="Right" Text="Set All (this page) -&gt;" Theme="NETheme01">
									<ClientSideEvents Click="function(s, e) {
	gv_transfer.PerformCallback('T|'+ddl_to.GetValue());

	//set_ddls(ddl_to.GetValue());
                                     
}" />
								</dx:ASPxButton>
							</td>
						</tr>
					</table>
				</HeaderCaptionTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Qty to Move" Name="qty_to_move" 
				ShowInCustomizationForm="True" VisibleIndex="13" Width="150px">
				<DataItemTemplate>
					<table style="width:100%;">
						<tr>
							<td>
								<dx:ASPxTextBox ID="txt_qty_to_move" runat="server" 
									oninit="txt_qty_to_move_Init" Theme="NETheme01" Width="80px">
									<ValidationSettings CausesValidation="True" Display="Dynamic" 
										ErrorDisplayMode="ImageWithTooltip">
										<RegularExpression ErrorText="Must be a valid number" 
											ValidationExpression="^([+]|-)?(([0-9]+[.]?[0-9]*)|([0-9]*[.]?[0-9]+))$" />
									</ValidationSettings>
								</dx:ASPxTextBox>
							</td>
							<td>
								<dx:ASPxButton ID="btn_move_one" runat="server" AutoPostBack="False" 
									ClientInstanceName="btn_move_one" oninit="btn_move_one_Init" Text="Move">
								</dx:ASPxButton>
							</td>
						</tr>
					</table>
				</DataItemTemplate>
				<FooterTemplate>
					<dx:ASPxButton ID="btn_move_all" runat="server" AutoPostBack="False" 
						ClientInstanceName="btn_move_all" Text="Move All Selected">
						<ClientSideEvents Click="function(s, e) {
									//gv_transfer.SetEnable(false);
	//gv_transfer.PerformCallback('ALL');
                            move_all(-1);
}" />
					</dx:ASPxButton>
				</FooterTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="loc_master_id" 
				ShowInCustomizationForm="True" Visible="False" VisibleIndex="14">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<SettingsPager PageSize="100">
		</SettingsPager>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" 
			ShowHeaderFilterButton="True" />
		<SettingsLoadingPanel Delay="0" ImagePosition="Top" />
		<SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
			AllowInsert="False" />
	    <Styles>
            <LoadingPanel VerticalAlign="Top">
            </LoadingPanel>
        </Styles>
	</dx:ASPxGridView>
			</dx:PanelContent>
</PanelCollection>
	</dx:ASPxCallbackPanel>
	<br />
					



			

 <dx:ASPxHiddenField runat="server" ClientInstanceName="h" ID="h"></dx:ASPxHiddenField>

</div>
<asp:SqlDataSource ID="sql_locations" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
a.id,
CONCAT(IF(a.type_id = 1, 'Inl - ', 'Exl - '), a.name) name
FROM inventory_location_master a WHERE a.business_unit_id = ?warehouse_bu_id ORDER BY name">
	<SelectParameters>
		<asp:SessionParameter Name="?warehouse_bu_id" SessionField="working_warehouse_bu_id"/>
	</SelectParameters>
</asp:SqlDataSource>

