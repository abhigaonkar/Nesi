<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="comments_fixup.aspx.cs" Inherits="Nesi.Web.sections.workorder.comments_fixup" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="sm"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="padding-right: 5px; padding-left: 5px;">
                    <asp:Label ID="lblHeading" runat="server" Font-Bold="True" Font-Names="Calibri" Font-Size="12pt" ForeColor="#999999" />
                    <br />
                    <br />
                    <asp:Image ID="ImageWarning" runat="server" ImageUrl="~/images/icon/icon[warning].gif" />
                    <asp:Image ID="ImageOkay" runat="server" ImageUrl="~/images/icon/CheckGreen.png" />
                    <asp:Label ID="lblWarningInfo" runat="server" Text="" Font-Bold="True" Font-Size="12pt"></asp:Label>
                    <br />

                    <dx:ASPxGridView ID="gdComments" runat="server" AutoGenerateColumns="False" Width="754px">
                        <SettingsPager PageSize="15">
                        </SettingsPager>
                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Comment ID" FieldName="WoComment_ID" VisibleIndex="0" Width="70px">
                                <HeaderStyle Font-Bold="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Print" FieldName="PrintComments" VisibleIndex="2" Width="50px">
                                <HeaderStyle Font-Bold="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Created Date" FieldName="Created_Date" VisibleIndex="3" Width="120px">
                                <HeaderStyle Font-Bold="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Modified Date" FieldName="Modified_Date" VisibleIndex="4" Width="120px">
                                <HeaderStyle Font-Bold="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataMemoColumn Caption="Comment" FieldName="comments" ShowInCustomizationForm="True" VisibleIndex="1">
                                <HeaderStyle Font-Bold="True" />
                            </dx:GridViewDataMemoColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <br />
                    <asp:Label ID="lblFix" runat="server" Text="" Font-Bold="True" Font-Size="12pt"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtComments" runat="server" Rows="5" TextMode="MultiLine" Width="747px" Height="185px"></asp:TextBox>
                    <br />
                    <br />
                    <br />
                </div>
                <asp:Button ID="btnUpdateTS" runat="server" Text="Save" OnClick="btnUpdateTS_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="workOrderID" runat="server" />
        <asp:HiddenField ID="workCommentIdList" runat="server" />
        <asp:HiddenField ID="HiddenFieldOrderNumber" runat="server" />
        <asp:HiddenField ID="HiddenFieldBuinessUnit" runat="server" />
    </form>
</body>
</html>
