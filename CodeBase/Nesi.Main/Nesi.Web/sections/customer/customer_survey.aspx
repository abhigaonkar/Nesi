<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="customer_survey" Title="Customer Survey" Codebehind="customer_survey.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



	


	
	
	
	<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript">

	</script>
		
			
<table style="width: 1000px; font-family: Arial; border-collapse: collapse;">
			<tr>
				<td>
					&nbsp;</td>
				<td>
					<img alt="nesi" id="_logo" runat="server" 
						 height="70" /></td>
				<td>
					<table style="width: 100%; font-size: small;">
						<tr>
							<td width="100%">
								&nbsp;</td>
							<td nowrap="nowrap">
								Surveys Completed To Date:</td>
							<td>
								<dx:ASPxLabel ID="s_comp" runat="server" ClientInstanceName="s_comp" Text="0">
								</dx:ASPxLabel>
							</td>
						</tr>
						
						<tr>
							<td>
								&nbsp;</td>
							<td>
								&nbsp;</td>
							<td>
								&nbsp;</td>
						</tr>
					</table>
				</td>
				<td>
					&nbsp;</td>
			</tr>
			<tr>
				<td style="font-family: Arial">
					&nbsp;</td>
				<td style="font-family: Arial">
					&nbsp;</td>
				<td>
					&nbsp;</td>
				<td>
					&nbsp;</td>
			</tr>
			<tr>
				<td style="font-family: Arial">
					&nbsp;</td>
				<td style="font-family: Arial; text-align: left;" colspan="2">
					<dx:ASPxLabel ID="lblname" runat="server" Font-Names="Arial" Font-Size="20px">
					</dx:ASPxLabel>
				</td>
				<td>
					&nbsp;</td>
			</tr>
			<tr>
				<td class="style2">
					&nbsp;</td>
				<td colspan="2" class="style2">
					We value your feedback and welcome you to let us know how we are doing.&nbsp; If 
					you would like to participate, please review the following work orders and rate 
					your overall satisfaction on the right side.&nbsp; If you wish to leave a note, 
					please do so in the yellow box beside each work order.</td>
				<td class="style2">
					&nbsp;</td>
			</tr>
			<tr>
				<td class="style2">
					&nbsp;</td>
				<td colspan="2" class="style2">
					<table valign="top" 
											style="width:100%; color: #000000; border-collapse: collapse; padding-top: 0px; margin-top: 0px;" 
											__designer:mapid="2714">
						<tr __designer:mapid="2715">
							<td valign="top" __designer:mapid="2716">
								<strong>Score Legend:</strong></td>
						</tr>
						<tr __designer:mapid="2717">
							<td align="left" class="style7" style="text-align: left; padding-left: 10px;" 
													__designer:mapid="2718">
								<span class="style4" __designer:mapid="2719">1-Failed to meet my any of my expectations<br />
								2-Sometimes failed to meet my expectations<br __designer:mapid="271a" />3-Was as I 
													expected<br />
								4-Sometimes exceeded my expectations</span><br class="style4" __designer:mapid="271b" />
								<span 
														class="style4" __designer:mapid="271c">5-Exceeded all of my 
													expectations<br />
								<br />
								<dx:ASPxLabel ID="lblsdate" runat="server" Font-Names="Arial">
								</dx:ASPxLabel>
								</span>
							</td>
						</tr>
					</table>
				</td>
				<td class="style2">
					&nbsp;</td>
			</tr>
			<tr>
				<td class="style1">
					&nbsp;</td>
				<td class="style1" colspan="2">
					<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" Theme="NETheme01"
						ClientInstanceName="gv" Font-Names="Arial" KeyFieldName="WOProg_ID" 
						Width="100%" SettingsBehavior-EnableRowHotTrack="False">
						<Columns>
							<dx:GridViewDataTextColumn Caption="Work Order" Width="80px" FieldName="WOProg_BVWO" 
								VisibleIndex="0">
								<CellStyle BackColor="#99CCFF">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Description" FieldName="woprog_description" 
								VisibleIndex="1" Width="200px" CellStyle-Wrap="True">
<CellStyle Wrap="True"></CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Date Finished" 
								FieldName="WOProg_OpenDateTime" VisibleIndex="2" Width="80px">
								<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
								</PropertiesTextEdit>
								<CellStyle Wrap="False">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Score" VisibleIndex="4" Width="100px">
								<DataItemTemplate>
									<dx:ASPxRatingControl ID="rc" runat="server" oninit="rc_Init" 
										 Value='<%# GetRatingValue(Eval("rating")) %>'>
									</dx:ASPxRatingControl>
								</DataItemTemplate>
								<CellStyle BackColor="#CCFFCC">
								</CellStyle>
								<HeaderCaptionTemplate>
									Score
								</HeaderCaptionTemplate>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Check List" Width="200px" VisibleIndex="5">
								<DataItemTemplate>
									<table style="width:100%; ">
										<tr>
											<td>
												<dx:ASPxCheckBox ID="chk1" runat="server" oninit="chk1_Init" 
													Text="Area was left clean" CheckState="Unchecked" Value=<%# Eval("clean") %> ValueType="System.Int32" Font-Names="Calibri">
												</dx:ASPxCheckBox>
											</td>
										</tr>
										<tr>
											<td>
												<dx:ASPxCheckBox ID="chk2" runat="server" oninit="chk2_Init" 
													Text="Job was completed on time" Wrap="False" CheckState="Unchecked" Value='<%# Eval("ontime") %>' Font-Names="Calibri" ValueType="System.Int32">
												</dx:ASPxCheckBox>
											</td>
										</tr>
										<tr>
											<td>
												<dx:ASPxCheckBox ID="chk3" runat="server" oninit="chk3_Init" 
													Text="Can someone please call me?" Width="200px" Wrap="False" Font-Names="Calibri" CheckState="Unchecked" 
													Value='<%# Eval("callme") %>' ValueType="System.Int32">
												</dx:ASPxCheckBox>
											</td>
										</tr>
									</table>
								</DataItemTemplate>
								<CellStyle BackColor="#CCFFCC">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Notes" VisibleIndex="6" Width="100%">
								<DataItemTemplate>
									<dx:ASPxMemo ID="notes" runat="server" Height="65px" oninit="notes_Init" 
										Width="100%" Text='<%# Eval("notes") %>' BackColor="#FFFFCC" Font-Names="Arial">
									</dx:ASPxMemo>
								</DataItemTemplate>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="woprog_id" FieldName="WOProg_ID" 
								Visible="False" VisibleIndex="12">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="rating" FieldName="rating" Visible="False" 
								VisibleIndex="10">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="notes" FieldName="notes" Visible="False" 
								VisibleIndex="11">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="clean" FieldName="clean" Visible="False" 
								VisibleIndex="9">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="ontime" FieldName="ontime" Visible="False" 
								VisibleIndex="7">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="callme" FieldName="callme" Visible="False" 
								VisibleIndex="8">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Project Manager" FieldName="pm" CellStyle-Wrap="True" Width="80px"
								VisibleIndex="3">
<CellStyle Wrap="True"></CellStyle>
							</dx:GridViewDataTextColumn>
						    <dx:GridViewDataTextColumn Caption="Done?" FieldName="survey_finished" Visible="True" 
						                               VisibleIndex="12" width="50px">
						        <DataItemTemplate>
                                    <dx:ASPxCheckBox ID="ch_done" runat="server" CheckState="Unchecked" OnInit="ch_done_Init" Text=" ">
                                        
                                    </dx:ASPxCheckBox>
                                </DataItemTemplate>
						    </dx:GridViewDataTextColumn>
						</Columns>
						<SettingsBehavior AllowSort="False" ColumnResizeMode="Control" />
						<SettingsPager Mode="ShowAllRecords" Visible="False">
						</SettingsPager>
						<Settings GridLines="Horizontal" />
						<Styles>
							
						</Styles>
					</dx:ASPxGridView>
				</td>
				<td class="style1">
					&nbsp;</td>
			</tr>
			
			<tr>
				<td>

					&nbsp;</td>
				<td>

				</td>
				<td>
					&nbsp;</td>
				<td>
					&nbsp;</td>
			</tr>
			
			<tr>
				<td valign="top">
					&nbsp;</td>
				<td valign="top">
					<dx:ASPxButton ID="ASPxButton1" runat="server" Visible="False" Theme="NETheme01"
						Text="I'm Finished, close this survey" Width="140px" Height="35px" 
						onclick="ASPxButton1_Click" ClientEnabled="False" Wrap="True">
						<ClientSideEvents Click="function(s, e) {
	s.SetEnabled(false);
cb.PerformCallback('c');
confirm('This survey has been closed.  To view this survey later, please log onto www.nesi.ca using your login credentials');
                          window.location.href=window.location.href;
}" />
					</dx:ASPxButton>
				</td>
				<td align="right" valign="top">
					&nbsp;</td>
				<td align="right" valign="top">
					&nbsp;</td>
			</tr>
			
			<tr>
				<td valign="top">
					&nbsp;</td>
				<td valign="top">
					
				</td>
				<td valign="middle" align="left" width="100%">
					</td>
				<td align="right" valign="top">
					&nbsp;</td>
			</tr>
			
			<tr>
				<td valign="top" style="text-align: center">
					&nbsp;</td>
				<td valign="top" colspan="2" style="text-align: center">
					<span class="style8">
					<br />
                    <br />
					<br />
					If you wish to review this survey on a later date, please visit </span>
					<a href="https://www.nesi.ca/"><span class="style8">www.nesi.ca</span></a><span 
						class="style8"> and use your customer login and password</span></td>
				<td valign="top" style="text-align: center">
					&nbsp;</td>
			</tr>
			
			<tr>
				<td valign="top" style="padding: 5px; text-align: center" class="style9">
					&nbsp;</td>
				<td valign="top" colspan="2" style="padding: 5px; text-align: center" 
					class="style9">
					<br />
					<dx:ASPxHyperLink ID="lnk_getpassword" runat="server" Cursor="pointer" 
						Font-Names="Arial" 
						Text="HELP! I forget my username and password for my login!">
						<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('p');
	
}" />
					</dx:ASPxHyperLink>
				</td>
				<td valign="top" style="padding: 5px; text-align: center" class="style9">
					&nbsp;</td>
			</tr>
			
			<tr>
				<td valign="top" style="padding: 5px; text-align: center">
					&nbsp;</td>
				<td valign="top" colspan="2" style="padding: 50px; text-align: center">
					<dx:ASPxHyperLink ID="lnk_unsubscribe" runat="server" Cursor="pointer" 
						Font-Names="Arial" 
						Text="Click here if you do not wish to participate in any more work order feedback surveys">
						<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('x');
                            gv.SetEnabled(false);
}" />
					</dx:ASPxHyperLink>
				</td>
				<td valign="top" style="padding: 5px; text-align: center">
					&nbsp;</td>
			</tr>
		</table>

		<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
						oncallback="cb_Callback" Width="150px">
					
							<ClientSideEvents BeginCallback="function(s, e) {
s.cp_alert = '';
	gv.SetEnabled(false);
}" 
 EndCallback="function(s, e) {
	
if (s.cp_alert!='')
{
	confirm(s.cp_alert);
                             
}
   gv.SetEnabled(true);
}" />
							<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True"></dx:PanelContent>
</PanelCollection>
					
							</dx:ASPxCallbackPanel>


		</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style1
		{
			height: 22px;
		}
		.style2
		{
			font-size: small;
			height: 34px;
		}
		.style4
		{
			font-size: x-small;
		}
		.style7
		{
			font-family:Arial;
			text-align: left;
			color: #000000;
		}
		.style8
		{
			font-size: small;
		}
		.style9
		{
			height: 25px;
		}
		.auto-style1 {
            text-align: left;
        }
		</style>
</asp:Content>
