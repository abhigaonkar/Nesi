<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_member_modules_wage" Codebehind="wage.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>	




<script>
		function check_date(obj)
		{
		var validformat	=/^\d{4}-\d{2}-\d{2}$/;
		var joined		= $("#<%=txtDateJoined.ClientID%>");
		var s			= joined.val().split("-");
		var next		= $("#<%=txtNextRaise.ClientID%>");
		if (!validformat.test(obj.value)) 
			{
			clear_dates(obj,"Invalid Date.\nFormat it like: YYYY-MM-DD")
			}
		else if(s[0] < <%= DateTime.Now.Year %>)
			{
		//	clear_dates(obj, "Invalid Year.\nIt needs to be equal to or greater than <%= DateTime.Now.Year %>")
			}
		else if(s[1] > 12)
			{
			clear_dates(obj, "Invalid Month.")
			}
		else if(s[2] > 31)
			{
			clear_dates(obj, "Invalid Day.");
			}
		else if($(obj).attr('id') == next.attr('id'))
			{
			next.val(((s[0]/1)+1)+"-"+s[1]+"-"+s[2]);
			}
		}
	function clear_dates(obj, msg)
		{
		alert(msg);
		var joined		= $("#<%=txtDateJoined.ClientID%>");
		var next		= $("#<%=txtNextRaise.ClientID%>");
		joined.val("");
		next.val("");
		obj.focus();
		}
	function check_datejoined(obj)
		{
		if(($("#<%=txtDateJoined.ClientID%>").val() != "" && $(obj).val() == "") || $(obj).attr('data-prev-val') != null)
			{
			var s		= $("#<%=txtDateJoined.ClientID%>").val().split("-");
			$(obj).val(((s[0]/1)+1)+"-"+s[1]+"-"+s[2]);
			$(obj).attr("data-prev-val", $(obj).val());
			}
		}
	$(document).ready(function()
						{
						$("#<%=txtDateJoined.ClientID%>").change(function()
																	{
																	check_date(this);
																	});
						$("#<%=txtNextRaise.ClientID%>").change(function()
																	{
																	check_date(this);
																	});
						$("#<%=txtNextRaise.ClientID%>").focus(function()
																	{
																	check_datejoined(this);
																	});
						});
		</script>
		<div style="margin:10px;">
<table cellpadding="0" cellspacing="0" border="0" width="100%" >
    <tr>
        <td>
            <asp:GridView ID="WageGrid" font="Arial" runat="server" 
				AllowPaging="True" AutoGenerateColumns="False" 
				CellPadding="4" DataKeyNames="MemberWage_ID" 
				OnSelectedIndexChanged="WageGrid_SelectedIndexChanged" width=100% 
				OnPageIndexChanging="WageGrid_PageIndexChanging" 
				OnRowDeleting="WageGrid_RowDeleting" EmptyDataText="No Wages added." 
				Font-Names="Arial" ForeColor="#333333" GridLines="None">
            <Columns>
                <asp:BoundField DataField="Date" DataFormatString="{0:yyyy-MM-dd}" HeaderText="Effective Date"
                    HtmlEncode="False" >
                <ItemStyle Wrap="False" />
				</asp:BoundField>
                <asp:BoundField DataField="CurrentWage" HeaderText="Current Wage" />
                <asp:BoundField DataField="NextRaise" DataFormatString="{0:yyyy-MM-dd}" HeaderText="Next Raise"
                    HtmlEncode="False" >
                <ItemStyle Wrap="False" />
				</asp:BoundField>
                <asp:BoundField DataField="Comment" HeaderText="Comments"/>
                <asp:BoundField DataField="user" HeaderText="Entered By" >
                <ItemStyle Wrap="False" />
				</asp:BoundField>
                <asp:BoundField DataField="bonus_name" HeaderText="Bonus Type" />
				<asp:BoundField DataField="bonus_amount" HeaderText="Bonus Amt" />
                <asp:CommandField ButtonType="Button" SelectText="Edit" ShowSelectButton="True" 
					CancelText="" EditText="" InsertText="" NewText="" UpdateText="">
                <ControlStyle ForeColor="Black" Font-Bold="True" Font-Size="11px" BorderStyle="Solid" BorderWidth="0px" Height="26px" CssClass="button" />
                </asp:CommandField>
                <asp:BoundField DataField="memberwage_id" HeaderText="id"   />
           </Columns>                             
            	<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        		<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999"  />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            	<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
				<SortedAscendingCellStyle BackColor="#E9E7E2" />
				<SortedAscendingHeaderStyle BackColor="#506C8C" />
				<SortedDescendingCellStyle BackColor="#FFFDF8" />
				<SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
			<br />
			<asp:Button ID="btnInsertWage" runat="server" Text="Add New"  OnClick="btnInsertWage_Click" CausesValidation="False" Enabled="False" />
            
            <br /></td>
    </tr>
</table>
	<dx:ASPxPanel ID="pnlView" runat="server" Width="100%" Theme="NETheme01">
		<PanelCollection>
			<dx:PanelContent>
            <asp:CompareValidator ID="cmpDate" runat="server" ControlToCompare="txtDateJoined"
                ControlToValidate="txtNextRaise" Display="None" ErrorMessage="The Next Raise Date has to be greater than the Date Joined"
                Operator="GreaterThan" SetFocusOnError="True" ValidationGroup="vg_wage"></asp:CompareValidator>
	<table cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td style="width: 100px; ">
				ID</td>
			<td>
				<dx:ASPxTextBox ID="txt_id" runat="server" Theme="NETheme01" Width="170px">
                    <ValidationSettings ValidationGroup="vg_wage">
                        <RequiredField ErrorText="Current Wage is a required field" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
            <td style="width: 100px; ">Effective Date:</td>
            <td>
                <dx:ASPxDateEdit ID="txtDateJoined" runat="server" ClientInstanceName="txtdatejoined" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" Theme="NETheme01">
                    <ClientSideEvents DateChanged="function(s, e) {
	var d			= new Date(s.GetDate());
	d.setYear(d.getFullYear() + 1);
	txtnextraise.SetDate(d);
}" />
                    <ValidationSettings ValidationGroup="vg_wage">
                        <RequiredField ErrorText="Date Joined is a required field" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxDateEdit>
            </td>
        </tr>
		<tr>
			<td style="width: 100px; height: 27px;">
				<asp:Label ID="Label8" runat="server" Text="Next Raise"></asp:Label>:</td>
			<td style="height: 27px">
				<dx:ASPxDateEdit ID="txtNextRaise" runat="server" 
					DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd" ClientInstanceName="txtnextraise" 
					Theme="NETheme01">
					<ValidationSettings ValidationGroup="vg_wage">
						<RequiredField ErrorText="Next Raise is a required field" IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxDateEdit>
			</td>
		</tr>
		<tr>
			<td style="height: 47px">
				<asp:Label ID="txtComments" runat="server" Text="Comments"></asp:Label>:</td>
			<td style="height: 47px">
				<dx:ASPxMemo ID="txtComment" runat="server" Height="100px" Width="438px" 
					Theme="NETheme01">
					<ValidationSettings ValidationGroup="vg_wage">
						<RequiredField ErrorText="Please supply a comment" IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxMemo>
			</td>
		</tr>
		<tr>
			<td style="width: 100px; height: 26px">
				Current Wage:</td>
			<td>
				<dx:ASPxTextBox ID="txtCurrentWage" runat="server" Width="170px" 
					Theme="NETheme01">
					<ValidationSettings ValidationGroup="vg_wage">
						<RequiredField ErrorText="Current Wage is a required field" IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td style="width: 100px; height: 26px">
				Bonus Type:</td>
			<td>
				<dx:ASPxComboBox ID="ddlbonus" runat="server" TextField="bonus_type" 
					ValueField="id" ValueType="System.Int32" Theme="NETheme01">
					<ClientSideEvents SelectedIndexChanged="function(s, e) {
	cb_bonus.PerformCallback(s.GetValue());
}" />
				</dx:ASPxComboBox>
			</td>
		</tr>
		<tr>
			<td style="width: 100px; height: 26px">
				Bonus Amount:</td>
			<td>
				<dx:ASPxCallbackPanel ID="cb_bonus" runat="server" 
					ClientInstanceName="cb_bonus" OnCallback="cb_bonus_Callback" Width="200px">
					<PanelCollection>
						<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
							<dx:ASPxSpinEdit ID="spn_bonusamt" runat="server" ClientEnabled="False" 
								DecimalPlaces="3" DisplayFormatString="P1" Height="21px" Increment="0.005" 
								Number="0" Theme="NETheme01">
							</dx:ASPxSpinEdit>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxCallbackPanel>
			</td>
		</tr>
		<tr>
			<td style="width: 100px; height: 26px">
			</td>
			<td>
				<table>
					<tr>
						<td>
							<dx:ASPxButton ID="bt_wage_update" runat="server" 
								OnClick="bt_wage_update_Click" Text="Update" ValidationGroup="vg_wage" 
								Width="75px" Theme="NETheme01">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="bt_wage_add" runat="server" OnClick="bt_wage_add_Click" 
								Text="Add" ValidationGroup="vg_wage" Width="75px" Theme="NETheme01">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="bt_wage_cancel" runat="server" CausesValidation="False" 
								OnClick="bt_wage_cancel_Click" Text="Cancel" Width="75px" Theme="NETheme01">
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>

				
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxPanel></div>
<div style=" margin:10px; font-family: Segoe UI, Calibri, Arial; font-size: 10pt;">
<div >
<table cellpadding="2" cellspacing="0" width="100%">
<tr>
<td width="50%" bgcolor="Silver" style="font-size: x-large">Vacation</td>
<td width="50%" align="right" bgcolor="Silver" 
		style="font-size: x-large; width: 0%;">
	&nbsp;</td>
<td width="50%" align="right" bgcolor="Silver" 
		style="font-size: x-large; width: 25%;">
	<dx:ASPxButton ID="bt_savepanel" runat="server" Text="Save Panel" 
		CausesValidation="False" onclick="bt_savepanel_Click" Theme="NETheme01" 
		Enabled="False">
		<ClientSideEvents Click="function(s, e) {
	please_wait(&quot;start&quot;);
}" />
		<Image Url="~/images/icon/icon[save].gif">
		</Image>
	</dx:ASPxButton>
</td>
</tr>
</table>
</div>
<table cellspacing="0" cellpadding="5">
	<tr>
		<td width="400">
<dx:ASPxPanel ID="pnl_vacation" runat="server" Width="375px">
	<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<div style="font-size:11px;font-weight:bold;">Vacation Amounts</div>
	<div style="font-size:11px;">USA - Hours<br />Canada - Percentage</div>
	<table border="0" cellpadding="5" cellspacing="0" class="vacation">
		<tr>
			<td align="center" class="c">
				After <asp:TextBox ID="vacation_interval_1" runat="server" CssClass="months" 
					onkeydown="only_numeric(event)" Enabled="False"></asp:TextBox> Months
			</td>
			<td align="left" class="v">
				<asp:TextBox ID="vacation_amount_1" runat="server" Enabled="False" CssClass="amount" onkeydown="only_numeric(event)"></asp:TextBox>	Hours / %
			</td>
		</tr>
		<tr>
			<td align="center" class="c">
				After <asp:TextBox ID="vacation_interval_2" runat="server"  Enabled="False" CssClass="months" onkeydown="only_numeric(event)"></asp:TextBox> Months
			</td>
			<td align="left" class="v">
				<asp:TextBox ID="vacation_amount_2" runat="server" Enabled="False" CssClass="amount" onkeydown="only_numeric(event)"></asp:TextBox> Hours / %
			</td>
		</tr>
		<tr>
			<td align="center" class="c">
				After <asp:TextBox ID="vacation_interval_3" runat="server" Enabled="False" CssClass="months" onkeydown="only_numeric(event)"></asp:TextBox> Months
			</td>
			<td align="left" class="v">
				<asp:TextBox ID="vacation_amount_3" runat="server" Enabled="False" CssClass="amount" onkeydown="only_numeric(event)"></asp:TextBox>	Hours / %
			</td>
		</tr>
	</table>
		</dx:PanelContent>
</PanelCollection>
</dx:ASPxPanel>
		</td>
		<td valign="top">
			<table style="width:100%;">
				<tr>
					<td class="c" width="150">
						<b>Days Till Stat:</b></td>
					<td>
						<dx:ASPxTextBox ID="tb_tillstat" runat="server" Width="170px" 
							onkeydown="only_numeric(event)" Theme="NETheme01">
						</dx:ASPxTextBox>
					</td>
				</tr>
				<tr>
					<td class="c">
						<b>Doesn&#39;t Receive Stat Pay:</b></td>
					<td>
						<dx:ASPxCheckBox ID="chk_nostat" runat="server">
						</dx:ASPxCheckBox>
					</td>
				</tr>
				<tr>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td width="500">
			&nbsp;</td>
		<td width="50%">
			&nbsp;</td>
	</tr>
</table>
</div>