using System.Collections.Generic;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.BusinessUnit
{
    public class BusinessUnit : BLLBase
    {
        public List<DTO.ViewModels.Page.BusinessUnit.BusinessUnit> GetList(Employee user)
        {
           
                var list = _db.Database.SqlQuery<DTO.ViewModels.Page.BusinessUnit.BusinessUnit>(@"SELECT
  b.id Id,
  b.name Business_Unit,
  t.public_name Tax_Entity,
  CONCAT(
    b.address,
    ', ',
    b.city,
    ', ',
    b.Prov,
    b.state,
    ', ',
    b.postal,
    ', ',
    b.country
  ) Address,
  tax_id_no Tax_No,
  get_bm (b.id) business_unit_manager_id,
  (SELECT
    member.member_membertype_id
  FROM
    member
  WHERE member_id = business_unit_manager_id) manager_type_id,
  (SELECT
    member.member_fullname
  FROM
    member
  WHERE member_id = business_unit_manager_id) Manager,
  (SELECT
    membertype.membertype_name
  FROM
    membertype
  WHERE membertype_id = manager_type_id) Title,
  t.id Tax_Entity_Id,
  b.active Active
FROM
  business_unit b
  INNER JOIN tax_entity t
    ON t.id = b.tax_entity_id
WHERE FIND_IN_SET(b.id, @p0)", user.VisibleBusinessUnits);
                return list.ToList();
            }
    }
}