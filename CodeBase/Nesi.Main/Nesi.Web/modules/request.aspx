<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="request.aspx.cs" Inherits="Nesi.Web.modules.CustomerVendorRequest" EnableTheming="False" EnableEventValidation="false" %>

<!DOCTYPE html5>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer Request</title>
    <style type="text/css">
        html, body {
            font-size: 1em;
        }

        .input {
            font-family: "segoe ui", Arial, sans-serif;
            font-size: 1.35em;
            width: 90%;
            resize: none;
        }

        .content {
            text-align: center;
            padding: 5px;
        }

        .notes {
            width: 90%;
            height: 300px;
            font-family: "segoe ui", Arial, sans-serif;
            font-size: 1.35em;
        }

        .fourrow {
            height: 7.5em;
        }

        .field {
            padding: 5px;
        }

        .regarding {
            font-size: 0.9em;
            color: #999;
        }

        .contentTitle {
            text-align: left;
            width: 90%;
            font-family: "segoe ui", Arial, sans-serif;
            font-size: 1.35em;
        }

        .custReqTitle {
            font-size: 1.1em;
            text-align: left;
            padding-left: 10px;
        }

        .custReq {
            text-align: left;
            padding-left: 20px;
        }

        .info {
            text-align: left;
            width: 90%;
            font-family: "segoe ui", Arial, sans-serif;
            font-size: 1.35em;
        }

        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: transparent;
            z-index: 99;
            opacity: 1.0;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: transparent;
            z-index: 999;
        }
    </style>
    <script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="/js/functions.js"></script>

    <link href="/App_Themes/BlueStyle/common.css?new" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            //  page_obj.update_panel_progress.bind();
        })

    </script>

</head>
<body>
    <div id="top_level_modal_progress" class="loading" align="center">
        <img src="/images/loading_panel.gif" alt="" id="ld_pic" />
    </div>
    <form id="ReqForm" runat="server">
        <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
        
        <div class="content">
            <!-- VENDOR SECTION (Original - Don't Change) -->
            <div id="vendorreq" runat="server">
                <div class="field">
                    <div class="contentTitle" id="content_title" runat="server"></div>
                </div>
                <div class="field regarding" id="div_regarding" runat="server">
                    <div class="info" id="content_regarding" runat="server"></div>
                </div>
                <div class="field">
                    <input type="text" runat="server" id="name" placeholder="Enter Name" class="input" required />
                </div>
                <div class="field" id="div_ddladdress" runat="server">
                    <asp:DropDownList runat="server" ID="ddlAddress" DataValueField="id" DataTextField="text" placeholder="Select Address" AutoPostBack="True" CssClass="input" OnSelectedIndexChanged="ddlAddress_OnSelectedIndexChanged" />
                </div>
                <div class="field">
                    <textarea id="address" runat="server" placeholder="Enter Address" class="input fourrow" required></textarea>
                </div>
                <div class="field">
                    <input type="tel" runat="server" id="phone" placeholder="Enter Phone #" name="phone" class="input" required />
                </div>
                <div class="field">
                    <input type="email" runat="server" id="email" placeholder="Enter Email" class="input" />
                </div>
                <div class="field">
                    <input type="text" runat="server" id="contact" placeholder="Enter Contact Name" class="input" />
                </div>
            </div>
            <!-- END VENDOR SECTION -->

            <!-- CUSTOMER SECTION (New/Modified) -->
            <div id="customerreq" runat="server" visible="false">
                <div class="field">
                    <div class="contentTitle" id="Div1" runat="server"></div>
                </div>
                <div class="field regarding" id="div2" runat="server">
                    <div class="info" id="Div3" runat="server"></div>
                </div>
                
                <div class="field">
                    <input type="text" runat="server" id="custname" placeholder="Enter Company Name" class="input" required />
                </div>

                <div class="field">
                    <input type="text" runat="server" id="custcontact" placeholder="Enter Contact Name" class="input" />
                </div>

                <div class="field">
                    <input type="text" runat="server" id="jobtitle" placeholder="Enter Job Title" class="input" />
                </div>

                <div class="field">
                    <input type="email" runat="server" id="custemail" placeholder="Enter Email" class="input" />
                </div>

                <div class="field">
                    <input type="tel" runat="server" id="custphone" placeholder="Main Line #" class="input" required />
                </div>

                <div class="field">
                    <input type="tel" runat="server" id="mobile" placeholder="Mobile #" class="input" />
                </div>

                <!-- Customer addresses -->
                <div id="divcustaddress" runat="server">
                    <div class="field">
                        <textarea id="billingAddress" runat="server" placeholder="Billing Address" class="input fourrow"></textarea>
                    </div>

                    <div class="field">
                        <textarea id="shippingAddress" runat="server" placeholder="Site Address" class="input fourrow"></textarea>
                    </div>
                </div>

                <!-- Customer Request Type -->
                <div id="divCustomerReqType" class="custReq" runat="server">
                    <p class="custReqTitle">Customer Request Type:</p>
                    <input type="radio" runat="server" id="custProspect" name="reqtype" value="Prospect" />
                    <label for="custProspect">Prospect</label>
                    <input type="radio" runat="server" id="custLead" name="reqtype" value="Lead" />
                    <label for="custLead">Lead</label>
                </div>

                <!-- Customer Dropdowns -->
                <div id="divDropdowns" runat="server">
                    <div class="field">
                        <label for="ddlNatAccount">National Account Manager:</label>
                        <asp:DropDownList runat="server" ID="ddlNatAccount" CssClass="input">
                            <asp:ListItem Text="-- Select National Account Manager --" Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="field">
                        <label for="ddlBDM">Business Development Manager:</label>
                        <asp:DropDownList runat="server" ID="ddlBDM" CssClass="input">
                        </asp:DropDownList>
                    </div>

                    <div class="field">
                        <label for="ddlInsideSales">Inside Sales Team Member:</label>
                        <asp:DropDownList runat="server" ID="ddlInsideSales" CssClass="input">
                        </asp:DropDownList>
                    </div>

                    <div class="field">
                        <label for="ddlIndustrialType">Industrial Type:</label>
                        <asp:UpdatePanel ID="upInd" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="ddlIndustrialType" CssClass="input"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlIndustrialType_Changed" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="field">
                        <label for="ddlEndMarketSegment">End Market Segment:</label>
                        <asp:UpdatePanel ID="upEnd" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="ddlEndMarketSegment" CssClass="input" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="field">
                        <label for="ddlLeadSource">Lead Source:</label>
                        <asp:DropDownList runat="server" ID="ddlLeadSource" CssClass="input">
                        </asp:DropDownList>
                    </div>

                    <div class="field">
                        <label for="txtLeadSourceDesc">Source Description (if Other):</label>
                        <input type="text" runat="server" id="txtLeadSourceDesc" placeholder="Source Description if Other" class="input" />
                    </div>

                    <div class="field">
                        <p class="custReqTitle">Business Units:</p>
                        <asp:CheckBoxList runat="server" ID="chkBusinessUnits" RepeatColumns="2" RepeatDirection="Horizontal" CssClass="input" />
                    </div>
                </div>
            </div>
            <!-- END CUSTOMER SECTION -->

            <!-- COMMON FIELDS -->
            <div class="field">
                <textarea id="request_notes" runat="server" placeholder="Additional Details" class="notes" required minlength="11"></textarea>
            </div>

            <div class="field">
                <asp:FileUpload ID="add_File" runat="server" />
            </div>

            <div class="action">
                <asp:Button runat="server" CssClass="input" ID="btn_send" Text="Send" OnClick="btn_send_OnClick" />
            </div>
        </div>

    </form>

</body>
</html>