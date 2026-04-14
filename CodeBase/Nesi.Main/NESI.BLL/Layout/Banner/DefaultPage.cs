using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Layout.Banner
{
    public class DefaultPage : BLLBase
    {
        public DefaultPage(Employee user): base(user)
        {
        }
        public string SetDefaultPage(int id)
        {


            bllToolbox.doSQL_void(@"update member set member_default_page = @v0  where member_id =@v1 ", id, CurrentUser.Id);
            // var pageName = bllToolbox.doSQL_string(@"select menu_name from page where page_id =@v0 ", id);
            
            var page = from x in _db.page
                where x.page_id == id
                select x.menu_name;


            return page.FirstOrDefault();
        }

	    public int GetDefaultPageId()
	    {
		    return bllToolbox.doSQL_int(@"select ifnull(member_default_page,1) from member where member_id=@p0", UserId);
	    }
    }
}
