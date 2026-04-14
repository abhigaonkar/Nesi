<%@ Control Language="C#" AutoEventWireup="true" Inherits="mobile_modules_schedule" EnableTheming="True" Codebehind="schedule.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>






<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<head>
<meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1' name='viewport' />
</head>

<style type="text/css">
	.style2
	{
		font-family: Arial, Helvetica, sans-serif;
		text-align: center;
		font-size: large;
	}
	.style3
	{
		height: 17px;
	}
	.style4
	{
		font-family: Arial, Helvetica, sans-serif;
	}
	.style5
	{
		text-align: center;
		font-family: Arial;
		color: #808080;
	}
</style>

<script type="text/javascript">
	function OnTextChanged(s, e) {
		s.Upload();
	}
	function OnFileUploadComplete(s, e) {
		alert(e.callbackData);
	}
	function refresh_header() {
		load_current_week_entries();
	}
	function send_alert(_text) {
		alert(_text);
	}
	function add_alert(_text) {
		var div = document.getElementsByClassName('drill_cursor')[0];

		div.addEventListener('click', function (event) {
			alert(_text);
		});
	}

    function finish_time(pass) {

        if (pass == 1) {

            var selectedJobType = 0;
            if (typeof (ddl_jobtypes) != "undefined" && ddl_jobtypes) {
                selectedJobType = ddl_jobtypes.GetValue();
            }

			if (confirm('You are about to add ' + tb_hours.GetText() + ' hours, is this correct?')) {
				dv.PerformCallback('ts|' + lblid.GetText() + '|' + mem_ts.GetText() + '|' + ddl_pt.GetValue() + '|' + tb_hours.GetText() + '|' + selectedJobType + '|' + ddl_scope.GetValue());

            }
        }

    }

	

//position:absolute;top:40%;left:20%; height:100px; width 200px;
	function tempAlert(msg) {
		var el = document.createElement("div99");
		var el2 = document.createElement("div100");
		el.setAttribute("style", "padding: 20px; width:200px; height:100px; position: absolute; top: 50%; left: 50%; margin-top: -100px; margin-left: -100px; color: white; background-color:black;opacity:.60;");
		el2.setAttribute("style", "display:table-cell;vertical-align:middle;text-align:center;font-family: Arial; font-weight: bold; font-size: large;");
		el2.innerHTML = msg;
		el.appendChild(el2); 
		setTimeout(function () {
			el.parentNode.removeChild(el);
		}, 1500);
		document.body.appendChild(el);
	}

	window.alert = function (msg) {
		tempAlert(msg);
	}
	
</script>

	
<table cellpadding="0" cellspacing="0">
	<tr>
		<td class="style2">
			<asp:ImageButton ID="ImageButton2" runat="server" OnClientClick="pop_oncall_cal.Show();return false;" ImageUrl="~/images/icon/icon[calendar].gif" UseSubmitBehavior="False" />
        </td>
		<td class="style2">
			<strong>Scheduler</strong></td>
		<td class="style2">
			<asp:ImageButton ID="ImageButton1" runat="server" OnClientClick="pop_oncall.Show();document.getElementById('spacerDiv').style.display='block';return false;" ImageUrl="~/images/icon/icon[member].gif" UseSubmitBehavior="False" />
        </td>
	</tr>
	<tr>
		<td class="style2" colspan="3">
			<dx:ASPxCallbackPanel ID="cb_header" runat="server" Width="100%" 
				ClientInstanceName="cb_header" oncallback="cb_header_Callback">
			
			
				<PanelCollection>
<dx:PanelContent runat="server">
	<table id="tbl_summary" style="width:100%; height: 20px; border-collapse: collapse;" runat="server">
				<tr>
					<td style="border: 2px solid #C0C0C0; font-family: Arial; font-size: 9px; width:10%;"><div id="Div0" runat="server"></div></td>
					<td style="border: 2px solid #C0C0C0; font-family: Arial; font-size: 9px; width:10%;">
						<div id="Div1" runat="server"></div></td>
					<td style="border: 2px solid #C0C0C0; font-family: Arial; font-size: 9px; width:10%;">
						<div id="Div2" runat="server"></div></td>
					<td style="border: 2px solid #C0C0C0; font-family: Arial; font-size: 9px; width:10%;">
						<div id="Div3" runat="server"></div></td>
					<td style="border: 2px solid #C0C0C0; font-family: Arial; font-size: 9px; width:10%;">
						<div id="Div4" runat="server"></div></td>
					<td style="border: 2px solid #C0C0C0; font-family: Arial; font-size: 9px; width:10%;">
						<div id="Div5" runat="server"></div></td>
					<td style="border: 2px solid #C0C0C0; font-family: Arial; font-size: 9px; width:10%;">
						<div id="Div6" runat="server"></div></td>
				</tr>
			</table>
					</dx:PanelContent>
</PanelCollection>
			
			
</dx:ASPxCallbackPanel>
		</td>
	</tr>
	<tr>
		<td align="center" valign="top" width="100%" colspan="3">
<dx:ASPxDataView ID="dv" runat="server" ClientInstanceName="dv" 
				DataSourceID="SqlDataSource1" Height="500px" Theme="NETheme01" 
				oncustomcallback="dv_CustomCallback" EmptyDataText="Nothing Scheduled" 
				onprerender="dv_PreRender" Width="98%">
	<ClientSideEvents Init="function(s, e) {
	
	if (s.cp_start_index !=null)
	{
	if ((s.cp_start_index!='')&&(s.cp_start_index!='0'))
{
s.GotoPage(s.cp_start_index);
}
}
}" EndCallback="function(s, e) {
	if ((s.cp_alert!=null)&&(s.cp_alert!=''))
{
send_alert(s.cp_alert);
s.cp_alert='';

}
if ((s.cp_disable_page!=null)&&(s.cp_disable_page!=''))
{
add_alert(s.cp_disable_page);
s.cp_disable_page='';
}
cb_header.PerformCallback();
}" />
	<ItemTemplate>
	<div id="_overlay" class="drill_cursor" >
		<table style="width:98%; border-collapse: collapse;" cellpadding="0" cellspacing="0" bgcolor="#E8E8E8">
			<tr id="header" bgcolor="#CCCCCC">
				<td style="text-align: center" colspan="2">
					<dx:ASPxDateEdit ID="dte" runat="server" AnimationType="None" 
						BackColor='<%# GetColor(Convert.ToInt32(Eval("Status")),Convert.ToInt32(Eval("quote_id"))) %>' ForeColor='<%# GetfColor(Convert.ToInt32(Eval("Status")),Convert.ToInt32(Eval("quote_id"))) %>' ClientEnabled="False" Date="09/01/2014 21:20:24" 
						DisplayFormatString='<%# GetTopDateFormat(Convert.ToInt32(Eval("oncall"))) %>' Font-Bold="True" Font-Names="Arial" 
						Font-Size="16pt" HorizontalAlign="Center" PopupHorizontalAlign="Center" 
						Value='<%# Eval("StartDate") %>' Width="100%" ClientInstanceName="dte">
						<Border BorderStyle="None" />
						
						<Border BorderStyle="None" />
						
						<Border BorderStyle="None" />
						
						<Border BorderStyle="None" />
						
						
						<Border BorderStyle="None" />
						
						<Border BorderStyle="None" />
						
						<Border BorderStyle="None" />
						
						<Border BorderStyle="None" />
						
					</dx:ASPxDateEdit>
				</td>
				
			</tr>
			<tr>
				<td style="text-align: center; width: 100%;" colspan="2" width="50%">
					<table style="width:100%;">
						<tr>
							<td width="50%">
								<dx1:ASPxDateEdit ID="ASPxDateEdit2" runat="server" AnimationType="None" 
									BackColor="Transparent" ClientEnabled="False" Date="09/01/2014 21:20:24" 
									DisplayFormatString="'Start'  HH:mm" Font-Bold="True" Font-Names="Arial" 
									Font-Size="12pt" ForeColor="#666666" HorizontalAlign="Center" 
									PopupHorizontalAlign="Center" Value='<%# Eval("StartDate") %>' Width="100%">
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									<DisabledStyle ForeColor="Black">
									</DisabledStyle>
								</dx1:ASPxDateEdit>
							</td>
							<td>
								<dx1:ASPxDateEdit ID="ASPxDateEdit3" runat="server" AnimationType="None" 
									BackColor="Transparent" ClientEnabled="False" Date="09/01/2014 21:20:24" 
									DisplayFormatString="'Finish'  HH:mm" Font-Bold="True" Font-Names="Arial" 
									Font-Size="12pt" ForeColor="#666666" HorizontalAlign="Center" 
									PopupHorizontalAlign="Center" Value='<%# Eval("EndDate") %>' Width="100%">
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									<Border BorderStyle="None" />
									<DisabledStyle ForeColor="Black">
									</DisabledStyle>
								</dx1:ASPxDateEdit>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td colspan="2" align="center">
					<dx1:ASPxLabel ID="lblwo" runat="server" Font-Bold="True" Font-Names="Arial" 
						Font-Size="10pt" ForeColor="#000099" Text='<%# Eval("bvwo") %>'>
					</dx1:ASPxLabel>
				</td>
			</tr>
			<tr>
				<td style="overflow: hidden;" colspan="2" align="center">
					<dx1:ASPxLabel ID="lblcustomer" runat="server" Font-Bold="True" 
						Font-Names="Arial" Font-Size="9pt" ForeColor="#000099" 
						Text='<%# Eval("Subject") %>'>
					</dx1:ASPxLabel>
				</td>
			</tr>
			<tr>
				<td style="text-align: left" colspan="2">
					&nbsp;</td>
			</tr>
			<tr>
				<td style="padding: 2px 0px 2px 0px" colspan="2">
				
					<a id="link_addr" href=<%#Eval("addr1") %> target="_blank" 
						style="font-family: Arial, Helvetica, sans-serif; text-decoration: none; font-size: small;"><%# Eval("addr") %></a>		
		
				</td>
			</tr>
           <tr>
				<td class="style3" colspan="2">
					<dx:ASPxLabel ID="lbllocation" runat="server" Text='<%# Eval("location_in_plant") %>' 
						Font-Names="Arial">
					</dx:ASPxLabel>
				</td>
			</tr>
			<tr>
				<td class="style3" colspan="2">
					<dx:ASPxLabel ID="lblcontact" runat="server" Text='<%# Eval("Contact_Name") %>' 
						Font-Names="Arial">
					</dx:ASPxLabel>
				</td>
			</tr>
            <tr>
				<td style="padding: 2px 0px 2px 0px" colspan="2">
					<a id="A1" href=<%#Eval("phone1") %> target="_blank" style="font-family: Arial, Helvetica, sans-serif; text-decoration: none; font-size: small;"><%# Eval("phone") %></a>	
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<dx:ASPxLabel ID="lbltruck" runat="server" Text='<%# Eval("truckno") %>' 
						Font-Names="Arial">
					</dx:ASPxLabel>
				</td>
			</tr>
			<tr>
				<td style="padding: 2px 0px 2px 0px" colspan="2">
					<a id="A2" href=<%#Eval("pmcell") %> target="_blank" style="font-family: Arial, Helvetica, sans-serif; text-decoration: none; font-size: small;"><%# Eval("pm") %></a>	
				</td>
			</tr>
			<tr>
				<td style="padding: 2px 0px 2px 0px" colspan="2">
					<dx1:ASPxLabel ID="lbl_meet_at_shop" runat="server" Font-Names="Arial" ClientVisible='<%# Getwovis(Convert.ToInt32(Eval("woprog_id"))) %>'
						Text='<%# Eval("meet") %>'>
					</dx1:ASPxLabel>
				</td>
			</tr>
			<tr>
				<td align="center" valign="middle">
					<dx:ASPxMemo ID="memnotes" runat="server" Height="71px" Width="90%" ClientVisible='<%# Getwovis(Convert.ToInt32(Eval("woprog_id"))) %>'
						Text='<%# Eval("notes") %>' Font-Names="Arial" NullText="No Special Instructions" 
						Font-Italic="True" ForeColor="Gray" ReadOnly="True">
						<Border BorderStyle="None" />
						<NullTextStyle Font-Italic="True">
						</NullTextStyle>
					</dx:ASPxMemo><br />
				</td>
				<td align="center" valign="middle">
					<dx1:ASPxLabel ID="lbl_woprog_id" runat="server" 
						ClientInstanceName="lbl_woprog_id" ClientVisible="False" 
						Text='<%# Eval("woprog_id") %>'>
					</dx1:ASPxLabel>
				</td>
			</tr>
			
			<tr>
				<td colspan="2">
					<dx:ASPxPanel ID="pnl" runat="server" ClientInstanceName="pnl" 
						
						
						BackColor='<%# GetColor(Convert.ToInt32(Eval("Status")),Convert.ToInt32(Eval("quote_id"))) %>' ForeColor='<%# GetfColor(Convert.ToInt32(Eval("Status")),Convert.ToInt32(Eval("quote_id"))) %>'
						ClientVisible='<%# Getvis(Convert.ToInt32(Eval("Status"))) %>' Width="100%" 
						Font-Names="Arial" Enabled= <%# Get_Page_Enable(Eval("wo_status"),Eval("wo_isonhold"),Eval("woprog_id")) %>>
						<PanelCollection>
							<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
								<table ID="ts_table" runat="server" style="border-collapse: collapse;" 
									width="100%">
									<tr>
				<td colspan="2">
					<dx:ASPxLabel ID="lblid" runat="server" ClientInstanceName="lblid" 
						ClientVisible="False" Font-Names="Arial" Text='<%# Eval("id") %>'>
					</dx:ASPxLabel>
				</td>
			</tr>
			<tr id="Tr1" runat="server">
										<td id="Td1" colspan="2" runat="server" align="center" class="style4" 
											style="border-collapse: collapse; " valign="middle">
											<span style="padding-top: 5px;font-size: 25px; font-family: Arial, Helvetica, sans-serif; font-weight: bold; text-align: center;">
											Timesheet Entry</span></td>
									</tr>
                                   <tr id="tr_jobtype_label" runat="server"  style="height:20px !important;" oninit="tr_jobtype_label_Init">
                                        <td  colspan="2" runat="server" align="left" class="style4">Job Type</td>
                                    </tr>
                                    <tr id="tr_jobtype_info" runat="server"  oninit="tr_jobtype_info_Init">
                                        <td colspan="2">
                                            <dx:ASPxComboBox ID="ASPxComboBox_jobTypes" runat="server" ClientInstanceName="ddl_jobtypes" OnInit="ASPxComboBox_jobTypes_Init"
                                                Font-Names="Arial" TextField="membertype_name"
                                                ValueField="membertype_id" Width="100%" ValueType="System.Int32"
                                                Height="40px">
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
									<tr id="tr_scope" runat="server">
                                        <td colspan="2">
                                            <dx:ASPxComboBox ID="ASPxComboBox_scope" runat="server" ClientInstanceName="ddl_scope" 
                                                Font-Names="Arial" TextField="task" NullText="Select scope of work"
                                                ValueField="id" Width="100%" ValueType="System.String" OnInit="ASPxComboBox_scope_Init"
                                                Height="40px">
                                            </dx:ASPxComboBox>
											
                                        </td>
			
                                    </tr>
									<tr runat="server">
										<td runat="server" align="center" class="style4" colspan="2"
											style="border-collapse: collapse; height:40px;" valign="middle">
											<asp:HiddenField ID="woid" runat="server" Value='<%# Eval("woprog_id") %>' />
											<asp:SqlDataSource ID="sql_pt" runat="server" 
												ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
												ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT PayTypeHours_ID, Description FROM paytypehours WHERE PayTypeHours_ID > ?jobtype">
                                                <SelectParameters>
													<asp:ControlParameter ControlID="hdn_hourtype" Name="jobtype" PropertyName="Value" />
												</SelectParameters>
											</asp:SqlDataSource>
											<asp:SqlDataSource ID="sql_scope" runat="server" 
												ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
												ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
												SelectCommand="Select id, task from woprog_tasks where woprog_id =?woid order by _order,id">
												<SelectParameters>
													<asp:ControlParameter ControlID="woid" Name="woid" PropertyName="Value" />
												</SelectParameters>
											</asp:SqlDataSource>
											<asp:HiddenField ID="quoteid" runat="server" Value='<%# Eval("quote_id") %>' />
											<asp:SqlDataSource ID="sql_descriptions" runat="server" 
												ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
												ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
												SelectCommand="SELECT distinct comments FROM wocomment WHERE (woprog_id !=0 and woprog_id =?woid) AND wocomment_member_id != 0 order by wocomment.wocomment_id desc">
												<SelectParameters>
													<asp:ControlParameter ControlID="woid" Name="woid" PropertyName="Value" />
												</SelectParameters>
											</asp:SqlDataSource>
											<dx:ASPxLabel ID="lblhrs" runat="server" ClientInstanceName="lblhrs" 
												Font-Names="Arial" Text='<%# Eval("hours") %>' Font-Italic="False" Font-Size="11pt" 
												style="font-weight: 700;" ForeColor='<%# GetfColor(Convert.ToInt32(Eval("Status")),Convert.ToInt32(Eval("quote_id"))) %>'  >
											</dx:ASPxLabel>
										</td>
									</tr>
									<tr id="Tr2" runat="server">
										<td id="Td2" runat="server" align="right" class="style4" 
											style="border-collapse: collapse; " valign="bottom" width="70%">
											<dx:ASPxComboBox ID="ddl_pt" runat="server" ClientInstanceName="ddl_pt" 
												DataSourceID="sql_pt" Font-Names="Arial" TextField="Description" 
												Value=<%# Convert.ToInt32(Eval("pt")) %> ValueField="PayTypeHours_ID" Width="93%" ValueType="System.Int32" 
												Height="40px">
											</dx:ASPxComboBox>
											
										</td>
										<td align="left" width="30%">
											<asp:TextBox ID="tb_hours1" runat="server" Font-Names="Arial" Font-Size="Large" 
												 Width="82%" Visible="False"></asp:TextBox>
											<dx:ASPxTextBox ID="tb_hours" runat="server" ClientInstanceName="tb_hours"
												Font-Names="Arial" NullText="hours" Text='<%# Eval("hours_left", "{0:0.##}") %>' 
												Width="83%" Font-Size="Large" Height="40px">
												<NullTextStyle Font-Italic="True">
												</NullTextStyle>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr runat="server">
										<td colspan="2" runat="server" align="center" 
											style="border-collapse: collapse">
											<dx:ASPxComboBox ID="cb_desc" runat="server" AnimationType="None" 
												ClientInstanceName="cb_desc" 
												DataSourceID="sql_descriptions" Font-Names="Arial" 
												Font-Size="9px" TextField="comments" Width="90%" NullText="Select Previous Work Description">
												<ClientSideEvents SelectedIndexChanged="function(s, e) {
	mem_ts.SetText(s.GetText());
}" />
												<ItemStyle Height="15px" Wrap="True" />
												<NullTextStyle Font-Italic="True">
												</NullTextStyle>
											</dx:ASPxComboBox>
										</td>
									</tr>
									<tr runat="server">
										<td runat="server" align="center" colspan="2" style="border-collapse: collapse">
											<dx:ASPxMemo ID="mem_ts" runat="server" ClientInstanceName="mem_ts" 
												Font-Names="Arial" Height="61px" NullText="Enter Work Description Here...." 
												Width="90%">
												<NullTextStyle Font-Italic="True" ForeColor="#999999">
												</NullTextStyle>
											</dx:ASPxMemo>
										</td>
									</tr>
									<tr runat="server">
										<td colspan="2" runat="server" align="center" class="style4" 
											style="border-collapse: collapse; " valign="middle">
											&nbsp;</td>
									</tr>
									<tr runat="server">
										<td runat="server" align="right" 
											style="border-collapse: collapse; padding-bottom: 10px;" width="70%">
											<dx:ASPxButton ID="btn_addtime" runat="server" AutoPostBack="False" 
												Height="40px" Text="Enter Time" Width="93%" Font-Names="Arial" Font-Size="14pt" CommandName='<%# Eval("id") %>'
												Theme="NETheme01" ClientInstanceName="btn_addtime" 
												ClientVisible='<%# Getsavevis(Convert.ToDateTime(Eval("StartDate"))) %>' >
												<ClientSideEvents Click="function(s, e) {
var pass =0;
var hours_ondate = lblhrs.GetText().split(':');

    if (mem_ts.GetText()=='')
	{
		alert('You must enter a something in the comments');
	}
	else
	{
		pass=1;
	}

 if (hours_ondate[1]&gt;8)
{
if (confirm('Are you sure, you have already entered 8 hours for today?'))
{
 pass=1;
}
else
{
 pass=0;
}

}

finish_time(pass);
}" />
												
																	
												
												
																	
											</dx:ASPxButton>
										</td>
										<td runat="server" align="center"  rowspan="2" valign="middle"
											style="border-collapse: collapse; padding-bottom: 10px;" width="30%">
											<dx:ASPxUploadControl ID="ASPxUploadControl1" runat="server" 
												ClientInstanceName="upload" FileUploadMode="OnPageLoad" 
												OnFileUploadComplete="ASPxUploadControl1_FileUploadComplete" 
												ShowClearFileSelectionButton="False" ShowProgressPanel="True" Width="30px" 
												ClientVisible='<%# Getphotovis(Convert.ToInt32(Eval("Status"))) %>' >
												<ClientSideEvents FileUploadComplete="OnFileUploadComplete" 
													TextChanged="function(s, e) {
	hdn_woprog_id.Set('aptid', lblid.GetText());
	OnTextChanged(s,e);
}" />
												<BrowseButton Text="">
													<Image Height="30px" Url="~/images/icon/icon[photo].gif">
													</Image>
												</BrowseButton>
												<AdvancedModeSettings>
													<FileListItemStyle CssClass="pending dxucFileListItem">
													</FileListItemStyle>
												</AdvancedModeSettings>
												<TextBoxStyle BackColor="Transparent" Wrap="True">
												<Paddings Padding="0px" />
												<Border BorderStyle="None" />
												</TextBoxStyle>
											</dx:ASPxUploadControl>
                                            <input type="hidden" id="hid_woprog_id" class="hid_woprog_id" value='<%# Eval("woprog_id") %>' runat="server" /></input>
										</td>
									</tr>
									<tr>
										<td align="right" width="70%">
											<dx:ASPxButton ID="bt_signoff" CssClass="button" runat="server" Font-Names="Arial" Font-Size="14pt" AutoPostBack="false" Text="Cust. Signature" Width="93%" ClientVisible='<%# Getwovis(Convert.ToInt32(Eval("woprog_id"))) %>'>
												<ClientSideEvents Click="function(s,e){window.open('/mobile/index.aspx?a=signoff&woprog_id='+$('.hid_woprog_id').val()+'&from=scheduler');}" />
											</dx:ASPxButton>
										</td>
									</tr>
									<tr runat="server">
										<td  runat="server" colspan="2" 
											style="border-collapse: collapse;  text-align: left;">
												<dx1:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Arial" ClientVisible='<%# Getwovis(Convert.ToInt32(Eval("woprog_id"))) %>'
						Text='You will be working with:'>
					</dx1:ASPxLabel></td>
									</tr>
									<tr runat="server" align="center">
										<td runat="server" align="center" colspan="2" 
											style="border-collapse: collapse; padding-bottom: 10px; ">
											<asp:SqlDataSource ID="sql_whoelse" runat="server" 
												ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
												ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select distinct member_fullname,member.member_id from member,appointments 
where  appointments.startdate = ?startdate
and appointments.woprog_id = ?woid
and appointments.quote_id = 0
and appointments.status = 0
and appointments.member_id = member.member_id and 
member.member_id != ?mid">
												<SelectParameters>
													<asp:ControlParameter ControlID="dte" Name="startdate" PropertyName="Value" />
													<asp:ControlParameter ControlID="woid" Name="woid" PropertyName="Value" />
													<asp:ControlParameter ControlID="hdn_mid" Name="mid" PropertyName="Value" />
												</SelectParameters>
											</asp:SqlDataSource>
											<dx:ASPxListBox ID="lb_whoelse" runat="server" ClientInstanceName="lb_whoelse" 
												EnableFocusedStyle="False" Font-Names="Arial" Rows="4" SelectionMode="Multiple" 
												Width="90%" BackColor="Transparent" DataSourceID="sql_whoelse" TextField="member_fullname" 
												ValueField="member_id" ValueType="System.Int32" ItemStyle-HorizontalAlign="Center" Border-BorderStyle="None" 
												Border-BorderColor="Transparent" Font-Size="1em" ForeColor="Black">
												<ItemStyle HorizontalAlign="Center" >
												<Border BorderStyle="None" />
												</ItemStyle>
												<ReadOnlyStyle>
													<Border BorderStyle="None" />
												</ReadOnlyStyle>
												<Border BorderStyle="None" />
											</dx:ASPxListBox>
										</td>
									</tr>
								</table>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxPanel>
				</td>
			</tr>
			<tr>
				<td class="style5" colspan="2">
					<strong>&lt; SWIPE &gt;</strong></td>
			</tr>
			</table>
			
	</ItemTemplate>
	<Paddings PaddingTop="0px" PaddingLeft="0px" PaddingRight="0px" />

<SettingsFlowLayout ItemsPerPage="1"></SettingsFlowLayout>

<SettingsTableLayout ColumnCount="1" RowsPerPage="1"></SettingsTableLayout>

	<PagerSettings Position="Bottom">
		
		<Summary Text="Sched {0} of {1}" />

<Summary Text="Sched {0} of {1}"></Summary>
	</PagerSettings>
	<ContentStyle BackColor="Transparent">
		<Paddings PaddingTop="0px" />
	</ContentStyle>
	<ItemStyle BackColor="Transparent" VerticalAlign="Top" Width="100%">
	<Paddings padding="0px" />
	<Border BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" />
	<Border BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"></Border>
	<BorderTop BorderStyle="None" BorderWidth="0px" />
	</ItemStyle>
</dx:ASPxDataView>
		</td>
	</tr>
	<tr>
		<td colspan="3">
			
			&nbsp;</td>
	</tr>
</table>
<div id="spacerDiv" style="height: 1200px; display: none;"></div>

<dx:ASPxPopupControl ID="pop_oncall" ClientInstanceName="pop_oncall"  runat="server"
    HeaderText="On Call - By Day" 
	Height="100%" PopupHorizontalAlign="WindowCenter" 
	PopupVerticalAlign="TopSides" Width="100%" style="min-width: 100%;" close
    AppearAfter="0"  Theme="NETheme01" CloseAction="CloseButton" onwindowcallback="pop_oncall_WindowCallback" >
    	<ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
		<dx1:aspxcalendar id="oncall_chooseday" runat="server" autopostback="false"><clientsideevents selectionchanged="function(s,e){pop_oncall.PerformCallback();}"/> </dx1:aspxcalendar>
	<div id="oncall_div" runat="server" style="font-family:Arial;"></div>
		</dx:PopupControlContentControl>
</ContentCollection>
    <ClientSideEvents CloseUp="function(s,e) { document.getElementById('spacerDiv').style.display='none';}" />
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="pop_oncall_cal" ClientInstanceName="pop_oncall_cal"  runat="server"
    HeaderText="On Call Schedule" 
	Height="100%" PopupHorizontalAlign="WindowCenter" 
	PopupVerticalAlign="TopSides" Width="100%"
    AppearAfter="0"  Theme="NETheme01"  CloseAction="CloseButton" >
    	<ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
	<dx1:ASPxCalendar ID="cal" runat="server" ClientInstanceName="cal" OnDayCellPrepared="cal_DayCellPrepared" Rows="2" ShowClearButton="False">
    </dx1:ASPxCalendar>
		</dx:PopupControlContentControl>
</ContentCollection>

</dx:ASPxPopupControl>


<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	
	
	
	SelectCommand="SELECT
appointments.StartDate,
appointments.EndDate,
IF ((appointments.woprog_id!=0),woprog.woprog_description,appointments.Description) SUBJECT,
appointments.Location,
appointments.Description,
appointments.`Status`,
appointments.ResourceId,
appointments.business_unit_id,
appointments.woprog_id,
appointments.quote_id,
appointments.setby,
appointments.assetid,
appointments.member_id,
appointments.membertype_id,
appointments.confirmed,
CONCAT(address.Address_Addr1,', ',address.Address_City) addr,
CONCAT('//maps.google.com?q=',urlencode(CONCAT(address.Address_Addr1,' ',address.Address_City))) addr1,
CONCAT('Contact: ',' ',contact.Contact_Name) Contact_Name,
    CONCAT('Location in Plant: ',' ',woprog.woprog_location_in_plant) location_in_plant,
contact.Contact_CellPhone phone,
CONCAT('tel:',REPLACE(REPLACE(REPLACE(REPLACE(contact.Contact_CellPhone, ' ', ''), '(', ''), ')', ''), '-', '')) phone1,
CONCAT('Truck ',assets.assets_No) truckno,
IF ((appointments.woprog_id!=0),CONCAT('Special Instructions: ',woprog.WOProg_SpecialInstructions),appointments.notes) notes,
appointments.id id,
CONCAT('tel:',(IFNULL((SELECT cellphone.number FROM cellphone WHERE cellphone.member_id = member.member_id AND cellphone_status_id = 1  LIMIT 1),'9999999999'))) pmcell,
CONCAT('PM: ',member.member_fullname) pm,

CONCAT(' Hours Entered ',DATE_FORMAT(appointments.StartDate,'%b %d'),' : ', ROUND((SELECT 
	IFNULL(SUM(membertime.NumberofHours),0) 
FROM 
	membertime 
WHERE 
	membertime_memberid = ?MID AND 
	DATE(membertime.date) = DATE(appointments.StartDate) AND
	MemberTime_Mileage != 'true'),1)) hours,

	(
	IF ((TIMESTAMPDIFF(MINUTE,appointments.StartDate,appointments.EndDate)/60)&gt;8,
	ROUND((TIMESTAMPDIFF(MINUTE,appointments.StartDate,appointments.EndDate)/60),2)-0.5,
	ROUND((TIMESTAMPDIFF(MINUTE,appointments.StartDate,appointments.EndDate)/60),2))
	-(SELECT 
	IFNULL(SUM(membertime.NumberofHours),0) 
FROM 
	membertime 
WHERE 
	membertime_memberid = ?MID AND 
	DATE(membertime.date) = DATE(appointments.StartDate) AND
	MemberTime_Mileage != 'true')
	) hours_left,

IF((SELECT 
	IFNULL(SUM(membertime.NumberofHours),0) 
FROM 
	membertime 
WHERE 
	membertime_memberid = ?MID AND 
	DATE(membertime.date) = DATE(appointments.StartDate) AND
	MemberTime_Mileage != 'true')&gt;8,2,1) pt,
	CONCAT('WO ',TRIM(LEADING '0' FROM woprog.woprog_bvwo),' ',woprog.woprog_customername) bvwo,
	IF(appointments.meet_at_shop=1,'Meet at the shop','Meet on Site') meet,
IF ((appointments.woprog_id!=0),woprog.WOProg_Hold,0) wo_isonhold,
IF ((appointments.woprog_id!=0),woprog.WOProg_Status,'Open') wo_status,
IF (oncall_schedule.id IS NULL,FALSE,TRUE) oncall

FROM
appointments
LEFT JOIN woprog ON appointments.woprog_id = woprog.WOProg_ID
LEFT JOIN quote_master ON appointments.quote_id = quote_master.quote_id 
LEFT JOIN address ON woprog.WOProg_Address_ID = address.Address_ID
LEFT JOIN contact ON woprog.WOProg_Contact_ID = contact.Contact_ID
LEFT JOIN member ON woprog.woprog_pm_memberid = member.member_id AND member.business_unit_id = woprog.business_unit_id
LEFT JOIN assets ON appointments.assetid = assets.assets_ID 
LEFT JOIN oncall_schedule ON appointments.ResourceId = oncall_schedule.member_id AND DATE(oncall_schedule.date)= DATE(appointments.StartDate)
WHERE appointments.resourceid = ?MID 

AND appointments.StartDate&gt;=?did AND (appointments.status=0 OR appointments.status=98 OR (appointments.status&gt;50 AND appointments.status&lt;71))
AND (appointments.StartDate&lt;=CURDATE() + INTERVAL 14 DAY) 
UNION
SELECT
vacation_master.date_start StartDate,
vacation_master.date_end EndDate,
vacation_type.type `Subject`,
'' Location,
IFNULL(vacation_type.type,'') Description,
 (vacation_type.id+100) `Status`,
vacation_master.member_id ResourceId,
IFNULL(vacation_master.business_unit_id,0) business_unit_id,
0 woprog_id,
0 quote_id,
IFNULL(vacation_master.create_member_id,0) setby,
0 assetid,
vacation_master.member_id member_id,
0 membertype_id,
0 confirmed,
'' addr,
'' addr1,
'' Contact_Name,
    '' location_in_plant,
''  phone,
''  phone1,
''  truckno,
CONCAT(vacation_status.`status`, ' - ', IFNULL(vacation_master.comments,'')) notes,
(vacation_master.vacation_id + 100000000) id,
'' pm,
'' pmcell,
0 hours,
0 hours_left,
1 pt,
'' bvwo,'' meet,
0 wo_isonhold,
'' wo_status,
IF (oncall_schedule.id IS NULL,FALSE,TRUE) oncall

FROM vacation_master 
INNER JOIN vacation_type ON vacation_master.type_id = vacation_type.id 
INNER JOIN vacation_status ON vacation_master.`status` = vacation_status.status_id 
LEFT JOIN oncall_schedule ON vacation_master.member_id = oncall_schedule.member_id AND DATE(oncall_schedule.date)= DATE(vacation_master.date_start)

WHERE vacation_master.member_id = ?MID AND vacation_master.date_start &gt;= ?did 

UNION
SELECT
TIMESTAMP(CONCAT(DATE(training_header_history.date),' ',training_header_history.start_time))  StartDate,
TIMESTAMP(CONCAT(DATE(training_header_history.date),' ',training_header_history.end_time))  EndDate,
'Cap Training' `Subject`,
IFNULL(cap_training_schedule.location,'') Location,
IFNULL(training_header.`name`,'') Description,
52 `Status`,
training_header_history.member_id ResourceId,
training_header_history.business_unit_id,
0 woprog_id,
0 quote_id,
0 setby,
0 assetid,
training_header_history.member_id member_id,
0 membertype_id,
0 confirmed,
'' addr,
'' addr1,
IFNULL(cap_training_schedule.location,'') Contact_Name,
       '' location_in_plant,
''  phone,
''  phone1,
''  truckno,
training_header.`name` notes,
(training_header_history.id + 400000000) id,
'' pm,
'' pmcell,
0 hours,
IF ((TIMESTAMPDIFF(MINUTE,TIMESTAMP(CONCAT(DATE(training_header_history.date),' ',training_header_history.start_time)),TIMESTAMP(CONCAT(DATE(training_header_history.date),' ',training_header_history.end_time)))/60)&gt;8,
	ROUND((TIMESTAMPDIFF(MINUTE,TIMESTAMP(CONCAT(DATE(training_header_history.date),' ',training_header_history.start_time)),TIMESTAMP(CONCAT(DATE(training_header_history.date),' ',training_header_history.end_time)))/60),2)-0.5,
	ROUND((TIMESTAMPDIFF(MINUTE,TIMESTAMP(CONCAT(DATE(training_header_history.date),' ',training_header_history.start_time)),TIMESTAMP(CONCAT(DATE(training_header_history.date),' ',training_header_history.end_time)))/60),2)) hours_left,
1 pt,
'' bvwo,'' meet,
0 wo_isonhold,
'' wo_status,
IF (oncall_schedule.id IS NULL,FALSE,TRUE) oncall

FROM
        training_header_history
INNER JOIN training_header ON training_header.id = training_header_history.training_header_id
INNER JOIN cap_training_schedule ON cap_training_schedule.id = training_header_history.cap_training_schedule_id
LEFT JOIN oncall_schedule ON training_header_history.member_id = oncall_schedule.member_id AND DATE(oncall_schedule.date)= DATE(training_header_history.date)

WHERE
training_header_history.member_id= ?MID
AND  training_header_history.date &gt;= ?did
ORDER BY StartDate
">
	<SelectParameters>
		<asp:ControlParameter ControlID="hdn_mid" Name="mid" PropertyName="Value" />
		<asp:ControlParameter ControlID="hdn_did" Name="did" PropertyName="Value" />
       
	</SelectParameters>
</asp:SqlDataSource>

<dx:ASPxHiddenField ID="hdn_woprog_id" runat="server" ClientInstanceName="hdn_woprog_id">
</dx:ASPxHiddenField>
<asp:HiddenField ID="hdn_mid" runat="server" />
<asp:HiddenField ID="hdn_did" runat="server" />
<asp:HiddenField ID="hdn_hourtype" runat="server" />
<asp:HiddenField ID="hdn_showJobType" runat="server" />