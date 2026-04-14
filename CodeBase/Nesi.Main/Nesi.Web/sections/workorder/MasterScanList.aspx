<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_workorder_MasterScanList" EnableTheming="true" Title="Master Scan List" Codebehind="MasterScanList.aspx.cs" %>


<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register src="../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	&nbsp;<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
	&nbsp; &nbsp;
    	<uc1:layout_control ID="layout" runat="server" />
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:UpdateProgress ID="UPDATEPROGRESS1" runat="server" DisplayAfter="250">
				<ProgressTemplate>
					<div id="Layer1" style="position: absolute; z-index: 1; left: 50%; top: 50%;">
						<img id="Img1" alt="progressing" src="/images/loading_panel.gif" />
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>
		
			<dx:ASPxGridView ID="gv_scanreport" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_scanreport" DataSourceID="sds_scanreport" KeyFieldName="WOID" Width="100%"  OnSummaryDisplayText="gv_scanreport_SummaryDisplayText" onhtmlrowprepared="gv_scanreport_HtmlRowPrepared" Theme="NETheme01" OnCustomCallback="gv_scanreport_CustomCallback" OnCustomJSProperties="gv_scanreport_CustomJSProperties">
				<Styles GroupButtonWidth="28">
					<Header SortingImageSpacing="5px" ImageSpacing="5px" Font-Size="Small" HorizontalAlign="Center">
					</Header>
					<LoadingPanel ImageSpacing="8px">
					</LoadingPanel>
					<GroupRow Wrap="False">
					</GroupRow>
					
					<GroupFooter Wrap="False">
					</GroupFooter>
					<GroupPanel Wrap="False">
					</GroupPanel>
				</Styles>
				<SettingsPager PageSize="600" NumericButtonCount="600" AlwaysShowPager="True">
					<AllButton Text="All">
					</AllButton>
				</SettingsPager>
				<GroupSummary>
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="QuotedAmount" SummaryType="Sum" Tag="BranchQuotedAmount" />
					<dx:ASPxSummaryItem />
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="QuotedAmount" ShowInColumn="Quoted Amount" ShowInGroupFooterColumn="PM" SummaryType="Sum" Tag="Total_PM_Quoted" />
				</GroupSummary>
                <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/EmptyNotes.JPG" Image-Width="16px" Image-Height="16px">
        <Image Height="16px" Url="~/images/EmptyNotes.JPG" Width="16px">
        </Image>
                    </EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px">
        <Image Height="16px" Url="~/images/icon/icon[add].gif" Width="16px">
        </Image>
                    </NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px">
        <Image Height="16px" Url="~/images/icon/icon[delete].gif" Width="16px">
        </Image>
                    </DeleteButton>
</SettingsCommandButton>
				<Columns>
					<dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Visible="False" ShowEditButton="true">
						
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn FieldName="Business Unit" VisibleIndex="1">
						
					    <Settings HeaderFilterMode="CheckedList" />
						
					</dx:GridViewDataTextColumn>
					
					<dx:GridViewDataTextColumn FieldName="BVWO" VisibleIndex="3">
						<DataItemTemplate>
							<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text='<%# Eval("BVWO") %>' NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}&fromwo=scanlist', 'workorder', 1280,768)&quot;, Eval(&quot;WOID&quot;), Eval(&quot;business_unit_id&quot;)) %>" />
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Customer" VisibleIndex="4">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="5">
						<CellStyle HorizontalAlign="Left">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Quote" VisibleIndex="6">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="QuotedAmount" VisibleIndex="7">
						<PropertiesTextEdit DisplayFormatString="c">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="WOID" ReadOnly="True" VisibleIndex="8" Visible="False">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn FieldName="Cut_Date" VisibleIndex="10" Caption="Cut Date" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd" PropertiesDateEdit-EditFormatString="yyyy-MM-dd">
					    <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd">
                        </PropertiesDateEdit>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="11">
					    <Settings HeaderFilterMode="CheckedList" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="PM" VisibleIndex="12">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Progress" VisibleIndex="13">
						<PropertiesTextEdit DisplayFormatString="c">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn FieldName="Open_Date" VisibleIndex="9" Caption="Date Scanned" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd" PropertiesDateEdit-EditFormatString="yyyy-MM-dd">
					    <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd">
                        </PropertiesDateEdit>
					</dx:GridViewDataDateColumn>
				    <dx:GridViewDataTextColumn Caption="Amount" FieldName="amount" VisibleIndex="14">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
				</Columns>
				<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowGroupPanel="True" ShowFilterBar="Visible" ShowGroupedColumns="False" ShowGroupFooter="VisibleIfExpanded" ShowFooter="True" VerticalScrollableHeight="1500" VerticalScrollBarMode="Auto"></Settings>
				<StylesEditors>
					<CalendarHeader Spacing="1px">
					</CalendarHeader>
					<ProgressBar Height="29px">
					</ProgressBar>
				</StylesEditors>
				<SettingsEditing EditFormColumnCount="1" Mode="EditForm" />
				<SettingsText CommandUpdate="sss" />
				<SettingsLoadingPanel ImagePosition="Top" />
				<Paddings Padding="1px" />
				<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control" />
				<TotalSummary>
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="QuotedAmount" ShowInColumn="Quoted Amount" SummaryType="Sum" Tag="Total_Quoted_Amount" />
                    <dx:ASPxSummaryItem DisplayFormat="c" FieldName="amount" ShowInColumn="Amount" SummaryType="Sum" Tag="Total_Invoice_Amount" />
				</TotalSummary>
			</dx:ASPxGridView>
			<br />
			<asp:SqlDataSource ID="sds_scanreport" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT * FROM wo_scan_report"></asp:SqlDataSource>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
