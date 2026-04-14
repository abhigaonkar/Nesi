using System;
using System.Web.UI.WebControls;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Web.UI;
using DevExpress.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using System.Web;
using nesi.core;

public partial class map_frame : System.Web.UI.Page
	{
	NeMember myMember;
	private const int _page_id = 145; // from Page table in DB

    string connStr = Toolbox.str_connection_string;
    
    private List<string> colors = GetColours();
	protected void Page_Load(object sender, EventArgs e)
		{
		    var _tools = new Toolbox();
		    myMember = Toolbox.do_handle_authentication(_page_id);
		    var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		    divMenu.InnerHtml = menu.MenuHTML;
		    divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
	        if (Page.Master == null) return;
	        var lbltemp = (Label)Page.Master.FindControl("lblHeading");
	        lbltemp.Text = @"User Map Interface";
            myMember = Toolbox.do_handle_authentication(145);
            if (!Page.IsPostBack)
            {
                var dateFormat = "MM/dd/yyyy hh:mm tt";

                dateTo.DisplayFormatString = dateFormat;
                dateTo.EditFormatString = dateFormat;
                dateFrom.DisplayFormatString = dateFormat;
                dateFrom.EditFormatString = dateFormat;

                dateFrom.Date = DateTime.Today;
                dateTo.Date = (DateTime.Today).AddHours(23).AddMinutes(59);


                Session["userUsers"] = null;
                fill_userSelector();
            }

		}

    private void fill_userSelector()
    {
        //if see all
        DataTable dt;
        if (myMember.AuthenticatedForPrivilege(133))
        {
            dt = Toolbox.doSQL_dt(@"select c.name as name, member_fullname AS `name`, a.member_id FROM member AS a LEFT JOIN cellphone_number AS b ON a.cellphone_number_id = b.id LEFT JOIN business_unit c ON c.id = a.business_unit_id  where (b.simcard IS NOT NULL OR b.number IS NOT NULL) AND b.status = 'Active' AND a.Member_Status = 'Active' order by a.business_unit_id, member_fullname" , null);
            

        }
        else
        {
            dt = Toolbox.doSQL_dt(@"select c.name, member_fullname AS `name`, a.member_id FROM member AS a LEFT JOIN cellphone_number AS b ON a.cellphone_number_id = b.id LEFT JOIN business_unit c ON c.id = a.business_unit_id  where (b.simcard IS NOT NULL OR b.number IS NOT NULL) AND b.status = 'Active' AND a.Member_Status = 'Active' AND a.business_unit_id =@v0 order by a.business_unit_id, member_fullname", new object[] { myMember.business_unit.id });
        }



        NavBarDataBind(dt);

       
    }
    private void NavBarDataBind(DataTable dt)
    {
        //create groups:
		if(dt.Rows.Count == 0) return;
        var currentGroup = dt.Rows[0].ItemArray[0].ToString();
        var gr = new NavBarGroup();
        gr.Text = currentGroup;
        gr.Name = currentGroup;
        userSelector.Groups.Add(gr);
        for (var i = 0; i < dt.Rows.Count; i++)
        {
            var rowGroup = dt.Rows[i].ItemArray[0].ToString();
            if (rowGroup != currentGroup)
            {
                var rowGr = new NavBarGroup();
                rowGr.Text = rowGroup;
                rowGr.Name = rowGroup;
                userSelector.Groups.Add(rowGr);
                currentGroup = rowGroup;
            }
        }

        //fill items:

        for (var j = 0; j < dt.Rows.Count; j++)
        {
            var gr1 = dt.Rows[j].ItemArray[0].ToString();
            var navBarGr = userSelector.Groups.FindByName(gr1);
            if (navBarGr != null)
            {
                var it = new NavBarItem();
                it.Text = dt.Rows[j].ItemArray[1].ToString();
                it.Name = dt.Rows[j].ItemArray[2].ToString();
                it.NavigateUrl =  dt.Rows[j].ItemArray[2].ToString();
                it.ClientEnabled = true;
                userSelector.Groups[navBarGr.Index].Items.Add(it);
            }
        }

        var urGroup = userSelector.Groups.FindByName(myMember.business_unit.name);
        var index = userSelector.Groups.IndexOf(urGroup);
        userSelector.AutoCollapse = false;
        userSelector.Groups.CollapseAll();
        userSelector.Groups[index].Expanded = true;

    }

    protected void chk_Init(object sender, EventArgs e)
    {
        var c = sender as ASPxCheckBox;
        var container = c.NamingContainer as NavBarItemTemplateContainer;
        var p = container.Item.NavigateUrl;
        c.ClientInstanceName = "chk_" + p;
        c.ClientSideEvents.CheckedChanged = string.Format(@"function(s, e) {{chk_callBackPan.PerformCallback('{0}|'+ s.GetValue());}}", p);

    }

    protected void chk_callBackPan_Callback(object sender, CallbackEventArgsBase e)
    {
        var p = e.Parameter.Split('|');
        Dictionary<string, string> users;
        if (Session["userUsers"] == null)
        {
            users = new Dictionary<string, string>();
        }
        else
        {
            users = (Dictionary<string, string>)Session["userUsers"];
        }

        if (p.Length >= 2)
        {
            var mID = p[0];
            var action = p[1];


            

            if (action == "true")
            {
               


                var number = Toolbox.doSQL_string(@"SELECT 
b.number
FROM
member AS a
LEFT JOIN cellphone_number AS b ON a.cellphone_number_id = b.id 
where a.member_id = @v0", mID);

                number = number.Replace("-", "");

               
                users.Add(mID, number);
                Session["userUsers"] = users;

                


            }
            else if (action == "false")
            {
                users.Remove(mID);
                Session["userUsers"] = users;
            }
            if (users.Count != 0)
            {
                var numArr = users.Values.ToArray();

                int phCount; 
                List<LocationTemp> list;
                string sb;
                GetSql(numArr, out phCount, out list, out sb);
                MakeLocationObj(phCount, list, sb);
                var json = MakeLocationJson(phCount, list);
                chk_callBackPan.JSProperties.Add("cpJson", json);
            }
            else
            {
                chk_callBackPan.JSProperties.Add("cpJson", "[{'Pho':'No valid data'}]");
            }
        }
        else
        {
           
            
            if (users.Count != 0)
            {
                var numArr = users.Values.ToArray();

                int phCount; 
                List<LocationTemp> list;
                string sb;
                GetSql(numArr, out phCount, out list, out sb);
                MakeLocationObj(phCount, list, sb);
                var json = MakeLocationJson(phCount, list);
                chk_callBackPan.JSProperties.Add("cpJson", json);
            }
            else
            {
                chk_callBackPan.JSProperties.Add("cpJson", "[{'Pho':'No valid data'}]");
            }
             
        }
    }
    protected void whoishere_callBackPan_Callback(object sender, CallbackEventArgsBase e)
    {

        var location = e.Parameter.Split('&');
        if (location.Length == 2)
        {
            Dictionary<string, string> users;
            
                users = new Dictionary<string, string>();
           //SELECT max(timestamp_dt),phonenumber_ds, ( 3959 * acos( cos( radians(43.431021) ) * cos( radians( latitude_db ) ) * cos( radians( longitude_db ) - radians(-79.782466) ) + sin( radians(43.431021) ) * sin( radians( latitude_db ) ) ) ) AS distance 
//FROM ne_location 
//group by phonenumber_ds
//HAVING distance < 5
//ORDER BY distance 
//LIMIT 0 , 20;


        }

    }

    private void MakeLocationObj( int phCount, List<LocationTemp> list, string sb)
    {
        using (var conn = new MySqlConnection(connStr))
        {
            var cmd = new MySqlCommand(sb, conn);
            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var lo = new LocationTemp();
                    lo.lat = reader["lat"].ToString();
                    lo.lng = reader["lng"].ToString();
                    lo.phonenumber_ds = reader["phone"].ToString();
                    lo.imei_ds = reader["imei_ds"].ToString();
                    lo.timestamp_dt = reader["dat"].ToString();
                    lo.bat =( Convert.ToDouble(reader["battery_db"]) * 100 ) + "%";
                    list.Add(lo);
                }
            }
        }
    }

    private void GetSql(string[] p_phone, out int phCount, out List<LocationTemp> list, out string sb)
    {
        var isNow = chkBox.Checked;
        var ph = p_phone;
        phCount = ph.Length;
        list = new List<LocationTemp>();
        sb = string.Empty;
        if (isNow && phCount > 1)
        {
            sb = MakeSql(ph);
        }
        else
            sb = MakeSql( isNow, ph, phCount);
    }

    private string MakeSql( bool isNow, string[] ph, int phCount)
    {
        
        var sb = new StringBuilder();
        var startDate = dateFrom.Date.ToString("yyyy-MM-dd HH:mm:ss");
        var endDate = dateTo.Date.ToString("yyyy-MM-dd HH:mm:ss");
        sb.Append("SELECT latitude_db as lat,longitude_db as lng,imei_ds,timestamp_dt as dat,TRIM(LEADING '+1' FROM   phonenumber_ds) as phone, battery_db FROM it.ne_location ");
        sb.Append(" WHERE  latitude_db !=0 AND longitude_db !=0 ");
        //
        // true = see locations from 5am - 5pm
        // false = see all locatons
        //
        if (!myMember.AuthenticatedForPrivilege(183))
        {
            sb.Append("  AND TIME_FORMAT(timestamp_dt, '%H:%i') > '05:00:00' and TIME_FORMAT(timestamp_dt, '%H:%i') < '17:00:00'");
        }

        sb.Append(string.Format("{0}", !isNow ? " and timestamp_dt>='" + startDate + "' and  timestamp_dt<='" + endDate + "' and (" : " and ( "));
        for (var i = 0; i < phCount; i++)
        {
            if (string.IsNullOrEmpty(ph[i])) continue;
            if (i == 0)
            {
                sb.Append(" TRIM(LEADING '+1' FROM   phonenumber_ds)=" + ph[i] + "");
            }
            else
            {
                sb.Append(" or TRIM(LEADING '+1' FROM   phonenumber_ds)=" + ph[i] + "");
            }
        }
        sb.Append(" ) ");
        sb.Append(" ORDER BY timestamp_dt desc,TRIM(LEADING '+1' FROM   phonenumber_ds)");
        sb.Append(string.Format(" {0}", !isNow ? string.Empty : "  LIMIT 1"));
        return sb.ToString();
    }

    string MakeSql(string[] ph)
    {
        var sb = new StringBuilder();
        sb.Append("SELECT latitude_db AS lat,longitude_db AS lng,imei_ds,timestamp_dt AS dat,TRIM(LEADING '+1' FROM  phonenumber_ds) AS phone, battery_db FROM ");
        sb.Append(" (SELECT * FROM it.ne_location WHERE latitude_db !=0 AND longitude_db!=0 and  phonenumber_ds !='null' and (");
        for (var i = 0; i < ph.Length; i++)
        {
            sb.Append("  TRIM(LEADING '+1' FROM   phonenumber_ds)=" + ph[i] + "");
            if (i < ph.Length - 1)
            {
                sb.Append(" or ");
            }
        }
        sb.Append(" )");
        sb.Append(" ORDER BY timestamp_dt DESC)");
        sb.Append("AS  ne_location");
        sb.Append(" GROUP BY TRIM(LEADING '+' FROM   phonenumber_ds) ORDER BY timestamp_dt DESC");
        return sb.ToString();
    }

    string GetRgb()
    {

        /*
        if (Session["userColors"] == null)
        {
            Session["userColors"] = colors;
        }
        else
        {
            colors = (List<string>)Session["userColors"];
        }
        */

        var colorCount = colors.Count;
        if (colorCount >= 0)
        {

            var c = colors[0];
            colors.Remove(c);
            return c;
        }
        else
        {
            colors = GetColours();
            return GetRgb();
        }
    }
    private string MakeLocationJson( int phCount, List<LocationTemp> list)
    {
        var _tools = new Toolbox();
        var member_dt = Toolbox.doSQL_dt(@"SELECT member.member_id member_id, member_name(member.member_id) name, cellphone_number.number phone FROM member LEFT JOIN cellphone_number ON member.cellphone_number_id = cellphone_number.id  where cellphone_number.number is not null" , null);
        if (list != null && list.Count > 0)
        {
            var q = (from c in list
                     group c by new
                     {
                         phone = c.phonenumber_ds,
                         imei = c.imei_ds
                     } into gr
                     select new
                     {
                         Pho = gr.Key,
                         Other = gr,
                         imei = gr.Key.imei
                     }).ToList();
            var ems = new List<Employee>();
            var j = 0;
            foreach (var e in q)
            {
                var i = 0;
                var em = new Employee();
                em.phone = e.Pho.phone;

                em.phone = string.Format("{0:###-###-####}", double.Parse(em.phone));

                var n = member_dt.Select("phone = '" + em.phone + "'").Count() > 0 ? (string)member_dt.Select("phone ='" + em.phone + "'")[0]["name"] : "";
                var id = member_dt.Select("phone = '" + em.phone + "'").Count() > 0 ? member_dt.Select("phone ='" + em.phone + "'")[0]["member_id"].ToString() : "";
                //                string n    = (string) member_dt
                em.Pho = e.Pho.phone;
                em.Col = GetRgb();
                em.Name = n;
                em.id = id;
                var lstLocation = new List<Location>();
                foreach (var temp in e.Other)
                {
                    i++;
                    var lo = new Location();
                    lo.lat = temp.lat;
                    lo.lng = temp.lng;
                    lo.bat = temp.bat;
                    lo.num = i;
                    lo.time = temp.timestamp_dt;
                    lstLocation.Add(lo);
                }
                if (lstLocation.Count > 0)
                {
                    em.Locations = lstLocation;
                }
                ems.Add(em);
                j++;
            }

            var json = ToJSON(ems);
            return json;
        }
        return "[{'Pho':'No valid data'}]";
    }

    public string ToJSON(object obj)
    {
        var serializer = new JavaScriptSerializer();
        return serializer.Serialize(obj);
    }


    public static List<string> GetColours()
    {
        var list = new List<string>();
        list.Add("FF3333");
        list.Add("98FB98");
        list.Add("EE00EE");
        list.Add("9F79EE");
        list.Add("8FBC8F");
        list.Add("F4A460");
        list.Add("A2B5CD");
        list.Add("FFFF00");
        list.Add("33FF33");
        list.Add("FB9898");
        list.Add("00EEEE");
        list.Add("EE9F79");
        list.Add("BC8F8F");
        list.Add("A4F460");
        list.Add("B5A2CD");
        list.Add("95EE72");
        list.Add("FF00FF");
        list.Add("3333FF");
        list.Add("9898FB");
        list.Add("EEEE00");
        list.Add("9FEE79");
        list.Add("8F8FBC");
        list.Add("F460A4");
        list.Add("B5CDA2");
        list.Add("9572EE");
        list.Add("00FFFF");
        list.Add("72ee72");
        list.Add("ee7272");
        list.Add("aaf460");
        list.Add("60cff4");
        list.Add("f460aa");


        //add more colors if u want
        //these are all duplicates just to stop errors 
        list.Add("FF3333");
        list.Add("98FB98");
        list.Add("EE00EE");
        list.Add("9F79EE");
        list.Add("8FBC8F");
        list.Add("F4A460");
        list.Add("A2B5CD");
        list.Add("FFFF00");
        list.Add("33FF33");
        list.Add("FB9898");
        list.Add("00EEEE");
        list.Add("EE9F79");
        list.Add("BC8F8F");
        list.Add("A4F460");
        list.Add("B5A2CD");
        list.Add("95EE72");
        list.Add("FF00FF");
        list.Add("3333FF");
        list.Add("9898FB");
        list.Add("EEEE00");
        list.Add("9FEE79");
        list.Add("8F8FBC");
        list.Add("F460A4");
        list.Add("B5CDA2");
        list.Add("9572EE");
        list.Add("00FFFF");
        list.Add("72ee72");
        list.Add("ee7272");
        list.Add("aaf460");
        list.Add("60cff4");
        list.Add("f460aa");
        return list;
    }
    private class Employee
    {
        public string Pho { get; set; }
        public string Col { get; set; }
        public string Name { get; set; }
        public string id { get; set; }
        public string phone { get; set; }
        public List<Location> Locations { get; set; }
    }

    private class Location
    {
        public string lat { get; set; }
        public string lng { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public int num { get; set; }
        public string time { get; set; }
        public string bat { get; set; }
    }

    private class LocationTemp
    {
        public string lat { get; set; }
        public string lng { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public int No { get; set; }
        public string Col { get; set; }
        public string imei_ds { get; set; }
        public string phonenumber_ds { get; set; }
        public string timestamp_dt { get; set; }
        public string bat { get; set; }
    }
}
