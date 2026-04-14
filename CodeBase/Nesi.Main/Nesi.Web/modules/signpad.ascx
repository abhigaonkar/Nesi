<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_signpad" Codebehind="signpad.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

			
<link type="text/css" rel="stylesheet" href="/css/signature_pad.css" />
<script type="text/javascript" src="/js/signature_pad.js"></script>
	<div id="wrapper">
		<div align="center" id="signature-pad">
			<table width="98%" cellpadding="0" cellspacing="2">
				<tr>
					<td width="80%" align="right">
							<div style="width:410px;float:right; margin-right:2px;"><asp:TextBox ID="text_po" class="po_number" runat="server" Width="410px" Font-Size="2em" placeholder="Enter PO # (if applicable)" Visible="false"></asp:TextBox></div>
							<div style="width:35px;float:right;margin-left:5px;margin-right:10px;margin-top:3px;"><span style="font-size:0.85em;display:block;">CUST</span><span style="font-size:1.35em;margin-right:2px;">PO</span></div>
					</td>
					<td width="20%" valign="top" rowspan="3">
						<button type="button" data-action="cancel" style="width:100%;margin:2px;height:35px;" id="button_cancel" runat="server">Cancel</button>
						<button type="button" data-action="clear" style="width:100%;margin:2px;height:35px;" id="button_clear" runat="server">Clear Signature</button>
						<button type="button" data-action="save" style="width:100%;margin:2px;height:35px;" id="button_save" runat="server">Ok</button>
						<div class="chk_diff" align="center">
							<div class="text">Different signee than email recipient?</div>
							<input type="checkbox" id="chk_diff_signee" onchange="sig_obj.signee.handle_check(this)" />
						</div>
					</td>
				</tr>
				<tr>
					<td class="canvas">
						<canvas id="canvas"></canvas>
					</td>
				</tr>
				<tr>
					<td>
						<asp:TextBox ID="text_printed_name" class="printed_name" runat="server" Width="100%" onfocus="sig_obj.printed_name.focus(this);" onblur="sig_obj.printed_name.blur(this)" Font-Size="2em" placeholder="Print Name"></asp:TextBox>
						<table cellspacing="0" cellpadding="0" width="100%">
							<tr>
								<td width="66%">
									<dx:ASPxComboBox ID="combo_contact" runat="server" Width="100%" Font-Size="2em" NullText="Select Contact" height="35px" ClientInstanceName="combo_contact" ValueType="System.Int32" Native="true" TextField="name" ValueField="id" oncallback="combo_contact_Callback">
										<ClientSideEvents selectedindexchanged="save_contact.handle_selection_change" endcallback="save_contact.combo_contact_end_cb" />
									</dx:ASPxComboBox>
								</td>
								<td width="17%" align="right">
									<dx:ASPxButton ID="add_contact" Text="" runat="server" AutoPostBack="false" Width="95%" height="35px">
										<Image Url="/images/icon/icon[add].gif" Width="16" Height="16"></Image>
										<ClientSideEvents Click="function(s,e){popup_newcontact.PerformCallback(0);popup_newcontact.Show();}" />
									</dx:ASPxButton>
								</td>
								<td width="17%" align="right">
									<dx:ASPxButton ID="edit_contact" Text="" runat="server" AutoPostBack="false" Width="95%" height="35px">
										<Image Url="/images/icon/icon[edit].gif" Width="16" Height="16"></Image>
										<ClientSideEvents Click="function(s,e){var contact_id = combo_contact.GetValue(); if(contact_id == null){alert('Please select a contact to edit'); return false;} if(contact_id == 0 || contact_id == 1){alert('You cannot edit this contact.'); return false;} popup_newcontact.PerformCallback(combo_contact.GetValue());popup_newcontact.Show();}" />
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<div class="multiple_recipients" align="right">
<dx:aspxcallbackpanel id="cbp_recipient_list" runat="server" clientinstancename="cbp_recipient_list" oncallback="cbp_recipient_list_Callback">
	<panelcollection>
		<dx:panelcontent>
							<dx:aspxcheckboxlist	id="recipient_list" 
													runat="server" 
													clientinstancename="recipient_list" 
													clientvisible="false" 
													valuefield="id" 
													textfield="name" 
													valuetype="System.Int32" 
													width="100%" 
													border-borderwidth="0px" 
													repeatlayout="Table"  
													repeatcolumns="3" 
													font-size = "1.15em"
													 native ="true"
													repeatdirection="Horizontal" 
													checkboxstyle-cssclass="mrcb">
							</dx:aspxcheckboxlist>
		</dx:panelcontent>
	</panelcollection>
</dx:aspxcallbackpanel>
						</div>
					</td>
				</tr>
			</table>
		</div>
	</div>

	<dx:ASPxPopupControl ID="popup_newcontact" runat="server" ClientInstanceName="popup_newcontact" Width="500px" Maximized="True" PopupAnimationType="None" ShowHeader="False" onwindowcallback="popup_newcontact_WindowCallback">
		<ModalBackgroundStyle BackColor="Transparent" />
		<SettingsLoadingPanel Text="" />
<ContentCollection>
<dx:PopupControlContentControl runat="server">
	<div align="center" style="padding: 10px">
		<dx:ASPxTextBox ID="tb_name" ClientInstanceName="tb_name" runat="server" 
				Caption="Name" Font-Size="1.25em" Width="250px">
			<CaptionSettings VerticalAlign="Middle" />
			<CaptionCellStyle Width="125px">
			</CaptionCellStyle>
		</dx:ASPxTextBox>
		<dx:ASPxTextBox ID="tb_email" ClientInstanceName="tb_email" runat="server" Caption="Email" Font-Size="1.25em" Width="250px">
			<CaptionSettings VerticalAlign="Middle" />
			<CaptionCellStyle Width="125px">
			</CaptionCellStyle>
		</dx:ASPxTextBox>
		<input type="hidden" ID="hid_contact_id" runat="server" class="hid_contact_id" />
		<br />
		<dx:ASPxButton ID="bt_cancel" runat="server" Text="Cancel" Width="150px" native="true" AutoPostBack="false">
			<ClientSideEvents Click="function(s,e){popup_newcontact.Hide();}" />
		</dx:ASPxButton>
		<dx:ASPxButton ID="bt_save" runat="server" Text="Save" Width="150px" native="true" AutoPostBack="false">
			<ClientSideEvents Click="save_contact.run" />
		</dx:ASPxButton>
	</div>
	<dx:ASPxCallback ID="cb_newcontact" runat="server" ClientInstanceName="cb_newcontact" OnCallback="cb_newcontact_Callback">
		<ClientSideEvents BeginCallback="save_contact.start" CallbackComplete="save_contact.success" EndCallback="save_contact.stop" />
	</dx:ASPxCallback>
	<dx:ASPxCallback ID="cb_existingcontact" runat="server" ClientInstanceName="cb_existingcontact" OnCallback="cb_existingcontact_Callback">
		<ClientSideEvents BeginCallback="save_contact.start" CallbackComplete="save_contact.success" EndCallback="save_contact.stop" />
	</dx:ASPxCallback>
	</dx:PopupControlContentControl>
</ContentCollection>
		<LoadingPanelImage Url="~/images/loading_panel.gif">
		</LoadingPanelImage>
	<ClientSideEvents Shown="function(s,e){tb_name.Focus();}" />
	</dx:ASPxPopupControl>
	<script type="text/javascript" src="/js/signature_pad.min.js"></script>
	<script type="text/javascript">
		var signaturePad, canvas;
		function resizeCanvas()
		{
			var ratio =  Math.max(window.devicePixelRatio || 1, 1);
			canvas.width = canvas.offsetWidth * ratio;
			canvas.height = canvas.offsetHeight * ratio;
			canvas.getContext("2d").scale(ratio, ratio);
		}
		$(document).ready(function()
		{
			var wrapper			= document.getElementById("signature-pad"),
				clearButton			= wrapper.querySelector("[data-action=clear]"),
				saveButton			= wrapper.querySelector("[data-action=save]");
			cancelButton		= wrapper.querySelector("[data-action=cancel]");
			canvas				= wrapper.querySelector("canvas");
			signaturePad		= new SignaturePad(canvas);
			window.onresize		= resizeCanvas;
			resizeCanvas();
			$("body").css(
			{
				"-moz-user-select"		: "none",
				"-webkit-user-select"	: "none",
				"-ms-user-select"		: "none"
			});
			cancelButton.addEventListener("click",
				function (event)
				{
					<%= this.cancel_event %>
				});
			clearButton.addEventListener("click",
				function (event)
				{
					signaturePad.clear();
				});

			saveButton.addEventListener("click",
				function (event)
				{
					var po_n			= $(".po_number");
					var print_name_visible	= $(".printed_name").is(":visible");
					var print_name_blank	= $(".printed_name").val() == "";
					 if (signaturePad.isEmpty())
					{
						alert("Please provide signature first.");
					}
					else if(print_name_visible && print_name_blank)
						{
						alert("Please provide your name");
						}
					else if(typeof combo_contact !== 'undefined' && combo_contact.GetValue() == null)
					{
						alert("Please select a contact");
					}
					else if(typeof combo_contact === 'undefined' && print_name_blank)
					{
						alert("Please provide your name");
					}
					else 
					{
						var po_not_needed		= true;
						if(typeof po_n !== "undefined" && po_n.val().trim() == "")
						{
							po_not_needed		= confirm("The PO field is blank.\nPress cancel if you want to add a signature, otherwise press ok to proceed.");
						}
						if(po_not_needed)
						{
							if(confirm("Are you sure you want to approve this?"))
							{
								<%= this.js_action %>
							}
						}
						else
						{
							po_n.focus();
						}
					}
				});
		});
	</script>
	