using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Quotes
{
	public class QuoteBLLBase : BLLBase
	{
		protected NeQuote _quote;
		protected NeBusinessUnit _company_obj;
		protected NeMember current_user;
		public bool Privilege61 { get; set; }
		public string Error { get; set; }

		public QuoteBLLBase() : base()
		{
			
		}

		public QuoteBLLBase(Employee user) : base(user)
		{
			Privilege61 = user.AuthenticatedForPrivilege(61);
			current_user = new NeMember(UserId);
			_quote = GetQuoteByNeMember(current_user);
			_company_obj = new NeBusinessUnit(current_user.business_unit_id);
		}

		protected NeQuote GetQuoteByNeMember(NeMember user)
		{
			var cultureInfo = new CultureInfo("en-US");
			var textInfo = cultureInfo.TextInfo;

			return new NeQuote
			{
				conn = Toolbox.connect(),
				VisibleBusinessUnit = BLL.Core.BusinessUnit.GetVisibleBusinessUnits(user.id),
				textinfo = textInfo,
				quoted_by = user.id.ToString(),
				current_quoter = user.id.ToString(),
				business_unit_id = user.business_unit_id.ToString(),
				this_member = user,
				quoted_business_unit_id = user.business_unit_id.ToString()
			};
		}

		public LabelValueInt[] GetBusinessUnitList()
		{
			var list=new BLL.Core.BusinessUnit(CurrentUser).GetVisibleBusinessUnitList();
			return list.Where(x => x.enable_timesheet == 1).Select(x => new LabelValueInt()
			{
				Label = x.ddl_Name,
				Value = x.ID
			}).OrderBy(x=> x.Label).ToArray();
		}

        public LabelValueInt[] GetRevenueLine()
        {
            var list = _quote.Get_RevenueLine();

            return list.ToArray();

        }

		public List<LabelValueInt> GetUserListByBusinessUnit(int buId, bool addAll = true)
		{
			var list = _quote.get_Quoters(buId);
			if (addAll)
			{
				list.Insert(0, new LabelValueInt()
				{
					Value = 0,
					Label = "All"
				});
			}
			return list;
		}

        public List<LabelValueInt> GetRevenueLineByBusinessUnit(int buId)
        {
            var list = _quote.Get_RevenueLine(buId);
            return list;
        }


		public string LockQuote(string quote_id, string rev = "1", bool locked = true)
		{
			bllToolbox.doSQL_void(@"UPDATE quote_master SET quoter_locked = @v2 , updated_by_page = 'quote - lock_quoter()' WHERE quote_id = @v0  AND revision = @v1",
				quote_id, rev, locked ? 1 : 0);
			return "success.";
		}

        public bool DoesQuoteExist(int quote_id)
        {
            var quoteId = bllToolbox.doSQL_int(@"SELECT quote_id FROM quote_master WHERE quote_id = @v0 AND status_id != 9", quote_id);
            return quoteId > 0;
        }




	}
}