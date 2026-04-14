<%@ Control Language="C#" AutoEventWireup="true" Inherits="mobile_modules_workorder_details" enabletheming="true" Codebehind="workorder_details.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="wo_filemanager.ascx" tagname="wo_filemanager" tagprefix="uc" %>
<%@ register src="~/mobile/modules/timesheet.ascx" tagprefix="uc" tagname="timesheet" %>

<%@ Register src="../../sections/workorder/modules/analysis.ascx" tagname="analysis" tagprefix="uc1" %>

<link rel="stylesheet" type="text/css" href="/mobile/css/workorder.css" />
<style type="text/css">
	.content
		{
		padding: 5px;
		}
</style>
<asp:dropdownlist id="ddl_tabs" runat="server" cssclass="ddl_tabs menuddl" autopostback="true" onselectedindexchanged="ddl_tabs_SelectedIndexChanged">
	<asp:listitem value="0" text="General" />
	<asp:listitem value="1" text="Notes" />
    <asp:ListItem value="2" text="Analysis" />
	<asp:listitem value="3" text="Sign Off" />
	<asp:listitem value="7" text="Line Items" />
	<asp:listitem value="4" text="Files" />
	<asp:listitem value="5" text="Time Entry" />
	<asp:listitem value="6" text="Invoice Preview" />
</asp:dropdownlist>
<br /><br />
<div class="content">
<asp:multiview id="mv_content" runat="server">
	<asp:view id="v_general" runat="server">
		<iframe id="if_wo" runat="server" width="99%" height="100%" style="border:none;"></iframe>
	</asp:view>

	<asp:view id="v_notes" runat="server">
			<dx:aspxcallbackpanel id="cbp_notes" clientinstancename="cbp_notes" runat="server" oncallback="cbp_notes_Callback">
				<panelcollection>
                <dx:PanelContent><div class="notes"><div><div class="title">New Note</div><div class="body"><dx:ASPxMemo ID="new_note" runat="server" ClientInstanceName="new_note" Height="125px" Width="100%"></dx:ASPxMemo><br />
                    <button id="submit_note" runat="server" class="whitetext" onclick="mobile_wo.addnote(this)" type="button">
                        Add Note
                    </button>
                    </div>
                    </div>
                    <br />
                    <div>
                        <div class="title">
                            Past Notes</div><div id="past_notes" runat="server" class="body"></div></div></div></dx:PanelContent>
                </panelcollection>
		</dx:aspxcallbackpanel>
	</asp:view>
     <asp:view id="v_analysis" runat="server">
		<div width="100%" style="background-color:#F2F5FF">
	    <uc1:analysis ID="uc_analysis" runat="server" />
            </div>
		
	</asp:view>
	<asp:view id="v_signoff" runat="server">
	</asp:view>
	<asp:view id="v_files" runat="server">
		<uc:wo_filemanager id="uc_filemanager" runat="server" />
	</asp:view>
	<asp:view id="v_timeentry" runat="server">
		<uc:timesheet runat="server" id="timesheet" only_wo="true" />
	</asp:view>
	<asp:view id="v_preview" runat="server">
			<div style="padding-top:10px;">
                <div style="float:left;width:48%;margin-right:2px;text-align:right;">
                    <button id="bt_invoicepreview" runat="server" class="whitetext aligncenter" type="button">
                        Invoice Preview
                    </button>
                </div>
                <div style="float:left;width:48%;margin-right:2px;">
                    <button id="bt_sendreworks" runat="server" onclick="mobile_wo.reworks.show()" class="whitetext aligncenter" type="button">
                        Send to Rework
                    </button>
                </div>
            </div>
	</asp:view>
   
</asp:multiview>
</div>