<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_master_chat" Codebehind="master_chat.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

			<div id="m1" style="border-collapse: collapse; border-width: 0px; border-style: none;">
				<dx:ASPxCallbackPanel ID="cb_chat" runat="server" ClientInstanceName="cb_chat" Theme="NETheme01" OnCallback="cb_chat_Callback" Width="100%">
					<PanelCollection>
						<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
							<table cellpadding="0" cellspacing="0" bgcolor="#CCCCCC" width="100%">
								<tr>
									<td align="center">
										<dx:ASPxComboBox ID="ASPxComboBox1" runat="server" TextField="name" 
											ValueField="id" ValueType="System.Int32" Width="100%" Font-Names="Arial" Theme="NETheme01">
											<ClientSideEvents SelectedIndexChanged="function(s, e) {cb_chat.PerformCallback('change_company');}" />
										</dx:ASPxComboBox>
									</td>
								</tr>
								<tr>
									<td class="dxtcLeftAlignCell" align="center">
										<dx:ASPxMemo ID="d_txtaddchat" Theme="NETheme01" runat="server" ClientInstanceName="d_txtaddchat" 
                                            Height="71px" NullText="Type here to add to the discussion" Width="100%">
										</dx:ASPxMemo>
									</td>
								</tr>
								<tr>
									<td align="center" style="padding: 5px">
										<dx:ASPxButton ID="ASPxButton1" runat="server" Font-Names="Arial" 
											Font-Size="8pt" Height="20px" Text="Add" Width="100%" AutoPostBack="False" 
											UseSubmitBehavior="False" Theme="NETheme01">
											<ClientSideEvents Click="function(s, e) {cb_chat.PerformCallback();}" />
										</dx:ASPxButton>
									</td>
								</tr>
								<tr>
									<td align="center">
										<dx:ASPxMemo ID="d_mem_chat" runat="server" Theme="NETheme01" ReadOnly="True"
                                            ClientInstanceName="d_mem_chat" Font-Names="Arial" Font-Size="8pt" Height="200px" Width="100%" HorizontalAlign="Left" >
										</dx:ASPxMemo>
									</td>
								</tr>
							</table>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxCallbackPanel>
				</div>