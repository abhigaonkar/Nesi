using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using DevExpress.Web;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data.Common;
using System.Web.Configuration;
using System.Web.SessionState;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Security;
using JetBrains.Annotations;
using Microsoft.Ajax.Utilities;
using MySql.Data.Entity;
using NESI.Common.Exceptions;
using log4net;
using System.Reflection;
using NESI.Common.Models;

namespace nesi.core
{

    public class Toolbox : IDisposable
    {
        public void Dispose()
        {
        }
        #region Variable Declaration
        private NeMember _current_user;
        private string _connection_string = str_connection_string;
        public static string str_connection_string = ConfigurationManager.ConnectionStrings["MySQLdotnet"].ConnectionString;
        public NeMember current_user
        {
            get { return _current_user; }
            set { _current_user = value; }
        }
        private NeMember _page_author;
        // Obs
        public NeMember page_author
        {
            get { return _page_author; }
            set { _page_author = value; }
        }
        private HttpRequest _request;
        private HttpResponse _response;
        private HttpSessionState _session;
        private string _hostname;
        #endregion

        public Toolbox()
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    _request = HttpContext.Current.Request;
                    _response = HttpContext.Current.Response;
                    _session = HttpContext.Current.Session;
                    _hostname = HttpContext.Current.Request.Url.Host;
                }
            }
            catch
            {
            }
        }
        public bool leave_open_connection { get; set; }
        public bool connection_is_open { get; set; }
        private MySqlConnection my_connection { get; set; }
        public static string conn_string()
        {
            return str_connection_string;
        }
        public static Regex regex_only_numbers()
        {
            return new Regex("[^0-9]");
        }
        public string connection_string
        {
            get { return _connection_string; }
            set { _connection_string = value; }
        }
        public static string MySQLNow_long()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string MySQLNow_short()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        public static string Bool_to_UserFriendlyResponse(bool response)
        {
            return response ? "Yes" : "No";
        }
        public static void RedirectToN2(HttpResponse res, HttpRequest req, int pageId, string pageName, string url)
        {
            var type = doSQL_int("select menu_type from page where page_id = @v0", new object[] { pageId });

            if (type == 2)
            {
                var em = new NeEMail();
                em.To = app_setting("debug_email");
                em.Subject = "Old ASPX page loaded " + pageName;
                em.From = "debug@" + Toolbox.app_setting("DomainForEmail");
                em.Body = "from: " + req.UrlReferrer + " <br/>PageId: " + pageId + " <br/>URL: " + req.RawUrl;
                em.Send();

                res.Redirect("#/" + url);
            }
        }

        public static void RedirectToN2(HttpResponse res, HttpRequest req, int pageId, string pageName = "")
        {

            var url = doSQL_string("select menu_router from page where page_id = @v0", new object[] { pageId });
            RedirectToN2(res, req, pageId, pageName, url);

        }
        public static void IncrementCountBag(string name)
        {
            if (HttpContext.Current != null)
            {
                var ap = HttpContext.Current.Application;
                if (ap["count_bag"] == null)
                {
                    ap["count_bag"] = new Dictionary<string, int>();
                }
                var cb = (Dictionary<string, int>)ap["count_bag"];
                if (!cb.ContainsKey(name))
                {
                    cb.Add(name, 0);
                }
                cb[name]++;
            }
        }
        public static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            var daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
        public static long session_size(HttpSessionState _s)
        {
            long total_session_bytes = 0;
            var b = new BinaryFormatter();
            foreach (string key in _s)
            {
                var obj = _s[key];
                if (obj != null && obj.GetType().IsSerializable)
                {
                    using (var m = new MemoryStream())
                    {
                        b.Serialize(m, obj);
                        total_session_bytes += m.Length;
                    }
                }
            }
            return total_session_bytes;
        }
		public static string session_contents(System.Web.SessionState.HttpSessionState session)
			{
			var session_contents = "";
			foreach (string key in session.Keys)
				{
				session_contents += $"{key}: {session[key]}<br/>";
				}
			return session_contents;
			}

		public static string HumanReadableSize(long _len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (_len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                _len = _len / 1024;
            }
            return string.Format("{0:0.##}{1}", _len, sizes[order]);
        }
        public static long ObjectMemoryFootprint(object _o)
        {
            long size = 0;
            if (_o != null)
            {
                using (Stream s = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(s, _o);
                    size = s.Length;
                }
            }
            return size;
        }
        public static int get_whos_on_count(MySqlConnection _conn)
        {
            return doSQL_int(_conn, @"SELECT COUNT(id) FROM (SELECT
a.member_id id
FROM
log_page_asax a
INNER JOIN member b ON a.member_id = b.Member_ID
INNER JOIN business_unit c ON b.business_unit_id = c.id
WHERE
a.dt > NOW()-INTERVAL 4 HOUR and find_in_set(c.id, @v0)
GROUP BY
a.member_id
HAVING TIMEDIFF(NOW(),MAX(a.dt))<3600
ORDER BY TIMEDIFF(NOW(),MAX(a.dt))) users", new object[] { new Current_User().visible_business_units });
        }

        public static DataTable get_whos_on()
        {

            return doSQL_dt(@"SELECT
fun_time(max(a.dt)) last_contact,
Concat(b.member_fullname,' (',c.Name,')') _name,
a.member_id id
FROM
log_page_asax a
INNER JOIN member b ON a.member_id = b.Member_ID
INNER JOIN business_unit c ON b.business_unit_id = c.id
WHERE
a.dt > now()-interval 4 hour and find_in_set(c.id, @v0)
GROUP BY
a.member_id
having TIMEDIFF(now(),Max(a.dt))<3600
order by TIMEDIFF(now(),Max(a.dt))", new object[] { new Current_User().visible_business_units });


        }
        public static string string_length_check(string s, int req_length, string _default)
        {
            var returned = "";
            if (s == null)
            {
                returned = _default;
            }
            else if (s.Length <= req_length)
            {
                returned = s;
            }
            else
            {
                returned = s.Substring(0, req_length);
            }
            return returned;
        }
        public static void do_set_plain_header(HttpResponse _r)
        {
            _r.Clear();
            _r.ContentType = "text/plain";
        }
        public static void do_set_XML_header(HttpResponse _r)
        {
            _r.Clear();
            _r.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            _r.Cache.SetValidUntilExpires(false);
            _r.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            _r.Cache.SetCacheability(HttpCacheability.NoCache);
            _r.Cache.SetNoStore();
            _r.ContentType = "text/xml";
            _r.Write("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
        }

        public static string MySQL_shortdt(DateTime _dt)
        {
            if (_dt == null || _dt.Year < 1900)
            {
                return "";
            }
            else
            {
                return _dt.ToString("yyyy-MM-dd");
            }
        }
        public static string MySQL_longdt(DateTime _dt)
        {
            if (_dt == null || _dt.Year < 1900)
            {
                return "";
            }
            else
            {
                return _dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public static string MySQL_longdt(DateTime? _dt)
        {
            if (_dt == null)
            {
                return "";
            }
            else
            {
                var dt = Convert.ToDateTime(_dt);
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public static bool is_valid_date(DateTime? dt)
        {
            if (dt == null)
            {
                return false;
            }
            else
            {
                return is_valid_date((DateTime)dt);
            }
        }
        public static bool is_valid_date(DateTime dt)
        {
            return dt.Year > 2000;
        }
        public static string Surround(object s, char c)
        {
            return c + s.ToString() + c;
        }
        public static bool CheckEmail(string e)
        {
            if (string.IsNullOrEmpty(e)) return false;
            return Regex.IsMatch(e.Trim(), "^([0-9a-zA-Z']([-.\\w']*[0-9a-zA-Z'])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
        }
        public static string remove_duplicate_spaces(string s)
        {
            var i = 0;
            while (s.Contains("  "))
            {
                if (i > 10000)
                {
                    throw new Exception("Possible endless loop, aborting.");
                }
                s = s.Replace("  ", " ");
                i++;
            }
            return s;
        }
        public static string AddSlashes(object st)
        {
            if (st == null)
            {
                st = "";
            }
            var input = st.ToString();
            if (input.Contains("\\") && input.EndsWith("\\"))
            {
                input = input.TrimEnd('\\');
            }
            var i = 0;
            while (input.Contains("\\") && Regex.Match(input, @"(\\)([\000\010\011\012\015\032\042\047\134\140])").Success)
            {
                if (i > 10000)
                {
                    throw new Exception("Possible endless loop, aborting.");
                }
                input = StripSlashes(st);
                i++;
            }
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            var Result = input;

            try
            {
                Result = Regex.Replace(input, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
            }
            catch (Exception Ex)
            {

                do_errorLog(Ex, st.ToString());
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }
        public static void exception_test(bool is_page)
        {
            if (is_page)
            {
                using (var p = new Page())
                {
                    HttpContext.Current.Handler = p;
                    throw new Exception("Something happened - Page");
                }
            }
            else
            {
                HttpContext.Current.Handler = null;
                throw new Exception("Something happened - null");
            }
        }
        public static string app_setting(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key];
            }
            else
            {
                return "";
            }
        }

        public static string GetRequiredAppSetting([NotNull] string settingKey)
        {
            if (string.IsNullOrEmpty(settingKey)) throw new ArgumentNullException(nameof(settingKey));
            return ConfigurationManager.AppSettings[settingKey] ?? throw new InvalidOperationException($"Required application setting '{settingKey}' not found");
        }



        public static string StripSlashes(object st)
        {
            var input = st.ToString();
            if (input.Contains("\\") && input.EndsWith("\\"))
            {
                input = input.TrimEnd('\\');
            }
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            var Result = input;

            try
            {
                Result = Regex.Replace(input, @"(\\)([\000\010\011\012\015\032\042\047\134\140])", "$2");
            }
            catch (Exception Ex)
            {
                do_errorLog(Ex, st.ToString());
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }
            return Result;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
        public static string GetSubDomain(Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                var host = url.Host;
                if (host.Split('.').Length >= 2)
                {
                    var ret = host.Split('.')[0];
                    if (ret == "www")
                        ret = host.Split('.')[1];
                    return ret;
                }
            }
            return "";
        }
        public static string MySQL_safe(string st)
        {
            return st.Replace("\"", "\\\"").Replace("'", "\\'");
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
        public static decimal ReturnZeroIfNull_decimal(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(n);
            }
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
        public static double ReturnZeroIfNull_double(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(n);
            }
        }
        /// <summary>
        /// Returns a comma delimited string from an ASPxListbox
        /// </summary>
        /// <param name="lb"></param>
        /// <returns></returns>
        public static string CommaDelimit(ASPxListBox lb)
        {
            return string.Join(", ", lb.Items.Cast<ListEditItem>().Where(i => i.Selected).OrderBy(i => i.Value).Select(i => i.Value)).Replace(" ", "");
        }
        /// <summary>
        /// Returns a comma delimited string from a Listbox
        /// </summary>
        /// <param name="lb"></param>
        /// <returns></returns>
        public static string CommaDelimit(ListBox lb)
        {
            return string.Join(", ", lb.Items.Cast<ListItem>().Where(i => i.Selected).OrderBy(i => i.Value).Select(i => i.Value)).Replace(" ", "");
        }
        public static void load_ctrl(Page p, string path, Control destination)
        {
            var ctrl = p.LoadControl(path);
            destination.Controls.Clear();
            destination.Controls.Add(ctrl);
        }
        public static int ReturnZeroIfNull_int(object n)
        {
            return ReturnDefaultIfNull_int(n, 0);
        }

        public static decimal ReturnZeroIfNull_decimal(object n, decimal defaultValue)
        {
            if (n == DBNull.Value || n == null || string.IsNullOrWhiteSpace(n.ToString()))
            {
                return defaultValue;
            }
            else
            {
                return Convert.ToDecimal(n);
            }
        }
        public static int ReturnDefaultIfNull_int(object n, int defaultValue)
        {
            if (n == DBNull.Value || n == null || string.IsNullOrWhiteSpace(n.ToString()))
            {
                return defaultValue;
            }
            else
            {
                return Convert.ToInt32(n);
            }
        }

        public static string ReturnBlankIfNull_string(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "")
            {
                return "";
            }
            else
            {
                return n.ToString();
            }
        }
        public static DateTime? ReturnNullDateTime(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "" || Convert.ToDateTime(n).Year < 1900)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(n);
            }
        }
        public static DateTime ReturnBlankDateTimeIfNull(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "")
            {
                return new DateTime();
            }
            else
            {
                return Convert.ToDateTime(n);
            }
        }
        public static DateTime? ReturnNullableDateTimeIfNull(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "")
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(n);
            }
        }
        public static class StyleReferenceManager
        {
            private static Dictionary<Type, string> defaultCssStyleInfo = new Dictionary<Type, string>();

            static StyleReferenceManager()
            {
                defaultCssStyleInfo.Add(typeof(ASPxWebControl), "DevExpress.Web.Css.Default.css");
                defaultCssStyleInfo.Add(typeof(ASPxEdit), "DevExpress.Web.Css.Default.css");
                defaultCssStyleInfo.Add(typeof(ASPxGridView), "DevExpress.Web.Css.default.css");
            }

            public static void AddStyleLinksToHead(Page page)
            {
                foreach (var pair in defaultCssStyleInfo)
                {
                    var url = page.ClientScript.GetWebResourceUrl(pair.Key, pair.Value);
                    var link = new WebControl(HtmlTextWriterTag.Link);
                    link.Attributes["type"] = "text/css";
                    link.Attributes["rel"] = "stylesheet";
                    link.Attributes["href"] = url;
                    page.Header.Controls.Add(link);
                }
            }
        }
        #region catch_error
        /// <summary>
        /// <para>Catches and emails error to the pages author</para>
        /// <example>
        /// <para>Example</para>
        /// <para>----------------------------</para>
        /// <para>Toolbox _tools	= new Toolbox();</para>
        /// <para>_tools.page_author	= new NeMember(711);</para>
        /// <para>try</para>
        ///	<para>{</para>
        ///	<para>...</para>
        ///	<para>}</para>
        /// <para>catch (Exception ee)</para>
        ///	<para>{</para>
        ///	<para>_tools.catch_error(ee);</para>
        ///	<para>}</para>
        /// </example>
        /// </summary>
        /// <param name="_error"></param>
        public void catch_error(Exception _error)
        {
            if (_error != null)
            {
                do_errorLog(_error);
                // catch_error(_error, 711);
                //catch_error(_error, 1359);

            }
        }

        public static void do_catch_error(Exception _error, int admin_member_id = OpsStaticEmployees.MattHyde)
        {
            try
            {
                if (_error != null && HttpContext.Current != null)
                {
                    do_errorLog(_error);
                    var _request = HttpContext.Current.Request;
                    var _session = HttpContext.Current.Session;
                    var _page_author = new NeMember(admin_member_id);
                    var _current_user = _session != null && _session["session"] != null
                        ? new NeMember(HttpContext.Current.User.Identity.Name, true)
                        : new NeMember(admin_member_id);
                    if (_current_user != null)
                    {
                        var s = new StackTrace(_error, true);
                        var _message = new NeEMail();
                        _message.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
                        _message.isHTML = true;
                        _message.To = _page_author.NEEmail;
                        if (_request != null)
                        {
                            _message.Body = string.Format("{0}\n----------------------\nStack Trace:{1}\nOrigin: {2}\nCurrent User: {3}\nSource: {4}\nTargetSite: {5}\n", _error.Message, _error.StackTrace, _request.RawUrl, _current_user.FullName, _error.Source, _error.TargetSite);
                        }
                        else
                        {
                            _message.Body = string.Format("{0}\n----------------------\nStack Trace:{1}\nCurrent User: {2}\nSource: {3}\nTargetSite: {4}\n", _error.Message, _error.StackTrace, _current_user.FullName, _error.Source, _error.TargetSite);
                        }
                        if (s != null && s.FrameCount > 0)
                        {
                            foreach (var frame in s.GetFrames())
                            {
                                var method = frame.GetMethod();
                                _message.Body += string.Format("==\nMethod Name: {0}\nMethod Module: {1}\nFrame filename: {2}\nFrame Line Number: {3}\n", method.Name, method.Module, frame.GetFileName(), frame.GetFileLineNumber());
                            }
                        }
                        _message.Body = _message.Body.Replace("\n", "<br/>\n");
                        if (_request != null)
                        {
                            _message.Subject = string.Format(Toolbox.app_setting("Domain") +" script error ({0})", _request.Path);
                        }
                        else
                        {
                            _message.Subject = Toolbox.app_setting("Domain") + " script error - Null Request... probably backgrounded process";
                        }
                        _message.Send();

                    }
                }
            }
            catch
            {
                // Don't endlessly throw errors.
            }
        }
        public void catch_error(Exception _error, int admin_member_id)
        {
            try
            {
                if (_error != null)
                {

                    do_errorLog(_error);
                    _page_author = new NeMember(admin_member_id);
                    if (_current_user == null)
                    {
                        if (_session != null && _session["session"] != null)
                        {
                            current_user = new NeMember(_session["session"].ToString());
                        }
                        else
                        {
                            current_user = _page_author;
                        }
                    }
                    if (_current_user != null)
                    {
                        try
                        {
                            var s = new StackTrace(_error, true);
                            var _message = new NeEMail();
                            _message.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
                            _message.isHTML = true;
                            _message.To = _page_author.NEEmail;
                            if (_request != null)
                            {
                                _message.Body = string.Format("{0}\n----------------------\nStack Trace:{1}\nOrigin: {2}\nCurrent User: {3}\nSource: {4}\nTargetSite: {5}\n", _error.Message, _error.StackTrace, _request.RawUrl, _current_user.FullName, _error.Source, _error.TargetSite);
                            }
                            else
                            {
                                _message.Body = string.Format("{0}\n----------------------\nStack Trace:{1}\nCurrent User: {2}\nSource: {3}\nTargetSite: {4}\n", _error.Message, _error.StackTrace, _current_user.FullName, _error.Source, _error.TargetSite);
                            }
                            if (s != null && s.FrameCount > 0)
                            {
                                foreach (var frame in s.GetFrames())
                                {
                                    var method = frame.GetMethod();
                                    _message.Body += string.Format("==\nMethod Name: {0}\nMethod Module: {1}\nFrame filename: {2}\nFrame Line Number: {3}\n", method.Name, method.Module, frame.GetFileName(), frame.GetFileLineNumber());
                                }
                            }
                            _message.Body = _message.Body.Replace("\n", "<br/>\n");
                            if (_request != null)
                            {
                                _message.Subject = string.Format(Toolbox.app_setting("Domain") + " script error ({0})", _request.Path);
                            }
                            else
                            {
                                _message.Subject = Toolbox.app_setting("Domain") + " script error - Null Request... probably backgrounded process";
                            }
                            //_message.Send();
                        }
                        catch (Exception ee)
                        {
                            //catch_error(ee);
                        }

                    }
                }
            }
            catch
            {
            }
        }
        #endregion catch_error
        #region dont_cache_page
        /// <summary>
        /// Disables caching of this page
        /// </summary>
        public void dont_cache_page()
        {
            _response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            _response.Cache.SetValidUntilExpires(false);
            _response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            _response.Cache.SetCacheability(HttpCacheability.NoCache);
            _response.Cache.SetNoStore();
        }
        public static void do_dont_cache_page(HttpResponse Response)
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }
        #endregion
        #region get_blob
        /// <summary>
        /// Pulls blobs from MySQL DB
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public byte[] getSQL_BLOB(string sql, object[] paramObjects, bool isBase64= true)
        {
            return doSQL_BLOB(sql, paramObjects,isBase64);
        }
        public static byte[] doSQL_BLOB(string sql, object[] paramObjects, bool isBase64 = true)
        {
            if (isBase64)
            {
                var _BLOBstring = doSQL_string(sql, paramObjects);
                var _BLOB = Convert.FromBase64String(_BLOBstring);
                return _BLOB;
            }
            else
            {
                var my_blob = new byte[0];              
                var my_connection = do_open_conn();
                DbCommand comm = my_connection.CreateCommand();
                comm.CommandText = sql;
                AddParametersToComm(comm, paramObjects);
                try
                {
                    my_blob = comm.ExecuteScalar() as byte[];
                }
                catch (Exception ee)
                {
                    do_errorLog_query(ee, sql, paramObjects);
                    throw ee;
                }
                finally
                {
                    comm.Dispose();
                    do_close_connection(my_connection);
                }
                return my_blob;
            }
        }
        #endregion
        #region get_bool
        /// <summary>
        /// Retrieve a boolean typed value from the MySQL database
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool getSQL_bool(string sql, object[] paramObjects)
        {
            var my_bool = false;
            my_connection = do_open_conn();
            DbCommand comm = my_connection.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                comm.ExecuteNonQuery();
                my_bool = true;
            }
            catch
            {
                my_bool = false;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(my_connection);
            }
            return my_bool;
        }
        #endregion
        #region ReportWriter(DataTable _dt)
        public static string do_ReportWriter(DataTable _dt) { return do_ReportWriter(_dt, ""); }
        public static string do_ReportWriter(DataTable _dt, string onclick_event) { return do_ReportWriter(_dt, onclick_event, new string[0]); }
        public static string do_ReportWriter(DataTable _dt, string onclick_event, string[] col_widths)
        {
            var Output = "";
            if (_dt.Rows.Count > 0)
            {
                var gv = new GridView();
                if (onclick_event != "")
                {
                    var _actions = new DataColumn("#");
                    _dt.Columns.Add(_actions);
                    _dt.Columns["#"].SetOrdinal(0);
                }
                gv.DataSource = _dt;
                gv.DataBind();
                gv.UseAccessibleHeader = true;
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
                gv.Width = Unit.Percentage(100);
                gv.CssClass = "tablesorter";
                gv.Attributes["id"] = "ts" + do_RandomString(_dt.Rows.Count);
                gv.Style.Add("font-family", "arial");
                gv.Style.Add("font-size", "11px");
                gv.Style.Add("display", "none");
                var re_num = new Regex("^[0-9]+$");
                for (var r = 0; r < gv.Rows.Count; r++)
                {
                    if (onclick_event != "")
                    {
                        gv.Rows[r].Cells[0].Style.Add("text-align", "center");
                        gv.Rows[r].Cells[0].Text = string.Format(@"<img src='/images/icon/icon[details].gif' style='cursor:pointer;' onClick=""{0}"" />", onclick_event);
                    }
                    for (var c = 0; c < gv.Rows[r].Cells.Count; c++)
                    {
                        var _cell = gv.Rows[r].Cells[c];
                        if (col_widths.Length > 0)
                        {
                            _cell.Style.Add("width", col_widths[c] + "px");
                        }
                        _cell.Style.Add("border-right", "solid 1px #ddd");
                        _cell.Style.Add("border-bottom", "solid 1px #ddd");
                        if (re_num.Match(_cell.Text).Success || _cell.Text[0] == '<')
                        {
                            _cell.Style.Add("text-align", "center");
                        }
                        else
                        {
                            _cell.Style.Add("text-align", "left");
                        }
                    }

                }
                gv.BorderWidth = 0;
                gv.GridLines = 0;
                var sw = new StringWriter();
                var hw = new HtmlTextWriter(sw);
                gv.RenderControl(hw);
                var _pager = "";
                if (gv.Rows.Count > 10)
                {
                    _pager = "$('#" + gv.Attributes["id"] + "').tablesorter().tablesorterPager({positionFixed:false, size:10,container: $('#pages_" + gv.Attributes["id"] + "')}).show();$('#pages_" + gv.Attributes["id"] + "').slideDown();";
                }
                else
                {
                    _pager = "$('#" + gv.Attributes["id"] + "').tablesorter().show();";
                }
                Output += sw.ToString();
                Output += string.Format(@"
				<div id='pages_" + gv.Attributes["id"] + @"' align='center' style='display:none;'>
					<form>
						<button style='font-size:11px' type='button' class='first'>&lt;&lt;</button>
						<button style='font-size:11px' type='button' class='prev'>&lt;</button>
						<input type='text' size='5' style='font-size:11px;font-weight:bold;' class='pagedisplay'/>
						<button style='font-size:11px' type='button' class='next'>&gt;</button>
						<button style='font-size:11px' type='button' class='last'>&gt;&gt;</button>
						<select class='pagesize' style='width:50px;font-size:11px;'>
							<option selected='selected'  value='10'>10</option>
							<option value='20'>20</option>
							<option value='30'>30</option>
							<option  value='40'>40</option>
						</select>
					</form>
				</div>
				<script>
					$('body').ready(function()
										{{
										{0}
										}});
				</script>", _pager);
            }
            else
            {
                Output = "No available data";
            }
            return Output;
        }
        public string ReportWriter(DataTable _dt) { return ReportWriter(_dt, ""); }
        public string ReportWriter(DataTable _dt, string onclick_event) { return do_ReportWriter(_dt, onclick_event, new string[0]); }
        public string ReportWriter(DataTable _dt, string onclick_event, string[] col_widths) { return do_ReportWriter(_dt, onclick_event, col_widths); }
        #endregion
        public string RandomString(int size)
        {
            var builder = new StringBuilder();
            var random = new Random();
            char ch;
            for (var i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
        public static string do_RandomString(int size)
        {
            var builder = new StringBuilder();
            var random = new Random();
            char ch;
            for (var i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
        public static string do_md5(int size)
        {
            var str = do_RandomString(size);
            var enc = Encoding.Unicode.GetEncoder();
            var unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            var result = md5.ComputeHash(unicodeText);
            var sb = new StringBuilder();
            for (var i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public static MySqlConnection connect()
        {
            var conn = new MySqlConnection(str_connection_string);
            conn.Open();
            return conn;
        }
        public static MySqlConnection connect(string _connectionString)
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
        public MySqlConnection connection_manager(bool do_open)
        {
            if (do_open)
            {
                if (!connection_is_open || my_connection == null)
                {
                    my_connection = new MySqlConnection();
                    my_connection.ConnectionString = connection_string;
                    my_connection.Open();
                    connection_is_open = true;
                    return my_connection;
                }

                return my_connection;
            }
            else
            {
                if (my_connection != null && my_connection.State == ConnectionState.Open)
                {
                    my_connection.Dispose();
                    return my_connection;
                }
                else
                {
                    return my_connection;
                }
            }
        }
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }
        private Dictionary<string, string> parse_connection_string(string str)
        {
            return str.TrimEnd(';').Split(';').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1]);
        }
        private bool connection_strings_are_different(string conn1, string conn2)
        {
            conn1 = conn1.ToLower();
            conn2 = conn2.ToLower();
            var dict1 = parse_connection_string(conn1);
            var dict2 = parse_connection_string(conn2);
            var is_different = false;
            var has_server_key1 = dict1.ContainsKey("server");
            var has_server_key2 = dict2.ContainsKey("server");
            var has_db_key1 = dict1.ContainsKey("database");
            var has_db_key2 = dict2.ContainsKey("database");

            if (has_server_key1 && has_server_key2 && dict1["server"] != dict2["server"])
            {
                is_different = true;
            }
            else if (has_db_key1 && has_db_key2 && dict1["database"] != dict2["database"])
            {
                is_different = true;
            }
            return is_different;
        }
        #region get_datatable	
        /// <summary>
        /// Retrieve a datatable from a query result of the MySQL database
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable getSQL_datatable(string sql, object[] paramObjects)
        {
            my_connection = do_open_conn();
            if (connection_strings_are_different(my_connection.ConnectionString, connection_string))
            {
                my_connection.Close();
                my_connection.ConnectionString = connection_string;
                my_connection.Open();
            }
            var my_dt = new DataTable();
            var my_ds = new DataSet();
            var my_da = new MySqlDataAdapter();
            my_da.SelectCommand = my_connection.CreateCommand();
            my_da.SelectCommand.CommandText = sql;
            AddParametersToComm(my_da.SelectCommand, paramObjects);
            try
            {
                my_da.Fill(my_ds);
                if (my_ds.Tables.Count > 0)
                {
                    my_dt = my_ds.Tables[0].Copy();
                }
            }
            catch (Exception ee)
            {

                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                my_da.Dispose();
                do_close_connection(my_connection);
            }
            return my_dt;
        }

        #endregion
        public static DataTable doSQL_dt(string sql, object[] paramObjects)
        {
			using (var conn = do_open_conn())
            {
                using (var my_da = new MySqlDataAdapter())
                {
                    var my_ds = new DataSet();
                    my_da.SelectCommand = conn.CreateCommand();
                    my_da.SelectCommand.CommandText = sql;
                    AddParametersToComm(my_da.SelectCommand, paramObjects);

                    my_da.Fill(my_ds);
					DataTable my_dt = my_ds.Tables[0].Copy();
					return my_dt;
                }
            }
        }

        public static DataTable doSQL_dt(string sql)
        {
            var my_dt = new DataTable();
            using (var conn = do_open_conn())
            {
                using (var my_da = new MySqlDataAdapter())
                {
                    var my_ds = new DataSet();
                    my_da.SelectCommand = conn.CreateCommand();
                    my_da.SelectCommand.CommandText = sql;
                    my_da.Fill(my_ds);
                    my_dt = my_ds.Tables[0].Copy();
                    return my_dt;
                }
            }
        }

        public static MySqlConnection do_open_conn(string db)
        {
            var conn = new MySqlConnection();
            conn.ConnectionString = str_connection_string;
            try
            {
                if (!conn.Ping())
                {
                    conn.Open();
                }
            }
            catch (Exception ee)
            {
                do_errorLog(ee);
                throw new Exception("Unable to connect to any of the specified MySQL hosts");
            }
            return conn;
        }
		public static MySqlConnection do_open_conn()
			{
			var conn = new MySqlConnection
				{
				ConnectionString = str_connection_string + ";Pooling=true;Min Pool Size=5;Max Pool Size=200;Keepalive=1;ConnectionLifeTime=50;"
				};

			try
				{
				if (conn.State == ConnectionState.Closed)
					{
					conn.Open();
					}
				}
			catch (Exception ex)
				{
				// Consider logging the exception
				// For now, we will just rethrow it
				throw ex;
				}
			return conn;
			}

		public void close_connection()
        {
            my_connection.Dispose();
        }
        public static string MobileEncryptionPassword()
        {
            return "$!18$uv58SjET9QaIH5";
        }
        public static void do_close_connection(MySqlConnection c)
        {
            if (c != null && c.State == ConnectionState.Open)
            {
                try
                {
                    c.Close();
                    c.Dispose();
                }
                catch (Exception ee)
                {
                    do_errorLog(ee);
                }
            }
        }
        public static int get_currentMemberId()
        {
            var id = 0;
            HttpSessionState s = null;
            if (HttpContext.Current != null)
                s = HttpContext.Current.Session;

            var m = s != null && s["profile"] != null ? (NeMember)s["profile"] : new NeMember();
            id = m.id;
            return id;
        }
        public static string get_stackTrace(Exception ee = null, bool useException = false)
        {
            var stackTraceData = "1";

            if (useException)
            {
                if (ee.StackTrace != null)
                {
                    return ee.StackTrace;
                }
            }
            else
            {
                return Environment.StackTrace;
            }

            return stackTraceData;
        }
        public static void do_errorLog_go(string intError, string shortError, string stackTrace, bool isGlobal = false)
        {

            var id = get_currentMemberId();
            var stackTraceData = "";



            if (stackTrace != null)
            {
                stackTraceData = stackTrace;
            }

            var host = "";
            var ip = "";
            var absPath = "";
            var query_string = "";
            var user_agent = "";
            var machine_name = "";
            var abs_uri = "";
            var isLocal = "0";
            if (HttpContext.Current != null)
            {
                var u = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                host = u.Host;


                ip = HttpContext.Current.Request.UserHostAddress;
                absPath = HttpContext.Current.Request.Url.AbsolutePath;
                query_string = HttpContext.Current.Request.Url.Query;
                user_agent = HttpContext.Current.Request.UserAgent;
                machine_name = HttpContext.Current.Server.MachineName;
                abs_uri = HttpContext.Current.Request.Url.AbsoluteUri;
                if (HttpContext.Current.Request.IsLocal)
                    isLocal = "1";


            }



            var sql = string.Format(@"call error_log.insertError2({0}, '{1}', '{2}', '{3}', '{4}', 
				'{5}', '{6}', '{7}', '{8}', '{9}',
				'{10}', {11}, {12})",
                id, smallString(AddSlashes(intError), 700), AddSlashes(shortError), host, AddSlashes(stackTraceData), ip, smallString(AddSlashes(machine_name), 75),
                smallString(AddSlashes(query_string), 50), smallString(AddSlashes(absPath), 70), smallString(AddSlashes(user_agent), 499), "123", isGlobal, isLocal);


            var conn = do_open_conn();
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                //Should er emailthis?
                if (!isLocal.Equals("1"))
                {
                    var em = new NeEMail();
                    em.To = app_setting("debug_email");
                    em.Body = string.Format(@"{0}, '{1}', '{2}', '{3}', '{4}', 
				'{5}', '{6}', '{7}', '{8}', '{9}',
				'{10}', {11}, {12}",
                        id, smallString(AddSlashes(intError), 700), AddSlashes(shortError), host, AddSlashes(stackTraceData), ip, smallString(AddSlashes(machine_name), 75),
                        smallString(AddSlashes(query_string), 50), smallString(AddSlashes(absPath), 70), smallString(AddSlashes(user_agent), 499), "123", isGlobal, isLocal);
                }
            }
            finally
            {
                comm.Dispose();
                do_close_connection(conn);
            }

        }
        public static string smallString(string strIn, int max)
        {
            if (strIn.Length > max)
            {
                strIn = strIn.Substring(0, max);
            }
            return strIn;
        }
        public static void do_errorLog_query(Exception ee, string sql)
        {
            do_errorLog_go(ee.Message, "Bad query: " + sql, get_stackTrace(ee));
        }
        public static void do_errorLog_query(Exception ee, string sql, object[] paramsObj)
        {
            if (paramsObj != null)
            {
                do_errorLog_go(ee.Message, "Bad query: " + sql + dict_dump(dict_create(paramsObj)), get_stackTrace(ee));
            }
            else
            {
                do_errorLog_go(ee.Message, "Bad Query: " + sql, get_stackTrace(ee));
            }
        }
        public static void do_errorLog(Exception ee, string error)
        {
            do_errorLog_go(ee.Message, "Error: " + error, get_stackTrace(ee));
        }
        public static void do_errorLog_errorStack(Exception ee)
        {
            do_errorLog_go(ee.Message, "", ee.StackTrace, true);
        }
        public static void do_errorLog(Exception ee)
        {
            do_errorLog_go(ee.Message, "", get_stackTrace(ee));
        }
        public static void do_errorLog(string error)
        {
            do_errorLog_go(error, "", "");
        }
        public static void do_errorLog(string error, string error2 = "", int level = 0)
        {
            do_errorLog_go(error, error2, get_stackTrace());
        }
		
        /// <summary>
        /// Adds a stylesheet to the current Http Context
        /// </summary>
        /// <param name="url"></param>
        public void add_css(string url)
        {
            if (HttpContext.Current != null)
            {
                var page = HttpContext.Current.Handler as Page;
                var this_css = new HtmlLink();
                this_css.Href = url;
                this_css.Attributes.Add("type", "text/css");
                this_css.Attributes.Add("rel", "stylesheet");
                if (page != null)
                {
                    page.Header.Controls.Add(this_css);
                }
            }
        }
        public static void do_add_css(Page page, string url)
        {
            var this_css = new HtmlLink();
            this_css.Href = url;
            this_css.Attributes.Add("type", "text/css");
            this_css.Attributes.Add("rel", "stylesheet");
            if (page != null)
            {
                page.Header.Controls.Add(this_css);
            }
        }
        public static void inject_css(string url, Page page)
        {
            var this_css = new HtmlLink { Href = url };
            this_css.Attributes.Add("type", "text/css");
            this_css.Attributes.Add("rel", "stylesheet");
            page.Header.Controls.Add(this_css);
        }
        #region get_dataset	
        /// <summary>
        /// Retrieve a dataset from a query result of the MySQL database
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet getSQL_dataset(string sql, object[] paramObjects)
        {
            my_connection = do_open_conn();
            var my_ds = new DataSet();
            var my_da = new MySqlDataAdapter();
            my_da.SelectCommand = my_connection.CreateCommand();
            my_da.SelectCommand.CommandText = sql;
            AddParametersToComm(my_da.SelectCommand, paramObjects);
            try
            {
                my_da.Fill(my_ds);
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                my_da.Dispose();
                do_close_connection(my_connection);
            }
            return my_ds;
        }
        #endregion
        public static void do_debug(object a)
        {
            Debug.WriteLine(a + " @ " + MySQLNow_long());
        }
        #region get_double	
        /// <summary>
        /// Retrieve a double point precision value from the MySQL database
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public double getSQL_double(string sql, object[] paramObjects)
        {
            double my_double = 0;
            my_connection = do_open_conn();
            DbCommand comm = my_connection.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                my_double = Convert.ToDouble(comm.ExecuteScalar().ToString());
            }
            catch (Exception ee)
            {

                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(my_connection);
            }
            return my_double;
        }
        #endregion
        /// <summary>
        /// <para>Perform a SQL statement that returns a BOOL. </para>
        /// <para>For use with MySQL</para>
        /// </summary>
        /// <param name="sql"></param>
        public static bool doSQL_bool(string sql, object[] paramObjects)
        {
            var my_bool = false;
            var conn = do_open_conn();
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);

            try
            {
                bool.TryParse(comm.ExecuteScalar().ToString(), out my_bool);
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(conn);
            }
            return my_bool;
        }

        public static bool doSQL_bool(MySqlConnection _conn, string _sql, object[] paramObjects)
        {
            bool result;
            using (var comm = _conn.CreateCommand())
            {
                comm.CommandText = _sql;
                AddParametersToComm(comm, paramObjects);
                result = Convert.ToBoolean(comm.ExecuteScalar().ToString());
            }
            return result;
        }

        public static string doSQL_string(string sql, int paramInt)
        {
            return doSQL_string(sql, new object[] { paramInt });
        }

        public static string doSQL_string(string sql, string paramString)
        {
            return doSQL_string(sql, new object[] { paramString });
        }
        public static string doSQL_string(string sql)
        {
            return doSQL_string(sql, paramObjects: null);
        }

        /// <summary>
        /// <para>Perform a SQL statement that returns an INT. </para>
        /// <para>For use with MySQL</para>
        /// </summary>
        /// <param name="sql"></param>
        public static string doSQL_string(string sql, object[] paramObjects)
        {
            var my_string = "";
            var conn = do_open_conn();
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                my_string = comm.ExecuteScalar().ToString();
            }
            catch (Exception ee)
            {
                //Toolbox.doSQL_void(@"INSERT INTO matt (jumble, dt)  VALUES ('STATIC do_string, bad query:@v0, NOW())",new object[] { sql }  , null);

                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(conn);
            }
            return my_string;
        }
        public static string doSQL_string(string sql, object[] paramsObjects, bool close_connection)
        {
            do_errorLog(Environment.StackTrace.ToString(), "STATIC do_string(sql, close_connection) has been deprecated", 5);
            var my_string = "";
            var conn = new MySqlConnection(conn_string());
            conn.Open();
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramsObjects);
            try
            {
                my_string = comm.ExecuteScalar().ToString();
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramsObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                conn.Dispose();
            }
            return my_string;
        }
        /// <summary>
        /// <para>Perform a SQL statement that returns an INT. </para>
        /// <para>For use with MySQL</para>
        /// </summary>
        /// <param name="sql"></param>
        public static DateTime doSQL_datetime(string sql, object[] paramObjects)
        {
            var my_datetime = new DateTime();
            var conn = do_open_conn();
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);

            try
            {
                my_datetime = Convert.ToDateTime(comm.ExecuteScalar().ToString());
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(conn);
            }
            return my_datetime;
        }
        /// <summary>
        /// <para>Perform a SQL statement that returns a double</para>
        /// <para>For use with MySQL</para>
        /// </summary>
        /// <param name="sql"></param>
        public static double doSQL_double(string sql, object[] paramObjects)
        {
            double my_double = 0;
            var conn = do_open_conn();
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);

            try
            {
                //Toolbox.doSQL_void(@"INSERT INTO matt (jumble, dt)  VALUES ('STATIC do_double, bad query:@v0, NOW())",new object[] { sql }  , null);
                my_double = Convert.ToDouble(comm.ExecuteScalar().ToString());
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(conn);
            }
            return my_double;
        }
        public static DataTable doSQL_dt(MySqlConnection _conn, string _sql, object[] paramObjects)
        {
            using (var my_da = new MySqlDataAdapter())
            {
                var my_ds = new DataSet();
                my_da.SelectCommand = _conn.CreateCommand();
                my_da.SelectCommand.CommandText = _sql;
                AddParametersToComm(my_da.SelectCommand, paramObjects);
                my_da.Fill(my_ds);
                var my_dt = my_ds.Tables[0].Copy();
                return my_dt;
            }
        }

      
        public static string doSQL_string(MySqlConnection _conn, string _sql, object[] paramsObjects)
        {
            string my_string;
            using (var comm = _conn.CreateCommand())
            {
                comm.CommandText = _sql;
                AddParametersToComm(comm, paramsObjects);
                my_string = comm.ExecuteScalar().ToString();
            }
            return my_string;
        }
        public static int doSQL_int(MySqlConnection _conn, string _sql, object[] paramObjects)
        {
            int my_int;
            using (var comm = _conn.CreateCommand())
            {
                comm.CommandText = _sql;
                AddParametersToComm(comm, paramObjects);
                my_int = Convert.ToInt32(comm.ExecuteScalar().ToString());
            }
            return my_int;
        }
        public static int doSQL_int(string _sql, object[] paramObjects)
        {
            using (var conn = connect())
            {
                return doSQL_int(conn, _sql, paramObjects);
            }
        }
        public static long doSQL_long(string _sql, object[] _paramObjects)
        {
            using (var conn = connect())
            {
                return doSQL_long(conn, _sql, _paramObjects);
            }
        }
        public static long doSQL_long(MySqlConnection _conn, string _sql, object[] _paramObjects)
        {
            long myLong;
            DbCommand comm = _conn.CreateCommand();
            comm.CommandText = _sql;
            AddParametersToComm(comm, _paramObjects);
            try
            {
                myLong = comm.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, _sql, _paramObjects);
                throw;
            }
            return myLong;
        }
        public static int doSQL_int(string _sql)
        {
            using (var conn = connect())
            {
                return doSQL_int(conn, _sql, null);
            }
        }
        public static int doSQL_int(string _sql, string paramString)
        {
            using (var conn = connect())
            {
                return doSQL_int(conn, _sql, new object[] { paramString });
            }
        }
        public static int doSQL_int(string _sql, int paramInt)
        {
            using (var conn = connect())
            {
                return doSQL_int(conn, _sql, new object[] { paramInt });
            }
        }
        public static decimal doSQL_decimal(string _sql, object[] paramObjects)
			{
            using(var myConn = connect())
				{
                return doSQL_decimal(myConn, _sql, paramObjects);
				}
			}
        public static decimal doSQL_decimal(MySqlConnection _conn, string _sql, object[] paramObjects)
			{
			decimal my_decimal;
            using (var comm = _conn.CreateCommand())
            {
                comm.CommandText = _sql;
                AddParametersToComm(comm, paramObjects);

			decimal.TryParse(comm.ExecuteScalar().ToString(), out my_decimal);
            }
            return my_decimal;
			}
        public static double doSQL_double(MySqlConnection _conn, string _sql, object[] paramObjects)
        {
            double my_double;
            using (var comm = _conn.CreateCommand())
            {
                comm.CommandText = _sql;
                AddParametersToComm(comm, paramObjects);

                double.TryParse(comm.ExecuteScalar().ToString(), out my_double);
            }
            return my_double;
        }
        #region get_int
        /// <summary>
        /// Retrieve an integer typed value from the MySQL database
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int getSQL_int(string sql, object[] paramObjects)
        {
            var my_int = 0;
            my_connection = do_open_conn();
            if (connection_strings_are_different(my_connection.ConnectionString, connection_string))
            {
                my_connection.Close();
                my_connection.ConnectionString = connection_string;
                my_connection.Open();
            }
            DbCommand comm = my_connection.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                my_int = Convert.ToInt32(comm.ExecuteScalar().ToString());
            }
            catch (Exception ee)
            {

                do_errorLog_query(ee, sql, paramObjects);

                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(my_connection);
            }
            return my_int;
        }

        public int getSQL_int(MySqlConnection connection, string sql, object[] paramObjects)
        {
            var my_int = 0;
            DbCommand comm = connection.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                my_int = Convert.ToInt32(comm.ExecuteScalar().ToString());
            }
            catch (Exception ee)
            {

                do_errorLog_query(ee, sql, paramObjects);

                throw ee;
            }
            finally
            {
                comm.Dispose();
            }
            return my_int;
        }
        #endregion

        /// <summary>
        /// Runs the supplied sql query and returns the number of affected rows in an INT format.
        /// </summary>
        /// <param name="sql"></param>
        public int getSQL_affectedrows(string sql, object[] paramObjects)
        {
            var my_int = 0;
            var conn = do_open_conn();
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                my_int = comm.ExecuteNonQuery();
            }
            catch (Exception ee)
            {

                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(conn);
            }
            return my_int;
        }
        public static int doSQL_affectedrows(string sql, object[] paramObjects)
        {
            using (var myConn = Toolbox.connect())
            {
                return doSQL_affectedrows(myConn, sql, paramObjects);
            }
                
        }
        public static int doSQL_affectedrows(MySqlConnection conn , string sql, object[] paramObjects)
        {
            var my_int = 0;
           
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                my_int = comm.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(conn);
            }
            return my_int;
        }
        #region debug_note
        public static void do_debug_note(object note)
        {
            var _note = note.ToString();
            Toolbox.doSQL_void(@"INSERT INTO matt (jumble) VALUES (@v0)", new object[] { _note });
        }
        public void debug_note(object note)
        {
            var _note = note.ToString();
            getSQL_void(@"INSERT INTO matt (jumble) VALUES (@v0)", new object[] { _note });
        }
        #endregion debug_note
        #region get_string
        /// <summary>
        /// Retrieve a string typed value from the MySQL database
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string getSQL_string(string sql, object[] paramObjects)
        {
            var my_string = "";
            my_connection = do_open_conn();
            DbCommand comm = my_connection.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                my_string = comm.ExecuteScalar().ToString();
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(my_connection);
            }
            return my_string;
        }

        /// <summary>
        /// Retrieve a string typed value from the MySQL database
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="paramObjects"></param>
        /// <returns></returns>
        public string getSQL_string(MySqlConnection connection, string sql, object[] paramObjects)
        {
            var my_string = "";
            DbCommand comm = connection.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                my_string = comm.ExecuteScalar().ToString();
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
            }
            return my_string;
        }

        #endregion
        #region get_void

        /// <summary>
        /// <para>Perform a SQL statement, returning the inserted id. </para>
        /// <para>For use with MySQL</para>
        /// </summary>
        /// <param name="sql"></param>
        public int getSQL_return_id(string sql, object[] paramObjects)
        {
            var my_int = 0;
            my_connection = do_open_conn();
            if (connection_strings_are_different(my_connection.ConnectionString, connection_string))
            {
                my_connection.Close();
                my_connection.ConnectionString = connection_string;
                my_connection.Open();
            }
            DbCommand comm = my_connection.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                // Execute insert query
                comm.ExecuteNonQuery();
                // Create new command, same connection though
                comm = my_connection.CreateCommand();
                comm.CommandText = "SELECT LAST_INSERT_ID()";
                my_int = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(my_connection);
            }
            return my_int;
        }

        public void getSQL_void(string sql)
        {
            getSQL_void(sql, null);
        }

        public void getSQL_void(string sql, object[] paramsObjects, MySqlConnection conn, MySqlTransaction transaction)
        {
            my_connection = conn;

            DbCommand comm = my_connection.CreateCommand();
            comm.Transaction = transaction;
            comm.CommandText = sql;
            AddParametersToComm(comm, paramsObjects);
            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramsObjects);
                throw ee;
            }

        }
        /// <summary>
        /// <para>Perform a SQL statement that doesn't require a return. </para>
        /// <para>For use with MySQL</para>
        /// </summary>
        /// <param name="sql"></param>
        public void getSQL_void(string sql, object[] paramsObjects)
        {
            my_connection = do_open_conn();
            if (connection_strings_are_different(my_connection.ConnectionString, connection_string))
            {
                my_connection.Close();
                my_connection.ConnectionString = connection_string;
                my_connection.Open();
            }
            DbCommand comm = my_connection.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramsObjects);
            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception ee)
            {

                do_errorLog_query(ee, sql, paramsObjects);

                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(my_connection);
            }
        }
        #endregion




        static public void AddParametersToComm(DbCommand comm, object[] paramObjects)
        {
            if (paramObjects == null) return;
            for (int i = 0; i < paramObjects.Length; i++)
            {
                DbParameter param = comm.CreateParameter();
                param.ParameterName = "v" + i.ToString();
                if (paramObjects[i] is String)
                    paramObjects[i] = paramObjects[i].ToString().Replace("''", "'");
                if (paramObjects[i] != null && paramObjects[i].ToString().ToLower() == "null")
                {
                    param.Value = DBNull.Value;
                }
                else
                {
                    param.Value = paramObjects[i];
                }
                comm.Parameters.Add(param);
            }
        }



        /// <summary>
        /// <para>Perform a SQL statement that doesn't require a return. </para>
        /// <para>For use with MySQL</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramsStrings"></param>
        public static void doSQL_void(MySqlConnection _conn, string _sql, object[] paramObjects, MySqlTransaction transation = null)
        {
            using (var comm = _conn.CreateCommand())
            {
                if (transation != null)
                {
                    comm.Transaction = transation;
                }
                comm.CommandText = _sql;
                AddParametersToComm(comm, paramObjects);
                comm.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// <para>Perform a SQL statement that doesn't require a return. </para>
        /// <para>For use with MySQL</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramsStrings"></param>
        static public void doSQL_void(string sql, object[] paramObjects)
        {
            var conn = do_open_conn();
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception ee)
            {

                do_errorLog_query(ee, sql, paramObjects);
                throw new Exception(ee.Message + "--" + sql);
            }
            finally
            {
                comm.Dispose();
                do_close_connection(conn);
            }
        }
        public static void doSQL_void(string _sql)
        {
            doSQL_void(_sql, paramObjects: null);
        }

        public static void doSQL_void(string _sql, string paramString)
        {
            doSQL_void(_sql, new object[] { paramString });
        }
        public static void doSQL_void(string _sql, int paramInt)
        {

            doSQL_void(_sql, new object[] { paramInt });
        }


        #region handle_authentication

        public static NeMember Authenticate([NotNull] string username, [NotNull] string password)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var currentUser = new NeMember(username, password);
            return currentUser;
        }

        /// <summary>
        /// Return member authenticated with the given ID (provided only for completeness)
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static NeMember Authenticate(int memberId)
        {
            var member = new NeMember(memberId);
            if (!member.Authenticated) throw new InvalidOperationException($"Member with if {memberId} invalid state");
            return member;
        }

        public static ne_session CreateSession(NeMember member, bool isN1 = true)
        {
            if (member == null || !member.Authenticated) throw new ArgumentException(@"Invalid member", nameof(member));
            var currentSession = HttpContext.Current.Session;
            var neSession = new ne_session
            {
                ip = HttpContext.Current.Request.UserHostAddress,
                member_id = member.id,
                is_active = true,
                is_contact = member.isContact,
                res_x = 0,
                res_y = 0
            };
            neSession.save();
            if (isN1)
            {
                currentSession.Add("is_n1", 1);
            }

            currentSession.Add("session", neSession.id);
            currentSession.Add("profile", member);
            Logger.Debug($"Created session for ASP session {currentSession.SessionID}. Member: {member.id}");

            return neSession;
        }


        public static void DoSignOut()
        {
            var context = HttpContext.Current;
            var currentSession = context.Session;
            var applicationState = context.Application;

            if (currentSession != null)
            {
                var user = new NeMember();
                if (!string.IsNullOrEmpty(currentSession["session"]?.ToString()))
                {
                    if (int.TryParse(currentSession["session"].ToString(), out var sessionId))
                    {
                        user = new NeMember(sessionId.ToString());
                        var session = new ne_session(sessionId) { is_active = false };
                        session.save();
                        Logger.Debug($"Cleared out session for ASP session {currentSession.SessionID}. Database session: {sessionId}");

                    }
                }

                currentSession.Clear();
                if (user.id > 0 && notification.user_exists(user.id, applicationState))
                {
                    notification.remove_user(user.id, applicationState);
                }
            }

        }

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static NeMember do_handle_authentication(int pageId)
        {
            var currentMember = new NeMember();
            if (HttpContext.Current == null) throw new InvalidOperationException("No current context");

            var currentSession = HttpContext.Current.Session;
            var httpResponse = HttpContext.Current.Response;
            var httpRequest = HttpContext.Current.Request;
            var hasOverrideQueryString = !string.IsNullOrEmpty(httpRequest.QueryString["override_member_id"]);
            var hasOverrideSession = currentSession["override_member_id"] != null && !string.IsNullOrEmpty(currentSession["override_member_id"].ToString());
            var overrideExistingSession = hasOverrideQueryString && hasOverrideSession && currentSession["override_member_id"].ToString() != httpRequest.QueryString["override_member_id"];

            if (httpRequest.Url.Host == "sparkops.web.localhost" && (hasOverrideQueryString || hasOverrideSession ))
				{
                if(hasOverrideQueryString && (!hasOverrideSession || overrideExistingSession)) // It has a query string request, and the session variable hasn't been set
                    {
                    currentSession["override_member_id"] = httpRequest.QueryString["override_member_id"];
                    }
				var tempMember = new NeMember(Convert.ToInt32(currentSession["override_member_id"]));
				currentSession["global_visible_business_units"] = doSQL_string(@"SELECT IFNULL(GET_VISIBLE_BUSINESS_UNITS_GROUP_CONCAT(@v0), '0')", new object[]{tempMember.id});
				currentSession["global_visible_tax_entities"] = doSQL_string(@"CALL GET_VISIBLE_TAX_ENTITIES_GROUP_CONCAT(@v0)", new object[] { tempMember.id });
                var session = new ne_session
                                  {
                                  ip         = httpRequest.UserHostAddress,
                                  member_id  = tempMember.id,
                                  is_active  = true,
                                  is_contact = false,
                                  res_x      = 0,
                                  res_y      = 0
                                  };
                session.save();
				currentSession["session"] = currentSession["session"] ?? session.id;
				currentSession["profile"] = tempMember;
                return tempMember;
				}
            Logger.Debug($"Authenticating for page {pageId} from session {currentSession.SessionID}");

            if (currentSession["session"] != null)
            {
                if (currentMember.id == 0)
                {
                    currentMember = new NeMember(currentSession["session"].ToString());
                }

                currentSession.Add("member_id", currentMember.id.ToString());

                SetGlobalVisibility(currentMember);

                if (!currentMember.Authenticated)
                {
                    httpResponse.Redirect($"/default.aspx?origin={HttpContext.Current.Server.UrlEncode(httpRequest.RawUrl)}", true);
                }
                else if (!currentMember.AuthenticatedForPage(pageId))
                {
                    httpResponse.Redirect($"/error.aspx?page_id={pageId}", true);
                }

                if (currentMember.isContact)
                {
                    if (new NePage(Convert.ToInt32(pageId)).cust_enabled == false)
                    {
                        httpResponse.Redirect($"/error.aspx?page_id={pageId}", true);
                    }
                }

                return currentMember;
            }

            if (httpRequest.HttpMethod == "POST")
            {
                throw new NesiException("Your session has been logged out, you will need to log back in... your last request did not go through.");
            }

            httpResponse.Redirect($"/default.aspx?origin={HttpContext.Current.Server.UrlEncode(httpRequest.RawUrl)}", true);
            return null;
        }

        public static void SetGlobalVisibility(NeMember currentMember)
        {
            var currentSession = HttpContext.Current.Session;

            if (currentSession != null && currentMember.id > 0 && (currentSession["global_visible_tax_entities"] == null ||
                                         currentSession["global_visible_business_units"] == null ||
                                         currentSession["global_visible_users"] == null ||
                                         currentSession["global_logo_icon"] == null ||
                                         currentSession["global_logo_full"] == null ||
                                         currentSession["global_visible_reporting_users"] == null)
            )
            {
               NeMember.set_global_visibility_vars(currentSession, currentMember.id);
            }
        }


        private static string fixURLBase64(string a)
        {

            HttpContext.Current.Response.Write(a + ", 1<br>");
            a = a.Replace(" ", "+");

            if (a.Contains(","))
            {

                a = a.Substring(0, a.IndexOf(",", StringComparison.Ordinal));
            }

            HttpContext.Current.Response.Write(a + ", 2<br>");
            int mod4 = a.Length % 4;
            if (mod4 > 0)
            {
                a += new string('=', 4 - mod4);
            }

            HttpContext.Current.Response.Write(a + ", 3 <br>");
            return a;
        }
        
     
       
        #endregion
        #region HTMLize_datatable
        /// <summary>
        /// This will take raw, URI encoded data from a datatable and convert it
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>
        public DataTable HTMLize_datatable(DataTable _dt)
        {
            var column_count = _dt.Columns.Count;
            foreach (DataRow _dr in _dt.Rows)
            {
                for (var i = 0; i < column_count; i++)
                {
                    if (_dr[i].GetType() == Type.GetType("System.String"))
                    {
                        _dr[i] = HttpContext.Current.Server.HtmlDecode(_dr[i].ToString());
                    }
                }
            }
            return _dt;
        }
        #endregion
        #region recode_datatable
        /// <summary>
        /// This will take raw, URI encoded data from a datatable and convert it
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="_encode">True will make it html, false will just url decode it.</param>
        /// <returns></returns>
        public DataTable recode_datatable(DataTable _dt, bool _encode)
        {
            var column_count = _dt.Columns.Count;
            foreach (DataRow _dr in _dt.Rows)
            {
                for (var i = 0; i < column_count; i++)
                {
                    var column_name = _dt.Columns[i].Caption;
                    if (_dr[i].GetType() == Type.GetType("System.String") && !string.IsNullOrEmpty((string)_dr[i]))
                    {
                        var val = (string)_dr[i];
                        val = val.Replace("+", " ");
                        try
                        {
                            val = Uri.UnescapeDataString(val);
                        }
                        catch
                        {
                            val = value_from(val, false);
                        }
                        _dr[i] = val;
                    }
                }
            }
            return _dt;
        }
        #endregion
        #region Monetize
        /// <summary>
        /// Provides a shortcut way of formatting an object into a monetary format
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public string Monetize(object amount)
        {
            return do_Monetize(amount);
        }
        #endregion Monetize
        #region Monetize
        /// <summary>
        /// Provides a shortcut way of formatting an object into a monetary format
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string do_Monetize(object amount)
        {
            if (amount.ToString().Trim().Length == 0)
            {
                amount = 0;
            }
            var _money = Convert.ToDouble(amount.ToString().Replace(",", ""));
            return _money.ToString("$#,###,##0.00;-$#,###,##0.00");
        }
        #endregion Monetize

        public static int doSQL_return_id(string sql, object[] paramObjects)
        {
            var my_int = 0;
            var MySQL = do_open_conn();
            DbCommand comm = MySQL.CreateCommand();
            comm.CommandText = sql;
            if (paramObjects != null) AddParametersToComm(comm, paramObjects);
            try
            {
                // Execute insert query
                comm.ExecuteNonQuery();
                // Create new command, same connection though
                comm = MySQL.CreateCommand();
                comm.CommandText = "SELECT LAST_INSERT_ID()";
                my_int = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
                do_close_connection(MySQL);
            }
            return my_int;
        }

        /// <summary>
        /// <para>Perform a SQL statement, returning the inserted id. </para>
        /// <para>For use with MySQL</para>
        /// </summary>
        /// <param name="sql"></param>
        public static int doSQL_return_id(string sql, object[] paramObjects, MySqlConnection conn, MySqlTransaction transaction)
        {
            var my_int = 0;
            var MySQL = conn;
            DbCommand comm = MySQL.CreateCommand();
            comm.Transaction = transaction;
            comm.CommandText = sql;
            if (paramObjects != null) AddParametersToComm(comm, paramObjects);

            // Execute insert query
            comm.ExecuteNonQuery();
            // Create new command, same connection though
            comm = MySQL.CreateCommand();
            comm.CommandText = "SELECT LAST_INSERT_ID()";
            my_int = Convert.ToInt32(comm.ExecuteScalar());
            return my_int;
        }
        /// <summary>
        /// <para>Perform a SQL statement, returning the inserted id. </para>
        /// <para>For use with MySQL</para>
        /// </summary>
        /// <param name="sql"></param>
        public static int doSQL_return_id(MySqlConnection conn, string sql, object[] paramObjects)
        {
            var my_int = 0;
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            AddParametersToComm(comm, paramObjects);
            try
            {
                // Execute insert query
                comm.ExecuteNonQuery();
                // Create new command, same connection though
                comm = conn.CreateCommand();
                comm.CommandText = "SELECT LAST_INSERT_ID()";
                my_int = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ee)
            {
                do_errorLog_query(ee, sql, paramObjects);
                throw ee;
            }
            finally
            {
                comm.Dispose();
            }
            return my_int;
        }
        #region return_id
        /// <summary>
        /// Insert a record into the MySQL database and returns the associated id
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string returnSQL_id(string sql, object[] paramObjects, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            if (connection == null || transaction == null)
                return doSQL_return_id(sql, paramObjects).ToString();
            else
                return doSQL_return_id(sql, paramObjects, connection, transaction).ToString();
        }
        #endregion
        #region set_plain_header
        /// <summary>
        /// Sets the appropriate header for Plain Text documents
        /// </summary>
        public void set_plain_header()
        {
            _response.Clear();
            _response.ContentType = "text/plain";
        }
        #endregion
        #region set_XML_header
        /// <summary>
        /// Sets the appropriate header for XML documents
        /// </summary>
        public void set_XML_header()
        {
            _response.Clear();
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
                HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetNoStore();
            }
            _response.ContentType = "text/xml";
            _response.Write("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
        }
        #endregion
        #region value_from
        /// <summary>
        /// This is used when you want to make a value acceptable for HTML
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string value_from(object value)
        {
            return value_from(value, true);
        }
        #endregion value_from
        #region value_from
        /// <summary>
        /// This is used when you want to make a value acceptable for HTML or just plainly decode a value
        /// </summary>
        /// <param name="value">Anything</param>
        /// <param name="force">True will return a html encoded string, false will not.</param>
        /// <returns></returns>
        public string value_from(object value, bool force)
        {
			if(value == null) return "";
            var temp_string = force ? HttpUtility.HtmlEncode(HttpUtility.UrlDecode(value.ToString())).Replace("%22", "&quot;") : HttpUtility.UrlDecode(value.ToString());
            return temp_string;
        }
        #endregion value_from
        #region value_from
        /// <summary>
        /// This is used when you want to make a value acceptable for HTML
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string do_value_from(object value)
        {
            return do_value_from(value, true);
        }
        #endregion value_from
        #region value_from
        /// <summary>
        /// This is used when you want to make a value acceptable for HTML or just plainly decode a value
        /// </summary>
        /// <param name="value">Anything</param>
        /// <param name="force">True will return a html encoded string, false will not.</param>
        /// <returns></returns>
        public static string do_value_from(object value, bool force)
        {
            var temp_string = force ? HttpUtility.HtmlEncode(HttpUtility.UrlDecode(value.ToString())).Replace("%22", "&quot;") : HttpUtility.UrlDecode(value.ToString());
            return temp_string;
        }
        #endregion value_from
        #region value_to
        /// <summary>
        /// This is used when you want to make a value acceptable for MySQL
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string value_to(object value)
        {
            var _value = value == null ? "" : value.ToString();
            var already_encoded = new Regex("%[0-9A-F][0-9A-F]", RegexOptions.IgnoreCase);
            try
            {
                while (already_encoded.Match(_value).Success)
                {
                    _value = HttpUtility.UrlDecode(_value);
                }
                _value = HttpUtility.UrlEncode(_value);
                _value = _value.Replace(" ", "%20")
                    .Replace("+", "%20")
                    .Replace("'", "%27")
                    .Replace("/", "%2F")
                    .Replace("#", "%23")
                    .Replace(".", "%2E")
                    .Replace("(", "%28")
                    .Replace(")", "%29")
                    .Replace("-", "%2D");
            }
            catch
            {
                _value = "";
            }
            return _value;
        }
        #endregion value_to
        /// <summary>
        /// This is used when you want to make a value acceptable for MySQL
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string do_value_to(object value)
        {
            var _value = value == null ? "" : value.ToString();
            var already_encoded = new Regex("%[0-9A-F][0-9A-F]", RegexOptions.IgnoreCase);
            while (already_encoded.Match(_value).Success)
            {
                _value = HttpUtility.UrlDecode(_value);
            }

            _value = HttpUtility.UrlEncode(_value);
            _value = _value.Replace(" ", "%20")
                .Replace("+", "%20")
                .Replace("'", "%27")
                .Replace("/", "%2F")
                .Replace("#", "%23")
                .Replace(".", "%2E")
                .Replace("(", "%28")
                .Replace(")", "%29")
                .Replace("-", "%2D");
            return _value;
        }
        public static bool Contains(int to_check, int[] list)
        {
            return list.Contains(to_check);
        }
        public static bool Contains(string to_check, string[] list)
        {
            return list.Contains(to_check);
        }
        #region ConvertToDouble
        public double ConvertToDouble(string value)
        {
            var returnValue = 0.0;
            double.TryParse(value, out returnValue);
            return returnValue;
        }
        public static double do_ConvertToDouble(string value)
        {
            var returnValue = 0.0;
            double.TryParse(value, out returnValue);
            return returnValue;
        }
        #endregion ConvertToDouble
        public static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;
            var UTF8 = new UTF8Encoding();
            var HashProvider = new MD5CryptoServiceProvider();
            var TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));
            var TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            var DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                var Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            catch (Exception ee)
            {
                do_errorLog(ee, Message);
                throw ee;
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }
        public static Dictionary<string, string> dict_create(object obj)
        {
            var result = new Dictionary<string, string>();
            var t = obj.GetType();
            var properties = t.GetProperties().OrderBy(v => v.Name);

            foreach (var property in properties)
            {
                try
                {
                    var value = property.GetValue(obj, null);
                    if (value != null)
                    {
                        var parsedt = new DateTime();
                        if (property.PropertyType == Type.GetType("System.String") && DateTime.TryParse(value.ToString(), out parsedt))
                        {
                            value = MySQL_shortdt(parsedt);
                        }
                        var displayValue = value.ToString();
                        if (value is ArrayList || value is NeBusinessUnit)
                        {

                        }
                        else if(property.Name == "business_unit_id")
                        { 
                            var BUName = new NeBusinessUnit(displayValue); 
                            result.Add(property.Name, " <span style='color:f00;'>" + BUName.ddl_name + "</span>");
                        }
                        else if(property.Name == "hrstatus_id")
                        {
                            var display_hrstatus = "";
                            var new_hrstatus = Convert.ToInt32(displayValue);
                            switch(new_hrstatus)
                            {
                                case HRStatus.ByID.New:
                                    display_hrstatus = HRStatus.ByName.New;
                                    break;
                                case HRStatus.ByID.WaitingforInitialSetup:
                                    display_hrstatus = HRStatus.ByName.WaitingforInitialSetup;
                                    break;                    
                                case HRStatus.ByID.Approved:     
                                    display_hrstatus = HRStatus.ByName.Approved;
                                    break;                    
                                case HRStatus.ByID.PendingClosure:
                                    display_hrstatus = HRStatus.ByName.PendingClosure;
                                    break;                     
                                case HRStatus.ByID.Past:          
                                    display_hrstatus = HRStatus.ByName.Past;
                                    break;                     
                                case HRStatus.ByID.Probation:     
                                    display_hrstatus = HRStatus.ByName.Probation;
                                    break;
                            }
                             result.Add(property.Name, " <span style='color:f00;'>" + display_hrstatus + "</span>");   
                        }
                        else
                        {
                            result.Add(property.Name, " <span style='color:f00;'>" + displayValue + "</span>");
                        }
                    }
                    else
                    {
                        result.Add(property.Name, "null");
                    }
                }
                catch
                {
                }
            }

            return result;
        }
        public static string var_dump(object obj)
        {
            var result = new StringBuilder();
            var t = obj.GetType();
            var properties = t.GetProperties();

            foreach (var property in properties)
            {
                try
                {
                    var value = property.GetValue(obj, null);
                    if (value != null)
                    {
                        var displayValue = value.ToString();
                        if (value is string) displayValue = string.Concat('"', displayValue, '"');
                        if (value is ArrayList || value is NeBusinessUnit)
                        {

                        }
                        else
                        {
                            result.AppendFormat("{0} = {1}\n", property.Name, displayValue);
                        }
                    }
                    else
                    {
                        result.AppendFormat("{0} = {1}\n", property.Name, "null");
                    }
                }
                catch (Exception ee)
                {
                    do_errorLog(ee);
                }
            }

            return result.ToString();
        }
		public struct doubleString
			{
			public double amount;
			public string reason;
			}
        public struct boolstr
        {
            public bool success;
            public string message;
        }
        public static string dict_dump(Dictionary<string, string> obj)
        {
            var result = new StringBuilder();
            result.Append("<br/><hr/>");
            foreach (var e in obj)
            {
                result.AppendFormat("<b>{0}</b>: {1}<br/>", e.Key, e.Value);
            }
            result.Append("<hr/>");
            return result.ToString();
        }
        public static void write_event(string e, int type_id)
        {
            var type = EventLogEntryType.Information;
            switch (type_id)
            {
                case 0:
                    type = EventLogEntryType.Error;
                    break;
                case 1:
                    type = EventLogEntryType.Information;
                    break;
                case 2:
                    type = EventLogEntryType.Warning;
                    break;
            }
            var source = "NESI";
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, "Application");
            }
            EventLog.WriteEntry(source, e, type, 1114);
        }
        public static double do_Round(double number, int digits)
        {
            //if (number == 4.415)
            //{

            //}
            return (Convert.ToDouble(Math.Round((decimal)number, digits, MidpointRounding.AwayFromZero)));

        }
        public double Round(double number, int digits)
        {
            //if (number == 4.415)
            //{

            //}
            return (Convert.ToDouble(Math.Round((decimal)number, digits, MidpointRounding.AwayFromZero)));

        }
        public static void QuickReponse(HttpResponse resp, string msg)
        {
            resp.Clear();
            resp.ContentType = "text/plain";
            resp.Write(msg);
            resp.Flush();
            resp.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        /// <summary>
        /// This is a friendly exception, no code, just the error message and a way to leave the page
        /// </summary>
        /// <param name="resp">This should always just be the current Response object, as this is static, I don't fully know how to access the Response object from a static.</param>
        /// <param name="Message">The friendly error message</param>
        /// <param name="url">Where you want the user to go after hitting this error.</param>
        public static void FriendlyException(HttpResponse resp, string Message, string url)
        {
            resp.Clear();
            var backbutton = "";
            if (!string.IsNullOrWhiteSpace(url) && url.Contains("/"))
            {
                // url
                backbutton = string.Format(@"
				<tr>
					<td align='center'><button type='button' onclick=""location.href='{0}';"">Return To Previous Page</td>
				</tr>", url);
            }
            else if (url.Contains("("))
            {
                // javascript
                backbutton = string.Format(@"
				<tr>
					<td align='center'><button type='button' onclick=""{0}"">Return To Previous Page</td>
				</tr>", url);
            }
            resp.Write(string.Format(@"
<html>
	<head>
		<title>An Error Has Occurred.</title>
		<style>
			.error_box			{{
								font-size:		12px;
								font-family:	arial;
								width:			400px;
								border:			solid 2px #00964D;
								border-radius:	10px;
								}}
			.error_box .title	{{
								background-color:	#00964D;
								color:				#fff;
								padding:			5px;
								font-size:			15px;
								font-weight:		bold;
								}}
			.error_box .body	{{
								padding:			50px;
								}}
		</style>
	</head>
	<body>
		<center>
			<a href='/' alt='home' title='Go home'><img src='{1}' width='250'  border='0' style='margin:50px;'/></a>
			<table cellpadding='0' cellspacing='0' class='error_box'>
				<tr>
					<td align='center' class='title'>An error has occurred!</td>
				</tr>
				<tr>
					<td align='center' class='body'>{0}</td>
				</tr>
{2}
			</table>
		</center>
	</body>
</html>
", Message, OpsBase64Image.SparkOpsLogo, backbutton));
            resp.Flush();
            try
            {
                resp.End();
            }
            catch
            { }
        }
        public static void FriendlyPopup(HttpResponse resp, string Message, string url, string title)
        {
            resp.Clear();
            var backbutton = "";
            if (!string.IsNullOrWhiteSpace(url) && url.Contains("/"))
            {
                // url
                backbutton = string.Format(@"
				<tr>
					<td align='center'><button type='button' onclick=""location.href='{0}';"">Return To Previous Page</td>
				</tr>", url);
            }
            else if (url.Contains("("))
            {
                // javascript
                backbutton = string.Format(@"
				<tr>
					<td align='center'><button type='button' onclick=""{0}"">Return To Previous Page</td>
				</tr>", url);
            }
            resp.Write(string.Format(@"
<html>
	<head>
		<title>Message</title>
		<style>
			.error_box			{{
								font-size:		12px;
								font-family:	arial;
								width:			400px;
								border:			solid 2px #047;
								border-radius:	10px;
								}}
			.error_box .title	{{
								background-color:	#047;
								color:				#fff;
								padding:			5px;
								font-size:			15px;
								font-weight:		bold;
								}}
			.error_box .body	{{
								padding:			50px;
								}}
		</style>
	</head>
	<body>
		<center>
			<a href='/' alt='home' title='Go home'><img src='/images/Logos/{4}' width='165'  border='0' style='margin:10px;'/></a>
			<table cellpadding='0' cellspacing='0' class='error_box'>
				<tr>
					<td align='center' class='title'>{3}</td>
				</tr>
				<tr>
					<td align='center' class='body'>{0}</td>
				</tr>
{2}
			</table>
		</center>
	</body>
</html>
", Message, url, backbutton, title, new NeMember(get_currentMemberId()).business_unit.logo_file));

            resp.Flush();
            try
            {
                resp.End();
            }
            catch
            { }
        }
        public static string fun_time(DateTime d)
        {
            if (d.Year < 1900)
            {
                return "";
            }
            // 1.
            // Get time span elapsed since the date.
            var s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            var dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            var secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0)
            {
                return "";
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "just now";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "1 minute ago";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} minutes ago",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 hour ago";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} hours ago",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "yesterday";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} days ago",
                    dayDiff);
            }
            // 7.
            // Handle previous weeks.
            if (dayDiff < 31)
            {
                return string.Format("{0} weeks ago",
                    Math.Ceiling((double)dayDiff / 7));
            }
            // 8.
            // Handle previous months.
            if (dayDiff > 31 && dayDiff < 365)
            {
                return string.Format("{0} months ago",
                    Math.Ceiling((double)dayDiff / 30));
            }
            // 9.
            // Handle previous years.
            if (dayDiff > 365)
            {
                return string.Format("{0} years ago",
                    Math.Ceiling((double)dayDiff / 365));
            }
            return "";
        }
        public static string DecryptString(string Message, string Passphrase)
        {
            var reBase64Chk = new Regex("^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$");
            if (!reBase64Chk.IsMatch(Message)) return Message;
            byte[] Results = { };
            var UTF8 = new UTF8Encoding();
            var HashProvider = new MD5CryptoServiceProvider();
            var TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));
            var TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            var DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                var Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            catch (Exception ee)
            {
                //Not a real error here
                // do_errorLog(ee, Message);
                //_tools.catch_error(ee);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            if (Results != null)
            {
                return UTF8.GetString(Results);
            }
            else
            {
                return "";
            }
        }

        public static string getHostName()
        {

            var host = "";
            if (HttpContext.Current != null)
            {
                var u = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                if (HttpContext.Current.Request.IsLocal)
                {
                    host = "localhost:" + u.Port;
                }
                else if (HttpContext.Current != null)
                {
                    host = u.Host;
                }
            }
            return host;
        }

        public static bool isNeCell(string number)
        {
            //strip - and space
            number = Regex.Replace(number, "[^0-9]+", string.Empty);
            if (!string.IsNullOrWhiteSpace(number) && number.Length >= 10)
            {
                if (number.Substring(0, 1).Equals("1"))
                    number = number.Substring(1);
            }
            else
            {
                return false;
            }

            var areaCode = number.Substring(0, 3);
            var phoneFirst = number.Substring(3, 3);
            var phoneLast = number.Substring(6, 4);

            var i = doSQL_int(@"select count(Member_AreaCode) from member
                                           where Member_AreaCode = @v0
AND Member_PhoneFirst = @v1 and Member_PhoneLast = @v2",
new object[] {
                areaCode, phoneFirst, phoneLast});
            if (i >= 1)
            {
                return true;
            }
            else
            {
                var j = doSQL_int(@"select count(id) from cellphone_number
                                                              where number = @v0",
                                                              new object[] {
                                                              areaCode + "-" + phoneFirst + "-" + phoneLast});
                if (j >= 1)
                {
                    return true;
                }
            }



            return false;
        }


        /**
    * toNum == fromNum
    * toNum.isCell && fromNum.isCell
    * toNum.isCell && fromNum.isExt
    * fromNum.isCell && toNum.isExt
    * toNum.isExt && fromNum.isExt
    * 
    **/
        public static bool isInternalCall(string toNum, string fromNum)
        {
            var toCell = isNeCell(toNum);
            var toExt = toNum.Length == 3;
            var fromCell = isNeCell(fromNum);
            var fromExt = fromNum.Length == 3;

            if (toNum == fromNum)
                return true;
            if (toCell && fromCell)
                return true;
            if (toCell && fromExt)
                return true;
            if (fromCell && toExt)
                return true;
            if (toExt && fromExt)
                return true;


            return false;
        }



        public static string escapeString(string x, bool escapeDoubleQuotes = true)
        {
            StringBuilder sBuilder = new StringBuilder(x.Length * 11 / 10);

            int stringLength = x.Length;

            for (int i = 0; i < stringLength; ++i)
            {
                char c = x[i];

                switch (c)
                {
                    case '\n': /* Must be escaped for logs */
                        sBuilder.Append('\\');
                        sBuilder.Append('n');

                        break;

                    case '\r':
                        sBuilder.Append('\\');
                        sBuilder.Append('r');

                        break;

                    case '\\':
                        sBuilder.Append('\\');
                        sBuilder.Append('\\');

                        break;

                    case '\'':
                        sBuilder.Append('\\');
                        sBuilder.Append('\'');

                        break;

                    case '"': /* Better safe than sorry */
                        if (escapeDoubleQuotes)
                        {
                            sBuilder.Append('\\');
                        }

                        sBuilder.Append('"');

                        break;

                    default:
                        sBuilder.Append(c);
                        break;
                }
            }

            return sBuilder.ToString();
        }

    }
}