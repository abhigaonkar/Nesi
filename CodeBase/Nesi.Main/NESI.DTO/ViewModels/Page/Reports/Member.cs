using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Reports
{
    public class Member
    {
        public int MemberID { get; set; }

        [SQLInjection()]
        public string MemberName { get; set; }

    }
}
