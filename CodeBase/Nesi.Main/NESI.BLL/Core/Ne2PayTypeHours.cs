using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Pages.Timesheet;
using System;

namespace NESI.BLL.Core
{
	public class Ne2PayTypeHours : BLLBase
	{
	//	public LinkedPayTypeHours [] GetList(int buid)
	//	{
    //        var str = @"SELECT a.* FROM paytypehours a LEFT JOIN bu_paytype_link b ON a.PayTypeHours_ID = b.paytype_id WHERE b.business_unit_id = @v0";
    //        return bllToolbox.doSQL_Array<LinkedPayTypeHours>(str,new object[] { buid});

	//	}
        public DTO.Models.Core.PayTypeHour[] GetList(int buid)
        {
            var allowUnlinked = (new Timesheet()).GetAllowUnlinkTimesheet(buid);

            if (allowUnlinked)
            {
                // No linked, so get the first three items.
                var listForUnlinked = bllToolbox.doSQL_Array<DTO.Models.Core.PayTypeHour>(@"SELECT * FROM paytypehours WHERE PayTypeHours_ID <= 3");
                if (listForUnlinked == null || listForUnlinked.Length == 0) { return new DTO.Models.Core.PayTypeHour[0]; }
                return listForUnlinked;
            }

            var list =bllToolbox.doSQL_Array<DTO.Models.Core.PayTypeHour>(@"SELECT a.* FROM paytypehours a LEFT JOIN bu_paytype_link b ON a.PayTypeHours_ID = b.paytype_id WHERE b.business_unit_id = @v0",buid);
            if(list == null || list.Length == 0) { return new DTO.Models.Core.PayTypeHour[0]; }
            return list;

        }
    

    }
}