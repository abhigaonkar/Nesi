<%@ Page Language="C#" AutoEventWireup="true"  theme="" Inherits="Quoting" Title="Quote" Codebehind="index.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Import Namespace="nesi.core" %>



<%@ Register src="modules/post_mortem.ascx" tagname="post_mortem" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<script type="text/javascript">
			var _sellprice_parent_obj = null;

			function start_quote() {
				popnew.Show();
				var f = document.getElementById('popnew_frm_newquote');
				f.src = "startframe.aspx?p=q";
			}
			function resizeIframe(obj) {
				obj.style.height = (obj.contentWindow.document.body.scrollHeight + 1) + 'px';

			}
			function kill_quote() {
				poppost_mortem.Show();
				//	var f = document.getElementById('poppost_mortem');
				//	f.src = "post_mortem_frame.aspx?p=q";
			}
			function show_post_mortem_popup() {
				poppost_mortem.Show();
			}
		</script>
		<link type="text/css" media="screen" href="/css/quote.css" rel="stylesheet" />
		<link type="text/css" href="/css/base/ui.all.css" rel="stylesheet" />
		<link type="text/css" href="/css/autocomplete.css" rel="stylesheet" />
		<link type="text/css" href="/css/jquery_custom_mods.css" rel="stylesheet" />
		
		<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
		<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
		<script type="text/javascript" src="/js/jquery.spellcheck.js"></script>
		<script type="text/javascript" src="/js/jquery.tablesorter.min.js"></script>
		<script type="text/javascript" src="/js/jquery.tip.js"></script>
		<script type='text/javascript' src='/js/jquery.autocomplete.js'></script>
		<script type="text/javascript" src="/js/jquery.inventory_new.js?unique=<%=Toolbox.do_RandomString(10) %>"></script>
		<script type="text/javascript" src="/js/functions.js?unique=<%=Toolbox.do_RandomString(10) %>"></script>
		<script type="text/javascript" src="/sections/member/quote/modules/quote_js.ashx?unique=<%=Toolbox.do_RandomString(10) %>"></script>
		<title>Quoting System</title>
		</head>
		<body>
			<form id="form1" runat="server">
				<div id="quoting" class="quoting" runat="Server">
				
				</div>
			<dx:ASPxPopupControl ID="popstage1" runat="server" ClientInstanceName="popstage1" HeaderText="Stage 1 Go No-Go" Height="500px" Modal="True" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="700px" Font-Names="Arial" CloseAction="None">
				<HeaderStyle BackColor="#4682B4" Font-Names="Arial" Font-Size="14pt" ForeColor="#CCCCCC" />
				<ModalBackgroundStyle Opacity="0">
				</ModalBackgroundStyle>
				<ContentCollection>
					<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
						<iframe id="iframe_approval" runat="server" frameborder="0" src="about:blank" height="600px" name="I1" width="100%"></iframe>
					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>
			<dx:ASPxPopupControl ID="popblock" runat="server" ClientInstanceName="popblock" CloseAction="None" Modal="True" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowHeader="False" Style="text-align: center" Width="220px">
				<ContentCollection>
					<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxLabel ID="lblprocess_step_warning" runat="server" Text="This quote has net yet been approved past Stage1 or Stage 4. You must speak to your branch manager before proceeding">
						</dx:ASPxLabel>
					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>
			<dx:ASPxPopupControl ID="popnew" runat="server" ClientInstanceName="popnew" PopupAnimationType="None" AppearAfter="0" CloseAction="CloseButton" HeaderText="Start New Quote" Height="900px"  Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" Width="750px" Font-Names="Arial" ShowPageScrollbarWhenModal="True" Font-Size="11pt">
				<HeaderStyle BackColor="#4682B4" Font-Bold="True" ForeColor="White">
					<Paddings Padding="5px" />
				</HeaderStyle>
				<ContentCollection>
					<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
						<iframe id="frm_newquote" runat="server" width="100%" height="880px" src="about:blank" frameborder="0"></iframe>
					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>
			<dx:ASPxPopupControl ID="poppost_mortem" runat="server" ClientInstanceName="poppost_mortem" PopupAnimationType="None" AppearAfter="0" CloseAction="CloseButton" HeaderText="Post Mortem" Height="700px"  Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" PopupVerticalOffset="0" Width="950px" Font-Names="Arial" ShowPageScrollbarWhenModal="True" Font-Size="11pt">
				<HeaderStyle BackColor="#4682B4" Font-Bold="True" ForeColor="White">
					<Paddings Padding="5px" />
				</HeaderStyle>
				<ModalBackgroundStyle Opacity="0">
				</ModalBackgroundStyle>
				<ContentCollection>
					<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
						<uc1:post_mortem ID="post_mortem1" runat="server" />
					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>
			<dx:ASPxPopupControl ID="pop_revision" runat="server" ClientInstanceName="pop_revision" PopupAnimationType="None" AppearAfter="0" CloseAction="CloseButton" HeaderText="New Version" Height="250px"  Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" Width="550px" Font-Names="Arial" ShowPageScrollbarWhenModal="True" Font-Size="11pt" Theme="NETheme01" AllowDragging="True">
				<ContentStyle HorizontalAlign="Center">
				</ContentStyle>
				<HeaderStyle BackColor="#4682B4" Font-Bold="True" ForeColor="White">
					<Paddings Padding="5px" />
				</HeaderStyle>

<SettingsLoadingPanel Delay="0"></SettingsLoadingPanel>
				<ContentCollection>
					<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
					<div align="center">
						<dx:ASPxMemo ID="memo_revision" runat="server" ClientInstanceName="memo_revision" Height="150px" Theme="NETheme01" Width="100%" Caption="Please add a reason why you are revising this quote:" MaxLength="1000">
							<CaptionSettings Position="Top" ShowColon="False" />
						</dx:ASPxMemo><br />
						<dx:ASPxButton ID="bt_rev_cancel" runat="server" Text="Cancel" Theme="NETheme01" Width="100px" AutoPostBack="False">
							<ClientSideEvents Click="quote_obj.revision.cancel" />
						</dx:ASPxButton>
						<dx:ASPxButton ID="bt_rev_go" runat="server" Text="Go!" Theme="NETheme01" Width="100px" AutoPostBack="False">
							<ClientSideEvents Click="quote_obj.revision.go" />
						</dx:ASPxButton>
						<br /></div>
					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>
			</form>
		</body>
</html>
