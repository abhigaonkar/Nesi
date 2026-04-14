<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" EnableTheming="false" validateRequest="false"  enableEventValidation="false" CodeBehind="picklist.aspx.cs" Theme="" Inherits="sections_workorder_picklist" %>
<%@ Import Namespace="nesi.core" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="head_content" ContentPlaceHolderID="header_placeholder" runat="server">

<style type="text/css">
	.dxgvPreviewRow td {
	   font-family: Arial;
font-size: 8pt;
vertical-align: Top;
padding-top:2px!important;
	 
		padding-left:15px!important;
	}
    
    .wrap { white-space: normal; width: 100px; height:35px;border:none; font-size:10px; background-color:whitesmoke; cursor:pointer;}

	</style>
</asp:Content>
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<a name="top"></a>
	<script type="text/javascript" src="/js/picklist_wo.js?unique=<%=Toolbox.do_RandomString(10) %>"></script>
	<div id="picklist" style="padding: 10px" Font-Names="Calibri,Arial">
	<asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
	<dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="agv">
								</dx:ASPxGridViewExporter>
	<ASP:UPDATEPANEL id="UpdatePanel1" runat="server" UpdateMode="Conditional">
  <triggers>
	  <asp:PostBackTrigger ControlID="btnXlsxExport" runat="server" />
  </triggers>     
	<contenttemplate>
		<table style="padding-top: 10px; padding-right: 5px; padding-left: 0px; border-collapse: collapse;" cellpadding="3px">
			<tr>
				<td><dx:ASPxButton ID="btnPrintAll" runat="server" AutoPostBack="False" 
						Visible="False" BackColor="Transparent" 
						ToolTip="Print all bar codes for this PO" Height="25px" Width="25px">
						<Image Url="~/images/icon/icon[print_barcode].GIF" Width="20px">
						</Image>
						<ClientSideEvents Click="print_all" />
					</dx:ASPxButton></td>
				<td><dx:ASPxButton ID="btnXlsxExport" runat="server" Theme="NETheme01" 
						 Height="25px" Width="25px" OnClick="btnXlsxExport_Click" AutoPostBack="True" UseSubmitBehavior="False" >
						<Image Url="~/images/icon/icon[excel].gif" Width="20px">
						</Image>
                        
					</dx:ASPxButton>
                    </td>
				<td>
					<img style="cursor: pointer" id="ImgBtn_Print" runat="server" onclick="" alt="Print" src='~/images/icon/icon[printwprice].gif' />
				</td>
				<td>
					<img style="cursor: pointer" id="ImgBtn_PrintWO" runat="server" onclick="" alt="Print" src="~/images/iconsbuttons/32px-crystal_clear_action_shoppingcart.png" title="Full Work Order Picking List" />
				</td>
				<td>
					<img style="cursor: pointer" id="ImgBtn_PrintWOUnf" runat="server" onclick="" alt="Print" src="~/images/iconsbuttons/unfulfilled_shoppingcart.png" title="Picking List of parts still needed" />
				</td>
				<td nowrap="nowrap">
					<img style="cursor: pointer" id="btn_comment" runat="server" onclick="" height="25" visible="False" title="Comments" />


				</td>
				<td nowrap="nowrap">
					<dx:ASPxButton ID="btn_Summarizelabor" runat="server" Theme="NETheme01"
						Height="25px" Width="25px" OnClick="btnSummarizelabor_Click" AutoPostBack="False" UseSubmitBehavior="true">
					</dx:ASPxButton>
				</td>


				<td width="100%"></td>
				<td>
					<asp:Button ID="SetallitemstoDoNotInclude" runat="server" CssClass="wrap"
						OnClick="SetallitemstoDoNotInclude_Click" Text="Set Items to Do Not Include"
						Visible="False" />
				</td>
				<td>
					<asp:Button ID="btnSetItemsToJobCostforQuote" runat="server" CssClass="wrap"
						OnClick="btnSetItemsToJobCostforQuote_Click"
						Text="Set Items to Job Cost For Quote" Visible="False" />
				</td>
				<td>
					<asp:Button ID="btnSet0qtystoNoCharge" runat="server" CssClass="wrap"
						OnClick="btnSet0qtystoNoCharge_Click" Text="Change 0 Qty to No Charge"
						Visible="False" />
				</td>
				<td>
					<asp:Button ID="b_warranty" runat="server" CssClass="wrap"
						OnClick="b_warranty_Click" Text="Set Items to Visible No Charge"
						Visible="False" />
				</td>

				<td>
					<asp:Button ID="btnSetToRegular" runat="server" OnClick="btnSetToRegular_Click" CssClass="wrap"
						Text="Budget Quote" Visible="False" />
				</td>
			</tr>
		</table>
	   
	<div id="stuff" runat="server" ></div>
		
					<dx:ASPxPanel ID="pnl_header" runat="server" Width="100%" >
						<PanelCollection>
							<dx:PanelContent ID="PanelContent2" runat="server">
<table width="100%" cellpadding="2" cellspacing="0" class="header_labels">
	<tr>
	   <td align="right" width="100" class="c"><asp:Label ID="lblCusotmerName" runat="server" Text="Customer Name:" ></asp:Label></td>
	   <td align="left" class="v"><asp:Label ID="lblCustNameDisp" runat="server" Text="" ></asp:Label> 
								 <br />
								 <asp:Label ID="lblerrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="" Visible="False" Width="100%"></asp:Label></td>
		<td align="right" class="total_quoted_c"><asp:Label ID="lblQuoted" runat="server" Text="Total Quoted (~Ext'd):"></asp:Label></td>
		<td align="left" width="150" class="total_quoted_v"><asp:Label ID="lblQuotedDisp" runat="server" Text=""></asp:Label></td>
	</tr>
	<tr>
		<td align="right" class="c"><asp:Label ID="lblMemberName" runat="server" Text="Member Name:" ></asp:Label></td>
		<td rowspan="3" style="vertical-align: top" valign="top" class="v">
			<asp:Label ID="lblMembNameDisp" runat="server" Text=""  Width="100%"></asp:Label>
			<dx:ASPxMemo ID="txtGroupNotes" runat="server" Visible="False" Width="100%"></dx:ASPxMemo>
		</td>
		<td align="right" class="c">
			<asp:Label ID="lblTM" runat="server" Text="Total Benchmark Sell (Ext'd):"></asp:Label>
		</td>
		<td class="v">
			<asp:Label ID="lblTMDisp" runat="server" Text="" ></asp:Label>
		</td>
	</tr>
	<tr>
		<td align="right" class="c" colspan="3">
			<asp:Label ID="lb_benchextddiff" runat="server" Text="Bench & ~Extd Difference:"></asp:Label>
		</td>
		<td class="v">
			<asp:Label ID="lbl_benchextddiff" runat="server" Text="" ></asp:Label>
		</td>
	</tr>
	<tr>
		<td align="right" class="c" colspan="3">
			<asp:Label ID="lblCost" runat="server" Text="Total Cost:"></asp:Label>
		</td>
		<td class="v">
			<asp:Label ID="lblCostDisp" runat="server" Text="" ></asp:Label>
		</td>
	</tr>
	<tr>
		<td align="right" class="c" style="height: 22px" colspan="3">
			<asp:Label ID="lblTotalItems" runat="server" Text="Total Items:"></asp:Label>
		</td>
		<td class="v" style="height: 22px">
			<asp:Label ID="lblTotalItemsDisp" runat="server" Text="" ></asp:Label>
		</td>
	</tr>
	<tr>
		<td rowspan="3" valign="middle">&nbsp;&nbsp;<dx:ASPxButton ID="btnSaveHeader" runat="server" Height="14px" Visible="False" Width="0px"><Image Url="~/images/icon/icon[save].gif"></Image></dx:ASPxButton>
		</td>
		<td rowspan="3">
		
		<dx:ASPxCheckbox ID="chkQuoteDiscount" ClientInstanceName="quotediscount" runat="server" Wrap="false" Visible="false" AutoPostBack="True" OnCheckedChanged="chkApplyDiscount_CheckedChanged" Text="Customer Discount Applied?" />
		<dx:ASPxCheckbox ID="btn_use_current_cost" runat="server" Wrap="false" Visible="false" AutoPostBack="True" Text="Show &quot;Today's Prices&quot;" /></td>
		<td align="right" class="c">
			<asp:Label ID="lblTotalCustom" runat="server" Text="Total Custom Items:"  Width="150px"></asp:Label>
		</td>
		<td class="v">
			<asp:Label ID="lblTotalCustomDisp" runat="server" Text="" ></asp:Label>
		</td>
	</tr>
	<tr>
		<td align="right" class="c">
			<asp:Label ID="lblMargAbovCos" runat="server" Text="Margin Above Cost:" ></asp:Label>
		</td>
		<td class="v">
			<asp:Label ID="lblMargAbovCosDisp" runat="server" Text="" ></asp:Label>
		</td>
	</tr>
	<tr>
		<td align="right" class="c">
			<asp:Label ID="lblMargAbov" runat="server" Text="Branch Margin if we get this?" ></asp:Label>
		</td>
		<td class="v">
			<asp:Label ID="lblTotalMargAbovDisp" runat="server" Text="" ></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="middle">
		</td>
		<td>&nbsp;

		</td>
		<td align="right" class="c">
			<asp:Label ID="lblTotalMaterial" runat="server" Text="Total ~Ext'd Material:"></asp:Label>
		</td>
		<td class="v">
			<asp:Label ID="lblTotalMaterialDisp" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="middle">
		</td>
		<td>
		</td>
		<td align="right" class="c">
			<asp:Label ID="lblTotalLabor" runat="server" Text="Total ~Ext'd Labour:"></asp:Label>
		</td>
		<td class="v">
			<asp:Label ID="lblTotalLaborDisp" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="middle">
		</td>
		<td>
		</td>
		<td align="right" class="c">
			<asp:Label ID="lblTotalQuotedLabor" runat="server" Text="Total Quoted hours:"></asp:Label>
		</td>
		<td class="v">
			<asp:Label ID="lblTotalQuotedLaborDisp" runat="server"></asp:Label>
		</td>
	</tr>
								</table>

							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="pnl_WODetails" runat="server" Width="100%" Visible="false">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent3" runat="server">
								<asp:Panel ID="WOTotalsPanel" runat="server" Visible="False" Width="100%">
									<table cellpadding="5" cellspacing="0" width="100%" >
										
									    <tr>
                                            <td nowrap="nowrap" style="vertical-align: top;">
												<asp:Table ID="TotalsTable" runat="server" CellPadding="2" CellSpacing="0" Font-Names="Arial" Visible="False">
													<asp:TableHeaderRow runat="server" BackColor="#309144" ForeColor="#FFFFFF">
														<asp:TableHeaderCell runat="server" Height="0px" HorizontalAlign="Left" Wrap="False">
															<asp:Label runat="server" Text="Bill Types" Font-Bold="True" Font-Names="Arial" Width="100px" ID="lblLineTotals" Visible="False"></asp:Label>
														</asp:TableHeaderCell>
														<asp:TableHeaderCell runat="server" Height="0px" HorizontalAlign="Right" Wrap="False">
															<asp:Label runat="server" Text="Labour" Font-Bold="True" Font-Names="Arial" Width="100px" ID="lblLabour1" Visible="False"></asp:Label>
														</asp:TableHeaderCell>
														<asp:TableHeaderCell runat="server" Height="0px" HorizontalAlign="Right" Wrap="False">
															<asp:Label runat="server" Text="Material" Font-Bold="True" Font-Names="Arial" Width="100px" ID="lblMaterial1" Visible="False"></asp:Label>
														</asp:TableHeaderCell>
														<asp:TableHeaderCell runat="server" Height="0px" HorizontalAlign="Right" Wrap="False">
															<asp:Label runat="server" Text="Bill Type Totals" Font-Bold="True" Font-Names="Arial" Width="100px" ID="lblTotal1" Visible="False"></asp:Label>
														</asp:TableHeaderCell>
													</asp:TableHeaderRow>
													<asp:TableRow runat="server">
														<asp:TableCell runat="server" HorizontalAlign="Left">
															<asp:Label runat="server" Text="Regular " Font-Bold="True" Font-Names="Arial" ID="lblRegular1" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblRegularLabour2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblRegularMaterial2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblRegularTotal2" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow runat="server">
														<asp:TableCell runat="server" HorizontalAlign="Left">
															<asp:Label runat="server" Text="Visible No Charge " Font-Bold="True" Font-Names="Arial" ID="lblVNC1" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblVNCLabour2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblVNCMaterial2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblVNCTotal2" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow runat="server">
														<asp:TableCell runat="server" HorizontalAlign="Left">
															<asp:Label runat="server" Text="Blended " Font-Bold="True" Font-Names="Arial" ID="lblBlended1" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblBlendedLabour2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblBlendedMaterial2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblBlendedTotal2" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow runat="server">
														<asp:TableCell runat="server" HorizontalAlign="Left">
															<asp:Label runat="server" Text="Invisible Credit " Font-Bold="True" Font-Names="Arial" ID="lblICR1" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblICRLabour2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblICRMaterial2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblICRTotal2" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow runat="server">
														<asp:TableCell runat="server" HorizontalAlign="Left">
															<asp:Label runat="server" Text="Do Not Include " Font-Bold="True" Font-Names="Arial" ID="lblDNI1" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblDNILabour2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblDNIMaterial2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblDNITotal2" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow runat="server">
														<asp:TableCell runat="server" HorizontalAlign="Left">
															<asp:Label runat="server" Text="Visible Credit " Font-Bold="True" Font-Names="Arial" ID="lblVC1" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblVCLabour2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblVCMaterial2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblVCTotal2" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow runat="server">
														<asp:TableCell runat="server" HorizontalAlign="Left">
															<asp:Label runat="server" Text="Total" Font-Bold="True" Font-Names="Arial" ID="lblBTTotals" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblLabour2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Names="Arial" ID="lblMaterial2" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell runat="server" HorizontalAlign="Right">
															<asp:Label runat="server" Font-Bold="True" Font-Names="Arial" ID="lblTotal2" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
                                            </td>
                                            <td align="right" valign="top">
                                                <dx:ASPxLabel ID="lbl_discount_warning" runat="server" Text=""  Font-Size="14"></dx:ASPxLabel>
                                            </td>
                                            
                                        </tr>
									</table>
								</asp:Panel>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					
					 <dx:ASPxPanel ID="pnl_addnewline" runat="server" Width="100%" Font-Names="Calibri,Arial">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent4" runat="server">   
                                <span class="section_label">Add New Item</span>
								<table id="add_line_table" border="0" cellpadding="3" cellspacing="0" class="add_line" width="100%">
                                    <thead>
                                        <tr>
                                            <th id="add_hc2" runat="server" data-name="MatType">
                                                <asp:Label ID="lblPartType" runat="server" Text="Source"></asp:Label>
                                            </th>
                                            <th id="add_hc4" runat="server" data-name="PartNo">
                                                <asp:Label ID="lblPartNo" runat="server" Text="Master ID"></asp:Label>
                                            </th>
                                            <th id="add_hc6" runat="server" data-name="Description">
                                                <asp:Label ID="lblMatDescription" runat="server" Text="Description"></asp:Label>
                                            </th>
                                            <th id="add_hc7" runat="server" data-name="LaborDescription">
                                                <asp:Label ID="lblLabourDescription" runat="server" Text="Description"></asp:Label>
                                            </th>
                                            <th id="add_hc8" runat="server" data-name="QTY">
                                                <asp:Label ID="lblQty" runat="server" Text="QtyReq"></asp:Label>
                                            </th>
                                            <th id="add_hc24" runat="server" data-name="REQ Qty">
                                                <asp:Label ID="lblReqQty" runat="server" Text="Comd"></asp:Label>
                                            </th>
                                            <th id="add_hc27" runat="server" data-name="Loc">Loc</th>
                                            <th id="add_hc23" runat="server" data-name="AvailQTY">
                                                <asp:Label ID="lblAvailQty" runat="server" Text="Avl"></asp:Label>
                                            </th>
                                            <th id="add_hc9" runat="server" data-name="Cost">
                                                <asp:Label ID="LabelCost" runat="server" Text="Cost"></asp:Label>
                                            </th>
                                            <th id="add_hc10" runat="server" data-name="Tm_SELL">
                                                <asp:Label ID="lblTM_Sell" runat="server" Text="Bench"></asp:Label>
                                            </th>
                                            <th id="add_hc13" runat="server" data-name="TMExtd">
                                                <asp:Label ID="lblTMExtd" runat="server" Text=" Ext'd"></asp:Label>
                                            </th>
                                            <th id="add_hc26" runat="server" data-name="ReqDate">Req. Date</th>
                                            <th id="add_hc20" runat="server" class="buttons" data-name="Buttons">
                                                <asp:Label ID="lblButtons" runat="server" Visible="False"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td id="add_bc2" runat="server" class="td">
                                                <asp:DropDownList ID="ddlMatType" runat="server" AutoPostBack="True" CssClass="ddlMatType" Height="20px" OnSelectedIndexChanged="ddlMatType_SelectedIndexChanged" Width="100%">
                                                    <asp:ListItem Selected="True" Text="Matl" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td id="add_bc4" runat="server" class="td">
                                                <asp:TextBox ID="TextPartNo" runat="server" CssClass="TextPartNo part_no" onblur="part_handler(this, event);" onkeydown="var key= event.keyCode ? event.keyCode : event.which;if(key == 13){return false;}"></asp:TextBox>
                                            </td>
                                            <td id="add_bc6" runat="server" class="td">
                                                <asp:TextBox ID="TextDescription" runat="server" AutoCompleteType="Disabled" CssClass="Description"></asp:TextBox>
                                            </td>
                                            <td id="add_bc7" runat="server" class="td">
												<div id="matTypeGroup" runat="server" Visible="False" class="addline_container">
													<div>
														<dx:ASPxComboBox ID="cbAddlineGroup" runat="server" AnimationType="None" AutoPostBack="True" CallbackPageSize="50" ClientInstanceName="cbAddlineGroup" DropDownRows="15" DropDownWidth="500px" EnableCallbackMode="True" IncrementalFilteringDelay="250" OnSelectedIndexChanged="cbAddlineGroup_SelectedIndexChanged" RenderIFrameForPopupElements="True" TextField="Name" ValueField="ID" ValueType="System.Int32" Width="100%">
															<ItemStyle HorizontalAlign="Left" />
														</dx:ASPxComboBox>
													</div>
												</div>
												<div id="matTypeQuote" runat="server" Visible="False" class="addline_container">
													<div>
														<dx:ASPxComboBox ID="cbAddlineQuote" runat="server" AnimationType="None" AutoPostBack="True" CallbackPageSize="50" ClientInstanceName="cbAddlineQuote" DropDownRows="15" DropDownWidth="500px" EnableCallbackMode="True" IncrementalFilteringDelay="250" OnSelectedIndexChanged="cbAddlineQuote_SelectedIndexChanged" RenderIFrameForPopupElements="True" TextField="Name" ValueField="ID" ValueType="System.Int32" Width="100%">
															<ItemStyle HorizontalAlign="Left" />
														</dx:ASPxComboBox>
													</div>
												</div>
												<div id="matTypeWorkOrder" runat="server" Visible="False" class="addline_container">
													<div class="customer_container">
														<dx:ASPxComboBox ID="cbAddlineCustomer" runat="server" AnimationType="None" AutoPostBack="True" CallbackPageSize="50" ClientInstanceName="cbAddlineCustomer" DropDownRows="15" DropDownWidth="500px" EnableCallbackMode="True" IncrementalFilteringDelay="250" OnSelectedIndexChanged="cbAddlineCustomer_SelectedIndexChanged" RenderIFrameForPopupElements="True" TextField="Name" ValueField="ID" ValueType="System.Int32" Width="100%">
															<ItemStyle HorizontalAlign="Left" />
														</dx:ASPxComboBox>
													</div>
													<div class="workorder_container">
														<dx:ASPxComboBox ID="cbAddlineWorkOrder" runat="server" AnimationType="None" AutoPostBack="True" CallbackPageSize="50" ClientInstanceName="cbAddlineWorkOrder" DropDownRows="15" DropDownWidth="500px" EnableCallbackMode="True" IncrementalFilteringDelay="250" OnSelectedIndexChanged="cbAddlineWorkOrder_SelectedIndexChanged" RenderIFrameForPopupElements="True" TextField="Name" ValueField="ID" ValueType="System.Int32" Width="100%">
															<ItemStyle HorizontalAlign="Left" />
														</dx:ASPxComboBox>
													</div>
												</div>
                                            </td>
                                            <td id="add_bc8" runat="server" class="td">
                                                <dx:ASPxTextBox ID="TextQty" runat="server" BackColor="White" ClientInstanceName="ReqQty" CssClass="QtyPart" ForeColor="Black" Native="True" Width="95%">
                                                    <ClientSideEvents 
														Init="function(s, e) {$(s.mainElement).attr('autocomplete', 'off');s.SetClientVisible(true);}" 
														KeyDown="function(s, e) {only_numeric(event)}" 
														TextChanged="addline_qty_handler"
														LostFocus = "function(s,e){picklist.addline.validateQty(s,e,1)}"
														 />
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td id="add_bc24" runat="server" class="td" width="50px">
                                                <dx:ASPxTextBox ID="TextComQty" runat="server" BackColor="White" ClientInstanceName="TextComQty" CssClass="TextComQty" ForeColor="Black" Native="True" Width="95%">
                                                    <ClientSideEvents	Init="function(s, e) {s.SetClientVisible(true);}" 
																		KeyDown="function(s, e) {only_numeric(event)}" 
																		TextChanged="addline_qty_handler" 
																		LostFocus = "function(s,e){picklist.addline.validateQty(s,e,0)}"
																		/>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td id="add_bc27" runat="server" class="td">
                                                <dx:ASPxComboBox ID="addline_location" runat="server" ClientInstanceName="addline_location" CssClass="addline_location" DataSourceID="ds_addline_location" OnCallback="addline_location_cb" TextField="name" ValueField="id" Width="97%">
                                                    <ClientSideEvents EndCallback="control_location_stock" SelectedIndexChanged="control_location_stock" ValueChanged="control_location_stock" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </dx:ASPxComboBox>
                                                <asp:SqlDataSource ID="ds_addline_location" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
	a.id, 
	CONCAT(IF(a.type_id = 1, 'Inl - ', 'Exl - '), a.name,' - (', IFNULL(b.qty,0), ')') name, 
	IFNULL(b.qty,0) qty 
FROM
	inventory_location_master a 
LEFT JOIN 
	inventory_location b 
		ON b.location_master_id = a.id AND b.master_id = @master_id 
WHERE 
	a.business_unit_id = @warehouse_bu_id AND 
	a.type_id IN (1,2) 
ORDER BY 
	a.type_id ASC, b.qty DESC, b.max DESC">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="@warehouse_bu_id" SessionField="warehouse_bu_id" />
                                                        <asp:SessionParameter Name="@master_id" SessionField="addline_master_id" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </td>
                                            <td id="add_bc23" runat="server" class="td">
                                                <asp:Label ID="TextAvailQty" runat="server" CssClass="TextAvailQty qty_avail"></asp:Label>
                                            </td>
                                            <td id="add_bc9" runat="server" class="td">
                                                <dx:ASPxTextBox ID="TextCost" runat="server" ClientEnabled="False" ClientInstanceName="TextCost" CssClass="TextCost" style="text-align: right" ToolTip="You Can Only Set a Cost for a Custom Part">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td id="add_bc10" runat="server" class="td">
                                                <dx:ASPxTextBox ID="TextSell" runat="server" ClientEnabled="False" ClientInstanceName="addline_TextSell" CssClass="TextSell" HorizontalAlign="Right" style="text-align: right">
													<ClientSideEvents TextChanged="picklist.addline.extendSell"></ClientSideEvents>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td id="add_bc13" runat="server" class="td">
                                                <dx:ASPxTextBox ID="TextTMExtd" runat="server" ClientEnabled="False" ClientInstanceName="addline_TMSellExt" CssClass="TMSellExt" HorizontalAlign="Right">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td id="add_bc26" runat="server" class="td">
                                                <dx:ASPxDateEdit ID="DateRequired" runat="server" AnimationType="None" ClientInstanceName="DateRequired" EditFormat="Custom" EditFormatString="yyyy-MM-dd" Width="100%">
                                                </dx:ASPxDateEdit>
                                            </td>
                                            <td id="add_bc20" runat="server" class="td">
                                                <table cellpadding="1" cellspacing="0">
                                                    <tr>
                                                        <td valign="middle">
                                                            <dx:ASPxButton ID="btn_Save" runat="server" ClientInstanceName="btn_Save" CssClass="btn_Save" Height="25px" UseSubmitBehavior="false" ImageSpacing="0px" OnClick="add_line_save" ToolTip="Save Row" Width="20px">
                                                                <ClientSideEvents Click="check_save" />
                                                                <Image Url="~/images/icon/icon[save].gif">
                                                                </Image>
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td valign="middle">
                                                            <asp:ImageButton ID="btn_Clear" runat="server" ImageUrl="~/images/icon/icon[reset].gif" OnClientClick="return confirm('Are you sure you want to clear the add line?');" OnClick="add_line_clear" ToolTip="Clear Row" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <asp:SqlDataSource ID="sds_add_workorder_ddl" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
                                <asp:SqlDataSource ID="SqlDeptDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
                                 </br>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>

					<label id="addline_visible_reason" runat="server"></label>
					<div ID="InformationLabel" class="information_label" runat="server" style="font-weight:bold;background-color:#090;color:#fff;font-size:11px;padding:5px;display:none;"></div>
					<div ID="ErrorLabel" class="error_label" runat="server" style="font-weight:bold;background-color:#f00;color:#fff;font-size:11px;padding:5px;display:none;"></div>
					<br />
					<dx:ASPxPanel ID="pnl_gv" runat="server" Width="100%" Font-Names="Calibri,Arial">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent6" runat="server">
                                <span class="section_label" id="existingItems" runat="server">Existing Items</span>
								<dx:ASPxGridView 
									id                        = "agv" 
									cssclass                  = "master_grid" 
									clientinstancename        = "agv" 
									runat                     = "server" 
									autogeneratecolumns       = "False" 
									keyfieldname              = "id" 
									onrowupdating             = "agv_RowUpdating" 
									oncelleditorinitialize    = "agv_CellEditorInitialize" 
									onrowdeleting             = "agv_RowDeleting" 
									onhtmlrowprepared         = "agv_HtmlRowPrepared"
									onhtmldatacellprepared    = "agv_HtmlDataCellPrepared" 
									width                     = "100%"
									onheaderfilterfillitems   = "agv_HeaderFilterFillItems" 
									oncommandbuttoninitialize = "agv_CommandButtonInitialize" 
									onparsevalue              = "agv_ParseValue" 
									oncustomcallback          = "agv_CustomCallback"
									oncustombuttoninitialize  = "agv_CustomButtonInitialize" 
									oncustomjsproperties      = "agv_CustomJSProperties"
									onafterperformcallback    = "agv_DataBound"
									ondatabound               = "agv_DataBound" 
									oninit					  = "agv_DataBound"
									onhtmlfootercellprepared  = "agv_HtmlFooterCellPrepared" 
                                    OnSummaryDisplayText ="agv_OnSummaryDisplayText"
                                    OnCustomSummaryCalculate ="agv_CustomSummaryCalculate"
									font-names                = "Arial" 
									SettingsLoadingPanel-Text="" Border-BorderStyle="None">
                                    <SettingsCommandButton>
	                                    <EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px" ></EditButton>
	                                    <NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	                                    <DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
                                        <UpdateButton Text="Update" Image-Url="~/images/icon/icon[save].gif" Image-Width="16px" Image-Height="16px"></UpdateButton>
                                        <CancelButton Text="Cancel" Image-Url="~/images/icon/icon[cancel].gif" Image-Width="16px" Image-Height="16px"></CancelButton>
                                    </SettingsCommandButton>
									<Columns>
									<dx:GridViewDataTextColumn Caption="Select" Name="select" Width="30px"  EditFormSettings-Visible="False">
											<DataItemTemplate>
												<asp:CheckBox ID="chk_indiv" runat="server" AutoPostBack="false" CssClass="sel_box" onclick="row_selection_handler(this)" Enabled='True'></asp:CheckBox>
											</DataItemTemplate>
											<HeaderTemplate>
												<dx:ASPxCheckBox ID="chk_all" runat="server" AutoPostBack="false" CssClass="sel_box_all" Text="All" ReadOnly="false" Enabled='True' Width="20px">
													<ClientSideEvents CheckedChanged="do_sel_all" />
												</dx:ASPxCheckBox>
											</HeaderTemplate>
											<FooterCellStyle HorizontalAlign="Center">
											</FooterCellStyle>
											<CellStyle HorizontalAlign="Center" ></CellStyle>
											<HeaderStyle HorizontalAlign="Center" />
											<FooterTemplate>
												<dx:ASPxButton ID="chk_button_datereq" ClientVisible="false" OnClick="mass_set_date_req" ClientInstanceName="chk_button_datereq" runat="server">
												</dx:ASPxButton>
											</FooterTemplate>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataComboBoxColumn Caption="Location" FieldName="location" name="location"  Width="80px">
											<PropertiesComboBox CallbackPageSize="100" DataSourceID="ds_wo_edit_form" DropDownRows="5"
												AnimationType="None" EnableCallbackMode="True" IncrementalFilteringDelay="250"
												IncrementalFilteringMode="Contains" TextField="WorkOrder" ValueField="WOProg_ID"
												ValueType="System.Int32">
											</PropertiesComboBox>
											<DataItemTemplate>
												<dx:ASPxComboBox	id								= "combo_location" 
																	runat							= "server" 
																	callbackpagesize				= "10" 
																	dropdownrows					= "10"
																	native							= "true" 
																	cssclass						= "combo_location" 
																	incrementalfilteringmode		= "contains" 
																	enablecallbackmode				= "true" 
																	incrementalfilteringdelay		= "100" 
																	width							= "100%" 
																	autopostback					= "false" 
																	valuetype						= "System.Int32" 
																	valuefield						= "id" 
																	textfield						= "name"
																	renderiframeforpopupelements	= "true" 
																	visible							= "false"
																	animationtype					= "none" 
																	enabletheming					= "false" 
																	paddings						= "0px">
													<ClientSideEvents SelectedIndexChanged="control_location_stock" />
												</dx:ASPxComboBox>
											</DataItemTemplate>
											<CellStyle Wrap="False">
											</CellStyle>
										</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataTextColumn Caption="RecNo" FieldName="rec_no" Name="rec_no"  Width="20px">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Type" FieldName="linetype" Name="linetype"  Width="20px">
										<CellStyle HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Master ID" FieldName="master_id" Name="master_id"  Width="50">
                                            <PropertiesTextEdit>
                                                <clientsideevents textchanged="handle_part_text_changed" />
                                            </PropertiesTextEdit>
											<CellStyle CssClass="c_part_no"></CellStyle>
                                        </dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Description" FieldName="description" Name="description" minwidth="200"  Width="100%">
											<Settings AutoFilterCondition="Contains" />
											<DataItemTemplate>
												<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("description") %>'></dx:ASPxLabel>
												<br />
												<div id="tooltip" runat="server" class="tip"></div>
											</DataItemTemplate>
											<CellStyle CssClass="c_desc">
											</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Qty Req'd" FieldName="qty" Name="qty"  Width="50px">
											<PropertiesTextEdit>
												<ClientSideEvents TextChanged="grid_qty_handler" />
											</PropertiesTextEdit>
											<CellStyle HorizontalAlign="Center"  CssClass="qty_req_back" >
											</CellStyle>
											<HeaderStyle BackColor="#C0FFC0" CssClass="blacktext"/>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Qty Com'td" FieldName="qtyrec" Name="qtyrec" Width="50px">
											<PropertiesTextEdit>
												<ClientSideEvents TextChanged="grid_recdtodate_handler" />
											</PropertiesTextEdit>
											<CellStyle HorizontalAlign="Center"  CssClass="qty_com_back">
											</CellStyle>
											<HeaderStyle BackColor="#FFC0C0" CssClass="blacktext" />
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Qty Rec'd" FieldName="qty_receiving" cellstyle-cssclass="data-qty_recd"  Width="60px" Name="qty_receiving">
											<PropertiesTextEdit>
												<ClientSideEvents TextChanged="grid_recd_handler" />
											</PropertiesTextEdit>
											<DataItemTemplate>
											  <asp:TextBox ID="txtRecSelectQty" runat="server" Width="35px" onfocus="zero_chk(this, false)" onblur="zero_chk(this, true)" CssClass="recqty" style="text-align: right" Text="0" Height="16px" BorderStyle="Solid"></asp:TextBox>
											</DataItemTemplate>
											<FooterCellStyle HorizontalAlign="Center">
											</FooterCellStyle>
											<CellStyle HorizontalAlign="Center" BorderRight-BorderStyle="None"></CellStyle>
											<FooterTemplate>
												<dx:ASPxButton ID="btnUpdatePORecQty_client" runat="server" AutoPostBack="False"
													ClientInstanceName="btnUpdatePORecQty_client" ClientVisible="False" ToolTip="Since these are shipped via 'Pick Up', you can specify if they should be committed directly to the work order or not.">
													<Image Url="~/images/icon/icon[popup].gif">
													</Image>
													<ClientSideEvents Click="check_commit" />
												</dx:ASPxButton>
												<dx:ASPxButton ID="btnUpdatePORecQty" runat="server" ClientInstanceName="btnUpdatePORecQty"
													ClientVisible="False" OnClick="mass_receive" ToolTip="Update The Qty Received">
													<Image Url="~/images/icon/Icon[save-green].gif">
													</Image>
													<ClientSideEvents Click="check_commit" />
												</dx:ASPxButton>
											</FooterTemplate>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Cost" FieldName="cost" Name="cost"  Width="50px">
											<PropertiesTextEdit DisplayFormatString="#,###.00">
												<ClientSideEvents TextChanged="gv_cost_editor" />
											</PropertiesTextEdit>
											<DataItemTemplate>
										<dx:ASPxLabel ID="FieldCostTextBox" runat="server"  Text='<%# Bind("cost", "{0:C3}") %>' >
                                            
										</dx:ASPxLabel>
											</DataItemTemplate>
											 <CellStyle HorizontalAlign="Center"  CssClass="cost_back">
											</CellStyle>
											<HeaderStyle BackColor="#E0E0E0"  CssClass="blacktext"/>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Sell" FieldName="sell" Name="sell" ReadOnly="True"  Width="60px">
											<PropertiesTextEdit DisplayFormatString="#,###.00">
												<ClientSideEvents TextChanged="grid_sellchange" />
											</PropertiesTextEdit>
											<DataItemTemplate>
										<dx:ASPxLabel ID="FieldSellTextBox" runat="server" Text='<%# Bind("sell", "{0:C3}") %>'></dx:ASPxLabel>
											</DataItemTemplate>
											<CellStyle HorizontalAlign="Right"  CssClass="sell_back">
											</CellStyle>
											<HeaderStyle BackColor="#C0FFFF"  CssClass="blacktext"/>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="% Disc" FieldName="discount" Name="discount"  Width="35px">
											<PropertiesTextEdit>
												<ClientSideEvents TextChanged="grid_discount_handler" />
											</PropertiesTextEdit>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Extd" FieldName="extTandM" Name="extTandM" ReadOnly="True"  Width="60px" EditFormSettings-Visible="False">
												<PropertiesTextEdit DisplayFormatString="c2">
												</PropertiesTextEdit>
												<DataItemTemplate>
											<dx:ASPxLabel ID="FieldextdtmTextBox" runat="server" Text='<%# Bind("extTandM", "{0:C}") %>'></dx:ASPxLabel>
												</DataItemTemplate>
												<CellStyle HorizontalAlign="Right">
												</CellStyle>
											</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Unfulfilled" FieldName="unfulfilled" Name="unfulfilled" cellstyle-cssclass="data-unfulfilled"  Width="20px" ReadOnly="True">
											<PropertiesTextEdit DisplayFormatString="#,###.00">
											</PropertiesTextEdit>
											<CellStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" HorizontalAlign="Right">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataDateColumn Caption="Dt Req" FieldName="reqdate" Name="required_date"  Width="50px">
											<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" AnimationType="None">
											</PropertiesDateEdit>
											<CellStyle Font-Size="7pt" Wrap="False">
											</CellStyle>
										</dx:GridViewDataDateColumn>
									<dx:GridViewDataTextColumn Caption="Transfer" FieldName="transfer" Name="transfer"  Width="25px">
											<DataItemTemplate>
											   <a href="javascript:void(0);" onclick="TransferItems('<%# Container.KeyValue %>', false)"><img alt="Transfer" src="/images/icon/transfer.jpg" width="16" height="16" /></a>
											</DataItemTemplate>
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataComboBoxColumn Caption="Bill Type" FieldName="billtype_id" Name="billtype_id"  Width="40px">
											<PropertiesComboBox DataSourceID="SqlDataSourceBillTypes" AnimationType="None"
												TextField="wo_lineitem_billtype_name" ValueField="wo_lineitem_billtypeid" ValueType="System.String">
											</PropertiesComboBox>
											<CellStyle Wrap="False">
											</CellStyle>
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataTextColumn Caption="Activity Code"  Width="100px" Name="activity_code" FieldName="activity_code" PropertiesTextEdit-MaxLength="15" CellStyle-HorizontalAlign="Center" ></dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Cost Element"  Width="100px" Name="cost_element" FieldName="cost_element" PropertiesTextEdit-MaxLength="10" CellStyle-HorizontalAlign="Center" ></dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Client WO"  Width="100px" Name="client_wo" FieldName="client_wo" PropertiesTextEdit-MaxLength="10" CellStyle-HorizontalAlign="Center" ></dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Client PO"  Width="100px" Name="client_po" FieldName="client_po" PropertiesTextEdit-MaxLength="10" CellStyle-HorizontalAlign="Center" ></dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Notes" name="notes" Width="25px">
											<EditCellStyle HorizontalAlign="Center">
												<Paddings Padding="0px" />
											</EditCellStyle>
											<DataItemTemplate>
											   <a href="javascript:void(0);" id="linknotes"  onclick="showNotes('<%# Container.KeyValue %>',event)"><%# note_handler(Container)%></a>
											</DataItemTemplate>
									
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Hist" Name="history" Width="20px">
											<DataItemTemplate>
												<%# history_handler(Container)%>
											</DataItemTemplate>
											<EditItemTemplate>
												<%# history_handler(Container)%>
											</EditItemTemplate>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Issues" Name="issues"  Width="25px">
											<DataItemTemplate>
												<%# issue_handler(Container) %>
											</DataItemTemplate>
											<EditItemTemplate>
												<%# issue_handler(Container) %>
											</EditItemTemplate>
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Margin" FieldName="margin" Name="margin"  Width="40px">
											<PropertiesTextEdit EnableFocusedStyle="False">
											</PropertiesTextEdit>
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Origin" FieldName="origin" Name="origin"  Width="25px">
											<DataItemTemplate>
												<%# origin_handler(Container)%>
											</DataItemTemplate>
											<EditItemTemplate>
												<%# origin_handler(Container)%>
											</EditItemTemplate>
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Avl Qty" FieldName="qty_avail"  Name="qty_avail" Tooltip="Available Quantity" Width="40px">
										</dx:GridViewDataTextColumn>
									<dx:GridViewDataCheckColumn Caption="Track" FieldName="track_part" Tooltip="Track Part" Name="track_part" Width="30px">
										</dx:GridViewDataCheckColumn>
									<dx:GridViewDataTextColumn Caption="membertypename" FieldName="membertypename" Name="membertypename"  Visible="False">
									</dx:GridViewDataTextColumn>
			                        <dx:GridViewCommandColumn ButtonType="Image" Caption=" " Name="buttons" Tooltip="Buttons"  Width="40px" MinWidth="75"  ShowEditButton="true" ShowDeleteButton="true" ShowUpdateButton="true" ShowCancelButton="true" ShowClearFilterButton="true"  >
											<CustomButtons>
												<dx:GridViewCommandColumnCustomButton ID="delete_multiple">
													<Image ToolTip="Delete Selected lines" Url="~/images/icon/icon[deletemultiple].gif">
													</Image>
												</dx:GridViewCommandColumnCustomButton>
												<dx:GridViewCommandColumnCustomButton id="partially_billed">
													<Image ToolTip="Is this line partial billed?" Url="~/images/icon/checkbox.png"></Image>
												</dx:GridViewCommandColumnCustomButton>
											</CustomButtons>
										</dx:GridViewCommandColumn>
									</Columns>
									<SettingsPager NumericButtonCount="25" PageSize="50" Position="TopAndBottom"/>
									<SettingsEditing Mode="Inline" />
									<Settings ShowFooter="True" ShowTitlePanel="false" ShowHeaderFilterButton="True" ShowFilterBar="Visible" ShowFilterRow="true" />
									<TotalSummary>
										<dx:ASPxSummaryItem FieldName="extended_per" SummaryType="sum" DisplayFormat="n" />
										<dx:ASPxSummaryItem FieldName="extTandM" SummaryType="sum" DisplayFormat="c" />
                                        <dx:ASPxSummaryItem FieldName="qty" SummaryType="sum" DisplayFormat="n" />
                                        <dx:ASPxSummaryItem FieldName="qtyrec" SummaryType="sum" DisplayFormat="n" />
										<dx:ASPxSummaryItem FieldName="sell" SummaryType="sum" DisplayFormat="c" />
                                        <dx:ASPxSummaryItem FieldName="cost" SummaryType="sum" DisplayFormat="c" />
									</TotalSummary>
									<SettingsBehavior AllowFocusedRow="False" ConfirmDelete="True" AllowDragDrop="false" ColumnResizeMode="Control" AutoFilterRowInputDelay="6000" />
									<Styles FilterBar-CssClass="fb_class">
										<AlternatingRow CssClass="ar" />
										
										<Footer CssClass="f" />
										<Header CssClass="h" />
										<LoadingPanel HorizontalAlign="Center" VerticalAlign="Top" Border-BorderStyle="None" />
										<LoadingDiv Opacity="0" Border-BorderStyle="None" />
									</Styles>
									<ClientSideEvents EndCallback="main_gv_endcallback" CustomButtonClick="custom_button_click" FocusedRowChanged="row_focus" />
									<Images>
										<LoadingPanelOnStatusBar Url="/images/loading_panel.gif" />
										<LoadingPanel Url="/images/loading_panel.gif" />
									</Images>
									<SettingsDetail ShowDetailRow="false"></SettingsDetail>
									<SettingsLoadingPanel ImagePosition="Top" />
									<Templates>
										<HeaderCaption>
											<div title="<%# Container.Column.Caption %>"><%# Container.Column.Caption %></div>
										</HeaderCaption>
										<DetailRow>
											asdf
										</DetailRow>
									</Templates>
								</dx:ASPxGridView>

							<asp:HiddenField ID="which_footer_save" runat="server" />
								
								<asp:SqlDataSource ID="ds_wo_edit_form" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
									ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
									></asp:SqlDataSource>
								<asp:SqlDataSource ID="SqlDataSourceBillTypes" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT wo_lineitem_billtypeid, wo_lineitem_billtype_name FROM wo_detail_lineitem_billtype">
								</asp:SqlDataSource>
								<asp:SqlDataSource ID="SqlDeptDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
								</asp:SqlDataSource>
								&nbsp;&nbsp;
								</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>

		<dx:ASPxPopupControl ID="pop_commit" runat="server" ClientInstanceName="pop_commit"
			CloseAction="CloseButton" AnimationType="None" 
									HeaderText="Which of these part  do you want to commit directly to work order?" 
									PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
			ShowShadow="False" Width="750px" Modal="True" AllowDragging="True">
			<ContentCollection>
				<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
					<dx:ASPxPanel ID="panel_pop" runat="server" ClientInstanceName="panel_pop" Width="100%">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent7" runat="server">
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<table cellpadding="5" cellspacing="0" width="225">
						<tr>
							<td>
					<dx:ASPxButton ID="btnUpdatePORecQty_pop" runat="server" ClientInstanceName="final_receival" OnClick="mass_receive"
						Text="Receive Parts">
						<ClientSideEvents Click="function(s, e) {
	pop_commit.Hide();
}" />
						<Image Url="~/images/icon/icon[save].gif">
						</Image>
					</dx:ASPxButton>
							</td>
							<td>
								<dx:ASPxButton ID="bt_cancel" runat="server" AutoPostBack="False" Text="Cancel" UseSubmitBehavior="False">
									<ClientSideEvents Click="function(s, e) {
	pop_commit.Hide();
}" />
									<Image Url="~/images/icon/icon[undo].gif">
									</Image>
								</dx:ASPxButton>
							</td>
						</tr>
					</table>
				</dx:PopupControlContentControl>
			</ContentCollection>
			<ModalBackgroundStyle Opacity="0">
			</ModalBackgroundStyle>
			<HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
		</dx:ASPxPopupControl>
		<dx:ASPxPopupControl ID="ASPxpuNotes" runat="server" ClientInstanceName="NotesPopUp"
			CloseAction="CloseButton" HeaderText="Notes" Modal="True" 
			ShowPageScrollbarWhenModal="True" PopupHorizontalAlign="WindowCenter" ModalBackgroundStyle-BackColor="Transparent" 
			PopupVerticalAlign="WindowCenter" AllowDragging="True" ContentStyle-Paddings-Padding="2px">
			<ContentCollection>
				<dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
					<table>
						<tr>
							<td valign="top">
							<div style="width:250px;overflow-y:scroll;height:250px;white-space:pre;font-size:11px;" class="note_history" id="note_history" runat="server"></div>
							</td>
							<td valign="top">
							<div style="font-size:11px;"><b>New Note</b></div>
					<asp:TextBox ID="textNotes" CssClass="textNotes" runat="server" Rows="4" TextMode="MultiLine" Width="300px" Height="150px"></asp:TextBox><br /><br />
					<asp:Button ID="imgbnotesupdate" runat="server"  Text="Save" ToolTip="Save" OnClick="imgbnotesupdate_Click" CssClass="imgbnotesupdate" />
					<asp:Button ID="imgbtnPOLineActive" runat="server" Text="Save" ToolTip="Save" CssClass="imgbtnPOLineActive" />
						</td>
						</tr>
					</table>
				</dx:PopupControlContentControl>
			</ContentCollection>
			<HeaderStyle BackColor="Gray" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" ForeColor="White">
			</HeaderStyle>
			<ClientSideEvents Shown="function(s,e){$('.textNotes').focus();}" Closing="function(s, e) {
		agv.PerformCallback();
		 document.getElementById('ctl00_cphMasterBody_ASPxpuNotes_textNotes').value = '';
}" />
			<HeaderImage Url="~/images/FullNotes.JPG">
			</HeaderImage>
		</dx:ASPxPopupControl>
		<dx:ASPxPopupControl ID="Popup_GroupSelect" runat="server" HeaderText="Group Listing" 
			Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
			Width="800px" ShowPageScrollbarWhenModal="True" AllowDragging="True" ModalBackgroundStyle-BackColor="Transparent" AllowResize="True">
			<ContentCollection>
				<dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
					<table style="width: 100%;">
						<tr id="td_grouppanel_date_row" runat="server">
							<td width="200">
								<asp:Label ID="lb_required" runat="server" Style="font-weight: 700" Text="Date Required (for all items):" Visible="False"></asp:Label>
							</td>
							<td>
								<dx:ASPxDateEdit ID="date_required" runat="server" Visible="False">
								</dx:ASPxDateEdit>
							</td>
							<td align="right">
							<dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="popup_addpart" Text="Apply" ToolTip="Press this button to transfer all selected parts back to the worksheet">
						<ClientSideEvents Click="function(s, e) {
	try
		{
		if(ddlSection.GetSelectedIndex() == 0 &amp;&amp; ddlSection.GetItemCount() &gt; 1)
			{
			ddlSection.SetSelectedIndex(1);
			}
		else
			{
			return false;
			}
		}
	catch(err)
		{
		return true;
		}
}" />
					</dx:ASPxButton>
							</td>
						</tr>
					</table>
					<dx:ASPxGridView ID="Grid_GroupSelect" runat="server" AutoGenerateColumns="False" DataSourceID="sds_groupselect" ClientInstanceName="Grid_GroupSelect" 
						KeyFieldName="groupselectid" OnHtmlDataCellPrepared="Grid_GroupSelect_HtmlDataCellPrepared" OnCommandButtonInitialize="Grid_GroupSelect_CommandButtonInitialize" Width="100%">
						<Columns>
							<dx:GridViewCommandColumn Caption=" " ShowSelectCheckbox="True" Name="chk" VisibleIndex="0">
								<HeaderCaptionTemplate>
									<dx:ASPxCheckBox ID="chkGroupsSelectAll" runat="server" Font-Size="8pt" Text="Select All" Wrap="False">
										<ClientSideEvents CheckedChanged="function(s, e) {
	Grid_GroupSelect.SelectAllRowsOnPage(s.GetChecked());
}" />
									</dx:ASPxCheckBox>
								</HeaderCaptionTemplate>
							</dx:GridViewCommandColumn>
							<dx:GridViewDataTextColumn Caption="Part No" FieldName="master_id" Name="master_id"
								ReadOnly="True" VisibleIndex="2" Width="40px">
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Description" FieldName="description" Name="description"
								ReadOnly="True" VisibleIndex="3" Width="100%">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Qty" FieldName="Qty" Name="Qty" ToolTip="Enter Amount to Transfer"
								VisibleIndex="4" Width="40px">
								<DataItemTemplate>
									<asp:TextBox ID="txtGroupSelectQty" runat="server" 
										Width="40px" Text='<%# Bind("Qty")%>' BackColor="Cornsilk" cssclass="group_qty" autocomplete="off"  onkeydown="picklist.group_pop.down(this, event);" onkeyup="picklist.group_pop.up(this, event);">
									</asp:TextBox>
								</DataItemTemplate>
								<HeaderStyle HorizontalAlign="Center" />
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Repair ID" FieldName="RepairID" Visible="False"
								VisibleIndex="1">
							</dx:GridViewDataTextColumn>
						</Columns>
						<SettingsPager NumericButtonCount="20" PageSize="20" Mode="ShowAllRecords">
							<AllButton Text="All">
							</AllButton>
							<NextPageButton Text="Next &gt;">
							</NextPageButton>
							<PrevPageButton Text="&lt; Prev">
							</PrevPageButton>
						</SettingsPager>
					</dx:ASPxGridView>
					<div align="right">
					</div>
					<asp:SqlDataSource ID="sds_groupselect" runat="server" OnLoad="sds_groupselect_Load" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" SelectCommand="" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
				</dx:PopupControlContentControl>
			</ContentCollection>
			<ClientSideEvents Closing="function(s,e){$('.ddlMatType').val('1').change();}"></ClientSideEvents>
		</dx:ASPxPopupControl>
		<dx:ASPxPopupControl ID="pop_xfer" 
			runat="server" ClientInstanceName="TransferPopUp" 
			CloseAction="CloseButton" HeaderText="Transfer Parts" Modal="True" 
			PopupHorizontalAlign="NotSet"  PopupVerticalAlign="NotSet" PopupVerticalOffset="300"
			Width="600px" ShowPageScrollbarWhenModal="True" AllowDragging="True" ModalBackgroundStyle-Opacity="0">
			<ModalBackgroundStyle Opacity="0">
            </ModalBackgroundStyle>
			<ContentCollection>
				<dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
					<dx:ASPxCallbackPanel ID="xfer_cbp" ClientInstanceName="xfer_cbp" runat="server" Width="100%" OnCallback="xfer_cbp_Callback">
						<PanelCollection>
							<dx:PanelContent>
					<dx:ASPxPanel ID="xfer_single" ClientInstanceName="xfer_single" runat="server" ClientVisible="false">
						<PanelCollection>
							<dx:PanelContent ID="xfer_single_pan1" runat="server">
					<table style="width:600px;">
						<tr>
							<td style="width: 100px">
								<asp:Label ID="TranPartID" runat="server" Font-Bold="True" Font-Names="Arial" Text="Part Number"></asp:Label>
							</td>
							<td colspan="2">
								<asp:Label ID="lblTranPartID" runat="server" CssClass="lblTranPartID" Font-Names="Arial"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Label ID="TranQTY" runat="server" Font-Bold="True" Font-Names="Arial" Text="Quantity Left"></asp:Label>
							</td>
							<td colspan="2">
								<asp:Label ID="lblTranQty" runat="server" CssClass="lblTranQty" Font-Names="Arial"></asp:Label>
								<input type="hidden" runat="server" ID="hidmaxqty"  class="hidmaxqty" />
							</td>
						</tr>
						<tr>
							<td>Origin</td>
							<td colspan="2">
								<asp:Label ID="lblTTOrigin" runat="server" CssClass="lblOrigin" Font-Names="Arial"></asp:Label>
							</td>
						</tr>
					</table></dx:PanelContent>
					</PanelCollection>
					</dx:ASPxPanel>
					
					<dx:ASPxPanel ID="xfer_results" ClientInstanceName="xfer_results" runat="server" ClientVisible="false">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent11" runat="server">
								<dx:ASPxLabel ID="lb_xfer_results" runat="server" EncodeHtml="false"></dx:ASPxLabel>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
					<dx:ASPxPanel ID="xfer_multiple" ClientInstanceName="xfer_multiple" runat="server" ClientVisible="false">
						<PanelCollection>
							<dx:PanelContent ID="xfer_multiple_pan1" runat="server">
								<dx:ASPxGridView ID="gv_xfer_multiple" ClientInstanceName="gv_xfer_multiple" runat="server" OnInit="gv_xfer_multiple_Init" OnHtmlDataCellPrepared="gv_xfer_multiple_HtmlDataCellPrepared" Width="100%" KeyFieldName="id">
									<Settings ShowTitlePanel="true" />
									<Columns>
										<dx:GridViewDataColumn Name="part_n" FieldName="part_n" Caption="Part #">
											<DataItemTemplate>
												<input type="hidden" class="xfer_hid_id" value="<%# Eval("id") %>"/>
												<span class="xfer_lb_part_no"><%# Eval("part_n") %></span>
											</DataItemTemplate>
											<Settings AllowSort="False" />
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</dx:GridViewDataColumn>
										<dx:GridViewDataColumn Name="qty" FieldName="qty" Caption="Xfer Qty">
											<DataItemTemplate>
												<input type='text' class="xfer_lb_qty" id="xfer_lb_qty" onkeydown='only_int(event)' onkeyup="control_min_max(this, 0, $(this).attr('data-max'))" runat="server" data-max='<%# Eval("qty") %>' value='<%# Eval("qty") %>' style='width:35px;'/>
											</DataItemTemplate>
											<Settings AllowSort="False" />
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</dx:GridViewDataColumn>
										<dx:GridViewDataColumn Name="origin" FieldName="origin" Caption="Origin">
											<DataItemTemplate>
												<span class="xfer_lb_origin"><%# Eval("origin") %></span>
											</DataItemTemplate>
											<Settings AllowSort="False" />
											<CellStyle HorizontalAlign="Left"></CellStyle>
										</dx:GridViewDataColumn>
										<dx:GridViewDataColumn Name="location" Caption="Location">
											<DataItemTemplate>
												<dx:ASPxComboBox ID="loc" Native="true" Width="100%" CssClass="loc_combo" ValueField="id" TextField="name" DataSourceID="loc_ds" runat="server" />
												<asp:SqlDataSource ID="loc_ds" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" />
											</DataItemTemplate>
											<Settings AllowSort="False" />
											<CellStyle HorizontalAlign="Left"></CellStyle>
										</dx:GridViewDataColumn>
										<dx:GridViewDataColumn Name="use_wo" Caption="Use WO">
											<DataItemTemplate>
												<input type="checkbox" onclick="use_wo_handler(this)" class="xfer_chk_wo" />
											</DataItemTemplate>
											<Settings AllowSort="False" />
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</dx:GridViewDataColumn>
									</Columns>
									<Templates>
										<TitlePanel>
											<dx:ASPxCheckBox ID="check_all" runat="server" ClientSideEvents-CheckedChanged="function(s,e){$('.xfer_chk_wo').each(function(){$(this).attr('checked', s.GetChecked());use_wo_handler(this);});}" Text="Toggle All"></dx:ASPxCheckBox>
										</TitlePanel>
									</Templates>
									<SettingsPager PageSize="50"></SettingsPager>
								</dx:ASPxGridView>

								<br />
								OR select a work order:<br />
								Please also make sure the "USE WO" check box is checked for any lines you wish to transfer to the below work order. <br />
								If the check box is not checked, the part will transfer to the location denoted on the line.<br />
							</dx:PanelContent></PanelCollection>
					</dx:ASPxPanel>
								<table style="width:450px;" id="tbl_xfer_single" runat="server">
									<tr>
										<td>
											&nbsp;</td>
										<td colspan="2">
											&nbsp;</td>
									</tr>
									<tr>
										<td colspan="2">
											<dx:ASPxLabel ID="xfer_wo_lb" runat="server" Font-Bold="True" Font-Names="Arial" ClientVisible="true" Text="Select Work Order"></dx:ASPxLabel>
										</td>
													<td align='center' id="tbl_xfer_qty_wo_hdr" runat="server"><b>QTY</b></td>
									</tr>
												<tr>
													<td colspan="2">
														<dx:ASPxComboBox ID="xfer_wo_combo" runat="server" ClientVisible="true" DataSourceID="ds_open_workorders" AnimationType="None" Native="true" EnableCallbackMode="false" CallbackPageSize="100" IncrementalFilteringMode="Contains" TextField="WorkOrder" ValueField="woprogid" Width="400px" ClientInstanceName="xfer_wo_combo">
															<ClientSideEvents SelectedIndexChanged="" />
														</dx:ASPxComboBox>
														<asp:SqlDataSource ID="ds_open_workorders" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
													</td>
													<td align="center">
														<dx:ASPxTextBox ID="xfer_wo_qty" CssClass="xfer_wo_qty" runat="server" Width="44px" ClientInstanceName="xfer_wo_qty">
															<ClientSideEvents TextChanged="function(s, e) {
		 var qtytt = s.GetText();
		 var qtycur = $('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_hidorigttamt').val() / 1;
		 var this_xfer_wo_qty	= 0;
		 var this_xfer_int_qty	= 0;
		 var this_xfer_ext_qty	= 0;
		 try
			{
			this_xfer_wo_qty	= xfer_wo_qty.GetValue() / 1;
			}
		catch(ex){alert(ex)}
		 try
			{
			this_xfer_int_qty	= xfer_internal_qty.GetValue() / 1;
			}
		catch(ex){alert(ex)}
		 try
			{
			this_xfer_ext_qty	= xfer_external_qty.GetValue() / 1;
			}
		catch(ex){alert(ex)}
		 var newqtytt = qtycur - this_xfer_wo_qty - this_xfer_int_qty - this_xfer_ext_qty;
		 if(newqtytt &lt; 0)
			{
			newqtytt = 0;
			s.SetValue(0);
			}
		
		 $('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_xfer_single_lblTranQty').html(newqtytt);
		 $('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_hidTranQTY').val(newqtytt);
		 
}" />
														</dx:ASPxTextBox>
													</td>
												</tr>
												<tr id="trInternalBranchLocation" runat="server">
													<td colspan="2">
														<dx:ASPxLabel ID="xfer_internal_lb" runat="server" Font-Bold="True" Font-Names="Arial" Text="Select Internal Branch Location"></dx:ASPxLabel>
													</td>
													<td align='center' id="tbl_xfer_qty_int_hdr" runat="server"><b>QTY</b></td>
												</tr>
												<tr id="trtxtInternalBranchLocation" runat="server">
													<td colspan="2">
														<asp:HiddenField ID="xfer_internal_hid" runat="server" />
														<dx:ASPxComboBox ID="xfer_internal_combo" runat="server" AnimationType="None" TextField="name" EnableCallbackMode="false" Native="true" CallbackPageSize="100" ValueField="id" Width="400px" DataSourceID="ds_internal_locations" ClientInstanceName="xfer_internal_combo">
															<ClientSideEvents SelectedIndexChanged="" />
														</dx:ASPxComboBox>
														<asp:SqlDataSource ID="ds_internal_locations" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, CONCAT(a.name,' - (', IFNULL(b.qty,0), ') - ', c.ddl_name) name, IFNULL(b.qty,0) qty FROM inventory_location_master a LEFT JOIN inventory_location b ON b.location_master_id = a.id AND b.master_id = @master_id LEFT JOIN business_unit c ON a.business_unit_id = c.id WHERE a.business_unit_id = @warehouse_bu_id AND a.type_id = 1">
															<SelectParameters>
																<asp:SessionParameter Name="@warehouse_bu_id" SessionField="warehouse_bu_id" />
																<asp:SessionParameter Name="@master_id" SessionField="xfer_master_id" />
															</SelectParameters>
														</asp:SqlDataSource>
													</td>
													<td align="center">
														<dx:ASPxTextBox ID="xfer_internal_qty" runat="server" Width="44px" ClientInstanceName="xfer_internal_qty">
															<ClientSideEvents TextChanged="function(s, e) {
		 var qtytt = s.GetText();
		 var qtycur = $('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_hidorigttamt').val() / 1;
		 var this_xfer_wo_qty	= 0;
		 var this_xfer_int_qty	= 0;
		 var this_xfer_ext_qty	= 0;
		 try
			{
			this_xfer_wo_qty	= xfer_wo_qty.GetValue() / 1;
			}
		catch(ex){alert(ex)}
		 try
			{
			this_xfer_int_qty	= xfer_internal_qty.GetValue() / 1;
			}
		catch(ex){alert(ex)}
		 try
			{
			this_xfer_ext_qty	= xfer_external_qty.GetValue() / 1;
			}
		catch(ex){alert(ex)}
		 var newqtytt = qtycur - this_xfer_wo_qty - this_xfer_int_qty - this_xfer_ext_qty;
		 if(newqtytt &lt; 0)
			{
			newqtytt = 0;
			s.SetValue(0);
			}
		 $('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_xfer_single_lblTranQty').html(newqtytt);
		 $('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_hidTranQTY').val(newqtytt);
		 
}" />
														</dx:ASPxTextBox>
													</td>
												</tr>
												<tr id="trBranchLocation" runat="server" >
													<td colspan="2">
														<dx:ASPxLabel ID="xfer_external_lb" runat="server" Font-Bold="True" Font-Names="Arial" Text="Select External Location" ClientInstanceName="xfer_external_lb"></dx:ASPxLabel>
													</td>
													<td align='center' id="tbl_xfer_qty_ext_hdr" runat="server"><b>QTY</b></td>
												</tr>
												<tr  id="trtxtBranchLocation" runat="server">
													<td colspan="2">
														<asp:HiddenField ID="xfer_external_hid" runat="server" />
														<dx:ASPxComboBox ID="xfer_external_combo" runat="server" AnimationType="None" IncrementalFilteringMode="Contains" Native="true" EnableCallbackMode="false" CallbackPageSize="100" TextField="name" ValueField="id" Width="400px" ClientInstanceName="xfer_external_combo" DataSourceID="ds_external_locations">
															<ClientSideEvents SelectedIndexChanged="" />
														</dx:ASPxComboBox>
														<asp:SqlDataSource ID="ds_external_locations" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, CONCAT(a.name,' - (',IFNULL(b.qty,0), ') - ', c.ddl_name) name, IFNULL(b.qty,0) qty FROM inventory_location_master a LEFT JOIN inventory_location b ON b.location_master_id = a.id AND b.master_id = @master_id LEFT JOIN business_unit c ON a.business_unit_id = c.id WHERE a.business_unit_id = @warehouse_bu_id AND a.type_id = 2">
															<SelectParameters>
																<asp:SessionParameter Name="@warehouse_bu_id" SessionField="warehouse_bu_id" />
																<asp:SessionParameter Name="@master_id" SessionField="xfer_master_id" />
															</SelectParameters>
														</asp:SqlDataSource>
													</td>
													<td align="center">
														<dx:ASPxTextBox ID="xfer_external_qty" runat="server" Width="44px" ClientInstanceName="xfer_external_qty">
															<ClientSideEvents TextChanged="function(s, e) {
		 var qtytt = s.GetText();
		 var qtycur = $('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_hidorigttamt').val() / 1;
		 var this_xfer_wo_qty	= 0;
		 var this_xfer_int_qty	= 0;
		 var this_xfer_ext_qty	= 0;
		 try
			{
			this_xfer_wo_qty	= xfer_wo_qty.GetValue() / 1;
			}
		catch(ex){alert(ex)}
		 try
			{
			this_xfer_int_qty	= xfer_internal_qty.GetValue() / 1;
			}
		catch(ex){alert(ex)}
		 try
			{
			this_xfer_ext_qty	= xfer_external_qty.GetValue() / 1;
			}
		catch(ex){alert(ex)}
		 var newqtytt = qtycur - this_xfer_wo_qty - this_xfer_int_qty - this_xfer_ext_qty;
		 if(newqtytt &lt; 0)
			{
			newqtytt = 0;
			s.SetValue(0);
			}
		 $('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_xfer_single_lblTranQty').html(newqtytt);
		 $('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_hidTranQTY').val(newqtytt);
		 
}" />
														</dx:ASPxTextBox>
													</td>
												</tr>
									<tr>
										<td colspan="2">
											&nbsp;</td>
									</tr>
									<tr>
										<td colspan="3">
											<asp:Label ID="lblTransferReason" runat="server" Font-Bold="True" Font-Names="Arial" Text="Reasons for Transfer" ></asp:Label>
										</td>
									</tr>
									<tr>
										<td colspan="3">
											<asp:TextBox ID="txtTransferReason" runat="server" Rows="2" CssClass="txtTransferReason" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td colspan="3">
											<table width='100%'>
												<tr>
													<td>
											<dx:ASPxButton ID="xfer_save" runat="server" OnClick="PartTransfer" Text="Transfer">
												<ClientSideEvents Click="" />
												<Image Url="~/images/icon/icon[save].gif">
												</Image>
											</dx:ASPxButton>
													</td>
												</tr>
											</table>
											<asp:HiddenField ID="hidttwoprogid" runat="server" />
											<asp:HiddenField ID="hidorigttamt" runat="server" />
											<asp:HiddenField ID="hidTranQTY" runat="server" />
											<asp:HiddenField ID="hidTTPartNo" runat="server" />
										</td>
									</tr>
								</table>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxCallbackPanel>
				</dx:PopupControlContentControl>
			</ContentCollection>
			<ClientSideEvents Shown="function(s, e) {
	xfer_wo_combo.SetSelectedIndex(-1);
}" Closing="function(s,e) {agv.Refresh();}" />
			<HeaderStyle BackColor="Gray" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" ForeColor="White" />
			<HeaderImage Height="16" Url="~/images/icon/transfer.jpg" Width="16">
			</HeaderImage>
		</dx:ASPxPopupControl>
		<dx:ASPxPopupControl ID="ASPxpuHistory" runat="server" ClientInstanceName="HistoryPopUp"
			CloseAction="CloseButton" HeaderText="History" Modal="True" 
			PopupHorizontalAlign="NotSet"  PopupVerticalAlign="NotSet" PopupVerticalOffset="100" 
			ShowPageScrollbarWhenModal="True" AllowDragging="True" ModalBackgroundStyle-Opacity="0">
			<ContentCollection>
				<dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
					<asp:TextBox ID="textHistory" CssClass="textHistory" runat="server" Rows="4" TextMode="MultiLine" Width="385px" Height="100px" ReadOnly="True"></asp:TextBox>
					&nbsp;&nbsp;<br />
				</dx:PopupControlContentControl>
			</ContentCollection>
			<HeaderStyle BackColor="Gray" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" ForeColor="White">
			</HeaderStyle>
			<ClientSideEvents Closing="function(s, e) {
			agv.Refresh();
		 document.getElementById('ctl00_cphMasterBody_ASPxpuNotes_textNotes').value = '';
}" />
			<HeaderImage Url="~/images/FullNotes.JPG">
			</HeaderImage>
		</dx:ASPxPopupControl>
		<asp:GridView ID="gv_PartsDeleted" runat="server" Visible="false" AllowPaging="true" AllowSorting="true">
		</asp:GridView>
		<input id="hidID" runat="server" class="hid_id" type="hidden" />
		<input id="hidWorkingBusinessUnitID" runat="server" class="hid_working_bu_id" type="hidden" />
		<input id="hidWarehouseBusinessUnitID" runat="server" class="hid_warehouse_bu_id" type="hidden" />
		<input id="hidSellPrice" runat="server" class="hid_sellprice" type="hidden" />
        <input id="hidfixed_markup" runat="server" class="hidfixed_markup" type="hidden" />
        <input id="hidfixed_labour" runat="server" class="hidfixed_labour" type="hidden" />
        <input id="hidfixed_markup_value" runat="server" class="hidfixed_markup_value" type="hidden" />
        <input id="hidfixed_labour_value" runat="server" class="hidfixed_labour_value" type="hidden" />
		<asp:HiddenField ID="hidDivisionID" runat="server" />
		<asp:HiddenField ID="hidNotes" runat="server" />
		<asp:HiddenField ID="hidNotesID" runat="server" />
		<asp:HiddenField ID="hidVendorID" runat="server" />
		<asp:HiddenField ID="hdnMU_Parameters" runat="server" />
		<asp:HiddenField ID="hidForceClickable" runat="server" />
		<asp:HiddenField ID="hidHistoryID" runat="server" />
		<asp:HiddenField ID="hidWODelete" runat="server" />
		<asp:HiddenField ID="hidPartIsExclude" runat="server" />
		<dx:ASPxHiddenField ID="hid_row" runat="server" ClientInstanceName="hid_row">
		</dx:ASPxHiddenField>
		<asp:HiddenField ID="hidWODiscount" runat="server" />
	    <input id="hiddenIsCreditWorkerOrder" runat="server" class="hiddenIsCreditWorkerOrder" type="hidden" />
		<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True" ContainerElementID="btn_Save">
				<LoadingDivStyle Opacity="0">
				</LoadingDivStyle>
		</dx:ASPxLoadingPanel>
		<a name="top"/>
		
		
	<dx:ASPxPopupControl ID="pop_date_required" runat="server" 
			HeaderText="Change Required Date for Selected Lines" Modal="True" 
			PopupHorizontalAlign="NotSet"  PopupVerticalAlign="NotSet" PopupVerticalOffset="200" 
			Width="400px" ClientInstanceName="pop_date_required" AnimationType="None" 
			AllowDragging="True" CloseAction="CloseButton" ModalBackgroundStyle-Opacity="0">
			<ModalBackgroundStyle Opacity="0"></ModalBackgroundStyle>
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControl11" runat="server">
				<div style="position:relative;width:350px;height:75px;">
					<div style="position:absolute;left:0px;">Date:</div>
					<div style="position:absolute;left:150px;"><dx:ASPxDateEdit ID="mass_date_req" ClientInstanceName="mass_date_req" runat="server" /></div>
					<div style="position:absolute;left:150px;top:35px;"><dx:ASPxButton ID="bt_mass_date_req" Text="Set Dates" OnClick="mass_set_date_req" Image-Url="~/images/icon/icon[calendar].gif" runat="server" >
						<ClientSideEvents Click="dt_req_click" /></dx:ASPxButton></div>
				</div>
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
	<dx:ASPxPopupControl ID="pop_billtype_edit" runat="server" 
			HeaderText="Change Bill Type for Selected Lines" Modal="True" 
			PopupHorizontalAlign="NotSet"  PopupVerticalAlign="NotSet" PopupVerticalOffset="200"
			Width="400px" ClientInstanceName="pop_billtype_edit" AnimationType="None" 
			AllowDragging="True" CloseAction="CloseButton">
			<ModalBackgroundStyle Opacity="0"></ModalBackgroundStyle>
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControl12" runat="server">
				<div style="position:relative;width:350px;height:75px;">
					<div style="position:absolute;left:0px;">Bill Type:</div>
					<div style="position:absolute;left:150px;"><dx:ASPxComboBox ID="mass_billtype_edit" DataSourceID="ds_pop_billtypes" TextField="name" ValueField="id" ClientInstanceName="mass_billtype_edit" runat="server" /></div>
					<div style="position:absolute;left:150px;top:35px;"><dx:ASPxButton ID="bt_mass_billtype_edit" Text="Set Billtypes" OnClick="mass_set_billtype" Image-Url="~/images/icon/icon[accounting].gif" runat="server" >
						<ClientSideEvents Click="billtype_edit_click" /></dx:ASPxButton></div>
				</div>
								<asp:SqlDataSource ID="ds_pop_billtypes" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" SelectCommand="SELECT wo_lineitem_billtypeid id, wo_lineitem_billtype_name name FROM wo_detail_lineitem_billtype" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
								</asp:SqlDataSource>
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
	</contenttemplate>
	</ASP:UPDATEPANEL></div>
	</ASP:CONTENT>
