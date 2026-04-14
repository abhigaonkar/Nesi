using System;
using System.Data;
using System.IO;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.WorkOrder
{
	public class WorkOrderRenameFile : WorkOrderEdit
	{


		public WorkOrderRenameFile(Employee user, int buid, int woid) : base(user, buid, woid.ToString())
		{
		}



		public object Profile()
		{
			return new
			{
				business_unit_name = wo_bu.name,
				customerName = wo.CustomerName,
				orderNumber = wo.OrderNumber,
				//nameList = GetNameList(),
				pdf_path = GetPdf_Path(),
			};
		}




		public DataExtra Delete()
		{
			if (GetPdf_Path() == @"/images/NoScan.pdf")
			{
				return new DataExtra("File does not exist.");
			}
			var fileName = wo.woprog_id + ".pdf";
			var businessUnitWoPath = wo_bu.WOPath;
			var fileServer = NeTaxEntity.BaseFolder(wo_bu.id, false);
			var pathWo = $"{fileServer}\\WOs\\{fileName}";
		//	var pathProg = $"{businessUnitWoPath}WorkPro\\{wo.woprog_id}.pdf";
			var sig = new shared.signature(wo.woprog_id, "woprog");
			if (sig.id > 0)
			{
				sig.delete();
			}
			NeWOProg.reopen_workorder(wo);
			if (File.Exists(pathWo))
			{
				File.Delete(pathWo);
			}
			//if (File.Exists(pathProg))
			//{
			//	File.Delete(pathProg);
		//	}
			return new DataExtra("The file has been deleted successfully.");
		}


		//		public DataExtra Rename(int woprogid)
		//		{
		//			if (woprogid == 0)
		//			{
		//				return new DataExtra("Please select a work order.");
		//			}
		//			var path = wo_bu.WOPath;
		//			var fileServer = NeTaxEntity.BaseFolder(wo_bu.id, false);
		//			var _currentUser = new NeMember(UserId);

		//			try
		//			{
		//				var woprogIdSelected = bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(woprog_id), 0) 
		//FROM woprog WHERE woprog_bvwo =@v0  AND woprog_status='Open' AND
		//business_unit_id=@v1 ", woprogid, wo_bu.id);

		//				if ((woprogIdSelected > 0) && (file_name != ""))
		//				{
		//					var woSelected = new NeWOProg(woprogIdSelected);
		//					try
		//					{
		//						NeWOProg.check_before_move(woSelected, NeWOProg.woprog_status_enum.InitialPrep);
		//					}
		//					catch (Exception check_move_error)
		//					{
		//						return new DataExtra(check_move_error.Message);
		//						// Toolbox.FriendlyException(this.Response, check_move_error.Message, this.Page.Request.Url.AbsoluteUri);
		//					}

		//					var fileName = woprogIdSelected + ".pdf";
		//					//New Work Order is Being Associated with a Scan

		//					NeWOProg.move_scans(fileServer, path, scanfile, fileName);

		//					woSelected = new NeWOProg(woprogIdSelected);

		//					foreach (DataRow drChildWos in NeWOProg.get_child_workorders(woprogIdSelected).Rows)
		//					{
		//						var status = drChildWos["woprog_status"].ToString();
		//						var childWoprogId = Convert.ToInt32(drChildWos["woprog_id"]);
		//						if (status == "Invoiced" || status == OpsWOStatus.WaitingToBeInvoiced || status == "Deleted")
		//						{
		//							continue;
		//						}
		//						var childWo = new NeWOProg(childWoprogId);
		//						var childBusinessUnit = new NeBusinessUnit(woSelected.business_unit_id);
		//						var emailChildPm = new NeEMail
		//						{
		//							To = new NeMember(childWo.intProjectManager).NEEmail,
		//							CC = childBusinessUnit.branch_manager.NEEmail,
		//							Subject =
		//								$"WO {woSelected.OrderNumber} from {childBusinessUnit.name} is being invoiced.. your WO {childWo.OrderNumber} needs to be finalized ASAP.",
		//							From = "noreply@newelectric.com"
		//						};
		//						emailChildPm.Send();
		//					}

		//					// Move the selected WO to initial prep
		//					NeWOProg.move_status(woSelected, _currentUser, "", NeWOProg.woprog_status_enum.InitialPrep);
		//				}
		//			}
		//			catch (Exception)
		//			{
		//				return new DataExtra("The file has been submitted failed.");
		//			}
		//			return new DataExtra("The file has been submitted successfully.");

		//		}
	}

}