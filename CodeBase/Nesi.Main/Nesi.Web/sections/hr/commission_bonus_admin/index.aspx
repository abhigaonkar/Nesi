<%@ Page Language="C#" MasterPageFile="../../../IntraDefault.master" AutoEventWireup="true" Inherits="commission_bonus_admin_index" Title="NE:Bonuses & Commissions" Codebehind="index.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
		<asp:SqlDataSource ID="Users" runat="server"></asp:SqlDataSource>
	</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server"></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
		<script type="text/javascript">
			function chk_request()
				{
				var type			= $('#type_select').val();
				var member_id		= $('#member_select').val();
				var payperiod_id	= $('#payperiod_select').val();
				var amount			= $('.form input').val();
				var note			= $('.form textarea').val();
				var validate		= 0;
				if(type != 'B' && type != 'C' && type != 'M')
					{validate--;}
				if(member_id == '0')
					{validate--;}
				if(payperiod_id == '0')
					{validate--;}
				if(amount <= 0 || amount.length == 0)
					{validate--;}
				if(note.length == 0)
					{validate--;}
				if(validate < 0)
					{
					$('.s .save').attr('disabled', true);
					}
				else
					{
					$('.s .save').attr('disabled', false);
					}
				}
				
			
			$.fn.clearForm = function()
								{
								return this.each(function()
									{
									var type = this.type, tag = this.tagName.toLowerCase();
									if (type == 'text' || type == 'password' || tag == 'textarea')
										{
										this.value = '';
										}
									else if (type == 'checkbox' || type == 'radio')
										{
										this.checked = false;
										}
									else if (tag == 'select')
										{
										this.selectedIndex = 0;
										}
									else if (tag == 'button' && $(this).attr('class') == 'save')
										{
										$(this).attr('disabled', true);
										}
									if($(this).attr('id') == 'member_select')
										{
										$(this).attr('disabled', true);
										}
									switch($('#save_type').val())
										{
										case "new":		$('.form .save').text("SEND REQUEST");
														$('.form table th').html("NEW REQUEST");
										break;
										case "edit":	$('.form .save').text("EDIT REQUEST");
														$('.form table th').html("EDIT REQUEST");
										break;
										}
								});
							};
				
			function save_request()
				{
				var type			= $('#type_select').val();
				var member_id		= $('#member_select').val();
				var payperiod_id	= $('#payperiod_select').val();
				var amount			= $('.form input').val();
				var request_id		= $('#request_id').val();
				var note			= escape($('.form textarea').val());
				var save_type		= $('#save_type').val();
				$.get(	"./index.aspx",	
						{
						a:				"save",
						type:			type,
						member_id:		member_id,
						payperiod_id:	payperiod_id,
						amount:			amount,
						save_type:		save_type,
						note:			note,
						request_id:		request_id
						},
						function(data)
							{
							if(data == "SUCCESS")
								{
								$('.form *').clearForm();
								$('.form #type_select').focus();
								get_requests();
								}
							else
								{
								alert(data);
								}
							});
				}
				
			function edit_request(request_id)
				{
				$.ajax(
					{
					type: "GET",
					url: './index.aspx?a=xml_request_detail&request_id='+request_id,
					dataType: "xml",
					beforeSend: function()
									{
									$('#save_type').val('edit');
									$('#request_id').val(request_id);
									$('.requests').clearForm('');
									},
					success:function(xml)
						{
						$(xml).find('request').each(
							function()
								{
								var type					= $(this).attr("type");
								var payperiod_id			= $(this).attr("payperiod_id");
								var member_id				= $(this).attr("member_id");
								var business_unit_id				= $(this).attr("business_unit_id");
								var amount					= $(this).attr("amount");
								var note					= $(this).attr("note");
								$('.form #type_select').val(type);
								$('.form #company_select').val(business_unit_id);
								get_memberlist(business_unit_id, member_id);
								$('.form textarea').val(unescape(note));
								$('.form #payperiod_select').val(payperiod_id);
								$('.form input').val(amount);
								});
						},
					complete: function()
								{
								}			
					});
				}
			var is_processing		= false;

			function handle_request(_obj, _type, _id)
				{
				var ts = $(_obj).attr("data-ts");
				if(is_processing)
					{
					return;
					}
				else
					{
					is_processing		= true;
					}
				$.get(	"./index.aspx",
						{
						a:				"handle",
						request_id:		_id,
						ts: ts,
						type: _type
						},
						function(data)
							{
							if(data == "SUCCESS")
								{
								get_requests();
								}
							else
								{
								alert(data);
								get_requests();
								}
							is_processing		= false;
							});
				}
				
				
			function get_requests()
				{
				var o					= "<div class='h'>PENDING REQUESTS</div><table cellspacing='0' cellpadding='5' width='100%'><thead><th>TYPE</th><th>MEMBER ID</th><th>DATE REQ</th><th>PPID</th><th>PAY PERIOD</th><th>AMOUNT</th><th>&nbsp;</th></thead><tbody>";
				var count				= 0;
				$.ajax(
					{
					type: "GET",
					url: './index.aspx?a=xml_requests',
					dataType: "xml",
					beforeSend: function()
									{
									$('.requests').html('');
									clearInterval();
									},
					success:function(xml)
						{
						$(xml).find('request').each(
							function()
								{
								var id							= $(this).find("id").text();
								var type						= $(this).find("type").text();
								switch(type)
									{
									case "B": type				= "Bonus";
									break;
									case "C": type				= "Commission";
									break;
									case "R": type				= "Referral";
									break;
									}
								var payperiod_id				= $(this).find("payperiod_id").text();
								var payperiod_daterange			= $(this).find("payperiod_daterange").text();
								var member_name					= $(this).find("member_name").text();
								var requested_when				= $(this).find("requested_when").text();
								var approved					= $(this).find("approved").text();
								var approved_when				= $(this).find("approved_when").text();
								var ts							= $(this).find("ts").text();
								var disabled					= "";
								if(approved == -1)
									{
									approved					= "Not Approved";
									disabled					= "";
									}
								else
									{
									approved					= approved_when;
									disabled					= " disabled";
									}
								var amount						= $(this).find("amount").text();
								var note						= $(this).find("note").text();
								o += "<tr><td width='100'><b>"+type+"</b></td><td>"+member_name+"<div class='tip'>"+note+"</div></td><td width='100'>"+requested_when+"</td><td width='50'>"+payperiod_id+"</td><td width='175'><b>"+payperiod_daterange+"</b></td><td width='100'>"+amount+"</td><td width='65'><button type='button' data-ts='"+ts+"' onclick='handle_request(this, 0, "+id+");' "+disabled+"><img src='/images/icon/icon[deny].gif' /></button><button type='button' data-ts='"+ts+"' onclick='handle_request(this, 1, "+id+");' "+disabled+"><img src='/images/icon/icon[approve].gif' /></button></td></tr>";
								});
						if($(xml).find('request').length == 0)
							{
							o			+= "<tr><td colspan='7' style='padding:50px;background-color:#fff;text-align:center;'>No Requests Pending</td></tr>";
							}
						count		= $(xml).find('request').length;
						row_count	= count;
						},
					complete: function()
								{
								o						+= "</tbody></table>";
								$(o).appendTo('.requests').hide().fadeIn();
								if(count > 0)
									{
									$('.requests table').tablesorter();
									$('.requests table tbody > tr').each(function(){$(this).tip()});
									}
								//setInterval('chk_requests()', 5000);
								}			
					});
				}
			var row_count		= 0;
			function chk_requests()
				{
				$.ajax(
					{
					type: "GET",
					url: './index.aspx?a=xml_requests',
					dataType: "xml",
					success:function(xml)
						{
						if($(xml).find('request').length != row_count)
							{
							get_requests();
							}
						count		= $(xml).find('request').length;
						}		
					});
				}
			function get_memberlist(business_unit_id, member_id)
				{
				var url			= "./index.aspx?a=xml_member&business_unit_id="+business_unit_id;
				$.ajax(
					{
					type: "GET",
					url: url,
					dataType: "xml",
					success:function(xml)
						{
						var detail								= $("#detail");
						detail.html("");
						var member								= $("#member_select");
						member.attr("disabled", false);
						member[0].options.length			= 0;
						member.append("<option value='0'>Employee</option");
						$(xml).find('member').each(
							function()
								{
								var id							= $(this).attr("id");
								var name						= $(this).attr("name");
								var selected					= "";
								if(id == member_id)
									{
									selected					= "selected";
									}
								else
									{
									selected					= "";
									}
								member.append("<option value='"+id+"'"+selected+">"+name+"</option>");
								});
						}
					});
				}
		</script>
		<div id="outer" class="payroll_bonuses" Runat="Server" align="center">

		</div>
</asp:Content>
