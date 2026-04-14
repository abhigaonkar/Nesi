using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Page.TimeSheet;

namespace NESI.BLL.Pages.Timesheet
{
	public partial class Timesheet
	{
		public string UpdateTelem(Employee user, UpdateTimeSheetTelem model)
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
			comment.comments = model.MemberTime_WoComment;
			entity.NumberOfHours = model.NumberOfHours;
			entity.rating = model.Rating;

			return UpdateTimeSheet(user,entity, comment, model.Record);
		}

		public string SaveTelem(Employee user, InsertTimeSheetTelem model)
		{
            // Limit daily cumulative time entry to 24 hours
            var error = ExceedMaxHoursOnOneDay_InsertCase(user, model);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }

            model.PercentComplete = 100;
			
			var res = SetInsertEntity(model, out membertime entity, out wocomment comment);
			if (!string.IsNullOrWhiteSpace(res)) return res;
			entity.WOType = "Telem";
			entity.membertime_shop_type_id = 0;
			entity.membertype_id = user.EmployeeProfile.member_membertype_id;
			entity.membertype_chargeout_id = 0;
			entity.Date = model.LocalDate;

			return SaveTimeSheet(user, entity, comment, model.WoProg_Bvwo, model.Record);

		}

        private string ExceedMaxHoursOnOneDay_InsertCase(Employee user, InsertTimeSheetTelem model)
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

        private string ExceedMaxHoursOnOneDay_UpdateCase(Employee user, UpdateTimeSheetTelem model)
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