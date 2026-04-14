<%@ Control Language="C#" AutoEventWireup="true" Inherits="mobile_modules_timesheet" Codebehind="timesheet.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1' name='viewport' />
<link type="text/css" rel="stylesheet" href="/mobile/css/timesheet.css"/>
<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
<script type="text/javascript">
function bind_datepicker()
	{
	var days = -1;
	if (typeof daysDatePick !== 'undefined' && daysDatePick != null)
	    days = daysDatePick;

	$(".date").datepicker({
		minDate: days,
		maxDate: 1,
		dateFormat: "yy-mm-dd"
		});
	}
function EndReqHandler(sender, args)
	{
	if (args != undefined && args.get_error() != undefined)
		{
		var err				= args.get_error().toString().replace(/Sys.+\Exception:/g, "").trim();
		alert(err);
		}
	bind_datepicker();
}

function OnTextChanged(s, e) {
	s.Upload();
}
function OnFileUploadComplete(s, e) {
	alert(e.callbackData);
}
</script>
<asp:UpdatePanel ID="up" runat="server">
	<ContentTemplate>
		<div class="field">
			<div class="name">Today's Date:</div>
			<div class="value text"><label id="current_date" runat="server" /></div>
		</div>
		<div class="field">
			<div class="name">Selected Date:</div>
			<div class="value"><asp:TextBox ID="date" runat="server" CssClass="date" onmousedown="bind_datepicker();" ontextchanged="date_TextChanged" AutoPostBack="True"></asp:TextBox></div>
		</div>
		<table cellspacing="0" cellpadding="2" class="date_chooser">
			<tr>
				<td class="c" colspan="2">
					<asp:Button ID="btn_toggle_entries" runat="server" CssClass="whitetext aligncenter" onclick="btn_toggle_entries_Click" Text="Show Time Entries" />
					<asp:Panel ID="pnl_entries" runat="server" CssClass="day_entries" Width="100%">
					<table id='todays_entries' runat="server" width="100%" cellspacing="0" cellpadding="2">
						
							<tr>
								<th class='cu'>Customer</th>
								<th class='wo'>#</th>
								<th class='hr'>Hrs</th>
								<th class='ac'>Action</th>
							</tr>
						
					</table>
					</asp:Panel>
				</td>
			</tr>
		</table>
		<div class="error aligncenter" runat="server" id="error_div"></div>
		<div class="warning aligncenter">Time entries have to be made within 24 hours of the time worked. If you missed it, please talk to your branch manager. </div>
		<asp:dropdownlist id="ddl_tabs" runat="server" autopostback="true" cssclass="whitetext alignleft" onselectedindexchanged="ddl_tabs_SelectedIndexChanged">
			<asp:listitem value="0" text="Work Order"/>
			<asp:listitem value="1" text="Quote"/>
			<asp:listitem value="2" text="Business Dev"/>
			<asp:listitem value="3" text="Shop time"/>
		</asp:dropdownlist>
		<asp:multiview id="mv_tabs" runat="server">
			<asp:view id="tab_wo" runat="server">
				<div class="field" id="tr_business_unit" runat="server">
					<div class="name">Business Unit:</div>
					<div class="value">
						<asp:DropDownList ID="wo_ddl_business_unit" DataTextField="name" DataValueField="id" CssClass="ddl" runat="server" AutoPostBack="True" OnSelectedIndexChanged="wo_ddl_business_unit_OnSelectedIndexChanged">
							</asp:DropDownList>
					</div>
				</div>
				<div class="field" id="tr_customer" runat="server">
					<div class="name">Customer:</div>
				    <div class="value">
				        <asp:TextBox ID="wo_txt_customer" CssClass="textbox" runat="server" >
				        </asp:TextBox>
				    </div>
					<div class="value">
						<asp:DropDownList ID="wo_ddl_customer" CssClass="ddl" runat="server" DataTextField="name" DataValueField="id" AutoPostBack="True" OnSelectedIndexChanged="wo_ddl_customer_SelectedIndexChanged">
							</asp:DropDownList>
					</div>
				</div>
				<div class="field" id="tr_wo" runat="server">
					<div class="name">WO #:</div>
				    <div class="value">
				        <asp:TextBox ID="wo_txt_workorder" CssClass="textbox" runat="server" >
				        </asp:TextBox>
				    </div>
					<div class="value">
						<asp:DropDownList ID="wo_ddl_workorder" CssClass="ddl" runat="server" OnSelectedIndexChanged="wo_ddl_workorder_SelectedIndexChanged" AutoPostBack="true">
							</asp:DropDownList>
					</div>
				</div>
				<div class="field" id="tr_scan" runat="server">
					<div class="name">Scan WO:</div>
					<div class="value">
						<asp:TextBox ID="wo_tb_workorder" placeholder="Scan WO barcode"  runat="server" AutoPostBack="True" style="text-align:center;background-color:#f00;" OnTextChanged="wo_tb_workorder_TextChanged"></asp:TextBox>
					</div>
				</div>
				<div class="field">
					<div class="name">% Done:</div>
					<div class="value">
						<asp:DropDownList ID="wo_ddl_percent_complete" runat="server" CssClass="ddl">
								<asp:ListItem Value="0">0%</asp:ListItem>
								<asp:ListItem Value="10">10%</asp:ListItem>
								<asp:ListItem Value="25">25%</asp:ListItem>
								<asp:ListItem Value="50">50%</asp:ListItem>
								<asp:ListItem Value="75">75%</asp:ListItem>
								<asp:ListItem Value="90">90%</asp:ListItem>
								<asp:ListItem Value="100">100%</asp:ListItem>
							</asp:DropDownList>
					</div>
				</div>
				<div class="field">
					<div class="name">Hours:</div>
					<div class="value">
						<asp:TextBox ID="wo_tb_hours" placeholder="Hours Worked" CssClass="textbox" type="number" min="0" max="24"  step="any" runat="server"/>
					</div>
				</div>
				<div class="field">
					<div class="name">Hour Type:</div>
					<div class="value">
						<asp:DropDownList ID="wo_ddl_hourtype" CssClass="ddl" runat="server">
							<asp:ListItem Value="1">Regular</asp:ListItem>
							<asp:ListItem Value="2">Overtime</asp:ListItem>
							<asp:ListItem Value="3">Double Time</asp:ListItem>
							<asp:ListItem Value="4">Regular Shift Premium</asp:ListItem>
							<asp:ListItem Value="5">Overtime Shift Premium</asp:ListItem>
							<asp:ListItem Value="6">Double Time Shift Premium</asp:ListItem>
						</asp:DropDownList>
					</div>
				</div>
				<div class="field">
					<div class="name">Comment:</div>
					<div class="value"><asp:TextBox ID="wo_tb_comment" CssClass="textarea" runat="server" placeholder="Please enter a comment describing the work done" TextMode="MultiLine"></asp:TextBox></div>
				</div>
				<div class="field">
					<div class="name">How was your day?</div>
					<div class="value"><dx:ASPxRatingControl ID="rc_rating" runat="server" Value="0"/></div>
				</div>
				<br />
				<br />
				<asp:Button ID="wo_b_cancel" CssClass="whitetext aligncenter" Text="CANCEL" runat="server" Visible="False" OnClick="b_cancel_Click"/>
				<asp:Button ID="wo_b_submit" runat="server" CssClass="whitetext aligncenter" Text="SAVE" OnClick="b_submit_Click" />
				<div id="row_signoff" runat="server" visible="false">
					<dx:ASPxButton ID="bt_signoff" runat="server" AutoPostBack="False" CssClass="button" Text="Cust. Signature" Width="100%"/>
				</div>
				<dx:aspxuploadcontrol id="ASPxUploadControl1" runat="server" clientinstancename="upload"
					onfileuploadcomplete="ASPxUploadControl1_FileUploadComplete" fileuploadmode="OnPageLoad"
					showclearfileselectionbutton="False" showprogresspanel="True" width="50px">
					<clientsideevents textchanged="OnTextChanged" fileuploadcomplete="OnFileUploadComplete" />
					<browsebutton text="">
						<image height="40px" url="~/images/icon/icon[photo].gif">
						</image>
					</browsebutton>
					<advancedmodesettings>
						<filelistitemstyle cssclass="pending dxucFileListItem">
						</filelistitemstyle>
					</advancedmodesettings>
					<textboxstyle backcolor="Transparent" wrap="True">
						<paddings padding="0px" />
						<border borderstyle="None" />
					</textboxstyle>
				</dx:aspxuploadcontrol>
			</asp:view>
			<asp:view id="tab_quote" runat="server">
				<div class="field">
					<div class="name">Customer:</div>
					<div class="value">
						<asp:dropdownlist id="quote_ddl_customer" runat="server" cssclass="ddl" onselectedindexchanged="quote_ddl_customer_SelectedIndexChanged" autopostback="True">
							</asp:dropdownlist>
					</div>
				    <div class="value">
				        <asp:TextBox id="quote_tb_customer" runat="server" cssclass="ddl"  >
				        </asp:TextBox>
				    </div>
				</div>
				<div class="field">
					<div class="name">Quote:</div>
					<div class="value">
						<asp:dropdownlist id="quote_ddl_quote" runat="server" cssclass="ddl" autopostback="True" onselectedindexchanged="quote_ddl_quote_SelectedIndexChanged">
							</asp:dropdownlist>
					</div>
				    <div class="value">
				        <asp:TextBox id="quote_tb_quote" runat="server" cssclass="ddl" >
				        </asp:TextBox>
				    </div>
				</div>
				<div class="field">
					<div class="name">% Done:</div>
					<div class="value">
						<asp:dropdownlist id="quote_ddl_percent_complete" runat="server" cssclass="ddl">
								<asp:listitem value="0">0%</asp:listitem>
								<asp:listitem value="10">10%</asp:listitem>
								<asp:listitem value="25">25%</asp:listitem>
								<asp:listitem value="50">50%</asp:listitem>
								<asp:listitem value="75">75%</asp:listitem>
								<asp:listitem value="90">90%</asp:listitem>
								<asp:listitem value="100">100%</asp:listitem>
							</asp:dropdownlist>
					</div>
				</div>
				<div class="field">
					<div class="name">Hours:</div>
					<div class="value"><asp:textbox id="quote_tb_hours" runat="server" placeholder="Hours Worked" cssclass="textbox" type="number" step="any"/></div>
				</div>
				<asp:button id="quote_b_cancel" runat="server" cssclass="whitetext aligncenter" text="Cancel" visible="False" onclick="b_cancel_Click" />
				<asp:button id="quote_b_submit" runat="server" cssclass="whitetext aligncenter" text="Save" onclick="b_submit_Click" />
			</asp:view>
			<asp:view id="tab_bizdev" runat="server">
				<div class="field">
					<div class="name">Business Unit:</div>
					<div class="value"><asp:DropDownList ID="bizdev_ddl_branch" runat="server" CssClass="ddl"/></div>
				</div>
				<div class="field">
					<div class="name">Hours:</div>
					<div class="value"><asp:TextBox ID="bizdev_tb_hours" runat="server" CssClass="textbox" type="number" step="any"/></div>
				</div>
				<div class="field">
					<div class="name">Call(s):</div>
					<div class="value"><asp:TextBox ID="bizdev_tb_calls" runat="server" CssClass="textbox" type="number"/></div>
				</div>
				<div class="field">
					<div class="name">Meeting(s):</div>
					<div class="value"><asp:TextBox ID="bizdev_tb_meetings" runat="server" CssClass="textbox" type="number"/></div>
				</div>
				<div class="field">
					<div class="name">Drop Off(s):</div>
					<div class="value"><asp:TextBox ID="bizdev_tb_dropoffs" runat="server" CssClass="textbox" type="number"/></div>
				</div>
				<div class="field">
					<div class="name">Email(s):</div>
					<div class="value"><asp:TextBox ID="bizdev_tb_emails" runat="server" CssClass="textbox" type="number"/></div>
				</div>
				<div class="field">
					<div class="name">VM(s):</div>
					<div class="value"><asp:TextBox ID="bizdev_tb_vms" runat="server" CssClass="textbox" type="number"/></div>
				</div>
				<div class="field">
					<div class="name">Quote Opp(s):</div>
					<div class="value"><asp:TextBox ID="bizdev_tb_quoteopps" runat="server" CssClass="textbox" type="number"/></div>
				</div>
				<asp:Button ID="bizdev_b_cancel" runat="server" CssClass="whitetext aligncenter" Text="Cancel" Visible="False" OnClick="b_cancel_Click" />
				<asp:Button ID="bizdev_b_submit" runat="server" CssClass="whitetext aligncenter" Text="Save" OnClick="b_submit_Click" />
			</asp:view>
			<asp:view id="tab_shop" runat="server">
				<div class="field">
					<div class="name">Shop Time Type:</div>
					<div class="value"><asp:dropdownlist id="shop_ddl_shoptype" runat="server" cssclass="ddl"/></div>
				</div>
				<div class="field">
					<div class="name">Hours:</div>
					<div class="value"><asp:textbox id="shop_tb_hours" runat="server" cssclass="textbox" type="number" step="any"/></div>
				</div>
				<div class="field">
					<div class="name">Comment:</div>
					<div class="value"><asp:textbox id="shop_tb_comment" runat="server" cssclass="textarea" placeholder="Please enter a comment describing the work done" textmode="MultiLine"/></div>
				</div>

				<asp:button id="shop_b_cancel" runat="server" cssclass="whitetext aligncenter" text="Cancel" visible="False" onclick="b_cancel_Click" />
				<asp:button id="shop_b_submit" runat="server" cssclass="whitetext aligncenter" text="Save" onclick="b_submit_Click" />
			</asp:view>
		</asp:multiview>
	</ContentTemplate>
</asp:UpdatePanel>
