using System;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Applicant
{
	public class ApplicantEdit : ApplicantBase
	{
		public bool show_title_panel { get; set; }
		public bool auth_for_edit { get; set; }
		public bool auth_for_edit_all { get; set; }
		public bool is_branch_manager_hr { get; set; }
		public LabelValueInt[] businessUnitList { get; set; }
		public LabelValueString[] provinceList { get; set; }
		public LabelValueString[] countryList { get; set; }
		public LabelValueInt[] memberTypeList { get; set; }
		public DTO.ViewModels.Page.Applicant.ApplicantEdit entity { get; set; }
		public ApplicantEdit(Employee user) : base(user)
		{
		}

		public ApplicantEdit(Employee user, int applicantId) : base(user, applicantId)
		{
			show_title_panel = CurrentUser.AuthenticatedForPrivilege(5);
			auth_for_edit = CurrentUser.AuthenticatedForPrivilege(32);
			auth_for_edit_all = CurrentUser.AuthenticatedForPrivilege(152);
			is_branch_manager_hr = CurrentUser.AuthenticatedForPrivilege(33);

			businessUnitList = VisibleBusinessUnit();
			provinceList = GetProvList();
			countryList = GetCountryList2();
			memberTypeList = GetMemberTypeList(false);
			entity = new DTO.ViewModels.Page.Applicant.ApplicantEdit();
			if (applicantId > 0)
			{
				var o = new NeApplicant(applicantId);
				entity = (DTO.ViewModels.Page.Applicant.ApplicantEdit)MapperFrom(entity, o);
			}
			else
			{
				entity.dateentered = DateTime.Today;
				entity.addedbymemberid = UserId;
                entity.country = Toolbox.Contains(user.BusinessUnit.Country,new[] {"CAN","CDN"})?"CAN":user.BusinessUnit.Country;
				entity.status = "New";
				entity.postal = "";
				entity.address = "";
				entity.apt = "";
				entity.cellphone = "";
				entity.city = "";
				entity.email = "";
				entity.firstname = "";
				entity.lastname = "";
				entity.notes = "";
				entity.province = "";
			}

		}

		public DataExtra Save(DTO.ViewModels.Page.Applicant.ApplicantEdit model)
		{

			model.postal = string.IsNullOrEmpty(model.postal) ? "" : model.postal;
			model.postal = model.postal.Replace(" ", "").ToUpper();
			var o = model.id > 0 ? new NeApplicant(model.id) : new NeApplicant();
			if (model.id == 0)
			{
				o = (NeApplicant)MapperFrom(o, model);
				o.dateentered = DateTime.Today;
				o.addedbymemberid = UserId;
			}
			else
			{
				if (model.status == "Hired" && o.becomes_memberid == 0)
				{
					model.status = o.status;
					return new DataExtra(@"You cannot manually set the status to HIRED unless they are already an employee in the employee table. To convert them to an employee, you must an employment agreement and press the 'They Accepted' button.", model);
				}
				o = (NeApplicant)MapperFrom(o, model);
			}
			o.save();
			model.id = o.id;
			return new DataExtra("Applicant has been saved successfully.", model);
		}



	}
}