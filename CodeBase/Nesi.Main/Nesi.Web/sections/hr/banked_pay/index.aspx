<%@ Page Language="C#" AutoEventWireup="true" Inherits="BankedPay" Codebehind="index.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>NE:BANK</title>
	<link type="text/css" href="/css/base/ui.all.css" rel="Stylesheet" />	
</head>
<body style="background-image:none;">
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script>
	
function check_hours(send)
	{
	var available_box					= $("#available_hours");
	var requested_box					= $("#requested_hours");
	var amount_available				= parseFloat(available_box.val());
	var amount_requested				= parseFloat(requested_box.val());
	if(amount_requested > amount_available)
		{
		requested_box.val("");
		alert("The number of hours requested is greater than the number of hours available.");
		requested_box.focus();
		}
	else if(amount_requested.length > 0 && isNaN(amount_requested))
		{
		requested_box.val("");
		alert("Only numbers may be requested");
		requested_box.focus();
		}
	else
		{
		if((send == "W" || send == "D") && amount_requested > 0)
			{
			if(!requested_box.val().match(/^\d+(|\.25|\.5|\.50|\.75)$/))
				{
				requested_box.val("");
				alert("Only quarter numbers may be requested");
				requested_box.focus();
				}
			else
				{
				switch(send)
					{
					case "D":	if(confirm("Are you sure you want to bank "+amount_requested+" hours?"))
									{
									location.href				= "./?action=D&hours="+amount_requested;
									return true;
									}
								else
									{
									return false;
									}
					break;
					case "W":	if(confirm("Are you sure you want to withdraw "+amount_requested+" hours?"))
									{
									location.href				= "./?action=W&hours="+amount_requested;
									return true;
									}
								else
									{
									return false;
									}
					break;
					}
				}
			}
		else
			{
			return false;
			}
		}
	}

function retract_entry(id)
	{
	if(confirm("Are you sure you want to retract this transaction?"))
		{
		location.href				= "./?action=retract&id="+id;
		}
	else
		{
		return false;
		}
	}



function hours_test(max)
	{
	var hours_target			= document.getElementById("howmany");
	var payrate_target			= document.getElementById("payrate");
	var total_target			= document.getElementById("total");
	var submit_target			= document.getElementById("submit");
	var notes_target			= document.getElementById("notes");
	var error					= 0;
	if(isNaN(hours_target.value))
		{
		hours_target.value			= "";
		error						= 1;
		}
	else if(hours_target.value != "")
		{
		if(payrate_target.value != "" && payrate_target.value != "0")
			{
			var total					= payrate_target.value * hours_target.value;
			var formatted_total			= total.toFixed(2);
			total_target.innerHTML		= "$"+formatted_total;
			}
		else
			{
			alert("There is an error with this member's pay information that needs to be corrected.");
			window.close();
			}
		}
	else
		{
		error						= 1;
		total_target.innerHTML		= "$0.00";
		}

	if(notes_target.value.length < 10)
		{
		error						= 1;
		}

	if(max != "" && max < hours_target.value)
		{
		error						= 1;
		hours_target.value			= "";
		total_target.innerHTML		= "$0.00";
		alert("You have requested to deduct more than what is available.");
		hours_target.focus();
		}

	if(error != 1)
		{
		submit_target.disabled		= false;
		}
	else
		{
		submit_target.disabled		= true;		
		}
	}


function hours_submit(type)
	{
	switch(type)
		{
		case "add":		if(confirm("Please confirm you wish to add these funds to this account."))
							{
							var hours			= $("#howmany").val();
							var payrate			= $("#payrate").val();
							var memberid		= $("#memberid").val();
							var payperiod		= $("#payperiod").val();
							var notes			= $("#notes").val();
							var url				= "./?action=addhours&hours="+hours+"&payrate="+payrate+"&id="+memberid+"&payperiod="+payperiod+"&notes="+escape(notes);
							location.href		= url;
							}
						else
							{
							return false;
							}
		break;
		case "deduct":	if(confirm("Please confirm you wish to deduct these funds from this account."))
							{
							var hours			= $("#howmany").val();
							var payrate			= $("#payrate").val();
							var memberid		= $("#memberid").val();
							var payperiod		= $("#payperiod").val();
							var notes			= $("#notes").val();
							var url				= "./?action=deducthours&hours="+hours+"&payrate="+payrate+"&id="+memberid+"&payperiod="+payperiod+"&notes="+escape(notes);
							location.href		= url;
							}
						else
							{
							return false;
							}
		break;
		case "payout":	if(confirm("Are you sure you want to payout this person's bank?"))
							{
							var hours			= $("#howmany").val();
							var payrate			= $("#payrate").val();
							var memberid		= $("#memberid").val();
							var payperiod		= $("#payperiod").val();
							var notes			= $("#notes").val();
							var url				= "./?action=payouthours&hours="+hours+"&payrate="+payrate+"&id="+memberid+"&payperiod="+payperiod+"&notes="+escape(notes);
							location.href		= url;
							}
						else
							{
							return false;
							}
		break;
		}
}
	</script>
	<center>
    <div id="bank">
		<table cellpadding="0" cellspacing="0" height="100%" width="100%">
			<tr>
				<td class='menu'><button id="bankhome" title='SUMMARY OF YOUR ACCOUNT' runat="server" onclick="location.href='./'"><img src="/images/icon/icon[bank].gif" align="absmiddle"/> bank home</button></td>
  				<td class='menu'><button id="ledger" title='LEDGER OF ALL ACCOUNT ACTIVITY' runat="server" onclick="location.href='./?action=ledger'"><img src="/images/icon/icon[report].gif" align="absmiddle"/> ledger</button></td>
				<td class='menu'><button id="withdraw" title='WITHDRAW FUNDS FROM YOUR ACCOUNT' runat="server" onclick="location.href='./?action=withdraw'"><img src="/images/icon/icon[remove].gif" align="absmiddle"/> withdraw</button></td>
				<td class='menu'><button id="deposit" title='DEPOSIT FUNDS FROM THIS PAYPERIOD INTO YOUR ACCOUNT' runat="server" onclick="location.href='./?action=deposit'"><img src="/images/icon/icon[add].gif" align="absmiddle"/> deposit</button></td>
			</tr>
			<tr>
				<td colspan='4' id="body" class="body" runat="server"></td>
			</tr>
		</table>
    </div>
	</center>
</body>
</html>
