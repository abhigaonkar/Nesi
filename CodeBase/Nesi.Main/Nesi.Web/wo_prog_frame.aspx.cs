using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Specialized;
using NESI.Common.Models;
using nesi.core;

public partial class wo_prog_frame : System.Web.UI.Page
	{
	NeMember myMember;
	private const int _page_id = 12; // from Page table in DB
    NameValueCollection _q;
	protected void Page_Init(object sender, EventArgs e)
		{
		myMember = Toolbox.do_handle_authentication(12);
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		if (!IsPostBack)
			{

			var strPathPDF = "";
			var strPagePDF = "";

			//move justscan to progress
			if (Request.QueryString["action"] == "justscan")
				{
				var business_unit_id = Request.QueryString["business_unit_id"];
				var company = new NeBusinessUnit(Convert.ToInt32(business_unit_id));
				var strScanPath = company.WOPath;
				var scanfile = Request.QueryString["scanfile"];//.Replace("&", "");
				var testfile = new FileInfo(strScanPath + scanfile);


				if (testfile.Exists)
					{
					// copy it locally so it can be displayed
					var newfilename = scanfile.Replace("-", "").Replace("&", "");
					try
						{
						File.Move(strScanPath + scanfile, strScanPath + newfilename);
						}
					catch (Exception ee)
						{
						throw new Exception("\"" + strScanPath + scanfile + "\" is open currently and cannot be moved");
						}
					File.Copy(strScanPath + newfilename, NeTaxEntity.BaseFolder(business_unit_id, false) + @"\WOs\" + newfilename, true);
					strPagePDF = "/wo_prog_rename.aspx?woprog_id=0&business_unit_id=" + business_unit_id + "&scanfile=" + newfilename;
					strPathPDF = NeTaxEntity.BaseFolder(business_unit_id, true)+"/WOs/" + newfilename;
					}
				else
					{
					Response.Redirect("/wo_prog_edit.aspx?&business_unit_id=" + business_unit_id + "&msg=" + testfile.FullName);
					}
				}
			else if (Request.QueryString["action"] == "show")
				{
				var strID = Request.QueryString["woprog_id"];
				var wo = new NeWOProg(Convert.ToInt32(strID));
				var fromwo = "no";
				try
					{
					fromwo = Request.QueryString["fromwo"];
					}
				catch { }
				if (string.IsNullOrEmpty(wo.OrderNumber))
					{
					Toolbox.FriendlyException(Response, "Work order doesn't exist", "/default.aspx");
					}
				this.Title = wo.OrderNumber + " - " + wo.CustomerName;

				strPagePDF = "/sections/workorder/index.aspx?woprog_id=" + wo.woprog_id + "&is_n1=true&business_unit_id=" + wo.business_unit_id + "&fromwo=" + fromwo;
				if ((wo.woprog_scanned_date.Year > 2000 || wo.OrderNumber == "") && wo.Status != "Invoiced")
					{
					strPathPDF = NeTaxEntity.BaseFolder(wo.business_unit_id, true)+"/WOs/" + strID + ".pdf";
					}
				else
					{
					strPathPDF = NeTaxEntity.BaseFolder(wo.business_unit_id, true)+"/WOs/Invoiced/" + strID + ".pdf";
					}
				if (wo.Status == OpsWOStatus.Open)
					{
					strPathPDF = "/images/NoScan.pdf";
					}
               // Additional code to find out if any scan resides in the Work Pro folder              
                   
                    // If the PDF does not exist in the wos file structure
                if (!File.Exists(Server.MapPath(strPathPDF)))
                    {
                    var buid = new NeBusinessUnit(wo.business_unit_id);
                    var wopath = buid.WOPath;
                    var workProPath = Path.Combine(wopath, "WorkPro", $"{strID}.pdf");
                    NeBusinessUnit.CheckBUProcessFolderStructure(wo.business_unit_id);
                    //Is it invoiced?
                    if (wo.Status == "Invoiced")
                            {
                           
                            var teExists = NeTaxEntity.BaseFolder(wo.business_unit_id, false) + "/WOs/Invoiced/" + strID + ".pdf";// does it exist in the TE structure
                            if(!File.Exists(teExists))
                                {
                                // Does it exist in the WorkPro structure                               
                                 if (File.Exists(workProPath))
                                     {
                                    // Move it to Invoiced Folder                                    
                                     File.Move(workProPath, teExists);
                                     }
                                }
                            }
                    else
                            {
                             var teExists = NeTaxEntity.BaseFolder(wo.business_unit_id, false) + "/WOs/" + strID + ".pdf";// does it exist in the TE structure
                            if (!File.Exists(teExists))
                            {
                            // Does it exist in the WorkPro structure                               
                               if (File.Exists(workProPath))
                                {
                                // Move it to wos Folder                                    
                                File.Move(workProPath, teExists);
                               }
                            }
                    }

                    }
                    
               
				}


			//load the frames
            
			frmPage.Attributes["src"] = strPagePDF;
          
            var curFile = strPathPDF;
			if (File.Exists(Server.MapPath(curFile)))
				{
				//frmScan.Attributes["src"] = "pdf.aspx?pdf=" + Server.UrlEncode(strPathPDF);
				}           
			else
				{
				var lbl = new Label();
				lbl.Text = "Scanned file not found at " + strPathPDF;
				//frmScan.Attributes["src"] = "pdf.aspx?pdf=" + Server.UrlEncode("/images/NoScan.pdf");


				}

			}
		}
	}
