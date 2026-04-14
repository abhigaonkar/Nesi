using System;
using System.Data;
using System.IO;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common.Models;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.WorkOrder
{
	public class WorkOrderScannedFile : WorkOrderFileBase
	{
		public string file_name { get; set; }


		public WorkOrderScannedFile(Employee user, int buid, string filename) : base(user)
		{
			business_unit_id = buid;
			file_name = filename;
		}



		public object Profile()
		{
			var bu = BLL.Common.Cache.Global.BusinessUnit.GetValue(business_unit_id);
			var business_unit_name = bu.Name;
			var strScanPath = bu.wopath;
			var testfile = new FileInfo(strScanPath + file_name);
			var pdf_path = "";
			var newfilename = file_name; //.Replace("-", "").Replace("&", "");

			if (testfile.Exists)
			{
				// copy it locally so it can be displayed
				try
				{
					File.Move(strScanPath + file_name, strScanPath + newfilename);
				}
				catch (Exception)
				{
					throw new Exception("\"" + strScanPath + file_name + "\" is open currently and cannot be moved");
				}
				File.Copy(strScanPath + newfilename, NeTaxEntity.BaseFolder(business_unit_id, false) + @"\WOs\" + newfilename,
					true);
				pdf_path = NeTaxEntity.BaseFolder(business_unit_id, true) + "/WOs/" + newfilename;
			}
			else
			{
				pdf_path = NeTaxEntity.BaseFolder(business_unit_id, true) + "/WOs/" + newfilename;
			}

			return new
			{
				business_unit_name,
				nameList = GetNameList(),
				pdf_path,
			};
		}


	public DataExtra Delete()
		{
			var bu = BLL.Common.Cache.Global.BusinessUnit.GetValue(business_unit_id);
			var path = bu.wopath;
			var scanfile = file_name; //.Replace("-", "").Replace("&", "");
			var fileServer = NeTaxEntity.BaseFolder(bu.ID, false);
			try
			{
				File.Delete(path + scanfile);
				File.Delete(fileServer + @"\WOs\" + scanfile);
				return new DataExtra("The file has been deleted successfully.");
			}
			catch (Exception)
			{
				return new DataExtra("The file has been deleted failed.");
			}

		}


		public DataExtra Rename(int woprogid)
		{
			if (woprogid == 0)
			{
				return new DataExtra("Please select a work order.");
			}
			var bu = BLL.Common.Cache.Global.BusinessUnit.GetValue(business_unit_id);
			var path = bu.wopath;
			var scanfile = file_name; //.Replace("-", "").Replace("&", "");
			var fileServer = NeTaxEntity.BaseFolder(bu.ID, false);
			var _currentUser = new NeMember(UserId);

			try
			{
				int woprogIdSelected = bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(woprog_id), 0) 
FROM woprog WHERE woprog_bvwo =@v0  AND woprog_status='Open' AND
business_unit_id=@v1 ", woprogid, bu.ID);

				if ((woprogIdSelected > 0) && (file_name != ""))
				{
					var woSelected = new NeWOProg(woprogIdSelected);
					try
					{
						NeWOProg.check_before_move(woSelected, OpsWOStatus.InitialPrep);
					}
					catch (Exception check_move_error)
					{
						return new DataExtra(check_move_error.Message);
						// Toolbox.FriendlyException(this.Response, check_move_error.Message, this.Page.Request.Url.AbsoluteUri);
					}

					string fileName = woprogIdSelected + ".pdf";
					//New Work Order is Being Associated with a Scan
                    
					NeWOProg.move_scans(fileServer, path, scanfile, fileName, NeWOProg.MoveScanType.New);

					woSelected = new NeWOProg(woprogIdSelected);

					foreach (DataRow drChildWos in NeWOProg.get_child_workorders(woprogIdSelected).Rows)
					{
						var status = drChildWos["woprog_status"].ToString();
						var childWoprogId = Convert.ToInt32(drChildWos["woprog_id"]);
						if (status == "Invoiced" || status == OpsWOStatus.WaitingToBeInvoiced || status == "Deleted")
						{
							continue;
						}
						var childWo = new NeWOProg(childWoprogId);
						var childBusinessUnit = new NeBusinessUnit(woSelected.business_unit_id);
						var emailChildPm = new NeEMail
						{
							To = new NeMember(childWo.intProjectManager).NEEmail,
							CC = childBusinessUnit.branch_manager.NEEmail,
							Subject =
								$"WO {woSelected.OrderNumber} from {childBusinessUnit.name} is being invoiced.. your WO {childWo.OrderNumber} needs to be finalized ASAP.",
							From = "noreply@" + Toolbox.app_setting("DomainForEmail")
						};
						emailChildPm.Send();
					}

					// Move the selected WO to initial prep
					NeWOProg.move_status(woSelected, _currentUser, "", OpsWOStatus.Enums.InitialPrep);
				}
			}
			catch (Exception ex)
			{
				Toolbox.do_errorLog_errorStack(ex);
				return new DataExtra("There was an issue attaching this scan to the selected work order. This is most likely a server setup issue, please do not try again, and please send the help desk a ticket.");
			}
			return new DataExtra("The file has been submitted successfully.");

		}
	}


}