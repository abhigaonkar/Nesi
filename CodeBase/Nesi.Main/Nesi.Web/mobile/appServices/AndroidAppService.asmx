<%@ WebService Language="C#" Class="AndroidAppService" %>

using System;
using System.Web.Services;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data;
using nesi.core;
using Newtonsoft.Json.Linq;

//Class hold the contact information
public class ContactApp
{
    public string memberID;
    public string FirstName;
    public string LastName;
    public string NeEmail;
    public string CompanyName;
    public string PhoneNumber;
    public string PhoneExtension;
}

public class Mobile_CustomerList
{
    public string customer_id;
    public string customer_name;
}

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class AndroidAppService : WebService
{


    string passphrase = Toolbox.config_setting("mobile_auth_key"); //Should get thisfrom ToolBox


    private List<ContactApp> getContacts(string member_user)
    {
        var contacts = new List<ContactApp>();
        member_user = Toolbox.DecryptString(member_user, passphrase);
        var member_id = Toolbox.doSQL_string("select member_id from member where member_user = @v0", member_user);
        var visibleBusinessUnits = Toolbox.doSQL_string("select get_visible_business_units_group_concat(@v0)", member_id);
        var backofficeDT = Toolbox.doSQL_dt(@"SELECT id FROM business_unit WHERE is_backoffice AND active = 'T' AND istest = 'F'", null);
        foreach(DataRow b in backofficeDT.Rows)
            {
            visibleBusinessUnits += "," + b["id"];
            }
        var dt = Toolbox.doSQL_dt(@" 
SELECT
    m.Member_ID,
    m.member_neemail,
    m.member_fullName,
    c.number,
    comp.ddl_name NAME,
    m.Member_PhoneExtension
FROM
    member m
    LEFT OUTER JOIN cellphone_number c
        ON m.cellphone_number_id = c.id
    LEFT OUTER JOIN business_unit comp
        ON comp.id = m.business_unit_id
WHERE m.Member_Status = 'Active'
    AND FIND_IN_SET(comp.id, @v0)
    AND comp.IsTest = 'F'
    AND include_in_mobile_contactlist = 1
    AND (
        m.member_hrstatus_id = 3
        OR m.member_hrstatus_id = 6
        OR m.member_hrstatus_id = 2
        OR m.member_hrstatus_id = 1
    )
ORDER BY m.Member_FirstName",new object[] {visibleBusinessUnits + ",11"});


        foreach (DataRow dr in dt.Rows)
        {

            var c = new ContactApp();
            c.memberID = dr["Member_ID"].ToString();
            var temp = dr["member_fullName"].ToString();
            var tempInt = temp.IndexOf(" ");
            c.FirstName = temp.Substring(0, tempInt);
            c.LastName = temp.Substring(tempInt);

            c.NeEmail = dr["member_neemail"].ToString();

            if (c.NeEmail == "nomail@thatsnew.com" || c.NeEmail == "nomail@newelectric.com")
            {
                c.NeEmail = "";
            }

            c.PhoneNumber = dr["number"].ToString();
            if (!string.IsNullOrWhiteSpace(c.PhoneNumber))
                c.PhoneNumber = "1" + c.PhoneNumber;
            c.CompanyName = dr["name"].ToString();
            if (dr["Member_PhoneExtension"].ToString() != "")
            {
                c.PhoneExtension = "ext: " + (dr["Member_PhoneExtension"]);
            }
            else
            {
                c.PhoneExtension = "";
            }
            contacts.Add(c);

        }

        return contacts;
    }

    [WebMethod]
    public List<ContactApp> GetContactsWithLogin(string user, string pass)
    {


        //this will hold the plant text version of the password
        //string plainTextPass = Toolbox.DecryptString(pass, passphrase);
        //encrypt the password so it can be checked against the database
        //string encrypted_pass_app = Toolbox.EncryptString(plainTextPass, Toolbox.config_setting("encryption_pass"));
        //get the encrypted password from the tatabase
        //string encrypted_pass_db = getPass(Toolbox.DecryptString(user, passphrase));

        //if the passwords match return the contacts else return blank arraylist.

        if (isValidUser(user, pass))
        {
            return getContacts(user); ;
        }

        //return a blank list will tell the app to wipe all contacts off the phone. 

        return new List<ContactApp>();
    }

    [WebMethod]
    public List<string> isInternalPhoneNumber(string number, string user)
    {
        if (user == null || user.Equals(""))
        {
            return new List<string>() { "1" };
        }

        var user_id = Toolbox.doSQL_string(@"select member_id from member where member_user = @v0", user);

        var i = Toolbox.doSQL_int(@"SELECT count(*) FROM neintranet.memberpageprivilege 
                                where MemberPagePrivilege_Member_ID = " + user_id + @" AND
                                MemberPagePrivilege_Privilege_ID = 181");
        if (i != 1)
        {
            return new List<string>() { "1" };
        }

        string areaCode, phoneFirst, phoneLast;
        //strip - and space
        number = Regex.Replace(number, "[^0-9]+", string.Empty);
        if (!string.IsNullOrWhiteSpace(number) && number.Length >= 10)
        {
            if (number.Substring(0, 1).Equals("1"))
                number = number.Substring(1);
        }

        else
        {
            return new List<string>() { "1" };
        }

        areaCode = number.Substring(0, 3);
        phoneFirst = number.Substring(3, 3);
        phoneLast = number.Substring(6, 4);

        i = Toolbox.doSQL_int(@"select count(Member_AreaCode) from member
                                           where Member_AreaCode = @v0 AND Member_PhoneFirst = @v1 and Member_PhoneLast = @v2",
            new object[] {areaCode, phoneFirst, phoneLast});
        if (i >= 1)
        {
            return new List<string>() { "1" };
        }
        else
        {
            var j = Toolbox.doSQL_int(@"select count(id) from cellphone_number
                                                              where number = @v0", new object[] {areaCode + "-" + phoneFirst + "-" + phoneLast});
            if (j >= 1)
            {
                return new List<string>() { "1" };
            }
        }

        return new List<string>() { "0" };
    }

    private bool isValidUser(string user, string pass)
    {

        if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass))
        {
            //this will hold the plant text version of the password

            var plainTextPass = Toolbox.DecryptString(pass, passphrase);
            var plainTextUser = Toolbox.DecryptString(user, passphrase);

            //encrypt the password so it can be checked against the database

            var current_user = new NeMember(plainTextUser, plainTextPass);

            //get the encrypted password from the tatabase

            //if login is good return 1 else return 0
            if (current_user.Authenticated)
            {
                //update last login
                Toolbox.doSQL_void(@"UPDATE member
                                    SET last_mobile_login = CURRENT_TIMESTAMP()
                                    WHERE member_user = @v0 LIMIT 1",plainTextUser);
                return true;
            }
            return false;
        }
        return false;
    }

    [WebMethod]
    public List<string> getIsValidUser(string user, string pass)
    {


        if (isValidUser(user, pass))
            return new List<string>() { "1" };

        return new List<string>() { "0" };
    }



    [WebMethod]
    public List<string> setGpsData(string user, string pass, string lat, string lon, string timestamp, string imei, string phoneNumber, string softwareversion,
        string subscribeid, string simserial, string androidid, string battery, string provider, string speed, string accuracy)
    {
        //decrypt user&pass
        //Validate Valid User&Pass
        //if yes continue
        //else return loginError 
        if (isValidUser(user, pass))
        {
            //validate input data
            //else return invalid data
            //send to log
            //phone can try to get the data again ONE time then give up and try next gps sync interval.
            bool isValidData = !string.IsNullOrWhiteSpace(lat);
            if (string.IsNullOrWhiteSpace(lon))
                isValidData = false;
            if (string.IsNullOrWhiteSpace(timestamp) || timestamp.Trim().Equals("0") || timestamp.Trim().Equals("'0'"))
                isValidData = false;
            //if IMEI or phone number is empty send and email or enter into a log.

            //if correct Insert into database
            if (isValidData)
            {
                var sql = @"INSERT INTO cellphone_gps 
    (latitude_db,longitude_db,timestamp_dt,imei_ds,phonenumber_ds,softwareversion_ds,subscribeid_ds,simserial_ds,androidid_ds,battery_ft,provider_fg,member_username, speed, accuracy)
    VALUES (" + lat + "," + lon + "," + timestamp + "," + imei + "," + phoneNumber + "," + softwareversion + "," + subscribeid + "," + simserial + "," + androidid + "," + battery + "," +
provider + ",'" + Toolbox.DecryptString(user, passphrase) + "', " + speed + ", " + accuracy + @");
    ";
                try
                {
                    Toolbox.doSQL_void(sql);
                }
                catch (Exception ee)
                {
                    Toolbox.do_errorLog(ee.StackTrace, sql);
                }
                return new List<string>() { "1" };
            }

            return new List<string>() { "0" };
        }

        return new List<string>() { "0" };

    }


    public class NeLocationFromApp
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string timestamp_dt { get; set; }
        public string imei_ds { get; set; }
        public string phonenumber_ds { get; set; }
        public string softwareversion_ds { get; set; }
        public string simserial_ds { get; set; }
        public string subscribeid_ds { get; set; }
        public string androidid_ds { get; set; }
        public string battery_ft { get; set; }
        public string provider_fg { get; set; }
        public string speed { get; set; }
        public string accuracy { get; set; }
        public string username { get; set; }

        public NeLocationFromApp(JToken j)
        {

            latitude = (string)j.SelectToken("latitude");
            longitude = (string)j.SelectToken("longitude");
            timestamp_dt = (string)j.SelectToken("timestamp_dt");
            imei_ds = (string)j.SelectToken("imei_ds");
            phonenumber_ds = (string)j.SelectToken("phonenumber_ds");
            softwareversion_ds = (string)j.SelectToken("softwareversion_ds");
            simserial_ds = (string)j.SelectToken("simserial_ds");
            subscribeid_ds = (string)j.SelectToken("subscribeid_ds");
            androidid_ds = (string)j.SelectToken("androidid_ds");
            battery_ft = (string)j.SelectToken("battery_ft");
            provider_fg = (string)j.SelectToken("provider_fg");
            speed = (string)j.SelectToken("speed");
            accuracy = (string)j.SelectToken("accuracy");
            username = (string)j.SelectToken("username");
        }
    }

    [WebMethod]
    public List<string> setGpsData2(string user, string pass, string json)
    {

        if (isValidUser(user, pass))
        {

            var obj = JObject.Parse(json);

            var jsonLocals = obj.SelectToken("locations").Select(s => (JToken)s).ToList();
            var locations = new List<NeLocationFromApp>();

            foreach (var l in jsonLocals)
            {
                locations.Add( new NeLocationFromApp(l) );
            }

            var sql = @"INSERT INTO cellphone_gps 
        (latitude_db,longitude_db,timestamp_dt,imei_ds,phonenumber_ds,softwareversion_ds,
    simserial_ds,subscribeid_ds,androidid_ds,battery_ft,provider_fg,speed, accuracy, member_username) ";
            var prefix = "VALUES";
            foreach (var local in locations)
            {
                bool isValidData = !string.IsNullOrWhiteSpace(local.latitude);
                if (string.IsNullOrWhiteSpace(local.longitude))
                    isValidData = false;
                if (string.IsNullOrWhiteSpace(local.timestamp_dt) || local.timestamp_dt.Trim().Equals("0") || local.timestamp_dt.Trim().Equals("'0'"))
                    isValidData = false;

                if (isValidData)
                {
                    sql += prefix;
                    sql+= string.Format(@"({0}, {1}, '{2}', {3}, '{4}', {5}, {6}, {7}, {8}, {9}, '{10}', {11}, {12}, '{13}' )",
                      local.latitude, local.longitude,
                        local.timestamp_dt, local.imei_ds, local.phonenumber_ds,
                        local.softwareversion_ds, local.simserial_ds, local.subscribeid_ds,
                        local.androidid_ds, local.battery_ft, local.provider_fg, local.speed,
                        local.accuracy, local.username);
                    prefix = " ,";
                }else return new List<string>() { "0" };

            }

            try
            {
                Toolbox.doSQL_void(sql);
            }
            catch (Exception ee)
            {
                Toolbox.do_errorLog(ee.StackTrace, sql);
            }

            return new List<string>() { "1" };
        }

        return new List<string>() { "0" };

    }
    [WebMethod]
    public List<string> setNECallLog(string phonenumber_ds, string calltype_fg, string callername_ds, string numbertype_fg, string duration_nb,
        string calltime_nb, string imei_ds, string devicenumber_ds, string username)
    {
        //  Toolbox ne_onlineTools = new Toolbox();
        //  ne_onlineTools.connection_string = connGPSDB;
        if (string.IsNullOrEmpty(phonenumber_ds))
            return new List<string>() { "1" };

        var SQL_INSERT = @"INSERT INTO it.ne_calllog (
                phonenumber_ds,	calltype_fg,	callername_ds, numbertype_fg, 
                duration_nb,		calltime_nb,	imei_ds		 , devicenumber_ds, member_username
                ) VALUES (@v0,@v1,@v2,@v3,  @v4,@v5,@v6,@v7,@v8)";

        var paramObjects= new object[] {
        phonenumber_ds.Substring(0,40), calltype_fg, callername_ds, numbertype_fg,
                                                               duration_nb, calltime_nb, imei_ds, devicenumber_ds, username};
        Toolbox.doSQL_void(SQL_INSERT, paramObjects);

        return new List<string>() { "1" };
    }

    [WebMethod]
    public List<string> setNEGPS(string latitude_db, string longitude_db, string imei_ds, string phonenumber_ds, string softwareversion_ds, string simserial_ds,
                                   string subscribeid_ds, string androidid_ds, string battery_db, string timestamp_dt, string provider_fg, string username)
    {
        //  Toolbox ne_onlineTools = new Toolbox();
        //ne_onlineTools.connection_string = connGPSDB;
        var SQL_INSERT = @"INSERT INTO it.ne_location (
                latitude_db, 		 longitude_db, imei_ds, 	   phonenumber_ds,
                softwareversion_ds, simserial_ds, subscribeid_ds, androidid_ds,
                battery_db, 		 timestamp_dt, provider_fg, member_username
                ) VALUES (@v0,@v1,@v2,@v3,  @v4,@v5,@v6,@v7, @v8,@v9,@v10,@v11)";

        var paramObjects=   new object[] {
            latitude_db, longitude_db, imei_ds, phonenumber_ds, softwareversion_ds,
                                                             simserial_ds, subscribeid_ds, androidid_ds, battery_db,
                                                             timestamp_dt, provider_fg, username};
        Toolbox.doSQL_void(SQL_INSERT,paramObjects);

        return new List<string>() { "1" };
    }

    [WebMethod]
    public List<string> retrieveMaxCallTime(string imei_ds)
    {
        if (imei_ds == null || imei_ds == "null")
        {
            return new List<string>() { "-1" };
        }

        long time=-1;
        var SQL_QUERY = string.Format("select ifnull((SELECT MAX(calltime_nb) FROM it.ne_calllog WHERE imei_ds = '{0}' ), -1) ", imei_ds);
        try
        {
            time = Toolbox.doSQL_long(@"select ifnull((SELECT MAX(calltime_nb) FROM it.ne_calllog WHERE imei_ds = @v0  ), -1) ", new object[] {  imei_ds } );
        }
        catch (Exception ee)
        {
            Toolbox.do_errorLog_query(ee, SQL_QUERY);
        }

        return new List<string>() { time.ToString() };
    }


    [WebMethod]
    public List<string> isUpdate(string versionName, string versionCode, string username)
    {

        string curVersion;
        var SQL_QUERY = @"SELECT max(code_version) current_version 
                                 FROM it.nesi_app_versions
                                 WHERE isLive = 1;";
        try
        {

            curVersion = Toolbox.doSQL_string(SQL_QUERY);

            SQL_QUERY = @"call it.nesi_app_update_version(@v0, @v1, @v2)";
            Toolbox.doSQL_void(SQL_QUERY,new object[] { username, versionCode, versionName});
        }
        catch (Exception ee)
        {
            Toolbox.do_errorLog_query(ee,SQL_QUERY);
            return new List<string>() { "-1" };
        }

        if (Convert.ToInt16(versionCode) >= Convert.ToInt16(curVersion))
        {

            return new List<string>() { "1" };
        }
        return new List<string>() { "0" };
    }
    [WebMethod]
    public List<string> isUpdateNoUser(string versionName, string versionCode)
    {

        string curVersion;
        var SQL_QUERY = @"SELECT max(code_version) current_version 
                                 FROM it.nesi_app_versions
                                 WHERE isLive = 1;";
        try
        {

            curVersion = Toolbox.doSQL_string(SQL_QUERY);
        }
        catch (Exception ee)
        {
            Toolbox.do_errorLog_query(ee, SQL_QUERY);
            return new List<string>() { "-1" };
        }

        if (Convert.ToInt16(versionCode) >= Convert.ToInt16(curVersion))
        {

            return new List<string>() { "1" };
        }
        return new List<string>() { "0" };
    }


    [WebMethod]
    public List<string> updateDeviceInfoV3(string versionName, string versionCode, string username, string imei, string simSerial, string gcmID,
                    string manufacturer, string model, string serial, string phoneNum, string password, string androidID)
    {


        var mem_id = 1359;
        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
        {
            //this will hold the plant text version of the password

            // string plainTextPass = Toolbox.DecryptString(password, passphrase);
            // string plainTextUser = Toolbox.DecryptString(username, passphrase);
            var plainTextPass = password;
            var plainTextUser = username;

            //encrypt the password so it can be checked against the database

            var current_user = new NeMember(plainTextUser, plainTextPass);

            mem_id = current_user.id;

        }

        string curVersion;
        try
        {
            curVersion = Toolbox.doSQL_string(@"SELECT max(code_version) current_version 
                                 FROM it.nesi_app_versions
                                 WHERE isLive = 1;");

            Toolbox.doSQL_void(@"call cellphone_app_device_update(@v0, @v1, @v2, @v3, @v4, @v5, @v6, @v7)", new object[] {
                             mem_id, versionCode, versionName, gcmID, manufacturer, model, serial, androidID});

            var device_id = Toolbox.doSQL_int(@"select device_id from cellphone_app_device where serial =@v0", serial);

            Toolbox.doSQL_void(@"call cellphone_app_imei_update(@v0, @v1)", new object[] {
                             imei, device_id});

            Toolbox.doSQL_void(@"call cellphone_app_sim_update(@v0, @v1, @v2)", new object[] {
                             imei, simSerial, phoneNum });
        }
        catch (Exception ee)
        {
            Toolbox.do_errorLog(ee);
            return new List<string>() { "-1" };
        }

        if (Convert.ToInt16(versionCode) >= Convert.ToInt16(curVersion))
        {

            return new List<string>() { "1" };
        }
        return new List<string>() { "0" };
    }

    //not needed for 98 and up
    [WebMethod]
    public List<string> updateDeviceInfoV2(string versionName, string versionCode, string username, string imei, string simSerial, string gcmID,
                    string manufacturer, string model, string serial)
    {

        string curVersion;
        var SQL_QUERY = @"SELECT max(code_version) current_version 
                                 FROM it.nesi_app_versions
                                 WHERE isLive = 1;";
        try
        {


            curVersion = Toolbox.doSQL_string(SQL_QUERY);

            SQL_QUERY = @"call it.nesi_app_update_device2(get_member_id(@v0), @v1, @v2, @v3, @v4, @v5, @v6, @v7, @v8)";
            var paramObjects= new object[] {username, versionCode, versionName, imei, simSerial, gcmID, manufacturer, model, serial};
            Toolbox.doSQL_void(SQL_QUERY,paramObjects);
        }
        catch (Exception ee)
        {
            Toolbox.do_errorLog_query(ee, SQL_QUERY);
            return new List<string>() { "-1" };
        }

        if (Convert.ToInt16(versionCode) >= Convert.ToInt16(curVersion))
        {

            return new List<string>() { "1" };
        }
        return new List<string>() { "0" };
    }

    //Not needed for App version 96 and up
    [WebMethod]
    public List<string> updateDeviceInfo(string versionName, string versionCode, string username, string imei,
        string simSerial, string gcmID)
    {

        string curVersion;
        var SQL_QUERY = @"SELECT max(code_version) current_version 
                                 FROM it.nesi_app_versions
                                 WHERE isLive = 1;";
        try
        {


            curVersion = Toolbox.doSQL_string(SQL_QUERY);

            SQL_QUERY =
                    @"call it.nesi_app_update_device(get_member_id(@v0), @v1, @v2, @v3, @v4, @v5);";

            Toolbox.doSQL_void(SQL_QUERY,new object[] {username, versionCode, versionName, imei, simSerial, gcmID});
        }
        catch (Exception ee)

        {

            Toolbox.do_errorLog_query(ee, SQL_QUERY);
            return new List<string>() { "-1" };
        }

        if (Convert.ToInt16(versionCode) >= Convert.ToInt16(curVersion))
        {

            return new List<string>() { "1" };
        }
        return new List<string>() { "0" };
    }
}

