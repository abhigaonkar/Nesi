<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_messaging_popup_videoplayer" Codebehind="popup_videoplayer.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Video Player</title>
		<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
		<script type="text/javascript">
			function switch_video(s,e)
				{
				var id			= s.GetValue() / 1;
				if(id > 0)
					{
					$("#video_frame").attr("src", "/_tools/get_file/index.aspx?file_id="+s.GetValue()+"&iframe=true");
					}
				}
		</script>
	</head>
	<body>
		<form id="form1" runat="server">
			<div>
				<table width="100%" cellpadding="5" cellspacing="0">
					<tr>
						<td style="height:300px;background-color:#01447c;"><iframe src="about:blank" id="video_frame" frameborder="0" style="width:100%;height:330px;border:solid 1px #ccc;"></iframe></td>
					</tr>
					<tr>
						<td>
							<dx:ASPxComboBox Native="true" runat="server" ID="ddl_videos" ValueType="System.Int32" Width="100%">
								<ClientSideEvents SelectedIndexChanged="switch_video" />
							</dx:ASPxComboBox>
						</td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</html>
