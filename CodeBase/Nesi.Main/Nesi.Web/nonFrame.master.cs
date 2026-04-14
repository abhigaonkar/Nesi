using System;
using System.Collections.Specialized;
using System.Web.UI;
using nesi.core;

public partial class nonFrame : System.Web.UI.MasterPage
	{
	NeMember myMember;
	NameValueCollection _q;
	public bool is_mobile { get; set; }
	ne_page_log pl = new ne_page_log();
	protected void Page_PreInit(object sender, EventArgs e)
		{
		Page.Theme = "";
		}

	protected void Page_Init(object sender, EventArgs e)
		{
		try
			{
			if (Session["session"] != null)
				{
				myMember = new NeMember(Session["session"].ToString());
				pl.ip_address = Request.UserHostAddress;
				pl.member_id = myMember.id;
				pl.url = Request.Url.AbsolutePath;
				pl.query_string = Request.Url.Query;
                pl.request_start = DateTime.Now;
                    pl.save();
                    pl_id.Value = pl.id.ToString();
                
            }
        }
		catch (Exception ee)
			{
			Toolbox.do_errorLog_errorStack(ee);
			}
		_q				= Request.QueryString;
		this.is_mobile = Request.Browser.IsMobileDevice || (_q["is_mobile"] != null && _q["is_mobile"] == "1");
		}

	protected void Page_Load(object sender, EventArgs e)
		{
		// NeMember myMember = new NeMember(Session["session"].ToString());
		//string path = HttpContext.Current.Request.Url.Host;
		if(!is_mobile)
			{
			functionsjs.InnerHtml		= @"
		<script type='text/javascript'>
			var ts_start = new Date().getTime();
			window.alert = function (msg, duration, run_script) {
				page_obj.global_message.alert(msg, duration, run_script);
			}
			



function HideProgress() {
	var loading = $('#top_level_modal_progress');
	loading.hide();
	var modal = $('#top_level_modal_modal');
	modal.remove();
}

function ShowProgress() {
	if(typeof Sys !== 'undefined')
		{
		setTimeout(function () {
			var modal = $('<div id=""top_level_modal_modal"" />');
			modal.addClass(""modal"");
			$('body').append(modal);
			var loading = $(""#top_level_modal_progress"");
			loading.show();
			var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
			var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
			loading.css({ top: top, left: left });
		}, 200);
		}
	if(typeof Sys !== 'undefined')
		{
		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(HideProgress);
		}

}


$('form').live('submit', function () {
	ShowProgress();
});

$('form').live(""input[type = 'submit']"", function () {
	ShowProgress();
});

$('form').live(""button[type = 'submit']"", function () {
	ShowProgress();
});

			
		</script>
";
			    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "set_ico",

			        @"var link = document.getElementById('ctl00_ico');
            
	       
	        link.rel = 'shortcut icon';
	        link.href = '/images/Logos/" + new Current_User().logo_icon + @"'
   


	        ", true);

        }
		}
	protected void Page_Unload(object sender, EventArgs e)
		{
		pl.request_end = DateTime.Now;
			
			pl.save();
			
		}



	}
