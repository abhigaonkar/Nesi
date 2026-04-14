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
    public class PartnerSpecialties : BLLGridBase<DTO.ViewModels.Page.Reports.PartnerSpecialties>
    {
        public PartnerSpecialties()
        {
        }

        public PartnerSpecialties(Employee user) : base(user)
        {
        }

        public PartnerSpecialties(Employee user, BodyParams param) : base(user, param, new object[] { })
        {
            this.query = @"SELECT
partner_skills.id,
partner_skills.`table` type,
partner_skills.skill skill,
date(partner_skills.date_added) `dateAdded`,
member.member_fullname `addedBy`,
business_unit.ddl_name business_unit,
ifnull(customer.customer_name,
vendor.Vendor_Name) AS name,
CASE WHEN IFNULL(customer.is_partner,vendor.is_partner) = 1 THEN TRUE ELSE FALSE END is_partner,
ifnull(Concat(address.Address_PhoneArea,' ',address.Address_PhoneFirst,' ', address.Address_PhoneLast) ,
Concat(address1.Address_PhoneArea,' ',address1.Address_PhoneFirst,' ', address1.Address_PhoneLast)) phone

FROM
partner_skills
INNER JOIN member ON partner_skills.added_by = member.Member_ID
INNER join business_unit ON member.business_unit_id = business_unit.id
LEFT JOIN customer ON partner_skills.table_id = customer.customer_id AND partner_skills.`table` = 'Customer'
LEFT JOIN vendor ON partner_skills.table_id = vendor.Vendor_ID AND partner_skills.`table` = 'Vendor'
LEFT JOIN address on address.Address_Table_ID = customer.customer_id and address.address_table = 'Customer' and address.Address_Type = 'B'
LEFT JOIN address address1 on address1.Address_Table_ID = vendor.Vendor_ID and address1.address_table = 'Vendor' and address1.Address_Type = 'B'";

        }
    }
}
