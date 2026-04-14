<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_member_expense_company_credit_card" EnableTheming="true" Theme="" Codebehind="company_credit_card.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
	<style type="text/css">
		.c			{
			color:					#000;
			font-weight:			bold; 
			font-size:				11px; 
			font-family:			calibri;
					}
		.n			{
			color:					#060;
			font-weight:			bold; 
			font-size:				11px; 
			font-family:			calibri;
					padding-bottom:	20px;
			}
.m			{
			color:					#000;
			font-weight:			bold; 
			font-size:				12px; 
			width:					15px; 
			font-family:			calibri;
			text-align:				right;
			}
.v			{
			color:					#000;
			font-weight:			bold; 
			font-size:				11px;
			font-family:			calibri;
			}
.con		{
			}
		.error		{
					color:			#f00;
					font-size:		12px;
					font-family:	calibri;
					display:		block;
					padding:		10px;
					}
		.error .upload		{
							display:	block;
							}
		.ccwarning
			{
			font-weight:		bold;
			font-size:			14px;
			color:				#f00;
			text-align:			center;
			margin-bottom:		10px;
			}
	</style>
	<link type="text/css" href="/css/autocomplete.css" rel="Stylesheet" />
	<link type="text/css" href="/css/base/ui.all.css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" >

	    $("document").ready(function () {

	        please_wait('stop');
	    });

        	function purchased_from_handler(s,e)
			{
			var this_text		= s.GetText();
			this_text			= this_text.replace(" ", "");
			if(this_text.match(/perdi[e|o|u]m/gi))
				{
				s.SetSelectedIndex(-1);
				s.SetText("");
				alert("You must use the 'Per Diem' tab above to request per diem expenses");
				pc_main.SetActiveTabIndex(1);
				}
			}
		function wo_change_handler(s,e)
			{
			if(s.GetSelectedItem() == null)
				{
				s.SetText("");
				s.SetValue(null);
				}
			}
		function shop_check_handler(s,e)
			{
			c_workorder.SetText("");
			c_workorder.SetEnabled(!s.GetChecked());
            //ddl_div.SetEnabled(s.GetChecked());
			}
		function handle_link(file) { boing(file, 'CC Receipt', 800, 600); }

        var btn_clicked = false;

       
		function amount_negative(s, e) {
			var row = $(".receipts_row");
			var title = $(".receipts_title");
			var value = t_amount.GetValue();
			if (value < 0) {
				row.show();
				title.show();

				cb_shop.SetEnabled(false);
				cb_user.SetEnabled(false);
				de_purchase.SetEnabled(false);
				c_seller.SetEnabled(false);
				c_workorder.SetEnabled(false);
				ddl_credit_card.SetEnabled(false);
				c_category.SetEnabled(false);
			} else {
				row.hide();
				title.hide();

				cb_shop.SetEnabled(true);
				cb_user.SetEnabled(true);
				de_purchase.SetEnabled(true);
				c_seller.SetEnabled(true);
				c_workorder.SetEnabled(true);
				ddl_credit_card.SetEnabled(true);
				c_category.SetEnabled(true);
			}
		}

		</script>

    <div>
		<asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
		<dx:ASPxPanel ID="pnl_expense" runat="server" Width="95%">
			<PanelCollection>
				<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<div class="expense">
					<asp:Label ID="lb_error" runat="server" CssClass="error"></asp:Label>
					<div ID="lb_ccwarning" runat="server" class="ccwarning" style="font-family: Calibri; font-size: large;">This is used for recording Company Owned Credit Card transactions <u>only</u>.</div>
					
					<div style="padding-left:10px">* - Denotes a required field</div>
					<div class="row">
						<div class="title">User:</div>
						<div class="control">
							<dx:ASPxComboBox ID="cb_user" ClientInstanceName="cb_user" runat="server" Enabled="False" TextField="name" ValueField="id" AutoPostBack="true" ValueType="System.Int32" OnSelectedIndexChanged="cb_user_SelectedIndexChanged"></dx:ASPxComboBox>
							<asp:SqlDataSource ID="sds_users" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
						</div>
					</div>
					<div class="row" id="row_pp" runat="server">
						<div class="title">For the pay period of:</div>
						<div class="control"><dx:ASPxLabel ID="lb_payperiod" runat="server"></dx:ASPxLabel></div>
					</div>
					<div class="row">
						<div class="title">Purchase Date:</div>
						<div class="control">
							<dx:ASPxDateEdit ID="de_purchase" ClientInstanceName="de_purchase" runat="server">
								<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorText="*" ValidationGroup="v_expense">
									<RequiredField ErrorText="Field is Required" IsRequired="True" />
								</ValidationSettings>
							</dx:ASPxDateEdit>
						</div>
					</div>
					<div class="row">
						<div class="title">*Purchased From?:</div>
						<div class="control">
							<dx:ASPxComboBox runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" DataSourceID="ds_sellers" TextField="name_seller" ValueField="id_seller" Width="90%" ID="c_seller" CallbackPageSize="1000" ClientInstanceName="c_seller" ValueType="System.Int32">
								<ClientSideEvents SelectedIndexChanged="purchased_from_handler" />
								<ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" ValidationGroup="v_expense">
								<RequiredField IsRequired="True" ErrorText="Field is Required"></RequiredField>
								</ValidationSettings>
							</dx:ASPxComboBox>
							<asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" InsertCommand="INSERT INTO expense_seller (name) VALUES (@seller_name);" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT MIN(id_seller) id_seller, name_seller FROM expense_seller WHERE id_seller != 309 GROUP BY name_seller ORDER BY name_seller" ID="ds_sellers">
								<InsertParameters>
									<asp:ControlParameter ControlID="c_seller" PropertyName="Value" Name="@seller_name"></asp:ControlParameter>
								</InsertParameters>
							</asp:SqlDataSource>
						</div>
					</div>
					<div class="row">
						<div class="title">Work Order #:</div>
						<div class="control">
							<dx:ASPxComboBox ID="c_workorder" runat="server" CallbackPageSize="25" ClientInstanceName="c_workorder" DropDownWidth="90%" DataSourceID="ds_workorder" DropDownStyle="DropDown" TextField="woprog_bvwo" ValueField="woprog_id" Width="90%" ValueType="System.Int32" DropDownRows="15" AutoPostBack="True" OnSelectedIndexChanged="c_workorder_SelectedIndexChanged1">
								<ClientSideEvents LostFocus="wo_change_handler" />
								<Columns>
								    <dx:ListBoxColumn FieldName="ddl_name" Name="Business_Unit" Caption="Business_Unit" Width="80px" />
									<dx:ListBoxColumn FieldName="woprog_bvwo" Name="WO" Caption="WO" Width="70px" />
									<dx:ListBoxColumn Caption="Customer" FieldName="customer_name" Name="Customer" Width="150px" />
									<dx:ListBoxColumn Caption="Description" FieldName="description" Name="Description" Width="300px" />
								</Columns>
							</dx:ASPxComboBox>
						</div>
					</div>
				    <div class="row">
				        <div class="title"></div>
				        <div class="control">
				            If you can't find your work order, it might be on hold, or it's been set as a labor only work order.
				        </div>
				    </div>
					<div class="row">
						<div class="title">OR was this a shop expense?</div>
						<div class="control">
							<dx:ASPxCheckBox runat="server" CheckState="Unchecked" ClientInstanceName="cb_shop" ID="cb_shop">
								<ClientSideEvents CheckedChanged="shop_check_handler"></ClientSideEvents>
							</dx:ASPxCheckBox>
						</div>
					</div>
                    <div class="row">
						<div class="title">Business Unit:</div>
						<div class="control">
							
						    <dx:ASPxComboBox ID="ddl_div" runat="server" ClientInstanceName="ddl_div" DropDownStyle="DropDown" TextField="name" ValueField="id" ValueType="System.Int32" Width="90%" ClientEnabled="False">
                            </dx:ASPxComboBox>
                            
							
						</div>
					</div>

					<div class="row">
						<div class="title">*Pre-Tax Amount: <br />
							<div style="font-size:0.75em;">(Total Excluding taxes)</div></div> 
						<br />
						<div class="control">
					<table cellpadding="0" cellspacing="0">
						<tr> 						
							<td>
								<dx:ASPxTextBox runat="server" Width="100px" ClientInstanceName="t_amount" ID="t_amount">
									<ClientSideEvents LostFocus="amount_negative" />
									<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="v_expense">
										<RequiredField IsRequired="True" ErrorText="Field is Required"></RequiredField>
									</ValidationSettings>
								</dx:ASPxTextBox>
							</td>
							<td id="receipts_title" style="display: none" runat="server" class="receipts_title">Receipts (From this accounting period):</td>
							<td>
								<div id="receipts_row" style="display: none" class="title receipts_row" runat="server">
									<dx:ASPxComboBox ID="ddl_receipts" runat="server" ClientInstanceName="ddl_receipts" Width="100px" ValueType="System.Int32" TextField="item_text" ValueField="id" AutoPostBack="true" OnSelectedIndexChanged="ddl_receipts_SelectedIndexChanged">
										<ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" ValidationGroup="v_expense">
										</ValidationSettings>
										<ClientSideEvents SelectedIndexChanged="function(s,e){please_wait('start');}" />
									</dx:ASPxComboBox>
								</div>
							</td>
						</tr>
					</table>
</div>
					</div>
					<div class="row">
						<div class="title">*Total Due: <br />
							<div style="font-size:0.75em;">(Total Including taxes)</div></div> 
						<br />
						<div class="control">
					<table cellpadding="0" cellspacing="0">
						<tr> 						
							<td>
								<dx:ASPxTextBox runat="server" Width="100px" ClientInstanceName="total" ID="total">	
									<ClientSideEvents LostFocus="total_amount_validation" />
									<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="v_expense">
										<RequiredField IsRequired="True" ErrorText="Field is Required"></RequiredField>
									</ValidationSettings>
								</dx:ASPxTextBox>
							</td>
							
						</tr>
					</table>
</div>
					</div>

					<br />
					
					<div class="row">
						<div class="title">Currency:</div>
						<div class="control">
							<dx:ASPxComboBox ID="c_currency" runat="server" Width="100px" ReadOnly="true" Enabled="false">
								<Items>
									<dx:ListEditItem Text="USD" Value="USA" />
									<dx:ListEditItem Text="CDN" Value="CDN" />
									<dx:ListEditItem Text="EUR" Value="EUR" />
								</Items>
							</dx:ASPxComboBox>
						</div>
					</div>
                    <div class="row">
						<div class="title">*Credit Card Used:</div>
						<div class="control" visible="true">
							<dx:ASPxComboBox ID="ddl_credit_card" runat="server" Width="100px" ValueType="System.Int32" TextField="name" ValueField="id" ClientInstanceName="ddl_credit_card">
								<ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" ValidationGroup="v_expense">
								<RequiredField IsRequired="True" ErrorText="Field is Required"></RequiredField>
								</ValidationSettings>
							</dx:ASPxComboBox>
                            
						</div>
					</div>
					<div class="row">
						<div class="title">*Category of Purchase:</div>
						<div class="control">
							<dx:ASPxComboBox runat="server" AutoResizeWithContainer="True" ID="c_category" Width="90%" ValueType="System.Int32" 
							                 textfield="name" valuefield="account"
                                IncrementalFilteringMode="Contains" ClientInstanceName="c_category">
						
								<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="v_expense">
									<RequiredField IsRequired="True" ErrorText="Field is Required"></RequiredField>
								</ValidationSettings>
							</dx:ASPxComboBox>
						</div>
					</div>
					
					<div class="row" id="row_receiptnumber" runat="server">
						<div class="title">Receipt #:</div>
						<div class="control"><dx:ASPxTextBox runat="server" Width="90%" ClientInstanceName="t_receipt" ID="t_receipt"></dx:ASPxTextBox></div>
					</div>
					<div class="row">
						<div class="title">Physical copy of receipt:</br><div style="font-size:0.75em;">Please attach JPEG / PDF only</div></div>
						<div class="control">
							<dx:ASPxUploadControl ID="uc_receipt" runat="server" ClientInstanceName="uc_receipt" Width="90%" ShowProgressPanel="True" Native="true">
								<ValidationSettings MaxFileSize="8096000" AllowedFileExtensions=".jpe,.jpeg,.jpg,.pdf" NotAllowedFileExtensionErrorText="This file extension is not supported"></ValidationSettings>

                                <ClientSideEvents FileUploadStart="function(s, e) {
	please_wait('start');
}" />

<AdvancedModeSettings>
<FileListItemStyle CssClass="pending dxucFileListItem"></FileListItemStyle>
</AdvancedModeSettings>
							</dx:ASPxUploadControl>
							<b class="n">* Max file size - 4MB</b>
						</div>
					</div>
				
					<div class="row">
						<div class="title">
							*Description of&nbsp;item(s) purchased:<br />
							<em style="font-weight: normal; font-size: 10px; font-style: normal">This goes on the work order, so please be careful with wording.</em>
					</div>
						<div class="control">
							<dx:ASPxMemo runat="server" Height="100px" Width="90%" ID="memo_description">
								<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="v_expense">
								<RequiredField IsRequired="True" ErrorText="Field is Required"></RequiredField>
								</ValidationSettings>
							</dx:ASPxMemo>
							
							</div></div>
				 <div class="row" id="row_receiptlink" runat="server" visible="false">
						<div class="title">Link to copy of receipt:</div>
						<div  class="control"><dx:ASPxHyperLink runat="server" ID="hl_receipturl"></dx:ASPxHyperLink></br>
											<dx:ASPxLabel ID="UploadWarning" Text="If you upload a new receipt, this file will be backed up & replaced." runat="server" Visible="false"></dx:ASPxLabel>
						</div>
					</div>
					<div class="row">
						<div class="title"></div>
						<div class="control">
							<dx:ASPxButton ID="b_prev" runat="server" ClientInstanceName="b_prev" ToolTip="Prev Request" AutoPostBack="false" Visible="false">
								<Image Url="~/images/icon/icon[left].gif">
								</Image>
							</dx:ASPxButton>
							<dx:ASPxButton ID="b_save" runat="server" ClientInstanceName="b_save" Text="Save Request" ValidationGroup="v_expense" OnClick="b_save_Click" AutoPostBack="False" >
								
								<Image Url="~/images/icon/icon[save].gif">
								</Image>                                
							</dx:ASPxButton>
							<dx:ASPxButton ID="b_next" runat="server" ClientInstanceName="b_next" ToolTip="Next Request" AutoPostBack="false" Visible="false">
								<Image Url="~/images/icon/icon[right].gif">
								</Image>
							</dx:ASPxButton>
							<asp:SqlDataSource ID="ds_workorder" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
							</asp:SqlDataSource>
						</div>
					</div>
				</div>
				</dx:PanelContent>
			</PanelCollection>
		</dx:ASPxPanel>
        
    </div>
    </form>
</body>
</html>
