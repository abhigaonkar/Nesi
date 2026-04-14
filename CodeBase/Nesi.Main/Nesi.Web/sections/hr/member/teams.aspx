<%@ Page Language="C#" MasterPageFile="~/nonFrame.master"  AutoEventWireup="true" Inherits="member_teams" Title="Teams" Codebehind="teams.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>








<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript">

    	function InitalizejQuery() {
    		$('.lbItem').draggable(
                {
                	helper: 'clone'
                }
            );

                $('.listBoxLeft, .listBoxRight').droppable(
                {
                	activeClass: "hover",
                	drop: function (ev, ui) {
                		/* do nothing when the parent == destination */
                		//		if ($(ui.draggable).parents(".listBoxLeft").length != 0 && ($(this)).hasClass("listBoxLeft") ||
                		//           $(ui.draggable).parents(".listBoxRight").length != 0 && ($(this)).hasClass("listBoxRight"))
                		//		return;

                		var itemIndex = $(ui.draggable).parent().index(); // this is a fragile part of the application

                		var fromListBox, toListBox;
                		//	alert($(ui.draggable).attr("id"));
                		if ($(ui.draggable).parents(".listBoxRight").length != 0 && ($(this)).hasClass("listBoxRight")) {
                			cb.PerformCallback("1|" + $(this).attr("id") + "|" + itemIndex + "|" + $(ui.draggable).attr("id"));
               
                		}
                		else if ($(this).hasClass("listBoxRight")) { // determine a source and a destination
                			//alert($(this).attr("id"));
                			//alert(lbChosen.GetName());
                			//			toListBox = lbChosen;
                			fromListBox = lbAvailable;
                			cb.PerformCallback("0|" + $(this).attr("id") + "|" + fromListBox.GetItem(itemIndex).value);
                		}
                		else {
                			//	alert(ui.attr("id"));
                			//		toListBox = lbAvailable;
                			//	fromListBox = lbChosen;
                			cb.PerformCallback("1|" + $(this).attr("id") + "|" + itemIndex + "|" + $(ui.draggable).attr("id"));
                		}


                		//		toListBox.AddItem(fromListBox.GetItem(itemIndex).text,fromListBox.GetItem(itemIndex).value);

                		//	fromListBox.RemoveItem(itemIndex);

                		InitalizejQuery(); // repeat the initialization for new items
                	}
                }
              );
    	}
    </script>
    <style type="text/css">
        .lbItem
        {
            width: 200px;
        }
        
        /* like SelectedItem style */
        .ui-draggable-dragging
        {
            background-color: #A0A0A0;
            color: White;
        }
        
        /* small glowing effect */
        .hover
        {
            -webkit-box-shadow: 0 0 15px #ff0000;
            -moz-box-shadow: 0 0 15px #ff0000;
            box-shadow: 0 0 15px #ff0000;
        }
    	.style1
		{
			font-family: Arial, Helvetica, sans-serif;
		}
    </style>


   
        <dx:ASPxGlobalEvents ID="ge" runat="server">
            <ClientSideEvents ControlsInitialized="function (s, e) { InitalizejQuery(); }" />
        </dx:ASPxGlobalEvents>
		
        <dx:ASPxCallbackPanel ID="cb" runat="server" Width="100%" 
		ClientInstanceName="cb" oncallback="cb_Callback">
			<Paddings Padding="0px" />
			<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	
	<table style="width: 100%; border-collapse: collapse;">
		<tr>
			<td style="margin: 10px; white-space: nowrap;" nowrap="nowrap">
				<dx:ASPxComboBox ID="ddlbranch" runat="server" AutoPostBack="True" 
					 Font-Names="Arial" TextField="name" 
					ValueField="id" ValueType="System.Int32">
				</dx:ASPxComboBox>
			</td>
			<td class="style1" colspan="2" nowrap="nowrap" 
				style="margin: 10px; white-space: nowrap;">
				<strong>* Drag and Drop as Required</strong></td>
		</tr>
		<tr style="padding: 0px" valign="top">
			<td style="padding: 10px; margin: 10px; font-family: Arial, Helvetica, sans-serif; font-weight: 700;" 
				valign="top" align="center" bgcolor="#C1E0FF" colspan="2">
				Pool<br />
				<dx:ASPxListBox ID="lbAvailable" runat="server" 
					ClientInstanceName="lbAvailable" CssClass="listBoxLeft" Font-Names="Arial" Height="500px" 
					TextField="member_fullname" Theme="NETheme01" ValueField="Member_ID" 
					ValueType="System.Int32" Width="200px">
					<Columns>
						<dx:ListBoxColumn FieldName="member_id" Visible="False" />
						<dx:ListBoxColumn Caption="Name" FieldName="member_fullname" />
						<dx:ListBoxColumn Caption="Title" FieldName="membertype_name" />
					</Columns>
					<ItemStyle CssClass="lbItem" >
					<Border BorderStyle="None" />
					</ItemStyle>
					<Border BorderStyle="None" />
				</dx:ASPxListBox>
			</td>
			<td style="padding: 10px; margin: 10px;" valign="top" width="100%" 
				align="center" bgcolor="#DDEEFF">
				<span class="style1"><strong>Project Managers</strong></span><strong><br class="style1" />
				</strong>
				<dx:ASPxPanel ID="pnl" runat="server" RenderMode="Table" Width="100%" 
					BackColor="Transparent">
					<Paddings Padding="0px" />
					<PanelCollection>
						<dx:PanelContent runat="server" SupportsDisabledAttribute="True" BorderStyle="None">
						</dx:PanelContent>
					</PanelCollection>
					<Border BorderStyle="None" BorderWidth="0px" />
				</dx:ASPxPanel>
			</td>
		</tr>
		<tr>
			<td valign="top" colspan="2">
				&nbsp;</td>
			<td valign="top">
				&nbsp;</td>
		</tr>
	</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>

</asp:Content>
