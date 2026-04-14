<%@ Page Title="" Language="C#"  MasterPageFile="~/mobile.master" AutoEventWireup="true" EnableTheming = "True"  Theme="mobile"  Inherits="sections_workorder_mobile_wo_index" Codebehind="index.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_body" Runat="Server">
    <div id='customer_page' runat="server">
	<dx:aspxbutton id="btn_cancel" runat="server" text="Cancel" autopostback="false"  native="true" cssclass="whitetext aligncenter" UseSubmitBehavior="False">
		<clientsideevents click="function(s,e){if(confirm('Are you sure you want to leave this page?')){if(window != null && window.opener != null && window.opener.mobile_wo != null ){window.opener.mobile_wo.close_popup();}else{location.href = '/mobile/index.aspx?a=workorder';}}}"/>
	</dx:aspxbutton>
	<div class='wo_details'>
        <div class='general' id="div_general" runat="server">
            <dx:ASPxCallbackPanel ID="cb_wo_header" runat="server" OnCallback="ASPxCallbackPanel1_Callback" ClientInstanceName="cb_wo_header">
               
                <PanelCollection>
<dx:PanelContent runat="server">
    <input type="hidden" ID="defaultTM" class="" runat="server" />
    <input type="hidden" ID="defaultQuoted" class="" runat="server" />

    <div> <dx:ASPxLabel ID="lblError" ClientInstanceName="lblError" runat="server" ForeColor="Red"></dx:ASPxLabel></div>
	<div id="tbl_header_info" runat="server">
		<div class="field">
			<div class="name">Order Number:</div>
			<div class="value text" ID="lbl_bvwo" runat="server"></div>
		</div>
		<div class="field">
			<div class="name">Status:</div>
			<div class="value text" ID="lbl_status" runat="server"></div>
		</div>
		<div class="field" id="tr_total" runat="server">
			<div class="name">Total $:</div>
			<div class="value text" ID="lbl_total" runat="server"></div>
		</div>
		<div class="field" id="tr_margin" runat="server">
			<div class="name">Margin:</div>
			<div class="value text" ID="lbl_margin" runat="server"></div>
		</div>
		<div class="field">
			<div class="name">Location:</div>
			<div class="value text"><asp:Label ID="lbl_address" runat="server"></asp:Label></div>
		</div>
	</div>
		<div class="field">
			<div class="name">Business Unit:</div>
			<div class="value clearbg"><asp:DropDownList ID="BusinessUnitDropDownList" runat="server" AutoPostBack="True"  DataTextField="ddl_name" DataValueField="id"
                 OnSelectedIndexChanged="ddldept_SelectedIndexChanged"  Enabled="False">
                </asp:DropDownList>
			</div>
		</div>
		<div class="field">
			<div class="name">Description:</div>
			<div class="value">
				 <asp:TextBox ID="text_desc" runat="server" TextMode="MultiLine" style="padding-left:4px;"></asp:TextBox>
			</div>
		</div>
		<div class="field">
			<div class="name">Project Manager:</div>
			<div class="value clearbg">
				<asp:DropDownList ID="ddlpm" runat="server" DataSourceID="Sqlpm" DataTextField="Name" ondatabound="ddlpm_DataBound" DataValueField="Member_id">
                </asp:DropDownList>
                <asp:SqlDataSource ID="Sqlpm" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
                    SelectCommand="Select Member_id,Name from vw_activepms where business_unit_id=@business_unit_id ">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdn_company" Name="@business_unit_id" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
			</div>
		</div>
		<div class="field">
			<div class="name">PO Number:</div>
			<div class="value"><asp:TextBox ID="txt_po" runat="server" size="5" style="padding-left:4px;"></asp:TextBox></div>
		</div>
    <script type="text/javascript">
        var defaultTM =  <%= defaultTM.Value %>;
        var defaultQuoted =  <%= defaultQuoted.Value %>;
    </script>
		<div class="field">
			<div class="name">Quote (Optional):</div>
			<div class="value clearbg"><dx:ASPxComboBox ID="ddlquote" runat="server" DataSourceID="sql_quotes" TextField="Quote_Stuff" ValueField="Quote_n" ValueType="System.Int32" ClientIDMode="Static" Font-Size="16px">
			        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	if(s.GetValue() != 0) 
		{
ddlRevenueLines.SetValue(defaultQuoted); 
                    } else {
ddlRevenueLines.SetValue(defaultTM);

        }
}" />
                </dx:ASPxComboBox>
                <asp:SqlDataSource ID="sql_quotes" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select 0 quote_n, ' Select Quote (Optional)' Quote_Stuff union (Select quote_n,Quote_Stuff from vw_openquotes where customer_id = @cust_id and business_unit_id = @business_unit_id  AND status_id = 4) order by Quote_Stuff">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlcustomername" Name="@cust_id" PropertyName="Value" />
                        <asp:ControlParameter ControlID="hdn_company" Name="@business_unit_id" PropertyName="Value" />
                      
                    </SelectParameters>
                </asp:SqlDataSource>
			</div>
		</div>
    
    <div class="field">
        <div class="name">Revenue Line (Classification):</div>
        <div class="value clearbg">
            <dx:ASPxComboBox ID="ddlRevenueLines" runat="server"  ClientEnabled="false" ReadOnly="true" ClientIDMode="Static"
                             AnimationType="None"  ClientInstanceName="ddlRevenueLines"
                             ForeColor="Black" TextField="name" ValueField="ID" EnableCallbackMode="True"
                             ValueType="System.Int32" Width="100%" Font-Size="16px">
            </dx:ASPxComboBox>
        </div>
    </div>

		<div>
		<div class="field">
			<div class="name">Customer Name:</div>
			<div class="value">
				<dx:ASPxComboBox ID="ddlcustomername" runat="server" ValueType="System.Int32" cssclass="font16" ClientInstanceName="ddlcustomername"  ValueField="customer_id" TextField="customer_name" OnSelectedIndexChanged="ddlcustomername_SelectedIndexChanged" AutoPostBack="True" AnimationType="None" CallbackPageSize="14" EnableCallbackMode="True" IncrementalFilteringDelay="1700"></dx:ASPxComboBox>
                <asp:SqlDataSource ID="sql_customer" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
	0 customer_id, ' Select Customer' customer_name 
UNION 
	(
	SELECT 
		customer_id,
		customer_name 
	FROM 
		vw_activecustomers 
	WHERE 
		customer_status NOT IN (4,5,6) AND 
		IF(@woprog_id = 0, 
			customer_name LIKE 'Division Transfer to Other Division%' OR
			customer_id NOT IN 
				(
				SELECT 
					internal_companyno_intranet_custid 
				FROM 
					internal_companyno WHERE internal_companyno_id != 11 AND business_unit_id = @companyid
				), TRUE) AND 
		customer_qc_member_id IS NOT NULL 
	) 
ORDER BY customer_name
				">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdn_company" Name="@companyid" PropertyName="Value" />
                        <asp:ControlParameter ControlID="hidWOProgID" DefaultValue="0" Name="@woprog_id" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
			</div>
		</div>
		<div class="field">
			<div class="name">Location:</div>
			<div class="value clearbg">
                <asp:DropDownList ID="ddllocation" runat="server" AutoPostBack="True" DataSourceID="Sqllocation" DataTextField="address" DataValueField="address_id" OnSelectedIndexChanged="ddllocation_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:SqlDataSource ID="Sqllocation" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select address_id,address as address from vw_address where cust_id = @custid and active=1">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlcustomername" Name="@custid" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
			</div>
		</div>
		<div class="field">
			<div class="name">Contact:</div>
			<div class="value clearbg">
				<asp:DropDownList ID="ddlcontact" runat="server" DataSourceID="Sqlcontact" DataTextField="contact_name" DataValueField="contact_id">
                </asp:DropDownList>
                <asp:SqlDataSource ID="Sqlcontact" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 0 contact_id, ' Select Contact' contact_name UNION ALL (SELECT contact_id, contact_name FROM contact WHERE contact_type = 'Customer' AND contact_cust_id= @cust_id AND contact_status = 'Active') order by contact_name">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlcustomername" Name="@cust_id" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
			</div>
		</div>
		<div class="field">
			<div class="name">Customer Asset:</div>
			<div class="value clearbg"><asp:DropDownList ID="ddlca" runat="server" DataSourceID="Sqlcustomer_asset" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
                <asp:SqlDataSource ID="Sqlcustomer_asset" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="select 0 id, ' Select Asset (Optional)' Name union (Select ID, urldecode(Name) as Name from customer_asset where address_id = ?custid) order by name">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddllocation" Name="custid" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
			</div>
		</div>
		</div>
		<asp:HiddenField ID="hdn_company" runat="server" />
        <asp:HiddenField ID="hidWOProgID" runat="server" />
        <asp:HiddenField ID="hid_ts" runat="server" />
		<div class="field">
			<div class="name">Exp. Start Date:</div>
			<div class="value clearbg">
				 <asp:TextBox ID="expected_startDate" runat="server" Type="Date" style="padding-left:4px;"></asp:TextBox>
			</div>
		</div>
		<div class="field">
			<div class="name">Exp. End Date:</div>
			<div class="value clearbg">
				<asp:TextBox ID="expected_endDate" runat="server"  Type="Date" style="padding-left:4px;"></asp:TextBox>
			</div>
		</div>
		<div class="field" id="tr_sales_value" runat="server">
			<div class="name">Exp. Sales Value:</div>
			<div class="value clearbg">
				<asp:TextBox ID="txt_expect_sales" runat="server" Step="10" Type="Number" style="padding-left:4px;"></asp:TextBox>
			</div>
		</div>
		<div class="field" id="tr_hours_value" runat="server">
			<div class="name">Exp. Hrs Needed</div>
			<div class="value clearbg">
                <asp:TextBox ID="txt_expected_hrs" runat="server" Step=".5" Type="Number" style="padding-left:4px;"></asp:TextBox>
			</div>
		</div>
        <div class="field" id="tr_inspectioncb" runat="server">
			<div class="name">Inspection Rqd?</div>
			<div class="value clearbg">
                <asp:CheckBox ID="chk_inspection" runat="server" />
			</div>
		</div>
        <div class="field" id="tr_inspection" runat="server">
			<div class="name">Inspection Link</div>
			<div class="value clearbg">
                <asp:TextBox ID="tb_inspection" runat="server" style="padding-left:4px;"></asp:TextBox>
			</div>
		</div>
    <div class="field" id="tr_chk_labor_only" runat="server">
        <div class="name">Labour Only?</div>
        <div class="value clearbg">
            <asp:CheckBox ID="chk_labor_only" runat="server" />
        </div>
    </div>
        <dx:ASPxButton ID="btn_save" runat="server" AutoPostBack="False" Text="Save" native="true" cssclass="whitetext aligncenter" UseSubmitBehavior="False" ClientInstanceName="btn_save">
            <ClientSideEvents Click="function(s, e) {s.SetEnabled(false);cb_wo_header.PerformCallback();}" />
        </dx:ASPxButton>
	</div>
                    </dx:PanelContent>
</PanelCollection>
                 <ClientSideEvents EndCallback="function(s, e) {
	            if (lblError.GetText()=='')
                    {
                    alert(s.cp_alert);
                    if (s.cp_close!=null && s.cp_close=='true')
                    {
						btn_save.SetEnabled(false);
					if(window != null && window.opener != null && window.opener.cbp != null)
						{
						window.opener.cbp.PerformCallback('view|'+s.cp_newid);
						window.close();
						}
					else
						{
						location.href='/mobile/index.aspx?a=workorder&woprog_id='+s.cp_newid;
						}
                    }
					else
						{
						btn_save.SetEnabled(true);
						}
                    }
				else
					{
                    btn_save.SetEnabled(true);
					}
}" />
            </dx:ASPxCallbackPanel>
</div>
	</div>
</div>
  
    </asp:Content>