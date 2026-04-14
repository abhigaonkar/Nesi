
<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true"  Inherits="markup_popup" Title="Markup Calculator" Theme="NETheme01" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>













<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <script type="text/javascript" src="/js/functions.js"></script>
<script type ="text/javascript">
var keyValue;

function popup_Shown(s, e) {
    callbackPanel.PerformCallback(keyValue);
}
    function GetNewPrice(cost)
	{
	   
        var url           = "/_tools/sell_price/index.aspx?cost="+cost;
	    var sellprice = "0";
	$.ajax(
	       {
	       type:		'GET',
			url:		url,
			dataType:	'xml',
			async: false,
			
			beforeSend:	function()
						{
						
						},
	        success:	function(xml)
						{
						// This is for if it successfully receives a response it can work with
						$(xml).find('PriceReturn').each(function()
							{
							sellprice	= $(this).attr('price');
							document.getElementById("ctl00_cphMasterBody_hidSellPrice").Value = sellprice;
							
							})
						},
						complete:	function()
					    {
						     return sellprice;
						},
						error:	function(XMLHttpRequest, textStatus, errorThrown)
						{
						return "0";
						}
	
	       }
	);
        
	}
	function roundNumber(number, digits) {
            var multiple = Math.pow(10, digits);
            var rndedNum = Math.round(number * multiple) / multiple;
            return rndedNum;
        }
        
        function base_sell_price() 
        {
            var sell;
            document.getElementById("ctl00_cphMasterBody_ASPxRoundPanel1_ASPxPanel3_lblSell").Value = "3344";
            if($('#ctl00_cphMasterBody_ASPxRoundPanel1_ASPxPanel3_txtCost').val() > $('#ctl00$cphMasterBody$hdn_costxQty').val())
            {
            tbox.value = "5544";
            }
           
        }
         

</script>
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <ASP:UPDATEPANEL id="UpdatePanel1" runat="server">
    <contenttemplate>
    <ASP:UPDATEPROGRESS ID="UPDATEPROGRESS1" runat="server" DisplayAfter="250">
        <PROGRESSTEMPLATE>
        <div id="Layer1" style="position:absolute; z-index:1; left: 50%; top: 50%;">
          <img id="Img1" src="/images/loading_panel.gif" alt="progressing" />
        </div>
        </PROGRESSTEMPLATE>
    </ASP:UPDATEPROGRESS>
       
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" DefaultButton="ImageButton5" Width="250px" HeaderText="Markup Calculator" HorizontalAlign="Center">
            <PanelCollection>
                <dx:PanelContent runat="server">
                    &nbsp;<dx:ASPxPanel ID="ASPxPanel3" runat="server" Width="250px">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server"> 
                                            
                                    <SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control" />
                                    &nbsp;<table cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td align="left" style="font-weight: normal; font-size: 10pt; font-family: Arial;
                                                background-color: gainsboro; text-align: left" valign="middle">
                                                Business Unit:</td>
                                            <td align="left" style="font-weight: normal; font-size: 10pt; font-family: Arial;
                                                background-color: gainsboro; text-align: left" valign="middle">
                                                <dx:ASPxComboBox ID="ddl_bu" runat="server" TextField="ddl_name" Theme="NETheme01" ValueField="id" ValueType="System.Int32">
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="font-weight: normal; font-size: 10pt; font-family: Arial;
                                                background-color: gainsboro; text-align: left" valign="middle">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="font-weight: normal; font-size: 10pt; font-family: Arial;
                                                background-color: gainsboro; text-align: left" valign="middle">Base Markup....</td>
                                        </tr>
                                    <tr>
                                        <td align="left" style="font-weight: bold; font-size: 10pt; width: 100px; font-family: Arial;
                                            background-color: gainsboro; text-align: left; " valign="middle">
                                            Cost:</td>
                                        <td style="font-weight: bold; font-size: 10pt; width: 100px; font-family: Arial; background-color: gainsboro; border-top-style: none; border-right-style: none; border-left-style: none; text-align: center; border-bottom-style: none;" align="center">
                                            <dx:ASPxTextBox ID="txtCost" runat="server" AutoPostBack="True" DisplayFormatString="C"
                                                HorizontalAlign="Right" OnTextChanged="txtCost_TextChanged2" Width="75px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="font-weight: bold; font-size: 10pt; width: 100px; font-family: Arial;
                                            background-color: gainsboro; text-align: left; " valign="middle">
                                            Sell:</td>
                                        <td style="font-weight: bold; font-size: 10pt; font-family: Arial; background-color: gainsboro;
                                            text-align: center; width: 100px; " align="center">
                                            <dx:ASPxLabel ID="lblSell" runat="server" Text="ASPxLabel">
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="font-weight: bold; font-size: 10pt; width: 100px; font-family: Arial;
                                            background-color: gainsboro; text-align: left; " valign="middle">
                                            Margin:</td>
                                        <td style="font-weight: bold; font-size: 10pt; font-family: Arial; background-color: gainsboro;
                                            text-align: center; width: 100px; " align="center">
                                            <dx:ASPxLabel ID="lblMargin" runat="server" Text="ASPxLabel">
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="middle" style="width: 100px; background-color: gainsboro; font-weight: bold; font-size: 10pt; border-top-style: none; font-family: Arial; border-right-style: none; border-left-style: none; text-align: left; border-bottom-style: none;">
                                            &nbsp;</td>
                                        <td style="width: 100px; background-color: gainsboro; font-weight: bold; font-size: 10pt; border-top-style: none; font-family: Arial; border-right-style: none; border-left-style: none; text-align: left; border-bottom-style: none;">
                                        </td>
                                    </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="font-weight: normal; font-size: 10pt; font-family: Arial;
                                                background-color: #ffff99; text-align: left; " valign="middle">
                                                If the part is a "QTY" part....</td>
                                        </tr>
                                    <tr>
                                        <td align="left" style="font-weight: bold; font-size: 10pt; font-family: Arial; background-color: #ffff99;
                                            text-align: left; " valign="middle">
                                            Cost:</td>
                                        <td style="font-weight: bold; font-size: 10pt; font-family: Arial; background-color: #ffff99; text-align: center; ">
                                            <dx:ASPxTextBox ID="txtQtyCost" runat="server" AutoPostBack="True" DisplayFormatString="C"
                                                HorizontalAlign="Right" OnTextChanged="txtQtyCost_TextChanged" Width="70px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="font-weight: bold; font-size: 10pt; font-family: Arial; background-color: #ffff99;
                                            text-align: left; " valign="middle">
                                            Normal Sell:</td>
                                        <td style="font-weight: bold; font-size: 10pt; font-family: Arial; background-color: #ffff99; text-align: center; ">
                                            <dx:ASPxTextBox ID="txtQtySell" runat="server" AutoPostBack="True" DisplayFormatString="C" ClientEnabled="false"
                                                HorizontalAlign="Right" Width="70px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="font-weight: bold; font-size: 10pt; font-family: Arial; background-color: #ffff99;
                                            text-align: left; " valign="middle">
                                            Qty:</td>
                                        <td style="font-weight: bold; font-size: 10pt; font-family: Arial; background-color: #ffff99; text-align: center; ">
                                            <dx:ASPxTextBox ID="txtQty" runat="server" AutoPostBack="True" HorizontalAlign="Right"
                                                OnTextChanged="txtQty_TextChanged" Width="70px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="font-weight: bold; font-size: 10pt; font-family: Arial; background-color: #ffff99;
                                            text-align: left; " valign="middle">
                                            Qty Sell (ea):</td>
                                        <td style="font-weight: bold; font-size: 10pt; font-family: Arial; background-color: khaki; text-align: center; width: 100px; " align="center">
                                            <dx:ASPxLabel ID="lblQtySell" runat="server" Text="ASPxLabel">
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>
                                        <tr>
                                            <td align="left" style="font-weight: bold; font-size: 10pt; border-top-style: none;
                                                font-family: Arial; border-right-style: none; border-left-style: none; background-color: #ffff99;
                                                text-align: left; border-bottom-style: none" valign="middle">
                                                Total Sell:</td>
                                            <td align="center" style="font-weight: bold; font-size: 10pt; width: 100px; border-top-style: none;
                                                font-family: Arial; border-right-style: none; border-left-style: none; background-color: khaki;
                                                text-align: center; border-bottom-style: none">
                                                <dx:ASPxLabel ID="lblqtytotalsell" runat="server" Text="ASPxLabel">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                </table><br /><br />
                                </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>
                    </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        &nbsp; &nbsp; &nbsp;&nbsp;
    </contenttemplate>
    </ASP:UPDATEPANEL>
    </ASP:CONTENT>
