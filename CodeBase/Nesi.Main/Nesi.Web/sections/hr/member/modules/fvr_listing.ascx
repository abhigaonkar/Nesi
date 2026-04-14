<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_member_modules_fvr_listing" Codebehind="fvr_listing.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<script type="text/javascript">
	

	function preview(id) {
		if (id != null) {
			boing("/_tools/get_file/index.aspx?file_id=" + id + "&iframe=true", preview, 768, 480);
		}
		else {
			alert("Please select a file");
		}
	}
			</script>

<dx:ASPxGridView runat="server" AutoGenerateColumns="False" 
	DataSourceID="ds_past_fvrs" Font-Names="Arial" ID="gv_past_fvrs" 
	OnHtmlDataCellPrepared="gv_past_fvrs_HtmlDataCellPrepared" Theme="NETheme01">
	<Columns>
		<dx:gridviewcommandcolumn ShowInCustomizationForm="True" Caption=" "   ShowClearFilterButton="true"
			Visible="False" VisibleIndex="0">
			
		</dx:gridviewcommandcolumn>
		<dx:gridviewdatatextcolumn FieldName="type" ShowInCustomizationForm="True" 
			Width="80px" Caption="FVR Type" VisibleIndex="1">
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatatextcolumn FieldName="name" ShowInCustomizationForm="True" 
			Width="60px" Caption="Category" VisibleIndex="2">
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatadatecolumn FieldName="ts" ShowInCustomizationForm="True" 
			Width="100px" Caption="Verified Date" VisibleIndex="3">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="DateTime">
			</PropertiesDateEdit>
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:gridviewdatadatecolumn>
		<dx:gridviewdatatextcolumn FieldName="id" ShowInCustomizationForm="True" 
			Visible="False" VisibleIndex="4">
			<EditFormSettings Visible="False">
			</EditFormSettings>
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatatextcolumn ShowInCustomizationForm="True" Width="200px" 
			Caption="Uploaded File" VisibleIndex="6">
			<DataItemTemplate>
				<dx:aspxhyperlink ID="ASPxHyperLink1" runat="server" 
													NavigateUrl='<%# string.Format("javascript:preview({0})", Eval("uploaded_id")) %>' 
													Text='<%# Eval("uploaded_filename") %>' />
			</DataItemTemplate>
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatatextcolumn FieldName="filename" ShowInCustomizationForm="True" 
			Width="100%" Caption="Filename" VisibleIndex="5">
			<DataItemTemplate>
				<dx:aspxhyperlink ID="ASPxHyperLink2" runat="server" 
													NavigateUrl='<%# string.Format("javascript:preview({0})", Eval("id")) %>' 
													Text='<%# Eval("filename") %>' />
			</DataItemTemplate>
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatadatecolumn FieldName="issued" ShowInCustomizationForm="True" 
			Width="100px" Caption="Issued Date" VisibleIndex="7">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
			</PropertiesDateEdit>
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:gridviewdatadatecolumn>
	</Columns>
	<SettingsBehavior ColumnResizeMode="Control">
	</SettingsBehavior>
	<SettingsPager PageSize="20">
	</SettingsPager>
	<Settings ShowFilterRow="True" ShowFilterRowMenu="True">
	</Settings>
</dx:ASPxGridView>

							
<asp:HiddenField ID="HiddenField1" runat="server" />


							
<asp:SqlDataSource runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
d.type,
b.tab_index,
a.ts,
c.id,
CONCAT(c. NAME, &#39;.&#39;, c.ext) AS filename,
e.id AS uploaded_id,
CONCAT(e. NAME, &#39;.&#39;, e.ext) AS uploaded_filename,
d.ts AS issued,
neintranet.member_fvr_tab.`name`
FROM
neintranet.member_fvr_history AS a
LEFT JOIN neintranet.member_fvr_dtl AS b ON a.member_fvr_dtl_id = b.id
LEFT JOIN filestore.files AS c ON b.file_id = c.id
LEFT JOIN neintranet.member_fvr_hdr AS d ON b.member_fvr_hdr_id = d.id
LEFT JOIN filestore.files AS e ON b.uploaded_file_id = e.id
LEFT JOIN neintranet.member_fvr_tab ON b.tab_index = neintranet.member_fvr_tab.id
WHERE
          a.confirmed = 1
AND b.file_id != 0
AND b.member_id = ?member_id
order by a.ts desc,d.id desc" ID="ds_past_fvrs">
	<SelectParameters>
		<asp:ControlParameter ControlID="HiddenField1" Name="@member_id" 
			PropertyName="Value" />
	</SelectParameters>
</asp:SqlDataSource>



							
