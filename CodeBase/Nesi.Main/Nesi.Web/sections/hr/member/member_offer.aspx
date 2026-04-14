<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_hr_member_member_offer" Theme="NETheme01" Codebehind="member_offer.aspx.cs" %>	
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>




<asp:content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">

	
	<script type="text/javascript">

		$(document).unbind('keydown').bind('keydown', function (event) {
			var doPrevent = false;
			if (event.keyCode === 8) {
				var d = event.srcElement || event.target;
				if ((d.tagName.toUpperCase() === 'INPUT' &&
             (
                 d.type.toUpperCase() === 'TEXT' ||
                 d.type.toUpperCase() === 'PASSWORD' ||
                 d.type.toUpperCase() === 'FILE' ||
                 d.type.toUpperCase() === 'EMAIL' ||
                 d.type.toUpperCase() === 'SEARCH' ||
                 d.type.toUpperCase() === 'DATE')
             ) ||
             d.tagName.toUpperCase() === 'TEXTAREA') {
					doPrevent = d.readOnly || d.disabled;
				}
				else {
					doPrevent = true;
				}
			}

			if (doPrevent) {
				event.preventDefault();
			}
		});
		window.onbeforeunload = "Are you sure you want to leave?";


   

	</script>
	<dx:aspxcallbackpanel ID="ASPxCallbackPanel1" runat="server" 
		ClientInstanceName="cb" oncallback="ASPxCallbackPanel1_Callback" Width="100%" 
		Theme="NETheme01">
		<ClientSideEvents EndCallback="function(s, e) {
            if (s.cp_chargeout!=null)
            {
            pop_chargeout.Show();
            s.cp_chargeout=null;
            }

if ((s.cpalert!=null)&&(s.cpalert!=''))
{
alert(s.cpalert,3000);
s.cpalert=null;
}
	if (s.cpServerMessage != null)
		{
		window.location.replace('member_offer.aspx?id=' + s.cpServerMessage);
		s.cpServerMessage=null;
            }
}" />
		<SettingsLoadingPanel Text="" />
<ClientSideEvents EndCallback="function(s, e) {
	if (s.cpServerMessage != null)
		{
		window.location.replace(&#39;member_offer.aspx?id=&#39; + s.cpServerMessage);
		s.cpServerMessage=null;
    }

     if (s.cp_chargeout!=null)
            {
            pop_chargeout.Show();
            s.cp_chargeout=null;
            }
    

}"></ClientSideEvents>
		<Images>
			<LoadingPanel Url="~/images/loading_panel.gif">
			</LoadingPanel>
		</Images>
		<LoadingPanelImage Url="~/images/loading_panel.gif">
		</LoadingPanelImage>
		<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table width="100%">
		<tr>
			<td nowrap="nowrap">
				<table width="100%" >
					<tr>
						<td>
							<dx:ASPxButton ID="btnSave" runat="server" AutoPostBack="False" 
								HorizontalAlign="Center" Text="Save" Width="75px" Height="27px">
								<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback(&quot;s&quot;);
}" />
<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback(&quot;s&quot;);
}"></ClientSideEvents>

								<Image Url="~/images/icon/icon[save].gif">
								</Image>
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="btnreset" runat="server" ClientEnabled="False" 
								ClientVisible="False" HorizontalAlign="Center" OnClick="btnreset_Click" 
								Text="Reset" Width="75px" UseSubmitBehavior="False" Height="27px">
								<Image Url="~/images/icon/icon[reset].gif">
								</Image>
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="btnaction" runat="server" ClientVisible="False" 
								HorizontalAlign="Center" Width="115px" AutoPostBack="False" UseSubmitBehavior="False" 
								Height="27px">
								<ClientSideEvents Click="function(s, e) {

}" />
<ClientSideEvents Click="function(s, e) {

}"></ClientSideEvents>

								<Image Url="~/images/icon/icon[ball].gif">
								</Image>
							</dx:ASPxButton>
						</td>
						<td >
							<dx:ASPxButton ID="btnexpire" runat="server" HorizontalAlign="Center" 
								OnClick="btnexpire_Click1" Text="Expire" Width="75px" UseSubmitBehavior="False" Height="27px" 
								ToolTip="This will close the offer and remove all linked core responsibilities">
								<Image Url="~/images/icon/icon[dead].gif">
								</Image>
							</dx:ASPxButton>
						</td>
						<td >
							<dx:ASPxButton ID="btnredo" runat="server" HorizontalAlign="Center" Text="Redo" 
								Width="75px" AutoPostBack="False" ClientInstanceName="btnredo" 
								ClientVisible="False" UseSubmitBehavior="False" Height="27px">
								<ClientSideEvents Click="function(s, e) {
	redo_pop.Show();
}" />
<ClientSideEvents Click="function(s, e) {
	redo_pop.Show();
}"></ClientSideEvents>

								<Image Url="~/images/icon/icon[undo].gif">
								</Image>
							</dx:ASPxButton>
						</td>
						<td >
							<dx:ASPxButton ID="btnaccepted" runat="server" ClientVisible="False" 
								Height="27px" HorizontalAlign="Center"  OnClick="btnaccepted_Click"
								Text="They Accepted!" Width="125px" Wrap="False">
								<ClientSideEvents Click="function(s, e) {
                                    if (s.cp_warning!='')
                                    {
	e.processOnServer = confirm(s.cp_warning);
                                    }
                                    else
                                    {
                                    e.processOnServer =true;
                                    }

}" />
<ClientSideEvents Click="function(s, e) {
	 if (s.cp_warning!='')
                                    {
	e.processOnServer = confirm(s.cp_warning);
                                    }
     else
                                    {
                                    e.processOnServer =true;
                                    }
}"></ClientSideEvents>

								<Image Height="15px" Url="~/images/icon/icon[approve].gif">
								</Image>
							</dx:ASPxButton>
						</td>
						<td >
							<span class="style8">
                            <dx:ASPxButton ID="btnprint0" runat="server" ClientInstanceName="btnprint" Height="27px" HorizontalAlign="Center" OnClick="btnprint0_Click" Text="Preview" UseSubmitBehavior="False" Width="115px" Wrap="False">
                                <Image Url="~/images/icon/icon[print].gif">
                                </Image>
                            </dx:ASPxButton>
                            </span>
						</td>
						<td align="right" nowrap="nowrap" 
							style="font-family: Arial, Helvetica, sans-serif" valign="middle" >
							<span class="style8">
                            <dx:ASPxButton ID="btnprint" runat="server" ClientEnabled="False" ClientInstanceName="btnprint" Height="27px" HorizontalAlign="Center" OnClick="btnprint_Click" Text="Print" UseSubmitBehavior="False" Width="115px" Wrap="False">
                                <Image Url="~/images/icon/icon[print].gif">
                                </Image>
                            </dx:ASPxButton>
                            </span></td>
						<td align="right" nowrap="nowrap" style="font-family: Arial, Helvetica, sans-serif" valign="middle">&nbsp;</td>
						<td nowrap="nowrap" width="100%" valign="middle" style="width: 0%" align="right">
							<strong><span>Offer Status:</span></strong></td>
					    <td align="right" nowrap="nowrap" valign="middle">
                            <dx:ASPxLabel ID="txtstatus" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="13pt" Text="ASPxLabel"  Wrap="False" Width="100%">
                            </dx:ASPxLabel>
                        </td>
					</tr>
					<tr>
						<td colspan="9" valign="top">
							<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
								EnableTheming="True" Font-Names="Arial" Theme="NETheme01" Width="100%" ClientInstanceName="pc1" OnCallback="ASPxPageControl1_Callback">
								<TabPages>
									<dx:TabPage Text="Details">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<table style="width: 100%;" class="style4">
													<tr>
														<td class="style4" >
															<strong>Offer ID:</strong></td>
														<td>
															<dx:ASPxLabel ID="lblid" runat="server" ClientInstanceName="lblid" 
																Font-Names="Arial" Text="id">
															</dx:ASPxLabel>
														</td>
														<td>
															&nbsp;</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td class="style4" >
															<strong>Author:</strong></td>
														<td>
															<dx:ASPxLabel ID="lblauthor" runat="server" ClientInstanceName="lblauthor" 
																Font-Names="Arial" Text="Unknown">
															</dx:ASPxLabel>
														</td>
														<td>
															&nbsp;</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td class="style4" >
															<strong>Employee Name:</strong></td>
														<td>
															<dx:ASPxLabel ID="txtEmpName" runat="server">
															</dx:ASPxLabel>
														</td>
														<td>
															&nbsp;</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td class="style4" >
															<strong>Offerred Title:</strong></td>
														<td>
															<dx:ASPxComboBox ID="ddlmembertype" runat="server" 
																ClientInstanceName="ddlmembertype" DataSourceID="membertype" 
																EnableCallbackMode="True" EnableIncrementalFiltering="True" 
																IncrementalFilteringMode="StartsWith" TextField="membertype_name" 
																ValueField="membertype_id" ValueType="System.Int32" Width="150px">
																<ClientSideEvents SelectedIndexChanged="function(s, e) {

  

  
   cb1.PerformCallback(s.GetValue() + ':' + txtwage.GetText() + ':' + ddlcompany.GetValue());
    pc1.PerformCallback('benefits_check_mt');
     cb_bonus.PerformCallback('clear');  
  
}" />
<ClientSideEvents SelectedIndexChanged="function(s, e) {

 
	cb1.PerformCallback(s.GetValue() + &#39;:&#39; + txtwage.GetText() + &#39;:&#39; + ddlcompany.GetValue());
    pc1.PerformCallback('benefits_check_mt');
     cb_bonus.PerformCallback('clear');  
  
}"></ClientSideEvents>
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="membertype" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="Select membertype_id,membertype_name from membertype where active = 1 order by membertype_name">
															</asp:SqlDataSource>
														</td>
														<td class="style4">
															<strong>Reports To:</strong></td>
														<td>
															<dx:ASPxComboBox ID="ddlreportsto" runat="server" AnimationType="None" 
																CallbackPageSize="10" ClientInstanceName="ddlreportsto" CssClass="style5" 
																DataSourceID="sqlmember"  
																IncrementalFilteringMode="Contains"
                                                                 
																TextField="member_fullname" ValueField="id" ValueType="System.Int32" Width="250px"
                                                                
                                                                
                                                                >
															    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	pc1.PerformCallback(s.GetValue());
}" />
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="sqlmember" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" ></asp:SqlDataSource>
														</td>
													</tr>
													<tr>
														<td class="style4" 
															 rowspan="2" valign="top">
															<strong>Offer Start Date:</strong></td>
														<td>
															<dx:ASPxDateEdit ID="dteStart" runat="server" AllowUserInput="False" 
																AnimationType="None" ClientInstanceName="dteStart" 
																DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																EditFormatString="yyyy-MM-dd" 
																OnCalendarDayCellPrepared="dteStart_CalendarDayCellPrepared" Width="150px">
																<CalendarProperties ShowClearButton="False" ShowTodayButton="False">
																</CalendarProperties>
															   <ClientSideEvents DateChanged="function(s, e) {
  
	
    pc1.PerformCallback('benefits_check');
                                                                   


}" />
<ClientSideEvents DateChanged="function(s, e) {

 
	
    pc1.PerformCallback('benefits_check');
  
}"></ClientSideEvents>
															</dx:ASPxDateEdit>
														</td>
														<td class="style4" rowspan="2" valign="top">
															<strong>Expected End Date:</strong></td>
														<td rowspan="2" valign="top">
															<dx:ASPxDateEdit ID="dteEnd" runat="server" DisplayFormatString="yyyy-MM-dd" 
																EditFormat="Custom" EditFormatString="yyyy-MM-dd" Width="150px" AllowUserInput="False" AnimationType="None" 
																OnCalendarDayCellPrepared="dteEnd_CalendarDayCellPrepared">
																<CalendarProperties ShowClearButton="False" ShowTodayButton="False">
																</CalendarProperties>
															</dx:ASPxDateEdit>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<dx:ASPxLabel ID="lbl_pp_note" runat="server" ClientInstanceName="lbl_pp_note" 
																Text="* For existing employees the start date must be the beginning of a pay period" 
																Width="200px" Wrap="True">
															</dx:ASPxLabel>
														</td>
													</tr>
													<tr>
														<td class="style4" ><strong>Business Unit:</strong></td>
														<td>
															<dx:aspxcombobox id="ddlcompany" runat="server" clientinstancename="ddlcompany" datasourceid="SqlDataSource1" textfield="name" valuefield="business_unit_id" valuetype="System.Int32" width="150px">
																<clientsideevents selectedindexchanged="function(s, e) {
	cb1.PerformCallback(ddlmembertype.GetValue() + ':' + txtwage.GetText() + ':' + s.GetValue());
}" />
															</dx:aspxcombobox>
															<asp:sqldatasource id="SqlDataSource1" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" ></asp:sqldatasource>
														</td>
														<td style="font-family: Arial, Helvetica, sans-serif">
															<strong</strong>
                                                        </td>
														<td>
														
                                                        </td>
													</tr>
													<tr>
														<td class="style4" >
															&nbsp;</td>
														<td>
															<dx:aspxcombobox id="cb_business" runat="server" clientinstancename="ddlbusiness" clientenabled="false" textfield="name" valuefield="id" valuetype="System.Int32" width="150px" Visible="False">
															</dx:aspxcombobox>
															<asp:sqldatasource id="ds_business" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" selectcommand="Select id,name from business WHERE region = 'USA' OR id IN (1, 4, 10)"></asp:sqldatasource>

														</td>
														<td class="style4" >&nbsp;</td>
														<td>
															
														</td>
													</tr>
												</table>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage ClientEnabled="False" Text="Wage and Compensation">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<table style="width: 100%;">
													<tr>
														<td class="style4" >
															&nbsp;</td>
														<td colspan="2">
															<dx:ASPxCheckBox ID="chksalary" runat="server" CheckState="Unchecked" 
																ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0" ClientVisible="False">
															</dx:ASPxCheckBox>
														</td>
													</tr>
													<tr>
														<td class="style3" 
															style="font-family:  Calibri, sans-serif; font-size: 9pt; font-weight: 700;" nowrap="nowrap" width="150px">
															Type of Employment:</td>
														<td class="style3">
															<dx:ASPxComboBox ID="ddlpaytype" runat="server" ClientInstanceName="ddlpaytype" 
																DataSourceID="SqlDataSource4" DropDownRows="10" TextField="paytype" 
																ValueField="id" ValueType="System.Int32" AutoPostBack="True" OnSelectedIndexChanged="ddlpaytype_SelectedIndexChanged">
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="Select * from member_paytype"></asp:SqlDataSource>
														</td>
														<td class="style3" width="100%">
															<dx:ASPxCheckBox ID="chk_part_time" runat="server" CheckState="Unchecked" 
																Text="Part Time?" Theme="NETheme01">
															</dx:ASPxCheckBox>
														</td>
													</tr>
													<tr>
														<td style="font-family: Calibri, Helvetica, sans-serif; border-collapse: collapse;" 
															class="style1" colspan="3" nowrap="nowrap">
															<dx:ASPxPanel ID="pnl_subcontractor_stuff" runat="server" Width="100%">
																<PanelCollection>
																	<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																		<table style="width:100%;">
																			<tr>
																				<td class="style2" nowrap="nowrap" width="150px">
																					Subcontractor (Vendor):</td>
																				<td>
																					<dx:ASPxComboBox ID="ddlsubcontractor" runat="server" 
																						DataSourceID="SqlDataSource5" EnableCallbackMode="True" 
																						IncrementalFilteringMode="Contains" TextField="Vendor_Name" Theme="NETheme01" 
																						ValueField="Vendor_ID" ValueType="System.Int32">
																					</dx:ASPxComboBox>
																				</td>
																				<td nowrap="nowrap" width="100%">
																					<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
																						ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																						ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 0 Vendor_ID,'Not Set' Vendor_Name UNION
(SELECT
vendor.Vendor_ID,
vendor.Vendor_Name
FROM
vendor
where Vendor_Active = 1
order by Vendor_Name)"></asp:SqlDataSource>
																					(Use &#39;Not Set&#39; if there is no vendor)</td>
																			</tr>
																			<tr>
																				<td class="style2">
																					Agreed Hourly Rate:</td>
																				<td>
																					<dx:ASPxTextBox ID="txtwage_sub" runat="server" 
																						ClientInstanceName="txtwage_sub" Theme="NETheme01" Width="100px">
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					&nbsp;</td>
																			</tr>
																			<tr>
																				<td class="style2" valign="top">
																					Contract Details:</td>
																				<td colspan="2" valign="top" width="100%">
																					<dx:ASPxMemo ID="mem_contract_details" runat="server" 
																						ClientInstanceName="mem_contract_details" Height="71px" Theme="NETheme01" 
																						Width="100%">
																					</dx:ASPxMemo>
																				</td>
																			</tr>
																		</table>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxPanel>
															<dx:ASPxPanel ID="pnl_not_subcontractor_stuff" runat="server" Width="100%">
																<PanelCollection>
																	<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																		<table style="width:100%;">
																			<tr>
																				<td class="auto-style3" nowrap="nowrap">
																					Current pay range for this membertype for this Business Unit:</td>
																				<td colspan="2" class="auto-style4">
																					<dx:ASPxLabel ID="lblwagerange" runat="server" 
																						ClientInstanceName="lblwagerange" Text="ASPxLabel" Wrap="False">
																					</dx:ASPxLabel>
																				</td>
																			</tr>
																			<tr>
																				<td class="style2" nowrap="nowrap">
																					Current pay range for this membertype company wide:</td>
																				<td colspan="2">
																					<dx:ASPxLabel ID="lblwagerange0" runat="server" 
																						ClientInstanceName="lblwagerange" Text="ASPxLabel">
																					</dx:ASPxLabel>
																				</td>
																			</tr>
																			<tr>
																				<td class="style2">
																					Employment Start Date and Wage:</td>
																				<td colspan="2">
																					<dx:ASPxLabel ID="lblprevious_wage0" runat="server" ClientInstanceName="lblprevious_wage" style="text-align: right">
                                                                                    </dx:ASPxLabel>
																				</td>
																			</tr>
																			<tr>
                                                                                <td class="style2">Current Wage:</td>
                                                                                <td colspan="2">
                                                                                    <dx:ASPxLabel ID="lblprevious_wage" runat="server" ClientInstanceName="lblprevious_wage" style="text-align: right">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
																			<tr>
																				<td class="style2">
																					Hourly Wage (Salary / 2080 hrs):</td>
																				<td>
																					<dx:ASPxTextBox ID="txtwage" runat="server" ClientInstanceName="txtwage" 
																						Width="100px">
																						<ClientSideEvents TextChanged="function(s, e) {
	if (parseInt(s.GetText())&gt;1000)
{
s.SetText(parseFloat(parseInt(s.GetText())/2080).toFixed(2));
}
	cb1.PerformCallback(ddlmembertype.GetValue() + ':' + s.GetText() + ':' + ddlcompany.GetValue());
}" />
																					</dx:ASPxTextBox>
																				</td>
																				<td width="100%">
																					<dx:ASPxCallbackPanel ID="cb2" runat="server" ClientInstanceName="cb1" 
																						 OnCallback="cb1_Callback" 
																						style="text-align: left" Width="200px" Paddings-PaddingLeft="25px">
<SettingsLoadingPanel ImagePosition="Right"></SettingsLoadingPanel>

<Paddings PaddingLeft="25px"></Paddings>
																						<PanelCollection>
																							<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																								<dx:ASPxLabel ID="lbllabourmargin" runat="server" 
																									ClientInstanceName="lbllabourmargin" style="text-align: right" Text="ASPxLabel" Height="40px">
																								</dx:ASPxLabel>
																							</dx:PanelContent>
																						</PanelCollection>
																					</dx:ASPxCallbackPanel>
																				</td>
																			</tr>
																		</table>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxPanel>
															<br />
															<dx:ASPxPanel ID="pnl_vacation" runat="server" Width="100%">
																<PanelCollection>
																	<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																		<table style="width:100%;">
																			<tr>
																				<td colspan="3">
																					<em style="white-space: normal">(Only adjust vacation data upon a New Hire or if 
																					the settings seem crazy..
																					<br />
																					otherwise just leave these as they were from when you first hired the person)</em></td>
																				<td>
																					&nbsp;</td>
																			</tr>
																			<tr>
																				<td class="auto-style1">
																					
																					Vacation Level 1 Interval (Months Since Hired):</td>
																				<td>
																					<dx:ASPxTextBox ID="txtvac1int" runat="server" Width="100px">
																					</dx:ASPxTextBox>
																				</td>
																				<td><dx:ASPxLabel ID="lbl_vac_1int" runat="server" Text="Vacation Level 1 Hours Off">
																					</dx:ASPxLabel>
																					</td>
																				<td>
																					<dx:ASPxTextBox ID="txtvac1amt" runat="server" Width="100px">
																					</dx:ASPxTextBox>
																				</td>
																			</tr>
																			<tr>
																				<td class="auto-style1">
																					Vacation Level 2 Interval (Months Since Hired):</td>
																				<td>
																					<dx:ASPxTextBox ID="txtvac2int" runat="server" Width="100px">
																					</dx:ASPxTextBox>
																				</td>
																				<td><dx:ASPxLabel ID="lbl_vac_2int" runat="server" Text="Vacation Level 2 Hours Off">
																					</dx:ASPxLabel>
																					</td>
																				<td>
																					<dx:ASPxTextBox ID="txtvac2amt" runat="server" Width="100px">
																					</dx:ASPxTextBox>
																				</td>
																			</tr>
																			<tr>
																				<td class="auto-style1">
																					Vacation Level 3 Interval (Months Since Hired):</td>
																				<td>
																					<dx:ASPxTextBox ID="txtvac3int" runat="server" Width="100px">
																					</dx:ASPxTextBox>
																				</td>
																				<td><dx:ASPxLabel ID="lbl_vac_3int" runat="server" Text="Vacation Level 3 Hours Off">
																					</dx:ASPxLabel>
																					</td>
																				<td>
																					<dx:ASPxTextBox ID="txtvac3amt" runat="server" Width="100px">
																					</dx:ASPxTextBox>
																				</td>
																			</tr>
																			<tr>
																				<td class="auto-style1">
																					Benefits Start Date:</td>
																				<td>
																					<dx:ASPxDateEdit ID="dte_benefits_startdate" runat="server" 
																						DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																						EditFormatString="yyyy-MM-dd" Width="110px">
																					</dx:ASPxDateEdit>
																				</td>
																				<td>
																					&nbsp;</td>
																				<td>
																					&nbsp;</td>
																			</tr>
																		</table>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxPanel>
														</td>
													</tr>
												</table>
                                                <dx:ASPxPanel ID="pnl_compplan" ClientInstanceName="pnl_compplan" runat="server" Width="100%">
                                                    <PanelCollection>
                                                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table style="width: 100%;">
																			<tr id="bonus1" runat="server">
																				<td nowrap="nowrap" 
																					style="font-family: Arial, Helvetica, sans-serif; font-size: 9pt">
																					<dx:ASPxCallbackPanel ID="cb_bonus" runat="server" 
																						ClientInstanceName="cb_bonus" OnCallback="cb_bonus_Callback" Width="100%" Theme="NETheme01" Font-Size="9pt" BackColor="White" Font-Names="Arial">
																						<PanelCollection>
																							<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																								<table width="100%" >
																									<tr>
																										<td bgcolor="WhiteSmoke" colspan="4" height="30px" font-size="16px" color="LightGray" class="auto-style2">
																											<strong>Incentive Compensation</strong></td>
																									</tr>
																									<tr>
                                                                                                        <td>Bonus Type:</td>
                                                                                                        <td colspan="2" nowrap="nowrap">
                                                                                                            <dx:ASPxComboBox ID="ddlbonus" runat="server" ClientEnabled="False" ClientInstanceName="ddlbonus" TextField="bonus_type" ValueField="id" ValueType="System.Int32" Width="100px">
                                                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) {
	cb_bonus.PerformCallback(s.GetValue());
}" />
                                                                                                            </dx:ASPxComboBox>
                                                                                                        </td>
                                                                                                        <td nowrap="nowrap">&nbsp;</td>
                                                                                                    </tr>
																									<tr id="row_bonus" runat="server" >
																										<td>
																											Bonus Amount:</td>
																										<td>
																											<dx:ASPxSpinEdit ID="spn_bonus_amt" runat="server" ClientEnabled="False" 
																												ClientInstanceName="spn_bonus_amt" DecimalPlaces="3" DisplayFormatString="P1" 
																												Height="21px" Increment="0.001" Number="0" Width="100px" MinValue="0.001" MaxValue="1" IncrementButtonStyle-Wrap="Default">
																												<ClientSideEvents ValueChanged="function(s, e) {
cb_bonus.PerformCallback('update');
}" />
<ClientSideEvents ValueChanged="function(s, e) {
cb_bonus.PerformCallback(&#39;update&#39;);
}"></ClientSideEvents>
																											</dx:ASPxSpinEdit>
																										</td>
																										<td class="style1" style="font-style: italic" colspan="2">
																											(% of bonus type that will equal the $ bonus paid)</td>
																									</tr>
																									<tr id="row_rev" runat="server" >
																										<td>
																											Revenue Threshold (Target):</td>
																										<td>
																											<dx:ASPxTextBox ID="txt_bonus_revenue_threshold" runat="server" Width="170px">
																												<ClientSideEvents TextChanged="function(s, e) {
	cb_bonus.PerformCallback('update');
}" />
																												
<ClientSideEvents TextChanged="function(s, e) {
	cb_bonus.PerformCallback(&#39;update&#39;);
}"></ClientSideEvents>
																												
																											</dx:ASPxTextBox>
																										</td>
																										<td class="style1" nowrap="nowrap" >
																											<i>(Leave 0 if not used)</i></td>
																										<td class="style1" width="100%">
																											<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Pull from Target Setup" 
																												Wrap="False" AutoPostBack="False" ClientVisible="False">
																												<ClientSideEvents Click="function(s, e) {
	cb_bonus.PerformCallback('pull_rev');
}" />
<ClientSideEvents Click="function(s, e) {
	cb_bonus.PerformCallback(&#39;pull_rev&#39;);
}"></ClientSideEvents>
																											</dx:ASPxButton>
																										</td>
																									</tr>
																									<tr id="row_margin" runat="server" >
																										<td nowrap="nowrap">
																											Gross Margin Threshold (Target):</td>
																										<td>
																											<dx:ASPxTextBox ID="txt_bonus_margin_threshold" runat="server" Width="170px">
																												<ClientSideEvents TextChanged="function(s, e) {
	cb_bonus.PerformCallback('update');
}" />

<ClientSideEvents TextChanged="function(s, e) {
	cb_bonus.PerformCallback(&#39;update&#39;);
}"></ClientSideEvents>

																											</dx:ASPxTextBox>
																										</td>
																										<td class="style1">
																											<i>(Leave 0 if not used)</i></td>
																										<td class="style1">
																											<dx:ASPxButton ID="ASPxButton5" runat="server" Text="Pull from Target Setup" 
																												Wrap="False" AutoPostBack="False" ClientVisible="False">
																												<ClientSideEvents Click="function(s, e) {
	cb_bonus.PerformCallback('pull_margin');
}" />
<ClientSideEvents Click="function(s, e) {
	cb_bonus.PerformCallback(&#39;pull_margin&#39;);
}"></ClientSideEvents>
																											</dx:ASPxButton>
																										</td>
																									</tr>
																									<tr id="row_net" runat="server">
																										<td nowrap="nowrap">
																											Net Income Threshold (Target):</td>
																										<td>
																											<dx:ASPxTextBox ID="txt_bonus_netincome_threshold" runat="server" Width="170px">
																												<ClientSideEvents TextChanged="function(s, e) {
	cb_bonus.PerformCallback('update');
}" />

<ClientSideEvents TextChanged="function(s, e) {
	cb_bonus.PerformCallback(&#39;update&#39;);
}"></ClientSideEvents>

																											</dx:ASPxTextBox>
																										</td>
																										<td class="style1">
																											<i>(Leave 0 if not used)</i></td>
																										<td class="style1">
																											<dx:ASPxButton ID="ASPxButton6" runat="server" Text="Pull from Target Setup" 
																												Wrap="False" AutoPostBack="False" ClientVisible="False">
																												<ClientSideEvents Click="function(s, e) {
	cb_bonus.PerformCallback('pull_net');
}" />
<ClientSideEvents Click="function(s, e) {
	cb_bonus.PerformCallback(&#39;pull_net&#39;);
}"></ClientSideEvents>
																											</dx:ASPxButton>
																										</td>
																									</tr>
																									<tr id="row_net_hw" runat="server">
																										<td nowrap="nowrap">
																											Net Income Highwater Mark:</td>
																										<td>
																											<dx:ASPxTextBox ID="txt_bonus_hw" runat="server" Width="170px">
																												<ClientSideEvents TextChanged="function(s, e) {
	cb_bonus.PerformCallback('update');
}" />

<ClientSideEvents TextChanged="function(s, e) {
	cb_bonus.PerformCallback(&#39;update&#39;);
}"></ClientSideEvents>

																											</dx:ASPxTextBox></td>
																										<td class="style1">
																											&nbsp;</td>
																										<td class="style1">
																											&nbsp;</td>
																									</tr>
																									<tr>
																										<td nowrap="nowrap" width="150px">
																											Comp Details:</td>
																										<td colspan="2">
																											&nbsp;</td>
																										<td>
																											&nbsp;</td>
																									</tr>
																									<tr>
																										<td colspan="4" nowrap="nowrap" width="100%">
																											<dx:ASPxMemo ID="memcompdetails" runat="server" Font-Names="Arial" 
																												Height="250px" Width="100%">
																											</dx:ASPxMemo>
																										</td>
																									</tr>
																								</table>
																							</dx:PanelContent>
																						</PanelCollection>
																					</dx:ASPxCallbackPanel>
																				</td>
																			</tr>
																		</table>

                                                        </dx:PanelContent>

                                                    </PanelCollection>
												</dx:ASPxPanel>
												<br />
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage ClientEnabled="False" Text="Milestones / Objectives">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<table style="width: 100%;">
													<tr>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td width="100%">
															<dx:ASPxCallbackPanel ID="cb_milestones" runat="server" 
																ClientInstanceName="cb_milestones" OnCallback="cb_milestones_Callback" 
																Width="100%">
																<ClientSideEvents EndCallback="function(s, e) {

}" />
<ClientSideEvents EndCallback="function(s, e) {

}"></ClientSideEvents>
																<PanelCollection>
																	<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																		<dx:ASPxGridView ID="gv1_m" runat="server" AutoGenerateColumns="False" 
																			ClientInstanceName="gv1_m" KeyFieldName="id" 
																			OnPreRender="gv1_m_PreRender" OnRowDeleting="gv1_m_RowDeleting" Width="100%" 
																			OnCommandButtonInitialize="gv1_m_CommandButtonInitialize">
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
																			<Columns>
																				<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
																					ShowInCustomizationForm="True" VisibleIndex="0" Width="50px" ButtonRenderMode="Image" ShowDeleteButton="True">
																					
																					<CellStyle VerticalAlign="Middle" Wrap="False">
																					</CellStyle>
																				</dx:GridViewCommandColumn>
																				<dx:GridViewDataTextColumn Caption="Milestone" FieldName="milestone" 
																					ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
																					<DataItemTemplate>
																						<dx:ASPxMemo ID="mem1st" runat="server" Height="50px" oninit="mem1st_Init" 
																							Text='<%# Eval("milestone") %>' Width="100%">
																							<Border BorderStyle="None" />
																						</dx:ASPxMemo>
																					</DataItemTemplate>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
																					ShowInCustomizationForm="True" Visible="False" VisibleIndex="5">
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataDateColumn Caption="Due Date" FieldName="due" 
																					ShowInCustomizationForm="True" VisibleIndex="2" Width="100px">
																					<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																						EditFormatString="yyyy-MM-dd">
																					</PropertiesDateEdit>
																					<DataItemTemplate>
																						<dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" 
																							DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																							EditFormatString="yyyy-MM-dd" oninit="ASPxDateEdit1_Init" 
																							Value='<%# Eval("due") %>' Width="120px">
																						</dx:ASPxDateEdit>
																					</DataItemTemplate>
																				</dx:GridViewDataDateColumn>
																				<dx:GridViewDataTextColumn Caption=" " ShowInCustomizationForm="True" 
																					VisibleIndex="4" Width="120px">
																					<DataItemTemplate>
																						<dx:ASPxButton ID="btn_create_ticket" runat="server" AutoPostBack="False" 
																							ClientInstanceName="btn_create_ticket" Font-Names="Arial" Font-Size="8pt" 
																							oninit="btn_create_ticket_Init" Text="Convert to Ticket" Wrap="False">
																						</dx:ASPxButton>
																					</DataItemTemplate>
																					<CellStyle Wrap="False">
																					</CellStyle>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataTextColumn Caption="Ticket" FieldName="ticketid" 
																					ShowInCustomizationForm="True" VisibleIndex="1">
																					<DataItemTemplate>
																						<dx:ASPxHyperLink ID="hl_ticket" runat="server" ClientInstanceName="hl_ticket" 
																							Cursor="pointer" Font-Names="Arial" 
																							oninit="hl_ticket_Init" Text='<%# Eval("ticketid") %>' onprerender="hl_ticket_PreRender" />
																					</DataItemTemplate>
																				</dx:GridViewDataTextColumn>
																			</Columns>
																			<SettingsBehavior ConfirmDelete="True" />

<SettingsBehavior ConfirmDelete="True"></SettingsBehavior>

																			<SettingsPager Mode="ShowAllRecords" Visible="False">
																			</SettingsPager>
																			<Settings ShowTitlePanel="True" ShowFooter="True" />

<Settings ShowTitlePanel="True" ShowFooter="True"></Settings>

																			<Templates>
																				<TitlePanel>
																					<table style="width:100%; vertical-align: middle;">
																						<tr>
																							<td>
																								<dx:ASPxDateEdit ID="dtemilestone" runat="server" AnimationType="None" 
																									ClientInstanceName="dtemilestone" DisplayFormatString="yyyy-MM-dd" 
																									EditFormat="Custom" EditFormatString="yyyy-MM-dd" NullText="Select Due Date" 
																									Width="100px" Font-Names="Arial">
																								</dx:ASPxDateEdit>
																							</td>
																							<td width="100%">
																								<dx:ASPxMemo ID="add_milestone" runat="server" 
																									ClientInstanceName="add_milestone" Font-Names="Arial" Height="25px" 
																									NullText="Enter the milestone...   make your expectations are clear" 
																									Width="100%">
																								</dx:ASPxMemo>
																							</td>
																							<td valign="middle">
																								<dx:ASPxButton ID="btnadd_milestone" runat="server" AutoPostBack="False" 
																									Text="Add">
																									<ClientSideEvents Click="function(s, e) {
if ((add_milestone.GetText()!='')	&amp;&amp; (dtemilestone.GetText()!=''))
{
cb_milestones.PerformCallback('a|' + add_milestone.GetText() + '|' + dtemilestone.GetText());
	
}
}" />
																								</dx:ASPxButton>
																							</td>
																						</tr>
																					</table>
																				</TitlePanel>
																			</Templates>
																		</dx:ASPxGridView>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxCallbackPanel>
														</td>
													</tr>
												</table>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage ClientEnabled="False" Text="Responsibilities">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													SelectCommand="call proc_get_cr_listing (?mid, ?moid, 0)">
													<SelectParameters>
														<asp:ControlParameter ControlID="hdnmtype" Name="mid" PropertyName="Value" />
														<asp:ControlParameter ControlID="hid_moid" Name="moid" PropertyName="Value" />
													</SelectParameters>
												</asp:SqlDataSource>
												<asp:HiddenField ID="hdnmtype" runat="server" />
												<dx:ASPxGridView ID="gv_mocr" runat="server" AutoGenerateColumns="False" 
													ClientInstanceName="gv_mocr" DataSourceID="SqlDataSource3" Font-Names="Arial" 
													KeyFieldName="id" OnCustomCallback="gv_mocr_CustomCallback" Width="98%">
													<Columns>
														<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
															Visible="False" VisibleIndex="0">
														</dx:GridViewCommandColumn>
														<dx:GridViewDataTextColumn Caption="Group" FieldName="name" 
															ShowInCustomizationForm="True" VisibleIndex="2" Width="70px">
															<CellStyle VerticalAlign="Top">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Responsibility" 
															FieldName="core_responsibility" ShowInCustomizationForm="True" VisibleIndex="3" 
															Width="120px">
															<CellStyle VerticalAlign="Top" Wrap="True">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
															ShowInCustomizationForm="True" VisibleIndex="4" Width="100%">
															<DataItemTemplate>
																<table class="style10" style="width: 100%;">
																	<tr>
																		<td style="padding-bottom: 3px" valign="top">
																			<strong>Description:</strong></td>
																		<td valign="top" width="100%">
																			<dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Arial" 
																				Text='<%# Eval("description") %>' Width="100%" Wrap="True">
																			</dx:ASPxLabel>
																		</td>
																	</tr>
																	<tr>
																		<td style="padding-bottom: 3px" valign="top">
																			<strong>Daily:</strong></td>
																		<td valign="top">
																			<dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Names="Arial" 
																				Text='<%# Eval("daily") %>' Width="100%" Wrap="True">
																			</dx:ASPxLabel>
																		</td>
																	</tr>
																	<tr>
																		<td style="padding-bottom: 3px" valign="top">
																			<strong>Weekly:</strong></td>
																		<td valign="top">
																			<dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Names="Arial" 
																				Text='<%# Eval("weekly") %>' Width="100%" Wrap="True">
																			</dx:ASPxLabel>
																		</td>
																	</tr>
																	<tr>
																		<td style="padding-bottom: 3px" valign="top">
																			<strong>Monthly:</strong></td>
																		<td valign="top">
																			<dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Names="Arial" 
																				Text='<%# Eval("monthly") %>' Width="100%" Wrap="True">
																			</dx:ASPxLabel>
																		</td>
																	</tr>
																	<tr>
																		<td style="padding-bottom: 3px" valign="top">
																			<strong>Quarterly:</strong></td>
																		<td valign="top">
																			<dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Names="Arial" 
																				Text='<%# Eval("quarterly") %>' Width="100%" Wrap="True">
																			</dx:ASPxLabel>
																		</td>
																	</tr>
																	<tr>
																		<td style="padding-bottom: 3px" valign="top">
																			<strong>Annually:</strong></td>
																		<td valign="top">
																			<dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Names="Arial" 
																				Text='<%# Eval("annually") %>' Width="100%" Wrap="True">
																			</dx:ASPxLabel>
																		</td>
																	</tr>
																	<tr>
																		<td nowrap="nowrap" style="padding-bottom: 3px" valign="top">
																			<strong>As Needed:</strong></td>
																		<td valign="top">
																			<dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Names="Arial" 
																				Text='<%# Eval("asneeded") %>' Width="100%" Wrap="True">
																			</dx:ASPxLabel>
																		</td>
																	</tr>
																</table>
															</DataItemTemplate>
															<CellStyle VerticalAlign="Top">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="id" FieldName="id" ReadOnly="True" 
															ShowInCustomizationForm="True" Visible="False" VisibleIndex="1" Width="40px">
															<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Applies" ShowInCustomizationForm="True" 
															VisibleIndex="5" Width="130px">
															<DataItemTemplate>
																<dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" 
																	oninit="ASPxCheckBox1_Init" Text=" ">
																</dx:ASPxCheckBox>
															</DataItemTemplate>
															<HeaderStyle Wrap="True" />
															<CellStyle VerticalAlign="Top" HorizontalAlign="Center">
															</CellStyle>
															<HeaderTemplate>
																<table style="width: 100%;">
																	<tr>
																		<td>
																			<dx:ASPxCheckBox ID="ASPxCheckBox2" runat="server" CheckState="Unchecked" 
																				ClientInstanceName="ASPxCheckBox2" Font-Names="Arial" Font-Size="8pt" 
																				Text="Select / Deselect All" Width="80px" Wrap="True" onprerender="ASPxCheckBox2_Init">
																				<ClientSideEvents CheckedChanged="function(s, e) {
	gv_mocr.PerformCallback('dd|'+s.GetChecked());
}" />
																			</dx:ASPxCheckBox>
																		</td>
																	</tr>
																	<tr>
																		<td>
																			<dx:ASPxCheckBox ID="chkprev_offer" runat="server" CheckState="Unchecked" 
																				ClientInstanceName="chkprev_offer" Font-Names="Arial" Font-Size="8pt" Text="From Previous Offer" Width="80px" Wrap="True" 
																				onprerender="chkprev_offer_Init">
																				<ClientSideEvents CheckedChanged="function(s, e) {
	gv_mocr.PerformCallback('xx|'+s.GetChecked());
}" />
																			</dx:ASPxCheckBox>
																		</td>
																	</tr>
																</table>
															</HeaderTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Priority" FieldName="mt_cr_priority" 
															ShowInCustomizationForm="True" VisibleIndex="6" Width="40px">
															<CellStyle HorizontalAlign="Center">
															</CellStyle>
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsBehavior ColumnResizeMode="Control" AllowFocusedRow="False" />

<SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>

													<SettingsPager Mode="ShowAllRecords" Visible="False" >
													</SettingsPager>
													<Styles>
														<Row VerticalAlign="Top">
														</Row>
														<DetailRow VerticalAlign="Top">
														</DetailRow>
													</Styles>
												</dx:ASPxGridView>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage ClientEnabled="False" Text="Extras">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<table style="width: 100%;">
													<tr>
														<td width="250">
															<strong>Receives Vehicle:</strong></td>
														<td>
															<dx:ASPxCheckBox ID="chkvehicle" runat="server" CheckState="Unchecked" 
																ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
															</dx:ASPxCheckBox>
														</td>
													</tr>
													<tr>
														<td>
															<strong>Receives Cell Phone:</strong></td>
														<td>
															<dx:ASPxCheckBox ID="chkcell" runat="server" CheckState="Unchecked" 
																ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
															</dx:ASPxCheckBox>
														</td>
													</tr>
													<tr>
														<td>
															<strong>Receives Laptop:</strong></td>
														<td>
															<dx:ASPxCheckBox ID="chklaptop" runat="server" CheckState="Unchecked" 
																ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
															</dx:ASPxCheckBox>
														</td>
													</tr>
													<tr>
														<td>
															<strong>Receives Company Email (office365):</strong></td>
														<td nowrap="nowrap"><dx:ASPxCheckBox ID="chkemail" runat="server" CheckState="Unchecked" 
																ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0" 
																Text=" (This doesn't mean Gmail.  Everyone gets a Gmail account)">
															</dx:ASPxCheckBox></td>
													</tr>
													<tr>
														<td>
															<strong>Receives Phone Extension:</strong></td>
														<td><dx:ASPxCheckBox ID="chkphoneext" runat="server" CheckState="Unchecked" 
																ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
															</dx:ASPxCheckBox></td>
													</tr>
													<tr>
														<td>
															<strong>Receives Business Cards:</strong></td>
														<td><dx:ASPxCheckBox ID="chkbusinesscards" runat="server" CheckState="Unchecked" 
																ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
															</dx:ASPxCheckBox></td>
													</tr>
													<tr>
														<td>
															<strong>Receives Barcode Scanner:</strong></td>
														<td><dx:ASPxCheckBox ID="chkbarcode" runat="server" CheckState="Unchecked" 
																ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
															</dx:ASPxCheckBox></td>
													</tr>
													<tr>
														<td>
															<strong>Receives Direct Deposit:</strong><br /> <span style='font-size:10px;'>(upload void cheque/check or bank notification)</span></td>
														<td><dx:ASPxCheckBox ID="chkdirectdeposit" runat="server" CheckState="Unchecked" 
																ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
															</dx:ASPxCheckBox></td>
													</tr>
													<tr>
														<td>
															<strong>Notes (Not appearing on the offer):</strong></td>
														<td>
															<dx:ASPxMemo ID="memnotes" runat="server" BackColor="#FFFFCC" 
																Font-Names="Arial" Height="71px" Width="300px">
															</dx:ASPxMemo>
														</td>
													</tr>
												</table>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage ClientEnabled="False" Text="Signback">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<table width="100%"><tr><td>Upload the signed back offer</td></tr><tr><td>
												<iframe ID="iframe_upload" runat="server" frameborder="0" height="200" 
													width="650px"></iframe>
												<br />
												<div style="color: red; font-size: 11px;">
												</div></td></tr></table>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
								</TabPages>
								
								<ClientSideEvents EndCallback="function(s, e) {
	


if (s.cp_alert!='')
{
alert(s.cp_alert);
s.cp_alert='';
}

}" />
								
								<TabStyle Height="25px">
									<Paddings Padding="7px" />
								<Paddings Padding="7px"></Paddings>
								</TabStyle>
								
							</dx:ASPxPageControl>
						</td>
						<td align="left" valign="top" colspan="2" width="250px">
							<table style="width: 100%;">
								<tr>
									<td  colspan="2" class="auto-style4">
										<strong>Development Notes:</strong></td>
								</tr>
								<tr>
									<td width="100%">
										<dx:ASPxMemo ID="mem_addnote" runat="server" ClientInstanceName="mem_addnote" 
											Height="71px" Width="100%" Font-Names="Arial" Font-Size="8pt">
										</dx:ASPxMemo>
									</td>
									<td align="right">
										<dx:ASPxButton ID="btnaddnote" runat="server" AutoPostBack="False" 
											ClientInstanceName="btnaddnote" Text="Add" Width="100px" Theme="NETheme01">
											<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('y');
}" />
										</dx:ASPxButton>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<dx:ASPxMemo ID="memdevnotes" runat="server" BackColor="#FFFFCC" 
											ClientInstanceName="memdevnotes" Height="500px" ReadOnly="True" 
											style="text-align: left" Width="100%" Font-Names="Arial" Font-Size="8pt">
										</dx:ASPxMemo>
									</td>
								</tr>
							</table>
							<br />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br />
	<asp:HiddenField ID="hid_moid" runat="server" />

    <dx:ASPxPopupControl runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="Above" ClientInstanceName="pop_chargeout" HeaderText="Set Chargeout Rate First!" Width="250px" Height="100px" ID="pop_chargeout" Modal="True" OnWindowCallback="pop_chargeout_WindowCallback" CloseAction="CloseButton" PopupVerticalOffset="100">
        
        <ContentCollection>
<dx:PopupControlContentControl runat="server">
				<table style="width:100%;">
                    <tr>
                        <td>
                            <dx:ASPxTextBox ID="txt_chargeout" runat="server" Width="170px" DisplayFormatString="N2">
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="btn_save_chargeout" runat="server" OnClick="btn_save_chargeout_Click" Text="Save">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>

			</dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>

	<dx:ASPxPopupControl ID="redo_pop" runat="server" ClientInstanceName="redo_pop" 
		HeaderText="Redo Instructions" Height="200px" 
		PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
		Width="400px">
		<ContentCollection>
			<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
				<dx:ASPxMemo ID="memredo" runat="server" BackColor="#FFFFCC" 
					ClientInstanceName="memredo" Height="100px" Width="100%">
				</dx:ASPxMemo>
				<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
					Text="Save and Close" UseSubmitBehavior="False">
					<ClientSideEvents Click="function(s, e) {
	
cb.PerformCallback('d');

}"  />
<ClientSideEvents Click="function(s, e) {
	
cb.PerformCallback(&#39;d&#39;);

}"></ClientSideEvents>
				</dx:ASPxButton>
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
	<br />
	<br />
			</dx:PanelContent>
</PanelCollection>
	</dx:aspxcallbackpanel>




</asp:content>

<asp:content ID="Content4" runat="server" 
	contentplaceholderid="header_placeholder">
    <style type="text/css">
		.style1
		{
			font-size: xx-small;
		}
        .style4{
            font-family: 'Segoe UI',Calibri, Helvetica, sans-serif;
                 font-size: 9pt;
        }
		.style2
		{
			font-family:'Segoe UI', Calibri, Geneva, Verdana, sans-serif;
            font-size:12px;
        }
		.auto-style1 {
            
          font-family:'Segoe UI', Calibri, Geneva, Verdana, sans-serif;
            font-size:12px;
        }
        .auto-style2 {
            font-size: medium;
            color: #999999;
        }
		.auto-style3 {
            font-family: 'Segoe UI', Calibri, Geneva, Verdana, sans-serif;
            font-size: 12px;
            height: 19px;
        }
        .auto-style4 {
            height: 19px;
        }
		</style>
</asp:content>


