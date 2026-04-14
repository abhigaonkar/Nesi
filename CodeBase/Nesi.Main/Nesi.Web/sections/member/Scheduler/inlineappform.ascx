<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_inlineappform" Codebehind="inlineappform.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

	<div id="dc" class="dc" runat="server">
		<div class="wo remove_wrap">
			<span ID="lblwo" runat="server"></span>
			<asp:Image ID="Image1" runat="server" Height="15px" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_gohome.png" />
		</div>
		<div class="sub remove_wrap">
			<span ID="lblsubject" runat="server"></span>
		</div>
		<div class="hours remove_wrap">
			<span ID="lbltruck" runat="server"></span> 
			<span ID="lblhours" runat="server"></span>
		</div>
	</div>