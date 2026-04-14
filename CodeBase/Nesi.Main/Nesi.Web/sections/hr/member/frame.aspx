<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="member_frame" Title="Employees"  EnableTheming="True" Codebehind="frame.aspx.cs" %>


<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>



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

	<script type="text/javascript" language="javascript">
function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 10) + 'px';
	
 }
	function day_off(id)
		{
		window.open('/sections/hr/member/days_off.aspx?id='+id+'&single=true','day_off','width=540,height=350');
		}
	function get_member(id)
		{
		window.open('/sections/hr/member/MemberContactDisplay.aspx?MemID='+id,'','width=500,height=550');
		$("#member_search").val("");
		}
 </script>
			<lc:LayoutControl runat="server" id="layout" __is_private="True" GridviewID="gv_members" />
	<div align='right' style="background-color:#ddd;padding:5px;border-top:solid 1px #fff;height:30px;">
		<div style="float:left;">
									<dx:ASPxButton ID="btnMemberAccess" runat="server" Visible="false" AutoPostBack="False" Text="Member Access" Theme="NETheme01">
										<ClientSideEvents Click="function(s, e) {
	boing('/sections/hr/member_access/index.aspx','MemberAccess',900,900);
}" />
									</dx:ASPxButton></div>
		<div style="float:right"><input id="member_search" style="font-size:14px;padding:5px;width:350px;border:none 1px #777;border-radius:5px;" type="text" onfocus="attach_ac(this, 'member');" placeholder="Employee Search" data-click="get_member(item[1]);" /></div>
	</div>
        	<dx:ASPxGridView ID="gv_members" runat="server" AutoGenerateColumns="False" 
				KeyFieldName="memberid" 
				oncustombuttoncallback="gv_members_CustomButtonCallback" 
				Font-Names="Arial" Width="100%" 
		onhtmleditformcreated="gv_members_HtmlEditFormCreated" ClientInstanceName="gv_members" 
		oncustomcallback="gv_members_CustomCallback" 
		onstartrowediting="gv_members_StartRowEditing" 
		oncommandbuttoninitialize="gv_members_CommandButtonInitialize" 
		oncustomjsproperties="gv_members_CustomJSProperties" oncancelrowediting="gv_members_CancelRowEditing" onhtmldatacellprepared="gv_members_HtmlDataCellPrepared1" Theme="NETheme01" SettingsPager-PageSize="25">
		<Templates>
                	<EditForm>
						<iframe id="editframe" runat="server" width = "100%" height="1100" 
							frameborder="0"></iframe>
						<br />
                           
                            <div style="margin-top: 10px; margin-bottom: 20px; padding-bottom:20px;">
                               
                                <div style="float: left; margin-left: 10px">
                                    <dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="False" Text="Close" Width="100px"
                                        CssClass="input" 
										ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' Theme="NETheme01">
                                		<ClientSideEvents Click="function(s, e) {
gv_members.CancelEdit();
}" />
									</dx:ASPxButton>
                                </div>
                            </div>
					</EditForm>
                </Templates>
						<SettingsCommandButton>
							<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" >
<Image Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
                            </DeleteButton>
							<EditButton Image-Width="16px" Text="Edit" ButtonType="Link" RenderMode="Link" >

                            </EditButton>
							<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" >
<Image Width="16px" Url="~/images/icon/icon[add].gif"></Image>
                            </NewButton>
						</SettingsCommandButton>
				<Columns>
					<dx:GridViewCommandColumn Width="45px" Caption=" " ShowEditButton="True" ShowClearFilterButton="True" VisibleIndex="0" ButtonRenderMode="Link" ButtonType="Link">
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn Caption="Member Name" FieldName="_name" Name="name" Width="100%" VisibleIndex="1">
						<Settings AutoFilterCondition="Contains" />
						<EditFormSettings Visible="False" />
						<CellStyle Font-Bold="True">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Contact Info" FieldName="memberid"  Name="info" Width="50px" VisibleIndex="2">
						<PropertiesTextEdit DisplayFormatString="&lt;a href=javascript:window.open('MemberContactDisplay.aspx?MemID={0}','','width=500,height=550');&gt; VIEW &lt;/a&gt;">
						</PropertiesTextEdit>
						<Settings AutoFilterCondition="Contains" />
						<EditFormSettings Visible="False" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Login Status" FieldName="loginstatus" Name="status" Width="50px" VisibleIndex="3">
						<Settings HeaderFilterMode="CheckedList" />
						<EditFormSettings Visible="False" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Ext" FieldName="ext" Width="50" Name="ext" VisibleIndex="4">
						<Settings AutoFilterCondition="Equals" />
						<EditFormSettings Visible="False" />
						<CellStyle Font-Bold="True" HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Cell Phone" FieldName="cell" Width="100" Name="cell" VisibleIndex="5">
						<Settings AutoFilterCondition="Contains" />
						<EditFormSettings Visible="False" />
						<DataItemTemplate>
							<dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Arial" 
								Text='<%# Eval("cell") %>' onprerender="ASPxLabel1_PreRender">
							</dx:ASPxLabel>
						</DataItemTemplate>
						<CellStyle HorizontalAlign="Center" Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="branch" Name="branch" Width="150px" VisibleIndex="6">
						<PropertiesComboBox DataSourceID="sqlcomp" TextField="name" 
							ValueField="business_unit_id" ValueType="System.Int32" AnimationType="None">
						</PropertiesComboBox>
						<Settings HeaderFilterMode="CheckedList" AutoFilterCondition="Contains" />
						<EditFormSettings Visible="False" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataTextColumn Caption="Login Name" FieldName="user" Width="75" Name="login" VisibleIndex="7">
						<Settings AutoFilterCondition="Contains" />
						<EditFormSettings Visible="False" />
						<CellStyle HorizontalAlign="Left">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataComboBoxColumn Caption="HR Status" FieldName="hrstatus" Name="hrstatus_id" Width="75px" VisibleIndex="8">
						<PropertiesComboBox DataSourceID="sqlhrstatus" TextField="status" 
							ValueField="id" ValueType="System.Int32">
						</PropertiesComboBox>
						<Settings FilterMode="DisplayText" HeaderFilterMode="CheckedList" />
						<EditFormSettings Visible="False" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataComboBoxColumn Caption="Title" FieldName="membertypeid" Width="150px" VisibleIndex="9">
						<PropertiesComboBox DataSourceID="sqlmembertype" TextField="membertype_name" 
							ValueField="membertype_id" ValueType="System.Int32">
						</PropertiesComboBox>
						<Settings HeaderFilterMode="CheckedList" FilterMode="DisplayText" SortMode="DisplayText" />
						<EditFormSettings Visible="False" />
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataTextColumn Caption="Company Email" FieldName="neemail" Width="70px" VisibleIndex="10">
						<Settings AutoFilterCondition="Contains" />
					    <EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Barcode" Width="50px" VisibleIndex="11">
						<EditFormSettings Visible="False" />
						<DataItemTemplate>
							<dx:ASPxButton ID="print_bc" runat="server" AutoPostBack="False" oncustomjsproperties="print_bc_CustomJSProperties" Text="Print" Theme="NETheme01">
								<ClientSideEvents Click="function(s, e) {
	cb_printbc.PerformCallback(s.cp_ID);
}" />
								<Image Url="~/images/icon/icon[print_barcode].GIF">
								</Image>
							</dx:ASPxButton>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="On Vacation" FieldName="v" Width="50px" VisibleIndex="12">
					    <EditFormSettings Visible="False" />
					    <CellStyle HorizontalAlign="Center">
                        </CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Day Off" Name="day_off" Width="30px"  VisibleIndex="13">
						<Settings AllowSort="False" />
						<EditFormSettings Visible="False" />
						<DataItemTemplate>
							<div align="center"><img title="Add day off" alt="Add day off" src="/images/icon/icon[vacation_note].gif" onclick='day_off(<%# Eval("memberid") %>)' style="cursor:pointer;border:solid 1px #79c;" width="20" height="20" /></div>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					
					<dx:GridViewDataTextColumn Caption="Agreement Status" FieldName="mo_status" Name="mo_status" Width="100px" VisibleIndex="14">
					    <Settings HeaderFilterMode="CheckedList" />
					    <EditFormSettings Visible="False" />
                        <DataItemTemplate>
							<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" Text=<%# Eval("mo_status") %>
							                  NavigateUrl=<%# (Eval("mo_status").ToString()=="In Development")?string.Format(&quot;javascript:boing('member_offer.aspx?id={0}','mo',1100,800);&quot;,Eval(&quot;mo_id&quot;)):string.Format(&quot;javascript:boing('offer_print_off.aspx?moid={0}','mo',800,900);&quot;,Eval(&quot;mo_id&quot;)) %> 

							                  ondatabound="ASPxHyperLink1_Init"
                                		Theme="NETheme01" />
					
					</DataItemTemplate>
					</dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Mobile Contact List" FieldName="_in_mobile_contact_list" Width="60px" VisibleIndex="15">
                        <Settings AllowSort="False" />
                        <EditFormSettings Visible="False" />
                        <DataItemTemplate>
                            <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server"  OnInit="ASPxCheckBox1_Init"   ValueType="System.Int32" ValueUnchecked="0" ValueChecked="1">
                            </dx:ASPxCheckBox>
                        </DataItemTemplate>
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
				    <dx:GridViewDataTextColumn Caption="Years" FieldName="years" Width="50px" VisibleIndex="16">
                        <Settings AutoFilterCondition="LessOrEqual" HeaderFilterMode="CheckedList" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                     
				   
                    <dx:GridViewDataTextColumn Caption="Last Review Gen" FieldName="gen_score" Name="gen_score" Width="50px" MinWidth="30" VisibleIndex="17">
                        <Settings AutoFilterCondition="Less" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                         
                    </dx:GridViewDataTextColumn>
				    <dx:GridViewDataTextColumn Caption="Last Review CR" FieldName="cr_score" Name="cr_score" MinWidth="30" Width="50px" VisibleIndex="18">
                    <Settings AutoFilterCondition="Less" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="Last Review" FieldName="last_review_date" Width="80px" MinWidth="50" VisibleIndex="19">
                        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                        </PropertiesDateEdit>
                        <Settings AutoFilterCondition="Less" />
                        <DataItemTemplate>
							<dx:ASPxHyperLink ID="ASPxHyperLink3" runat="server" Text=<%# String.Format("{0:yyy-MM-dd}", Eval("last_review_date")) %>
                               
							NavigateUrl="<%# string.Format(&quot;javascript:boing('review_list_for_member.aspx?id={0}&memberid={1}','review{1}' ,900,900);&quot;,Eval(&quot;emp_review_id&quot;),Eval(&quot;memberid&quot;)) %>"
								Theme="NETheme01" ondatabound="ASPxHyperLink1_Init" />
					
					</DataItemTemplate>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataDateColumn Caption="Last Agreement" Width="80px" MinWidth="50" FieldName="last_agreement" Name="last_agreement" VisibleIndex="20">
                        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                        </PropertiesDateEdit>
                        <Settings AutoFilterCondition="Less" />
                       
                    </dx:GridViewDataDateColumn>
				    <dx:GridViewDataTextColumn Caption="Timesheet Rating" FieldName="morale" Visible="False" Width="50px" VisibleIndex="21">
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Emp ID" FieldName="memberid" 
						VisibleIndex="23" Name="member_id" Width="100%">
						<Settings AutoFilterCondition="Contains" />
						<EditFormSettings Visible="False" />
						<CellStyle Font-Bold="True">
						</CellStyle>
					</dx:GridViewDataTextColumn>
				    <dx:GridViewDataTextColumn Caption="mo_id" FieldName="mo_id" Visible="False" VisibleIndex="22">
                    </dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Address" FieldName="address" Visible="False" Width="50px" VisibleIndex="24">
				    <CellStyle HorizontalAlign="Center">
				    </CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataDateColumn Caption="Start Date" Width="80px" MinWidth="50" FieldName="startdate" Name="startdate" VisibleIndex="25">
				    <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
				    </PropertiesDateEdit>
				    <Settings AutoFilterCondition="Less" />
				</dx:GridViewDataDateColumn>
				<dx:GridViewDataTextColumn Caption="Supervisor" FieldName="supervisor" Visible="False" Width="50px" VisibleIndex="26">
				    <Settings HeaderFilterMode="CheckedList" />
                    <CellStyle HorizontalAlign="Center">
				    </CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Tax Entity" FieldName="tax_entity" Visible="False" Width="50px" VisibleIndex="27">
				    <Settings HeaderFilterMode="CheckedList" />
                     <CellStyle HorizontalAlign="Center">
				    </CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="ADP Payroll ID" FieldName="member_payroll_id" Visible="False" Width="50px" VisibleIndex="28">
				    <Settings HeaderFilterMode="CheckedList" />
                     <CellStyle HorizontalAlign="Center">
				    </CellStyle>
				</dx:GridViewDataTextColumn>
				    <dx:GridViewDataTextColumn Caption="Chargeout Rate" FieldName="chargeout_rate" VisibleIndex="29" Width="60px">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
				</Columns>
				<SettingsBehavior ColumnResizeMode="Control" />
				<SettingsPager PageSize="25">
				</SettingsPager>
				<SettingsEditing Mode="PopupEditForm" />
				<Settings ShowFilterRow="True" ShowFilterRowMenu="True" 
					ShowHeaderFilterButton="True" ShowTitlePanel="True" ShowGroupPanel="True" ColumnMinWidth="20" />
				<SettingsText PopupEditFormCaption="Member Details" />
				<SettingsPopup>
					<EditForm HorizontalAlign="WindowCenter" MinHeight="800px" MinWidth="950px" 
						VerticalAlign="WindowCenter" Width="1100px" />
				</SettingsPopup>
				
			</dx:ASPxGridView>
			
			<br />
	<asp:SqlDataSource ID="sqlcomp" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		
		>
        
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sqlmembertype" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select membertype_id,membertype_name from membertype">
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sqlhrstatus" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="select id,status from member_hrstatus"></asp:SqlDataSource>
	<dx:ASPxCallback ID="cb_printbc" runat="server" ClientInstanceName="cb_printbc" oncallback="cb_printbc_Callback">
		<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;, &quot;Printing Barcode&quot;);
}" CallbackComplete="function(s, e) {
	please_wait(&quot;stop&quot;);
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" EndCallback="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
	</dx:ASPxCallback>
	<br />
	<br />
</asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="header_placeholder">
    <style type="text/css">
		.style5 { width: 46px; height: 45px; }
	</style>
</asp:Content>



