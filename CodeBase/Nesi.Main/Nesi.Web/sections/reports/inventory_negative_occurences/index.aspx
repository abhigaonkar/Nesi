<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_reports_inventory_negative_occurences_index" Codebehind="index.aspx.cs" %>

<%@ Register assembly="DevExpress.XtraCharts.v19.2.Web, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<script type="text/javascript">

 function resizeIframe(obj)
 {

   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 10) + 'px';
	
 }
 </script>

	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
    <dxchartsui:WebChartControl ID="wcc" runat="server" CrosshairEnabled="True" DataSourceID="SqlDataSource1" Height="600px" SeriesDataMember="branch" Width="900px">
        <diagramserializable>
            <cc1:XYDiagram>
                <axisx visibleinpanesserializable="-1">
                </axisx>
                <axisy visibleinpanesserializable="-1">
                </axisy>
            </cc1:XYDiagram>
        </diagramserializable>
        <seriestemplate argumentdatamember="date" argumentscaletype="Qualitative" valuedatamembersserializable="count">
            <viewserializable>
                <cc1:LineSeriesView MarkerVisibility="True">
                    <linemarkeroptions size="4">
                    </linemarkeroptions>
                </cc1:LineSeriesView>
            </viewserializable>
        </seriestemplate>
        <titles>
            <cc1:ChartTitle Font="Calibri, 18pt" Text="Negative Location Count Occurences" />
        </titles>
    </dxchartsui:WebChartControl>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
business_unit.name branch,
concat(year(log.dt),'-',MONTH(log.dt)) date,
count(log.id) count
FROM
log
INNER JOIN log_action ON log.action_id = log_action.id AND log.action_id = 3
INNER JOIN log_section ON log.section_id = log_section.id AND log.section_id = 2
inner join business_unit on business_unit.id = log.business_unit_id
where log.dt &gt; curdate() - interval 12 month
AND log.value_old&gt;=0 and log.value_new &lt;0
and log.business_unit_id &lt;&gt;8
        and log.business_unit_id &lt;&gt;11
        and log.business_unit_id &lt;&gt;48

GROUP BY concat(year(log.dt),'-',MONTH(log.dt)),log.business_unit_id

order by log.business_unit_id,log.dt"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>


