var engine_client = {
    chosenReport: null,

    filter_submit: function(_s, _e) {
        console.log("Starting filter submission.");
        console.log("Before Extraction:", $(".hid_paras").val());
        var paras = $(".hid_paras").val().replace(/ /g, "").replace(/\?/g, "").split(",");
        console.log("Parameters extracted:", paras);
        var payload = [];
        console.log("Report ID:", id);
        var id = $(".hid_report_id").val();
		if (paras.length > 0 && !ASPxClientEdit.ValidateGroup('dynamicCtrls')) {
			console.log("Validation failed, submission aborted.");
			return; // Exit the function if validation fails
		}

        for (var i = 0; i < paras.length; i++) {
            var para = paras[i];
            console.log("Processing parameter:", para);
            var control = eval(para); // Note: eval can be unsafe
            console.log("Control found:", control);

            if (control != null && control !== undefined) {
                var control_value;
				var dateMatch = para.match(/date_/g);
				var textMatch = para.match(/text_/g);
                if (dateMatch || textMatch) {
                    control_value = control.GetText();
                    console.log(dateMatch ? "Date--" : "Text--" + para + " - Text:", control_value);
                } else {
                    control_value = control.GetValue();
                    console.log(para + " - Value:", control_value);
                }

                if (control_value != null) {
                    payload.push(control_value);
                    console.log("Payload updated:", payload);
                }
            }
        }
        console.log("Final Payload for callback:", payload);
        cbp_grid.PerformCallback('filter|' + id + '|' + payload.join(','));
        console.log("Filter callback performed.");
    },

    grid: {
        refresh: function(_s, _e) {
			var paras = $(".hid_paras").val();
			if(paras == "")
				{
				console.log("Refreshing grid for report:", engine_client.chosenReport);
				please_wait("start", "Loading fresh copy of report " + engine_client.chosenReport);
				cbp_grid.PerformCallback('load|' + engine_client.chosenReport + '|');
				console.log("Grid refresh callback performed.");
				}
        }
    },

    menu_click: function(_s, _e) {
        console.log("Menu item clicked. Item details:", _e.item);
        if (_e.item.items.length == 0) {
            engine_client.chosenReport = _e.item.name;
            console.log("Chosen report updated to:", engine_client.chosenReport);
            cbp_grid.PerformCallback('load|' + _e.item.name + "|");
            console.log("Menu click callback performed.");
        }
    },

    bind_permitted_users_tooltip: function() {
		$(".permitted_users").css({"display":"block"});
        console.log("Binding tooltips to permitted users.");
        $(".permitted_users").tip();
    },

    callback: {
        init: function(s, e) {
            console.log("Initializing callback.");
        },
        begin: function(s, e) {
            console.log("Callback begun.");
            // Placeholder for future implementation
        },
        end: function(s, e) {
            console.log("Callback ended.");
			$(".hid_report_id").val(engine_client.chosenReport);
			$(".hid_layout").val(gv.cpExp);
            engine_client.bind_permitted_users_tooltip();
            please_wait("stop");
        },
        error: function(s, e) {
            console.log("Error encountered in callback.");
            // Placeholder for future implementation
        }
    }
};
