using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
    public class IncomeStatementInputParameter
    {
        public int taxId { get; set; }
        public string businessUnitList { get; set; }
        public string fiscalPeriod { get; set; }
        public string query { get; set; }
        public int subSidiaryID { get; set; }
        public List<TaskData> SubSidiaryList { get; set; }
    }

    public class IncomeStatementInput : IncomeStatementInputParameter
    {
        public bool Valid { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string CompleteDate { get; set; }
        public DateTime FullyDate { get; set; }
    }

    public class IncomeStatementData
    {
        public string random { get; set; }
        public string last_update { get; set; }

        public List<IncomeStatementRecord> records { get; set; }

        public object innerOBJ { get; set; }

        public bool validInut { get; set; }
    }

    public class IncomeStatementRecord
    {
        public string acct_no { get; set; }
        public double ytd { get; set; }
        public string account { get; set; }
        public double mtd { get; set; }
        public double bmtd { get; set; }
        public double bytd { get; set; }
        public double bytddiff { get; set; }
        public double bmtddiff { get; set; }
        public string gl_group_alias { get; set; }
        public string drcr { get; set; }
        public string type { get; set; }
        public string pos { get; set; }
        public DateTime date_modified { get; set; }
        public string date_modified_string { get; set; }

        public double ytd_sales { get; set; }
        public double mtd_sales { get; set; }
        public string note { get; set; }
    }


    public enum AccountType
    {
        Income,
        OthIncome,

        COGS,

        Expense,
        OthExpense
    }

    public enum Pos
    {
        OrdinaryIncomeExpense,
        OtherIncomeExpense,
        Unknown
    }

    public class NetSuiteAccountRecord
    {
        public string AccountNSID { get; set; }
        public string AccountName { get; set; }
        public string Type { get; set; } // Income / COGS / Expense / OthExpense
        public double Amount { get; set; }
        public int Currency { get; set; }
        public string Description { get; set; }

        // Required by angular but not included in api.
        public string accountNameFirstPart { get; set; }
        public string AccountNameSecondPart { get; set; }
        public double Percentage { get; set; }
        public string strCurrency { get; set; }
        public string pos { get; set; }
        public double profile { get; set; }

        public string buName { get; set; }

        public string sortOrder { get; set; }
    }

    public class NetSuiteAccountData
    {
        public bool success { get; set; }
        public List<NetSuiteAccountRecord> accounts { get; set; }

        public string url;

        // error
        public string name { get; set; }
        public string message { get; set; }
        public bool notifyOff { get; set; }
    }
}
