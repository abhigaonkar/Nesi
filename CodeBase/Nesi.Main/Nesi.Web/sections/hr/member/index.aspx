<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" EnableTheming = "True" Inherits="sections_hr_member_index" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="modules/wage.ascx" TagName="wage_tab" TagPrefix="uc" %>
<%@ Register Src="modules/it.ascx" TagName="it_tab" TagPrefix="uc" %>
<%@ Register src="modules/days_off.ascx" tagname="days_off" tagprefix="uc" %>
<%@ Register src="modules/vacation.ascx" tagname="vacation" tagprefix="uc" %>







<%@ Register src="modules/uc_member_usage.ascx" tagname="uc_member_usage" tagprefix="uc1" %>


<%@ Register src="modules/discipline_tab.ascx" tagname="discipline_tab" tagprefix="uc2" %>


<asp:Content ContentPlaceHolderID="header_placeholder" runat="server" ID="content5">

	<style type="text/css">
		.c				{
						font-family:	Calibri;
						font-size:		12px;
						font-weight:	bold;
						padding-left:	5px;
						}
		.v				{
			width:			auto;
						}
		.t.l			{
						border:			solid 1px #EAEAEA;
						margin:			2px;
						border-radius:	0px 0px 0px 0px;
						width:			100%;
						}
		.l				{
						float:			left;
						width:			575px;
						margin:			5px;
						}
		.r				{
						float:			left;
						width:			450px;
						margin:			5px;
						}
		.t.r			{
						border:			solid 1px #EAEAEA;
						margin:			2px;
						border-radius:	0px 0px 0px 0px;
						width:			100%;
						}
		.t	td			{			text-align: left;
		}
		.h				{
						font-family:	Calibri;
						font-size:		25px;
						
						color:			#004C7F;
						padding:		0px;
						border-spacing:	0px;
						}
		.sub_h			{
						font-family:	Calibri;
						font-size:		18px;
						
						color:			#fff;
						padding:		5px;
						border-bottom:	solid 1px #EAEAEA;
						background-color:#00B33C;



						}
		.lb_status			{
							font-family:	arial;
							color:			#090;
							font-size:		15px;
							font-weight:	bold;
							}
		.lb_status .dtl		{
							padding-left:	20px;
							font-size:		10px;
							white-space:	pre-wrap;
							white-space:	-moz-pre-wrap;  /* Mozilla, since 1999 */
							white-space:	-pre-wrap;      /* Opera 4-6 */
							white-space:	-o-pre-wrap;    /* Opera 7 */
							word-wrap:		break-word;       /* Internet Explorer 5.5+ */
							max-width:		600px;
							}
		.lb_status.error	{
							color:			#C00;
							}
		.sub_h img		{
						vertical-align:	middle;
						margin:			5px;
						width:			18px;
						height:			18px;
						}
		.t .photo		{
						width:				150px;
						height:				90px;
						background-position:	center center;
						background-size:	contain;
						position:			relative;
						background-repeat:	no-repeat;
						background-color:	#cacaca;
						}
		.t .photo .change_photo		
						{
						position:		absolute;
						right:			10px;
						bottom:			10px;
						display:		none;
						cursor:			pointer;
						}
		.t .photo .upload_control
						{
						background-color:	#fff;
						text-align:			center;
						padding:			25px 0px 25px 0px;
						display:			none;
						width:				75%;
						border-radius:		5px;
						position:			relative;
						}
		.t .photo .upload_control b
						{
						display:			block;
						border-bottom:		solid 1px #EAEAEA;
						margin-bottom:		5px;
						position:			absolute;
						top:				0px;
						width:				100%;
						color:				#004C7F;
						}
		.t .photo .upload_control input
						{
						margin-top:			10px;
						}
		.t .photo .upload_control .buttons
						{
						padding:			25px;
						}
		.textbox		{
						width:			95%;
						padding:		2px;
						font-family:	arial;
						font-size:		12px;
						}
		.sub			{
						font-family:	arial;
						font-size:		9px;
						}
		.textarea		{
						width:			95%;
						height:			100px;
						padding:		2px;
						font-family:	arial;
						font-size:		12px;
						}
		.select			{
						width:			97%;
						font-family:	arial;
						font-size:		12px;
						}
		.phone			{
						
						}
		.phone .text	{
						text-align:		center;
						}
		.phone .prefix input,
		.phone .area input	{
							width:		50px;
							text-align:	center;
							}
		.phone .suffix input	{
							width:		65px;
							text-align:	center;
							}
		.user_title		{
						font-weight:		bold;
						margin-top:			10px;
						}
		
	.vacation .v			{
							border-bottom:				dotted 1px #EAEAEA;
							width:						50%;
							font-size:					12px;
							font-weight:				bold;
							text-align:					center;
							}
	.vacation .c			{
								font-weight:				bold;
								font-size:					12px;
								font-family:				arial;
								width:						50%;
								border-right:				dotted 1px #EAEAEA;
								border-bottom:				dotted 1px #EAEAEA;
								}


	.vacation .c .months	{
							width:						35px;
							text-align:					center;
							font-weight:				bold;
							}

	.vacation .v .amount	{
							width:						50px;
							text-align:					center;
							font-weight:				bold;
							}


	.vacation .h			{
							background-color:			#cfc;
							font-size:					20px;
							font-weight:				bold;
							}

	.vacation .details		{
							font-size:					11px;
							}

	.vacation .details	b	{
										padding:					0px;
										color:						#000;
										}
		.style2
		{
			width: 100%;
		}
		.page
			{
			color:		#00c;
			}
		.page:hover
			{
			color:		#f60;
			}
		.priv
			{
			color:		#0a0;
			}
		.priv:hover
			{
			color:		#f60;
			}
		</style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">

	<script type="text/javascript">
		$("document").ready(function () {
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
			//Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			$(".photo").hover(function () {
				if ($(".photo").attr("data-expanded") != "true") {
					$(this).find('.change_photo').show();
				}
			},
				function () {
					$(this).find('.change_photo').hide();
				});
		});
		function toggle_children(obj, e) {
			if (!$(e.target).is("input:checkbox")) {
				if ($(obj).attr("data-expanded") != "false") {
					$(obj).children().find("li").each(function () {
						$(this).show();
					});
					$(obj).attr("data-expanded", "false");
					$(obj).css({ "list-style-image": "url('/images/icon/icon[minus].gif')" });
				}
				else {
					$(obj).children().find("li").each(function () {
						$(this).hide();
					});
					$(obj).attr("data-expanded", "true")
					$(obj).css({ "list-style-image": "url('/images/icon/icon[plus].gif')" });
				}
				e.stopPropagation();
			}
		}
		function toggle_picture_change(obj) {
			var _parent = null;
			if (obj == undefined || obj == null) {
				// from iframe
				_parent = $(".photo").parents("table:first");
			}
			else {
				_parent = $(obj).parents("table:first");
			}
			_parent.find(".sub_h, .c, .v").each(function () {
				$(this).toggle();
			});
			if ($(".photo").attr("data-expanded") == "true") {
				$(".photo").animate({ "height": "90px" });
				$(".photo").attr("data-expanded", "false");
				var orig_bgimage = $(".photo").attr("data-default_bg");
				$(".photo").css({ "border-radius": "0px", "background-image": "url('" + orig_bgimage + "')" });
				_parent.css({ "border-radius": "0px 0px 10px 0px" });
				$(".upload_control").hide();
			}
			else {
				$(".photo").animate({ "height": "250px" });
				$(".photo").attr("data-expanded", "true");
				$(".photo").css({ "border-radius": "0px 0px 10px 0px", "background-image": "url('/images/pixel.gif')" });
				_parent.css({ "border-radius": "0px 0px 10px 0px" });
				$(".upload_control").fadeIn(1000);
			}
		}
		function BeginReqHandler() {
			$(".update_progress").css({ "height": $("body").height() + "px", "z-index": $.maxZ() });
		}

		function print_review(id) {
			//	boing("/sections/reports/invoice_preview/index.aspx?id=" + id, 'invoice_preview', 850, 850);

			boing("print_review.aspx?rid=" + id , 'printrev', 900, 900);
		}

		function open_review(pass) {

			var id = pass[0];
			var mid = pass[1];


			boing("./review_list_for_member.aspx?id=" + id + "&memberid=" + mid, 'review' + id, 960, 720);
		}

		function handle_image_upload(obj) {

		}
		var user_switch			= {
			add:
				function(s,e)
					{
					var selected		= lb_userswitch_available.GetValue();
					if(selected == null)
						{
						alert("Please select a user to add.");
						}
					else
						{
						cb_user.PerformCallback('a|'+selected);
						}
					},
			remove:
				function(s,e)
					{
					var selected		= lb_userswitch_selected.GetValue();
					if(selected == null)
						{
						alert("Please select a user to remove.");
						}
					else
						{
						if(confirm("Are you sure you want to delete this mapping?"))
							{
							cb_user.PerformCallback('r|'+selected);
							}
						}
					},
			start_callback:
				function(s,e)
					{
					please_wait("start");
					},
			end_callback:
				function(s,e)
					{
					please_wait("stop");
					lb_userswitch_selected.PerformCallback();
					lb_userswitch_available.PerformCallback();
					}
					};
		function bind_tooltips() {
			$(".opt1").each(function () {
				$(this).tip();
			});
		}
		$(document).ready(function () {
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
		});

		function EndReqHandler() {
			bind_tooltips();
			please_wait("stop");
		}

		function testChecked(me) {
			frmLength = document.forms[0].length; //glbal
			if (document.getElementById(me).checked == true) {
				var arrname = document.getElementById(me).id.split("_");
				// chack all the parent pages
				CheckParent(arrname[2]);
			}
			else {  // uncheck
				var arrname = document.getElementById(me).id.split("_");
				if (arrname[0] == "!page") {
					UnCheckChildren(arrname[1]);

				}
			}
		}

		function CheckParent(this_id) {

			for (var c = 0; c < frmLength; c++) {
				var arrname = document.forms[0][c].id.split("_");
				if (arrname[0] == "!page" && arrname[1] == this_id) {
					document.forms[0][c].checked = true;
					if (arrname[2] != 0) {
						CheckParent(arrname[2]);
					}
				}
			}
		}

		function UnCheckChildren(parent_id) {

			UnCheckPriv(parent_id);
			for (var c = 0; c < frmLength; c++) {
				//alert("p:"+parent_id+" c:"+c);                  

				var arrname = document.forms[0][c].id.split("_");
				if ((arrname[0] == "!page") && arrname[2] == parent_id) {
					document.forms[0][c].checked = false;
					UnCheckChildren(arrname[1]);

				}
			}

		}

		function resizeIframe(obj) {
			obj.style.height = (obj.contentWindow.document.body.scrollHeight) + 'px';

		}
		function pc_tabchange(s,e)
			{
			e.processOnServer	= e.tab.index == 5; // Only needed for refreshing the privileges
			}
		function UnCheckPriv(parent_id) {

			for (var c = 0; c < frmLength; c++) {
				//alert("p:"+parent_id+" c:"+c);                  

				var arrname = document.forms[0][c].id.split("_");
				if ((arrname[0] == "!priv") && arrname[2] == parent_id) {
					document.forms[0][c].checked = false;
				}
			}
		}    
		var employee			= {
			};
	</script>
	<a name="top"></a>
										<div " style="position: fixed; top: 50%; left: 50%">	<asp:Label ID="errorlabel" 
		runat="server" Font-Bold="True" Font-Underline="True"
													ForeColor="Red" Font-Names="Arial"/></div>
	 <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>

		
												<asp:HiddenField ID="hdnmember" runat="server" />
		
	<div runat="server" id="js_protected">
	
	</div>
												<table style="width:98%;">
													<tr>
														<td align="center" >
															<dx:ASPxPageControl ID="pc" runat="server" ActiveTabIndex="0" ClientInstanceName="pc" Width="98%" style="text-align: left" Theme="NETheme01">
																<TabPages>
																	<dx:TabPage Name="0" Text="User Information">
																		<TabImage Url="~/images/icon/icon[member].gif">
																		</TabImage>
																		<ContentCollection>
																			<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
																				<dx:ASPxLabel ID="lb_status" runat="server" CssClass="lb_status" 
																					EncodeHtml="False">
																				</dx:ASPxLabel>
                                                                                <div style="position: fixed; top: 50%; left: 50%">
																				<dx:ASPxValidationSummary ID="vs" runat="server" ClientInstanceName="vs" 
																					ValidationGroup="v">
																				</dx:ASPxValidationSummary></div>
																				<table cellpadding="2" cellspacing="0" class="t l" width="700px">
																					<tr>
																						<td ID="member_photo" runat="server" align="center" class="photo" rowspan="5">
																							<img src='/images/icon/icon[photo].gif' class='change_photo' alt='change photo' onclick="toggle_picture_change(this)" title='Change Photo' />
																							<div class="upload_control">
																								<b>Upload a new photo for this user.</b><br />
																								<iframe ID="if_photo" runat="server" frameborder="0" height="150" width="100%">
																								</iframe>
																								<div class="buttons">
																									<button onclick="toggle_picture_change(this)" type="button">
																										Cancel
																									</button>
																								</div>
																							</div>
																						</td>
																						<td class="sub_h" colspan="2">
																							<img src='/images/icon/icon[member].gif' alt="member" />
																							Employee</td>
																						<td align="right" class="sub_h">
																							<dx:ASPxButton ID="bt_print_barcode" runat="server" 
																								OnClick="btn_printbarcode_Click" Text="Print Barcode" Visible="false">
																								<Image Url="~/images/icon/icon[print_barcode].GIF">
																								</Image>
																							</dx:ASPxButton>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							First Name (Given Name):</td>
																						<td class="v">
																							<dx:ASPxTextBox ID="tb_firstname" runat="server" AutoCompleteType="Disabled" 
																								CssClass="textbox" MaxLength="25" ValidationGroup="v">
																								<ValidationSettings Display="None" ValidationGroup="v">
																									<RequiredField ErrorText="* Please supply their first name" IsRequired="True" ></RequiredField>
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																						<td class="v">
																							&nbsp;</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Last Name (Surname):</td>
																						<td class="v">
																							<dx:ASPxTextBox ID="tb_lastname" runat="server" AutoCompleteType="Disabled"  
																								CssClass="textbox" MaxLength="25" ValidationGroup="v">
																								<ValidationSettings Display="None" ValidationGroup="v">
																									<RequiredField ErrorText="* Please supply their last name" IsRequired="True" ></RequiredField>
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																						<td class="v">
																							&nbsp;</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Middle Initial:</td>
																						<td class="v">
																							<dx:ASPxTextBox ID="tb_middleinitial" runat="server" CssClass="textbox"  AutoCompleteType="Disabled" 
																								MaxLength="1" ValidationGroup="v" Width="32px">
																							</dx:ASPxTextBox>
																						</td>
																						<td class="v">
																							&nbsp;</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Pref. Name (Nickname):</td>
																						<td class="v">
																							<dx:ASPxTextBox ID="tb_nickname" runat="server" CssClass="textbox"  AutoCompleteType="Disabled" 
																								MaxLength="100" ValidationGroup="v">
																							</dx:ASPxTextBox>
																						</td>
																						<td class="v">
																							&nbsp;</td>
																					</tr>
																				</table>
																				<table cellpadding="2" cellspacing="0" class="t l" width="100%">
																					<tr>
																						<td class="sub_h" colspan="2">
																							<img src='/images/icon/icon[home].gif' alt="home" />
																							Address / Phone</td>
																						<td class="sub_h">
																							&nbsp;</td>
																						<td class="sub_h">
																							&nbsp;</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Street Address:</td>
																						<td>
																							<dx:ASPxTextBox ID="tb_address" runat="server" 
																								CssClass="textbox" MaxLength="100"  AutoCompleteType="Disabled" 
																								>
																								
																							</dx:ASPxTextBox>
																						</td>
																						<td>
																							&nbsp;</td>
																						<td>
																							&nbsp;</td>
																					</tr>
																					<tr>
																						<td class="c">
																							City:</td>
																						<td>
																							<dx:ASPxTextBox ID="tb_city" runat="server" AutoCompleteType="Disabled"  
																								CssClass="textbox" MaxLength="100" >
																								
																							</dx:ASPxTextBox>
																						</td>
																						<td>
																							<strong>Province/State:</strong></td>
																						<td>
																							<asp:DropDownList ID="ddl_provstate" runat="server" CssClass="select" 
																								DataTextField="prov_desc" DataValueField="prov_abbv" >
																							</asp:DropDownList>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Country:</td>
																						<td>
																							<asp:DropDownList ID="ddl_country" runat="server" CssClass="select" 
																								ValidationGroup="v">
																								<asp:ListItem Value="CAN">Canada</asp:ListItem>
																								<asp:ListItem Value="USA">USA</asp:ListItem>
																								<asp:ListItem Value="OTHER">OTHER</asp:ListItem>
																							</asp:DropDownList>
																						</td>
																						<td>
																							<strong>Postal (ZIP) Code:</strong></td>
																						<td>
																							<dx:ASPxTextBox ID="tb_postal" runat="server" AutoCompleteType="Disabled"  
																								CssClass="textbox" MaxLength="10" ValidationGroup="v">
																								<ValidationSettings Display="None" ValidationGroup="v">
																									<RequiredField ErrorText="* Please supply their postal / zip code" 
																										 ></RequiredField>
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Personal Email Address:</td>
																						<td>
																							<dx:ASPxTextBox ID="tb_email" runat="server" AutoCompleteType="Disabled"   
																								CssClass="textbox" ValidationGroup="v">
																								<ValidationSettings Display="None" ValidationGroup="v">
																									<RegularExpression ErrorText="* Please enter a valid personal email address" 
																										ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
																									<RequiredField ErrorText="* Please supply their personal email address" ></RequiredField>
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																						<td>
																							<strong>Home Phone #:</strong></td>
																						<td>
																							<table cellpadding="2" cellspacing="0" class="phone">
																								<tr>
																									<td class="text">
																										(</td>
																									<td class="area">
																										<dx:ASPxTextBox ID="tb_homephone_area" runat="server" CssClass="textbox"  AutoCompleteType="Disabled"  
																											MaxLength="3" ValidationGroup="v">
																											<ValidationSettings Display="None" ValidationGroup="v">
																												<RegularExpression ErrorText="Please enter a valid 3 digit Home Phone # area code" 
																													ValidationExpression="\d{3}" ></RegularExpression>
																												<RequiredField ErrorText="* Please supply the home phone's area code" 
																													 ></RequiredField>
																											</ValidationSettings>
																										</dx:ASPxTextBox>
																									</td>
																									<td colspan="text">
																										)</td>
																									<td class="prefix">
																										<dx:ASPxTextBox ID="tb_homephone_pref" runat="server" CssClass="textbox"  AutoCompleteType="Disabled"  
																											MaxLength="3" ValidationGroup="v">
																											<ValidationSettings Display="None" ValidationGroup="v">
																												<RegularExpression ErrorText="Please enter a valid 3 digit Home Phone # prefix" 
																													ValidationExpression="\d{3}" ></RegularExpression>
																												<RequiredField ErrorText="* Please supply the home phone's prefix" 
																													></RequiredField>
																											</ValidationSettings>
																										</dx:ASPxTextBox>
																									</td>
																									<td colspan="text">
																										-</td>
																									<td class="suffix">
																										<dx:ASPxTextBox ID="tb_homephone_suff" runat="server" CssClass="textbox"  AutoCompleteType="Disabled"  
																											MaxLength="4" ValidationGroup="v">
																											<ValidationSettings Display="None" ValidationGroup="v">
																												<RegularExpression ErrorText="Please enter a valid 4 digit Home Phone # suffix" 
																													ValidationExpression="\d{4}" ></RegularExpression>
																												<RequiredField ErrorText="* Please supply the home phone's suffix" 
																													></RequiredField>
																											</ValidationSettings>
																										</dx:ASPxTextBox>
																									</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Personal Cell #:</td>
																						<td>
																							<table cellpadding="2" cellspacing="0" class="phone">
																								<tr>
																									<td class="text">
																										(</td>
																									<td class="area">
																										<dx:ASPxTextBox ID="tb_pecell_area" runat="server" CssClass="textbox"  AutoCompleteType="Disabled"  
																											MaxLength="3" ValidationGroup="v">
																											<ValidationSettings Display="None" ValidationGroup="v">
																												<RegularExpression ErrorText="Please enter a valid 3 digit Cell # area code" 
																													ValidationExpression="\d{3}"></RegularExpression>
																											</ValidationSettings>
																										</dx:ASPxTextBox>
																									</td>
																									<td colspan="text">
																										)</td>
																									<td class="prefix">
																										<dx:ASPxTextBox ID="tb_pecell_pref" runat="server" CssClass="textbox"  AutoCompleteType="Disabled"  
																											MaxLength="3" ValidationGroup="v">
																											<ValidationSettings Display="None" ValidationGroup="v">
																												<RegularExpression ErrorText="Please enter a valid 3 digit Cell # prefix" 
																													ValidationExpression="\d{3}" ></RegularExpression>
																											</ValidationSettings>
																										</dx:ASPxTextBox>
																									</td>
																									<td colspan="text">
																										-</td>
																									<td class="suffix">
																										<dx:ASPxTextBox ID="tb_pecell_suff" runat="server" CssClass="textbox"  AutoCompleteType="Disabled"  
																											MaxLength="4" ValidationGroup="v">
																											<ValidationSettings Display="None" ValidationGroup="v">
																												<RegularExpression ErrorText="Please enter a valid 4 digit Cell # suffix" 
																													ValidationExpression="\d{4}"></RegularExpression>
																											</ValidationSettings>
																										</dx:ASPxTextBox>
																									</td>
																								</tr>
																							</table>
																						</td>
																						<td class="c">
																							</td>
																						<td>
																								
																						</td>
																					</tr>
																				</table>
																				<table cellpadding="2" cellspacing="0" class="t l" width="700px">
																					<tr>
																						<td class="sub_h" colspan="2">
																							<img src='/images/icon/icon[wallet].gif' alt="home" />
																							Reporting Info</td>
																						<td class="sub_h">
																							&nbsp;</td>
																						<td class="sub_h">
																							&nbsp;</td>
																					</tr>
																					<tr>
																						<td class="c" nowrap="nowrap">
																							Job Title:</td>
																						<td width="50%">
																							<asp:DropDownList ID="ddl_title" runat="server" AutoPostBack="True" 
																								CssClass="select" DataTextField="MemberTypeName" DataValueField="MemberTypeID" 
																								OnSelectedIndexChanged="ddl_title_SelectedIndexChanged" ValidationGroup="v">
																							</asp:DropDownList>
																						</td>
																						<td class="style1" nowrap="nowrap">
																							<strong>Type of Employment:</strong></td>
																						<td>
																							<asp:DropDownList ID="ddl_emptype" runat="server" CssClass="select" 
																								DataTextField="text" DataValueField="value">
																							</asp:DropDownList>
																						</td>
																					</tr>
																					<tr ID="Tr1" runat="server">
																						<td id="Td5" runat="server" class="c" nowrap="nowrap">
																							Reports To:</td>
																						<td id="Td6" runat="server">
																							<asp:DropDownList ID="ddl_reportsto" runat="server" CssClass="select" 
																								DataTextField="member_fullname" DataValueField="id">
																							</asp:DropDownList>
																						</td>
																						<td id="Td7" runat="server" class="style1" colspan="2">
																							<asp:Label ID="lbl_reporting" runat="server"></asp:Label>
																							</td>
																					</tr>
																					<tr ID="Tr2" runat="server">
																						<td id="Td9" runat="server" class="c" nowrap="nowrap">
																							Is a Canadian Board Member:</td>
																						<td id="Td10" runat="server" align="left">
																							<dx:ASPxCheckBox ID="chk_can_isboardmember" runat="server" CheckState="Unchecked" 
																								Text=" ">
																							</dx:ASPxCheckBox>
																							
																						</td>
																						<td id="Td11" runat="server" class="style1" colspan="2">
																							</td>
																					</tr>
																					<tr ID="Tr3" runat="server">
																						<td id="Td13" runat="server" class="c" nowrap="nowrap">
																							Is an American Board Member:</td>
																						<td id="Td14" runat="server" align="left">
																							<dx:ASPxCheckBox ID="chk_us_isboardmember" runat="server" 
																								CheckState="Unchecked" Text=" ">
																							</dx:ASPxCheckBox>
																						</td>
																						<td id="Td15" runat="server" class="style1">
																							<strong>Part Time?</strong></td>
																						<td id="Td16" runat="server" width="50%">
																							
																							<dx:ASPxCheckBox ID="chkpart_time" runat="server" CheckState="Unchecked">
																							</dx:ASPxCheckBox>
																							
																						</td>
																					</tr>
																					<tr ID="wage_info" runat="server">
																						<td >
																							&nbsp;</td>
																						<td >&nbsp;
																						</td>
																						<td id="Td3" runat="server" class="style1">
																							<strong>Charge-out Rate:</strong></td>
																						<td id="Td4" runat="server" width="50%">
																							<asp:Label ID="lb_chargout" runat="server">Not Set</asp:Label>
																						</td>
																					</tr>
																					<tr id="row_payroll_handler" runat="server">
																						<td id="Td1" runat="server" class="c" nowrap="nowrap">Payroll Handler:</td>
																						<td id="Td2" runat="server">
																							<asp:DropDownList ID="ddl_payrollhandler" runat="server" CssClass="select" 
																								DataTextField="name" DataValueField="id">
																							</asp:DropDownList>
																						</td>
																					</tr>
																					<tr ID="Tr4" runat="server">
																						<td id="Td8" runat="server" class="c" nowrap="nowrap">
																							Timesheet Watch:</td>
																						<td id="Td17" runat="server">
																							<dx:ASPxCheckBox ID="chk_ts_watch" runat="server" 
																								CheckState="Unchecked" Text=" ">
																							</dx:ASPxCheckBox>
																						</td>
																						<td id="Td18" runat="server" class="style1">
																							&nbsp;</td>
																						<td id="Td19" runat="server" width="50%">
																							
																						</td>
																					</tr>
																				</table>
																				<br />
																				<table cellpadding="2" cellspacing="0" class="t r" width="700px">
																					<tr>
																						<td class="sub_h" colspan="2">
																							<img src='/images/icon/icon[company].gif' alt="company" />
																							Business Unit Info</td>
																					</tr>
																					<tr>
																						<td class="c" width="150">
																							Member ID:</td>
																						<td>
																							<dx:ASPxLabel ID="lb_id" runat="server" ClientInstanceName="member_id" 
																								Text="Not Set">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Business Unit:</td>
																						<td>
																							<asp:DropDownList ID="ddl_branch" runat="server" AutoPostBack="true" 
																								CssClass="select" DataTextField="name" DataValueField="id" 
																								OnSelectedIndexChanged="ddl_branch_SelectedIndexChanged" ValidationGroup="v">
																							</asp:DropDownList>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							</td>
																						<td>
																							
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Status:</td>
																						<td>
																							<asp:DropDownList ID="ddl_status" runat="server" CssClass="select" 
																								ValidationGroup="v">
																								<asp:ListItem Value="Not Active">Not Active</asp:ListItem>
																								<asp:ListItem Value="Active">Active</asp:ListItem>
																							</asp:DropDownList>
																						</td>
																					</tr>
																					<tr ID="hr_status" runat="server" visible="false">
																						<td class="c">
																							HR Status:</td>
																						<td>
																							<dx:ASPxComboBox ID="ddl_hrstatus" runat="server" DataSourceID="SqlDataSource3" 
																								Font-Names="Arial" TextField="status" ValueField="id" ValueType="System.Int32" OnLoad="ddl_hrstatus_Load">
																								<ClientSideEvents  SelectedIndexChanged="function(s, e)
{
	if (s.GetValue()==4)
		{
			if (confirm('Are you sure you want to TERMINATE this employee?'))
			{
				pop_term.Show();
			}
			else
			{
			s.SetValue(s.cp_set); 
			}
		}
	else if (s.GetValue()==5)
		{
		alert('You cant set the HR status manually to Past');
		s.SetValue(s.cp_set); 
		}
	
}" />
																							</dx:ASPxComboBox>
																							<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
																								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																								SelectCommand="Select member_hrstatus.id, member_hrstatus.status from member_hrstatus">
																							</asp:SqlDataSource>
																						</td>
																					</tr>
																					
																					<tr id="row_username" runat="server">
																						<td class="c">
																							Username:</td>
																						<td>
																							<dx:ASPxTextBox ID="tb_username" runat="server" CssClass="textbox" 
																								MaxLength="50" ValidationGroup="v">
																								<ValidationSettings Display="None" ValidationGroup="v">
																									<RequiredField ErrorText="* Please supply their username" IsRequired="True" ></RequiredField>
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																					<tr id="row_password" runat="server">
																						<td class="c">
																							Password:</td>
																						<td>
																							<dx:ASPxTextBox ID="tb_password" runat="server" CssClass="textbox" 
																								MaxLength="50" ValidationGroup="v">
																								<ValidationSettings Display="None" ValidationGroup="v">
																									<RequiredField ErrorText="* Please supply their password" IsRequired="True"></RequiredField>
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Work Email:</td>
																						<td>
																							<dx:ASPxLabel ID="lb_neemail" runat="server"></dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Start Date:</td>
																						<td>
																							<dx:ASPxDateEdit ID="date_start" runat="server" EditFormat="Custom" 
																								EditFormatString="yyyy-MM-dd">
																								<ValidationSettings ValidationGroup="v">
																									<RequiredField ErrorText="* Please supply their start date" IsRequired="True"></RequiredField>
																								</ValidationSettings>
																							</dx:ASPxDateEdit>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							End Date:</td>
																						<td>
																							<dx:ASPxDateEdit ID="date_end" runat="server" EditFormat="Custom" 
																								EditFormatString="yyyy-MM-dd">
																								<ValidationSettings>
																									<RequiredField ErrorText=""  ></RequiredField>
																								</ValidationSettings>
																							</dx:ASPxDateEdit>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Vehicle #:</td>
																						<td>
																							<dx:ASPxTextBox ID="tb_truck_number" runat="server" CssClass="textbox">
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Default Inventory Location:</td>
																						<td>
																							<asp:DropDownList ID="ddl_default_location" runat="server" CssClass="select" 
																								DataTextField="name" DataValueField="id">
																							</asp:DropDownList>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Work Cell #:</td>
																						<td>
																							<table cellpadding="2" cellspacing="0" class="phone">
																								<tr>
																									<td class="text">
																										(</td>
																									<td class="area">
																										<dx:ASPxLabel ID="lb_necell_area" runat="server" ></dx:ASPxLabel>
																									</td>
																									<td colspan="text">
																										)</td>
																									<td class="prefix">
																										<dx:ASPxLabel ID="lb_necell_pref" runat="server"></dx:ASPxLabel>
																									</td>
																									<td colspan="text">
																										-</td>
																									<td class="suffix">
																										<dx:ASPxLabel ID="lb_necell_suff" runat="server"></dx:ASPxLabel>
																									</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Extension #:</td>
																						<td>
																							<dx:ASPxLabel ID="lb_extension" runat="server"></dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td class="c" valign="top">
																							Notes:</td>
																						<td>
																							<dx:ASPxMemo ID="tb_notes" runat="server" Height="150px" Width="50%">
																							</dx:ASPxMemo>
																						</td>
																					</tr>
                                                                                    <tr>
																						<td class="c">
																							Electrical License #:</td>
																						<td>
																							<dx:ASPxTextBox ID="tb_electrical_license" runat="server" CssClass="textbox">
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
                                                                                    <tr>
																						<td class="c">
																							Apprentice Contract:</td>
																						<td>
																							<dx:ASPxTextBox ID="tb_apprentice_contract" runat="server" CssClass="textbox">
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																				</table>
																				<table cellpadding="2" cellspacing="0" class="t r" id="personal_details" runat="server">
																					<tr>
																						<td class="sub_h" colspan="2">
																							<img src='/images/icon/icon[edit].gif' alt="personal" />
																							Personal Info</td>
																					</tr>
																					<tr>
																						<td class="c" width="150">
																							Date of Birth:</td>
																						<td align="left">
																							<dx:ASPxDateEdit ID="date_birth" runat="server" EditFormat="Custom" 
																								EditFormatString="yyyy-MM-dd">
																								<ValidationSettings>
																									<RequiredField ErrorText=""></RequiredField>
																								</ValidationSettings>
																							</dx:ASPxDateEdit>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							SIN / GST / SSN #:</td>
																						<td>
																							<dx:ASPxTextBox ID="tb_social" runat="server" CssClass="textbox"  AutoCompleteType="Disabled" 
																								ValidationGroup="v">
																								<ValidationSettings>
																									<RequiredField ErrorText="" ></RequiredField>
																									<RegularExpression ErrorText=""  ></RegularExpression>
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Driver&#39;s License #:</td>
																						<td>
																							<dx:ASPxTextBox ID="tb_drivers_license" runat="server" CssClass="textbox" AutoCompleteType="Disabled" >
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																					
																					
																					<tr>
																						<td class="c">
																							Has Dependants:</td>
																						<td>
																							<asp:DropDownList ID="ddl_hasdependants" runat="server" CssClass="select" 
																								ValidationGroup="v">
																								<asp:ListItem>Yes</asp:ListItem>
																								<asp:ListItem>No</asp:ListItem>
																							</asp:DropDownList>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Benefits ID:</td>
																						<td>
																							<table style="width:100%;">
																								<tr>
																									<td>
																										<dx:ASPxTextBox ID="tb_benefits_id" runat="server" Width="170px">
																										</dx:ASPxTextBox>
																									</td>
																									<td>
																										<dx:ASPxLabel ID="lbl_benefits_startdate" runat="server" Wrap="false">
																										</dx:ASPxLabel>
																									</td>
																									<td width="100%">
																										&nbsp;</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Life Insurance ID:</td>
																						<td>
																							<table style="width:100%;">
																								<tr>
																									<td>
																										<dx:ASPxTextBox ID="tb_life_insurance_id" runat="server" Width="170px">
																										</dx:ASPxTextBox>
																									</td>
																									<td>
																										&nbsp;</td>
																									<td width="100%">
																										&nbsp;</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																					<tr>
																						<td class="c">
																							Payroll ID:</td>
																						<td>
																							<table style="width:100%;">
																								<tr>
																									<td>
																										<dx:ASPxTextBox ID="tb_payroll_id" runat="server" Width="170px">
																										</dx:ASPxTextBox>
																									</td>
																									<td>
																										<dx:ASPxCallback ID="cb_reset_todo" runat="server" 
																											ClientInstanceName="cb_reset_todo" OnCallback="cb_reset_todo_Callback">
																											<ClientSideEvents CallbackComplete="function(s, e) {
window.opener.location.href = window.opener.location.href;
btn_payroll_verified.SetEnabled(false);
alert('Data Updated');
}" />
<ClientSideEvents CallbackComplete="function(s, e) {
window.opener.location.href = window.opener.location.href;
btn_payroll_verified.SetEnabled(false);
alert(&#39;Data Updated&#39;);
}"></ClientSideEvents>
																										</dx:ASPxCallback>
																										<dx:ASPxButton ID="btn_payroll_verified" runat="server" Text="Data Verified" 
																											Wrap="False" ClientEnabled="False" AutoPostBack="False" ClientInstanceName="btn_payroll_verified">
																											<ClientSideEvents Click="function(s, e) {
	cb_reset_todo.PerformCallback('payroll');
}" />
																										</dx:ASPxButton>
																									</td>
																									<td width="100%">
																										&nbsp;</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																				</table>
																				<table cellpadding="2" cellspacing="0" class="t r" width="700px">
																					<tr>
																						<td class="sub_h">
																							<img src='/images/icon/icon[edit].gif' alt="personal" />
																							Emergency Contact</td>
																					</tr>
																					<tr>
																						<td>
																							
																												<table cellpadding="2" cellspacing="0" width="100%">
																													<tr>
																														<td align="center" class="c" colspan="2" width="50%">
																															<u>Contact 1</u></td>
																														
																													</tr>
																													<tr>
																														<td class="c">
																															First Name:</td>
																														<td>
																															<dx:ASPxTextBox ID="tb_emerg_fname1" runat="server" CssClass="textbox" AutoCompleteType="Disabled" >
																																<ValidationSettings Display="None">
																																	<RequiredField ErrorText="" ></RequiredField>
																																</ValidationSettings>
																															</dx:ASPxTextBox>
																														</td>
																														
																													</tr>
																													<tr>
																														<td class="c">
																															Last Name:</td>
																														<td>
																															<dx:ASPxTextBox ID="tb_emerg_lname1" runat="server" CssClass="textbox" AutoCompleteType="Disabled" >
																																<ValidationSettings Display="None">
																																	<RequiredField ErrorText="" ></RequiredField>
																																</ValidationSettings>
																															</dx:ASPxTextBox>
																														</td>
																														
																													</tr>
																													<tr>
																														<td class="c">
																															Phone #:</td>
																														<td>
																															<table cellpadding="2" cellspacing="0" class="phone">
																																<tr>
																																	<td class="text">
																																		(</td>
																																	<td class="area">
																																		<dx:ASPxTextBox ID="tb_emerg1_area" runat="server" CssClass="textbox"  AutoCompleteType="Disabled"
																																			MaxLength="3">
																																			<ValidationSettings Display="None">
																																				<RequiredField ErrorText="* Please enter a phone area code for Contact 1" ></RequiredField>
																																			</ValidationSettings>
																																		</dx:ASPxTextBox>
																																	</td>
																																	<td colspan="text">
																																		)</td>
																																	<td class="prefix">
																																		<dx:ASPxTextBox ID="tb_emerg1_pref" runat="server" CssClass="textbox"  AutoCompleteType="Disabled"
																																			MaxLength="3">
																																			<ValidationSettings Display="None">
																																				<RequiredField ErrorText="* Please enter a phone prefix for Contact 1" ></RequiredField>
																																			</ValidationSettings>
																																		</dx:ASPxTextBox>
																																	</td>
																																	<td colspan="text">
																																		-</td>
																																	<td class="suffix">
																																		<dx:ASPxTextBox ID="tb_emerg1_suff" runat="server" CssClass="textbox"  AutoCompleteType="Disabled"
																																			MaxLength="4">
																																			<ValidationSettings Display="None">
																																				<RequiredField ErrorText="* Please enter a phone suffix for Contact 1" ></RequiredField>
																																			</ValidationSettings>
																																		</dx:ASPxTextBox>
																																	</td>
																																</tr>
																															</table>
																														</td>
																														
																													</tr>
																												</table>
																											
																							</td>
																					</tr>
																				</table>
																				
																				<div class="l">
																					<table>
																						<tr>
																							<td>
																								<dx:ASPxButton ID="bt_save" runat="server" OnClick="bt_save_Click" Text="Save" 
																									ValidationGroup="v" HorizontalAlign="Center">
																									<Image Url="~/images/icon/icon[save].gif">
																									</Image>
																								</dx:ASPxButton>
																							</td>
																							<td>
																								<dx:ASPxButton ID="bt_newhire" runat="server" AutoPostBack="False" 
																									Text="New Hire Sheet" Visible="False" Width="128px" HorizontalAlign="Center">
																									<ClientSideEvents Click="function(s, e) {
	var id = member_id.GetText();
	boing(&quot;./print_off.aspx?id=&quot;+id, &quot;EmployeeInfo&quot;, 800,1024);
}" />
																									<Image Url="~/images/icon/icon[report].gif">
																									</Image>
																								</dx:ASPxButton>
																							</td>
																							<td>
																								<dx:ASPxButton ID="bt_terminate" runat="server" AutoPostBack="False" 
																									Text="Terminate User" Width="128px" ClientVisible="False" HorizontalAlign="Center">
																									<ClientSideEvents />
																									<Image Url="~/images/icon/icon[report].gif">
																									</Image>
																								</dx:ASPxButton>
																							</td>
																						</tr>
																					</table>
																				</div>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Name="1" Text="IT" ClientEnabled="false">
																		<TabImage Url="~/images/icon/icon[support].gif">
																		</TabImage>
																		<ContentCollection>
																			<dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
																				<uc:it_tab ID="it_tab" runat="server" />
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage ClientEnabled="False" Name="2" Text="Wage">
																		<TabImage Url="~/images/icon/icon[wallet].gif">
																		</TabImage>
																		<ContentCollection>
																			<dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
																				<uc:wage_tab ID="wage_tab" runat="server" />
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage ClientEnabled="False" Name="3" Text="Days Off / Vacation">
																		<TabImage Url="~/images/icon/icon[calendar].gif">
																		</TabImage>
																		<ContentCollection>
																			<dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
																				<div align="left">
																				<uc:days_off ID="uc_days_off" runat="server" />
																				<br />
																				<br />
																				<br />
																				<uc:vacation ID="uc_vacation" runat="server" />
																				</div>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage ClientEnabled="False" Name="4" Text="Disciplinary">
																		<TabImage Url="~/images/icon/icon[alive].gif">
																		</TabImage>
																		<ContentCollection>
																			<dx:ContentControl ID="ContentControl6" runat="server" SupportsDisabledAttribute="True">
																				
																			
																				<uc2:discipline_tab ID="discipline_tab1" runat="server" />
																				
																			
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage ClientEnabled="False" Name="5" Text="Privileges">
																		<TabImage Url="~/images/icon/icon[report].gif">
																		</TabImage>
																		
																		<ContentCollection>
																			<dx:ContentControl ID="ContentControl7" runat="server" SupportsDisabledAttribute="True">
																				<dx:ASPxPanel ID="pnl_userswitch" runat="server">
																					<PanelCollection>
																						<dx:PanelContent>
																						<table cellpadding="2" cellspacing="0" width="550" style="border:solid 1px #ccc; margin:10px;">
																							<tr>
																								<td colspan="3" style="font-weight:bold;font-size:20px;">User Switching</td>
																							</tr>
																							<tr>
																								<td align="center"><b>Available Users</b></td>
																								<td>&nbsp;</td>
																								<td align="center"><b>Selected Users</b></td>
																							</tr>
																							<tr>
																								<td width="45%"><dx:ASPxListBox runat="server" ID="lb_userswitch_available" ClientInstanceName="lb_userswitch_available" DataSourceID="ds_userswitch_available" TextField="text" ValueField="id" Width="100%" Height="250px" OnCallback="lb_userswitch_available_Callback"></dx:ASPxListBox><asp:SqlDataSource ID="ds_userswitch_available" runat="server"
																								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT member_id id, member_fullname text FROM member WHERE member_status='Active' AND business_unit_id = 8 AND member_id NOT IN (SELECT mapped_member_id FROM user_switch WHERE member_id = @member_id) ORDER BY member_lastname, member_fullname"><SelectParameters><asp:ControlParameter ControlID="hdnmember" PropertyName="value" Name="@member_id" /></SelectParameters></asp:SqlDataSource></td>	
																								<td width="10%" align="center">	
																								<dx:ASPxButton runat="server" ID="bt_userswitch_moveright" AutoPostBack="false" Text=">>">
																								<ClientSideEvents Click="user_switch.add" />
<ClientSideEvents Click="user_switch.add"></ClientSideEvents>
																								</dx:ASPxButton>
																									<dx:ASPxCallback ID="cb_user" ClientInstanceName="cb_user" runat="server" OnCallback="cb_user_Callback">
																										<ClientSideEvents BeginCallback="user_switch.start_callback" EndCallback="user_switch.end_callback" />
<ClientSideEvents BeginCallback="user_switch.start_callback" EndCallback="user_switch.end_callback"></ClientSideEvents>
																									</dx:ASPxCallback>
																									<dx:ASPxButton runat="server" ID="bt_userswitch_moveleft" AutoPostBack="false" Text="<<">
																								<ClientSideEvents Click="user_switch.remove" />
<ClientSideEvents Click="user_switch.remove"></ClientSideEvents>
																									</dx:ASPxButton>	
																								</td>	
																								<td width="45%"><dx:ASPxListBox runat="server" ID="lb_userswitch_selected" ClientInstanceName="lb_userswitch_selected" Width="100%" Height="250px" DataSourceID="ds_userswitch_selected" TextField="text" ValueField="id" OnCallback="lb_userswitch_selected_Callback"></dx:ASPxListBox>
																									<asp:SqlDataSource ID="ds_userswitch_selected" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT b.member_id id, b.member_fullname text FROM user_switch a LEFT JOIN member b ON a.mapped_member_id = b.member_id WHERE a.member_id = @member_id ORDER BY b.member_lastname, b.member_fullname">
																										<SelectParameters>
																											<asp:ControlParameter ControlID="hdnmember" Name="@member_id" PropertyName="value" />
																										</SelectParameters>
																									</asp:SqlDataSource>
																								</td>	
																							</tr>
																						</table>
																						</dx:PanelContent>
																					</PanelCollection>
																				</dx:ASPxPanel>
																				<dx:ASPxPanel ID="pnl_privileges" runat="server">
																					<PanelCollection>
																						<dx:PanelContent>
																				<table style="width: 100%">
																					<tr>
																						<td>
																							<asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
																						</td>
																						<td>
																						</td>
																					</tr>
																					<tr>
																						<td style="white-space: nowrap">
																							<asp:Label ID="lblCompanyList" runat="server" 
																								Text=" Only apply mouseover popups for this company -&gt;"></asp:Label>
																							<asp:DropDownList ID="ddlcompany" runat="server" AutoPostBack="True" 
																								OnSelectedIndexChanged="ddlcompany_SelectedIndexChanged">
																							</asp:DropDownList>
																						</td>
																						<td style="white-space: nowrap">
																						</td>
																					</tr>
																					<tr>
																						<td style="white-space: nowrap">
																							<asp:DropDownList ID="ddlview" runat="server" AutoPostBack="True" 
																								OnSelectedIndexChanged="ddlview_SelectedIndexChanged">
																							</asp:DropDownList>
																							<asp:DropDownList ID="ddlMemberType" runat="server" AutoPostBack="true" 
																								DataTextField="MemberTypeName" DataValueField="MemberTypeID" 
																								OnSelectedIndexChanged="ddlMemberType_SelectedIndexChanged" Visible="false">
																							</asp:DropDownList>
																							<span ID="divmembertype" runat="server"></span>
																						</td>
																						<td style="white-space: nowrap">
																							&nbsp;
																						</td>
																					</tr>
																					<tr>
																						<td style="white-space: nowrap">
																							<asp:Label ID="lblPrivileges" runat="server" 
																								Text="Exceptions to the standard Member Type privilege set up are highlighted.."></asp:Label>
																						</td>
																						<td style="width: 100%">
																						</td>
																					</tr>
																				</table>
																				<div id="div_global" runat="server"></div>
																				<div ID="divSiteMap" runat="server" align="left"></div>

																				<asp:Button ID="btnEditPriv" runat="server" OnClick="btnEditPriv_Click" Text="Submit Changes" ValidationGroup="vg_priv" />
																				<asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="Reset Privileges by Member type" ValidationGroup="vg_priv" />
																				<asp:Button ID="btnMbrTypPriv" runat="server" OnClick="btnMbrTypPriv_Click" Text="Submit MemberType Changes" ValidationGroup="vg_priv" Visible="False" />
																				<asp:CheckBox ID="chkCascadeChanges" runat="server" Text="Apply and Overwrite All Employees Privileges of This Member Type" ValidationGroup="vg_priv" Visible="False" />
																						</dx:PanelContent>
																					</PanelCollection>
																				</dx:ASPxPanel>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage ClientEnabled="False" Name="6" Text="Files">
																		<TabImage Url="~/images/icon/icon[file].gif">
																		</TabImage>
																		<ContentCollection>
																			<dx:ContentControl ID="ContentControl8" runat="server" SupportsDisabledAttribute="True">
																				<fieldset style="margin-left:0; margin-right:0;padding:0; text-align: left;">
																					<dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" 
																						Text="Copy from Applicant Files" Width="200px">
																					</dx:ASPxButton>
																					<br />
																					<table style="width:100%;">
																						<tr>
																							<td>
																								<div ID="div_files" runat="server" align="left">
																								</div>
																							</td>
																						</tr>
																					</table>
																				</fieldset><br />
																				<asp:HiddenField ID="hidMemberID" runat="server" />
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage ClientVisible="false" Name="7" Text="Termination">
																		<TabImage Url="~/images/icon/icon[cancel].gif">
																		</TabImage>
																		<ContentCollection>
																			<dx:ContentControl ID="ContentControl9" runat="server" SupportsDisabledAttribute="True">
																				<iframe ID="if_termination" runat="server" frameborder="0" height="1024" class="if_termination"
																					name="if_termination" width="98%"></iframe>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage ClientEnabled="False" Name="8" Text="Employment Agreements">
																		<ContentCollection>
																			<dx:ContentControl ID="ContentControl10" runat="server" SupportsDisabledAttribute="True">
																				<table style="width: 100%;">
																					<tr>
																						<td>
																							<asp:SqlDataSource ID="membertype1" runat="server" 
																								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																								SelectCommand="Select membertype_id,membertype_name from membertype where active = 'T'">
																							</asp:SqlDataSource>
																							<dx:ASPxGridView ID="gv_offers" runat="server" AutoGenerateColumns="False" 
																								ClientInstanceName="gv_offers" DataSourceID="SqlDataSource1" KeyFieldName="id" 
																								OnCancelRowEditing="gv_offers_CancelRowEditing"  oncustomcallback="gv_offers_CustomCallback"
																								OnHtmlEditFormCreated="gv_offers_HtmlEditFormCreated" 
																								OnRowDeleting="gv_offers_RowDeleting" Width="100%" OnCommandButtonInitialize="gv_offers_CommandButtonInitialize" Theme="NETheme01">
																								
																							<SettingsCommandButton>
																								<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																								<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																								<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																							</SettingsCommandButton>
																								<Columns>
																									<dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"
																										ShowInCustomizationForm="True" VisibleIndex="0" Width="40px">
																										
																										
																										
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewCommandColumn>
																									<dx:GridViewDataDateColumn Caption="Date" FieldName="date" MinWidth="10" 
																										ShowInCustomizationForm="True" VisibleIndex="2" Width="90px">
																										<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																											EditFormatString="yyyy-MM-dd"></PropertiesDateEdit>
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataDateColumn>
																									<dx:GridViewDataComboBoxColumn Caption="Author" FieldName="enteredby" 
																										MinWidth="10" ShowInCustomizationForm="True" VisibleIndex="3" Width="120px">
																										<PropertiesComboBox DataSourceID="sqlmember" TextField="name" 
																											ValueField="member_id" ValueType="System.Int32"></PropertiesComboBox>
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataComboBoxColumn>
																									<dx:GridViewDataComboBoxColumn Caption="Branch" FieldName="business_unit_id" 
																										MinWidth="10" ShowInCustomizationForm="True" Visible="False" VisibleIndex="4" 
																										Width="70px">
																										<PropertiesComboBox DataSourceID="sqlcompany" TextField="name" 
																											ValueField="id" ValueType="System.Int32"></PropertiesComboBox>
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataComboBoxColumn>
																									<dx:GridViewDataTextColumn Caption="Wage" FieldName="wage" 
																										ShowInCustomizationForm="True" Visible="False" VisibleIndex="5" Width="50px">
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataDateColumn Caption="Start Date" FieldName="startdate" 
																										ShowInCustomizationForm="True" VisibleIndex="6" Width="90px">
																										<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																											EditFormatString="yyyy-MM-dd"></PropertiesDateEdit>
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataDateColumn>
																									<dx:GridViewDataDateColumn Caption="End Date" FieldName="enddate" 
																										ShowInCustomizationForm="True" VisibleIndex="7" Width="90px">
																										<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																											EditFormatString="yyyy-MM-dd"></PropertiesDateEdit>
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataDateColumn>
																									<dx:GridViewDataTextColumn Caption="Title" FieldName="membertypeid" 
																										ShowInCustomizationForm="True" VisibleIndex="11" Width="120px">
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataCheckColumn Caption="Vehicle" FieldName="gets_vehicle" 
																										ShowInCustomizationForm="True" Visible="False" VisibleIndex="12" Width="50px">
																									</dx:GridViewDataCheckColumn>
																									<dx:GridViewDataCheckColumn Caption="Cell Phone" FieldName="gets_phone" 
																										ShowInCustomizationForm="True" Visible="False" VisibleIndex="13" Width="50px">
																									</dx:GridViewDataCheckColumn>
																									<dx:GridViewDataCheckColumn Caption="Laptop" FieldName="gets_laptop" 
																										ShowInCustomizationForm="True" Visible="False" VisibleIndex="14" Width="50px">
																									</dx:GridViewDataCheckColumn>
																									<dx:GridViewDataTextColumn Caption="Reports To" FieldName="reports_to" 
																										ShowInCustomizationForm="True" VisibleIndex="15" Width="120px">
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataCheckColumn Caption="Comp?" FieldName="has_comp" 
																										ShowInCustomizationForm="True" VisibleIndex="16" Width="50px">
																									</dx:GridViewDataCheckColumn>
																									<dx:GridViewDataMemoColumn Caption="Comp Details" FieldName="comp_details" 
																										ShowInCustomizationForm="True" Visible="False" VisibleIndex="17" Width="60px">
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataMemoColumn>
																									<dx:GridViewDataMemoColumn Caption="Notes" FieldName="notes" 
																										ShowInCustomizationForm="True" VisibleIndex="18" Width="40px">
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataMemoColumn>
																									<dx:GridViewDataCheckColumn Caption="Salary" FieldName="is_salary" 
																										ShowInCustomizationForm="True" Visible="False" VisibleIndex="19" Width="50px">
																									</dx:GridViewDataCheckColumn>
																									<dx:GridViewDataCheckColumn Caption="Signed" FieldName="is_signed" 
																										ShowInCustomizationForm="True" VisibleIndex="20" Width="60px">
																									</dx:GridViewDataCheckColumn>
																									<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
																										ShowInCustomizationForm="True" VisibleIndex="24" Width="100px">
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataTextColumn Caption="ID" FieldName="id" 
																										ShowInCustomizationForm="True" Visible="False" VisibleIndex="23">
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataTextColumn Caption="memberid" FieldName="memberid" 
																										ShowInCustomizationForm="True" Visible="False" VisibleIndex="22">
																									</dx:GridViewDataTextColumn>
																								</Columns>
																								<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
																								<SettingsPager PageSize="20">
																								</SettingsPager>
																								<SettingsEditing Mode="PopupEditForm" />
																								<Settings ShowFilterRow="True" ShowTitlePanel="True" />
																								<SettingsText PopupEditFormCaption="Offer Details" />
																								<SettingsPopup>
																									<EditForm HorizontalAlign="WindowCenter" Modal="True" 
																										VerticalAlign="WindowCenter" Width="1050px" Height="1200px" />
																								</SettingsPopup>
																								<StylesPopup>
																									<EditForm>
																										<PopupControl>
																											<paddings padding="10px" />
																										</PopupControl>
																										<Content>
																											<Paddings Padding="20px" />
																										</Content>


																									</EditForm>
																								</StylesPopup>
																								<Templates>
																									<TitlePanel>
																										<table style="width: 100%;">
																											<tr>
																												<td align="left" style="text-align: left">
																													<dx:ASPxButton ID="btnAddNew" runat="server" AutoPostBack="False" 
																														ClientInstanceName="btnAddNew" HorizontalAlign="Center" Text="Add">
																														<ClientSideEvents Click="function(s, e) {
	gv_offers.AddNewRow();
}" />
																													</dx:ASPxButton>
																												</td>
																												<td>
																													&nbsp;</td>
																												<td>
																													&nbsp;</td>
																											</tr>
																										</table>
																									</TitlePanel>
																									<EditForm>
																										<iframe ID="IFrame_Offer" runat="server" frameborder="0" height="1200" 
																											name="IFrame_Offer" width="1050"></iframe>
																									</EditForm>
																								</Templates>
																							</dx:ASPxGridView>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
																								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT
a.id,
a.date,
a.memberid,
a.enteredby,
a.business_unit_id,
a.wage,
a.startdate,
a.enddate,
a.vacation_interval_1,
a.vacation_interval_2,
a.vacation_interval_3,
a.vacation_amount_1,
a.vacation_amount_2,
a.vacation_amount_3,
a.gets_vehicle,
a.gets_phone,
a.gets_laptop,
a.has_comp,
a.comp_details,
a.notes,
a.is_salary,
a.is_signed,
a.`status`,
c.member_fullname reports_to,
b.membertype_name membertypeid
FROM
member_offers a
INNER JOIN membertype AS b ON a.membertypeid = b.membertype_id
LEFT JOIN
	member c ON a.reports_to = c.member_id
LEFT JOIN
	member d ON a.memberid = d.member_id
WHERE
a.memberid = ?memberid AND 
a.isapplicant = 0 ORDER BY a.date DESC
">
																								<SelectParameters>
																									<asp:ControlParameter ControlID="hdnmember" Name="memberid" 
																										PropertyName="Value" />
																								</SelectParameters>
																							</asp:SqlDataSource>
																							<asp:SqlDataSource ID="sqlmember" runat="server" 
																								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																								SelectCommand="Select member_id,member_fullname name from member order by member_fullname">
																							</asp:SqlDataSource>
																							<asp:SqlDataSource ID="sqlcompany" runat="server" 
																								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																								SelectCommand="Select id, name from business_unit">
																							</asp:SqlDataSource>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<input id="Hidden1" type="hidden" />
																						</td>
																					</tr>
																				</table>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage ClientEnabled="False" Name="9" Text="Reviews">
																		<ContentCollection>
																			<dx:ContentControl ID="ContentControl11" runat="server" SupportsDisabledAttribute="True">
																				<table class="style2">
																					<tr>
																						<td colspan="3">
																							<dx:ASPxGridView ID="gv_review" runat="server" AutoGenerateColumns="False" 
																								ClientInstanceName="gv_review" DataSourceID="sql_reviewhistory" 
																								KeyFieldName="id" Width="100%" OnHtmlDataCellPrepared="gv_review_HtmlDataCellPrepared" OnCancelRowEditing="gv_review_CancelRowEditing" 
																								OnCommandButtonInitialize="gv_review_CommandButtonInitialize" 
																								OnHtmlEditFormCreated="gv_review_HtmlEditFormCreated" 
																								OnRowDeleting="gv_review_RowDeleting" 
																								OnStartRowEditing="gv_review_StartRowEditing" Theme="NETheme01" >
																								<clientsideevents selectionchanged="function(s, e) {
	boing('/sections/hr/member/review_list_for_member.aspx?id=' + s.GetRowKey(s.GetFocusedRowIndex()) + '&amp;memberid=' + document.getElementById('ctl00_cphMasterBody_hdnmember').value,'review',1100,900);

}" CustomButtonClick="function(s, e) {
	if(e.buttonID == 'print_review'){
                    var rowVisibleIndex = e.visibleIndex;
                    var rowKeyValue;
					s.GetRowValues( e.visibleIndex,'id',print_review);
				
		}
	else if (e.buttonID == 'edit'){ 
  					 var index = e.visibleIndex;
    				s.GetRowValues( e.visibleIndex,'id;member_id',open_review);

}
}" />
																								<SettingsCommandButton>
																									<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																									<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																									<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																								</SettingsCommandButton>
																								<Columns>
																									<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
																										ShowInCustomizationForm="True" VisibleIndex="0" Width="60px" ShowEditButton="true" ShowDeleteButton="true">
																										
																										
																										<CustomButtons>
																										<dx:GridViewCommandColumnCustomButton ID="edit" Text="Edit">
						<Image Url="~/images/icon/icon[edit].gif">
						</Image>
					</dx:GridViewCommandColumnCustomButton>
																											<dx:GridViewCommandColumnCustomButton ID="print_review">
																												<Image Height="15px" Url="~/images/icon/icon[print].gif" Width="15px">
																												</Image>
																											</dx:GridViewCommandColumnCustomButton>
																										</CustomButtons>
																										<CellStyle Wrap="False" HorizontalAlign="Right">
																										</CellStyle>
																									</dx:GridViewCommandColumn>
																									<dx:GridViewDataDateColumn Caption="Date" FieldName="date" 
																										ShowInCustomizationForm="True" SortIndex="0" SortOrder="Descending" 
																										VisibleIndex="2" Width="125px">
																										<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd"></PropertiesDateEdit>
																										<EditFormSettings Visible="False" />
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataDateColumn>
																									<dx:GridViewDataTextColumn Caption="Reviewed By" FieldName="reviewedby" 
																										ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
																										<EditFormSettings Visible="False" />
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
																										ShowInCustomizationForm="True" VisibleIndex="1">
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataTextColumn Caption="Average" ShowInCustomizationForm="True" 
																										VisibleIndex="4" Width="125px">
																										<EditFormSettings Visible="False" />
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataCheckColumn Caption="Locked" FieldName="locked" 
																										ShowInCustomizationForm="True" VisibleIndex="5" Width="30px">
																										<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
																											ValueUnchecked="0"></PropertiesCheckEdit>
																									</dx:GridViewDataCheckColumn>
																									<dx:GridViewDataTextColumn Caption="Offer" FieldName="offerid" 
																										ShowInCustomizationForm="True" VisibleIndex="7">
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
																										ShowInCustomizationForm="True" VisibleIndex="6">
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataCheckColumn Caption="was_printed" FieldName="was_printed" 
																										ShowInCustomizationForm="True" VisibleIndex="8" Width="30px" Visible="False">
																									</dx:GridViewDataCheckColumn>
																									<dx:GridViewDataCheckColumn Caption="member" FieldName="member_id" 
																										ShowInCustomizationForm="True" VisibleIndex="9" Width="30px" Visible="False">
																									</dx:GridViewDataCheckColumn>
																								</Columns>
																								<SettingsBehavior 
																									EnableRowHotTrack="True" ConfirmDelete="True" />
																								<SettingsPager Mode="ShowAllRecords" Visible="False">
																								</SettingsPager>
																								<SettingsEditing Mode="PopupEditForm" />
																								<Settings ShowTitlePanel="True" VerticalScrollableHeight="1000" />
																								<SettingsText PopupEditFormCaption="Review" />
																								<SettingsPopup>
																									<EditForm Height="950px" HorizontalAlign="WindowCenter" VerticalAlign="Above" 
																										Width="1050px" />
																								</SettingsPopup>
																								<StylesPopup>
																									<EditForm>
																										<Content>
																											<Paddings Padding="10px" />
																										</Content>
																										<Header BackColor="#009999">
																										</Header>
																									</EditForm>
																								</StylesPopup>
																								<Templates>
																									<TitlePanel>
																										<table class="dxflInternalEditorTable">
																											<tr>
																												<td class="dxtcLeftAlignCell">
																													<dx:ASPxButton ID="btnAddReview" runat="server" AutoPostBack="False" Text="Add">
																														<ClientSideEvents Click="function(s, e) {
	//	boing('/sections/hr/member/review_list_for_member.aspx?id=0&amp;memberid=' + document.getElementById('ctl00_cphMasterBody_hdnmember').value,'review',1100,900);
gv_review.AddNewRow();
}" />
	
																													</dx:ASPxButton>
																												</td>
																												<td width="100%">
																													&nbsp;</td>
																												<td>
																													&nbsp;</td>
																											</tr>
																										</table>
																									</TitlePanel>
																									<EditForm>
																										<table style="width:100%;">
																											<tr>
																												<td>
																													<iframe ID="IFrame_review" runat="server" frameborder="0" height="800px" 
																														name="IFrame_review" width="100%"></iframe>
																												</td>
																											</tr>
																										</table>
																									</EditForm>
																								</Templates>
																							</dx:ASPxGridView>
																							<asp:SqlDataSource ID="sql_reviewhistory" runat="server" 
																								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT
	a.id,
	a.date,
	a.member_id,
	a.locked,
	b.member_fullname as reviewedby,
	a.offerid,
	a.status,
	a.was_printed
FROM
	emp_review a 
LEFT JOIN
	member b ON a.reviewed_by_id = b.member_id 
WHERE 
	a.member_id = ?id">
																								<SelectParameters>
																									<asp:ControlParameter ControlID="hdnmember" Name="id" PropertyName="Value" />
																								</SelectParameters>
																							</asp:SqlDataSource>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							&nbsp;</td>
																						<td>
																							&nbsp;</td>
																						<td>
																							&nbsp;</td>
																					</tr>
																				</table>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage ClientEnabled="False" Name="10" Text="Footprints">
																		<ContentCollection>
																			<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																			<div align="left">
																				<uc1:uc_member_usage ID="uc_member_usage1" runat="server" /></div>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																</TabPages>
																<ActiveTabStyle>
																	
																</ActiveTabStyle>
																<TabStyle Height="25px">
																	
																</TabStyle>
																<ContentStyle>
																	<Paddings Padding="5px" />
																</ContentStyle>
																<ClientSideEvents ActiveTabChanging="pc_tabchange" />
															</dx:ASPxPageControl>
															<br />
															<dx:ASPxPopupControl ID="pop_term" runat="server" ClientInstanceName="pop_term" 
																HeaderText="Terminate Employee" Height="150px" 
																PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
																Theme="NETheme01" Width="600px" CloseAction="CloseButton" Modal="True">
																<HeaderStyle HorizontalAlign="Left" />
																<ModalBackgroundStyle Opacity="0">
																</ModalBackgroundStyle>
																<ContentCollection>
<dx:PopupControlContentControl runat="server">
	<table cellpadding="5" style="width:100%;">
		<tr>
			<td align="left" colspan="3">
				<dx:ASPxTextBox ID="txt_term_act" runat="server" ClientInstanceName="txt_term_act"
					Caption="Termination Reason (Actual):" Theme="NETheme01" Width="100%">
					<CaptionCellStyle Width="180px">
					</CaptionCellStyle>
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td align="left" colspan="3">
				<dx:ASPxTextBox ID="txt_term_roe" runat="server" ClientInstanceName="txt_term_roe"
					Caption="Termination Reason (ROE):" Theme="NETheme01" Width="100%">
					<CaptionCellStyle Width="180px">
					</CaptionCellStyle>
				</dx:ASPxTextBox>
			</td>
		</tr>
	    <tr>
	        <td align="left" colspan="3">
	            <dx:ASPxDateEdit ID="dte_last_day_worked" runat="server" ClientInstanceName="dte_last_day_worked"
	                            Caption="Last Day Worked:" Theme="NETheme01" Width="100%" DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd">
	                <CaptionCellStyle Width="180px">
	                </CaptionCellStyle>
	            </dx:ASPxDateEdit>
	        </td>
	    </tr>
		<tr>
			<td>
				&nbsp;</td>
			<td>
				&nbsp;</td>
			<td align="right">
				<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" 
					Text="Next -&gt;" Theme="NETheme01">
					<ClientSideEvents Click="function(s, e) {
if ((txt_term_roe.GetText()!='') && (txt_term_act.GetText()!='') && (dte_last_day_worked.GetText()!=''))
{
	pop_term.Hide();
	deactivate_user();
}
else
{
alert('You must enter the Last Day Worked, the ROE and Actual reasons for the termination',1000);
}
}" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
																	</dx:PopupControlContentControl>
</ContentCollection>
															</dx:ASPxPopupControl>
														</td>
													</tr>
												</table>
						
	




</asp:Content>

