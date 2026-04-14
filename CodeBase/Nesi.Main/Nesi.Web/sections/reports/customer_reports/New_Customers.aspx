<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits		= 'new_customers' 
			Title			= 'New Customer Report' 
 Codebehind="New_Customers.aspx.cs" %>
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



<asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Text="New Customer Report - Beta Version For Test" Height="31px" Width="231px"></asp:Label>
    <br />
    <asp:Label ID="lblCompany" runat="server" Text="Select Business Unit "></asp:Label>
    <asp:DropDownList ID="ddlCompany" runat="server" datatextfield="name" datavaluefield="id" AutoPostBack="true" Enabled="False" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
    </asp:DropDownList>
    <asp:CheckBox ID="chkARSel" runat="server" Text="Include AR VAlues (Slow)" visible="false"/><br />
    <ASP:LABEL id="Label8" runat="server" text="Select Month "></ASP:LABEL>
												
														
    </asp:Label><asp:DropDownList
        ID="ddlMonth" runat="server">
        <asp:ListItem Value="01" Text="January" ></asp:ListItem>
        <asp:ListItem Value="02" Text="February" ></asp:ListItem>
        <asp:ListItem Value="03" Text="March" ></asp:ListItem>
        <asp:ListItem Value="04" Text="April" ></asp:ListItem>
        <asp:ListItem Value="05" Text="May" ></asp:ListItem>
        <asp:ListItem Value="06" Text="June" ></asp:ListItem>
        <asp:ListItem Value="07" Text="July" ></asp:ListItem>
        <asp:ListItem Value="08" Text="August" ></asp:ListItem>
        <asp:ListItem Value="09" Text="September" ></asp:ListItem>
        <asp:ListItem Value="10" Text="October" ></asp:ListItem>
        <asp:ListItem Value="11" Text="November" ></asp:ListItem>
        <asp:ListItem Value="12" Text="December" ></asp:ListItem>
    </asp:DropDownList><asp:CheckBox ID="chkMnth" runat="server" Text="Previous Month Information" /><br />
    <br />
    <asp:GridView ID="GridView1" runat="server" autogeneratecolumns="False"  AllowSorting="false" AllowPaging="false">
        <COLUMNS>
           <ASP:BOUNDFIELD datafield="CustomerName" headertext="Customer Name" sortexpression="CustomerName">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="DarkRed" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="MTDSales" headertext="MTD Sales" sortexpression="MTDSales">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="DarkRed" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="YTDSales" headertext="YTD Sales" sortexpression="YTDSales">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="DarkRed" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="FirstOpen" headertext="Date of First WO Opened" sortexpression="FirstOpen">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="DarkRed" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="TotalOpen" headertext="Total Open WO" sortexpression="TotalOpen">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="DarkRed" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="TotalOpenDol" headertext="Total Open WO $" sortexpression="TotalOpenDol">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="DarkRed" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="ARBal" headertext="A/R Balance" sortexpression="ARBal">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="DarkRed" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="is_qc1" headertext="QC 1" sortexpression="is_qc1">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="DarkRed" />
	       </ASP:BOUNDFIELD>
	       <ASP:BOUNDFIELD datafield="is_qc2" headertext="QC 2" sortexpression="is_qc2">
	       <HEADERSTYLE horizontalalign="Center" font-underline="True" forecolor="DarkRed" />
	       </ASP:BOUNDFIELD>
	   </COLUMNS>
    </asp:GridView>
    <asp:Label ID="TotCust" runat="server" Text="Total new Customers" Visible="False"></asp:Label>
    <asp:Label ID="lblCustNum" runat="server" Visible="False"></asp:Label><br />
    <asp:Label ID="totSale" runat="server" Text="Total Sales" Visible="False"></asp:Label>
    <asp:Label ID="lblTotSale" runat="server" Visible="False"></asp:Label><br />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Print Report" style="border-top-style: dotted; border-right-style: dotted; border-left-style: dotted; border-bottom-style: dotted" />
    <table class='framework' border='0' cellpadding='0' cellspacing='0'>
							<tr>
								<td valign='top' align='center' id='detail' runat='server'></td>
							</tr>
	</table>
    <br />
    &nbsp;<br />
    
</asp:Content>
