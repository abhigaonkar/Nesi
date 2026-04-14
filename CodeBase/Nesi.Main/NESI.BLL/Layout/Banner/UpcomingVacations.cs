using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;

namespace NESI.BLL.Layout.Banner
{
    public class UpcomingVacations: BLLBase
    {

        public UpcomingVacations(Employee user): base(user)
        {
        }
    
        public DTO.ViewModels.CurrentUser.Layout.UpcomingVacations[] GetUpcomingVacations()
        {


	        // check if this actually returns correct data
            var sqlNEW = @"
SELECT 
	b.member_fullname AS NAME,
	MIN(DATE(a.date_start)) startDate,
	a.date_return ReturnDate,
	c.ddl_name BusinessUnitName
FROM
	vacation_master a
JOIN 
	member b ON a.member_id = b.Member_ID
JOIN 
	business_unit c ON b.business_unit_id = c.id 
WHERE
	a.date_start BETWEEN CAST( CURRENT_DATE AS DATE) AND CAST(CURDATE()+INTERVAL 2 WEEK AS DATE)
AND
	b.member_status = 'Active' AND 
	a.status=3 AND 
	FIND_IN_SET (c.id,@p0)
GROUP BY 
	b.member_fullname, a.date_return, c.ddl_name
ORDER BY 
	c.name,b.member_lastname, b.member_nickname, a.date_start,a.date_return,b.member_fullname, a.date_return";

            var whodt = _db.Database.SqlQuery<DTO.ViewModels.CurrentUser.Layout.UpcomingVacations>(
                sqlNEW,  
                CurrentUser.VisibleBusinessUnits).ToList();

            return whodt.ToArray();
        }
    }
}