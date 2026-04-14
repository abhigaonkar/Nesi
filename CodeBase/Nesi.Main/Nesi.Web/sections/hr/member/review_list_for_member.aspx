<%@ Page Title="Employee Review" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Theme="NETheme01" Inherits="sections_hr_member_review_list_for_member" EnableTheming="True" Codebehind="review_list_for_member.aspx.cs" %>	

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript">
		

 function bind_tooltips()
			{
			$(".opt1").each(function()
				{
				$(this).tip();
				});
			}
		$(document).ready(function()
			{
	//		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
			});

function EndReqHandler()
	{
	bind_tooltips();
	
	}
	function resizeIframe(obj)
 {
 //  obj.style.height = (obj.contentWindow.document.body.scrollHeight + 100) + 'px';
	
 }
   

	</script>
	
	<table>
		<tr>
			<td valign="top" width="0%">
	<dx:ASPxCallbackPanel runat="server" ClientInstanceName="cb" ID="cb" 
					OnCallback="cb_Callback" Width="100%">
		<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_parent =='1')
{

opener.location.href = opener.location.href;
location.href = s.cp_url;
}
}" />
		
		<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<table style="padding: 10px; font-family: Calibri, Helvetica, sans-serif; font-size: small;">
					
					<tr><td  >
						
									
										<table>
										<tr>
						<td align="left" colspan="2" nowrap="nowrap">
							<table style="width:100%;">
								<tr>
									<td valign="top" colspan="9">
										<asp:Label ID="lblerror" runat="server" Font-Names="Arial" ForeColor="#FF3300"></asp:Label>

									    &nbsp;</td>
								</tr>
								<tr>
									<td valign="top">
										<dx:ASPxButton ID="btnprint_worksheet" runat="server" HorizontalAlign="Center" 
											OnClick="btnprint_Click" ToolTip="Print" Width="30px" Height="25px">
											<Image Height="15px" Url="~/images/icon/icon[print].gif">
											</Image>
										    <Paddings Padding="2px" />
										</dx:ASPxButton>
									</td>
									<td>
										<dx:ASPxButton ID="btn_print_final" runat="server" ClientVisible="False" 
											HorizontalAlign="Center" OnClick="btnprint0_Click" 
											ToolTip="Print Employee's Copy" Width="30px" Height="25px">
											<Image Height="15px" Url="~/images/icon/icon[printwoPrice].GIF">
											</Image>
										    <Paddings Padding="2px" />
										</dx:ASPxButton>
									</td>
									<td>
										<dx:ASPxButton ID="btnclose" runat="server" ClientVisible="False" OnClick="btnclose_Click" ToolTip="Delete this review"
                                             Width="30px" Height="25px">
                                            <ClientSideEvents Click="function(s, e) {
	if (confirm('Are you sure you want to delete or close this review?'))
{
e.ProcessOnServer=true;
}
}" />
                                            <Image Height="15px" Url="~/images/icon/icon[delete].GIF">
                                            </Image>
                                            <Paddings Padding="2px" />
                                        </dx:ASPxButton>
                                    </td>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
									<td width="100%" align="right">
										&nbsp;</td>
								</tr>
							</table>
						</td>
						</tr>
						<tr>
						<td class="style1" nowrap="nowrap">
							Review ID:</td>
						<td>
							<dx:ASPxLabel runat="server" ClientInstanceName="lblid" CssClass="style1" 
								ID="lblid" Font-Names="Arial"></dx:ASPxLabel>

						</td>
					</tr>
					<tr>
						<td nowrap="nowrap">
							Employee Name:</td>
						<td>
							<dx:ASPxLabel ID="lblid0" runat="server" ClientInstanceName="lblid" 
								CssClass="style1" Font-Names="Arial">
							</dx:ASPxLabel>

						</td>
							</tr>
							<tr>
						<td nowrap="nowrap">
							Reviewed By:</td>
						<td>
							<dx:ASPxComboBox ID="ddlreviewed" runat="server" 
								ClientInstanceName="ddlreviewed" 
								 Font-Names="Arial" 
								IncrementalFilteringMode="StartsWith" OnDataBound="ddlreviewed_DataBound" 
								TextField="name" ValueField="member_id" ValueType="System.Int32">
							</dx:ASPxComboBox>


						</td>
					</tr>
					<tr>
						<td nowrap="nowrap">
							Date of Review:</td>
						<td>
							<dx:ASPxDateEdit ID="dtereview" runat="server" ClientInstanceName="dtereview" 
								DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
								EditFormatString="yyyy-MM-dd" Font-Names="Arial">
							</dx:ASPxDateEdit>

						</td>
					</tr>
					<tr>
						<td nowrap="nowrap">
							Employment Agreement:</td>
						<td>
							<dx:ASPxComboBox ID="ddloffers" runat="server" Font-Names="Arial" 
								TextField="_name" ValueField="id" ValueType="System.Int32">
							</dx:ASPxComboBox>

						</td>
					</tr>
					<tr>
						<td nowrap="nowrap">
							Review Status:</td>
						<td>
							<dx:ASPxLabel ID="lblstatus" runat="server" CssClass="style1" 
								Font-Names="Arial" Text="ASPxLabel">
							</dx:ASPxLabel>

						</td>
					</tr>
					<tr>
						<td class="style3" nowrap="nowrap">
							Membertype:</td>
						<td class="style4">
							<dx:ASPxLabel ID="lblmt" runat="server" CssClass="style1" Font-Names="Arial" 
								Text="ASPxLabel">
							</dx:ASPxLabel>
						</td>
											</tr>
						<tr>
						<td>
							Locked Down:</td>
						<td>
							<dx:ASPxCheckBox ID="chklocked" runat="server" CheckState="Unchecked" Text=" " 
								ValueChecked="1" ValueType="System.Int32" 
								ValueUnchecked="0" ClientEnabled="False">
							</dx:ASPxCheckBox>
							<dx:ASPxLabel ID="lbl_whocanunlock" runat="server" CssClass="style1" 
								Font-Names="Arial">
							</dx:ASPxLabel>
						</td>
					</tr>
											<tr>
												<td>
													<dx:ASPxButton ID="btnsave" runat="server" AutoPostBack="False" Text="Save">
														<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback();
}" />
													</dx:ASPxButton>
												</td>
												<td align="right" style="text-align: right">
													&nbsp;</td>
											</tr>
										</table>
									
								
</td>
						<td colspan="2" align="right" valign="top" width="100%">
						
									
										
										<div ID="results" runat="server" style="height: 100%">
										</div>
									<dx:ASPxButton ID="btn_refresh" runat="server" OnClick="btn_refresh_Click" 
												Text="Refresh">
											</dx:ASPxButton>
						</td>
					</tr>
					<tr>
						<td colspan="4">
							<dx:ASPxCallbackPanel ID="cb_grid" runat="server" ClientInstanceName="cb_grid" 
								OnCallback="cb_grid_Callback1" Width="200px">
								<PanelCollection>
									<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
									</dx:PanelContent>
								</PanelCollection>
							</dx:ASPxCallbackPanel>
							<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
								EnableClientSideAPI="True" Font-Names="Arial" Width="100%">
								<TabPages>
									<dx:TabPage ClientEnabled="False" Text="Universal Review">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<dx:ASPxCheckBox ID="cb_expand" runat="server" AutoPostBack="True" 
													CheckState="Unchecked" ClientInstanceName="cb_expand" Text="Expand All">
												</dx:ASPxCheckBox>
												<dx:ASPxGridView ID="gv_reviewitem" runat="server" AutoGenerateColumns="False" 
													ClientInstanceName="gv_reviewitem" Font-Names="Arial" KeyFieldName="id" 
													OnCustomCallback="gv_reviewitem_CustomCallback" 
													OnCustomColumnDisplayText="gv_reviewitem_CustomColumnDisplayText" 
													OnHtmlRowCreated="gv_reviewitem_HtmlRowCreated" 
													OnHtmlRowPrepared="gv_reviewitem_HtmlRowPrepared" Width="100%">
													<ClientSideEvents EndCallback="function(s, e) {

}" />
													<GroupSummary>
														<dx:ASPxSummaryItem DisplayFormat="This is a test" ShowInColumn="Group" 
															ShowInGroupFooterColumn="Group" />
													</GroupSummary>
													<Columns>
														<dx:GridViewDataTextColumn Caption="Group" FieldName="group" 
															ShowInCustomizationForm="True" VisibleIndex="0" Width="100px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Review Item" FieldName="reviewitem" 
															ShowInCustomizationForm="True" VisibleIndex="1" Width="220px" CellStyle-Wrap="True">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="historyid" FieldName="id" 
															ShowInCustomizationForm="True" Visible="False" VisibleIndex="5">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Score" FieldName="score" 
															ShowInCustomizationForm="True" VisibleIndex="2" Width="215px">
															<DataItemTemplate>
																<dx:ASPxRadioButtonList ID="ASPxRadioButtonList1" runat="server" 
																	Font-Names="Arial" Font-Size="7pt" Height="10px" ItemSpacing="0px" 
																	oninit="ASPxRadioButtonList1_Init" RepeatDirection="Horizontal" 
																	RepeatLayout="Flow" TextAlign="Left" TextSpacing="0px" TextWrap="False" 
																	Value='<%# Eval("score") %>' ValueType="System.Int32" Width="200px">
																	<RadioButtonStyle Wrap="False">
																	</RadioButtonStyle>
																	<Paddings Padding="0px" />
																	<Items>
																		<dx:ListEditItem Text="1" Value="1" />
																		<dx:ListEditItem Text="2" Value="2" />
																		<dx:ListEditItem Text="3" Value="3" />
																		<dx:ListEditItem Text="4" Value="4" />
																		<dx:ListEditItem Text="5" Value="5" />
																		<dx:ListEditItem Text="N/A" Value="0" />
																	</Items>
																	<Border BorderStyle="None" />
																</dx:ASPxRadioButtonList>
															</DataItemTemplate>
															<CellStyle HorizontalAlign="Center" Wrap="False">
																<Paddings PaddingLeft="0px" PaddingRight="0px" />
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
															ShowInCustomizationForm="True" VisibleIndex="3" Width="200px">
															<DataItemTemplate>
																<dx:ASPxMemo ID="memreviewnotes" runat="server" BackColor="#FFFFCC" 
																	Height="30px" oninit="memreviewnotes_Init" Text='<%# Eval("notes") %>' 
																	Width="100%">
																	<ClientSideEvents TextChanged="function(s, e) {
	cb_review.PerformCallback(s);
}" />
																</dx:ASPxMemo>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="groupid" FieldName="groupid" 
															ShowInCustomizationForm="True" Visible="False" VisibleIndex="4">
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsBehavior ColumnResizeMode="Control" />
													<SettingsPager Mode="ShowAllRecords">
													</SettingsPager>
													<Settings GroupFormat="{1} {2}" ShowGroupFooter="VisibleIfExpanded" 
														ShowTitlePanel="True" />
													<Styles>
                                                        <TitlePanel Font-Size="12pt">
                                                        </TitlePanel>
                                                    </Styles>
													<Templates>
														<TitlePanel>
															<table style="width:100%;">
																<tr>
																	<td>
																		1-Does Not Meet Expectations</td>
																	<td style="text-align: center">
																		2-Sometimes Meets Expectations</td>
																	<td style="text-align: center">
																		3-Regularly Meets Expectations</td>
																	<td style="text-align: center">
																		4-Sometimes Exceeds Expectations</td>
																	<td style="text-align: right">
																		5-Always Exceeds Expectations</td>
																</tr>
															</table>
														</TitlePanel>
													</Templates>
												</dx:ASPxGridView>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage ClientEnabled="False" Text="Core Responsibility Review">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<dx:ASPxCheckBox ID="cb_expand0" runat="server" AutoPostBack="True" 
													CheckState="Unchecked" ClientInstanceName="cb_expand" Text="Expand All">
												</dx:ASPxCheckBox>
												<dx:ASPxGridView ID="gv_reviewitem0" runat="server" AutoGenerateColumns="False" 
													ClientInstanceName="gv_reviewitem0" Font-Names="Arial" KeyFieldName="id" 
													OnCustomCallback="gv_reviewitem_CustomCallback" 
													OnCustomColumnDisplayText="gv_reviewitem0_CustomColumnDisplayText" 
													OnHtmlRowCreated="gv_reviewitem_HtmlRowCreated" 
													OnHtmlRowPrepared="gv_reviewitem_HtmlRowPrepared" Width="100%">
													<GroupSummary>
														<dx:ASPxSummaryItem DisplayFormat="This isa  test" 
															ShowInColumn="Responsibility" ShowInGroupFooterColumn="Responsibility" />
													</GroupSummary>
													<Columns>
														<dx:GridViewDataTextColumn Caption="Responsibility" FieldName="cr" 
															ShowInCustomizationForm="True" VisibleIndex="0" Width="100px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Review Item" FieldName="cr_review_question" 
															ShowInCustomizationForm="True" VisibleIndex="1" Width="220px" CellStyle-Wrap="True">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="historyid" FieldName="id" 
															ShowInCustomizationForm="True" Visible="False" VisibleIndex="5">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Score" FieldName="score" 
															ShowInCustomizationForm="True" VisibleIndex="2" Width="215px">
															<DataItemTemplate>
																<dx:ASPxRadioButtonList ID="ASPxRadioButtonList2" runat="server" 
																	Font-Names="Arial" Font-Size="7pt" Height="10px" ItemSpacing="0px" 
																	oninit="ASPxRadioButtonList2_Init" RepeatDirection="Horizontal" 
																	RepeatLayout="Flow" TextAlign="Left" TextSpacing="0px" TextWrap="False" 
																	Value='<%# Eval("score") %>' ValueType="System.Int32" Width="200px">
																	<RadioButtonStyle Wrap="False">
																	</RadioButtonStyle>
																	<Paddings Padding="0px" />
																	<Items>
																		<dx:ListEditItem Text="1" Value="1" />
																		<dx:ListEditItem Text="2" Value="2" />
																		<dx:ListEditItem Text="3" Value="3" />
																		<dx:ListEditItem Text="4" Value="4" />
																		<dx:ListEditItem Text="5" Value="5" />
																		<dx:ListEditItem Text="N/A" Value="0" />
																	</Items>
																	<Border BorderStyle="None" />
																</dx:ASPxRadioButtonList>
															</DataItemTemplate>
															<CellStyle HorizontalAlign="Center" Wrap="False">
																<Paddings PaddingLeft="0px" PaddingRight="0px" />
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
															ShowInCustomizationForm="True" VisibleIndex="3" Width="200px">
															<DataItemTemplate>
																<dx:ASPxMemo ID="memreviewnotes0" runat="server" BackColor="#FFFFCC" 
																	Height="30px" oninit="memreviewnotes_Init" Text='<%# Eval("notes") %>' 
																	Width="100%">
																</dx:ASPxMemo>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsBehavior ColumnResizeMode="Control" />
													<SettingsPager Mode="ShowAllRecords">
													</SettingsPager>
													<Settings GroupFormat="{1} {2}" ShowGroupFooter="VisibleIfExpanded" 
														ShowTitlePanel="True" />
													<SettingsText EmptyDataRow="There are no responsibilities in the employee agreement, or there are no review questions." />
													<Templates>
														<TitlePanel>
															<table style="width:100%;">
																<tr>
																	<td>
																		1-Does Not Meet Expectations</td>
																	<td style="text-align: center">
																		2-Sometimes Meets Expectations</td>
																	<td style="text-align: center">
																		3-Regularly Meets Expectations</td>
																	<td style="text-align: center">
																		4-Sometimes Exceeds Expectations</td>
																	<td style="text-align: right">
																		5-Always Exceeds Expectations</td>
																</tr>
															</table>
														</TitlePanel>
													</Templates>
												</dx:ASPxGridView>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage ClientVisible="False" Text="Action Plan" Visible="False">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage Text="Milestones" ClientEnabled="False">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<dx:ASPxGridView ID="gv_milestones" runat="server" AutoGenerateColumns="False" 
													ClientInstanceName="gv_milestones" Font-Names="Arial" KeyFieldName="id" 
													OnCustomCallback="gv_milestones_CustomCallback" Width="100%">
													<Columns>
														<dx:GridViewDataTextColumn FieldName="id" ShowInCustomizationForm="True" 
															Visible="False" VisibleIndex="5">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Milestone" FieldName="milestone" 
															ShowInCustomizationForm="True" VisibleIndex="0" Width="50%">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataDateColumn Caption="Due" FieldName="due" 
															ShowInCustomizationForm="True" VisibleIndex="1" Width="70px">
															<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
															</PropertiesDateEdit>
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
															ShowInCustomizationForm="True" VisibleIndex="2" Width="50%">
															<DataItemTemplate>
																<dx:ASPxMemo ID="mem_milestones" runat="server" BackColor="#FFFFCC" 
																	Height="30px" oninit="mem_milestones_Init" Text='<%# Eval("notes") %>' 
																	Width="100%">
																</dx:ASPxMemo>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataCheckColumn Caption="Completed" FieldName="completed" 
															ShowInCustomizationForm="True" VisibleIndex="4" Width="40px">
															<DataItemTemplate>
																<dx:ASPxCheckBox ID="milestone_complete" runat="server" CheckState="Unchecked" 
																	oninit="milestone_complete_Init" Text=" " Value='<%# Eval("completed") %>' 
																	ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
																</dx:ASPxCheckBox>
															</DataItemTemplate>
														</dx:GridViewDataCheckColumn>
														<dx:GridViewDataTextColumn Caption="Ticket" FieldName="ticketid" 
															ShowInCustomizationForm="True" VisibleIndex="3" Width="70px">
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsText EmptyDataRow="There are no milestones set in the employment agreement" />
												</dx:ASPxGridView>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage Text="Member History" ClientEnabled="False" ClientVisible="False">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<dx:ASPxGridView ID="gv_history" runat="server" AutoGenerateColumns="False" 
													Font-Names="Arial" Width="100%">
													<Columns>
														<dx:GridViewDataTextColumn Caption="Date" FieldName="date" 
															ShowInCustomizationForm="True" VisibleIndex="0">
															<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
															</PropertiesTextEdit>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Action" FieldName="action" 
															ShowInCustomizationForm="True" VisibleIndex="1">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
															ShowInCustomizationForm="True" VisibleIndex="2">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Added By" FieldName="added_by" 
															ShowInCustomizationForm="True" VisibleIndex="3">
														</dx:GridViewDataTextColumn>
													</Columns>
												</dx:ASPxGridView>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
								</TabPages>
							</dx:ASPxPageControl>
						</td>
					</tr>
				</table>
			</dx:PanelContent>
</PanelCollection>
</dx:ASPxCallbackPanel>

			</td>
		
		</tr>
	</table>
	<br />
	
	<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" >
		<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<asp:HiddenField ID="hdnr_id" runat="server" />
	<asp:HiddenField ID="hdnmemberid" runat="server" />
	<br />
			</dx:PanelContent>
</PanelCollection>
	</dx:ASPxCallbackPanel>




</asp:Content>

<asp:Content ID="Content4" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style1
		{
			font-family: Calibri;
			text-align: left;
		}
		.style2
		{
			width: 100%;
			font-family: Calibri;
		}
		.style3
		{
			font-family: Calibri;
			height: 19px;
		}
		.style4
		{
			height: 19px;
		}
		</style>
	</asp:Content>


