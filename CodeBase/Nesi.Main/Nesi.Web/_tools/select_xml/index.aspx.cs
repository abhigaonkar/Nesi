using System;
using System.Data;
using System.Collections.Specialized;
using System.Web;
using nesi.core;

public partial class select_xml : System.Web.UI.Page
	{
	NeMember myMember;
	private const int _page_id			= 1; // from Page table in DB
    protected void Page_Load(object sender, EventArgs e)
		{
		var _tools					= new Toolbox();
		myMember						= Toolbox.do_handle_authentication(_page_id);
		var func				= new this_functions();
		DataTable _dt;
		var _q			= Request.QueryString;
		_tools.set_XML_header();
		if(	_q["query"] != null && 
			_q["query"].ToUpper().Contains("UPDATE") == false && 
			_q["query"].ToUpper().Contains("INSERT") == false && 
			_q["query"].ToUpper().Contains("TRUNCATE") == false && 
			_q["query"].ToUpper().Contains("DELETE") == false && 
			_q["query"].ToUpper().Contains("DROP") == false && 
			_q["query"].ToUpper().Contains("ALTER") == false && 
			_q["query"].ToUpper().Contains("GRANT") == false && 
			_q["query"].ToUpper().Contains("CREATE") == false && 
			_q["query"].ToUpper().Contains("DESC") == false && 
			_q["query"].ToUpper().Contains("FLUSH") == false)
			{
			_dt							= _tools.getSQL_datatable(HttpUtility.UrlDecode(_q["query"])  , null);
			if(_dt != null)
				{
				if(_dt.Rows.Count > 0)
					{
					_dt.TableName				= "ROW";
					_dt.WriteXml(Response.OutputStream);
					}
				else
					{
					Response.Write("<error>Query returned zero rows</error>");
					}
				}
			else
				{
				Response.Write("<error>Bad SELECT statement</error>");
				}
			}
		else
			{
			Response.Write("<error>Query not provided</error>");
			}
		Response.End();
		}
	}

public class this_functions
	{
	public string _sql				= "";
	private	DataTable _dt;
	Toolbox _tools					= new Toolbox();
	
	public DataTable get_values(string _table)
		{
		try
			{
			_dt			= _tools.getSQL_datatable(_sql  , null);
			}
		catch
			{
			_dt			= null;
			}
		return _dt;
		}
	
	}