using System;
using System.Collections.Generic;
using System.Data;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;
using dto = NESI.DTO.ViewModels.Page.Employees.EmployeeDiscplinaryGrid;

namespace NESI.BLL.Pages.Employees
{
    public class EmployeeDiscplinaryGrid : BLLGridBase<dto>
    {
        public EmployeeDiscplinaryGrid()
        {

        }
        public EmployeeDiscplinaryGrid(Employee user) : base(user)
        {

        }

        const string QUERY_SQL = @"SELECT
  mn.*,
  m.Member_fullname AS UserNote,
  mm.Member_fullname AS addedby,
  f.id file_id,
  f.name file_name
FROM
  MemberNote mn
  INNER JOIN Member m
    ON mn.Member_ID_Audit = m.Member_Id
  INNER JOIN Member mm
    ON mn.Membernote_addedby_member_id = mm.Member_Id
  LEFT JOIN filestore.files f
    ON mn.MemberNote_ID = f.sub_folder_id
    AND f.page_id = 127
    AND f.folder_id = 6";

        public EmployeeDiscplinaryGrid(Employee user, BodyParams param, int memberid) : base(user, param, new object[] { memberid })
        {
            this.query = QUERY_SQL + @"
WHERE MemberNote_Member_ID = @p0
ORDER BY mn.date DESC
			";
        }

        public DataExtra Add(dto model)
        {
            var mem = new NeMember(model.membernote_member_id);
            var business = mem.business_unit;

            var dis = new NeDisciplinary
            {
                active = true,
                comments = model.comments,
                business_unit_id = mem.business_unit.id,
                date = model.date.ToString("yyyy-MM-dd"),
                member_id_audit = UserId,
                member_note_added_by_member_id = UserId,
                member_note_member_id = model.membernote_member_id,
                note_type = "Disciplinary"
            };
            dis.add(model.membernote_member_id);
            if (!string.IsNullOrEmpty(model.file_name))
            {
                var fs = new BLL.Core.FileManager.EmployeeDiscplineFileStore(CurrentUser, dis.member_note_id);
                fs.Save(model.file_name);
            }
            ClearCache(new object[] { model.membernote_member_id });


            if ( (business.country != "CAN" && business.country != "CDN") || (!business.is_backoffice && !business.is_corporate))
            {
                var email = new NeEMail();
                var dt_super = NeMember.get_supervisors(mem.id);
                bool stop = false;
                var memberList = new List<int>();
                foreach (DataRow dr_super in dt_super.Rows)
                {
                    var memberId = Convert.ToInt32(dr_super["memberid"]);

                    if (model.membernote_member_id != memberId)
                    {
                        if (!string.IsNullOrEmpty(business.country))
                        {
                            var memberTypeID = Convert.ToInt32(dr_super["MemberTypeID"]);
                            if (business.country == "USA")
                            {
                                stop = memberTypeID == 35; // CEO
                            }
                            else if (business.country == "CAN" || business.country == "CDN")
                            {
                                stop = memberTypeID == 83; // President
                            }
                            else
                            {
                                throw new NotImplementedException("Country is not supported for this business unit.");
                            }
                        }
                        else
                        {
                            throw new Exception("Business Unit Country is not set.");
                        }


                        email.To += dr_super["NEEmail"].ToString() + ";";
                        memberList.Add(memberId);
                        if (stop)
                        {
                            break;
                        }
                    }
                }
                // if tax_entity.include_owners_in_disc_emails 
                // Owner should also get the emails
                var te = new NeTaxEntity(mem.business_unit.tax_entity_id);
                if (te.IncludeOwnersInDiscEmails)
                {
                    var list = NeTaxEntity.GetOwners(te.id);
                    foreach (var m in list)
                    {
                        var myOwner = new NeMember(m);

                        if (!memberList.Contains(m) && myOwner.business_unit.country == "USA")
                        {

                            email.To += myOwner.NEEmail + ";";
                        }

                    }
                }

                if (string.IsNullOrEmpty(email.To))
                {
                    email.To = "hr@" + Toolbox.app_setting("DomainForEmail");
                }

                if (email.To != "hr@" + Toolbox.app_setting("DomainForEmail"))
                {
                    email.CC = "hr@" + Toolbox.app_setting("DomainForEmail");
                }

                email.Subject = "Discipline Note Created";
                email.From = "admin@" + Toolbox.app_setting("DomainForEmail");

                email.isHTML = true;
                email.Body = "<font face='Arial'><a href='"+ Toolbox.app_setting("Domain") + "/#/opens/127/employees/" + mem.id + "'>Go to employee page</a><br>";
                email.Body += CurrentUser.FullName + " added a disciplinary note for " + mem.FullName + " on " + dis.date + "<br>";
                email.Body += "<br>Note: <br>";
                email.Body += model.comments + "<br></font>";
                /*					if (auc.UploadedFiles[0].FileName != "")
                                    {

                                            System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(auc.UploadedFiles[0].FileContent, "disc file");
                                            email.Attachment = att;
                                    }
                 */
                email.Send();
            }


			return new DataExtra()
			{
				Data = "Displinary has been saved successfully.",
				Extra = Get(0)
			};
		}


		public DataExtra Edit(dto model)
		{
			bllToolbox.doSQL_void(@"Update membernote set
			date = @v0,
			comments =@v1 
			where membernote_id = @v2 limit 1", model.date, model.comments, model.membernote_id);

			ClearCache(new object[] { model.membernote_member_id });
			if (!string.IsNullOrEmpty(model.file_name))
			{
				var fs = new BLL.Core.FileManager.EmployeeDiscplineFileStore(CurrentUser, model.membernote_id);
				fs.Save(model.file_name);
			}
			return new DataExtra()
			{
				Data = "Displinary has been saved successfully.",
				Extra = Get(0)
			};
		}

		public object Get(int id)
		{
			var newo = new dto
			{
				membernote_id = 0,
				date = DateTime.Now,
				comments = ""
			};

			var o = new
			{
				entity = id > 0 ? (bllToolbox.doSQL_Object<dto>(
					QUERY_SQL + @"
					WHERE membernote_id = @p0
					ORDER BY mn.date DESC
			", id) ?? newo) : newo
			};
			o.entity.file_name = "";
			return o;
		}

		public DataExtra Delete(int id,int memberid)
		{
			bllToolbox.doSQL_void(@"Delete from membernote where membernote_id =@v0  limit 1", id);
			bllToolbox.doSQL_void(@"Delete from filestore.files  where sub_folder_id =@v0 and page_id = 127 and folder_id = 6", id);
			ClearCache(new object[] { memberid });
			return new DataExtra("Delted successfully.", null);
		}
	}
}