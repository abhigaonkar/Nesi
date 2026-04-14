<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="wo_prog" Title="Work Orders" EnableTheming="True" Codebehind="wo_prog.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register src="modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>
<ASP:CONTENT ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</ASP:CONTENT>
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <script type="text/javascript">

        function bc(master_id)
        {
            
            createPopupWin("/_tools/print_barcode/index.aspx?master_id=" + master_id, "Print WO", 600, 200);
        
		//popbcprint.SetHeaderText("Print Labels for: " + master_id);
		//hidbcmastertid.Set("id",master_id);
		//popbcprint.Show();

        }
        function createPopupWin(pageURL, pageTitle, 
                    popupWinWidth, popupWinHeight) { 
            var left = (screen.width-popupWinWidth)/2; 
            var top = (screen.height-popupWinHeight)/4;
            var myWindow = window.open(pageURL, pageTitle,  
                    'resizable=yes, width=' + popupWinWidth 
                    + ', height=' + popupWinHeight + ', top=' 
                    + top + ', left=' + left); 
        } 

		function bind_tooltips()
			{
			$(".ttip").each(function()
				{
				$(this).tip();
				});
			}
		function change_active_branch(c_id)
			{
			please_wait("start", "Setting active branch");
			$.get("./wo_prog.aspx", 
				{
				a:			"set_active_branch",
				business_unit_id:	c_id
				},
				function(resp)
					{
					please_wait("stop");
					switch(resp)
						{
						case "SUCCESS":
							gv_wosummary.Refresh();
						break;
						default:
							alert(resp);
						break;
						}
					});
			}
		$(document).ready(function()
			{
			bind_tooltips();
			});
		var wo_summary		= {
			print_barcode:
				{
				run:
					function(woprog_id)
						{
						PageMethods.print_barcode(woprog_id, this.complete, wo_summary.error, wo_summary.timeout)
						},
				complete:
					function(arg)
						{
						alert("Barcode for WO#"+arg+" printed");
						}
				},
			timeout: 
				function(arg)
					{
					alert("The server did not reply within a normal amount of time");
					},
			error: 
				function(arg)
					{
					alert("There was a problem contacting the server");
					}
			};
</script>
<br />
    <dx:ASPxComboBox ID="ddlcompany" runat="server" ValueType="System.Int32" AutoPostBack="True" OnSelectedIndexChanged="ddlcompany_SelectedIndexChanged" TextField="name" Theme="NETheme01" ValueField="id"></dx:ASPxComboBox>
    <br />
<div id="divCoSummary" runat="server">
    </div>
<br />
<dx:ASPxCallbackPanel ID="cbp_main" ClientInstanceName="cbp_main" runat="server" Width="100%">
		<PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server">
				<table width="100%">
					<tr>
						<td style="width: 150px">
				<dx:ASPxComboBox ID="cb_branches" runat="server" DataSourceID="ods_companies" TextField="ddl_name"
					ValueField="id" ValueType="System.String" ClientInstanceName="cb_branches" 
								OnDataBound="cb_branches_DataBound" Font-Bold="False" EnableTheming="True" 
								Theme="NETheme01">
					<ClientSideEvents SelectedIndexChanged="function(s, e) {
	change_active_branch(s.GetValue());
}"  />
				</dx:ASPxComboBox>
						</td>
						<td>
							&nbsp;<dx:ASPxLabel ID="lblcb_branches" runat="server" 
								Text="Invoiced &amp; Closed Workorders are displayed in the grid below." Theme="NETheme01" ClientVisible="False">
							</dx:ASPxLabel>
						</td>
					</tr>
				</table>
				<asp:ObjectDataSource ID="ods_companies" runat="server" SelectMethod="LoadMy_business_units" 
					TypeName="nesi.core.NeBusinessUnit" >
                   
                   
                   
                </asp:ObjectDataSource>
				<dx:ASPxCallback id="cb_company" runat="server" ClientInstanceName="cb_company" OnCallback="cb_company_Callback">
					<clientsideevents endcallback="function(s, e) {
	gv_wosummary.Refresh();
}"></clientsideevents>
				</dx:ASPxCallback>
				
				<asp:ScriptManager ID="sm" runat="server" EnablePageMethods="True">
				</asp:ScriptManager>
				
				<lc:LayoutControl ID="layout" runat="server" />
			
	<dx:ASPxGridView runat="server" ClientInstanceName="gv_wosummary" KeyFieldName="woprog_id" 
					AutoGenerateColumns="False" Width="100%"   ID="gv_wosummary" 
					OnCustomButtonCallback="gv_wosummary_CustomButtonCallback" 
					OnCustomCallback="gv_wosummary_CustomCallback"  
					OnCustomJSProperties="gv_wosummary_CustomJSProperties" 
					OnClientLayout="gv_wosummary_ClientLayout" 
					OnHtmlRowPrepared="gv_wosummary_HtmlRowPrepared" EnableTheming="True" Theme="NETheme01" SettingsBehavior-EnableCustomizationWindow="True"><Columns>
		<dx:GridViewDataTextColumn Caption=" " VisibleIndex="0">
			<DataItemTemplate>
				<asp:Image ID="btn_print" runat="server" ImageUrl="~/images/icon/icon[print_barcode].GIF" style="cursor:pointer" onclick='bc(<%#Eval("woprog_id") %>)' />
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="WO No" FieldName="woprog_bvwo"
			VisibleIndex="1" Width="100px">
			<Settings AutoFilterCondition="Contains" />
			<DataItemTemplate>
					<asp:HyperLink ID="hl_wo" runat="server" NavigateUrl='<%# "redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}", Eval("woprog_id"), Eval("business_unit_id"))) %>'
						Text='<%# Eval("WOProg_BVWO") %>' Target="_blank"></asp:HyperLink>
			</DataItemTemplate>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Customer Name" FieldName="woprog_customername" VisibleIndex="2"
			Width="100px">
			<EditCellStyle Wrap="False">
			</EditCellStyle>
			<DataItemTemplate>
				<asp:HyperLink ID="hl_customer" runat="server" NavigateUrl='<%# "redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/sections/customer/frame.aspx?customer_id={0}", Eval("customer_id"))) %>' Target="_blank" Text='<%# Eval("WOProg_CustomerName") %>' Width="100%"></asp:HyperLink>
			</DataItemTemplate>
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		
		<dx:GridViewDataTextColumn Caption="Description" FieldName="woprog_description" 
				VisibleIndex="5" Width="100%" MinWidth="40">
			<PropertiesTextEdit EncodeHtml="False">
			</PropertiesTextEdit>
			<Settings AutoFilterCondition="Contains" />
			<DataItemTemplate>
				<dx:ASPxLabel ID="text_container" runat="server" Theme="NETheme01"  OnDataBound="text_container_Init" Text='<%# Eval("woprog_description") %>' Width="100%">
				</dx:ASPxLabel>
			</DataItemTemplate>
			<CellStyle HorizontalAlign="Left" Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
				ShowInCustomizationForm="True" VisibleIndex="6" Width="75px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
		<dx:GridViewDataDateColumn Caption="Last Modified" FieldName="last_modified" VisibleIndex="7"
			Width="75px">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd HH:mm">
				</PropertiesDateEdit>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataTextColumn Caption="Last Modified By" FieldName="cutby" VisibleIndex="8"
			Width="75px">
			<EditCellStyle Wrap="False">
			</EditCellStyle>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Still to be Billed" FieldName="woprog_stilltobebilled"
			VisibleIndex="10" Width="70px">
			<PropertiesTextEdit DisplayFormatString="{0:C2}">
			</PropertiesTextEdit>
			<Settings FilterMode="DisplayText" SortMode="DisplayText" />
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Quote" FieldName="quote_id" VisibleIndex="11"
			Width="75px">
			<Settings AllowHeaderFilter="True" AllowSort="True" />
			<DataItemTemplate>
					<asp:HyperLink ID="hl_quote" runat="server" Font-Bold="True" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/member/quote/index.aspx?a=g&quote_id={0}&revision={1}','quote',1035,800);&quot;, Eval(&quot;quote_id&quot;), Eval(&quot;revision&quot;)) %>"
						OnInit="hl_quote_Init" Text='<%# Eval("quote_id") %>'></asp:HyperLink>
			</DataItemTemplate>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Account Mgr" FieldName="acct_manager" VisibleIndex="12"
			Width="75px">
			<EditCellStyle Wrap="False">
			</EditCellStyle>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="PM" FieldName="pm" VisibleIndex="13" 
				Width="75px">
			<EditCellStyle Wrap="False">
			</EditCellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Invoice #" FieldName="woprog_invoiceno" VisibleIndex="4"
			Width="75px">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Cust PO" FieldName="woprog_custpo" VisibleIndex="9"
			Width="50px">
		</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Cut Date" FieldName="woprog_cutdatetime" 
				ShowInCustomizationForm="True" VisibleIndex="14" Width="75px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn FieldName="woprog_vis_to_cust" 
				ShowInCustomizationForm="True" Visible="False" VisibleIndex="16" Width="50px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Contact" FieldName="Contact_Name" 
				ShowInCustomizationForm="True" VisibleIndex="15" Width="50px">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Next Date" FieldName="_date" 
				ShowInCustomizationForm="True" VisibleIndex="17" Width="60px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn FieldName="woprog_id" ShowInCustomizationForm="True" 
				Visible="False" VisibleIndex="18">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="On Hold" FieldName="hold" MinWidth="50" 
				ShowInCustomizationForm="True" VisibleIndex="19" Width="50px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Margin" FieldName="margin" 
				ShowInCustomizationForm="True" VisibleIndex="20" Width="50px">
				<PropertiesTextEdit DisplayFormatString="p2">
				</PropertiesTextEdit>
				<Settings FilterMode="DisplayText" SortMode="DisplayText" />
			</dx:GridViewDataTextColumn>
</Columns>

<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" 
			ShowFilterBar="Visible" ShowFooter="True" ShowGroupPanel="True" 
			ShowTitlePanel="True"></Settings>


<Styles>




<TitlePanel HorizontalAlign="Left" ></TitlePanel>
</Styles>
		<Templates>
			<TitlePanel>
				<table style="width: 100%;  color: #000000;">
					<tr>
						<td nowrap="nowrap" style="background-color: #CCFFCC; font-size: 15px; font-family: Calibri; font-weight: normal;">
							Scheduled in the Future</td>
						<td width="100%">
							&nbsp;</td>
					</tr>
				</table>
			</TitlePanel>
		</Templates>


<Border BorderWidth="0px"></Border>
		<SettingsBehavior ColumnResizeMode="Control" AutoFilterRowInputDelay="6000" />
		<ClientSideEvents EndCallback="function(s, e) {
	bind_tooltips();
            please_wait('stop');
}" />
        <GroupSummary>
            <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="woprog_stilltobebilled" ShowInColumn="Still to be Billed"
                SummaryType="Sum" />
        </GroupSummary>
        <SettingsPager PageSize="20">
        </SettingsPager>
</dx:ASPxGridView>
 <dx:ASPxHiddenField runat="server" ClientInstanceName="hh" ID="hh"></dx:ASPxHiddenField>
</dx:PanelContent>
</PanelCollection>
	</dx:ASPxCallbackPanel>
    
</ASP:CONTENT>
