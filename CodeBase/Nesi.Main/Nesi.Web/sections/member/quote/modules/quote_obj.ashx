<%@ WebHandler Language="C#" Class="quote_obj" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Collections.Specialized;
using nesi.core;

public class quote_obj : IHttpHandler, IReadOnlySessionState
{
    NeMember current_user;
    public void ProcessRequest(HttpContext context)
    {
        var _q          = context.Request.Form;
        current_user                    = (NeMember) context.Session["profile"];
        var _tools                  = new Toolbox();
        _tools.dont_cache_page();
        var resp                        = "";
        var quote_id                    = 0;
        var revision                    = 0;
        var line_id                     = 0;
        var r_ticks                 = 0;
        var v                       = "";
        if( _q.Count == 0 ||
            string.IsNullOrEmpty(_q["a"]) ||
            string.IsNullOrEmpty(_q["q"]) ||
            string.IsNullOrEmpty(_q["r"]) ||
            string.IsNullOrEmpty(_q["ts"])
            )
        {
            resp                        = "Invalid Request";
        }
        else
        {
            int.TryParse(_q["q"], out quote_id);
            int.TryParse(_q["r"], out revision);
            int.TryParse(_q["id"], out line_id);
            int.TryParse(_q["ts"], out r_ticks);
            var temp_dt         = new DateTime(1969, 6, 1);
            double temp_double          = 0;
            var do_save             = true;
            if(quote_id == 0 || revision == 0)
            {
                resp                    = "Invalid Request";
            }
            else
            {
                var q_ticks         = Toolbox.doSQL_datetime(@"SELECT last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] {  quote_id, revision } );
                if(r_ticks < q_ticks.Ticks)
                {
                    //resp				= "InvalidState";
                }
                DateTime last_modified;
                long ticks              = 0;
                var status_id           = Toolbox.doSQL_int(@"SELECT status_id FROM quote_master WHERE quote_id = @v0 AND revision =@v1 LIMIT 1", new object[] {
                    quote_id, revision});
                var extra           = "";
                var fields          = "";
                v                       = _q["v"];
                if(resp == "")
                {
                    switch(_q["a"])
                    {
                        case "s_customer":
                            fields          = string.Format(@"customer_id = '{0}', contact_id = null, address_id = null", v);
                            break;
                        case "s_contact":
                            fields          = string.Format(@"contact_id = '{0}'", v);
                            break;
                        case "s_datedue":
                            // Valid Date?
                            DateTime.TryParse(_q["v"], out temp_dt);
                            if(temp_dt.Year == 1969)
                            {
                                resp        = "Invalid Date";
                            }
                            else
                            {
                                fields          = string.Format(@"date_due = '{0}'", Toolbox.MySQL_shortdt(temp_dt));
                            }
                            break;
                        case "s_exp_podate":
                            // Valid Date?
                            DateTime.TryParse(_q["v"], out temp_dt);
                            if(temp_dt.Year == 1969)
                            {
                                resp        = "Invalid Date";
                            }
                            else
                            {
                                fields          = string.Format(@"exp_podate = '{0}'", Toolbox.MySQL_shortdt(temp_dt));
                            }
                            break;
                        case "s_pctchance":
                            fields          = string.Format(@"pct_chance = '{0}'", v);
                            break;
                        case "s_pctchancereason":
                            fields          = string.Format(@"pct_chance_reason = '{0}'", v);
                            break;
                        case "s_pctchancenote":
                            try
                            {
                                var wpn                 = new NeWoProgNotes(quote_id, "Q");
                                wpn.woprog_project_notes_memberid   = Toolbox.doSQL_int(@"SELECT quoted_by FROM quote_master WHERE quote_id = @v0 AND revision = @v1",new object[] {
                                    quote_id, revision});
                                wpn.woprog_project_notes_notes      = v;
                                wpn.woprog_project_notes_woprogid   = quote_id;
                                wpn.SaveWOProgProjectNote();
                                resp                                = "SUCCESS";
                            }
                            catch (Exception ee)
                            {
                                resp                                = ee.ToString();
                            }
                            break;
                        case "s_expcompletiondate":
                            // Valid Date?
                            do_save         = DateTime.TryParse(_q["v"], out temp_dt);
                            if(temp_dt.Year == 1969)
                            {
                                resp        = "Invalid Date";
                            }
                            else
                            {
                                fields          = string.Format(@"completion_date = '{0}'", Toolbox.MySQL_shortdt(temp_dt));
                            }
                            break;
                        case "s_jobdescription":
                            fields          = string.Format(@"job_description = '{0}'", Toolbox.AddSlashes(v));
                            break;
                        case "s_custspecdoc":
                            fields          = string.Format(@"cust_spec_doc = '{0}'", Toolbox.AddSlashes(v));
                            break;
                        case "s_company":
                            fields          = string.Format(@"business_unit_id = '{0}'", v);
                            break;
                        case "s_department":
                            fields          = string.Format(@"division_id = '{0}'", v);
                            break;
                        case "s_quotedby":
                            fields          = string.Format(@"quoted_by = '{0}'", v);
                            break;
                        case "s_follow_up":
                            fields          = string.Format(@"follow_up = '{0}' ", v);
                            break;
                        case "s_lockquoter":
                            fields          = string.Format(@"quoter_locked = '{0}' ", v);
                            break;
                        case "s_dateprint":
                            fields          = string.Format(@"last_print_date = {0}", v);
                            break;
                        case "s_datesent":
                            extra           = status_id == 2 ? ", status_id = 3" : "";
                            fields          = string.Format(@"last_fax_date = {0}{1}", v, extra);
                            break;
                        case "s_dateverified":
                            extra           = status_id == 3 ? ", status_id = 4" : "";
                            fields          = string.Format(@"verified_date = {0}{1}", v, extra);
                            break;
                        case "s_quotedprice":
                            do_save         = double.TryParse(v, out temp_double);
                            fields          = string.Format(@"quoted_price = '{0}'", temp_double);
                            break;
                        case "s_quotedpriceto":
                            fields          = string.Format(@"price_to = '{0}'", v);
                            break;
                        case "s_pricetype":
                            fields          = string.Format(@"pricetype_id = '{0}'", v);
                            break;
                        case "s_expvalue":
                            fields          = string.Format(@"expected_value = '{0}'", v);
                            break;
                        case "s_uscurrency":
                            fields          = string.Format(@"us_currency = {0},currency={1}", v,(v=="1"?1:2));
                            break;
                        case "s_inflationterm":
                            fields          = string.Format(@"inflation_term = {0}", v);
                            break;
                        case "s_pctdown":
                            fields          = string.Format(@"percentage_down = '{0}'", v);
                            break;
                        case "s_netdue":
                            fields          = string.Format(@"net_due = '{0}'", v);
                            break;
                        case "s_customterm":
                            fields          = string.Format(@"custom_term = '{0}'", Toolbox.AddSlashes(v));
                            break;
                        case "s_addressid":
                            fields          = string.Format(@"address_id = '{0}'", v);
                            break;
                        case "s_includetitle":
                            fields          = string.Format(@"include_title = '{0}'", v);
                            break;
                        case "s_detailrow":
                        case "s_notesrow":
                            try
                            {
                                Toolbox.doSQL_void(@"UPDATE quote_extratext SET linetext = @v0 WHERE id = @v1 LIMIT 1", new object[] {
                                    v, line_id});
                                if(_q["a"] == "s_detailrow")
                                {
                                    var detail_row          = Toolbox.doSQL_dt(@"SELECT * FROM quote_extratext WHERE id = @v0 ", new object[] {  line_id } ).Rows[0];
                                    // Need to update the associated section also.
                                    var section_id              = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(id),0) FROM quote_section WHERE detail_id = @v0",new object[] {
                                    line_id});
                                    if(section_id != 0)
                                    {
                                        var picklist_controlled = Convert.ToBoolean(Toolbox.doSQL_int(@"SELECT IFNULL(MAX(picklist_controlled),0) 
FROM quote_section WHERE id = @v0", new object[] {
                                        section_id}));
                                        if(!picklist_controlled)
                                        {
                                            var step                    = Convert.ToInt32(detail_row["line_number"]);
                                            var detail_name         = string.Format("{0}. {1}", step+1, v.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "));
                                            var len                     = detail_name.Length > 60 ? 60 : detail_name.Length;
                                            Toolbox.doSQL_void(@"UPDATE quote_section SET section = @v0 WHERE id = @v1 LIMIT 1",
                                                new object[] {
                                            detail_name.Substring(0,len), section_id});
                                        }
                                    }
                                }
                                last_modified   = Toolbox.doSQL_datetime(@"SELECT last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] {  quote_id, revision } );
                                ticks               = last_modified.Ticks;
                                resp        = "SUCCESS|"+ticks;
                            }
                            catch (Exception ee)
                            {
                                resp        = ee.ToString();
                            }
                            do_save         = false;
                            break;
                        case "update_order":
                            if(v != "")
                            {
                                var asdf            = v.Split(',');
                                foreach(var f in asdf)
                                {
                                    var bo          = f.Split('|');
                                    var id              = bo[0];
                                    var oo              = bo[1];
                                    Toolbox.doSQL_void(@"UPDATE quote_extratext SET line_number = @v1 WHERE id = @v0 LIMIT 1",new object[] {
                                    id, oo});
                                    var detail_row          = Toolbox.doSQL_dt(@"SELECT * FROM quote_extratext WHERE id = @v0 ", new object[] {  id } ).Rows[0];
                                    var detail_name         = detail_row["linetext"].ToString();
                                    var type                    = Convert.ToInt32(detail_row["type"]);
                                    if(type == 1)
                                    {
                                        // Need to update the associated section also.
                                        var section_id              = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(id),0) FROM quote_section WHERE detail_id = @v0", id);
                                        if(section_id != 0)
                                        {
                                            var picklist_controlled = Convert.ToBoolean(Toolbox.doSQL_int(@"SELECT IFNULL(MAX(picklist_controlled),0) FROM quote_section WHERE id = @v0",
                                                section_id));
                                            if(!picklist_controlled)
                                            {
                                                var step                    = Convert.ToInt32(detail_row["line_number"]);
                                                detail_name                 = string.Format("{0}. {1}", step+1, detail_name.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "));
                                                var len                     = detail_name.Length > 60 ? 60 : detail_name.Length;
                                                Toolbox.doSQL_void(@"UPDATE quote_section SET section = @v0 WHERE id = @v1 LIMIT 1",new object[] {
                                                    detail_name.Substring(0,len), section_id});
                                            }
                                        }
                                    }
                                }
                            }
                            last_modified   = Toolbox.doSQL_datetime(@"SELECT last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] {  quote_id, revision } );
                            ticks               = last_modified.Ticks;
                            resp        = "SUCCESS|"+ticks;
                            break;
                        case "do_revision":
                            var this_rev            = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(revision), 0) FROM quote_master WHERE quote_id = @v0 AND status_id != 9", quote_id);
                            if (this_rev != 0)
                            {
                                to_history(quote_id, revision, "New Revision Made");
                                this_rev            = Toolbox.doSQL_int(@"CALL RevisionQuote(@v0, @v1)", new object[] { 
	                                quote_id, revision});
                                try
                                {
                                    quote.copy_worksheet_file_to_new_rev(quote_id.ToString(), revision.ToString(), this_rev.ToString());
                                }
                                catch { }
                                Toolbox.doSQL_void(@"UPDATE quote_master SET why_revised = @v2 WHERE quote_id = @v0 AND revision = @v1 ",new object[] { 
	                                quote_id, this_rev, v});
                                resp                = "SUCCESS|"+this_rev;
                            }
                            else
                            {
                                resp                = "There isn't an active version to revise.";
                            }
                            break;
                    }
                }
                if(do_save && fields != "")
                {
                    try
                    {
                        Toolbox.doSQL_void(string.Format(@"UPDATE quote_master SET {0} WHERE quote_id = '{1}' AND revision = '{2}' LIMIT 1", fields, quote_id, revision));
                        last_modified       = Toolbox.doSQL_datetime(@"SELECT last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] {  quote_id, revision } );
                        ticks               = last_modified.Ticks;
                        resp        = "SUCCESS|"+ticks;
                    }
                    catch (Exception ee)
                    {
                        resp        = ee.ToString();
                    }
                }
            }
        }
        Toolbox.QuickReponse(context.Response, resp);
    }
    public void to_history(int quote_id, int revision, string _event)
    {
        Toolbox.doSQL_void(@"INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event) 
VALUES (now(), @v2, @v0, @v1, @v3)",new object[] { 
	        quote_id, revision, current_user.id, _event});
    }
    public bool IsReusable {get{return false;}}
}