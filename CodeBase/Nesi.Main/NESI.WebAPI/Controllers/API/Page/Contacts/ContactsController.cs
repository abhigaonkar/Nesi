using nesi.core;
using NESI.WebAPI.Controllers.API.Page.Employees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Contacts
{
    [RoutePrefix("api/Page/Employee/Contact")]
    public class ContactsController : EmployeesControllerBase
    {
        [HttpGet]
        public IHttpActionResult GetContacts(string member_user)
        {
            var o = getContacts(member_user);
            return Ok(o);
        }





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

       // string passphrase = Toolbox.config_setting("mobile_auth_key"); //Should get thisfrom ToolBox


        private List<ContactApp> getContacts(string member_user)
        {
            var contacts = new List<ContactApp>();
           // member_user = Toolbox.DecryptString(member_user, passphrase);
            var member_id = Toolbox.doSQL_string("select member_id from member where member_user = @v0", member_user);
            var visibleBusinessUnits = Toolbox.doSQL_string("select get_visible_business_units_group_concat(@v0)", member_id);
            var backofficeDT = Toolbox.doSQL_dt(@"SELECT id FROM business_unit WHERE is_backoffice AND active = 'T' AND istest = 'F'", null);
            foreach (DataRow b in backofficeDT.Rows)
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
ORDER BY m.Member_FirstName", new object[] { visibleBusinessUnits + ",11" });


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
    }
}
