using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Layout.Banner
{
	public class SwtichUser : BLLBase
	{
		private readonly Employee _originalUser;
		private readonly Guid _uid;

		public SwtichUser(Guid uid, Employee originalUser) : base()
		{
			_originalUser = originalUser;
			_uid = uid;
		}

		public bool CanSwitchUser
		{
			get
			{
				var countMappedUsers = _db.user_switch.Count(m => m.member_id == _originalUser.Id);
				return countMappedUsers > 0 || _originalUser.AuthenticatedForPrivilege(168);
			}
		}

		public void SwitchToUser(int memberId)
		{
			var emp = new Employee(memberId);
			Global.OnlineUser.Set(_uid, emp);
			ResetOrinialUser();
		}

		public void CancelSwitchToUser()
		{
			Global.OnlineUser.Set(_uid, _originalUser);
			ResetOrinialUser();
		}

		private void ResetOrinialUser()
		{
			_originalUser.SetIssueTime();
			Global.OriginalUser.Set(_uid, _originalUser);
		}


		public DataTable GetSwitchUserList()
		{

			// CALL get_visible_reporting_users(20)
			DataTable dt = null;
			if (_originalUser == null)
			{
				return null;
			}
			if (_originalUser.AuthenticatedForPrivilege(168))
			{
				var fromParentTE		= (from d in _db.tax_entity_hiearchy
										  where d.parent_tax_entity_id == _originalUser.TaxEntity.id
										  select d.tax_entity_id).Any();
				
				if (_originalUser.BusinessUnit.is_backoffice.GetValueOrDefault() || _originalUser.AuthenticatedForPrivilege(200)) // Full access
				{
					dt = _db.DataTable(@"SELECT 
b.name businessUnit_name,
b.id businessUnit_id, 
a.member_fullname member_name,
a.member_id member_id,

CONCAT(a.member_fullname,' (',a.member_id,') --- ',b.ddl_name,' (',b.id,')', IFNULL((SELECT
  ' FVR Lockout'
FROM
  member_fvr_hdr d
  LEFT JOIN member_fvr_dtl b
    ON d.id = b.member_fvr_hdr_id
  LEFT JOIN member_fvr_history c
    ON b.id = c.member_fvr_dtl_id
WHERE b.member_id = a.member_id
  AND d.active = 1
  AND b.active = 1
  AND IFNULL(c.confirmed, 0) = 0
AND expire_date < NOW() LIMIT 1), '')) label,
a.member_id VALUE 
FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id 
WHERE a.member_status = 'Active' ORDER BY a.business_unit_id, a.member_fullname
");
				}else if (_originalUser.AuthenticatedForPrivilege(196))
				{
					dt = _db.DataTable(@"
							SELECT 
								b.name businessUnit_name,
								b.id businessUnit_id, 
								a.member_fullname member_name,
								a.member_id member_id,
								CONCAT(a.member_fullname,'(',a.member_id,')---',b.name,'(',b.id,')') label,
								a.member_id value 
							FROM 
								member a 
							LEFT JOIN 
								business_unit b ON a.business_unit_id = b.id 
							WHERE 
								a.member_status = 'Active' AND 
								FIND_IN_SET(b.tax_entity_id, @p0)
							ORDER BY 
								a.business_unit_id, a.member_fullname", _originalUser.VisibleTaxEntities);	
				}
				else if(fromParentTE)
					{
					var finalTeList	= new List<int>();
					nesi.core.NeTaxEntity.GetChildren(ref finalTeList, _originalUser.TaxEntity.id);

					var csvList		= string.Join(",", finalTeList);
					
					dt = _db.DataTable(@"
							SELECT 
								b.name businessUnit_name,
								b.id businessUnit_id, 
								a.member_fullname member_name,
								a.member_id member_id,
								CONCAT(a.member_fullname,'(',a.member_id,')---',b.name,'(',b.id,')') label,
								a.member_id value 
							FROM 
								member a 
							LEFT JOIN 
								business_unit b ON a.business_unit_id = b.id 
							WHERE 
								a.member_status = 'Active' AND 
								FIND_IN_SET(b.tax_entity_id, @p0)
							ORDER BY 
								a.business_unit_id, a.member_fullname", csvList);
					}
				else
				{
					dt = _db.DataTable(@"SELECT

                            b.name businessUnit_name,
                            b.id businessUnit_id,
                            a.member_fullname member_name,
                            a.member_id member_id,
                            CONCAT(a.member_fullname, '(', a.member_id, ')---', b.name, '(', b.id, ')') label,
							a.member_id value

                            FROM member a
                            LEFT JOIN business_unit b ON a.business_unit_id = b.id

                            LEFT JOIN tax_entity te ON te.id = b.tax_entity_id


                            WHERE a.member_status = 'Active'

                            AND
                            ((is_supervisor(a.member_id, @p0) or b.id = 8 or a.member_id = @p0)

                            or
                            (is_owner(@p0, te.id) AND find_in_set(b.id, @p1))
							)
							ORDER BY a.business_unit_id, a.member_fullname", _originalUser.Id, _originalUser.VisibleBusinessUnits + ",8");
					/*    dt = _db.DataTable(@"SELECT 
                                b.name businessUnit_name,
                                b.id businessUnit_id, 
                                a.member_fullname member_name,
                                a.member_id member_id,
    CONCAT(a.member_fullname,'(',a.member_id,')---',b.name,'(',b.id,')') label,
                                a.member_id value 
                                FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id 
                                WHERE a.member_status = 'Active'
                                AND (is_supervisor(a.member_id, @p0) or b.id=8 or a.member_id=@p0)
                                AND find_in_set(b.id, @p1)
                                ORDER BY a.business_unit_id, a.member_fullname", _orignialUser.Id, _orignialUser.VisibleBusinessUnits + ",8");*/
				}

			}
			else
			{
				var countMappedUsers = _db.user_switch.Count(m => m.member_id == _originalUser.Id);
				if (countMappedUsers > 0)
				{
					dt = _db.DataTable(@"SELECT
  c.name businessUnit_name,
  c.id businessUnit_id,
  b.member_fullname member_name,
  a.mapped_member_id member_id,
  b.member_fullname label,
  a.mapped_member_id VALUE
FROM
  user_switch a
  LEFT JOIN member b
    ON a.mapped_member_id = b.member_id
  LEFT JOIN business_unit c
    ON b.business_unit_id = c.id
WHERE a.member_id = @p0
UNION
SELECT
   b.name businessUnit_name,
   b.id businessUnit_id,
   a.member_fullname mebmer_name,
  a.member_id member_id,
  a.member_fullname label,
  a.member_id VALUE
FROM
  member a
  LEFT JOIN business_unit b
    ON a.business_unit_id = b.id
WHERE a.member_id = @p0
ORDER BY businessUnit_id,
  label",
										_originalUser.Id);
				}
			}
			return dt;
		}
	}
}