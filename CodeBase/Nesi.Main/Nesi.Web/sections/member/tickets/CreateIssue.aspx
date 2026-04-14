<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_member_tickets_CreateIssue" MasterPageFile="~/nonFrame.master"  Title="Creating New Ticket" EnableTheming="True" Codebehind="CreateIssue.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<asp:Content ContentPlaceHolderID="header_placeholder" ID="c_head" runat="server">
	<style type="text/css" id="style_head" runat="server">
			body
			{
			padding:			0px !important;
			margin:				0px !important;
			height:				100% !important;
			}
			
		.hideme
			{
			display:none;
			}
		.dxdvItem
			{
			height: auto !important;
			padding: 3px !important;
			border-color: #ccc !important;
			}
	</style>
	<asp:PlaceHolder id="ph_meta" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphMasterBody" ID="c_body" runat="server">
	<script type="text/javascript" src="/js/jquery.paste.js"></script>
	<script type="text/javascript">
		function handle_pasted(obj)
			{
			$(".tb_pastebox").css({ "border": "dashed 3px #999", "background-repeat": "no-repeat", "background-size": "100%", backgroundImage: "url('/images/ticket/bg[paste].png')" });
			$(".mainfileupload").parents("tr:first").show();
			$(".cancelpasted").hide();
			}
		$(document).ready(function ()
			{
			if($(".tb_pastebox").size() > 0)
				{
				$(".tb_pastebox").css({ "padding": "5px", "border": "dashed 3px #999", "background-repeat": "no-repeat", "background-size": "100%", backgroundImage: "url('/images/ticket/bg[paste].png')" });
				$(".tb_pastebox").pasteImageReader(function (results)
					{
					var dataURL, filename;
					$(".tb_pastebox").css({ "border": "solid 3px #090", "background-repeat": "no-repeat", "background-size": "100%", backgroundImage: "url('" + results.dataURL + "')" });
					// alert(results.dataURL);

					$(".fileblob").attr("value", results.dataURL);
					$(".mainfileupload").parents("tr:first").hide();
					$(".cancelpasted").show();
					return filename = results.filename, dataURL = results.dataURL, results;
					});
				}
			});
		function reset_form(s,e)
			{
			if(confirm("Are you sure you want to reset this form?"))
				{
				ddl_group.SetSelectedIndex(-1);
				ddl_page.SetSelectedIndex(-1);
				ddl_type.SetSelectedIndex(-1);
				tb_search.SetValue("");
				cb_main.PerformCallback('reset');
				}
			}
		function reverify_sub(s,e)
			{
			var c = $(s.GetMainElement()).css("border-color").replace(/\s/g, "");
			if(c == "rgb(255,0,0)")
				{
				verify_criteria(s,e);
				}
			}
		function verify_criteria(s,e)
			{
			var is_valid		= new Object();
			is_valid.value		= true;
			var o				= 
				{
				group_c:		ddl_group.GetMainElement(),
				group_v:		ddl_group.GetValue(),
				page_c:			ddl_page.GetMainElement(),
				page_v:			ddl_page.GetValue(),
				type_c:			ddl_type.GetMainElement(),
				type_v:			ddl_type.GetValue(),
				criteria_c:		tb_search.GetMainElement(),
				criteria_v:		tb_search.GetValue()
				};
			verify_sub(o.group_c, o.group_v, is_valid);
			verify_sub(o.page_c, o.page_v, is_valid);
			verify_sub(o.type_c, o.type_v, is_valid);
			verify_sub(o.criteria_c, o.criteria_v, is_valid);
			if(!is_valid.value)
				{
				e.processOnServer	= false;
				}
			}
		function verify_sub(c,v,is_valid)
			{
			if(!v)
				{
				is_valid.value	= false;
				$(c).css({"border":"solid 1px #f00"});
				}
			else
				{
				$(c).css({"border":"solid 1px #eee"});
				}
			}
		function watch_ticket(obj, _type)
			{
			var parent				= $(obj).parents(".dxdvItem:first");
			var ticket_id			= parent.find(".row_id").val();
			dv_recent.PerformCallback(ticket_id+"|"+_type);
		}

		function disable_button()
		{
		    $("#btnSubmit").enabled = false;
		}

	</script>
	<div id="bt_mobile_back" runat="server" visible="false" align="center"><button type="button" onclick="if(confirm('Are you sure you want to leave?')){location.href = '/mobile/index.aspx?a=tickets';}" style="font-size:1.5em;width:90%;margin:5px;">Back</button></div>

					<div id="search_pane_desktop" runat="server" style="">
						<dx:ASPxCallbackPanel ID="cb_main" runat="server" Width="100%" ClientInstanceName="cb_main" OnCallback="cb_main_Callback" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif" Theme="NETheme01">
						<ClientSideEvents BeginCallback="function(s,e){please_wait('start');}" EndCallback="function(s,e){please_wait('stop');}" />
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
								<div style="padding:5px;">
									<div style="margin-bottom:3px;">
										<dx:ASPxComboBox Caption="Group" ID="ddl_group" ClientInstanceName="ddl_group" runat="server" Theme="NETheme01" TextField="ticket_group_name" ValueField="ticket_group_id" ValueType="System.Int32" Width="310px" AnimationType="None" CaptionSettings-ShowColon="false" EnableSynchronization="True">
											<ClientSideEvents SelectedIndexChanged="function(s, e) {cb_main.PerformCallback('group');}" />
											<CaptionSettings ShowColon="False"></CaptionSettings>
											<CaptionCellStyle Width="150px">
											</CaptionCellStyle>
											<CaptionStyle ForeColor="White"></CaptionStyle>
										</dx:ASPxComboBox>
									</div>
									<div style="margin-bottom:3px;">
										<dx:ASPxComboBox ID="ddl_page" ClientInstanceName="ddl_page" runat="server" Theme="NETheme01" ClientEnabled="false" TextField="ticketpage_name" ValueField="ticketpage_id" Width="310px" AnimationType="None" Caption="Pertaining To" CaptionSettings-ShowColon="false" EnableSynchronization="True">
											<ClientSideEvents SelectedIndexChanged="function(s, e) {cb_main.PerformCallback('page');}" />
											<CaptionSettings ShowColon="False"></CaptionSettings>
											<CaptionCellStyle Width="150px">
											</CaptionCellStyle>
											<CaptionStyle ForeColor="White"></CaptionStyle>
										</dx:ASPxComboBox>
									</div>
									<div style="margin-bottom:3px;">
										<dx:ASPxComboBox ID="ddl_type" ClientInstanceName="ddl_type" runat="server" Theme="NETheme01" ClientEnabled="false" TextField="tickettype_name" ValueField="tickettype_id" ValueType="System.Int32" Width="310px" AnimationType="None" Caption="Type of Ticket" CaptionSettings-ShowColon="false" EnableSynchronization="True">
											<ClientSideEvents SelectedIndexChanged="reverify_sub" />
											<CaptionSettings ShowColon="False"></CaptionSettings>
											<CaptionCellStyle Width="150px">
											</CaptionCellStyle>
											<CaptionStyle ForeColor="White"></CaptionStyle>
										</dx:ASPxComboBox>
									</div>
									<div style="margin-bottom:3px;">
										<dx:ASPxTextBox ID="tb_search" ClientInstanceName="tb_search" runat="server" Width="310px" ClientEnabled="false" Theme="NETheme01" MaxLength="150" Caption="Ticket Subject" HelpText="Maximum Length: 150 Characters" CaptionSettings-ShowColon="false">
											<HelpTextSettings Position="Bottom">
											</HelpTextSettings>
											<CaptionSettings ShowColon="False"></CaptionSettings>
											<CaptionCellStyle Width="150px">
											</CaptionCellStyle>
											<CaptionStyle ForeColor="White"></CaptionStyle>
											<ClientSideEvents KeyPress="reverify_sub" />
										</dx:ASPxTextBox>
									</div>
									<dx:ASPxButton ID="bt_reset" AutoPostBack="false" Text="Reset" runat="server">
										<ClientSideEvents Click="reset_form" />
									</dx:ASPxButton>
									<dx:ASPxButton ID="btn_search" runat="server" OnClick="btn_search_Click" Text="Search">
										<ClientSideEvents Click="verify_criteria" />
									</dx:ASPxButton>
								</div>
								<div align="left" style="margin-top:10px;">
								<asp:Label ID="lbl_error" runat="server" BackColor="Red" Font-Bold="True" Font-Names="" ForeColor="White" Text="Label" Visible="False" Width="97%"></asp:Label>
								</div>
								<div align="center" id="pnl_recent_tickets" runat="server" visible="false">
									<div style="width:98%;max-height:550px;border:solid 1px #999;margin:5px;border-radius: 5px;">
										<div style="color:White;background-color:SteelBlue;padding:5px;font-family:'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;font-weight:bold;font-size:12px;text-align:left;" id="recent_title" runat="server"></div>
										<div style="overflow-y:scroll;height:auto;max-height:480px;">
											<dx:ASPxDataView ID="dv_recent"  ClientInstanceName="dv_recent" runat="server" Width="100%" ItemSpacing="5px" Paddings-Padding="5px" OnCustomCallback="dv_recent_CustomCallback" on>
												<SettingsTableLayout ColumnCount="1" RowsPerPage="10" />
												<PagerSettings ShowNumericButtons="False" Visible="False"></PagerSettings>
												
												<ItemTemplate>
													<input type="hidden" id="hid_id" class="row_id" runat="server" value='<%# Eval("t_id") %>' />
													<table cellpadding="2" cellspacing="0" style="background-color:#eee;width:100%;">
														<tr>
															<td rowspan="2" width="35" align="center">
																<div id="btn_follow" runat="server" visible="<%# dv_button_visible(0, Container.DataItem) %>" onclick="watch_ticket(this, 1)" style="border: solid 1px #888;background-image: url('/DXR.axd?r=1_27-EWK2b');background-position:center bottom;background-size: contain;cursor:pointer;padding:2px;">
																	<div style="background-image:url('/images/icon/icon[watching].png');max-width:16px;background-position:right top;">&nbsp;</div>
																	<span style="font-size:9px;color:#666;">follow</span>
																</div>
																<div id="btn_unfollow" runat="server" visible="<%# dv_button_visible(1, Container.DataItem) %>" onclick="watch_ticket(this, 0)" style="border: solid 1px #888;background-image: url('/DXR.axd?r=1_27-EWK2b');background-position:center bottom;background-size: contain;cursor:pointer;padding:2px;">
																	<div style="background-image:url('/images/icon/icon[watching].png');max-width:16px;background-position:left top;">&nbsp;</div>
																	<span style="font-size:9px;color:#000;">following</span>
																</div>
															</td>
															<td style="padding-left:5px;">
																<div><dx:ASPxHyperLink ID="ticket_id" runat="server" Font-Bold="true" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/member/tickets/ticketpage.aspx?issue={0}', 'tickets{0}', 1095, 740)&quot;, Eval(&quot;t_id&quot;)) %>" Text=" <%# string.Format(&quot;Ticket #{0} - {1}&quot;, Eval(&quot;t_id&quot;),Eval(&quot;t_subject&quot;)) %>" /></div>
																<div><b>Pertaining To: </b><dx:ASPxLabel ID="pertaining" runat="server" Text='<%# Eval("t_pertaining") %>' /></div>
															</td>
															<td width="250">
																<div><b>Created: </b><dx:ASPxLabel ID="when" runat="server" Text='<%# Eval("t_when") %>' /></div>
																<div><b>Status: </b><dx:ASPxLabel ID="status" runat="server" Text='<%# Eval("t_status") %>' /></div>
															</td>
														</tr>
													</table>
													<div style="background-color:#fff;padding:5px;margin-top:5px;border:solid 1px #ddd;">
													<dx:ASPxLabel ID="body" runat="server" EncodeHtml="false" Text='<%# Eval("t_body") %>' />
													</div>
												</ItemTemplate>

<Paddings Padding="5px"></Paddings>
											</dx:ASPxDataView>
										</div>
									</div>
								</div>
								<div id="ticket_details" runat="server" visible="false" style="padding:5px;">
									<table cellpadding="2" cellspacing="0" width="100%">
										<tr style="background-color:#ddd;" id="details_headings" runat="server">
											<td valign="top"><b>Details for this ticket:</b> </td>
											<td valign="top"><b id="paste_title" runat="server">Paste a Screenshot:</b> </td>
										</tr>
										<tr>
											<td valign="top" width="450">
												<dx:ASPxHtmlEditor ID="txtIssue" runat="server" Visible="False" Width="100%" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif" Height="270px" Settings-AllowContextMenu="default">
													<SettingsImageUpload UploadImageFolder="">
													</SettingsImageUpload>
												</dx:ASPxHtmlEditor>
											</td>
											<td valign="top" id="pastbox_td" runat="server">
												<div id="pastebox" runat="server" visible="false">
													<button type="button" style="display: none" class='cancelpasted' onclick="handle_pasted(this);">Cancel Pasted File</button>
													<dx:ASPxTextBox ID="tb_pastebox" runat="server" CssClass="tb_pastebox" Height="240px" Native="True" ReadOnly="True" Width="320px">
													</dx:ASPxTextBox>
												</div>
											</td>
										</tr>
									</table>
									<table width="100%">
										<tr>
											<td valign="middle">
												<asp:Label ID="Label1" runat="server" Text="Include a File if Desired" Visible="False"></asp:Label>
											</td>
											<td style="width: 100%" valign="middle">
												<input runat="server" ID="hiddenshot" type="hidden" class="fileblob"></input>
</input>
													</input>
												<asp:FileUpload runat="server" CssClass="mainfileupload" ID="filMyFile" 
														EnableViewState="False" Visible="False"></asp:FileUpload>
											</td>
										</tr>
										<tr>
											<td colspan="2" valign="middle">
												<asp:CheckBox ID="chkPrivate" runat="server" Text="Mark This as Private (Will Not Be Viewable by Other Users)" Visible="False" />
											</td>
										</tr>
										<tr>
											<td valign="middle">
												<dx:ASPxLabel ID="lbl_selectass" runat="server" Text="Select Assignee:" Visible="False" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif">
												</dx:ASPxLabel>
											</td>
											<td style="width: 100%" valign="middle">
												<asp:DropDownList ID="ddlUser" runat="server" Visible="False" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td valign="middle">
												<dx:ASPxLabel ID="lbl_selectoo" runat="server" ClientVisible="False" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif" Text="Select Objective Owner:">
												</dx:ASPxLabel>
											</td>
											<td style="width: 100%" valign="middle">
												<asp:DropDownList ID="ddloo" runat="server" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif" Visible="False">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td valign="middle">
												<dx:ASPxLabel ID="lbl_selectpri" runat="server" Text="Select Priority:" Visible="False" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif">
												</dx:ASPxLabel>
											</td>
											<td style="width: 100%" valign="middle">
												<asp:DropDownList ID="ddlSeverity" runat="server" Visible="False" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td valign="middle">
												<dx:ASPxLabel ID="lbl_selectpri0" runat="server" ClientVisible="False" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif" Text="Expected Completion Date:" Wrap="False">
												</dx:ASPxLabel>
											</td>
											<td style="width: 100%" valign="middle">
												<dx:ASPxDateEdit ID="dteexpected" runat="server" ClientInstanceName="dteexpected" ClientVisible="False" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif">
												</dx:ASPxDateEdit>
											</td>
										</tr>
										<tr>
											<td valign="top" colspan="2">
												<asp:Button ID="btnSubmit" runat="server" EnableTheming="false" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" OnClick="btnSubmit_Click" Text="Cut Ticket" Visible="False" />
											</td>
										</tr>
									</table>
								</div>
										</dx:PanelContent>
							</PanelCollection>
						</dx:ASPxCallbackPanel>
					</div>
					<div style="margin:5px;">
					<dx:ASPxGridView ID="gv_matches" runat="server" AutoGenerateColumns="False" KeyFieldName="ticketheader_id" Visible="False" Width="98%" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif" Theme="NETheme01">
						<Columns>
							<dx:GridViewCommandColumn VisibleIndex="2" Visible="False" Width="2px">
								
							</dx:GridViewCommandColumn>
							<dx:GridViewDataHyperLinkColumn FieldName="RaisedIssue" VisibleIndex="0" Width="100%" Caption="Ticket">
								<DataItemTemplate>
									<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/member/tickets/ticketpage.aspx?issue={0}', 'tickets1',800,800)&quot;,Eval(&quot;ticketheader_id&quot;)) %>" Text='<%# Eval("RaisedIssue") %>'>
									</dx:ASPxHyperLink>
								</DataItemTemplate>
							</dx:GridViewDataHyperLinkColumn>
							<dx:GridViewDataDateColumn FieldName="DateCreated" VisibleIndex="5" 
								Width="80px">
								<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
								</PropertiesDateEdit>
								<CellStyle Wrap="False">
								</CellStyle>
							</dx:GridViewDataDateColumn>
							<dx:GridViewDataTextColumn FieldName="PageName" VisibleIndex="4" Width="80px" Caption="Pertaining To">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="6" Width="60px">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn FieldName="TypeOfTicket" VisibleIndex="3" Width="70px" Caption="Type">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn FieldName="AsignedTo" VisibleIndex="7" Caption="Assigned To" Width="60px">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn FieldName="Priority" VisibleIndex="8" Width="60px">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataProgressBarColumn FieldName="Relevance" Name="Relevance" ReadOnly="True" VisibleIndex="9" Width="70px">
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataProgressBarColumn>
							<dx:GridViewDataTextColumn Caption="Group" FieldName="groupname" ShowInCustomizationForm="True" VisibleIndex="1" Width="70px">
								<CellStyle Wrap="False">
								</CellStyle>
							</dx:GridViewDataTextColumn>
						</Columns>
						<SettingsBehavior ColumnResizeMode="Control" />
						<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" UseFixedTableLayout="True" />
					</dx:ASPxGridView>
					</div>
					<asp:LinkButton ID="lbCreateNewTicket" runat="server" OnClick="lbCreateNewTicket_Click" Visible="False" Font-Bold="True" Font-Size="Large" Font-Names="'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif">Create New Ticket if Required</asp:LinkButton>
	</asp:Content>