<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_huddle_modules_new" EnableTheming="True" Codebehind="new.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<dx:ASPxCallbackPanel runat="server" Width="100%" ClientInstanceName="cbp_new" ID="cbp_new" oncallback="cbp_new_Callback">
	<ClientSideEvents EndCallback="huddle.new_huddle.end_callback" />
	<PanelCollection>
		<dx:PanelContent>
		<div style="padding:2px;">
			<dx:ASPxDateEdit ID="date_of_huddle" Caption="Date of Huddle" Width="200px" 
				ClientInstanceName="new_date_of_huddle" runat="server" Theme="NETheme01" 
				DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" 
				>
				<TimeSectionProperties Visible="True">
				</TimeSectionProperties>
				<CaptionCellStyle Width="100px">
				</CaptionCellStyle>
			</dx:ASPxDateEdit>
			</div>
			<div style="padding:2px;">
			<dx:ASPxTextBox ID="new_name" runat="server" Caption="Name" Width="200px" 
					ClientInstanceName="new_name" Theme="NETheme01">
				<CaptionCellStyle Width="100px">
				</CaptionCellStyle>
			</dx:ASPxTextBox>
			</div>
			<div style="padding:2px;">
			<dx:ASPxMemo ID="new_description" runat="server" Caption="Description" 
					Height="71px" Width="200px" ClientInstanceName="new_description" 
					Theme="NETheme01">
				<CaptionCellStyle Width="100px">
				</CaptionCellStyle>
			</dx:ASPxMemo>
			</div>
			<div style="padding:2px;">
			<dx:ASPxComboBox ID="new_captain" runat="server" Caption="Captain" 
					ValueType="System.Int32" Width="200px" ClientInstanceName="new_captain" 
					DataSourceID="sds_users" TextField="name" ValueField="id" Theme="NETheme01">
				<CaptionCellStyle Width="100px">
				</CaptionCellStyle>
			</dx:ASPxComboBox>
			</div>
			<br />
			<div align="center">
			<dx:ASPxButton ID="new_cancel" runat="server" Text="Cancel" Width="100px" 
					AutoPostBack="False" ClientInstanceName="new_cancel" Theme="NETheme01">
				<ClientSideEvents Click="huddle.new_huddle.cancel" />
			</dx:ASPxButton>
			&nbsp;<dx:ASPxButton ID="new_save" runat="server" Text="Save" Width="100px" 
					AutoPostBack="False" ClientInstanceName="new_save" Theme="NETheme01">
				<ClientSideEvents Click="huddle.new_huddle.save" />
			</dx:ASPxButton>
			&nbsp;<dx:ASPxButton ID="new_saveclose" runat="server" Text="Save & Close" 
					Width="100px" AutoPostBack="False" ClientInstanceName="new_saveclose" 
					Theme="NETheme01">
				<ClientSideEvents Click="huddle.new_huddle.saveclose" />
			</dx:ASPxButton></div>

			<asp:SqlDataSource ID="sds_users" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="select a.member_id id, CONCAT(b.name, ' - ', member_fullname) name FROM member a LEFT join business_unit b ON a.business_unit_id = b.id WHERE member_status = 'Active' ORDER BY b.business_unit_id, a.member_lastname, member_nickname"></asp:SqlDataSource>
		</dx:PanelContent>
	</PanelCollection>
</dx:ASPxCallbackPanel>

