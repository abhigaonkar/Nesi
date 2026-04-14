<%@ control language="C#" autoeventwireup="true" inherits="mobile_modules_part_management2" Codebehind="part_management2.ascx.cs" %>
<%@ register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1' name='viewport' />
<link rel="stylesheet" href="/mobile/css/part_management.css" />
<script type="text/javascript" src="/mobile/js/part_management.js"></script>
<asp:updatepanel id="up" runat="server">
	<contenttemplate>
		<div id="to_top" onclick="scroll_to_top();"><span class="text">To Top</span></div>
		<div id="modal_overlay" style="display: none; z-index: 999; background-image: url('/images/loading_panel.gif'); background-repeat: no-repeat; background-position: center center; width: 100%; height: 100%; position: absolute; left: 0px; top: 0px; background-color: transparent;"></div>
		<div style="font-family: Arial; white-space: normal;" align="center">

			<table id="tbl_new" runat="server" cellspacing="0"
				style="background-color: whitesmoke;" width="100%">
				<tr>
					<td width="100%">
						<asp:hiddenfield id="hdn_member_id" runat="server" />
						<asp:hiddenfield id="hdn_view_id" runat="server" />
						<dx:aspxcallback id="cb_general" runat="server" clientinstancename="cb_general" oncallback="cb_general_Callback">
						</dx:aspxcallback>

						<div>

							<dx:aspxcombobox id="ddl_company" height="35px" runat="server" valuetype="System.Int32" width="75%" border-borderstyle="None"
								nulltext="Select Business Unit" font-size="14pt" enablecallbackmode="True" datasourceid="Sqlcompany"
								textfield="ddl_name" valuefield="id" animationtype="None" clientinstancename="ddl_company" ClientVisible="False"
								autopostback="True" onselectedindexchanged="ddl_company_SelectedIndexChanged">
								
								<clearbutton visibility="False">
								</clearbutton>
								<buttons>
									<dx:editbutton text="X"></dx:editbutton>
								</buttons>
								<clientsideevents buttonclick="function(s, e) {
		s.SetValue(null);
		set_tabs();
	}" />
								<border borderstyle="None" />
							</dx:aspxcombobox>
							<asp:sqldatasource id="Sqlcompany" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
								selectcommand="call get_visible_business_units(?m_id)">
								<selectparameters>

									<asp:controlparameter controlid="hdn_member_id" name="m_id" propertyname="Value" />
								</selectparameters>
							</asp:sqldatasource>
						</div>

						<dx:aspxcombobox id="ddl_wo" runat="server" datasourceid="sql_wos" textfield="_desc"
							valuefield="id" valuetype="System.Int32" width="75%"
							font-size="14pt"
							incrementalfilteringdelay="200" nulltext="Select Work Order" height="35px" clientinstancename="ddl_wo" dropdownwidth="75%"
							textformatstring="{1}" enablecallbackmode="True" dropdownheight="250px" OnSelectedIndexChanged="ddl_wo_OnSelectedIndexChanged" AutoPostBack="True">

							<columns>
								<dx:listboxcolumn fieldname="id" caption=" " width="25%" />
								<dx:listboxcolumn fieldname="_desc"  caption=" " width="75%"/>
							</columns>
							<buttons>
								<dx:editbutton text="X"></dx:editbutton>
							</buttons>
							<itemstyle height="35px" wrap="True" paddings-paddingtop="5" verticalalign="Top">
								<paddings padding="3px" />
								<border borderstyle="None" />

							</itemstyle>
							<clientsideevents buttonclick="function(s, e) {
		s.SetValue(null);
		set_tabs();
		cb_general.PerformCallback('');
		
	 
}"/>
							
							<clearbutton visibility="False">
							</clearbutton>
							<border borderstyle="None" />
						</dx:aspxcombobox>
						<asp:sqldatasource id="sql_wos" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
							selectcommand="select CAST(woprog_id AS SIGNED) id, concat(trim(leading'0' from woprog_bvwo),' ',woprog_customername,'-',woprog_description) _desc from woprog a LEFT JOIN business_unit b ON a.business_unit_id = b.id 
								where business_unit_id = ?bu_id and woprog_status = 'Open' and WOProg_Hold=0 and labor_only = false AND woprog_associate_woprog_id = 0 AND a.allow_mobile_part_management = 1 
								order by woprog_id desc">
							<selectparameters>
								<asp:SessionParameter SessionField="working_business_unit_id" name="bu_id" />
							</selectparameters>
						</asp:sqldatasource>
					</td>

				</tr>
				<tr>
					<td>
						<dx:ASPxComboBox ID="ddl_trucks" Height="35px" runat="server" NullText="Select Location/Truck" ValueType="System.Int32" Width="75%" Border-BorderStyle="None" Font-Size="14pt" EnableCallbackMode="True" DataSourceID="Sql_trucks" TextField="name" ValueField="id" AnimationType="None" AutoPostBack="True" OnSelectedIndexChanged="ddl_trucks_SelectedIndexChanged" ClientInstanceName="ddl_trucks">
							<buttons>
								<dx:editbutton text="X"></dx:editbutton>
							</buttons>
							<clientsideevents buttonclick="function(s, e) {
		s.SetValue(null);
		set_tabs();
	}" selectedindexchanged="function(s, e) {
set_tabs();
	
}" />
							<clearbutton visibility="False">
							</clearbutton>
							<border borderstyle="None" />
						</dx:aspxcombobox>
						<asp:sqldatasource id="Sql_trucks" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>"
							providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
							selectcommand="Select id,name from inventory_location_master where if(?_type=7,(type_id=1 or type_id=2),type_id=2) and business_unit_id = ?bu_id">
							<selectparameters>
								<asp:SessionParameter SessionField="warehouse_business_unit_id" name="bu_id" />
								<asp:controlparameter controlid="hdn_view_id" name="_type" propertyname="Value" />
							</selectparameters>
						</asp:sqldatasource>



					</td>
				</tr>
				<tr>
					<td>
						<dx:aspxcombobox id="ddl_other_wo" runat="server" datasourceid="sql_other_wos" textfield="_desc"
							valuefield="id" valuetype="System.Int32" width="75%" Enabled="True"
							enablecallbackmode="True" font-size="14pt" animationtype="None" callbackpagesize="10" ClientVisible="False"
							incrementalfilteringdelay="200" nulltext="Select Other Work Order" height="35px" clientinstancename="ddl_other_wo" dropdownwidth="75%"
							autopostback="true" onselectedindexchanged="ddl_trucks_SelectedIndexChanged" textformatstring="{1}">

							<columns>
								<dx:listboxcolumn fieldname="id" caption=" " width="25%" />
								<dx:listboxcolumn fieldname="_desc" width="75%" caption=" " />
							</columns>
							<buttons>
								<dx:editbutton text="X"></dx:editbutton>
							</buttons>
							<clientsideevents buttonclick="function(s, e) {
		s.SetValue(null);
		set_tabs();
	}" />
							<itemstyle height="35px" wrap="True" paddings-paddingtop="5" verticalalign="Top">
								<paddings padding="3px" />
								<border borderstyle="None" />
							</itemstyle>

							
							<clearbutton visibility="False">
							</clearbutton>
							<border borderstyle="None" />
						</dx:aspxcombobox>
						<asp:sqldatasource id="sql_other_wos" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
							selectcommand="SELECT 
	a.woprog_id id, 
	CAST(CONCAT('0', a.woprog_bvwo/1, ' ',a.woprog_customername, '-', a.woprog_description, '-', a.woprog_status) AS CHAR) _desc 
FROM 
	woprog a
LEFT JOIN
	business_unit b ON a.business_unit_id = b.id
WHERE 
	a.business_unit_id = ?bu_id AND 
	a.woprog_status NOT IN ('Invoiced','Waiting To Be Invoiced', 'Waiting PM Approval', 'Waiting BM Approval','Questions For PM','Deleted','Waiting For PO','Waiting Parent BM Approval') AND
	a.woprog_bvwo != 'Not Entered' AND 
	a.woprog_associate_woprog_id = 0 AND 
	a.woprog_hold = 0 AND
	a.woprog_iscredit = 0 AND
	a.woprog_isrebill = 0 AND
	a.woprog_id != ?wo_id AND 
	a.allow_mobile_part_management = 1
ORDER BY 
	a.woprog_customername,
	a.woprog_bvwo">
						<selectparameters>
									<asp:Sessionparameter SessionField="working_woprog_id" name="wo_id" />
									<asp:SessionParameter SessionField="warehouse_business_unit_id" name="bu_id" />
								</selectparameters>
						</asp:sqldatasource>
						<div>
							<dx:aspxcombobox id="ddl_other_trucks" height="35px" runat="server" valuetype="System.Int32" width="75%" border-borderstyle="None"
								nulltext="Select Other Truck" font-size="14pt" enablecallbackmode="True" datasourceid="Sqlothertrucks"
								textfield="name" valuefield="id" animationtype="None" clientinstancename="ddl_other_trucks"  ClientVisible="False"
								autopostback="True" onselectedindexchanged="ddl_trucks_SelectedIndexChanged">
								
								<clearbutton visibility="False">
								</clearbutton>
								<buttons>
									<dx:editbutton text="X"></dx:editbutton>
								</buttons>
								<clientsideevents buttonclick="function(s, e) {
		s.SetValue(null);
		set_tabs();
	}" />
								<border borderstyle="None" />
							</dx:aspxcombobox>
							<asp:sqldatasource id="Sqlothertrucks" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
								selectcommand="Select id,name from inventory_location_master where type_id=2 and business_unit_id = ?bu_id and id !=?otherid">
								<selectparameters>
									<asp:controlparameter controlid="ddl_trucks" name="otherid" propertyname="Value" />
									<asp:SessionParameter SessionField="warehouse_business_unit_id" name="bu_id" />
								</selectparameters>
							</asp:sqldatasource>
						</div>
					</td>
				</tr>

			</table>
		</div>
		<div id="top_tab_bar" align="center" style="overflow-x: auto;" width="150%">
			<div id="div_error" runat="server">
				<asp:label id="xfer_lb_notice" runat="server" font-bold="True"
					font-size="11px" forecolor="Black" width="375px"></asp:label><br />
				<asp:label id="xfer_lb_warning" runat="server" font-bold="True"
					font-size="11px" forecolor="Red" width="375px"></asp:label>
			</div>
			<div id="tab_bar" >
				<asp:Button ID="tab_wo" runat="server" OnClick="tab_wo_Click" Text="WO" UseSubmitBehavior="False" CssClass="button" BorderStyle="None" />
				<asp:Button ID="tab_status" runat="server" Text="Status" UseSubmitBehavior="False" CssClass="button"  BorderStyle="None" OnClick="tab_status_OnClick" />
				<asp:Button ID="tab_truck" runat="server" OnClick="tab_truck_Click" Text="Truck" UseSubmitBehavior="False" CssClass="button"  BorderStyle="None" />
				<asp:Button ID="tab_return" runat="server" OnClick="tab_return_Click" Text="Return" UseSubmitBehavior="False" CssClass="button" BorderStyle="None" />
				<asp:Button ID="tab_pull" runat="server" OnClick="tab_pull_Click" Text="Pull" UseSubmitBehavior="False" CssClass="button" BorderStyle="None" />
				<asp:Button ID="tab_history" runat="server" OnClick="tab_history_Click" Text="History" UseSubmitBehavior="False" CssClass="button" BorderStyle="None" />
				<asp:Button ID="tab_search" runat="server" OnClick="tab_search_Click" Text="Search" UseSubmitBehavior="False" CssClass="button"  BorderStyle="None" />
				<asp:Button ID="tab_incorrect" runat="server" OnClick="tab_incorrect_Click" Text="Corrections" UseSubmitBehavior="False" CssClass="button" BorderStyle="None" />
				<asp:button id="tab_annual" runat="server" onclick="tab_annual_Click" text="Annual Counts" usesubmitbehavior="False" cssclass="button"  borderstyle="None"/>
			</div>
		</div>
		<asp:multiview id="mv" runat="server" activeviewindex="0" onactiveviewchanged="mv_ActiveViewChanged">
			<asp:view id="vw_wo" runat="server">
				<asp:repeater id="rpt_wo" runat="server">
					<headertemplate>
						<table width="100%" id="rpt" cellpadding="2" cellspacing="0">
							<thead id="rpt_header" class="rpt_header">
								<tr>
									<th valign="middle">
										<asp:textbox id="tb_wo_search" runat="server" Placeholder="Search" borderstyle="None" class="truck_tb_search" height="25px" width="100px"></asp:textbox>
									</th>
									<th valign="middle">
										<asp:button id="btn_wo_search" runat="server"  onclick="btn_search_Click" font-size="8pt" style="white-space: normal" text="Search WO" width="50px" />
									</th>
									<th align="right" valign="middle">
										<asp:button id="btn_add_part" runat="server" backcolor="White" borderstyle="none" font-size="8pt" height="50px" onclick="btn_add_part_Click" style="white-space: normal" text="Add Part" width="50px" />
									</th>
									<th align="right" valign="middle">
										<asp:button id="btn_wo_save" runat="server" backcolor="White" borderstyle="none" font-size="8pt" height="50px" onclick="btn_wo_save_Click" style="white-space: normal" text="Apply Changes" width="50px" />
									</th>
								</tr>

							</thead>
							<tbody>
					</headertemplate>
					<itemtemplate>

						<td class="row" colspan="4">
							<table cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td colspan="4">
										<table width="100%">
											<tr>
												<td class="part">
													<div id="rpt_wo_master_id" runat="server" class="mid"><%# Eval("master_id") %></div>
												</td>
												<td class="top_right">Com Qty: <b><%# Eval("wo_qty") %></b></td>
											</tr>
											<tr>
												<td colspan="4" width="70%">
													<div class="desc" width="70%" onclick="pop_part('<%# Eval("master_id") %>');"><%# Eval("description") %></div>
												</td>

											</tr>
										</table>
									</td>

								</tr>
								<tr>
									<td class="tb_caption">Still Needed</td>
									<td class="tb_caption">Take from Stock</td>
									<td class="tb_caption"><%# Eval("truck_lbl") %></td>
									<td class="tb_caption"><%# Eval("o_wo_lbl") %></td>
								</tr>
								<tr>
									<td class="tr_wo" data-master_id='<%# Eval("master_id") %>' data-internal_qty='<%# Eval("internal_qty") %>' data-truck_qty='<%# Eval("truck_qty") %>' data-wo_qty='<%# Eval("wo_qty") %>'>
										<asp:textbox id="TextBox5" enabled='<%# Convert.ToInt32(Eval("lck")) == 1 %>' tooltip='<%# Eval("line_id") %>' runat="server" cssclass="repeater_tb" onkeydown="only_numeric(event)" text='<%# ((string)Eval("still_needed_qty")=="0"?"":(string)Eval("still_needed_qty")) %>' onkeyup="validate(this,'req');" textmode="Number"></asp:textbox>
									</td>
									<td class="tr_wo from_stock" data-master_id='<%# Eval("master_id") %>' data-internal_qty='<%# Eval("internal_qty") %>' data-truck_qty='<%# Eval("truck_qty") %>' data-wo_qty='<%# Eval("wo_qty") %>'>
										<input id="TextBox6" type="number" disabled='<%# Convert.ToInt32(Eval("lck")) == 0 %>' runat="server" class="repeater_tb" onkeydown="only_numeric(event)" onkeyup="validate(this,'stock_to_wo');"></input>
									</td>
									<td class="tr_wo from_truck" data-master_id='<%# Eval("master_id") %>' data-internal_qty='<%# Eval("internal_qty") %>' data-truck_qty='<%# Eval("truck_qty") %>' data-wo_qty='<%# Eval("wo_qty") %>'>
										<input id="TextBox7" type="number" disabled='<%# Convert.ToInt32(Eval("lck")) == 0 %>' runat="server" class="repeater_tb" onkeydown="only_numeric(event)" onkeyup="validate(this,'truck_to_wo');"></input>
									</td>
									<td class="tr_wo to_other" data-master_id='<%# Eval("master_id") %>' data-internal_qty='<%# Eval("internal_qty") %>' data-truck_qty='<%# Eval("truck_qty") %>' data-wo_qty='<%# Eval("wo_qty") %>'>
										<asp:textbox id="TextBox11" enabled='<%# Convert.ToInt32(Eval("lck")) == 1 %>' runat="server" cssclass="repeater_tb" onkeydown="only_numeric(event)" onkeyup="validate(this,'wo_to_wo');" textmode="Number" min="0"></asp:textbox>
									</td>
								</tr>
							</table>
						</td>
						</tr>

					</itemtemplate>
					<footertemplate>
						</tbody>
								  </table>
					</footertemplate>

				</asp:repeater>
				<iframe id="if_shoppingcart" class="if_shopping_cart" runat="server" height="380px" width="100%" style="border-style: none;"></iframe>

			</asp:view>
			<asp:view id="vw_truck" runat="server">
				<asp:repeater id="rpt_truck" runat="server">
					<headertemplate>
						<table width="100%" id="rpt" cellpadding="2" cellspacing="0">
							<thead id="rpt_header" class="rpt_header">
								<tr>
									<th valign="middle">
										<asp:textbox id="tb_truck_search" runat="server" borderstyle="None" height="25px" width="100px" class="truck_tb_search"></asp:textbox></th>
									<th valign="middle">
										<asp:button id="btn_truck_search" runat="server" onclick="btn_search_Click" font-size="8pt" style="white-space: normal" text="Search Truck" width="50px"  /></th>
									<th align="right" valign="middle">
										<asp:button id="btn_add_part_truck" runat="server" backcolor="White" borderstyle="none" font-size="8pt" height="50px" onclick="btn_add_part_truck_Click" style="white-space: normal" text="Add Part" width="50px" />
									</th>
									<th valign="middle" align="right">
										<asp:button id="btn_truck_save" runat="server" backcolor="White" font-size="8pt" height="50px" width="50px" style="white-space: normal" borderstyle="none" text="Apply Changes" onclick="btn_truck_save_Click" />
									</th>
								</tr>

							</thead>
							<tbody>
					</headertemplate>
					<itemtemplate>
						<tr>
							<td class="row" colspan="4">
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td colspan="3">
											<table width="100%">
												<tr>
													<td class="part">
														<div id="rpt_truck_master_id" runat="server" class="mid"><%# Eval("master_id") %></div>
													</td>
													<td class="top_right"><b>Qty on <%# Eval("truck_qty_lbl") %></b></td>
												</tr>
												<tr>
													<td colspan="2" width="100%">
														<div class="desc" width="70%" onclick="pop_part('<%# Eval("master_id") %>');"><%# Eval("description") %></div>
													</td>
												</tr>
											</table>
										</td>

									</tr>
									<tr>
										<td class="tb_caption">Take from Stock</td>
										<td class="tb_caption"><%# Eval("truck_lbl") %></td>
										<td class="tb_caption"><%# Eval("other_truck_lbl") %></td>
									</tr>
									<tr>
										<td class="tr_wo from_stock" data-master_id='<%# Eval("master_id") %>' data-internal_qty="<%# Eval("internal_qty") %>" data-truck_qty="<%# Eval("truck_qty") %>" data-wo_qty="<%# Eval("wo_qty") %>">
											<asp:textbox id="TextBox1" onkeydown="only_numeric(event)" onchange="validate(this,'stock_to_truck');" cssclass="repeater_tb" runat="server"></asp:textbox></td>
										<td class="tr_wo to_wo" data-master_id='<%# Eval("master_id") %>' data-internal_qty="<%# Eval("internal_qty") %>" data-truck_qty="<%# Eval("truck_qty") %>" data-wo_qty="<%# Eval("wo_qty") %>">
											<asp:textbox id="TextBox2" onkeydown="only_numeric(event)" onkeyup="validate(this,'truck_to_wo');" cssclass="repeater_tb" runat="server" textmode="Number"></asp:textbox></td>
										<td class="tr_wo to_other" data-master_id='<%# Eval("master_id") %>' data-internal_qty="<%# Eval("internal_qty") %>" data-truck_qty="<%# Eval("truck_qty") %>" data-wo_qty="<%# Eval("wo_qty") %>">
											<asp:textbox id="TextBox3" onkeydown="only_numeric(event)" onkeyup="validate(this,'truck_to_truck');" cssclass="repeater_tb" runat="server" textmode="Number"></asp:textbox></td>
									</tr>
								</table>
							</td>
						</tr>

					</itemtemplate>

					<footertemplate>
						</tbody>
				</table>
		
					</footertemplate>
				</asp:repeater>
				<iframe id="if_shoppingcart_truck" class="if_shopping_cart" runat="server" height="280px" width="100%" style="border-style: none"></iframe>
			</asp:view>

			<asp:view id="vw_return" runat="server">
				<asp:repeater id="rpt_return" runat="server">
					<headertemplate>
						<table width="100%" id="rpt" cellpadding="2" cellspacing="0">
							<thead id="rpt_header" class="rpt_header">
								<tr>
									<th valign="middle">
										<asp:textbox id="tb_return_search" runat="server" borderstyle="None" height="25px" width="100px" class="truck_tb_search"></asp:textbox></th>
									<th valign="middle">
										<asp:button id="btn_return_search" runat="server" onclick="btn_search_Click" style="white-space: normal" font-size="8pt" text="Search WO" width="50px"  /></th>
									<th valign="middle" align="right">
										<asp:button id="btn_return_save" runat="server" backcolor="White" font-size="8pt" height="50px" width="50px" style="white-space: normal" borderstyle="none" text="Apply Changes" onclick="btn_return_save_Click" />
									</th>
								</tr>

							</thead>
							<tbody>
					</headertemplate>
					<itemtemplate>
						<tr>
							<td class="row" colspan="3">
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td colspan="2">
											<table width="100%">
												<tr>
													<td class="part">
														<div id="rpt_return_master_id" runat="server" class="mid"><%# Eval("master_id") %></div>
													</td>
													<td class="top_right">WO Cmt Qty: <b><%# Eval("wo_qty") %></b></td>
												</tr>
												<tr>
													<td colspan="2" width="100%">
														<div class="desc" width="70%" onclick="pop_part('<%# Eval("master_id") %>');"><%# Eval("description") %></div>
													</td>
												</tr>
											</table>
										</td>

									</tr>
									<tr>
										<td class="tb_caption">Return To Stock</td>
										<td class="tb_caption">Return To Truck</td>
									</tr>
									<tr>
										<td class="tr_wo to_stock" data-master_id='<%# Eval("master_id") %>' data-internal_qty="<%# Eval("internal_qty") %>" data-truck_qty="<%# Eval("truck_qty") %>" data-wo_qty="<%# Eval("wo_qty") %>">
											<asp:textbox id="TextBox2" onkeyup="validate(this,'wo_to_stock');" cssclass="repeater_tb" runat="server" textmode="Number"></asp:textbox></td>
										<td class="tr_wo to_truck" data-master_id='<%# Eval("master_id") %>' data-internal_qty="<%# Eval("internal_qty") %>" data-truck_qty="<%# Eval("truck_qty") %>" data-wo_qty="<%# Eval("wo_qty") %>">
											<asp:textbox id="TextBox3" onkeyup="validate(this,'wo_to_truck');" cssclass="repeater_tb" runat="server" textmode="Number"></asp:textbox></td>
									</tr>
								</table>
							</td>
						</tr>
					</itemtemplate>

					<footertemplate>
						</tbody>
				</table>
		
					</footertemplate>
				</asp:repeater>
			</asp:view>
			<asp:view id="vw_pull" runat="server">
				<asp:repeater id="rpt_pull" runat="server">
					<headertemplate>
						<table width="100%" id="rpt" cellpadding="2" cellspacing="0">
							<thead id="rpt_header" class="rpt_header">
								<tr>
									<th valign="middle">
										<asp:textbox id="tb_pull_search" runat="server" borderstyle="None" height="25px" width="100px" class="truck_tb_search"></asp:textbox></th>
									<th valign="middle">
										<asp:button id="btn_pull_search" runat="server" onclick="btn_search_Click" style="white-space: normal" font-size="8pt" text="Search WO" width="50px"  /></th>
									<th valign="middle" align="right">
										<asp:button id="btn_pull_clear" runat="server" backcolor="White" font-size="8pt" height="50px" width="50px" style="white-space: normal" borderstyle="none" text="Clear Needed Qtys" onclick="btn_pull_clear_Click" />
									</th>
									<th valign="middle" align="right">
										<asp:button id="btn_pull_save" runat="server" backcolor="White" font-size="8pt" height="50px" width="50px" style="white-space: normal" borderstyle="none" text="Apply Changes" onclick="btn_pull_save_Click" />
									</th>
								</tr>

							</thead>
							<tbody>
					</headertemplate>
					<itemtemplate>
						<tr>
							<td class="row" colspan="4">
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td colspan="2">
											<table width="100%">
												<tr>
													<td class="part">
														<div id="rpt_pull_master_id" runat="server" class="mid"><%# Eval("master_id") %></div>
													</td>
													<td class="top_right">Needed Qty: <b><%# Eval("wo_qty") %></b></td>
												</tr>
												<tr>
													<td colspan="2" width="100%">
														<div class="desc" width="70%" onclick="pop_part('<%# Eval("master_id") %>');"><%# Eval("description") %></div>
													</td>
												</tr>
											</table>
										</td>


									</tr>
									<tr>
										<td class="tb_caption">Pull from Stock</td>
										<td class="tb_caption">Pull from Truck</td>
									</tr>
									<tr>
										<td class="tr_wo" data-master_id='<%# Eval("master_id") %>' data-internal_qty="<%# Eval("internal_qty") %>" data-truck_qty="<%# Eval("truck_qty") %>" data-wo_qty="<%# Eval("wo_qty") %>">
											<input type="number" id="TextBox2" onkeyup="validate(this,'stock_to_wo');" class="repeater_tb" runat="server" min="0"></input></td>
										<td class="tr_wo" data-master_id='<%# Eval("master_id") %>' data-internal_qty="<%# Eval("internal_qty") %>" data-truck_qty="<%# Eval("truck_qty") %>" data-wo_qty="<%# Eval("wo_qty") %>">
											<input type="number" id="TextBox4" onkeyup="validate(this,'truck_to_wo');" class="repeater_tb" runat="server" min="0"></input></td>
									</tr>
								</table>
							</td>
						</tr>

					</itemtemplate>

					<footertemplate>
						</tbody>
				</table>
		
					</footertemplate>
				</asp:repeater>
			</asp:view>
			<asp:view id="vw_correct" runat="server">
				<asp:repeater id="rpt_correct" runat="server">
					<headertemplate>
						<table width="100%" id="rpt" cellpadding="2" cellspacing="0">
							<thead id="rpt_header" class="rpt_header">
								<tr>
									<th valign="middle">
										<asp:textbox id="tb_correct_search" runat="server" borderstyle="None" height="25px" width="100px" class="truck_tb_search"></asp:textbox></th>
									<th valign="middle">
										<asp:button id="btn_correct_search" runat="server" onclick="btn_search_Click" style="white-space: normal" font-size="8pt" text="Search List" width="50px"  /></th>
									<th valign="middle" align="right">
										<asp:button id="btn_correct_save" runat="server" backcolor="White" font-size="8pt" height="50px" width="50px" style="white-space: normal" borderstyle="none" text="Apply Changes" onclick="btn_correct_save_Click" />
									</th>
								</tr>

							</thead>
							<tbody>
					</headertemplate>
					<itemtemplate>

						<tr>
							<td class="row" colspan="3">
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td width="60%">
											<table width="100%">
												<tr>
													<td class="part">
														<div id="rpt_correct_master_id" runat="server" class="mid"><%# Eval("master_id") %></div>
													</td>
												</tr>
												<tr>
													<td width="100%">
														<div class="desc" onclick="pop_part('<%# Eval("master_id") %>');"><%# Eval("description") %></div>
													</td>
												</tr>
											</table>
										</td>

										<td class="top_right" style="white-space: normal">At Location: <b><%# Eval("location_name") %></b></td>
									</tr>
									<tr>
										<td class="qty">System Qty: <b><%# Eval("internal_qty") %></b></td>
										<td>
											<table cellpadding="0" cellspacing="0" width="100%">
												<tr>
													<td class="tb_caption">Enter Correct Qty</td>
												</tr>
												<tr>
													<td class="tr_wo">
														<asp:hiddenfield id="hdn_ic_location_id" runat="server" value='<%# Eval("id") %>' />
														<asp:hiddenfield id="hdn_master_id" runat="server" value='<%# Eval("master_id") %>' />
														<asp:hiddenfield id="hdn_master_location_id" runat="server" value='<%# Eval("location_id") %>' />
														<asp:textbox id="TextBox2" cssclass="repeater_tb" runat="server" textmode="Number"></asp:textbox></td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</itemtemplate>


					<footertemplate>
						</tbody>
				</table>
		
					</footertemplate>
				</asp:repeater>
			</asp:view>
			<asp:view id="vw_history" runat="server">
				<asp:repeater id="rpt_history" runat="server">
					<headertemplate>
						<table width="100%" id="rpt" cellpadding="2" cellspacing="0">
							<thead id="rpt_header" class="rpt_header">
								<tr>
									<th valign="middle">
										<asp:textbox id="tb_history_search" runat="server" borderstyle="None" height="25px" width="100px" class="truck_tb_search"></asp:textbox></th>
									<th valign="middle"></th>
									<th valign="middle" align="right" colspan="1">
										<asp:button id="btn_history_search" runat="server" onclick="btn_search_Click" style="white-space: normal" font-size="10pt" text="Search History" width="50px"  />
									</th>
								</tr>
							</thead>
							<tbody>
					</headertemplate>
					<itemtemplate>
						<tr>
							<td class="row" colspan="3">
								<table width="100%">
									<tr>
										<td>
											<table width="50%">
												<tr>
													<td class="part">
														<div id="rpt_history_master_id" runat="server" class="mid"><%# Eval("master_id") %></div>
													</td>
												</tr>
												<tr>
													<td width="100%">
														<div class="desc" onclick="pop_part('<%# Eval("master_id") %>');"><%# Eval("description") %></div>
													</td>
												</tr>
											</table>
										</td>
										<td class="_date"><%# Eval("_date") %></td>
									</tr>

									<tr>
										<td class="_note" colspan="2"><%# Eval("note") %></td>
									</tr>
								</table>
							</td>
						</tr>
					</itemtemplate>

					<footertemplate>
						</tbody>
				</table>
		
					</footertemplate>
				</asp:repeater>
			</asp:view>
			<asp:view id="vw_po" runat="server">
				<asp:repeater id="rpt_po" runat="server">
					<headertemplate>
						<table id="rpt" cellpadding="2" cellspacing="0" width="100%">
							<thead id="rpt_header" class="rpt_header">
								<tr>
									<th valign="middle">
										<asp:textbox id="tb_po_search" runat="server" borderstyle="None" class="truck_tb_search" height="25px" width="100px"></asp:textbox>
									</th>
									<th valign="middle">
										<asp:button id="btn_po_search" runat="server" style="white-space: normal" text="Search PO" width="50px" font-size="8pt"  onclick="btn_search_Click"  />
									</th>
									<th align="right" valign="middle"></th>
									<th valign="middle" align="right">
										<asp:button id="btn_po_save" runat="server" backcolor="White" font-size="8pt" height="50px" width="50px" style="white-space: normal" borderstyle="none" text="Apply Changes" onclick="btn_po_save_Click" />
									</th>
								</tr>
							</thead>
							<tbody>
					</headertemplate>
					<itemtemplate>
						<tr>
							<td class="row" colspan="4">
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td colspan="3">
											<table width="100%">
												<tr>
													<td class="part">
														<div id="rpt_po_master_id" runat="server" class="mid">
															<%# Eval("master_id") %>
														</div>
													</td>
													<td class="top_right"><%# Eval("po_desc") %></td>
												</tr>
												<tr>
													<td width="70%">
														<div class="desc" onclick="pop_part('<%# Eval("master_id") %>');" width="70%">
															<%# Eval("description") %>
														</div>
													</td>
													<td class="top_right">Remaining Qty: <b><%# Eval("still_needed_qty") %></b></td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td class="tb_caption">Rec to WO</td>
										<td class="tb_caption">Rec to Stock</td>
										<td class="tb_caption">Rec to Truck</td>
									</tr>
									<tr>
										<td class="tr_wo to_wo" data-internal_qty='<%# Eval("internal_qty") %>' data-master_id='<%# Eval("master_id") %>' data-truck_qty='<%# Eval("truck_qty") %>' data-wo_qty='<%# Eval("po_qty") %>' data-still_needed_qty='<%# Eval("still_needed_qty") %>'>
											<asp:textbox id="TextBox8" runat="server" cssclass="repeater_tb" enabled='<%# ((int)Eval("wo_lck") == 1 ? true : false) %>' onkeydown="only_numeric(event)" onkeyup="validate(this,'po_to_wo');" textmode="Number" tooltip='<%# Eval("line_id") %>'></asp:textbox>
										</td>
										<td class="tr_wo to_stock" data-internal_qty='<%# Eval("internal_qty") %>' data-master_id='<%# Eval("master_id") %>' data-truck_qty='<%# Eval("truck_qty") %>' data-wo_qty='<%# Eval("po_qty") %>' data-still_needed_qty='<%# Eval("still_needed_qty") %>'>
											<asp:textbox id="TextBox9" runat="server" cssclass="repeater_tb" enabled='<%# ((int)Eval("lck") == 1 ? true : false) %>' onkeydown="only_numeric(event)" onkeyup="validate(this,'po_to_stock');" textmode="Number"></asp:textbox>
										</td>
										<td class="tr_wo to_truck" data-internal_qty='<%# Eval("internal_qty") %>' data-master_id='<%# Eval("master_id") %>' data-truck_qty='<%# Eval("truck_qty") %>' data-wo_qty='<%# Eval("po_qty") %>' data-still_needed_qty='<%# Eval("still_needed_qty") %>'>
											<asp:textbox id="TextBox10" runat="server" cssclass="repeater_tb" enabled='<%# ((int)Eval("lck") == 1 ? true : false) %>' onkeydown="only_numeric(event)" onkeyup="validate(this,'po_to_truck');" textmode="Number"></asp:textbox>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</itemtemplate>
					<footertemplate>
						</tbody>
								</table>
					</footertemplate>
				</asp:repeater>
			</asp:view>
			<asp:view id="vw_annual" runat="server">
				<div align="center" style="font-size:20px;">
						<asp:label id="n_results" runat="server"></asp:label>
				</div>
				<asp:repeater id="rpt_annual" runat="server">
					<headertemplate>
						<table width="100%" id="rpt" cellpadding="2" cellspacing="0">
							<thead id="rpt_header" class="rpt_header">

								<tr>
									<th valign="middle">
										<asp:textbox id="tb_annual_search" runat="server" borderstyle="None" height="25px" width="100px" class="truck_tb_search"></asp:textbox></th>
									<th valign="middle">
										<asp:button id="btn_annual_search" cssclass="btn_annual_search" runat="server" onclick="btn_search_Click" font-size="8pt" style="white-space: normal" text="Search List" width="50px"  /></th>
									<th align="right" valign="middle">
										<asp:button id="btn_add_part_to_location" runat="server" backcolor="White" borderstyle="none" font-size="8pt" height="50px" onclientclick="return check_truck_ddl();" onclick="btn_add_part_location_Click" style="white-space: normal" text="Add Part" width="50px" />
									</th>

									<th valign="middle" align="right" colspan="1">
										<asp:button id="btn_annual_save" runat="server" backcolor="White" font-size="8pt" height="50px" width="50px" style="white-space: normal" borderstyle="none" text="Apply Changes" onclick="btn_annual_save_Click" />
									</th>
								</tr>
							</thead>
							<tbody>
					</headertemplate>
					<itemtemplate>

						<tr>
							<td class="row" colspan="4">
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td colspan="2">
											<table cellspacing="0" cellpadding="0" width="100%">
												<tr>
													<td class="part">
														<div id="rpt_annual_master_id" runat="server" class="mid"><%# Eval("master_id") %> <%# ShowMergedInfo(Eval("old_id")) %></div>
													</td>
													<td class="top_right" style="white-space: normal !important">
														At Location: <b><%# Eval("location") %></b>
													</td>
												</tr>
												<tr>
													<td width="100%" colspan="2">
														<div class="annual_desc" onclick="pop_part('<%# Eval("master_id") %>');"><%# Eval("description") %></div>
													</td>
												</tr>

											</table>
										</td>


									</tr>
									<tr>
										<td style="padding-left: 8px; padding-top: 0; padding-bottom: 0;" width="70%">
											<table cellspacing="0" cellpadding="0">
												<tr>
													<td>Last Counted:</td>
													<td>
														<div id="rpt_annual_last_inv" runat="server"><b><%# Eval("last_inv_count") %></b></div>
													</td>
												</tr>
												<tr>
													<td>Is Consumable?:</td>
													<td><b><%# Eval("is_consumable") %></b></td>
												</tr>
												<tr>
													<td>Count History:</td>
													<td><b><%# Eval("count_hist") %></b></td>
												</tr>
											</table>
										</td>
										<td width="30%" rowspan="2">
											<asp:hiddenfield id="hdn_annual_last_inv" runat="server" value='<%# Eval("last_inv_count") %>' />
											<asp:hiddenfield id="hdn_master_id" runat="server" value='<%# Eval("master_id") %>' />
											<asp:hiddenfield id="hdn_location_id" runat="server" value='<%# Eval("location_master_id") %>' />
											<asp:hiddenfield id="hdn_annual_qty" runat="server" value='<%# Eval("qty") %>' />
											<asp:hiddenfield id="hdn_annual_db" runat="server" value='<%# Eval("dollar_balance") %>' />
											<asp:textbox id="TextBox2" cssclass="repeater_tb" runat="server" placeholder="Enter Qty" textmode="Number"></asp:textbox>
										</td>
									</tr>


								</table>
							</td>
						</tr>
					</itemtemplate>


					<footertemplate>
						</tbody>
				</table>
		
					</footertemplate>
				</asp:repeater>
				<iframe id="if_shoppingcart_annual" class="if_shopping_cart" runat="server" height="280px" width="100%" style="border-style: none"></iframe>
			</asp:view>
			<asp:view id="vw_search" runat="server">
				<iframe id="if_shoppingcart_search" class="if_shopping_cart" runat="server" height="280px" width="100%" style="border-style: none"></iframe>
			</asp:view>
			<asp:view ID="vw_status" runat="server">
					<div class="status_sort" class="rpt_header">
						<asp:DropDownList runat="server" id="ddl_status_sort" Width="70%" AutoPostBack="True" OnSelectedIndexChanged="ddl_status_sort_OnSelectedIndexChanged">
							<asp:ListItem text="Record #" value="1"></asp:ListItem>
							<asp:ListItem text="Master ID" value="2"></asp:ListItem>
							<asp:ListItem text="Description" value="3"></asp:ListItem>
							<asp:ListItem text="Committed vs. Required" value="4"></asp:ListItem>
						</asp:DropDownList>
						<asp:CheckBox runat="server" id="chkAscending" Text="Order Asc?" Width="28%" Checked="True" OnCheckedChanged="chkAscending_OnCheckedChanged" AutoPostBack="True"/>
					</div>
				<div class="status_container">
					<asp:repeater id="rpt_status" runat="server">
						<itemtemplate>
							<div class="status_row">
								<div class="rec_no"><%# Eval("rec_no") %></div>
								<div class="description"><b><%# Eval("master_id") %></b> - <%# Eval("description") %></div>
								<div class="qty_compare">
									<span class="text"><b><%# Eval("qty_committed") %> / <%# Eval("qty_ordered") %> Committed</b></span>
									<span class="fill" style="width:<%# Eval("width") %>% !important;"></span>
								</div>
							</div>
						</itemtemplate>
					</asp:repeater>
				</div>
			</asp:view>
		</asp:multiview>

		<div id="pop_fix_qty" class="pop_fix_qty" style="padding: 5px">
			<asp:imagebutton class="pop_part_close" id="ImageButton1" runat="server" onclientclick="close_fix_qty();return false;" imageurl="~/images/icon/icon[minus].gif" />
			<dx:aspxcallbackpanel id="cb_fix_qty" runat="server" width="100%" clientinstancename="cb_fix_qty" oncallback="cb_fix_qty_Callback" height="250px">
				<clientsideevents endcallback="function(s, e) {
	if (s.cp_close!=null &amp;&amp; s.cp_close!='')
{
close_fix_qty();
						 s.cp_close=null;
}
}" />
				<styles>
					<loadingpanel horizontalalign="Center" verticalalign="Middle">
					</loadingpanel>
				</styles>
				<loadingpanelstyle horizontalalign="Center" verticalalign="Middle">
				</loadingpanelstyle>
				<paddings />
				<panelcollection>
					<dx:panelcontent runat="server">
						<table style="width: 100%;" class="rpt_header">
							<tr>
								<td colspan="3"><b>Report Incorrect Qty</b> </td>
							</tr>
							<tr>
								<td>Part No:</td>
								<td width="100%">
									<asp:label id="lbl_fix_part" runat="server" font-bold="True" font-size="16pt"></asp:label>
								</td>
								<td align="right" style="padding-right: 40px;"></td>
							</tr>
							<tr>
								<td style="vertical-align: top;">Description:</td>
								<td width="100%" colspan="2" class="lbl_pop_description">
									<asp:label id="lbl_fix_description" runat="server" font-size="8pt"></asp:label>
								</td>
							</tr>
							<tr>
								<td>At Location:</td>
								<td>
									<asp:label id="lbl_fix_location" runat="server"></asp:label>
								</td>
								<td></td>

							</tr>

							<tr>
								<td></td>
								<td></td>
								<td></td>
							</tr>
							<tr>
								<td nowrap="nowrap">Qty In System:</td>
								<td>
									<asp:label id="lbl_fix_qty_insystem" runat="server"></asp:label>
								</td>
								<td></td>
							</tr>
							<tr>
								<td></td>
								<td></td>
								<td></td>
							</tr>
							<tr>
								<td>Actual Qty:</td>
								<td>
									<asp:textbox id="txt_fix_actual_qty" onkeydown="only_numeric(event)" font-size="14pt" width="70px" height="30px" runat="server" borderstyle="None" textmode="Number"></asp:textbox>

									</dx:ASPxTextBox>
										  
								</td>
								<td></td>

							</tr>
							<tr>
								<td></td>
								<td style="padding-top: 10px;">
									<asp:button id="btn_fix_qty_save" onclientclick="cb_fix_qty.PerformCallback('save');return false;" backcolor="white" runat="server" text="Send ->" borderstyle="None" height="40px" width="70px" />
								</td>
								<td></td>

							</tr>

						</table>
					</dx:panelcontent>
				</panelcollection>

			</dx:aspxcallbackpanel>
		</div>

		<div id="pop_part" class="pop_part" style="padding: 5px">
			<asp:imagebutton class="pop_part_close" id="close_pop_part" runat="server" onclientclick="close_pop_part(); return false;" imageurl="~/images/icon/icon[minus].gif" />
			<dx:aspxcallbackpanel id="cb_part" runat="server" width="100%" clientinstancename="cb_part" oncallback="cb_part_Callback" height="250px">

				<settingsloadingpanel text="" />
				<images>
					<loadingpanel url="~/images/loading_panel.gif">
					</loadingpanel>
				</images>
				<loadingpanelimage url="~/images/loading_panel.gif">
				</loadingpanelimage>

				<styles>
					<loadingpanel horizontalalign="Center" verticalalign="Middle" backcolor="Transparent">
					</loadingpanel>
					<loadingdiv backcolor="Transparent">
					</loadingdiv>
				</styles>
				<loadingpanelstyle horizontalalign="Center" verticalalign="Middle" backcolor="Transparent">
				</loadingpanelstyle>
				<paddings />
				<loadingdivstyle backcolor="Transparent">
				</loadingdivstyle>
				<panelcollection>
					<dx:panelcontent runat="server">
						<table style="width: 100%;" class="rpt_header">
							<tr>
								<td colspan="3"></td>
							</tr>
							<tr>
								<td>Part No:</td>
								<td>
									<asp:label id="lbl_pop_part" class="lbl_pop_part" runat="server" font-bold="True" font-size="16pt"></asp:label>
								</td>
								<td align="right" width="100%">
									<asp:image id="img_pop_part" runat="server" height="60px" width="60px" /></td>
							</tr>
							<tr>
								<td style="vertical-align: top;">Description:</td>
								<td width="100%" colspan="2" class="lbl_pop_description">
									<asp:label id="lbl_pop_description" runat="server" text="" font-size="8pt"></asp:label>
								</td>
							</tr>
							<tr>
								<td></td>
								<td></td>
								<td></td>
							</tr>
							<tr id="tr_qty_in_stock" runat="server">
								<td nowrap="nowrap">Qty Internal:</td>
								<td style="white-space: nowrap;">
									<div onclick="pop_fix_qty('internal');">
										<asp:label id="lbl_pop_qty_onhand" runat="server"></asp:label>
									</div>
									<asp:hiddenfield id="hdn_fix_qty_internal_location_id" runat="server" />
								</td>
								<td></td>
							</tr>
							<tr id="tr_qty_on_truck" runat="server">
								<td nowrap="nowrap">Qty External:</td>
								<td>
									<asp:label id="lbl_pop_qty_external" runat="server"></asp:label>
								</td>
								<td></td>
							</tr>
							<tr id="tr_qty_on_wo" runat="server">
								<td nowrap="nowrap">Qty On WOs:</td>
								<td>
									<asp:label id="lbl_pop_qty_onwos" runat="server"></asp:label>
								</td>
								<td></td>
							</tr>
							<tr id="tr_qty_on_order" runat="server">
								<td nowrap="nowrap">Qty Coming In:</td>
								<td>
									<asp:label id="lbl_pop_qty_onorder" runat="server"></asp:label>
								</td>
								<td></td>
							</tr>
							<tr>
								<td></td>
								<td></td>
								<td></td>
							</tr>
							<tr>
								<td>Internal Location:</td>
								<td colspan="2">
									<asp:label id="lbl_pop_location" runat="server" cssclass="lbl_pop_location"></asp:label>
								</td>


							</tr>
							<tr>
								<td></td>
								<td colspan="2"></td>
							</tr>
							<tr>
								<td colspan="2">
									<dx:aspxbutton id="btn_print_bc" runat="server" text="Print Barcode" autopostback="False" clientsideevents-click="function(s,e) {{ print_bc(s);}}"></dx:aspxbutton>
								</td>
								<td></td>
							</tr>

						</table>
					</dx:panelcontent>
				</panelcollection>

			</dx:aspxcallbackpanel>
		</div>

	</contenttemplate>

</asp:updatepanel>
