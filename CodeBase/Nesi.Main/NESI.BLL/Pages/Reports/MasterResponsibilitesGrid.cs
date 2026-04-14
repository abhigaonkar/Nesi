using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
    public class MasterResponsibilitesGrid : BLLGridBase<DTO.ViewModels.Page.Reports.MasterResponsibilitesGrid>
    {
        public MasterResponsibilitesGrid()
        {
        }

        public MasterResponsibilitesGrid(Employee user) : base(user)
        {
        }

        public MasterResponsibilitesGrid(Employee user, BodyParams param) : base(user, param, new object[] { })
        {
            query = @"SELECT
membertype_responsibilities.core_responsibility_id crid,
business_unit.ddl_name name,
core_responsibilities.core_responsibility,
membertype.membertype_name title,
get_name(member.Member_id) should_be,
ifnull((select membertype.membertype_name from memberoffer_cr, member_offers, membertype where member_offers.membertypeid = membertype.membertype_id and member_offers.`status`= 'Accepted' and member_offers.isapplicant = 0 and memberoffer_cr.memberoffer_moid = member_offers.id and member_offers.id = member.Member_ID and memberoffer_cr.memberoffer_crid = core_responsibilities.id),null) AS offered_title,
ifnull((select get_name(member_offers.id) from memberoffer_cr, member_offers where member_offers.`status`= 'Accepted' and member_offers.isapplicant = 0 and memberoffer_cr.memberoffer_moid = member_offers.id and member_offers.id = member.Member_ID and memberoffer_cr.memberoffer_crid = core_responsibilities.id),null) AS offered_cr
FROM
core_responsibilities
LEFT JOIN membertype_responsibilities ON core_responsibilities.id = membertype_responsibilities.core_responsibility_id
INNER JOIN membertype ON membertype_responsibilities.membertype_id = membertype.membertype_id
INNER JOIN member ON member.Member_MemberType_ID = membertype.membertype_id and member.Member_Status = 'Active'
INNER join business_unit ON member.business_unit_id = business_unit.id
WHERE
core_responsibilities.`status` = 'Active' AND
business_unit.id in ({bu_ids})
union 

SELECT
core_responsibilities.id crid,
business_unit.ddl_name name,
core_responsibilities.core_responsibility,
null title,
null should_be,
membertype.membertype_name as offered_title,
get_name(member.Member_ID) as offered_cr
FROM
memberoffer_cr
INNER JOIN member_offers ON memberoffer_cr.memberoffer_moid = member_offers.id
INNER JOIN core_responsibilities ON memberoffer_cr.memberoffer_crid = core_responsibilities.id
INNER JOIN member on member_offers.id = member.Member_ID and member.Member_Status = 'Active'
inner join business_unit on member.business_unit_id = business_unit.id 
inner join membertype on member_offers.membertypeid = membertype.membertype_id
WHERE
member_offers.isapplicant = 0 AND
member_offers.`status` = 'Accepted' AND
memberoffer_cr.memberoffer_crid NOT IN ((
SELECT GROUP_CONCAT(DISTINCT membertype_responsibilities.core_responsibility_id) stuff 
FROM
          membertype_responsibilities 
where membertype_responsibilities.membertype_id = member.Member_MemberType_ID 
group by membertype_responsibilities.membertype_id
)) AND
business_unit.id in ({bu_ids})

ORDER BY
crid ASC,name";

        }
     }
}
