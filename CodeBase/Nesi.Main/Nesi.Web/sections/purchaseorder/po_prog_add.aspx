<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_purchaseorder_po_prog_add" Title="Purchase Order" EnableViewStateMac="false" EnableSessionState="True" EnableEventValidation="false" ValidateRequest="false" ViewStateEncryptionMode="Never" EnableTheming="True" Codebehind="po_prog_add.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<script type="text/javascript" src="/js/purchaseorder.js?refresh=20180329"></script>
	<script type="text/javascript">
         
         var address1;
        var address2;  
        var city;
	    var prov;
	    var country;
        var postalCode;	
        po.id = '<%= hidPOProgID.Value %>';
        po.obj_ts = '<%= hid_ts.ClientID %>';
		function function_confirmprint(poid, ask) {
			var flag;
			if (ask == 1) {
				flag = confirm("Are you sure you want to print this purchase order?");
			}
			var should_redirect = $("input[id$='hidStatus']").val() / 1 < 7;
			var compid = document.getElementById("ctl00_cphMasterBody_hidCompanyID").value;
			if (flag || !ask) {
				var url = "/sections/purchaseorder/POOrderSlip.aspx?PO=" + poid;
				boing(url, "PO", 800, 800);
				//if (top.window != undefined && should_redirect) {
					//top.window.location = "/sections/purchaseorder/po_prog_edit.aspx?business_unit_id=" + compid;
				//}
			}
			//    return flag;
		}
		function update_header() {
			var status = document.getElementById("ctl00_cphMasterBody_tabs_lblStatusDisp").innerText;
			if (status == 'Not Issued') {
				document.getElementById("ctl00_cphMasterBody_ImageButtonOkayToOrder2").disabled = false;
				document.getElementById("ctl00_cphMasterBody_ImageButtonOkayToOrder").disabled = false;
				document.getElementById("ctl00_cphMasterBody_ImageButtonPrintPO2").disabled = false;
				document.getElementById("ctl00_cphMasterBody_ImageButtonPrintPO").disabled = false;

			}
			$.get("./po_prog_add.aspx",
				{
					a: "json_update_header",
					po_id: '<%= hidPOProgID.Value %>'
				},
				function (json_string) {
					var obj = $.parseJSON(json_string);
					$("#<%=lblPOTotal.ClientID %>").text(obj.po_total);
			$("#<%=lbllinesTotal.ClientID %>").text(obj.order_qty);
			$("#<%=lblRecdTotal.ClientID %>").text(obj.recd_qty);
			$("#<%=lbl_headerStatus.ClientID %>").text(obj.po_status);
		});
		}

		function hidetabs() {
			var action = document.getElementById("ctl00_cphMasterBody_hidAction").value;
			if (action == "add") {
				//tabs.GetTab(3).SetVisible(false);
				//tabs.GetTab(4).SetVisible(false);
			}


		}
		function framefresh() {
			parent.frames['frmScan'].location.href = parent.frames['frmScan'].location.href;
		}
		window.onload = function () {
			//hidetabs();
		};
		$("document").ready(function () {
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onBeginRequest);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest);
            try {
               
                    address1 = document.getElementById('ctl00_cphMasterBody_tabs_txtShippingAddress1').value;
                    address1 = address1.replace(/&lt;br \/&gt;/g, '');
                }
            catch (ex) {
                address1 = "";
            }
            
            try {
               
                    address2 = document.getElementById('ctl00_cphMasterBody_tabs_txtShippingAddress2').value;
                    address2 = address2.replace(/&lt;br \/&gt;/g, '');
                
            }
            catch (ex) {
                address2 = "";
            }
          
            ////alert(address2);

           
            try
            {
               
                    city = document.getElementById('ctl00_cphMasterBody_tabs_txtShippingCity').value;
                    city = city.replace(/&lt;br \/&gt;/g, '');
                
            }
            catch (ex)
            {
                 alert(ex.message);
                 city = "";
            }
             
           
            
               // alert(City);
            //Prov = "";
            try
            {
                 prov = document.getElementById('ctl00_cphMasterBody_tabs_txtShippingProv').value;
                 prov = prov.replace(/&lt;br \/&gt;/g, '');
            }
            catch (err) {
                prov = "";
               // alert(err.message);
            }

           
            
              // alert('country1');
            
            try
            {
                 country = document.getElementById('ctl00_cphMasterBody_tabs_txtShippingCountry').value;
                 country = country.replace(/&lt;br \/&gt;/g, '');
            }
            catch (ex) {
              //  country = "";
                  alert(ex.message);
            }
               
            
            
            try
            {
                postalCode = document.getElementById('ctl00_cphMasterBody_tabs_txtShippingPostCode').value;
                postalCode = postalCode.replace(/&lt;br \/&gt;/g, '');
            }
            catch (ex) {
                postalCode = "";
            }

        
            
           
                        
			if ($(".poprog_id").val() != "") {
				po.bv_status.set_timer();
			}
		});

		function onBeginRequest() { please_wait("start"); }
		function onEndRequest(sender, args) {
			please_wait("stop");
			var ifp_src = $(".if_picklist").attr("src");
			var iff_src = $(".if_files").attr("src");
			if (ifp_src == undefined && tabs.activeTabIndex == 3) {
				var poprog_id = $(".poprog_id").val();
				$(".if_picklist").attr("src", "/sections/member/picklist/pikclist.aspx?id=" + poprog_id + "&rev=0&origin=purchaseorder&iframe=yes");
			}
			if (iff_src == undefined && tabs.activeTabIndex == 8) {
				var poprog_id = $(".poprog_id").val();
				$(".if_files").attr("src", "/FileManager.aspx?parent_page=purchase_order&id=" + poprog_id);
				}

			try {

				var msg = args.get_error().message;
				if (msg.match(/PageRequestManagerServerErrorException/g) && msg.match(/collection/g)) {
					location.href = location.href;
				}
				else {
					alert(msg);
				}
			} catch (e) {
			}
			args.set_errorHandled(true);
		}

		function ImageButtonOkayToOrder_Click1() {
			if (confirm('Are you sure you are ready to proceed?')) {
				ImageButtonOkayToOrder_Click();

			}
		}
	</script>
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" OnAsyncPostBackError="ScriptManager1_AsyncPostBackError">
	</asp:ScriptManager>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:HiddenField runat="server" ID="hid_ts" />
			<asp:UpdateProgress ID="UpdateProgress1" runat="server"
				AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="100">
			</asp:UpdateProgress>

            <div id="header_info">
                <div class="l_pane">
		            <div>
		            <dx:ASPxComboBox ID="ddl_company" runat="server" AnimationType="None"
		                             AutoPostBack="True" Caption="Business Unit:"
		                             ClientInstanceName="branch" DataSourceID="SqlDataSource4"
		                             OnSelectedIndexChanged="ASPxCCBCompany_SelectedIndexChanged"
		                             TextField="ddl_name" Theme="NETheme01" Enabled="False"
		                             ValueField="id" ValueType="System.String" Width="500px" DropDownRows="15">
		                <CaptionSettings HorizontalAlign="Left" VerticalAlign="Middle" Position="Top" />
		                <ValidationSettings>
		                    <ErrorFrameStyle ImageSpacing="4px">
		                        <ErrorTextPaddings PaddingLeft="4px" />
		                    </ErrorFrameStyle>
		                </ValidationSettings>
		                <SettingsLoadingPanel ImagePosition="Top" />
		                <ButtonStyle Cursor="pointer" Width="11px">
		                </ButtonStyle>
		                <CaptionCellStyle Width="150px">
		                </CaptionCellStyle>
		            </dx:ASPxComboBox>
                </div>
				<div>
					<dx:ASPxComboBox ID="ddl_vendor" runat="server" AnimationType="None"
						AutoPostBack="True" CallbackPageSize="20" Caption="Vendor:"
						ClientInstanceName="ddl_vendor" 
						EnableCallbackMode="True" IncrementalFilteringDelay="100" IncrementalFilteringMode="Contains"
						OnSelectedIndexChanged="ASPxcbVendor_SelectedIndexChanged"
						TextField="vendor_name" TextFormatString="{0} {1}-{3} ({2} Branch)" Theme="NETheme01"
						ValueField="vendor_id" ValueType="System.Int32" Width="500px">
						<CaptionSettings HorizontalAlign="Left" VerticalAlign="Middle" Position="Top" />
						<ValidationSettings>
							<ErrorFrameStyle ImageSpacing="4px">
								<ErrorTextPaddings PaddingLeft="4px" />
							</ErrorFrameStyle>
						</ValidationSettings>
						<Columns>
							<dx:ListBoxColumn Caption="ID" FieldName="vendor_number" Width="50px" />
							<dx:ListBoxColumn Caption="Name" FieldName="vendor_name" Width="250px" />
							<dx:ListBoxColumn Caption="Branch" FieldName="vendor_branch" />
							<dx:ListBoxColumn Caption="City" FieldName="address_city" />
						</Columns>
						<SettingsLoadingPanel ImagePosition="Top" />
						<ButtonStyle Cursor="pointer" Width="11px">
						</ButtonStyle>
						<CaptionCellStyle Width="150px">
						</CaptionCellStyle>
					</dx:ASPxComboBox>
					<asp:Button ID="ImageButtonSaveVendor" runat="server" Height="22px"
							OnClick="ImageButtonSaveVendor_Click1" Text="Apply" />
				</div>
				<div>
					<dx:ASPxButton ID="lbAllPacking" runat="server" ImageSpacing="20px" OnClick="lbAllPacking_Click" Text="All Line Items are Complete.. Send it to 'Waiting for Invoice'" Theme="NETheme01" Visible="True" Width="400px" ClientInstanceName ="invoiceButton" ClientVisible="False">
					</dx:ASPxButton>
				</div>

                    <div class="button_pane">
						<asp:Label ID="linkbuttons2" runat="server"></asp:Label>
						<asp:ImageButton ID="ImageButtonAddScan2" runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_app_filetypes.png" OnClick="lbAddScan_Click" ToolTip="Add a Scan to This PO" Visible="False" />
						<asp:ImageButton ID="btnOpenScanWindow" runat="server" CausesValidation="False" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_app_scanner.png" ToolTip="Open Scanned File Window" />
						<asp:ImageButton ID="ImageButtonQuestions2" runat="server" ImageUrl="~/images/IconsButtons/question_mark.png" OnClick="ImageButtonQuestions_Click" OnClientClick="if ( !confirm('Are you sure you want to send this PO to questions?')) return false;" ToolTip="Send To Questions" Visible="False" />
						<asp:ImageButton ID="ImageButtonAPProblems" runat="server" ImageUrl="~/images/IconsButtons/ap_problems.png" OnClick="ImageButtonAPProblems_Click" OnClientClick="if ( !confirm('Are you sure want to send this to AP problems?')) return false;" ToolTip="Send To AP Problems" Visible="False" />
						<asp:ImageButton ID="ImageButtonAPProblemsFixed" runat="server" ImageUrl="~/images/IconsButtons/ap_fixed.png" OnClick="ImageButtonAPProblemsFixed_Click" ToolTip="Fixed AP Problem" Visible="False" />
						<asp:ImageButton ID="ImageButtonAnswer2" runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_info.png" OnClick="ImageButtonAnswer_Click" ToolTip="Information Provided and Question Answered" Visible="False" />
						<asp:ImageButton ID="ImageButtonOkayToOrder2" runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_apply.png" OnClick="ImageButtonOkayToOrder_Click" OnClientClick="if ( !confirm('Are you sure you are ready to proceed?')) return false;" ToolTip="Send to Approval -&gt;" Visible="False" />
						<asp:ImageButton ID="ImageButtonWaitPackingSlip2" runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_app_utilities.png" OnClick="ImageButtonWaitPackingSlip_Click" ToolTip="Issued - Waiting For Packing Slip Scan" Visible="False" />
						<asp:ImageButton ID="ImageButtonInvoiceSlip2" runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_filesystem_file_temporary.png" OnClick="ImageButtonInvoiceSlip_Click" ToolTip="Received - Waiting For Invoice To Scan" Visible="False" />
						<asp:ImageButton ID="Unissue2" runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_editdelete.png" OnClick="Unissue_Click" OnClientClick="if ( !confirm('Are you sure you want to unissue this PO?')) return false;" ToolTip="Unissue This PO" Visible="False" />
						<asp:ImageButton ID="ImgBtnCancelPO2" runat="server" ImageUrl="/images/iconsbuttons/32px-Crystal_Clear_action_button_cancel.png" OnClick="ImgBtnCancelPO_Click" OnClientClick="if ( !confirm('Are you sure you want to cancel this PO?')) return false;" ToolTip="Cancel This PO!" Visible="False" />
						<asp:ImageButton ID="ImageButton_BMApp" runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_apply-ApprovedbyBM.PNG" OnClick="ImageButton_BMApp_Click" OnClientClick="if ( !confirm('Are you sure you want to approve this PO?')) return false;" ToolTip="Approve This PO" Visible="False" />

						<asp:Label ID="lbTopPoNum" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Larger" Visible="False"></asp:Label>
						<asp:Label ID="lblLinkButtonPickList" runat="server" Visible="False"></asp:Label>
						<asp:ImageButton ID="ImageButtonPrintPO2" runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_fileprint.png" OnClientClick="function_confirmprint(ctl00_cphMasterBody_hidPOProgID.value,1); return false;" ToolTip="Place this Order and Issue the PO" Visible="False" />
						<asp:ImageButton ID="ImageButtonPrintPreview" runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_128_penguin.png" OnClick="ImageButton_PP_Click" ToolTip="Print Preview" Visible="True" />
                    </div>
				</div>
                <div class="r_pane">
						<table cellpadding="2" cellspacing="0"
							style="color: #666666; font-weight: bold;font-family: calibri; font-size: 12pt;"
							width="100%">
							<tr>
								<td align="right" class="style1">PO:</td>
								<td align="left" nowrap="nowrap" style="padding-left: 5px">
									<asp:Label ID="lblTopRightPO" runat="server" Font-Names="Calibri"
										Font-Size="12pt" Text="Unknown"></asp:Label>
								</td>
							</tr>
							<tr>
								<td align="right" class="style1" nowrap="nowrap">PO Total:</td>
								<td align="left" style="padding-left: 5px;">
									<asp:Label ID="lblPOTotal" runat="server" Font-Names="Calibri" Font-Size="12pt"
										Text="$0.00"></asp:Label>
								</td>
							</tr>
							<tr>
								<td align="right" class="style1" nowrap="nowrap">Order Qty:</td>
								<td align="left" style="padding-left: 5px">
									<asp:Label ID="lbllinesTotal" runat="server" Font-Names="Calibri"
										Font-Size="12pt" Text="0"></asp:Label>
								</td>
							</tr>
							<tr>
								<td align="right" class="style1" nowrap="nowrap">Rec&#39;d Qty:</td>
								<td align="left" style="padding-left: 5px">
									<asp:Label ID="lblRecdTotal" runat="server" Font-Names="Calibri"
										Font-Size="12pt" Text="0"></asp:Label>
								</td>
							</tr>
							<tr>
								<td align="right" class="style1">Status:</td>
								<td align="left" nowrap="nowrap" style="padding-left: 5px">
									<asp:Label ID="lbl_headerStatus" runat="server" Font-Names="Calibri"
										Font-Size="12pt"></asp:Label>
								</td>
							</tr>
							<tr>
								<td align="right" class="style1">Last Modified:</td>
								<td align="left" nowrap="nowrap" style="padding-left: 5px">
                                    <asp:Label ID="lbl_lastmodified" runat="server" Font-Bold="True" Font-Names="Calibri" Font-Size="12pt" ForeColor="#666666" Text=""></asp:Label>
                                    <img id="img_lastmodified" runat="server" Visible="False" src="/images/icon/icon[refresh].gif" onclick="po.handle_lastupdated(this);" style="cursor:pointer;" align="absmiddle" width="16" height="16" alt="Update Last Modified" />
								</td>
							</tr>
						</table>
				</div>
            </div>
			<asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, ddl_name from business_unit  WHERE enable_timesheet = 1"></asp:SqlDataSource>
			<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
				SelectCommand="SELECT vendor_id, vendor_number, vendor_name, name vendor_branch, address_city
FROM vendor a, business_unit b, address c WHERE b.ID = a.business_unit_id
AND a.Vendor_active = 1
AND a.Vendor_Hold = 'F'
AND c.Address_Table_ID = Vendor_ID
AND c.Address_Table = 'Vendor'
AND b.enable_timesheet = 1
ORDER BY a.Vendor_CreatedDateTime DESC, a.vendor_name"></asp:SqlDataSource>
			<asp:Label ID="errorlabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
			&nbsp;&nbsp;&nbsp;
			<dx:ASPxPageControl ID="tabs" ClientInstanceName="tabs" runat="server"
				ActiveTabIndex="0" TabSpacing="2px" Width="100%"
				Theme="NETheme01">
				<TabPages>
					<dx:TabPage Text="General">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl1" runat="server">

								<table width="100%" style="font-size: 12px; font-family: Arial">
									<tr id="nesi_cut_po" runat="server">
										<td style="font-weight: bold;" valign="middle">NESI Cut PO:
										</td>
										<td colspan="2" valign="middle" width="100%">
											<asp:CheckBox ID="chk_nesi_po" runat="server" />
										</td>
									</tr>
									<tr runat="server" id="tr_po_number">
										<td style="font-weight: bold;">
											<asp:Label ID="Label13" runat="server" Text="PO #:"></asp:Label>
										</td>
										<td colspan="2" style="padding-left: 5px">
											<asp:Label ID="lblPONumDisp" runat="server" Font-Bold="True"
												Font-Names="Calibri"></asp:Label>
										</td>
									</tr>
									<tr runat="server" id="tr_po_id">
										<td style="font-weight: bold;">
											<asp:Label ID="Label14" runat="server" Text="NESI ID:"></asp:Label>
										</td>
										<td colspan="2" style="padding-left: 5px">
											<asp:Label ID="lblPOProgIDDisp" runat="server" Font-Bold="False"
												Font-Names="Calibri"></asp:Label>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold; width: 141px;" valign="top">
											<asp:Label ID="Label7" runat="server" Text="Description:"></asp:Label>
										</td>
										<td colspan="2">
											<asp:TextBox ID="txtDescription" Style="max-width: 800px" runat="server"
												TextMode="MultiLine" Width="95%" Font-Names="Calibri" Height="75px"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold;" valign="middle">Vendor Contact:
										</td>
										<td colspan="2" valign="top" nowrap="nowrap" style="white-space: nowrap">
											<table style="width: 100%;">
												<tr>
													<td>
														<dx:ASPxComboBox ID="ddlContacts" runat="server" Theme="NETheme01"
															AnimationType="None" Font-Names="Arial" TextField="Contact"
															ValueField="Contact_id" ValueType="System.Int32">
															<ButtonStyle Wrap="False">
															</ButtonStyle>
														</dx:ASPxComboBox>
													</td>
													<td nowrap="nowrap" style="font-style: italic">&nbsp;&nbsp;
													</td>
													<td width="100%"  valign="middle" style="padding-left: 5px">
                                                       <%--  <asp:LinkButton ID="lbl_AddContact" runat="server" OnClick="lblAddContact_Click">Add Contact</asp:LinkButton>--%>
                                                         <asp:Button  ID="lbl_AddContact" runat="server" OnClick="lblAddContact_Click" Text="Add Contact" />
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold;" valign="top">
											<asp:Label ID="lblVendorNa" runat="server" Text="Vendor Details"></asp:Label>
										</td>
										<td colspan="2" valign="top" style="padding-left: 5px">
											<dx:ASPxLabel ID="lblVendDetails" runat="server" EncodeHtml="false"
												Font-Names="Calibri">
											</dx:ASPxLabel>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold;" valign="middle">
											<asp:Label ID="Label6" runat="server" Text="Vendor No:"></asp:Label>
										</td>
										<td colspan="2" valign="middle" style="padding-left: 5px">
											<div id="lblVendorNoDisp" runat="server">
											</div>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold;" valign="middle">
											<asp:Label ID="lblCurrency" runat="server" Text="Currency:"></asp:Label>
										</td>
										<td colspan="2" valign="middle">
											<dx:ASPxComboBox ID="DDLCurrency" runat="server" Theme="NETheme01" ValueType="System.Int32" EnableViewState="False" Spacing="0" AnimationType="None" DataSourceID="sql_currency" TextField="currency" ValueField="id">
												<ButtonStyle Width="13px">
												</ButtonStyle>
												<LoadingPanelStyle ImageSpacing="5px">
												</LoadingPanelStyle>
											</dx:ASPxComboBox>
											<asp:SqlDataSource ID="sql_currency" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, currency FROM currency"></asp:SqlDataSource>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold;" valign="middle">Default Location:
										</td>
										<td colspan="2" valign="middle">
											<dx:ASPxComboBox ID="combo_location" runat="server" AnimationType="None" EnableViewState="False" Spacing="0" TextField="name" ValueField="id" ValueType="System.Int32" Theme="NETheme01">
												<LoadingPanelStyle ImageSpacing="5px">
												</LoadingPanelStyle>
												<ButtonStyle Width="13px">
												</ButtonStyle>
											</dx:ASPxComboBox>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold;" valign="middle">
											<asp:Label ID="Label9" runat="server" Text="Credit Limit" Visible="False"></asp:Label>
										</td>
										<td colspan="2" valign="middle" style="padding-left: 5px">
											<asp:Label ID="lblCreditLimitDisp" runat="server" Visible="False"
												Font-Names="Calibri"></asp:Label>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold;" valign="middle">
											<asp:Label ID="Label10" runat="server" Text="Credit Type:" Visible="False"></asp:Label>
										</td>
										<td colspan="2" valign="middle" style="padding-left: 5px">
											<asp:Label ID="lblCreditTypeDisp" runat="server" Visible="False"
												Font-Names="Calibri"></asp:Label>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold;" valign="middle">
											<asp:Label ID="Label12" runat="server" Text="Status:"></asp:Label>
										</td>
										<td colspan="2" valign="middle" style="padding-left: 5px">
											<asp:Label ID="lblStatusDisp" CssClass="lblStatusDisp" runat="server"
												Font-Names="Calibri"></asp:Label>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold;" valign="middle">
											<asp:Label ID="Label15" runat="server" Text="Cut By:"></asp:Label>
										</td>
										<td colspan="2" valign="middle" style="padding-left: 5px">
											<asp:Label ID="Labelcutby" runat="server" Font-Names="Calibri"></asp:Label>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold;" valign="middle">
											<asp:Label ID="Label33" runat="server" Text="Ship Via:"></asp:Label>
										</td>
										<td colspan="2" style="vertical-align: middle;" valign="middle">
											<table cellpadding="0" cellspacing="0">
												<tr>
													<td style="width: 100%;">
														<dx:ASPxComboBox ID="ASPxComboBoxShipVia" runat="server" ValueType="System.String" EnableViewState="False" TextField="shipping_method" ValueField="id" Spacing="0" AnimationType="None" Theme="NETheme01">
															<LoadingPanelImage Url="../../images/loading_panel.gif">
															</LoadingPanelImage>
															<ButtonStyle Width="13px">
															</ButtonStyle>
															<ClientSideEvents SelectedIndexChanged="function(s, e) {
                       var a = s.GetText();
                       var pickup = ' - PICK UP';
                       if(a == 'Pick Up Orders')
                       {
                           if(address1.includes(pickup)) {                      
                              document.getElementById('ctl00_cphMasterBody_tabs_txtShippingAddress1').value=address1; 
                           }
                           else {
                              document.getElementById('ctl00_cphMasterBody_tabs_txtShippingAddress1').value=address1 + pickup;
                           }

                           document.getElementById('ctl00_cphMasterBody_tabs_txtShippingAddress2').value=address2;
                           document.getElementById('ctl00_cphMasterBody_tabs_txtShippingCity').value=city;                       
                           document.getElementById('ctl00_cphMasterBody_tabs_txtShippingCountry').value=country;
                           document.getElementById('ctl00_cphMasterBody_tabs_txtShippingPostCode').value=postalCode; 
                           document.getElementById('ctl00_cphMasterBody_tabs_txtShippingProv').value=prov;    
	                   }
	                   else
	                   {
                           if(address1.includes(pickup)) {                      
                              document.getElementById('ctl00_cphMasterBody_tabs_txtShippingAddress1').value=address1.replace(pickup, ''); 
                           }
                           else {
                              document.getElementById('ctl00_cphMasterBody_tabs_txtShippingAddress1').value=address1;
                           }

                           document.getElementById('ctl00_cphMasterBody_tabs_txtShippingAddress2').value=address2;
                           document.getElementById('ctl00_cphMasterBody_tabs_txtShippingCity').value=city;                       
                           document.getElementById('ctl00_cphMasterBody_tabs_txtShippingCountry').value=country;
                           document.getElementById('ctl00_cphMasterBody_tabs_txtShippingPostCode').value=postalCode; 
                           document.getElementById('ctl00_cphMasterBody_tabs_txtShippingProv').value=prov;
	                   }
                     }" />
															<LoadingPanelStyle ImageSpacing="5px">
															</LoadingPanelStyle>
														</dx:ASPxComboBox>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td style="font-weight: bold; display:none;" valign="top">
											<asp:Label ID="lblAddressBox" runat="server" Text="Address to Ship To:"></asp:Label>
										</td>
										<td colspan="2" style="height: 55px;display:none;" valign="top">
											<asp:TextBox ID="txtAddressBox" Style="max-width: 800px" runat="server" Rows="5" TextMode="MultiLine" Width="95%" CssClass="txtAddressBox" Font-Names="Calibri"></asp:TextBox>
										</td>
									</tr>
                                    <tr>
										<td style="font-weight: bold; " valign="top">
											<asp:Label ID="lblAddress1" runat="server" Text="Address line 1:"></asp:Label>
										</td>
										<td colspan="2" style="max-width: 100px; font-weight: bold;" valign="middle">
											<asp:TextBox ID="txtShippingAddress1" Style="max-width: 380px" runat="server" Rows="1" TextMode="SingleLine" Width="95%" CssClass="txtAddress1" Font-Names="Calibri"></asp:TextBox>
										</td>
									</tr>
                                    <tr>
										<td style="font-weight: bold;" valign="top">
											<asp:Label ID="lblAddress2" runat="server" Text="Address line 2:"></asp:Label>
										</td>
										<td colspan="2" style="max-width: 100px; font-weight: bold;" valign="middle">
											<asp:TextBox ID="txtShippingAddress2" Style="max-width: 380px" runat="server" Rows="1" TextMode="SingleLine" Width="95%" CssClass="txtAddress2" Font-Names="Calibri"></asp:TextBox>
										</td>
									</tr>
                                   
                                    <tr>
										<td style="font-weight: bold;" align="center" valign="top">
											     <asp:Label ID="lblCity" runat="server" Text="City:" Style="width:80px" ></asp:Label>
                                               
										</td>
										<td  colspan="2" style=" font-weight: bold;" valign="middle" align="left">
                                              <table cellpadding="0" cellspacing="0">
                                                       <tr>                                                             
                                                               <td  style="max-width: 150px;font-weight:bold;" valign="middle" align="left">
                                                                     <asp:TextBox ID="txtShippingCity" Style="max-width: 150px" runat="server" Rows="1" TextMode="SingleLine"  CssClass="txtAddressBox" Font-Names="Calibri"></asp:TextBox>                                                                   
                                                               </td>

                                                               <td style="font-weight: bold;min-width:105px" valign="top" align="center">
                                                                     <asp:Label ID="lblProv" runat="server" Text="Province/State:" Style="max-width: 105px"></asp:Label>
                                                               </td>

                                                               <td  style="max-width: 133px;font-weight:bold;" valign="middle" align="center">
                                                                     <asp:TextBox ID="txtShippingProv" Style="max-width: 133px" runat="server" Rows="1" TextMode="SingleLine"   CssClass="txtAddressBox" Font-Names="Calibri" MaxLength="2"></asp:TextBox>
                                                               </td>
                                                      </tr>
											    
                                                
                                                
                                              </table>
										</td>
                                       
											
										
									</tr>
                                    <tr>
										<td style="font-weight: bold;" valign="top" align="center">
											<asp:Label ID="lblCountry" runat="server" Text="Country:" Style="width: 80px"></asp:Label>
										</td>
										<td  colspan="2" style="font-weight: bold;" valign="middle" align="left">
                                            <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="max-width: 150px;font-weight:bold;" valign="middle" align="left">  
                                                              <asp:TextBox ID="txtShippingCountry" Style="max-width: 150px" runat="server" Rows="1" TextMode="SingleLine" CssClass="txtAddressBox"  Font-Names="Calibri"></asp:TextBox>
                                                        </td>

                                                        <td style="font-weight: bold;min-width:135px" valign="top" align="center">
                                                               <asp:Label ID="lblPostCode" runat="server" Text="Postal Code/ZIP Code:" Style="max-width:135px"></asp:Label>
                                                        </td>
											        
                                                        <td style="max-width: 103px;font-weight:bold;" valign="middle" align="center">
                                                              <asp:TextBox ID="txtShippingPostCode" Style="max-width: 103px" runat="server" Rows="1" TextMode="SingleLine"  CssClass="txtAddressBox" Font-Names="Calibri"></asp:TextBox>
                                                        </td>
                                                       
                                                    </tr>
                                            </table>
										</td>
                                       
											
										
									</tr>
									<tr>
										<td valign="middle">
											<asp:Label ID="lblPaymentMethod" runat="server" Text="Vendor Terms:" Font-Bold="True" Font-Names="Arial"></asp:Label>
										</td>
										<td colspan="2" valign="middle">
											<dx:ASPxComboBox ID="ASPxComboBoxPayment" runat="server" ValueType="System.Int32" EnableViewState="False" TextField="payment_method" ValueField="id" Spacing="0" AnimationType="None" Theme="NETheme01">
												<LoadingPanelImage Url="../../images/loading_panel.gif">
												</LoadingPanelImage>
												<ButtonStyle Width="13px">
												</ButtonStyle>
												<LoadingPanelStyle ImageSpacing="5px">
												</LoadingPanelStyle>
											</dx:ASPxComboBox>
										</td>
									</tr>
									<tr runat="server" id="tr_last4_credit_card">
										<td valign="middle" nowrap="nowrap">
											<strong>Last 4 of Credit Card Used:</strong></td>
										<td colspan="2" valign="middle">
											<dx:ASPxComboBox ID="combo_last4" runat="server"
												ClientInstanceName="combo_last4" DataSourceID="ds_cclast4" TextField="text"
												ValueField="id" ValueType="System.Int32" Theme="NETheme01"
												Font-Names="Segoe UI">
												<ClientSideEvents ButtonClick="function(s, e) {
	pop_cc.Show();
}" />
												<%--<Buttons>
													<dx:EditButton Text="Add/Edit">
														<Image ToolTip="Add new credit card #" Url="~/images/icon/icon[add].gif">
														</Image>
													</dx:EditButton>
												</Buttons>--%>
											</dx:ASPxComboBox>
										</td>
									</tr>
									<tr>
										<td valign="middle">
											<asp:Label ID="Excpet" runat="server" Text="Expected Order Date:" Font-Bold="True" Font-Names="Arial"></asp:Label>
										</td>
										<td valign="middle">
											<dx:ASPxDateEdit ID="combo_expectedorderdate" runat="server" EditFormat="Custom" EditFormatString="yyyy-MM-dd" Theme="NETheme01" ToolTip="The Date You Expect to Order This" Spacing="0" AnimationType="None">
												<CalendarProperties>
													<HeaderStyle Spacing="1px" />
												</CalendarProperties>
												<ButtonStyle Width="13px">
												</ButtonStyle>
											</dx:ASPxDateEdit>
										</td>
									</tr>
									<tr>
										<td colspan="1" valign="middle" style="font-weight: bold">Order Date:
										</td>
										<td colspan="2" valign="middle" style="padding-left: 5px">
											<asp:Label ID="LabelOrderDateDisp" runat="server" Width="95%"
												Font-Names="Calibri"></asp:Label>
										</td>
									</tr>
									<tr>
										<td style="width: 141px" valign="middle">
											<asp:Label ID="Label2" runat="server" Text="Required Date:" Font-Bold="True" Font-Names="Arial"></asp:Label>
										</td>
										<td style="vertical-align: middle" valign="middle">
											<dx:ASPxDateEdit ID="combo_required_date" runat="server" EditFormat="Custom" EditFormatString="yyyy-MM-dd" ToolTip="The Date That These Parts Are Required" Spacing="0" AnimationType="None" Theme="NETheme01">
												<CalendarProperties>
													<HeaderStyle Spacing="1px" />
												</CalendarProperties>
												<ButtonStyle Width="13px">
												</ButtonStyle>
											</dx:ASPxDateEdit>
										</td>
									</tr>
									<tr>
										<td colspan="1" valign="middle" style="font-weight: bold">Received Date:
										</td>
										<td colspan="2" valign="middle" style="padding-left: 5px">
											<asp:Label ID="lblReceivedDateDisp" runat="server" Width="95%"
												Font-Names="Calibri"></asp:Label>
										</td>
									</tr>
									<tr>
										<td style="width: 141px;" valign="middle">
											<asp:Label ID="Label5" runat="server" Text="Expected Receive Date:" Font-Bold="True" Font-Names="Arial"></asp:Label>
										</td>
										<td valign="middle">
											<dx:ASPxDateEdit ID="combo_expected_receive_date" runat="server" EditFormat="Custom" EditFormatString="yyyy-MM-dd" ToolTip="The Date That the Parts Are Expected to Arrive" Spacing="0" AnimationType="None" Theme="NETheme01">
												<CalendarProperties>
													<HeaderStyle Spacing="1px" />
												</CalendarProperties>
												<ButtonStyle Width="13px">
												</ButtonStyle>
											</dx:ASPxDateEdit>
										</td>
										<td valign="middle">
											<asp:Label ID="Label17" runat="server"></asp:Label>
										</td>
									</tr>
									<tr>
										<td style="width: 141px;" valign="middle">&nbsp;</td>
										<td valign="middle">&nbsp;</td>
										<td valign="middle">&nbsp;</td>
									</tr>
									<tr>
										<td style="width: 141px;" valign="middle">
											<dx:ASPxCheckBox ID="chkAckOfPORec" runat="server" CheckState="Unchecked"
												ClientInstanceName="chkAckOfPORec"
												Text="Acknowledgement of Receipt of PO Required" Theme="NETheme01" ClientVisible="False">
												<ClientSideEvents CheckedChanged="function(s, e) {
	cb_ack.PerformCallback('ack_req|'+s.GetValue());
	alert('Acknowledgment requirement saved');
}" />
											</dx:ASPxCheckBox>
										</td>
										<td valign="middle">
											<dx:ASPxCheckBox ID="chkPORec" runat="server" CheckState="Unchecked"
												ClientInstanceName="chkPORec" Text="Received" Theme="NETheme01" ClientVisible="False">
												<ClientSideEvents CheckedChanged="function(s, e) {
	cb_ack.PerformCallback('ack_rec|'+s.GetValue());
if (s.GetValue()==true)
{	
alert('Confirmation Received');
}
else
{
alert('Confirmation Removed');

}
	
}" />
											</dx:ASPxCheckBox>
										</td>
										<td valign="middle">
											<dx:ASPxCallback ID="cb_ack" runat="server" ClientInstanceName="cb_ack"
												OnCallback="cb_ack_Callback">
											</dx:ASPxCallback>
										</td>
									</tr>
									<tr>
										<td style="width: 141px;" valign="middle">
											<dx:ASPxCheckBox ID="chkShipNoticeReq" runat="server" CheckState="Unchecked"
												ClientInstanceName="chkShipNoticeReq" Text="Shipment Notification Required"
												Theme="NETheme01">
												<ClientSideEvents CheckedChanged="function(s, e) {
		cb_ack.PerformCallback('ship_req|'+s.GetValue());
	alert('Shipping notification requirement saved');

}" />
											</dx:ASPxCheckBox>
										</td>
										<td valign="middle">
											<dx:ASPxCheckBox ID="chkShipNotice" runat="server" CheckState="Unchecked"
												ClientInstanceName="chkShipNotice" Text="Received" Theme="NETheme01">
												<ClientSideEvents CheckedChanged="function(s, e) {
		cb_ack.PerformCallback('ship_rec|'+s.GetValue());
		if (s.GetValue()==true)
{	
	alert('Shipping notification received');
	}
else
{

alert('Shipping notification removed');
}
}" />
											</dx:ASPxCheckBox>
										</td>
										<td valign="middle">&nbsp;</td>
									</tr>
									<tr>
										<td colspan="2" valign="middle">&nbsp;</td>
										<td valign="middle">&nbsp;</td>
									</tr>

									<tr>
										<td nowrap="nowrap" style="width: 141px;" valign="middle">
											<asp:CheckBox ID="chkAllowEdit" runat="server" Font-Names="Segoe UI"
												Text="Allow Cost Edit After Issue" Visible="False" />
										</td>
										<td valign="middle">&nbsp;</td>
										<td valign="middle">&nbsp;</td>
									</tr>
									<tr>
										<td nowrap="nowrap" style="width: 141px;" valign="middle">&nbsp;</td>
										<td valign="middle">&nbsp;</td>
										<td valign="middle">&nbsp;</td>
									</tr>
								</table>
								<asp:Button ID="ButtonStartPO" runat="server" OnClick="ButtonStartPO_Click" Text="Next -&gt;" EnableTheming="False" />
								&nbsp; &nbsp;<asp:ImageButton ID="UpdateButton2" runat="server" Height="27px" ImageUrl="~/images/icon/icon[save].gif" OnClick="UpdateButton_Click" Width="27px" ToolTip="Update This PO" />
								<br />
								<br />

							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Work Order" Visible="False">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl2" runat="server">
								<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="949px" BackColor="#F3F3F3" HeaderText="Work Order">
									<PanelCollection>
										<dx:PanelContent ID="PanelContent2" runat="server">
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="Label4" runat="server" Text="Work Order"></asp:Label>
													</td>
													<td style="width: 365px">&nbsp;&nbsp;&nbsp; &nbsp;
														<dx:ASPxComboBox ID="ASPxcbwo" runat="server" OnSelectedIndexChanged="ASPxcbwo_SelectedIndexChanged" TextField="WorkOrder" ValueField="WOProg_ID" ValueType="System.String" AutoPostBack="True" Theme="NETheme01">
															<SettingsLoadingPanel ImagePosition="Top" />
															<LoadingPanelImage Url="~/App_Themes/SoftOrange/Editors/Loading.gif">
															</LoadingPanelImage>
															<ButtonStyle Cursor="pointer" Width="11px">
															</ButtonStyle>
															<ValidationSettings>
																<ErrorFrameStyle ImageSpacing="4px">
																	<ErrorTextPaddings PaddingLeft="4px" />
																</ErrorFrameStyle>
															</ValidationSettings>
														</dx:ASPxComboBox>
													</td>
													<td style="width: 77px">&nbsp;
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<asp:Label ID="lblWOInfo" runat="server"></asp:Label>
													</td>
												</tr>
											</table>
											<asp:ImageButton ID="ImageButton2" runat="server" Height="27px" ImageUrl="~/images/icon/icon[save].gif" OnClick="ButtonStartPO_Click" Width="27px" />
											<asp:ImageButton Height="27px" ID="UpdateButton3" ImageUrl="~/images/icon/icon[edit].gif" OnClick="UpdateButton_Click" runat="server" Width="27px" />
										</dx:PanelContent>
									</PanelCollection>
									<ContentPaddings PaddingBottom="12px" />
									<HeaderStyle>
										<border borderstyle="None" />
									</HeaderStyle>

									<Border BorderStyle="None" />
								</dx:ASPxRoundPanel>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Shipping Address" Visible="False">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl3" runat="server">
								<dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" BackColor="#F3F3F3" Width="949px">
									<ContentPaddings PaddingBottom="12px" />
									<HeaderStyle>
										<border borderstyle="None" />
									</HeaderStyle>

									<PanelCollection>
										<dx:PanelContent ID="PanelContent3" runat="server">
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="Label8" runat="server" Text="Shipping Address"></asp:Label>
													</td>
													<td>
														<asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Change</asp:LinkButton>
													</td>
													<td style="width: 77px"></td>
												</tr>
												<tr>
													<td colspan="3">
														<asp:Label runat="server" ID="lblShippingData"></asp:Label>
													</td>
												</tr>
											</table>
											<asp:Button ID="Button1a" runat="server" OnClick="ButtonStartPO_Click" Text="Next -&gt;" />
											<asp:ImageButton Height="27px" ID="UpdateButton4" ImageUrl="~/images/icon/icon[save].gif" OnClick="UpdateButton_Click" runat="server" Width="27px" />
										</dx:PanelContent>
									</PanelCollection>
									<Border BorderStyle="None" />
								</dx:ASPxRoundPanel>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="lineitems" Text="Line Items">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl4" runat="server">
								<dx:ASPxPanel ID="ASPxLinePanel" runat="server" Width="100%">
									<PanelCollection>
										<dx:PanelContent ID="PanelContent4" runat="server">
											<dx:ASPxButton ID="ASPxButtonRecAll" runat="server" Text="Receive All" Visible="False" AutoPostBack="False" ClientInstanceName="butreceiveall">
												<ClientSideEvents Click="function(s, e) {
popcommit.Show();

}" />
											</dx:ASPxButton>
											<iframe id="if_picklist" name="picklist" class="if_picklist" runat="server" scrolling="yes" width="100%" frameborder="0" style="overflow-x: scroll;"></iframe>
										</dx:PanelContent>
									</PanelCollection>
								</dx:ASPxPanel>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="history" Text="History">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl5" runat="server">
								<dx:ASPxRoundPanel ID="rp_history" runat="server" BackColor="#F3F3F3" HeaderText="" Width="949px" Height="700px" ShowHeader="False">
									<ContentPaddings PaddingBottom="12px" />
									<HeaderStyle BackColor="#DEDEDE">
										<border borderstyle="None" />
										<BorderLeft BorderStyle="None" />
										<BorderRight BorderStyle="None" />
										<BorderBottom BorderStyle="None" />
									</HeaderStyle>

									<PanelCollection>
										<dx:PanelContent ID="PanelContent5" runat="server">
											<div>
												<b>General History</b>
											</div>
											<div style="margin-bottom: 15px;">
												<asp:TextBox ID="txtPOHistory" runat="server" ReadOnly="True" Rows="5" TextMode="MultiLine" Width="900px" Height="200px" Font-Names="Arial" Font-Size="9pt"></asp:TextBox>
											</div>
											<div>
												<b>Part History</b><dx:ASPxComboBox runat="server" ID="ddlPartList" ValueType="System.Int32" AutoPostBack="True" TextField="t" ValueField="id" OnSelectedIndexChanged="ddlPartList_OnSelectedIndexChanged"></dx:ASPxComboBox>
											</div>
											<div style="margin-bottom: 15px;">
												<div ID="txtPartHistory" runat="server" style="overflow-y:scroll;width:900px;height:367px;font-family: arial; font-size: 9pt;border:solid 1px #999;padding:5px;"></div>
											</div>
											<div>
												<b>Status History</b>
											</div>
											<div style="margin-bottom: 15px;">
												<asp:TextBox ID="txtStatusHistory" runat="server" ReadOnly="True" Rows="5" TextMode="MultiLine" Width="900px" Height="150px" Font-Names="Arial" Font-Size="9pt"></asp:TextBox>
											</div>
										</dx:PanelContent>
									</PanelCollection>
									<Border BorderStyle="None" BorderWidth="0px" />
								</dx:ASPxRoundPanel>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="Comments" Text="Comments">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl6" runat="server">
								<table style="width: 319px">
									<tr>
										<td>
											<asp:TextBox ID="txtPONotes" runat="server" Rows="3" TextMode="MultiLine" Width="900px" Height="63px" Font-Names="Arial" Font-Size="9pt"></asp:TextBox>
										</td>
										<td>
											<asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/images/icon/icon[save].gif" OnClick="ImageButton7_Click" Height="26px" Width="25px" ToolTip="Add Note" />
										</td>
										<td></td>
									</tr>
									<tr>
										<td>
											<div id="div_comments" style="width: 100%; height: 100%;" runat="server">
											</div>
										</td>
										<td></td>
										<td></td>
									</tr>
									<tr>
										<td></td>
										<td></td>
										<td></td>
									</tr>
								</table>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Vendor">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl7" runat="server">
								<iframe id="if_vendor" runat="server" class="if_vendor" scrolling="yes" width="100%" frameborder="0" height="800px"></iframe>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Files">
						<ContentCollection>
							<dx:ContentControl runat="server">
								<iframe frameborder="0" style="width: 100%; height: 650px;" id="if_files" class="if_files" runat="server"></iframe>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
                    <dx:TabPage Text="Integration Transaction History">
                        <ContentCollection>
                            <dx:ContentControl ID="contentcontrol_integ" runat="server">
                                <dx:ASPxGridView id="gv_integ_history" runat="server" Theme="NETheme01" AutoGenerateColumns="false" Width="100%" OnHtmlRowPrepared="gv_integ_history_HtmlRowPrepared" EnableTheming="True" SettingsBehavior-EnableCustomizationWindow="True">
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="NESI Purchase Order Id" FieldName="NESI Purchase Order Id"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="NetSuite Purchase Order Internal Id" FieldName="NetSuite Purchase Order Internal Id"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="NetSuite Vendor Credit Internal Id" FieldName="NetSuite Vendor Credit Internal Id"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Integration Workload" FieldName="Integration Workload"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Integration Status" FieldName="Integration Status"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Start Time in UTC" FieldName="Start Time"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Completion Time in UTC" FieldName="Completion Time"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Retry Count" FieldName="Retry Count"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Message Type" FieldName="Message Type"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Message" FieldName="Message"  CellStyle-CssClass="integMsg"></dx:GridViewDataTextColumn>                                   
                                    </Columns>
                                </dx:ASPxGridView>                         
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
				</TabPages>

				<ClientSideEvents EndCallback="function(s, e) {hidetabs();}" Init="function(s, e) {hidetabs();}" TabClick="function(s, e) {hidetabs();}" ActiveTabChanged="po.tab_switch" />
				<SettingsLoadingPanel Text="" />
				<LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle">
				</LoadingPanelStyle>


			</dx:ASPxPageControl>
			<dx:ASPxPopupControl ID="ASPxpucAddressSel" runat="server"
				HeaderText="Change Address" CloseAction="CloseButton" Modal="True"
				PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
				ShowPageScrollbarWhenModal="True" AnimationType="None" Theme="NETheme01">
				<ModalBackgroundStyle Opacity="0">
				</ModalBackgroundStyle>
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
						<asp:Label ID="Label11" runat="server" Text="Manually Enter an Address:"></asp:Label>
						<dx:ASPxMemo ID="txtManualAddressAdd" runat="server" Height="50px" Width="320px" Theme="NETheme01"></dx:ASPxMemo>

						<asp:ImageButton ID="ImageButton4" runat="server"
							ImageUrl="~/images/icon/icon[save].gif" OnClick="ImageButton4_Click"
							Width="25px" />
					</dx:PopupControlContentControl>
				</ContentCollection>
				<HeaderStyle>
					<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
				</HeaderStyle>
			</dx:ASPxPopupControl>
			<asp:SqlDataSource ID="ds_cclast4" runat="server"
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
				SelectCommand="SELECT id, CONCAT(type,&quot; - &quot;, last_four, &quot;  &quot;,name) text FROM poprog_cc WHERE business_unit_id = @business_unit_id ORDER BY id;">
				<SelectParameters>
					<asp:ControlParameter ControlID="ddl_company" Name="@business_unit_id"
						PropertyName="Value" />
				</SelectParameters>
			</asp:SqlDataSource>
			<dx:ASPxPopupControl ID="ASPxPopupCancel" runat="server" Theme="NETheme01"
				Modal="True" PopupHorizontalAlign="WindowCenter"
				PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True"
				AnimationType="None" HeaderText="Cancel and Close PO">

				<ModalBackgroundStyle Opacity="0">
				</ModalBackgroundStyle>

				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
						<asp:Label ID="lblReason" runat="server" Font-Names="Arial"></asp:Label>
						<dx:ASPxMemo ID="txtReason" runat="server" Height="50px" Width="320px" Theme="NETheme01"></dx:ASPxMemo>
						<asp:ImageButton ID="CancelNotesSave" runat="server"
							ImageUrl="~/images/icon/icon[save].gif" OnClick="CancelNotesSave_Click"
							Visible="False" Width="25px" />
						<asp:ImageButton ID="InactiveNoteSave" runat="server"
							ImageUrl="~/images/icon/icon[save].gif" Visible="False" Width="25px" />
					</dx:PopupControlContentControl>
				</ContentCollection>
				<HeaderStyle>
					<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
				</HeaderStyle>
			</dx:ASPxPopupControl>
			<dx:ASPxPopupControl ID="ASPxpcScanList" runat="server" Theme="NETheme01" HeaderText="Scanned Items" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True" Width="550px">
				<ModalBackgroundStyle Opacity="0">
				</ModalBackgroundStyle>
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
						<asp:ListBox ID="lsbScannedItems" runat="server" Height="150px" Width="520px" AutoPostBack="True" Font-Size="9pt" OnSelectedIndexChanged="lsbScannedItems_SelectedIndexChanged"></asp:ListBox>
					</dx:PopupControlContentControl>
				</ContentCollection>
				<HeaderStyle>
					<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
				</HeaderStyle>
			</dx:ASPxPopupControl>
			&nbsp;
			<dx:ASPxPopupControl ID="ASPxpcScanDisplay" runat="server" AllowDragging="True" AllowResize="True" CloseAction="CloseButton" Height="100%" Width="900px" HeaderText="Use Scan" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True" AnimationType="None" Theme="NETheme01">
				<ModalBackgroundStyle Opacity="0">
				</ModalBackgroundStyle>
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
						<br />
						<table style="width: 100%;">
							<tr>
								<td colspan="3">
									<asp:Button ID="btnUseScan" runat="server" OnClick="btnUseScan_Click" Text="Use Scan" />
									<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
								</td>
							</tr>
							<tr>
								<td colspan="3">
									<asp:RadioButtonList ID="rbl_slip_type" runat="server" RepeatDirection="Horizontal">
										<asp:ListItem Selected="True" Value="1">Packing Slip</asp:ListItem>
										<asp:ListItem Value="2">Invoice Slip</asp:ListItem>
										<asp:ListItem Value="3">Quote</asp:ListItem>
										<asp:ListItem Value="4">Return Authourization</asp:ListItem>
										<asp:ListItem Value="5">Order Confirmations</asp:ListItem>
										<asp:ListItem Value="6">Shipping Documents</asp:ListItem>
										<asp:ListItem Value="7">AP Problems</asp:ListItem>
									</asp:RadioButtonList>
								</td>
							</tr>
							<tr>
								<td>Comments:
								</td>
								<td>
									<dx:ASPxMemo ID="mem_notes" runat="server" Height="50px" Width="373px">
									</dx:ASPxMemo>
								</td>
								<td width="100%">&nbsp;</td>
							</tr>
						</table>

						<br />
						<asp:Label ID="lblScanShow" runat="server" Text="Label"></asp:Label>
					</dx:PopupControlContentControl>
				</ContentCollection>
				<HeaderStyle>
					<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
				</HeaderStyle>
			</dx:ASPxPopupControl>
			&nbsp;&nbsp;<dx:ASPxPopupControl ID="popcommit" runat="server" HeaderText="Receive All" ClientInstanceName="popcommit" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="250px" AnimationType="None" Theme="NETheme01">
				<ContentStyle HorizontalAlign="Center">
				</ContentStyle>
				<ModalBackgroundStyle BackColor="Transparent" Opacity="0">
				</ModalBackgroundStyle>
				<ContentCollection>
					<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
						<center>
							If any of the lines are associated with a work order, would you like to commit the item directly to the work order?
							<div style="color: #f00;">(Inventory excludes automatically go to the work order, they will not update stock)</div>
							<br />
							<br />
							<dx:ASPxRadioButtonList ID="rb_receiveallcommit" runat="server" SelectedIndex="1" ValueType="System.Int32">
								<Items>
									<dx:ListEditItem Text="Yes" Value="1" />
									<dx:ListEditItem Text="No" Value="0" Selected="True" />
								</Items>
							</dx:ASPxRadioButtonList>
							<br />
							<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Receive All"
								OnClick="receive_all" Theme="NETheme01">
								<ClientSideEvents Click="function(s, e) {
	popcommit.Hide();
}" />
							</dx:ASPxButton>
						</center>
					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>
			<br />
			<dx:ASPxPopupControl ID="pop_cc" runat="server" ClientInstanceName="pop_cc"
				HeaderText="New Credit Card" PopupAnimationType="None"
				Width="300px" Theme="NETheme01" EnableClientSideAPI="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
				<ClientSideEvents Shown="function(s, e) {
	combo_cctype.Focus();
}" />
				<ModalBackgroundStyle Opacity="0">
				</ModalBackgroundStyle>
				<ContentCollection>
					<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
						<div align="center">
							<table cellpadding="5" cellspacing="0" width="100%">
								<tr>
									<td colspan="2">
										<dx:ASPxGridView ID="gv_cc" runat="server" AutoGenerateColumns="False"
											ClientInstanceName="gv_cc" DataSourceID="ds_cclast4" KeyFieldName="id"
											Width="100%">
											<Columns>
												<dx:GridViewCommandColumn ShowEditButton="True" ShowInCustomizationForm="True"
													ShowNewButtonInHeader="True" VisibleIndex="0">
												</dx:GridViewCommandColumn>
												<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True"
													ShowInCustomizationForm="True" VisibleIndex="1">
													<EditFormSettings Visible="False" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn FieldName="text" ShowInCustomizationForm="True"
													VisibleIndex="2">
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsPager Mode="ShowAllRecords">
											</SettingsPager>
										</dx:ASPxGridView>
									</td>
								</tr>
								<tr>
									<td colspan="2">Only provide the type of card and the last four digits of it&#39;s number, for 
										tracking purposes.</td>
								</tr>
								<tr>
									<td><b>Type:</b></td>
									<td>
										<dx:ASPxComboBox ID="combo_cctype" runat="server" ClientInstanceName="combo_cctype">
											<Items>
												<dx:ListEditItem Text="American Express" Value="AMEX" />
												<dx:ListEditItem Text="Discover" Value="DISC" />
												<dx:ListEditItem Text="Mastercard" Value="MC" />
												<dx:ListEditItem Text="Visa" Value="VISA" />
											</Items>
										</dx:ASPxComboBox>
									</td>
								</tr>
								<tr>
									<td><b>Last 4:</b></td>
									<td>
										<dx:ASPxTextBox ID="tb_lastfour_cc" runat="server" ClientInstanceName="tb_lastfour_cc" MaxLength="4" Width="170px"></dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td><b>Name:</b></td>
									<td>
										<dx:ASPxTextBox ID="tb_ccname" runat="server" ClientInstanceName="tb_ccname"
											MaxLength="49" Width="170px">
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="center">
										<dx:ASPxButton ID="tb_save_cc" runat="server" Text="Save" OnClick="tb_save_cc_Click">
											<ClientSideEvents Click="function(s, e) {
	var _branch		= branch.GetValue();
	var _process	= true
	if(_branch == &quot;&quot; || _branch == null)
		{
		alert(&quot;Please select a branch first&quot;);
		_process	= false;
		}
	var cc_type		= combo_cctype.GetValue();
	if((cc_type == &quot;&quot; || cc_type == null) &amp;&amp; _process)
		{
		alert(&quot;Please select a type of credit card&quot;);
		_process	= false;
		}
	var cc_last4	= tb_lastfour_cc.GetText();
	if(!cc_last4.match(/\d{4}/g) &amp;&amp; _process)
		{
		alert(&quot;Please enter the last 4 digits of the credit card.&quot;);
		_process	= false;
		}
	if(!_process)
		{
		e.processOnServer = false;
		}
}" />
											<Image Url="~/images/icon/icon[save].gif">
											</Image>
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
						</div>
					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>
           <dx:ASPxPopupControl ID="popAddContact" ClientInstanceName="popAddContact" runat="server" AllowDragging="True" AllowResize="True" CloseAction="CloseButton" Height="100%" Width="600px" HeaderText="Add Vendor's Contact" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True" AnimationType="None" Theme="NETheme01">
				<ModalBackgroundStyle Opacity="0">
				</ModalBackgroundStyle>
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
						<br />
						<table style="width: 100%;">
							<tr>
                                <td colspan="2"><asp:Label ID="lbl_name" runat="server" Text="Name:" Width="150px" ></asp:Label></td>
								<td colspan="3">
                                       <dx:ASPxTextBox ID="txt_ContactName" runat="server"  Width="100%" Font-Names="Arial" Font-Size="9pt" EnableViewState="False">
                                           <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right">
                                           <RequiredField ErrorText="Contact Name is required" IsRequired="True" />
                                          </ValidationSettings>
                                          <ClientSideEvents GotFocus="function(s, e) { s.SelectAll() }" />
                                      </dx:ASPxTextBox>
								</td>
							</tr>
							<%--<tr>
                                 <td colspan="2"><asp:Label ID="lbl_Title" runat="server" Text="Title:" Width="150px" ></asp:Label></td>
								 <td colspan="3">
	                                   <dx:ASPxTextBox ID="txt_ContactTitle" runat="server"  Width="100%" Font-Names="Arial" Font-Size="9pt" EnableViewState="False">
                                             <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right">
                                                 <RequiredField ErrorText="Title is required" IsRequired="True" />
                                             </ValidationSettings>
                                             <ClientSideEvents GotFocus="function(s, e) { s.SelectAll() }" />
                                      </dx:ASPxTextBox>
								 </td>							
							</tr>--%>
							<tr>
                                <td colspan="2"><asp:Label ID="lbl_Email" runat="server" Text="Email:" Width="150px" ></asp:Label></td>
								<td colspan="3">
                                     <dx:ASPxTextBox ID="txt_ContactEmail" runat="server"  Width="100%" Font-Names="Arial" Font-Size="9pt" EnableViewState="False">
                                           <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right">
                                           <RequiredField ErrorText="E-mail is required" IsRequired="True" />
                                           <RegularExpression ErrorText="Invalid e-mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                          </ValidationSettings>
                                          <ClientSideEvents GotFocus="function(s, e) { s.SelectAll() }" />
                                     </dx:ASPxTextBox>
								</td>
							</tr>
                            <%--<tr>
							
                                <td colspan="2"><asp:Label ID="lbl_Ext" runat="server" Text="Extension #:" Width="150px" ></asp:Label></td>
								<td colspan="3">
                                     <dx:ASPxTextBox ID="txt_ContactExt" runat="server"  Width="100%" Font-Names="Arial" Font-Size="9pt" EnableViewState="False">
                                           <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right">
                                                 <RequiredField ErrorText="Extension # is required" IsRequired="True" />
                                             </ValidationSettings>
                                           <ClientSideEvents GotFocus="function(s, e) { s.SelectAll() }" />
                                      </dx:ASPxTextBox>
								</td>
							</tr>--%>
                             <tr>
							
                                <td colspan="2"><asp:Label ID="lbl_Cellphone" runat="server" Text="Cell Phone:" Width="150px" ></asp:Label></td>
								<td colspan="3">
                                     <dx:ASPxTextBox ID="txt_ContactCellphone" runat="server"  Width="100%" Font-Names="Arial" Font-Size="9pt" EnableViewState="False">
                                           <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right">
                                           <RequiredField ErrorText="Phone Number is required" IsRequired="True" />
                                           <RegularExpression ErrorText="Invalid Phone Number" ValidationExpression="^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$" />
                                          </ValidationSettings>
                                          <ClientSideEvents GotFocus="function(s, e) { s.SelectAll() }" />
                                     </dx:ASPxTextBox>
								</td>
							</tr>
                            <tr>
                                <td colspan="2"></td>
                                <td>
                                <dx:ASPxButton ID="btn_Save" runat="server" Text="Save" Width="150px" OnClick="popAddContactSave_Click" Theme="NETheme01">
								       
							    </dx:ASPxButton>
                                </td>
                                <td>
                                 <dx:ASPxButton ID="btn_Cancel" runat="server" Text="Cancel" Width="150px" Theme="NETheme01">
								   <ClientSideEvents Click="function(s, e) {popAddContact.Hide();}" />
							    </dx:ASPxButton>
                                </td>
                                 <td>

                                </td>
                            </tr>
                           
						</table>

					
						
					</dx:PopupControlContentControl>
				</ContentCollection>
				<HeaderStyle>
					<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
				</HeaderStyle>
			</dx:ASPxPopupControl>
			<asp:HiddenField ID="hidShipToAddressID" runat="server" />
			<asp:HiddenField ID="hidWOProgID" runat="server" />
			<asp:HiddenField ID="hidCompanyID" runat="server" />
			<asp:HiddenField ID="hidVendorID" runat="server" />
			<asp:HiddenField ID="hidManualAddress" runat="server" />
			<asp:HiddenField ID="hidDefaultAddress" runat="server" />
			<input type="hidden" id="hidPOProgID" class="poprog_id" value="0" runat="server" />
			<asp:HiddenField ID="hidAction" runat="server" />
			<asp:HiddenField ID="hidFileName" runat="server" />
			<asp:HiddenField ID="hidStatus" runat="server" />
			&nbsp;
		</ContentTemplate>
	</asp:UpdatePanel>
    	
</asp:Content>

<asp:Content ID="Content5" runat="server"
	ContentPlaceHolderID="header_placeholder">
	<style type="text/css">
		.style1 {
			font-weight: normal;
            width: 150px !important;
		}
		#header_info {
		    text-align: left;
		    display: flex;
		    min-height: 200px;
		}
		#header_info .l_pane {
		    float: left;
		    width: 75%;
		}
		#header_info .r_pane {
		    float: left;
		    width: 14%;
		    text-align:right;
		    min-width: 200px;
		}
		#header_info .action_panel_l {
		    float:left;
		    width: 75%;
		}
		#header_info .action_panel_r {
		    float:right;
		    width: 20%;
		}
        #header_info .button_pane {
            padding-top: 50px;
        }
		#header_info .pad {
		    padding: 2px;
		}
		#header_info .left {
		    text-align: left;
		}
		#header_info .right {
		    text-align: right;
		}
		#header_info .clear {
		    clear: both;
		}
        .integMsg{
            white-space:pre;
        }
	</style>
</asp:Content>


