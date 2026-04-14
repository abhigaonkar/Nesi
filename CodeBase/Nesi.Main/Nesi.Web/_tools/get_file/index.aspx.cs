using System;
using System.IO;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using nesi.core;

public partial class get_file_index : System.Web.UI.Page
{
    NeMember _member;
    NeBusinessUnit _company;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        _tools.dont_cache_page();
        _member = Toolbox.do_handle_authentication(_page_id);
        _company = new NeBusinessUnit(_member.business_unit_id);
        var _q = Request.QueryString;
        var func = new this_functions();
        func.this_member = _member;
        var iframe = false;
        if (!string.IsNullOrEmpty(_q["iframe"]))
        {
            iframe = Convert.ToBoolean(_q["iframe"]);
            func.create_iframe();
        }
        if (!string.IsNullOrEmpty(_q["file_id"]))
        {
            func.get_file();
        }
    }

    public class this_functions
    {
        Toolbox _tools = new Toolbox();
        private HttpRequest _req = HttpContext.Current.Request;
        private HttpResponse _resp = HttpContext.Current.Response;
        private NameValueCollection _q = HttpContext.Current.Request.QueryString;
        public NeMember this_member { get; set; }
        //bool is_master						= true;
        public void create_iframe()
        {
            _resp.Clear();
            _resp.ClearContent();
            _resp.ClearHeaders();
            var _id = 0;
            if (_q["file_id"] == "message_from_the_president")
            {
                // Category = 28
                // Find most recent video that matches that category
                _id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(file_id), 0) FROM video  WHERE category_id = 28");
                if (_id == 0)
                {
                    Toolbox.FriendlyException(_resp, @"There currently isn't a message from the president to display.", "");
                }

            }
            else
            {
                _id = Convert.ToInt32(_q["file_id"]);
            }
            // check if exists
            var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM filestore.files WHERE id = @v0 ", new object[] { _id });
            if (c == 0)
            {
                Toolbox.FriendlyException(_resp, "This is an invalid file id", "/default.aspx");
            }
            var f = new file_store.fileObj(_id);
            if (f.ext == "mp4" || f.ext == "flv" || f.ext == "f4v")
            {
                _tools.getSQL_void(@"INSERT INTO video_watch_history (memberid,videoid,dt) VALUES (@v0 , @v1 , NOW())", new object[] { this_member.id, f.id });
            }
            var html = "";
            var sw = new StringWriter();
            using (var hw = new HtmlTextWriter(sw))
            {
                switch (f.ext.ToLower())
                {
                    case "xls":
                        _resp.Redirect("./index.aspx?file_id=" + f.id);
                        break;
                    case "xlsx":
                        _resp.Redirect("./index.aspx?file_id=" + f.id);
                        break;
                    case "doc":
                        _resp.Redirect("./index.aspx?file_id=" + f.id);
                        break;
                    case "rtf":
                        _resp.Redirect("./index.aspx?file_id=" + f.id);
                        break;
                    case "docx":
                        _resp.Redirect("./index.aspx?file_id=" + f.id);
                        break;
                    case "pdf":
                        _resp.Redirect("./index.aspx?file_id=" + f.id);
                        break;
                    case "bmp":
                    case "gif":
                    case "jpg":
                    case "jpeg":
                        html = @"
<html>
<head>
	<title>" + f.name + "." + f.ext + @"</title>
</head>
<body style='padding:0px;margin:0px;'>
<div style=""width:100%;height:96%;background-image:url('./index.aspx?file_id=" + f.id + @"');background-repeat:no-repeat;background-size:100% 100%;""></div>
<div><a href='./index.aspx?file_id=" + f.id + @"' target='_blank' style='font-size:11px;font-family:arial;font-weight:bold;'>Click to view picture, or right click to save</a></div>
</body>
</html>
";
                        break;
                    case "f4v":
                    case "flv":
                    case "mp4":
                        html = @"
<html>
<head>
	<title>" + f.name + "." + f.ext + @"</title>
</head>
<body style='padding:0px;margin:0px;'>
<center>
	<script type='text/javascript' src='/js/flow/flowplayer-3.2.11.min.js'></script>
		<a  
			 href='./index.aspx?file_id=" + f.id + @"'
			 style='display:block;width:520px;height:330px' 
			 id='player'> 
		</a> 
		<script>
			flowplayer('player', '/js/flow/flowplayer-3.2.15.swf');
		</script>
</center>
</body>
</html>";
                        break;
                }
            }
            _resp.Write(html);
            _resp.End();
        }
        public void get_file()
        {
            _resp.Clear();
            _resp.ClearContent();
            _resp.ClearHeaders();
            try
            {
                var _id = Convert.ToInt32(_q["file_id"]);
                if (Convert.ToInt32(_id) > 0)
                {
                    var f = new file_store.fileObj(_id);
                    if (f.ext == "flv" || f.ext == "f4v" || f.ext == "mp4")
                    {
                        //Toolbox.FriendlyException(_resp, @"There currently isn't a message from the president to display.", "");
                        _resp.Redirect("/_videos/" + _id + "." + f.ext);
                    }
                    else
                    {
                        _resp.ContentType = f.mime;
                        var _file = f.content;
                        _resp.AddHeader("Content-Length", _file.Length.ToString());
                        var file_name = string.Format("\"{0}.{1}\"", f.name, f.ext);
                        _resp.AddHeader("Content-Disposition", "inline;filename=" + file_name);
                        _resp.BinaryWrite(_file);
                    }
                }
            }
            catch (Exception ee)
            {
                Toolbox.do_errorLog_errorStack(ee);
                throw ee;
            }
            _resp.End();
        }
    }
}

