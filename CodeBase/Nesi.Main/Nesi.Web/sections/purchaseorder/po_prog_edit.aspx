<%@ PAGE language="C#" masterpagefile="~/IntraDefault.master" autoeventwireup="true" inherits="sections_purchaseorder_po_prog_edit" title="Purchase Order Progress"	 EnableTheming="True" Codebehind="po_prog_edit.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<ASP:CONTENT ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server"></div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server"></div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</ASP:CONTENT>
<ASP:CONTENT ID="Content5" ContentPlaceHolderID="header_placeholder" Runat="Server">
	<link type="text/css" rel="Stylesheet" href="/css/po_prog.css?refresh=20180412" />
</ASP:CONTENT>
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <span id="spanMSG" runat="server"></span>
<span id="divCoName" runat="server" ></span>
<script type="text/javascript">
    $(document).ready(function () {
        page_obj.update_panel_progress.bind();
    });
    function open_scan(_file)
    {
        scan_pop.Show();
        scan_pop.PerformCallback(_file);

    }

function selection_change(obj)
	{
	var _id			= $(obj).val();
	if(_id)
		{
	
	    boing('/redir.aspx?url=%2Fsections%2Fpurchaseorder%2Fpo_prog_add.aspx%3Faction%3Dshow%2526poprogid%3D' + _id, 'PO' + _id, 1386, 720);
		}
	}
function bind_tt()
	{
	$("body").find('.list').each(function()
			{
			$(this).val('');
			$(this).change(function(){selection_change(this)});
			});
	$(document).find(".opt").each(function()
		{
		var _props			=	{
								text:		$(this).text(),
								value:		$(this).attr('data-value'),
								url:		$(this).attr('data-url'),
								doTip:		$(this).attr('data-dotip') == null ? true : $(this).attr('data-dotip') == "true"
								};
		if(!_props.doTip) return;
		var _click			= function()
								{
		    location.href = "javascript:boing('" + _props.url + "','PO" + _props.value + "', 1386,1000);";
								}
		$(this).click(_click).tip({title:_props.text, width:500});
		});
	}
$("document").ready(function()
						{
						Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
						bind_tt();
						});
function EndReqHandler(sender, args)
	{
	if (args != undefined && args.get_error() != undefined)
		{
		var err				= args.get_error().toString().replace(/Sys.+\Exception:/g, "").trim();
		alert(err);
		}
	bind_tt();
	}
</script>

	<asp:ScriptManager ID="sm_main" runat="server">
	</asp:ScriptManager>
	<asp:UpdateProgress ID="prog_main" runat="server" AssociatedUpdatePanelID="up_main">
		<ProgressTemplate>
			<div style="position:absolute; left:50%; top:50%; z-index: 999;"/images/loading_panel.gif" /></div>
		</ProgressTemplate>
	</asp:UpdateProgress>
	<asp:UpdatePanel ID="up_main" runat="server">
		<ContentTemplate>
			<table border="0" style="font-size: 11px; font-family: arial" cellpadding="2" cellspacing="0" >
				<tr>
					<td align="left" colspan="7" valign="middle">
<asp:Label ID="lblCompanyIdentifier" runat="server" Font-Bold="True" Font-Names="Arial"
        Font-Size="Large"></asp:Label></td>
				</tr>
    <tr style="font-size: 8pt">
    <td valign="middle" align="center" style="width: 100px;" >
        <asp:LinkButton ID="lbAddNewPO" runat="server" OnClick="lbAddNewPO_Click" Font-Bold="True" Font-Size="14px" OnClientClick="please_wait('start');">Add New PO</asp:LinkButton>
    </td>
    <td valign="middle" align="center" style="width: 100px;" >
		<strong>Sorted By:</strong></td>
        <td align="center" style="width: 18px; height: 25px" valign="middle">
            <asp:dropdownlist id="DropDownList1" runat="server" autoPostBack="True" onselectedindexchanged="DropDownList1_SelectedIndexChanged"  Font-Size="12px" Width="150px">
            <asp:ListItem Value="POProg_CutDate" Selected="True">Cut Date</asp:ListItem>
            <asp:ListItem Value="Vendor_Name">Vendor Name</asp:ListItem>
            <asp:ListItem Value="poprog_bvpo">Purchase Order Number</asp:ListItem>
            <asp:ListItem Value="poprog_invoice_slip_scan_date">Invoice Scan Date</asp:ListItem>
        </asp:dropdownlist></td>
        <td align="right" valign="middle" style="width: 100px">
			<strong>PM:</strong></td>
    <td valign="middle" align="center" style="height: 25px; width: 3px;" >
        <asp:DropDownList ID="ddlMemberList" runat="server" OnSelectedIndexChanged="ddlMemberList_SelectedIndexChanged" datatextfield="member_name" datavaluefield="member_id" AutoPostBack="True" Font-Names="Arial" Font-Size="12px" Width="150px">
        </asp:DropDownList>
    </td>
    <td align="right" valign="middle" style="width: 100px">
		<strong>
      Payment Type:</strong>
    </td>
    <td valign="middle" align="center" style="height: 25px; width: 150px;" >
    <asp:DropDownList ID="DDLPaymentType" runat="server" OnSelectedIndexChanged="DDLPaymentType_SelectedIndexChanged" datatextfield="payment_type" datavaluefield="payment_typeid" AutoPostBack="True" Font-Size="12px" Width="150px">
    </asp:DropDownList>
    </td>
    </tr>
    <tr><td style="width: 100px; height: 29px;"></td>
		<td style="width: 120px; height: 29px;" nowrap="nowrap"><strong>Include Nesi Cut 
			POs:</strong></td><td style="height: 29px">
			<asp:CheckBox ID="chk_showNesi" runat="server" AutoPostBack="True" 
				oncheckedchanged="chk_showNesi_CheckedChanged" Text=" " />
		</td>
    <td align="right" valign="middle" style="width: 100px; height: 29px;">
		<strong>
      Work Order:</strong></td>
    <td valign="middle" align="center" style="height: 29px; width: 3px;">
    <asp:DropDownList ID="ddlWorkOrdersOnPO" runat="server" OnSelectedIndexChanged="ddlWorkOrdersOnPO_SelectedIndexChanged" DataTextField="BVWO" DataValueField="WOID" AutoPostBack="True" Font-Size="12px" Width="150px"></asp:DropDownList>
    </td>
    <td align="right" valign="middle" style="width: 100px; height: 29px;">
		<strong>&nbsp;Parts:</strong></td>
    <td valign="middle" align="center" style="height: 29px; width: 150px;">
    <asp:DropDownList ID="ddlPartsOnPO" runat="server" OnSelectedIndexChanged="ddlPartsOnPO_SelectedIndexChanged" DataTextField="partno" DataValueField="partid" AutoPostBack="True" Font-Size="12px" Width="150px"></asp:DropDownList>
    </td>
    </tr>
</table>

	<div id="prog_edit">
		<div class="bucket justcut">
			<div class="h"><span class="inner">Just Cut <br /> Not Issued</span></div>
			<div id="lsbJustCut" class='lbpo' runat="server"></div>
			<div id="summary_just_cut" runat="server" class="total"></div>
		</div>
		<div class="bucket approval">
			<div class="h"><span class="inner">Waiting for<br />Approval</span></div>
			<div id="lsbWaitingApp" class='lbpo' runat="server"></div>
			<div id="summary_waiting_app" runat="server" class="total"></div>
		</div>
		<div class="bucket orderapproved">
			<div class="h"><span class="inner">Order Approved <br /> Waiting to be Issued</span></div>
			<div id="lsbWaitBMApproval" runat="server" class="lbpo"></div>
			<div id="summary_order_app" runat="server" class="total"></div>
		</div>
		 <div class="bucket approblems">
			<div class="h red"><span class="inner">AP<br />Problems</span></div>
			<div id="lsbAPProblems" runat="server" class="lbpo"></div>
			<div id="summary_problems" runat="server" class="total"></div>
		</div>
		 <div class="bucket allscans" id="div_allscans" runat="server">
			<div class="h"><span class="inner" id="lbl_final_column" runat="server"></span></div>
			<ASP:LISTBOX ID="lsb_scans2" runat="server" Width="100%" Height="100%" Font-Size="9pt" AutoPostBack="True" OnSelectedIndexChanged="lsbScans2_SelectedIndexChanged"></ASP:LISTBOX>
		</div>
		<div class="bucket packingslips">
			<div class="h"><span class="inner">Waiting for<br />Complete Delivery</span></div>
			<div id="lsbWaitingPackingSlip" class='lbpo' runat="server"></div>
			<div id="summary_packing_slips" runat="server" class="total"></div>
		</div>
		<div class="bucket questions">
			<div class="h"><span class="inner">Questions</span></div>
			<div id="lsbQuestions" runat="server" class="lbpo"></div> 
			<div id="summary_questions" runat="server" class="total"></div>
		</div>
		<div class="bucket scans">
			<div class="h"><span class="inner">This Business Unit<br /> Scans</div>
			<ASP:LISTBOX ID="lsbScans" runat="server" Width="100%" Height="100%" Font-Size="9pt" AutoPostBack="True" OnSelectedIndexChanged="lsbScans_SelectedIndexChanged"></ASP:LISTBOX>
		</div>
		<div class="bucket waitinginvoice">
			<div class="h"><span class="inner">Waiting for Invoice</span></div>	
			<div ID="lsbwaiting_for_invoice" runat="server" class="lbpo"></div>
			<div id="summary_waiting_invoice" runat="server" class="total"></div>
		</div>
		<div class="bucket holdingwos">
			<div class="h red"><span class="inner">Holding Up<br />WO's</span></div>
			<div id="lsbWOHoldup" runat="server" class="lbpo"></div>
			<div id="summary_wo_holdup" runat="server" class="total"></div>
		</div>
	</div>


    <asp:Label ID="lblerrordebug" runat="server"></asp:Label><br />
 <dx:ASPxPopupControl ID="scan_pop" runat="server"
            CloseAction="CloseButton" Width="1300px" HeaderText="Use Scan" Modal="True" 
				PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
				ShowPageScrollbarWhenModal="True"   AllowDragging="True"
				ClientInstanceName="scan_pop" Theme="NETheme01" OnWindowCallback="ASPxpcScanDisplay_WindowCallback">
            <ModalBackgroundStyle Opacity="0">
			</ModalBackgroundStyle>
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                
    <asp:HiddenField ID="hidFileName" runat="server" />
                    <table style="width:100%;">
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnUseScan" runat="server" OnClick="btnUseScan_Click" Text="Use Scan" />
                                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
                                <asp:Button ID="btnDeleteScan" runat="server" OnClick="btnDeleteScan_Click" Text="Delete Scan" />
                                <asp:Button ID="btnpo_needed" runat="server" OnClick="btnPONeeded_Click" Text="PO STILL NEEDED!" />
                                <asp:Button ID="btn_savenotes" runat="server" OnClick="btn_savenotes_Click"  Text="Save Notes" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rbl_slip_type" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="1">Packing Slip</asp:ListItem>
                                    <asp:ListItem Value="2">Invoice Slip</asp:ListItem>
                                    <asp:ListItem Value="3">Quote</asp:ListItem>
                                    <asp:ListItem Value="4">Return Authourization</asp:ListItem>
                                    <asp:ListItem Value="5">Order Confirmation</asp:ListItem>
                                    <asp:ListItem Value="6">Shipping Document</asp:ListItem>
                                    <asp:ListItem Value="7">AP Problems</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                         <tr>
                                <td>
                                    Comments:
                                </td>
                                <td><dx:ASPxMemo ID="mem_notes" runat="server" Height="50px" Width="373px">
                                    </dx:ASPxMemo>
                                </td>
                                <td width="100%">&nbsp;</td>
                            </tr>
                    </table>
                    <br />
					<table cellpadding="2" cellspacing="0" width="100%">
						<tr>
							<td style="width: 20%" valign="top">
								<div runat="server" id="div_scan_comments" class="scan_comments">
									<div><b>Comments associated with this scan</b></div>
									<div id="scan_comments" runat="server">
										
									</div>
								</div>
								<dx:ASPxGridView ID="gv_open_pos" OnPreRender="gv_open_pos_PreRender" ClientInstanceName="gv_open_pos" runat="server" AutoGenerateColumns="False" DataSourceID="sqlDataSourceOpenPO" KeyFieldName="poprog_id" Theme="NETheme01" Width="250px">
									<Columns>
										<dx:GridViewDataTextColumn Caption="PO" FieldName="poprog_bv" ShowInCustomizationForm="True" VisibleIndex="1" Width="100px">
											<CellStyle Wrap="True">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption=" " FieldName="poprog_order_description" ShowInCustomizationForm="True" VisibleIndex="2" Width="200px">
											<CellStyle Wrap="True">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsBehavior AllowSelectSingleRowOnly="true" AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="Control" />
									<SettingsPager PageSize="15" NumericButtonCount="3">
									</SettingsPager>
									<SettingsLoadingPanel Mode="Disabled" />
									<SettingsSearchPanel Visible="True" />
									<Styles>
										<SelectedRow BackColor="#FF9933" CssClass="gv_selected">
										</SelectedRow>
									</Styles>


									<ClientSideEvents BeginCallback="function(s,e){please_wait('start');}" EndCallback="function(s,e){please_wait('stop');}" />
								</dx:ASPxGridView>
								<dx:ASPxListBox ID="lb_wos" runat="server" DataSourceID="sqlDataSourceOpenPO"
									TextField="poprog_bv" ValueField="poprog_id" Width="300px" Rows="30"
									EnableCallbackMode="True" Height="450px" Theme="NETheme01" ClientVisible="False">
								</dx:ASPxListBox>
								<asp:SqlDataSource ID="sqlDataSourceOpenPO" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
							</td>
							<td valign="top">
								<iframe id="scanshow" runat="server" scrolling="auto" width="900px" height="1000"></iframe>
							</td>
						</tr>
					</table>
                    <div id="Div1" style="z-index:500;">
						&nbsp;</div>
                    
                </dx:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle>
                <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
            </HeaderStyle>
			<clientsideevents BeginCallback="function(s,e){please_wait('start');}" endcallback="function(s,e){gv_open_pos.UnselectRows();please_wait('stop');}"/>
        </dx:ASPxPopupControl>
		</ContentTemplate>
	</asp:UpdatePanel>
	<br />
    <asp:Label ID="lblClrLeg" runat="server" Text="Colour Legend" CssClass="color_legend_title" />
	<asp:Label ID="lblPSDarkRed" runat="server" BackColor="DarkRed" CssClass="color_legend_item" Font-Names="Arial" ForeColor="White" Text="All Items Received, Packing Slips and Invoices Scanned"></asp:Label>
	<asp:Label ID="lblisred" runat="server" BackColor="Red" CssClass="color_legend_item" Font-Names="Arial" ForeColor="White" Text="Some Items Received, Packing Slips and Invoices Scanned"></asp:Label>
    <asp:Label ID="lblPSGreen" runat="server" BackColor="Green" CssClass="color_legend_item" ForeColor="White" Font-Names="Arial" Text="All Items Received, Invoice Slip Scanned"></asp:Label>
    <asp:Label ID="lblPSBlue" runat="server" BackColor="Blue" CssClass="color_legend_item" ForeColor="White" Text="All Items Received"></asp:Label>
    <asp:Label ID="Label4" runat="server" BackColor="DarkOrange" CssClass="color_legend_item" ForeColor="Black" Text="Partial Items Received, Packing Slip Scanned"></asp:Label>
    <asp:Label ID="Label5" runat="server" BackColor="LightGray" CssClass="color_legend_item" ForeColor="Black" Text="Partial Items Received"></asp:Label>
    <asp:Label ID="lblPSYellow" runat="server" BackColor="Yellow" CssClass="color_legend_item" Font-Names="Arial" Text="All Items Completed, Packing Slips Scanned"></asp:Label>

</ASP:CONTENT>