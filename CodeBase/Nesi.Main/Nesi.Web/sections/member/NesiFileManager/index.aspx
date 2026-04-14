<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits		= 'NesiFileManager' 
			Title			= 'File Manager' 
			Validaterequest	= 'false'
			enableEventValidation = 'false'
 Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:CONTENT ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
</div>
</asp:CONTENT>
<asp:Content ID="header_" ContentPlaceHolderID="header_placeholder" runat="server">

		<style type="text/css">
		.widthfix .dxm-gutter
			{
			width:	550px !important;
			}

.dxeBase
{
	font: 12px Tahoma;
}
		</style>
</asp:Content>
<ASP:CONTENT ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</ASP:CONTENT>
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

    <table width="100%">
        <tr>
            <td valign="top">
				<dx:ASPxComboBox ID="ddl_te" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_te_SelectedIndexChanged" TextField="ddl_name" Theme="NETheme01" ValueField="id" ValueType="System.Int32">
				</dx:ASPxComboBox>
            </td>
            <td valign="top">
				<dx:ASPxComboBox ID="ASPxComboBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ASPxComboBox1_SelectedIndexChanged" SelectedIndex="0" Theme="NETheme01">
					<Items>
						<dx:ListEditItem Text="Internal Public" Value="0" Selected="True"></dx:ListEditItem>
						<dx:ListEditItem Text="Confidential" Value="1"></dx:ListEditItem>
						<dx:ListEditItem Text="Wide Open Public" Value="2" />
					</Items>
				</dx:ASPxComboBox>
            </td>
            <td width="100%" rowspan="2">
                <dx:ASPxListBox ID="lb" runat="server" ClientVisible="False" Height="150px" ReadOnly="True" TextField="name" Theme="NETheme01" ValueField="id" ValueType="System.Int32" Width="300px" Caption="Who else can see this?">
                    <Columns>
                        <dx:ListBoxColumn Caption="Name" FieldName="name" />
                        <dx:ListBoxColumn Caption="Business Unit" FieldName="bu" />
                        <dx:ListBoxColumn Caption="id" FieldName="id" Visible="False" />
                    </Columns>
                    <CaptionSettings ShowColon="False" VerticalAlign="Top" />
                    <CaptionCellStyle>
                        <Paddings PaddingLeft="20px" />
                    </CaptionCellStyle>
                    <Border BorderStyle="None" />
                </dx:ASPxListBox>
            </td>
        </tr>
        <tr>
            <td>
	<dx:ASPxButton ID="expandall" runat="server" Text="Expand All Folders" AutoPostBack="false" Theme="NETheme01">
		<ClientSideEvents Click="function(s, e) {
	fm.GetTreeView().ExpandAll();
}" />
	</dx:ASPxButton>
 
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;<dx:ASPxLabel ID="lblError" runat="server" ClientInstanceName="lblError" ForeColor="Red"
                    Text="ASPxLabel" Width="100%">
                </dx:ASPxLabel>
            </td>
        </tr>
    </table>
	<dx:ASPxFileManager ID="fm" runat="server" ClientInstanceName="fm" 
		OnCustomJSProperties="ASPxFileManager1_CustomJSProperties"
		OnFileUploading="ASPxFileManager1_FileUploading" Font-Names="Arial"
		Font-Size="10pt" Settings-UseAppRelativePath=False Height="700px" Theme="MaterialCompact">
		<SettingsFileList View="Details">
		    <DetailsViewSettings>
		        <Columns>
		            <dx:FileManagerDetailsColumn Caption=" " FileInfoType="Thumbnail" 
		                                         VisibleIndex="0" Width="150px">
		            </dx:FileManagerDetailsColumn>
		            <dx:FileManagerDetailsColumn Caption="Name" VisibleIndex="1" Width="100%">
		            </dx:FileManagerDetailsColumn>
		            <dx:FileManagerDetailsColumn Caption="Date modified" 
		                                         FileInfoType="LastWriteTime" VisibleIndex="2" Width="100px">
		            </dx:FileManagerDetailsColumn>
		            <dx:FileManagerDetailsColumn Caption="Size" FileInfoType="Size" 
		                                         VisibleIndex="3" Width="100px">
		            </dx:FileManagerDetailsColumn>
		        </Columns>
		    </DetailsViewSettings>

		
		</SettingsFileList>
		<SettingsEditing AllowCreate="True" AllowRename="True" AllowMove="True" AllowDownload="true" AllowDelete="True"></SettingsEditing>
		<SettingsUpload AdvancedModeSettings-EnableMultiSelect="True">
			<AdvancedModeSettings EnableMultiSelect="True"></AdvancedModeSettings>
		</SettingsUpload>
		<SettingsToolbar ShowDownloadButton="True" />

		<Settings ThumbnailFolder="~\\App_Data\\Thumb\\" EnableMultiSelect="True" UseAppRelativePath="True"></Settings>
	
		<Styles>
			<File Font-Names="Arial" Font-Size="10pt">
			</File>
		</Styles>
		<ClientSideEvents SelectedFileOpened="function(s, e) {
e.file.Download();
e.processOnServer = false;
       }
       "
			FileUploaded="function(s, e) {
	
	
}"
			EndCallback="function(s, e) {
	lblError.SetText(fm.cplblError);
}" />
	    <Border BorderStyle="None" />
	</dx:ASPxFileManager>
<iframe id="downloader" width="1" height="1"></iframe>

</ASP:CONTENT>