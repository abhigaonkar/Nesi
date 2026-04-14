<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_reviewed_by_admin" Codebehind="reviewed_by_admin.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<!DOCTYPE HTML>
<html>
<head id="Head1" runat="server">
    <title>How to drag and drop items from/to ASPxListBox using jQuery UI</title>
    <script src="Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.14.min.js" type="text/javascript"></script>
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
                		if ($(ui.draggable).parents(".listBoxLeft").length != 0 && ($(this)).hasClass("listBoxLeft") ||
                            $(ui.draggable).parents(".listBoxRight").length != 0 && ($(this)).hasClass("listBoxRight"))
                			return;

                		var itemIndex = $(ui.draggable).parent().index(); // this is a fragile part of the application

                		var fromListBox, toListBox;

                		if ($(this).hasClass("listBoxRight")) { // determine a source and a destination
                			toListBox = lbChosen;
                			fromListBox = lbAvailable;
                		}
                		else {
                			toListBox = lbAvailable;
                			fromListBox = lbChosen;
                		}

                		toListBox.AddItem(fromListBox.GetItem(itemIndex).text,
                                          fromListBox.GetItem(itemIndex).value);

                		fromListBox.RemoveItem(itemIndex);

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxGlobalEvents ID="ge" runat="server">
            <ClientSideEvents ControlsInitialized="function (s, e) { InitalizejQuery(); }" />
        </dx:ASPxGlobalEvents>
        <table>
            <tr>
                <td style="width: 35%">
                    <dx:ASPxListBox ID="lbAvailable" runat="server" ClientInstanceName="lbAvailable"
                        Width="200px" Height="240px" CssClass="listBoxLeft">
                        <ItemStyle CssClass="lbItem" />
                        <Items>
                            <dx:ListEditItem Text="ASPxEditors Library" Value="ASPxEditors" />
                            <dx:ListEditItem Text="ASPxGauges Suite" Value="ASPxGauges" />
                            <dx:ListEditItem Text="ASPxGridView and Editors Suite" Value="ASPxGridView and Editors" />
                            <dx:ListEditItem Text="ASPxHTMLEditor Suite" Value="ASPxHTMLEditor" />
                            <dx:ListEditItem Text="ASPxperience Suite" Value="ASPxperience" />
                            <dx:ListEditItem Text="ASPxPivotGrid Suite" Value="ASPxPivotGrid" />
                            <dx:ListEditItem Text="ASPxScheduler Suite" Value="ASPxScheduler" />
                            <dx:ListEditItem Text="ASPxSpellChecker" Value="ASPxSpellChecker" />
                            <dx:ListEditItem Text="ASPxTreeList Suite" Value="ASPxTreeList" />
                            <dx:ListEditItem Text="XtraReports Suite" Value="XtraReports" />
                            <dx:ListEditItem Text="XtraCharts Suite" Value="XtraCharts" />
                        </Items>
                    </dx:ASPxListBox>
                </td>
                <td style="width: 30%">
                </td>
                <td style="width: 35%">
                    <dx:ASPxListBox ID="lbChosen" runat="server" ClientInstanceName="lbChosen" Width="200px"
                        Height="240px" CssClass="listBoxRight">
                        <ItemStyle CssClass="lbItem" />
                    </dx:ASPxListBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>