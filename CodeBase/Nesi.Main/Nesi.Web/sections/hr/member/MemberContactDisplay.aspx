<%@ Page Language="C#" AutoEventWireup="true" Inherits="MemberContactDisplay" Title="Member Contact Information" Theme="" Codebehind="MemberContactDisplay.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Src="modules/fvr_listing.ascx" TagName="fvr_listing" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Employee Contact Infomation</title>
	<base target="_self" />
	<link href="~/App_Themes/BlueStyle/common.css" rel="stylesheet" type="text/css" />
	<meta http-equiv="PRAGMA" content="NO-CACHE" />
	<style type="text/css">
		.t { font-size: 11px; font-family: arial; }
		.l { font-weight: bold; width: 175px; }
		.r { }
		
		.input { font-size: 12px; font-family: arial; }
	</style>
</head>
<body>
	<form id="form1" runat="server">
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript">
		function preview(id)
		{
			if (id != null)
			{
				boing("/_tools/get_file/index.aspx?file_id=" + id + "&iframe=true", preview, 768, 480);
			}
			else
			{
				alert("Please select a file");
			}
}

	</script>
	<asp:ScriptManager ID="sm" runat="server">
	</asp:ScriptManager>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
			<dx:ASPxPageControl ID="pc" runat="server" ActiveTabIndex="2" 
				Font-Names="Arial">
				<TabPages>
					<dx:TabPage Name="member_info" Text="Info">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
								<table cellpadding="3" cellspacing="0">
									<tr id="row_first_name" runat="server">
										<td class="l">First Name</td>
										<td>
											<asp:Label runat="server" Text="No First Name" ID="lblFirstName"></asp:Label>
										</td>
									</tr>
									<tr id="row_last_name" runat="server">
										<td class="l">Last Name</td>
										<td>
											<asp:Label runat="server" Text="No Last Name" ID="lblLastName"></asp:Label>
										</td>
									</tr>
									<tr id="row_branch" runat="server">
										<td class="l">Business Unit</td>
										<td>
											<asp:Label runat="server" Text="No Branch" ID="lblOffice"></asp:Label>
										</td>
									</tr>
									<tr id="row_position" runat="server">
										<td class="l">Title</td>
										<td>
											<asp:Label runat="server" Text="No Position" ID="lblPosition"></asp:Label>
										</td>
									</tr>
									<tr id="row_ne_phone_number" runat="server">
										<td class="l">Company Cell Phone</td>
										<td>
											<asp:Label runat="server" Text="No NE Cell Phone" ID="lblPhone"></asp:Label>
										</td>
									</tr>
									<tr id="row_ne_email" runat="server">
										<td class="l">Company Email</td>
										<td>
											<asp:Label runat="server" Text="No Company Email" ID="lblEmail"></asp:Label>
										</td>
									</tr>
									<tr id="row_ne_phone_ext" runat="server">
										<td class="l">Company Phone Extension</td>
										<td>
											<asp:Label runat="server" Text="No NE Phone Extension" ID="lblExtension"></asp:Label>
										</td>
									</tr>
									<tr id="row_country" runat="server">
										<td class="l">Country</td>
										<td>
											<asp:Label runat="server" Text="No Country" ID="lblCountry"></asp:Label>
										</td>
									</tr>
									<tr id="row_home_phone" runat="server">
										<td class="l">Home Phone</td>
										<td>
											<table cellpadding="2" cellspacing="0">
												<tr>
													<td>(</td>
													<td><asp:TextBox runat="server" MaxLength="3" Width="20px" CssClass="input" ID="txtHomePhoneArea" autocomplete="off"></asp:TextBox></td>
													<td>) </td>
													<td><asp:TextBox runat="server" MaxLength="3" Width="20px" CssClass="input" ID="txtHomePhoneFirst" autocomplete="off"></asp:TextBox></td>
													<td>-</td>
													<td><asp:TextBox runat="server" MaxLength="4" Width="30px" CssClass="input" ID="txtHomePhoneLast" autocomplete="off"></asp:TextBox></td>
												</tr>
											</table>
										</td>
									</tr>
									<tr id="row_address" runat="server">
										<td class="l">Address</td>
										<td>
											<asp:TextBox runat="server" CssClass="input" MaxLength="200" Width="200px" ID="txtAddress" autocomplete="off"></asp:TextBox>
										</td>
									</tr>
									<tr id="row_city" runat="server">
										<td class="l">City</td>
										<td>
											<asp:TextBox runat="server" CssClass="input" MaxLength="100" ID="txtCity" autocomplete="off"></asp:TextBox>
										</td>
									</tr>
									<tr id="row_provstate" runat="server">
										<td class="l">Province</td>
										<td>
											<asp:DropDownList runat="server" DataTextField="Prov_Desc" CssClass="input" DataValueField="Prov_Abbv" Enabled="False" ID="ddlProv">
											</asp:DropDownList>
										</td>
									</tr>
									<tr id="row_postal" runat="server">
										<td class="l">Postal/ZIP Code</td>
										<td>
											<asp:TextBox runat="server" CssClass="input" MaxLength="45" ID="txtPostal" autocomplete="off"></asp:TextBox>
										</td>
									</tr>
									<tr id="row_apprentice_contract" runat="server">
										<td class="l">Apprentice Contract</td>
										<td>
											<asp:TextBox runat="server" CssClass="input" Height="50px" Width="200px" ID="txtapprenticecontract" autocomplete="off"></asp:TextBox>
										</td>
									</tr>
									<tr id="row_personal_email" runat="server">
										<td class="l">Personal Email</td>
										<td>
											<asp:TextBox runat="server" CssClass="input" MaxLength="60" ID="txtPersonalEmail" autocomplete="off"></asp:TextBox>
										</td>
									</tr>
									<tr id="row_reason" runat="server">
										<td class="l">Reason For this change?</td>
										<td>
											<asp:TextBox runat="server" CssClass="input" Height="50px" Width="200px" ID="txtReason" autocomplete="off"></asp:TextBox>
										</td>
									</tr>
								</table>
								<asp:Label ID="lblConfirm" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
								<br />
								<asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update Information" Visible="False" />
								<br />
								<asp:Label ID="lblAuditWarn" runat="server" ForeColor="Red" Text="All changes made will be recorded and an email sent to HR informing them of the change" Visible="False"></asp:Label>
								<br />
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="fvr" Text="Past FVR's">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
								<uc1:fvr_listing ID="fvr_listing1" runat="server" />
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="contact_info" Text="Info">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								<table width="100%" cellspacing="0" cellpadding="5">
									<tr>
										<td width="100">
											Name:
										</td>
										<td>
											<dx:ASPxTextBox ID="contact_name" runat="server" Width="400px" autocompletetype="Disabled">
												<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="c">
													<RequiredField ErrorText="This is a required field" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>
											Email:
										</td>
										<td>
											<dx:ASPxTextBox ID="contact_email" runat="server" Width="400px" autocompletetype="Disabled">
												<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="c">
													<RegularExpression ErrorText="Please include a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
													<RequiredField ErrorText="This is a required field" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>
											Title:
										</td>
										<td>
											<dx:ASPxTextBox ID="contact_title" runat="server" Width="400px" autocompletetype="Disabled">
												<ValidationSettings ValidationGroup="c">
												</ValidationSettings>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>
											Cell Phone:
										</td>
										<td>
											<dx:ASPxTextBox ID="contact_phone" runat="server" Width="150px" autocompletetype="Disabled">
												<ClientSideEvents GotFocus="function(s, e) {
	s.SelectAll();
}" />
												<MaskSettings Mask="999-000-0000" />
												<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="c">
													<RegularExpression ErrorText="Please provide a valid phone number" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" />
													<RequiredField ErrorText="This is a required field" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>
											Direct Phone:
										</td>
										<td>
											<dx:ASPxTextBox ID="contact_direct" runat="server" Width="150px" autocompletetype="Disabled">
												<ClientSideEvents GotFocus="function(s, e) {
	s.SelectAll();
}" />
												<MaskSettings Mask="999-000-0000" />
												<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="c">
													<RegularExpression ErrorText="Please provide a valid phone number" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" />
													<RequiredField ErrorText="This is a required field" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>
											Extension:
										</td>
										<td>
											<dx:ASPxTextBox ID="contact_extension" runat="server" Width="100px" autocompletetype="Disabled">
												<ValidationSettings ValidationGroup="c">
												</ValidationSettings>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>
											&nbsp;
										</td>
										<td>
											<dx:ASPxButton ID="bt_savecontact" runat="server" Text="Save" OnClick="bt_savecontact_Click" ValidationGroup="c">
											</dx:ASPxButton>
										</td>
									</tr>
									<tr>
										<td>
											&nbsp;
										</td>
										<td>
											<dx:ASPxLabel ID="lb_status" runat="server" Font-Bold="True">
											</dx:ASPxLabel>
										</td>
									</tr>
								</table>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
				</TabPages>
			</dx:ASPxPageControl>
		</ContentTemplate>
	</asp:UpdatePanel>
	</form>
</body>
</html>
