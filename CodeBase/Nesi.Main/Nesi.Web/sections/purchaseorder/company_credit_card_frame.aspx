<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_purchaseorder_company_credit_card_frame" Title="Company Credit Card Expense" EnableTheming="True" Codebehind="company_credit_card_frame.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register src="../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>




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
	    function purchased_from_handler(s, e) {
	        var this_text = s.GetText();
	        this_text = this_text.replace(" ", "");
	        if (this_text.match(/perdi[e|o|u]m/gi)) {
	            s.SetSelectedIndex(-1);
	            s.SetText("");
	            alert("You must use the 'Per Diem' tab above to request per diem expenses");
	            pc_main.SetActiveTabIndex(1);
	        }
	    }
	    function wo_change_handler(s, e) {
	        if (s.GetSelectedItem() == null) {
	            s.SetText("");
	            s.SetValue(null);
	        }
	    }
	    function shop_check_handler(s, e) {
	        c_workorder.SetText("");
	        c_workorder.SetEnabled(!s.GetChecked());
	    }
	    function handle_link(id, ext) { boing('/files/credit_card_receipts/' + id + '.' + ext, 'CC Purchase', 800, 600); }

	    function get_expense(s, e) {

			if (e.buttonID == "Edit") {
				var id = gv.GetRowValues(e.visibleIndex, "id", exp_callback);
			}

	        
		}

		function exp_callback(id) {
			boing("/sections/purchaseorder/company_credit_card.aspx?id=" + id + "&type=1", "CCPurchase", 800, 600);
		}
	  
	    function get_attachment(e, id, ext) {
	        e = e || window.event;
	        e.stopPropagation();
	        /* boing('/files/credit_card_receipts/' + id + '.' + ext, 'CC Purchase', 800, 600); */
	         boing(ext, 'CC Purchase', 800, 600); 
	    }

	    $(document).ready(function () {
	        please_wait('stop');
	    });
	    var btn_clicked = false;
	</script>
<iframe src="company_credit_card.aspx" width="700px" frameborder="0" height="650px" 
		id="I1" name="I1"></iframe>
	
	<br />
	<br />
	<lc:LayoutControl ID="layout" runat="server" />
	
	<dx:ASPxGridView ID="gv" runat="server" Theme="NETheme01" Width="100%" 
		AutoGenerateColumns="False" DataSourceID="SqlDataSource1" 
		KeyFieldName="id" ClientInstanceName="gv" 
		oncustomcallback="gv_CustomCallback" 
		oncustomjsproperties="gv_CustomJSProperties" OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared" OnCustomButtonInitialize="gv_CustomButtonInitialize">
		<Columns>
			<dx1:GridViewCommandColumn Caption="Command" ShowClearFilterButton="True" 
				VisibleIndex="0" ButtonType="Link" SelectAllCheckboxMode="Page" ShowSelectCheckbox="True" MinWidth="150">
				
			    <CustomButtons>
					<dx1:GridViewCommandColumnCustomButton ID="Edit" Text="Edit">
                    </dx1:GridViewCommandColumnCustomButton>
                    <dx1:GridViewCommandColumnCustomButton ID="Approve" Text="Approve">
                    </dx1:GridViewCommandColumnCustomButton>
                    <dx1:GridViewCommandColumnCustomButton ID="Deny" Text="Deny">
                    </dx1:GridViewCommandColumnCustomButton>
                    <dx1:GridViewCommandColumnCustomButton ID="Paid" Text="Paid">
                    </dx1:GridViewCommandColumnCustomButton>
                </CustomButtons>
                <CellStyle Wrap="False">
                </CellStyle>
                <FooterCellStyle Wrap="False">
                </FooterCellStyle>
                <FooterTemplate>
                    <dx1:ASPxButton ID="btn_all_approve" runat="server" Text="Approve">
                    </dx1:ASPxButton>
                    <dx1:ASPxButton ID="btn_all_deny" runat="server" Text="Deny">
                    </dx1:ASPxButton>
                     <dx1:ASPxButton ID="btn_all_paid" runat="server" Text="Paid">
                    </dx1:ASPxButton>
                </FooterTemplate>
			</dx1:GridViewCommandColumn>
			<dx1:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" 
				VisibleIndex="1">
				<EditFormSettings Visible="False" />
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="Employee" FieldName="member_fullname" 
				VisibleIndex="2">
				<CellStyle Wrap="True">
				</CellStyle>
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="Receipt" FieldName="receipt_number" 
				VisibleIndex="4">
				<CellStyle Wrap="True">
				</CellStyle>
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataDateColumn Caption="Date" FieldName="date_purchased" 
				VisibleIndex="5">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
			</dx1:GridViewDataDateColumn>
			<dx1:GridViewDataTextColumn Caption="Work Order" FieldName="woprog_bvwo" VisibleIndex="6">
			    <DataItemTemplate>
			        <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Theme="NETheme01"
			                          NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/workorder/index.aspx?woprog_id={0}', 'workorder', 1200,800)&quot;, Eval(&quot;WOProg_ID&quot;)) %>" 
			                          Text='<%# Eval("WOProg_BVWO") %>'>
			        </dx:ASPxHyperLink>
			    </DataItemTemplate>
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="Customer" FieldName="woprog_customername" 
				VisibleIndex="7">
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="WO Description" 
				FieldName="woprog_description" VisibleIndex="8">
               
				<CellStyle Wrap="True">
				</CellStyle>
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="Pre-Tax Amount(Total Excluding Taxes)" FieldName="amount" 
				VisibleIndex="9">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="Total Due(Total Including Taxes)" FieldName="total" 
				VisibleIndex="10">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="Notes" FieldName="item_text" 
				VisibleIndex="111">
				<CellStyle Wrap="True">
				</CellStyle>
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="Vendor" FieldName="seller" 
				VisibleIndex="3">
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="Expense Category ID" FieldName="master_id" 
				VisibleIndex="12">
			</dx1:GridViewDataTextColumn>
            	
		    <dx1:GridViewDataTextColumn Caption="Employee Business Unit" FieldName="emp_branch" VisibleIndex="13">
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="Card Business Unit" FieldName="card_branch" VisibleIndex="14">
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="Approved By" FieldName="approved_by_name" VisibleIndex="15">
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="Approved Date" FieldName="approved_date" VisibleIndex="16">
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="Paid Date" FieldName="paid_date" VisibleIndex="17">
            </dx1:GridViewDataTextColumn>
            	
		    <dx1:GridViewDataTextColumn Caption="Card Used" FieldName="card_used" VisibleIndex="18">
            </dx1:GridViewDataTextColumn>
            	
		    <dx1:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="19">
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="Member_id" FieldName="member_id" Visible="False" VisibleIndex="20">
            </dx1:GridViewDataTextColumn>
		    <dx1:GridViewDataTextColumn Caption="woprog_id" FieldName="WOProg_ID" Visible="False" VisibleIndex="21">
		    </dx1:GridViewDataTextColumn>
           
            	
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<SettingsPager PageSize="25">
		</SettingsPager>
		<ClientSideEvents CustomButtonClick="get_expense"/>
		<Settings ShowFilterRow="True" ColumnMinWidth="30" ShowFooter="True" />
	</dx:ASPxGridView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL ds_creditcardpurchase(?mid)">
		<SelectParameters>
			<asp:Parameter Name="mid"  />
		</SelectParameters>
	</asp:SqlDataSource>
 </ASP:CONTENT>

