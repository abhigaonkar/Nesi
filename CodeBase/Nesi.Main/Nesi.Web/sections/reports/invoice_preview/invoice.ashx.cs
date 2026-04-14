using nesi.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web;
using DevExpress.XtraPrinting;
using System.Collections.Specialized;

namespace Nesi.Web.sections.reports.invoice_preview
{
    /// <summary>
    /// Summary description for Invoice
    /// </summary>
    public class invoice : IHttpHandler
    {
        NameValueCollection _q;
        public void ProcessRequest(HttpContext context)
        {
            _q = context.Request.QueryString;
            var woprog_ID = _q["id"] == "null" ? 0 : Toolbox.ReturnZeroIfNull_int(_q["id"]);
            bool is_credit = !string.IsNullOrEmpty(_q["is_credit"]) && _q["is_credit"] == "True";
           var pdf = NeWOProg.GeneratePDF(woprog_ID, is_credit);        
           context.Response.Clear();
           context.Response.ContentType = "application/pdf";  
           context.Response.BinaryWrite(pdf);
           context.Response.Flush();
           context.Response.End();           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}