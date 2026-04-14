<%@ Page  Language="C#" 
    MasterPageFile="~/IntraDefault.master" 
    AutoEventWireup="true" Theme="NETheme01" Inherits="sections_hr_expense_index"  EnableTheming="True" Title="Expense Report" Codebehind="index.aspx.cs" %> 

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>







<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>


<asp:content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
</asp:content>
<asp:content ID="Content2" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server"></div>
</asp:content>
<asp:content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
		<asp:scriptmanager ID="sm_expensereport" runat="server" EnablePageMethods="True">
	</asp:scriptmanager>
            <script type="text/javascript">


                String.repeat = function (chr, count) {
                    var str = "";
                    for (var x = 0; x < count; x++) { str += chr };
                    return str;
                }
                String.prototype.padL = function (width, pad) {
                    if (!width || width < 1)
                        return this;

                    if (!pad) pad = " ";
                    var length = width - this.length
                    if (length < 1) return this.substr(0, width);

                    return (String.repeat(pad, length) + this).substr(0, width);
                }
                String.prototype.padR = function (width, pad) {
                    if (!width || width < 1)
                        return this;

                    if (!pad) pad = " ";
                    var length = width - this.length
                    if (length < 1) this.substr(0, width);

                    return (this + String.repeat(pad, length)).substr(0, width);
                }

                Date.prototype.formatDate = function (format) {
                    var date = this;
                    if (!format)
                        format = "MM/dd/yyyy";

                    var month = date.getMonth() + 1;
                    var year = date.getFullYear();

                    format = format.replace("MM", month.toString().padL(2, "0"));

                    if (format.indexOf("yyyy") > -1)
                        format = format.replace("yyyy", year.toString());
                    else if (format.indexOf("yy") > -1)
                        format = format.replace("yy", year.toString().substr(2, 2));

                    format = format.replace("dd", date.getDate().toString().padL(2, "0"));

                    var hours = date.getHours();
                    if (format.indexOf("t") > -1) {
                        if (hours > 11)
                            format = format.replace("t", "pm")
                        else
                            format = format.replace("t", "am")
                    }
                    if (format.indexOf("HH") > -1)
                        format = format.replace("HH", hours.toString().padL(2, "0"));
                    if (format.indexOf("hh") > -1) {
                        if (hours > 12) hours - 12;
                        if (hours == 0) hours = 12;
                        format = format.replace("hh", hours.toString().padL(2, "0"));
                    }
                    if (format.indexOf("mm") > -1)
                        format = format.replace("mm", date.getMinutes().toString().padL(2, "0"));
                    if (format.indexOf("ss") > -1)
                        format = format.replace("ss", date.getSeconds().toString().padL(2, "0"));
                    return format;
                }

                function save_currencies(date) {
                    $.get('https://openexchangerates.org/api/historical/' + date + '.json', { app_id: '395f167f51744ac3a90d9e69a895d640' }, function (data) {
                      

                        PageMethods.set_currency(date, "USD", data.rates.USD);
                                       PageMethods.set_currency(date, "EUR", data.rates.EUR);
                                         PageMethods.set_currency(date, "CAD", data.rates.CAD);
                    });
                }

                function get_currency() {

                    var today = new Date();
                    var start = new Date();
                    var end = new Date();
                    var start_string;
                    var end_string;
                    var newDate2 = new Date();
                    
                    newDate2.setDate(today.getDate() - 1);

                    var dd = newDate2.getDate();
                    var mm = newDate2.getMonth() + 1; //January is 0!
                    var yyyy = newDate2.getFullYear();

                    if(dd<10) {
                        dd='0'+dd
                    } 

                    if(mm<10) {
                        mm='0'+mm
                    } 

                    var dd_end = end.getDate();
                    var mm_end = end.getMonth() + 1; //January is 0!
                    var yyyy_end = end.getFullYear();

                    if (dd_end < 10) {
                        dd_end = '0' + dd_end
                    }

                    if (mm_end < 10) {
                        mm_end = '0' + mm_end
                    }

                    start_string = mm+'/'+dd+'/'+yyyy;
                    end_string = mm_end + '/' + dd_end + '/' + yyyy_end;
                   
                    start = new Date(start_string);
                    end = new Date(end_string);
                   
                    var dated;
                    while (start <= end) {
                        dated = start;
                        dated = dated.formatDate("yyyy-MM-dd");
                       
               //     confirm("On " + dated);
                        save_currencies(dated);
     
                        var newDate = start.setDate(start.getDate() + 1);
                        start = new Date(newDate);
                       
                    }

                   // var date = "2010-10-10";

                



                }
				function row_click(s,e)
					{
					//var id_expense			= s.GetRowValues(e.visibleIndex, "id;seller", row_callback);
					}
				function row_callback(vals)
					{
					get_item(vals);
					}
				function get_item(_vals)
					{
					var vals_obj = _vals.toString().split(',');
					var seller = vals_obj[1];
					var id = vals_obj[0];
					var page = seller == "Per Diem" ? "index.aspx?id_perdiem" : "if_newexpense.aspx?id_expense";
					boing("/sections/member/expense/"+page+"="+id, "EXPPER"+id, 800, 465);
					}
				function get_wo(e, id)
					{
					e = e || window.event;
					e.stopPropagation();
					boing('/sections/workorder/index.aspx?woprog_id='+id,'WO',1035,800);
					}
				function get_attachment(e, id, ext)
					{
					e = e || window.event;
					e.stopPropagation();
					    boing( ext, 'Expense', 800, 600);
					}
				page_obj.update_panel_progress.bind();
			</script>

<asp:updatepanel ID="Updatepanel1" runat="server">
<ContentTemplate>
		<table width="500">
		    <tr>
		        <td  colspan="2" width="100px">Tax Entity:</td>
		        <td align="right"><dx:ASPxComboBox runat="server" ID="combo_tax_entity" Theme="NETheme01" Width="100%" ValueField="id" TextField="ddl_name" ValueType="System.Int32"  
		                                           AutoPostBack="true" OnSelectedIndexChanged="combo_tax_entity_SelectedIndexChanged"></dx:ASPxComboBox>
                
		        </td>
		    </tr>
		    <tr>
		        <td width="500" colspan="3">
		            <dx:ASPxCheckBoxList ID="cl_business_units" runat="server" 
		                                 RepeatColumns="3" TextField="name"
		                                 Theme="NETheme01" ValueField="id" ClientInstanceName="cl_business_units" Width="100%">
		            </dx:ASPxCheckBoxList>

		        </td>
		    </tr>
			
			<tr>
				<td rowspan="2"><asp:radiobutton id="rdo_date" runat="server" groupname="datepp" /></td>
                <td width="200"><b>Start Date:</b></td>
                <td width="300">
                    <dx:ASPxDateEdit ID="dte_start" runat="server" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                    </dx:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td width="200"><b>End Date:</b></td>
                <td width="300">
                    <dx:ASPxDateEdit ID="dte_end" runat="server" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                    </dx:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td width="200" colspan="2" align="center"> - OR - </td>
                <td width="300">&nbsp;</td>
            </tr>
			<tr>
				<td rowspan="1"><asp:radiobutton id="rdo_payperiod" runat="server" groupname="datepp" /></td>
				<td><b>Pay Period:</b></td>
				<td>
					<dx:ASPxComboBox ID="ddl_payperiods" DataSourceID="ds_payperiods" ClientInstanceName="ddl_payperiods" Width="100%" ValueField="id" TextField="date_spread" runat="server" ValueType="System.Int32">
					</dx:ASPxComboBox>
				</td>
			</tr> 
            <tr>
                <td width="200" colspan="2">&nbsp;</td>
                <td width="300">
                    <dx:ASPxButton ID="btn_go" runat="server" onclick="ASPxButton1_Click" Text="Go-&gt;" Theme="NETheme01">
                    </dx:ASPxButton>
                </td>
            </tr>
		</table>
    </ContentTemplate>
    </asp:updatepanel>
		<br />

		    <uc1:layout_control ID="layout" runat="server" ShowExcelExport="True" ShowPDFExport="True" />
<asp:updatepanel ID="up_expensereport" runat="server">
    <ContentTemplate>
    	<dx:ASPxGridView ID="gv_expensereport" ClientInstanceName="gv_expensereport" runat="server" AutoGenerateColumns="False" Width="100%" oncustomcallback="gv_expensereport_CustomCallback" EnableTheming="True" Theme="NETheme01" onhtmldatacellprepared="gv_expensereport_HtmlDataCellPrepared" KeyFieldName="id" OnCustomJSProperties="gv_expensereport_CustomJSProperties">
			<TotalSummary>
				<dx:ASPxSummaryItem DisplayFormat="Total: {0:C2}" FieldName="amount" ShowInColumn="Total Due" ShowInGroupFooterColumn="Total Due(Total Including Taxes)" SummaryType="Sum" />
				<dx:ASPxSummaryItem DisplayFormat="Total: {0:C2}" FieldName="pre_tax_amount" ShowInColumn="Pre-Tax Amount" ShowInGroupFooterColumn="Pre-Tax Amount(Total Excluding Taxes)" SummaryType="Sum" />
				<dx:ASPxSummaryItem DisplayFormat="Total: {0:C2}" FieldName="distance_reimbursement" ShowInColumn="Reimbursement for Distance Traveled" ShowInGroupFooterColumn="Reimbursement for Distance Traveled" SummaryType="Sum" />
			</TotalSummary>
			<GroupSummary>
				<dx:ASPxSummaryItem DisplayFormat="Sub Total: {0:C2}" FieldName="amount" ShowInGroupFooterColumn="Total Due(Total Including Taxes)" SummaryType="Sum" ValueDisplayFormat="C2" />
				<dx:ASPxSummaryItem DisplayFormat="Sub Total: {0:C2}" FieldName="pre_tax_amount" ShowInGroupFooterColumn="Pre-Tax Amount(Total Excluding taxes)" SummaryType="Sum" ValueDisplayFormat="C2" />
				<dx:ASPxSummaryItem DisplayFormat="Sub Total: {0:C2}" FieldName="distance_reimbursement" ShowInGroupFooterColumn="Reimbursement for Distance Traveled" SummaryType="Sum" ValueDisplayFormat="C2" />
			</GroupSummary>
			<Columns>				
				<dx:GridViewDataTextColumn Caption="Description" VisibleIndex="4" FieldName="description">
					<DataItemTemplate>
						<%# Eval("description") %>
					</DataItemTemplate>
				</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Pre-Tax Amount(Total Excluding Taxes)" VisibleIndex="4" FieldName="pre_tax_amount">
					<PropertiesTextEdit DisplayFormatString="C2">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				    <FooterCellStyle HorizontalAlign="Right">
                    </FooterCellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Total Due(Total Including Taxes)" VisibleIndex="5" FieldName="amount">
					<PropertiesTextEdit DisplayFormatString="C2">
					</PropertiesTextEdit>
				    <CellStyle HorizontalAlign="Center">
                    </CellStyle>
                    <FooterCellStyle HorizontalAlign="Right">
                    </FooterCellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Currency" VisibleIndex="6" FieldName="currency" Width="65px">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
			
				
				<dx:GridViewDataTextColumn Caption="Type" VisibleIndex="7" FieldName="expense_type">
					<Settings HeaderFilterMode="CheckedList" />
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Account Code" VisibleIndex="8" FieldName="account_code">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="WO Reference" VisibleIndex="9" FieldName="woprog_id">
					<DataItemTemplate>
						<a ID="ASPxHyperLink1" runat="server" onclick="<%# string.Format(&quot;get_wo(event, {0});&quot;, Eval(&quot;woprog_id&quot;)) %>" href="javascript:void(0);"><%# Eval("woprog_id") %></a>
					</DataItemTemplate>
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Attendees" VisibleIndex="10" FieldName="attendees">
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Distance (Personal Car Only)" VisibleIndex="11" FieldName="distance">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Reimbursement for Distance Traveled" Width="50px" VisibleIndex="12" FieldName="distance_reimbursement">
					<PropertiesTextEdit DisplayFormatString="C2">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Receipt" FieldName="receipt_number" VisibleIndex="13" Width="50px">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Request Date" FieldName="date_purchased" VisibleIndex="1" Width="150px">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Employee" FieldName="full_name" VisibleIndex="3">
				    <Settings HeaderFilterMode="CheckedList" />
				</dx:GridViewDataTextColumn>
			    <dx:GridViewDataTextColumn Caption="id" FieldName="id" Visible="False" VisibleIndex="14">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Approve/Deny" Visible="True" VisibleIndex="15">
	                <Settings HeaderFilterMode="CheckedList" />
	                <dataitemtemplate>
						<dx:aspxbutton id="app_exp" runat="server" autopostback="false">
							<image url="~/images/icon/icon[1].gif" height="16px" width="16px" />
						</dx:aspxbutton>
						<dx:aspxbutton id="del_exp" runat="server" autopostback="false">
							<image url="~/images/icon/icon[0].gif" height="16px" width="16px" />
						</dx:aspxbutton>
	                </dataitemtemplate>
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
                </dx:GridViewDataTextColumn>
              
				<dx:gridviewdatatextcolumn caption="Status" fieldname="STATUS"  visibleindex="0" width="50">
				    <Settings HeaderFilterMode="CheckedList" />
				</dx:gridviewdatatextcolumn>
			    <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" VisibleIndex="16">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataTextColumn>
			    <dx:GridViewDataTextColumn Caption="Seller" FieldName="seller" VisibleIndex="52">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="Date Added" FieldName="date_requested" VisibleIndex="53" Visible="false">
                    <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm" EditFormat="Custom" EditFormatString="yyyy-MM-dd HH:mm">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                 <dx:GridViewDataTextColumn Caption="Customer" FieldName="customername" VisibleIndex="54" Visible="false">
                    
                </dx:GridViewDataTextColumn>
			</Columns>
			<clientsideevents endcallback="function(s,e){please_wait('stop');if(s.cpMessage != ''){alert(s.cpMessage);}}" RowClick="row_click" />
			<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" AutoExpandAllGroups="True" />
			<SettingsPager PageSize="50">
			</SettingsPager>
			<Settings ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowHeaderFilterButton="True" />
			<Styles>
				<Header HorizontalAlign="Center">
				</Header>
				
				<Footer HorizontalAlign="Center">
                </Footer>
				
				<GroupFooter Font-Bold="True" HorizontalAlign="Center">
				</GroupFooter>
			</Styles>
		</dx:ASPxGridView>
		<asp:SqlDataSource ID="ds_payperiods" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT payperiodid id, CONCAT('(',payperiodid,') ',DATE_FORMAT(startdate, '%b %d,%Y'),' - ',DATE_FORMAT(enddate,'%b %d,%Y')) date_spread, payperiodid FROM payperiods WHERE payperiodid IN (SELECT DISTINCT(id_payperiod) FROM expense_reimbursement) ORDER BY payperiodid DESC"></asp:SqlDataSource>
			<dx:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="gv_expensereport">
			</dx:ASPxGridViewExporter>
			<dx:ASPxPopupControl ID="pop_detail" runat="server">
			</dx:ASPxPopupControl>
		</ContentTemplate>
		<Triggers>
			
		</Triggers>
	</asp:updatepanel>
</asp:content>
<asp:content ID="Content4" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:content>
<asp:content ID="Content5" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
		<asp:sqldatasource ID="Users" runat="server"></asp:sqldatasource>
	</div>
</asp:content>

