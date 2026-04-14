<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_global_ticket" EnableTheming="True" Codebehind="global_ticket.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
		<script type="text/javascript" src="/js/jquery.paste.js"></script>
<div class="interface">
	<dx:ASPxCallbackPanel ID="cbp_global_ticket" runat="server" ClientInstanceName="cbp_global_ticket" oncallback="cbp_global_ticket_Callback" ClientSideEvents-BeginCallback="page_obj.global_ticket.cb.begin" ClientSideEvents-CallbackError="page_obj.global_ticket.cb.error" ClientSideEvents-EndCallback="page_obj.global_ticket.cb.end">
<ClientSideEvents BeginCallback="page_obj.global_ticket.cb.begin" EndCallback="page_obj.global_ticket.cb.end" CallbackError="page_obj.global_ticket.cb.error"></ClientSideEvents>
		<PanelCollection>
			<dx:PanelContent>
				<div class="col_left">
					<div class="header">New Ticket</div>
					<dx:ASPxComboBox ID="ddl_group" runat="server" Caption="Group:" ValueType="System.Int32" Width="95%" TextField="name" Theme="NETheme01" CssClass="combo" ValueField="id" ClientInstanceName="root_flyout_group">
						<CaptionSettings Position="Top" />
						
						<ClientSideEvents SelectedIndexChanged="function(s,e){cbp_global_ticket.PerformCallback('refresh');}" />
					</dx:ASPxComboBox>
					<dx:ASPxComboBox ID="ddl_pertaining" runat="server" Caption="Pertaining To:" ValueType="System.Int32" Width="95%" TextField="name" Theme="NETheme01" CssClass="combo" ValueField="id" ClientInstanceName="root_flyout_pertaining">
						<CaptionSettings Position="Top" />
						
						<ClientSideEvents SelectedIndexChanged="function(s,e){if(root_flyout_group.GetValue() == 1){cbp_global_ticket.PerformCallback('refresh');}}" />
					</dx:ASPxComboBox>
					<dx:ASPxComboBox ID="ddl_type" runat="server" Caption="Type of Ticket" ValueType="System.Int32" Width="95%" TextField="name" Theme="NETheme01" CssClass="combo" ValueField="id" ClientInstanceName="root_flyout_type">
						<CaptionSettings Position="Top" />
						
					</dx:ASPxComboBox>
					<dx:ASPxTextBox ID="subject" runat="server" Caption="Ticket Subject" Width="95%" ClientInstanceName="root_flyout_subject" Theme="NETheme01">
						<CaptionSettings Position="Top" />
						
					</dx:ASPxTextBox>
					<dx:ASPxCheckBox ID="is_private" runat="server" Text="Mark as private"></dx:ASPxCheckBox>
					<dx:ASPxMemo ID="body" runat="server" Caption="Ticket Body" Height="100px" Width="95%">
						<CaptionSettings Position="Top" />
						
					</dx:ASPxMemo>
					<div class="paste_image">
						<img src="/images/icon/icon[delete].gif" width="16" height="16" style="position:absolute;cursor:pointer;top:3px; right:3px;display:none;" class="cancel" alt="remove" onclick="page_obj.global_ticket.paste_image.remove();" />
					&nbsp;
					</div>
					<div style="float:left;width:50%;text-align:center;"><dx:ASPxButton ID="cancel" 
							runat="server" AutoPostBack="false" Text="Cancel" TabIndex="9000" Width="100px"><ClientSideEvents Click="function(s,e){page_obj.root_flyout(true);}" /></dx:ASPxButton></div>
					<div style="float:left;width:50%;text-align:center;"><dx:ASPxButton ID="save" 
							runat="server" AutoPostBack="false" Text="Save" TabIndex="9001" Width="100px"><ClientSideEvents Click="function(s,e){if(!cbp_global_ticket.is_processing){root_flyout_subject.Focus();cbp_global_ticket.PerformCallback('save');}}" /></dx:ASPxButton></div>
					
					<input runat="server" ID="file_blob" type="hidden" class="file_blob" />
				</div>
				<div class="col_right">
					<div class="header">Existing Tickets for this Page</div>
					<div id="div_existing_tickets" runat="server" style="height:480px;overflow-y:scroll">
						<dx:ASPxGridView ID="gv_existing_tickets" runat="server" AutoGenerateColumns="False" Theme="NETheme01" Width="100%" CssClass="grid" onhtmldatacellprepared="gv_existing_tickets_HtmlDataCellPrepared" ClientInstanceName="gv_existing_tickets" oncustomcallback="gv_existing_tickets_CustomCallback" EnablePagingGestures="False">
							<Columns>
								<dx:GridViewDataTextColumn VisibleIndex="0" Width="25px" Name="watch" Caption="Watch" CellStyle-HorizontalAlign="Center">
									<DataItemTemplate>
										<div class="iconw watching" id="icon_holder" runat="server">
											
										</div>
									</DataItemTemplate>
									<Settings ShowFilterRowMenu="False" AllowSort="False" AllowAutoFilter="False" AllowHeaderFilter="False" />

<CellStyle HorizontalAlign="Center"></CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn VisibleIndex="1" Width="25px" MinWidth="25"  Name="type" FieldName="type" Caption="Type" CellStyle-HorizontalAlign="Center">
									<DataItemTemplate>
										<div class="icont empty" id="icon_holder" runat="server">
											
										</div>
									</DataItemTemplate>
									<Settings ShowFilterRowMenu="False" AllowSort="False" AllowAutoFilter="False" AllowHeaderFilter="False" />

<CellStyle HorizontalAlign="Center"></CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn VisibleIndex="1" Width="25px" MinWidth="25" Name="vote" FieldName="v" Caption="Vote">
									<DataItemTemplate>
										<div class="iconv wrapper" id="icon_holder_wrapper" runat="server">
										</div>
									</DataItemTemplate>
									<Settings ShowFilterRowMenu="False" AllowAutoFilter="False" AllowHeaderFilter="False" />
                                    <CellStyle HorizontalAlign="Center" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption=" " FieldName="id" VisibleIndex="2" Width="5%">
									<Settings AutoFilterCondition="BeginsWith" />
									<DataItemTemplate>
										<dx:ASPxHyperLink ID="hl" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/member/tickets/ticketpage.aspx?issue={0}', 'tickets{0}',1100,900)&quot;,Eval(&quot;id&quot;)) %>" Text='<%# Eval("id") %>'></dx:ASPxHyperLink>
									</DataItemTemplate>
									<HeaderStyle HorizontalAlign="Center" />
									
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="4" Width="10%" CellStyle-HorizontalAlign="Center">
									<Settings AutoFilterCondition="Contains" />
									<HeaderStyle HorizontalAlign="Center" />
									
<CellStyle HorizontalAlign="Center"></CellStyle>
									
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataDateColumn Caption="Date Created" FieldName="dt_created" VisibleIndex="5" Width="20%">
									<PropertiesDateEdit DisplayFormatString="{0:yyyy-MM-dd HH:mm}">
									</PropertiesDateEdit>
									<HeaderStyle HorizontalAlign="Center" />
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataMemoColumn Caption="Title" FieldName="name" ShowInCustomizationForm="True" VisibleIndex="3" Width="60%">
									<Settings AutoFilterCondition="Contains" />
									<CellStyle Wrap="True">
									</CellStyle>
								</dx:GridViewDataMemoColumn>
							</Columns>
							<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
							<SettingsBehavior EnableRowHotTrack="true" />
				
							
							<SettingsPager PageSize="100"></SettingsPager>
						</dx:ASPxGridView>
					</div>
				</div>			
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
</div>