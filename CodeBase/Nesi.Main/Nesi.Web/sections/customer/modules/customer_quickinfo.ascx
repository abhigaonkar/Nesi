<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_customer_modules_customer_quickinfo" Codebehind="customer_quickinfo.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>





	
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<style type="text/css">

.dxeBase
{
	font: 12px Tahoma, Geneva, sans-serif;
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

.dxeTextBoxSys td.dxic 
{
    *padding-left: 3px;
    *padding-top: 2px;
    *padding-bottom: 1px;
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

td.dxic
{
	font-size: 0;
}

.dxeTextBox .dxeEditArea
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

.dxeButtonEdit .dxeEditArea
{
	background-color: white;
}
.dxeButtonEditButton,
.dxeCalendarButton,
.dxeButtonEditButton td.dx,
.dxeCalendarButton td.dx
{
	font: normal 11px Tahoma, Geneva, sans-serif;
	text-align: center;
	white-space: nowrap;
} 

		.style4
	{
		font-size: small;
		font-weight: bold;
	}
</style>
	<script type="text/javascript">
	
	</script>
    <div>
    	
		<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%">
			<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table runat="server" style="width:100%; font-family: Arial;">
		<tr runat="server">
			<td runat="server" class="style4">
				Id:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxLabel ID="lbl_popcustomer_id" runat="server">
				</dx1:ASPxLabel>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Customer:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="txt_popcustomer_name" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Year End:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxSpinEdit ID="spn_popcustomer_yearend" runat="server" Height="21px" 
					Number="0">
				</dx1:ASPxSpinEdit>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Email:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="ASPxTextBox13" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Phone:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="ASPxTextBox14" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Cell:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="ASPxTextBox15" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Facebook:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="ASPxTextBox16" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Twitter:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="ASPxTextBox17" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				LinkedIn:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="ASPxTextBox18" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Status:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxComboBox ID="ASPxComboBox5" runat="server" Font-Names="Arial">
				</dx1:ASPxComboBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				&nbsp;</td>
			<td runat="server" colspan="2">
				&nbsp;</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Birthday:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxDateEdit ID="ASPxDateEdit2" runat="server" 
					DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd" Font-Names="Arial">
				</dx1:ASPxDateEdit>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Cell Phone:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="ASPxTextBox19" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Nesi Login Status:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxComboBox ID="ASPxComboBox6" runat="server" Font-Names="Arial">
				</dx1:ASPxComboBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Nesi Login:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="ASPxTextBox20" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Nesi Password:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="ASPxTextBox21" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style4">
				Hobby:</td>
			<td runat="server" colspan="2">
				<dx1:ASPxTextBox ID="ASPxTextBox22" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx1:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server">
				<dx1:ASPxButton ID="btnsavecustomer" runat="server" AutoPostBack="False" 
					Font-Names="Arial" Text="Save and Close" Width="120px">
					<ClientSideEvents Click="function(s, e) {
	pop_customer.PerformCallback('s');
	pop_customer.Hide();
}" />
				</dx1:ASPxButton>
			</td>
			<td runat="server">
				&nbsp;</td>
			<td runat="server">
				&nbsp;</td>
		</tr>
	</table>
				</dx:PanelContent>
</PanelCollection>
		</dx:ASPxCallbackPanel>
		<br />

    </div>