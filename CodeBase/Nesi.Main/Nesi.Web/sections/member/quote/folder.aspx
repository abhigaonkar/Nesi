<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_member_quote_folder" Codebehind="folder.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Quote Folder</title>
	<link type="text/css" href="/css/base/ui.all.css" rel="stylesheet" />
</head>
<body>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script language="javascript">
	function noError(){return true;}
window.onerror = noError;
	</script>
    <form id="form1" runat="server">
    <div>
		<dx:aspxfilemanager id="fm_quote" runat="server" height="650px" width="98%" ClientInstanceName="fm_quote">
<SettingsEditing AllowCreate="True" AllowRename="True" AllowMove="True" AllowDownload="true" AllowDelete="True"></SettingsEditing>
<SettingsToolbar ShowDownloadButton="True" />
<SettingsFileList ThumbnailsViewSettings-ThumbnailSize="48px" DetailsViewSettings-ThumbnailSize="48px"/>
			<ClientSideEvents SelectedFileOpened="function(s, e) {
var fn		= &quot;/quote_store/&quot;+fm_quote.GetCurrentPath()+&quot;/&quot;+e.file.name;
$(&quot;#dl&quot;).attr(&quot;src&quot;, &quot;./folder.aspx?a=dl&amp;file=&quot;+fn);
}" />
			<Styles>
				<UploadPanel>
					<Paddings PaddingLeft="0px" />
				</UploadPanel>
			</Styles>
</dx:aspxfilemanager>
<iframe style="width:0;height:0;" frameborder="0" id="dl"></iframe>
    
    </div>
    </form>
</body>
</html>
