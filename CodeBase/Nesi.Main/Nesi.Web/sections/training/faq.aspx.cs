using System;
using System.Text;
using System.Data;
using System.Web.Services;
using nesi.core;

public partial class sections_training_faq : System.Web.UI.Page
	{
	private NeMember current_user;
	private const int _page_id		= 1; // from Page table in DB
	private const string _page_name		= "FAQ";
	Toolbox _tools;

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		current_user						= Toolbox.do_handle_authentication(_page_id);
		var menu							= new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(current_user);
		_tools.dont_cache_page();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var sb			= new StringBuilder();
		if(current_user.isContact)
			{
			sb.Append("<div class='head'>Categories</div>");
			var sections		= Toolbox.doSQL_dt(@"SELECT a.id, a.name, COUNT(b.id) c FROM faq_section a LEFT JOIN faq b ON b.section_id = a.id AND b.customer_visible = TRUE GROUP BY a.id HAVING c > 0"  , null);
			foreach(DataRow section in sections.Rows)
				{
				var section_id		= section["id"];
				var name			= section["name"];
				var c				= section["c"];
				var plural			= (int) c > 1 ? "Articles" : "Article";
				sb.AppendFormat(@"<div class='g'><div class='s' data-level='{1}'><span class='d' onclick='faq.expand_section(this)'>[+]</span> <a href='javascript:void(0)' data-is_expanded='false' onclick='faq.expand_section(this);'>{0} ({2} {3})</a></div>", name, section_id, c, plural);
				var articles	= Toolbox.doSQL_dt(@"SELECT id, subject FROM faq WHERE section_id = @v0  AND customer_visible = TRUE", new object[] {  section_id } );
				foreach(DataRow article in articles.Rows)
					{
					var id			= article["id"];
					var subject		= article["subject"];
					sb.AppendFormat(@"<div class='a'>- <a href='javascript:void(0)' data-level='{0}' onclick='faq.view_article.run(this);'>{1}</a></div>", id, subject);
					}
				sb.Append("</div>");
				}
			}
		else
			{
			sb.Append("<div class='head'>Categories</div>");
			var sections		= Toolbox.doSQL_dt(@"SELECT a.id, a.name, COUNT(b.id) c FROM faq_section a LEFT JOIN faq b ON b.section_id = a.id GROUP BY a.id HAVING c > 0"  , null);
			foreach(DataRow section in sections.Rows)
				{
				var section_id		= section["id"];
				var name			= section["name"];
				var c				= Convert.ToInt32(section["c"]);
				var plural			= c > 1 ? "Articles" : "Article";
				sb.AppendFormat(@"<div class='g'><div class='s' data-level='{1}'><span class='d'>[+]</span> <a href='javascript:void(0)' data-is_expanded='false' onclick='faq.expand_section(this);'>{0} ({2} {3})</a></div>", name, section_id, c, plural);
				var articles	= Toolbox.doSQL_dt(@"SELECT id, subject FROM faq WHERE section_id = @v0 ", new object[] {  section_id } );
				foreach(DataRow article in articles.Rows)
					{
					var id			= article["id"];
					var subject		= article["subject"];
					sb.AppendFormat(@"<div class='a'>- <a href='javascript:void(0)' data-level='{0}' onclick='faq.view_article.run(this);'>{1}</a></div>", id, subject);
					}
				sb.Append("</div>");
				}
			}
		faq_sections.InnerHtml		= sb.ToString();
		}
	[WebMethod]
	public static string get_article(int id)
		{
		var f		= new faq(id);
		return f.body;
		}
	}