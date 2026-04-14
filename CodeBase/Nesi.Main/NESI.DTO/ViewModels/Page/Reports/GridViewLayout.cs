using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Reports
{
    public class GridViewLayout
    {
        public int id { get; set; }

        public long? memberId { get; set; }

        public string name { get; set; }
        public string firstName { get; set; }

        //Since this value is saved in Layout table using EF, there is not need to add [SQLInjection]. EF itself neutralizes the SQL Injection attempts.
        // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/security-considerations
        public string layout { get; set; }

        public int? isDefault { get; set; }

        [SQLInjection]
        public string gridId { get; set; }
        public string fullName { get; set; }
        public string ddlName { get; set; }
    }
}
