<%@ WebHandler Language="C#" Class="include" %>

using System;
using System.IO;
using System.Web;
using nesi.core;

public class include : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
		
	    if (context.Request.QueryString["file"] != null)
		    {
		   
		    try
			    {
			    var strPath = NETickets.ticketissue.attachment_folder + context.Request.QueryString["file"];
			    if(!File.Exists(strPath)) return;
			    var fi = new FileInfo(strPath);
			    context.Response.Clear();
			    if(context.Request.QueryString["download"] == null || context.Request.QueryString["download"] == "true")
				    {
				    context.Response.ContentType = "application/octet-stream";
				    context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + context.Request.QueryString["file"]  + "\"");
				    }
			    else
				    {
				    context.Response.ContentType = NeFiles.GetMimeType(fi.Extension).TrimStart('.');								
				    }
			    context.Response.Flush();
			    context.Response.WriteFile(strPath);
			    }
		    catch(Exception ee)
			    {
				Toolbox.do_errorLog_errorStack(ee);
			    }
			}
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}