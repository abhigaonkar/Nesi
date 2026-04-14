<%@ Control Language="C#" AutoEventWireup="true"  Inherits="modules_auto_bingo" EnableTheming="True" Codebehind="auto_bingo.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

        	<dx:ASPxGridView ID="gv_watch" runat="server"  OnHtmlDataCellPrepared="gv_watch_HtmlDataCellPrepared"
	AutoGenerateColumns="False" KeyFieldName="watch"  ClientInstanceName="gv_watch"
	 Width="100%">
				<Columns>
					<dx:GridViewDataTextColumn Caption="Watch" FieldName="watch" Name="watch" 
						ShowInCustomizationForm="True" VisibleIndex="0">
						<HeaderStyle HorizontalAlign="Left" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Count" FieldName="count" Name="count" 
						ShowInCustomizationForm="True" VisibleIndex="1" Width="100px">
						<HeaderStyle HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Time (ms) Taken" FieldName="time" 
						Name="time" ShowInCustomizationForm="True" VisibleIndex="2" Width="100px">
						<HeaderStyle HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
				</Columns>
				<SettingsBehavior AllowSort="False" />
				<SettingsPager PageSize="50">
				</SettingsPager>
				<Settings ShowTitlePanel="True" />
                <SettingsText Title="Auto Bingo Report" />
</dx:ASPxGridView>

<dx:ASPxTimer ID="autoBingoTimer" runat="server" Interval="300000" 
              ClientSideEvents-Tick='function (s,e){gv_watch.PerformCallback("refresh");}' 
              ClientInstanceName="timer" Enabled="True">
</dx:ASPxTimer>

            

