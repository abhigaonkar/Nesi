using Pervasive.Data.SqlClient;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
//using nesi.bv;
using nesi.core;
using System.Collections;
using System.Collections.Specialized;
using ne_xpo.cs;

public partial class tax_entities_business_units : System.Web.UI.Page
{
	

    NeMember myMember;
    public Toolbox _tools = new Toolbox();
    private const int _page_id = 9; // from Page table in DB
    NameValueCollection _q;
    protected void Page_Init(object sender, EventArgs e)
    {
        _q = Request.QueryString;
        hdn_id.Value = _q["te_id"];
        myMember = Toolbox.do_handle_authentication(_page_id);
        sql_member.SelectCommand =
            @"Select member.member_id id ,concat(member_fullname,' (',b.ddl_name,' - ',mt.membertype_name,')') _name 
        from member inner join business_unit b on b.id = member.business_unit_id 
        inner join membertype mt on mt.membertype_id = member.member_membertype_id 
        where member_status='Active' and find_in_set(member.business_unit_id,'" + new Current_User().visible_business_units + "')";
        sql_member.DataBind();
        sql_gl.SelectCommand =
            "Select gl_te.id, concat(gl_te.account_no,'-',gl_te.gl_chart_name) _name from gl_te where gl_te.tax_entity_id = " +
            hdn_id.Value;


        sql_bu2.SelectCommand = "Select id, ddl_name _name from business_unit where tax_entity_id = " + hdn_id.Value +
                                " and active='T' order by ddl_name";

        if (!IsPostBack)
        {
        
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

      
      
  gv_bu.DataBind();
        NeTaxEntity.CheckEntityFolderStructure(Convert.ToInt32(hdn_id.Value));
    }








    protected void gv_bu_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
    {

        // verify the division exists in BV

        // verify that the GL_chart of accounts exists in BV

        // verify GL_Chart of accoutns exists in Mysql
    try
        {
       NeAccounting.sync_gl_accounts(hdn_id.Value);
        }
    catch (Exception exception)
        {
        Console.WriteLine(exception.Message);
        throw;
        }
   
    }

    protected void gv_bu_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
    {
    try
        {
        NeAccounting.sync_gl_accounts(hdn_id.Value);
        NeTaxEntity.CheckEntityFolderStructure(Convert.ToInt32(hdn_id.Value));
        }
    catch (Exception exception)
        {
        Console.WriteLine(exception.Message);
            Toolbox.do_errorLog_errorStack(exception);
        throw;
        }

        
    }



    // verify the division exists in BV

    // verify that the GL_chart of accounts exists in BV

    // verify GL_Chart of accoutns exists in Mysql





    protected void gv_bu_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
     
    }
}