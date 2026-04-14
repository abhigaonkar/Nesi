<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_member_po_upload_receipt" EnableTheming="true" Theme="" CodeBehind="po_prog_upload_receipt.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v19.2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        .c {
            color: #000;
            font-weight: bold;
            font-size: 11px;
            font-family: calibri;
        }

        .n {
            color: #060;
            font-weight: bold;
            font-size: 11px;
            font-family: calibri;
            padding-bottom: 20px;
        }

        .m {
            color: #000;
            font-weight: bold;
            font-size: 12px;
            width: 15px;
            font-family: calibri;
            text-align: right;
        }

        .v {
            color: #000;
            font-weight: bold;
            font-size: 11px;
            font-family: calibri;
        }

        .con {
        }

        .error {
            color: #f00;
            font-size: 12px;
            font-family: calibri;
            display: block;
            padding: 10px;
        }

            .error .upload {
                display: block;
            }

        .ccwarning {
            font-weight: bold;
            font-size: 14px;
            color: #f00;
            text-align: center;
            margin-bottom: 10px;
        }
    </style>

    <link type="text/css" href="/css/autocomplete.css" rel="Stylesheet" />
    <link type="text/css" href="/css/base/ui.all.css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
        <script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
        <script type="text/javascript" src="/js/functions.js"></script>
        <script type="text/javascript">
            $("document").ready(function () {
                Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onBeginRequest);
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest);
            });
            function onBeginRequest() { please_wait("start"); }
            function onEndRequest(sender, args) {
                please_wait("stop");
            }
            function please_wait(action) {
                var loader = document.getElementById("loader"); // Assuming "loader" is the ID of your loader div
                if (action === "start") {
                    loader.style.display = "block";
                } else if (action === "stop") {
                    loader.style.display = "none";
                }
            }
        </script>
        <div>
            <asp:Label ID="Label1" runat="server" CssClass="error"></asp:Label>
            <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
            <dx:ASPxPanel ID="pnl_expense" runat="server" Width="95%">
                <PanelCollection>
                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                        <div id="loader" style="display: none; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); z-index: 1000;">
                            <!-- Add your loading spinner or text here -->
                            <img src="../../images/loading_panel.gif" alt="Loading..." />
                        </div>
                        <div class="expense">
                            <asp:Label ID="lb_error" runat="server" CssClass="error"></asp:Label>
                            <div style="padding-left: 10px">* - Denotes a required field</div>

                            <!-- User Dropdown -->
                            <div class="row">
                                <div class="title">* User:</div>
                                <div class="control">
                                    <dx:ASPxComboBox ID="cb_user" ClientInstanceName="cb_user" runat="server" DropDownStyle="DropDown" ClientEnabled="False" TextField="FullName" ValueField="Member_ID" AutoPostBack="true" ValueType="System.Int32"></dx:ASPxComboBox>

                                </div>
                            </div>

                            <!-- Business Unit Dropdown -->
                            <div class="row">
                                <div class="title">* Business Unit:</div>
                                <div class="control">
                                    <dx:ASPxComboBox ID="ddlBusinessUnit" runat="server"
                                        TextField="name"
                                        ValueField="id"
                                        ValueType="System.Int32"
                                        AutoPostBack="True"
                                        Width="250px">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>

                            <!-- File Upload -->
                            <div class="control">
                                <div class="row">
                                    <div class="title">
                                        * Packing Slip:<br />
                                        <div style="font-size: 0.75em;">Please attach JPEG/JPG/PNG/PDF only</div>
                                    </div>
                                    <%-- <asp:FileUpload ID="add_File" runat="server" />--%>
                                    <div class="control">
                                        <dx:ASPxUploadControl ID="po_receipt" runat="server" ClientInstanceName="po_receipt" Width="90%" ShowProgressPanel="True" Native="true">
                                                <ValidationSettings
                                                AllowedFileExtensions=".jpe,.jpeg,.jpg,.pdf,.png"
                                                NotAllowedFileExtensionErrorText="This file extension is not supported"
                                                MaxFileSizeErrorText="File size exceeds the maximum allowed size, which is 4MB."
                                                MaxFileSize="4194304">
                                            </ValidationSettings>

                                            <ClientSideEvents
                                                FileUploadStart="function(s, e) { please_wait('start'); }"
                                                FileUploadComplete="function(s, e) { please_wait('stop'); }" />
                                            <AdvancedModeSettings>
                                                <FileListItemStyle CssClass="pending dxucFileListItem"></FileListItemStyle>
                                            </AdvancedModeSettings>
                                        </dx:ASPxUploadControl>
                                        <b class="n">* Max file size - 4MB</b>
                                    </div>
                                </div>
                            </div>
                            <br />

                            <!-- Save Button -->
                            <div class="row">
                                <div class="title"></div>
                                <div class="control">
                                    <dx:ASPxButton ID="b_save" runat="server" ClientInstanceName="b_save" Text="Upload Packing Slip" OnClick="b_save_Click" AutoPostBack="True">
                                        <ClientSideEvents Click="function(s, e) { please_wait('start'); }" />
                                        <Image Url="~/images/icon/icon[save].gif"></Image>
                                    </dx:ASPxButton>

                                </div>
                            </div>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </div>

    </form>
    <script type="text/javascript">
        // Ensure the endRequest handler is attached once the page loads
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            please_wait("stop");
        });
    </script>
</body>
</html>
