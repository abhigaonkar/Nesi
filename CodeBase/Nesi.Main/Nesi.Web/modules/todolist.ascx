<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_todolist" Codebehind="todolist.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<style type="text/css">

 
 
 
</style>
<script type="text/javascript">
		
</script>
<div width="100%">
                    <dx:ASPxGridView runat="server" ClientInstanceName="gv_todo" AutoGenerateColumns="False" Width="100%" Font-Names="Arial" Font-Size="8pt" ID="gv_todo"><Columns>
<dx:GridViewDataTextColumn FieldName="link" ShowInCustomizationForm="True" Width="150px" Caption="link" VisibleIndex="2"><DataItemTemplate>
							<dx:ASPxHyperLink ID="hl_stuff" runat="server" 
							NavigateUrl="<%# string.Format(&quot;javascript:boing('{0}', 'stuff', 1100,750)&quot;, Eval(&quot;link&quot;)) %>" 
							Text='<%# Eval("link2") %>' Font-Names="Arial" Font-Size="8pt">
							</dx:ASPxHyperLink>
				
</DataItemTemplate>

<CellStyle Wrap="False"></CellStyle>
</dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="description" ShowInCustomizationForm="True" Width="100%" Caption="description" VisibleIndex="3">
<CellStyle Wrap="False"></CellStyle>
</dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="overdue" ShowInCustomizationForm="True" Width="100px" Caption="overdue" VisibleIndex="0">
<CellStyle Wrap="False"></CellStyle>
</dx:GridViewDataTextColumn>
<dx:GridViewDataDateColumn FieldName="star" SortIndex="0" SortOrder="Ascending" ShowInCustomizationForm="True" Width="150px" Caption="star" Visible="False" VisibleIndex="1">
<PropertiesDateEdit DisplayFormatString=""></PropertiesDateEdit>

<Settings SortMode="Value"></Settings>

<CellStyle Wrap="False"></CellStyle>
</dx:GridViewDataDateColumn>
</Columns>

<SettingsPager PageSize="25" AlwaysShowPager="True"></SettingsPager>

<Settings ShowHeaderFilterBlankItems="False" ShowColumnHeaders="False" GridLines="Vertical" ShowTitlePanel="True"></Settings>

                    <SettingsText Title="To Do Right Now" EmptyDataRow="Yeah!  you are caught up!" />

<Styles>
<Table>
<Border BorderStyle="None"></Border>
</Table>
    <TitlePanel BackColor="#66FF33" Font-Bold="True" Font-Names="Calibri" Font-Size="14px" ForeColor="Black" HorizontalAlign="Left">
        <Paddings Padding="8px" />
        <BorderBottom BorderStyle="None" />
    </TitlePanel>
</Styles>

<Border BorderStyle="None"></Border>
</dx:ASPxGridView>
    </div>

						
            

      
