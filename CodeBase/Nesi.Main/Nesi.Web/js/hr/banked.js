var bank_admin =
	{
	withdraw: 
		function (s, e) {
			boing('./index.aspx?member_id=' + s.cpid + '&action=withdraw', 'Withdraw', 320, 400);
		},
	deposit: function(s,e)
		{
		boing('./index.aspx?member_id=' + s.cpid +'&action=deposit', 'Deposit', 320, 400);
		},
	deduct: function(s,e)
		{
		boing('./index.aspx?member_id='+s.cpid+'&action=deduct', 'Deduct', 320, 400);
		},
	ledger: function(s,e)
		{
		boing('./index.aspx?member_id='+s.cpid+'&action=ledger', 'Add', 800,300);
		},
	payout: function(s,e)
		{
		boing('./index.aspx?member_id='+s.cpid+'&action=payout', 'Add', 320, 400);
		},
	}

function banked_admin(id)
	{
	location.href				= "./index.aspx?business_unit_id="+id;
	}


function hours_test(max)
	{
	var hours_target			= $('#howmany');
	var payrate_target			= $('#payrate');
	var total_target			= $('#total');
	var submit_target			= $('#submit');
	var notes_target			= $('#notes');
	var error					= 0;
	var error_text				= "";
	var can_see					= total_target.val() != '--';
	if(isNaN(hours_target.val()))
		{
		hours_target.val('');
		error						= 1;
		error_text					= "Hours are not a number";
		}
	else if(hours_target.val() != '')
		{
		if(payrate_target.val() != '' && payrate_target.val() != '0')
			{
			var total					= payrate_target.val() * hours_target.val();
			var formatted_total			= total.toFixed(2);
			if(can_see)
				{
				total_target.html('$'+formatted_total);
				}
			}
		else
			{
			alert('There is an error with this members pay information that needs to be corrected.');
			window.close();
			}
		}
	else
		{
		error						= 1;
		error_text					= "Unknown Error - Hours";
			if(can_see)
				{
				total_target.html('$0.00');
				}
		}

	if (notes_target.val().length < 10)
		{
		error = 1;
		submit_target.attr("title", "Must submit at least a 10 character note as to why you are doing this transaction.");
		}
	else
		{
		submit_target.removeAttr("title");
		}

	if(max != '' && max < hours_target.val())
		{
		error						= 1;
		hours_target.val('');
		if(can_see)
			{
			total_target.html('$0.00');
			}
		alert('You have requested to deduct more than what is available.');
		hours_target.focus();
		}

	if(error != 1)
		{
		submit_target.removeAttr('disabled');
		}
	else
		{
		submit_target.attr('disabled',true);		
		}
	}


function hours_submit(type)
	{
	var hours			= $('#howmany').val();
	var member_id		= $('#memberid').val();
	var payperiod		= $('#payperiod').val();
	var notes = $('#notes').val();
	var action = type + "hours";
	var message = "";
	switch(type)
		{
		case 'add':
			message = 'Please confirm you wish to add these funds to this account.';
		break;
		case 'deduct':
			message = 'Please confirm you wish to deduct these funds from this account.';
		break;
		case 'payout':
			message = 'Are you sure you want to payout this persons bank?';
		break;
		case 'withdraw':
			message = 'Please confirm you wish to withdraw these funds from this account & add to the selected pay period';
		break;
		}
	if (confirm(message))
		{
		var url = './index.aspx?action='+action+'&hours=' + hours + '&member_id=' + member_id + '&payperiod=' + payperiod + '&notes=' + escape(notes);
		location.href = url;
	} else
		{
		return false;
		}
	}