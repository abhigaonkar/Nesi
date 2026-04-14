<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_invoiceService_runTimes" Codebehind="invoiceService_runTimes.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<dx:ASPxGridView ID="gv_invoice" runat="server" Width ="100%" 
    ClientInstanceName="gv_invoice" 
				oncustomcallback="gv_CustomCallback" >
    <Settings ShowTitlePanel="True" />
    <SettingsText Title="Invoice Service Run Times" />
    <SettingsLoadingPanel Mode="Disabled" />

</dx:ASPxGridView>

	<dx:ASPxTimer ID="invoiceServiceTimer" runat="server" Interval="60000" 
				ClientSideEvents-Tick='function (s,e){gv_invoice.PerformCallback("refresh");}' 
				ClientInstanceName="timer" Enabled="True">
	</dx:ASPxTimer>


            



