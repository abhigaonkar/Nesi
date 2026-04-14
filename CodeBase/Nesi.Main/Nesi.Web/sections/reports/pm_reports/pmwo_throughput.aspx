<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits		= 'pmwo_throughput' 
			Title			= 'PM Work Order Throughput' 
			EnableTheming = 'true'
 Codebehind="pmwo_throughput.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
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



<asp:Label ID="lblErrorMsg" runat="server" ForeColor="DimGray" Text="PM Throughput - Total $ managed by this PM" Height="31px" Width="522px" Font-Size="Medium"></asp:Label>
    <table>
        <tr>
            <td style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; color: white;
                border-top-style: none; padding-top: 0px; border-right-style: none; border-left-style: none;
                background-color: black; border-bottom-style: none">
    <ASP:LABEL id="Label8" runat="server" text="Starting Date"></ASP:LABEL></td>
            <td>
												
	            <dx:ASPxDateEdit runat="server" id="txtSelectedDate" messagealignment="RightCalendarControl"
	                             format="yyyy mm dd" control="txtSelectedDate" AutoPostBack="false" ></dx:ASPxDateEdit>
            </td>
        </tr>
        <tr>
            <td style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; color: white;
                border-top-style: none; padding-top: 0px; border-right-style: none; border-left-style: none;
                background-color: black; border-bottom-style: none">
    
    <asp:Label ID="LABEL1" runat="server" Text="Ending Date"></asp:Label></td>
            <td colspan="3">
                
	            <dx:ASPxDateEdit runat="server" id="txtEndDate" messagealignment="RightCalendarControl"
	                             format="yyyy mm dd" control="txtSelectedDate" AutoPostBack="false" ></dx:ASPxDateEdit>
            </td>
        </tr>
        <tr>
            <td style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; color: white;
                border-top-style: none; padding-top: 0px; border-right-style: none; border-left-style: none;
                background-color: black; border-bottom-style: none">
    <asp:Label ID="lblCompany" runat="server" Text="Select Company"></asp:Label></td>
            <td colspan="3">
    <asp:DropDownList ID="ddlCompany" runat="server" datatextfield="name" datavaluefield="id" AutoPostBack="true" Enabled="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
    </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; color: white;
                border-top-style: none; padding-top: 0px; border-right-style: none; border-left-style: none;
                background-color: black; border-bottom-style: none">
    <asp:Label ID="lblPM" runat="server" Text="Select Project Manager"></asp:Label></td>
            <td colspan="3">
    <asp:DropDownList ID="ddlPM" runat="server" DataTextField="name" DataValueField="id">
    </asp:DropDownList></td>
        </tr>
    </table>
    <br />
    <dx:ASPxGridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
		Theme="NETheme01" Width="100%">
		<Columns>
			<dx:GridViewDataTextColumn Caption="BVWO" FieldName="bvwo" VisibleIndex="0">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Customer" FieldName="customer" 
				VisibleIndex="1">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Invoice Date" FieldName="_date" 
				VisibleIndex="2">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Invoiced Amount" FieldName="invoiced_net" 
				VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="C2" CellStyle-HorizontalAlign="Right">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Invoice No" FieldName="invoice_no" 
				VisibleIndex="4">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager Mode="ShowAllRecords">
		</SettingsPager>
		<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
			ShowFooter="True" />
	</dx:ASPxGridView>
    &nbsp;
    <br />
    
    &nbsp;<br />
    <br />
    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_reload.png"
        OnClick="Button1_Click" />
    <table class='framework' border='0' cellpadding='0' cellspacing='0'>
							<tr>
								<td valign='top' align='center' id='detail' runat='server'></td>
							</tr>
	</table>
    <br />
    &nbsp;<br />
    
</asp:Content>
