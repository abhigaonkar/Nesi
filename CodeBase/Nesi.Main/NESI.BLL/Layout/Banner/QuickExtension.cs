using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;

namespace NESI.BLL.Layout.Banner
{
    public class QuickExtension: BLLBase
    {

        public QuickExtension(Employee user): base(user)
        {
        }


        public DTO.ViewModels.CurrentUser.Layout.QuickExtension[] GetQuickExtensions()
        {

            

            var quickExtensions = _db.Database.SqlQuery<DTO.ViewModels.CurrentUser.Layout.QuickExtension>(
                @"SELECT DISTINCT
  CONCAT(
    b.member_nickname,
    ' ',
    b.member_lastname
  ) AS Name,
  cellphone_number.number AS Phone,
  b.Member_PhoneExtension AS Extension
FROM
  quick_extensions AS a
  INNER JOIN member AS b
    ON b.Member_ID = a.Quick_Extensions_Member_ID
  LEFT JOIN cellphone_number
    ON cellphone_number.id = b.cellphone_number_id
WHERE a.quick_extensions_mymember_id = @p0
  AND b.member_status = 'active'",
                CurrentUser.Id).ToList();

            return quickExtensions.ToArray();
        }
    }




}