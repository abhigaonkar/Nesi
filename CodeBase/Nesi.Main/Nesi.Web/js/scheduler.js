var schedulerObj =
{
	menuinit: function (s, e) {
		scheduler.menuManager.UpdateAptSubMenu = function (menu, submenuName, subMenuItemIndex) {
			var apt = scheduler.GetAppointmentById(scheduler.GetSelectedAppointmentIds()[0]);
			var statusid = apt.GetStatusId();
			s.cpWarning = "";
			if (statusid == 99) {
				menu.rootItem.items[0].SetVisible(false);
				menu.rootItem.items[1].SetVisible(false);
				menu.rootItem.items[2].SetVisible(false);
				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(false);
				menu.rootItem.items[5].SetVisible(false);
				menu.rootItem.items[6].SetVisible(false);
				menu.rootItem.items[7].SetVisible(false);
				menu.rootItem.items[8].SetVisible(false);
				menu.rootItem.items[9].SetVisible(false);
				menu.rootItem.items[10].SetVisible(false);

			}
			else if (statusid == 200) {
				menu.rootItem.items[0].SetVisible(false);
				menu.rootItem.items[1].SetVisible(false);
				menu.rootItem.items[2].SetVisible(false);
				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(false);
				menu.rootItem.items[5].SetVisible(false);
				menu.rootItem.items[6].SetVisible(false);
				menu.rootItem.items[8].SetVisible(false);
				menu.rootItem.items[9].SetVisible(false);
				menu.rootItem.items[10].SetVisible(false);
			}
			else if (statusid == 500) {
				menu.rootItem.items[0].SetVisible(true);
				menu.rootItem.items[1].SetVisible(false);
				menu.rootItem.items[2].SetVisible(false);
				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(false);
				menu.rootItem.items[5].SetVisible(false);
				menu.rootItem.items[6].SetVisible(false);
				menu.rootItem.items[8].SetVisible(false);
				menu.rootItem.items[9].SetVisible(false);
				menu.rootItem.items[10].SetVisible(false);
			}
			else if (statusid == 97) {
				menu.rootItem.items[0].SetVisible(true);
				menu.rootItem.items[1].SetVisible(true);
				menu.rootItem.items[2].SetVisible(true);
				menu.rootItem.items[3].SetVisible(true);
				menu.rootItem.items[4].SetVisible(false);
				menu.rootItem.items[5].SetVisible(false);
				menu.rootItem.items[8].SetVisible(false);
				menu.rootItem.items[9].SetVisible(false);
				menu.rootItem.items[10].SetVisible(false);
			}
			else if (statusid == 0) {
				menu.rootItem.items[0].SetVisible(true);

				if ((apt.scheduled_by != apt.thisuser) && (apt.can_edit_all == 0) && (apt.setby != apt.thisuser.toString())) {
					menu.rootItem.items[1].SetVisible(false);
					menu.rootItem.items[2].SetVisible(false);
				}
				else {
					menu.rootItem.items[1].SetVisible(true);
					menu.rootItem.items[2].SetVisible(true);
				}

				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(true);
				menu.rootItem.items[5].SetVisible(true);
				menu.rootItem.items[8].SetVisible(false);
				menu.rootItem.items[9].SetVisible(false);
				menu.rootItem.items[10].SetVisible(false);
				if (apt.meet_at_shop == 1) {
					menu.rootItem.items[6].SetVisible(true);
					menu.rootItem.items[7].SetVisible(false);
				}
				else {
					menu.rootItem.items[6].SetVisible(false);
					menu.rootItem.items[7].SetVisible(true);
				}

			}

			else if (statusid < 65) {
				menu.rootItem.items[0].SetVisible(true);
				menu.rootItem.items[1].SetVisible(true);
				menu.rootItem.items[2].SetVisible(true);
				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(false);
				menu.rootItem.items[5].SetVisible(true);
				menu.rootItem.items[8].SetVisible(false);
				menu.rootItem.items[9].SetVisible(false);
				menu.rootItem.items[10].SetVisible(false);
			}
			else if (statusid == 69) {
				menu.rootItem.items[0].SetVisible(false);
				menu.rootItem.items[1].SetVisible(true);
				menu.rootItem.items[2].SetVisible(false);
				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(false);
				menu.rootItem.items[5].SetVisible(false);
				menu.rootItem.items[6].SetVisible(false);
				menu.rootItem.items[7].SetVisible(false);
				menu.rootItem.items[8].SetVisible(false);
				menu.rootItem.items[9].SetVisible(false);
				menu.rootItem.items[10].SetVisible(false);

			}
			else if (statusid < 98) {

				menu.rootItem.items[0].SetVisible(true);
				menu.rootItem.items[1].SetVisible(true);
				menu.rootItem.items[2].SetVisible(true);
				menu.rootItem.items[3].SetVisible(true);
				menu.rootItem.items[4].SetVisible(true);
				menu.rootItem.items[5].SetVisible(true);
				menu.rootItem.items[6].SetVisible(true);
				menu.rootItem.items[7].SetVisible(true);
				menu.rootItem.items[8].SetVisible(false);
				menu.rootItem.items[9].SetVisible(false);
				menu.rootItem.items[10].SetVisible(false);

			}
			else if (statusid == 104) {
				menu.rootItem.items[0].SetVisible(false);
				menu.rootItem.items[1].SetVisible(false);
				menu.rootItem.items[2].SetVisible(false);
				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(false);
				menu.rootItem.items[5].SetVisible(false);
				menu.rootItem.items[6].SetVisible(false);
				menu.rootItem.items[7].SetVisible(false);
				if (apt.user_is_super == 1) {
					menu.rootItem.items[8].SetVisible(true);
					menu.rootItem.items[9].SetVisible(true);
					menu.rootItem.items[10].SetVisible(true);
				}
				else {
					menu.rootItem.items[8].SetVisible(false);
					menu.rootItem.items[9].SetVisible(false);
					menu.rootItem.items[10].SetVisible(false);
				}
			}
			else if ((statusid > 99) && (statusid < 106)) {
				menu.rootItem.items[0].SetVisible(false);
				menu.rootItem.items[1].SetVisible(true);
				menu.rootItem.items[2].SetVisible(true);
				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(false);
				menu.rootItem.items[6].SetVisible(false);
				menu.rootItem.items[7].SetVisible(false);
				if (apt.user_is_super == 1) {
					menu.rootItem.items[8].SetVisible(true);
					menu.rootItem.items[9].SetVisible(true);
					menu.rootItem.items[10].SetVisible(true);
				}
				else {
					menu.rootItem.items[8].SetVisible(false);
					menu.rootItem.items[9].SetVisible(false);
					menu.rootItem.items[10].SetVisible(false);
				}

			}

			if (apt.member_id > 100000000) {
				if ((apt.setby != apt.thisuser) && (apt.can_edit_all == 0)) {
					menu.rootItem.items[1].SetVisible(false);
					menu.rootItem.items[2].SetVisible(false);
					menu.rootItem.items[3].SetVisible(false);
					menu.rootItem.items[4].SetVisible(false);
					menu.rootItem.items[5].SetVisible(false);
					menu.rootItem.items[6].SetVisible(false);
					menu.rootItem.items[7].SetVisible(false);


				}
			}



		}
	}
};


function check_enddate(s) {

	var diff = s.GetValue() - te_start.GetValue();
	diff = Math.abs(diff) / 36e5;
	if (diff <= 0) {
		alert('The end time must be after the start time');
		s.SetValue(te_start.GetValue());
	}
	else if (diff > 22) {
		alert('The end time must be within 22 hours of the start time.');
		s.SetValue(s.lastChangedValue);
	}
}

function check_startdate(s) {

	if (s.lastChangedValue.getDate() != s.GetValue().getDate()) {
		alert('The starting date has to remain the same, you can change the time however.');
		s.SetValue(s.lastChangedValue);
	}
}
function recurrenceDateChanged(s, e) {

	var selectedWeekends = [];
	var selectedDates = s.GetSelectedDates();

	if (chkallow_weekends.GetValue() == 0) {
		for (i = 0; i < selectedDates.length; i++) {
			if (selectedDates[i].getDay() == 6 || selectedDates[i].getDay() == 0) {
				selectedWeekends[selectedWeekends.length] = selectedDates[i];
			}
		}
		for (i = 0; i < selectedWeekends.length; i++) {
			s.DeselectDate(selectedWeekends[i]);
		}
	}
}


function DefaultViewMenuHandler(scheduler, s, e) {
	if (e.item.GetItemCount() <= 0) {
		if (e.item.name == "GotoDate") {
			scheduler.RaiseCallback('MNUVIEW|GotoDate')
		}
		if (e.item.name == "GotoToday") {
			scheduler.RaiseCallback('MNUVIEW|GotoToday')
		}
	}

	var nameIsNumeric = isNumber(e.item.name);
	var isLateAppearance = e.item.name == 3; // Late Appearance
	var isShortageOfWork = e.item.name == 7; // Shortage of Work
	if (nameIsNumeric && !isLateAppearance && !isShortageOfWork) {
		scheduler.RaiseCallback("MYAPTMENU|" + e.item.name);
	}
	else if (e.item.name == "Copy") {
		var aptid = scheduler.GetSelectedAppointmentIds()[0];
		scheduler.RaiseCallback("copy|" + aptid);
	}
	else if (isLateAppearance) {
		pop_ts.Show();
		pop_ts.PerformCallback(e.item.name);
		//	scheduler.RaiseCallback("MYAPTMENU|" + e.item.name);
	}
	else if (e.item.name == "unavailable") {
		scheduler.RaiseCallback("MYAPTMENU|" + e.item.name);
	}
	else if (isShortageOfWork) {
		scheduler.RaiseCallback("MYAPTMENU|" + e.item.name);
	}

	if (e.item.name == "_open") {
		var aptid = scheduler.GetSelectedAppointmentIds()[0];


		var apt = scheduler.GetAppointmentById(scheduler.GetSelectedAppointmentIds()[0]);
		var statusid = apt.GetStatusId();

		if (((statusid != 500) && (statusid > 62)) || (statusid == 0)) {
			pop_edit.cp_aptid = scheduler.GetSelectedAppointmentIds()[0]; pop_edit.Show(); pop_edit.PerformCallback(aptid);
		}
		else {
			pop_event.cp_aptid = scheduler.GetSelectedAppointmentIds()[0];
			pop_event.Show(); pop_event.PerformCallback(aptid);
		}
	}
	if (e.item.name == "DeleteAppointment") {

		scheduler.RaiseCallback("DELETE");
	}
	if (e.item.name == "OpenAppointment") {
		scheduler.RaiseCallback("MYAPTMENU|" + e.item.name);
	}
	if (e.item.name == "addrecurrence") {

		hf.Set('res', scheduler.GetSelectedAppointmentIds()[0]);
		pop_recurrence.Show();
		pop_recurrence.PerformCallback(scheduler.GetSelectedAppointmentIds()[0]);

	}
	if (e.item.name == "confirm") {

		scheduler.RaiseCallback("CONFIRM");
	}
	if (e.item.name == "tentative") {

		scheduler.RaiseCallback("TENTATIVE");
	}
	if (e.item.name == "MEET_AT_SHOP") {
		scheduler.RaiseCallback("MEET_AT_SHOP");
	}
	if (e.item.name == "MEET_ON_SITE") {
		scheduler.RaiseCallback("MEET_ON_SITE");
	}
	else if (e.item.name == "APPROVE") {
		scheduler.RaiseCallback("APPROVE");
	}
	else if (e.item.name == "DENY") {
		scheduler.RaiseCallback("DENY");
	}
	else if (e.item.name == "PENDING") {
		scheduler.RaiseCallback("PENDING");
	}
	if (e.item.name == "addevent") {

		pop_event.Show(); pop_event.PerformCallback();
	}
	if (e.item.name == "email") {

		pop_email.Show();
		lbl_pop_id.SetText(scheduler.GetSelectedAppointmentIds()[0]);
		pop_email.PerformCallback(scheduler.GetSelectedAppointmentIds()[0]);

	}
}

function appointmentMenu_PopUp(s, e) {



	var openItem = e.item.GetItemByName("addevent");

	var openItem2 = e.item.GetItemByName("1");
	var openItem3 = e.item.GetItemByName("2");
	var openItem4 = e.item.GetItemByName("3");
	var openItem5 = e.item.GetItemByName("5");
	var openItem6 = e.item.GetItemByName("6");
	var openItem7 = e.item.GetItemByName("7");
	var openItem8 = e.item.GetItemByName("8");
	var openItem9 = e.item.GetItemByName("9");
	var openItem10 = e.item.GetItemByName("10");

	if (openItem != null) {
		openItem.SetVisible(true);
		if (scheduler.GetSelectedResource() > 100000000) {
			openItem.SetVisible(false);
		}
	}

	if (openItem2 != null) {
		openItem2.SetVisible(true);
		if (scheduler.GetSelectedResource() > 100000000) {
			//	openItem2.SetVisible(false);
		}
	}
	if (openItem3 != null) {
		openItem3.SetVisible(true);
		if (scheduler.GetSelectedResource() > 100000000) {
			openItem3.SetVisible(false);
		}
	}
	if (openItem4 != null) {
		openItem4.SetVisible(true);
		if (scheduler.GetSelectedResource() > 100000000) {
			openItem4.SetVisible(false);
		}
	}
	if (openItem5 != null) {
		openItem5.SetVisible(true);
		if (scheduler.GetSelectedResource() > 100000000) {
			openItem5.SetVisible(false);
		}
	}
	if (openItem6 != null) {
		openItem6.SetVisible(true);
		if (scheduler.GetSelectedResource() > 100000000) {
			openItem6.SetVisible(false);
		}
	}
	if (openItem7 != null) {
		openItem7.SetVisible(true);
		if (scheduler.GetSelectedResource() > 100000000) {
			openItem7.SetVisible(false);
		}
	}
	/*		if (openItem8 != null) {
				openItem8.SetVisible(false);
				if (statusid==105) {
				//    openItem8.SetVisible(true);
				}
			}
			if (openItem9 != null) {
				openItem9.SetVisible(false);
				if (statusid == 105) {
			//        openItem9.SetVisible(true);
				}
			}
			if (openItem10 != null) {
				openItem10.SetVisible(false);
				if (statusid == 105) {
			  //      openItem10.SetVisible(true);
				}
			}
		*/
}


function WinLoad() {
	var ssio = document.getElementById('draggable3');
	if (hf.Get('can_edit') == "1") {
		ssio.style.visibility = "visible";

	}
}
window.onload = WinLoad;

function InitalizejQuery(s, e) {
	$('.draggable').draggable({ helper: 'clone', appendTo: 'parent', zIndex: 100, refreshPositions: true });

	$('.droppable').droppable({
		activeClass: "dropTargetActive",
		hoverClass: "dropTargetHover",

		drop: function (ev, ui) {
			// Make a clone of the dragged item

			var clone = (ui.draggable).clone();

			if ($(ui.draggable).attr("id") != "draggable3") {

				// Get a row index:
				row = $(clone).find("input[type='hidden']").val();
				if (row != 'N') {
					hf.Set('row', row);
					// Calculate an active time cell
					var cell = scheduler.CalcHitTest(ev).cell;

					// Initiate a scheduler callback to create an appointment based on a cell interval

					if (cell != null) {

						//			var drop_date = cell.interval.start;
						//			var apt_id = cell.id;
						//			var isok = PageMethods.Check_wo_date(drop_date,apt_id)
						scheduler.getCellInfoProvider().initializeCell(cell);
						//alert(hf.Get('is_wo'));
						hf.Set('res', cell.resource);

						if (hf.Get('is_wo') == '1') {
							scheduler.RaiseCallback('CRTAPT|' + ASPx.DateUtils.GetInvariantDateTimeString(cell.interval.start));
						}
						else if (hf.Get('is_wo') == '2') {
							scheduler.RaiseCallback('CRTAPTS|' + ASPx.DateUtils.GetInvariantDateTimeString(cell.interval.start));
						}
						else {
							scheduler.RaiseCallback('CRTAPTQ|' + ASPx.DateUtils.GetInvariantDateTimeString(cell.interval.start));
						}
					}
					else {
						console.log(ev);
						alert('Drop the dragged item on a specific time cell.');
					}
				}
			}
			// Additional logic goes here...
		}
	}
	);
}
$(function () {
	$("#draggable3").draggable();

});

$(document).ready(function () {

	load_ov();

});

function load_ov() {
	//	$("#ov").attr("src", "if_overview.aspx?cid=" + ddlbranch.GetValue() + "&pid=" + ddlpm.GetValue());
}

function go_email() {

	pop_email.PerformCallback('send|' + lbl_pop_id.GetText());


}

