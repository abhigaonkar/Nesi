using System;
using System.Data;
using nesi.core;

public partial class this_get_taxes : System.Web.UI.Page
{
    NeMember _member;
    NeBusinessUnit _company;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        _member = Toolbox.do_handle_authentication(_page_id);
        _company = new NeBusinessUnit(_member.business_unit_id);
        var _taxinfo = new DataTable();
        var tax1 = "0";
        var tax2 = "0";
        var tax3 = "0";
        var tax4 = "0";
        var taxname1 = "";
        var taxname2 = "";
        var taxname3 = "";
        var taxname4 = "";

        var _q = Request.QueryString;  //System.Collections.Specialized.NameValueCollection
        //string _a = Request.QueryString[1];
        var OUTPUT = "";
        _tools.set_XML_header();
        var sql = "";
        if (_q["id"] != null)
        {
            if (_q["source"] == "workorder")
            {
                sql = string.Format(@"SELECT wo_detail_current_tax1, wo_detail_current_tax2, wo_detail_current_tax3, wo_detail_current_tax4,
                                    woprog_customer_id, woprog_address_id,
                                    Address_Tax1, Address_Tax2, Address_Tax3, Address_Tax4
                                    FROM wo_detail_current, woprog, address
                                    WHERE wo_detail_current_woprog_id = woprog_id
                                    AND woprog_address_id = address_id
                                    AND wo_detail_current_ID = {0}", _q["id"]);
            }
            try
            {
               _taxinfo = _tools.getSQL_datatable (sql,null);
            }
            catch { }
            foreach(DataRow _taxinforow in _taxinfo.Rows)
            {
                tax1 = _taxinforow[0].ToString();
                tax2 = _taxinforow[1].ToString();
                tax3 = _taxinforow[2].ToString();
                tax4 = _taxinforow[3].ToString();
                if (tax1 != "0")
                {
                    try
                    {
                        taxname1 = _tools.getSQL_string(@"SELECT tax_name FROM tax  WHERE tax_id =@v0", new object[] { _taxinforow[6].ToString() });
                    }
                    catch { }
                }
                if (tax2 != "0")
                {
                    try
                    {
                        taxname2 = _tools.getSQL_string(@"SELECT tax_name FROM tax  WHERE tax_id =@v0", new object[] { _taxinforow[7].ToString() });
                    }
                    catch { }
                }
                if (tax3 != "0")
                {
                    try
                    {
                        taxname3 = _tools.getSQL_string(@"SELECT tax_name FROM tax  WHERE tax_id =@v0", new object[] { _taxinforow[8].ToString() });
                    }
                    catch { }
                }
                if (tax4 != "0")
                {
                    try
                    {
                        taxname4 = _tools.getSQL_string(@"SELECT tax_name FROM tax  WHERE tax_id =@v0", new object[] { _taxinforow[9].ToString() });
                    }
                    catch { }
                }
            }
            OUTPUT += string.Format(@"
            <taxinfo tax1       =""{0}""
             tax2               =""{1}""
             tax3               =""{2}""
             tax4               =""{3}""
             taxname1           =""{4}""
             taxname2           =""{5}""
             taxname3           =""{6}""
             taxname4           =""{7}""/>",
             tax1,                  //0
             tax2,                  //1
             tax3,                  //2
             tax4,                  //3
             taxname1,              //5
             taxname2,              //6
             taxname3,              //7
             taxname4);             //8
            Response.Write(OUTPUT);
            Response.End();

        }

    }
}
