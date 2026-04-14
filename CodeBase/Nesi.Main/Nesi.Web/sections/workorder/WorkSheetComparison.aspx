<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_workorder_WorkSheetComparison"  EnableTheming="True" Codebehind="WorkSheetComparison.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       
                    <table width="100%">
                        <tr>
                            <td style="width: 100px">
                    <asp:Label ID="lblWSMessage" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width: 100px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" Width="100%"  Theme="NETheme01">
                                    <TotalSummary>
										<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="spread" 
											ShowInColumn="Total Difference" SummaryType="Sum" />
									</TotalSummary>
                                    <Columns>
                                        <dx:GridViewCommandColumn ShowClearFilterButton="True" Visible="False" 
											VisibleIndex="0">
										</dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn Caption="Part Number" FieldName="master_id" VisibleIndex="1"
                                            Width="50px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Description" FieldName="description" VisibleIndex="2"
                                            Width="100%" CellStyle-Wrap="True">
<CellStyle Wrap="True"></CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Qty on Quote" FieldName="on_ws" VisibleIndex="3"
                                            Width="50px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="~Ext'd on Quote" FieldName="EXTD_From_ws" 
											VisibleIndex="4" Width="50px">
                                            <PropertiesTextEdit DisplayFormatString="C2">
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Qty on WO" FieldName="on_wo" VisibleIndex="5"
                                            Width="50px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="~Ext'd on WO" FieldName="WO_Sell_Price" 
											VisibleIndex="6">
                                            <PropertiesTextEdit DisplayFormatString="C2">
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                    	<dx:GridViewDataTextColumn Caption="Total Difference" FieldName="spread" 
											SortIndex="0" SortOrder="Descending" VisibleIndex="7" Width="50px">
											<PropertiesTextEdit DisplayFormatString="c2">
											</PropertiesTextEdit>
											<Settings FilterMode="DisplayText" />
										</dx:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager Visible="True" PageSize="250">
                                    </SettingsPager>
                                    
                                	<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" 
										ShowHeaderFilterButton="True" />
                                    
                                </dx:ASPxGridView>
                            </td>
                        </tr>
                    </table>
             
    
    </div>
    </form>
</body>
</html>
