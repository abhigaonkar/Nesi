<%@ Page Language="C#" AutoEventWireup="true" Theme="" Inherits="dashboard_modules_headcount" Codebehind="headcount.aspx.cs" %>
<%@ OutputCache Duration="60" VaryByParam="DXRefresh;business_unit_id" %>


<%@ Register assembly="DevExpress.XtraCharts.v19.2.Web, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link type="text/css" href="/css/dashboard.css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    <table style="width:100%;">
		<tr>
			<td>
						
							<div class='title' id="bm_headcount_title" style="width:99%" runat="server">Headcount</div>
							
                <dxchartsui:WebChartControl ID="chart_wo" runat="server" Height="190px" Width="1000px" PaletteName="Module" SideBySideBarDistanceFixed="100" SideBySideBarDistanceVariable="50" BinaryStorageMode="Session" CrosshairEnabled="True">
									<fillstyle>
										<optionsserializable>
											<cc1:SolidFillOptions />
										</optionsserializable>
									</fillstyle>
									<padding bottom="0" left="0" right="0" top="0" />
		<Padding Left="0" Top="0" Right="0" Bottom="0"></Padding>
		                            <DiagramSerializable>
                                        <cc1:XYDiagram>
                                            <AxisX VisibleInPanesSerializable="-1">
                                            </AxisX>
                                            <AxisY VisibleInPanesSerializable="-1">
                                            </AxisY>
                                        </cc1:XYDiagram>
                                    </DiagramSerializable>
									<legend font="Tahoma, 8pt, style=Bold"></legend>
									<SeriesSerializable>
                                        <cc1:Series Name="Headcount">
                                            <ViewSerializable>
                                                <cc1:LineSeriesView>
                                                </cc1:LineSeriesView>
                                            </ViewSerializable>
                                        </cc1:Series>
                                       
                                        <cc1:Series Name="Trucks">
                                            <ViewSerializable>
                                                <cc1:LineSeriesView>
                                                </cc1:LineSeriesView>
                                            </ViewSerializable>
                                        </cc1:Series>
                                       
                                    </SeriesSerializable>
									<seriestemplate>
										<viewserializable>
											<cc1:SideBySideBarSeriesView>
											</cc1:SideBySideBarSeriesView>
										</viewserializable>
										<labelserializable>
											<cc1:SideBySideBarSeriesLabel LineVisible="True">
												<fillstyle>
													<optionsserializable>
														<cc1:SolidFillOptions />
													</optionsserializable>
												</fillstyle>
												<pointoptionsserializable>
													<cc1:PointOptions>
													</cc1:PointOptions>
												</pointoptionsserializable>
											</cc1:SideBySideBarSeriesLabel>
										</labelserializable>
										<legendpointoptionsserializable>
											<cc1:PointOptions>
											</cc1:PointOptions>
										</legendpointoptionsserializable>
									</seriestemplate>
								</dxchartsui:WebChartControl>
							
						
    		</td>
		</tr>
		
	</table>
	<br />
    </form>
</body>
</html>
