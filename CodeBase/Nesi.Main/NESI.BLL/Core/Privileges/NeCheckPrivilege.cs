using System;
using System.Text.RegularExpressions;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.FileManager;
using NESI.BLL.Pages.Quotes;

namespace NESI.BLL.Core.Privileges
{
	public enum ChekcPrivilegeType
	{
		Create,
		Read,
		Update,
		Delete
	}

	public class NeCheckPrivilege : BLLBase
	{
		public NeCheckPrivilege(Employee.Employee user) : base(user)
		{

		}

		public bool CheckBusinessUnitId(int buid)
		{
			return buid <= 0 || CurrentUser.IsVisibleBusinessUnitId(buid);
		}

		public bool CheckWorkOrder(int woId, int buid, ChekcPrivilegeType type)
		{
			if (woId <= 0)
			{
				return true;
			}
			var wo = new Ne2WOProg(woId);
			if (buid > 0 && buid != wo.BusinessUnitId)
			{
				return false;
			}
			return wo.Entity == null || CheckBusinessUnitId(wo.Entity.business_unit_id.GetValueOrDefault(0));
		}

		public bool CheckQuote(int quoteId, int revision, int buid, ChekcPrivilegeType type)
		{
			if (quoteId <= 0)
			{
				return true;
			}
			if (revision <= 0)
			{
				revision = 1;
			}
			var q = new Ne2Quote(quoteId, revision);
			if (buid > 0 && buid != q.BusinessUnitId)
			{
				return false;
			}
			return q.Entity == null || CheckBusinessUnitId(q.Entity.business_unit_id.GetValueOrDefault(0));
		}


		public bool CheckFile(string filename, int te = 0, ChekcPrivilegeType type = ChekcPrivilegeType.Read)
		{

			if (string.IsNullOrEmpty(filename))
			{
				return true;
			}

			filename = filename.ToUpper();
			var fileName_config = (new TempFile(CurrentUser)).FileServer.ToUpper();
			if (!filename.StartsWith(fileName_config))
			{
				return false;
			}

			var te_id = 0;
			var reg = new Regex(@"\\TE\\TE(\d+)\\");
			var matches = reg.Matches(filename);
			if (matches.Count >= 1)
			{
				try
				{
					te_id = Convert.ToInt32(matches[0].Groups[1].Value);
				}
				catch (Exception)
				{
					return true;
				}
			}
			else
			{
				return true;
			}

			if (te_id > 0 && te > 0 && te != te_id)
			{
				return false;
			}

			return te_id <= 0 || IsVisibleTaxentity(te_id);
		}
	}
}