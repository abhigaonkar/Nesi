<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_oncall" CodeBehind="oncall.ascx.cs" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxScheduler" Assembly="DevExpress.Web.ASPxScheduler.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<style type="text/css">
    .dropTargetActive {
        border-color: #ff0000;
    }
    .dropTargetHover {
        border-color: #ff0000;
    }
    #draggable3 {
        width: 500px;
        height: 500px;
        padding: 0em;
        padding-top: 0em;
        z-index: 200;
    }
    .shadow {
        box-shadow: 20px 20px 20px rgba(0, 0, 0, 0.5);
    }
</style>
<script type="text/javascript" language="javascript">
    function recurrenceDateChanged(s, e) {
        var selectedWeekends = [];
        var selectedDates = s.GetSelectedDates();
        if (chkallow_weekends.GetValue() == 0) {
            for (i = 0; i < selectedDates.length; i++) {
                if (selectedDates[i].getDay() == 6 || selectedDates[i].getDay() == 0) {
                    selectedWeekends[selectedWeekends.length] = selectedDates[i];
                }
            }
            for (i = 0; i < selectedWeekends.length; i++) {
                s.DeselectDate(selectedWeekends[i]);
            }
        }
    }
</script>
<div>
    <table style="font-family: Arial, Helvetica, sans-serif; border-collapse: collapse;">
        <tr>
            <td style="vertical-align: top">
                <dx:ASPxComboBox ID="ddlbranch" runat="server" AutoPostBack="True"
                    DataSourceID="sqlbranch" Font-Names="Arial" TextField="name"
                    ValueField="business_unit_id" ValueType="System.Int32"
                    OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged" Theme="NETheme01">
                </dx:ASPxComboBox>
                <asp:SqlDataSource ID="sqlbranch" runat="server"
                    ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
            </td>
            <td style="vertical-align: top" nowrap="nowrap">&nbsp;</td>
            <td style="vertical-align: top">&nbsp;</td>
            <td style="vertical-align: top" width="100%">
                <dx:ASPxComboBox ID="ddl_viewresources" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddl_viewresources_SelectedIndexChanged"
                    SelectedIndex="0" ClientVisible="False" Font-Names="Arial">
                    <Items>
                        <dx:ListEditItem Selected="True" Text="All" Value="All" />
                        <dx:ListEditItem Text="Licensed" Value="Licensed" />
                        <dx:ListEditItem Text="Apprentices" Value="Apprentices" />
                        <dx:ListEditItem Text="PM" Value="PM" />
                    </Items>
                </dx:ASPxComboBox>
            </td>
            <td style="vertical-align: top">&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: top">&nbsp;</td>
            <td style="vertical-align: top" nowrap="nowrap">&nbsp;</td>
            <td style="vertical-align: top" nowrap="nowrap">&nbsp;</td>
            <td style="vertical-align: top" width="100%">&nbsp;</td>
            <td style="vertical-align: top">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="5">
                <dx:ASPxCallbackPanel ID="cb_cal" runat="server" ClientInstanceName="cb_cal"
                    OnCallback="cb_cal_Callback1" Width="100%" Theme="NETheme01">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table cellpadding="0" style="width: 100%; border-collapse: collapse;">
                                <tr>
                                    <td>&nbsp;&nbsp;</td>
                                    <td colspan="2" width="100%">
                                        <dx:ASPxLabel ID="lblerror" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                            Theme="NETheme01">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#00B33C" style="padding: 10px 10px 3px 3px" valign="top">
                                        <dx:ASPxButton ID="btn_print" runat="server" AutoPostBack="False" Text="Print">
                                            <ClientSideEvents Click="function(s, e) {
	javascript:boing('on_call_report.aspx','oncall_report','1000','800');
}" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td colspan="2" rowspan="7" valign="top" style="padding-left: 20px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 12px;">* Each calendar day respresents the START of the oncall for that night.&nbsp; On-Call time is from 4pm until 730AM the following morning.<br />
                                        <br />
                                        For example: Bill is allocated to June 7 according to the below calendar.&nbsp; This means Bill is On-Call from June 7 at 4pm until June 8 at 730AM.<br />
                                        <br />
                                        <dx:ASPxCalendar ID="ASPxCalendar2" runat="server" ClientInstanceName="cal"
                                            Columns="3" EnableMultiSelect="True" EnableTheming="True"
                                            EnableYearNavigation="False" Font-Names="Arial"
                                            OnDayCellPrepared="ASPxCalendar1_DayCellPrepared" Rows="4"
                                            ShowClearButton="False" ShowShadow="False" ShowTodayButton="False"
                                            Theme="NETheme01" Width="800px">
                                            <MonthGridPaddings Padding="10px" />
                                            <ClientSideEvents SelectionChanged="function(s, e) {
	recurrenceDateChanged(s, e);
}" />
                                        </dx:ASPxCalendar>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#00B33C" style="padding: 10px 10px 3px 3px" valign="top">
                                        <dx:ASPxCheckBox ID="chk_backup" runat="server" AutoPostBack="True" CheckState="Unchecked" OnCheckedChanged="chk_backup_CheckedChanged" Text="Show Backup">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#00B33C" style="padding: 10px 10px 3px 3px" valign="top">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" ForeColor="White" Text="Set On-Call Dates for:" Theme="NETheme01">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#00B33C" valign="top">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <dx:ASPxComboBox ID="ddl_active_member" runat="server"
                                                        DataSourceID="SqlDataSource1" TextField="_name" Theme="NETheme01"
                                                        ValueField="id" ValueType="System.Int32" ClientInstanceName="ddl_mem">
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Save" Theme="NETheme01"
                                                        AutoPostBack="False">
                                                        <ClientSideEvents Click="function(s, e) {cb_cal.PerformCallback('save|'+ddl_mem.GetValue());}" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;&nbsp;<dx:ASPxLabel ID="ASPxLabel3" runat="server" ForeColor="White"
                                                    Text="With selected dates, Clear -&gt;" Theme="NETheme01">
                                                </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False"
                                                        Text="Clear" Theme="NETheme01">
                                                        <ClientSideEvents Click="function(s, e) {cb_cal.PerformCallback('clear');}" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#E2E2E2" style="padding: 10px 10px 3px 3px" valign="top">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server"
                                            Text="Show These People on the Calendar" Theme="NETheme01">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#E2E2E2" style="padding: 10px 10px 3px 3px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 12px;" valign="top">(<b>Bolded people</b> are already scheduled) :
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#E2E2E2" height="100%" valign="top">
                                        <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False"
                                            DataSourceID="SqlDataSource1" KeyFieldName="id" Theme="NETheme01" Width="100%" OnHtmlDataCellPrepared="ASPxGridView1_HtmlDataCellPrepared">
                                            <ClientSideEvents SelectionChanged="function(s, e) {{cb_cal.PerformCallback();}}" />
                                            <Columns>
                                                <dx:GridViewCommandColumn SelectAllCheckboxMode="Page"
                                                    ShowInCustomizationForm="True" ShowSelectCheckbox="True" VisibleIndex="0"
                                                    Width="25px" Caption=" ">
                                                </dx:GridViewCommandColumn>
                                                <dx:GridViewDataTextColumn Caption="Employee" FieldName="_name"
                                                    ShowInCustomizationForm="True" VisibleIndex="1" Width="100%">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="id" FieldName="id"
                                                    ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataColorEditColumn Caption=" " FieldName="color"
                                                    ShowInCustomizationForm="True" VisibleIndex="3" Width="65px">
                                                    <EditFormSettings Caption=" " />
                                                    <DataItemTemplate>
                                                        <dx:ASPxColorEdit ID="ASPxColorEdit1" runat="server" AllowUserInput="False"
                                                            Caption=" " EnableCustomColors="True" OnInit="ASPxColorEdit1_Init"
                                                            Theme="NETheme01" Value='<%# Eval("color") %>' Width="17px">
                                                            <CaptionSettings RequiredMarkDisplayMode="Hidden" ShowColon="False" />
                                                            <CaptionStyle ForeColor="Transparent">
                                                            </CaptionStyle>
                                                        </dx:ASPxColorEdit>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColorEditColumn>
                                            </Columns>
                                            <SettingsPager Mode="ShowAllRecords">
                                            </SettingsPager>
                                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False"
                                                AllowInsert="False" />
                                        </dx:ASPxGridView>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                                            ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                                            SelectCommand="Select member_id id, member_fullname _name, color 
                    from member inner join membertype on membertype.membertype_id = member.member_membertype_id 
                    where
 member_status = 'Active' 
and (membertype.is_oncall=1 || (member.Member_ElecLicence is not null and member.Member_ElecLicence !=''))
                    and business_unit_id = ?cid order by member_fullname">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <dx:ASPxCallback ID="cb_color_change" runat="server"
                                            ClientInstanceName="cb_color_change" OnCallback="cb_color_change_Callback">
                                            <ClientSideEvents CallbackComplete="function(s, e){ {cb_cal.PerformCallback();}}" />
                                        </dx:ASPxCallback>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px" valign="top">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
