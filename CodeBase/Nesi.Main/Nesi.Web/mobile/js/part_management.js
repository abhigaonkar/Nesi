
    function resizeIframe(obj) {

        obj.height = (obj.contentWindow.document.body.scrollHeight + 5) + 'px';

        }

    $(document).ready(function () {
        set_tabs();
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
        //	$("#tab_bar").scrollLeft(200);
        $("#top_tab_bar").scroll(
            function (e) {


                $("#arrow_div").hide();

                });


        $(window).scroll(
            function (e) {
                var t = $(window).scrollTop();

                if (t > 800) {
                    $("#to_top").show();
                    }
                else {
                    $("#to_top").hide();
                    }
                });

        });


    function scroll_to_top() {
        $("html, body").animate({ scrollTop: 0 }, 250);
        }

    function EndReqHandler() {

        }

    function scroll_menu_left() {
        $("#top_tab_bar").animate({ scrollLeft: 0 }, 250);
        confirm('hello');
        }

    function set_tabs() {


        $("#top_tab_bar").scroll(
            function (e) {
                $("#arrow_div").hide();
                });

        }


    function BeginReqHandler() {
        //		document.getElementById("modal_overlay").style.display		= "";
        }
    function pop_search_open() {
        pop_search.Show();

        pop_search.PerformCallback();
        //	document.getElementById("pop_search_txt_search").focus();
        }
    function set_focus(InputID) {
        document.getElementById(InputID).focus();
        }

    function pop_part(id) {
        var x = document.getElementById("pop_part");
        x.style.visibility = 'visible';
        cb_part.PerformCallback(id);
        cb_fix_qty.PerformCallback(id);
        }
    function close_pop_part() {
        var x = document.getElementById("pop_part");
        x.style.visibility = 'hidden';
        return false;
        }
    function pop_fix_qty(id) {
        var master_id = document.getElementsByClassName("lbl_pop_part")[0].textContent;
        if (master_id == '777') {

            }
        else {
            var x = document.getElementById("pop_fix_qty");
            x.style.visibility = 'visible';
            }
        }

    function print_bc(id) {
        var master_id = document.getElementsByClassName("lbl_pop_part")[0].textContent;
        if (master_id == '777') {

            }
        else {
            cb_part.PerformCallback('print_bc|' + master_id);
            }


        }

    function check_truck_ddl() {

        if (ddl_trucks.GetValue() != null && ddl_trucks.GetValue() != '') {

            return true;
            }
        else {
            alert('You must select a location');
            return false;
            }

        }

    function close_fix_qty() {
        var x = document.getElementById("pop_fix_qty");
        x.style.visibility = 'hidden';
        return false;
        }


// first thing I notice here is "tb", nothing is being passed from the text box, so this function is now trying to get ".Id" on an undefined object... this will always error.
// You would need to pass "this" in the function call... so: check_wo(this)
// This passes the object back to the function and it can now work with it.
    function validate(tb, action) {
        var p = $(tb).parents(".tr_wo:first");
        var qty = tb.value;

        var internal_qty = p.attr("data-internal_qty");
        var truck_qty = p.attr("data-truck_qty");
        var wo_qty = p.attr("data-wo_qty");
        var master_id = p.attr("data-master_id");

        if ((master_id == 777) && (action != 'req')) {
            alert('You cannot commit 777s to work orders, you can only request them.');
            tb.value = "";
            }

        else if (action == 'wo_to_wo') {

            if (ddl_other_wo.GetValue() == '' || ddl_other_wo.GetValue() == null) {

                alert('you must select the OTHER work order first');
                tb.value = "";
            }
            else if (Number(qty) > Number(wo_qty)) {
                alert('There are only ' + wo_qty + ' on this work order.. qty has been lowered');
                tb.value = wo_qty;
            }
        }

        else if (action == 'stock_to_wo') {

            if (ddl_wo.GetValue() == '' || ddl_wo.GetValue() == null) {

                alert('you must select a work order first');
                tb.value = "";
                }
            else if (Number(qty) > Number(internal_qty)) {
                alert('There are only ' + internal_qty + ' in stock.. this will drive the stock qty BELOW zero');
                //   tb.value = internal_qty;
                }
            }
        else if (action == "wo_to_stock") {

            var tb_to_truck = tb.parentElement.parentElement.getElementsByClassName("to_truck")[0].getElementsByClassName("repeater_tb")[0].value;

            if ((Number(qty) + Number(tb_to_truck)) > Number(wo_qty)) {
                alert('There are only ' + wo_qty + ' on this work order.. qty has been lowered');
                tb.value = wo_qty - Number(tb_to_truck);
                }

            else if (ddl_wo.GetValue() == '' || ddl_wo.GetValue() == null) {
                alert('you must select a work order first');
                tb.value = "";
                }
            else if (Number(qty) > Number(wo_qty)) {
                alert('There are only ' + wo_qty + ' on this work order.. qty has been lowered');
                tb.value = wo_qty;
                }

            }
        else if (action == "truck_to_wo") {
            if (ddl_wo.GetValue() == '' || ddl_wo.GetValue() == null) {
                alert('you must select a work order first');
                tb.value = "";
                return;
                }
            else if (ddl_trucks.GetValue() == '' || ddl_trucks.GetValue() == null) {
                alert('you must select a truck first');
                tb.value = "";
                return;
                }

            var tb_to_other = tb.parentElement.parentElement.getElementsByClassName("to_other")[0].getElementsByClassName("repeater_tb")[0].value;
            var tb_from_stock = tb.parentElement.parentElement.getElementsByClassName("from_stock")[0].getElementsByClassName("repeater_tb")[0].value;
            if ((Number(qty) + Number(tb_to_other)) > (Number(truck_qty) + Number(tb_from_stock))) {
                if (Number(tb_from_stock) == 0) {
                    alert('There are only ' + truck_qty + ' on this truck.. this will drive the qty on the truck BELOW zero');
                    //   tb.value = Number(tb_from_stock) + Number(truck_qty) - Number(tb_to_other);

                    }
                else {
                    if (Number(tb_to_other) > 0) {
                        alert('After taking ' + tb_from_stock + ' from stock and moving ' + tb_to_other + ' to the other truck, there are only ' + Number(Number(truck_qty) + Number(tb_from_stock) - Number(tb_to_other)) + ' on this truck.. this will drive the qty on the truck BELOW zero');
                        }
                    else {
                        alert('After taking ' + tb_from_stock + ' from stock there are only ' + Number(Number(truck_qty) + Number(tb_from_stock)) + ' on this truck.. this will drive the qty on the truck BELOW zero');
                        }
                    }
                //   tb.value = Number(tb_from_stock) + Number(truck_qty) - Number(tb_to_other);
                }
            else if (ddl_trucks.GetValue() == '' || ddl_trucks.GetValue() == null) {
                alert('you must select a truck from the drop down above.. scroll up');
                tb.value = "";
                }
            }
        else if (action == "wo_to_truck") {
            var tb_to_stock = tb.parentElement.parentElement.getElementsByClassName("to_stock")[0].getElementsByClassName("repeater_tb")[0].value;

            if ((Number(qty) + Number(tb_to_stock)) > Number(wo_qty)) {
                alert('There are only ' + wo_qty + ' on this work order.. qty has been lowered');
                tb.value = wo_qty - Number(tb_to_stock);
                }
            else if (ddl_wo.GetValue() == '' || ddl_wo.GetValue() == null) {
                alert('you must select a work order first');
                tb.value = "";
                }
            else if (Number(qty) > Number(wo_qty)) {
                alert('There are only ' + wo_qty + ' on this work order.. qty has been lowered');
                tb.value = wo_qty;
                }
            else if (ddl_trucks.GetValue() == '' || ddl_trucks.GetValue() == null) {
                alert('you must select a truck first');
                tb.value = "";
                }

            }
        else if (action == "stock_to_truck") {
            var tb_to_wo = tb.parentElement.parentElement.getElementsByClassName("to_wo")[0].getElementsByClassName("repeater_tb")[0].value;
            var tb_to_other = tb.parentElement.parentElement.getElementsByClassName("to_other")[0].getElementsByClassName("repeater_tb")[0].value;

            if (Number(qty) < 0) {
                if (Number(qty) + Number(truck_qty) < 0) {
                    alert('There are only ' + truck_qty + ' on this truck.. this will drive the truck qty BELOW zero');
                    return;
                    }

                }

            if (Number(qty) > Number(internal_qty)) {
                alert('There are only ' + internal_qty + ' in stock.. this will drive the stock qty BELOW zero');
                //    tb.value = internal_qty;
                return;
                }

            else if (((Number(tb_to_wo) + Number(tb_to_other))) > (Number(truck_qty) + Number(qty))) {

                if (Number(tb_to_other) > 0) {
                    alert('You need to take ' + Number((Number(tb_to_other) + Number(tb_to_wo)) - Number(truck_qty)) + ' from stock or the truck to move these to the other work order');
                    tb.value = (Number(tb_to_other) + Number(tb_to_wo)) - Number(truck_qty);
                    return;
                    }

                //    else {
                //           if (Number(tb_to_wo) > 0) {
                //              alert('After taking ' + tb_from_stock + ' from stock and moving ' + tb_to_wo + ' to the WO, there are only ' + Number(Number(truck_qty) + Number(tb_from_stock) - Number(tb_to_wo)) + ' on this truck.. qty has been lowered');
                //         }
                //        else {
                //           alert('After taking ' + tb_from_stock + ' from stock there are only ' + Number(Number(truck_qty) + Number(tb_from_stock)) + ' on this truck.. qty has been lowered');
                //         }
                }
            //    tb.value = Number(tb_from_stock) + Number(truck_qty) - Number(tb_to_wo);

            else if (ddl_trucks.GetValue() == '' || ddl_trucks.GetValue() == null) {
                alert('you must select a truck from the drop down above.. scroll up');
                tb.value = "";
                }

            }
        else if (action == "truck_to_stock") {
            var tb_to_wo = tb.parentElement.parentElement.getElementsByClassName("to_wo")[0].getElementsByClassName("repeater_tb")[0].value;

            if ((Number(qty) + Number(tb_to_wo)) > Number(truck_qty)) {
                alert('There are only ' + truck_qty + ' on this truck.. this will drive the qty on the truck BELOW zero');
                //   tb.value = truck_qty - Number(tb_to_wo);
                }


            else if (ddl_trucks.GetValue() == '' || ddl_trucks.GetValue() == null) {
                alert('you must select a truck from the drop down above.. scroll up');
                tb.value = "";
                }
            else if (Number(qty) > Number(truck_qty)) {
                alert('There are only ' + truck_qty + ' on this truck.. this will drive the qty on the truck BELOW zero');
                //    tb.value = truck_qty;
                }
            }
        else if (action == "truck_to_truck") {

            var tb_to_wo = tb.parentElement.parentElement.getElementsByClassName("to_wo")[0].getElementsByClassName("repeater_tb")[0].value;
            var tb_from_stock = tb.parentElement.parentElement.getElementsByClassName("from_stock")[0].getElementsByClassName("repeater_tb")[0].value;
            if (ddl_other_trucks.GetValue() == '' || ddl_other_trucks.GetValue() == null) {
                alert('you must select the other truck from the drop down above.. scroll up');
                tb.value = "";
                }

            else if ((Number(qty) + Number(tb_to_wo)) > (Number(truck_qty) + Number(tb_from_stock))) {
                var tb_to_other = tb.parentElement.parentElement.getElementsByClassName("to_other")[0].getElementsByClassName("repeater_tb")[0].value;
                if (Number(tb_from_stock) == 0) {
                    alert('There are only ' + truck_qty + ' on this truck.. this will drive the qty on the truck BELOW zero');
                    //  tb.value = Number(tb_from_stock) + Number(truck_qty) - Number(tb_to_other);
                    }
                else {
                    if (Number(tb_to_wo) > 0) {
                        alert('After taking ' + tb_from_stock + ' from stock and moving ' + tb_to_wo + ' to the WO, there are only ' + Number(Number(truck_qty) + Number(tb_from_stock) - Number(tb_to_wo)) + ' on this truck.. this will drive the qty on the truck BELOW zero');
                        }
                    else {
                        alert('After taking ' + tb_from_stock + ' from stock there are only ' + Number(Number(truck_qty) + Number(tb_from_stock)) + ' on this truck.. this will drive the qty on the truck BELOW zero');
                        }
                    }
                //   tb.value = Number(tb_from_stock) + Number(truck_qty) - Number(tb_to_wo);
                }

            }

        else if (action == "po_to_wo") {
            var remaining_qty = p.attr("data-still_needed_qty");
            var po_qty = p.attr("data-po_qty");
            var tb_to_stock = tb.parentElement.parentElement.getElementsByClassName("to_stock")[0].getElementsByClassName("repeater_tb")[0].value;
            var tb_to_truck = tb.parentElement.parentElement.getElementsByClassName("to_truck")[0].getElementsByClassName("repeater_tb")[0].value;

            if ((Number(qty) + Number(tb_to_stock) + Number(tb_to_truck)) > (Number(remaining_qty))) {
                alert('After sending ' + tb_to_stock + ' to stock and ' + tb_to_truck + ' to the truck, there are only ' + Number(Number(remaining_qty) - (Number(tb_to_stock) + Number(tb_to_truck))) + ' on this PO.. qty has been lowered');
                tb.value = Number(remaining_qty) - (Number(tb_to_stock) + Number(tb_to_truck));
                }
            }
        else if (action == "po_to_stock") {
            var remaining_qty = p.attr("data-still_needed_qty");
            var po_qty = p.attr("data-po_qty");
            var tb_to_truck = tb.parentElement.parentElement.getElementsByClassName("to_truck")[0].getElementsByClassName("repeater_tb")[0].value;
            var tb_to_wo = tb.parentElement.parentElement.getElementsByClassName("to_wo")[0].getElementsByClassName("repeater_tb")[0].value;

            if ((Number(qty) + Number(tb_to_truck) + Number(tb_to_wo)) > (Number(remaining_qty))) {
                alert('After sending ' + tb_to_wo + ' to the work order and ' + tb_to_truck + ' to the truck, there are only ' + Number(Number(remaining_qty) - (Number(tb_to_wo) + Number(tb_to_truck))) + ' on this PO.. qty has been lowered');
                tb.value = Number(remaining_qty) - (Number(tb_to_wo) + Number(tb_to_truck));
                }


            }
        else if (action == "po_to_truck") {
            var remaining_qty = p.attr("data-still_needed_qty");
            var po_qty = p.attr("data-po_qty");
            var tb_to_stock = tb.parentElement.parentElement.getElementsByClassName("to_stock")[0].getElementsByClassName("repeater_tb")[0].value;
            var tb_to_wo = tb.parentElement.parentElement.getElementsByClassName("to_wo")[0].getElementsByClassName("repeater_tb")[0].value;

            if (ddl_trucks.GetValue() == '' || ddl_trucks.GetValue() == null) {
                alert('You must select the truck from the drop down above..');
                tb.value = "";
                }
            else if ((Number(qty) + Number(tb_to_stock) + Number(tb_to_wo)) > (Number(remaining_qty))) {
                alert('After sending ' + tb_to_stock + ' to stock and ' + tb_to_wo + ' to the work order, there are only ' + Number(Number(remaining_qty) - (Number(tb_to_stock) + Number(tb_to_wo))) + ' on this PO.. qty has been lowered');
                tb.value = Number(remaining_qty) - (Number(tb_to_stock) + Number(tb_to_wo));
                }

            }


        }

// Here is a boilerplate javascript object for arranging your logic:
// This is much easier to use and read... we need this to be the standard for all pages using javascript.
// The above call would now read like this: onchange="part_management.check.quantity(this);"
// I've already hooked up 
    var part_management = {
        check:
        {
            wo:
                function (_obj) // _obj really isn't needed here, but for this example I am passing it.
                    {
                    var wo_val = ddl_wo.GetValue();
                    if (wo_val == null || wo_val === "") {
                        alert('You must select a work order first');
                        }
                    },
            other_truck:
                function (_obj) // _obj really isn't needed here, but for this example I am passing it.
                    {
                    var truck_val = ddl_other_trucks.GetValue();
                    if (truck_val == null || truck_val === "") {
                        alert('You must select the other truck.. scroll up');
                        }
                    },
            quantity:
                function (_obj) {
                    var max_qty_str = $(_obj).parents("tr:first").find(".qty").text();
                    var this_qty_str = $(_obj).val();
                    var max_qty_float = parseFloat(max_qty_str);
                    var this_qty_float = parseFloat(this_qty_str);
                    var should_allow = true;
                    if (isNaN(max_qty_float)) {
                        alert("Something is wrong with the quantity on this line");
                        should_allow = false;
                        }
                    else if (max_qty_float === 0) {
                        alert("The quantity on this line is zero");
                        should_allow = false;
                        }
                    else if (max_qty_float < 0) {
                        alert("No parts exist in this location");
                        should_allow = false;
                        }
                    else if (this_qty_float > max_qty_float) {
                        alert("You are trying to use more than the available quantity");
                        should_allow = false;
                        }

                    if (!should_allow) {
                        $(_obj).val("");
                        return;
                        }
                    }
        }
    };


    function check_other_truck() {
        if (ddl_other_trucks.GetValue() == '' || ddl_other_trucks.GetValue() == null) {
            alert('You must select the other truck.. scroll up');
            //var x = document.getElementById("pop_pick_other_truck");
            //x.style.visibility='visible';
            }

        }

    function what(s) {
        alert(s);
        }
