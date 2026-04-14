<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_fvr_modules_tax_entity_fvr_settings" Codebehind="tax_entity_fvr_settings.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Xpo.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Xpo" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<table width="100%" height="750">
	<tr>
		<td width="300" valign="top">
			<div  style="height: 750px; overflow-y: scroll;">
<dx:ASPxGridView ID=gv_taxentity runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_taxentity"  Settings-ShowColumnHeaders="False" KeyFieldName="id" Theme="NeTheme01" Width="300px" OnDetailRowExpandedChanged="gv_taxentity_DetailRowExpandedChanged">
	<ClientSideEvents RowClick="switch_focus_te" />
	<Columns>
		<dx:GridViewDataTextColumn Caption="ID" FieldName="id" Visible="False" ReadOnly="True" VisibleIndex="0" Width="25px">
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Tax Entity" FieldName="ddl_name" VisibleIndex="2">
		</dx:GridViewDataTextColumn>
	</Columns>
	<SettingsBehavior AllowFocusedRow="True" FilterRowMode="OnClick" />
	<SettingsPager PageSize="30">
	</SettingsPager>

<Settings ShowColumnHeaders="False"></Settings>

	<SettingsDetail ShowDetailRow="True" AllowOnlyOneMasterRowExpanded="True" />
	<SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
	
	<Styles>
		<DetailCell>
			<Paddings Padding="2px" />
		</DetailCell>
	</Styles>
	
	<Templates>
		<DetailRow>
			<dx:ASPxGridView ID="gv_bus" runat="server" AutoGenerateColumns="False" KeyFieldName="id" ClientInstanceName="gv_bus" Theme="NeTheme01" Settings-ShowColumnHeaders="False"><ClientSideEvents RowClick="switch_focus_bu" Init="function(s,e){s.SetFocusedRowIndex(-1)}" />
				<Columns>
					<dx:GridViewDataTextColumn FieldName="id" VisibleIndex="0" ReadOnly="True" Visible="False" UnboundType="Integer">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Unit" FieldName="ddl_name" VisibleIndex="1" UnboundType="String">
					</dx:GridViewDataTextColumn>
				</Columns>
				<SettingsBehavior AllowFocusedRow="True"  FilterRowMode="OnClick"  />
				<SettingsPager Mode="ShowAllRecords">
				</SettingsPager>
				<SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
			</dx:ASPxGridView>
		</DetailRow>
	</Templates>
	
</dx:ASPxGridView>
				</div>
			</td>
			<td valign="top">
				<dx:ASPxCallbackPanel ID="cbp_taxentity" runat="server" ClientInstanceName="cbp_taxentity" Height="400px" Width="100%" OnCallback="cbp_taxentity_OnCallback">
					<PanelCollection>
<dx:PanelContent runat="server">
					<table>
						<tr>
							<td>
	<dx:ASPxListBox ID="lb_layouts" ClientInstanceName="lb_layouts" SelectionMode="Multiple" runat="server" ValueField="id" ValueType="System.Int32" TextField="name" Caption="Default Onboarding FVR Layout" Width="350px">
		<CaptionSettings Position="Top" />
	</dx:ASPxListBox>
							</td>
						</tr>
						<tr>
							<td><br/><br/>
	Onboarding Email Template: <img src="/images/icon/icon[help].png" align="absmiddle" class="ttip" data-title="Variables" data-tooltip="[USERNAME]<br/>[PASSWORD]<br/>[REPORTSTO]<br/>[REPORTSTOCELL]<br/>[PUBLICNAME]<br/>[FIRSTNAME]</br>[LASTNAME]<br/>[FULLNAME]</br>[REPORTSTOEMAIL]<br/>[MEMBERTYPENAME]<br/>" />
							</td>
						</tr>
						<tr>
							<td>
								<dx:ASPxHtmlEditor ID="html_emaillayout" ClientInstanceName="html_emaillayout" runat="server">
								</dx:ASPxHtmlEditor><br/><br/>
	<dx:ASPxButton runat="server" ID="bt_savesettings" Text="Save Settings" AutoPostBack="False">
		<ClientSideEvents Click="buentity_savesettings" />
		</dx:ASPxButton>
							</td>
						</tr>
					</table>
						</dx:PanelContent>
</PanelCollection>
					<ClientSideEvents  EndCallback="function(s,e){$('.ttip').tip();}"></ClientSideEvents>
				</dx:ASPxCallbackPanel>
			</td>
		</tr>
	</table>


