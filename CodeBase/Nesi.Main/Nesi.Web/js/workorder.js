var wo = {
	id: 0,
	header:
	{
		obj_margin: "",
		obj_tobeinvoiced: "",
		obj_status: "",
		obj_ts: "",
		obj_business_unit_id: ""
	},
	quote_info:
		function (s, e) {
			if (e.result != null && e.result.match(/\{/g)) {
				var v = $.parseJSON(e.result);
				txtWODescription.SetValue(v.job_description.replace(/\&quot\;/g, '"'));
				document.getElementById("ctl00_cphMasterBody_pc_main_cbp_left_div_quoteterms_tr").style.display = "";
				lblquoteterms.SetText(v.quoteterms.replace(/\&quot\;/g, '"'));
				ddlContact.SetValue(v.contact_id);
				txtSalesValue.SetValue(v.quoted_price);
				ddl_terms.SetValue(v.term_id);
				if (typeof (ddlPM) === "undefined") {
					alert("The PM that is currently selected for this work order is either inactive, or is no longer with this business unit. Before you can proceed with attaching this quote, this PM's work orders will need to be changed via the 'footprints' tab of their employee profile. ");
					ddlquote.SetSelectedIndex(0);
					please_wait("stop");
					return;
				}
				ddlPM.SetValue(v.quoted_by);
				ddl_currency.SetValue(v.currency);
				if (v.address_id != 0) {
					ddlAddress.SetValue(v.address_id);
				}
				if (v.expected_end.match(/-/g)) {
					var d = v.expected_end.split("-");
					var end_date = new Date(d[0], d[1] - 1, d[2]);
					dteExpEndDate.SetDate(end_date);
				}
				txtlaborvalue.SetText(v.expected_hours);
				if (ddlRevenueLines != null) { ddlRevenueLines.SetValue($('.defaultQuoted').val()); }

				ddl_custram.SetValue(v.bdm);
			}
			else {
				ddlquote.SetSelectedIndex(-1);
				alert(e.result);
			}
			please_wait("stop");
		},
	is_customer_set:
		function (s, e) {
			var customer_id = ddlCustomer.GetValue();
			if (customer_id == null) {
				e.processOnServer = false;
				s.SetChecked(false);
				alert("Please select a customer first.");
			}
		},
	ddlcustomer_changed:
		function (s, e) {
			lblError.SetText("");
			chkProgress.SetEnabled(true);
			chkProgress.SetChecked(false);
			tblProgress.SetVisible(false);
			dteExpEndDate.SetText("");
			dteStartDate.SetText("");
			txtSalesValue.SetText("");
			txtCustPO.SetText("");
			txtWODescription.SetText("");
			var c = $('input[id$=hidCompanyID').val() / 1;
			if (c == 1 || c == 4 || c == 7 || c == 23) {
				x = 0;
				txtRDQuestion.SetText("Will this work improve the cycle time of the customers production or assembly? ");
				ASPxPopupControl1.Show();
			}

			try {
				ddlJobCostWO.SetValue("");
			}
			catch (err) {
			}
			cbp_left.PerformCallback();


		},
	is_exactamount:
		function (s, e) {
			var is_checked = s.GetChecked();
			if (is_checked) {
				$(".exact_row").show();
				$(".pct_row").hide();
			}
			else {
				$(".exact_row").hide();
				$(".pct_row").show();
			}
		},
	parent_company_changed:
		function (s, e) {
			if (ddl_parent_workorder != undefined) {
				ddl_parent_workorder.PerformCallback(ddlCompany.GetValue() + "|" + s.GetValue());
			}
		},
	cbp_left:
	{
		callback_begin: function (s, e) {
			please_wait('start');
		},
		callback_end: function (s, e) {
			try {
				please_wait('stop');
				if (typeof ddl_creditwolink !== 'undefined' && ddl_creditwolink.GetSelectedItem() != null) {
			
					txtWODescription.SetText(s.cp_description.toString());
					txtSalesValue.SetText(s.cp_salesvalue.toString());
					var startdate = new Date(s.cp_expectedstartdate.toString());
					var enddate = new Date(s.cp_expectedenddate.toString());
					dteStartDate.SetDate(startdate);
					dteExpEndDate.SetDate(enddate);
					txtCustPO.SetText(s.cp_custpo.toString());
					chkProgress.SetChecked(false);
				}
				//chkProgress.SendPostBack();
			}
			catch (ex) {
				alert('Error while processing.');
			}
		},
		callback_error: function (s, e) {
			please_wait('stop');
		}
	},
	handle_iframe:
	{
		quote:
		{
			bind:
				function () {
					var frame = $(".quote_frame");
					var lastheight = 0;
					setInterval(function () {
						frame = $(".quote_frame");
						if (frame.is(":visible") && frame[0].contentDocument != null) {
							if (frame[0].contentDocument.body.scrollHeight != lastheight) {
								lastheight = frame[0].contentDocument.body.scrollHeight;
								frame.animate({ "height": lastheight }, 350);
							}
						}
					}, 200);
				}
		}
	},
	handle_customer_request:
		function (obj) {
			var customer_id = ddlCustomer.GetValue();
			if (customer_id == null) {
				customer_id = 0;
			}
			boing("/modules/request.aspx?type=1&id=" + customer_id + "&woprog_id=" + wo.id, "customer_request" + Math.random(), 500, 900);
		},
	handle_lastupdated:
		function (obj) {
			please_wait("start");
			$.get("./index.aspx",
				{
					a: "update_ts",
					id: wo.id
				},
				function () {
					update_ts();
					please_wait("stop");
				}
			);

		}
};

function update_header() {
	$.get("./index.aspx",
		{
			a: "json_update_header",
			woprog_id: wo.id
		},
		function (json_string) {
			var obj = $.parseJSON(json_string);
			$(".picklist").height($(".picklist").contents().height());
			$("#" + wo.header.obj_margin).text(obj.GP);
			$("#" + wo.header.obj_tobeinvoiced).text(obj.Invoicelbl);
			$("#" + wo.header.obj_status).text(obj.status);
			$("#ctl00_lblHeading").html(obj.title + "<br/>" + obj.description.replace("\n", "<br/>"));
			try {
				if (window.parent != null) {
					window.parent.window.document.title = obj.title.replace("Work Order: ", "");
				} else {
					document.title = obj.title.replace("Work Order: ", "");
				}
			} catch (ex) {
				if (window != null) {
					window.document.title = obj.title.replace("Work Order: ", "");
				} else {
					document.title = obj.title.replace("Work Order: ", "");
				}
			}
		});
}

function update_ts() {
	$.get("./index.aspx",
		{
			a: "get_ts",
			woprog_id: wo.id
		},
		function (resp) {
			if (resp.match(/\d+/g)) {
				var vals = resp.split("|");
				$("#" + wo.header.obj_ts).val(vals[0]);
				$("[id$='lbl_lastmodified']").text("Last Modified: " + vals[1]);
			}
		});
}

function AllowClickOnce() {

	LoadingPanel.Show();
	return true;

}



function OnPCInit() {
	var isNetscapeOrMozilla = (__aspxNetscape || __aspxMozilla);
	if (isNetscapeOrMozilla)
		window.addEventListener("DOMMouseScroll", OnPCUpdatePos, true);
	if (__aspxFirefox) {
		_aspxAttachEventToElement(document.documentElement, "scroll", OnPCUpdatePos);
		_aspxAttachEventToElement(window, "scroll", OnPCUpdatePos);
	} else {
		var elementForScrollListener = isNetscapeOrMozilla ? document.documentElement : window;
		_aspxAttachEventToElement(elementForScrollListener, "scroll", OnPCUpdatePos);
	}
	_aspxAttachEventToElement(window, "resize", OnPCUpdatePos);
}
function OnPCUpdatePos() {
	if (pc1.IsVisible())
		pc1.UpdatePosition();
}


function GetInvPrice(masterid, amount, partno, price) {
	if (masterid >= 990000) {
		document.getElementById("ctl00_cphMasterBody_hidSellPrice").Value = price;
		return price;
	}
	var c = document.getElementById(wo.header.obj_business_unit_id).value;
	var url = "/_tools/inventory_price/index.aspx?q=" + masterid + "&a=" + amount + "&c=" + c + "&pn=" + partno + "&pr=" + price;
	var sellprice = "0";

	$.ajax(
		{
			type: 'GET',
			url: url,
			dataType: 'xml',
			async: false,

			beforeSend: function () {

			},
			success: function (xml) {
				// This is for if it successfully receives a response it can work with
				$(xml).find('price').each(function () {
					sellprice = $(this).text();
					document.getElementById("ctl00_cphMasterBody_hidSellPrice").Value = sellprice;
				})
			},
			complete: function () {
				return sellprice;
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
				alert(errorThrown);
				return "0";
			}

		}
	);

}

function FocusQuantity() {
	ASPxGridPartLines.FocusEditor('wo_detail_current_qty_invoiced');
}
function FocusPartNo() {
	ASPxGridPartLines.FocusEditor('wo_detail_current_code');
}
function stuff(e) {
	document.getElementById("ctl00_cphMasterBody_HiddenWOID").value = e;
}

function SomeCallbackFunction(v) {
	var mySplitResult = v.split("||");

	if (v == "") {


		ASPxPopupParts.Show();
	}
	else {


		var qty = ASPxGridPartLines.GetEditor("wo_detail_current_qty_invoiced").GetValue();
		if (qty == "" || qty == null) {
			qty = "1"
		}
		//var total = "0"
		var price = "0";
		price = GetInvPrice(mySplitResult[0], "1", mySplitResult[1]);
		price = document.getElementById("ctl00_cphMasterBody_hidSellPrice").Value;
		ASPxGridPartLines.GetEditor("wo_detail_current_master_id").SetValue(mySplitResult[0]);
		ASPxGridPartLines.GetEditor("wo_detail_current_code").SetValue(mySplitResult[1]);
		ASPxGridPartLines.GetEditor("CleanDescription").SetValue(mySplitResult[2]);
		//ASPxGridPartLines.GetEditor("wo_detail_current_price_sell").SetValue(mySplitResult[3]); 
		ASPxGridPartLines.GetEditor("wo_detail_current_price_sell").SetValue(price);
		ASPxGridPartLines.GetEditor("wo_detail_current_qty_invoiced").SetValue(qty);
		//ASPxGridPartLines.GetEditor("TotalPrice").SetValue(total);
		FocusQuantity();


	}
	LoadingPanel.Hide();
}

var x;

function TABLE2_onclick() {
}


var modalResult = false;
function closePopup1() {
	if (memAdvancement.GetText() == "") {
		alert("You must enter something valid for the technological advancement that will be the result of the work being done.");
	}
	else if (memUncertainty.GetText() == "") {
		alert("You must enter something valid for the uncertainty that inherently exists on this job.");
	}
	else if (memCoreCompetency.GetText() == "") {
		alert("You must enter something valid for the core competency that we possess that relating to this job.");
	}
	else {
		ASPxPopupControl1.Hide();
		PageMethods.btnSaveGeneral_Click();
	}
}
function closePopup(result) {
	//      var tab = ASPxPageControl1.GetTab(8);
	modalResult = result;
	if (modalResult == true) {
		pnlRD.SetVisible(true);
		pnlrdYesNo.SetVisible(false);
		chkRD.SetChecked(true);

		//      tab.SetEnabled(true);

		//   ASPxPopupControl1.Hide();
	}
	else {

		x = x + 1;

		if (x == 1) {

			txtRDQuestion.SetText("Will this work result in the machine being able to handle more parts or a new design of part(s)? ");
		}
		else if (x == 2) {
			txtRDQuestion.SetText("Will this work automate a previously manual process?  ");
		}
		else if (x == 3) {
			txtRDQuestion.SetText("Will this result in a new production technique being implemented?");
		}
		else {
			hdnRD.Set("hdnCompetency", memCoreCompetency.GetValue());
			hdnRD.Set("hdnAdvancement", memAdvancement.GetValue());
			hdnRD.Set("hdnUncertainty", memUncertainty.GetValue());
			ASPxPopupControl1.Hide();

		}
	}


}

function save_click() {
	var c = document.getElementById(wo.header.obj_business_unit_id).value;
	if (c == "1" || c == "4" || c == "7" || c == "23" || c == "8") {
		x = 0;
		txtRDQuestion.SetText("Will this work improve the cycle time of the customers production or assembly? ");
		ASPxPopupControl1.Show();
	}
	else {



	}

}

function OnCustomerChanged(ddlCustomer) {
	//var c = document.getElementById(wo.header.obj_business_unit_id).value;
	//if (c=="1"|| c== "4" || c== "7" ||c=="23")
	//	{
	//	x=0;
	//	txtRDQuestion.SetText("Will this work improve the cycle time of the customers production or assembly? ");
	//	ASPxPopupControl1.Show();
	//	}

	try {

		if (typeof (ddlJobCostWO) !== 'undefined') {
			ddlJobCostWO.SetValue("");
			txtpb_amt.SetText("");
			chkDownPayment.SetChecked(false);

			chkProgress.SetChecked(false);
			tblProgress.SetVisible(false);
		}
		cbp_left.PerformCallback();
		//chkProgress.SendPostBack();

	}
	catch (err) {
		alert(err);
	}

}

function OnddlCreditWO(ddl_creditwolink) {
	cbp_left.PerformCallback("ddlcreditwochanged");

}

function quote_changed(ddlquote) {
	ddlPM.PerformCallback(ddlquote.GetValue().toString());
}
function l_cb() {
	cbp_left.PerformCallback();
}

function showNotes(lineid, e) {
	document.getElementById("ctl00_cphMasterBody_hidNotesID").value = lineid;
	if (lineid != "0") {
		var url = "/_tools/get_notes/index.aspx?id=" + lineid + "&source=purchaseorder";
		var notes = "";
		$.ajax(
			{
				type: 'GET',
				url: url,
				dataType: 'xml',
				async: false,

				beforeSend: function () {

				},
				success: function (xml) {
					// This is for if it successfully receives a response it can work with
					$(xml).find('notesreturn').each(function () {
						notes = $(this).text();
						$("#ctl00_cphMasterBody_ASPxpuNotes_textNotes").val(notes);
					})
				},
				complete: function () {
					return notes;
				},
				error: function (XMLHttpRequest, textStatus, errorThrown) {
					return "0";
				}

			}
		);

	}
	document.getElementById("ctl00_cphMasterBody_ASPxpuNotes_imgbnotesupdate").style.display = "";

	NotesPopUp.SetHeaderText("Notes");

	var pos_x = e.x;
	var pos_y = e.y;

	NotesPopUp.ShowAtPos(pos_x, pos_y);

	//NotesPopUp.Show();  

}
function file_download(filename) {
	//alert("Hello");
	$("#downloader").attr("src", "../../download.aspx?file_id=" + filename + "");
}

function to_approval(woprog_id, business_unit_id, bvwo, extra, more, approval_type) {
	var ts = $("#" + wo.header.obj_ts).val();
	var base_url = "/wo_prog_edit.aspx?woprog_id=" + woprog_id + "&business_unit_id=" + business_unit_id + "&bvwo=" + bvwo + "&ts=" + ts;
	base_url += extra != undefined && extra != null && extra != "" ? "&" + extra : "";
	base_url += more != undefined && more != null && more != "" ? "&" + more : "";
	var approvals = {
		waitpo: "Are you sure you want to commit this work order?",
		toinvoice: "Are you sure you want to invoice this work order?  This is very hard to undo.."
	}
	confirm_approval(base_url);
	/*
	if(approval_type != undefined && approval_type != null)
		{
		if(confirm(approvals[approval_type]))
			{
			//please_wait("start");
			//top.location.href		= base_url;
			}
		}
	else
		{
		confirm_approval(base_url);
		//please_wait("start");
		//top.location.href		= base_url;
		}
	*/
}
function autoResize(obj) {
	var id = obj.id;
	obj.width = "100%";
}
function updatereqdates(newdate) {
	var wo_id = wo === undefined ? "0" : wo.id;

	if (wo_id != "0") {
		if (confirm("Do you want to update the required dates for parts ?  Press CANCEL to leave required dates as they are...")) {
			var url = "/_tools/WorkOrder/lineitems.aspx?id=" + wo_id + "&action=updatereqdates&newdate=" + newdate;
			var updatedlines = "NA";
			var resultdate = "";
			$.ajax(
				{
					type: 'GET',
					url: url,
					dataType: 'xml',
					async: false,

					beforeSend: function () {

					},
					success: function (xml) {
						// This is for if it successfully receives a response it can work with
						$(xml).find('result').each(function () {
							updatedlines = $(this).text();
						})
						$(xml).find('resultdate').each(function () {
							resultdate = $(this).text();
						})

					},
					complete: function () {
						alert(updatedlines + " updated to " + resultdate);
					},
					error: function (XMLHttpRequest, textStatus, errorThrown) {
						return "0";
					}

				});


		}
	}

}

$(document).ready(function () {
	Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
	Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
	bind_tooltips();
	wo.handle_iframe.quote.bind();
});
function EndReqHandler() {
	bind_tooltips();
	please_wait("stop");
	if (typeof (ddlCompany) !== "undefined") {
		ddlCompany.SetEnabled(!chkProgress.GetChecked() && wo.id == 0);
	}
	update_header();
}
function BeginReqHandler() {
	please_wait("start");
}
function bind_tooltips() {
	$(".ttip").each(function () {
		$(this).tip();
	});
}
//TODO this is not actually one less than top.
//dashboard.js is also coded this way
function getOneLessThanTop(obj) {
	if (obj.parent != top)
		return obj.parent;
	return obj;

}
function sendto(base_url) {
	var ts = $("#" + wo.header.obj_ts).val();


	if (!base_url.includes("&ts")) {
		base_url += '&ts=' + ts;
	}

	//console.log('send to url: ' + base_url + '&ts=' + ts)
	sessionStorage.setItem('nesi1IframeUrl_V1', base_url);
	sessionStorage.setItem('nesi1IframeUrl_parent_V1', '12');
	sessionStorage.setItem('currentURL_V4', '/home/1/12/default');
	//getOneLessThanTop(parent)
	top.window.location.href = base_url;



}
function RecalculateCharsRemaining(editor) {
	var maxLength = parseInt(editor.maxLength ? editor.maxLength : editor.GetInputElement().maxLength);
	var editValue = editor.GetValue();
	var valueLength = editValue != null ? editValue.toString().length : 0;
	var charsRemaining = maxLength - valueLength;
	SetCharsRemainingValue(editor, charsRemaining >= 0 ? charsRemaining : 0);
}
function SetCharsRemainingValue(textEditor, charsRemaining) {
	var associatedLabel = ASPxClientControl.GetControlCollection().Get(textEditor.name + "_cr");
	var color = GetLabelColor(charsRemaining).toString();
	associatedLabel.SetText("<span style='color: " + color + ";'>" + charsRemaining.toString() +
		"</span>");
}
function GetLabelColor(charsRemaining) {
	if (charsRemaining < 50) return "red";
	if (charsRemaining < 100) return "#F3A250";
	return "green";
}

// ASPxMemo - MaxLength emulation
function InitMemoMaxLength(memo, maxLength) {
	memo.maxLength = maxLength;
}
function EnableMaxLengthMemoTimer(memo) {
	memo.maxLengthTimerID = window.setInterval(function () {
		var text = memo.GetText();
		if (text.length > memo.maxLength) {
			memo.SetText(text.substr(0, memo.maxLength));
			RecalculateCharsRemaining(memo);
		}
	}, 50);
}
function DisableMaxLengthMemoTimer(memo) {
	if (memo.maxLengthTimerID) {
		window.clearInterval(memo.maxLengthTimerID);
		delete memo.maxLengthTimerID;
	}
}

		function parent_ddl_OnInit(s, e) {
			ASPxClientUtils.AttachEventToElement(s.GetInputElement(), "click", function (event) {

				if (s.GetValue() != "0" && (s.GetValue() != null)) {
					boing('/wo_prog_frame.aspx?action=show&woprog_id=' + s.GetValue(), 0, 1024, 700);
				}

				s.HideDropDown();
			});
		}


       var postponedCallbackRequired = false;
        function OnListBoxIndexChanged(s, e) {
                CallbackPanel.PerformCallback();          
        }
        function OnEndCallback(s, e) {     
                      ddl_Customer.SetEnabled(true);
                      ddl_Address.SetEnabled(true);
                      ddl_Contact.SetEnabled(true);
                      txt_PONum.SetEnabled(true);
                      txtCloseReassign.SetEnabled(true);    
                      btnCloseReassigningSave.SetEnabled(true);
        }
       function OnBeginCallback(s, e) {
                    ddl_Customer.SetEnabled(false);
                    ddl_Address.SetEnabled(false);
                    ddl_Contact.SetEnabled(false);
                    txt_PONum.SetEnabled(false);
                    txtCloseReassign.SetEnabled(false);
                    btnCloseReassigningSave.SetEnabled(false);               
        };

        var isPostbackInitiated = false;
        function OnSaveClient_Click(s, e) {
            if (!CallbackPanel.InCallback()) {  
                 var CloseReassign = '';
                 var customer=ddl_Customer.GetValue();
                if (customer == '0') {
                    e.processOnServer = false;
                    alert('Customer can not be empty!');
                    return false;
                }
              
                var address= ddl_Address.GetValue();
                if (address == '0') {
                    e.processOnServer = false;
                    alert('Address can not be empty!');
                    return false;
                }
                 var contact = ddl_Contact.GetValue();
                if (contact == '0') {
                    e.processOnServer = false;
                    alert('Contact can not be empty!');
                    return false;
                }

                 CloseReassign =txtCloseReassign.GetText();
                if (CloseReassign=='') {
                    e.processOnServer = false;
                    alert('Close & Reassign can not be empty!');
                    return false;
                }

                if (!isPostbackInitiated) {
                    if (confirm('Are you sure you want to close this work order?')) {
                        isPostbackInitiated = true;
                        e.processOnServer = true;
                     
                        s.SendPostBack('Click');
                        btnCloseReassigningSave.SetEnabled(false);
                        ddl_Customer.SetEnabled(false);
                        ddl_Address.SetEnabled(false);
                        ddl_Contact.SetEnabled(false);
                        txt_PONum.SetEnabled(false);
                        txtCloseReassign.SetEnabled(false);
                    }
                    else {
                        isPostbackInitiated = false;
                        btnCloseReassigningSave.SetEnabled(true);
                        ddl_Customer.SetEnabled(true);
                        ddl_Address.SetEnabled(true);
                        ddl_Contact.SetEnabled(true);
                        txt_PONum.SetEnabled(true);
                        txtCloseReassign.SetEnabled(true);
                        e.processOnServer = false;
                        return false;
                    }
                }
                else {
                    e.processOnServer = false;
                    return false;
                }
            }
            else {
                 e.processOnServer = false;
                    return false;
            }
           
        }


        function bc(master_id)
        {
            
            createPopupWin("/_tools/print_barcode/index.aspx?master_id=" + master_id, "Print Barcode", 600, 200);
        
		//popbcprint.SetHeaderText("Print Labels for: " + master_id);
		//hidbcmastertid.Set("id",master_id);
		//popbcprint.Show();

        }
        function createPopupWin(pageURL, pageTitle, 
                    popupWinWidth, popupWinHeight) { 
            var left = (screen.width-popupWinWidth)/2; 
            var top = (screen.height-popupWinHeight)/4;
            var myWindow = window.open(pageURL, pageTitle,  
                    'resizable=yes, width=' + popupWinWidth 
                    + ', height=' + popupWinHeight + ', top=' 
                    + top + ', left=' + left); 
        } 