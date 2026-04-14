using NESI.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using NESI.BLL.Base;

namespace NESI.BLL.Pages.Reports
{
    public class Member : BLLBase
    {
        public List<DTO.ViewModels.Page.Reports.Member> GetList()
        {
            List<DTO.ViewModels.Page.Reports.Member> member = new List<DTO.ViewModels.Page.Reports.Member>();
            var list = _db.member.Select(p => new DTO.ViewModels.Page.Reports.Member()
            {
               MemberID = p.Member_ID,
               MemberName = p.member_fullname,

            }).ToList();
            return list;

        }
    }
}
