<%@ Page Title="" Language="C#" MasterPageFile="" AutoEventWireup="true" CodeBehind="part_cost_upload.aspx.cs" Inherits="Nesi.Web.sections.member.inventory.part_cost_upload" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style>
        html, body {
            height: 100%;
        }

        body {
            margin: 0;
        }

        #content {
            height: 100%;
            padding: 50px;
            margin: 0;
            display: flex;
            align-items: start;
            justify-content: left;
        }

            #content .instructions {
                font-family: Roboto, Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
                font-size: 1.0em;
                font-weight: normal !important;
                width: 560px;
            }

                #content .instructions .fieldname {
                    font-size: 0.8em;
                    display: block;
                    margin-left: 10px;
                }

        .dxgvControl .dxgvTable td,
        .dxgvControl .dxgvTable th {
            border: none !important;
        }
    </style>

    <script type="text/javascript">


        function uploadFile(s, e) {
            if (upControl.GetText() != "") {


                var label = document.getElementById("lblUploadStatus");
                if (label) {
                    label.style.display = "none";
                }


                var gridWrapper = document.getElementById("panelFailedUploads");
                if (gridWrapper) {
                    gridWrapper.style.display = "none";
                }

                btProcess.SetText("Please wait...");
                upControl.Upload();
                btProcess.SetEnabled(false);
            }
            else {
                alert("Please provide a csv file to process.");
            }
        }

        function fileComplete(s, e) {
            btProcess.SetEnabled(true);
            btProcess.SetText("Process");
            var message = e.callbackData;
            var results = "";
            var typeOfResponse = "";
            var shouldProcess = true;


            try {
                results = message.split("\n");
                typeOfResponse = results[0].split("|")[1];
            } catch (e) {
                shouldProcess = false;
            }

            var label = document.getElementById("lblUploadStatus");
            if (label) {
                label.innerHTML = message;
                label.style.display = "block";
            }

            if (message.includes("not uploaded")) {
                callbackPanel.PerformCallback();
            }
        }
    </script>

    <title>Part Cost Upload</title>
</head>
<body>
    <div id="content">
        <form id="form1" runat="server">
            <div class="instructions">
                <b>Instructions for Importing</b><br />
                <br />

                Please ensure the following fields are listed in this exact order and with the exact names shown below.<br />
                The format must match exactly to ensure proper processing:<br />
                <ul>
                    <li>internal_org_item_id
						<span class="fieldname">DataType: <i>integer (eg: 1228)</i></span></li>
                    <li>avg_price
						<span class="fieldname">DataType: <i>decimal (eg: 19.29)</i></span></li>
                    <li>recommended_price
						<span class="fieldname">DataType: <i>decimal (eg: 19.29)</i></span></li>
                    <li>week_of
						<span class="fieldname">DataType: <i>date (eg: 4/14/2025)</i></span></li>
                    <li>Branch
						<span class="fieldname">DataType: <i>text (eg: Belleville Service)</i></span></li>

                </ul>
                Only CSV files can be imported.<br />

                <br />
                <br />
                <br />
            </div>

            <dx:ASPxUploadControl ID="upControl" runat="server" UploadMode="Advanced" Width="280px" ClientInstanceName="upControl"
                Theme="MaterialCompact" NullText="Select CSV File" ValidateRequestMode="Enabled" ShowProgressPanel="False"
                FileUploadMode="BeforePageLoad" OnFileUploadComplete="upControl_OnFileUploadComplete">
                <ValidationSettings AllowedFileExtensions=".csv" MaxFileCount="1" ShowErrors="True">
                </ValidationSettings>
                <ClientSideEvents FileUploadComplete="fileComplete"></ClientSideEvents>
            </dx:ASPxUploadControl>
            <br />

            <dx:ASPxButton ID="btProcess" ClientInstanceName="btProcess" runat="server" Text="Process" Width="280px"
                AutoPostBack="False" Theme="MaterialCompact">
                <ClientSideEvents Click="uploadFile"></ClientSideEvents>
            </dx:ASPxButton>
            <br />
            <br />

            <asp:Label ID="lblUploadStatus" runat="server" ForeColor="" Visible="True"></asp:Label>
            <br />

            <dx:ASPxCallbackPanel ID="callbackPanel" runat="server" ClientInstanceName="callbackPanel"
                OnCallback="callbackPanel_Callback">
                <PanelCollection>
                    <dx:PanelContent>
                        <div id="panelFailedUploads">
                            <dx:ASPxGridView ID="gridFailedUploads" runat="server" AutoGenerateColumns="False" Width="100%" Visible="False" OnPageIndexChanged="gridFailedUploads_PageIndexChanged">
                                <Styles>
                                    <Header BackColor="#35B86B" ForeColor="White" />
                                    <AlternatingRow BackColor="#D3D3D3" />
                                    <RowHotTrack BackColor="#E3F2FD" />
                                </Styles>
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="InternalOrgItemId" Caption="Part Number" />
                                    <dx:GridViewDataTextColumn FieldName="AvgPrice" Caption="Average Price" />
                                    <dx:GridViewDataTextColumn FieldName="RecommendedPrice" Caption="Recommended Price" />
                                    <dx:GridViewDataDateColumn FieldName="WeekOf" Caption="Week Of" />
                                    <dx:GridViewDataTextColumn FieldName="Branch" Caption="Branch" />
                                    <dx:GridViewDataTextColumn FieldName="ErrorMessage" Caption="Error Message" />
                                </Columns>
                            </dx:ASPxGridView>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </form>
    </div>
</body>
</html>
