using System;
using nesi.core;

public partial class sections_hr_i_modules_template : System.Web.UI.UserControl
	{
	emp_checks chks = new emp_checks();
	private NeMember _current_user = new NeMember();
	public NeMember current_user { get { return _current_user; } set { _current_user = value; } }
	private member_fvr_dtl _dtl = new member_fvr_dtl();
	public member_fvr_dtl dtl { get { return _dtl; } set { _dtl = value; } }
	public int detail_id {get;set;}
	public string template_type {get;set;}
	public string template_subtype {get;set;}
	protected void Page_Init(object sender, EventArgs e)
		{
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		chks.populate_session(Page, ref chks);
		}
	public void populate(int _id)
		{
		dtl											= new member_fvr_dtl(_id);
		cb.Attributes["data-id"]					= _id.ToString();
		cb.Attributes["data-detail_id"]				= dtl.id.ToString();
		cb.Attributes["data-type"]					= template_type;
		cb.Attributes["data-subtype"]				= template_subtype;
		var hist						= new member_fvr_history(dtl.id);
		if(dtl.file_id > 0)
			{
			var f							= new file_store.fileObj(dtl.file_id);
		//	print_instructions.Visible					= f.ext.ToLower() == "pdf";
			cb.Checked									= hist.confirmed == 1;
			if(Request.Browser.IsMobileDevice)
				{
				iframe.Visible								= false;
				mobile_link.Visible							= true;
				mobile_link.Attributes["onclick"]			= "preview("+dtl.file_id+");";
				}
			else
				{
				iframe.Attributes["src"]					= "/_tools/get_file/index.aspx?file_id="+dtl.file_id+"&iframe=true";
				}
			}
		else
			{
			//print_instructions.Visible					= false;
			cb.Checked									= hist.confirmed == 1;
			iframe.Attributes["src"]					= dtl.url;
			}
		}
	}