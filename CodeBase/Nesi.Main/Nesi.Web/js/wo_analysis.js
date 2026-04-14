function save_percentage() {
    var new_val = $('input:perc_complete').val()
    cb_posting.PerformCallback('save_perc|' + new_val);
    window.location.href = window.location.href;
}

function fix_perc(v) {
    if (v != '') {
        cb_quote.PerformCallback("p|" + v);
    }

}



function fix_men(v) {
    if (v != '') {
        cb_quote.PerformCallback("m|" + v);
    }

}

function fix_days(v) {
    if (v != '') {
        cb_quote.PerformCallback("d|" + v);
    }

}

