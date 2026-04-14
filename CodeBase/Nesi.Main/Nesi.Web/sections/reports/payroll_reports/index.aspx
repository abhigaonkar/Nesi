<%@ Page Language="C#" MasterPageFile="../../../IntraDefault.master" AutoEventWireup="true" Inherits="Payroll_Report" Title="NE:Payroll Report" Codebehind="index.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
		<asp:SqlDataSource ID="Users" runat="server"></asp:SqlDataSource>
	</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script>

					function report_E1(business_unit_id)
						{
						var url			= "./index.aspx?a=xml_E1&business_unit_id="+business_unit_id;
						$.ajax(
							{
							type: "GET",
							url: url,
							dataType: "xml",
							success:function(xml)
								{
								var E1_rowset								= $("#E1_rowset");
								E1_rowset.html("<thead><tr><th>First Name</th><th>Last Name</th></tr></thead>");
								$(xml).find('membertype').each(
									function() {
										console.log(this);
										var type							= $(this).attr("type");
										var count							= $(this).attr("count");
										E1_rowset.append("<tr><td colspan='2' class='E1_rowhead'><div style='float:left;' class='type'>"+type+"</div><div style='float:right;clear:both;' class='count'>("+count+")</div></td></tr>");
										$(this).find('member').each(
											function()
												{
												var member_id				= $(this).attr("member_id");
												var first_name				= $(this).attr("first_name");
												var last_name				= $(this).attr("last_name");
												E1_rowset.append("<tr class='E1_rowitem'><td>"+first_name+"</td><td>"+last_name+"</td></tr>");
												});
										});
								}
							});
						}

					function report_E2(business_unit_id)
						{
						var url			= "./index.aspx?a=xml_E2&business_unit_id="+business_unit_id;
						$.ajax(
							{
							type: "GET",
							url: url,
							dataType: "xml",
							success:function(xml)
								{
								var E2_rowset								= $("#E2_rowset");
								E2_rowset.html("<thead><tr><th>First Name</th><th>Last Name</th><th>Previous</th><th>Last Raise</th><th>Current</th></tr></thead>");
								E2_rowset.append("<tbody>");
								$(xml).find('member').each(
									function()
										{
										var firstname							= $(this).attr("firstname");
										var lastname							= $(this).attr("lastname");
										var previous							= $(this).attr("previous");
										var previous_class						= "";
										if(previous == "" || previous == 0.00)
											{
											previous							= 0.00;
											previous_class						= "class='x'";
											}
										else if(previous == "--") 
											{
											previous_class						= "class='x'";
											}
										else
											{
											previous_class						= "";
											previous							= parseFloat(previous).toFixed(2);
											}

										var last_raise							= $(this).attr("last_raise");
										var last_raise_class					= "";
										if(last_raise == "" || last_raise == "00/00/0000")
											{
											last_raise							= "00/00/0000";
											last_raise_class					= "class='x'";
											}
										else
											{
											last_raise_class					= "";
											}

										var current								= $(this).attr("current");
										var current_class						= "";
										if(current == "" || current == 0.00)
											{
											current								= 0.00;
											current_class						= "class='x'";
											}
										else if(current == "--") 
											{
											current_class						= "class='x'";
											}
										else
											{
											current								= parseFloat(current).toFixed(2);
											current_class						= "";
											}
										E2_rowset.append("<tr class='E2_rowitem'><td>"+firstname+"</td><td>"+lastname+"</td><td "+previous_class+">"+previous+"</td><td "+last_raise_class+">"+last_raise+"</td><td "+current_class+">"+current+"</td></tr>");
										});
								E2_rowset.append("</tbody>");
								$(document).ready(function(){E2_rowset.tablesorter({
								headers:
										{
										2: { sorter: 'currency'},
										4: { sorter: 'currency'}
										}
								})});
								}
							});
						}

					function report_E3(business_unit_id)
						{
						var url			= "./index.aspx?a=xml_E3&business_unit_id="+business_unit_id;
						$.ajax(
							{
							type: "GET",
							url: url,
							dataType: "xml",
							success:function(xml)
								{
								var E3_rowset								= $("#E3_rowset");
								E3_rowset.html("<thead><tr><th>First Name</th><th>Last Name</th><th>StartDate</th><th>PayType</th><th>Length</th></tr></thead>");
								E3_rowset.append("<tbody>");
								$(xml).find('member').each(
									function()
										{
										var firstname							= $(this).attr("firstname");
										var lastname							= $(this).attr("lastname");
										var startdate							= $(this).attr("startdate");
										var paytype								= $(this).attr("paytype");
										var paytype_class						= "";
										if(paytype == "-")
											{
											paytype_class						= "class='x'";
											}
										else
											{
											paytype_class						= "";
											}
										var length								= $(this).attr("length");
										
										E3_rowset.append("<tr class='E3_rowitem'><td>"+firstname+"</td><td>"+lastname+"</td><td>"+startdate+"</td><td "+paytype_class+">"+paytype+"</td><td>"+length+" yr</td></tr>");
										});
								E3_rowset.append("</tbody>");
								$(document).ready(function(){E3_rowset.tablesorter({
								headers:
										{
										4: { sorter: 'currency'}
										}
								})});
								}
							});
						}
</script>
		<table cellpadding="5" cellspacing="0" id='report_payroll'>
			<tr>
				<td>
					<div id='reports' runat="server" class='reports'></div>
					<div id='report' runat="server" class='report'></div>
				</td>
			</tr>
		</table>
</asp:Content>
