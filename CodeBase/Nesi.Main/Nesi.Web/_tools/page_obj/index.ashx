<%@ WebHandler Language="C#" Class="index" %>

using System.Web;
using System.Collections.Specialized;
using System.Web.SessionState;
using nesi.core;

public class index : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        var _q          = context.Request.QueryString;
        var resp                        = "";
        int id, c, e, cl_process        = 0;
        if(_q.Count == 0 || string.IsNullOrEmpty(_q["a"]))
        {
            resp                        = "Invalid Request";
        }
        else
        {
            switch(_q["a"])
            {
                case "plcapture":
                    if(string.IsNullOrEmpty(_q["id"]))
                    {
                        resp                        = "Invalid Request";
                    }
                    else
                    {
                        int.TryParse(_q["id"], out id);
                        if(id == 0)
                        {
                            resp                        = "Invalid Request";
                        }
                        else
                        {
                            // Check if there is already a Render End datetime
                            c                           = Toolbox.doSQL_int(@"SELECT COUNT(id) FROM log_page WHERE id = @v0  AND render_end IS NULL", new object[] {  id } );
                            if(c == 0 || c > 1)
                            {
                                resp                        = "Invalid Request";
                            }
                            else if(c == 1)
                            {
                                try
                                {
                                    int.TryParse(_q["elements"], out e);
                                    int.TryParse(_q["cl_process"], out cl_process);
                                    Toolbox.doSQL_void(@"UPDATE log_page SET render_end = @v1 , elements = @v2 , cl_process = @v3  WHERE id = @v0  LIMIT 1", new object[] {  id, Toolbox.MySQLNow_long(), e, cl_process } );
                                    resp                        = "SUCCESS";
                                }
                                catch
                                {
                                    resp                        = "ERROR";
                                }
                            }
                        }
                    }
                    break;
                case "sell_price":
                    double qty      = 0;
                    double cost     = 0;
                    double.TryParse(_q["qty"], out qty);
                    double.TryParse(_q["cost"], out cost);
                    var business_unit_id1                = 1;
                    if (_q["business_unit_id"] != null)
                    {
                        int.TryParse(_q["business_unit_id"], out business_unit_id1);
                    }
                    if(business_unit_id1 > 0)
                    {
                        context.Session["working_business_unit_id"]     = business_unit_id1;
                    }
                    var price   = shared.GetSellPrice(cost, 0, true, qty,business_unit_id1);
                    resp                        = price.ToString();
                    break;
                case "working_company":
                    var business_unit_id                = 0;
                    int.TryParse(_q["business_unit_id"], out business_unit_id);
                    if(business_unit_id > 0)
                    {
                        context.Session["working_business_unit_id"]     = business_unit_id;
                        resp                    = "SUCCESS";
                    }
                    else
                    {
                        resp                    = "ERROR";
                    }
                    break;
                case "working_user":
                    var s           = HttpContext.Current.Session;
                    if((s["masq"] != null && ((bool) s["masq"])) || s["orig_masq_user"] != null)
                    {
                        var member_id               = 0;
                        int.TryParse(_q["member_id"], out member_id);
                        var current_user        = new NeMember(member_id);
                        s["profile"]                = current_user;
                        context.Session["working_business_unit_id"] = current_user.business_unit_id;
                        s["master_permissions"]     = null;
                        s["shown_warning"]          = null;
                        Toolbox.doSQL_void(@"UPDATE ne_session SET member_id = @v0  WHERE id = @v1  LIMIT 1", new object[] {  member_id, s["session"] } );

                        NeMember.set_global_visibility_vars(context.Session, member_id);

                        resp                    = "SUCCESS";
                    }
                    else
                    {
                        resp                    = "Invalid Request";
                    }
                    break;
                default:
                    resp                        = "Invalid Request";
                    break;
            }
        }
        Toolbox.QuickReponse(context.Response, resp);
    }

    public bool IsReusable  {get{return false;}}
}