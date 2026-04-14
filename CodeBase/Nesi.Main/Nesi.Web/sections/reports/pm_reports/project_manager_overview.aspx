<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits		= 'pm_reports' 
			Title			= 'Project Manager Overview' 
 Codebehind="project_manager_overview.aspx.cs" %>
<%@ REGISTER assembly="RJS.Web.WebControl.PopCalendar" namespace="RJS.Web.WebControl"
	tagprefix="rjs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				<div id='divSide' runat='server'>
					<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
				</div>
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
				<div id='divMenu' runat='server'></div>
</asp:Content>
<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server'>
<asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Medium" Text="Select Date Range for Invoiced Work Orders"></asp:Label><br />
<ASP:LABEL id="Label8" runat="server" text="Starting Date"></ASP:LABEL>
<ASP:TEXTBOX id="txtSelectedDate" runat="server" AutoPostBack="true"  ReadOnly="True"></ASP:TEXTBOX>
<RJS:PopCalendar id="txtCalendarSearch" runat="server" messagealignment="RightCalendarControl" format="yyyy mm dd" control="txtSelectedDate" AutoPostBack="false"  ></RJS:PopCalendar>
<asp:Label ID="LABEL1" runat="server" Text="Ending Date"></asp:Label>
<asp:TextBox ID="txtEndDate" runat="server" AutoPostBack="true" ReadOnly="True"></asp:TextBox>
<rjs:PopCalendar ID="POPCALENDAR1" runat="server" Control="txtEndDate" Format="yyyy mm dd" MessageAlignment="RightCalendarControl" AutoPostBack="false"  />
 
 
    
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" KeyFieldName="business_unit_id" OnDetailRowExpandedChanged="ASPxGridView1_DetailRowExpandedChanged" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
        <SettingsPager Mode="ShowAllRecords">
        </SettingsPager>
        <Templates>
        <DetailRow>
        <dx:ASPxGridView ID="ASPxGridView2" runat="server" AutoGenerateColumns="False" KeyFieldName="WOProg_PM_MemberID" OnBeforePerformDataSelect="ASPxGridView2_BeforePerformDataSelect" OnInit="ASPxGridView2_Init" OnDetailRowExpandedChanged="ASPxGridView2_DetailRowExpandedChanged" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
        <SettingsPager Mode="ShowAllRecords">
        </SettingsPager>
        <Templates>
            <DetailRow>
                <dx:ASPxGridView ID="ASPxGridViewCustomer" runat="server" AutoGenerateColumns="False" KeyFieldName="woprog_customer_id" OnInit="ASPxGridViewCustomer_Init" OnBeforePerformDataSelect="ASPxGridViewCustomer_BeforePerformDataSelect" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue">
                    <SettingsDetail ShowDetailRow="True" />
                    <Columns>
                    <dx:GridViewDataTextColumn FieldName="WOProg_CustomerName" VisibleIndex="0" Caption="Customer">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="LABSUM" VisibleIndex="1" Caption="Labor">
                     <PropertiesTextEdit DisplayFormatString="c">
                     </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MATSUM" VisibleIndex="2" Caption="Material">
                     <PropertiesTextEdit DisplayFormatString="c">
                     </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="INVSSUM" VisibleIndex="3" Caption="Invoiced">
                     <PropertiesTextEdit DisplayFormatString="c">
                     </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn FieldName="OVERAGESUM" VisibleIndex="4" Caption="Overage">
                      <PropertiesTextEdit DisplayFormatString="c">
                      </PropertiesTextEdit>
                     </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="custer_count" VisibleIndex="5" Caption="# of Customers">
                    </dx:GridViewDataTextColumn>
                    </Columns>
                    <Templates>
                        <DetailRow>
                            <dx:ASPxGridView ID="ASPxGridViewWorkOrders" runat="server" KeyFieldName="woprog_id" OnInit="ASPxGridViewWorkOrders_Init" AutoGenerateColumns="False" CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange">
                                <SettingsDetail />
                                <Columns>
                                 <dx:GridViewDataTextColumn FieldName="woprog_bvwo" VisibleIndex="0" Caption="Work Order">
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="woprog_invoiceno" VisibleIndex="1" Caption="Invoice#">
                                 </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn FieldName="woprog_quoteid" VisibleIndex="2" Caption="Quote#">
                                 </dx:GridViewDataTextColumn>     
                                 <dx:GridViewDataTextColumn FieldName="WOProg_LabourTotalSell" VisibleIndex="3" Caption="Labor">
                                  <PropertiesTextEdit DisplayFormatString="c">
                                  </PropertiesTextEdit>
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="WOProg_MaterialTotalSell" VisibleIndex="4" Caption="Material">
                                  <PropertiesTextEdit DisplayFormatString="c">
                                 </PropertiesTextEdit>
                                 </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="WOProg_InvoicedNetTotal" VisibleIndex="5" Caption="Invoiced">
                                  <PropertiesTextEdit DisplayFormatString="c">
                                   </PropertiesTextEdit>
                                   </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="OVERAGESUM" VisibleIndex="6" Caption="Overage">
                                      <PropertiesTextEdit DisplayFormatString="c">
                                        </PropertiesTextEdit>
                                       </dx:GridViewDataTextColumn>
                                </Columns>
                                <Styles CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange"
                                    GroupButtonWidth="28">
                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                    </Header>
                                    <LoadingPanel ImageSpacing="8px">
                                    </LoadingPanel>
                                </Styles>
                                <SettingsLoadingPanel ImagePosition="Top" />
                                <ImagesFilterControl>
                                    <LoadingPanel Url="~/App_Themes/SoftOrange/Editors/Loading.gif">
                                    </LoadingPanel>
                                </ImagesFilterControl>
                                <Images SpriteCssFilePath="~/App_Themes/SoftOrange/{0}/sprite.css">
                                    <LoadingPanelOnStatusBar Url="~/App_Themes/SoftOrange/GridView/gvLoadingOnStatusBar.gif">
                                    </LoadingPanelOnStatusBar>
                                    <LoadingPanel Url="~/App_Themes/SoftOrange/GridView/Loading.gif">
                                    </LoadingPanel>
                                </Images>
                                <Paddings Padding="1px" />
                                <StylesEditors>
                                    <CalendarHeader Spacing="1px">
                                    </CalendarHeader>
                                    <ProgressBar Height="29px">
                                    </ProgressBar>
                                </StylesEditors>
                            </dx:ASPxGridView>
                        </DetailRow>
                    </Templates>
                    <Styles CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue">
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                    </Styles>
                    <ImagesFilterControl>
                        <LoadingPanel Url="~/App_Themes/Office2003Blue/Editors/Loading.gif">
                        </LoadingPanel>
                    </ImagesFilterControl>
                    <Images SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css">
                        <LoadingPanelOnStatusBar Url="~/App_Themes/Office2003Blue/GridView/gvLoadingOnStatusBar.gif">
                        </LoadingPanelOnStatusBar>
                        <LoadingPanel Url="~/App_Themes/Office2003Blue/GridView/Loading.gif">
                        </LoadingPanel>
                    </Images>
                    <StylesEditors>
                        <ProgressBar Height="25px">
                        </ProgressBar>
                    </StylesEditors>
                </dx:ASPxGridView>
            </DetailRow>
        </Templates>
        <SettingsDetail AllowOnlyOneMasterRowExpanded="True" ShowDetailRow="True" />
        <Columns>
           <dx:GridViewDataTextColumn FieldName="Member_Name" VisibleIndex="0" Caption="Project Manager">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LABSUM" VisibleIndex="1" Caption="Labour">
             <PropertiesTextEdit DisplayFormatString="c">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MATSUM" VisibleIndex="2" Caption="Material">
             <PropertiesTextEdit DisplayFormatString="c">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="INVSUM" VisibleIndex="3" Caption="Invoiced">
             <PropertiesTextEdit DisplayFormatString="c">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn FieldName="OVERAGESUM" VisibleIndex="4" Caption="Overage">
              <PropertiesTextEdit DisplayFormatString="c">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Cust_Count" VisibleIndex="5" Caption="# of Customers">
            </dx:GridViewDataTextColumn>
        </Columns>
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
            </Styles>
            <ImagesFilterControl>
                <LoadingPanel Url="~/App_Themes/Glass/Editors/Loading.gif">
                </LoadingPanel>
            </ImagesFilterControl>
            <Images SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css">
                <LoadingPanelOnStatusBar Url="~/App_Themes/Glass/GridView/gvLoadingOnStatusBar.gif">
                </LoadingPanelOnStatusBar>
                <LoadingPanel Url="~/App_Themes/Glass/GridView/Loading.gif">
                </LoadingPanel>
            </Images>
            <StylesEditors>
                <CalendarHeader Spacing="1px">
                </CalendarHeader>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
        </dx:ASPxGridView>
        </DetailRow>
        </Templates>
        <SettingsDetail ShowDetailRow="True" AllowOnlyOneMasterRowExpanded="True" />
        <Columns>
            <dx:GridViewDataTextColumn FieldName="name" VisibleIndex="0" Caption="Business Unit">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Customer_Count" VisibleIndex="5" Caption="Number of Customers">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LABSUM" VisibleIndex="1" Caption="Sum of Labor Sold">
             <PropertiesTextEdit DisplayFormatString="c">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MATSUM" VisibleIndex="2" Caption="Sum of Material Sold">
             <PropertiesTextEdit DisplayFormatString="c">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="INVSUM" VisibleIndex="3" Caption="Sum of Workorders Invoiced">
             <PropertiesTextEdit DisplayFormatString="c">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Overage" FieldName="OVERAGESUM" VisibleIndex="4">
                <PropertiesTextEdit DisplayFormatString="c">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
        </Columns>
        <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
            <LoadingPanel ImageSpacing="8px">
            </LoadingPanel>
        </Styles>
        <SettingsLoadingPanel ImagePosition="Top" />
        <ImagesFilterControl>
            <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
            </LoadingPanel>
        </ImagesFilterControl>
        <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
            <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
            </LoadingPanelOnStatusBar>
            <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
            </LoadingPanel>
        </Images>
        <StylesEditors>
            <CalendarHeader Spacing="1px">
            </CalendarHeader>
            <ProgressBar Height="25px">
            </ProgressBar>
        </StylesEditors>
        <ImagesEditors>
            <DropDownEditDropDown>
                <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
            </DropDownEditDropDown>
            <SpinEditIncrement>
                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                    PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
            </SpinEditIncrement>
            <SpinEditDecrement>
                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                    PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
            </SpinEditDecrement>
            <SpinEditLargeIncrement>
                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                    PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
            </SpinEditLargeIncrement>
            <SpinEditLargeDecrement>
                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                    PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
            </SpinEditLargeDecrement>
        </ImagesEditors>
    </dx:ASPxGridView>
    <br />
   </asp:Content>
