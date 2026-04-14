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

public partial class segments : System.Web.UI.Page
{
	NeMember myMember;
	public Toolbox _tools			= null;
	private const int _page_id	= 9; // from Page table in DB
    public int te_id;
    NeTaxEntity te;
    protected void Page_Load(object sender, EventArgs e)
		{
        var _q = Request.QueryString;
        te_id = Convert.ToInt32(_q["te_id"]);
        te = new NeTaxEntity(te_id);
        Title = te.public_name + " GL Segments";
		myMember				= Toolbox.do_handle_authentication(_page_id);
		if (!IsPostBack)
			{
			

			}
		}




}