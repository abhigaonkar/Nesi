using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Configuration;
//using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.IO;
//using System.DirectoryServices;
using System.Linq;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using nesi.core;

//using System.Reflection;
//using System.Collections.Generic;
//using PropertyCollection = System.Data.PropertyCollection;
//using System.Text.RegularExpressions;
//using SharpSvn;
//using System.Net;
//using SharpSvn.Security;
//using System.Collections.ObjectModel;
//using System.Security.Principal;
//using Cassia;
//using Company.Security;
//using System.Data.OleDb;
//using Pervasive.Data.SqlClient;

//using System.Runtime.CompilerServices;
//using System.Drawing;
//using System.IO;
//using System.Net.Mail;
//using DevExpress.XtraReports.UI;
using DevExpress.Xpo;
//using nesi.bv;
using Pervasive.Data.SqlClient;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Configuration;
using MySql.Data.MySqlClient;
using ne_xpo.cs;

//using DevExpress.Data.Filtering;
//using DevExpress.Data.Linq;
//using System.Data.Objects.SqlClient;

public partial class test : System.Web.UI.Page
    {
    private Toolbox _tools;
	bool allow = true;
	NameValueCollection _q;
	protected void Page_Init(object sender, EventArgs e)
		{
		_q = Request.QueryString;
		var member = Toolbox.do_handle_authentication(1);
		allow= new[] { 711, 1295, 2739 }.Contains(member.id);
	}
	protected void Page_Load(object sender, EventArgs e)
		{
		if(!allow)
			{
			Response.Redirect("/index.html", true);
			}
        _tools = new Toolbox();
        //invoiced_workorders();
        //correct_quote_worksheets();
        //correct_inventory_cost();
        //correct_inventory_branch();
        //inventory_cost_fix();
        //get_errant_rows();

        //  slowestClass();
        //fastestString();
        /*var pass = "";
        var unencrypted_pass	= Toolbox.DecryptString(pass, Toolbox.config_setting("encryption_pass"));
        Toolbox.QuickReponse(Response, unencrypted_pass);*/

        //var typepriv = new NEMemberTypePrivilege();
        //						typepriv.ClearPrivileges(2025);
        //						typepriv.SetMemberPrivileges(2025);
        //sync_customers();
        //sync_vendors();
        //sync_terms();
        //sync_taxes();
        //using (var uow = new UnitOfWork())
        //	{
        //	var customersList		= (from cust in new XPQuery<ne_xpo.cs.customer>(uow)
        //							  where new []{0,1,2,3,7,8,9}.Contains(cust.customer_status.customer_or_contact_status_id)
        //							  select cust.customer_id).ToList();
        //	
        //	Parallel.ForEach(customersList, new ParallelOptions {MaxDegreeOfParallelism = 3 }, _id =>
        //		{
        //		try
        //			{
        //			var c = new NECustomer(_id);
        //			c.sync_bvs(new NeMember(711));
        //			}
        //		catch
        //			{
        //			}
        //		});
        //	}
        //using (var conn = Toolbox.connect())
        //	{
        //	var dt = Toolbox.doSQL_dt(conn, "SELECT id FROM business_unit WHERE FIND_IN_SET(id, @v0)", new object[] {"1,2,3,4,7,8"});
        //	Response.Write(dt.Rows.Count.ToString());
        //	}

        //using (var bv_conn = BVDB.connect("NSNEEI"))
        //	{
        //	var test = BVDB.getSQL_int(bv_conn, @"SELECT COUNT(*) FROM customer WHERE cus_no = @v0 ", new object[] { 100468 });
        //	Response.Write(test.ToString());
        //	}
        //using (var uow = new UnitOfWork())
        //	{
        //	var taxEntities = (from te in new XPQuery<ne_xpo.cs.tax_entity>(uow)
        //		where te.is_active
        //		select te).ToList();
        //	foreach (var te in taxEntities)
        //		{
        //		NeTaxEntity.CheckEntityFolderStructure(te.id);
        //		}
        //	}
        /*var cust = new NECustomer(2361);
		var w = DateTime.Now;
		var x = cust.Get_outstanding_balance(45, cust.Customer_Number, "NSNEEI");
		var y = DateTime.Now;
		var z = y.Subtract(w).TotalMilliseconds;
		var aa = 2;*/
        //NeWOProg.CheckBvHeader(new NeWOProg(1017219), new NeMember(711));
        //	var subFolderList = new[]{
        //		"applicant_files",
        //		"asset_pics",
        //		"branch_files",
        //		"credit_card_receipts",
        //		"customer_asset_files",
        //		"customer_files",
        //		"discipline_files",
        //		"ERItemFolders",
        //		"files",
        //		"ftp",
        //		"inventory_files",
        //		"member_files",
        //		"MessageboardImages",
        //		"pos",
        //		"ProjectFolders",
        //		"quote_store",
        //		"quote_worksheet_files",
        //		"safety_files",
        //		"task_files",
        //		"training_documents",
        //		"training_files",
        //		"TrainingVideos",
        //		"vendor",
        //		"videos",
        //		"workorder",
        //		"wos"
        //		};

        // pos

        // wos
        //var di		 = new DirectoryInfo(@"G:\WOs\Invoiced");
        //var pdf_list = di.GetFiles("*.*");
        //var only_num = new Regex(@"^[0-9]+\.pdf$");
        //using (var conn = Toolbox.connect("Sql Server Mode=True;server=ne-azmysql-01.newelectric.local;user id=root; password=nepass98;database=neintranet;default command timeout=180;allow user variables=true;"))
        //	{
        //	foreach (var pdf in pdf_list)
        //		{
        //		var isIllegal = !only_num.IsMatch(pdf.Name);
        //		if(!isIllegal)
        //			{
        //			var id		= pdf.Name;
        //			var exists	= Toolbox.do_dt(conn, string.Format("SELECT * FROM woprog WHERE woprog_id = '{0}'", id));
        //			if(exists.Rows.Count == 0)
        //				{
        //				isIllegal = true;
        //				}
        //			}
        //		if(isIllegal)
        //			{
        //			File.Move(pdf.FullName, @"G:\WOs\Invoiced\illegal_name\"+pdf.Name);
        //			}
        //		else
        //			{
        //			
        //			}
        //		}
        //	}
        //}
        //var te			= new NeTaxEntity(2);
        //foreach( DataRow dr in te.business_units.Rows)
        //	{
        //	var bu		= new NeBusinessUnit(dr["id"]);
        //	}



		       //sync_vendors();
		       // sync_customers();
			   iterateAppSettings();
				iterateConnections();
            // only run it in your sandbox
            // this.sync_territories();
			//HandleOrbisExpenseReimbursements();
    }
	private void HandleOrbisExpenseReimbursements()
		{
		var businessUnit = new NeBusinessUnit(_q["business_unit_id"]);
		NeTaxEntity.CheckEntityFolderStructure(businessUnit.tax_entity_id);
		var teFolder = NeTaxEntity.BaseFolder(businessUnit.id, false);
		var files = Directory.GetFiles($@"{teFolder}\expense_receipts\");
		var allowedTypes = new []{".pdf", ".jpeg", ".jpg", ".png", ".xlsx", ".docx", ".doc", ".xls"};
		foreach(var f in files)
			{
			var fileType = Path.GetExtension(f).ToLower();
			if(!Toolbox.Contains(fileType, allowedTypes))
				{
				push_response("Invalid file type - "+fileType);
				continue;
				}
			var fileName = Path.GetFileNameWithoutExtension(f);
			int.TryParse(fileName, out int expenseId);
			if(expenseId == 0)
				{
				push_response("Invalid file name (not an expense id) - "+fileName);
				continue;
				}
			var exp = new nesi.core.payroll.expense(expenseId);
			if(exp.approved == 0 || exp.approved == -1)
				{
				push_response($"Expense {(exp.approved == -1 ? " hasn't been dealt with " : " was already denied " )} ({fileName}) - Skipping");
				continue;
				}
			if(!exp.has_file)
				{
				push_response($"Expense does not have a file ({fileName}) - Skipping");
				continue;
				}
			if(exp.woprog_id == 0)
				{
				push_response($"Expense is not connected to a work order ({fileName}) - Skipping");
				continue;
				}
			var WorkOrderObj = new NeWOProg(exp.woprog_id);
			var employee = new NeMember(exp.id_member);
			var added = nesi.core.payroll.expense.AddToProjectFolder(f, WorkOrderObj, exp, employee);
			push_response($"{f} has been added to folder?: {added}");
			}
		}
	private void iterateAppSettings()
	{
		push_response("<h1>Retrieving App Settings</h1>");
		var conf = ConfigurationManager.AppSettings;
		foreach (var key in conf.AllKeys)
		{
			push_response(key + "=>" + conf[key]);
		}
	}
	private void iterateConnections()
	{
		push_response("<h1>Retrieving Connection Strings</h1>");
		var conf = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>();
		foreach (var key in conf)
		{
			push_response(key.Name + "=" + key.ConnectionString);
		}
	}
	void sync_territories()
    {
        List<int> list = new List<int> { 1, 4, 7, 22, 23, 25, 28, 38, 112, 120, 49, 50, 51, 52, 53, 58 };
        foreach (var item in list)
        {
          //  NeBusinessUnit.ValidateTerritory("BV7Debug", new NeBusinessUnit(item));
        }

    }

    public static void CheckBvHeader(NeWOProg _nesiwo, NeMember _user)
		{
		/*
		var bvPacket = new NeSalesOrder.bv_wo_packet();
		var businessUnit = new NeBusinessUnit(_nesiwo.business_unit_id);
		try
			{
			bvPacket.address = new NEAddress(_nesiwo.woprog_Address_ID);
			bvPacket.customer = new NECustomer(_nesiwo.WOProg_Customer_ID);
			bvPacket.customer_po = _nesiwo.PONumber;
			bvPacket.division = businessUnit.gl_div;
			bvPacket.user = _user;
			bvPacket.quote_number = _nesiwo.QuoteID;
			bvPacket.er_job_id = _nesiwo.woprog_ERID.ToString();
			bvPacket.business_unit = businessUnit;
			bvPacket.description = _nesiwo.Description;
			bvPacket.is_progress = _nesiwo.woprog_associate_woprog_id > 0;
			bvPacket.quote_percent = _nesiwo.quo;
			bvPacket.progress_bill_wo = _nesiwo.woprog_associate_woprog_id > 0 ? new NeWOProg(_nesiwo.woprog_associate_woprog_id) : new NeWOProg();
			bvPacket.progress_bill_type = cbopb_type.Text;
			bvPacket.quote_flat_amount = ;
			bvPacket.is_credit = _nesiwo.woprog_iscredit == 1;
			bvPacket.is_down_payment = chkDownPayment.Checked;
			bvPacket.mysql_wo = _nesiwo;
			bvPacket.dsn = businessUnit.DSN;
			bvPacket.order_number = _nesiwo.woprog_id.ToString().PadLeft(10, '0');
			var bv_wo_bs = NeSalesOrder.create_bv_wo(bvPacket, ref error_level, ref soi_quote);
			if (!bv_wo_bs.success)
				{
				throw new Exception(bv_wo_bs.message);
				}
			}
		catch (Exception ee)
			{
			shared.alert_debug("Error cutting workorder", ee.ToString());
			return;
			}
		*/
		}
	private void sync_taxes()
		{
		using (var uow = new UnitOfWork())
			{
			var taxList = from t in new XPQuery<ne_xpo.cs.tax>(uow)
						  select t;
			foreach(var t in taxList)
				{
				var tax = new NeTax(t.tax_id);
				//tax.Save_to_bv();
				}
			}
		}

	private void sync_terms()
		{
		//using (var bvConn = BVDB.connect("NSBV7D"))
			{
		//	NeAccounting.Terms.sync_bv(bvConn);
			}
		}

	public static int getSQL_int(PsqlConnection connection)
		{
			using(var comm = connection.CreateCommand())
				{
				comm.CommandText = "SELECT COUNT(*) FROM CUSTOMER WHERE CUS_NO = ? and name = ?";
				comm.Parameters.AddWithValue("v0", 100468);
				comm.Parameters.AddWithValue("v1", "Test Customer2");
				var r = comm.ExecuteScalar();
				var i = 0;
				if (r != null)
					{
					int.TryParse(r.ToString(), out i);
					}
				return i;
				}
			}
	static public void AddParametersToComm(DbCommand comm, object[] paramObjects)
		{
			if (paramObjects == null) return;
			for (int i = 0; i < paramObjects.Length; i++)
			{
				DbParameter param = comm.CreateParameter();
				param.ParameterName = "?v";
				if (paramObjects[i] is String)
					paramObjects[i] = paramObjects[i].ToString().Replace("''", "'");
				param.Value = paramObjects[i];
				comm.Parameters.Add(param);
			}
		}
	private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
		// Get the subdirectories for the specified directory.
		DirectoryInfo dir = new DirectoryInfo(sourceDirName);

		if (!dir.Exists)
			{
			throw new DirectoryNotFoundException(
				"Source directory does not exist or could not be found: "
				+ sourceDirName);
			}

		DirectoryInfo[] dirs = dir.GetDirectories();
		// If the destination directory doesn't exist, create it.
		if (!Directory.Exists(destDirName))
			{
			Directory.CreateDirectory(destDirName);
			}

		// Get the files in the directory and copy them to the new location.
		FileInfo[] files = dir.GetFiles();
		foreach (FileInfo file in files)
			{
			string temppath = Path.Combine(destDirName, file.Name);
			file.CopyTo(temppath, false);
			}

		// If copying subdirectories, copy them and their contents to new location.
		if (copySubDirs)
			{
			foreach (DirectoryInfo subdir in dirs)
				{
				string temppath = Path.Combine(destDirName, subdir.Name);
				DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}
	double total;
	int current;
	double progress = 0.0;
	object lockTarget = new object();

	private void push_response(string _text)
		{
		    
		        Response.Write(_text + "<br/>");
		        Response.Flush();
		        
		    }
	private void updateProgress(int _c, string _mess)
		{
		progress = _c/total;
		push_response(_c+" of "+total+"|"+Toolbox.MySQLNow_long()+" | "+_mess+" | "+progress.ToString("N10"));
		}
	private void sync_customers()
		{
		using (var uow = new UnitOfWork())
			{
			var customersList		= (from cust in new XPQuery<ne_xpo.cs.customer>(uow)
									  where new []{0,1,2,3,7,8,9}.Contains(cust.customer_status.customer_or_contact_status_id)
									  select cust.customer_id).ToList();
			total = customersList.Count;
			var user = new NeMember(1316);
			Parallel.ForEach(customersList, _id =>
				{
				try
					{
                        var c = new NECustomer(_id);
					//c.sync_bv("NSNEEI", user);
					}
				catch(Exception ee)
					{
					Toolbox.do_errorLog(ee);
					}
				finally
					{
					lock(lockTarget) {updateProgress(++current, "");}
					}
				});
			}
		}

	    private void sync_vendors()
	        {

	        using (var uow = new UnitOfWork())
	            {
	            var vendorsList = (from vend in new XPQuery<vendor>(uow)
	                    select vend.vendor_id).OrderBy(x => x)
	                .ToList();
	            total = vendorsList.Count;
	            var te = new NeTaxEntity(2);
            
	            //Parallel.ForEach(vendorsList, new ParallelOptions {MaxDegreeOfParallelism = 8}, _id =>
	            //    {
	            //    var bv_conn = new PsqlConnection(BVDB.ConStr +
	            //                                            ";Min Pool Size=0;Max Pool Size=100;Pooling=true;dbq=" +
	            //                                            te.DSN);
	                    
	            //        bv_conn.Open();
	            //        var my_conn =
	            //            new MySqlConnection(Toolbox.str_connection_string+
	            //                                "pooling=true;Min Pool Size=0;Max Pool Size=100;");
	                        
	            //            my_conn.Open();
	            //            var user = new NeMember(1316);
	            //            var mess = "synced";
	            //            try
	            //                {
             //                       var v = new NEVendor(_id);
             //                       v.sync_bv(bv_conn, my_conn, te, user, true);
             //           }
             //           catch (Exception ee)
	            //                {
	            //                mess = ee.Message;
	            //                updateProgress(current, mess);
	            //                }
	            //            finally
	            //                {
	            //                 my_conn.Close();
	            //                 bv_conn.Close();
	            //                 if (current % 10 == 0 || mess != "synced")
	            //                 updateProgress(++current, mess);
	            //                 else
	            //                    current++;
	            //                }
	                        
	                    
	            //    });
	            }
	        updateProgress(0, "DONE");
	        }
	    }
