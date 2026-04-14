<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_member_expense_if_newexpense" Codebehind="if_newexpense.aspx.cs" %>

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
			font-family:			arial;
					}
		.n			{
			color:					#060;
			font-weight:			bold; 
			font-size:				11px; 
			font-family:			arial;
					padding-bottom:	20px;
			}
.m			{
			color:					#000;
			font-weight:			bold; 
			font-size:				12px; 
			width:					15px; 
			font-family:			arial;
			text-align:				right;
			}
.v			{
			color:					#000;
			font-weight:			bold; 
			font-size:				11px;
			font-family:			arial;
			}
.con		{
			}
		.error		{
					color:			#f00;
					font-size:		12px;
					font-family:	arial;
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
	<script type="text/javascript" src="/js/wo_timesheet.js"></script>
	<script type="text/javascript">
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
			else
				{
				if(c_category.GetValue() == null)
					{
		//			c_category.SetValue(13020);
					}
				}
			}
		function shop_check_handler(s,e)
			{
			c_workorder.SetText("");
			c_workorder.SetEnabled(!s.GetChecked());
			}
		function handle_link(id, ext){boing(id, 'Expense', 800,600);}
		
		
		$(document).ready(function()
			{
			page_obj.update_panel_progress.bind();
			});
	var btn_clicked		= false;
	</script>
	
    <div>
		<asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
		<asp:UpdatePanel ID="up" runat="server">
			<ContentTemplate>
			

				<div class="expense">
					<asp:Label ID="lb_error" runat="server" CssClass="error"></asp:Label>
					
					<div style="padding-left:10px">* - Denotes a required field</div>
					<div class="row">
						<div class="title">User:</div>
						<div class="control">
							<dx:ASPxComboBox ID="cb_user" runat="server" Enabled="False" Width="90%" TextField="name" ValueField="id" AutoPostBack="true" ValueType="System.Int32" OnSelectedIndexChanged="cb_user_SelectedIndexChanged"></dx:ASPxComboBox>
							<asp:SqlDataSource ID="sds_users" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
						</div>
					</div>
					<div class="row">
						<div class="title">Purchase Date:<br/>
							<em style="font-weight: normal; font-size: 10px; font-style: normal">This defines the pay period the expense will be entered against</em></div>
						<div class="control">
							<dx:ASPxDateEdit ID="de_purchase" runat="server">
								<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorText="*" ValidationGroup="v_expense">
									<RequiredField ErrorText="Field is Required" IsRequired="True" />
								</ValidationSettings>
							</dx:ASPxDateEdit>
						</div>
					</div>
					<div class="row">
						<div class="title">*Purchased From?:</div>
						<div class="control">
							<dx:ASPxComboBox runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" DataSourceID="ds_sellers" TextField="name_seller" ValueField="id_seller" Width="90%" ID="c_seller" CallbackPageSize="100" EnableCallbackMode="True" ClientInstanceName="c_seller" ValueType="System.Int32">
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
							<dx:ASPxComboBox ID="c_workorder" runat="server" CallbackPageSize="25" ClientInstanceName="c_workorder" DropDownWidth="90%" DataSourceID="ds_workorder" DropDownStyle="DropDown" TextField="woprog_bvwo" ValueField="woprog_id" Width="90%" ValueType="System.Int32" AutoPostBack="True" OnSelectedIndexChanged="c_workorder_SelectedIndexChanged1">
								<ClientSideEvents LostFocus="wo_change_handler" />
								<Columns>
									<dx:ListBoxColumn FieldName="woprog_bvwo" Name="WO" Caption="WO" />
									<dx:ListBoxColumn Caption="Customer" FieldName="customer_name" Name="Customer" />
									<dx:ListBoxColumn Caption="Description" FieldName="description" Name="Description" />
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
						<div class="title">*Amount:<br /></div>
						<div class="control">
							<dx:ASPxTextBox runat="server" Width="170px" ClientInstanceName="t_amount" ID="t_amount" DisplayFormatString="N2">
								<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="v_expense">
								<RequiredField IsRequired="True" ErrorText="Field is Required"></RequiredField>
								</ValidationSettings>
							</dx:ASPxTextBox>
						</div>
					</div>
					<div class="row">
						<div class="title">Currency:</div>
						<div class="control">
							<dx:ASPxComboBox ID="c_currency" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="c_currency_SelectedIndexChanged">
								<Items>
									<dx:ListEditItem Text="USD" Value="USA" />
									<dx:ListEditItem Text="CAN" Value="CAN" />
									<dx:ListEditItem Text="EUR" Value="EUR" />
								</Items>
							</dx:ASPxComboBox>
						    </div>
                        <div class="control">
                            <dx:ASPxLabel ID="lbl_converted_currency" runat="server" Text=""></dx:ASPxLabel>
						    &nbsp;</div>
					</div>
					<div class="row">
						<div class="title">
                            *Category of Purchase:</div>
						<div class="control">
							<dx:ASPxComboBox runat="server" AutoResizeWithContainer="True" textfield="name" valuefield="account" 
                                ID="c_category" Width="90%" AutoPostBack="True" OnSelectedIndexChanged="c_category_SelectedIndexChanged" 
                                ValueType="System.String" IncrementalFilteringMode="Contains" ClientInstanceName="c_category">

								<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="v_expense">
									<RequiredField IsRequired="True" ErrorText="Field is Required"></RequiredField>
								</ValidationSettings>
							</dx:ASPxComboBox>
						</div>
					</div>
					<div class="row" id="row_distance" runat="server">
						<div class="title">Distance Traveled:</div>
						<div class="control">
							<dx:ASPxSpinEdit ID="se_distance" runat="server" Height="21px" Number="0" ClientInstanceName="se_distance" OnLoad="se_distance_Load">
							    
							</dx:ASPxSpinEdit>
							<dx:ASPxRadioButtonList ID="rbl_distance_unit" runat="server" RepeatDirection="Horizontal" ClientInstanceName="rbl_distance_unit" OnLoad="rbl_distance_unit_Load">
								<Items>
									<dx:ListEditItem Text="Kilometers" Value="KM" />
									<dx:ListEditItem Text="Miles" Value="M" />
								</Items>
								<Border BorderWidth="0px" />
							</dx:ASPxRadioButtonList>
						</div>
					</div>
					<div class="row" id="row_attendees" runat="server">
						<div class="title">Attendees:</div>
						<div class="control"><dx:ASPxTextBox ID="tb_attendees" runat="server" Width="90%"></dx:ASPxTextBox></div>
					</div>
					<div class="row" id="row_receiptnumber" runat="server">
						<div class="title">Receipt #:</div>
						<div class="control"><dx:ASPxTextBox runat="server" Width="90%" ClientInstanceName="t_receipt" ID="t_receipt"></dx:ASPxTextBox></div>
					</div>
					<div class="row">
						<div class="title">Physical copy of receipt:</div>
						<div class="control">
							<dx:ASPxUploadControl ID="uc_receipt" runat="server" ClientInstanceName="uc_receipt" Width="90%" Native="True">

<AdvancedModeSettings>
<FileListItemStyle CssClass="pending dxucFileListItem"></FileListItemStyle>
</AdvancedModeSettings>
							</dx:ASPxUploadControl>
						</div>
					</div>
					<div class="row" id="row_receiptlink" runat="server" visible="false">
						<div class="title">Link to copy of receipt:</div>
						<div class="control">
							<dx:ASPxHyperLink ID="hl_receipturl" runat="server"></dx:ASPxHyperLink><br />If you upload a new receipt, this file will be backed up & replaced.
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
						</div>
					</div>
					<div class="row">
						<div class="title"></div>
						<div class="control">
							<dx:ASPxButton ID="b_save" runat="server" ClientInstanceName="b_save" Text="Save Request" ValidationGroup="v_expense" OnClick="b_save_Click">
								<Image Url="~/images/icon/icon[save].gif">
								</Image>
							</dx:ASPxButton>
							<asp:SqlDataSource ID="ds_workorder" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
							</asp:SqlDataSource>
						</div>
					</div>
				</div>
			</ContentTemplate>
			<Triggers>
				<asp:PostBackTrigger ControlID="b_save" />
			</Triggers>
		</asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
