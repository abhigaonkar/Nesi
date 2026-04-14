using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Quotes;

namespace NESI.BLL.Pages.Quotes
{
	public class QuoteProfile : QuoteBLLBase
	{
		public QuoteSummary Summary { get; set; }

		public DataTable Todonow_list { get; set; }
		public LabelValueInt[] businessUnit_list { get; set; }
		public DataTable member_list { get; set; }
		public bool use_quote_process { get; set; }

		public QuoteProfile() : base()
		{

		}

		public QuoteProfile(Employee user) : base(user)
		{
			Summary = new QuoteSummary()
			{
				Header = GetSummaryHeader(_quote),
				Body = GetSummaryBody(_quote),
				Sum = GetSummarySum(_quote)
			};
			Todonow_list = GetTodonow_list(UserId);
			businessUnit_list = CurrentUser.VisibleBusinessUnitLabelValueList;
			member_list = GetMemberListByBusinessUnitList();
			use_quote_process = CurrentUser.BusinessUnit.uses_quote_process.GetValueOrDefault() || Todonow_list.Rows.Count > 0;
		}

		public object GetQuoteSummaryByMemberList(int[] selected_list)
		{

			var headersList = new List<string[]>();
			var bodyList = new List<List<QuoteListItem[]>>();
			var sumList = new List<string[]>();
			var todoList = new List<DataTable>();
			foreach (var user_id in selected_list)
			{
				var q = GetQuoteByNeMember(new NeMember(user_id));
				headersList.Add(GetSummaryHeader(q));
				bodyList.Add(GetSummaryBody(q));
				sumList.Add(GetSummarySum(q));
				todoList.Add(GetTodonow_list(user_id));
			}
			var int_header = new int[8];
			var str_header = new string[8];
			foreach (var _header in headersList)
			{
				for (var i = 0; i < 8; i++)
				{
					int_header[i] += Convert.ToInt32(_header[i]);
				}
			}
			for (var i = 0; i < 8; i++)
			{
				str_header[i] = int_header[i].ToString();
			}

			var body = new List<QuoteListItem[]>();
			var line = new List<QuoteListItem>[8];
			foreach (var _body in bodyList)
			{
				var i = 0;
				foreach (var _body_item in _body)
				{
					foreach (var item in _body_item)
					{
						if (line[i] == null) { line[i] = new List<QuoteListItem>(); }
						line[i].Add(item);
					}
					i++;
				}
			}
			for (var i = 0; i < 8; i++)
			{
				body.Add(line[i] != null && line[i].Count > 0 ? line[i].ToArray() : null);
			}

			var double_sum = new double[8];
			foreach (var _sum in sumList)
			{
				var i = 0;
				foreach (var _sumitem in _sum)
				{
					var value = Convert.ToDouble(_sumitem.Replace("$", "").Replace(",", ""));
					double_sum[i] += value;
					i++;
				}
			}
			var str_sum = new string[8];
			for (var i = 0; i < 8; i++)
			{
				str_sum[i] = double_sum[i].ToString("C");
			}
			var todo = todoList[0];
			if (todoList.Count > 1)
			{
				for (var i = 1; i < todoList.Count; i++)
				{
					todo.Merge(todoList[i]);
				}
			}
			return new
			{
				todonow_list = todo,
				header = str_header,
				body = body,
				sum = str_sum
			};
		}

		public DataTable GetMemberListByBusinessUnitList()
		{
			return bllToolbox.doSQL_dt(@"
select a.member_id value, CONCAT(b.ddl_name,' - ',a.member_fullname) label, a.business_unit_id bu_id
from member a inner join business_unit b on a.business_unit_id=b.id
inner join memberpage p on a.member_id=p.memberpage_member_id and p.memberpage_page_id=65 and p.active=1
where a.member_status='active' 
and find_in_set(a.business_unit_id,@p0)
order by b.ddl_name, a.member_fullname
", CurrentUser.VisibleBusinessUnits);
		}

		public List<QuoteListItem[]> GetSummaryBody(NeQuote quote)
		{
			return new List<QuoteListItem[]>
			{
				quote.quote_list("10"),
				quote.quote_list("11"),
				quote.quote_list("1"),
				quote.quote_list("12"),
				quote.quote_list("2"),
				quote.quote_list("3"),
				quote.quote_list("4"),
				quote.quote_list("5")
			};
		}

		public string[] GetSummarySum(NeQuote quote)
		{
			return new[]
			{
				quote.quote_dollar_total("10"),
				quote.quote_dollar_total("11"),
				quote.quote_dollar_total("1"),
				quote.quote_dollar_total("12"),
				quote.quote_dollar_total("2"),
				quote.quote_dollar_total("3"),
				quote.quote_dollar_total("4"),
				quote.quote_dollar_total("5")

			};
		}

		public string[] GetSummaryHeader(NeQuote quote)
		{
			return new[]
			{
				quote.quote_count("10"),
				quote.quote_count("11"),
				quote.quote_count("1"),
				quote.quote_count("12"),
				quote.quote_count("2"),
				quote.quote_count("3"),
				quote.quote_count("4"),
				quote.quote_list("5").Length.ToString()
			};
		}

		public DataTable GetTodonow_list(int user_id)
		{
			return new quote().get_quote_todo_list(user_id);
		}


		public string GetActiveRevision(string quoteId)
		{
			var o = bllToolbox.doSQL_string("SELECT MAX(revision) FROM quote_master WHERE quote_id = @v0  AND status_id != 9",
				quoteId);
			return string.IsNullOrEmpty(o) ? "1" : o;
		}
	}
}
