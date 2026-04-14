<%@ Import Namespace="System.IO"%>
<%@ Import Namespace="nesi.core" %>
<head runat="server"/>
<script language="C#" runat="server">
    NeMember myMember;
    Toolbox _Tools = new Toolbox();
    protected void Page_Load(object sender, EventArgs e)
    {
		var fileServer				= NeTaxEntity.BaseFolder(myMember.business_unit_id, false);
        if (Request.QueryString["file_id"] != null)
        {
            var file_id = Request.QueryString["file_id"];

            if (file_id.Substring(0, 2) == "WO")
            {

                myMember = new NeMember(Session["session"].ToString());
                var strPath = fileServer + @"\ProjectFolders\" + file_id;
                    if (File.Exists(strPath))
                    {

                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file_id + "\"");
                        Response.Flush();
                        Response.WriteFile(strPath);
                    }
            }
			else if (file_id.Substring(0, 1) == "C")
			{

				myMember = new NeMember(Session["session"].ToString());

                var strPath = fileServer + @"\customer_files\" + file_id;
				if (File.Exists(strPath))
				{

					Response.Clear();
					Response.ContentType = "application/octet-stream";
					Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file_id + "\"");
					Response.Flush();
					Response.WriteFile(strPath);
				}
			}
			else
            {
/*                OdbcConnection conn = NeDB.getCon();
                string strSelect = "SELECT *"
                                        + " FROM Download WHERE Download_ID=" + file_id;
                OdbcCommand com = new OdbcCommand(strSelect, conn);
                OdbcDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    try
                    {
                        myMember = new NeMember(Session["session"].ToString());
                        if (dr["Download_Private"].ToString() == "F" || myMember.AuthenticatedForPrivilege("19", "19") == true)
                        {
                            string strPath = iisServer + @"\Inetpub\Downloads\" + dr["Download_ID"].ToString() + dr["Download_Ext"].ToString();
                            if (File.Exists(strPath))
                            {

                                Response.Clear();
                                Response.ContentType = "application/octet-stream";
                                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + dr["Download_Name"].ToString() + dr["Download_Ext"].ToString() + "\"");
                                Response.Flush();
                                Response.WriteFile(strPath);

                            }
                        }

                    }
                    catch { }
                }
 */
			try
				{
				
                myMember = new NeMember(Session["session"].ToString());
				//TODO: This is wonky....
               // var strPath = iisServer + @"\Inetpub\" + file_id;
               // if (File.Exists(strPath))
               // {
			   //
               //     Response.Clear();
               //     Response.ContentType = "application/octet-stream";
               //     Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file_id + "\"");
               //     Response.Flush();
               //     Response.WriteFile(strPath);
			   //
               // }
				}
			catch(Exception ee)
            {
                _Tools.catch_error(ee);
				}
               
            }
        }
        
        
    }

</script>
<html><body></body></html>