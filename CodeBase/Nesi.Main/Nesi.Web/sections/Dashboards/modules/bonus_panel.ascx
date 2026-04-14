<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_Dashboards_modules_bonus_panel" Codebehind="bonus_panel.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<style type="text/css">


.dxWeb_rpHeaderTopLeftCorner {
    background-position: -104px -88px;
    width: 5px;
    height: 5px;
}

.dxWeb_rpHeaderTopLeftCorner,
.dxWeb_rpHeaderTopRightCorner,
.dxWeb_rpBottomLeftCorner,
.dxWeb_rpBottomRightCorner,
.dxWeb_rpTopLeftCorner,
.dxWeb_rpTopRightCorner,
.dxWeb_rpGroupBoxBottomLeftCorner,
.dxWeb_rpGroupBoxBottomRightCorner,
.dxWeb_rpGroupBoxTopLeftCorner,
.dxWeb_rpGroupBoxTopRightCorner,
.dxWeb_mHorizontalPopOut,
.dxWeb_mVerticalPopOut,
.dxWeb_mVerticalPopOutRtl,
.dxWeb_mSubMenuItem,
.dxWeb_mSubMenuItemChecked,
.dxWeb_mScrollUp,
.dxWeb_mScrollDown,
.dxWeb_tcScrollLeft,
.dxWeb_tcScrollRight,
.dxWeb_tcScrollLeftHover,
.dxWeb_tcScrollRightHover,
.dxWeb_tcScrollLeftPressed,
.dxWeb_tcScrollRightPressed,
.dxWeb_tcScrollLeftDisabled,
.dxWeb_tcScrollRightDisabled,
.dxWeb_nbCollapse,
.dxWeb_nbExpand,
.dxWeb_splVSeparator,
.dxWeb_splVSeparatorHover,
.dxWeb_splHSeparator,
.dxWeb_splHSeparatorHover,
.dxWeb_splVCollapseBackwardButton,
.dxWeb_splVCollapseBackwardButtonHover,
.dxWeb_splHCollapseBackwardButton,
.dxWeb_splHCollapseBackwardButtonHover,
.dxWeb_splVCollapseForwardButton,
.dxWeb_splVCollapseForwardButtonHover,
.dxWeb_splHCollapseForwardButton,
.dxWeb_splHCollapseForwardButtonHover,
.dxWeb_pcCloseButton,
.dxWeb_pcPinButton,
.dxWeb_pcRefreshButton,
.dxWeb_pcCollapseButton,
.dxWeb_pcMaximizeButton,
.dxWeb_pcSizeGrip,
.dxWeb_pcSizeGripRtl,
.dxWeb_pPopOut,
.dxWeb_pPopOutDisabled,
.dxWeb_pAll,
.dxWeb_pAllDisabled,
.dxWeb_pPrev,
.dxWeb_pPrevDisabled,
.dxWeb_pNext,
.dxWeb_pNextDisabled,
.dxWeb_pLast,
.dxWeb_pLastDisabled,
.dxWeb_pFirst,
.dxWeb_pFirstDisabled,
.dxWeb_tvColBtn,
.dxWeb_tvColBtnRtl,
.dxWeb_tvExpBtn,
.dxWeb_tvExpBtnRtl,
.dxWeb_fmFolder,
.dxWeb_fmFolderLocked,
.dxWeb_fmCreateButton,
.dxWeb_fmMoveButton,
.dxWeb_fmRenameButton,
.dxWeb_fmDeleteButton,
.dxWeb_fmRefreshButton,
.dxWeb_fmDwnlButton,
.dxWeb_fmCreateButtonDisabled,
.dxWeb_fmMoveButtonDisabled,
.dxWeb_fmRenameButtonDisabled,
.dxWeb_fmDeleteButtonDisabled,
.dxWeb_fmRefreshButtonDisabled,
.dxWeb_fmDwnlButtonDisabled,
.dxWeb_fmThumbnailCheck,
.dxWeb_ucClearButton,
.dxWeb_isPrevBtnHor,
.dxWeb_isNextBtnHor,
.dxWeb_isPrevBtnVert,
.dxWeb_isNextBtnVert,
.dxWeb_isPrevPageBtnHor,
.dxWeb_isNextPageBtnHor,
.dxWeb_isPrevPageBtnVert,
.dxWeb_isNextPageBtnVert,
.dxWeb_isPrevBtnHorDisabled,
.dxWeb_isNextBtnHorDisabled,
.dxWeb_isPrevBtnVertDisabled,
.dxWeb_isNextBtnVertDisabled,
.dxWeb_isPrevPageBtnHorDisabled,
.dxWeb_isNextPageBtnHorDisabled,
.dxWeb_isPrevPageBtnVertDisabled,
.dxWeb_isNextPageBtnVertDisabled,
.dxWeb_isDot,
.dxWeb_isDotDisabled,
.dxWeb_isDotSelected
 {
  
    background-repeat: no-repeat;
    background-color: transparent;
    display:block;
}

img 
{
	border-width: 0px;
}

.dxrpControl .dxrpTE,
.dxrpControl .dxrpNHTE,
.dxrpControlGB .dxrpNHTE
{
	border-top: 1px solid #8B8B8B;
}
.dxrpControl .dxrpTE,
.dxrpControl .dxrpHLE, 
.dxrpControl .dxrpHRE
{
    background-image: none;
	background-color: #DEDEDE;
}

.dxWeb_rpHeaderTopRightCorner {
    background-position: -117px -88px;
    width: 5px;
    height: 5px;
}

.dxrpControl .dxrpHLE,
.dxrpControl .dxrpHRE
{
	border-bottom: 1px solid #C6C6C6;
}
.dxrpControl .dxrpLE,
.dxrpControl .dxrpHLE,
.dxrpControlGB .dxrpLE,
.dxrpControlGB .dxrpHLE
{
	border-left: 1px solid #8B8B8B;
}
.dxrpControl .dxrpHI,
.dxrpControl .dxrpHeader,
.dxrpControl .dxrpHeader td.dxrp
{
	vertical-align: top;
	white-space: nowrap;
}
.dxrpControl .dxrpHeader
{
	background-color: #DEDEDE;
	border-bottom: 1px solid #C6C6C6;
}
.dxrpControl .dxrpHeader,
.dxrpControl .dxrpHeader td.dxrp,
.dxrpControlGB span.dxrpHeader
{
	color: #313131;
}
.dxrpControl td.dxrp,
.dxrpControlGB td.dxrp
{
	font: 12px Tahoma, Geneva, sans-serif;
	color: #000000;
}
.dxeTrackBar, 
.dxeIRadioButton, 
.dxeButtonEdit, 
.dxeTextBox, 
.dxeRadioButtonList, 
.dxeCheckBoxList, 
.dxeMemo, 
.dxeListBox, 
.dxeCalendar, 
.dxeColorTable
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}

.dxeTextBox,
.dxeButtonEdit,
.dxeIRadioButton,
.dxeRadioButtonList,
.dxeCheckBoxList
{
    cursor: default;
}

.dxeButtonEdit
{
	background-color: white;
	border: 1px solid #9F9F9F;
}

.dxeButtonEditSys 
{
    width: 170px;
}

*[cellspacing="1"].dxeButtonEditSys td.dxic 
{
    padding: 2px 2px 1px 2px;
}


.dxeButtonEdit td.dxic 
{
    *padding-left: 2px;
}
.dxeButtonEditSys td.dxic {
    *padding-top: 1px;
    *padding-bottom: 0px;
}

.dxeTextBoxSys td.dxic,
.dxeButtonEditSys td.dxic 
{
    padding: 3px 3px 2px 3px;
    overflow: hidden;
}

.dxeButtonEditSys .dxeEditAreaSys,
.dxeButtonEditSys td.dxic,
.dxeTextBoxSys td.dxic,
.dxeMemoSys td,
.dxeEditAreaSys
{
	width: 100%;
}



.dxeButtonEdit .dxeEditArea
{
	background-color: white;
}

.dxeEditArea
{
	font: 12px Tahoma, Geneva, sans-serif;
	border: 1px solid #A0A0A0;
}
.dxeEditAreaSys 
{
    height: 14px;
    line-height: 14px;
    border: 0px!important;
	padding: 0px 1px 0px 0px; /* B146658 */
    background-position: 0 0; /* iOS Safari */
}
.dxeButtonEditButton,
.dxeSpinIncButton,
.dxeSpinDecButton,
.dxeSpinLargeIncButton,
.dxeSpinLargeDecButton
{
	padding: 0px 2px 0px 3px;

}
.dxeButtonEditButton,
.dxeCalendarButton,
.dxeButtonEditButton td.dx,
.dxeCalendarButton td.dx,
.dxeSpinIncButton,
.dxeSpinDecButton,
.dxeSpinLargeIncButton,
.dxeSpinLargeDecButton,
.dxeSpinIncButton td.dx,
.dxeSpinDecButton td.dx,
.dxeSpinLargeIncButton td.dx,
.dxeSpinLargeDecButton td.dx
{
	font: normal 11px Tahoma, Geneva, sans-serif;
	text-align: center;
	white-space: nowrap;
} 
.dxeButtonEditButton,
.dxeCalendarButton,
.dxeSpinIncButton,
.dxeSpinDecButton,
.dxeSpinLargeIncButton,
.dxeSpinLargeDecButton
{
	vertical-align: middle;
	border: 1px solid #7f7f7f;
	cursor: pointer;
} 

.dxeButtonEditButton table.dxbebt,
.dxeSpinIncButton table.dxbebt,
.dxeSpinDecButton table.dxbebt, 
.dxeSpinLargeIncButton table.dxbebt,
.dxeSpinLargeDecButton table.dxbebt
{
	width: 10px;
}

.dxEditors_edtDropDown {
    background-position: -95px 0px;
    width: 10px;
    height: 14px;
}

.dxEditors_edtError,
.dxEditors_edtCalendarPrevYear,
.dxEditors_edtCalendarPrevYearDisabled,
.dxEditors_edtCalendarPrevMonth,
.dxEditors_edtCalendarPrevMonthDisabled,
.dxEditors_edtCalendarNextMonth,
.dxEditors_edtCalendarNextMonthDisabled,
.dxEditors_edtCalendarNextYear,
.dxEditors_edtCalendarNextYearDisabled,
.dxEditors_edtCalendarFNPrevYear,
.dxEditors_edtCalendarFNNextYear,
.dxEditors_edtEllipsis,
.dxEditors_edtEllipsisDisabled,
.dxEditors_edtDropDown,
.dxEditors_edtDropDownDisabled,
.dxEditors_edtSpinEditIncrementImage,
.dxEditors_edtSpinEditIncrementImageDisabled,
.dxEditors_edtSpinEditDecrementImage,
.dxEditors_edtSpinEditDecrementImageDisabled,
.dxEditors_edtSpinEditLargeIncImage,
.dxEditors_edtSpinEditLargeIncImageDisabled,
.dxEditors_edtSpinEditLargeDecImage,
.dxEditors_edtSpinEditLargeDecImageDisabled
{
	display:block;
	margin:auto;
}

.dxEditors_edtError,
.dxEditors_edtCalendarPrevYear,
.dxEditors_edtCalendarPrevYearDisabled,
.dxEditors_edtCalendarPrevMonth,
.dxEditors_edtCalendarPrevMonthDisabled,
.dxEditors_edtCalendarNextMonth,
.dxEditors_edtCalendarNextMonthDisabled,
.dxEditors_edtCalendarNextYear,
.dxEditors_edtCalendarNextYearDisabled,
.dxEditors_edtCalendarFNPrevYear,
.dxEditors_edtCalendarFNNextYear,
.dxEditors_edtRadioButtonChecked,
.dxEditors_edtRadioButtonUnchecked,
.dxEditors_edtRadioButtonCheckedDisabled,
.dxEditors_edtRadioButtonUncheckedDisabled,
.dxEditors_edtEllipsis,
.dxEditors_edtEllipsisDisabled,
.dxEditors_edtDropDown,
.dxEditors_edtDropDownDisabled,
.dxEditors_edtDETSClockFace,
.dxEditors_edtDETSHourHand,
.dxEditors_edtDETSMinuteHand,
.dxEditors_edtDETSSecondHand,
.dxEditors_edtSpinEditIncrementImage,
.dxEditors_edtSpinEditIncrementImageDisabled,
.dxEditors_edtSpinEditDecrementImage,
.dxEditors_edtSpinEditDecrementImageDisabled,
.dxEditors_edtSpinEditLargeIncImage,
.dxEditors_edtSpinEditLargeIncImageDisabled,
.dxEditors_edtSpinEditLargeDecImage,
.dxEditors_edtSpinEditLargeDecImageDisabled,
.dxEditors_fcadd,
.dxEditors_fcaddhot,
.dxEditors_fcremove,
.dxEditors_fcremovehot,
.dxEditors_fcgroupaddcondition,
.dxEditors_fcgroupaddgroup,
.dxEditors_fcgroupremove,
.dxEditors_fcopany,
.dxEditors_fcopbegin,
.dxEditors_fcopbetween,
.dxEditors_fcopcontain,
.dxEditors_fcopnotcontain,
.dxEditors_fcopnotequal,
.dxEditors_fcopend,
.dxEditors_fcopequal,
.dxEditors_fcopgreater,
.dxEditors_fcopgreaterorequal,
.dxEditors_fcopnotblank,
.dxEditors_fcopblank,
.dxEditors_fcopless,
.dxEditors_fcoplessorequal,
.dxEditors_fcoplike,
.dxEditors_fcopnotany,
.dxEditors_fcopnotbetween,
.dxEditors_fcopnotlike,
.dxEditors_fcgroupand,
.dxEditors_fcgroupor,
.dxEditors_fcgroupnotand,
.dxEditors_fcgroupnotor,
.dxEditors_caRefresh,
.dxEditors_edtTBDecBtn,
.dxEditors_edtTBIncBtn,
.dxEditors_edtTBMainDH,
.dxEditors_edtTBSecondaryDH,
.dxEditors_edtTBIncBtnDisabled,
.dxEditors_edtTBDecBtnDisabled,
.dxEditors_edtTBMainDHDisabled,
.dxEditors_edtTBSecondaryDHDisabled
{
 
    background-repeat: no-repeat;
    background-color: transparent;
}

.dxrpControl .dxrpRE,
.dxrpControl .dxrpHRE,
.dxrpControlGB .dxrpRE
{
	border-right: 1px solid #8B8B8B;
}
.dxrpControl .dxrpLE,
.dxrpControl .dxrpRE,
.dxrpControl .dxrpBE,
.dxrpControl .dxrpNHTE
{
    background-image: none;
	background-color: #F7F7F7;
}
.dxrpControl .dxrpcontent
{
    background-image: none;
    background-color: #F7F7F7;
}
.dxrpControl .dxrpcontent,
.dxrpControlGB .dxrpcontent
{
	vertical-align: top;
}
.dxeBase
{
	font: 12px Tahoma, Geneva, sans-serif;
}
.dxeTextBox,
.dxeMemo
{
	background-color: white;
	border: 1px solid #9f9f9f;
}

.dxeTextBoxSys, 
.dxeMemoSys 
{
    border-collapse:separate!important;
}

.dxeMemoSys td 
{ 
    *padding: 0px; 
}
.dxeMemoSys td 
{
    padding-right: 7px\0/;
}
.dxeMemoSys td 
{
    padding: 0px 6px 0px 0px;
}
.dxeMemoEditArea
{
	background-color: white;
	font: 12px Tahoma, Geneva, sans-serif;
	outline: none;
}


.dxeMemoEditAreaSys 
{
    *margin: -1px 0px;
    *padding-right: 4px;
}
.dxeMemoEditAreaSys 
{
    padding-right: 4px\0/;
}
.dxeMemoEditAreaSys 
{
    padding: 3px 3px 0px 3px;
    margin: 0px;
    border-width: 0px;
	display: block;
	resize: none;
}
.dxbButton
{
	color: #000000;
	font: normal 12px Tahoma, Geneva, sans-serif;
	vertical-align: middle;
	border: 1px solid #7F7F7F;

	padding: 1px;
	cursor: pointer;
}
.dxbButton div.dxb
{
	padding: 3px 8px 4px;
	border-width: 0px;
}

.dxWeb_rpBottomLeftCorner {
    background-position: -78px -88px;
    width: 5px;
    height: 5px;
}

.dxrpControl .dxrpBE,
.dxrpControlGB .dxrpBE
{
	border-bottom: 1px solid #8B8B8B;
}

.dxWeb_rpBottomRightCorner {
    background-position: -91px -88px;
    width: 5px;
    height: 5px;
}

		.style7
		{
			font-family: Arial;
			font-size: medium;
		}
		</style>

		
								<dx:ASPxRoundPanel ID="mypanel" runat="server" HeaderText="Potential Incentive: " 
									style="font-family: Arial" Width="100%">
									<HeaderTemplate>
										<span class="style7">
										<table style="width: 100%;">
											<tr>
												<td nowrap="nowrap" 
													style="font-size: 15px; font-family: Arial, Helvetica, sans-serif; font-weight: bold; color: #808080;">
													Incentive Plan</td>
												<td>
													<span class="style7">
													<dx:ASPxComboBox ID="ddlpm" runat="server" AutoPostBack="True" 
														ClientInstanceName="ddlpm" ClientVisible="False" 
														onselectedindexchanged="ddlpm_SelectedIndexChanged" TextField="_name" 
														ValueField="member_id" ValueType="System.Int32" ClientEnabled="False">
													</dx:ASPxComboBox>
													</span>
												</td>
												<td style="width: 0%" width="100%">
													&nbsp;</td>
												<td align="right" width="100%">
													&nbsp;</td>
											</tr>
											<tr>
												<td nowrap="nowrap" 
													style="font-size: 12px; font-family: Arial, Helvetica, sans-serif; color: #000000;">
													Potential Incentive for Fiscal Year:</td>
												<td>
													<span class="style7">
													<dx:ASPxDateEdit ID="myqtr_date" runat="server" AutoPostBack="True" 
														ClientInstanceName="myqtr_date" DisplayFormatString="y" EditFormat="Custom" 
														EditFormatString="y" ondatechanged="myqtr_date_DateChanged">
													</dx:ASPxDateEdit>
													</span>
												</td>
												<td style="width: 0%" width="100%">
													<span class="style7">
													<dx:ASPxButton ID="btn_agreement" runat="server" AutoPostBack="False" 
														Text="View Agreement">
													</dx:ASPxButton>
													</span></td>
												<td align="right" width="100%">
													&nbsp;</td>
											</tr>
										</table>
										</span>
									</HeaderTemplate>
									<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table style="width: 100%; font-family: Arial;">
		<tr>
			<td colspan="3" nowrap="nowrap">
				<div ID="div_details" runat="server">
				</div>
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap">
				Notes:</td>
			<td>
				<dx:ASPxMemo ID="ASPxMemo1" runat="server" BackColor="#FFFFCC" Height="40px" 
					Width="400px">
				</dx:ASPxMemo>
			</td>
			<td width="100%">
				<dx:ASPxButton ID="btn_save_note" runat="server" OnClick="btn_save_note_Click" 
					Text="Save">
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
										</dx:PanelContent>
</PanelCollection>
								</dx:ASPxRoundPanel>
								
