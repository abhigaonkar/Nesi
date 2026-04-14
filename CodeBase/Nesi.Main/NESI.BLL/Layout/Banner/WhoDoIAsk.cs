using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;

namespace NESI.BLL.Layout.Banner
{
    public class WhoDoIAsk: BLLBase
    {

        public WhoDoIAsk(Employee user): base(user)
        {
        }
    
        public DTO.ViewModels.CurrentUser.Layout.WhoDoIAsk[] GetWhoDoIAsks()
        {


          


            var whodt = _db.Database.SqlQuery<DTO.ViewModels.CurrentUser.Layout.WhoDoIAsk>(
                @"SELECT
  a.who_to_ask_question AS Question,
  IF(
    a.who_to_ask_memberid != 0,
    c.member_fullname,
    b.member_fullname
  ) AS TalkTo
FROM
  who_to_ask a
  LEFT JOIN member b
    ON a.who_to_ask_membertype = b.Member_MemberType_ID
    AND b.business_unit_id = @p0
    AND b.Member_Status = 'Active'
  LEFT JOIN member c
    ON a.who_to_ask_memberid = c.member_id
    AND c.member_status = 'Active'",  
                CurrentUser.BusinessUnitId).ToList();

            return whodt.ToArray();
        }
    }
}