<%@ page language="C#" autoeventwireup="true" inherits="mobile_shopping_cart"  title="Shopping Cart"  Codebehind="shopping_cart.aspx.cs" %>
<%@ register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	namespace="DevExpress.Web" tagprefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta content="True" name="HandheldFriendly" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=10, minimum-scale=1 user-scalable=1" name="viewport" />
	<meta name="viewport" content="width=device-width" />
	<link rel="stylesheet" href="css/shopping_cart.css" />
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	
	
    <script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" src="/mobile/js/base.js"></script>
	<script type="text/javascript" src="/mobile/js/shopping_cart.js"></script>
        	<script type="text/javascript">
		$(document).ready(function()
			{
			cart.update_panel_progress.bind();
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(cart.bind);
			set_controls();
			
		//	$(".btn_search").click();
			$(window).scroll(
				function(e)
					{
					var t = $(window).scrollTop();
					var button_pane_position = $("#button_pane").offset().top;
					//console.log("current inner t:"+t+" - button_pane position"+button_pane_position);
					if(t < button_pane_position)
						{
				//		$("#button_pane").addClass("fixed");
				//		$("#button_pane_anchor").height($("#button_pane").outerHeight());
						//$(".button_placeholder").addClass("show");
						}
					else
						{
				//		$("#button_pane").removeClass("fixed");
				//		$("#button_pane_anchor").height(0);
						//$(".button_placeholder").removeClass("show");
						}
					if (t > 400)
						{
						$("#to_top").show();
						}
					else
						{
						$("#to_top").hide();
						}
					});
			});
		function clear_all(s) {
			var x = 0;
			while (x < s.cp_rowcount) {
				var req_tb = ASPxClientControl.GetControlCollection().GetByName("cs_qty_cart_" + x);
				req_tb.SetText('');
				req_tb = ASPxClientControl.GetControlCollection().GetByName("cs_qty_cart_truck_to_wo_" + x);
				req_tb.SetText('');
				req_tb = ASPxClientControl.GetControlCollection().GetByName("cs_qty_cart_stock_to_truck_" + x);
				req_tb.SetText('');
				req_tb = ASPxClientControl.GetControlCollection().GetByName("cs_qty_cart_stock_to_wo_" + x);
				req_tb.SetText('');
				req_tb = ASPxClientControl.GetControlCollection().GetByName("cs_date_" + x);
				req_tb.SetText('');
				x++;
			}
		}

		function set_controls()
		{
		    var woprog_id = $(".hdn_woprog_id").val();
		    var location_id = $(".hdn_location_id").val();
		    var is_annual = $(".hdn_is_annual").val();
		    if (woprog_id != 0) {
		        $("#search_pane").css({ "background-Color": "#99FF99" });
		        $(".btn_777").show();
		        $(".qty_req").show();
		        $(".qty_t").show();
		        $(".caption").show();
		        $(".addtocart").text('Add to Work Order');
		    }
		    else if (is_annual==1)
		    {
		        $("#search_pane").css({ "background-Color": "LightBlue" });
		        $(".btn_777").hide();
		        $(".qty_req").hide();
		        $(".qty_t").hide();
		        $(".caption").hide();
		        $(".addtocart").text('Add to location');
		    }
		    else if (location_id != 0) {
		        $("#search_pane").css({ "background-Color": "#FFCC99" });
		        $(".btn_777").hide();
		        $(".qty_req").hide();
		        $(".qty_t").hide();
		        $(".caption").hide();
		        $(".addtocart").text('Add to truck');
		    }
		    else
		    {
		        $("#search_pane").css({ "background-Color": "#FF66FF" });
		        $(".btn_777").hide();
		        $(".qty_req").hide();
		        $(".qty_t").hide();
		        $(".caption").hide();
		        $(".addtocart").hide();
		        $(".qty_s").hide();
		        $(".top_right_caption").hide();
		        $(".qc").hide();
		        $(".qr").hide();
		        $(".button spacer hideme addtocart").hide();
		    }
		}

		function open_777_div()
		{
		  
		}

		function validate_tb(s) {
			//        max = gv_destination.GetRowValues(gv_destination.GetFocusedRowIndex(), 'qty_on_wo', OnGetRowValues);
			var text = s.GetText();
			var max = s.cp_max;
			var min = s.cp_min;
			var count = text.split("-").length;
			// var count = (text.match(/-/g) || []).length;
			if (count > 2) {
				alert("You have " + count - 1 + " '-' symbols in the number (" + text + ").");
				s.SetText('');
			}
			count = text.split(".").length;
			if (count > 2) {
				alert("You have " + count - 1 + " '.' symbols in the number (" + text + ").");
				s.SetText('');
			}
			if (text != '') {
				if (text > max) {
					alert("The maximum you can enter is " + max + " because of a stock constraint.");
					s.SetText(max);
				}
				else if (text < min) {
					alert("The minimum you can enter is " + min + " because of a stock constraint.");
					s.SetText(min);
				}
			}
		}
		function fill_requested(s) {
			var x = 0;
			while (x < s.cp_rowcount) {
				var req_tb = ASPxClientControl.GetControlCollection().GetByName("cs_qty_cart_" + x);
				var req_tb1 = ASPxClientControl.GetControlCollection().GetByName("cs_date_" + x);
				req_tb.SetText(req_tb.cp_cartqty);
				req_tb1.SetText(req_tb1.cp_date);
				x++;
			}
		}
		function fill_truck_to_wo(s) {
			var x = 0;
			while (x < s.cp_rowcount) {
				var req_tb = ASPxClientControl.GetControlCollection().GetByName("cs_qty_cart_truck_to_wo_" + x);
				req_tb.SetText(req_tb.cp_cartqty);
				x++;
			}
		}
		function fill_stock_to_truck(s) {
			var x = 0;
			while (x < s.cp_rowcount) {
				var req_tb = ASPxClientControl.GetControlCollection().GetByName("cs_qty_cart_stock_to_truck_" + x);
				req_tb.SetText(req_tb.cp_cartqty);
				x++;
			}
		}
		function fill_stock_to_wo(s) {
			var x = 0;
			while (x < s.cp_rowcount) {
				var req_tb = ASPxClientControl.GetControlCollection().GetByName("cs_qty_cart_stock_to_wo_" + x);
				req_tb.SetText(req_tb.cp_cartqty);
				x++;
			}
		}

		function print_bc(id) {
		    var master_id = document.getElementsByClassName("lbl_pop_part")[0].textContent;
		    if (master_id == '777') {

		    }
		    else {
		        cb_part.PerformCallback('print_bc|' + master_id);
		    }


		}

	</script>

    
</head>
<body>
      <form id="form1" runat="server">
	<asp:scriptmanager id="sm" runat="server"></asp:scriptmanager>
	<asp:updatepanel id="up" runat="server">
		<contenttemplate>
			<div><input type="hidden" ID="hdn_woprog_id" runat="server" class="hdn_woprog_id" /><input type="hidden" ID="hdn_location_id" runat="server" class="hdn_location_id" />
                <input type="hidden" ID="hdn_is_annual" runat="server" class="hdn_is_annual" />
				<asp:hiddenfield id="hdn_member_id" runat="server" />
				<asp:sqldatasource id="sds_carts" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" selectcommand="Select id,name from shopping_cart_header where member_id = @id order by name">
					<selectparameters>
						<asp:sessionparameter name="id" sessionfield="member_id" />
					</selectparameters>
				</asp:sqldatasource>
				<dx:aspxcallbackpanel id="cbp_header" runat="server" clientinstancename="cbp_header" oncallback="cbp_header_Callback">
						<panelcollection>
							<dx:panelcontent>
								<div style="visibility:hidden; text-space-collapse:collapse;" aria-hidden="True" hidden="hidden" >
                                    <table width="100%" cellpadding="4" cellspacing="0" style="padding: 3px; background-color:#99FF99;" width="100%">
                                        <tr>
                                        
                                        <td><div class="r"><dx:aspxcombobox id="ddl_cart" runat="server" autopostback="True" clientinstancename="ddl_cart" datasourceid="sds_carts"  onselectedindexchanged="ddl_cart_SelectedIndexChanged" valuetype="System.Int32" textfield="name" valuefield="id" width="100%" Font-Size="14pt" Height="35px" NullText="Select Shopping Cart" DropDownButton-ClientVisible="false">
                                               
                                <ClearButton Visibility="True">
                                </ClearButton>
                                            <ItemStyle Height="35px" Wrap="True"  Paddings-PaddingTop="5" VerticalAlign="Top">
                                <Paddings Padding="3px" />
                                <Border BorderStyle="None" />
                                </ItemStyle>
                                <Border BorderStyle="None" />
                                            </dx:aspxcombobox></div></td>
                                            <td width="100px">
                                                <asp:Button ID="btn_show_cart" BackColor="white" Width="100px" Height="35px" runat="server" Text="Add Parts" OnClick="btn_show_cart_Click" UseSubmitBehavior="False" Font-Bold="False" />
                                            </td>
                                        </tr>
									
                                        </table>
								</div>
							</dx:panelcontent>
						</panelcollection>
				</dx:aspxcallbackpanel>
				<asp:MultiView ID="mv" runat="server">
                    <asp:View ID="vw_search" runat="server">
                        <dx:aspxcallbackpanel id="cb_search" runat="server" clientinstancename="cb_search"   oncallback="cb_search_Callback" width="100%">
										<settingsloadingpanel delay="0" text="" />
										<clientsideevents endcallback="cart.control_handler.cb_search.encallback" />
										<images>
											<loadingpanel url="~/images/loading_panel.gif">
											</loadingpanel>
										</images>
										<loadingpanelimage url="~/images/loading_panel.gif">
										</loadingpanelimage>
										<panelcollection>
											<dx:panelcontent runat="server">
												<div id="to_top" onclick="cart.scroll_to_top();"><span class="text">To Top</span></div>
												<div id="search" >
													<div id="search_pane">
														<table class="controls" cellpadding="4" cellspacing="0" width="100%" height="50">
															<tr>
																<td width="10%" style="min-width:50px;" align="center" valign="middle">
																	<dx:aspxcombobox id="ddl_type" runat="server"  native="true" valuetype="System.Int32" font-size="14pt" height="35px" selectedindex="0" width="100%" ForeColor="#818181">
																		<clientsideevents selectedindexchanged="cart.control_handler.type" />
																		<items>
																			<dx:listedititem text="Description" value="0" />
																			<dx:listedititem text="Most Commonly Used" value="1" />
																			<dx:listedititem selected="True" text="Only Stocked" value="2" />
																			<dx:listedititem text="Recent WOs" value="3" />
																			<dx:listedititem text="Recent Quotes" value="4" />
                                                                            <dx:listedititem text="Vendor Part No" value="5" />
                                                                          
																		</items>
																	    <Border BorderStyle="None" />
																	</dx:aspxcombobox>
																</td>
																<td width="85%" align="center" valign="middle">
																	<input type="text" id="tb_search" runat="server" placeholder="Enter Search Criteria" class="tb_search" height="35px"></input>
                                                                     <dx:aspxcombobox id="ddl_type_id" runat="server" font-size="1.5em" height="35px" nulltext="Select an Item" cssclass="type_id" clientinstancename="ddl_type_id" clientvisible="False" oncallback="ddl_type_id_Callback" textfield="name" valuefield="id" valuetype="System.Int32"   width="100%">
																		<itemstyle font-size="0.75em" />
																	    
																	    <Border BorderStyle="None" />
																	</dx:aspxcombobox>
																</td>
                                                                <td width="5%" align="center" style="min-width:50px;" valign="middle">
                                                                    <input id="Button1" type="button" value="777" onclick="pop_777()" class="btn_777" />
																</td>
																<td width="5%" align="center" style="min-width:50px;" valign="middle">
																	<asp:imagebutton id="btn_search" cssclass="btn_search" runat="server" height="35px" imageurl="~/images/icon/icon[find-white].png" onclick="btn_search_Click" width="35px" />
																</td>
															</tr>
														</table>
													</div>
													<div id="button_pane_anchor"></div>
													<div style="background-color:whitesmoke;" id="button_pane">
                                                        <div style="height:35px;padding-left:5px; padding-right:5px;padding-top:5px;" id="refine_button_pane"></div>
														<div class="button_placeholder" style="background-color:whitesmoke; padding:5px;">
															<button type="button"  class="button spacer hideme addtocart " onclick="cart.add_items.run();" >Add part(s) to Work Order</button>
														</div>
                                                    </div>
													
													<div style="width: 100%;">
														<div runat="server" id="container_refine">
                                                            <div id="search_path" ></div>
															<div id="div_refine">
															</div>
														</div>
														<div runat="server" id="container_results" >
                                                            <div id="container_results_scroll"  >
                                                            <div id="div_results" style="overflow-y:hidden;"></div>
                                                            </div>
														</div>
													</div>
													<dx:aspxcallback id="cb_addpart" runat="server" clientinstancename="cb_addpart" oncallback="cb_addpart_Callback">
														<clientsideevents begincallback="cart.add_items.cb.begin" callbackcomplete="cart.add_items.cb.complete" endcallback="cart.add_items.cb.end" callbackerror="cart.add_items.cb.error" />
													</dx:aspxcallback>
												</div>
											</dx:panelcontent>
										</panelcollection>
									</dx:aspxcallbackpanel>
                    </asp:View>
                    <asp:View ID="vw_cart" runat="server">
                        	<div id="contents" style="width:auto">
										<div class="buttons">
											<div>
												<dx:aspxbutton id="btn_newcart" runat="server" onclick="btn_group1_Click" text="Create New Cart" cssclass="button">
												</dx:aspxbutton>
											</div>
											<div>
												<dx:aspxbutton id="btn_deletecart" runat="server" onclick="btn_group0_Click" text="Delete Cart"  cssclass="button">
													<clientsideevents click="cart.contents.bt_delete.verify" />
												</dx:aspxbutton>
											</div>
											<div>
												<dx:aspxbutton id="btn_consolidate" runat="server" autopostback="False" text="Consolidate" cssclass="button" clientvisible="False">
													<clientsideevents click="cart.contents.bt_consolidate.verify" />
												</dx:aspxbutton>
											</div>
											<div>
												<dx:aspxbutton id="btn_clear" runat="server" autopostback="False" text="Clear Cart"  cssclass="button">
													<clientsideevents click="cart.contents.bt_clear.verify" />
												</dx:aspxbutton>
											</div>
										</div>
										<div class="header">Parts in the shopping cart</div>
										<dx:aspxgridview	id="gv_cart" 
															runat="server" 
															autogeneratecolumns="False" 
															visible="true" 
															clientinstancename="gv_cart" 
															keyfieldname="id" 
															oncustomcallback="gv_cart_CustomCallback" 
															onrowdeleting="gv_cart_RowDeleting"
															width="100%"
															>
											<clientsideevents endcallback="cart.contents.gv_cart.endcallback" rowclick="cart.contents.gv_cart.rowclick" />
											<columns>
												<dx:gridviewcommandcolumn buttontype="Image" caption=" " showdeletebutton="True" visibleindex="0" width="5%">
													<cellstyle horizontalalign="Center" paddings-padding="4px" >
														<paddings padding="4px" />
													</cellstyle>
												</dx:gridviewcommandcolumn>
												<dx:gridviewdatatextcolumn caption="Qty" fieldname="qty" visibleindex="1" width="15%">
													<dataitemtemplate>
														<dx:aspxtextbox id="tb_car_qty" runat="server" font-size="1.5em" height="20px" horizontalalign="Center" oninit="tb_car_qty_Init" text='<%# Eval("qty") %>'  width="100%">
															<clientsideevents gotfocus="function(_s,_e){_s.SelectAll();}" />
														</dx:aspxtextbox>
													</dataitemtemplate>
													<headerstyle horizontalalign="Center" />
													<cellstyle horizontalalign="Center" paddings-padding="4px" >
														<paddings padding="4px" />
													</cellstyle>
												</dx:gridviewdatatextcolumn>
												<dx:gridviewdatatextcolumn caption="Part" fieldname="master_id" visibleindex="2" width="5%">
													<cellstyle horizontalalign="Center" paddings-padding="4px" >
														<paddings padding="4px" />
													</cellstyle>
												</dx:gridviewdatatextcolumn>
												<dx:gridviewdatatextcolumn caption="Description" fieldname="desc" visibleindex="3" width="75%">
													<cellstyle horizontalalign="Left" wrap="True" paddings-padding="4px" >
														<paddings padding="4px" />
													</cellstyle>
												</dx:gridviewdatatextcolumn>
											</columns>
											<settingsbehavior allowfocusedrow="True" confirmdelete="True" allowdragdrop="false" />
											<settingspager mode="ShowAllRecords" numericbuttoncount="4">
											</settingspager>
											<settingstext emptydatarow="This shopping cart is empty" searchpaneleditornulltext="Search This Cart Only..." />
											<settingscommandbutton>
												<deletebutton buttontype="Image">
													<image url="~/images/icon/icon[delete].gif">
													</image>
												</deletebutton>
											</settingscommandbutton>
											<settingssearchpanel visible="True" />
											<styles>
												<header cssclass="gv_header">
												</header>
												<focusedrow cssclass="focused_row">
												</focusedrow>
												<cell paddings-padding="0px">
													<paddings padding="0px" />
												</cell>
											</styles>
											<styles header-cssclass="gv_header" />
										</dx:aspxgridview>
										

										<div class="header">Other common parts used with the highlighted part</div>
										<dx:aspxgridview id="gv_other_parts" runat="server" autogeneratecolumns="False" clientinstancename="gv_other_parts" oncustomcallback="gv_other_parts_CustomCallback"  width="100%">
											<clientsideevents endcallback="cart.contents.gv_otherparts.endcallback" />
											<columns>
												<dx:gridviewdatatextcolumn caption="Qty" visibleindex="0" width="7%">
													<dataitemtemplate>
														<dx:aspxtextbox id="tb_other_qty" runat="server" horizontalalign="Center" width="100%">
														</dx:aspxtextbox>
													</dataitemtemplate>
													<headerstyle horizontalalign="Center" />
													<cellstyle horizontalalign="Center" paddings-padding="4px" >
														<paddings padding="4px" />
													</cellstyle>
												</dx:gridviewdatatextcolumn>
												<dx:gridviewdatatextcolumn caption="Category" fieldname="tag" visibleindex="1" width="19%">
													<settings headerfiltermode="CheckedList" />
													<cellstyle horizontalalign="Center" wrap="True" paddings-padding="4px" >
														<paddings padding="4px" />
													</cellstyle>
												</dx:gridviewdatatextcolumn>
												<dx:gridviewdatatextcolumn caption="Part" fieldname="master_id" visibleindex="2" width="4%">
													<cellstyle horizontalalign="Center" paddings-padding="4px" >
														<paddings padding="4px" />
													</cellstyle>
												</dx:gridviewdatatextcolumn>
												<dx:gridviewdatatextcolumn caption="Description" fieldname="description" visibleindex="3" width="80%">
													<cellstyle horizontalalign="Left" wrap="True" paddings-padding="4px" >
														<paddings padding="4px" />
													</cellstyle>
												</dx:gridviewdatatextcolumn>
											</columns>
											<settings showfooter="True" showheaderfilterbutton="True" />
											<settingstext emptydatarow="There are no common parts used with the selected part from above" />
											<settingsbehavior allowdragdrop="false" allowfocusedrow="false" />
											<settingssearchpanel visible="True" />
											<paddings padding="5px" />
											<styles header-cssclass="gv_header" >
												<header cssclass="gv_header">
												</header>
											</styles>
										</dx:aspxgridview>
										<dx:aspxbutton id="btn_add_other_part" runat="server" clientenabled="true" autopostback="False" clientinstancename="btn_add_other_part" text="Add to WO" cssclass="button" wrap="True">
											<clientsideevents click="cart.contents.gv_otherparts.add_part.click" />
										</dx:aspxbutton>
									</div>
                    </asp:View>
                    <br />
                </asp:MultiView>
               
				
			</div>
                        

                  <div id="pop_777" class="pop_part" style="padding: 5px">
                <asp:ImageButton  class="pop_part_close" ID="ImageButton2" runat="server" OnClientClick="close_pop_777(); return false;" ImageUrl="~/images/icon/icon[minus].gif" />
                <dx:ASPxCallbackPanel ID="cb_777" runat="server" Width="100%" ClientInstanceName="cb_777" OnCallback="cb_777_Callback" Height="250px">

                    <ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_alert!=null &amp;&amp; s.cp_alert!=&quot;&quot;)
{
confirm(s.cp_alert);
s.cp_alert=&quot;&quot;;
}
                        if (s.cp_close!=null &amp;&amp; s.cp_close!=&quot;&quot;)
{

s.cp_close=&quot;&quot;;
                     close_pop_777();  
}
}" />

                    <Styles>
                        <LoadingPanel HorizontalAlign="Center" VerticalAlign="Middle">
                        </LoadingPanel>
                    </Styles>
                    <LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </LoadingPanelStyle>
                    <Paddings />
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table style="width: 100%; color: #FFFFFF; " cellpadding="5px">
                                <tr>
                                   <td >Add 777 Part Request</td>
                                </tr>
                                <tr>
                                   <td ></td>
                                </tr>
                                <tr>
                                   <td >Enter Description:</td>
                                </tr>
                               
                                 <tr>
                                    
                                    <td width="100%" Class="lbl_pop_description">
                                        <asp:TextBox ID="tb_777_desc" Font-Size="14pt" Width="100%" Height="30px" runat="server" BorderStyle="None" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                  <tr>
                                      <td>
                                          Qty:
                                      </td>
                                      </tr><tr>
                                      <td>
                                          <asp:TextBox ID="tb_777_qty" onkeydown="only_numeric(event)"  Font-Size="14pt" Width="70px" Height="30px" runat="server" BorderStyle="None" TextMode="Number"></asp:TextBox>
                                      </td>
                                  </tr>
                                <tr>
                                    <td >
                                        <asp:Button ID="btn_save_777" runat="server" width="60px" Text="Save" OnClientClick="cb_777.PerformCallback('save');return false;" />
                                    </td>
                                    <td>

                                    </td>
                                </tr>
                                
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>

                </dx:ASPxCallbackPanel>
            </div>

               <div id="pop_part" class="pop_part" style="padding: 5px">
                <asp:ImageButton  class="pop_part_close" ID="close_pop_part" runat="server" OnClientClick="close_pop_part(); return false;" ImageUrl="~/images/icon/icon[minus].gif" />
                <dx:ASPxCallbackPanel ID="cb_part" runat="server" Width="100%" ClientInstanceName="cb_part" OnCallback="cb_part_Callback" Height="350px">

                    <Styles>
                        <LoadingPanel HorizontalAlign="Center" VerticalAlign="Middle">
                        </LoadingPanel>
                    </Styles>
                    <LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </LoadingPanelStyle>
                    <Paddings />
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table style="width: 100%; color: #FFFFFF; padding-top:25px;">
                                <tr>
                                    <td colspan="3"></td>
                                </tr>
                                <tr>
                                    <td>Part No:</td>
                                    <td >
                                        <asp:Label ID="lbl_pop_part" class="lbl_pop_part" runat="server"  Font-Bold="True" Font-Size="16pt"></asp:Label>
                                    </td>
                                    <td align="right" width="100%" >
                                        <asp:Image ID="img_pop_part" runat="server" Height="60px" width="60px" /></td>
                                </tr>
                                 <tr>
                                    <td style="vertical-align:top;">Description:</td>
                                    <td width="100%" colspan="2" Class="lbl_pop_description">
                                        <asp:Label ID="lbl_pop_description" runat="server" Text="" Font-Size="8pt" ></asp:Label>
                                    </td>
                                </tr>
                                     <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr id="tr_qty_in_stock" runat="server">
                                        <td nowrap="nowrap">Qty in stock:</td>
                                        <td style="white-space:nowrap;"><div onclick="pop_fix_qty('internal');">
                                            <asp:Label ID="lbl_pop_qty_onhand" runat="server"></asp:Label></div>
                                            <asp:HiddenField ID="hdn_fix_qty_internal_location_id" runat="server" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="tr_qty_on_truck" runat="server">
                                        <td nowrap="nowrap">Qty on truck:</td>
                                        <td>
                                            <asp:Label ID="lbl_pop_qty_external" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="tr_qty_on_wo" runat="server">
                                        <td nowrap="nowrap">Qty on this WO:</td>
                                        <td>
                                            <asp:Label ID="lbl_pop_qty_onwos" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="tr_qty_on_order" runat="server">
                                        <td nowrap="nowrap">Qty On Order:</td>
                                        <td>
                                            <asp:Label ID="lbl_pop_qty_onorder" runat="server"></asp:Label>
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
                                            <asp:Label ID="lbl_pop_location" runat="server" CssClass="lbl_pop_location"></asp:Label>
                                        </td>
                                        

                                    </tr>
                                <tr>
                                        <td></td>
                                        <td colspan="2">
                                            
                                        </td>
                                    </tr>
                                <tr>
                                        <td colspan="2">
                                            <dx:ASPxButton ID="btn_print_bc" runat="server" Text="Print Barcode" AutoPostBack="False" ClientSideEvents-Click="function(s,e) {{ print_bc(s);}}"></dx:ASPxButton>
                                        </td>
                                        <td >
                                            
                                        </td>
                                    </tr>
                                
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>

                </dx:ASPxCallbackPanel>
            </div>

                 <div id="pop_fix_qty" class="pop_fix_qty" style="padding: 5px">
                <asp:ImageButton  class="pop_part_close" ID="ImageButton1" runat="server" OnClientClick="close_fix_qty();return false;" ImageUrl="~/images/icon/icon[minus].gif" />
                <dx:ASPxCallbackPanel ID="cb_fix_qty" runat="server" Width="100%" ClientInstanceName="cb_fix_qty" OnCallback="cb_fix_qty_Callback" Height="250px">

                    <ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_close!=null &amp;&amp; s.cp_close!='')
{
close_fix_qty();
                         s.cp_close=null;
}
}" />

                    <Styles>
                        <LoadingPanel HorizontalAlign="Center" VerticalAlign="Middle">
                        </LoadingPanel>
                    </Styles>
                    <LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </LoadingPanelStyle>
                    <Paddings />
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table style="width: 100%; color: black;">
                                <tr>
                                    <td colspan="3"><b>Report Incorrect Qty</b> </td>
                                </tr>
                                <tr>
                                    <td>Part No:</td>
                                    <td width="100%">
                                        <asp:Label ID="lbl_fix_part" runat="server"  Font-Bold="True" Font-Size="16pt"></asp:Label>
                                    </td>
                                    <td align="right" style="padding-right:40px;">
                                       </td>
                                </tr>
                                 <tr>
                                    <td style="vertical-align:top;">Description:</td>
                                    <td width="100%" colspan="2" Class="lbl_pop_description">
                                        <asp:Label ID="lbl_fix_description" runat="server"  Font-Size="8pt" ></asp:Label>
                                    </td>
                                </tr>
                              <tr>
                                        <td>At Location:</td>
                                        <td>
                                            <asp:Label ID="lbl_fix_location" runat="server"></asp:Label>
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
                                            <asp:Label ID="lbl_fix_qty_insystem" runat="server"></asp:Label>
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
                                            <asp:TextBox ID="txt_fix_actual_qty" onkeydown="only_numeric(event)"  Font-Size="14pt" Width="70px" Height="30px" runat="server" BorderStyle="None" TextMode="Number"></asp:TextBox>
                                       
                                           
                                          
                                        </td>
                                        <td></td>

                                    </tr>
                                 <tr>
                                        <td></td>
                                        <td style="padding-top:10px;">
                                            <asp:Button ID="btn_fix_qty_save" OnClientClick="cb_fix_qty.PerformCallback('save');return false;" BackColor="white" runat="server" Text="Send ->" BorderStyle="None" Height="40px"  Width="70px" />
                                        </td>
                                        <td></td>

                                    </tr>
                                
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>

                </dx:ASPxCallbackPanel>
            </div>

		</contenttemplate>
	</asp:updatepanel>
          </form>
    </body>


