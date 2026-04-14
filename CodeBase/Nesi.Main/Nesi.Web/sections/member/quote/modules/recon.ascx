<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_quote_modules_recon" Codebehind="recon.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>







<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<script type="text/javascript" language="javascript">
	function DisableButton() {
		window.setTimeout('btnmanpower_done.SetEnabled(false)', 0);
		window.setTimeout('pc1.SetEnabled(false)', 0);
		window.setTimeout('btnmarket_done.SetEnabled(false)', 0);
		window.setTimeout('btn_custrecon_done.SetEnabled(false)', 0);
		window.setTimeout('btnfinance_done.SetEnabled(false)', 0);
		window.setTimeout('btnclear_market_recon.SetEnabled(false)', 0);
		window.setTimeout('btnclear_manpower_recon.SetEnabled(false)', 0);
		window.setTimeout('btnclear_cust_recon.SetEnabled(false)', 0);
		window.setTimeout('btnclear_finance_recon.SetEnabled(false)', 0);

		window.setTimeout('btn_printrecon.SetEnabled(false)', 0);
	}


</script>
<style type="text/css">
	.style1
	{
		font-family: Arial;
		font-size: small;
		font-weight: bold;
	}
	.style2
	{
		text-align: center;
		font-size: medium;
	}
	.style3
	{
		height: 25px;
	}
	.style4
	{
		font-size: small;
		color: #FFFFFF;
	}
	.dxbButton
{
	color: #000000;
	font: normal 12px Tahoma, Geneva, sans-serif;
	vertical-align: middle;
	border: 1px solid #7F7F7F;
	
	padding: 1px;
	cursor: pointer;
}
	.style6
	{
		font-family: Arial;
		font-size: medium;
		text-align: left;
		color:black;
	}
.dxgvControl,
.dxgvDisabled
{
	border: 1px Solid #9F9F9F;
	font: 12px Tahoma, Geneva, sans-serif;
	background-color: #F2F2F2;
	color: Black;
	cursor: default;
}

.dxgvTitlePanel, 
.dxgvTable caption
{
	font-size: 15px;
	font-weight: normal;
	padding: 3px 3px 5px;
	text-align: center;
	background-color: #ACACAC;
	color: White;
	border-bottom: 1px Solid #9F9F9F;
}
.dxeBase
{
	font: 12px Tahoma, Geneva, sans-serif;
}
.dxgvTable
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}

.dxgvTable
{
	background-color: White;
	border-width: 0;
	border-collapse: separate!important;
	overflow: hidden;
	color: Black;
}
.dxeTextBoxSys, 
.dxeMemoSys 
{
    border-collapse:separate!important;
}

.dxeTrackBar, 
.dxeIRadioButton, 
.dxeButtonEdit, 
.dxeTextBox, 
.dxeRadioButtonList, 
.dxeCheckBoxList, 
.dxeMemo, 
.dxeListBox, 
.dxeCalendar, 
.dxeColorTable
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}
.dxeTextBox,
.dxeMemo
{
	background-color: white;
	border: 1px solid #9f9f9f;
}

.dxeMemoEditAreaSys 
{
    *margin: -1px 0px;
    *padding-right: 4px;
}
.dxeMemoEditAreaSys 
{
    padding-right: 4px\0/;
}
.dxeMemoEditAreaSys 
{
    padding: 3px 3px 0px 3px;
    margin: 0px;
    border-width: 0px;
	display: block;
	resize: none;
}
.dxeMemoEditArea
{
	background-color: white;
	font: 12px Tahoma, Geneva, sans-serif;
	outline: none;
}


.dxeButtonEdit .dxeEditArea
{
	background-color: white;
}

.dxeEditArea
{
	font: 12px Tahoma, Geneva, sans-serif;
	border: 1px solid #A0A0A0;
}
.dxeButtonEditButton,
.dxeSpinIncButton,
.dxeSpinDecButton,
.dxeSpinLargeIncButton,
.dxeSpinLargeDecButton
{
	padding: 0px 2px 0px 3px;
	
}
.dxeButtonEditButton,
.dxeCalendarButton,
.dxeSpinIncButton,
.dxeSpinDecButton,
.dxeSpinLargeIncButton,
.dxeSpinLargeDecButton
{
	vertical-align: middle;
	border: 1px solid #7f7f7f;
	cursor: pointer;
} 

.blink_me {
    -webkit-animation-name: blinker;
    -webkit-animation-duration: 1s;
    -webkit-animation-timing-function: linear;
    -webkit-animation-iteration-count: infinite;
    
    -moz-animation-name: blinker;
    -moz-animation-duration: 1s;
    -moz-animation-timing-function: linear;
    -moz-animation-iteration-count: infinite;
    
    animation-name: blinker;
    animation-duration: 1s;
    animation-timing-function: linear;
    animation-iteration-count: infinite;
}

@-moz-keyframes blinker {  
    0% { opacity: 1.0; }
    50% { opacity: 0.0; }
    100% { opacity: 1.0; }
}


@-webkit-keyframes blinker {  
    0% { opacity: 1.0; }
    50% { opacity: 0.0; }
    100% { opacity: 1.0; }
}

@keyframes blinker {  
    0% { opacity: 1.0; }
    50% { opacity: 0.0; }
    100% { opacity: 1.0; }
}

	
	.style7
	{
		font-family: Arial;
		font-size: medium;
		text-align: left;
		color:white;
	}

	
	.style8
	{
		font-size: medium;
	}
	.style9
	{
		text-align: center;
	}
	.style10
	{
		height: 25px;
		text-align: center;
	}

	
</style>
<div>
	<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
		BackColor="#E8E8E8" Width="100%" Font-Names="Arial" ClientInstanceName="pc1">
		<TabPages>
			<dx:TabPage Text="Process Schedule">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table cellpadding="5px" 
							style="margin: 0px; padding: 0px; width: 100%; font-family: Arial, Helvetica, sans-serif; border-collapse: collapse;">
							<tr class="style1">
								<td align="center">
									&nbsp;</td>
								<td colspan="4">
									<dx:ASPxLabel ID="lbl_lvl" runat="server" Font-Bold="True" Font-Names="Arial" 
										ForeColor="#FF3300" Text="ASPxLabel">
									</dx:ASPxLabel>
								</td>
							</tr>
							<tr class="style1">
								<td align="center" class="style8">
									Completed</td>
									<td></td>
								<td class="style2" style="background-color: #E8E8E8">
									Milestone</td>
								<td class="style2">
									Deadline Dates</td>
								<td class="style2">
									Assigned To</td>
								<td class="style2">
									Notes</td>
							</tr>
							<tr>
								<td nowrap="nowrap" align="center">
									&nbsp;</td><td>
									<asp:Image ID="chk_openquote" runat="server" ImageUrl="~/images/icon/CheckGreen.png" />
								</td>
								<td bgcolor="#E8E8E8" nowrap="nowrap">
									Open Quote</td>
								<td>
									<dx:ASPxDateEdit ID="dte_openquote" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Height="20px" Width="150px" ReadOnly="True" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td class="style9">
									<dx:ASPxComboBox ID="ddl_openquote" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True"  
										Font-Names="Arial" Height="20px" IncrementalFilteringMode="Contains" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_all_members" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td width="100%">
									<dx:ASPxMemo ID="mem" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td nowrap="nowrap" align="center">
									&nbsp;</td><td><asp:Image ID="chk_screeningcompleted" runat="server" ImageUrl="~/images/icon/CheckGreen.png" /></td>
								<td nowrap="nowrap" bgcolor="#9CFF9C" style="background-color: #E8E8E8">
									
												Stage 1 Screening Completed</td>
								<td>
									<dx:ASPxDateEdit ID="dte_screeningcompleted" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" ReadOnly="True" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_screeningcompleted" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True" 
										Font-Names="Arial" IncrementalFilteringMode="Contains" 
										TextField="memberfullname" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_screeningcompleted" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td  id="schedule_title" runat="server" nowrap="nowrap" align="center">
									<dx:ASPxButton ID="btn_emailschedule" runat="server" ClientEnabled="False" 
										Font-Bold="False" Font-Names="Arial" Height="20px" 
										OnClick="btn_emailschedule_Click" Text="Email" Width="75px" HorizontalAlign="Center">
										<Image Url="~/images/icon/icon[email].gif">
										</Image>
										<Paddings Padding="0px" />

<Paddings Padding="0px"></Paddings>
									</dx:ASPxButton>
								</td><td><asp:Image ID="chk_scheduleproduced" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td nowrap="nowrap" style="background-color: #E8E8E8">
									Schedule Produced and Distributed (This is the Point Person)</td>
								<td>
									<dx:ASPxDateEdit ID="dte_scheduleproduced" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_scheduleproduced" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True" 
										Font-Names="Arial" IncrementalFilteringMode="StartsWith" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_all_members" ToolTip="THIS IS THE POINT PERSON FOR THIS QUOTE!" 
										OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_scheduleproduced" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr id="manpower_info_tr" runat="server">
								<td id="manpower_title" runat="server" nowrap="nowrap" align="center">
									&nbsp;</td><td><asp:Image ID="chk_manpower" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td runat="server" nowrap="nowrap" style="background-color: #E8E8E8" >
									Manpower Information Collected</td>
								<td>
									<dx:ASPxDateEdit ID="dte_manpower" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_manpower" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True"
										Font-Names="Arial" IncrementalFilteringMode="Contains" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_field_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_manpower" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr id="cust_info_tr" runat="server">
<td  id="customer_title" runat="server" nowrap="nowrap" align="center"></td><td><asp:Image ID="chk_customerinfo" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td runat="server" nowrap="nowrap" style="background-color: #E8E8E8">
									Customer Information Collected</td>
								<td>
									<dx:ASPxDateEdit ID="dte_customerinfo" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_customerinfo" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True" 
										Font-Names="Arial" IncrementalFilteringMode="Contains" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_sales_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_customerinfo" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr id="market_info_tr" runat="server">
							<td  id="market_title" runat="server" nowrap="nowrap" align="center"></td><td><asp:Image ID="chk_marketinfo" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td runat="server" nowrap="nowrap" style="background-color: #E8E8E8">
									Market Information Collected</td>
								<td>
									<dx:ASPxDateEdit ID="dte_marketinfo" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_marketinfo" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True"
										Font-Names="Arial" IncrementalFilteringMode="Contains" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_sales_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_marketinfo" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr id="finance_info_tr" runat="server">
							<td  id="finance_title" runat="server" nowrap="nowrap" align="center"></td><td><asp:Image ID="chk_financeinfo" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td runat="server" nowrap="nowrap" style="background-color: #E8E8E8">
									Finance Information Collected</td>
								<td>
									<dx:ASPxDateEdit ID="dte_financeinfo" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_financeinfo" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True"
										Font-Names="Arial" IncrementalFilteringMode="Contains" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_finance_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_financeinfo" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr id="recon_created_tr" runat="server">
								<td  id="recon_report_title" runat="server" align="center">
									<dx:ASPxButton ID="btn_printrecon" runat="server" ClientEnabled="False" 
										Font-Bold="False" Font-Names="Arial" Height="20px" HorizontalAlign="Center" 
										OnClick="btn_printrecon_Click" Text="Verified" Width="75px" ClientInstanceName="btn_printrecon">
										<ClientSideEvents Click="function(s, e) {
	DisableButton();
}" />
										<Image Height="15px" Url="~/images/icon/icon[approve].gif">
										</Image>
										<Paddings Padding="0px" />

<Paddings Padding="0px"></Paddings>
									</dx:ASPxButton>
								</td><td><asp:Image ID="chk_reconinfo" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td runat="server" nowrap="nowrap" style="background-color: #E8E8E8">
									Recon Information Inspected</td>
								<td>
									<dx:ASPxDateEdit ID="dte_reconinfo" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_reconinfo" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True"
										Font-Names="Arial" IncrementalFilteringMode="Contains" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_all_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_reconinfo" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr id="stage4_tr" runat="server">
								<td  id="stage4_title2" runat="server" nowrap="nowrap" align="center"></td><td><asp:Image ID="chk_gonogo" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td id="stage4_title" runat="server" bgcolor="#FF9966" class="dxmLite" nowrap="nowrap" tooltip="All Quote Review Members Must Sign Off Before Proceeding">
									<strong>Stage 4 Go-No Go Review Meeting</strong></td>
									
								<td bgcolor="#FF9966">
									<dx:ASPxDateEdit ID="dte_gonogo" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td bgcolor="#FF9966" align="center" class="style9">
									<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="200px" 
										ClientInstanceName="cb_s4_review" OnCallback="cb_s4_review_Callback">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
												<table style="width:100%;">
													<tr>
														<td>
															<dx:ASPxCheckBox ID="chk_rt1_s4" runat="server" CheckState="Unchecked" 
																Font-Bold="True" Font-Names="Arial" ClientEnabled="False">
																<ClientSideEvents CheckedChanged="function(s, e) {
	cb_s4_review.PerformCallback('1');
}" />
<ClientSideEvents CheckedChanged="function(s, e) {
	cb_s4_review.PerformCallback(&#39;1&#39;);
}"></ClientSideEvents>

																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxCheckBox>
														</td>
														<td>
															<dx:ASPxLabel ID="lbl_dte_rt1_s4" runat="server" Text="" Font-Bold="True" 
																Font-Names="Arial">
																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxLabel>
														</td>
														</tr>
													<tr>
														<td>
															<dx:ASPxCheckBox ID="chk_rt2_s4" runat="server" CheckState="Unchecked" 
																Font-Bold="True" Font-Names="Arial" ClientEnabled="False">
																<ClientSideEvents CheckedChanged="function(s, e) {
	cb_s4_review.PerformCallback('2');
}" />
<ClientSideEvents CheckedChanged="function(s, e) {
	cb_s4_review.PerformCallback(&#39;2&#39;);
}"></ClientSideEvents>

																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxCheckBox>
														</td>
														<td>
															<dx:ASPxLabel ID="lbl_dte_rt2_s4" runat="server" Text="" Font-Bold="True" 
																Font-Names="Arial">
																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxLabel>
														</td>
														
													</tr>
													<tr>
														<td>
															<dx:ASPxCheckBox ID="chk_rt3_s4" runat="server" CheckState="Unchecked" 
																Font-Bold="True" Font-Names="Arial" ClientEnabled="False">
																<ClientSideEvents CheckedChanged="function(s, e) {
	cb_s4_review.PerformCallback('3');
}" />
<ClientSideEvents CheckedChanged="function(s, e) {
	cb_s4_review.PerformCallback(&#39;3&#39;);
}"></ClientSideEvents>

																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxCheckBox>
														</td>
														<td>
															<dx:ASPxLabel ID="lbl_dte_rt3_s4" runat="server" Text="" Font-Bold="True" 
																Font-Names="Arial">
																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxLabel>
														</td>
														
													</tr>
													<tr>
														<td>
															<dx:ASPxCheckBox ID="chk_rt4_s4" runat="server" CheckState="Unchecked" 
																Font-Bold="True" Font-Names="Arial" ClientEnabled="False">
																<ClientSideEvents CheckedChanged="function(s, e) {
	cb_s4_review.PerformCallback('4');
}" />
<ClientSideEvents CheckedChanged="function(s, e) {
	cb_s4_review.PerformCallback(&#39;4&#39;);
}"></ClientSideEvents>

																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxCheckBox>
														</td>
														<td>
															<dx:ASPxLabel ID="lbl_dte_rt4_s4" runat="server" Text="" Font-Bold="True" 
																Font-Names="Arial">
																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxLabel>
														</td>
														
													</tr>
												</table>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxCallbackPanel>
								</td>
								<td bgcolor="#FF9966">
									<dx:ASPxMemo ID="mem_gonogo" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td  id="strategy_title" runat="server" nowrap="nowrap" align="center"></td><td><asp:Image ID="chk_sellstrategy" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td nowrap="nowrap" style="background-color: #E8E8E8">
									Sell Strategy Developed</td>
								<td>
									<dx:ASPxDateEdit ID="dte_sellstrategy" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_sellstrategy" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True" IncrementalFilteringMode="Contains" 
										Font-Names="Arial" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_sales_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_sellstrategy" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td  id="estimated_title" runat="server" nowrap="nowrap" align="center">
									<dx:ASPxButton ID="btn_est_review" runat="server" ClientEnabled="False" 
										Font-Bold="False" Font-Names="Arial" Height="20px" HorizontalAlign="Center" 
										OnClick="btn_est_review_Click" Text="Done" Width="75px">
										<Image Height="15px" Url="~/images/icon/icon[approve].gif">
										</Image>
										<Paddings Padding="0px" />

<Paddings Padding="0px"></Paddings>
									</dx:ASPxButton>
								</td><td>
									<asp:Image ID="chk_projectestimated" runat="server" ImageUrl="~/images/icon/CheckGrey.png" />
								</td>
								<td nowrap="nowrap" style="background-color: #E8E8E8">
									<asp:Label ID="lblestimate" runat="server" Text="Project Estimated"></asp:Label>
								</td>
								<td>
									<dx:ASPxDateEdit ID="dte_projectestimated" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_projectestimated" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True" IncrementalFilteringMode="Contains" 
										Font-Names="Arial" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_field_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_projectestimated" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td  id="ws_review_title" runat="server" nowrap="nowrap" align="center">
									<dx:ASPxButton ID="btn_ws_review" runat="server" ClientEnabled="False" 
										Font-Bold="False" Font-Names="Arial" Height="20px" HorizontalAlign="Center" 
										OnClick="btn_ws_review_Click" Text="Done" Width="75px">
										<Image Url="~/images/icon/icon[approve].gif" Height="15px">
										</Image>
										<Paddings Padding="0px" />

<Paddings Padding="0px"></Paddings>
									</dx:ASPxButton>
								</td><td>
									<asp:Image ID="chk_worksheetreview" runat="server" ImageUrl="~/images/icon/CheckGrey.png" />
								</td>
								<td nowrap="nowrap" style="background-color: #E8E8E8">
									Estimate Worksheet Review</td>
								<td>
									<dx:ASPxDateEdit ID="dte_worksheetreview" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_worksheetreview" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True" IncrementalFilteringMode="StartsWith" 
										Font-Names="Arial" 
										TextField="memberfullname" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_worksheetreview" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr id="final_review_tr" runat="server">
								<td  id="stage6_title2" runat="server" nowrap="nowrap" align="center" 
									style="text-align: center">
									<dx:ASPxButton ID="btn_print_final_quote0" runat="server" ClientEnabled="False" 
										Font-Bold="False" Font-Names="Arial" Font-Size="8pt" Height="20px" 
										HorizontalAlign="Center" OnClick="btn_print_final_quote_Click" 
										Text="Print Quote" Width="75px">
										<ClientSideEvents Click="function(s, e) {

	

}" />
<ClientSideEvents Click="function(s, e) {

	

}"></ClientSideEvents>

										<Image Url="~/images/icon/icon[print].gif">
										</Image>
										<Paddings Padding="0px" />

<Paddings Padding="0px"></Paddings>
									</dx:ASPxButton>
								</td><td>
									<asp:Image ID="chk_finalreview" runat="server" ImageUrl="~/images/icon/CheckGrey.png" />
								</td>
								<td id="stage6_title" runat="server" bgcolor="#FF9966" class="dxmLite" nowrap="nowrap">
									<strong>Stage 6 Final Review Meeting</strong></td>
								<td bgcolor="#FF9966">
									<dx:ASPxDateEdit ID="dte_finalreview" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td bgcolor="#FF9966" align="center" class="style9">
									<dx:ASPxCallbackPanel ID="cb_f_review" runat="server" Width="200px" 
										ClientInstanceName="cb_f_review" OnCallback="cb_f_review_Callback">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
												<table id ="stage_f_table" style="width:100%;">
													<tr>
														<td style="margin-left: 40px">
															<dx:ASPxCheckBox ID="chk_rt1_f" runat="server" CheckState="Unchecked" 
																Font-Bold="True" Font-Names="Arial" ClientEnabled="False">
																<ClientSideEvents CheckedChanged="function(s, e) {
	cb_f_review.PerformCallback('1');
}" />
<ClientSideEvents CheckedChanged="function(s, e) {
	cb_f_review.PerformCallback(&#39;1&#39;);
}"></ClientSideEvents>

																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxCheckBox>
														</td>
														<td>
															<dx:ASPxLabel ID="lbl_dte_rt1_f" runat="server" Text="" Font-Bold="True" 
																Font-Names="Arial">
																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxLabel>
														</td>
														</tr>
													<tr>
														<td>
															<dx:ASPxCheckBox ID="chk_rt2_f" runat="server" CheckState="Unchecked" 
																Font-Bold="True" Font-Names="Arial" ClientEnabled="False">
																<ClientSideEvents CheckedChanged="function(s, e) {
	cb_f_review.PerformCallback('2');
}" />
<ClientSideEvents CheckedChanged="function(s, e) {
	cb_f_review.PerformCallback(&#39;2&#39;);
}"></ClientSideEvents>

																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxCheckBox>
														</td>
														<td>
															<dx:ASPxLabel ID="lbl_dte_rt2_f" runat="server" Text="" Font-Bold="True" 
																Font-Names="Arial">
																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxLabel>
														</td>
														
													</tr>
													<tr>
														<td>
															<dx:ASPxCheckBox ID="chk_rt3_f" runat="server" CheckState="Unchecked" 
																Font-Bold="True" Font-Names="Arial" ClientEnabled="False">
																<ClientSideEvents CheckedChanged="function(s, e) {
	cb_f_review.PerformCallback('3');
}" />
<ClientSideEvents CheckedChanged="function(s, e) {
	cb_f_review.PerformCallback(&#39;3&#39;);
}"></ClientSideEvents>

																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxCheckBox>
														</td>
														<td>
															<dx:ASPxLabel ID="lbl_dte_rt3_f" runat="server" Text="" Font-Bold="True" 
																Font-Names="Arial">
																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxLabel>
														</td>
														
													</tr>
													<tr>
														<td>
															<dx:ASPxCheckBox ID="chk_rt4_f" runat="server" CheckState="Unchecked" 
																Font-Bold="True" Font-Names="Arial" ClientEnabled="False">
																<ClientSideEvents CheckedChanged="function(s, e) {
	cb_f_review.PerformCallback('4');
}" />
<ClientSideEvents CheckedChanged="function(s, e) {
	cb_f_review.PerformCallback(&#39;4&#39;);
}"></ClientSideEvents>

																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxCheckBox>
														</td>
														<td>
															<dx:ASPxLabel ID="lbl_dte_rt4_f" runat="server" Text="" Font-Bold="True" 
																Font-Names="Arial">
																<DisabledStyle ForeColor="Gray">
																</DisabledStyle>
															</dx:ASPxLabel>
														</td>
														
													</tr>
												</table>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxCallbackPanel>
								</td>
								<td bgcolor="#FF9966">
									<dx:ASPxMemo ID="mem_finalreview" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td  id="quote_delivered_title" runat="server" nowrap="nowrap" align="center" 
									style="text-align: center">
									<dx:ASPxButton ID="btn_delivered" runat="server" ClientEnabled="False" 
										Font-Bold="False" Font-Names="Arial" Height="20px" HorizontalAlign="Center" 
										OnClick="btn_delivered_click" Text="Delivered" Width="75px" Font-Size="8pt">
										<Image Height="15px" Url="~/images/icon/icon[approve].gif">
										</Image>
										<Paddings Padding="0px" />

<Paddings Padding="0px"></Paddings>
									</dx:ASPxButton>
								</td><td>
									<asp:Image ID="chk_quotedelivered" runat="server" Height="16px" ImageUrl="~/images/icon/CheckGrey.png" />
								</td>
								<td bgcolor="Red" class="style4" nowrap="nowrap">
									<strong>Quote Delivered (Due Date)</strong></td>
								<td bgcolor="Red">
									<dx:ASPxDateEdit ID="dte_quotedelivered" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td bgcolor="Red" align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_quotedelivered" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True" IncrementalFilteringMode="Contains" 
										Font-Names="Arial" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_all_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td bgcolor="Red">
									<dx:ASPxMemo ID="mem_quotedelivered" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td  id="followup1_title" runat="server" nowrap="nowrap" align="center"></td><td>
								<asp:Image ID="chk_followup1" runat="server" ImageUrl="~/images/icon/CheckGrey.png" />
								</td>
								<td class="style3" nowrap="nowrap" style="background-color: #E8E8E8">
									Follow up 1</td>
								<td class="style3">
									<dx:ASPxDateEdit ID="dte_followup1" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td class="style10" align="center">
									<dx:ASPxComboBox ID="ddl_followup1" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True" IncrementalFilteringMode="Contains" 
										Font-Names="Arial" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_all_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td class="style3">
									<dx:ASPxMemo ID="mem_followup1" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr id="followup_two_tr" runat="server">
								<td  id="followup2_title" runat="server" nowrap="nowrap" align="center"></td><td><asp:Image ID="chk_followup2" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td runat="server" nowrap="nowrap" style="background-color: #E8E8E8">
									Follow up 2</td>
								<td>
									<dx:ASPxDateEdit ID="dte_followup2" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_followup2" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True" IncrementalFilteringMode="Contains" 
										Font-Names="Arial" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_all_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem15" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td  id="convert_kill_title" runat="server" nowrap="nowrap" align="center">
									<dx:ASPxButton ID="btn_convert" runat="server" 
										Font-Bold="False" Font-Names="Arial" Font-Size="7pt" Height="20px" 
										HorizontalAlign="Center" OnClick="btn_convert_Click" Text="Convert" 
										Width="75px" ClientEnabled="False">
										<Image Height="15px" Url="~/images/icon/icon[approve].gif">
										</Image>
										<Paddings Padding="0px" />

<Paddings Padding="0px"></Paddings>
									</dx:ASPxButton>
									<dx:ASPxButton ID="btn_kill_quote" runat="server" 
										Font-Bold="False" Font-Names="Arial" Height="20px" HorizontalAlign="Center" Text="Kill" Width="75px" 
										AutoPostBack="False" visible="false">
										<ClientSideEvents Click="function(s, e) {
	pop_close.Show();
}" />
<ClientSideEvents Click="function(s, e) {
	pop_close.Show();
}"></ClientSideEvents>

										<Image Height="15px" Url="~/images/icon/icon[dead].gif">
										</Image>
										<Paddings Padding="0px" />

<Paddings Padding="0px"></Paddings>
									</dx:ASPxButton>
								</td><td><asp:Image ID="chk_convertkill" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td nowrap="nowrap" style="background-color: #E8E8E8">
									Convert or Kill</td>
								<td>
									<dx:ASPxDateEdit ID="dte_convertkill" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init" ClientEnabled="False">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_convertkill" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True" IncrementalFilteringMode="Contains" 
										Font-Names="Arial" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_all_members" OnInit="ddl0_Init" ClientEnabled="False">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem16" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr runat="server" id="row_postmortem" visible="false">
								<td  id="post_mortem_title" runat="server" nowrap="nowrap" align="center"></td><td><asp:Image ID="chk_postmortem" runat="server" ImageUrl="~/images/icon/CheckGrey.png" /></td>
								<td nowrap="nowrap" style="background-color: #E8E8E8">
									Post Mortem Completed</td>
								<td>
									<dx:ASPxDateEdit ID="dte_postmortem" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="DateTime" EditFormatString="yyyy-MM-dd HH:mm" Font-Names="Arial" 
										Width="150px" OnInit="dte0_Init">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<Border BorderStyle="None" />

<Border BorderStyle="None"></Border>
									</dx:ASPxDateEdit>
								</td>
								<td align="center" class="style9">
									<dx:ASPxComboBox ID="ddl_postmortem" runat="server" AnimationType="None" 
										BackColor="#F7F7F7" EnableCallbackMode="True"
										Font-Names="Arial" IncrementalFilteringMode="StartsWith" 
										TextField="member_name" ValueField="memberid" ValueType="System.Int32" 
										Width="150px" DataSourceID="sql_all_members" OnInit="ddl0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxMemo ID="mem_postmortem" runat="server" BackColor="#FFFFCC" Height="20px" 
										OnPreRender="mem_PreRender" Width="100%" OnInit="mem0_Init">
										<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td align="center">
									&nbsp;</td><td>&nbsp;</td>
								<td>
									&nbsp;</td>
								<td>
									&nbsp;</td>
								<td>
									&nbsp;</td>
								<td>
									&nbsp;</td>
							</tr>
							<tr>
								<td align="center">
									&nbsp;</td><td></td>
								<td>
									<dx:ASPxCallbackPanel ID="cb10" runat="server" ClientInstanceName="cb10" 
										OnCallback="cb10_Callback" Width="200px">
										<PanelCollection>
											<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxCallbackPanel>
								</td>
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
			<dx:TabPage Text="Manpower Recon">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxGridView ID="gv_manpower" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_manpower" DataSourceID="SqlDataSource3" 
							KeyFieldName="id" Width="100%" Font-Names="Arial" OnDataBound="gv_manpower_DataBound">
							<Columns>
								<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Question" FieldName="question" 
									ShowInCustomizationForm="True" VisibleIndex="0" Width="50%">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="score" FieldName="checked" 
									ShowInCustomizationForm="True" VisibleIndex="1" Width="60px">
									<DataItemTemplate>
										<dx:ASPxCheckBox ID="ASPxCheckBox3" runat="server" CheckState="Unchecked" 
											Height="25px" oninit="ASPxCheckBox3_Init" Text=" " 
											Value='<%# Eval("checked") %>' ValueChecked="1" ValueType="System.Int32" 
											ValueUnchecked="0">
										</dx:ASPxCheckBox>
									</DataItemTemplate>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="notes" FieldName="notes" 
									ShowInCustomizationForm="True" VisibleIndex="2" Width="50%">
									<DataItemTemplate>
										<dx:ASPxMemo ID="ASPxMemo3" runat="server" BackColor="#FFFFCC" Height="25px" 
											oninit="ASPxMemo3_Init" OnPreRender="mem_PreRender" Text='<%# Eval("notes") %>' 
											Width="100%">
											<Border BorderStyle="None" />
											<Border BorderStyle="None" />
										</dx:ASPxMemo>
									</DataItemTemplate>
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsPager Visible="False">
							</SettingsPager>
							<Settings ShowColumnHeaders="False" ShowTitlePanel="True" />

<Settings ShowTitlePanel="True" ShowColumnHeaders="False"></Settings>

							<Styles>
								<TitlePanel BackColor="#FFFF66">
								</TitlePanel>
							</Styles>
							<Templates>
								<TitlePanel>
									<table style="width:100%;">
										<tr>
											<td>
												<dx:ASPxButton ID="btnclear_manpower_recon" runat="server" 
													OnClick="btnclear_manpower_recon_Click" Text="Clear" ClientEnabled="False" 
													ClientInstanceName="btnclear_manpower_recon">
													<ClientSideEvents Click="function(s, e) {
	DisableButton();
}" />
												</dx:ASPxButton>
											</td>
											<td class="style6" width="100%">
												<strong>&nbsp;Manpower</strong></td>
											<td align="right" bgcolor="White" style="padding: 5px">
												<dx:ASPxCallbackPanel ID="cb2" runat="server" ClientInstanceName="cb2" 
													Height="25px" OnCallback="cb2_Callback">
													<PanelCollection>
														<dx:PanelContent ID="PanelContent3" runat="server" 
															SupportsDisabledAttribute="True">
															<dx:ASPxLabel ID="lblmanpower_recon_score" runat="server" ForeColor="Black" 
																style="font-family: Arial; text-align: right; font-size: large" Text="Score:">
															</dx:ASPxLabel>
														</dx:PanelContent>
													</PanelCollection>
												</dx:ASPxCallbackPanel>
											</td>
										</tr>
									</table>
								</TitlePanel>
							</Templates>
						</dx:ASPxGridView>
						<dx:ASPxButton ID="btnmanpower_done" runat="server" ClientEnabled="False" 
							Font-Bold="True" Font-Names="Arial" OnClick="btnmanpower_done_Click" 
							Text="All Done!" ClientInstanceName="btnmanpower_done">
							<ClientSideEvents Click="function(s, e) {
	
DisableButton();

}" />
							
						</dx:ASPxButton>
						<br />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Market Recon">
				<ContentCollection>
					<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxGridView ID="gv_market" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_market" DataSourceID="SqlDataSource2" Width="100%" KeyFieldName="id">
							<Columns>
								<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Question" FieldName="question" 
									ShowInCustomizationForm="True" VisibleIndex="0" Width="50%">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="score" FieldName="checked" 
									ShowInCustomizationForm="True" VisibleIndex="1" Width="60px">
									<DataItemTemplate>
										<dx:ASPxCheckBox ID="ASPxCheckBox2" runat="server" 
											Value='<%# Eval("checked") %>' CheckState="Unchecked" Height="25px" 
											oninit="ASPxCheckBox2_Init" Text=" " ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
										</dx:ASPxCheckBox>
									</DataItemTemplate>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="notes" FieldName="notes" 
									ShowInCustomizationForm="True" VisibleIndex="2" Width="50%">
									<DataItemTemplate>
										<dx:ASPxMemo ID="ASPxMemo2" runat="server" BackColor="#FFFFCC" Height="25px" 
											OnPreRender="mem_PreRender" oninit="ASPxMemo2_Init" Text='<%# Eval("notes") %>' Width="100%">
											<Border BorderStyle="None" />
											<Border BorderStyle="None" />
										</dx:ASPxMemo>
									</DataItemTemplate>
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsPager Visible="False">
							</SettingsPager>
							<Settings ShowColumnHeaders="False" ShowTitlePanel="True" />

<Settings ShowTitlePanel="True" ShowColumnHeaders="False"></Settings>

							<Styles>
								<TitlePanel BackColor="#0099FF">
								</TitlePanel>
							</Styles>
							<Templates>
								<TitlePanel>
									<table style="width:100%;">
										<tr>
											<td>
												<dx:ASPxButton ID="btnclear_market_recon" runat="server" 
										OnClick="btnclear_market_recon_Click" Text="Clear" ClientEnabled="False" ClientInstanceName="btnclear_market_recon">
													<ClientSideEvents Click="function(s, e) {
	DisableButton();
}" />
									</dx:ASPxButton>
											</td>
											<td width="100%" class="style6">
												<strong>&nbsp;Market</strong></td>
											<td align="right" bgcolor="White" style="padding: 5px">
												<dx:ASPxCallbackPanel ID="cb1" runat="server" ClientInstanceName="cb1" 
													Height="25px" OnCallback="cb1_Callback">
													<PanelCollection>
														<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
															<dx:ASPxLabel ID="lblmarket_recon_score" runat="server" 
																style="font-family: Arial; text-align: right; font-size: large" ForeColor="Black" Text="Score:">
															</dx:ASPxLabel>
														</dx:PanelContent>
													</PanelCollection>
												</dx:ASPxCallbackPanel>
											</td>
										</tr>
									</table>
								</TitlePanel>
							</Templates>
						</dx:ASPxGridView>
						<dx:ASPxButton ID="btnmarket_done" runat="server" ClientEnabled="False" 
							Font-Bold="True" Font-Names="Arial" OnClick="btnmarket_done_Click" 
							Text="All Done!" ClientInstanceName="btnmarket_done">
						</dx:ASPxButton>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Customer Recon">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxGridView ID="gv_cr" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_cr" DataSourceID="SqlDataSource1" Width="100%" KeyFieldName="id" 
							Font-Names="Arial">
							<Columns>
								<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Question" FieldName="question" 
									ShowInCustomizationForm="True" VisibleIndex="0" Width="50%">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="score" FieldName="checked" 
									ShowInCustomizationForm="True" VisibleIndex="1" Width="60px">
									<DataItemTemplate>
										<dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" 
											Value='<%# Eval("checked") %>' CheckState="Unchecked" Height="25px" 
											oninit="ASPxCheckBox1_Init" Text=" " ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
										</dx:ASPxCheckBox>
									</DataItemTemplate>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="notes" FieldName="notes" 
									ShowInCustomizationForm="True" VisibleIndex="2" Width="50%">
									<DataItemTemplate>
										<dx:ASPxMemo ID="ASPxMemo1" runat="server" BackColor="#FFFFCC" Height="25px" 
											OnPreRender="mem_PreRender" oninit="ASPxMemo1_Init" Text='<%# Eval("notes") %>' Width="100%">
											<Border BorderStyle="None" />
											<Border BorderStyle="None" />
										</dx:ASPxMemo>
									</DataItemTemplate>
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsPager Visible="False">
							</SettingsPager>
							<Settings ShowColumnHeaders="False" ShowTitlePanel="True" />

<Settings ShowTitlePanel="True" ShowColumnHeaders="False"></Settings>

							<Styles>
								<TitlePanel BackColor="#33CC33">
								</TitlePanel>
							</Styles>
							<Templates>
								<TitlePanel>
									<table style="width:100%;">
										<tr>
											<td>
												<dx:ASPxButton ID="btnclear_cust_recon" runat="server" 
													OnClick="btnclear_cust_recon_Click" Text="Clear" ClientEnabled="False" ClientInstanceName="btnclear_cust_recon">
													<ClientSideEvents Click="function(s, e) {
	DisableButton();
}" />
												</dx:ASPxButton>
											</td>
											<td width="100%" class="style7">
												<strong>Customer</strong></td>
											<td align="right" bgcolor="White" style="padding: 5px">
												<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
													Height="25px" OnCallback="cb_Callback">
													<PanelCollection>
														<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
															<dx:ASPxLabel ID="lblcust_recon_score" runat="server" 
																ClientInstanceName="lblcust_recon_score" ForeColor="Black" 
																style="font-family: Arial; text-align: right; font-size: large" Text="Score:">
															</dx:ASPxLabel>
														</dx:PanelContent>
													</PanelCollection>
												</dx:ASPxCallbackPanel>
											</td>
										</tr>
									</table>
								</TitlePanel>
							</Templates>
						</dx:ASPxGridView>
						<dx:ASPxButton ID="btn_custrecon_done" runat="server" ClientEnabled="False" 
							Font-Bold="True" Font-Names="Arial" OnClick="btn_custrecon_done_Click" 
							Text="All Done!" ClientInstanceName="btn_custrecon_done">
						</dx:ASPxButton>
						<br />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Finance Recon">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxGridView ID="gv_finance" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_finance" DataSourceID="SqlDataSource4" KeyFieldName="id" 
							Width="100%" OnDataBound="gv_finance_DataBound">
							<Columns>
								<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Question" FieldName="question" 
									ShowInCustomizationForm="True" VisibleIndex="0" Width="50%">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="score" FieldName="checked" 
									ShowInCustomizationForm="True" VisibleIndex="1" Width="60px">
									<DataItemTemplate>
										<dx:ASPxCheckBox ID="ASPxCheckBox4" runat="server" CheckState="Unchecked" 
											Height="25px" oninit="ASPxCheckBox4_Init" Text=" " 
											Value='<%# Eval("checked") %>' ValueChecked="1" ValueType="System.Int32" 
											ValueUnchecked="0">
										</dx:ASPxCheckBox>
									</DataItemTemplate>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="notes" FieldName="notes" 
									ShowInCustomizationForm="True" VisibleIndex="2" Width="50%">
									<DataItemTemplate>
										<dx:ASPxMemo ID="ASPxMemo4" runat="server" BackColor="#FFFFCC" Height="25px" 
											oninit="ASPxMemo4_Init" OnPreRender="mem_PreRender" Text='<%# Eval("notes") %>' 
											Width="100%">
											<Border BorderStyle="None" />
											<Border BorderStyle="None" />
										</dx:ASPxMemo>
									</DataItemTemplate>
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsPager Visible="False">
							</SettingsPager>
							<Settings ShowColumnHeaders="False" ShowTitlePanel="True" />

<Settings ShowTitlePanel="True" ShowColumnHeaders="False"></Settings>

							<Styles>
								<TitlePanel BackColor="#FF3300">
								</TitlePanel>
							</Styles>
							<Templates>
								<TitlePanel>
									<table style="width:100%;">
										<tr>
											<td>
												<dx:ASPxButton ID="btnclear_finance_recon" runat="server" 
													OnClick="btnclear_finance_recon_Click" Text="Clear" ClientEnabled="False" 
													ClientInstanceName="btnclear_finance_recon">
													<ClientSideEvents Click="function(s, e) {
	DisableButton();
}" />
												</dx:ASPxButton>
											</td>
											<td class="style6" width="100%">
												<strong>&nbsp;Finance</strong></td>
											<td align="right" bgcolor="White" style="padding: 5px">
												<dx:ASPxCallbackPanel ID="cb3" runat="server" ClientInstanceName="cb3" 
													Height="25px" OnCallback="cb3_Callback">
													<PanelCollection>
														<dx:PanelContent ID="PanelContent4" runat="server" 
															SupportsDisabledAttribute="True">
															<dx:ASPxLabel ID="lblfinance_recon_score" runat="server" ForeColor="Black" 
																style="font-family: Arial; text-align: right; font-size: large" Text="Score:">
															</dx:ASPxLabel>
														</dx:PanelContent>
													</PanelCollection>
												</dx:ASPxCallbackPanel>
											</td>
										</tr>
									</table>
								</TitlePanel>
							</Templates>
						</dx:ASPxGridView>
						<dx:ASPxButton ID="btnfinance_done" runat="server" ClientEnabled="False" 
							Font-Bold="True" Font-Names="Arial" OnClick="btnfinance_done_Click" 
							Text="All Done!" ClientInstanceName="btnfinance_done">
						</dx:ASPxButton>
						<br />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Sell Strategy">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxHtmlEditor ID="html_sell" runat="server" Width="100%" 
							BackColor="#FF9966">
						</dx:ASPxHtmlEditor>
						<table><tr><td>
						<dx:ASPxButton ID="ASPxButton2" runat="server" OnClick="ASPxButton2_Click" 
							Text="Save Notes">
						</dx:ASPxButton></td><td>
						<dx:ASPxButton ID="btnsell_done" runat="server" ClientEnabled="False" 
							Font-Bold="True" Font-Names="Arial" OnClick="btnsell_done_Click" 
							Text="All Done!">
						</dx:ASPxButton>
						</td></tr></table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Stats">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Estimating Concerns">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxHtmlEditor ID="html_estimating" runat="server" 
							ClientInstanceName="html_estimating" Width="100%" BackColor="#3399FF">
						</dx:ASPxHtmlEditor>
						<dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" 
							Text="Save Notes">
						</dx:ASPxButton>
						<br />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
		<Paddings PaddingTop="10px" />

<Paddings PaddingTop="10px"></Paddings>
	</dx:ASPxPageControl>

</div>

<p>
	<dx:ASPxPopupControl ID="pop_close" runat="server" 
		HeaderText="Kill this quote!" Height="300px" Width="400px" 
		ClientInstanceName="pop_close" CloseAction="CloseButton" Font-Names="Arial" 
		Modal="True" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" 
		PopupVerticalAlign="WindowCenter" onwindowcallback="pop_close_WindowCallback">
		<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_close=='close')
{

pop_close.Hide();

window.parent.window.location.reload();
}
}" />
		<HeaderStyle BackColor="#99FF66" Font-Size="14pt" />
		<ModalBackgroundStyle Opacity="0">
		</ModalBackgroundStyle>
		<ContentCollection>
			<dx:popupcontrolcontentcontrol runat="server" SupportsDisabledAttribute="True">
				<table style="width:100%;">
					<tr>
						<td colspan="3">
							&nbsp;</td>
					</tr>
					<tr>
						<td colspan="3">
							<div id="pop_close_list" runat="server"></div></td>
					</tr>
					<tr>
						<td colspan="3" style="font-weight: 700">
							Why are you closing this quote?</td>
					</tr>
					<tr>
						<td colspan="3">
							<dx:aspxmemo ID="mem_close_notes" runat="server" BackColor="#FFFFCC" 
								ClientInstanceName="mem_close_notes" Height="71px" Width="100%">
							</dx:aspxmemo>
						</td>
					</tr>
					<tr>
						<td>
							&nbsp;</td>
						<td>
							&nbsp;</td>
						<td align="right" style="text-align: right">
							&nbsp;</td>
					</tr>
					<tr>
						<td>
							<dx:aspxbutton ID="ASPxButton4" runat="server" Text="Kill" 
								AutoPostBack="False" Font-Names="Arial">
								<ClientSideEvents Click="function(s, e) {
	pop_close.PerformCallback();
}" />
							</dx:aspxbutton>
						</td>
						<td>
							&nbsp;</td>
						<td align="right">
							<dx:aspxbutton ID="ASPxButton5" runat="server" Text="Cancel" 
								AutoPostBack="False" Font-Names="Arial">
								<ClientSideEvents Click="function(s, e) {
	pop_close.Hide();
}" />
							</dx:aspxbutton>
						</td>
					</tr>
				</table>
			</dx:popupcontrolcontentcontrol>
		</ContentCollection>
	</dx:ASPxPopupControl>
	<br />
</p>

<asp:HiddenField ID="hdn_quote_id" runat="server" />
<asp:HiddenField ID="hdn_rev" runat="server" />
<asp:HiddenField ID="hdn_cid" runat="server" />
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	
	
	SelectCommand="Select * from quote_process_question_history inner join quote_process_questions on quote_process_question_history.question_id = quote_process_questions.id where quoteid = ?qid and quote_process_questions.type = 'Customer'">
	<SelectParameters>
		<asp:ControlParameter ControlID="hdn_quote_id" Name="?qid" 
			PropertyName="Value" />
	</SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	
	
	SelectCommand="Select * from quote_process_question_history inner join quote_process_questions on quote_process_question_history.question_id = quote_process_questions.id where quoteid = ?qid  and quote_process_questions.type = 'Market'">
	<SelectParameters>
		<asp:ControlParameter ControlID="hdn_quote_id" Name="?qid" 
			PropertyName="Value" />
	</SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	
	
	SelectCommand="Select * from quote_process_question_history inner join quote_process_questions on quote_process_question_history.question_id = quote_process_questions.id where quoteid = ?qid  and quote_process_questions.type = 'Manpower'">
	<SelectParameters>
		<asp:ControlParameter ControlID="hdn_quote_id" Name="?qid" 
			PropertyName="Value" />
	</SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	
	
	SelectCommand="Select * from quote_process_question_history inner join quote_process_questions on quote_process_question_history.question_id = quote_process_questions.id where quoteid = ?qid and quote_process_questions.type = 'Finance'">
	<SelectParameters>
		<asp:ControlParameter ControlID="hdn_quote_id" Name="?qid" 
			PropertyName="Value" />
	</SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="sql_field_members" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	
	
	SelectCommand="Select a.member_id memberid,CONCAT(b.ddl_name, ' - ', a.member_fullname) member_name from member a LEFT join business_unit b on a.business_unit_id = b.id where a.member_status = 'Active' and a.Member_MemberType_ID in (4,5,11,12,17,23,24,25,29,33,34,39,38,49,53,52,55) and a.business_unit_id = ?cid UNION select 0 memberid,'Not Assigned' member_fullname order by member_name">
	<SelectParameters>
		<asp:ControlParameter ControlID="hdn_cid" Name="cid" PropertyName="Value" />
	</SelectParameters>
</asp:SqlDataSource>
<asp:sqldatasource id="sql_all_members" runat="server"
	connectionstring="<%$ ConnectionStrings:MySQLdotnet %>"
	providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
	selectcommand="Select a.member_id memberid,CONCAT(b.ddl_name, ' - ', a.member_fullname) member_name from member a LEFT join business_unit b on a.business_unit_id = b.id where a.member_status = 'Active' and a.Member_MemberType_ID in (2,4,5,8,9,11,12,17,21,24,25,27,29,33,34,35,37,38,39,41,46,49,52,54,55,56,59,60)  UNION select 0 memberid,'Not Assigned' member_fullname order by member_name"></asp:sqldatasource>
<asp:sqldatasource id="sql_sales_members" runat="server"
	connectionstring="<%$ ConnectionStrings:MySQLdotnet %>"
	providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
	selectcommand="Select a.member_id memberid,CONCAT(b.ddl_name, ' - ', a.member_fullname) member_name from member a LEFT join business_unit b on a.business_unit_id = b.id where a.member_status = 'Active' and a.Member_MemberType_ID in (8,5,4,27,37,38,41,49,52,54,55,56,59,60)  UNION select 0 memberid,'Not Assigned' member_fullname order by member_name"></asp:sqldatasource>
<asp:sqldatasource id="sql_branchmanagers" runat="server"
	connectionstring="<%$ ConnectionStrings:MySQLdotnet %>"
	providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
	selectcommand="Select a.member_id memberid,CONCAT(b.ddl_name, ' - ', a.member_fullname) member_name from member a LEFT join business_unit b on a.business_unit_id = b.id where a.member_status = 'Active' and a.Member_MemberType_ID in (5,11,39,29,52,36)  UNION select 0 memberid,'Not Assigned' member_fullname order by member_name"></asp:sqldatasource>
<asp:SqlDataSource ID="sql_finance_members" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	
	SelectCommand="SELECT
          a.member_id memberid,
          CONCAT(b.ddl_name, ' - ', a.member_fullname) member_name,
		  a.member_membertype_id 
		  from member a LEFT join business_unit b on a.business_unit_id = b.id 
WHERE
          a.member_status = 'Active'
AND a.Member_MemberType_ID IN (58, 56, 51, 50, 47, 45, 36, 30,44,48,7,67)
UNION
          SELECT
                    0 memberid,
                    'Not Assigned' member_fullname,
0 m
          ORDER BY
                    member_name">
</asp:SqlDataSource>

