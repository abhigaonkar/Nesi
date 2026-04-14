<%@ Page Title="Wage Gridview" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" EnableTheming="true" Theme="NETheme01" Inherits="sections_hr_wage_gridview" Codebehind="wage_gridview.aspx.cs" %>	



<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>


	



	
<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>


	
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

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
	</script>

<table width = "100%">
<tr>
<td>
	<uc1:layout_control ID="lc" runat="server" GridviewID="gv" />
	<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
		ClientInstanceName="gv" Font-Names="Arial" 
		KeyFieldName="Member_ID" Width="100%"  
		oncustomjsproperties="gv_CustomJSProperties" OnCustomCallback="gv_CustomCallback" OnCustomButtonCallback="gv_CustomButtonCallback">
		<SettingsResizing ColumnResizeMode="Control" />
		<Columns>
			<dx:GridViewCommandColumn Caption=" " ShowClearFilterButton="True" VisibleIndex="0" ButtonRenderMode="Image" ButtonType="Image">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="print_er">
                        <Image ToolTip="Print Employment Letter" Height="20" Url="~/images/icon/icon[printwoPrice].gif">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                      <dx:GridViewCommandColumnCustomButton ID="print_ext" >
                        <Image ToolTip="Print Extended Employment Letter" Height="20" Url="~/images/icon/icon[print].GIF">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="Member ID" FieldName="Member_ID" ReadOnly="True" 
				VisibleIndex="1">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Employee" FieldName="member_fullname" 
				VisibleIndex="2">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Cursor="pointer" 
						oninit="ASPxHyperLink1_Init" style="font-family: Arial, Helvetica, sans-serif" 
						Text='<%# Eval("member_fullname") %>' />
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Reports To" FieldName="reports_to" 
				VisibleIndex="3">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" 
				VisibleIndex="4">
			    <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
			</dx:GridViewDataTextColumn>
			
			
			
			<dx:GridViewDataTextColumn Caption="Latest Accepted Offer" 
				FieldName="moid" VisibleIndex="5">
                  <DataItemTemplate>
							<dx:ASPxHyperLink ID="ASPxHyperLink4" runat="server" Text=<%# Eval("moid") %>
							NavigateUrl=<%# string.Format(&quot;javascript:boing('offer_print_off.aspx?moid={0}','mo',800,900);&quot;,Eval(&quot;moid&quot;)) %> 
								Theme="NETheme01"  />
					
					</DataItemTemplate>
				
			</dx:GridViewDataTextColumn>

			
			<dx:GridViewDataTextColumn Caption="HR Status" FieldName="hr_status" 
				VisibleIndex="9">
			    <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
			</dx:GridViewDataTextColumn>
		 
		    <dx:GridViewDataTextColumn Caption="Wage" FieldName="wage" ShowInCustomizationForm="True" VisibleIndex="6">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Payroll ID" FieldName="payroll_id" ShowInCustomizationForm="True" VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Social Security (SIN)" FieldName="sin" ShowInCustomizationForm="True" VisibleIndex="10">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Drivers Licence" FieldName="drivers_licence" ShowInCustomizationForm="True" VisibleIndex="11">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="moid" FieldName="moid" Visible="False" VisibleIndex="15">
            </dx:GridViewDataTextColumn>
		 
		    <dx:GridViewDataTextColumn Caption="Member Type" FieldName="membertype_name" VisibleIndex="16">
            </dx:GridViewDataTextColumn>
		 
		    <dx:GridViewDataDateColumn Caption="Last Day in Timesheet" FieldName="last_day" ShowInCustomizationForm="True" VisibleIndex="17">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Login Status" FieldName="member_status" VisibleIndex="18">
                <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
            </dx:GridViewDataTextColumn>
              <dx:GridViewDataDateColumn Caption="Start Date" FieldName="member_startdate" VisibleIndex="12">
                  <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
                  </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn Caption="Term Date" FieldName="member_termdate" VisibleIndex="13">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn Caption="Birth Date" FieldName="member_birthdate" VisibleIndex="14">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn Caption="Current Offer Start Date" FieldName="current_offer_start_date" VisibleIndex="8">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn Caption="Vacation 1 Date" FieldName="vac1_date" VisibleIndex="19">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
             <dx:GridViewDataDateColumn Caption="Vacation 2 Date" FieldName="vac2_date" VisibleIndex="20">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
             <dx:GridViewDataDateColumn Caption="Vacation 3 Date" FieldName="vac3_date" VisibleIndex="21">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
             <dx:GridViewDataTextColumn Caption="Vacation 1 Level" FieldName="vac1_level" VisibleIndex="22">
                <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Vacation 2 Level" FieldName="vac2_level" VisibleIndex="23">
                <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Vacation 3 Level" FieldName="vac3_level" VisibleIndex="24">
                <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
            </dx:GridViewDataTextColumn>
		   <dx:GridViewDataTextColumn Caption="Company Email" FieldName="neemail" VisibleIndex="25">
            </dx:GridViewDataTextColumn>
              <dx:GridViewDataTextColumn Caption="Personal Email" FieldName="email" VisibleIndex="26">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Reason for Termination" FieldName="reason_roe" VisibleIndex="27" Visible ="false">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Years" FieldName="years" VisibleIndex="28" Visible ="false">
            </dx:GridViewDataTextColumn>
           
		    <dx:GridViewDataTextColumn Caption="Days Since Term" FieldName="days_term" Visible="False" VisibleIndex="29">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Days Employed" FieldName="days_employed" Visible="False" VisibleIndex="30">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Days Since Title Change" FieldName="days_since_mt" Visible="False" VisibleIndex="31">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Change Type" FieldName="change_type" Visible="False" VisibleIndex="32">
            </dx:GridViewDataTextColumn>
           
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<ClientSideEvents EndCallback="function(s, e) {
            
            please_wait('stop');
	        if (s.cp_redirect1!=null) {
                boing('/sections/reports/print_employment_record/index.aspx?id=' + s.cp_redirect1 + '&_details=false','',800,900, '_blank')
                delete (s.cp_redirect1);
            }
             if (s.cp_redirect2!=null) {
                boing('/sections/reports/print_employment_record/index.aspx?id=' + s.cp_redirect2 + '&_details=true','',800,900, '_blank')
                delete (s.cp_redirect2);
            }
}" />
		<SettingsPager PageSize="75">
		</SettingsPager>
	    <Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" ShowHeaderFilterButton="True" />
	</dx:ASPxGridView>
	<br />

</td>
</tr>
</table>



</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	</asp:Content>

