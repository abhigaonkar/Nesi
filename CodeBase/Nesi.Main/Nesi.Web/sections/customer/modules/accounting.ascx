<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_customer_modules_accounting" Codebehind="accounting.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<p>
    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" Height="800px" Theme="NETheme01" Width="100%" EnableCallBacks="True">
        <TabPages>
            <dx:TabPage Text="AR Notes">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <table style="width: 100%;">
                            <tr>
                                <td nowrap="nowrap" style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 12px; vertical-align: top;">Customer AR Notes:</td>
                                <td width="100%">
                                    <dx:ASPxMemo ID="mem_ar_notes" runat="server" Height="200px" Width="100%"></dx:ASPxMemo>
                                    </td>
                                
                            </tr>
                            <tr>
                                <td nowrap="nowrap" style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 12px; vertical-align: top;">Special Invoicing Instructions:</td>
                                <td><dx:ASPxMemo ID="mem_si" runat="server" Height="71px" Width="100%"></dx:ASPxMemo></td>
                                
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="btn_save" runat="server" Text="Save" AutoPostBack="False" Theme="NETheme01" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {
	cb_note_save.PerformCallback();
}" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxCallback ID="cb_note_save" runat="server" ClientInstanceName="cb_note_save" OnCallback="cb_note_save_Callback">
                                        <ClientSideEvents CallbackComplete="function(s, e) {
	alert(e.result);
}" />
                                    </dx:ASPxCallback>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Name="Settings" Text="Settings">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <table cellpadding="2" cellspacing="0" style="font-family: 'Segoe UI', Calibri, Geneva, Verdana, sans-serif; font-size: 12px;" width="100%">
                            <tr>
                                <td class="c" colspan="2">
                                    <dx:ASPxLabel ID="lb_error" runat="server" EncodeHtml="False" ForeColor="Red">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%">Selling Price Level: </td>
                                <td class="v">
                                    <dx:ASPxComboBox ID="cb_sell_level" runat="server" Font-Names="Segoe UI" Font-Size="12px" Native="True" Width="45px" >
                                        <Items>
                                            <dx:ListEditItem Text="01" Value="01" Selected="True" />
                                            <dx:ListEditItem Text="02" Value="02" />
                                            <dx:ListEditItem Text="03" Value="03" />
                                            <dx:ListEditItem Text="04" Value="04" />
                                            <dx:ListEditItem Text="05" Value="05" />
                                            <dx:ListEditItem Text="06" Value="06" />
                                            <dx:ListEditItem Text="07" Value="07" />
                                            <dx:ListEditItem Text="08" Value="08" />
                                            <dx:ListEditItem Text="09" Value="09" />
                                            <dx:ListEditItem Text="10" Value="10" />
                                            <dx:ListEditItem Text="11" Value="11" />
                                            <dx:ListEditItem Text="12" Value="12" />
                                            <dx:ListEditItem Text="13" Value="13" />
                                            <dx:ListEditItem Text="14" Value="14" />
                                            <dx:ListEditItem Text="15" Value="15" />
                                            <dx:ListEditItem Text="16" Value="16" />
                                            <dx:ListEditItem Text="17" Value="17" />
                                            <dx:ListEditItem Text="18" Value="18" />
                                            <dx:ListEditItem Text="19" Value="19" />
                                            <dx:ListEditItem Text="20" Value="20" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%">Prompt For Tax: </td>
                                <td class="v">
                                    <dx:ASPxCheckBox ID="chk_prompt_tax" runat="server" CheckState="Unchecked">
                                    </dx:ASPxCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%">Credit Type: </td>
                                <td class="v">
                                    <dx:ASPxComboBox ID="cb_credit_type" runat="server" Font-Names="Segoe UI" Font-Size="12px" Native="True" ValueType="System.Int32">
                                        <Items>
                                            <dx:ListEditItem Text="Unlimited" Value="1" />
                                            <dx:ListEditItem Text="No Credit" Value="0" />
                                            <dx:ListEditItem Text="Limit" Value="2" />
                                            <dx:ListEditItem Text="Credit Card Only" Value="3" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%">Credit Limit: </td>
                                <td class="v">
                                    <dx:ASPxTextBox ID="tb_credit_limit" runat="server" Native="True" Text="10000" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c">Days Credit: </td>
                                <td class="v" style="height: 25px">
                                    <dx:ASPxSpinEdit ID="spin_dayscredit" runat="server" ClientInstanceName="dayscredit" Height="21px" Increment="5" MaxValue="160" Number="30" NumberType="Integer" Width="55px">
                                    </dx:ASPxSpinEdit>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%">Overall Margin: </td>
                                <td class="v">
                                    <dx:ASPxLabel ID="lb_overallmargin" runat="server">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%">Statements: </td>
                                <td class="v">
                                    <dx:ASPxComboBox ID="cb_statements" runat="server" Font-Names="Segoe UI" Font-Size="12px" Native="True" SelectedIndex="1">
                                        <Items>
                                            <dx:ListEditItem Text="Email" Value="E" />
                                            <dx:ListEditItem Selected="True" Text="Print Form" Value="F" />
                                            <dx:ListEditItem Text="Print Form and Email" Value="B" />
                                            <dx:ListEditItem Text="Not Required" Value="N" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%">Invoices: </td>
                                <td class="v">
                                    <dx:ASPxComboBox ID="cb_invoices" runat="server" Font-Names="Segoe UI" Font-Size="12px" Native="True" SelectedIndex="1">
                                        <Items>
                                            <dx:ListEditItem Text="Email" Value="E" />
                                            <dx:ListEditItem Selected="True" Text="Print Form" Value="F" />
                                            <dx:ListEditItem Text="Print Form and Email" Value="B" />
                                            <dx:ListEditItem Text="Not Required" Value="N" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%">Apply Finance Charges: </td>
                                <td class="v">
                                    <dx:ASPxCheckBox ID="chk_finance_charges" runat="server" CheckState="Unchecked">
                                    </dx:ASPxCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%" valign="top">Auto Account Statement: </td>
                                <td class="v">
                                    <dx:ASPxCheckBox ID="chk_auto_statement" runat="server" Checked="True" CheckState="Checked" ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                                    </dx:ASPxCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%" valign="top">Customer requires copy of WO w/ Invoice:</td>
                                <td class="v">
                                    <dx:ASPxCheckBox ID="chk_req_wo" runat="server" CheckState="Unchecked" ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                                    </dx:ASPxCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%" valign="top">Statement Email Address: </td>
                                <td class="v">
                                    <dx:ASPxTextBox ID="tb_statement_email" runat="server" Width="170px">
                                        <ClientSideEvents TextChanged="function(s, e) {
					if (s.GetText()!='')
						{
						NE_Customer.validate_email(s, s.GetText());
						}
					}" />
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%" valign="top">Statement Email CC Address: </td>
                                <td class="v">
                                    <dx:ASPxTextBox ID="tb_cc_statement_email" runat="server" Width="170px">
                                        <ClientSideEvents TextChanged="function(s, e) {
					if (s.GetText()!='')
						{
						NE_Customer.validate_email(s, s.GetText());
						}
					}" />
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%" valign="top">Invoice Email Address: </td>
                                <td class="v">
                                    <dx:ASPxTextBox ID="tb_invoice_email" runat="server" ClientInstanceName="txtInvoiceEmail" Width="170px">
                                        <ClientSideEvents TextChanged="function(s, e) {
					if (s.GetText()!='')
						{
						NE_Customer.validate_email(s, s.GetText());
						}
					}" />
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%" valign="top">Invoice Email CC Address: </td>
                                <td class="v">
                                    <dx:ASPxTextBox ID="tb_invoice_cc_email" runat="server" Width="170px">
                                        <ClientSideEvents TextChanged="function(s, e) {
					if (s.GetText()!='')
						{
						NE_Customer.validate_email(s, s.GetText());
						}	
					}" />
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%" valign="top">Default Invoice Type: </td>
                                <td class="v">
                                    <asp:SqlDataSource ID="Sql_default_invoice_types" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select * from customer_default_invoice_types"></asp:SqlDataSource>
                                    <dx:ASPxComboBox ID="cb_default_invoicetype" runat="server" DataSourceID="Sql_default_invoice_types" Font-Names="Segoe UI" Font-Size="12px" Native="True" TextField="invoice_type" ValueField="id" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%" valign="top">Auto Invoicing (WO&#39;s Under $2,000): </td>
                                <td class="v">
                                    <dx:ASPxCheckBox ID="chk_auto_invoicing" runat="server" Checked="True" CheckState="Checked" ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                                        <ClientSideEvents CheckedChanged="function(s, e) {
					if (!NE_Customer.validate_email(txtInvoiceEmail, txtInvoiceEmail.GetText()))
						{
						s.SetValue(0);
						}
					}" />
                                    </dx:ASPxCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%" valign="top">&nbsp;</td>
                                <td class="v">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="c" style="width: 20%" valign="top">
                                    <dx:ASPxButton ID="bt_save" runat="server" OnClick="bt_save_Click" Text="Save">
                                    </dx:ASPxButton>
                                </td>
                                <td class="v">&nbsp;</td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Rates">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <iframe id="rates_iframe" runat="server" class="rates_if" frameborder="0" height="100%" name="I1" width="100%"></iframe>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>
</p>
<p>
    &nbsp;</p>
&nbsp;<p>
	<table style="width:100%;">
		<tr>
			<td>
                &nbsp;</td>
			<td valign="top" width="70%">
				&nbsp;</td>
		</tr>
	</table>
	<br />
</p>


<asp:HiddenField ID="hdn_address_id" runat="server" />
<asp:HiddenField ID="hdn_customer_id" runat="server" />