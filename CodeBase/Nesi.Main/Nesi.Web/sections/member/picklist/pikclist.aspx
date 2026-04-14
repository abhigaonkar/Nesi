<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" EnableTheming="false" validateRequest="false"  enableEventValidation="false" Theme="" Inherits="sections_member_picklist_pikclist" Codebehind="pikclist.aspx.cs" %>
<%@ Import Namespace="nesi.core" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="head_content" ContentPlaceHolderID="header_placeholder" runat="server">
<script type="text/javascript">
function bc(master_id) {
createPopupWin("/_tools/print_barcode/index.aspx?master_id=" + master_id, "Print Barcode", 600, 200);
//popbcprint.SetHeaderText("Print Labels for: " + master_id);
            //hidbcmastertid.Set("id",master_id);
            //popbcprint.Show();
}
        function createPopupWin(pageURL, pageTitle,
            popupWinWidth, popupWinHeight) {
            var left = (screen.width - popupWinWidth) / 2;
            var top = (screen.height - popupWinHeight) / 4;
            var myWindow = window.open(pageURL, pageTitle,
                'resizable=yes, width=' + popupWinWidth
                + ', height=' + popupWinHeight + ', top='
                + top + ', left=' + left);
        } 
    </script>
<style type="text/css">
    .dxgvPreviewRow td {
       font-family: Arial;
font-size: 8pt;
vertical-align: Top;
padding-top:2px!important;
     
        padding-left:15px!important;
    }
	</style>
</asp:Content>
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<a name="top"></a>
	<script type="text/javascript" src="/js/picklist.js?unique=<%=Toolbox.do_RandomString(10) %>"></script>
	<div id="picklist">
	<button type="submit" onclick="return false" style="display:none" data-whythisexists="Prevent default submission when pressing enter">&nbsp;</button>
		<table style="padding-top: 10px; padding-right: 5px; padding-left: 0px; border-collapse: collapse;">
			<tr>
				<td>
					<dx:ASPxButton ID="btnPrintAll" runat="server" AutoPostBack="False" 
						Visible="False" BackColor="Transparent" 
						ToolTip="Print all bar codes for this PO" Height="25px" Width="25px">
						<Image Url="~/images/icon/icon[print_barcode].GIF" Width="20px">
						</Image>
						<ClientSideEvents Click="print_all" />
					</dx:ASPxButton>
				</td>
				<td>
					
				</td>
			</tr>
		</table>
        <dx:ASPxButton ID="hid_btn_exporter" runat="server" Text="ASPxButton" ClientInstanceName="hid_btn_exporter" OnClick="btnXlsxExport_Click"  ClientVisible="False" ></dx:ASPxButton>
          
    <asp:ScriptManager runat="server" ID="ScriptManager1" OnAsyncPostBackError="Script_error"></asp:ScriptManager>
    <ASP:UPDATEPANEL id="UpdatePanel1" runat="server">
        
    <contenttemplate>
<dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"  GridViewID="agv">
                                </dx:ASPxGridViewExporter>
       
	<div id="stuff" runat="server"></div>
        <dx:ASPxRoundPanel ID="rp_Main" runat="server" Width="100%" HeaderText="Pick List" Border-BorderWidth="0px" BackColor="White" >
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <dx:ASPxPanel ID="pnl_header" runat="server" Width="100%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
								<table width="100%" cellpadding="2" cellspacing="0" class="header_labels">
									<tr>
										<td align="right" width="100" class="c"></td>
										<td align="left" class="v"></td>
										<td align="right" class="total_quoted_c">
											<asp:Label ID="lblQuoted1" runat="server" Text="Quoted Price (Gen Info Tab):"></asp:Label></td>
										<td align="left" width="150" class="total_quoted_v">
											<asp:Label ID="lblQuotedDisp1" runat="server" Text=""></asp:Label></td>
									</tr>
									<tr>
										<td align="right" width="100" class="c">
											<asp:Label ID="lblCusotmerName" runat="server" Text="Customer Name:"></asp:Label></td>
										<td align="left" class="v">
											<asp:Label ID="lblCustNameDisp" runat="server" Text=""></asp:Label>
											<br />
											<dx:ASPxTextBox ID="txtGroupName" runat="server" Visible="False" Width="100%">
											</dx:ASPxTextBox>
											<asp:Label ID="lblerrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="" Visible="False" Width="100%"></asp:Label></td>
										<td align="right" class="c">
											<asp:Label ID="lblQuoted" runat="server" Text="Total ~Ext'd Sell:"></asp:Label></td>
										<td align="left" width="150" class="v">
											<asp:Label ID="lblQuotedDisp" runat="server" Text=""></asp:Label></td>
									</tr>
									<tr>
										<td align="right" class="c">
											<asp:Label ID="lblMemberName" runat="server" Text="Employee Name:"></asp:Label></td>
										<td rowspan="3" style="vertical-align: top" valign="top" class="v">
											<asp:Label ID="lblMembNameDisp" runat="server" Text="" Width="100%"></asp:Label>
											<dx:ASPxMemo ID="txtGroupNotes" runat="server" Visible="False" Width="100%"></dx:ASPxMemo>
										</td>
										<td align="right" class="c">
											<asp:Label ID="lblTM" runat="server" Text="Total Benchmark Sell (Ext'd):"></asp:Label>
										</td>
										<td class="v">
											<asp:Label ID="lblTMDisp" runat="server" Text=""></asp:Label>
										</td>
									</tr>
									<tr>
										<td align="right" class="c" colspan="3">
											<asp:Label ID="lb_benchextddiff" runat="server" Text="Bench & ~Extd Difference:"></asp:Label>
										</td>
										<td class="v">
											<asp:Label ID="lbl_benchextddiff" runat="server" Text=""></asp:Label>
										</td>
									</tr>
									<tr>
										<td align="right" class="c" colspan="3">
											<asp:Label ID="lblCost" runat="server" Text="Total Cost:"></asp:Label>
										</td>
										<td class="v">
											<asp:Label ID="lblCostDisp" runat="server" Text=""></asp:Label>
										</td>
									</tr>
									<tr>
										<td align="right" class="c" style="height: 22px" colspan="3">
											<asp:Label ID="lblTotalItems" runat="server" Text="Total Items:"></asp:Label>
										</td>
										<td class="v" style="height: 22px">
											<asp:Label ID="lblTotalItemsDisp" runat="server" Text=""></asp:Label>
										</td>
									</tr>
									<tr>
										<td rowspan="3" valign="middle">&nbsp;&nbsp;<dx:ASPxButton ID="btnSaveHeader" runat="server" Height="14px" OnClick="btnSaveHeader_Click" Visible="False" Width="0px">
											<Image Url="~/images/icon/icon[save].gif"></Image>
										</dx:ASPxButton>
										</td>
										<td rowspan="3">

											<dx:ASPxCheckBox ID="chkQuoteDiscount" ClientInstanceName="quotediscount" runat="server" Wrap="false" Visible="false" AutoPostBack="True" OnCheckedChanged="chkApplyDiscount_CheckedChanged" Text="Customer Discount Applied?" />
											<dx:ASPxCheckBox ID="btn_use_current_cost" runat="server" Wrap="false" Visible="false" AutoPostBack="True" OnCheckedChanged="btn_use_current_cost_click" Text="Show &quot;Today's Prices&quot;" />
										</td>
										<td align="right" class="c">
											<asp:Label ID="lblTotalCustom" runat="server" Text="Total Custom Items:" Width="150px"></asp:Label>
										</td>
										<td class="v">
											<asp:Label ID="lblTotalCustomDisp" runat="server" Text=""></asp:Label>
										</td>
									</tr>
									<tr>
										<td align="right" class="c">
											<asp:Label ID="lblMargAbovCos" runat="server" Text="Margin Above Cost (from quoted price):"></asp:Label>
										</td>
										<td class="v">
											<asp:Label ID="lblMargAbovCosDisp" runat="server" Text=""></asp:Label>
										</td>
									</tr>
									<tr>
										<td align="right" class="c">
											<asp:Label ID="lblMargAbovCos_dollars" runat="server" Text="Margin $ Above Cost (from quoted price):"></asp:Label>
										</td>
										<td class="v">
											<asp:Label ID="lblMargAbovCosDisp_dollars" runat="server" Text=""></asp:Label>
										</td>
									</tr>
									<tr>
										<td valign="middle"></td>
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
										<td valign="middle"></td>
										<td></td>
										<td align="right" class="c">
											<asp:Label ID="lblTotalLabor" runat="server" Text="Total ~Ext'd Labour:"></asp:Label>
										</td>
										<td class="v">
											<asp:Label ID="lblTotalLaborDisp" runat="server"></asp:Label>
										</td>
									</tr>
									<tr>
										<td valign="middle"></td>
										<td></td>
										<td align="right" class="c">
											<asp:Label ID="lblTotalKitted" runat="server" Text="Total ~Ext'd Kitted:"></asp:Label>
										</td>
										<td class="v">
											<asp:Label ID="lblTotalKittedDisp" runat="server"></asp:Label>
										</td>
									</tr>
									<tr>
										<td valign="middle"></td>
										<td></td>
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
									<table cellpadding="5" cellspacing="0" width="100%">
										<tr>
											<td style="vertical-align: top;">
												<asp:Table ID="TotalsTable" runat="server" Visible="False" Font-Names="Arial">
													<asp:TableHeaderRow BackColor="#80FF80">
														<asp:TableHeaderCell Height="0px" HorizontalAlign="Left" Wrap="false">
															<asp:Label ID="lblLineTotals" runat="server" Font-Bold="True" Font-Names="Arial" Width="100px" Text="Bill Types" Visible="False"></asp:Label>
														</asp:TableHeaderCell>
														<asp:TableHeaderCell Height="0px" HorizontalAlign="Right" Wrap="false">
															<asp:Label ID="lblLabour1" runat="server" Font-Bold="True" Font-Names="Arial" Width="100px" Text="Labour" Visible="False"></asp:Label>
														</asp:TableHeaderCell>
														<asp:TableHeaderCell Height="0px" HorizontalAlign="Right" Wrap="false">
															<asp:Label ID="lblMaterial1" runat="server" Font-Bold="True" Font-Names="Arial" Width="100px" Text="Material" Visible="False"></asp:Label>
														</asp:TableHeaderCell>
														<asp:TableHeaderCell Height="0px" HorizontalAlign="Right" Wrap="false">
															<asp:Label ID="lblTotal1" runat="server" Font-Bold="True" Font-Names="Arial" Width="100px" Text="Bill Type Totals" Visible="False"></asp:Label>
														</asp:TableHeaderCell>
													</asp:TableHeaderRow>
													<asp:TableRow>
														<asp:TableCell HorizontalAlign="Left">
															<asp:Label ID="lblRegular1" runat="server" Text="Regular " Font-Names="Arial" Visible="False" Font-Bold="True"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblRegularLabour2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblRegularMaterial2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblRegularTotal2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell HorizontalAlign="Left">
															<asp:Label ID="lblVNC1" runat="server" Text="Visible No Charge " Font-Names="Arial" Visible="False" Font-Bold="True"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblVNCLabour2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblVNCMaterial2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblVNCTotal2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell HorizontalAlign="Left">
															<asp:Label ID="lblBlended1" runat="server" Text="Blended " Font-Names="Arial" Visible="False" Font-Bold="True"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblBlendedLabour2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblBlendedMaterial2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblBlendedTotal2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell HorizontalAlign="Left">
															<asp:Label ID="lblICR1" runat="server" Text="Invisible Credit " Font-Names="Arial" Visible="False" Font-Bold="True"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblICRLabour2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblICRMaterial2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblICRTotal2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell HorizontalAlign="Left">
															<asp:Label ID="lblDNI1" runat="server" Text="Do Not Include " Font-Names="Arial" Visible="False" Font-Bold="True"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblDNILabour2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblDNIMaterial2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblDNITotal2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell HorizontalAlign="Left">
															<asp:Label ID="lblVC1" runat="server" Text="Visible Credit " Font-Names="Arial" Visible="False" Font-Bold="True"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblVCLabour2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblVCMaterial2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblVCTotal2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell HorizontalAlign="Left">
															<asp:Label ID="lblBTTotals" runat="server" Text="Total" Font-Names="Arial" Visible="False" Font-Bold="True"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblLabour2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblMaterial2" runat="server" Font-Names="Arial" Visible="False"></asp:Label>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:Label ID="lblTotal2" runat="server" Font-Names="Arial" Font-Bold="True" Visible="False"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
											</td>
											<td style="vertical-align: top;">
												<asp:Label ID="lbCommentPopup" runat="server" Visible="False"></asp:Label>
											<asp:CheckBox CssClass="chkdiscount" ID="chkApplyDiscount" runat="server" AutoPostBack="True" OnCheckedChanged="chkApplyDiscount_CheckedChanged" Text="Discount Applied?" />
											</td>
										</tr>
									</table>
								</asp:Panel>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
                     <dx:ASPxPanel ID="pnl_addnewline" runat="server" Width="100%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent4" runat="server">   
                    <div id="loading_time" runat="server"></div>
								<div id="div_refactor" runat="server" class="alert" onclick="refactor_handler(this)">
									<b style="font-size:11px;"><img alt="attention" src="/images/icon/icon[attention].gif" /> Duplicate parts exist in this worksheet with differing sell prices.<br /> Click to refactor the duplicates</b>
								</div><br />Add New Item:<br />
                    		<table width="100%" border="0" cellpadding="3" cellspacing="0" id="add_line_table" class='add_line'>
								<thead>
									<tr>
										<th id="add_hc1"  data-name="Sections" runat="server"><asp:Label ID="lblsections" runat="server" Text="Sections" Width="70px"></asp:Label></th>
										<th id="add_hc2" data-name="MatType" runat="server"><asp:Label ID="lblPartType" runat="server" Text="Source"></asp:Label></th>
										<th id="add_hc3" data-name="WorkOrder" runat="server"><asp:Label ID="lblWorkOrder" runat="server" Text="Work Order"></asp:Label></th>
										<th id="add_hc21" data-name="Department" runat="server"></th>
										<th id="add_hc4" data-name="PartNo" runat="server"><asp:Label ID="lblPartNo" runat="server" Text="Master ID"></asp:Label></th>
										<th id="add_hc5" data-name="VendorPartNo" runat="server"><asp:Label ID="lblVendorPartNo" runat="server" Text="Vend Part#"></asp:Label></th>
										<th id="add_hc6" data-name="Description" runat="server"><asp:Label ID="lblMatDescription" runat="server" Text="Description"></asp:Label></th>
										<th id="add_hc7" data-name="LaborDescription" runat="server"><asp:Label ID="lblLabourDescription" runat="server" Text="Description"></asp:Label></th>
										<th id="add_hc8" data-name="QTY" runat="server"><asp:Label ID="lblQty" runat="server" Text="Qty"></asp:Label></th>
										<th id="add_hc24" data-name="REQ Qty" runat="server"><asp:Label ID="lblReqQty" runat="server" Text="Comd"></asp:Label></th>
										<th id="add_hc27" data-name="Loc" runat="server">Loc</th>
										<th id="add_hc23" data-name="AvailQTY" runat="server"><asp:Label ID="lblAvailQty" runat="server"  Text="Avl"></asp:Label></th>
										<th id="add_hc9" data-name="Cost" runat="server"><asp:Label ID="LabelCost" runat="server" Text="Cost"></asp:Label></th>
										<th id="add_hc10" data-name="Tm_SELL" runat="server"><asp:Label ID="lblTM_Sell" runat="server" Text="Bench"></asp:Label></th>
										<th id="add_hc22" data-name="Discount" runat="server" style="white-space:nowrap;"><asp:Label ID="lblDsct" runat="server" Text="% Disc"></asp:Label></th>
										<th id="add_hc11" data-name="DateExpected" runat="server"><asp:Label ID="lblDate_Expected" runat="server" Text="Date Exp" Visible="False"></asp:Label></th>
										<th id="add_hc12" data-name="QtyPerPart" runat="server"><asp:Label ID="lblQtyPerPart" runat="server" Text="Qty / Part" Visible="False"></asp:Label></th>
										<th id="add_hc13" data-name="TMExtd" runat="server"><asp:Label ID="lblTMExtd" runat="server" Text=" Ext'd"></asp:Label></th>
										<th id="add_hc26" data-name="ReqDate" runat="server">Req. Date</th>
										<th id="add_hc14" data-name="~Extd" runat="server"><asp:Label ID="lblQuotedExtd" runat="server" Text="~ Ext'd"></asp:Label></th>
										<th id="add_hc15" data-name="Include" runat="server"><asp:Label ID="lblInclude" runat="server" Text="Include" Visible="False"></asp:Label></th>
										<th id="add_hc16" data-name="NewLine" runat="server"><asp:Label ID="lblNewLine" runat="server" Text="New Line" Visible="False"></asp:Label></th>
										<th id="add_hc25" data-name="TrackPart" runat="server"><asp:Label ID="lblTrackPart" runat="server" Text="Track" Visible="False"></asp:Label></th>
										<th id="add_hc17" data-name="Image" runat="server"></th>
										<th id="add_hc18" data-name="BillType" runat="server"><asp:Label ID="lblbilltype" runat="server" Text="Bill Type" Visible="False"></asp:Label></th>
										<th id="add_hc19" data-name="Notes" runat="server"><asp:Label ID="lblNotes" runat="server" Visible="False">Notes*</asp:Label></th>
										<th id="add_hc20" data-name="Buttons" runat="server" class="buttons"><asp:Label ID="lblButtons" runat="server" Visible="False"></asp:Label></th>
									</tr>
                                </thead>
                                <tfoot>
                                    <tr>
                                       <td id="add_lc1" class='td' runat="server" colspan="20">
										   &nbsp;<asp:Label ID="lblQuoteLabDesc" runat="server" Text="Enter a Description to be Appended to the Labour Description if you want (Optional)"></asp:Label>
										   <br />
                                           <asp:TextBox ID="txtQuoteLabDesc" runat="server"></asp:TextBox>
                                       </td>
                                     </tr>
                                </tfoot>
                                <tbody>
									<tr>
										<td id="add_bc1" runat="server" class="td" align="center" width="200">
												<dx:ASPxComboBox ID="ddlSection" runat="server" CssClass="add_section" ValueType="System.String" Width="200px" TextField="section" ValueField="id" BackColor="White" ClientInstanceName="ddlSection" OnCallback="ddlSection_Callback" EnableSynchronization="True" AnimationType="None" CallbackPageSize="10"><ItemStyle HorizontalAlign="Left" BackColor="White" >
															<HoverStyle BackColor="#1199FF" ForeColor="White">
															</HoverStyle>
															<SelectedStyle BackColor="#1199FF">
															</SelectedStyle>
														</ItemStyle>
															<ListBoxStyle BackColor="White">
															</ListBoxStyle>
													<ClientSideEvents ButtonClick="function(s, e) {
	persist_sections.Set(&quot;last_deleted_id&quot;, &quot;&quot;);
	switch(e.buttonIndex)
		{
		case 0:
			btn_Save.SetEnabled(false);
			btnAddSection.SetText(&quot;Save&quot;);
			section_popup.Show();
			txtSectionName.Focus();
			persist_sections.Set(&quot;type&quot;, &quot;edit&quot;);
			persist_sections.Set(&quot;id&quot;, s.GetValue());
			lst_addsections_section.PerformCallback();
		break;
		case 1:
			btn_Save.SetEnabled(false);
			btnAddSection.SetText(&quot;Add&quot;);
			section_popup.Show();
			btnSectionDelete.SetEnabled(false);
			txtSectionName.Focus();
			persist_sections.Set(&quot;type&quot;, &quot;add&quot;);
			persist_sections.Set(&quot;id&quot;, &quot;0&quot;);
			persist_sections.Set(&quot;section_name&quot;, &quot;&quot;);
			lst_addsections_section.PerformCallback();
		break;
		}
}" ValueChanged="function(s, e) {
	s.mainElement.title = s.GetText();
}" EndCallback="function(s, e) {
	if(persist_sections.Get(&quot;last_deleted_id&quot;) != persist_sections.Get(&quot;ddlselected&quot;) &amp;&amp; (persist_sections.Get(&quot;id&quot;) == persist_sections.Get(&quot;ddlselected&quot;) || persist_sections.Get(&quot;id&quot;) == &quot;0&quot; || persist_sections.Get(&quot;last_deleted_id&quot;) == &quot;&quot;))
		{
		s.SetValue(persist_sections.Get(&quot;ddlselected&quot;));
		}
	//alert(&quot;type:&quot;+persist_sections.Get(&quot;type&quot;)+&quot;\nid:&quot;+persist_sections.Get(&quot;id&quot;)+&quot;\nddlselected:&quot;+persist_sections.Get(&quot;ddlselected&quot;)+&quot;\nlast_deleted_id:&quot;+persist_sections.Get(&quot;last_deleted_id&quot;));
	persist_sections.Set(&quot;id&quot;, &quot;0&quot;);
}" Init="function(s, e) {
	s.mainElement.title = s.GetText();
}" />
													<Buttons>
														<dx:EditButton ImagePosition="Right">
															<Image Url="~/images/icon/icon[edit].gif">
															</Image>
														</dx:EditButton>
														<dx:EditButton ImagePosition="Right">
															<Image Url="~/images/icon/icon[add].gif">
															</Image>
														</dx:EditButton>
													</Buttons>
														</dx:ASPxComboBox>
                                </td>
                                <td id="add_bc2" runat="server" class="td">
                                    <asp:DropDownList ID="ddlMatType" CssClass="ddlMatType" runat="server" 
										AutoPostBack="True" OnSelectedIndexChanged="ddlMatType_SelectedIndexChanged" 
										Width="100%">
                                    <asp:ListItem Text="Matl" Value="1" Selected="true"></asp:ListItem>
                                    <asp:ListItem Text="Labr" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                 <td id="add_bc3" class='td' align="center" runat="server">
                                    <dx:ASPxComboBox ID="ddlWorkOrder" runat="server"  OnCustomJSProperties="TextWorkOrder_CustomJSProperties"
                                        ValueType="System.String" TextField="WorkOrder" ValueField="WOProg_ID" IncrementalFilteringMode="Contains" Width="100%"  DataSourceID="sds_add_workorder_ddl" ClientInstanceName="WOHeadComboBox" AnimationType="None" EnableCallbackMode="False">
                                        <ClientSideEvents Init="function(s, e) {
	     WOHeadComboBox.defaultDropDownHeight = '';
         showorhidden_expense_categories(); 
}" SelectedIndexChanged="ddlwo_switch" />
										<ItemStyle HorizontalAlign="Left" />
                                    </dx:ASPxComboBox>
									<div style="position:relative" align="center">
									<div style="position:absolute;top:2px;color:#fff;background-color:#f00;padding:5px;display:none;font-weight:bold;width:95%;" id="lbl_gl_warning">Items cut to GL's will not be received into stock.</div>
									</div>
                                </td>
                                <td id="add_bc21" class='td' runat="server">
                                </td>
                                
                                         
                                <td id="add_bc4" class='td' runat="server">
                                    <asp:TextBox ID="TextPartNo" CssClass="TextPartNo part_no" runat="server" onkeydown="var key= event.keyCode ? event.keyCode : event.which;if(key == 13){return false;}" onblur="part_handler(this, event);"></asp:TextBox>  
                                </td>
                                <td id="add_bc5" class='td' runat="server">
									<dx:ASPxComboBox ID="TextVendorPartNo" runat="server" CssClass="VendPartNo" DataSourceID="ds_vendpart"
										TextField="code" ValueField="id" ValueType="System.String" Width="99%" ClientInstanceName="TextVendorPartNo" OnCallback="TextVendorPartNo_Callback" EncodeHtml="False" OnCustomJSProperties="TextVendorPartNo_CustomJSProperties" AnimationType="None">
										<ClientSideEvents BeginCallback="picklist.vendor_part_no.begin" EndCallback="picklist.vendor_part_no.end" ButtonClick="picklist.vendor_part_no.click" SelectedIndexChanged="picklist.vendor_part_no.index_change" />
										<ItemStyle Font-Bold="False" HorizontalAlign="Left" />
										<Buttons>
											<dx:EditButton>
												<Image Url="~/images/icon/icon[add].gif">
												</Image>
											</dx:EditButton>
										</Buttons>
									</dx:ASPxComboBox>
									<asp:SqlDataSource ID="ds_vendpart" runat="server" SelectCommand="SELECT id,UPPER(vendor_code) code FROM inventory_price WHERE master_id = @master_id AND  vendor_id = @vendor_id AND business_unit_id = @business_unit_id;" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
										<SelectParameters>
											<asp:ControlParameter ControlID="TextPartNo" Name="@master_id" PropertyName="Text" DbType="String" />
											<asp:ControlParameter ControlID="hidVendorID" Name="@vendor_id" PropertyName="Value" DbType="String" />
											<asp:ControlParameter ControlID="hidWarehouseBusinessUnitID" Name="@business_unit_id" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
								</td>
								<td id="add_bc6" class='td' runat="server">
                                     <asp:TextBox ID="TextDescription" runat="server" CssClass="Description"></asp:TextBox>
                                </td>
                                <td id="add_bc7" class='td' runat="server">
									<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent5" runat="server">
									<table width="100%" cellpadding="2" cellspacing="0">
										<tr>
											<td width='50%'><dx:ASPxComboBox ID="ddlMemberName" runat="server" ValueType="System.String" TextField="MemberName" ValueField="member_id" Enabled="False" IncrementalFilteringMode="Contains" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlMemberName_SelectedIndexChanged" ClientInstanceName="ddlMemberName" DropDownRows="10" AnimationType="None">
												<ItemStyle HorizontalAlign="Left" />
											</dx:ASPxComboBox>
                                                <dx:ASPxComboBox ID="ddlMemberType" runat="server" TextField="MemberType" ValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlMemberType_SelectedIndexChanged" Width="100%" AnimationType="None">
												<ItemStyle HorizontalAlign="Left" />
											</dx:ASPxComboBox>
											</td>
											<td width='50%'><dx:ASPxComboBox ID="ddlLabourChargeType" runat="server" ValueType="System.String" TextField="Description" ValueField="PayTypeHours_ID" Enabled="False" AutoPostBack="True" OnSelectedIndexChanged="ddlLabourChargeType_SelectedIndexChanged" Width="100%" ClientInstanceName="ddlLabourChargeType" AnimationType="None">
												<ItemStyle HorizontalAlign="Left" />
											</dx:ASPxComboBox>
											<dx:ASPxComboBox ID="ddlKittedParts" runat="server" AutoPostBack="True" DropDownWidth="500px" ClientInstanceName="add_kit"
											IncrementalFilteringMode="Contains" OnSelectedIndexChanged="ddlKittedParts_SelectedIndexChanged"
											ValueType="System.Int32" Width="100%" TextField="Name" ValueField="ID" DropDownRows="15" IncrementalFilteringDelay="250" CallbackPageSize="50" AnimationType="None" EnableCallbackMode="True" RenderIFrameForPopupElements="True" >
												<ItemStyle HorizontalAlign="Left" />
												<ClientSideEvents Init="function(s, e) {kit_builder();}" SelectedIndexChanged="function(s, e) {kit_builder();}" />
											</dx:ASPxComboBox>
											</td>
										</tr>
									</table>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxCallbackPanel>
                                </td>
                                <td id="add_bc8" class='td' runat="server">
									<dx:ASPxTextBox ID="TextQty" runat="server" CssClass="QtyPart" 
										ClientInstanceName="QtyPartCIN" Width="95%" BackColor="White" 
										ForeColor="Black" Native="True">
                                        <ClientSideEvents Init="function(s, e) {
$(s.mainElement).attr('autocomplete', 'off');
s.SetClientVisible(true);	
}"
                                            KeyDown="function(s, e) {
only_numeric(e);
	
}"
                                            TextChanged="qty_handler" />
									</dx:ASPxTextBox>
                                </td>
                                <td id="add_bc24" class='td' runat="server" width="50px">
                                <dx:ASPxTextBox ID="TextComQty" runat="server" CssClass="TextComQty" 
										ClientInstanceName="TextComQty" Width="95%" BackColor="White" 
										ForeColor="Black" Native="True">
                                <ClientSideEvents Init="function(s, e) {
s.SetClientVisible(true);	
}" TextChanged="qty_handler" KeyDown="function(s, e) {
	only_numeric(event)
}" />
                                </dx:ASPxTextBox>
                                </td>
								<td id="add_bc27" class='td' runat="server">
									<dx:ASPxComboBox runat="server" id="addline_location" CssClass="addline_location" ClientInstanceName="addline_location" OnCallback="addline_location_cb" Width="97%" TextField="name" ValueField="id" DataSourceID="ds_addline_location" IncrementalFilteringMode="Contains">
										<ItemStyle HorizontalAlign="Left" />
										<ClientSideEvents SelectedIndexChanged="control_location_stock" EndCallback="control_location_stock" ValueChanged="control_location_stock" />
									</dx:ASPxComboBox>
									<asp:SqlDataSource runat="server" id="ds_addline_location" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
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
	a.business_unit_id = @business_unit_id AND 
	a.type_id IN (1,2) 
ORDER BY 
	a.type_id ASC, b.qty DESC, b.max DESC">
										<SelectParameters>
											<asp:SessionParameter Name="@business_unit_id" SessionField="addline_business_unit_id" />
											<asp:SessionParameter Name="@master_id" SessionField="addline_master_id" />
											<asp:SessionParameter Name="@dept" SessionField="addline_dept" />
										</SelectParameters>
									</asp:SqlDataSource>
									</td>
								<td id="add_bc23" class='td' runat="server"><asp:Label ID="TextAvailQty" CssClass="TextAvailQty qty_avail" runat="server" /></td>
                                <td id="add_bc9" class='td' runat="server">
									<dx:ASPxTextBox ID="TextCost" Native="True" runat="server" 
                                        ClientInstanceName="TextCost"  CssClass="TextCost" ToolTip="You Can Only Set a Cost for a Custom Part" ClientEnabled="False">
									</dx:ASPxTextBox>
                                </td>
                                <td id="add_bc10" class='td' runat="server">
                                                       	<dx:ASPxTextBox ID="TextSell" runat="server" AutoPostBack="True" 
										CssClass="TextSell" ClientInstanceName="addline_TextSell" ClientEnabled="False" HorizontalAlign="Right" 
										OnTextChanged="TextSell_TextChanged" style="text-align: right" Text="" >
									</dx:ASPxTextBox>
                                </td>
                                <td id="add_bc22" class='td' runat="server">
                                    <asp:TextBox ID="ASPxTextDiscount" runat="server" CssClass="ASPxTextDiscount" Enabled="False" Width="40" style="text-align: right"></asp:TextBox>
                                </td>
                                <td id="add_bc11" class='td' runat="server">
                                <dx:ASPxDateEdit ID="ASPxDateExpected" ClientInstanceName="DateExpected" 
										runat="server" Width="100%" Visible="False" EditFormat="Custom" 
										EditFormatString="yyyy-MM-dd" AnimationType="None">
                                </dx:ASPxDateEdit>
                                </td>
								<td id="add_bc12" class='td' runat="server"><asp:TextBox ID="txtQtyPerPart" runat="server" Visible="False" CssClass="QtyPerPart"></asp:TextBox></td>
								<td id="add_bc13" class='td' runat="server">
									<dx:ASPxTextBox ID="TextTMExtd" runat="server" CssClass="TMSellExt" ClientInstanceName="addline_TMSellExt" 
										ClientEnabled="False" HorizontalAlign="Right" Text="">
									</dx:ASPxTextBox>
										</td>
								<td id="add_bc26" class='td' runat="server"><dx:ASPxDateEdit ID="DateRequired" 
										runat="server" ClientInstanceName="DateRequired" EditFormat="Custom" 
										EditFormatString="yyyy-MM-dd" Width="100%" AnimationType="None"></dx:ASPxDateEdit></td>
								<td id="add_bc14" class='td' runat="server"><asp:TextBox ID="TextQuotedExtd" runat="server" CssClass="TMQuoteExt" ></asp:TextBox></td>
								<td id="add_bc15" class='td' runat="server"><asp:CheckBox ID="CheckBoxInclude" runat="server" Checked="True" Visible="False" Enabled="False" /></td>
								<td id="add_bc16" class='td' runat="server"><asp:CheckBox ID="chkNewPart" runat="server" Visible="False" /></td>
								<td id="add_bc25" class='td' runat="server"><asp:CheckBox ID="chkTrackPart" runat="server" Visible="False" /></td>
                                <td id="add_bc17" class='td' runat="server"><asp:ImageButton ID="lbVpic" runat="server" OnClick="lbVpic_Click" ImageUrl="~/images/EmptyPicture.JPG"  UseSubmitBehavior="false" /></td>
                                <td id="add_bc18" class='td' runat="server"><dx:ASPxComboBox ID="ddlBillType" 
										runat="server" ValueType="System.String" TextField="wo_lineitem_billtype_name" 
										ValueField="wo_lineitem_billtypeid"  ClientInstanceName="ddlBillType" 
										AnimationType="None">
									<ItemStyle HorizontalAlign="Left" />
								</dx:ASPxComboBox></td>
								<td id="add_bc19" class='td' runat="server"><img ID="btn_Notes" runat="server" src="~/images/icon/icon[note_blank].gif" title="Add A Note" alt="Add A Note" onclick="showNotes('0', event)"  />&nbsp;</img>&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img></img></img></img>&nbsp;&nbsp;</img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img>&nbsp;&nbsp;</img></img></img></td>
                                <td id="add_bc20" class='td' runat="server">
                                    <table cellspacing="0" cellpadding="1">
                                        <tr>
                                            <td valign="middle">
                                    <dx:ASPxButton ID="btn_Save" runat="server" Border-BorderStyle="None"
                                         OnClick="add_line_save"  CssClass="btn_Save" Height="15px" ClientInstanceName="btn_Save"
                                         UseSubmitBehavior="false" Width="20px" ToolTip="Save Row" ImageSpacing="0px" BackgroundImage-Repeat="NoRepeat" Theme="NETheme01"
                                         ClientSideEvents-Click="check_row_before_save(s,e)">
                                        <Image Url="../../../images/icon/icon[add].gif">
                                        </Image>
                                       
                                        <ClientSideEvents Click="check_save" />
                                        <BackgroundImage Repeat="NoRepeat" />
                                        <Border BorderStyle="None" />
                                    </dx:ASPxButton>
                                            </td>
                                            <td valign="middle">
                                    <asp:ImageButton ID="btn_Clear" CssClass="btn_Clear"  runat="server" ImageUrl="~/images/icon/icon[reset].gif" ToolTip="Clear Row" OnClick="add_line_clear" UseSubmitBehavior="false"/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                </tr>
                                </tbody>
                                </table>
                                <asp:SqlDataSource ID="sds_add_workorder_ddl" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
                                <asp:SqlDataSource ID="SqlDeptDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>
					<div id="lb_currentfilter" runat="server" class='current_filter' style="padding:5px;">&nbsp;</div>
					<label id="addline_visible_reason" runat="server"></label>
                    <div ID="InformationLabel" class="information_label" runat="server" style="font-weight:bold;background-color:#090;color:#fff;font-size:11px;padding:5px;display:none;"></div>
                    <div ID="ErrorLabel" class="error_label" runat="server" style="font-weight:bold;background-color:#f00;color:#fff;font-size:11px;padding:5px;display:none;"></div>
					<br />Existing items:<br />
                    <dx:ASPxPanel ID="pnl_gv" runat="server" Width="100%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent6" runat="server"> 
                                <dx:ASPxGridView 
									ID                        = "agv" 
									CSSCLASS                  = "master_grid" 
									CLIENTINSTANCENAME        = "agv" 
									RUNAT                     = "server" 
									AUTOGENERATECOLUMNS       = "False" 
									KEYFIELDNAME              = "id" 
									ONROWUPDATING             = "agv_RowUpdating" 
									ONCELLEDITORINITIALIZE    = "agv_CellEditorInitialize" 
									ONROWDELETING             = "agv_RowDeleting" 
									ONHTMLROWPREPARED         = "agv_HtmlRowPrepared"
									ONHTMLDATACELLPREPARED    = "agv_HtmlDataCellPrepared" 
									WIDTH                     = "100%"
									ONCANCELROWEDITING        = "agv_CancelRowEditing" 
									ONHEADERFILTERFILLITEMS   = "agv_HeaderFilterFillItems" 
									ONCOMMANDBUTTONINITIALIZE = "agv_CommandButtonInitialize" 
									ONPARSEVALUE              = "agv_ParseValue" 
									ONCUSTOMCALLBACK          = "agv_CustomCallback"
									ONCUSTOMBUTTONINITIALIZE  = "agv_CustomButtonInitialize" 
									ONCUSTOMJSPROPERTIES      = "agv_CustomJSProperties"
									ONAFTERPERFORMCALLBACK    = "agv_DataBound"
									ONDATABOUND               = "agv_DataBound" 
									ONHTMLFOOTERCELLPREPARED  = "agv_HtmlFooterCellPrepared" 
									FONT-NAMES                = "Arial" 
									 SettingsLoadingPanel-Text="">
									<SettingsCommandButton>
										<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" >
											<Image Url="~/images/icon/icon[delete].gif" Width="16px">
                                            </Image>
                                        </DeleteButton>
										<UpdateButton  Image-Url="~/images/icon/icon[save].gif" Image-Width="16px" Text="Save" >
											<Image Url="~/images/icon/icon[save].gif" Width="16px">
                                            </Image>
                                        </UpdateButton>
										<EditButton  Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" >
											<Image Url="~/images/icon/icon[edit].gif" Width="16px">
                                            </Image>
                                        </EditButton>
										<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" >
											<Image Url="~/images/icon/icon[add].gif" Width="16px">
                                            </Image>
                                        </NewButton>
										<CancelButton Image-Url="~/images/icon/icon[cancel].gif" Image-Width="16px" Text="Cancel" >
											<Image Url="~/images/icon/icon[cancel].gif" Width="16px">
                                            </Image>
                                        </CancelButton>
									</SettingsCommandButton>
                                    <Columns>
                                        <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="33" Width="40px" MinWidth="75" ShowEditButton="true" ShowUpdateButton="true" ShowDeleteButton="true" ShowClearFilterButton="true">
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="delete_multiple">
                                                    <Image ToolTip="Delete Selected lines" Url="~/images/icon/icon[deletemultiple].gif">
                                                    </Image>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataComboBoxColumn Caption="Work Order" FieldName="workorder" VisibleIndex="1" Width="80px">
                                            <PropertiesComboBox CallbackPageSize="100" DataSourceID="ds_wo_edit_form" DropDownRows="5"
                                                AnimationType="None" EnableCallbackMode="True" IncrementalFilteringDelay="250"
                                                IncrementalFilteringMode="Contains" TextField="WorkOrder" ValueField="WOProg_ID"
                                                ValueType="System.Int32">
                                            </PropertiesComboBox>
                                            <DataItemTemplate>
                                                <dx:ASPxComboBox ID="combo_location" runat="server" DataSourceID="ds_locations" CallbackPageSize="10" CssClass="combo_location" 
                                                    IncrementalFilteringMode="Contains" EnableCallbackMode="true" IncrementalFilteringDelay="100" Width="100%" 
                                                    AutoPostBack="false" ValueType="System.Int32" RenderIFrameForPopupElements="True" 
                                                    AnimationType="None" EnableTheming="False" Paddings="0px">
                                                    <ClientSideEvents SelectedIndexChanged="control_location_stock" />
                                                </dx:ASPxComboBox>
                                                <asp:SqlDataSource ID="ds_locations" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                                                    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" ></asp:SqlDataSource>
                                                <div style="white-space: nowrap; overflow-x: hidden; max-width: 150px;" id="wo_link" runat="server">
                                                    <a href="/redir.aspx?url=%2Fsections%2Fworkorder%2Findex.aspx%3Fwoprog_id%3D<%# Eval("workorder").ToString() %>"   target="_blank"><%# Container.Text %></a>
                                                </div>
                                            </DataItemTemplate>
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataComboBoxColumn Caption="Sections" FieldName="sectionid" VisibleIndex="0"
                                            Width="100px">
                                            <PropertiesComboBox DataSourceID="SqlDataSource1" AnimationType="None" TextField="section"
                                                ValueField="id" ValueType="System.Int32">
                                            </PropertiesComboBox>
                                            <Settings AllowAutoFilter="False" AllowHeaderFilter="True" FilterMode="DisplayText" />
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataTextColumn Caption="Master ID" FieldName="part_no" Name="master_id" VisibleIndex="5" Width="50">
                                            <PropertiesTextEdit>
                                                <ClientSideEvents TextChanged="function(s, e) {
                      var partid = s.GetText(); 
                      var qty = agv.GetEditor('qty').GetValue();                         
	                  grid_part_handler(partid, qty);
}" />
                                            </PropertiesTextEdit>
                                            <CellStyle CssClass="c_part_no"></CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Vend Part#" FieldName="vend_part_no" VisibleIndex="6"
                                            Width="50">
                                            <PropertiesTextEdit>
                                                <ClientSideEvents TextChanged="function(s, e) {
                       var vendorpartno = s.GetText();                         
	                   var partno = agv.GetEditor('part_no').GetValue();
	                   CheckVendorPartNo(partno,vendorpartno);
}" />
                                            </PropertiesTextEdit>
                                            <EditCellStyle Font-Bold="True">
                                            </EditCellStyle>
                                            <CellStyle CssClass="c_ven_prt" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Description" FieldName="description" Name="description" MinWidth="200"
                                            VisibleIndex="7" Width="100%">
                                            <Settings AutoFilterCondition="Contains" />
                                            <DataItemTemplate>
                                                &nbsp;<asp:Image ID="kitted_container" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/icon/icon[list_creation].gif"
                                                    Visible="False" />
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("description") %>' Font-Names="Arial"></dx:ASPxLabel>
                                                <br />
                                                <div id="tooltip" runat="server" class="tip"></div>
                                            </DataItemTemplate>
                                            <CellStyle CssClass="c_desc">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Qty" FieldName="qty" Name="qty" VisibleIndex="8"
                                            Width="50px">
                                            <PropertiesTextEdit>
                                                <ClientSideEvents 
KeyDown="function(s, e) {
    if (e.htmlEvent.keyCode == 13){         
             event.returnValue=false;
             event.cancel = true;
             agv.UpdateEdit();                                      
   }
    if(e.htmlEvent.keyCode == 27) {   
        event.returnValue=false;
        event.cancel = true;
        agv.CancelEdit(); 
    } 
} "  TextChanged="grid_qty_handler"  />
                                            </PropertiesTextEdit>
                                            <CellStyle HorizontalAlign="Center" CssClass="qty_req_back">
                                            </CellStyle>
                                            <HeaderStyle BackColor="#C0FFC0" Border-BorderStyle="None" >
                                            <Border BorderStyle="None" />
                                            </HeaderStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Bench" FieldName="sell" ReadOnly="True" VisibleIndex="13"
                                            Width="60px">
                                            <PropertiesTextEdit DisplayFormatString="#,###.00">
                                                <ClientSideEvents TextChanged="grid_sellchange" />
                                            </PropertiesTextEdit>
                                            <DataItemTemplate>
                                                <dx:ASPxLabel ID="FieldSellTextBox" Font-Names="Arial" runat="server" Text='<%# Bind("sell", "{0:C3}") %>'></dx:ASPxLabel>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Right" CssClass="sell_back">
                                            </CellStyle>
                                            <HeaderStyle BackColor="#C0FFFF" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Cost" FieldName="cost" VisibleIndex="12" Width="60px">
                                            <PropertiesTextEdit DisplayFormatString="#,###.00">
                                                <ClientSideEvents TextChanged="cost_editor" />
                                            </PropertiesTextEdit>
                                            <DataItemTemplate>
                                                <dx:ASPxLabel ID="FieldCostTextBox" Font-Names="Arial" runat="server" Text='<%# Bind("cost", "{0:C4}") %>'></dx:ASPxLabel>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Center" CssClass="cost_back">
                                            </CellStyle>
                                            <HeaderStyle BackColor="#E0E0E0" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Ext'd" FieldName="extTandM" ReadOnly="True" VisibleIndex="16"
                                            Width="80px">
                                            <PropertiesTextEdit DisplayFormatString="c2">
                                            </PropertiesTextEdit>
                                            <DataItemTemplate>
                                                <dx:ASPxLabel ID="FieldextdtmTextBox" Font-Names="Arial" runat="server" Text='<%# Bind("extTandM", "{0:C}") %>'></dx:ASPxLabel>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Right">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="~Ext'd" FieldName="extended_per" VisibleIndex="17"
                                            Width="80px">
                                            <PropertiesTextEdit DisplayFormatString="N4">
                                                <ClientSideEvents KeyDown="function(s, e) {
    if (e.htmlEvent.keyCode == 13){         
             event.returnValue=false;
             event.cancel = true;
             agv.UpdateEdit();                                      
   }
    if(e.htmlEvent.keyCode == 27) {   
        event.returnValue=false;
        event.cancel = true;
        agv.CancelEdit(); 
    } 
}"></ClientSideEvents>
                                            </PropertiesTextEdit>
                                            <CellStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" HorizontalAlign="Right">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataCheckColumn Caption="Inc" FieldName="include" VisibleIndex="18" Width="40px">
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataCheckColumn>
                                        <dx:GridViewDataTextColumn Caption="Spec" ReadOnly="True" VisibleIndex="19" Width="40px">
                                            <PropertiesTextEdit DisplayFormatString="{0}">
                                            </PropertiesTextEdit>
                                            <EditFormSettings Visible="False" />
                                            <EditCellStyle HorizontalAlign="Center">
                                            </EditCellStyle>
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" Font-Names="Arial" onclick="OnViewPicture(this, '<%# Container.KeyValue %>')"><%# picture_handler(Container)%></a>
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <a href="javascript:void(0);" Font-Names="Arial" onclick="OnViewPicture(this, '<%# Container.KeyValue %>')"><%# picture_handler(Container)%></a>
                                            </EditItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Notes" VisibleIndex="21" Width="25px">
                                            <EditCellStyle HorizontalAlign="Center">
                                                <Paddings Padding="0px" />
                                            </EditCellStyle>
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" Font-Names="Arial" onclick="showNotes('<%# Container.KeyValue %>',event)"><%# note_handler(Container)%></a>
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <a href="javascript:void(0);" Font-Names="Arial" id="notes_button" data-stuff="1234" runat="server" onclick="showNotes('<%# Container.KeyValue %>',event)"><%# note_handler(Container)%></a>
                                            </EditItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="Date Expected" FieldName="dateex" VisibleIndex="3"
                                            Width="5%">
                                            <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" AnimationType="None">
                                            </PropertiesDateEdit>
                                            <CellStyle Font-Size=".85em" HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn Caption="Rec'd to date" FieldName="qtyrec" VisibleIndex="10"
                                            Width="50px">
                                            <PropertiesTextEdit>
                                                <ClientSideEvents TextChanged="grid_recdtodate_handler" />
                                            </PropertiesTextEdit>
                                            <CellStyle HorizontalAlign="Center" CssClass="qty_com_back">
                                            </CellStyle>
                                            <HeaderStyle BackColor="#FFC0C0" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataComboBoxColumn Caption="Bill Type" FieldName="wo_detail_current_billtypeid" Name="billtype_id"
                                            VisibleIndex="22" Width="40px">
                                            <PropertiesComboBox DataSourceID="SqlDataSourceBillTypes" AnimationType="None"
                                                TextField="wo_lineitem_billtype_name" ValueField="wo_lineitem_billtypeid" ValueType="System.String">
                                            </PropertiesComboBox>
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataTextColumn Caption="Trans" FieldName="Trans" Name="transfer" VisibleIndex="23" Width="25px">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" Font-Names="Arial" onclick="TransferItems('<%# Container.KeyValue %>', false)">
                                                    <img alt="Transfer" src="/images/icon/transfer.jpg" width="16" height="16" /></a>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Hist" VisibleIndex="25" Width="20px">
                                            <DataItemTemplate>
                                                <%# history_handler(Container)%>
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <%# history_handler(Container)%>
                                            </EditItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Qty Rec'd" FieldName="emptyval" VisibleIndex="11"
                                            Width="60px" Name="qty_received">
                                            <PropertiesTextEdit>
                                                <ClientSideEvents TextChanged="grid_recd_handler" />
                                            </PropertiesTextEdit>
                                            <DataItemTemplate>
                                                <asp:TextBox ID="txtRecSelectQty" Font-Names="Arial" runat="server" Width="35px" onfocus="zero_chk(this, false)" onblur="zero_chk(this, true)" CssClass="recqty" Style="text-align: right" Text="0"></asp:TextBox>
                                            </DataItemTemplate>
                                            <FooterCellStyle HorizontalAlign="Center">
                                            </FooterCellStyle>
                                            <CellStyle HorizontalAlign="Center" BorderRight-BorderStyle="None">
                                                <BorderRight BorderStyle="None" />
                                            </CellStyle>
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
                                        <dx:GridViewDataTextColumn Caption="Qty Per Part" FieldName="qty_per_part" ToolTip="Quantity Per Vendor Part Number"
                                            VisibleIndex="15" Width="40px">
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Origin" FieldName="origin" VisibleIndex="28" Width="25px">
                                            <DataItemTemplate>
                                                <%# origin_handler(Container)%>
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <%# origin_handler(Container)%>
                                            </EditItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="RecNo" FieldName="wo_detail_current_rec_no" VisibleIndex="4"
                                            Width="20px">
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                            <DataItemTemplate>
                                                <div class="rec_no"><%# Container.Text %></div>
                                                </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Taxes" VisibleIndex="24" Width="25px">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" Font-Names="Arial" onclick="showTaxes('<%# Container.KeyValue %>', event)"><%# tax_handler(Container) %></a>
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Active" FieldName="active" VisibleIndex="27"
                                            Width="40px">
                                            <PropertiesTextEdit EnableFocusedStyle="False">
                                            </PropertiesTextEdit>
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" Font-Names="Arial" onclick="showNotesForInactive('<%# Container.KeyValue %>',event)"><%# noteinactive_handler(Container) %></a>
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <a href="javascript:void(0);" Font-Names="Arial" onclick="showNotesForInactive('<%# Container.KeyValue %>',event)"><%# noteinactive_handler(Container) %></a>
                                            </EditItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Issues" VisibleIndex="26" Width="25px">
                                            <DataItemTemplate>
                                                <%# issue_handler(Container) %>
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <%# issue_handler(Container) %>
                                            </EditItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Part Req Qty" VisibleIndex="9" Width="40px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataComboBoxColumn Caption="Dept" FieldName="Division_ID" VisibleIndex="2"
                                            Width="40px">
                                            <PropertiesComboBox DataSourceID="SqlDeptDataSource2" AnimationType="None" TextField="Division_Name"
                                                ValueField="Division_ID" ValueType="System.Int32">
                                            </PropertiesComboBox>
                                            <DataItemTemplate>
                                                <div style="white-space: nowrap">
                                                    <%# Container.Text %>
                                                </div>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Center" Wrap="False">
                                            </CellStyle>
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataTextColumn Caption="% Disc" FieldName="wo_detail_current_discount"
                                            VisibleIndex="14" Width="35px">
                                            <PropertiesTextEdit>
                                                <ClientSideEvents TextChanged="grid_discount_handler" />
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="BrCd" VisibleIndex="29" Width="25px">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="bc('<%# Container.KeyValue %>')">
                                                    <img alt="print barcode" title="Print Barcode" src='/images/icon/icon[print_barcode].gif' width='16' height='16' border='0' /></a>
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <a href="javascript:void(0);" onclick="bc('<%# Container.KeyValue %>')">
                                                    <img alt="print barcode" src='/images/icon/icon[print_barcode].gif' width='16' height='16' border='0' /></a>
                                            </EditItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Avl Qty" FieldName="qty_avail" VisibleIndex="30"
                                            Width="40px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataCheckColumn Caption="Track" FieldName="track_part" VisibleIndex="31">
                                        </dx:GridViewDataCheckColumn>
                                        <dx:GridViewDataTextColumn Caption="WOs" VisibleIndex="32" Width="45px">
                                            <DataItemTemplate>
                                                <%# po_stock_wo_track(Container)%>
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <%# po_stock_wo_track(Container)%>
                                            </EditItemTemplate>
                                            <CellStyle Font-Size="7pt" HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="Dt Req" FieldName="reqdate" Name="required_date" VisibleIndex="20"
                                            Width="50px">
                                            <PropertiesDateEdit DisplayFormatString="MM/dd/yy" AnimationType="None">
                                            </PropertiesDateEdit>
                                            <CellStyle Font-Size="7pt" Wrap="False">
                                            </CellStyle>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn Name="commit_wo" Caption="Commit" UnboundType="Boolean" VisibleIndex="35" FieldName="emptyval" CellStyle-HorizontalAlign="Center"
                                            Width="0px">
                                            <DataItemTemplate>
                                                <asp:CheckBox ID="cb_commit" CssClass="cb_commit" runat="server" />
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Select" Name="select" Width="10px" EditFormSettings-Visible="False" VisibleIndex="34">
											<EditFormSettings Visible="False" />
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
											</FooterTemplate>
										</dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="is_gl_account" FieldName="is_gl_account" VisibleIndex="36" visible="False"
                                                               Width="40px">
                                    </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager NumericButtonCount="25" PageSize="25" Position="TopAndBottom">
                                    </SettingsPager>
									<SettingsResizing ColumnResizeMode="Control" Visualization="Live" />
                                    <SettingsEditing Mode="Inline" />
                                    <Settings ShowFooter="True" ShowTitlePanel="false" ShowHeaderFilterButton="True" ShowFilterBar="Visible" ShowFilterRow="true" />
                                    <TotalSummary>
	                                    <dx:ASPxSummaryItem FieldName="extended_per" SummaryType="sum" DisplayFormat="c" />
	                                    <dx:ASPxSummaryItem FieldName="extTandM" SummaryType="sum" DisplayFormat="c" />
                                    </TotalSummary>
                                    <Styles FilterBar-CssClass="fb_class" InlineEditRow-CssClass="inlineeditrow" InlineEditCell-CssClass="inlineeditcell">
                                        <AlternatingRow CssClass="ar">
                                        </AlternatingRow>
										
                                        <Footer CssClass="f">
                                        </Footer>
										<Header CssClass="h">
										</Header>
										
                                        <LoadingPanel HorizontalAlign="Center" VerticalAlign="Top" Border-BorderStyle="None">
                                            <Border BorderStyle="None" />
                                        </LoadingPanel>
                                        <LoadingDiv Opacity="0" Border-BorderStyle="None">
                                            <Border BorderStyle="None" />
                                        </LoadingDiv>
                                        
                                        <InlineEditCell CssClass="inlineeditcell">
                                        </InlineEditCell>
                                        <InlineEditRow CssClass="inlineeditrow">
                                        </InlineEditRow>
                                        <FilterBar CssClass="fb_class">
                                        </FilterBar>
                                        
                                    </Styles>
                                     <SettingsBehavior ConfirmDelete="True" AllowDragDrop="False" AutoFilterRowInputDelay="3000"   />
                                    <ClientSideEvents EndCallback="function(s, e) {
    update_header_totals();
	bind_tooltips();
	EndReqHandler();
	if(s.cpInfo != undefined)
		{
		switch(s.cpInfo)
			{
			case &quot;show_refactor&quot;:
				$(&quot;#ctl00_cphMasterBody_rp_Main_pnl_addnewline_div_refactor&quot;).show();
			break;
			}
		s.cpInfo	= undefined;
		}
}" CustomButtonClick="delete_multiple" />
                                    <Images>
                                        <LoadingPanelOnStatusBar Url="../../../images/loading_panel.gif">
                                        </LoadingPanelOnStatusBar>
                                        <LoadingPanel Url="../../../images/loading_panel.gif">
                                        </LoadingPanel>
                                    </Images>
									<StylesFilterControl >
									</StylesFilterControl>
                                    <SettingsLoadingPanel ImagePosition="Top" />
									<Templates>
									    <HeaderCaption>
                                            <div title='<%# Container.Column.Caption %>'><%# Container.Column.Caption %></div>
                                        </HeaderCaption>
									</Templates>
                                </dx:ASPxGridView>
                            <asp:HiddenField ID="which_footer_save" runat="server" />
                              
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                                    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                                    >
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="ds_wo_edit_form" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                                    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                                    ></asp:SqlDataSource>
                                <asp:SqlDataSource ID="SqlDataSourceBillTypes" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                                    >
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="SqlDeptDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
                                </asp:SqlDataSource>
								&nbsp;&nbsp;
                                </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>
                 </dx:PanelContent>
            </PanelCollection>
            <HeaderStyle BackColor="DodgerBlue" Font-Names="Arial" Font-Size="16pt" ForeColor="White"
                Height="25px" />
            <HeaderTemplate>
                <table width="100%" cellpadding="2" cellspacing="0">
                    <tr>
                        <td align="left" valign="middle">
						<asp:Label ID="lblHeaderText" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="16pt" Text="" Width="100%"></asp:Label>
						<div style="white-space: normal"><asp:Label ID="lblHeaderText_sub" runat="server" Font-Size="0.75em" Text=""></asp:Label></div></td>
                       <td width="100%" align="right" style="white-space:nowrap; vertical-align:middle;">
							<img style="cursor:pointer" ID="ImgBtn_Print" runat="server" visible="false" onclick="" alt="Print" src='~/images/icon/icon[printwprice].gif' />
                            <img style="cursor:pointer" ID="ImgBtn_PrintWO" runat="server" visible="false" onclick="" alt="Print" src="~/images/iconsbuttons/32px-crystal_clear_action_shoppingcart.png" title="Inventory Picklist" />
                            <img style="cursor:pointer" ID="ImgBtn_PrintWOUnf" runat="server" visible="false" onclick="" alt="Print" src="~/images/iconsbuttons/unfulfilled_shoppingcart.png" title="Unfulfilled Picklist" />
                        </td>
                        <td><dx:ASPxButton ID="ASPxButton2"  Theme ="NETheme01"  
						BackColor="transparent" Border-BorderStyle="None" Height="25px" Width="25px" runat="server" OnClick="bnExport_Click"  ClientSideEvents-Click="function(s,e){{ hid_btn_exporter.DoClick();}}" AutoPostBack="False">
						<Image Url="~/images/icon/icon[excel].gif" Height="30px" Width="30px">
						</Image>
					</dx:ASPxButton></td>
                    </tr>
                </table>
            </HeaderTemplate>
            <ContentPaddings Padding="5px" />
        	<Border BorderWidth="0px" />
        </dx:ASPxRoundPanel>
        
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
        <dx:ASPxPopupControl id="ASPxPopupSectionAdd" runat="server"
            HeaderText="Sections" Modal="True" PopupHorizontalAlign="WindowCenter" 
			PopupVerticalAlign="WindowCenter" Width="735px" 
			OnLoad="ASPxPopupSectionAdd_Load" AppearAfter="100" 
			ShowPageScrollbarWhenModal="True" ClientInstanceName="section_popup" 
			Height="500px" AllowDragging="True">
            <contentcollection>
<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
	<dx:ASPxCallbackPanel ID="cbp_sections" runat="server" ClientInstanceName="cbp_sections"
		OnCallback="cbp_sections_Callback" Width="100%">
		<ClientSideEvents EndCallback="function(s, e) {
	lst_addsections_section.PerformCallback();
	btnAddSection.SetEnabled(true);
	btnSectionDelete.SetEnabled(false);
	btnAddSection.SetText(&quot;Add&quot;);
	persist_sections.Set(&quot;type&quot;, &quot;add&quot;);
	persist_sections.Set(&quot;ddlselected&quot;, ddlSection.GetValue());
	ddlSection.PerformCallback();
	txtSectionName.SetText(&quot;&quot;);
	txtSectionName.Focus();
	//$('.current_filter').html(inspect(ddlSection, 1));
}" BeginCallback="function(s, e) {
	btnAddSection.SetEnabled(false);
}" />
		<PanelCollection>
			<dx:PanelContent ID="PanelContent8" runat="server">
				<table width="100%" cellpadding="5" cellspacing="0">
                    <tr>
                        <td>
                            <dx:ASPxTextBox ID="txtSectionName" runat="server" Width="400px" ClientInstanceName="txtSectionName" AutoResizeWithContainer="True">
								<ClientSideEvents Init="function(s, e) {
	persist_sections.Set(&quot;section_name&quot;, s.GetText());
}" KeyDown="function(s, e){
	if(e.htmlEvent.keyCode == 13)
		{
		$(&quot;.save_section&quot;).click();
		}
	}" TextChanged="function(s, e) {
	var _text		= s.GetText();
	persist_sections.Set(&quot;section_name&quot;, _text.trim());
}" />
                            </dx:ASPxTextBox>
                        </td>
						<td style="width: 150px">
                            <table cellpadding="2" cellspacing="0">
                                <tr>
									<td colspan="2">
                                        <dx:ASPxButton ID="btnAddSection" runat="server" ClientInstanceName="btnAddSection" Text="Save" AutoPostBack="False" UseSubmitBehavior="False" CssClass="save_section">
                                            <Image Url="~/images/icon/icon[save].gif">
                                            </Image>
											<ClientSideEvents Click="function(s, e) {
	var _section_name		= txtSectionName.GetText();
	if(_section_name.trim() == &quot;&quot;)
		{
		alert(&quot;Invalid section name&quot;);
		txtSectionName.Focus();
		return false;
		}
	else
		{
		cbp_sections.PerformCallback();
		btnAddSection.SetEnabled(false);
		btnSectionDelete.SetEnabled(false);
		}
}" />
                                        </dx:ASPxButton>
									</td>
                                    <td style="width: 41px">
                                        <dx:ASPxButton ID="btnSectionDelete" runat="server" ClientInstanceName="btnSectionDelete" AutoPostBack="False" UseSubmitBehavior="False" ToolTip="Clear">
                                            <Image Url="~/images/icon/icon[delete].gif">
                                            </Image>
											<ClientSideEvents Click="function(s, e) {
	if(confirm(&quot;Please confirm you wish to delete this section&quot;))
		{
		persist_sections.Set(&quot;type&quot;, &quot;delete&quot;);
		cbp_sections.PerformCallback();
		txtSectionName.SetText(&quot;&quot;);
		}
}" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
						</td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <dx:ASPxListBox ID="lst_addsections_section" runat="server" Rows="15"
                                Width="100%" ClientInstanceName="lst_addsections_section"  TextField="section" ValueField="id" ValueType="System.Int32" EncodeHtml="False" Height="350px" OnCallback="lst_addsections_section_Callback" CallbackPageSize="200" EnableCallbackMode="True" EnableClientSideAPI="True">
                                <ClientSideEvents ValueChanged="function(s, e)
	{
	var txt = s.GetItem(s.GetSelectedIndex()).GetColumnTextByIndex(0);
	txtSectionName.Focus();
	txtSectionName.SetText(txt);
	persist_sections.Set(&quot;section_name&quot;, txt);
	btnAddSection.SetText(&quot;Save&quot;);
	var isLocked	= true;
	persist_sections.Set(&quot;type&quot;, &quot;edit&quot;);
	persist_sections.Set(&quot;id&quot;, s.GetItem(s.GetSelectedIndex()).value);
	var _title		= &quot;&quot;;
	if((s.GetItem(s.GetSelectedIndex()).GetColumnTextByIndex(1) /1) != 0)
		{
		isLocked = false;
		_title		= &quot;Section cannot be deleted, there are still parts that exist in this section. Please delete them first.&quot;;
		}
	if(s.GetItem(s.GetSelectedIndex()).GetColumnTextByIndex(2) != &quot;&quot;)
		{
		isLocked = false;
		_title		= &quot;This section is linked to a detail. In order to delete this section, you must delete the linked detail.&quot;;
		}
	btnSectionDelete.SetEnabled(isLocked);
	btnSectionDelete.GetMainElement().title 	= _title;
	}" EndCallback="function(s, e) {
	if(persist_sections.Get(&quot;type&quot;) != &quot;add&quot;)
		{
		var _item		= persist_sections.Get(&quot;id&quot;) == &quot;&quot; ? &quot;0&quot; : persist_sections.Get(&quot;id&quot;);
		lst_addsections_section.SetSelectedItem(lst_addsections_section.FindItemByValue(_item));
		if(_item != &quot;0&quot;)
			{
			var txt = lst_addsections_section.GetItem(lst_addsections_section.GetSelectedIndex()).GetColumnTextByIndex(0);
			txtSectionName.SetText(txt);
			}
		txtSectionName.Focus();
		persist_sections.Set(&quot;section_name&quot;, txtSectionName.GetText());
		var _title		= &quot;&quot;;
		var isLocked	= true;
		if((lst_addsections_section.GetItem(lst_addsections_section.GetSelectedIndex()).GetColumnTextByIndex(1) /1) != 0)
			{
			isLocked = false;
			_title		= &quot;Section cannot be deleted, there are still parts that exist in this section. Please delete them first.&quot;;
			}
		if(lst_addsections_section.GetItem(lst_addsections_section.GetSelectedIndex()).GetColumnTextByIndex(2) != &quot;&quot;)
			{
			isLocked = false;
			_title		= &quot;This section is linked to a detail. In order to delete this section, you must delete the linked detail.&quot;;
			}
		btnSectionDelete.SetEnabled(isLocked);
		btnSectionDelete.GetMainElement().title 	= _title;
		}
}" BeginCallback="function(s, e) {
	e.processOnServer = true;
	if(persist_sections.Get(&quot;type&quot;) == &quot;add&quot;)
		{
		txtSectionName.SetText(&quot;&quot;);
		txtSectionName.Focus();
		}
}" />
                                <Columns>
                                    <dx:ListBoxColumn Caption="id" FieldName="id" Name="id" Visible="False" Width="1px" />
                                    <dx:ListBoxColumn Caption="Sections" FieldName="section" Name="section" Width="60%" />
									<dx:ListBoxColumn Caption="# of items" FieldName="dependants" Name="dependants" Width="10%" />
									<dx:ListBoxColumn Caption="detail_id" FieldName="detail_id" Name="detail_id" Width="10%" />
                                </Columns>
                                <LoadingPanelImage Url="../../../images/loading_panel.gif">
                                </LoadingPanelImage>
                                <ValidationSettings>
                                    <ErrorFrameStyle ImageSpacing="4px">
                                        <ErrorTextPaddings PaddingLeft="4px" />
                                    </ErrorFrameStyle>
                                </ValidationSettings>
								<LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle" Border-BorderStyle="None">
								</LoadingPanelStyle>
                            </dx:ASPxListBox>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:HiddenField ID="hdnSectionID" runat="server" />
							<dx:ASPxHiddenField ID="persist_sections" runat="server" ClientInstanceName="persist_sections">
							</dx:ASPxHiddenField>
                        </td>
                    </tr>
                </table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
</dx:PopupControlContentControl>
</contentcollection>
            <HeaderStyle Font-Bold="True" Font-Names="arial" Font-Size="8pt" BackColor="Gray" ForeColor="White" >
            </HeaderStyle>
            <ClientSideEvents Closing="function(s, e) {
	location.href = location.href;
	//btn_Save.SetEnabled(true);
}" />
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="ASPxPopupControlPicture" runat="server" CloseAction="CloseButton"
            HeaderText="From the Photo Archives..." Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True" ModalBackgroundStyle-Opacity="0" Theme="NETheme01" Width="700px" Height="700px">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                    <asp:Image ID="Image1" runat="server" />
                    </dx:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="Gray" Font-Bold="True" Font-Names="arial" Font-Size="8pt" ForeColor="White">
            </HeaderStyle>
            <HeaderImage Url="~/images/FullPicture.JPG">
            </HeaderImage>
        </dx:ASPxPopupControl>
		<dx:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="popup" HeaderText="" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Modal="True" ShowPageScrollbarWhenModal="True" CloseAction="CloseButton" ModalBackgroundStyle-Opacity="0" Theme="NETheme01" Width="700px" Height="700px">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                    <dx:ASPxCallbackPanel ID="callbackPanel" ClientInstanceName="callbackPanel" runat="server" OnCallback="callbackPanel_Callback">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent9" runat="server">
                                <asp:Image ID="Image2" runat="server" CssClass="image2" />
                                <iframe id="if_popup_part_files" runat="server" width="100%" height="500px" scrolling="no" frameborder="0"></iframe>
                                </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                </dx:PopupControlContentControl>
            </ContentCollection>
              <ClientSideEvents Shown="popup_Shown" />
           
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="ASPxpuNotes" runat="server" ClientInstanceName="NotesPopUp"
            CloseAction="CloseButton" HeaderText="Notes" Modal="True" 
			ShowPageScrollbarWhenModal="True" PopupHorizontalAlign="WindowCenter" ModalBackgroundStyle-BackColor="Transparent" 
			PopupVerticalAlign="WindowCenter" AllowDragging="True" ContentStyle-Paddings-Padding="2px" Width="800px" Height="600px">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
					<table width="95%">
					    <tr>
					        <td valign="top">
					        <div style="font-size:11px;"><b>New Note</b></div>
                    <asp:TextBox ID="textNewNote" CssClass="textNotes" runat="server" Rows="4" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox><br /><br />
                    <asp:Button ID="imgbnotesupdate" runat="server"  Text="Add" ToolTip="Add Note" OnClick="imgbnotesupdate_Click" CssClass="imgbnotesupdate" />
                    <asp:Button ID="imgbtnPOLineActive" runat="server" Text="Add" ToolTip="Add Note" OnClick="imgbtnPOLineActive_Click" CssClass="imgbtnPOLineActive" />
</td>
					    </tr>
						<tr>
							<td valign="top">
							    <asp:TextBox ID="note_history" CssClass="note_history" runat="server" Rows="4" TextMode="MultiLine" Width="100%" Height="250px"></asp:TextBox>
							    <asp:Button ID="btn_edit_note" runat="server" Text="Save" ToolTip="Save" OnClick="Edit_Note_Click"  />
							</td>
						</tr>
					</table>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="Gray" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" ForeColor="White">
            </HeaderStyle>
            <ClientSideEvents Shown="function(s,e){$('.textNotes').focus();}" Closing="function(s, e) {
		agv.PerformCallback();
	     document.getElementById('ctl00_cphMasterBody_ASPxpuNotes_textNewNote').value = '';
}" />
            <HeaderImage Url="~/images/FullNotes.JPG">
            </HeaderImage>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="Popup_GroupSelect" runat="server" HeaderText="Group Listing" 
            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"   CloseAction="CloseButton"
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
                                        Width="40px" Text='<%# Bind("Qty")%>' BackColor="Cornsilk">
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
        </dx:ASPxPopupControl>
		<dx:ASPxPopupControl ID="pop_xfer" 
			runat="server" ClientInstanceName="TransferPopUp" 
            CloseAction="CloseButton" HeaderText="Transfer Parts" Modal="True" 
			PopupHorizontalAlign="NotSet"  PopupVerticalAlign="NotSet" PopupVerticalOffset="300"
			Width="600px" ShowPageScrollbarWhenModal="True" AllowDragging="True" ModalBackgroundStyle-Opacity="0">
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
												<tr>
													<td colspan="2">
														<dx:ASPxLabel ID="xfer_internal_lb" runat="server" Font-Bold="True" Font-Names="Arial" Text="Select Internal Branch Location"></dx:ASPxLabel>
													</td>
													<td align='center' id="tbl_xfer_qty_int_hdr" runat="server"><b>QTY</b></td>
												</tr>
												<tr>
													<td colspan="2">
														<asp:HiddenField ID="xfer_internal_hid" runat="server" />
														<dx:ASPxComboBox ID="xfer_internal_combo" runat="server" AnimationType="None" TextField="name" EnableCallbackMode="false" Native="true" CallbackPageSize="100" ValueField="id" Width="400px" DataSourceID="ds_internal_locations" ClientInstanceName="xfer_internal_combo">
															<ClientSideEvents SelectedIndexChanged="" />
														</dx:ASPxComboBox>
														<asp:SqlDataSource ID="ds_internal_locations" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, CONCAT(a.name,' - (', IFNULL(b.qty,0), ')') name, IFNULL(b.qty,0) qty FROM inventory_location_master a LEFT JOIN inventory_location b ON b.location_master_id = a.id AND b.master_id = @master_id WHERE a.business_unit_id = @business_unit_id AND a.type_id = 1">
															<SelectParameters>
																<asp:SessionParameter Name="@business_unit_id" SessionField="xfer_business_unit_id" />
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
												<tr>
													<td colspan="2">
														<dx:ASPxLabel ID="xfer_external_lb" runat="server" Font-Bold="True" Font-Names="Arial" Text="Select External Location"></dx:ASPxLabel>
													</td>
													<td align='center' id="tbl_xfer_qty_ext_hdr" runat="server"><b>QTY</b></td>
												</tr>
												<tr>
													<td colspan="2">
														<asp:HiddenField ID="xfer_external_hid" runat="server" />
														<dx:ASPxComboBox ID="xfer_external_combo" runat="server" AnimationType="None" IncrementalFilteringMode="Contains" Native="true" EnableCallbackMode="false" CallbackPageSize="100" TextField="name" ValueField="id" Width="400px" ClientInstanceName="xfer_external_combo" DataSourceID="ds_external_locations">
															<ClientSideEvents SelectedIndexChanged="" />
														</dx:ASPxComboBox>
														<asp:SqlDataSource ID="ds_external_locations" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, CONCAT(a.name,' - (',IFNULL(b.qty,0), ')') name, IFNULL(b.qty,0) qty FROM inventory_location_master a LEFT JOIN inventory_location b ON b.location_master_id = a.id AND b.master_id = @master_id WHERE a.business_unit_id = @business_unit_id AND a.type_id = 2">
															<SelectParameters>
																<asp:SessionParameter Name="@business_unit_id" SessionField="xfer_business_unit_id" />
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
											<asp:Label ID="lblTransferReason" runat="server" Font-Bold="True" Font-Names="Arial" Text="Reasons for Transfer"></asp:Label>
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
        <asp:HiddenField ID="hidRev" runat="server" />
		<input id="hidID" runat="server" class="hid_id" type="hidden" />
		<input id="hidCompanyID" runat="server" class="OLD_hid_business_unit_id" type="hidden" />
		<input id="hidWorkingBusinessUnitID" runat="server" class="OLD_hid_business_unit_id" type="hidden" />
		<input id="hidWarehouseBusinessUnitID" runat="server" class="hid_business_unit_id" type="hidden" />
		<input id="hidSellPrice" runat="server" class="hid_sellprice" type="hidden" />
		<input id="hidOrigin" runat="server" class="hid_origin" type="hidden" />
        <input id="hidWo" runat="server" class="hid_WO" type="hidden" />
        <input id="hidCurrentLoginUser" runat="server" class="hid_Current_Login_User" type="hidden" />
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
        <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True" ContainerElementID="btn_Save">
				<LoadingDivStyle Opacity="0">
				</LoadingDivStyle>
		</dx:ASPxLoadingPanel>
	<dx:ASPxPopupControl ID="pop_vendor_line" runat="server" 
			HeaderText="Add new vendor part number" Modal="True" 
			PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="NotSet" 
			Width="800px" ClientInstanceName="pop_vendor_line" AnimationType="None" 
			AllowDragging="True"  ModalBackgroundStyle-Opacity="0" PopupVerticalOffset="100" CloseAction="CloseButton">
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server">
				<dx:ASPxCallbackPanel ID="cbp_vendor_line" runat="server" Width="100%" ClientInstanceName="cbp_vendor_line" OnCallback="cbp_vendor_line_Callback">
					<PanelCollection>
						<dx:PanelContent ID="PanelContent10" runat="server">
							<table cellpadding="5" cellspacing="0" width="50%">
								<tr>
									<td align="center" style="width: 33%">
										<strong>Vendor Part #</strong></td>
									<td align="center" style="width: 33%">
										<strong>Cost</strong></td>
									<td align="center" style="width: 33%">
										<strong>Qty / Part</strong></td>
									<td align="center" style="width: 33%">
										&nbsp;</td>
								</tr>
								<tr>
									<td align="center" style="width: 33%">
										<dx:ASPxTextBox ID="t_new_vendor_part_number" ClientInstanceName="t_new_vendor_part_number" runat="server" Width="125px">
											<ClientSideEvents TextChanged="picklist.po.new_vendor_price.part_n_chk" />
											<ValidationSettings ErrorText="" ValidationGroup="vs_new_vendor_line" Display="None">
												<RequiredField ErrorText="* Please supply a vendor part number" IsRequired="True" />
											</ValidationSettings>
										</dx:ASPxTextBox>
									</td>
									<td align="center" style="width: 33%">
										<dx:ASPxTextBox ID="t_new_vendor_cost" ClientInstanceName="t_new_vendor_cost" runat="server" Width="75px">
											<ValidationSettings ErrorText="" ValidationGroup="vs_new_vendor_line" Display="None">
												<RequiredField ErrorText="* Please supply a vendor cost" IsRequired="True" />
											</ValidationSettings>
											<ClientSideEvents TextChanged="picklist.po.new_vendor_price.cost_chk" />
										</dx:ASPxTextBox>
									</td>
									<td align="center" style="width: 33%">
										<dx:ASPxTextBox ID="t_new_vendor_qty" ClientInstanceName="t_new_vendor_qty" runat="server" Width="50px">
											<ValidationSettings ErrorText="" ValidationGroup="vs_new_vendor_line" Display="None">
												<RequiredField ErrorText="* Please supply a qty/part" IsRequired="True" />
											</ValidationSettings>
											<ClientSideEvents TextChanged="picklist.po.new_vendor_price.qty_check" />
										</dx:ASPxTextBox>
									</td>
									<td style="width: 33%">
									<input type="hidden" id="Hidden1" />
										<dx:ASPxButton ID="b_new_vendor_save" runat="server" Text="Save" AutoPostBack="False" ClientInstanceName="b_new_vendor_save">
											<Image Url="~/images/icon/icon[save].gif">
											</Image>
											<ClientSideEvents Click="picklist.po.new_vendor_price.save" />
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
							<dx:ASPxValidationSummary ID="vs_new_vendor_line" runat="server" ValidationGroup="vs_new_vendor_line" ClientInstanceName="vs_new_vendor_line">
							</dx:ASPxValidationSummary>
							<dx:ASPxLabel ID="lb_new_vendor_line_error" runat="server" ClientInstanceName="lb_new_vendor_line_error" EncodeHtml="False" Width="100%"></dx:ASPxLabel>
						</dx:PanelContent>
					</PanelCollection>
					<ClientSideEvents EndCallback="function(s, e) {
		b_new_vendor_save.SetEnabled(true);
		//TextVendorPartNo.PerformCallback();
		$(&quot;#hid_vendor_part&quot;).val(t_new_vendor_part_number.GetText());
	if(y == &quot;save&quot; &amp;&amp; lb_new_vendor_line_error.GetText() == &quot;&quot;)
		{
		t_new_vendor_part_number.SetText(&quot;&quot;);
		t_new_vendor_part_number.Focus();
		pop_vendor_line.Hide();
		}
	else if(t_new_vendor_cost.GetText() == &quot;&quot;)
		{
		t_new_vendor_cost.Focus();
		}
	else if(t_new_vendor_qty.GetText() == &quot;&quot;)
		{
		t_new_vendor_qty.Focus();
		}
	else
		{
		b_new_vendor_save.Focus();
		}
}" BeginCallback="function(s, e) {
		b_new_vendor_save.SetEnabled(false);
}" CallbackError="function(s, e) {
		b_new_vendor_save.SetEnabled(true);
}" />
				</dx:ASPxCallbackPanel>
				<dx:ASPxHiddenField ID="hid_vendor_info" ClientInstanceName="hid_vendor_info" SyncWithServer="true" runat="server">
				</dx:ASPxHiddenField>
			</dx:PopupControlContentControl>
		</ContentCollection>
		<ClientSideEvents Shown="function(s, e) {
	t_new_vendor_part_number.Focus();
}" Closing="function(s, e) {
TextVendorPartNo.PerformCallback();
}" />
	</dx:ASPxPopupControl>
    	<a name="top"/>
		
		
    </contenttemplate>
        
    </ASP:UPDATEPANEL>
	</div>
    </ASP:CONTENT>
