<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_i_modules_template" Codebehind="template.ascx.cs" %>
<div id="print_instructions" runat="server" visible="false">
	<table width="100%" cellspacing="0">
		<tr>
			<td>
				<button type="button" style="width:100%" onclick="$('#inst').toggle();">Toggle Print Instructions</button>
				<div style="display:none;padding:10px;" id="inst">
				Printing through either the built-in Chrome PDF viewer, or the Adobe Acrobat PDF Reader uses basically the same method. <br />
				If you move your mouse pointer over the document below you should see this image at the bottom-right  of the page if you are using Chrome:<br />
				<img src="/images/chrome_pdf.png" width="294" height="70" /><br />
				Or this image if you are using Adobe Acrobat Reader:<br />
				<img src="/images/adobe_pdf.png" width="363" height="58" /><br />
				Both will have a print button, circled in red, simply press that and it should begin the print process.
				</div>
			</td>
		</tr>
	</table>
</div>
<iframe runat="server" id="iframe" width="100%" style="min-height:675px;"  frameborder="0"></iframe>
<button type="button" id="mobile_link" runat="server" Visible="False" class="whitetext aligncenter">View Document</button>
<asp:Label runat="server" ForeColor="Red" ID="lb_Error"></asp:Label>
<asp:HiddenField ID="dtl_id" runat="server" />
<p align="center">
			<input type="checkbox" ID="cb" runat="server" onchange="fvr.client.check_for_acknowledge(this)" class="cb" /><label for="<%= cb.ClientID %>"> I Acknowledge and/or Agree <div align="center" style="font-size:11px;">(Checking this checkbox will automatically sign & submit this document.<br />If this is the last page, the window will close.)</div></label></p>