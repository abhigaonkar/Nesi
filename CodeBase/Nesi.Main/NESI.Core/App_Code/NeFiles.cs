using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Web;
//using CrystalDecisions.Enterprise;
using DevExpress.XtraScheduler.Outlook.Interop;
using nesi.core;
using DataTable = System.Data.DataTable;
using Exception = System.Exception;
using File = System.IO.File;
using Path = System.IO.Path;

namespace nesi.core
	{
    /// <summary>
    /// Summary description for NeFiles
    /// </summary>
    // -- Standards --
    // The format for folders always contains a leading backslash, and no trailing backslashes, and the string is a verbatim string @""
    // i.e: @"\asdf";
    public class NeFiles
		{
		public int ID { get; set; }

		public string parentpage { get; set; }
		
		public NeFiles()
			{

			}

		public void copy_all(DirectoryInfo _source, DirectoryInfo _target, bool _delete_source)
			{
			if (_source.FullName.ToLower() == _target.FullName.ToLower())
				{
				return;
				}
			// Check if the target directory exists, if not, create it.
			if (!Directory.Exists(_target.FullName))
				{
				Directory.CreateDirectory(_target.FullName);
				}
			// Copy each file into it's new directory.
			foreach (var fi in _source.GetFiles())
				{
				fi.CopyTo(Path.Combine(_target.ToString(), fi.Name), true);
				if(_delete_source)
					{
					fi.Delete();
					}
				}
			// Copy each subdirectory using recursion.
			foreach (var source_sub_dir in _source.GetDirectories())
				{
				var next_target_sub_dir = _target.CreateSubdirectory(source_sub_dir.Name);
				copy_all(source_sub_dir, next_target_sub_dir, _delete_source);
				if(_delete_source && Directory.Exists(source_sub_dir.FullName))
					{
					source_sub_dir.Delete();
					}
				}
			if(_delete_source)
				{
				_source.Delete();
				}
			}
		public void copy_applicant_to_member(int _appid, int _memid)
			{
			var member					= new NeMember(_memid);
			var fileServer				= NeTaxEntity.BaseFolder(member.business_unit_id, false);
			var base_applicant_path		= fileServer + @"\applicant_files";
			var base_member_path		= fileServer + @"\member_files";
			var applicant_path			= base_applicant_path + @"\A"+_appid;
			var member_path				= base_member_path + @"\M"+_memid;
		
			if (!Directory.Exists(member_path)) // if member folder doesn't exist
				{
				verify_folders_exist(member_path, "Certificates", "Resumes", "Tests", "HR");
				}

			if (!Directory.Exists(applicant_path)) return;

			var dirs = Directory.GetDirectories(applicant_path, @"*", SearchOption.AllDirectories);
			foreach (var d in dirs) // loop through each applicant folder
				{
				var dir = Path.GetFileName(d);
				var files = Directory.GetFiles(d);
				if (string.IsNullOrEmpty(dir)) continue;
				var member_sub_dir_path	= Path.Combine(member_path, dir);
				verify_folder_exists(member_sub_dir_path);
				foreach (var file in files) // copy files into the member folder
					{
					var name = Path.GetFileName(file);
					var dest_path = Path.Combine(member_sub_dir_path, name);
					File.Copy(file, dest_path, true);
					}
				}
			}
		public void CreateFolder(int _id, string _parentpage, int _address_id, int _business_unit_id)
			{
			var fileServer				= NeTaxEntity.BaseFolder(_business_unit_id, false);
			if (_parentpage == "customer")
				{
				var base_folder = fileServer + @"\customer_files";
				var customer = new NECustomer(_id);
				var customer_folder	= string.Format(@"C{0}-{1}-{2}", customer.Customer_ID, customer.Customer_Number, clean_filename(customer.Customer_Name));
				var base_path = Path.Combine(base_folder, customer_folder);

				var by_id = Directory.EnumerateDirectories(base_folder, string.Format(@"C{0}-*", _id));
				var by_id_arr = by_id as string[] ?? by_id.ToArray();
				var l = by_id_arr.Length;

				// For handling base folder
				if(l == 0) // Just create it
					{
					verify_folders_exist(string.Format(@"\{0}\[B] - {1}", base_path, clean_filename(customer.Address.Addr1)), "Accounts Receivables", "Sales", "Correspondance", "Equipment");
					foreach (NEAddress a in customer.ShippingAddresses)
						{
						CreateFolder(_id, _parentpage, a.id, _business_unit_id);
						}
					}
				else if(l == 1) // Only one exists
					{
					// Is the matched path the same as the proposed path?
					if(by_id_arr[0] != base_path)
						{
						// Rename the matched path to be the correct name
						Directory.Move(by_id_arr[0], base_path);
						}
					}
				else if(l > 1) // Multiple exist for this id... not ideal.
					{
					var true_path_found = false;
					foreach(var p in by_id_arr)
						{
						if(p == base_path)
							{
							true_path_found	= true;
							}
						}
					if(!true_path_found) // We need to make sure the TRUE base folder exists.
						{
						verify_folders_exist(string.Format(@"\{0}\[B] - {1}", base_path, clean_filename(customer.Address.Addr1)), "Accounts Receivables", "Sales", "Correspondance", "Equipment");
						foreach (NEAddress a in customer.ShippingAddresses)
							{
							CreateFolder(_id, _parentpage, a.id, _business_unit_id);
							}
						}
					// If we get to this point, we need to merge the remaining folders into the true one.
				
					foreach(var p in by_id_arr)
						{
						if(p != base_path)
							{
							copy_all(new DirectoryInfo(p), new DirectoryInfo(base_path), true);
							}
						}
					}

				if(l > 0)
					{
					if(_address_id > 0)
						{
						var a = new NEAddress(_address_id);
						verify_folders_exist(string.Format(@"\{0}\[{2}] - {1}", base_path, clean_filename(a.Addr1), a.Type), "Accounts Receivables", "Sales", "Correspondance", "Equipment");
						}
					}
				}
			else
				{
				CreateFolder(_id, _parentpage, _business_unit_id);
				}
			}
		private static void verify_folder_exists(string _folder)
			{
			try
				{
				if(_folder.StartsWith(@"\f:\"))
					{
					_folder		= _folder.TrimStart('\\');
					}
				if(!Directory.Exists(_folder))
					{
					Directory.CreateDirectory(_folder);
					}
				}
			catch(System.Exception ee)
				{
			//	shared.alert_debug("Verify_folder_exist(s) had an exception", "Supplied folder: "+_folder+"<br/>Exception: "+ee);
				}
			}
		public static void verify_folders_exist(string _subpath, params string[] _folders)
			{
			try
				{
				verify_folder_exists(_subpath);
				foreach(var f in _folders)
					{
					verify_folder_exists(_subpath + @"\"+ f);
					}
				}
			catch(System.Exception ee)
				{
				if(HttpContext.Current != null && HttpContext.Current.Request.IsLocal) return;
	//			shared.alert_debug("Verify_folder(s)_exist had an exception", "Supplied folder: "+_folders+"<br/>SubPath: "+_subpath+"<br/>Exception:"+ee);
				}
			}
		public struct WoFolder
			{
			public const string AccountsReceivables		= "Accounts Receivables";
			public const string PartSpecs				= "Part Specs";
			public const string Correspondance			= "Correspondance";
			public const string CreditCardReceipts		= "Credit Card Receipts";
			public const string Equipment				= "Equipment";
			public const string PLCandHMI				= "PLC and HMI";
			public const string Schematics				= "Schematics";
			public const string Pictures				= "Pictures";
			public const string Safety					= "Safety";
			public const string SignatureFiles			= "signature_files";
			public const string ExpenseReceipts			= "Expense Receipts";
			public const string CustomerPurchaseOrder	= "Customer PO";
			}
		public static void chk_wo_folders(string path)
			{
			verify_folders_exist(path,	WoFolder.AccountsReceivables, 
										WoFolder.PartSpecs, 
										WoFolder.Correspondance, 
										WoFolder.CreditCardReceipts,
										WoFolder.Equipment,
										WoFolder.PLCandHMI, 
										WoFolder.Schematics, 
										WoFolder.Pictures,
										WoFolder.Safety,
										WoFolder.SignatureFiles,
										WoFolder.ExpenseReceipts,
										WoFolder.CustomerPurchaseOrder
										);
			}
		public void CreateFolder(int _id, string _parentpage, int _business_unit_id)
			{
			var fileServer				= NeTaxEntity.BaseFolder(_business_unit_id, false);
			if (_parentpage == "customer")
				{
				CreateFolder(_id, _parentpage, 0, _business_unit_id);
				}
			else if (_parentpage == "customer_assets")
				{
				var path = Path.Combine(fileServer + @"\customer_asset_files", string.Format("CA{0}", _id));
				Directory.CreateDirectory(path);
				}
			else if (_parentpage == "quote")
				{
				var path = Path.Combine(fileServer + @"\quote_store", string.Format("{0}", _id));
				verify_folders_exist(path, "Pictures");
				}
			else if (_parentpage == "workorder")
				{
				chk_wo_folders(GetProjectFolder(_id));
				}
			else if (_parentpage == "vendor")
				{
				var vend = new NEVendor(_id);
				var path = Path.Combine(fileServer + @"\vendor_files", string.Format("V{0}-{1}-{2}", _id, vend.vendor_number, clean_filename(vend.Vendor_Name)));
				verify_folders_exist(path, "Accounts Payable", "Contracts", "Correspondance", "Line Cards");
				}
			else if (_parentpage == "inventory_files")
				{
				var path = Path.Combine(fileServer + @"\inventory_files", string.Format("I{0}", _id));
				verify_folders_exist(path, "Spec Sheets");
				}
			else if (_parentpage == "quote_worksheet_files")
				{
				var path = Path.Combine(fileServer + @"\quote_store\quote_worksheet_lines", string.Format("L{0}", _id));
				verify_folders_exist(path);
				}
			else if (_parentpage == "business_unit_files")
				{
				
				var path = Path.Combine( fileServer + @"\business_unit_files", string.Format("BU{0}", _id));
				verify_folders_exist(path, "Safety", "Procedures", "Forms", "Safety");
				}
			else if (_parentpage == "member_files")
				{
				var mem = new NeMember(_id);
				var path = Path.Combine(fileServer + @"\member_files", string.Format("M{0}", mem.id));
				verify_folders_exist(path, "Certificates", "Resumes", "Tests", "HR");
				}
			else if (_parentpage == "applicant_files")
				{
				var path = Path.Combine(fileServer + @"\applicant_files", string.Format("A{0}", _id));
				verify_folders_exist(path, "Certificates", "Resumes", "Tests", "HR");
				}
			else if (_parentpage == "training_files")
			{
			    var path = Path.Combine(Toolbox.GetRequiredAppSetting("UNC_training_files"), $"TR{_id}");
				verify_folders_exist(path, "Videos", "Documentation", "Tests");
				}
			else if (_parentpage == "safety_files")
				{
				var path = Path.Combine(fileServer + @"\safety_files", string.Format("SA{0}", _id));
				verify_folder_exists(path);
				}
			else if (_parentpage == "ftp")
				{
				var mem = new NeMember(_id);
				var path = Path.Combine(fileServer + @"\ftp", string.Format("FTP{0}", mem.id));
				verify_folder_exists(path);
				}
			else if (_parentpage == "task_files")
				{
				var path = Path.Combine(fileServer + @"\task_files", string.Format("T{0}", _id));
				verify_folder_exists(path);
				}


			}
		public string GetProjectFolder(int _id, string parentpath, string folder, int address_id, int _business_unit_id)
			{
			var fileServer				= NeTaxEntity.BaseFolder(_business_unit_id, false);
			if(address_id == 0)
				{
				return GetProjectFolder(_id, parentpath, folder, _business_unit_id);
				}
			if(parentpath == "customer")
				{
				var base_folder = fileServer + @"\customer_files";
				var customer = new NECustomer(_id);
				var customer_folder	= string.Format(@"C{0}-{1}-{2}", customer.Customer_ID, customer.Customer_Number, clean_filename(customer.Customer_Name));
				var base_path = Path.Combine(base_folder, customer_folder);
				var address						= new NEAddress(address_id);
				if(address.Table_ID != customer.id)
					{
					throw new Exception("The supplied address id doesn't belong to this customer");
					}
				CreateFolder(_id, parentpath, address_id, _business_unit_id);
				return base_path+@"\["+address.Type+"] - "+clean_filename(address.Addr1);
				}
			else
				{
				return "";
				}
			}
		/// <summary>
		/// -- Work Order Only -- 
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>
		public string GetProjectFolder(int _id)
			{
			var businessUnitId = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(business_unit_id),0) FROM woprog WHERE woprog_id = @v0", new object[] {_id} );
			if(businessUnitId == 0) return "";
			return GetProjectFolder(_id, "workorder", "", businessUnitId);
			}
		public string GetProjectFolder(int _id, string parentpath, string folder, int _business_unit_id)
			{
			if (_business_unit_id == 0)
				{
				return "";
				}            
			var fileServer				= NeTaxEntity.BaseFolder(_business_unit_id, false);
				var path = "";
				string base_folder;
				if (parentpath == "customer")
					{
					base_folder = fileServer + @"\customer_files";
					var customer = new NECustomer(Convert.ToInt32(_id));
					var customer_folder	= string.Format(@"C{0}-{1}-{2}", customer.Customer_ID, customer.Customer_Number, clean_filename(customer.Customer_Name));
					var basepath = Path.Combine(base_folder, customer_folder);
					CreateFolder(_id, parentpath, _business_unit_id);
					path = basepath;
					}
				else if (parentpath == "customer_assets")
					{
					base_folder = fileServer + @"\customer_asset_files";
					path = Path.Combine(base_folder, string.Format(@"CA{0}", _id));
					}
				else if (parentpath == "workorder")
				{
				try
					{
					using (var conn = Toolbox.connect())
						{
						var quoteNumber = Toolbox.doSQL_string(conn, @"SELECT woprog_quoteid FROM woprog WHERE woprog_id = @v0", new object[] { _id });
						int quoteId = 0;
						if (quoteNumber != "0")
							{
							quote.splice(quoteNumber, out quoteId, out int quoteRev);
							}
						var BVOrderNumber = Toolbox.doSQL_string(conn, @"SELECT woprog_bvwo FROM woprog WHERE woprog_id = @v0", new object[] { _id });
						var CustomerName = Toolbox.doSQL_string(conn, @"SELECT woprog_customername FROM woprog WHERE woprog_id = @v0", new object[] { _id });
						if (_id != 0 && _business_unit_id != 0)
							{
							var fileserverWO = NeTaxEntity.BaseFolder(_business_unit_id, false);
							base_folder = fileserverWO + @"\ProjectFolders";
							// MH: Old pattern
							path = Path.Combine(base_folder, string.Format("WO{0}-{1}-{2}", _id, BVOrderNumber, clean_filename(CustomerName)));
							// MH: New pattern - Chunking based on 10K clusters
							var subFolder = _id < 1000000
												? _id < 10000
													? @"preConsolidation\0K"
													: $@"preConsolidation\{_id.ToString().Substring(0, 1)}0K"
												: _id.ToString().Substring(0, 3) + "000";
							var newPath = Path.Combine(base_folder, subFolder, _id.ToString());
							// MH: Do a little work to the old structure
							if (Directory.Exists(path))
								{
								if (Directory.Exists(newPath)) // MH: Hopefully this returns false... this indicates there is an error with the initial move
									{
									path = newPath;
									}
								else // MH: This is the expected block... the old customer folder path exists, just move it to the new path and move on
									{
									Directory.Move(path, newPath);
									path = newPath;
									}
								}
							// MH: Otherwise just use the current structure
							else
								{
								path = newPath;
								}
							chk_wo_folders(path);
							if (quoteId > 100000)
								{
								ValidateQuoteFolderExists(path, _business_unit_id, quoteId);
								}
							}
						else
							{
							throw new Exception("Work order doesn't exist");
							}
						}
					}
				catch (Exception ex)
					{
					Toolbox.do_errorLog(ex, "Error getting WO project folder.");
					}
				}
			else if (parentpath == "quote")
					{
                    
                    base_folder = fileServer + @"\quote_store";
					path = Path.Combine(base_folder, _id.ToString());
					}
				else if (parentpath == "quote_worksheet_files")
					{
					base_folder = fileServer + @"\quote_store\quote_worksheet_lines";
					path = Path.Combine(base_folder, "L" + _id);
					}
				else if (parentpath == "vendor")
					{
					var vend = new NEVendor(_id);
					base_folder = fileServer + @"\vendor_files";
					path = Path.Combine(base_folder, string.Format("V{0}-{1}-{2}", _id, vend.vendor_number, clean_filename(vend.Vendor_Name)));
					}
				else if (parentpath == "inventory_files")
					{
					base_folder = fileServer + @"\inventory_files";
					path = Path.Combine(base_folder, string.Format("I{0}", _id));
					}
			
				else if (parentpath == "member_files")
					{
					base_folder = fileServer + @"\member_files";
					path = !string.IsNullOrEmpty(folder) ? Path.Combine(base_folder, string.Format(@"M{0}", _id), folder) : Path.Combine(base_folder, string.Format(@"M{0}", _id));
					}
				else if (parentpath == "applicant_files")
					{
					base_folder = fileServer + @"\applicant_files";
					path = !string.IsNullOrEmpty(folder) ? Path.Combine(base_folder, string.Format(@"A{0}", _id), folder) : Path.Combine(base_folder, string.Format(@"A{0}", _id));
					}
				else if (parentpath == "task_files")
					{
					base_folder = fileServer + @"\task_files";
					path = Path.Combine(base_folder, string.Format(@"T{0}", _id));
					}
				else if (parentpath == "training_files")
					{
				    base_folder = Toolbox.GetRequiredAppSetting("UNC_training_files");
					path = Path.Combine(base_folder, $@"TR{_id}");
					}
				else if (parentpath == "safety_files")
					{
					base_folder = fileServer + @"\safety_files";
					path = Path.Combine(base_folder, string.Format(@"SA{0}", _id));
					}
				else if (parentpath == "ftp")
					{
					base_folder = fileServer + @"\ftp";
					path = Path.Combine(base_folder, string.Format(@"FTP{0}", _id));
					}
				else if (parentpath == "purchase_order")
					{
                    var PO = new NePOProg(_id);
                    var fileserverPO = NeTaxEntity.BaseFolder(PO.business_unit_id, false);

                    base_folder = fileserverPO + @"\po_files";
					path = Path.Combine(base_folder, _id.ToString());
					}
				else if (parentpath == "Business_Unit_Files")
				    {
				    base_folder = fileServer + @"\business_unit_files\BU" + _id;
				    path = base_folder;
				    }
				else if (parentpath == "_protected")
				    {
				    base_folder = fileServer + @"\_protected";
				    path = base_folder;

				    }
            else if (parentpath == "ticket_attachments")
            {
                base_folder = Toolbox.GetRequiredAppSetting("UNC_ticket_attachments");
                path = Path.Combine(base_folder, _id.ToString());
            }
            if (path != "" && !Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ee)
                {
					Toolbox.do_errorLog(ee, "Error creating folder");
                }

            }
            return path;
        }
		public static string clean_filename(string filename)
			{
			filename			= filename.Trim();
			var illegals		= Path.GetInvalidFileNameChars();
			return illegals.Aggregate(filename, (current, c) => current.Replace(c.ToString(), "")).TrimEnd('.');
			}
		private void ValidateQuoteFolderExists(string wo_path, int businessUnitId, int quoteId)
			{
			var fileServer = NeTaxEntity.BaseFolder(businessUnitId, false);
			string quote_path = $@"{fileServer}\quote_store\{quoteId}\";
			string wo_quote_path = Path.Combine(wo_path, $"From Quote ({quoteId})");

			// Check if quote folder already exists
			if (Directory.Exists(quote_path) && !Directory.Exists(wo_quote_path))
			    {
			    // If so check if folder has contents
			    if (Directory.GetFiles(quote_path).Length > 0)
			        {
			        Directory.Move(quote_path, wo_quote_path);
			        }
			    }
			}
		public static bool DirectoryAccessible(string _path)
			{
			try
				{
				return Directory.Exists(_path);
				}
			catch(Exception e)
				{
				Toolbox.do_errorLog_errorStack(e);
				return false;

				}
			}

		#region Big freaking list of mime types
		private static IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
																																			// combination of values from Windows 7 Registry and 
																																			// from C:\Windows\System32\inetsrv\config\applicationHost.config
																																			// some added, including .7z and .dat
																																				{".323", "text/h323"},
																																				{".3g2", "video/3gpp2"},
																																				{".3gp", "video/3gpp"},
																																				{".3gp2", "video/3gpp2"},
																																				{".3gpp", "video/3gpp"},
																																				{".7z", "application/x-7z-compressed"},
																																				{".aa", "audio/audible"},
																																				{".AAC", "audio/aac"},
																																				{".aaf", "application/octet-stream"},
																																				{".aax", "audio/vnd.audible.aax"},
																																				{".ac3", "audio/ac3"},
																																				{".aca", "application/octet-stream"},
																																				{".accda", "application/msaccess.addin"},
																																				{".accdb", "application/msaccess"},
																																				{".accdc", "application/msaccess.cab"},
																																				{".accde", "application/msaccess"},
																																				{".accdr", "application/msaccess.runtime"},
																																				{".accdt", "application/msaccess"},
																																				{".accdw", "application/msaccess.webapplication"},
																																				{".accft", "application/msaccess.ftemplate"},
																																				{".acx", "application/internet-property-stream"},
																																				{".AddIn", "text/xml"},
																																				{".ade", "application/msaccess"},
																																				{".adobebridge", "application/x-bridge-url"},
																																				{".adp", "application/msaccess"},
																																				{".ADT", "audio/vnd.dlna.adts"},
																																				{".ADTS", "audio/aac"},
																																				{".afm", "application/octet-stream"},
																																				{".ai", "application/postscript"},
																																				{".aif", "audio/x-aiff"},
																																				{".aifc", "audio/aiff"},
																																				{".aiff", "audio/aiff"},
																																				{".air", "application/vnd.adobe.air-application-installer-package+zip"},
																																				{".amc", "application/x-mpeg"},
																																				{".application", "application/x-ms-application"},
																																				{".art", "image/x-jg"},
																																				{".asa", "application/xml"},
																																				{".asax", "application/xml"},
																																				{".ascx", "application/xml"},
																																				{".asd", "application/octet-stream"},
																																				{".asf", "video/x-ms-asf"},
																																				{".ashx", "application/xml"},
																																				{".asi", "application/octet-stream"},
																																				{".asm", "text/plain"},
																																				{".asmx", "application/xml"},
																																				{".aspx", "application/xml"},
																																				{".asr", "video/x-ms-asf"},
																																				{".asx", "video/x-ms-asf"},
																																				{".atom", "application/atom+xml"},
																																				{".au", "audio/basic"},
																																				{".avi", "video/x-msvideo"},
																																				{".axs", "application/olescript"},
																																				{".bas", "text/plain"},
																																				{".bcpio", "application/x-bcpio"},
																																				{".bin", "application/octet-stream"},
																																				{".bmp", "image/bmp"},
																																				{".c", "text/plain"},
																																				{".cab", "application/octet-stream"},
																																				{".caf", "audio/x-caf"},
																																				{".calx", "application/vnd.ms-office.calx"},
																																				{".cat", "application/vnd.ms-pki.seccat"},
																																				{".cc", "text/plain"},
																																				{".cd", "text/plain"},
																																				{".cdda", "audio/aiff"},
																																				{".cdf", "application/x-cdf"},
																																				{".cer", "application/x-x509-ca-cert"},
																																				{".chm", "application/octet-stream"},
																																				{".class", "application/x-java-applet"},
																																				{".clp", "application/x-msclip"},
																																				{".cmx", "image/x-cmx"},
																																				{".cnf", "text/plain"},
																																				{".cod", "image/cis-cod"},
																																				{".config", "application/xml"},
																																				{".contact", "text/x-ms-contact"},
																																				{".coverage", "application/xml"},
																																				{".cpio", "application/x-cpio"},
																																				{".cpp", "text/plain"},
																																				{".crd", "application/x-mscardfile"},
																																				{".crl", "application/pkix-crl"},
																																				{".crt", "application/x-x509-ca-cert"},
																																				{".cs", "text/plain"},
																																				{".csdproj", "text/plain"},
																																				{".csh", "application/x-csh"},
																																				{".csproj", "text/plain"},
																																				{".css", "text/css"},
																																				{".csv", "text/csv"},
																																				{".cur", "application/octet-stream"},
																																				{".cxx", "text/plain"},
																																				{".dat", "application/octet-stream"},
																																				{".datasource", "application/xml"},
																																				{".dbproj", "text/plain"},
																																				{".dcr", "application/x-director"},
																																				{".def", "text/plain"},
																																				{".deploy", "application/octet-stream"},
																																				{".der", "application/x-x509-ca-cert"},
																																				{".dgml", "application/xml"},
																																				{".dib", "image/bmp"},
																																				{".dif", "video/x-dv"},
																																				{".dir", "application/x-director"},
																																				{".disco", "text/xml"},
																																				{".dll", "application/x-msdownload"},
																																				{".dll.config", "text/xml"},
																																				{".dlm", "text/dlm"},
																																				{".doc", "application/msword"},
																																				{".docm", "application/vnd.ms-word.document.macroEnabled.12"},
																																				{".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
																																				{".dot", "application/msword"},
																																				{".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
																																				{".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
																																				{".dsp", "application/octet-stream"},
																																				{".dsw", "text/plain"},
																																				{".dtd", "text/xml"},
																																				{".dtsConfig", "text/xml"},
																																				{".dv", "video/x-dv"},
																																				{".dvi", "application/x-dvi"},
																																				{".dwf", "drawing/x-dwf"},
																																				{".dwp", "application/octet-stream"},
																																				{".dxr", "application/x-director"},
																																				{".eml", "message/rfc822"},
																																				{".emz", "application/octet-stream"},
																																				{".eot", "application/octet-stream"},
																																				{".eps", "application/postscript"},
																																				{".etl", "application/etl"},
																																				{".etx", "text/x-setext"},
																																				{".evy", "application/envoy"},
																																				{".exe", "application/octet-stream"},
																																				{".exe.config", "text/xml"},
																																				{".fdf", "application/vnd.fdf"},
																																				{".fif", "application/fractals"},
																																				{".filters", "Application/xml"},
																																				{".fla", "application/octet-stream"},
																																				{".flr", "x-world/x-vrml"},
																																				{".flv", "video/x-flv"},
																																				{".fsscript", "application/fsharp-script"},
																																				{".fsx", "application/fsharp-script"},
																																				{".generictest", "application/xml"},
																																				{".gif", "image/gif"},
																																				{".group", "text/x-ms-group"},
																																				{".gsm", "audio/x-gsm"},
																																				{".gtar", "application/x-gtar"},
																																				{".gz", "application/x-gzip"},
																																				{".h", "text/plain"},
																																				{".hdf", "application/x-hdf"},
																																				{".hdml", "text/x-hdml"},
																																				{".hhc", "application/x-oleobject"},
																																				{".hhk", "application/octet-stream"},
																																				{".hhp", "application/octet-stream"},
																																				{".hlp", "application/winhlp"},
																																				{".hpp", "text/plain"},
																																				{".hqx", "application/mac-binhex40"},
																																				{".hta", "application/hta"},
																																				{".htc", "text/x-component"},
																																				{".htm", "text/html"},
																																				{".html", "text/html"},
																																				{".htt", "text/webviewhtml"},
																																				{".hxa", "application/xml"},
																																				{".hxc", "application/xml"},
																																				{".hxd", "application/octet-stream"},
																																				{".hxe", "application/xml"},
																																				{".hxf", "application/xml"},
																																				{".hxh", "application/octet-stream"},
																																				{".hxi", "application/octet-stream"},
																																				{".hxk", "application/xml"},
																																				{".hxq", "application/octet-stream"},
																																				{".hxr", "application/octet-stream"},
																																				{".hxs", "application/octet-stream"},
																																				{".hxt", "text/html"},
																																				{".hxv", "application/xml"},
																																				{".hxw", "application/octet-stream"},
																																				{".hxx", "text/plain"},
																																				{".i", "text/plain"},
																																				{".ico", "image/x-icon"},
																																				{".ics", "application/octet-stream"},
																																				{".idl", "text/plain"},
																																				{".ief", "image/ief"},
																																				{".iii", "application/x-iphone"},
																																				{".inc", "text/plain"},
																																				{".inf", "application/octet-stream"},
																																				{".inl", "text/plain"},
																																				{".ins", "application/x-internet-signup"},
																																				{".ipa", "application/x-itunes-ipa"},
																																				{".ipg", "application/x-itunes-ipg"},
																																				{".ipproj", "text/plain"},
																																				{".ipsw", "application/x-itunes-ipsw"},
																																				{".iqy", "text/x-ms-iqy"},
																																				{".isp", "application/x-internet-signup"},
																																				{".ite", "application/x-itunes-ite"},
																																				{".itlp", "application/x-itunes-itlp"},
																																				{".itms", "application/x-itunes-itms"},
																																				{".itpc", "application/x-itunes-itpc"},
																																				{".IVF", "video/x-ivf"},
																																				{".jar", "application/java-archive"},
																																				{".java", "application/octet-stream"},
																																				{".jck", "application/liquidmotion"},
																																				{".jcz", "application/liquidmotion"},
																																				{".jfif", "image/pjpeg"},
																																				{".jnlp", "application/x-java-jnlp-file"},
																																				{".jpb", "application/octet-stream"},
																																				{".jpe", "image/jpeg"},
																																				{".jpeg", "image/jpeg"},
																																				{".jpg", "image/jpeg"},
																																				{".js", "application/x-javascript"},
																																				{".json", "application/json"},
																																				{".jsx", "text/jscript"},
																																				{".jsxbin", "text/plain"},
																																				{".latex", "application/x-latex"},
																																				{".library-ms", "application/windows-library+xml"},
																																				{".lit", "application/x-ms-reader"},
																																				{".loadtest", "application/xml"},
																																				{".lpk", "application/octet-stream"},
																																				{".lsf", "video/x-la-asf"},
																																				{".lst", "text/plain"},
																																				{".lsx", "video/x-la-asf"},
																																				{".lzh", "application/octet-stream"},
																																				{".m13", "application/x-msmediaview"},
																																				{".m14", "application/x-msmediaview"},
																																				{".m1v", "video/mpeg"},
																																				{".m2t", "video/vnd.dlna.mpeg-tts"},
																																				{".m2ts", "video/vnd.dlna.mpeg-tts"},
																																				{".m2v", "video/mpeg"},
																																				{".m3u", "audio/x-mpegurl"},
																																				{".m3u8", "audio/x-mpegurl"},
																																				{".m4a", "audio/m4a"},
																																				{".m4b", "audio/m4b"},
																																				{".m4p", "audio/m4p"},
																																				{".m4r", "audio/x-m4r"},
																																				{".m4v", "video/x-m4v"},
																																				{".mac", "image/x-macpaint"},
																																				{".mak", "text/plain"},
																																				{".man", "application/x-troff-man"},
																																				{".manifest", "application/x-ms-manifest"},
																																				{".map", "text/plain"},
																																				{".master", "application/xml"},
																																				{".mda", "application/msaccess"},
																																				{".mdb", "application/x-msaccess"},
																																				{".mde", "application/msaccess"},
																																				{".mdp", "application/octet-stream"},
																																				{".me", "application/x-troff-me"},
																																				{".mfp", "application/x-shockwave-flash"},
																																				{".mht", "message/rfc822"},
																																				{".mhtml", "message/rfc822"},
																																				{".mid", "audio/mid"},
																																				{".midi", "audio/mid"},
																																				{".mix", "application/octet-stream"},
																																				{".mk", "text/plain"},
																																				{".mmf", "application/x-smaf"},
																																				{".mno", "text/xml"},
																																				{".mny", "application/x-msmoney"},
																																				{".mod", "video/mpeg"},
																																				{".mov", "video/quicktime"},
																																				{".movie", "video/x-sgi-movie"},
																																				{".mp2", "video/mpeg"},
																																				{".mp2v", "video/mpeg"},
																																				{".mp3", "audio/mpeg"},
																																				{".mp4", "video/mp4"},
																																				{".mp4v", "video/mp4"},
																																				{".mpa", "video/mpeg"},
																																				{".mpe", "video/mpeg"},
																																				{".mpeg", "video/mpeg"},
																																				{".mpf", "application/vnd.ms-mediapackage"},
																																				{".mpg", "video/mpeg"},
																																				{".mpp", "application/vnd.ms-project"},
																																				{".mpv2", "video/mpeg"},
																																				{".mqv", "video/quicktime"},
																																				{".ms", "application/x-troff-ms"},
																																				{".msi", "application/octet-stream"},
																																				{".mso", "application/octet-stream"},
																																				{".mts", "video/vnd.dlna.mpeg-tts"},
																																				{".mtx", "application/xml"},
																																				{".mvb", "application/x-msmediaview"},
																																				{".mvc", "application/x-miva-compiled"},
																																				{".mxp", "application/x-mmxp"},
																																				{".nc", "application/x-netcdf"},
																																				{".nsc", "video/x-ms-asf"},
																																				{".nws", "message/rfc822"},
																																				{".ocx", "application/octet-stream"},
																																				{".oda", "application/oda"},
																																				{".odc", "text/x-ms-odc"},
																																				{".odh", "text/plain"},
																																				{".odl", "text/plain"},
																																				{".odp", "application/vnd.oasis.opendocument.presentation"},
																																				{".ods", "application/oleobject"},
																																				{".odt", "application/vnd.oasis.opendocument.text"},
																																				{".one", "application/onenote"},
																																				{".onea", "application/onenote"},
																																				{".onepkg", "application/onenote"},
																																				{".onetmp", "application/onenote"},
																																				{".onetoc", "application/onenote"},
																																				{".onetoc2", "application/onenote"},
																																				{".orderedtest", "application/xml"},
																																				{".osdx", "application/opensearchdescription+xml"},
																																				{".p10", "application/pkcs10"},
																																				{".p12", "application/x-pkcs12"},
																																				{".p7b", "application/x-pkcs7-certificates"},
																																				{".p7c", "application/pkcs7-mime"},
																																				{".p7m", "application/pkcs7-mime"},
																																				{".p7r", "application/x-pkcs7-certreqresp"},
																																				{".p7s", "application/pkcs7-signature"},
																																				{".pbm", "image/x-portable-bitmap"},
																																				{".pcast", "application/x-podcast"},
																																				{".pct", "image/pict"},
																																				{".pcx", "application/octet-stream"},
																																				{".pcz", "application/octet-stream"},
																																				{".pdf", "application/pdf"},
																																				{".pfb", "application/octet-stream"},
																																				{".pfm", "application/octet-stream"},
																																				{".pfx", "application/x-pkcs12"},
																																				{".pgm", "image/x-portable-graymap"},
																																				{".pic", "image/pict"},
																																				{".pict", "image/pict"},
																																				{".pkgdef", "text/plain"},
																																				{".pkgundef", "text/plain"},
																																				{".pko", "application/vnd.ms-pki.pko"},
																																				{".pls", "audio/scpls"},
																																				{".pma", "application/x-perfmon"},
																																				{".pmc", "application/x-perfmon"},
																																				{".pml", "application/x-perfmon"},
																																				{".pmr", "application/x-perfmon"},
																																				{".pmw", "application/x-perfmon"},
																																				{".png", "image/png"},
																																				{".pnm", "image/x-portable-anymap"},
																																				{".pnt", "image/x-macpaint"},
																																				{".pntg", "image/x-macpaint"},
																																				{".pnz", "image/png"},
																																				{".pot", "application/vnd.ms-powerpoint"},
																																				{".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
																																				{".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
																																				{".ppa", "application/vnd.ms-powerpoint"},
																																				{".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
																																				{".ppm", "image/x-portable-pixmap"},
																																				{".pps", "application/vnd.ms-powerpoint"},
																																				{".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
																																				{".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
																																				{".ppt", "application/vnd.ms-powerpoint"},
																																				{".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
																																				{".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
																																				{".prf", "application/pics-rules"},
																																				{".prm", "application/octet-stream"},
																																				{".prx", "application/octet-stream"},
																																				{".ps", "application/postscript"},
																																				{".psc1", "application/PowerShell"},
																																				{".psd", "application/octet-stream"},
																																				{".psess", "application/xml"},
																																				{".psm", "application/octet-stream"},
																																				{".psp", "application/octet-stream"},
																																				{".pub", "application/x-mspublisher"},
																																				{".pwz", "application/vnd.ms-powerpoint"},
																																				{".qht", "text/x-html-insertion"},
																																				{".qhtm", "text/x-html-insertion"},
																																				{".qt", "video/quicktime"},
																																				{".qti", "image/x-quicktime"},
																																				{".qtif", "image/x-quicktime"},
																																				{".qtl", "application/x-quicktimeplayer"},
																																				{".qxd", "application/octet-stream"},
																																				{".ra", "audio/x-pn-realaudio"},
																																				{".ram", "audio/x-pn-realaudio"},
																																				{".rar", "application/octet-stream"},
																																				{".ras", "image/x-cmu-raster"},
																																				{".rat", "application/rat-file"},
																																				{".rc", "text/plain"},
																																				{".rc2", "text/plain"},
																																				{".rct", "text/plain"},
																																				{".rdlc", "application/xml"},
																																				{".resx", "application/xml"},
																																				{".rf", "image/vnd.rn-realflash"},
																																				{".rgb", "image/x-rgb"},
																																				{".rgs", "text/plain"},
																																				{".rm", "application/vnd.rn-realmedia"},
																																				{".rmi", "audio/mid"},
																																				{".rmp", "application/vnd.rn-rn_music_package"},
																																				{".roff", "application/x-troff"},
																																				{".rpm", "audio/x-pn-realaudio-plugin"},
																																				{".rqy", "text/x-ms-rqy"},
																																				{".rtf", "application/rtf"},
																																				{".rtx", "text/richtext"},
																																				{".ruleset", "application/xml"},
																																				{".s", "text/plain"},
																																				{".safariextz", "application/x-safari-safariextz"},
																																				{".scd", "application/x-msschedule"},
																																				{".sct", "text/scriptlet"},
																																				{".sd2", "audio/x-sd2"},
																																				{".sdp", "application/sdp"},
																																				{".sea", "application/octet-stream"},
																																				{".searchConnector-ms", "application/windows-search-connector+xml"},
																																				{".setpay", "application/set-payment-initiation"},
																																				{".setreg", "application/set-registration-initiation"},
																																				{".settings", "application/xml"},
																																				{".sgimb", "application/x-sgimb"},
																																				{".sgml", "text/sgml"},
																																				{".sh", "application/x-sh"},
																																				{".shar", "application/x-shar"},
																																				{".shtml", "text/html"},
																																				{".sit", "application/x-stuffit"},
																																				{".sitemap", "application/xml"},
																																				{".skin", "application/xml"},
																																				{".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
																																				{".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
																																				{".slk", "application/vnd.ms-excel"},
																																				{".sln", "text/plain"},
																																				{".slupkg-ms", "application/x-ms-license"},
																																				{".smd", "audio/x-smd"},
																																				{".smi", "application/octet-stream"},
																																				{".smx", "audio/x-smd"},
																																				{".smz", "audio/x-smd"},
																																				{".snd", "audio/basic"},
																																				{".snippet", "application/xml"},
																																				{".snp", "application/octet-stream"},
																																				{".sol", "text/plain"},
																																				{".sor", "text/plain"},
																																				{".spc", "application/x-pkcs7-certificates"},
																																				{".spl", "application/futuresplash"},
																																				{".src", "application/x-wais-source"},
																																				{".srf", "text/plain"},
																																				{".SSISDeploymentManifest", "text/xml"},
																																				{".ssm", "application/streamingmedia"},
																																				{".sst", "application/vnd.ms-pki.certstore"},
																																				{".stl", "application/vnd.ms-pki.stl"},
																																				{".sv4cpio", "application/x-sv4cpio"},
																																				{".sv4crc", "application/x-sv4crc"},
																																				{".svc", "application/xml"},
																																				{".swf", "application/x-shockwave-flash"},
																																				{".t", "application/x-troff"},
																																				{".tar", "application/x-tar"},
																																				{".tcl", "application/x-tcl"},
																																				{".testrunconfig", "application/xml"},
																																				{".testsettings", "application/xml"},
																																				{".tex", "application/x-tex"},
																																				{".texi", "application/x-texinfo"},
																																				{".texinfo", "application/x-texinfo"},
																																				{".tgz", "application/x-compressed"},
																																				{".thmx", "application/vnd.ms-officetheme"},
																																				{".thn", "application/octet-stream"},
																																				{".tif", "image/tiff"},
																																				{".tiff", "image/tiff"},
																																				{".tlh", "text/plain"},
																																				{".tli", "text/plain"},
																																				{".toc", "application/octet-stream"},
																																				{".tr", "application/x-troff"},
																																				{".trm", "application/x-msterminal"},
																																				{".trx", "application/xml"},
																																				{".ts", "video/vnd.dlna.mpeg-tts"},
																																				{".tsv", "text/tab-separated-values"},
																																				{".ttf", "application/octet-stream"},
																																				{".tts", "video/vnd.dlna.mpeg-tts"},
																																				{".txt", "text/plain"},
																																				{".u32", "application/octet-stream"},
																																				{".uls", "text/iuls"},
																																				{".user", "text/plain"},
																																				{".ustar", "application/x-ustar"},
																																				{".vb", "text/plain"},
																																				{".vbdproj", "text/plain"},
																																				{".vbk", "video/mpeg"},
																																				{".vbproj", "text/plain"},
																																				{".vbs", "text/vbscript"},
																																				{".vcf", "text/x-vcard"},
																																				{".vcproj", "Application/xml"},
																																				{".vcs", "text/plain"},
																																				{".vcxproj", "Application/xml"},
																																				{".vddproj", "text/plain"},
																																				{".vdp", "text/plain"},
																																				{".vdproj", "text/plain"},
																																				{".vdx", "application/vnd.ms-visio.viewer"},
																																				{".vml", "text/xml"},
																																				{".vscontent", "application/xml"},
																																				{".vsct", "text/xml"},
																																				{".vsd", "application/vnd.visio"},
																																				{".vsi", "application/ms-vsi"},
																																				{".vsix", "application/vsix"},
																																				{".vsixlangpack", "text/xml"},
																																				{".vsixmanifest", "text/xml"},
																																				{".vsmdi", "application/xml"},
																																				{".vspscc", "text/plain"},
																																				{".vss", "application/vnd.visio"},
																																				{".vsscc", "text/plain"},
																																				{".vssettings", "text/xml"},
																																				{".vssscc", "text/plain"},
																																				{".vst", "application/vnd.visio"},
																																				{".vstemplate", "text/xml"},
																																				{".vsto", "application/x-ms-vsto"},
																																				{".vsw", "application/vnd.visio"},
																																				{".vsx", "application/vnd.visio"},
																																				{".vtx", "application/vnd.visio"},
																																				{".wav", "audio/wav"},
																																				{".wave", "audio/wav"},
																																				{".wax", "audio/x-ms-wax"},
																																				{".wbk", "application/msword"},
																																				{".wbmp", "image/vnd.wap.wbmp"},
																																				{".wcm", "application/vnd.ms-works"},
																																				{".wdb", "application/vnd.ms-works"},
																																				{".wdp", "image/vnd.ms-photo"},
																																				{".webarchive", "application/x-safari-webarchive"},
																																				{".webtest", "application/xml"},
																																				{".wiq", "application/xml"},
																																				{".wiz", "application/msword"},
																																				{".wks", "application/vnd.ms-works"},
																																				{".WLMP", "application/wlmoviemaker"},
																																				{".wlpginstall", "application/x-wlpg-detect"},
																																				{".wlpginstall3", "application/x-wlpg3-detect"},
																																				{".wm", "video/x-ms-wm"},
																																				{".wma", "audio/x-ms-wma"},
																																				{".wmd", "application/x-ms-wmd"},
																																				{".wmf", "application/x-msmetafile"},
																																				{".wml", "text/vnd.wap.wml"},
																																				{".wmlc", "application/vnd.wap.wmlc"},
																																				{".wmls", "text/vnd.wap.wmlscript"},
																																				{".wmlsc", "application/vnd.wap.wmlscriptc"},
																																				{".wmp", "video/x-ms-wmp"},
																																				{".wmv", "video/x-ms-wmv"},
																																				{".wmx", "video/x-ms-wmx"},
																																				{".wmz", "application/x-ms-wmz"},
																																				{".wpl", "application/vnd.ms-wpl"},
																																				{".wps", "application/vnd.ms-works"},
																																				{".wri", "application/x-mswrite"},
																																				{".wrl", "x-world/x-vrml"},
																																				{".wrz", "x-world/x-vrml"},
																																				{".wsc", "text/scriptlet"},
																																				{".wsdl", "text/xml"},
																																				{".wvx", "video/x-ms-wvx"},
																																				{".x", "application/directx"},
																																				{".xaf", "x-world/x-vrml"},
																																				{".xaml", "application/xaml+xml"},
																																				{".xap", "application/x-silverlight-app"},
																																				{".xbap", "application/x-ms-xbap"},
																																				{".xbm", "image/x-xbitmap"},
																																				{".xdr", "text/plain"},
																																				{".xht", "application/xhtml+xml"},
																																				{".xhtml", "application/xhtml+xml"},
																																				{".xla", "application/vnd.ms-excel"},
																																				{".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
																																				{".xlc", "application/vnd.ms-excel"},
																																				{".xld", "application/vnd.ms-excel"},
																																				{".xlk", "application/vnd.ms-excel"},
																																				{".xll", "application/vnd.ms-excel"},
																																				{".xlm", "application/vnd.ms-excel"},
																																				{".xls", "application/vnd.ms-excel"},
																																				{".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
																																				{".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
																																				{".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
																																				{".xlt", "application/vnd.ms-excel"},
																																				{".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
																																				{".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
																																				{".xlw", "application/vnd.ms-excel"},
																																				{".xml", "text/xml"},
																																				{".xmta", "application/xml"},
																																				{".xof", "x-world/x-vrml"},
																																				{".XOML", "text/plain"},
																																				{".xpm", "image/x-xpixmap"},
																																				{".xps", "application/vnd.ms-xpsdocument"},
																																				{".xrm-ms", "text/xml"},
																																				{".xsc", "application/xml"},
																																				{".xsd", "text/xml"},
																																				{".xsf", "text/xml"},
																																				{".xsl", "text/xml"},
																																				{".xslt", "text/xml"},
																																				{".xsn", "application/octet-stream"},
																																				{".xss", "application/xml"},
																																				{".xtp", "application/octet-stream"},
																																				{".xwd", "image/x-xwindowdump"},
																																				{".z", "application/x-compress"},
																																				{".zip", "application/x-zip-compressed"},
																																			};
		#endregion

		public static string GetMimeType(string extension)
			{
			if (extension == null)
				{
				throw new ArgumentNullException("extension");
				}
			if (!extension.StartsWith("."))
				{
				extension = "." + extension;
				}
			string mime;
			return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
			}

		}
	}

public class file_store
{
    public class fileObj
    {
        public int id { get; set; }
        public int sub_folder_id { get; set; }
        public int folder_id { get; set; }
        public int page_id { get; set; }
        public string name { get; set; }
        public string ext { get; set; }
        public string mime { get; set; }
        public byte[] content { get; set; }
        public DateTime dt { get; set; }
        public fileObj()
        {
        }
        public fileObj(int _id)
        {
            load(_id, false);
        }

        private void load(int _id, bool is_video)
        {
            var _tools = new Toolbox();
            var dr = Toolbox.doSQL_dt(@"SELECT * FROM filestore.files  WHERE id = @v0  LIMIT 1", new object[] { _id }).Rows[0];
            id = _id;
            ext = dr["ext"].ToString();
            mime = dr["mime"].ToString();
            dt = Convert.ToDateTime(dr["dt"]);
            page_id = Convert.ToInt32(dr["page_id"]);
            sub_folder_id = Convert.ToInt32(dr["sub_folder_id"]);
            folder_id = Convert.ToInt32(dr["folder_id"]);
            name = dr["name"].ToString();
            if (!is_video && dr["content"] != DBNull.Value)
            {
                content = (byte[])dr["content"];
            }
        }
        public void save()
        {
            if (id == 0)
            {
                //NEW
                using (var conn = Toolbox.connect())
                {
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO filestore.files (folder_id,sub_folder_id,page_id, name, ext, mime, dt, content) VALUES (@folder_id, @sub_folder_id, @page_id, @name, @ext, @mime, NOW(), @content)";
                        cmd.Parameters.AddWithValue("@content", content);
                        cmd.Parameters.AddWithValue("@page_id", page_id);
                        cmd.Parameters.AddWithValue("@folder_id", folder_id);
                        cmd.Parameters.AddWithValue("@sub_folder_id", sub_folder_id);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@ext", ext);
                        cmd.Parameters.AddWithValue("@mime", mime);
                        cmd.ExecuteNonQuery();
                        id = Convert.ToInt32(cmd.LastInsertedId);
                    }
                }
            }
            else
            {
                //UPDATE
                using (var conn = Toolbox.connect())
                {
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE filestore.files SET folder_id = @folder_id, sub_folder_id = @sub_folder_id, page_id = @page_id, name = @name, ext = @ext, mime = @mime, dt= NOW(), content = @content WHERE id = @id";
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@content", content);
                        cmd.Parameters.AddWithValue("@page_id", page_id);
                        cmd.Parameters.AddWithValue("@folder_id", folder_id);
                        cmd.Parameters.AddWithValue("@sub_folder_id", sub_folder_id);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@ext", ext);
                        cmd.Parameters.AddWithValue("@mime", mime);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

        }  
        }
        public class folder
        {
            public int id { get; set; }
            public string name { get; set; }
            public DateTime dt_created { get; set; }
            public int member_id_created { get; set; }
            public void save()
            {
                if (id == 0)
                {
                    //NEW
                }
                else
                {
                    //UPDATE
                }
            }
        }
        public DataTable get_files(int _page_id, int _folder_id, int _sub_folder_id)
        {
            return Toolbox.doSQL_dt(@"SELECT * FROM filestore.files  WHERE page_id =@v0 AND folder_id =@v1  AND sub_folder_id =@v2  AND active = TRUE ORDER BY dt DESC", new object[] { _page_id, _folder_id, _sub_folder_id });
        }
        public DataTable get_header_fileinfo(int _page_id, int _folder_id, int _sub_folder_id)
        {
            return Toolbox.doSQL_dt(@"SELECT id,page_id,folder_id,sub_folder_id,name,dt,ext,mime,active FROM filestore.files  WHERE page_id =@v0 AND folder_id =@v1  AND sub_folder_id =@v2  AND active = TRUE ORDER BY dt DESC", new object[] { _page_id, _folder_id, _sub_folder_id });
        }
    }
