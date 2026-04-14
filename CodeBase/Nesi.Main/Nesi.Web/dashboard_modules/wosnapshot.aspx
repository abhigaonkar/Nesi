<%@ Page Language="C#" AutoEventWireup="true" Theme="" Inherits="dashboard_modules_wosnapshot" Codebehind="wosnapshot.aspx.cs" %>
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
						<div class="tile g clickable graph" id="bm_tile_workorders" runat="server">
							<div class='title' id="bm_workorders_title" runat="server">WO Snap Shot</div>
							<div class='content' id="bm_workorders_content" runat="server">
								<dxchartsui:WebChartControl ID="chart_wo" runat="server" Height="200px" Width="450px" PaletteName="Module" SideBySideBarDistanceFixed="100" SideBySideBarDistanceVariable="50" SideBySideEqualBarWidth="True" BinaryStorageMode="Session">
									<padding bottom="0" left="0" right="0" top="0" />
		<Padding Left="0" Top="0" Right="0" Bottom="0"></Padding>
									<diagramserializable>
										<cc1:XYDiagram3D PerspectiveAngle="45" PlaneDepthFixed="5" SeriesDistanceFixed="10" VerticalScrollPercent="5" ZoomPercent="125" RotationOrder="XZY" RotationType="UseAngles">
											<axisx>
												<range sidemarginsenabled="True" />
												<label visible="False" />
		<Range SideMarginsEnabled="True"></Range>
											</axisx>
											<axisy>
												<range sidemarginsenabled="True" />
		<Range SideMarginsEnabled="True"></Range>
											</axisy>
										</cc1:XYDiagram3D>
									</diagramserializable>
									<fillstyle>
										<optionsserializable>
											<cc1:SolidFillOptions />
										</optionsserializable>
									</fillstyle>
									<legend font="Tahoma, 8pt, style=Bold"></legend>
									<seriesserializable>
										<cc1:Series Name="Open" SynchronizePointOptions="False" SeriesPointsSorting="Descending" SeriesPointsSortingKey="Value_1" LabelsVisibility="true">
											<points>
												<cc1:SeriesPoint ArgumentSerializable="Open" Values="148">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="PM Approval" Values="2">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="BM Approval" Values="9">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Initial Prep" Values="5">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Questions" Values="7">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Rework" Values="1">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Waiting Invoicing" Values="1">
												</cc1:SeriesPoint>
											</points>
											<viewserializable>
												<cc1:SideBySideBar3DSeriesView BarDepth="1" BarDepthAuto="False" BarWidth="0.85" ColorEach="True">
												</cc1:SideBySideBar3DSeriesView>
											</viewserializable>
											<labelserializable>
												<cc1:Bar3DSeriesLabel Font="Tahoma, 8pt, style=Bold" LineVisible="True">
													<fillstyle>
														<optionsserializable>
															<cc1:SolidFillOptions />
														</optionsserializable>
													</fillstyle>
													<pointoptionsserializable>
														<cc1:PointOptions>
														</cc1:PointOptions>
													</pointoptionsserializable>
												</cc1:Bar3DSeriesLabel>
											</labelserializable>
											<legendpointoptionsserializable>
												<cc1:PointOptions PointView="Argument">
												</cc1:PointOptions>
											</legendpointoptionsserializable>
										</cc1:Series>
									</seriesserializable>
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
							</div>
						</div>
    		</td>
		</tr>
		<tr>
			<td>
			<div>
						<div class="tile g clickable graph" id="bm_tile_purchaseorders" runat="server">
							<div class='content' id="bm_purchaseorders_content" runat="server">
							
							<div class='title' id="bm_purchaseorders_title" runat="server">PO Snap Shot</div>
								<dxchartsui:WebChartControl ID="chart_po" runat="server" Height="200px" PaletteName="Module" SideBySideBarDistanceFixed="100" SideBySideBarDistanceVariable="50" SideBySideEqualBarWidth="True" Width="450px" BinaryStorageMode="Session">
									<padding bottom="0" left="0" right="0" top="0" />
		<Padding Left="0" Top="0" Right="0" Bottom="0"></Padding>
									<smallcharttext font="Tahoma, 12pt, style=Bold" />
									<diagramserializable>
										<cc1:XYDiagram3D PerspectiveAngle="45" PlaneDepthFixed="5" SeriesDistanceFixed="10" VerticalScrollPercent="5" ZoomPercent="125" RotationOrder="XYZ" RotationType="UseAngles">
											<axisx>
												<range sidemarginsenabled="True" />
												<label visible="False" />
		<Range SideMarginsEnabled="True"></Range>
											</axisx>
											<axisy>
												<range sidemarginsenabled="True" />
		<Range SideMarginsEnabled="True"></Range>
											</axisy>
										</cc1:XYDiagram3D>
									</diagramserializable>
									<fillstyle>
										<optionsserializable>
											<cc1:SolidFillOptions />
										</optionsserializable>
									</fillstyle>
									<legend font="Tahoma, 8pt, style=Bold"></legend>
									<seriesserializable>
										<cc1:Series Name="Open" SynchronizePointOptions="False" SeriesPointsSorting="Descending" SeriesPointsSortingKey="Value_1" LabelsVisibility="true">
											<points>
												<cc1:SeriesPoint ArgumentSerializable="Not Issued" Values="9">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Waiting PM Approval" Values="0">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Awaiting Packing Slip" Values="31">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Approved to Order" Values="0">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Waiting to Be Closed" Values="0">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Closed-Paid" Values="0">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Questions" Values="0">
												</cc1:SeriesPoint>
											</points>
											<datafilters>
												<cc1:DataFilter />
											</datafilters>
											<viewserializable>
												<cc1:SideBySideBar3DSeriesView BarDepth="1" BarDepthAuto="False" BarWidth="0.85" ColorEach="True">
												</cc1:SideBySideBar3DSeriesView>
											</viewserializable>
											<labelserializable>
												<cc1:Bar3DSeriesLabel Font="Tahoma, 8pt, style=Bold" LineVisible="True" ShowForZeroValues="True" TextAlignment="Far">
													<fillstyle>
														<optionsserializable>
															<cc1:SolidFillOptions />
														</optionsserializable>
													</fillstyle>
													<pointoptionsserializable>
														<cc1:PointOptions>
														</cc1:PointOptions>
													</pointoptionsserializable>
												</cc1:Bar3DSeriesLabel>
											</labelserializable>
											<legendpointoptionsserializable>
												<cc1:PointOptions PointView="Argument">
												</cc1:PointOptions>
											</legendpointoptionsserializable>
										</cc1:Series>
									</seriesserializable>
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
							</div>
				</div>
		</tr>
		<tr>
			<td>
				<div>
						<div class="tile g clickable graph" id="bm_tile_quotes" runat="server">
							<div class='title' id="bm_quotes_title" runat="server">Quote Snapshot</div>
							<div class='content' id="bm_quotes_content" runat="server">
								<dxchartsui:WebChartControl ID="chart_quote" runat="server" Height="200px" Width="450px" BinaryStorageMode="Session">
									<diagramserializable>
										<cc1:SimpleDiagram3D RotationOrder="XYZ" RotationType="UseAngles" ZoomPercent="175">
										</cc1:SimpleDiagram3D>
									</diagramserializable>
									<fillstyle fillmode="Gradient">
										<optionsserializable>
											<cc1:RectangleGradientFillOptions />
										</optionsserializable>
									</fillstyle>
									<legend font="Tahoma, 8pt, style=Bold"></legend>
									<seriesserializable>
										<cc1:Series Name="Series 1" SynchronizePointOptions="False">
											<points>
												<cc1:SeriesPoint ArgumentSerializable="Waiting to be Quoted" SeriesPointID="0" Values="10">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Waiting to be Sent" SeriesPointID="2" Values="10">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Awaiting Verification" SeriesPointID="3" Values="10">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Awaiting Approval" SeriesPointID="4" Values="10">
												</cc1:SeriesPoint>
												<cc1:SeriesPoint ArgumentSerializable="Po Received" SeriesPointID="5" Values="10">
												</cc1:SeriesPoint>
											</points>
											<viewserializable>
												<cc1:Pie3DSeriesView Depth="25" ExplodedDistancePercentage="20" ExplodeMode="MaxValue">
												</cc1:Pie3DSeriesView>
											</viewserializable>
											<labelserializable>
												<cc1:Pie3DSeriesLabel Font="Tahoma, 8pt, style=Bold" LineVisible="True">
													<fillstyle>
														<optionsserializable>
															<cc1:SolidFillOptions />
														</optionsserializable>
													</fillstyle>
													<pointoptionsserializable>
														<cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
<ArgumentNumericOptions Format="General"></ArgumentNumericOptions>

<ValueNumericOptions Format="General"></ValueNumericOptions>
														</cc1:PiePointOptions>
													</pointoptionsserializable>
												</cc1:Pie3DSeriesLabel>
											</labelserializable>
											<legendpointoptionsserializable>
												<cc1:PiePointOptions PercentOptions-ValueAsPercent="False" PointView="Argument">
<ArgumentNumericOptions Format="General"></ArgumentNumericOptions>

<ValueNumericOptions Format="Percent"></ValueNumericOptions>
												</cc1:PiePointOptions>
											</legendpointoptionsserializable>
											<topnoptions enabled="True" />

		<TopNOptions Enabled="True"></TopNOptions>
										</cc1:Series>
									</seriesserializable>
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
<ArgumentNumericOptions Format="General"></ArgumentNumericOptions>

<ValueNumericOptions Format="General"></ValueNumericOptions>
													</cc1:PointOptions>
												</pointoptionsserializable>
											</cc1:SideBySideBarSeriesLabel>
										</labelserializable>
										<legendpointoptionsserializable>
											<cc1:PointOptions>
<ArgumentNumericOptions Format="General"></ArgumentNumericOptions>

<ValueNumericOptions Format="General"></ValueNumericOptions>
											</cc1:PointOptions>
										</legendpointoptionsserializable>
									</seriestemplate>

<CrosshairOptions ArgumentLineColor="222, 57, 205" ValueLineColor="222, 57, 205"><CommonLabelPositionSerializable>
<cc1:CrosshairMousePosition></cc1:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<cc1:ToolTipMousePosition></cc1:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
								</dxchartsui:WebChartControl>
							</div>
						</div>
    </div></td>
		</tr>
	</table>
    <div id="tickets" runat="server" style="margin-top:5px;"></div>
    <div id="anniversaries" runat="server" style="margin-top:5px;"></div>
	<br />
    </form>
</body>
</html>
