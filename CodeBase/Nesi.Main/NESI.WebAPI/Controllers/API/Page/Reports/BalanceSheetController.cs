using NESI.BLL.Pages.Reports;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    [PageAuthorizationFilter(139)]
    [RoutePrefix("api/Page/BalanceSheet")]
    public class BalanceSheetController : EmployeeController
    {
        [Route("Search")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBalanceSheetData([FromUri] BalanceSheetInputParameter parameters)
        {
            var fetcher = new BalanceSheetFetcher();
            var data = await fetcher.Get(parameters);
            return Ok(data);
        }

        [Route("TaxEntity")]
        [HttpGet]
        public IHttpActionResult GetTaxEntities()
        {
            var fetcher = new BaseAccess();
            var data = fetcher.GetTaxEntities(this.CurrentUser.VisibleTaxEntities);
            return Ok(data);
        }

        [Route("BusinessUnit")]
        [HttpGet]
        public IHttpActionResult GetBus(int taxid)
        {
            var fetcher = new BaseAccess();
            var data = fetcher.GetBusinessUnits(taxid,
                CurrentUser.BusinessUnit.is_backoffice ?? false,
                CurrentUser.BusinessUnit.is_corporate ?? false,
                CurrentUser.BusinessUnit.is_backoffice ?? false,
                CurrentUser.BusinessUnit.is_corporate ?? false,
                CurrentUser.Id);
            return Ok(data);
        }

        [Route("fiscal")]
        [HttpGet]
        public IHttpActionResult GetFiscal(int taxid)
        {
            var fetcher = new BaseAccess();
            var data = fetcher.GetFiscalPeriodRecords(taxid);
            return Ok(data);
        }

        [Route("initial")]
        [HttpGet]
        public IHttpActionResult GetInitialData()
        {
            var fetcher = new BaseAccess();
            var data = fetcher.GetInitialData(
                CurrentUser.VisibleTaxEntities,
                CurrentUser.TaxEntityId,
                CurrentUser.BusinessUnitId,
                CurrentUser.BusinessUnit.is_backoffice ?? false,
                CurrentUser.BusinessUnit.is_corporate ?? false,
                CurrentUser.BusinessUnit.is_backoffice ?? false,
                CurrentUser.BusinessUnit.is_corporate ?? false,
                CurrentUser.Id);
            return Ok(data);
        }
    }

    #region NetSuit stuff
    public class BalanceSheetFetcher
    {
        #region code to get the report data
        public async Task<NetSuiteBalanceSheetAccountData> Get(BalanceSheetInputParameter parameter)
        {
            var incomeStatementInput = this.PrepareInputData(parameter);
            if (!incomeStatementInput.Valid)
            {
                // If the data is not good, return an empty list.
                return new NetSuiteBalanceSheetAccountData { accounts = new List<NetSuiteBalancesheetAccountRecord> { }, success = false };
            }

            var netsuiteData = await NetSuite_BalanceSheet.GetIncomeStatement(incomeStatementInput.SubSidiaryList, incomeStatementInput.FullyDate);

            return netsuiteData;
        }

        private BalanceSheetInput PrepareInputData(BalanceSheetInputParameter parameter)
        {
            var incomeStatementInput = new BalanceSheetInput() { Valid = false };
            if (parameter == null || string.IsNullOrWhiteSpace(parameter.businessUnitList) ||
                string.IsNullOrWhiteSpace(parameter.fiscalPeriod) || parameter.taxId == 0)
            {
                return incomeStatementInput;
            }

            incomeStatementInput.businessUnitList = parameter.businessUnitList;
            incomeStatementInput.taxId = parameter.taxId;

            // Date
            string[] Year_Month = parameter.fiscalPeriod.Split('_');
            if (Year_Month.Length != 2)
            {
                return incomeStatementInput;
            }

            // SubSidiaryID List
            incomeStatementInput.SubSidiaryList =
                this.GetSubsidiaryIDsBasedOnSelectedBusinessUint(parameter.businessUnitList, parameter.taxId);
            if (incomeStatementInput.SubSidiaryList.Count == 0)
            {
                // No subsidiary ID in list
                return incomeStatementInput;
            }

            incomeStatementInput.Year = Convert.ToInt32(Year_Month[0]);
            incomeStatementInput.Month = Convert.ToInt32(Year_Month[1]);
            incomeStatementInput.CompleteDate = $"{incomeStatementInput.Month}/{1}/{incomeStatementInput.Year}"; // mm/dd/yyyy
            incomeStatementInput.FullyDate = Convert.ToDateTime(incomeStatementInput.CompleteDate);

            incomeStatementInput.Valid = true;
            return incomeStatementInput;
        }

        private List<TaskData> GetSubsidiaryIDsBasedOnSelectedBusinessUint(string selectedBU, int taxId)
        {
            var fetch = new BaseAccess();
            return fetch.GetSubsidiaryIDsBasedOnSelectedBusinessUint(selectedBU, taxId);
        }
        #endregion
    }

    public class NetSuite_BalanceSheet
    {
        public static async Task<NetSuiteBalanceSheetAccountData> GetIncomeStatement(List<TaskData> subsidiaryIdList, DateTime accountingDate)
        {
            var data = new NetSuiteBalanceSheetAccountData
            {
                success = false,
                accounts = new List<NetSuiteBalancesheetAccountRecord> { }
            };

            if (subsidiaryIdList.Count == 0)
            {
                return data;
            }

            List<Task<string>> tasks = new List<Task<string>>();

            return data;
        }
    }

    #endregion
}
