<%@ Page
	Language='C#'
	MasterPageFile='../../../IntraDefault.master'
	AutoEventWireup='true'
	Inherits='trial_balance'
	Title='Trial Balance' Codebehind="index.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="../../../modules/layout_control.ascx" TagName="layout_control" TagPrefix="uc1" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' runat='Server'>
    <div id='divSide' runat='server'>
		<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
	</div>
</asp:Content>
<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' runat='Server'>
    <div id='divMenu' runat='server'></div>
</asp:Content>
<asp:Content ContentPlaceHolderID="header_placeholder" runat="server" ID="cph_head">
    <script type="text/javascript">
		$("document").ready(
		function()
			{
			page_obj.update_panel_progress.bind();
			})
	</script>
</asp:Content>
<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' runat='Server'>
                        <uc1:layout_control ID="layout" runat="server" />
	<asp:ScriptManager ID="sm" runat="server" AsyncPostBackTimeout="600"></asp:ScriptManager>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
			
	<div id='income_statement'>
		<table class='framework' border='0' cellpadding='0' cellspacing='0'>
			<tr>
				<td valign='top' align='center' id='detail' runat='server'></td>
			</tr>
		</table>
		<div id='errormsg'>
			<table>
				<tr>
					<td width="175">
						<strong>Tax Entity:</strong></td>
					<td>
					    <dx:ASPxComboBox runat="server" ID="combo_tax_entity" Theme="NETheme01" Width="100%" ValueField="id" TextField="ddl_name" ValueType="System.Int32"  
                            AutoPostBack="true" OnSelectedIndexChanged="combo_tax_entity_SelectedIndexChanged"></dx:ASPxComboBox>
					</td>
				</tr>
				<tr>
                    <td width="175">
                        <asp:Label ID="select_branch_label" runat="server" AssociatedControlID="select_branch_select" CssClass="style6" Font-Bold="True" Text="Business Unit:" Visible="False" Width="135px"></asp:Label>
                    </td>
                    <td>
                        <br />
                        <dx:ASPxCheckBoxList ID="cl_companies" runat="server" RepeatColumns="3" TextField="name" Theme="NETheme01" ValueField="id">
                        </dx:ASPxCheckBoxList>
                        <br />
                        <div id="div_waiting_rollover" runat="server">
                            (*) Denotes a business unit that is awaiting its year-end rollover</div>
                    </td>
                </tr>
				<tr>
					<td class="style8">Fiscal Period Ending:</td>
					<td class="style8">
						<asp:DropDownList ID="select_enddate" CssClass="enddate" runat="server"></asp:DropDownList></td>
				</tr>
			    <tr>
			        <td>Type:</td>
			        <td>
			            <asp:DropDownList ID="select_type" runat="server" CssClass="dept">
			                <asp:ListItem>Actuals</asp:ListItem>
			                <asp:ListItem>Budget</asp:ListItem>
			            </asp:DropDownList>
			        </td>
			    </tr>
				<tr>
				    <td align="left">
				        <dx:ASPxButton ID="ASPxButton3" Theme="NETheme01" runat="server" ClientVisible="False" OnClick="ASPxButton3_Click" Text="Sync BV"></dx:ASPxButton>
				    </td>   
					<td>
						<dx:ASPxButton ID="ASPxButton1" runat="server" 
							Text="Go-&gt;" Theme="NETheme01">
						</dx:ASPxButton>
					</td>
				</tr>
			</table>
			<input type="hidden" class="" runat="server" id="hid_business_unit_id" />
			<table width="100%" cellpadding="2" cellspacing="0">
				<tr>
					<td style="border-bottom: black thin solid">
						<asp:Label ID="name_branch" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="24pt"
							Text="Business Unit Name"></asp:Label></td>
				</tr>
				<tr>
					<td valign="top" style="height: 392px" class="style7">
						<dx:ASPxGridView
							ID="gv_trial_balance"
							ClientInstanceName="gv_trial_balance" Theme="NETheme01"
							runat="server"
							AutoGenerateColumns="False"
							Font-Names="Arial" 
							Font-Size="8pt" KeyFieldName="account_no" OnCustomCallback="gv_trial_balance_CustomCallback" OnCustomJSProperties="gv_trial_balance_CustomJSProperties">
							<TotalSummary>
                                <dx:ASPxSummaryItem FieldName="YTD" ShowInColumn="YTD" SummaryType="Sum" />
                                <dx:ASPxSummaryItem FieldName="this_yr07" ShowInColumn="this_yr07" SummaryType="Sum" />
                                <dx:ASPxSummaryItem ShowInColumn="this_yr06" FieldName="this_yr06" SummaryType="Sum" />
                            </TotalSummary>
							<GroupSummary>
								<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="YTD" ShowInColumn="YTD" ShowInGroupFooterColumn="GLGroup" SummaryType="Sum" />
								<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="YTD" ShowInColumn="GLGroup" ShowInGroupFooterColumn="YTD" SummaryType="Sum" />
							</GroupSummary>
							<Columns>
								<dx:GridViewDataTextColumn FieldName="type" ShowInCustomizationForm="True"
									Width="100px" Caption="Type"
									VisibleIndex="0" SortIndex="0" SortOrder="Ascending">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Normal Desig" FieldName="drcr" PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="3" Width="60px">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="GLGroup" FieldName="gl_group_alias"
									VisibleIndex="1" Width="100px" SortIndex="1" SortOrder="Ascending">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Account" FieldName="ACCOUNT"
									VisibleIndex="2" Width="200px">
									<CellStyle Font-Bold="True">
									</CellStyle>
									<FooterCellStyle HorizontalAlign="Left">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Note" VisibleIndex="4" Width="30px" Visible="False">
									<DataItemTemplate>
										<img class="_note" id="note" runat="server" onclick="note_show(this);" data-gv="gv_revenue_400" data-acct_no='<%# Eval("account_no") %>' data-cr_dr="cr" src="/images/icon/icon[note_blank].gif" />
									</DataItemTemplate>
									<HeaderStyle HorizontalAlign="Center" />
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="YTD" FieldName="YTD" VisibleIndex="5"
									PropertiesTextEdit-DisplayFormatString="C2">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle BackColor="#C4E1FF">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="MTD" FieldName="MTD" VisibleIndex="6"
									Visible="False">
									<PropertiesTextEdit DisplayFormatString="C2">
									</PropertiesTextEdit>
									<HeaderStyle HorizontalAlign="Center" />
									<CellStyle HorizontalAlign="Right" BackColor="#EAF4FF" />
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="MTD_LastYear" FieldName="mtdlastyear"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="7" Visible="False">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Open Balance" FieldName="open_bal" PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="8" Width="100px" Visible="False">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr01" FieldName="this_yr01"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="21">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr02" FieldName="this_yr02"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="19">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr03" FieldName="this_yr03"
									VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="C2">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr04" FieldName="this_yr04"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="17">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr05" FieldName="this_yr05"
									PropertiesTextEdit-DisplayFormatString="C2" VisibleIndex="16">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center"></CellStyle>
									<FooterCellStyle Font-Bold="True"></FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr06" FieldName="this_yr06"
									PropertiesTextEdit-DisplayFormatString="C2" VisibleIndex="15">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center"></CellStyle>
									<FooterCellStyle Font-Bold="True"></FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr07" FieldName="this_yr07"
									PropertiesTextEdit-DisplayFormatString="C2" VisibleIndex="14">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center"></CellStyle>
									<FooterCellStyle Font-Bold="True"></FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr08" FieldName="this_yr08"
									PropertiesTextEdit-DisplayFormatString="C2" VisibleIndex="13">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center"></CellStyle>
									<FooterCellStyle Font-Bold="True"></FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr09" FieldName="this_yr09"
									PropertiesTextEdit-DisplayFormatString="C2" VisibleIndex="12">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center"></CellStyle>
									<FooterCellStyle Font-Bold="True"></FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr10" FieldName="this_yr10"
									PropertiesTextEdit-DisplayFormatString="C2" VisibleIndex="11">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center"></CellStyle>
									<FooterCellStyle Font-Bold="True"></FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr11" FieldName="this_yr11"
									PropertiesTextEdit-DisplayFormatString="C2" VisibleIndex="10">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center"></CellStyle>
									<FooterCellStyle Font-Bold="True"></FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="this_yr12" FieldName="this_yr12"
									PropertiesTextEdit-DisplayFormatString="C2" VisibleIndex="9">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center"></CellStyle>
									<FooterCellStyle Font-Bold="True"></FooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="last_yr12" FieldName="last_yr_total"
									PropertiesTextEdit-DisplayFormatString="C2" VisibleIndex="22">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Acct No" FieldName="account_no"
									 VisibleIndex="23">
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr01" FieldName="last_yr01"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="36">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr02" FieldName="last_yr02"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="35">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr03" FieldName="last_yr03"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="34">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr04" FieldName="last_yr04"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="33">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr05" FieldName="last_yr05"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="32">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr06" FieldName="last_yr06"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="31">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr07" FieldName="last_yr07"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="30">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr08" FieldName="last_yr08"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="29">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr09" FieldName="last_yr09"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="28">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr10" FieldName="last_yr10"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="27">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="last_yr11" FieldName="last_yr11"
									PropertiesTextEdit-DisplayFormatString="C2"
									VisibleIndex="26">
									<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
									<FooterCellStyle Font-Bold="True">
									</FooterCellStyle>
								</dx:GridViewDataTextColumn>
                              

							</Columns>
							<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control"
								AutoExpandAllGroups="True" />
							<SettingsPager Mode="ShowAllRecords">
							</SettingsPager>
							<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="true"
								ShowHeaderFilterButton="True" ShowFooter="True" GridLines="None" GroupFormat="{1} {2}"
								ShowGroupFooter="VisibleAlways" UseFixedTableLayout="True" ColumnMinWidth="30" />
							<Styles GroupButtonWidth="1">
								<GroupRow BackColor="White">
								</GroupRow>
								<RowHotTrack BackColor="#FFCCCC">
								</RowHotTrack>
								<Footer HorizontalAlign="Right">
								</Footer>
								<GroupFooter BackColor="White">
									<Border BorderStyle="None" />
								</GroupFooter>
								<GroupPanel>
									<Border BorderStyle="None" />
								</GroupPanel>
							</Styles>
						</dx:ASPxGridView>
					</td>
				</tr>
			</table>
		</div>
	</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
