var showEditForm = null;

function InitalizejQuery(s, e) {
    $('.draggable').draggable({ helper: 'clone' });
    $('.droppable').droppable(
                {
                    activeClass: "hover",
                    drop: function (ev, ui) {
                        // make a clone of the dragged item
                        var clone = (ui.draggable).clone();
                        // get the row index:
                        row = $(clone).find("input[type='hidden']").val();
                        hf.Set('row', row);
                        hf.Set('CustomInsertion', true);
                        showEditForm = true;

                        gv.GetRowValues(row, "WO", OnGetRowValues);
                    }
                }
              );
}

function OnEndCallback(s, e) {
    if (showEditForm) {
        showEditForm = false;
        s.InplaceEditFormShowMore();
    }
    if (s.cp_resetHf)
        hf.Set('CustomInsertion', false);
}

function OnGetRowValues(date) {
    scheduler.ShowInplaceEditor(date, date);
}