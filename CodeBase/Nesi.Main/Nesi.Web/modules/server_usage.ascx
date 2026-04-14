<%@ Control Language="C#" AutoEventWireup="true"  Inherits="modules_server_usage" EnableTheming="True" Codebehind="server_usage.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>


            

<%@ Register src="layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>

<link href="/css/jquery.circliful.css" rel="stylesheet" type="text/css" />
	<link href="/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

	<script src="//code.jquery.com/jquery-1.4.min.js"></script>
	<script src="/js/jquery.circliful.js"></script>
<style type="text/css" >
    .circle-text-half{
        
        color: #000;
        font-size: 80px;
    }

    .circle-info-half{
        font-size: 40px;
        color: #000;

    }
</style>
            

			<dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" Text="Enable Timer" 
				CheckState="Unchecked">
				<ClientSideEvents CheckedChanged="function(s, e) {
	timer.SetEnabled(s.GetChecked());
}" />
			</dx:ASPxCheckBox>
			

<div id="usage"></div>
<script>
    var serversLeft = 0;
    var serversDown = 0;
    function loadServer(server, load) {
        console.log(server + "  "+ load);
        //Does server exist
        if ($("#" + server).length == 0) {

            console.log("NEW " + server);
            $("#usage").append("<div id='" + server + "' style='left: " + serversLeft + "px;top: " + serversDown + "px;' ></div>");
            var value = $("#" + server).circliful({
                getText: function () {
                    if (this.usesTotal()) {
                        return Math.round(this.getCurrentValue()) + " %";
                    } else {
                        return this.getCurrentValue() + " %";
                    }
                },
                getInfoText: function () {
                    return server;
                },
                "dimension": 500,
                "background-radius": 250,
                "foreground-radius": 250,
                "background-width": 35,
                "foreground-width": 35,
                "max-angle": 1,
                "start-point": 1,
                "display-style": "half",
                "circle-text-class": "percent",
                "info-text-class": "serverName"

            });
            if (serversLeft == 1200) {
                serversLeft = 0;
                serversDown += 400;
            } else {
                serversLeft += 600;
            }
        }
        $("#" + server).circliful('animateToValue', load);
        
    }



function createStat() {
    
}

</script>
			 
	<dx:ASPxGridView ID="gv_cpu" runat="server" ClientInstanceName="gv_cpu" Width ="100%" 
				oncustomcallback="gv_CustomCallback" 
				KeyFieldName="source" PreviewFieldName="message" OnRowDataBound="gv_RowDataBound">

		<ClientSideEvents EndCallback="function(s, e) {
            
            var counter = 0;

            
            //loadServer(s.cp_0server, s.cp_0load)
            //loadServer(s.cp_1server, s.cp_1load)
            //loadServer(s.cp_2server, s.cp_2load)
            //loadServer(s.cp_3server, s.cp_3load)
            //loadServer(s.cp_4server, s.cp_4load)
            //loadServer(s.cp_5server, s.cp_5load)

        }" />

		<SettingsBehavior ColumnResizeMode="Control" AllowGroup="False" />
		<SettingsPager Visible="False" PageSize="100">
        </SettingsPager>
		<Settings ShowTitlePanel="True" GridLines="Horizontal" />
		<SettingsText Title="CPU usage" />
		<SettingsLoadingPanel ImagePosition="Top" ShowImage="False" Mode="Disabled" />
		<SettingsSearchPanel Visible="false" />
		<SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
		<Styles>
		</Styles>
	</dx:ASPxGridView>
	<dx:ASPxTimer ID="serverUsageTimer" runat="server" Interval="25000" 
				ClientSideEvents-Tick='function (s,e){gv_cpu.PerformCallback("refresh");}' 
				ClientInstanceName="timer" Enabled="False">
	</dx:ASPxTimer>


            

<dx:ASPxCallbackPanel ID="loadServers" runat="server" Width="200px">
    <ClientSideEvents EndCallback="function(s, e) {
	
               alert(s.cp_server);
          
}" />
    <PanelCollection>
<dx:PanelContent runat="server"></dx:PanelContent>
</PanelCollection>
</dx:ASPxCallbackPanel>



            

