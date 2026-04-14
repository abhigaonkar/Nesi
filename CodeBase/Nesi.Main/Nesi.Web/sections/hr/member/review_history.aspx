<%@ Page Title="Employee Review History" Language="C#" MasterPageFile="~/NonFrame.master" AutoEventWireup="true" EnableTheming = "True" Inherits="sections_hr_member_review_history" Codebehind="review_history.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" runat="Server">
        <script type="text/javascript">


			function bind_tooltips() {
				$(".opt1").each(function () {
					$(this).tip();
				});
			}
			$(document).ready(function () {
				//		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
				bind_tooltips();
			});

			function EndReqHandler() {
				bind_tooltips();

			}
			function resizeIframe(obj) {
				obj.style.height = (obj.contentWindow.document.body.scrollHeight + 100) + 'px';

			}
			function print_review(id) {
				//	boing("/sections/reports/invoice_preview/index.aspx?id=" + id, 'invoice_preview', 850, 850);

				boing("print_review.aspx?rid=" + id, 'printrev', 900, 900);
			}
			function print_mreview(mid) {
				//	boing("/sections/reports/invoice_preview/index.aspx?id=" + id, 'invoice_preview', 850, 850);

				boing("print_review.aspx?rid=0&locked=0&is_worksheet=1&mid=" + mid, 'printrev', 900, 900);
			}

			function open_review(pass) {

				var id = pass[0];
				var mid = pass[1];

				
			boing("./review_list_for_member.aspx?id=" + id + "&memberid=" + mid , 'review' + id, 960, 720);
			}
			function start_review(s, e) {
				var id = combo_select_user.GetValue();
				if (id != null && !isNaN(id)) {
					boing("./review_list_for_member.aspx?id=0&memberid=" + id, "review" + id, 960, 720);
				}
				else {
					alert("Please select a user")
				}
			}

		</script>
		<div runat="server" id="js_handlers">
		<script type="text/javascript">
			function change_reviewer(obj)
				{
				var query_string	=	{
										a:		"update_reviewer",
										id:		$(obj).attr("data-review_id"),
										rid:	$(obj).val()
										}
				$.ajax(
					{
					type:		"GET",
					cached:		false,
					url:		"./review_history.aspx",
					data:		query_string,
					dataType:	"text",
					beforeSend:	function()
									{
									please_wait("start", "Saving...");
									},
					success:	function(ret)
									{
									if(ret != "SUCCESS")
										{
										alert(ret);
										$(obj).val($(obj).attr("data-original_id"));
										}
									else
										{
										alert("Saved");
										$(obj).attr("data-original_id", query_string.id);
										}
									},
					error:		function()
									{
									alert("There was an error saving your request");
									$(obj).val($(obj).attr("data-original_id"));
									please_wait("stop");
									},
					complete:	function()
									{
									please_wait("stop");
									}
					});
				}
		</script>
		</div>
	<dx:ASPxPanel ID="pnl_menu" runat="server" Width="100%">
		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<table>
					<tr>
						<td>
							<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
								OnLoad="ASPxButton1_Init" Text="What will I be reviewed on?" Width="214px" Theme="NETheme01">
							</dx:ASPxButton>
						</td>
					</tr>
					<tr>
						<td>
							<table ID="pnl_create_review" runat="server" 
								style="font-size: 12px; font-family: arial;" Visible="False">
								<tr runat="server">
									<td runat="server">
										<b ID="lbl_select_user" runat="server">Create a new review for a user that 
										reports to you:</b>
									</td>
									<td runat="server">
										<dx:ASPxComboBox ID="combo_select_user" runat="server" 
											ClientInstanceName="combo_select_user" EnableCallbackMode="True" 
											IncrementalFilteringMode="Contains" TextField="member_fullname" 
											ValueField="member_id" Theme="NETheme01">
										</dx:ASPxComboBox>
									</td>
									<td runat="server">
										<dx:ASPxButton ID="bt_select_user" runat="server" AutoPostBack="False" 
											Text="Start Review" Theme="NETheme01">
											<ClientSideEvents Click="start_review" />
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxPanel>
		<br />
		<div style='font-size:14px;font-family:arial;'>
		This grid includes all your existing reviews and reviews for all the people who report to you.<br />
		</div>
	<lc:LayoutControl runat="server" ID="layout" GridviewID="gv_review" ShowExcelExport="True" ShowPDFExport="True" ShowToggle="True" Visible="True" />
	<dx:ASPxGridView ID="gv_review" runat="server" AutoGenerateColumns="False" 
			KeyFieldName="id" Width="100%" 
			OnHtmlEditFormCreated="gv_review_HtmlEditFormCreated" 
			OnStartRowEditing="gv_review_StartRowEditing" 
			OnHtmlRowPrepared="gv_review_HtmlRowPrepared" 
			OnCustomCallback="gv_review_CustomCallback" ClientInstanceName="gv_review" 
			OnCustomJSProperties="gv_review_CustomJSProperties" 
			onhtmldatacellprepared="gv_review_HtmlDataCellPrepared" 
			oncommandbuttoninitialize="gv_review_CommandButtonInitialize" 
			onhtmlcommandcellprepared="gv_review_HtmlCommandCellPrepared" 
			onrowdeleting="gv_review_RowDeleting" Theme="NETheme01" OnBatchUpdate="gv_review_BatchUpdate" 
		>
		<Settings ShowFilterRow="True" ShowFilterBar="Visible" ShowFilterRowMenu="True" ShowHeaderFilterButton="True"></Settings>
		<Templates>
		</Templates>
		<ClientSideEvents CustomButtonClick="function(s, e) {
	if(e.buttonID == 'print_review'){
                    var rowVisibleIndex = e.visibleIndex;
                    var rowKeyValue;
					s.GetRowValues( e.visibleIndex,'id',print_review);
					}
	else if (e.buttonID == 'edit'){ 
  					 var index = e.visibleIndex;
    				s.GetRowValues( e.visibleIndex,'id;memberid',open_review);

}
}" />
		<SettingsPager PageSize="25">
		</SettingsPager>
		<SettingsEditing Mode="Batch" />
		<Settings ShowFilterRow="True" ShowTitlePanel="True" VerticalScrollableHeight="900" ShowGroupPanel="True" />
		<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" 
			ConfirmDelete="True" />
		<SettingsResizing ColumnResizeMode="Control" />
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" >
<Image Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
                                                                    </DeleteButton>
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" >
<Image Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
                                                                    </EditButton>
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" >
<Image Width="16px" Url="~/images/icon/icon[add].gif"></Image>
                                                                    </NewButton>
																</SettingsCommandButton>
		<SettingsPopup>
			<EditForm Height="900px" HorizontalAlign="WindowCenter" Modal="True" VerticalAlign="WindowCenter" Width="1050px" />
		</SettingsPopup>
		<Columns>
			<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" ShowDeleteButton="true" ShowClearFilterButton="true"
				Width="60px">
				
				
				
				<CustomButtons>
					<dx:GridViewCommandColumnCustomButton ID="edit" Text="Edit">
						<Image Url="~/images/icon/icon[edit].gif" ToolTip="Open Review">
						</Image>
					</dx:GridViewCommandColumnCustomButton>
					<dx:GridViewCommandColumnCustomButton ID="print_review">
						<Image Url="~/images/icon/icon[print].gif" ToolTip="Print Review">
						</Image>
					</dx:GridViewCommandColumnCustomButton>
				</CustomButtons>
				<CellStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False">
				</CellStyle>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" VisibleIndex="1" Width="25px">
				<EditFormSettings Visible="False" />
				<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Date of Review" FieldName="date" 
				VisibleIndex="2" Width="90px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<Settings AutoFilterCondition="Equals" />
				<EditFormSettings Visible="False" />
				<EditFormSettings Visible="False"></EditFormSettings>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="Employee" FieldName="member_id" VisibleIndex="3" Width="40%">
				<Settings AutoFilterCondition="Contains" />
				<EditFormSettings Visible="False"></EditFormSettings>
				<DataItemTemplate>
					<asp:HyperLink ID="hl_member" runat="server" Font-Bold="false" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/hr/member/index.aspx?id={0}','Employee',1035,800);&quot;, Eval(&quot;memberid&quot;)) %>" Text='<%# Eval("member_id") %>'></asp:HyperLink>
				</DataItemTemplate>
				<EditFormSettings Visible="False" />
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Locked" FieldName="locked" VisibleIndex="4" Width="60px">
				<Settings AutoFilterCondition="Equals" />
				<EditFormSettings Visible="False" />
				<EditFormSettings Visible="False"></EditFormSettings>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Business Unit" VisibleIndex="6" Width="80px" 
				FieldName="ddl_name">
				<Settings AutoFilterCondition="Equals" />
				<SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
				<EditFormSettings Visible="False" />
				<EditFormSettings Visible="False"></EditFormSettings>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Offer" FieldName="offerid" VisibleIndex="7" Width="50px">
				<EditFormSettings Visible="False"></EditFormSettings>
				<DataItemTemplate>
					<asp:HyperLink ID="hl_offer" runat="server" Font-Bold="false" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/hr/member/member_offer.aspx?id={0}','Employee_Agreement',1035,800);&quot;, Eval(&quot;offerid&quot;)) %>" Text='<%# Eval("offerid") %>'></asp:HyperLink>
				</DataItemTemplate>
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Membertype" FieldName="membertype_name" VisibleIndex="8" Width="75px">
				<EditFormSettings Visible="False"></EditFormSettings>
				<DataItemTemplate>
					<asp:HyperLink ID="hl_mt" runat="server" Font-Bold="false" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/hr/member/m_type_detail.aspx?mt_id={0}','Membertype',1035,800);&quot;, Eval(&quot;membertype_id&quot;)) %>" Text='<%# Eval("membertype_name") %>'></asp:HyperLink>
				</DataItemTemplate>
				<EditFormSettings Visible="False" />
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Released" FieldName="was_printed" VisibleIndex="9" Width="60px">
				<Settings AutoFilterCondition="Contains" />
				<EditFormSettings Visible="False" />
				<EditFormSettings Visible="False"></EditFormSettings>
				<CellStyle Wrap="False" HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Reports To" FieldName="rt_name" VisibleIndex="10" Width="75px">
				<Settings AutoFilterCondition="Equals" />
				<EditFormSettings Visible="False" />
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="memberid" FieldName="memberid" 
				Visible="False" VisibleIndex="15" ShowInCustomizationForm="False">
				<EditFormSettings Visible="False" />
				<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="membertype_id" FieldName="membertype_id" 
				Visible="False" VisibleIndex="16" ShowInCustomizationForm="False">
				<EditFormSettings Visible="False" />
				<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Status" FieldName="status" MinWidth="20" 
				VisibleIndex="11" Width="100px">
				<Settings AutoFilterCondition="Contains" />
				<SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
				<EditFormSettings Visible="False" />
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="HR Status" FieldName="hrstatus" 
				VisibleIndex="12" Width="60px">
				<Settings AutoFilterCondition="Contains" />
			    <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
			    <EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Gen Score" FieldName="score" 
				ReadOnly="True" VisibleIndex="13" Width="60px">
				<Settings AutoFilterCondition="Less" />
				<EditFormSettings Visible="False" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="CR Score" FieldName="_score" 
				VisibleIndex="14" Width="60px">
				<Settings AutoFilterCondition="Less" />
				<EditFormSettings Visible="False" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
		    <dx:GridViewDataTextColumn Caption="Years" FieldName="year" VisibleIndex="17" Width="50px">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
		    <dx:GridViewDataComboBoxColumn Caption="Reviewed By" FieldName="rb_id" VisibleIndex="5" Width="200px" ToolTip="Click to Edit">
                <PropertiesComboBox TextField="_name" ValueField="id" ValueType="System.Int32" DataSourceID="Sqlmem">
                </PropertiesComboBox>
                <Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
                <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
                <EditFormSettings Visible="True" />
                <BatchEditModifiedCellStyle BackColor="#CCFFCC">
                </BatchEditModifiedCellStyle>
                <CellStyle HorizontalAlign="Center" Wrap="False" BackColor="#FFFFCC">
                </CellStyle>
            </dx:GridViewDataComboBoxColumn>
		</Columns>
		<Styles>
			<CommandColumnItem HorizontalAlign="Center" VerticalAlign="Middle">
				<Paddings PaddingLeft="2px" />
				<Paddings PaddingLeft="2px"></Paddings>
			</CommandColumnItem>
		</Styles>
	</dx:ASPxGridView>
	<br />
	<asp:HiddenField ID="hdnr_id" runat="server" />
	  <asp:SqlDataSource ID="Sqlmem" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" >
								
							</asp:SqlDataSource>
	<asp:HiddenField ID="hdnmemberid" runat="server" />
    </asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="header_placeholder">
	</asp:Content>

