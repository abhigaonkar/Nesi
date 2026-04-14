using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    public class MemberWithUnit : NESI.DTO.Models.Users.Member
    {
        public string ddlName { get; set; }
    }
}
