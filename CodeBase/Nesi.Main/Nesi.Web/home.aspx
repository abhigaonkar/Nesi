<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="home" Title="Home Page"   Codebehind="home.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ MasterType TypeName="IntraDefault" %>


 


<%@ Register src="modules/todolist.ascx" tagname="todolist" tagprefix="uc" %>


 


<%@ Register src="modules/auto_bingo.ascx" tagname="auto_bingo" tagprefix="uc" %>
<%@ Register src="modules/slow_page.ascx" tagname="slow_page" tagprefix="uc" %>
<%@ Register Src="modules/invoiceService_runTimes.ascx" TagPrefix="uc1" TagName="invoiceService_runTimes" %>

 


<%@ Register src="modules/stuff_i_did.ascx" tagname="stuff_i_did" tagprefix="uc1" %>


 



<asp:content ID="Content1" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    
    </div>
</asp:content>
<asp:content ID="Content4" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server"><div style="height:250px;"></div>
 
 
	
	</asp:content>



<asp:content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<div id="logo" runat="server" visible="false">
	<table>
		<tr>
			<td><div class='sq_button quote_button' id="quote_button" runat="server">I would like a quote...</div></td>
			<td><div class='sq_button wo_button' id="wo_button" runat="server">Let's start another job...</div></td>
			<td><div class='sq_button faq_button' id="faq_button" runat="server">F.A.Qs</div></td>
		</tr>
	</table>
</div>
<table width="100%" style="border:none;">
        <tr>
            <td align="left" valign="top" colspan="2">
                <uc:todolist ID="todolist1" runat="server"  EnableTheming="false"/>
              
                <br />
            </td>
        </tr>
        <tr style="border:none;">
            <td align="left" valign="top">
                <dx:aspxroundpanel ID="pnl_myquotes" runat="server" HeaderText="My Quotes" 
					Width="100%" BackColor="White" Visible="False">
                <Border BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                <HeaderStyle BackColor="DarkGray" Font-Bold="True" Font-Names="Arial" ForeColor="White" >
                    <BorderLeft BorderStyle="None" />
                    <BorderRight BorderStyle="None" />
                    <BorderBottom BorderStyle="None" />
                </HeaderStyle>
                <HeaderContent>
                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/Images/ASPxRoundPanel/1465305193/HeaderContent.png"
                        Repeat="RepeatX" VerticalPosition="bottom" />
                </HeaderContent>
                	<HeaderTemplate>
									<table style="width:100%;">
										<tr>
											<td>
												My Quotes</td>
										</tr>
										<tr>
											<td>
												<table style="width: 100%; font-weight: normal; font-size: xx-small;">
													<tr>
														<td bgcolor="Red">
															&nbsp;&nbsp;&nbsp;&nbsp;
														</td>
														<td nowrap="nowrap">
															More Than 8</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="#FFFF66" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															More Than 4</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="Lime" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															Less Than 4</td>
														<td>
															&nbsp;</td>
													</tr>
												</table>
											</td>
										</tr>
										
									</table>
					</HeaderTemplate>
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <dx:ASPxGridView ID="grid_quotes" runat="server" AutoGenerateColumns="False" Width="370px" KeyFieldName="status" OnHtmlDataCellPrepared="grid_quotes_HtmlDataCellPrepared"  >
                            <columns>
                                <dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="0" Width="120px">
                                    <PropertiesTextEdit DisplayFormatString="{0}">
                                    </PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <CellStyle>
                                        <Paddings PaddingLeft="3px" />
                                        <Border BorderStyle="None" />
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataProgressBarColumn Caption="Qty" FieldName="quotecount" Name="Qty2"
                                    VisibleIndex="1" Width="150px">
                                    <PropertiesProgressBar ClientInstanceName="Qty2" DisplayFormatString="" DisplayMode="Position"
                                        Height="" Width="">
                                        <Style>
<Border BorderStyle="None"></Border>
</Style>
                                    </PropertiesProgressBar>
                                    <DataItemTemplate>
                                        <div>
                                            <dx:ASPxProgressBar ID="ASPxProgressBar2" runat="server" Height="21px" OnDataBound="ASPxProgressBar2_DataBound"
                                    Value='<%# Eval("wocount") %>' DisplayFormatString="" DisplayMode="Position" Maximum="50" Width="150px" BackColor="WhiteSmoke" >
                                                <Border bordercolor="#cccccc" borderwidth="1px"/>
                                            </dx:ASPxProgressBar>
                                        </div>
                                    </DataItemTemplate>
                                    <CellStyle HorizontalAlign="Center">
                                        <Border BorderStyle="None" />
                                    </CellStyle>
                                </dx:GridViewDataProgressBarColumn>
                                <dx:GridViewDataTextColumn Caption="Age (days)" FieldName="Age" Name="Age2" VisibleIndex="3"
                                    Width="70px">
                                    <CellStyle Font-Names="Arial" Font-Size="11pt" HorizontalAlign="Center">
                                        <Border BorderStyle="None" />
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsPager PageSize="15">
                            </SettingsPager>
                            <Settings UseFixedTableLayout="True" />
                            <Styles>
                                <Header HorizontalAlign="Center" BackColor="Transparent" forecolor="Black">
                                    <Border BorderStyle="None" />
                                </Header>
                                <DetailCell>
                                    <Border BorderStyle="None" />
                                    <BorderLeft BorderStyle="None" />
                                    <BorderRight BorderStyle="None" />
                                </DetailCell>
                                <Cell>
                                    <Paddings Padding="0px" PaddingBottom="3px" PaddingTop="3px" />
                                    <BorderLeft BorderStyle="None" />
                                    <BorderRight BorderStyle="None" />
                                </Cell>
                            </Styles>
                            <Border BorderStyle="None" />
                        </dx:ASPxGridView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:aspxroundpanel>
                <br />
                <dx:aspxroundpanel ID="pnl_mywoprocess" runat="server" 
					HeaderText="My Work Orders being Processed" Width="100%" 
					BackColor="Transparent" Visible="False">
                <Border BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                <HeaderStyle BackColor="DarkGray" Font-Bold="True" Font-Names="Arial" ForeColor="White" >
                    <BorderLeft BorderStyle="None" />
                    <BorderRight BorderStyle="None" />
                    <BorderBottom BorderStyle="None" />
                </HeaderStyle>
                <HeaderContent>
                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/Images/ASPxRoundPanel/1465108585/HeaderContent.png"
                        Repeat="RepeatX" VerticalPosition="bottom" />
                </HeaderContent>
				<HeaderTemplate>
									<table style="width:100%;">
										<tr>
											<td>
												My Work Orders being Processed</td>
										</tr>
										<tr>
											<td>
												<table style="width: 100%; font-weight: normal; font-size: xx-small;">
													<tr>
														<td bgcolor="Red">
															&nbsp;&nbsp;&nbsp;&nbsp;
														</td>
														<td nowrap="nowrap">
															More Than 7</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="#FFFF66" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															More Than 3</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="Lime" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															Less Than 3</td>
														<td>
															&nbsp;</td>
													</tr>
												</table>
											</td>
										</tr>
										
									</table>
					</HeaderTemplate>
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <dx:ASPxGridView ID="grid_woStatus" runat="server" AutoGenerateColumns="False" 
							Width="370px" KeyFieldName="status" 
							OnHtmlDataCellPrepared="grid_woStatus_HtmlDataCellPrepared" 
							OnHtmlRowPrepared="grid_woStatus_HtmlRowPrepared" >
                            <columns>
                                <dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="0" Width="120px">
                                    <PropertiesTextEdit DisplayFormatString="{0}">
                                    </PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataProgressBarColumn Caption="Qty" FieldName="wocount" Name="Qty" VisibleIndex="1" Width="150px">
                                    <PropertiesProgressBar ClientInstanceName="Qty" DisplayFormatString="" DisplayMode="Position"
                                            Height="" Width="">
                                    </PropertiesProgressBar>
                                    <DataItemTemplate>
                                        <div>
                                            <dx:ASPxProgressBar ID="ASPxProgressBar1" runat="server" Height="21px" OnDataBound="ASPxProgressBar1_DataBound"
                                    Value='<%# Eval("wocount") %>' DisplayFormatString="" DisplayMode="Position" Maximum="50" Width="150px" BackColor="WhiteSmoke" >
                                                <Border bordercolor="#cccccc" borderwidth="1px" />
                                            </dx:ASPxProgressBar>
                                        </div>
                                    </DataItemTemplate>
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dx:GridViewDataProgressBarColumn>
                                <dx:GridViewDataTextColumn Caption="Age (days)" FieldName="Age" Name="Age" VisibleIndex="3" Width="70px">
                                    <CellStyle Font-Names="Arial" Font-Size="11pt" HorizontalAlign="Center">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsPager Mode="ShowAllRecords">
							</SettingsPager>
                            <Settings UseFixedTableLayout="True" />
                            <Styles>
                                <Header HorizontalAlign="Center" BackColor="Transparent" forecolor="Black">
                                    <Border BorderStyle="None" />
                                </Header>
                                <DetailCell>
                                    <Border BorderStyle="None" />
                                    <BorderLeft BorderStyle="None" />
                                    <BorderRight BorderStyle="None" />
                                </DetailCell>
                                <Cell>
                                    <Paddings Padding="0px" PaddingBottom="3px" PaddingTop="3px" />
                                    <Border BorderStyle="None" />
                                    <BorderLeft BorderStyle="None" />
                                    <BorderRight BorderStyle="None" />
                                </Cell>
                            </Styles>
                            <Border BorderStyle="None" />
                        </dx:ASPxGridView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:aspxroundpanel>
                <br />
            <dx:aspxroundpanel ID="pnl_branchpo" runat="server" 
					HeaderText="Branch Purchase Orders" Width="100%" BackColor="White" 
					Visible="False">
                <Border BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                <HeaderStyle BackColor="DarkGray" Font-Bold="True" Font-Names="Arial" ForeColor="White" >
                    <BorderLeft BorderStyle="None" />
                    <BorderRight BorderStyle="None" />
                    <BorderBottom BorderStyle="None" />
                </HeaderStyle>
                <HeaderContent>
                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/Images/ASPxRoundPanel/1465305193/HeaderContent.png"
                        Repeat="RepeatX" VerticalPosition="bottom" />
                </HeaderContent>
				<HeaderTemplate>
									<table style="width:100%;">
										<tr>
											<td>
												Branch Purchase Orders</td>
										</tr>
										<tr>
											<td>
												<table style="width: 100%; font-weight: normal; font-size: xx-small;">
													<tr>
														<td bgcolor="Red">
															&nbsp;&nbsp;&nbsp;&nbsp;
														</td>
														<td nowrap="nowrap">
															More Than 7</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="#FFFF66" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															More Than 3</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="Lime" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															Less Than 3</td>
														<td>
															&nbsp;</td>
													</tr>
												</table>
											</td>
										</tr>
										
									</table>
					</HeaderTemplate>
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <dx:ASPxGridView ID="grid_pos" runat="server" AutoGenerateColumns="False" Width="370px" KeyFieldName="status" OnHtmlDataCellPrepared="grid_quotes_HtmlDataCellPrepared" >
                            <columns>
                                <dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="0" Width="150px">
                                    <PropertiesTextEdit DisplayFormatString="{0}">
                                    </PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <CellStyle>
                                        <Paddings PaddingLeft="3px" />
                                        <Border BorderStyle="None" />
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataProgressBarColumn Caption="Qty" FieldName="quotecount" Name="Qty2"
                                    VisibleIndex="1" Width="150px">
                                    <PropertiesProgressBar ClientInstanceName="Qty2" DisplayFormatString="" DisplayMode="Position"
                                        Height="" Width="">
                                        <Style>
<Border BorderStyle="None"></Border>
</Style>
                                    </PropertiesProgressBar>
                                    <DataItemTemplate>
                                        <div>
                                            <dx:ASPxProgressBar ID="ASPxProgressBar4" runat="server" Height="21px" OnDataBound="ASPxProgressBar1_DataBound"
                                    Value='<%# Eval("wocount") %>' DisplayFormatString="" DisplayMode="Position" Maximum="50" Width="150px" BackColor="WhiteSmoke" >
                                                <Border BorderStyle="None" />
                                            </dx:ASPxProgressBar>
                                        </div>
                                    </DataItemTemplate>
                                    <CellStyle HorizontalAlign="Center">
                                        <Border BorderStyle="None" />
                                    </CellStyle>
                                </dx:GridViewDataProgressBarColumn>
                                <dx:GridViewDataTextColumn Caption="Age (days)" FieldName="Age" Name="Age2" VisibleIndex="3"
                                    Width="70px">
                                    <CellStyle Font-Names="Arial" Font-Size="11pt" HorizontalAlign="Center">
                                        <Border BorderStyle="None" />
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <Settings UseFixedTableLayout="True" />
                            <Styles>
                                <Header HorizontalAlign="Center" BackColor="Transparent" forecolor="Black">
                                    <Border BorderStyle="None" />
                                </Header>
                                <DetailCell>
                                    <Border BorderStyle="None" />
                                    <BorderLeft BorderStyle="None" />
                                    <BorderRight BorderStyle="None" />
                                </DetailCell>
                                <Cell>
                                    <Paddings Padding="0px" PaddingBottom="3px" PaddingTop="3px" />
                                    <BorderLeft BorderStyle="None" />
                                    <BorderRight BorderStyle="None" />
                                </Cell>
                            </Styles>
                            <Border BorderStyle="None" />
                        </dx:ASPxGridView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:aspxroundpanel>
            </td>
            <td valign="top" align="left"><dx:aspxroundpanel ID="pnl_branchquotes" 
					runat="server" HeaderText="Branch Quotes Waiting for Approval" Width="100%" 
					BackColor="White" Visible="False">
                <Border BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                <HeaderStyle BackColor="DarkGray" Font-Bold="True" Font-Names="Arial" ForeColor="White" >
                    <BorderLeft BorderStyle="None" />
                    <BorderRight BorderStyle="None" />
                    <BorderBottom BorderStyle="None" />
                </HeaderStyle>
                <HeaderContent>
                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/Images/ASPxRoundPanel/1465305193/HeaderContent.png"
                        Repeat="RepeatX" VerticalPosition="bottom" />
                </HeaderContent>
				<HeaderTemplate>
									<table style="width:100%;">
										<tr>
											<td>
												Branch Quotes Waiting for Customers</td>
										</tr>
										<tr>
											<td>
												<table style="width: 100%; font-weight: normal; font-size: xx-small;">
													<tr>
														<td bgcolor="Red">
															&nbsp;&nbsp;&nbsp;&nbsp;
														</td>
														<td nowrap="nowrap">
															More Than 12</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="#FFFF66" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															More Than 8</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="Lime" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															Less Than 8</td>
														<td>
															&nbsp;</td>
													</tr>
												</table>
											</td>
										</tr>
										
									</table>
					</HeaderTemplate>
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <dx:ASPxGridView ID="grid_comp_quotes" runat="server" AutoGenerateColumns="False" Width="370px" KeyFieldName="pm" OnHtmlDataCellPrepared="grid_quotes_HtmlDataCellPrepared" >
                            <columns>
                                <dx:GridViewDataTextColumn Caption="Project Manager" FieldName="pm" VisibleIndex="0"
                                    Width="120px">
                                    <PropertiesTextEdit DisplayFormatString="{0}">
                                    </PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <CellStyle>
                                        <Paddings PaddingLeft="3px" />
                                        <Border BorderStyle="None" />
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataProgressBarColumn Caption="Qty" FieldName="quotecount" Name="Qty2"
                                    VisibleIndex="1" Width="150px">
                                    <PropertiesProgressBar ClientInstanceName="Qty2" DisplayFormatString="" DisplayMode="Position"
                                        Height="" Width="">
                                        <Style>
<Border BorderStyle="None"></Border>
</Style>
                                    </PropertiesProgressBar>
                                    <DataItemTemplate>
                                        <div>
                                            <dx:ASPxProgressBar ID="ASPxProgressBar3" runat="server" Height="21px" OnDataBound="ASPxProgressBar3_DataBound"
                                    Value='<%# Eval("quotecount") %>' DisplayFormatString="" Maximum="50" DisplayMode="Position" Width="150px" BackColor="WhiteSmoke" >
                                                <Border bordercolor="#cccccc" borderwidth="1px" />
                                            </dx:ASPxProgressBar>
                                        </div>
                                    </DataItemTemplate>
                                    <CellStyle HorizontalAlign="Center">
                                        <Border BorderStyle="None" />
                                    </CellStyle>
                                </dx:GridViewDataProgressBarColumn>
                                <dx:GridViewDataTextColumn Caption="Age (days)" FieldName="Age" Name="Age2" VisibleIndex="3"
                                    Width="70px">
                                    <CellStyle Font-Names="Arial" Font-Size="11pt" HorizontalAlign="Center">
                                        <Border BorderStyle="None" />
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <Settings UseFixedTableLayout="True" />
                            <Styles>
                                <Header HorizontalAlign="Center" BackColor="Transparent" forecolor="Black">
                                    <Border BorderStyle="None" />
                                </Header>
                                <DetailCell>
                                    <Border BorderStyle="None" />
                                    <BorderLeft BorderStyle="None" />
                                    <BorderRight BorderStyle="None" />
                                </DetailCell>
                                <Cell>
                                    <Paddings Padding="0px" PaddingBottom="3px" PaddingTop="3px" />
                                    <BorderLeft BorderStyle="None" />
                                    <BorderRight BorderStyle="None" />
                                </Cell>
                            </Styles>
                            <Border BorderStyle="None" />
                            <SettingsPager PageSize="25">
                            </SettingsPager>
                        </dx:ASPxGridView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:aspxroundpanel>
                <br />
                <dx:aspxroundpanel ID="pnl_branchwoprocess" runat="server" 
					HeaderText="Branch Work Orders being Processed" Width="100%" 
					BackColor="Transparent" Visible="False">
                    <Border BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                    <HeaderStyle BackColor="DarkGray" Font-Bold="True" Font-Names="Arial" ForeColor="White" >
                        <BorderLeft BorderStyle="None" />
                        <BorderRight BorderStyle="None" />
                        <BorderBottom BorderStyle="None" />
                    </HeaderStyle>
                    <HeaderContent>
                        <BackgroundImage HorizontalPosition="left" ImageUrl="~/Images/ASPxRoundPanel/1465108585/HeaderContent.png"
                        Repeat="RepeatX" VerticalPosition="bottom" />
                    </HeaderContent>
					<HeaderTemplate>
									<table style="width:100%;">
										<tr>
											<td>
												Branch Work Orders being Processed</td>
										</tr>
										<tr>
											<td>
												<table style="width: 100%; font-weight: normal; font-size: xx-small;">
													<tr>
														<td bgcolor="Red">
															&nbsp;&nbsp;&nbsp;&nbsp;
														</td>
														<td nowrap="nowrap">
															More Than 20</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="#FFFF66" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															More Than 10</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="Lime" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															Less Than 10</td>
														<td>
															&nbsp;</td>
													</tr>
												</table>
											</td>
										</tr>
										
									</table>
					</HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <dx:ASPxGridView ID="grid_woStatus_branch" runat="server" AutoGenerateColumns="False" Width="370px" KeyFieldName="status" OnHtmlDataCellPrepared="grid_woStatus_HtmlDataCellPrepared" >
                                <columns>
                                    <dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="0" Width="120px">
                                        <PropertiesTextEdit DisplayFormatString="{0}">
                                        </PropertiesTextEdit>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataProgressBarColumn Caption="Qty" FieldName="wocount" Name="Qty" VisibleIndex="1" Width="150px">
                                        <PropertiesProgressBar ClientInstanceName="Qty" DisplayFormatString="" DisplayMode="Position"
                                            Height="" Width="">
                                        </PropertiesProgressBar>
                                        <DataItemTemplate>
                                            <div>
                                                <dx:ASPxProgressBar ID="ASPxProgressBar1" runat="server" Height="21px" OnDataBound="ASPxProgressBar4_DataBound"
                                    Value='<%# Eval("wocount") %>' DisplayFormatString="" DisplayMode="Position" Maximum="50" Width="150px" BackColor="WhiteSmoke" >
                                                    <Border BorderStyle="Solid" bordercolor="#cccccc" borderwidth="1px"/>
                                                </dx:ASPxProgressBar>
                                            </div>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataProgressBarColumn>
                                    <dx:GridViewDataTextColumn Caption="Age (days)" FieldName="Age" Name="Age" VisibleIndex="3" Width="70px">
                                        <CellStyle Font-Names="Arial" Font-Size="11pt" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <Settings UseFixedTableLayout="True" />
                                <Styles>
                                    <Header HorizontalAlign="Center" BackColor="Transparent" forecolor="Black">
                                        <Border BorderStyle="None" />
                                    </Header>
                                    <DetailCell>
                                        <Border BorderStyle="None" />
                                        <BorderLeft BorderStyle="None" />
                                        <BorderRight BorderStyle="None" />
                                    </DetailCell>
                                    <Cell>
                                        <Paddings Padding="0px" PaddingBottom="3px" PaddingTop="3px" />
                                        <Border BorderStyle="None" />
                                        <BorderLeft BorderStyle="None" />
                                        <BorderRight BorderStyle="None" />
                                    </Cell>
                                </Styles>
                                <Border BorderStyle="None" />
                            </dx:ASPxGridView>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:aspxroundpanel>
                &nbsp;<dx:aspxroundpanel ID="pnl_wowaitingpm" runat="server" 
					HeaderText="Branch Work Orders Waiting for PM" Width="100%" 
					BackColor="Transparent" Visible="False">
                <Border BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                <HeaderStyle BackColor="DarkGray" Font-Bold="True" Font-Names="Arial" ForeColor="White" >
                    <BorderLeft BorderStyle="None" />
                    <BorderRight BorderStyle="None" />
                    <BorderBottom BorderStyle="None" />
                </HeaderStyle>
                <HeaderContent>
                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/Images/ASPxRoundPanel/1465108585/HeaderContent.png"
                        Repeat="RepeatX" VerticalPosition="bottom" />
                </HeaderContent>
				<HeaderTemplate>
									<table style="width:100%;">
										<tr>
											<td>
												Branch Work Orders Waiting for PM</td>
										</tr>
										<tr>
											<td>
												<table style="width: 100%; font-weight: normal; font-size: xx-small;">
													<tr>
														<td bgcolor="Red">
															&nbsp;&nbsp;&nbsp;&nbsp;
														</td>
														<td nowrap="nowrap">
															More Than 7</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="#FFFF66" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															More Than 3</td>
														<td nowrap="nowrap">
															&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
														<td bgcolor="Lime" nowrap="nowrap">
															&nbsp;&nbsp; &nbsp;</td>
														<td nowrap="nowrap">
															Less Than 3</td>
														<td>
															&nbsp;</td>
													</tr>
												</table>
											</td>
										</tr>
										
									</table>
					</HeaderTemplate>
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <dx:ASPxGridView ID="grid_comp_wo" runat="server" AutoGenerateColumns="False" Width="370px" KeyFieldName="pm" OnHtmlDataCellPrepared="grid_woStatus_HtmlDataCellPrepared" >
                            <columns>
                                <dx:GridViewDataTextColumn Caption="Project Manager" FieldName="pm" VisibleIndex="0"
                                    Width="120px">
                                    <PropertiesTextEdit DisplayFormatString="{0}">
                                    </PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataProgressBarColumn Caption="Qty" FieldName="wocount" Name="Qty" VisibleIndex="1" Width="150px">
                                    <PropertiesProgressBar ClientInstanceName="Qty" DisplayFormatString="" DisplayMode="Position"
                                            Height="" Width="">
                                    </PropertiesProgressBar>
                                    <DataItemTemplate>
                                        <div>
                                            <dx:ASPxProgressBar ID="ASPxProgressBar1" runat="server" Height="21px" OnDataBound="ASPxProgressBar1_DataBound"
                                    Value='<%# Eval("wocount") %>' DisplayFormatString="" DisplayMode="Position"  Maximum="50" Width="150px" BackColor="WhiteSmoke" >
                                                <Border  bordercolor="#cccccc" borderwidth="1px"/>
                                            </dx:ASPxProgressBar>
                                        </div>
                                    </DataItemTemplate>
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dx:GridViewDataProgressBarColumn>
                                <dx:GridViewDataTextColumn Caption="Age (days)" FieldName="Age" Name="Age" VisibleIndex="3" Width="70px">
                                    <CellStyle Font-Names="Arial" Font-Size="11pt" HorizontalAlign="Center">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <Settings UseFixedTableLayout="True" />
                            <Styles>
                                <Header HorizontalAlign="Center" BackColor="Transparent" forecolor="Black">
                                    <Border BorderStyle="None" />
                                </Header>
                                <DetailCell>
                                    <Border BorderStyle="None" />
                                    <BorderLeft BorderStyle="None" />
                                    <BorderRight BorderStyle="None" />
                                </DetailCell>
                                <Cell>
                                    <Paddings Padding="0px" PaddingBottom="3px" PaddingTop="3px" />
                                    <Border BorderStyle="None" />
                                    <BorderLeft BorderStyle="None" />
                                    <BorderRight BorderStyle="None" />
                                </Cell>
                            </Styles>
                            <Border BorderStyle="None" />
                            <SettingsPager PageSize="25">
                            </SettingsPager>
                        </dx:ASPxGridView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:aspxroundpanel>
            </td>
        </tr>
		<tr>
		<td colspan="2" id="rp_devwatch" runat="server" visible="false">
			<uc:auto_bingo ID="uc_auto_bingo" runat="server" />
			<br />
		    <uc1:invoiceService_runTimes runat="server" ID="invoiceService_runTimes" />
		    <br />
		    <uc:slow_page ID="uc_slow_page" runat="server" />
		</td>
		</tr>
    </table>
</asp:content>


<asp:content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	</asp:content>



