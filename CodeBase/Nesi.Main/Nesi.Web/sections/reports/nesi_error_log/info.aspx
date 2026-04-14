<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_reports_nesi_error_log_info" Codebehind="info.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        table td:first-child {
            font-weight: bold;
            padding-right: 7px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tbody>
                <tr>
                    <td>Member: </td>
                    <td>
                        <dx:ASPxLabel ID="lab_member" runat="server" Text=""></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>Host: </td>
                    <td>
                        <dx:ASPxLabel ID="lab_host" runat="server" Text=""></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>Date: </td>
                    <td>
                        <dx:ASPxLabel ID="lab_date" runat="server" Text=""></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>Server Name: </td>
                    <td>
                        <dx:ASPxLabel ID="lab_server" runat="server" Text=""></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>Path: </td>
                    <td>
                        <dx:ASPxLabel ID="lab_path" runat="server" Text=""></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>User Agent String: </td>
                    <td>
                        <dx:ASPxLabel ID="lab_uag" runat="server" Text=""></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>User IP: </td>
                    <td>
                        <dx:ASPxLabel ID="lab_ip" runat="server" Text=""></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>Message: </td>
                    <td>
                        <dx:ASPxLabel ID="lab_message" runat="server" Text=""></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>Query: </td>
                    <td>
                        <dx:ASPxLabel ID="lab_query" runat="server" Text=""></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>Stack Trace: </td>
                    <td>
                        <dx:ASPxLabel ID="lab_stack" runat="server" Text=""></dx:ASPxLabel>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
