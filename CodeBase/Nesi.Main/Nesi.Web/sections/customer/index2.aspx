<%@ Page	Language		="C#" 
			AutoEventWireup	="true" 
			Async			="true" 
			Theme			="" 
			Inherits		="sections_customer_index2" Codebehind="index2.aspx.cs" %>


<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>













<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<script type="text/javascript">
		var ts_start = new Date().getTime();
	</script>
	<title>Customer</title>
	<style type="text/css">
		.style1
		{
			height: 20px;
		}
		.style2
		{
			height: 29px;
		}
		.style3
		{
			height: 20px;
			font-size: xx-small;
		}
		.style4
		{
			font-size: xx-small;
		}
		.style5
		{
			font-size: x-small;
		}
	</style>
	</head>
	<body>
	
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui.js"></script>
	<script type="text/javascript" src="/js/jquery.tip.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" src="/js/customer.js"></script>
		<form id="form1" runat="server">
		<table style="width:100%;">
			<tr>
				<td class="style2" colspan="5" width="100%">
					<dx:ASPxMenu ID="ASPxMenu1" runat="server" BackColor="#CCCCCC" 
						AnimationType="None" HorizontalAlign="Left" ItemAutoWidth="False" 
						ShowSubMenuShadow="False" Width="100%">
						<Items>
							<dx:MenuItem Text="File">
								<Items>
									<dx:MenuItem Text="New">
										<Items>
											<dx:MenuItem Text="Address">
											</dx:MenuItem>
											<dx:MenuItem Text="Customer">
											</dx:MenuItem>
											<dx:MenuItem Text="Contact">
											</dx:MenuItem>
										</Items>
									</dx:MenuItem>
								</Items>
							</dx:MenuItem>
							<dx:MenuItem Text="Reports">
							</dx:MenuItem>
						</Items>
						<ItemStyle>
						<HoverStyle BackColor="White">
						</HoverStyle>
						</ItemStyle>
						<Border BorderStyle="None" />
					</dx:ASPxMenu>
				</td>
			</tr>
			<tr>
				<td>
					<dx:ASPxComboBox ID="ddl_top_selector" runat="server" AnimationType="None" 
						AutoPostBack="True" ClientVisible="False" DropDownRows="100" 
						onselectedindexchanged="ddl_top_selector_SelectedIndexChanged" TextField="name" 
						ValueField="id" ValueType="System.String" Width="250px">
					</dx:ASPxComboBox>
				</td>
				<td>
					&nbsp;</td>
				<td align="right" style="text-align: right" width="100%">
					<dx:ASPxPager ID="pg" runat="server" ItemsPerPage="1" 
						onpageindexchanged="pg_PageIndexChanged">
					</dx:ASPxPager>
				</td>
				<td>
					<dx:ASPxComboBox ID="ddlsearchtype" runat="server" BackColor="#FFFFCC" 
						Height="25px" SelectedIndex="1" Width="100px" ClientInstanceName="ddlsearchtype">
						<Items>
							<dx:ListEditItem Text="Contact" Value="Contact" />
							<dx:ListEditItem Selected="True" Text="Customer" Value="Customer" />
							<dx:ListEditItem Text="Address" Value="Address" />
						</Items>
					</dx:ASPxComboBox>
				</td>
				<td>
					<dx:ASPxButtonEdit ID="btneditSearch" runat="server" BackColor="#FFFFCC" 
						Height="25px" NullText="Search for..." style="text-align: right" Width="200px" 
						onbuttonclick="btneditSearch_ButtonClick">
						<Buttons>
							<dx:EditButton>
								<Image Url="~/images/icon/icon[search].gif">
								</Image>
							</dx:EditButton>
						</Buttons>
					</dx:ASPxButtonEdit>
				</td>
			</tr>
		</table>
		<dx:ASPxSplitter ID="ASPxSplitter1" runat="server" Font-Names="Arial" 
			Font-Size="10pt" Height="1000px" Orientation="Vertical">
			<panes>
				<dx:SplitterPane MinSize="100px" Name="General" ScrollBars="Auto" 
					Size="380px">
					<PaneStyle BackColor="#EAEAEA">
					</PaneStyle>
					<ContentCollection>
<dx:SplitterContentControl runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
		OnCallback="cb_Callback" Width="100%">
		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<table style="width: 100%; font-family: Arial, Helvetica, sans-serif;  border-collapse: collapse;" 
					cellpadding="2px">
					<tr>
						<td nowrap="nowrap" style="padding-left: 5px">
							Contact:</td>
						<td style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcontact_name" runat="server" 
								ClientInstanceName="txtcontact_name" Width="100px">
							</dx:ASPxTextBox>
						</td>
						<td style="padding-left: 4px" bgcolor="#CCCCCC">
							Customer:</td>
						<td bgcolor="#CCCCCC" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcustname" runat="server" 
								ClientInstanceName="txtcustname" Width="120px">
							</dx:ASPxTextBox>
						</td>
						<td bgcolor="#999999" style="padding-right: 10px; padding-left: 5px;">
							Address:</td>
						<td bgcolor="#999999" style="padding-right: 10px">
							<dx:ASPxComboBox ID="ddladdress" runat="server" Width="120px">
								<Buttons>
									<dx:EditButton>
										<Image Url="~/images/icon/icon[add].gif">
										</Image>
									</dx:EditButton>
								</Buttons>
							</dx:ASPxComboBox>
						</td>
						<td>
							&nbsp;</td>
						<td align="left" rowspan="12" 
							style="padding: 0px; text-align: left;" valign="top">
							<table bgcolor="#CCFFCC" cellpadding="4px" 
								
								style="border-collapse: collapse; font-family: Arial, Helvetica, sans-serif; font-size: 9pt; ">
								<tr>
									<td>
										Last Reach:</td>
									<td>
										<dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Width="150px">
										</dx:ASPxTextBox>
									</td>
									<td align="center" colspan="2" style="text-align: center">
										<dx:ASPxButton ID="ASPxButton2" runat="server" Font-Bold="True" 
											Font-Names="Arial" Font-Size="12pt" Text="Manual Update" Width="150px">
										</dx:ASPxButton>
									</td>
								</tr>
								<tr>
									<td>
										Last Edited:</td>
									<td>
										<dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ReadOnly="True" Width="150px">
										</dx:ASPxTextBox>
									</td>
									<td>
										&nbsp;By:</td>
									<td>
										<dx:ASPxTextBox ID="txtlastedited" runat="server" ReadOnly="True" Width="120px">
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										Last Work Order Date:</td>
									<td>
										<dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Width="150px">
										</dx:ASPxTextBox>
									</td>
									<td nowrap="nowrap">
										&nbsp;</td>
									<td>
										<dx:ASPxHyperLink ID="hl_lastwo" runat="server" Text="Unknown" 
											Cursor="pointer">
										</dx:ASPxHyperLink>
									</td>
								</tr>
								<tr>
									<td nowrap="nowrap">
										Quotes Waiting:</td>
									<td>
										<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text="Unknown" 
											Cursor="pointer">
										</dx:ASPxHyperLink>
									</td>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
								</tr>
								<tr>
									<td>
										Contact Frequency:</td>
									<td>
										<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="110px">
										</dx:ASPxTextBox>
									</td>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
								</tr>
								<tr>
									<td>
										Next Follow Up Date:</td>
									<td>
										<dx:ASPxDateEdit ID="ASPxDateEdit5" runat="server" 
											DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
											EditFormatString="yyyy-MM-dd" Width="110px">
										</dx:ASPxDateEdit>
									</td>
									<td align="center" colspan="2" style="text-align: center">
										<dx:ASPxButton ID="ASPxButton3" runat="server" Font-Bold="True" 
											Font-Names="Arial" Font-Size="12pt" HorizontalAlign="Center" Text="Schedule" 
											Width="150px">
											<Image Url="~/images/icon/icon[add].gif">
											</Image>
										</dx:ASPxButton>
									</td>
								</tr>
								<tr>
									<td valign="top">
										Notes for Next Followup:</td>
									<td colspan="3">
										<table cellpadding="0" cellspacing="0" style="width:100%;">
											<tr>
												<td style="padding-bottom: 3px">
													<dx:ASPxButtonEdit ID="ASPxButtonEdit2" runat="server" Width="100%" 
														NullText="Enter New Note Here....">
														<Buttons>
															<dx:EditButton Text="Add" Width="25px">
																<Image Url="~/images/icon/icon[add].gif">
																</Image>
															</dx:EditButton>
														</Buttons>
														<ButtonStyle Width="25px">
														</ButtonStyle>
													</dx:ASPxButtonEdit>
												</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxMemo ID="ASPxMemo1" runat="server" BackColor="#FFFFCC" Height="71px" 
														Width="100%">
													</dx:ASPxMemo>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
						<td align="left" nowrap="nowrap" rowspan="12" 
							style="padding: 0px; text-align: left;" valign="top" width="100%">
							&nbsp;</td>
					</tr>
					<tr>
						<td nowrap="nowrap" style="padding-left: 5px">
							ID:</td>
						<td style="padding-right: 10px">
							<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ASPxLabel">
							</dx:ASPxLabel>
						</td>
						<td style="padding-left: 4px" bgcolor="#CCCCCC">
							ID:</td>
						<td bgcolor="#CCCCCC" style="padding-right: 10px" >
							<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="ASPxLabel">
							</dx:ASPxLabel>
						</td>
						<td bgcolor="#999999" style="padding-right: 10px; padding-left: 5px;">
							Address 1:</td>
						<td bgcolor="#999999" style="padding-right: 10px">
							<dx:ASPxTextBox ID="ASPxTextBox6" runat="server" Width="120px">
							</dx:ASPxTextBox>
						</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style1" nowrap="nowrap" style="padding-left: 5px">
							Title:</td>
						<td class="style1" style="padding-right: 10px">
							<asp:SqlDataSource ID="sqltitles" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								SelectCommand="Select * from titles"></asp:SqlDataSource>
							<dx:ASPxComboBox ID="ddltitle" runat="server" DataSourceID="sqltitles" 
								TextField="title_name" ValueField="title_id" ValueType="System.Int32" 
								Width="100px">
							</dx:ASPxComboBox>
						</td>
						<td class="style1" style="padding-left: 4px" bgcolor="#CCCCCC">
							Branch:</td>
						<td class="style1" bgcolor="#CCCCCC" style="padding-right: 10px" >
							<dx:ASPxComboBox ID="ASPxComboBox1" runat="server" Width="120px">
							</dx:ASPxComboBox>
						</td>
						<td class="style1" style="padding-left: 5px; padding-right: 10px;" 
							bgcolor="#999999">
							City:</td>
						<td bgcolor="#999999" class="style1" style="padding-right: 10px">
							<dx:ASPxTextBox ID="ASPxTextBox5" runat="server" Width="120px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1">
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style1" nowrap="nowrap" style="padding-left: 5px">
							Email:</td>
						<td class="style1" style="padding-right: 10px">
							<dx:ASPxHyperLink ID="hl_email" runat="server" Cursor="pointer" Text="Unknown">
							</dx:ASPxHyperLink>
						</td>
						<td class="style1" style="padding-left: 4px" bgcolor="#CCCCCC">
							On Hold?</td>
						<td class="style1" bgcolor="#CCCCCC" style="padding-right: 10px">
							<dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" 
								Text=" ">
							</dx:ASPxCheckBox>
						</td>
						<td class="style1" bgcolor="#999999" 
							style="padding-right: 10px; padding-left: 5px;">
							State/Prov:</td>
						<td bgcolor="#999999" class="style1" style="padding-right: 10px">
							<dx:ASPxComboBox ID="ASPxComboBox8" runat="server" Width="120px">
							</dx:ASPxComboBox>
						</td>
						<td class="style1">
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style1" nowrap="nowrap" style="padding-left: 5px">
							Phone:</td>
						<td class="style1" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcontactphone" runat="server" Width="100px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1" style="padding-left: 4px" bgcolor="#CCCCCC">
							&nbsp;</td>
						<td class="style1" bgcolor="#CCCCCC" style="padding-right: 10px">
							&nbsp;</td>
						<td class="style1" bgcolor="#999999" 
							style="padding-right: 10px; padding-left: 5px;">
							Phone:</td>
						<td bgcolor="#999999" class="style1" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcustphone" runat="server" Width="120px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1">
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style1" nowrap="nowrap" style="padding-left: 5px">
							Cell:</td>
						<td class="style1" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcontactcell" runat="server" Width="100px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1" style="padding-left: 4px" bgcolor="#CCCCCC">
							&nbsp;</td>
						<td class="style1" bgcolor="#CCCCCC" style="padding-right: 10px" >
							&nbsp;</td>
						<td class="style1" bgcolor="#999999" 
							style="padding-right: 10px; padding-left: 5px;">
							Website:</td>
						<td bgcolor="#999999" class="style1" style="padding-right: 10px">
							<dx:ASPxHyperLink ID="hl_website" runat="server" Cursor="pointer" 
								OnInit="hl_website_Init">
							</dx:ASPxHyperLink>
						</td>
						<td class="style1">
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style1" nowrap="nowrap" style="padding-left: 5px">
							Facebook:</td>
						<td class="style1" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcustname9" runat="server" Width="100px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1" style="padding-left: 4px" bgcolor="#CCCCCC">
							Facebook:</td>
						<td class="style1" bgcolor="#CCCCCC" style="padding-right: 10px" >
							<dx:ASPxTextBox ID="txtcustfacebook" runat="server" Width="120px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1" bgcolor="#999999" 
							style="padding-right: 10px; padding-left: 5px;">
							Facebook:</td>
						<td bgcolor="#999999" class="style1" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcustfacebook0" runat="server" Width="120px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1">
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style1" nowrap="nowrap" style="padding-left: 5px">
							Twitter:</td>
						<td class="style1" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcustname8" runat="server" Width="100px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1" style="padding-left: 4px" bgcolor="#CCCCCC">
							Twitter:</td>
						<td class="style1" bgcolor="#CCCCCC" style="padding-right: 10px" >
							<dx:ASPxTextBox ID="txtcusttwitter" runat="server" Width="120px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1" bgcolor="#999999" 
							style="padding-right: 10px; padding-left: 5px;">
							Twitter:</td>
						<td bgcolor="#999999" class="style1" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcusttwitter0" runat="server" Width="120px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1">
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style1" nowrap="nowrap" style="padding-left: 5px">
							LinkedIn:</td>
						<td class="style1" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcustname7" runat="server" Width="100px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1" style="padding-left: 4px" bgcolor="#CCCCCC">
							LinkedIn:</td>
						<td class="style1" bgcolor="#CCCCCC" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcustlinkin" runat="server" Width="120px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1" bgcolor="#999999" 
							style="padding-right: 10px; padding-left: 5px;">
							LinkedIn:</td>
						<td bgcolor="#999999" class="style1" style="padding-right: 10px">
							<dx:ASPxTextBox ID="txtcustlinkin0" runat="server" Width="120px">
							</dx:ASPxTextBox>
						</td>
						<td class="style1">
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style1" nowrap="nowrap" style="padding-left: 5px">
							Status:</td>
						<td class="style1" style="padding-right: 10px">
							<dx:ASPxComboBox ID="ASPxComboBox3" runat="server" Width="100px">
							</dx:ASPxComboBox>
						</td>
						<td bgcolor="#CCCCCC" class="style1" style="padding-left: 4px">
							Status:</td>
						<td bgcolor="#CCCCCC" class="style1" style="padding-right: 10px">
							<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" Width="120px">
							</dx:ASPxComboBox>
						</td>
						<td bgcolor="#999999" class="style1" 
							style="padding-right: 10px; padding-left: 5px;">
							&nbsp;</td>
						<td bgcolor="#999999" class="style1" style="padding-right: 10px">
							&nbsp;</td>
						<td class="style1">
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style3" nowrap="nowrap" style="padding-left: 5px">
							Branch Contact:</td>
						<td class="style1" style="padding-right: 10px">
							<dx:ASPxComboBox ID="ASPxComboBox5" runat="server" Width="100px">
							</dx:ASPxComboBox>
						</td>
						<td bgcolor="#CCCCCC" class="style3" nowrap="nowrap" style="padding-left: 4px">
							Branch Contact:</td>
						<td bgcolor="#CCCCCC" class="style1" style="padding-right: 10px">
							<dx:ASPxComboBox ID="ASPxComboBox4" runat="server" Width="120px">
							</dx:ASPxComboBox>
						</td>
						<td bgcolor="#999999" class="style3" nowrap="nowrap" 
							style="padding-right: 10px; padding-left: 5px;">
							Branch Contact:</td>
						<td bgcolor="#999999" class="style1" style="padding-right: 10px">
							<dx:ASPxComboBox ID="ASPxComboBox9" runat="server" Width="120px">
							</dx:ASPxComboBox>
						</td>
						<td class="style1">
							&nbsp;</td>
					</tr>
					<tr>
						<td class="style3" nowrap="nowrap" style="padding-left: 5px">
							Sales Contact:</td>
						<td class="style1" style="padding-right: 10px">
							<dx:ASPxComboBox ID="ASPxComboBox7" runat="server" Width="100px">
							</dx:ASPxComboBox>
						</td>
						<td bgcolor="#CCCCCC" class="style3" nowrap="nowrap" style="padding-left: 4px">
							Sales Contact:</td>
						<td bgcolor="#CCCCCC" class="style1" style="padding-right: 10px">
							<dx:ASPxComboBox ID="ASPxComboBox6" runat="server" Width="120px">
							</dx:ASPxComboBox>
						</td>
						<td bgcolor="#999999" class="style3" 
							style="padding-right: 10px; padding-left: 5px;">
							Sales Contact:</td>
						<td bgcolor="#999999" class="style1" style="padding-right: 10px">
							<dx:ASPxComboBox ID="ASPxComboBox10" runat="server" Width="120px">
							</dx:ASPxComboBox>
						</td>
						<td class="style1">
							&nbsp;</td>
					</tr>
				</table>
				<br />
				<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Save">
					<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('s');
}" />
				</dx:ASPxButton>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
						</dx:SplitterContentControl>
</ContentCollection>
				</dx:SplitterPane>
				<dx:SplitterPane Name="Details">
					<ContentCollection>
<dx:SplitterContentControl runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
		Height="300px" style="margin-top: 8px" Width="100%">
		<TabPages>
			<dx:TabPage Name="Notes" Text="Notes">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Name="Accounting" Text="Accounting">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Name="History" Text="History">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Name="Contacts" Text="Contacts">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Attn Required">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Opportunities">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
		<ActiveTabStyle BackColor="#3399FF" ForeColor="White">
		</ActiveTabStyle>
		<TabStyle Font-Bold="False" Font-Names="Arial" Font-Size="10pt" Height="20px" 
			Width="100px">
		</TabStyle>
	</dx:ASPxPageControl>
						</dx:SplitterContentControl>
</ContentCollection>
				</dx:SplitterPane>
			</panes>
			<Border BorderStyle="None" />
		</dx:ASPxSplitter>
		<br />
		<br />
		</form>
	
	</body>
	</html>
