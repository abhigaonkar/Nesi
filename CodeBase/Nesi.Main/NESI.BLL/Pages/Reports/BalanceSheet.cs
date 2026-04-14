using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
    public class BalanceSheetInputParameter
    {
        public int taxId { get; set; }
        public string businessUnitList { get; set; }
        public string fiscalPeriod { get; set; }
        public string query { get; set; }
        public int subSidiaryID { get; set; }
        public List<TaskData> SubSidiaryList { get; set; }
    }

    public class BalanceSheetInput : BalanceSheetInputParameter
    {
        public bool Valid { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string CompleteDate { get; set; }
        public DateTime FullyDate { get; set; }
    }

    public class NetSuiteBalanceSheetAccountData
    {
        public bool success { get; set; }
        public List<NetSuiteBalancesheetAccountRecord> accounts { get; set; }
    }

    public class NetSuiteBalancesheetAccountRecord
    {
        public string AccountNSID { get; set; }
        public string AccountName { get; set; }
        public string Type { get; set; } // Income / COGS / Expense / OthExpense
        public double Amount { get; set; }

        // From OLD
        public int GLGROUP { get; set; }
        public string TYPE { get; set; }

        public int no_acct { get; set; }

        public string dr_cr_desig { get; set; }

        public double CURR { get; set; }

        public string ACCOUNT { get; set; }

        public int GL_GROUP { get; set; }


        public string buName { get; set; }
    }
}
