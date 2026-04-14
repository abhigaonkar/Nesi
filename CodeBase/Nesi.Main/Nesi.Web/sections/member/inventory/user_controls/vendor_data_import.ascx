<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_inventory_user_controls_vendor_data_import" Codebehind="vendor_data_import.ascx.cs" %>
<%@ register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<div id="vendor_data_import_container">
	<div class="import">
		This tool allows you to mass import vendor data into the database.<br />
		You will need to follow these guidelines to successfully import:<br />
		<ul>
			<li>The file will need to be in a CSV format (Comma Seperated Values)</li>
			<li>The file cannot contain any column names</li>
			<li>The only columns that can be supplied are (In this order <u>only</u>):<ul>
				<li>Master ID</li>
				<li>Vendor ID</li>
				<li>Vendor Part #</li>
				<li>Vendor Package Price</li>
				<li>Vendor Quantity Per</li>
				</ul>
			</li>
			<li>Master ID can only be numbers and a valid part #</li>
			<li>Vendor ID can only be numbers (Vendor IDs can be found by just performing a search in the vendor module, the first column will be ID)</li>
			<li>Vendor Part # can be any characters except commas</li>
			<li>Vendor Package Price - The entire price of the package, not the per unit price.</li>
			<li>Vendor Quantity Per - (i.e. If there are 100 screws in a box, the quantity per is 100.)</li>
		</ul>
		<dx:aspxuploadcontrol id="up_file" runat="server" uploadmode="Auto" clientinstancename="up_file_vdi" width="280px" fileuploadmode="OnPageLoad" showprogresspanel="True" onfileuploadcomplete="up_file_OnFileUploadComplete">
			<ValidationSettings allowedfileextensions=".csv"></ValidationSettings>
			<ClientSideEvents fileuploadcomplete="
			function(s,e)
				{
				if(e.callbackData.match(/\|/g))
					{
					var results		= e.callbackData.split('|');
					var r			= results[0];
					var p			= results[1];
					up_cbp.PerformCallback(r+'|'+p);
					}
				}"></ClientSideEvents>
		</dx:aspxuploadcontrol>
		<dx:aspxbutton runat="server" id="bt_upload" text="Upload File" autopostback="False">
			<ClientSideEvents click="function(s,e){up_file_vdi.Upload();}"></ClientSideEvents>
		</dx:aspxbutton>
	</div>
	<div class="results">
		<dx:aspxcallbackpanel id="up_cbp" runat="server" clientinstancename="up_cbp" width="100%" oncallback="up_cbp_OnCallback">
			<PanelCollection>
				<dx:PanelContent runat="server">
					<dx:ASPxLabel runat="server" id="lb_error" clientvisible="False" encodehtml="false" />
					<dx:aspxgridview runat="server" id="gv_results" autogeneratecolumns="False" width="100%" keyfieldname="id" theme="NETheme01" clientvisible="False" ondatabound="gv_results_OnDataBound">
						<columns>
							<dx:gridviewdatatextcolumn caption="Exists?" fieldname="exists" showincustomizationform="True" visibleindex="0" width="35px">
									<settings autofiltercondition="Contains" />
									<CellStyle horizontalalign="Center"/>
									<HeaderStyle horizontalalign="Center"/>
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Master ID" fieldname="master_id" showincustomizationform="True" visibleindex="1" width="50px">
									<settings autofiltercondition="Contains" />
									<CellStyle horizontalalign="Center"/>
									<HeaderStyle horizontalalign="Center"/>
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Vendor" fieldname="vendor_name" showincustomizationform="True" visibleindex="2">
									<settings autofiltercondition="Contains" />
									<CellStyle horizontalalign="Left"/>
									<HeaderStyle horizontalalign="Center"/>
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Vendor Part #" fieldname="vendor_code" showincustomizationform="True" visibleindex="3" width="150px">
									<settings autofiltercondition="Contains" />
									<CellStyle horizontalalign="Center"/>
									<HeaderStyle horizontalalign="Center"/>
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Package Cost" fieldname="total_cost" showincustomizationform="True" visibleindex="4" width="40px">
									<settings autofiltercondition="Contains" />
									<CellStyle horizontalalign="Center"/>
									<PropertiesTextEdit displayformatstring="{0:C3}"/>
									<HeaderStyle horizontalalign="Center"/>
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Package Qty" fieldname="qty_per" showincustomizationform="True" visibleindex="5" width="40px">
									<settings autofiltercondition="Contains" />
									<CellStyle horizontalalign="Center"/>
									<PropertiesTextEdit displayformatstring="{0:N2}"/>
									<HeaderStyle horizontalalign="Center"/>
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Per Unit Cost" fieldname="per_unit_cost" showincustomizationform="True" visibleindex="6" width="40px">
									<settings autofiltercondition="Contains" />
									<CellStyle horizontalalign="Center"/>
									<PropertiesTextEdit displayformatstring="{0:C3}"/>
									<HeaderStyle horizontalalign="Center"/>
							</dx:gridviewdatatextcolumn>
						</columns>
						<settingspager mode="ShowAllRecords">
						</settingspager>
                        <SettingsDetail ShowDetailRow="True" />
						
						<settings showfilterrow="True" />
						<settingsdatasecurity allowdelete="False" allowedit="False" allowinsert="False" />
						<Templates>
							<DetailRow>
								<div style="padding-left: 20px;"><%# Eval("description") %></div>
 							</DetailRow>
						</Templates>
					</dx:aspxgridview>
					<div style="text-align: center">
					<dx:aspxbutton runat="server" id="bt_import" clientvisible="False" autopostback="False" text="Looks good - Go ahead and process.">
						<ClientSideEvents click="function(s,e){if(confirm('One last time, are you sure you want to import these?')){s.SetEnabled(false);up_cbp.PerformCallback('PROCESS');}}"></ClientSideEvents>
					</dx:aspxbutton>
						
					</div>
				</dx:PanelContent>
			</PanelCollection>
		</dx:aspxcallbackpanel>
	</div>

</div>