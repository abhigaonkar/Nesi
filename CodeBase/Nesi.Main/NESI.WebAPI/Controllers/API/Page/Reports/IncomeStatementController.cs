using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using nesi.core;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using NESI.BLL.Common.Shared;
using NESI.BLL.Pages.Reports;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    [PageAuthorizationFilter(52)]
    [RoutePrefix("api/Page/IncomeStatement")]
    public class IncomeStatementController : EmployeeController
    {
        [Route("Search")]
        [HttpGet]
        public async Task<IHttpActionResult> GetIncomeStatementData([FromUri] IncomeStatementInputParameter parameters)
        {
            var fetcher = new Fetcher();
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

        [Route("Fiscal")]
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
    public class Fetcher
    {
        #region code to get the report data
        public async Task<NetSuiteAccountData> Get(IncomeStatementInputParameter parameter)
        {
            var incomeStatementInput = this.PrepareInputData(parameter);
            if (!incomeStatementInput.Valid)
            {
                // If the data is not good, return an empty list.
                return new NetSuiteAccountData { accounts = new List<NetSuiteAccountRecord> { }, success = false, message = "Report parameters are incorrect." };
            }

            var netsuiteData = await NetSuite_IncomeReport.GetIncomeStatement(incomeStatementInput.SubSidiaryList, incomeStatementInput.FullyDate);

            return netsuiteData;
        }

        private IncomeStatementInput PrepareInputData(IncomeStatementInputParameter parameter)
        {
            var incomeStatementInput = new IncomeStatementInput() { Valid = false };
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

            //incomeStatementInput.subSidiaryID = this.GetSubSidiaryID(parameter.taxId);
            //incomeStatementInput.SubSidiarayList.Add(incomeStatementInput.subSidiaryID.ToString()); // bring the subsidiary id on tax_entity level.

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

    public class NetSuite_IncomeReport
    {
        public static async Task<NetSuiteAccountData> GetIncomeStatement(List<TaskData> subsidiaryIdList, DateTime accountingDate)
        {
            var data = new NetSuiteAccountData
            {
                success = false,
                accounts = new List<NetSuiteAccountRecord> { },
                message = "There was an error fetching the income statement",
                url = "",
                name = ""
            };

            if (subsidiaryIdList.Count == 0)
            {
                return data;
            }

            List<Task<string>> tasks = new List<Task<string>>();
            foreach (var bu in subsidiaryIdList)
            {
                var task = GetPlainIncomeStatement(bu.SubSidiaryId, accountingDate);
                bu.TaskID = task.Id;
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            foreach (var task in tasks) // Only 1 is supported
            {
                if (string.IsNullOrWhiteSpace(task.Result) || task.Result == "Error!")
                {
                    return data;
                }

                data = new JavaScriptSerializer().Deserialize<NetSuiteAccountData>(task.Result);
                data.accounts = new List<NetSuiteAccountRecord> { };
                if (string.IsNullOrEmpty(data.url))
                {
                    data.url = "";
                }
                if (string.IsNullOrEmpty(data.name))
                {
                    data.name = "";
                }
                if (string.IsNullOrEmpty(data.message))
                {
                    data.message = "";
                }

                if (data.success)
                {
                    // One time token.
                    data.message = "The income statement report file will be automatically downloaded. If not, please verify that popups are being allowed for this page, then re-submit your request.";
                }
            }

            return data;
        }

        private static void UpdateBusinessName(NetSuiteAccountData dataFromTask, string buName)
        {
            if (string.IsNullOrWhiteSpace(buName))
            {
                return;
            }

            foreach (var record in dataFromTask.accounts)
            {
                record.buName = buName;
            }
        }

        private static string GetBusinessUnitBasedOnTask(int taskID, List<TaskData> TaskDataList)
        {
            var buName = "";
            if (taskID == 0 || TaskDataList.Count == 0)
            {
                return buName;
            }

            foreach (var taskData in TaskDataList)
            {
                if (taskData.TaskID == taskID)
                {
                    buName = taskData.Name;
                    break;
                }
            }

            return buName;
        }

        private static List<NetSuiteAccountRecord> Merge(List<NetSuiteAccountRecord> accounts)
        {
            List<NetSuiteAccountRecord> list = new List<NetSuiteAccountRecord>{};
            return list;
        }

        private static async Task<NetSuiteAccountData> MappingFromNetSuiteToNesiWorld(NetSuiteAccountData netSuiteData)
        {
            // do what ever pre-patch in here.
            if (netSuiteData == null || !netSuiteData.success || netSuiteData.accounts == null ||
                netSuiteData.accounts.Count == 0)
            {
                return netSuiteData;
            }

            foreach (var record in netSuiteData.accounts)
            {
                //
                // Seperate two parts
                //
                record.accountNameFirstPart = record.AccountName;

                if (string.IsNullOrWhiteSpace(record.buName))
                {
                    record.AccountNameSecondPart = record.AccountName  + record.buName;
                }
                else
                {
                    record.AccountNameSecondPart = record.AccountName + " (" + record.buName + ")";
                }

                if (record.AccountName.Contains(":"))
                {
                    string[] twoParts = record.AccountName.Split(':');
                    record.accountNameFirstPart = twoParts[0];

                    if (string.IsNullOrWhiteSpace(record.buName))
                    {
                        record.AccountNameSecondPart = record.AccountName + record.buName;
                    }
                    else
                    {
                        record.AccountNameSecondPart = record.AccountName + " (" + record.buName + ")";
                    }
                }

                if (record.Currency == 1)
                {
                    record.strCurrency = "CAN";
                }
                else
                {
                    record.strCurrency = "US";
                }

                //
                // Top Type
                //
                if (record.Type == AccountType.COGS.ToString() || record.Type == AccountType.Income.ToString() || record.Type == AccountType.Expense.ToString())
                {
                    record.pos = Pos.OrdinaryIncomeExpense.ToString();
                }
                else if (record.Type == AccountType.OthIncome.ToString() || record.Type == AccountType.OthExpense.ToString())
                {
                    record.pos = Pos.OtherIncomeExpense.ToString();
                }
                else
                {
                    record.pos = Pos.Unknown.ToString();
                }

                //
                // Prepare data
                //
                PrepareRecord(record);

                //
                // profile 
                //
                record.profile = 0;
                if (record.pos == Pos.OrdinaryIncomeExpense.ToString() && (record.Type == AccountType.COGS.ToString() || record.Type == AccountType.Income.ToString()))
                {
                    record.profile = record.Amount;
                }

                // Set the order
                CalculateSortOrder(record);
            }

            netSuiteData.accounts = netSuiteData.accounts.OrderBy(item => item.sortOrder).ToList();

            //
            // Calculate the precentage
            //
            // CalculatePercentage(netSuiteData);
            // CalculatePercentage2(netSuiteData);
            CalculatePercentage3(netSuiteData);
            return netSuiteData;
        }

        private static void CalculateSortOrder(NetSuiteAccountRecord record)
        {
            if (record.pos == Pos.OrdinaryIncomeExpense.ToString())
            {
                if (record.Type == AccountType.Income.ToString())
                {
                    record.sortOrder = " "; // one space
                }

                if (record.Type == AccountType.COGS.ToString())
                {
                    record.sortOrder = "  "; // two spaces
                }

                if (record.Type == AccountType.Expense.ToString())
                {
                    record.sortOrder = "   "; // three spaces
                }
            }

            if (record.pos == Pos.OtherIncomeExpense.ToString())
            {
                if (record.Type == AccountType.OthIncome.ToString())
                {
                    record.sortOrder = " "; // one
                }

                if (record.Type == AccountType.OthExpense.ToString())
                {
                    record.sortOrder = "  "; // two
                }
            }

            // we are using spaces to help do the sort. idea from Matt.
            record.Type = record.sortOrder  + record.Type;
        }

        private static void CalculatePercentage3(NetSuiteAccountData netSuiteData)
        {
            // do what ever pre-patch in here.
            if (netSuiteData == null || !netSuiteData.success || netSuiteData.accounts == null ||
                netSuiteData.accounts.Count == 0)
            {
                return;
            }

            var incomeFromOridinary = netSuiteData
                                      .accounts.Where(x =>
                                          x.pos == Pos.OrdinaryIncomeExpense.ToString() &&
                                          x.Type.Trim() == AccountType.Income.ToString()).Sum(x => x.Amount);

            var incomeFromOtherOridinary = netSuiteData
                                           .accounts.Where(x =>
                                               x.pos == Pos.OtherIncomeExpense.ToString() &&
                                               x.Type.Trim() == AccountType.OthIncome.ToString()).Sum(x => x.Amount);

            var income = incomeFromOridinary;

            // Calculate for each recrod
            foreach (var record in netSuiteData.accounts)
            {
                if (record.pos == Pos.OrdinaryIncomeExpense.ToString())
                {
                    if (income != 0)
                    {
                        record.Percentage = record.Amount / income;
                    }
                }
            }
        }

        private static void PrepareRecord(NetSuiteAccountRecord record)
        {
            if (record.pos == Pos.OrdinaryIncomeExpense.ToString())
            {
                if (record.Type.Trim() == AccountType.COGS.ToString() || record.Type.Trim() == AccountType.Expense.ToString())
                {
                    record.Amount = -record.Amount;
                }
            }

            if (record.pos == Pos.OtherIncomeExpense.ToString())
            {
                if (record.Type.Trim() == AccountType.OthExpense.ToString())
                {
                    record.Amount = -record.Amount;
                }
            }
        }

        private static async Task<string> GetPlainIncomeStatement(string subsidiaryId, DateTime accountingDate)
        {
            string baseUrl = Configuration.GetNetSuiteIncomeStatementUrl;
            string realm = Configuration.GetNetSuiteRealm;
            string consumerKey = Configuration.GetNetSuiteConsumerKey;
            string consumerSecret = Configuration.GetNetSuiteConsumerSecret;
            string token = Configuration.GetNetSuiteToken;
            string tokenSecret = Configuration.GetNetSuiteTokenSecret;

            var authorizationHeader = new OAuthUtility().GenerateOauthHeader(
                        baseUrl,
                        realm,
                        consumerKey,
                        consumerSecret,
                        token,
                        tokenSecret,
                        "POST"
                );

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(baseUrl),
            };

            requestMessage.Headers.TryAddWithoutValidation("Accept", "application/json");
            requestMessage.Headers.TryAddWithoutValidation("Authorization", authorizationHeader);
            var model = new Model {  subsidiary = subsidiaryId.ToString(), accountingperiod = accountingDate.ToString("MMMMM yyyy", CultureInfo.InvariantCulture), debug="" };
            var json = new JavaScriptSerializer().Serialize(model);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var httpClient = new HttpClient();

            try
            {
                using (httpClient)
                {
                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                    requestMessage.Content = stringContent;
                    var response = await httpClient.SendAsync(requestMessage);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
            }
            catch (Exception ex)
            {
                return "Error!";
            }
            finally
            {
            }
        }

        class Model
        {
            public string subsidiary { get; set; }
            public string accountingperiod { get; set; }
            public string debug { get; set; }
        }
    }

    #endregion
}
