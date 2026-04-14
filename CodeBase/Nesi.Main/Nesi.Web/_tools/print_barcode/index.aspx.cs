using System;
using nesi.core;

public partial class this_print_barcode_index : System.Web.UI.Page
{
    NeMember _member;
    NeBusinessUnit _company;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        //var _tools = new Toolbox();
        //_member = Toolbox.do_handle_authentication(_page_id);
        //_tools.dont_cache_page();
        //_company = new NeBusinessUnit(_member.business_unit_id);
        var _q = Request.QueryString;
        var OUTPUT = "";
        //_tools.set_XML_header();
        var master_id = _q["master_id"];
       

      
        if (master_id != "0")
        {
            OUTPUT = GetBarCodeHTML(master_id);      
        }
        //OUTPUT += "</MESSAGERETURN>";
        Response.Write(OUTPUT);
        Response.End();
    }






    public string GetBarCodeHTML(string master_id)
    {
        string barcode = "*" + master_id+"*";
        string Render_Html = @"<html><head><link href ='https://fonts.googleapis.com/css?family=Libre+Barcode+39+Text&display=swap' rel ='stylesheet'>
                             <style>.barcode {font-family: 'Libre Barcode 39 Text', cursive; font-size:100px; }</style></head><body><div class='barcode'>" + barcode + "</div></body></html>";
        return Render_Html;
        
    }
}
