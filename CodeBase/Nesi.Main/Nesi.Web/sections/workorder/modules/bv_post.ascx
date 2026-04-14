<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_workorder_modules_bv_post" Codebehind="bv_post.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="accounting_notes.ascx" tagname="accounting_notes" tagprefix="uc1" %>


<link type="text/css" href="/css/analysis.css" rel="Stylesheet" />
	
	<style runat="server" type="text/css" id="css"></style>
<script type="text/javascript" src="/js/wo_analysis.js"></script>

<script type="text/javascript">

    function fix_perc(v) {
        if (v != '') {
            cb_quote.PerformCallback(v);
        }

    }
</script>

  <div width="100%" style="float:left;">
   

       
   <div class="wrapper_accounting">
<div ID="lbl_accounting_hdr" runat="server" class="header_accounting">Accounting</div>
<div class="panel_">
<div id="div_accounting" runat="server">
												<dx:ASPxCallbackPanel ID="cb_posting" runat="server" 
													ClientInstanceName="cb_posting" Font-Names="Arial" 
													OnCallback="cb_posting_Callback" Width="100%">
													<PanelCollection>
														<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
															<div ID="div_requires_wo_copy" runat="server" 
																style="font-weight:bold;font-size:13px;text-align:center;padding:5px;">
																CUSTOMER REQUIRES COPY OF WORK ORDER WITH INVOICE
															</div>
															

															<dx:ASPxButton runat="server" ID="btn_invoice" Text="Invoice WO" AutoPostBack="False" Visible="False" Theme="NeTheme01" OnClick="btn_invoice_OnClick" Width="100%">
																<ClientSideEvents Click="
																	function(s,e)
																		{ 
																		if(confirm('Are you sure you want to move this to the Invoiced status?'))
																			{ 
																			__doPostBack(s.name, ''); 
																			}
																		else
																			{
																			e.processOnServer = false;
																			}
																		}" />
															</dx:ASPxButton><% // Line breaks added to easily see the invoice button %>
															

															<div ID="div_bvposted" runat="server">
															</div>
															<dx:ASPxPanel ID="panel_accounting_notes" runat="server" RenderMode="Table" 
																Width="100%">
																<Paddings Padding="0px" />
																<PanelCollection>
																	<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																		<uc1:accounting_notes ID="uc_accounting_notes" runat="server" />
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxPanel>
                                                           
                                                            <table width="100%">
                                                                <tr>
                                                                    <td><b>Posting Notes:</b></td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100%">
                                                                        <dx:ASPxMemo ID="mem_posted_notes" runat="server" BackColor="#FFFF99" ClientInstanceName="mem_posted_notes" Theme="NETheme01" Height="200px" Width="100%">
                                                                            <Border BorderStyle="None" />
                                                                        </dx:ASPxMemo>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxButton ID="btnSavePostingNote" runat="server" AutoPostBack="False" ClientInstanceName="btnSavePostingNote" Text="Save" Theme="NETheme01">
                                                                            <ClientSideEvents Click="function(s, e) {
	cb_posting.PerformCallback();
}" />
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                         
                                                                                                                     
														</dx:PanelContent>
													</PanelCollection>
												</dx:ASPxCallbackPanel>
</div>
    </div></div>

  
   
       </div>
                           



	<asp:HiddenField ID="hdn_woid" runat="server" />

