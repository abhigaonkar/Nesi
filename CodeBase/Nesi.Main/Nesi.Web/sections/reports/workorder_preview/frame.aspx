<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" EnableTheming = "True" Inherits="sections_reports_workorder_preview_frame" Codebehind="frame.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="../../../modules/signpad.ascx" tagname="signpad" tagprefix="uc" %>
<asp:Content ID="head" ContentPlaceHolderID="CPH_head" runat="server">
	
	<style  type="text/css">
		html
			{
			background-color:	#00447C;
			}
		.wrap { width: 100%;height: 100%;  overflow: hidden; }
		.frame 
        {
             width: 100%; 
           
		}
		.frame_wrapper 
			{
			padding-top:10px;
			background-color:#fff;
			}
		.bottom_frame 
			{
			background-color:	#00447C;
			padding-top:	25px;
			padding-bottom:	50px;
			}
		.btn
			{
			font-size:	1.5em;
			width:		78%;
			padding:		5px;
			font-weight:	bold;
			}
	</style>
	<script type="text/javascript">
		$(document).ready(function () {
			//loadingPanel0.Show();
			//setTimeout(function () {
			//	loadingPanel0.Hide();
		//	}, 2000);
			

			//var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
			//document.getElementsByTagName("iframe")[0].contentWindow.document.body.style.backgroundColor	= "#ff0000";
		});
		function use_signature(sig)
			{
			var printed_name		= "";
			var contact_id			= 0;
			var po_n				= 0;
			if(typeof combo_contact !== 'undefined')
				{
				contact_id			= combo_contact.GetValue();
				printed_name		= contact_id == 0 || contact_id == 1 || $("#chk_diff_signee").is(":checked") ? $(".printed_name").val() : combo_contact.GetText();
				}
			else
				{
				printed_name		= $(".printed_name").val();
				}
			var text_po				= $(".po_number");
			if(typeof text_po !== 'undefined' && text_po != null && $.trim(text_po.val()) != "")
				{
				po_n				= $.trim(text_po.val());
				}
			if($.trim(printed_name) == "" && (contact_id == 0 || contact_id == 1))
				{
				alert("Please enter a printed name");
				}
			else if(contact_id == 1 && recipient_list.GetSelectedItems().length == 0)
				{
				alert("Please select some contacts you want to send this to.");
				}
			else
				{
				if(recipient_list.GetSelectedItems().length != 0)
					{
					contact_id	= "";
					for(var i = 0; i < recipient_list.GetSelectedItems().length; i++)
						{
						var a	= recipient_list.GetSelectedItems()[i].value;
						contact_id += a+",";
						}
					contact_id	= contact_id.substr(0, contact_id.length - 1);
					}
				cbp.PerformCallback('usesignature|'+printed_name+'|'+contact_id+'|'+sig+'|'+po_n);
				}
			}
		function do_cancel()
			{
			var from = $(".hid_from").val();
			if(confirm("Are you sure you wish to cancel?"))
				{
				window.parent.close();
				}
		}
		function show_comments() {
			edit_comments.Show();
			edit_comments.PerformCallback();

		}
		function stuff() {
			alert("hello world");
		}
		function do_back()
			{
			var from = $(".hid_from").val();
			window.parent.close();
			}
		function cb_handler(s,e)
			{
			if(s.cpCLOSE && s.cpSUCCESS)
				{
				alert('Successfully approved WO');
				if(window.opener == null)
					{
					window.parent.location.href			= "/mobile/index.aspx?a=workorder&t=approval";
					}
				else
					{
					window.opener.location.reload();
					window.close();
					}
				}
			else if(s.cpDoUpdate)
				{
				delete s.cpDoUpdate;
				var vars	=	{ 
								id:				$(".hid_id").val(),
								is_signoff:		"true",
								pdf:			1
								}
				$.get("./index.aspx", vars,
						function(returned)
							{
							if(returned == "SUCCESS")
								{
								cbp.PerformCallback("updateworkorder|0");
								}
							else
								{
								alert("There was an error sending an email confirmation, but the signature has saved");
								window.parent.location.href			= "/mobile/index.aspx?a=signoff&woprog_id="+$(".hid_id").val();
								}
							});
				}
			else if(s.cpComplete)
				{
				delete s.cpComplete;
				if(s.cpEmail != "")
					{
					alert("Thank you for your signature, an email confirmation has been sent to: "+s.cpEmail);
					}
				else
					{
					alert("Thank you for your signature");
					}
				window.parent.location.href			= "/mobile/index.aspx?a=signoff&woprog_id="+$(".hid_id").val();
				}
			else if(s.cpERROR != null && s.cpERROR != "")
				{
				alert(s.cpERROR);
				delete s.cpERROR;
				}
		}
			
	</script>
</asp:Content>
<asp:Content ID="body" ContentPlaceHolderID="CPH_body" runat="server">
	<input type="hidden" id="hid_id" runat="server" class="hid_id" />
	<input type="hidden" id="hid_from" runat="server" class="hid_from" />
	<dx:ASPxCallbackPanel ID="cbp" runat="server" ClientInstanceName="cbp" oncallback="cbp_Callback" CssClass="wrap" Theme="NETheme01">
		<SettingsLoadingPanel Text="" />
		<ClientSideEvents EndCallback="cb_handler" />
		
		<Images>
			<LoadingPanel Url="~/images/loading_panel.gif">
			</LoadingPanel>
		</Images>
		<LoadingPanelImage Url="~/images/loading_panel.gif">
		</LoadingPanelImage>
		
		<PanelCollection>
			<dx:PanelContent>
				<dx:ASPxPanel ID="panel_frame" runat="server" Visible="false">
					<PanelCollection>
						<dx:PanelContent>
							<div class="frame_wrapper">
								<dx:ASPxLoadingPanel ID="loadingPanel0" runat="server" 
									ClientInstanceName="loadingPanel0" HorizontalAlign="Center" Modal="False" Text="" 
									VerticalAlign="Middle" Border-BorderStyle="None" BackColor="Transparent" 
									ContainerElementID="if_invoice">
									<Image Url="~/images/loading_panel.gif">
									</Image>

<Border BorderStyle="None"></Border>
								</dx:ASPxLoadingPanel>
								<iframe id="if_invoice" class="frame" runat="server" frameborder="0"></iframe>
							</div>
							<div align="center" class="bottom_frame">
								<dx:ASPxMemo ID="ts_comments" runat="server"  Height="71px" Width="78%">
								</dx:ASPxMemo>
								<button id="btn_edit_ts_comments" type="button" class="btn" 
									onclick="cbp.PerformCallback('save_comments|')" runat="server" style="margin-top: 5px;">
									Update Work Order Description</button>
                                <button id="btn_open_parent" type="button" class="btn" 
									 runat="server"  style="margin-top: 5px;">
									Open Parent Work Order</button>
								<button id="btn_pm_approve" type="button" class="btn" 
									onclick="if(confirm('Are you sure you want to move this to BM Approval?')){cbp.PerformCallback('pm_approve|0');}" 
									runat="server" style="margin-top: 5px;">PM Approve</button>
								<button id="btn_bm_approve" type="button" class="btn" onclick="if(confirm('Are you sure you want to move this to approve this work order?')){cbp.PerformCallback('bm_approve|0');}" runat="server">BM / DM Approve</button>
								<button id="btn_del_signature" type="button" class="btn" runat="server" visible="false" style="margin-top: 5px;">Delete Signature</button>
								<button id="btn_signature" type="button" class="btn" runat="server" visible="false" style="margin-top: 5px;">Add Signature</button>
								<button id="btn_back" type="button" class="btn" onclick="do_back()" style="margin-top:5px;" runat="server" visible="true">Back</button>
								<img runat="server" ID="signature_graphic" Visible="False" style="background-color:#fff;width:99%;height:auto;"></img>

								
								</img>

								
								</img>
                                </img>
                                </img>

								
								</img>

								
								</img>

								
								</img>

								
								</img>

								
								</img>

								
								</img>

								
								</img>

								
								</img>

								
								</img>

								
								</img>

								
								<div id="signature_printed" runat="server" visible="false" style="font-size:1.5em;color:#fff"></div>
							</div>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxPanel>
				<dx:ASPxPanel ID="panel_signature" runat="server" Visible="false">
					<PanelCollection>
						<dx:PanelContent>
							<uc:signpad ID="uc_signpad" runat="server" js_action="use_signature(signaturePad.toDataURL());" cancel_event="do_cancel();" is_mobile="true" />
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxPanel>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
</asp:Content>