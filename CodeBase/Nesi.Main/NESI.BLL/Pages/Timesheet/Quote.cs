using System.Linq;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.TimeSheet;

namespace NESI.BLL.Pages.Timesheet
{
	public partial class Timesheet
	{
		private string VIEW_QUOTE_CUSTOMER(int order = 0)
		{
			return
			$@" 
				SELECT 
					CONCAT(c.ddl_name,' => ', b.customer_name) name, 
					b.customer_id id,
					{order} orderby,
					c.ddl_name,
					c.id business_unit_id 
				FROM 
					quote_master a
				INNER JOIN 
					customer b ON
					b.customer_id = a.customer_id AND 
					a.status_id NOT IN (6,7,8,9)  
				INNER JOIN
					business_unit c ON c.id = a.business_unit_id AND c.istest = 'F' AND c.active = 'T'";
		}

		public QuoteCustomer[] GetQuoteCustomerList(int buId, Employee user)
		{
			var sql = $@"{VIEW_QUOTE_CUSTOMER(0)}
				WHERE 
					a.business_unit_id = @p0
				GROUP BY a.customer_id, c.ddl_name
				UNION 
				{VIEW_QUOTE_CUSTOMER(1)}
				WHERE 
					FIND_IN_SET(a.business_unit_id, @p1) AND
					a.business_unit_id != @p0
				GROUP BY a.customer_id, c.ddl_name
				ORDER BY  
					orderby,
					ddl_name,
					name";

			return _db.Database.SqlQuery<QuoteCustomer>(sql, buId, user.VisibleBusinessUnits).ToArray();
		}

		public QuoteCustomer GetQuoteCustomerById(int custId)
		{
			return _db.Database.SqlQuery<QuoteCustomer>(
				$@"{VIEW_QUOTE_CUSTOMER()} where a.customer_id=@p0", custId
				).FirstOrDefault();
		}

		private const string VIEW_QUOTE_QUOTE = @"	SELECT CONCAT(a.quote_id, a.revision) Value, 
					CONCAT(a.quote_id, ' v', a.revision, ' - ', a.job_description, ' - $', a.quoted_price) Label 
					FROM quote_master a 
					LEFT JOIN customer b ON a.customer_id = b.customer_id 
					LEFT join business_unit c ON a.business_unit_id = c.id  
					";

		public LabelValueInt[] GetQuoteQuoteList(int buId, int customerId)
		{
			 var sql = $@"{VIEW_QUOTE_QUOTE}
					WHERE a.status_id NOT IN (6,7,9,8) 
					and b.customer_id =@p1 
					AND a.business_unit_id = @p0 
					ORDER BY c.name, a.quote_id desc";
			return _db.Database.SqlQuery<LabelValueInt>(sql, buId, customerId).ToArray();
		}


		public LabelValueInt GetQuoteQuoteById(int quoteId)
		{
			return _db.Database.SqlQuery<LabelValueInt>($@"SELECT * FROM ({VIEW_QUOTE_QUOTE}) tb where tb.Value = @p0", quoteId).FirstOrDefault();
		}

		public string UpdateQuote(Employee user, UpdateTimeSheetQuote model)
		{
            // Limit daily cumulative time entry to 24 hours
            var error = ExceedMaxHoursOnOneDay_UpdateCase(user, model);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }

            var ret = GetEntityById(model.MemberTime_ID, out membertime entity, out wocomment comment);
			if (!string.IsNullOrEmpty(ret)) return ret;
			comment.Member_ID_Audit = user.Id;
			entity.wo_percent_complete = model.PercentComplete;
			entity.NumberOfHours = model.NumberOfHours;
			entity.rating = model.Rating;
			return UpdateTimeSheet(user, entity, comment);
		}

		public string SaveQuote(Employee user, InsertTimeSheetQuote model)
		{
            // Limit daily cumulative time entry to 24 hours
            var error = ExceedMaxHoursOnOneDay_InsertCase(user, model);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }

            var res = SetInsertEntity(model, out membertime entity, out wocomment comment);
			if (!string.IsNullOrWhiteSpace(res)) return res;
			entity.WOType = "Quote";
			entity.membertime_shop_type_id = 0;
			entity.MemberTime_WOProg_id = 0;
			comment.comments = "Quoted Job.  See Quote Manager for details.";

			return SaveTimeSheet(user, entity, comment, model.WoProg_Bvwo);
		}

        private string ExceedMaxHoursOnOneDay_InsertCase(Employee user, InsertTimeSheetQuote model)
        {
            CumulativeParameter parameter = new CumulativeParameter();
            parameter.OnWhichDate = model.Date;
            parameter.ForWho = user.Id;
            parameter.action = TimesheetAction.Add;
            parameter.newValue = model.NumberOfHours;
            parameter.oldValue = 0;

            var result = this.GetCumulativeInformation(parameter);
            if (result.DoesItExceedMaxValue)
            {
                return result.Errors[0].error;
            }
            else
            {
                // Not Exceeds
                return "";
            }
        }

        private string ExceedMaxHoursOnOneDay_UpdateCase(Employee user, UpdateTimeSheetQuote model)
        {
            CumulativeParameter parameter = new CumulativeParameter();
            parameter.OnWhichDate = model.Date;
            parameter.ForWho = model.SelectedUserId;
            parameter.action = TimesheetAction.Edit;
            parameter.newValue = model.NumberOfHours;
            parameter.oldValue = this.GetbyId(model.MemberTime_ID).NumberOfHours.Value;

            var result = this.GetCumulativeInformation(parameter);
            if (result.DoesItExceedMaxValue)
            {
                return result.Errors[0].error;
            }
            else
            {
                // Not Exceeds
                return "";
            }
        }
    }
}