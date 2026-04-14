using System;
using nesi.core;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.Quotes
{


	public class QuoteStrategBase : QuoteBLLBase
	{

		protected NeQuoteSchedule qs;
		protected readonly quote quote;
		protected readonly NeBusinessUnit bu;
		protected NeMember bm;
		public string quote_level { get; set; }
		public string Revision { get; set; }
		public int buId { get; set; }
		public int quote_id { get; set; }
		public int Branch_manager_id { get; set; }

		public QuoteStrategBase(Employee user, int quoteId) : base(user)
		{
			quote_id = quoteId;
			qs = new NeQuoteSchedule(quoteId.ToString());
			quote = new quote(quoteId);
			Revision = quote.Revision.ToString();
			buId = quote.business_unit_id;
			bu = new NeBusinessUnit(buId);
			bm = bu.branch_manager;
			Branch_manager_id = bu.branch_manager.id;
			if (Branch_manager_id == 0)
			{
				bm = new NeMember(37);
			}
			if (quote.expected_value >= bu.quote_level_3_start)
			{
				quote_level = "3";
			}
			else if (quote.expected_value >= bu.quote_level_2_start)
			{
				quote_level = "2";
			}
		}

		protected string getStageText(object value)
		{
			return value != null
				? bllToolbox.doSQL_string(@"Select fun_time('" + Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss") + "')")
				: "";
		}

	}
}