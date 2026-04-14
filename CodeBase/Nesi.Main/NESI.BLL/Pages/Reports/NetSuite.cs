using nesi.core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NESI.BLL.Pages.Reports
{
    #region Base Access
    public class BaseAccess
    {
        public List<TaskData> GetSubsidiaryIDsBasedOnSelectedBusinessUint(string selectedBU, int taxId)
        {
            List<TaskData> subSidiarayList = new List<TaskData> { };
            if (string.IsNullOrEmpty(selectedBU) || taxId == 0)
            {
                return subSidiarayList;
            }

            var _dt = Toolbox.doSQL_dt(@"SELECT NetSuite_BU_Internal_Id, ddl_name FROM business_unit WHERE tax_entity_id = @v0 and find_in_set(ID,@v1)", new object[] { taxId, selectedBU });
            if (_dt == null || _dt.Rows == null || _dt.Rows.Count == 0)
            {
                return subSidiarayList;
            }

            foreach (DataRow row in _dt.Rows)
            {
                var id = row["NetSuite_BU_Internal_Id"] != DBNull.Value
                    ? Convert.ToInt32(row["NetSuite_BU_Internal_Id"])
                    : 0;
                if (id != 0)
                {
                    subSidiarayList.Add(new TaskData { SubSidiaryId = id.ToString(), Name = row["ddl_name"].ToString() });
                }
            }

            return subSidiarayList;
        }

        private int GetSubSidiaryID(int taxId)
        {
            var _dt = Toolbox.doSQL_dt(
                @"SELECT NetSuite_TE_Internal_Id FROM tax_entity WHERE id = @v0",
                new object[] { taxId });

            if (_dt == null || _dt.Rows == null || _dt.Rows.Count != 1)
            {
                return 0;
            }

            var dr = _dt.Rows[0];
            var neetSuite_TE_Internal_Id = Convert.ToInt32(dr["NetSuite_TE_Internal_Id"]);
            return neetSuite_TE_Internal_Id;
        }

        #region func to get the tax
        public List<TaxEntityRecord> GetTaxEntities(string visible_tax_entities)
        {
            var records = new List<TaxEntityRecord> { };

            if (string.IsNullOrWhiteSpace(visible_tax_entities))
            {
                return records;
            }

            /*
             SELECT id, ddl_name FROM tax_entity WHERE NetSuite_TE_Internal_Id > 0 AND FIND_IN_SET(id, @te_list) ORDER BY ddl_name;
             SELECT id, ddl_name FROM business_unit WHERE tax_entity_id = 2 AND NetSuite_BU_Internal_Id > 0 AND FIND_IN_SET(id, @vbu_list) ORDER BY ddl_name;
             */
            var _dt = Toolbox.doSQL_dt(@"Select id ,ddl_name name from tax_entity  where find_in_set(id, @v0) order by ddl_name ", new object[] { visible_tax_entities });
            if (_dt != null && _dt.Rows != null && _dt.Rows.Count > 0)
            {
                foreach (DataRow row in _dt.Rows)
                {
                    var record = new TaxEntityRecord()
                    {
                        id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0,
                        name = row["name"] != DBNull.Value ? row["name"].ToString() : ""
                    };

                    records.Add(record);
                }
            }

            return records;
        }
        #endregion

        #region func to get the business units
        public BusinessUnitData GetBusinessUnits(int tax_entity_id, bool is_US_boardmember, bool is_CAN_boardmember, bool is_backoffice, bool is_corporate, int member_id)
        {
            var data = new BusinessUnitData
            {
                records = new List<BusinessUnitRecord> { },
                waitingRollover = false
            };

            if (tax_entity_id == 0)
            {
                return data;
            }

            data = this.fill_business_units(tax_entity_id, is_US_boardmember, is_CAN_boardmember, is_backoffice, is_corporate, member_id);
            return data;
        }

        private BusinessUnitData fill_business_units(int tax_entity_id, bool is_US_boardmember, bool is_CAN_boardmember, bool is_backoffice, bool is_corporate, int member_id)
        {
            var data = new BusinessUnitData
            {
                records = new List<BusinessUnitRecord> { },
                waitingRollover = false
            };

            var visible_reporting_users = Toolbox.doSQL_string(@"Call get_visible_reporting_users_group_concat(@v0)", new object[] { member_id });

            var dt_business_units = new DataTable();

            if (is_US_boardmember || is_CAN_boardmember)
            {
                dt_business_units = Toolbox.doSQL_dt(
                    @"Select id,ddl_name name from business_unit  where tax_entity_id = @v0 and  (active = 'T')",
                    new object[] { tax_entity_id, visible_reporting_users });
            }
            else if (is_backoffice || is_corporate)
            {
                dt_business_units = Toolbox.doSQL_dt(
                    @"Select id,ddl_name name from business_unit  where tax_entity_id = @v0  and ( active = 'T') ",
                    new object[] { tax_entity_id });
            }
            else
            {
                dt_business_units = Toolbox.doSQL_dt(
                    @"Select distinct id,ddl_name name from business_unit inner join member m on m.business_unit_id = business_unit.id and find_in_set(m.member_id,@v1) and m.member_status='Active' where tax_entity_id = @v0 and (active = 'T' )",
                    new object[] { tax_entity_id, visible_reporting_users });
            }

            var waiting_rollover_i = 0;
            foreach (DataRow dr in dt_business_units.Rows)
            {
                var bu_id = Convert.ToInt32(dr["id"]);
                var waiting_rollover = false; //NeBusinessUnit.IsWaitingRollover(Convert.ToInt32(tax_entity_id));
                var bu_name = dr["name"].ToString();
                if (waiting_rollover)
                {
                    bu_name = bu_name + "*";
                    waiting_rollover_i++;
                }

                var record = new BusinessUnitRecord()
                {
                    id = bu_id,
                    text = bu_name
                };

                data.records.Add(record);
            }

            data.waitingRollover = waiting_rollover_i > 0;
            return data;
        }

        #endregion

        #region fiscal Dates
        public List<FiscalPeriodRecord> GetFiscalPeriodRecords(int tax_entity_id)
        {
            return this.populate_dates(tax_entity_id);
        }

        private List<FiscalPeriodRecord> populate_dates(int tax_entity_id)
        {
            var records = new List<FiscalPeriodRecord> { };

            if (tax_entity_id == 0)
            {
                return records;
            }

            var DATE = DateTime.Now;
            #region Fiscal End Date
            // This is for getting the current year's options
            var base_c = new NeBusinessUnit(Toolbox.doSQL_string("Select id from business_unit where tax_entity_id = @v0 limit 1", new object[] { tax_entity_id }));
            for (var i = 12; i >= 1; i--)
            {
                var working_month = tax_entity_id != 99 ? NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i] : NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i];
                var working_year = base_c.fiscal_yearstart_month == 1 || working_month < base_c.fiscal_yearstart_month
                                            ? DATE.Year
                                                : DATE.Year - 1;
                var month_name = new DateTime(working_year, working_month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
                var month_end = DateTime.DaysInMonth(working_year, working_month);

                var record = new FiscalPeriodRecord
                {
                    text = string.Format("{0} {1}, {2}", month_name, month_end, working_year),
                    value = string.Format("{0}_{1}", working_year, working_month)
                };

                records.Add(record);
            }

            var recordseprator = new FiscalPeriodRecord
            {
                text = "-----------------------------",
                value = "-----------------------------"
            };

            records.Add(recordseprator);

            // This is for getting the previous year's options
            for (var i = 12; i >= 1; i--)
            {
                var working_month = NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i];
                var working_year = base_c.fiscal_yearstart_month == 1 || working_month < base_c.fiscal_yearstart_month
                                            ? DATE.Year - 1
                                                : DATE.Year - 2;
                var month_name = new DateTime(working_year, working_month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
                var month_end = DateTime.DaysInMonth(working_year, working_month);

                var record = new FiscalPeriodRecord
                {
                    text = string.Format("{0} {1}, {2}", month_name, month_end, working_year),
                    value = string.Format("{0}_{1}", working_year, working_month)
                };

                records.Add(record);
            }

            records.Add(recordseprator);

            for (var i = 12; i >= 1; i--)
            {
                var working_month = NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i];
                var working_year = base_c.fiscal_yearstart_month == 1 || working_month < base_c.fiscal_yearstart_month
                    ? DATE.Year - 2
                    : DATE.Year - 3;
                var month_name =
                    new DateTime(working_year, working_month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
                var month_end = DateTime.DaysInMonth(working_year, working_month);

                var record = new FiscalPeriodRecord
                {
                    text = string.Format("{0} {1}, {2}", month_name, month_end, working_year),
                    value = string.Format("{0}_{1}", working_year, working_month)
                };

                records.Add(record);
            }

            return records;

            #endregion Fiscal End Date
        }
        #endregion

        #region Initial

        public InitialData GetInitialData(string visible_tax_entities,
            int tax_entity_id, int defaultBusinessUnit, bool is_US_boardmember, bool is_CAN_boardmember, bool is_backoffice, bool is_corporate, int member_id)
        {
            var data = new InitialData
            {
                taxEntities = new List<TaxEntityRecord> { },
                defaultTaxEntity = 0,
                businessUnits = new List<BusinessUnitRecord> { },
                defaultSelectedBusinessUnits = new List<int> { },
                fiscalPeriods = new List<FiscalPeriodRecord> { },
                defaultFiscalPeriod = ""
            };

            data.taxEntities = this.GetTaxEntities(visible_tax_entities);
            data.defaultTaxEntity = tax_entity_id;

            var bu = this.GetBusinessUnits(tax_entity_id, is_US_boardmember, is_CAN_boardmember, is_backoffice, is_corporate, member_id);
            data.businessUnits = bu.records;
            data.defaultBusinessUnit = defaultBusinessUnit;
            data.defaultSelectedBusinessUnits.Add(defaultBusinessUnit);

            data.fiscalPeriods = this.GetFiscalPeriodRecords(tax_entity_id);
            data.defaultFiscalPeriod = data.fiscalPeriods.Count > 0 ? data.fiscalPeriods[0].value : "";
            return data;
        }

        #endregion
    }

    public class TaxEntityRecord
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class BusinessUnitRecord
    {
        public int id { get; set; }
        public string text { get; set; }
    }

    public class BusinessUnitData
    {
        public List<BusinessUnitRecord> records { get; set; }
        public bool waitingRollover { get; set; }
    }

    public class FiscalPeriodRecord
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class InitialData
    {
        public List<TaxEntityRecord> taxEntities { get; set; }
        public int defaultTaxEntity { get; set; }

        public List<BusinessUnitRecord> businessUnits { get; set; }
        public List<int> defaultSelectedBusinessUnits { get; set; }
        public int defaultBusinessUnit { get; set; }

        public List<FiscalPeriodRecord> fiscalPeriods { get; set; }
        public string defaultFiscalPeriod { get; set; }
    }
    public class TaskData
    {
        public string SubSidiaryId { get; set; }
        public string Name { get; set; }
        public int TaskID { get; set; }
    }

    #endregion

    #region NetSuite
    public class OAuthUtility
    {
        /// <summary>
        /// Provides a predefined set of algorithms that are supported officially by the protocol
        /// </summary>
        public enum SignatureTypes
        {
            HMACSHA256,
            PLAINTEXT,
            RSASHA1
        }

        #region Data memebers

        /// <summary>
        /// Provides an internal structure to sort the query parameter
        /// </summary>
        public class QueryParameter
        {
            public QueryParameter(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; } = null;

            public string Value { get; } = null;
        }

        /// <summary>
        /// Comparer class used to perform the sorting of the query parameters
        /// </summary>
        public class QueryParameterComparer : IComparer<QueryParameter>
        {
            #region IComparer<QueryParameter> Members

            public int Compare(QueryParameter x, QueryParameter y)
            {
                if (x == null) throw new ArgumentNullException(nameof(x));
                if (y == null) throw new ArgumentNullException(nameof(y));

                return x.Name == y.Name ?
                    string.CompareOrdinal(x.Value, y.Value) :
                    string.CompareOrdinal(x.Name, y.Name);
            }

            #endregion
        }

        protected const string OAuthVersion = "1.0";
        protected const string OAuthParameterPrefix = "oauth_";

        //
        // List of know and used oauth parameters' names
        //        
        protected const string OAuthConsumerKeyKey = "oauth_consumer_key";
        protected const string OAuthCallbackKey = "oauth_callback";
        protected const string OAuthVersionKey = "oauth_version";
        protected const string OAuthSignatureMethodKey = "oauth_signature_method";
        protected const string OAuthSignatureKey = "oauth_signature";
        protected const string OAuthTimestampKey = "oauth_timestamp";
        protected const string OAuthNonceKey = "oauth_nonce";
        protected const string OAuthTokenKey = "oauth_token";
        protected const string OAuthTokenSecretKey = "oauth_token_secret";
        protected const string OAuthVerifier = "oauth_verifier";    // OAuth 1.0a

        protected const string HMACSHA256SignatureType = "HMAC-SHA256";
        protected const string PlainTextSignatureType = "PLAINTEXT";
        protected const string RSASHA1SignatureType = "RSA-SHA1";

        protected Random random = new Random();

        protected string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        #endregion

        /// <summary>
        /// Generates an outh header
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="realm"></param>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="token"></param>
        /// <param name="tokenSecret"></param>
        /// <returns></returns>
        public string GenerateOauthHeader(
            string uri,
            string realm,
            string consumerKey,
            string consumerSecret,
            string token,
            string tokenSecret,
            string verb
        )
        {
            var timestamp = EncryptionUtility.ComputeTimestamp().ToString();
            var nonce = EncryptionUtility.ComputeNonce();

            var signature = GenerateSignature(
                new Uri(uri),
                consumerKey,
                consumerSecret,
                token,
                tokenSecret,
                verb,
                timestamp,
                nonce,
                out _,
                out _);

            var header = "OAuth ";
            header += "realm=\"" + HttpUtility.UrlEncode(realm) + "\", ";
            header += $"{OAuthConsumerKeyKey}=\"{HttpUtility.UrlEncode(consumerKey)}\", ";
            header += $"{OAuthTokenKey}=\"{HttpUtility.UrlEncode(token)}\", ";
            header += $"{OAuthNonceKey}=\"{HttpUtility.UrlEncode(nonce)}\", ";
            header += $"{OAuthTimestampKey}=\"{HttpUtility.UrlEncode(timestamp)}\", ";
            header += $"{OAuthSignatureMethodKey}=\"{HMACSHA256SignatureType}\", ";
            header += $"{OAuthVersionKey}=\"1.0\", ";
            header += $"{OAuthSignatureKey}=\"{HttpUtility.UrlEncode(signature)}\"";

            return header;
        }


        /// <summary>
        /// Generate the signature base that is used to produce the signature
        /// </summary>
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>        
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="signatureType">The signature type. To use the default values use <see cref="OAuthUtility.SignatureTypes">OAuthBase.SignatureTypes</see>.</param>
        /// <returns>The signature base</returns>
        public string GenerateSignatureBase(
            Uri url, string consumerKey, string token, string tokenSecret,
            string httpMethod, string timeStamp, string nonce, string signatureType,
            string verifier,
            out string normalizedUrl, out string normalizedRequestParameters)
        {
            if (token == null)
            {
                token = string.Empty;
            }

            if (tokenSecret == null)
            {
                tokenSecret = string.Empty;
            }

            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException(nameof(consumerKey));
            }

            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            if (string.IsNullOrEmpty(signatureType))
            {
                throw new ArgumentNullException(nameof(signatureType));
            }

            normalizedUrl = null;
            normalizedRequestParameters = null;

            List<QueryParameter> parameters = GetQueryParameters(url.Query);
            parameters.Add(new QueryParameter(OAuthVersionKey, OAuthVersion));
            parameters.Add(new QueryParameter(OAuthNonceKey, nonce));
            parameters.Add(new QueryParameter(OAuthTimestampKey, timeStamp));
            parameters.Add(new QueryParameter(OAuthSignatureMethodKey, signatureType));
            parameters.Add(new QueryParameter(OAuthConsumerKeyKey, consumerKey));
            if (!string.IsNullOrEmpty(verifier))
                parameters.Add(new QueryParameter(OAuthVerifier, verifier));

            if (!string.IsNullOrEmpty(token))
            {
                parameters.Add(new QueryParameter(OAuthTokenKey, token));
            }

            parameters.Sort(new QueryParameterComparer());

            normalizedUrl = $"{url.Scheme}://{url.Host}";
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }
            normalizedUrl += url.AbsolutePath;
            normalizedRequestParameters = NormalizeRequestParameters(parameters);

            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{0}&", httpMethod.ToUpper());
            signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl));
            signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));

            return signatureBase.ToString();
        }

        /// <summary>
        /// Generate the signature value based on the given signature base and hash algorithm
        /// </summary>
        /// <param name="signatureBase">The signature based as produced by the GenerateSignatureBase method or by any other means</param>
        /// <param name="hash">The hash algorithm used to perform the hashing. If the hashing algorithm requires initialization or a key it should be set prior to calling this method</param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
        {
            return ComputeHash(hash, signatureBase);
        }

        /// <summary>
        /// Generates a signature using the HMAC-SHA1 algorithm
        /// </summary>		
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer seceret</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="normalizedRequestParameters"></param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignature(
            Uri url,
            string consumerKey,
            string consumerSecret,
            string token,
            string tokenSecret,
            string httpMethod,
            string timeStamp,
            string nonce,
            out string normalizedUrl,
            out string normalizedRequestParameters)
        {
            return GenerateSignature(
                        url, consumerKey, consumerSecret, token, tokenSecret,
                        httpMethod, timeStamp, nonce, SignatureTypes.HMACSHA256, null,
                        out normalizedUrl, out normalizedRequestParameters);
        }

        public string GenerateSignature(
            Uri url, string consumerKey, string consumerSecret, string token, string tokenSecret,
            string httpMethod, string timeStamp, string nonce,
            string verifier,
            out string normalizedUrl, out string normalizedRequestParameters)
        {
            return GenerateSignature(
                        url, consumerKey, consumerSecret, token, tokenSecret,
                        httpMethod, timeStamp, nonce, SignatureTypes.HMACSHA256, verifier,
                        out normalizedUrl, out normalizedRequestParameters);
        }

        /// <summary>
        /// Generates a signature using the specified signatureType 
        /// </summary>		
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer secret</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="nonce"></param>
        /// <param name="signatureType">The type of signature to use</param>
        /// <param name="timeStamp"></param>
        /// <param name="verifier"></param>
        /// <param name="normalizedUrl"></param>
        /// <param name="normalizedRequestParameters"></param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignature(
            Uri url,
            string consumerKey,
            string consumerSecret,
            string token,
            string tokenSecret,
            string httpMethod,
            string timeStamp,
            string nonce,
            SignatureTypes signatureType,
            string verifier,
            out string normalizedUrl,
            out string normalizedRequestParameters)
        {
            normalizedUrl = null;
            normalizedRequestParameters = null;

            switch (signatureType)
            {
                case SignatureTypes.PLAINTEXT:
                    return HttpUtility.UrlEncode($"{consumerSecret}&{tokenSecret}");
                case SignatureTypes.HMACSHA256:
                    string signatureBase =
                        GenerateSignatureBase(
                            url, consumerKey, token, tokenSecret,
                            httpMethod, timeStamp, nonce, HMACSHA256SignatureType, verifier,
                            out normalizedUrl, out normalizedRequestParameters);

                    HMACSHA256 hmacsha256 = new HMACSHA256();
                    hmacsha256.Key = Encoding.ASCII.GetBytes(
                        $"{UrlEncode(consumerSecret)}&{(string.IsNullOrEmpty(tokenSecret) ? "" : UrlEncode(tokenSecret))}");

                    return GenerateSignatureUsingHash(signatureBase, hmacsha256);
                case SignatureTypes.RSASHA1:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Unknown signature type", nameof(signatureType));
            }
        }


        #region Helpers

        /// <summary>
        /// Helper function to compute a hash value
        /// </summary>
        /// <param name="hashAlgorithm">The hashing algoirhtm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param>
        /// <param name="data">The data to hash</param>
        /// <returns>a Base64 string of the hash value</returns>
        private string ComputeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(hashAlgorithm));
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            byte[] dataBuffer = Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Internal function to cut out all non oauth query string parameters (all parameters not begining with "oauth_")
        /// </summary>
        /// <param name="parameters">The query string part of the Url</param>
        /// <returns>A list of QueryParameter each containing the parameter name and value</returns>
        private List<QueryParameter> GetQueryParameters(string parameters)
        {
            if (parameters.StartsWith("?"))
            {
                parameters = parameters.Remove(0, 1);
            }

            List<QueryParameter> result = new List<QueryParameter>();

            if (!string.IsNullOrEmpty(parameters))
            {
                string[] p = parameters.Split('&');
                foreach (string s in p)
                {
                    if (!string.IsNullOrEmpty(s) && !s.StartsWith(OAuthParameterPrefix))
                    {
                        if (s.IndexOf('=') > -1)
                        {
                            string[] temp = s.Split('=');
                            result.Add(new QueryParameter(temp[0], temp[1]));
                        }
                        else
                        {
                            result.Add(new QueryParameter(s, string.Empty));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        private string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + $"{(int)symbol:X2}");
                }
            }

            return result.ToString();
        }

        private string UrlEncode(string value, Encoding encode)
        {
            StringBuilder result = new StringBuilder();
            byte[] data = encode.GetBytes(value);
            int len = data.Length;

            for (int i = 0; i < len; i++)
            {
                int c = data[i];
                if (c < 0x80 && unreservedChars.IndexOf((char)c) != -1)
                {
                    result.Append((char)c);
                }
                else
                {
                    result.Append('%' + $"{(int)data[i]:X2}");
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Normalizes the request parameters according to the spec
        /// </summary>
        /// <param name="parameters">The list of parameters already sorted</param>
        /// <returns>a string representing the normalized parameters</returns>
        private string NormalizeRequestParameters(IList<QueryParameter> parameters)
        {
            StringBuilder sb = new StringBuilder();
            QueryParameter p = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                p = parameters[i];
                sb.AppendFormat("{0}={1}", p.Name, p.Value);

                if (i < parameters.Count - 1)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        #endregion
    }

    public static class EncryptionUtility
    {
        /// <summary>
        /// Compute a nonce value
        /// </summary>
        /// <returns></returns>
        public static string ComputeNonce()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] data = new byte[20];
            rng.GetBytes(data);
            var value = Math.Abs(BitConverter.ToInt32(data, 0));
            return value.ToString();
        }

        /// <summary>
        /// General unix style date computation used in encryption
        /// </summary>
        /// <returns></returns>
        public static long ComputeTimestamp()
        {
            return ((long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }
    }
    #endregion
}
