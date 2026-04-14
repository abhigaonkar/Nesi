<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="Messaging" Title="Messaging" Theme="BlueStyle" Codebehind="Messaging.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
<div id="divSide" runat="server">
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<script type ="text/javascript">
    function openwindow(memberid) {
        var winURL = "member_List.aspx?&MemberID=" + memberid + "&OPID=" + "29";
        window.open(winURL,'_blank','height=500,width=300,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes');
    }
</script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
							&nbsp; &nbsp;
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" HeaderText="CC List" CloseAction="CloseButton" Modal="True" RenderIFrameForPopupElements="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="True" ModalBackgroundStyle-Opacity="0">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <div style="height: 400px; overflow-x: hidden; overflow-y: scroll">
                                            <asp:GridView ID="MemberListGrid" runat="server" AutoGenerateColumns="False" >
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkMember" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ID" HeaderText="Member ID" />
                                    <asp:BoundField DataField="nickname" HeaderText="First Name" />
                                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                  
                                </Columns>
                            </asp:GridView>
                            </div>
                             <asp:Button ID="btnSelectUsers" runat="server" Text="Select Users" OnClick="btnSelectUsers_Click" />
                             <asp:CheckBox ID="chkSendAll" runat="server" Text="Select All" OnCheckedChanged="chkSendAll_CheckedChanged"
                            Width="84px" AutoPostBack="True" />
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            &nbsp;&nbsp;
                            <asp:Menu ID="Menu1" runat="server" OnMenuItemClick="Menu1_MenuItemClick" Orientation="Horizontal" 
                                StaticEnableDefaultPopOutImage="False" BorderColor="#4E7D9A" ForeColor="White" BorderStyle="None" style="text-align: center">
                                <Items>
                                    <asp:MenuItem Text="Summary" Value="0"></asp:MenuItem>
									<asp:MenuItem Text="Inbox" Value="1"></asp:MenuItem>
									<asp:MenuItem Text="Sent" Value="2"></asp:MenuItem>
									<asp:MenuItem Text="New" Value="3"></asp:MenuItem>
                                </Items>
                                <StaticMenuItemStyle BackColor="#618DBE" Font-Bold="True" Font-Names="Arial" ForeColor="White"
                                    Height="15px" HorizontalPadding="45px" Width="100px" BorderStyle="Solid" VerticalPadding="6px" />
                                <StaticHoverStyle BackColor="#FF8000" />
                                <StaticSelectedStyle BackColor="#E6E6E6" />
                            </asp:Menu>
                            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                <asp:View ID="Tab1" runat="server">
                                    <asp:Panel ID="pnlMessageList" runat="server" Width="100%" CssClass ="main_border">
                                        <table border="0" cellpadding="0" cellspacing="1" width="100%" class = "main_border">
                                            <tr>
                                                <td align="center" style="text-align: left">
                                                    <asp:Label ID="Label1" runat="server" Text="Message Type"></asp:Label></td>
                                                <td align="center">
                                                    <asp:Label ID="Label6" runat="server" Text="Number of New Messages"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="row1">
                                                    <asp:Label ID="Label4" runat="server" Text="Task Related"></asp:Label></td>
                                                <td align="center" >
                                                    <asp:Label ID="lblPersonalNo" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="row1" align = center style="text-align: left">
                                                    <asp:Label ID="Label5" runat="server" Text="General Question"></asp:Label></td>
                                                <td align="center">
                                                    <asp:Label ID="lblAdminNo" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="row1">
                                                    <asp:Label ID="Label7" runat="server" Text="Urgent"></asp:Label></td>
                                                <td align="center">
                                                    <asp:Label ID="lblSystemNo" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="row1">
                                                    <asp:Label ID="Label8" runat="server" Text="Total New Messages"></asp:Label></td>
                                                <td align="center">
                                                    <asp:Label ID="lblTotalNew" runat="server"></asp:Label></td>
                                            </tr>
                                        </table><asp:GridView ID="GridviewSummary" runat="server" AutoGenerateColumns="False" CssClass = "mainborder" EmptyDataText="There are no Messages for this member" Width = "100%" PageSize="15">
                                            <Columns>
                                                <asp:BoundField DataField="Messagetype" HeaderText="Message Type" HtmlEncode="False" />
                                                <asp:BoundField HeaderText="Status" DataField="Message_status" />
                                                <asp:BoundField DataField="CountOfMessage_Status" HeaderText="Quantity" />
                                            </Columns>
                                            <RowStyle CssClass="row1" />
                                            <EditRowStyle CssClass=" row1" />
                                            <HeaderStyle CssClass="header1" />
                                            <AlternatingRowStyle CssClass="row2" />
                                        </asp:GridView>
                                    </asp:Panel>
                                    <br />
                                </asp:View>
                                <asp:View ID="Tab2" runat="server">
                                    <table cellpadding="0" cellspacing="1" width="100%" class = "main_border">
                                        <tr valign="top">
                                            <td>
                                                <asp:Panel ID="pnlMessageTypeSelect" runat="server">
                                                    <br />
                                                    </asp:Panel>
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                                    ShowSummary="False" ValidationGroup="ValSendMessage" />
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="drpUsers"
                                                    Display="None" ErrorMessage="Select a User" Operator="NotEqual" ValidationGroup="ValSendMessage"
                                                    ValueToCompare="0"></asp:CompareValidator><asp:RequiredFieldValidator ID="valSubject" runat="server" ControlToValidate="txtSubject"
                                                    Display="None" ErrorMessage="Enter a Subject" ValidationGroup="ValSendMessage"></asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="drpMessageType"
                                                    Display="None" ErrorMessage="Select a Message Type" Operator="NotEqual" ValidationGroup="ValSendMessage"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                                <asp:RangeValidator ID="rngdrpMessageType" runat="server" ControlToValidate="drpMessageType"
                                                    Display="None" ErrorMessage="Please select type of message"
                                                    MaximumValue="3" MinimumValue="1" ValidationGroup="ValSendMessage"></asp:RangeValidator>
                                                <asp:Panel ID="pnlMessageGrid" runat="server" Width ="100%">
                                                    <asp:GridView ID="MessageGrid" runat="server" AutoGenerateColumns="False" EmptyDataText="No Messages to display" OnSelectedIndexChanged="MessageGrid_SelectedIndexChanged" Width = "100%" OnRowDataBound="MessageGrid_RowDataBound" DataKeyNames="MessageTo_ID" OnRowDeleting="MessageGrid_RowDeleting" OnPageIndexChanging="MessageGrid_PageIndexChanging" PageSize="50" CellPadding="2" GridLines="None" Font-Names="Arial" AllowPaging="True" AllowSorting="True">
                                                        
                                                        <Columns>
                                                            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" >
																<ItemStyle HorizontalAlign="Center" />
															</asp:BoundField>
                                                            <asp:BoundField HeaderText="To/From" DataField="fullname" />
                                                            <asp:BoundField HeaderText="Subject" DataField="Subject" />
                                                            <asp:BoundField DataField="MessType" HeaderText="Message Type" >
																<ItemStyle HorizontalAlign="Center" />
															</asp:BoundField>
                                                            <asp:BoundField DataField="Status" HeaderText="Status" >
																<ItemStyle HorizontalAlign="Center" />
															</asp:BoundField>
                                                            <asp:TemplateField HeaderText="Action" ShowHeader="False">
                                                                <ControlStyle BorderStyle="Solid" BorderWidth="0px" Font-Bold="True"
                                                                    Font-Size="11px" ForeColor="Black"  />
                                                                <ItemTemplate>
                                                                    &nbsp;<asp:ImageButton ID="Button1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/icon/icon[edit].gif" />
                                                                    <asp:ImageButton ID="Button2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/icon/icon[delete].gif" />
                                                                </ItemTemplate>
																<ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle BackColor="#618DBE" Font-Bold="True" Font-Names="Arial" ForeColor="White" />
                                                        
                                                        
                                                           
                                                        
                                                    </asp:GridView>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlMessageDetails" runat="server" Width="100%" Visible="False">
                                                    <table border="0" cellpadding="2" cellspacing="0" width="100%" style="font-family: Arial">
                                                        <tr>
                                                            <td class="row1">
                                                                &nbsp;</td>
                                                            <td style="width:100%;">
                                                                <asp:DropDownList ID="drpMessageType" runat="server" AutoPostBack="True" DataTextField="MessageType" DataValueField="MessageType_ID" OnSelectedIndexChanged="drpMessageType_SelectedIndexChanged" Width="250px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="row1">
                                                                <asp:Label ID="Label9" runat="server" Text="Date"></asp:Label>
                                                            </td>
                                                            <td style="width:100%;">
                                                                <asp:TextBox ID="txtDate" runat="server" BackColor="White" BorderColor="#E0E0E0" BorderStyle="Solid" Font-Names="Arial" Width="250px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td rowspan="2" class="row1">
                                                                <asp:Label ID="lblToFrom" runat="server" Text="To / From"></asp:Label></td>
                                                            <td style="width: 100%; height: 22px;">
                                                                <asp:DropDownList ID="drpUsers" runat="server" DataTextField="member_fullname" 
																	DataValueField="member_id" Width="250px">
                                                                </asp:DropDownList>
															</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtCCUsers" runat="server" Enabled="False" Width="250px" BackColor="White" BorderColor="#E0E0E0" BorderStyle="Solid" Font-Names="Arial"></asp:TextBox><asp:Button ID="btnCC" runat="server"
                                                                    Text="CC" OnClick="btnCC_Click" BorderWidth="0px" Width="70px"/>
                                                                </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="row1">
                                                                <asp:Label ID="Label2" runat="server" Text="Subject"></asp:Label></td>
                                                            <td style="width:100%;">
                                                                <asp:TextBox ID="txtSubject" runat="server" width ="250px" BackColor="White" BorderColor="#E0E0E0" BorderStyle="Solid" Font-Names="Arial"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="row1" style="height: 142px">
                                                                <asp:Label ID="Label3" runat="server" Text="Message"></asp:Label></td>
                                                            <td style="height: 100%">
                                                                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" width ="550px" Height="250px" BackColor="White" onkeyup='$(this).spellcheck();' BorderColor="#E0E0E0" BorderStyle="Solid" Font-Names="Arial" ></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="row1">
                                                            </td>
                                                            <td>
                                                                <asp:Panel ID="pnlButtons" runat="server" Width = "100%">
                                                                <asp:Button ID="btnSend" runat="server" Text="Send" ForeColor="Black" Font-Bold="True" Font-Size="11px" BorderStyle="Solid" BorderWidth="0px" Height="26px" CssClass="button" OnClick="btnSend_Click" ValidationGroup="ValSendMessage" Width="70px"></asp:Button>
                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ForeColor="Black" Font-Bold="True" Font-Size="11px" BorderStyle="Solid" BorderWidth="0px" Height="26px" CssClass="button" OnClick="btnCancel_Click" Width="70px"></asp:Button>
																	<asp:Button ID="btnreply" runat="server" BorderStyle="Solid" BorderWidth="0px" 
																		CssClass="button" Font-Bold="True" Font-Size="11px" ForeColor="Black" 
																		Height="26px" OnClick="btnReply_Click" Text="Reply" Visible="False" 
																		Width="70px" />
																</asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                </td>
                                        </tr>
                                        <tr valign="top">
                                            <td>
                                                <asp:Panel ID="pnlreply" runat="server" Visible="False">
													<table border="0" cellpadding="2" cellspacing="0" style="font-family: Arial" 
														width="100%">
														<tr>
															<td class="row1">
																<asp:Label ID="lblToFrom0" runat="server" Text="To / From"></asp:Label>
															</td>
															<td style="width: 100%; height: 22px;">
																<asp:DropDownList ID="drpUsers0" runat="server" datasourceid="SqlDataSource2" 
																	DataTextField="member_fullname" DataValueField="member_id" Enabled="False" 
																	Width="250px">
																</asp:DropDownList>
															</td>
														</tr>
														<tr>
															<td class="row1">
																<asp:Label ID="Label11" runat="server" Text="Subject"></asp:Label>
															</td>
															<td style="width:100%;">
																<asp:TextBox ID="txtSubject0" runat="server" BackColor="White" 
																	BorderColor="#E0E0E0" BorderStyle="Solid" Enabled="False" Font-Names="Arial" 
																	width="250px"></asp:TextBox>
															</td>
														</tr>
														<tr>
															<td class="row1" style="height: 142px">
																<asp:Label ID="Label12" runat="server" Text="Message"></asp:Label>
															</td>
															<td style="height: 100%">
																<asp:TextBox ID="txtMessage0" runat="server" BackColor="White" 
																	BorderColor="#E0E0E0" BorderStyle="Solid" Font-Names="Arial" Height="250px" 
																	onkeyup="$(this).spellcheck();" TextMode="MultiLine" width="550px"></asp:TextBox>
															</td>
														</tr>
														<tr>
															<td class="row1">
															</td>
															<td>
																<asp:Panel ID="pnlButtons0" runat="server" Width="100%">
																	<asp:Button ID="btnSend0" runat="server" BorderStyle="Solid" BorderWidth="0px" 
																		CssClass="button" Font-Bold="True" Font-Size="11px" ForeColor="Black" 
																		Height="26px" OnClick="btnSend0_Click" Text="Send" Width="70px" />
																	<asp:Button ID="btnCancel0" runat="server" BorderStyle="Solid" 
																		BorderWidth="0px" CssClass="button" Font-Bold="True" Font-Size="11px" 
																		ForeColor="Black" Height="26px" OnClick="btnCancel0_Click" Text="Cancel" 
																		Width="70px" />
																</asp:Panel>
															</td>
														</tr>
													</table>
												</asp:Panel>
                                                </td>
                                        </tr>
                                        <tr valign="top">
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                                <asp:Label ID="errorlabel" runat="server" Font-Bold="True" Font-Underline="True"
                                                    ForeColor="Red"></asp:Label></asp:View>
                                <asp:View ID="Tab3" runat="server">
                                    <table cellpadding="0" cellspacing="1" width="100%">
                                        <tr valign="top">
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="Tab4" runat="server">
                                    <table cellpadding="0" cellspacing="1" width="100%">
                                        <tr valign="top">
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                            </asp:MultiView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            &nbsp;<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
	c.member_id,
	CONCAT(member_fullname, ' - ', f.name) member_fullname
FROM
member AS c
INNER JOIN business_unit AS f ON c.business_unit_id = f.id
WHERE
c.Member_Status = 'Active'
ORDER BY 
	f.id ASC,
	c.member_nickname ASC,
	c.member_lastname ASC"></asp:SqlDataSource>
							<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>

