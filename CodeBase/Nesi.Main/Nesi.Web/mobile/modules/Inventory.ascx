<%@ Control Language="C#" AutoEventWireup="true" Inherits="mobile_modules_inventory" Codebehind="inventory.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1' name='viewport' />

<style type="text/css">
	.style1
	{
		background-color: #FF0000;
		
	}
	   .dxgvPreviewRow td {
        padding-top:0px!important;
        padding-bottom:4px!important;
        padding-right:0px!important;
        padding-left:10px!important;
        }
        
	.style2
	{
		font-family: Arial, Helvetica, sans-serif;
	}
        
</style>
<script type='text/javascript' src='/js/jquery.form.js'></script>
<script type='text/javascript' src='/js/inventory/_branch.js'></script>
<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
		<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
		<script type="text/javascript" src="/js/jquery.spellcheck.js"></script>
		<script type="text/javascript" src="/js/jquery.autocomplete.js"></script>
		<script type="text/javascript" src="/js/jquery.tablesorter.min.js"></script>
		<script type="text/javascript" src="/js/jquery.tip.js"></script>
		<script type="text/javascript" src="/js/jquery.inventory_new.js"></script>
		<script type="text/javascript" lang="en-us" src="/js/functions.js"></script>

<asp:UpdatePanel ID="up" runat="server">
	<ContentTemplate>
		<table style="width: 100%;">
			<tr>
				<td nowrap="nowrap" style="vertical-align: middle" valign="middle">
					<span class="style2">Search</span><input type="text" id="partsearch" name="partsearch" onfocus="$(this).inventory(
																						{
																						force_clickable: true,
																						click: function(row)
																								{
																							location.href		= './index.aspx?a=inventory&id='+row.master_id;
																								}
																						});" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:TextBox ID="txtbc" runat="server" AutoPostBack="True" CssClass="style1" 
						Font-Size="10pt" Height="30px" ontextchanged="txtbc_TextChanged"></asp:TextBox>
					<asp:HiddenField ID="hdnmasterid" runat="server" />
				</td>
			</tr>
			<tr>
				<td>
					<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
						Width="300px">
						<tabpages>
							<dx:TabPage Name="General" Text="General">
								<contentcollection>
									<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
										<table style="width: 100%;">
											<tr>
												<td style="white-space: nowrap; font-weight: bold; font-family: Arial;">
													Master ID:</td>
												<td style="font-family: Arial" width="100%">
													<asp:Label ID="lblmasterid" runat="server" Text="Label" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td colspan="2">
													<asp:Label ID="lbldesc" runat="server" CssClass="style2" Text="Label" 
														Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td style="font-family: Arial; font-weight: bold;">
													In Stock:</td>
												<td style="font-family: Arial" ID="lbladdress">
													<asp:Label ID="lblstock" runat="server" Text="Label"></asp:Label>
												</td>
											</tr>
											<tr>
												<td style="font-family: Arial; font-weight: bold;" nowrap="nowrap">
													My Location:</td>
												<td style="font-family: Arial" ID="lblcontact">
													<asp:Label ID="lbllocation" runat="server" Text="Label"></asp:Label>
												</td>
											</tr>
										</table>
									</dx:ContentControl>
								</contentcollection>
							</dx:TabPage>
						</tabpages>
					</dx:ASPxPageControl>
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;</td>
			</tr>
		</table>
	</ContentTemplate>
</asp:UpdatePanel>