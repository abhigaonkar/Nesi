<%@ WebService Language="C#" Class="errorMessageData" %>

using System;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nesi.core;

public class ErrorData
{
    public string memberID;
    public string errorDesc;
    public string LastName;
    public string NeEmail;
    public string CompanyName;
    public string PhoneNumber;
    public string criticalTickets;
    public string randomTicket;
    public string errorCount;
}

public class ErrorLogData
{
    public string errorDesc;
    public string host;
    public string member;
    public string errorsToday;
    public string criticalTickets;
    public string randomTicket;
    public string errorCount;
    public string isGlobal;
}
public class serverUsage
{

    public string server;
    public double errorDesc;
    public double host;
    public string time;
}
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class errorMessageData  : System.Web.Services.WebService {


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getInfo()
    {
        var data = new
        {
            name = Toolbox.doSQL_string(@"select jumble from matt 
order by dt DESC
limit 1"),
            dt = Toolbox.doSQL_string(@"select dt from matt 
order by dt DESC
limit 1")
        };

        // We are using an anonymous object above, but we could use a typed one too (SayHello class is defined below)
        // SayHello data = new SayHello { Greeting = "Hello", Name = firstName + " " + lastName };

        var js = new System.Web.Script.Serialization.JavaScriptSerializer();

        return js.Serialize(data);
    }

    [WebMethod]
    public List<ErrorData> getLastError()
    {
        var cont = new List<ErrorData>();



        var dt_old = Toolbox.doSQL_dt(@"SELECT  dt, e.member_id, m.member_fullname, error_desc, error_short, host_url, full_stacktrace
                                            FROM neintranet.error_log e
                                            left OUTER join member m 
                                            ON m.member_id = e.member_id
                                            where host_url != 'localhost' AND host_url != 'andy.nesi.ca' AND host_url != 'jordan.nesi.ca' AND host_url != 'matt.nesi.ca' AND host_url != 'iain.nesi.ca' AND host_url != ''
                                            order by dt DESC limit 5",null);


        var dt = Toolbox.doSQL_dt(@"SELECT      e.error_timestamp as dt, e.member_id, m.member_fullname, mes.message_text as error_desc,  e.error_query as error_short ,
		h.host_name as host_url,e.error_stackTrace as full_stacktrace,

   l.line_number, mn.machine_name, 
o.origin_path, oq.queryString_string, u.userAgent_string, ip.userIP_address, e.error_is_global, e.error_is_local
FROM error_log.error e
left join neintranet.member m 
ON m.member_id = e.member_id
left join error_log.error_host h 
ON h.host_id = e.error_host_id
left join error_log.error_line l 
ON l.line_id = e.error_line_id
left join error_log.error_machine_name mn
ON mn.machine_id = e.error_machine_id
left join error_log.error_messages mes
ON mes.message_id = e.error_message_id
left join error_log.error_origin o
ON o.origin_id = e.error_origin_id
left join error_log.error_origin_queryString oq
ON oq.queryString_id = e.error_origin_queryString_id
left join error_log.error_useragent u
ON u.useragent_id = e.error_useragent_id
left join error_log.error_userip ip
ON ip.userip_id = e.error_userip_id",null);





        //  DataRow dr = dt.Rows[0];


        foreach (DataRow dr in dt.Rows)
        {
            var c = new ErrorData();
            c.errorDesc = dr["Error_Desc"].ToString();




            if (c.errorDesc.Contains(" at "))
                c.errorDesc = c.errorDesc.Substring(0, c.errorDesc.IndexOf(" at "));
            var query = dr["error_short"].ToString();
            if (query.Contains("bad query"))
            {
                if (query.Length > 500)
                {
                    query = query.Substring(0, 400);
                }
                c.errorDesc += "<br>" + "<br>" + query;

            }

            var errors = dr["full_stacktrace"].ToString();
            var printError = "";

            var inst2 = new List<int>();
            var indexs = 0;
            while (indexs >= 0)
            {
                if (errors.Length >= 5)
                {
                    indexs = errors.IndexOf(":line ", indexs + 1);
                    if (indexs >= 0)
                        inst2.Add(indexs);
                }
                else
                {
                    indexs = -1;
                }
            }
            var first = true;
            foreach (var value in inst2)
            {
                if (value == -1)
                {
                    break;
                }
                var index = 0;
                while (index >= 0)
                {
                    index = errors.IndexOf("c:\\inetpub", index + 1);
                    if (index > value || first)
                    {
                        first = false;
                        var end = errors.IndexOf("at ", index + 1);
                        printError += "<br>";
                        if (end == -1)
                        {
                            printError += errors.Substring(index);
                        }
                        else
                        {
                            printError += errors.Substring(index, end - index);
                        }
                        break;
                    }

                }



            }
            errors = errors.Replace("at ", "\nat ");

            var lines = errors.Split(new string[] { "at "}, StringSplitOptions.None);
            var useLine = "";

            foreach (var str in lines)
            {
                if (str.Contains(":line "))
                {
                    if (!str.Contains("Toolbox.catch_error") && !str.Contains("Toolbox.do_errorLog") &&
                        !str.Contains("Toolbox.get_stackTrace") && !str.Contains("c:\\Windows\\Microsoft.NET"))
                    {
                        var b1 = str.IndexOf("(", 1);
                        var b2 = str.LastIndexOf(")");

                        var remove = str.Substring(b1, (b2 - b1 + 1));

                        var newStr = str.Replace(remove, "");

                        var firstIn = newStr.IndexOf(" in ");
                        var firstDot = newStr.IndexOf(".");

                        var toBack = newStr.Substring(firstDot + 1, firstIn - firstDot-1) + "()";
                        newStr = newStr.Substring(firstIn + 4);
                        newStr += " - "+  toBack;

                        useLine += newStr + "<br>";
                    }
                }
            }

            //c.PhoneExtension = "<br>"+ useLine;



            c.errorDesc += "<br><br>" + useLine;
            c.errorDesc = c.errorDesc.Replace(" at", "<br> at");

            var host = dr["host_url"].ToString();

            /*Toolbox.do_string(@"SELECT host_url FROM neintranet.error_log 
                                                where host_url != 'localhost' AND host_url != 'jordan.nesi.ca' AND host_url != 'andy.nesi.ca' AND 
                                                      host_url != 'matt.nesi.ca' AND host_url != 'iain.nesi.ca'
                                            order by dt desc limit 1");*/

            if (host.Equals("alpha.nesi.ca"))
            {
                host = "nesi.ca";
            }
            if (printError.Contains("AndroidAppService") || printError.Contains("callRecord.aspx.cs"))
            {
                host += "<strong>&emsp;&emsp;<mark>Android App</mark></strong>";
            }

            var member = dr["member_fullname"].ToString();

            if (member.Equals("0") || member.Equals(""))
            {
                c.LastName = "Date: </strong>" + dr["dt"] + "<strong>&emsp;Host: </strong>" + host + "<strong>";
            }
            else
            {
                c.LastName = "Date: </strong>" + dr["dt"] + "<strong>&emsp;Host: </strong>" + host + "<strong>&emsp;Member:</strong> " + member + "<strong> (" + dr["member_id"] + ")";
            }

            var tErrors = Toolbox.doSQL_int(@"select count(dt) from error_log
                                                        where dt > CURDATE() AND
                                                        host_url != 'localhost' AND host_url != 'matt.nesi.ca' AND host_url != 'iain.nesi.ca' AND host_url != 'jordan.nesi.ca' AND host_url != 'andy.nesi.ca' AND host_url != ''");

            c.PhoneNumber = tErrors + "";


            var stopwatch = new Stopwatch();
            stopwatch.Start();
            // NeMember m4 = new NeMember(8);
            stopwatch.Stop();
            c.NeEmail = "" + (stopwatch.Elapsed.TotalMilliseconds);

            var stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            //NeWOProg p = new NeWOProg(88085);
            //NECustomer nec = new NECustomer(3758L);
            stopwatch2.Stop();
            c.CompanyName = "" + (stopwatch2.Elapsed.TotalMilliseconds);

            var ct = Toolbox.doSQL_int(@"SELECT count(*) FROM neintranet.ticketheader
where ticketheader_priority_id = 2
AND ticketheader_status_id != 5
AND ticketheader_modified_date < (NOW() - interval 3 day)");

            if (ct == 0)
            {
                c.criticalTickets = "<span  style='color:#000000; background:#00FF00; padding: 5px 5px 5px 5px;'> NONE! <img height='100' width='100' src='http://i241.photobucket.com/albums/ff165/SunWers/Congratulations/congratulations.jpg'/> </span>";
            }
            else
            {

                if (ct >= 5)
                {
                    c.criticalTickets = "<span  style='color:#000000; background:#FF2B2B; padding: 5px 5px 5px 5px;'> " + ct + " </span>";
                }
                else
                {
                    c.criticalTickets = ct.ToString();
                }


            }

            var rt = Toolbox.doSQL_int(@"SELECT ticketheader_id FROM neintranet.ticketheader
where ticketheader_priority_id = 2
AND ticketheader_status_id != 5
AND ticketheader_modified_date < (NOW() - interval 3 day)
ORDER BY RAND()
LIMIT 1");

            c.randomTicket = rt.ToString();
            cont.Add(c);
        }

        cont.Reverse();
        return cont;
    }

    public object getCPUCounter()
    {

        var cpuCounter = new PerformanceCounter();
        cpuCounter.CategoryName = "Processor";
        cpuCounter.CounterName = "% Processor Time";
        cpuCounter.InstanceName = "_Total";

        // will always start at 0
        dynamic firstValue = cpuCounter.NextValue();
        System.Threading.Thread.Sleep(1000);

        // now matches task manager reading
        dynamic secondValue = cpuCounter.NextValue();

        return secondValue;

    }


    [WebMethod]
    public List<ErrorData> getLastError_new()
    {
        var cont = new List<ErrorData>();




        var dt = Toolbox.doSQL_dt(@"


SELECT  MAX(e.main_id), e.member_id, m.member_fullname, MAX(e.error_timestamp) as dt, e.error_query, e.error_stackTrace, h.host_name, l.line_number, mn.machine_name, 
mes.message_text, o.origin_path, oq.queryString_string, u.userAgent_string, ip.userIP_address,

 (
    SELECT COUNT(*)
    FROM error_log.error
    WHERE error_message_id = e.error_message_id AND error_origin_id = e.error_origin_id
  ) AS error_count
  
FROM error_log.error e
left join neintranet.member m 
ON m.member_id = e.member_id
left join error_log.error_host h 
ON h.host_id = e.error_host_id
left join error_log.error_line l 
ON l.line_id = e.error_line_id
left join error_log.error_machine_name mn
ON mn.machine_id = e.error_machine_id
left join error_log.error_messages mes
ON mes.message_id = e.error_message_id
left join error_log.error_origin o
ON o.origin_id = e.error_origin_id
left join error_log.error_origin_queryString oq
ON oq.queryString_id = e.error_origin_queryString_id
left join error_log.error_useragent u
ON u.useragent_id = e.error_useragent_id
left join error_log.error_userip ip
ON ip.userip_id = e.error_userip_id

where e.error_is_local = false
AND message_text != 'LongError'
AND e.error_timestamp > CURDATE()


group by e.error_message_id,  e.error_origin_id
Order by MAX(e.error_timestamp) desc
LIMIT 7

",null);





        //  DataRow dr = dt.Rows[0];


        foreach (DataRow dr in dt.Rows)
        {
            var c = new ErrorData();
            c.errorDesc = dr["message_text"] + " <br><strong>Page:</strong>  " + dr["origin_path"];




            if (c.errorDesc.Contains(" at "))
                c.errorDesc = c.errorDesc.Substring(0, c.errorDesc.IndexOf(" at "));
            var query = dr["error_query"].ToString();
            if (query.Contains("bad query"))
            {
                if (query.Length > 500)
                {
                    query = query.Substring(0, 400);
                }
                c.errorDesc += "<br>" + "<br>" + query;

            }

            var errors = dr["error_stackTrace"].ToString();
            var printError = "";

            var inst2 = new List<int>();
            var indexs = 0;
            while (indexs >= 0)
            {
                if (errors.IndexOf(":line ") != -1)
                {
                    indexs = errors.IndexOf(":line ", indexs + 1);
                    inst2.Add(indexs);
                }
                else
                {
                    indexs = -1;
                }

            }
            var first = true;
            foreach (var value in inst2)
            {
                if (value == -1)
                {
                    break;
                }
                var index = 0;
                while (index >= 0)
                {
                    index = errors.IndexOf("c:\\inetpub", index + 1);
                    if (index > value || first)
                    {
                        first = false;
                        var end = errors.IndexOf("at ", index + 1);
                        printError += "<br>";
                        if (end == -1)
                        {
                            printError += errors.Substring(index);
                        }
                        else
                        {
                            printError += errors.Substring(index, end - index);
                        }
                        break;
                    }

                }



            }
            errors = errors.Replace("at ", "\nat ");

            var lines = errors.Split(new string[] { "at " }, StringSplitOptions.None);
            var useLine = "";

            foreach (var str in lines)
            {
                if (str.Contains(":line "))
                {
                    if (!str.Contains("Toolbox.catch_error") && !str.Contains("Toolbox.do_errorLog") &&
                        !str.Contains("Toolbox.get_stackTrace") && !str.Contains("c:\\Windows\\Microsoft.NET"))
                    {
                        var b1 = str.IndexOf("(", 1);
                        var b2 = str.LastIndexOf(")");

                        var remove = str.Substring(b1, (b2 - b1 + 1));

                        var newStr = str.Replace(remove, "");

                        var firstIn = newStr.IndexOf(" in ");
                        var firstDot = newStr.IndexOf(".");

                        var toBack = newStr.Substring(firstDot + 1, firstIn - firstDot - 1) + "()";
                        newStr = newStr.Substring(firstIn + 4);
                        newStr += " - " + toBack;

                        useLine += newStr + "<br>";
                    }
                }
            }

            //c.PhoneExtension = "<br>"+ useLine;



            c.errorDesc += "<br><br>" + useLine;
            c.errorDesc = c.errorDesc.Replace(" at", "<br> at");

            var host = dr["host_name"].ToString();

            /*Toolbox.do_string(@"SELECT host_url FROM neintranet.error_log 
                                                where host_url != 'localhost' AND host_url != 'jordan.nesi.ca' AND host_url != 'andy.nesi.ca' AND 
                                                      host_url != 'matt.nesi.ca' AND host_url != 'iain.nesi.ca'
                                            order by dt desc limit 1");*/

            if (host.Equals("alpha.nesi.ca"))
            {
                host = "nesi.ca";
            }
            if (printError.Contains("AndroidAppService") || printError.Contains("callRecord.aspx.cs"))
            {
                host += "<strong>&emsp;&emsp;<mark>Android App</mark></strong>";
            }

            var member = dr["member_fullname"].ToString();

            if (member.Equals("0") || member.Equals(""))
            {
                c.LastName = "Date: </strong>" + dr["dt"] + "<strong>&emsp;Host: </strong>" + host + "<strong>";
            }
            else
            {
                c.LastName = "Date: </strong>" + dr["dt"] + "<strong>&emsp;Host: </strong>" + host + "<strong>&emsp;Member:</strong> " + member + "<strong> (" + dr["member_id"] + ")";
            }

            var tErrors = Toolbox.doSQL_int(@"select count(e.error_timestamp) from error_log.error e
                            left join error_log.error_host h 
                            ON h.host_id = e.error_host_id
	                        where e.error_timestamp > CURDATE() AND
	                        h.host_name != 'localhost' AND h.host_name != 'matt.nesi.ca' AND h.host_name != 'iain.nesi.ca' AND
                            h.host_name != 'jordan.nesi.ca' AND h.host_name != 'andy.nesi.ca' AND h.host_name != ''");

            if (tErrors >= 200)
            {
                c.PhoneNumber = "<span  style='color:#000000; background:#FF2B2B; padding: 5px 5px 5px 5px;'>" + tErrors + "</span>";
            }
            else
            {
                c.PhoneNumber = tErrors.ToString();
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            // NeMember m4 = new NeMember(8);
            stopwatch.Stop();
            c.NeEmail = "" + (stopwatch.Elapsed.TotalMilliseconds);

            var stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            //NeWOProg p = new NeWOProg(88085);
            //NECustomer nec = new NECustomer(3758L);
            stopwatch2.Stop();
            c.CompanyName = "" + (stopwatch2.Elapsed.TotalMilliseconds);

            var ct = Toolbox.doSQL_int(@"SELECT count(*) FROM neintranet.ticketheader
where ticketheader_priority_id = 2
AND ticketheader_status_id != 5
AND ticketheader_modified_date < (NOW() - interval 3 day)");

            if (ct == 0)
            {
                c.criticalTickets = "<span  style='color:#000000; background:#00FF00; padding: 5px 5px 5px 5px;'> NONE! <img height='100' width='100' src='http://i241.photobucket.com/albums/ff165/SunWers/Congratulations/congratulations.jpg'/> </span>";
            }
            else
            {

                if (ct >= 5)
                {
                    c.criticalTickets = "<span  style='color:#000000; background:#FF2B2B; padding: 5px 5px 5px 5px;'> " + ct + " </span>";
                }
                else
                {
                    c.criticalTickets = ct.ToString();
                }


            }

            var rt = Toolbox.doSQL_int(@"SELECT ticketheader_id FROM neintranet.ticketheader
where ticketheader_priority_id = 2
AND ticketheader_status_id != 5
AND ticketheader_modified_date < (NOW() - interval 3 day)
ORDER BY RAND()
LIMIT 1");

            c.randomTicket = rt.ToString();




            c.errorCount = dr["error_count"].ToString();

            cont.Add(c);
        }

        //  cont.Reverse();
        return cont;
    }




    [WebMethod]
    public List<ErrorLogData> getMostError()
    {

        var cont = new List<ErrorLogData>();




        var dt = Toolbox.doSQL_dt(@"

SELECT  MAX(e.main_id), e.member_id, m.member_fullname, MAX(e.error_timestamp) as dt, e.error_query, e.error_stackTrace, h.host_name, l.line_number, mn.machine_name, 
mes.message_text, o.origin_path, oq.queryString_string, u.userAgent_string, ip.userIP_address,e.error_is_global,

 (
    SELECT COUNT(*)
    FROM error_log.error
    WHERE error_message_id = e.error_message_id
  ) AS error_count
  
FROM error_log.error e
left join neintranet.member m 
ON m.member_id = e.member_id
left join error_log.error_host h 
ON h.host_id = e.error_host_id
left join error_log.error_line l 
ON l.line_id = e.error_line_id
left join error_log.error_machine_name mn
ON mn.machine_id = e.error_machine_id
left join error_log.error_messages mes
ON mes.message_id = e.error_message_id
left join error_log.error_origin o
ON o.origin_id = e.error_origin_id
left join error_log.error_origin_queryString oq
ON oq.queryString_id = e.error_origin_queryString_id
left join error_log.error_useragent u
ON u.useragent_id = e.error_useragent_id
left join error_log.error_userip ip
ON ip.userip_id = e.error_userip_id

where e.error_is_local = false
AND message_text != 'LongError'
AND e.error_timestamp > CURDATE() 
AND h.host_name != 'jordan.nesi.ca'
AND h.host_name != 'andy.nesi.ca'
AND h.host_name != 'matt.nesi.ca'
AND h.host_name != 'iain.nesi.ca'
AND h.host_name != ''

group by (e.error_message_id)
Order by error_count desc
limit 6
",null);





        //  DataRow dr = dt.Rows[0];


        foreach (DataRow dr in dt.Rows)
        {
            var c = new ErrorLogData();
            c.errorDesc = dr["message_text"].ToString();




            if (c.errorDesc.Contains(" at "))
                c.errorDesc = c.errorDesc.Substring(0, c.errorDesc.IndexOf(" at "));
            var query = dr["error_query"].ToString();
            if (query.Contains("bad query"))
            {
                if (query.Length > 500)
                {
                    query = query.Substring(0, 400);
                }
                c.errorDesc += "<br>" + "<br>" + query;

            }

            var errors = dr["error_stackTrace"].ToString();
            var printError = "";

            var inst2 = new List<int>();
            var indexs = -1;
            while (indexs >= 0)
            {
                indexs = errors.IndexOf(":line ", indexs + 1);
                inst2.Add(indexs);
            }
            var first = true;
            foreach (var value in inst2)
            {
                if (value == -1)
                {
                    break;
                }
                var index = 0;
                while (index >= 0)
                {
                    index = errors.IndexOf("c:\\inetpub", index + 1);
                    if (index > value || first)
                    {
                        first = false;
                        var end = errors.IndexOf("at ", index + 1);
                        printError += "<br>";
                        if (end == -1)
                        {
                            printError += errors.Substring(index);
                        }
                        else
                        {
                            printError += errors.Substring(index, end - index);
                        }
                        break;
                    }

                }

            }
            errors = errors.Replace("at ", "\nat ");

            var lines = errors.Split(new string[] { "at " }, StringSplitOptions.None);
            var useLine = "";

            foreach (var str in lines)
            {
                if (str.Contains(":line "))
                {
                    if (!str.Contains("Toolbox.catch_error") && !str.Contains("Toolbox.do_errorLog") &&
                        !str.Contains("Toolbox.get_stackTrace") && !str.Contains("c:\\Windows\\Microsoft.NET"))
                    {
                        var b1 = str.IndexOf("(", 1);
                        var b2 = str.LastIndexOf(")");

                        var remove = str.Substring(b1, (b2 - b1 + 1));

                        var newStr = str.Replace(remove, "");

                        var firstIn = newStr.IndexOf(" in ");
                        var firstDot = newStr.IndexOf(".");

                        var toBack = newStr.Substring(firstDot + 1, firstIn - firstDot - 1) + "()";
                        newStr = newStr.Substring(firstIn + 4);
                        newStr += " - " + toBack;

                        useLine += newStr + "<br>";
                    }
                }
            }

            //c.PhoneExtension = "<br>"+ useLine;



            c.errorDesc += "<br><br>" + useLine;
            c.errorDesc = c.errorDesc.Replace(" at", "<br> at");

            var host = dr["host_name"].ToString();

            /*Toolbox.do_string(@"SELECT host_url FROM neintranet.error_log 
                                                where host_url != 'localhost' AND host_url != 'jordan.nesi.ca' AND host_url != 'andy.nesi.ca' AND 
                                                      host_url != 'matt.nesi.ca' AND host_url != 'iain.nesi.ca'
                                            order by dt desc limit 1");*/

            if (host.Equals("alpha.nesi.ca"))
            {
                c.host = "nesi.ca";
            }
            else
            {
                c.host = host;
            }

            var member = dr["member_fullname"].ToString();

            if (member.Equals("0") || member.Equals(""))
            {


                c.member = "unknown";
            }
            else
            {
                c.member = member + "<strong> (" + dr["member_id"] + ")</strong>";
            }

            var tErrors = Toolbox.doSQL_int(@"select count(e.error_timestamp) from error_log.error e
                            left join error_log.error_host h 
                            ON h.host_id = e.error_host_id
	                        where e.error_timestamp > CURDATE() AND
	                        h.host_name != 'localhost' AND h.host_name != 'matt.nesi.ca' AND h.host_name != 'iain.nesi.ca' AND
                            h.host_name != 'jordan.nesi.ca' AND h.host_name != 'andy.nesi.ca' AND h.host_name != ''");

            if (tErrors >= 200)
            {
                c.errorsToday = "<span  style='color:#000000; background:#FF2B2B; padding: 5px 5px 5px 5px;'>Byte overflow</span>";
            }
            else
            {
                c.errorsToday = tErrors.ToString();
            }

            var ct = Toolbox.doSQL_int(@"SELECT count(*) FROM neintranet.ticketheader
where ticketheader_priority_id = 2
AND ticketheader_status_id != 5
AND ticketheader_modified_date < (NOW() - interval 3 day)");

            if (ct == 0)
            {
                c.criticalTickets = "<span  style='color:#000000; background:#00FF00; padding: 5px 5px 5px 5px;'> NONE! <img height='100' width='100' src='http://i241.photobucket.com/albums/ff165/SunWers/Congratulations/congratulations.jpg'/> </span>";
            }
            else
            {

                if (ct >= 5)
                {
                    c.criticalTickets = "<span  style='color:#000000; background:#FF2B2B; padding: 5px 5px 5px 5px;'> " + ct + " </span>";
                }
                else
                {
                    c.criticalTickets = ct.ToString();
                }


            }

            var rt = Toolbox.doSQL_int(@"SELECT ticketheader_id FROM neintranet.ticketheader
where ticketheader_priority_id = 2
AND ticketheader_status_id != 5
AND ticketheader_modified_date < (NOW() - interval 3 day)
ORDER BY RAND()
LIMIT 1");

            c.randomTicket = rt.ToString();




            c.errorCount = dr["error_count"].ToString();
            c.isGlobal = dr["error_is_global"].ToString();
            cont.Add(c);
        }

        //cont.Reverse();
        return cont;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]//Specify return format.

    public string getUsage()
    {

        var dt = Toolbox.doSQL_dt(@"SELECT server_id, cpu_load, ram_load, id as dt FROM it.server_usage_log
where server_id = 2
order by dt desc
limit 15",null);

        // We are using an anonymous object above, but we could use a typed one too (SayHello class is defined below)
        // SayHello data = new SayHello { Greeting = "Hello", Name = firstName + " " + lastName };

        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        var rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return "{ \"Servers\": {\"AZ\": " + serializer.Serialize(rows) + "} } ";
    }
}