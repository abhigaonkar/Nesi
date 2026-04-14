<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="quote_startframe" EnableTheming="True" Theme="NETheme01" Title="Start Quote" Codebehind="startframe.aspx.cs" %>


<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>











<asp:content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<asp:scriptmanager ID="sm" runat="server">
	</asp:scriptmanager>
<asp:updatepanel ID="up" runat="server">
<ContentTemplate>
	<script type="text/javascript" language="javascript">
function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 100) + 'px';

 }
	
 </script>
	
				<div id="lblerror" Runat="Server" style="font-family: Arial, Helvetica, sans-serif; font-size: 10pt; font-weight: bold; color: #FF0000"></div>
				<table style="border-style: none; width: 100%; font-family: Arial; font-size: small; border-collapse: collapse; border-spacing: 0px;" 
		cellpadding="3">
					<tr>
						<td>
							Customer:</td>
						<td width="100%">
							<dx:ASPxComboBox ID="ddlcustomer" runat="server" CallbackPageSize="10" 
								EnableCallbackMode="True" TextField="_name" 
								ValueField="id" ValueType="System.Int32" Width="100%" FilterMinLength="3" Font-Names="Arial" 
								IncrementalFilteringDelay="0" IncrementalFilteringMode="Contains" 
								OnItemsRequestedByFilterCondition="ASPxComboBox1_ItemsRequestedByFilterCondition" 
								ClientInstanceName="ddlcustomer">
								<ClientSideEvents TextChanged="function(s, e) {
	gv.PerformCallback(s.GetValue());
}" SelectedIndexChanged="function(s, e) {
	ddlcontact.PerformCallback(s.GetValue());
ddladdress1.PerformCallback(s.GetValue());
}" />

							</dx:ASPxComboBox>
							<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
							</asp:SqlDataSource>
						</td>
					</tr>
					<tr>
						<td>
							Address:</td>
						<td>
							<dx:ASPxComboBox ID="ddladdress1" runat="server" CallbackPageSize="10" 
								TextField="_addr" 
								ValueField="id" ValueType="System.Int32" Width="100%" Font-Names="Arial" 
								IncrementalFilteringDelay="0" 
								ClientInstanceName="ddladdress1" oncallback="ddladdress_Callback">
							</dx:ASPxComboBox>
						</td>
					</tr>
					<tr>
						<td>
							Contact:</td>
						<td>
							<dx:ASPxComboBox ID="ddlcontact" runat="server" ClientInstanceName="ddlcontact" 
								IncrementalFilteringMode="StartsWith" 
								OnCallback="ASPxComboBox2_Callback" TextField="_name" ValueField="id" 
								ValueType="System.Int32" DropDownStyle="DropDown" Width="250px">
								<ClientSideEvents ButtonClick="function(s, e) {
if (ddlcustomer.GetText()!=&quot;&quot;)
{
tbnewcontact.SetText(s.GetInputElement().value);
popc.Show();}
}" />
								<Buttons>
									<dx:EditButton Text="Add">
										<Image Url="~/images/icon/icon[add].gif">
										</Image>
									</dx:EditButton>
								</Buttons>
							</dx:ASPxComboBox>
						</td>
					</tr>
					<tr>
						<td>
							Expected $ Value:</td>
						<td>
							<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="170px">
								<clientsideevents keydown="function(s, e) {only_numeric(event)}" />
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
                        <td>Chance of Winning:</td>
                        <td>
                            <dx:ASPxSpinEdit ID="spn_chances" runat="server" AllowUserInput="False" ClientInstanceName="spn_chances" Increment="30" MinValue="30" MaxValue="90" Number="30" ValidationSettings-ErrorDisplayMode="None" ShowOutOfRangeWarning="False" />
                        </td>
                    </tr>
					<tr>
						<td>
							When is this quote due?</td>
						<td>
							<dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" 
								DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
								EditFormatString="yyyy-MM-dd">
							</dx:ASPxDateEdit>
						</td>
					</tr>
					<tr>
						<td nowrap="nowrap">
							When will the job be invoiced?</td>
						<td>
							<dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" 
								DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
								EditFormatString="yyyy-MM-dd">
							</dx:ASPxDateEdit>
						</td>
					</tr>
					<tr>
						<td valign="top">
							Job Description:</td>
						<td>
							<dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="30px" Width="100%">
							</dx:ASPxMemo>
						</td>
					</tr>
					<tr>
						<td>
							Business Unit:</td>
						<td>
							<dx:ASPxComboBox ID="ddlbranch" runat="server" ClientInstanceName="ddlbranch" 
								DataSourceID="SqlDataSource3" TextField="ddl_name" ValueField="id" 
								ValueType="System.Int32">
								<ClientSideEvents SelectedIndexChanged="function(s, e) {
var this_val		= s.GetValue();
	if(!chk_lockquoter.GetChecked())
	{
	ddlpm.PerformCallback(this_val);
	}
}" />
							</dx:ASPxComboBox>
							<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								>
							</asp:SqlDataSource>
						</td>
					</tr>
					
					<tr>
						<td>
							To be Managed By:</td>
						<td>
							<table cellpadding="0" cellspacing="0">
							<tr>
							<td>
							<dx:ASPxComboBox ID="ddlpm" runat="server" ClientInstanceName="ddlpm" 
								DataSourceID="SqlDataSource5" oncallback="ddlpm_Callback" TextField="_name" 
								ValueField="member_id" ValueType="System.Int32">
								<ClientSideEvents EndCallback="function(s, e) {
	 var index = s.cpSelectedIndex;
                                    if (index!=null)
                                    {
          s.SetSelectedIndex(index);
                                    }
}" />
							</dx:ASPxComboBox>
							</td>
							<td><img src="/images/icon/icon[lock].gif" title="Lock Quoter, allowing the selecting of other branches" /></td>
							<td><dx:ASPxCheckBox runat="server" ClientInstanceName="chk_lockquoter" ID="chk_lockquoter" Text="Lock Quoter" AutoPostBack="True" oncheckedchanged="chk_lockquoter_CheckedChanged" >
							</dx:ASPxCheckBox></td>
							</tr>
							</table>
							<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
							</asp:SqlDataSource>
						</td>
					</tr>
					<tr>
						<td style="padding: 5px 0px 0px 0px; border-collapse: collapse; background-color: #FFFFFF;" 
							valign="top" colspan="2" width="100%">
							<table id="tblq" RunAt="server" style="display: block; width: 100%;" 
								bgcolor="#CCFFCC">
								<tr>
									<td valign="top">
							Qualifiers:</td>
									<td width="100%">
							<dx:ASPxCheckBoxList ID="cbl" runat="server" ClientInstanceName="cbl" TextSpacing="10px" 
								ValueType="System.Int32" Width="100%" ItemSpacing="5px" TextField="question" ValueField="id">
								<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
							</dx:ASPxCheckBoxList>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td colspan="2" style="padding: 5px 0px 0px 0px; border-collapse: collapse" 
							valign="top">
							<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
								ClientInstanceName="gv" Font-Names="Arial" oncustomcallback="gv_CustomCallback" 
								Width="100%" Font-Size="8pt" Theme="NETheme01">
								<Columns>
									<dx:GridViewDataTextColumn Caption="Quote ID" FieldName="quoteid" 
										VisibleIndex="0" Width="60px">
										<PropertiesTextEdit DisplayFormatString="{0}">
										</PropertiesTextEdit>
										<DataItemTemplate>
													<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
														NavigateUrl="<%# string.Format(&quot;javascript:boing('index.aspx?a=g&quote_id={0}&revision={1}', 'quote_d',1100,900)&quot;,Eval(&quot;quoteid&quot;),Eval(&quot;rev&quot;)) %>" 
														Text='<%# Eval("quoteid") %>' />
												</DataItemTemplate>
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="1" 
										Width="70px">
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
										VisibleIndex="2" Width="100%">
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Quoted By" FieldName="quotedby" 
										VisibleIndex="3" Width="100px">
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
								</Columns>
								<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" />

<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True"></SettingsBehavior>

								<SettingsPager pagesize="5">
								</SettingsPager>
								<Settings ShowTitlePanel="True" GridLines="Vertical" 
									ShowColumnHeaders="False" />
								<SettingsText Title="Other Open Quotes for this Customer" />

<Settings ShowTitlePanel="True" ShowColumnHeaders="False" GridLines="Vertical"></Settings>

<SettingsText Title="Other Open Quotes for this Customer"></SettingsText>

								<Styles>
									<TitlePanel HorizontalAlign="Left"  Font-Bold="True" 
										Font-Size="11pt" ForeColor="White" verticalalign="Middle">
									</TitlePanel>
								</Styles>
							</dx:ASPxGridView>
						</td>
					</tr>
					<tr>
						<td>
							<dx:aspxbutton id="btnsave" runat="server" onclick="btnsave_Click" text="Save">
								<clientsideevents click="function(s, e) {
	 setTimeout(function () { s.SetEnabled(false); }, 10); 
}" />
							</dx:aspxbutton>
						</td>
						<td>
							&nbsp;</td>
					</tr>
					</table>
			
	<dx:ASPxCallbackPanel ID="cb_contact" runat="server" 
		ClientInstanceName="cb_contact" oncallback="cb_contact_Callback" Width="200px">
		<ClientSideEvents EndCallback="function(s, e) {
	ddlcontact.SetValue($('.hid_newcontact_id').val());
	ddlcontact.SetText(tbnewcontact.GetText());
	$('html').css({'overflow':'auto'});
}" />
		<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<br />
	<input type="hidden" ID="hid_newcontact_id" class="hid_newcontact_id" runat="server" />


	<dx:ASPxPopupControl ID="popc" runat="server" AppearAfter="0" 
		ClientInstanceName="popc" CloseAction="CloseButton" HeaderText="New Contact" 
		Modal="True" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" 
		PopupVerticalAlign="WindowCenter" Width="400px" Theme="NETheme01">
		
		<ModalBackgroundStyle Opacity="0">
		</ModalBackgroundStyle>
		<ContentCollection>
			<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
				<table style="width: 100%;">
					<tr>
						<td>
							Name:</td>
						<td nowrap="nowrap" style="white-space: nowrap">
							<dx:ASPxTextBox ID="tbnewcontact" runat="server" 
								ClientInstanceName="tbnewcontact" NullText="Enter Name" Width="100%">
								<NullTextStyle Font-Italic="True" ForeColor="#666666">
								</NullTextStyle>
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td>
							Title:</td>
						<td>
							<dx:ASPxComboBox ID="ddltitle" runat="server" DataSourceID="SqlDataSource6" 
								TextField="title_name" ValueField="title_id" ValueType="System.Int32">
							</dx:ASPxComboBox>
							<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								SelectCommand="Select * from titles"></asp:SqlDataSource>
						</td>
					</tr>
					<tr>
						<td>
							Email:</td>
						<td>
							<dx:ASPxTextBox ID="txtcontactemail" runat="server" Width="170px">
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td nowrap="nowrap">
							Cell Phone:</td>
						<td>
							<dx:ASPxTextBox ID="txtcellphone" runat="server" Width="170px">
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td>
							&nbsp;</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxButton ID="btnaddcontact" runat="server" AutoPostBack="False" 
								Text="Add">
								<ClientSideEvents Click="function(s, e) {
	cb_contact.PerformCallback(ddlcustomer.GetValue());
	ddlcontact.PerformCallback(ddlcustomer.GetValue());
	ddlcontact.SetText(tbnewcontact.GetText());

}" />
							</dx:ASPxButton>
						</td>
						<td>
							&nbsp;</td>
					</tr>
				</table>
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
			</dx:PanelContent>
</PanelCollection>
	</dx:ASPxCallbackPanel>
	<br />
			</ContentTemplate>
	</asp:updatepanel>
</asp:content>


