<%@ Control Language="C#" AutoEventWireup="true"  Inherits="modules_crash_log" EnableTheming="True" Codebehind="crash_log.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

			<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
				SelectCommand="
                
                SELECT
error_log.error_origin_querystring.queryString_string,
error_log.error_machine_name.machine_name,
error_log.error.error_query,
error_log.error_origin.origin_path,
error_log.error_useragent.userAgent_string,
error_log.error.main_id,
error_log.error.error_timestamp,
m.member_fullname as member_id,
h.host_name Host,
error_log.error_userip.userIP_address,
concat(error_log.error.error_origin_id,'-',
error_log.error.error_message_id) _master_id
FROM
error_log.error
left join neintranet.member m                   ON m.member_id = error.member_id
LEFT JOIN error_log.error_origin_querystring    ON error_log.error.error_origin_queryString_id = error_log.error_origin_querystring.queryString_id
LEFT JOIN error_log.error_machine_name          ON error_log.error.error_machine_id = error_log.error_machine_name.machine_id
LEFT JOIN error_log.error_origin                ON error_log.error.error_origin_id = error_log.error_origin.origin_id
LEFT JOIN error_log.error_useragent             ON error_log.error.error_userIP_id = error_log.error_useragent.userAgent_id
LEFT JOIN error_log.error_userip                ON error_log.error.error_userIP_id = error_log.error_userip.userIP_id            
left join error_log.error_host h                ON h.host_id = error.error_host_id
              
where
concat(error_log.error.error_origin_id,'-',
error_log.error.error_message_id) = ?_id
AND error.error_is_local = false
AND h.host_name != 'jordan.nesi.ca'
AND h.host_name != 'andy.nesi.ca'
AND h.host_name != 'matt.nesi.ca'
AND h.host_name != 'iain.nesi.ca'
order by error.error_timestamp desc
                
                ">
				<SelectParameters>
					<asp:SessionParameter Name="_id" SessionField="_master_ID" />
				</SelectParameters>
			</asp:SqlDataSource>
	<dx:ASPxGridView ID="gv_crashLog" runat="server" ClientInstanceName="gv_crashLog" Width ="100%"
				 AutoGenerateColumns="False" 
				oncustomcallback="gv_CustomCallback" 
				oncustomjsproperties="gv_CustomJSProperties"  
				KeyFieldName="_master_id" PreviewFieldName="ferror_stackTrace"
				onhtmldatacellprepared="gv_HtmlRowPrepared">

		<Columns>
			<dx:GridViewDataDateColumn FieldName="dt" VisibleIndex="1" Caption="Date">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm:ss">
				</PropertiesDateEdit>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn FieldName="member_id" Visible="False" 
				VisibleIndex="2">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="member_fullname" VisibleIndex="4"  Caption="Member Name">
			</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="error_query" VisibleIndex="7"     Visible="False">
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="error_stackTrace" VisibleIndex="8"    Visible="False">
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="ferror_stackTrace" VisibleIndex="16"    Visible="False" >
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="host_name" VisibleIndex="3" width="200px" Caption="Host">
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="message_text" VisibleIndex="9" Visible="False" >
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="origin_path" VisibleIndex="10" Visible="False" >
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="queryString_string" VisibleIndex="11" Visible="False" >
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="userIP_address" VisibleIndex="12" Visible="False" >
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="error_count" VisibleIndex="5" Caption="Crash Count">
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="total_error_count" VisibleIndex="6" Caption="Total Crashes" ToolTip="80px">
				<CellStyle Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="id" FieldName="id" Visible="False" 
				VisibleIndex="13">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="id" FieldName="_master_id" Visible="False" 
				VisibleIndex="14">
			</dx:GridViewDataTextColumn>
			
			<dx:GridViewDataTextColumn Caption="Is global" FieldName="error_is_global" Visible="False" 
				VisibleIndex="15">
			</dx:GridViewDataTextColumn>
			
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" AllowGroup="False" />
		<SettingsPager Visible="False">
        </SettingsPager>
		<Settings ShowPreview="true" ShowTitlePanel="True" GridLines="Horizontal" />
		<SettingsText Title="Words" />
		<SettingsLoadingPanel ImagePosition="Top" ShowImage="False" Mode="Disabled" />
		<SettingsDetail ShowDetailRow="True" />
		<SettingsSearchPanel Visible="false" />
		<SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
		<Templates>
			<DetailRow>
				<dx:ASPxGridView ID="gv_detail" runat="server" Theme="NETheme01" ClientInstanceName="gv_detail"
					Width="100%" DataSourceID="SqlDataSource2" AutoGenerateColumns="False" 
					KeyFieldName="main_id" onbeforeperformdataselect="gv_detail_BeforePerformDataSelect">
					<ClientSideEvents RowClick="function(s, e) {

                            var key = s.GetRowKey(e.visibleIndex);
                            //alert('Last Key = ' + key);
                            // console.log('Last Key = ' + key);
                        

	window.open('/sections/reports/nesi_error_log/info.aspx?id=' + key,'','width=1150,height=800');
                     

}" />
					<Columns>
						<dx:GridViewDataTextColumn Caption="Query String" FieldName="queryString_string" VisibleIndex="2">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="machine_name" Caption="Machine Name" VisibleIndex="1" Visible="false">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="origin_path" Caption="Page" VisibleIndex="3" Visible="False">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="userAgent_string" Caption="UserAgent" VisibleIndex="4" Visible="False">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="main_id" ReadOnly="True" VisibleIndex="5" Visible="false">
							<EditFormSettings Visible="False" />
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="member_id" Caption="Member Name" VisibleIndex="0">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="userIP_address" Caption="IP Address"  VisibleIndex="6">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="_master_id" VisibleIndex="7" Visible="false">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="error_timestamp" Caption="Time" VisibleIndex="8">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="Host" Caption="Host" VisibleIndex="9">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="error_query" Caption="query" VisibleIndex="10">
						</dx:GridViewDataTextColumn>
                       
					</Columns>
                    <Border BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
				</dx:ASPxGridView>
				
			</DetailRow>
		</Templates>
	    <Border BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
	</dx:ASPxGridView>
	<dx:ASPxTimer ID="crashTimer" runat="server" Interval="20000" 
				ClientSideEvents-Tick='function (s,e){gv_crashLog.PerformCallback("refresh");}' 
				ClientInstanceName="timer" Enabled="False">
	</dx:ASPxTimer>


            
