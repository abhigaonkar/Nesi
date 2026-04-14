<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_admin_vendor_index" EnableEventValidation="False" Codebehind="merge-index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript" src="/js/functions.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="Lbl1" runat="server" Font-Bold="True">Duplicate Phone Numbers</asp:Label>
        <dx:aspxgridview id="ASPxGridView1" runat="server" autogeneratecolumns="False"
            keyfieldname="AVID" EnableCallBacks="False" OnSelectionChanged="ASPxGridView1_SelectionChanged" KeyboardSupport="True">
            <Columns>
               <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" ShowClearFilterButton="true">
                    
                </dx:GridViewCommandColumn>
<dx:GridViewDataTextColumn  FieldName="AID" ReadOnly="True" VisibleIndex="1"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="BID" ReadOnly="True" VisibleIndex="2"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="PhoneA" VisibleIndex="3"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="PhoneB" VisibleIndex="4"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="ANum" VisibleIndex="5"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="BNum" VisibleIndex="6"></dx:GridViewDataTextColumn>    
<dx:GridViewDataTextColumn FieldName="BVID" ReadOnly="True" VisibleIndex="7"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="ANAME" VisibleIndex="8"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="BNAME" VisibleIndex="9"></dx:GridViewDataTextColumn>
</Columns>
                <SettingsBehavior EnableRowHotTrack="True" AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" AllowFocusedRow="True" />
        <Settings ShowFilterRow="True" GridLines="None" />
            <SettingsPager Mode="ShowAllRecords">
            </SettingsPager>
            <ClientSideEvents RowClick="function(s, e) {
	ASPxLoadingPanel1.Show();
}" />
</dx:aspxgridview>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
            ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
            SelectCommand="SELECT A.address_id AS AID, b.address_id AS BID,&#13;&#10;        CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast,A.address_type) AS PhoneA,&#13;&#10;        CONCAT(B.address_phonearea,B.address_phonefirst,B.address_phonelast,B.address_type) AS PhoneB,&#13;&#10;        C.vendor_number AS ANum, D.vendor_number AS BNum,&#13;&#10;        C.Vendor_ID AS AVID,D.Vendor_ID AS BVID,&#13;&#10;        C.Vendor_Name AS ANAME, D.Vendor_Name AS BNAME&#13;&#10;        FROM address as A, address as B, Vendor as C, Vendor as D&#13;&#10;        WHERE &#13;&#10;        CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast,A.address_type) = CONCAT(B.address_phonearea,B.address_phonefirst,B.address_phonelast,B.address_type)&#13;&#10;        AND CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast) != '' &#13;&#10;        AND CONCAT(B.address_phonearea,B.address_phonefirst,B.address_phonelast) != ''&#13;&#10;        AND CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast) != '0000000000' &#13;&#10;        AND CONCAT(B.address_phonearea,B.address_phonefirst,B.address_phonelast) != '0000000000'&#13;&#10;        AND CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast) != '9058272555' &#13;&#10;        AND CONCAT(B.address_phonearea,B.address_phonefirst,B.address_phonelast) != '9058272555'&#13;&#10;        AND CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast) != '9056891845' &#13;&#10;        AND CONCAT(B.address_phonearea,B.address_phonefirst,B.address_phonelast) != '9056891845'&#13;&#10;        AND CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast) != '8002044153'&#13;&#10;        AND CONCAT(B.address_phonearea,B.address_phonefirst,B.address_phonelast) != '8002044153'&#13;&#10;        AND CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast) != '9058274255'&#13;&#10;        AND CONCAT(B.address_phonearea,B.address_phonefirst,B.address_phonelast) != '9058274255'&#13;&#10;        AND A.address_id != b.address_id&#13;&#10;        AND A.Address_Table_ID = C.Vendor_ID&#13;&#10;        AND B.Address_Table_ID = D.Vendor_ID&#13;&#10;        AND A.Address_Table = 'Vendor'&#13;&#10;        AND B.Address_Table = 'Vendor'&#13;&#10;        AND C.Vendor_Active = 1&#13;&#10;        AND D.Vendor_Active = 1&#13;&#10;        GROUP BY A.address_id&#13;&#10;        ORDER BY C.Vendor_Name">
        </asp:SqlDataSource>
        <br />
        <asp:Label ID="Label1" runat="server" Font-Bold="True">Duplicate Names</asp:Label>
        <dx:ASPxGridView ID="ASPxGridView2" KeyFieldName="vendor_id" EnableCallBacks="False" runat="server" AutoGenerateColumns="False" OnSelectionChanged="ASPxGridView2_SelectionChanged">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="vendor_id" ReadOnly="True" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="vendor_name" VisibleIndex="1">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="vendor_number" VisibleIndex="2">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="vendor_name1" VisibleIndex="3">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="vendor_number1" VisibleIndex="4">
                </dx:GridViewDataTextColumn>
            </Columns>
             <SettingsBehavior EnableRowHotTrack="True" AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" AllowFocusedRow="True" />
        <Settings ShowFilterRow="True" GridLines="None" />
            <SettingsPager Mode="ShowAllRecords">
            </SettingsPager>
        </dx:ASPxGridView>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
            ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
            SelectCommand="SELECT b.vendor_id, a.vendor_name, a.vendor_number, b.vendor_name, &#13;&#10;       b.vendor_number FROM vendor AS a, vendor as b &#13;&#10;       WHERE a.vendor_name LIKE CONCAT (b.vendor_name,'%') &#13;&#10;       AND a.vendor_id != b.vendor_id &#13;&#10;        AND a.Vendor_Active = 1&#13;&#10;       AND b.Vendor_Active = 1&#13;&#10;ORDER BY A.Vendor_Name&#13;&#10;     ">
        </asp:SqlDataSource>
        <br />
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Duplicate Addresses" Visible="False"></asp:Label><br />
        <dx:ASPxGridView ID="ASPxGridView3" KeyFieldName="AVID" EnableCallBacks="False" runat="server" AutoGenerateColumns="False" OnSelectionChanged="ASPxGridView3_SelectionChanged" Visible="False">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="AID" ReadOnly="True" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BID" ReadOnly="True" VisibleIndex="1">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="AAddr" VisibleIndex="2">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BAddr" VisibleIndex="3">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ANum" VisibleIndex="4">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BNum" VisibleIndex="5">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="AVID" ReadOnly="True" VisibleIndex="6">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BVID" ReadOnly="True" VisibleIndex="7">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ANAME" VisibleIndex="8">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BNAME" VisibleIndex="9">
                </dx:GridViewDataTextColumn>
            </Columns>
             <SettingsBehavior EnableRowHotTrack="True" AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" AllowFocusedRow="True" />
        <Settings ShowFilterRow="True" GridLines="None" />
            <SettingsPager Mode="ShowAllRecords">
            </SettingsPager>
        </dx:ASPxGridView>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
            ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
            SelectCommand="SELECT A.address_id AS AID, b.address_id AS BID,&#13;&#10;        A.address_addr1 AS AAddr, B.address_addr1 AS BAddr,&#13;&#10;        C.vendor_number AS ANum, D.vendor_number AS BNum,&#13;&#10;        C.Vendor_ID AS AVID,D.Vendor_ID AS BVID,&#13;&#10;        C.Vendor_Name AS ANAME, D.Vendor_Name AS BNAME&#13;&#10;        FROM address as A, address as B, Vendor as C, Vendor as D&#13;&#10;        WHERE&#13;&#10;        A.address_addr1 = B.address_addr1&#13;&#10;        AND A.address_addr1 != ''&#13;&#10;        AND B.address_addr1 != ''&#13;&#10;        AND A.address_id != b.address_id&#13;&#10;        AND A.Address_Table_ID = C.Vendor_ID&#13;&#10;        AND B.Address_Table_ID = D.Vendor_ID&#13;&#10;        AND A.Address_Table = 'Vendor'&#13;&#10;        AND B.Address_Table = 'Vendor'&#13;&#10;        AND C.Vendor_Active = 1&#13;&#10;        AND D.Vendor_Active = 1&#13;&#10;        GROUP BY A.address_id&#13;&#10;        ORDER BY C.Vendor_Name">
        </asp:SqlDataSource>
        <br />
        <dx:ASPxLoadingPanel id="ASPxLoadingPanel1" runat="server" ClientInstanceName="ASPxLoadingPanel1"
            Modal="True">
        </dx:ASPxLoadingPanel>
    
    </div>
    </form>
</body>
</html>
