using System.Data;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Quotes
{
    public class QuoteSearch : QuoteBLLBase
    {
        public LabelValueString[] Status { get; set; }
        public LabelValueInt[] BusinessUnitList { get; set; }
        public LabelValueInt[] UserListInBusinessUnit { get; set; }


        public QuoteSearch(Employee user) : base(user)
        {
            Status = GetStatus();
            var list = GetBusinessUnitList().ToList();
            list.Insert(0, new LabelValueInt() { Label = "All", Value = 0 });
            BusinessUnitList = list.ToArray();
            UserListInBusinessUnit = GetUserListByBusinessUnit(user.BusinessUnitId).ToArray();
        }



        public DatatableResult doSearch(DTO.ViewModels.Page.Quotes.QuoteSearch model)
        {
			var dt = _quote.search_Quotes(model.Job_description, model.Quote_by, model.Customer_name, model.Date_before,
				model.Date_after, model.Status, model.Quote_id, model.Quote_businessUnit);

			return new DatatableResult()
			{
				Page = model.Page,
				PageSize = model.PageSize,
				TotalRecorders = dt.Rows.Count,
				Data = dt //BLL.Common.Shared.DataTablePagination.GetPageData(dt, model.Page, model.PageSize)
			};
		}

		public LabelValueString[] GetStatus()
		{
			return new LabelValueString[]
			{
				new LabelValueString()
				{
					Value="0",
					Label="Choose Status"
				},
//				new LabelValueString()
//				{
//					Value="0",
//					Label="All Open Quotes"
//				},
				new LabelValueString()
				{
					Value="OPEN",
					Label="All Open Quotes"
				},
				new LabelValueString()
				{
					Value="8",
					Label="PO Received"
				},
				new LabelValueString()
				{
					Value="6",
					Label="Dead Quote"
				},
				new LabelValueString()
				{
					Value="6_8",
					Label="Quote is Closed"
				},
				new LabelValueString()
				{
					Value="1",
					Label="Waiting to be Quoted"
				},
				new LabelValueString()
				{
					Value="3",
					Label="Waiting to be Verified"
				},
				new LabelValueString()
				{
					Value="4",
					Label="Waiting Approval"
				},
				new LabelValueString()
				{
					Value="10",
					Label="Waiting for Stage 1 Go"
				},
				new LabelValueString()
				{
					Value="11",
					Label="Waiting for Stage 4 Go"
				},
				new LabelValueString()
				{
					Value="12",
					Label="Waiting for Final Review"
				},
				new LabelValueString()
				{
					Value="13",
					Label="Waiting for Post Mortem"
				}
			};
		}

        public bool DoesQuoteExist(string quoteId)
        {
            int quoteID = 0;
            if (string.IsNullOrWhiteSpace(quoteId) || quoteId.Trim().Length > 6)
            {
                return false;
            }
            else
            {
                bool valid = int.TryParse(quoteId, out quoteID);
                if (!valid)
                {
                    return false;
                }
            }

            return this.DoesQuoteExist(quoteID);
        }
    }
}