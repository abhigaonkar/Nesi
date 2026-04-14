<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="" Theme="" CodeBehind="invoice_service.aspx.cs" Inherits="invoice_service_root_page" Title="Task Scheduler" %>
<!doctype html>

<html lang="en">
<head>
	<meta charset="utf-8">
	<title>Task Scheduler</title>
	<meta name="description" content="Task Scheduler">
	<meta name="author" content="Spark Power - Matt Hyde">
	<style>
		body {
			margin: 0 auto;
			max-width: 60%;
			padding: 1em 0;
			font-family: "segoe ui";
		}
		.timer  {
			min-height: 80px;
			text-align: center;
			padding: 15px 0;
		}
		.timer .time {
			font-size: 2em;
		}
		.timer .subtext {
			font-size: 0.75em;
			opacity: 0.5;
		}
		.grid {
			/* Grid Fallback */
			display: flex;
			flex-wrap: wrap;
  
			display: grid;
			grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
			grid-auto-rows: minmax(150px, auto);
			grid-gap: 1em;
		}

		.module {
			align-items: center;
			justify-content: center;
			height: 200px;
			cursor: pointer;
			margin-left: 5px;
			margin-right: 5px;
			flex: 1 1 200px;
		}
		.module.on {
			border: solid 1px #090;
		}
		.module.off {
			border: solid 1px #999;
			
		}
			
		.module .title {
			color: #fff;
			width: 100%;
			height:10%;
			font-size: 1em;
			text-align: center;
			padding: 5px 0;
		}
		
		.module.disabled {
			-ms-opacity: 0.25;
			opacity: 0.25;
		}

		.module .title.on {
			background-color: #090;
		}
		
		.module .title.off {
			background-color: #999;
		}

		.module .title input{
			float: left;
		}
		.module .status {
			width: 100%;
			height:85%;
			background-color: #eee;
			position: relative;
		}
		.module .status button {
			width: 50px;
			height: 20px;
			position: absolute;
			left: 50%;
			top: 50%;
			margin: -30px 0 0 -25px;
		}
		.module .status .updated {
			position: absolute;
			text-align: center;
			font-size: 1em;
			margin-top: 20%;
			color: #090;
			top: 0;
			width:100%;
		}
		.module .status .lastran {
			position: absolute;
			text-align: center;
			font-size: 0.85em;
			margin-top: 50%;
			color: #777;
			top: 0;
			width:100%;
		}
		.module .status .total {
			position: absolute;
			text-align: center;
			font-size: 1.75em;
			margin-top: 50%;
			top: -40px;
			width:100%;
		}
		.module .status .running {
			border: 8px solid #ccc;
			border-top: 8px solid #309144; 
			border-radius: 50%;
			width: 50px;
			height: 50px;
			position: absolute;
			left: 50%;
			top: 50%;
			margin: -30px 0 0 -30px;
			animation: spin 2s linear infinite;
		}

		@keyframes spin {
			0% { transform: rotate(0); }
			100% { transform: rotate(360deg); }
		}

		@supports (display: grid) {
			.module {
				margin: 0;
			}
		}
		
	</style>
	<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
</head>

<body>
	<form id="taskform" runat="server">
		<input type="hidden" id="token" class="token" runat="server" />
		<input type="hidden" id="access_key" value="HS6iAKRDSg6wR49EC5M8YbnqMmwtMlNFlBUVJemF" runat="server" class="access_key" />
		<div class="timer" onclick="task_scheduler.timer.reset();">
			<div class="time"></div>
			<div class="subtext"></div>
		</div>
		<div class="grid">
			<div class="module" data-method="move_invoices">
				<div class="title"><input type="checkbox" />Move Invoices</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>Check Open Payrolls</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>Send Approval Emails</div>
				<div class="status"></div>
			</div>
			<div class="module" data-method="update_offers">
				<div class="title"><input type="checkbox"/>Update Offers</div>
				<div class="status"></div>
			</div>
			<div class="module" data-method="clean_properties">
				<div class="title"><input type="checkbox"/>Clean Properties</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>Customer Rates Warnings</div>
				<div class="status"></div>
			</div>
			<div class="module" data-method="missing_csps">
				<div class="title"><input type="checkbox"/>Missing CSP's</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>Termination Reminders</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>Send TS Watch</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>TS Reminders</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>Auto Reports</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>Daily PO Report</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>WO Proc History</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>Collection Notes</div>
				<div class="status"></div>
			</div>
			<div class="module">
				<div class="title"><input type="checkbox"/>Employee Status Emails</div>
				<div class="status"></div>
			</div>
            <div class="module" data-method="inactivate_closedpo_lines">
				<div class="title"><input type="checkbox"/>Inactivate Closed PO lines</div>
				<div class="status"></div>
			</div>
		</div>
		<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
		<script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
		<script src="/js/functions.js"></script>
		<script src="/js/task_scheduler.js"></script>
		<script>
			document.addEventListener("DOMContentLoaded", function(event) { 
				task_scheduler.run();
				});
		</script>
	</form>
</body>
</html>