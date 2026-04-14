using System.Linq;
using AutoMapper;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.Models.TimeSheet;
using NESI.DTO.ViewModels.Page.TimeSheet;
using nesi.core;
using System;
using NESI.DTO.ViewModels.Core;
using System.Data;
using System.Data.Entity.Migrations;

namespace NESI.BLL.Pages.Timesheet
{
	public partial class Timesheet
	{
		public MemberTimeShopType[] GetShopTypeList(Employee user)
		{
            var info = bllToolbox.doSQL_string(@"SELECT REPORTS_TO(@v0)", new object[] { CurrentUser.Id });
            var list = !string.IsNullOrEmpty(info) ?
                 (from x in _db.membertime_shop_type where x.status == "active" orderby x.type select x).ToArray()
                : (from x in _db.membertime_shop_type where x.status == "active" && x.id != 15 orderby x.type select x).ToArray();
            return AutoMapper.Mapper.Map<MemberTimeShopType[]>(list);
        }

		public string UpdateShop(Employee user, UpdateTimeSheetShop model)
		{
            // Limit daily cumulative time entry to 24 hours
            var error = ExceedMaxHoursOnOneDay_UpdateCase(user, model);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }

            var ret = GetEntityById(model.MemberTime_ID, out membertime entity, out wocomment comment);
			if (!string.IsNullOrEmpty(ret)) return ret;
			comment.comments = model.MemberTime_WoComment;
			comment.Member_ID_Audit = user.Id;
			entity.NumberOfHours = model.NumberOfHours;
			entity.rating = model.Rating;
            entity.internal_project_id = model.Internal_Project_Id;
            return UpdateTimeSheet(user, entity, comment);

		}

		public string SaveShop(Employee user, InsertTimeSheetShop model)
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
			entity.WOType = "Shop";

			entity.membertime_shop_type_id = model.SelectedShopTimeTypeId;
			entity.MemberTime_WOProg_id = model.SelectedShopTimeTypeId;
            entity.internal_project_id = model.internal_project_id;
            return SaveTimeSheet(user, entity, comment, model.WoProg_Bvwo, null, 0, model);
        }
        public internal_project[] Get_InternalProjectList(Employee user)
        {
            var info = bllToolbox.doSQL_string(@"SELECT REPORTS_TO(@v0)", new object[] { user.Id });
            var list = new internal_project[] { };
            if (!string.IsNullOrEmpty(info))
            {
                list = (from x in _db.internal_project
                        where x.member_id == user.Id && x.active
                        orderby x.name
                        select x).ToArray();
                return AutoMapper.Mapper.Map<internal_project[]>(list);
            }
            return list;
        }
        public string UpdateProject(Employee user, UpdateTimeSheetProject model)
        {

            if (model != null)
            {
                var entity = _db.internal_project.Where(x => x.id == model.id && x.active == true).FirstOrDefault();
                if (model.id < 0) { throw new Exception(); }
                entity.name = model.name;
                entity.member_id = user.Id;
                var match = (from u in _db.internal_project
                             where u.name.Equals(model.name)
                             select u.name).FirstOrDefault();
                if (!String.IsNullOrEmpty(match))
                {
                    return "Project already exists";
                }
                _db.internal_project.AddOrUpdate(entity);
                _db.SaveChanges();
                return "Updated successfully";
            }
            return "Error occured in updating";

        }
        public string New_Project(Employee user, InsertTimeSheetProject model)
        {



            if (model != null)
            {
                internal_project entity = new internal_project { };
                if (String.IsNullOrEmpty(model.name)) { throw new Exception(); }
                var match = (from u in _db.internal_project
                             where u.name.Equals(model.name)
                             select u.name).FirstOrDefault();
                if (!String.IsNullOrEmpty(match))
                {
                    return "Project already exists";
                }
                entity.name = model.name;
                entity.member_id = user.Id;
                entity.active = true;
                _db.internal_project.Add(entity);
                _db.SaveChanges();
                return "Saved successfully";
                //Insert
            }
            else
            {
                return "Error occured in saving";
            }



        }

        private string ExceedMaxHoursOnOneDay_InsertCase(Employee user, InsertTimeSheetShop model)
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

        private string ExceedMaxHoursOnOneDay_UpdateCase(Employee user, UpdateTimeSheetShop model)
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