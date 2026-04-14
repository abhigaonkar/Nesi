<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits		= 'sales_commision' 
			Title			= 'Sales Commision'
            EnableTheming="true"
            Theme="NETheme01"
 Codebehind="sales_commision.aspx.cs" %>

<%@ REGISTER assembly="RJS.Web.WebControl.PopCalendar" namespace="RJS.Web.WebControl"
	tagprefix="rjs" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				<div id='divSide' runat='server'>
					<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
				</div>
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
				<div id='divMenu' runat='server'></div>
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server'>

<script type="text/javascript">

document.getElementsByClassName = function(cl) {
var retnode = [];
var myclass = new RegExp('\\b'+cl+'\\b');
var elem = this.getElementsByTagName('*');
for (var i = 0; i < elem.length; i++) {
var classes = elem[i].className;
if (myclass.test(classes)) retnode.push(elem[i]);
}
return retnode;
}; 


function ToggleRow(id) {

var e = document.getElementsByClassName(id);
for(var i = 0, len = e.length; i<len; i++){ 

 if( e[i].style.display=='none' ){
   e[i].style.display = '';
 }else{
   e[i].style.display = 'none';
 }
}
}
</script>



<asp:Label ID="lblErrorMsg" runat="server" ForeColor="DimGray" Text="Sales by This Employee" Height="31px" Width="522px" Font-Size="Medium"></asp:Label>
    <table>
        <tr>
            <td style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; color: white;
                border-top-style: none; padding-top: 0px; border-right-style: none; border-left-style: none;
                background-color: black; border-bottom-style: none">
    <ASP:LABEL id="Label8" runat="server" text="Starting Date"></ASP:LABEL></td>
            <td>
												
														<ASP:TEXTBOX id="txtSelectedDate" runat="server" autopostback="false"  ReadOnly="True"></ASP:TEXTBOX>
                <RJS:PopCalendar
														id="txtCalendarSearch" runat="server" messagealignment="RightCalendarControl"
															 format="yyyy mm dd" control="txtSelectedDate" AutoPostBack="false"  >
													</RJS:PopCalendar>
            </td>
        </tr>
        <tr>
            <td style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; color: white;
                border-top-style: none; padding-top: 0px; border-right-style: none; border-left-style: none;
                background-color: black; border-bottom-style: none">
    
    <asp:Label ID="LABEL1" runat="server" Text="Ending Date"></asp:Label></td>
            <td colspan="3">
                <asp:TextBox
        ID="txtEndDate" runat="server" AutoPostBack="false" ReadOnly="True"></asp:TextBox><rjs:PopCalendar
            ID="POPCALENDAR1" runat="server" Control="txtEndDate"
            Format="yyyy mm dd" MessageAlignment="RightCalendarControl" AutoPostBack="false"  />
            </td>
        </tr>
        <tr>
            <td style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; color: white;
                border-top-style: none; padding-top: 0px; border-right-style: none; border-left-style: none;
                background-color: black; border-bottom-style: none">
    <asp:Label ID="lblCompany" runat="server" Text="Select Business Unit"></asp:Label></td>
            <td colspan="3">
    <asp:DropDownList ID="ddlCompany" runat="server" datatextfield="name" datavaluefield="id" AutoPostBack="true" Enabled="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
    </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; color: white;
                border-top-style: none; padding-top: 0px; border-right-style: none; border-left-style: none;
                background-color: black; border-bottom-style: none">
    <asp:Label ID="lblPM" runat="server" Text="Select Employee"></asp:Label></td>
            <td colspan="3">
    <asp:DropDownList ID="ddlPM" runat="server" DataTextField="Username" DataValueField="MemberID" OnSelectedIndexChanged="ddlPM_SelectedIndexChanged">
    </asp:DropDownList></td>
        </tr>
    </table>
    <br />
    &nbsp;
    <br />
    <asp:GridView ID="GridView1" runat="server" autogeneratecolumns="False">
        <COLUMNS>
           <ASP:BOUNDFIELD datafield="WONum" headertext="WorkOrder Number" sortexpression="WONum">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="RoyalBlue" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="CustomerName" headertext="Customer Name" sortexpression="CustomerName">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="RoyalBlue" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="Status" headertext="Status of Work Order" sortexpression="Status">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="RoyalBlue" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="InvoiceNo" headertext="Invoice No" sortexpression="InvoiceNo">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="RoyalBlue" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="DateInvoiced" headertext="Date Invoiced" sortexpression="YTDSales">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="RoyalBlue" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="InvoicedAmount" headertext="Amount Invoiced" sortexpression="InvoicedAmount">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="RoyalBlue" />
	       </ASP:BOUNDFIELD>
            <asp:BoundField DataField="AmountPaid" HeaderText="Amount Paid" SortExpression="AmountPaid">
                <HeaderStyle ForeColor="RoyalBlue" HorizontalAlign="Center" />
            </asp:BoundField>
	       
	       
	   </COLUMNS>
        <HeaderStyle ForeColor="Navy" />
        <AlternatingRowStyle BackColor="#E0E0E0" />
    </asp:GridView>
    &nbsp;&nbsp;<br />
    <br />
    <asp:Button ID="btnReport" runat="server" Text="Get Report" OnClick="btnReport_Click" />
    <table class='framework' border='0' cellpadding='0' cellspacing='0'>
							<tr>
								<td valign='top' align='center' id='detail' runat='server'></td>
							</tr>
	</table>
    <br />
    &nbsp;<br />
    
</asp:Content>
