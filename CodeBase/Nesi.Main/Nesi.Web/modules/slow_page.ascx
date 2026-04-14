<%@ Control Language="C#" AutoEventWireup="true"  Inherits="modules_slow_page" EnableTheming="True" Codebehind="slow_page.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

        	<dx:ASPxGridView ID="gv_slowpages" runat="server" AutoGenerateColumns="False"  
				ClientVisible="False" KeyFieldName="url" Width="100%" style=" font-family:'Arial'; PADDING-LEFT: 5px; PADDING-RIGHT: 5px">
				<Columns>
					<dx:GridViewDataTextColumn Caption="Page Hits" FieldName="count_id" 
						VisibleIndex="1">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Page" FieldName="url" VisibleIndex="0">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Hits Over 3s" 
						FieldName="count_over_three" VisibleIndex="2">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Total AVG Time" FieldName="avg_time" 
						VisibleIndex="3">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Time Over 3s" 
						FieldName="t_over_three" VisibleIndex="4">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Total Time" FieldName="t_total" 
						VisibleIndex="5">
					</dx:GridViewDataTextColumn>
				</Columns>
				<Settings ShowTitlePanel="True" />
                <SettingsPager PageSize="17" />
				<SettingsText Title="Page load speeds (Past 7 days)" />

			</dx:ASPxGridView>
            

