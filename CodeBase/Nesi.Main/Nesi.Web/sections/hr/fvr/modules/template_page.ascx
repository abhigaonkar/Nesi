<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_fvr_modules_template_page" Codebehind="template_page.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<table cellpadding="2" cellspacing="0" style="border:solid 1px #aaa; margin:3px; width: 565px;background-color:<%= this.bgColor %>" class="template_page" data-fclientid="<%= list_files.ClientID %>">
	<tr>
		<td width="50" align="center">
			<input type="checkbox" runat="server" id="chk_enablepage"  />
		</td>
		<td width="500" align="left">
			<input type="hidden" id="hid_pageid" runat="server" />
			<input type="hidden" id="hid_id" runat="server" />
			<a href="javascript:void(0);" id="lnk_pagename" runat="server"><b><%= this.PageName %></b></a>
		</td>
	</tr>
	<tr>
		<td colspan="2" align="left">
			<div id="div_list_files" runat="server" class="div_list_files" style="display:none;">
				<dx:ASPxListBox id="list_files" runat="server" ValueType="System.Int32" Width="550px" Native="true" Font-Size="12px"></dx:ASPxListBox>
			</div>
			<div id="div_url" runat="server" class="div_url" style="display:none;margin-top:5px;">
				<dx:ASPxTextBox ID="tb_url" runat="server" NullText="OR enter a url for a page" Width="100%"></dx:ASPxTextBox>
			</div>
			<div id="div_file_buttons" runat="server" class="div_file_buttons" style="display:none">
				<table>
					<tr>
						<td><dx:ASPxButton ID="preview_file" runat="server" Text="Preview Selected File" AutoPostBack="false"><ClientSideEvents Click="fvr_template.preview_file" /></dx:ASPxButton></td>
					</tr>
				</table>
			</div>
			<div id="div_isrequired" runat="server" class="div_isrequired" style="display:none;">
				<asp:CheckBox ID="chk_isrequired" runat="server" Text="Needs to upload this file back?:" TextAlign="Left" />
			</div>
		</td>
	</tr>
</table>