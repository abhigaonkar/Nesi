<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="opportunity_import.aspx.cs" Theme="" MasterPageFile="" Inherits="Nesi.Web.sections.customer.opportunity_import" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<style>
		html,body {
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
			font-size: 0.75em;
			font-weight: normal !important;
			width: 280px;
		}
		#content .instructions .fieldname {
			font-size: 0.8em;
			display: block;
			margin-left: 10px;
		}
		#divResults {
			font-family: Roboto, Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
			font-size: 0.85em;
			position: absolute;
			left: 350px;
			top: 50px;
		}
		#divResults #errors {
			width: 100%;
			border-bottom: solid 1px #35B86B;
		}
		#divResults #errors th {
			background-color: #35B86B;
			color: #fff;
			font-weight: normal !important;
		}
		#divResults #errors td {
			font-weight: normal !important;
		}
		#divResults #errors tr:nth-of-type(even) {
			background-color: #f2f2f2;
		}
		#divResults #errors tr:nth-of-type(odd) {
			background-color: #fff;
		}
		#divResults #errors .row_n {
			width: 75px;
			text-align: center;
			border-left: solid 1px #35B86B;
		}
		#divResults #errors .text {
			width: 350px;
			text-align: left;
			border-left: solid 1px #35B86B;
		}
		#divResults #errors .value {
			width: 50px;
			text-align: center;
			border-left: solid 1px #35B86B;
			border-right: solid 1px #35B86B;
		}
		#divResults .scroll {
			width: 700px;
			max-height: 700px;
			overflow-y: scroll;
		}
		#divResults .success {
			width: 100%;
			text-align: center;
			padding-bottom:25px;
			font-weight: normal !important;
		}
		#divResults .success .thumbs {
			width: 100%;
			font-size: 3em;
			display:block;
			text-align: center;
		}

		</style>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/table2excel/table2excel.js"></script>
	<script type="text/javascript">
		function uploadFile(s,e)
			{
			if(upControl.GetText() != "")
				{
				upControl.Upload();
				btProcess.SetEnabled(false);
				}
			else
				{
				alert("Please provide a csv file to process.");
				}
			} 
		function excelFile()
			{
			$("#errors").table2excel({
				name: "Opportunity Import Exceptions",
				filename: "ExceptionsReport-"+Date.now(),
				fileext: ".xls",
				exclude_img:true,
				exclude_links:true,
				exclude_inputs:true
				});
			}
		function fileComplete(s,e)
			{
			btProcess.SetEnabled(true);
			var results = "";
			var typeOfResponse = "";
			var shouldProcess = true;
			try
				{
				results = e.callbackData.split("\n");
				typeOfResponse = results[0].split("|")[1];
				} 
			catch (e)
				{
				shouldProcess = false;
				}
			var success = "<div class='success'><span class='thumbs'>&#128077;</span><br/>The import has processed successfully</div>";
			var hardError = "<div class='success'><span class='thumbs'>&#128078;</span><br/>The import file has issues and cannot import till it is fixed.<br/><a  href='javascript:excelFile()'>Click to download the below exception report</a></div>";
			var softError = "<div class='success'><span class='thumbs'>&#128077;</span><br/>The import succeeded, though there were references to <br/> inactive subsidiaries/customers<br/><a href='javascript:excelFile()'>Click to download the below exception report</a></div>";
			var systemFailure = "<div class='success'><span class='thumbs'>&#128078;</span><br/>The import has encountered possible system issues while processing.<br/>Please contact support.</div>";
			if(shouldProcess && e.isValid)
				{
				if (typeOfResponse == "HardErrors" || typeOfResponse == "SoftErrors")
					{
					var errorTable =	(typeOfResponse == "SoftErrors" ? softError : hardError) + "<div class='scroll'><table id='errors' cellpadding='5' cellspacing='0'>"+
										"<thead><tr>"+
											"<th class='row_n'>Row #</th>"+
											"<th class='text'>Error</th>"+
											"<th class='value'>Value/Id</th>"+
										"</tr></thead>"+
										"<tbody>";
					for(var i = 1; i < results.length; i++)
						{
						if(results[i].trim() === "") continue;
						var resp = results[i].split("|");
						var rowNumber = resp[0];
						var errorText = resp[1];
						var erroredValue = resp[2];
						errorTable +=	"<tr><td class='row_n'>"+rowNumber+
										"</td><td class='text'>"+errorText+
										"</td><td class='value'>"+erroredValue+
										"</td></tr>";
						}
					errorTable += "</tbody></table></div>";
					$("#divResults").html(errorTable);
					}
				else 
					{
					divResults.innerHTML = success;
					}
				}
			else
				{
				divResults.innerHTML = systemFailure;
				}
			}
	</script>
    <title>Opportunity Import</title>
</head>
<b>
	<div id="content">
		<form id="form1" runat="server">
			<div class="instructions">
				<b>Instructions for Importing</b><br/>
				Please only use the saved search from NetSuite.<br/>
				<br/>
				The supported format is as follows - column ordering matters:<br/>
				<ul>
					<li>Opportunity Id
						<span class="fieldname">FieldName: <i>internalId</i></span>
					<li>Opportunity Number
						<span class="fieldname">FieldName: <i>nsNumber</i></span>
					<li>Subsidiary Id
						<span class="fieldname">FieldName: <i>subsidiaryId</i></span>
					<li>Customer Id
						<span class="fieldname">FieldName: <i>customerId</i></span>
					<li>Projected Total
						<span class="fieldname">FieldName: <i>total</i></span>
					<li>Title
						<span class="fieldname">FieldName: <i>title</i></span>
				</ul>
				Only CSV files can be imported.<br/>
				<br/><br/><br/>
			</div>
        	<dx:ASPxUploadControl ID="upControl" runat="server" UploadMode="Advanced" Width="280px" ClientInstanceName="upControl" Theme="MaterialCompact" NullText="Select CSV File" ValidateRequestMode="Enabled" ShowProgressPanel="True" FileUploadMode="BeforePageLoad" OnFileUploadComplete="upControl_OnFileUploadComplete">
                <ValidationSettings AllowedFileExtensions=".csv" MaxFileCount="1" ShowErrors="True">
                </ValidationSettings>
				<ClientSideEvents FileUploadComplete="fileComplete"></ClientSideEvents>
			</dx:ASPxUploadControl>
			<br/>
			<dx:ASPxButton ID="btProcess" ClientInstanceName="btProcess" runat="server" Text="Process" Width="280px" AutoPostBack="False" Theme="MaterialCompact">
				<ClientSideEvents Click="uploadFile"></ClientSideEvents>
			</dx:ASPxButton>
			<div id="divResults"></div>

			
		</form>
	</div>
</body>
</html>
