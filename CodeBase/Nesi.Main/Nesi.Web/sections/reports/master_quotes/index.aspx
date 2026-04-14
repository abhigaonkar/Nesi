<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" 
    AutoEventWireup="true"  
     Inherits="master_quotes" Title="Master Quote Grid" Codebehind="index.aspx.cs" %>
<%@ Import Namespace="nesi.core" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
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
    <script type="text/javascript">
        
        function yoyo(fff) {
           
            cb_chkprivate.PerformCallback(fff);
        }

        function q(id, rev) {
            boing("/#/opens/65/quotes/" + id + "/" + rev, "quote", 1035, 800);
        }
        function note_show(obj) {
            var exists = $("#note_window").size() > 0;
            var o = $(obj).offset();
            var x = o.left;
            var y = o.top;
            var quote_id = $(obj).attr('data-qid');
            if (exists && $("#note_window")) {
                $("#note_window").remove();
            }
            var html = "<div id='note_window'>";
            html += "<b>Add Note:</b>\
										<br/><textarea style='width:99%;height:100px;' maxlength='512'></textarea>\
										<br><button style='font-size:11px;font-weight:bold;' onclick='handle_note(this)' data-quote_id='" + quote_id + "' type='button'><img align='absmiddle' src='/images/icon/icon[save].gif'/> Save</button>\
										<button style='font-size:11px;font-weight:bold;' onclick=\"$('#note_window').remove();\"><img align='absmiddle' src='/images/icon/icon[delete].gif'/> Close</button>";
            html += "</div>";
            $("body").append(html);
            var off = $(obj).offset();
            $("#note_window").css({ "position": "absolute",
                "top": off.top + "px",
                "left": off.left + $(obj).width() + "px",
                "width": "300px",
                "padding": "5px",
                "border": "solid 1px #cc7",
                "background-color": "#ff7"
            }).find("textarea").focus().val();
            var left = $(document).outerWidth() - $(window).width();
            $('body, html').scrollLeft(left);
        }
        function handle_note(obj) {
            $(obj).attr("disabled", true);
            please_wait("start");
            var _defs = {
                a: "handle_note",
                qid: $(obj).attr('data-quote_id'),
                note: $("#note_window").find("textarea").val()
            };
            $.get("./index.aspx", _defs, function (_returned) {
                if (_returned == "SUCCESS") {
                    $('#note_window').remove();
                    please_wait("stop");
                    gv_quotes.Refresh();
                }
                else {
                    please_wait("stop");
                    $(obj).removeAttr('disabled');
                    alert(_returned);
                }
            });
        }
        function note_show_edit(obj) {
            var exists = $("#note_window").size() > 0;
            var o = $(obj).offset();
            var x = o.left;
            var y = o.top;
            var quote_id = $(obj).attr('data-qid');
            if (exists && $("#note_window")) {
                $("#note_window").remove();
            }
            var html = "<div id='note_window'>";
            html += "<b>Add Note:</b>\
										<br/><textarea style='width:99%;height:100px;' maxlength='512'></textarea>\
										<br><button style='font-size:11px;font-weight:bold;' onclick='handle_note_edit(this)' data-quote_id='" + quote_id + "' type='button'><img align='absmiddle' src='/images/icon/icon[save].gif'/> Save</button>\
										<button style='font-size:11px;font-weight:bold;' onclick=\"$('#note_window').remove();\"><img align='absmiddle' src='/images/icon/icon[delete].gif'/> Close</button>";
            html += "</div>";
            $("body").append(html);
            var off = $(obj).offset();
            $("#note_window").css({
                "position": "absolute",
                "top": off.top + "px",
                "left": off.left + $(obj).width() + "px",
                "width": "300px",
                "padding": "5px",
                "border": "solid 1px #cc7",
                "background-color": "#ff7"
            }).find("textarea").focus().val();
            var left = $(document).outerWidth() - $(window).width();
            $('body, html').scrollLeft(left);
        }
        function handle_note_edit(obj) {
            $(obj).attr("disabled", true);
            please_wait("start");
            var _defs = {
                a: "handle_note_edit",
                qid: $(obj).attr('data-quote_id'),
                note: $("#note_window").find("textarea").val()
            };
            $.get("./index.aspx", _defs, function (_returned) {
                if (_returned != "Saving of note failed.") {
                    memx.SetText(_returned);
                    $('#note_window').remove();
                    please_wait("stop");
                }
                else {
                    please_wait("stop");
                    $(obj).removeAttr('disabled');
                    alert(_returned);
                }
            });
        }
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
        });
        function EndReqHandler() {
            please_wait("stop");
        }
        function BeginReqHandler() {
            please_wait("start");
        }
       
     </script>
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
	</asp:ScriptManager>
	
	<lc:LayoutControl ID="layout" runat="server" is_private="True" ShowExcelExport="True" />
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT quote_chance_id, quote_chance_name FROM quote_chance"></asp:SqlDataSource>
         
		<dx:ASPxCallback ID="cb_chkprivate" ClientInstanceName="cb_chkprivate" OnCallback="cb_chkprivate_Callback"    runat="server">
		    <ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" EndCallback="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
		</dx:ASPxCallback>
           
			<dx:ASPxGridView ID="gv_quotes" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_quotes" Theme="NETheme01" KeyFieldName="quote_n" OnCommandButtonInitialize="gv_quotes_CommandButtonInitialize" OnCustomCallback="gv_quotes_CustomCallback" OnCustomJSProperties="gv_quotes_CustomJSProperties" OnHtmlDataCellPrepared="gv_quotes_HtmlDataCellPrepared" OnHtmlEditFormCreated="gv_quotes_HtmlEditFormCreated" OnHtmlRowPrepared="gv_quotes_HtmlRowPrepared" OnPageIndexChanged="gv_quotes_PageIndexChanged" PreviewFieldName="Description">
				<SettingsPager NumericButtonCount="5" PageSize="25" Position="TopAndBottom">
					<AllButton Text="All">
					</AllButton>
					<NextPageButton Text="Next &gt;">
					</NextPageButton>
					<PrevPageButton Text="&lt; Prev">
					</PrevPageButton>
				</SettingsPager>
				<TotalSummary>
					<dx:ASPxSummaryItem DisplayFormat="c2" FieldName="price" ShowInColumn="Price" SummaryType="Sum" />
				    <dx:ASPxSummaryItem DisplayFormat="c2" FieldName="pipeline" ShowInColumn="Pipeline" SummaryType="Sum" />
                    <dx:ASPxSummaryItem DisplayFormat="c2" FieldName="expected_value" ShowInColumn="Exp. Value" SummaryType="Sum" />
                    <dx:ASPxSummaryItem DisplayFormat="n1" FieldName="hours_spent" ShowInColumn="Hours Spent" SummaryType="Sum" />
				    <dx:ASPxSummaryItem DisplayFormat="c2" FieldName="dollar_margin" ShowInColumn="$ Margin" SummaryType="Sum" />
				</TotalSummary>
				<GroupSummary>
					<dx:ASPxSummaryItem FieldName="Customer" ShowInColumn="Customer" SummaryType="Count" />
					<dx:ASPxSummaryItem FieldName="Status" ShowInColumn="Status" SummaryType="Count" />
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="Job Cost" ShowInColumn="Branch" SummaryType="Sum" />
					<dx:ASPxSummaryItem FieldName="price" ShowInColumn="PM" SummaryType="Sum" ValueDisplayFormat="{0:C2}" />
				</GroupSummary>
				<ClientSideEvents RowClick="function(s, e) {

}" /><SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
				<Columns>
					<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" Width="25px" ShowEditButton="false">
						
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn Caption="Quote #" FieldName="quote" Name="quote_id" ReadOnly="True" ToolTip="Quote #" VisibleIndex="1" Width="75px" MinWidth="10">
						<DataItemTemplate>
							<asp:HyperLink ID="hl_quote"  runat="server" Font-Bold="True"  NavigateUrl="<%# string.Format(&quot;javascript:boing('/#/opens/65/quotes/{0}/{1}','quote',1035,800);&quot;,Eval(&quot;quote&quot;),Eval(&quot;rev&quot;)) %>" Text='<%# Eval("quote").ToString() %>' Width="100%"></asp:HyperLink>
						</DataItemTemplate>
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Rev" FieldName="rev" ReadOnly="True" ToolTip="Revision" VisibleIndex="2" Width="25px" MinWidth="10">
						<Settings HeaderFilterMode="CheckedList" />
					</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" ReadOnly="True" ToolTip="Business Unit" VisibleIndex="3" Width="70px" MinWidth="10">
				    <Settings AutoFilterCondition="Equals" HeaderFilterMode="CheckedList" />
				</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="customer" ReadOnly="True" ToolTip="Customer" VisibleIndex="4" Width="100px" Caption="Customer" MinWidth="10">
						<Settings AutoFilterCondition="Contains" FilterMode="DisplayText" SortMode="DisplayText" HeaderFilterMode="CheckedList" />
						<DataItemTemplate>
							<asp:HyperLink ID="hl_customer" runat="server" Font-Bold="True" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/index.aspx?customer_id={0}','customer',1035,800);&quot;,Eval(&quot;customer_id&quot;)) %>" Text='<%# Eval("customer").ToString() %>' Width="100%"></asp:HyperLink>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Contact" FieldName="contact" ReadOnly="True" ToolTip="Contact" VisibleIndex="5" Width="80px" MinWidth="10">
						<Settings AutoFilterCondition="BeginsWith" HeaderFilterMode="CheckedList" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Desc" FieldName="description" ReadOnly="True" ToolTip="Quote Description" VisibleIndex="6" Width="200px" MinWidth="10">
						<CellStyle Wrap="True">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Price" FieldName="price" ReadOnly="True" ToolTip="Price" VisibleIndex="7" Width="70px" MinWidth="10">
						<PropertiesTextEdit DisplayFormatString="#,###">
							
						</PropertiesTextEdit>
						<CellStyle BackColor="#CCFFCC">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Status" FieldName="status" ReadOnly="True" ToolTip="Status" VisibleIndex="8" Width="100px" MinWidth="10">
						<Settings HeaderFilterMode="CheckedList" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Open Date" FieldName="open_date" ReadOnly="True" ToolTip="Open Date" VisibleIndex="9" Width="100px" MinWidth="10">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd">
							
						</PropertiesDateEdit>
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="PM" FieldName="pm" ReadOnly="True" ToolTip="Project Manager" VisibleIndex="10" Width="100px" MinWidth="10">
						<Settings AutoFilterCondition="Equals" HeaderFilterMode="CheckedList" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="customer_id" ReadOnly="True" ShowInCustomizationForm="False" Visible="False" VisibleIndex="12" Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="contact_id" ReadOnly="True" ShowInCustomizationForm="False" Visible="False" VisibleIndex="13" Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Contact" FieldName="contact" ReadOnly="True" ToolTip="Contact" Visible="False" VisibleIndex="11" Width="50px" MinWidth="10">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" ToolTip="Notes" VisibleIndex="14" Width="150px" MinWidth="200">
						<DataItemTemplate>
							<dx:ASPxMemo ID="mem" runat="server" ReadOnly="True" Theme="NETheme01" ClientInstanceName="mem" Height="20px" OnCustomJSProperties="mem_CustomJSProperties" Text='<%# Eval("notes") %>' data-quote_id = "" Width="100%" OnInit="mem_Init" OnPreRender="mem_PreRender" OnDataBound="mem_DataBound">
								<ClientSideEvents Init="function(s, e) {
	var text = s.GetText().replace(/\r/g, '');
	if (text.length &gt;1 )
		{
			s.SetHeight(40);
			s.GetMainElement().style.backgroundColor = &quot;yellow&quot;
			s.GetInputElement().style.backgroundColor = &quot;yellow&quot;
		}
}" TextChanged="function(s,e){cb_chkprivate.PerformCallback(s.cpKeyValue+'|' + s.GetText()+ '|note');}"  />
							</dx:ASPxMemo>
						</DataItemTemplate>
					    <EditItemTemplate>
                          
                        </EditItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="quote_n" ReadOnly="True" Visible="False" VisibleIndex="16" Width="25px" ShowInCustomizationForm="False">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Comp Date" FieldName="completion" Name="Completion" ReadOnly="True" ToolTip="Completed Date (used on dashboard for pipeline report)" VisibleIndex="15" Width="70px" MinWidth="10">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
						</PropertiesDateEdit>
						<Settings AutoFilterCondition="LessOrEqual" />
						<DataItemTemplate>
							<dx:ASPxDateEdit ID="dtecomp" runat="server" Theme="NETheme01" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" AnimationType="None" Value='<%# Bind("completion") %>' Width="100%" OnCustomJSProperties="dtecomp_CustomJSProperties">
								<ClientSideEvents DateChanged="function(s,e)
		{
		cb_chkprivate.PerformCallback(s.cpKeyValue+'|' + s.GetText()+ '|completion_date');

		}" />
							</dx:ASPxDateEdit>
						</DataItemTemplate>
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn Caption="Due Date" FieldName="date_due" ToolTip="Due Date" VisibleIndex="17" Width="100px" MinWidth="10">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
						</PropertiesDateEdit>
                        <DataItemTemplate>
							<dx:ASPxDateEdit ID="dtedue_date" runat="server" Theme="NETheme01" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" AnimationType="None" Value='<%# Bind("date_due") %>' Width="100%" OnCustomJSProperties="date_due_CustomJSProperties" 
                               >
								<ClientSideEvents DateChanged="function(s,e)
		{
		cb_chkprivate.PerformCallback(s.cpKeyValue+'|' + s.GetText()+ '|date_due');

		}" GotFocus="function(s,e){if (s.cpdr!='')
                                    {
                                   alert(s.cpdr); 
                                    }}" />
							</dx:ASPxDateEdit>
						</DataItemTemplate>
						<Settings AutoFilterCondition="LessOrEqual" />
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Customer Entered Notes" FieldName="cust_notes" ToolTip="Customer Entered Notes" VisibleIndex="18" Width="50px" MinWidth="10">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataSpinEditColumn Caption="Chance %" FieldName="pct_chance" VisibleIndex="19" Width="50px" MinWidth="10">
						<PropertiesSpinEdit DisplayFormatString="g" Increment="30" MaxValue="90" NumberType="Integer" LargeIncrement="30" MinValue="0" AllowUserInput="False" ShowOutOfRangeWarning="False">
							
						</PropertiesSpinEdit>
						<DataItemTemplate>
							<dx:ASPxSpinEdit ID="spn_chance" runat="server" ClientInstanceName="spn_chance" Height="25px" Increment="30" MaxValue="90" NumberType="Integer" OnInit="spn_chance_Init" Value='<%# Eval("pct_chance") %>' Width="100%" LargeIncrement="30" MinValue="0" AllowUserInput="False" oncustomjsproperties="spn_chance_CustomJSProperties" ShowOutOfRangeWarning="False">
								<SpinButtons HorizontalSpacing="0">
								</SpinButtons>
								<ClientSideEvents ValueChanged="function(s, e) {
	cb_chkprivate.PerformCallback(s.cpKeyValue+'|' + s.GetValue()+ '|pct_chance');
}" />
							</dx:ASPxSpinEdit>
						</DataItemTemplate>
					</dx:GridViewDataSpinEditColumn>
					<dx:GridViewDataComboBoxColumn Caption="Chance_Reason" FieldName="pct_chance_reason" VisibleIndex="20" Width="50px" MinWidth="10">
						<PropertiesComboBox DataSourceID="SqlDataSource1" TextField="quote_chance_name" ValueField="quote_chance_id" ValueType="System.Int32">
							
						</PropertiesComboBox>
						<Settings HeaderFilterMode="CheckedList" />
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataTextColumn Caption="Hours Spent" FieldName="hours_spent" VisibleIndex="21" Width="50px" MinWidth="10">
						<Settings AutoFilterCondition="GreaterOrEqual" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="% Margin" FieldName="margin" MinWidth="25" Visible="False" VisibleIndex="27">
						<PropertiesTextEdit DisplayFormatString="{0:P2}">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Exp. Value" FieldName="expected_value" Visible="False" VisibleIndex="23" MinWidth="10" Width="75px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Verified Date" FieldName="verified_date" MinWidth="10" VisibleIndex="22" Width="75px">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
							
						</PropertiesDateEdit>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn Caption="Sent Date" FieldName="last_sent_date" MinWidth="10" VisibleIndex="24" Width="75px">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
							
						</PropertiesDateEdit>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn Caption="Last Printed Date" FieldName="last_print_date" MinWidth="10" VisibleIndex="25" Width="75px">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
							
						</PropertiesDateEdit>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Account Manager" FieldName="acct_manager" Visible="False" VisibleIndex="73" MinWidth="50">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="PM To Follow Up On Quote" FieldName="follow_up" ToolTip="25" Visible="False" VisibleIndex="75" MinWidth="50">
						<DataItemTemplate>
							<dx:ASPxCheckBox ID="cb_followup" runat="server"  OnCustomJSProperties="cb_followup_CustomJSProperties" Checked='<%# Bind("follow_up") %>'>
								<ClientSideEvents CheckedChanged="function(s,e)
		{
		cb_chkprivate.PerformCallback(s.cpKeyValue+'|' + s.GetChecked()+ '|follow_up');

		}" />
							</dx:ASPxCheckBox>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Killed Date" FieldName="killed_date" Visible="False" VisibleIndex="78" Width="100px">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Country" FieldName="country" VisibleIndex="26" Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="BDM" FieldName="BDM" MinWidth="100" Visible="False" VisibleIndex="79">
					</dx:GridViewDataTextColumn>
				    <dx:GridViewDataDateColumn Caption="Start Date From WO" FieldName="startdate" MinWidth="100" Visible="False" VisibleIndex="80">
				    </dx:GridViewDataDateColumn>
				    <dx:GridViewDataTextColumn Caption="Pipeline" FieldName="pipeline" VisibleIndex="81" Width="50px">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="status_id" FieldName="status_id"  Visible="False" VisibleIndex="82">
					</dx:GridViewDataTextColumn>
				    <dx:GridViewDataDateColumn Caption="Exp PO date" FieldName="exp_podate" MinWidth="20" VisibleIndex="83" Width="50px">
                        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                        </PropertiesDateEdit>
				        <DataItemTemplate>
				            <dx:ASPxDateEdit ID="dteexp_podate" runat="server" Theme="NETheme01" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" AnimationType="None" Value='<%# Bind("exp_podate") %>' Width="100%" OnCustomJSProperties="dtecomp_CustomJSProperties">
				                <ClientSideEvents DateChanged="function(s,e)
		{
		cb_chkprivate.PerformCallback(s.cpKeyValue+'|' + s.GetText()+ '|exp_podate');

		}" />
				            </dx:ASPxDateEdit>
				        </DataItemTemplate>
                    </dx:GridViewDataDateColumn>
				<dx:GridViewDataTextColumn Caption="$ Margin" FieldName="dollar_margin" MinWidth="25" Visible="False" VisibleIndex="84">
				    <PropertiesTextEdit DisplayFormatString="{0:N2}">
				    </PropertiesTextEdit>
				</dx:GridViewDataTextColumn>
                    <dx:GridViewDataCheckColumn Caption="Parallel Bid" FieldName="parallel_bid" MinWidth="25" Visible="False" VisibleIndex="85" >
				      <DataItemTemplate>
                          <dx:ASPxCheckBox ID="cb_parallel_bid" runat="server"  OnCustomJSProperties="cb_parallel_bid_CustomJSProperties" Checked='<%# Bind("parallel_bid") %>' >
								<ClientSideEvents CheckedChanged="function(s,e)
		{
		yoyo(s.cpKeyValue+'|' + s.GetChecked()+ '|parallel_bid');

		}" />
							</dx:ASPxCheckBox>
                           </DataItemTemplate>
				</dx:GridViewDataCheckColumn>
				    <dx:GridViewDataTextColumn Caption="City" FieldName="address_city" VisibleIndex="86" Visible="False">
				    </dx:GridViewDataTextColumn>
				    <dx:GridViewDataTextColumn Caption="Province" FieldName="address_prov" Visible="False" VisibleIndex="87">
				    </dx:GridViewDataTextColumn>
				    <dx:GridViewDataDateColumn Caption="Expected Start Date" FieldName="QO_startdate" MinWidth="100" Visible="False" VisibleIndex="88">
				        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
				        </PropertiesDateEdit>
				        <Settings AutoFilterCondition="LessOrEqual" />
				        <DataItemTemplate>
				            <dx:ASPxDateEdit ID="dteQO_startdate" runat="server" Theme="NETheme01" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" AnimationType="None" Value='<%# Bind("QO_startdate") %>' Width="100%" OnCustomJSProperties="QO_startdate_CustomJSProperties">
				                <ClientSideEvents DateChanged="function(s,e)
		{
		cb_chkprivate.PerformCallback(s.cpKeyValue+'|' + s.GetText()+ '|date_expected_start');

		}" />
				            </dx:ASPxDateEdit>
				        </DataItemTemplate>
				        <CellStyle Wrap="False">
				        </CellStyle>
				    </dx:GridViewDataDateColumn>
                      <dx:GridViewDataTextColumn Caption="Total Labour QTY" FieldName="total_labour_qty" MinWidth="20" VisibleIndex="89">
				    </dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Controls Person" FieldName="controls_person" MinWidth="100"  VisibleIndex="90">
					</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="SAM" FieldName="SAM" MinWidth="100"  VisibleIndex="120" Visible="False">
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="ADS" FieldName="ADS" MinWidth="100"  VisibleIndex="121" Visible="False">
				</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Acting BDM" FieldName="acting_bdm" MinWidth="100" Visible="False" VisibleIndex="122">
					</dx:GridViewDataTextColumn>
				</Columns>
				<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" ShowGroupPanel="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" UseFixedTableLayout="True" />
				<SettingsPopup>
					<customizationwindow horizontalalign="LeftSides" verticalalign="TopSides" />
					<HeaderFilter Height="500px" Width="200px" />
				</SettingsPopup>
				
				<Templates>
					<TitlePanel>
						<table cellpadding="2" cellspacing="2" style="font-family: Arial; font-size: x-small; font-weight: bold;" width="100%">
							<tr>
								<td colspan="2" style="font-size: 13px;" nowrap="nowrap">
									Percent Chance
								</td>
								<td style="font-size: 13px;">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td>
									30%
								</td>
								<td bgcolor="#FFFFCC" style="color: #000;">
									Budget quote/ Bid Tender
								</td>
								<td nowrap="nowrap" style="font-size: medium;">
									Quote Hit Rate:&nbsp;
									<dx:ASPxLabel ID="lblhr" runat="server" ClientInstanceName="lblhr" Style="font-family: Arial, Helvetica, sans-serif; font-size: large; font-weight: 700" Text="N/A" OnInit="lblhr_Init">
										<ClientSideEvents Init="function(s, e) {

}" />
									</dx:ASPxLabel>
								</td>
							</tr>
							<tr>
								<td>
									60%
								</td>
								<td bgcolor="#FFFF99" style="color: #000;">
									We are 1 of 3 contractors bidding
								</td>
								<td>
									&nbsp;
								</td>
							</tr>
							<tr>
								<td>
									90%
								</td>
								<td bgcolor="#CCFF99" width="100px" style="color: #000;">
									We are the only contractor bidding
								</td>
								<td width="100%">
									&nbsp;
								</td>
							</tr>
						</table>
					</TitlePanel>
					<EditForm>
						<dx:ASPxCallbackPanel ID="cbp_edit_note" runat="server" ClientInstanceName="cbp_edit_note" OnCallback="cbp_edit_note_Callback" Width="500px">
							<PanelCollection>
								<dx:PanelContent runat="server">
									<table cellpadding="2" cellspacing="0" width="700">
									    <tr>
									        <td style="width: 150px">
									            <b>Expected End Date:</b>
									        </td>
									        <td>
									            <dx:ASPxDateEdit ID="date_expected" Theme="NETheme01" runat="server" DateOnError="Today" EditFormat="Custom" EditFormatString="yyyy-MM-dd" AnimationType="None" Value='<%# Eval("completion") %>'>
									            </dx:ASPxDateEdit>
									            &nbsp;
									        </td>
									    </tr>
									    <tr>
									        <td style="width: 150px">
									            <b>Expected Start Date:</b>
									        </td>
									        <td>
									            <dx:ASPxDateEdit ID="QO_startdate" Theme="NETheme01" runat="server" DateOnError="Today" EditFormat="Custom" EditFormatString="yyyy-MM-dd" AnimationType="None" Value='<%# Eval("QO_startdate") %>'>
									            </dx:ASPxDateEdit>
									            &nbsp;
									        </td>
									    </tr>
										<tr>
											<td class="style2">
												<b>Due Date:</b>
											</td>
											<td class="style3">
												<dx:ASPxDateEdit ID="date_due" Theme="NETheme01" runat="server" DateOnError="Today" EditFormat="Custom" EditFormatString="yyyy-MM-dd" AnimationType="None" Value='<%# Eval("date_due") %>'>
												</dx:ASPxDateEdit>
											</td>
										</tr>
										<tr>
											<td style="width: 150px">
												<b> Chance %:</b>
											</td>
											<td>
												<dx:ASPxSpinEdit ID="spin_success_pct" ShowOutOfRangeWarning="False" runat="server" Height="21px" MaxValue="90" Increment="30" LargeIncrement="30" MinValue="0" AllowUserInput="False" NumberType="Integer" Value='<%# Eval("pct_chance") %>' Width="50px">
												</dx:ASPxSpinEdit>
											</td>
										</tr>
										<tr>
											<td style="width: 150px">
												<strong>Reason for Chance: </strong>
											</td>
											<td>
												<dx:ASPxComboBox ID="cbchance" Theme="NETheme01" runat="server" ClientInstanceName="cbchance" DataSourceID="SqlDataSource1" OnDataBound="cbchance_DataBound" Text='<%# Eval("pct_chance_reason") %>' TextField="quote_chance_name" ValueField="quote_chance_id" ValueType="System.Int32">
												</dx:ASPxComboBox>
											</td>
										</tr>
										<tr>
											<td style="width: 150px">
												<b>Notes:</b>
											</td>
											<td>
												  <dx:ASPxMemo ID="memx" runat="server" ClientInstanceName="memx" data-quote_id="" Height="100px" OnCustomJSProperties="memx_CustomJSProperties" OnDataBound="memx_DataBound" OnInit="memx_Init" OnPreRender="mem_PreRender" ReadOnly="True" Text='<%# Eval("notes") %>' Theme="NETheme01" Width="100%">
                                <ClientSideEvents Init="function(s, e) {
	var text = s.GetText().replace(/\r/g, '');
	if (text.length &gt;1 )
		{
			s.SetHeight(100);
			s.GetMainElement().style.backgroundColor = &quot;yellow&quot;
			s.GetInputElement().style.backgroundColor = &quot;yellow&quot;
		}
}" TextChanged="function(s,e){cb_chkprivate.PerformCallback(s.cpKeyValue+'|' + s.GetText()+ '|note');}" />
                            </dx:ASPxMemo>
											</td>
										</tr>
                                        
										<tr>
											<td align="center" style="width: 150px">
												<dx:ASPxButton ID="btn_update" runat="server" Theme="NETheme01" AutoPostBack="False" Text="Update">
													<Image Url="~/images/icon/icon[save].gif">
													</Image>
													<ClientSideEvents Click="function(s, e) {
	cbp_edit_note.PerformCallback();
}" />
												</dx:ASPxButton>
											</td>
											<td>
												<dx:ASPxButton ID="btn_cancel" runat="server" Theme="NETheme01" AutoPostBack="False" Text="Cancel">
													<Image Url="~/images/icon/icon[undo].gif">
													</Image>
													<ClientSideEvents Click="function(s, e) {
	gv_quotes.CancelEdit();
}" />
												</dx:ASPxButton>
											</td>
										</tr>
									</table>
								</dx:PanelContent>
							</PanelCollection>
							<ClientSideEvents EndCallback="function(s, e) {
	gv_quotes.CancelEdit();
}" />
						</dx:ASPxCallbackPanel>
					</EditForm>
				</Templates>
				<SettingsEditing EditFormColumnCount="1" Mode="EditForm" />
				<SettingsBehavior AutoFilterRowInputDelay="6000" ColumnResizeMode="Control" EnableRowHotTrack="True" EnableCustomizationWindow="True"/>
        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides"/>
			</dx:ASPxGridView>
            
		
			<br />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
