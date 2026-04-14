using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using nesi.core;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace Nesi.Web.modules
{
    public partial class CustomerVendorRequest : Page
    {
        private NameValueCollection q;
        private int type, id, woprogId, poprogId, quoteId, version;
        private bool isCustomer, isVendor, isNew;
        private NeMember currentUser;
        private NEVendor vendor;
        private NECustomer customer;
        private int addressid;

        public class CustVendRequest
        {
            public int id { get; set; }
            public DateTime dt { get; set; }
            public int type { get; set; }
            public int member_id { get; set; }
            public string type_name { get; set; }
            public int type_id { get; set; }
            public int address_id { get; set; }
            public string type_address { get; set; }
            public string type_phone { get; set; }
            public string type_email { get; set; }
            public string type_contact { get; set; }
            public string request_type { get; set; }
            public string additional_details { get; set; }
            public int woprog_id { get; set; }
            public int poprog_id { get; set; }
            public int quote_id { get; set; }
            public int rev { get; set; }

            // Customer-specific fields
            public string natAccount { get; set; }
            public string BDM { get; set; }
            public string PrimarySales { get; set; }
            public string InsideSales { get; set; }
            public string IndustrialType { get; set; }
            public string EndMarketSegment { get; set; }
            public string LeadSource { get; set; }
            public string LeadSourceDesc { get; set; }
            public string BusinessUnits { get; set; }
            public string JobTitle { get; set; }
            public string ShippingAddress { get; set; }
            public string Mobile { get; set; }
        }

        public CustVendRequest CVP;

        #region ---------- End Market Segments with ASP.NET Cache ----------
        private class EndMarketRow
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int IndustrialTypeId { get; set; }
        }

        private List<EndMarketRow> GetEndMarkets()
        {
            const string cacheKey = "EndMarketSegments_Cache";
            var cached = Cache[cacheKey] as List<EndMarketRow>;

            if (cached == null)
            {
                var dt = Toolbox.doSQL_dt(
                    @"SELECT id, name, industrial_type_id
                      FROM cus_end_market_segment
                      WHERE is_active = 1
                      ORDER BY name");

                cached = dt.AsEnumerable()
                    .Select(r => new EndMarketRow
                    {
                        Id = r.Field<int>("id"),
                        Name = r.Field<string>("name"),
                        IndustrialTypeId = r.Field<int>("industrial_type_id")
                    })
                    .ToList();

                // Cache for 60 seconds for faster refresh
                Cache.Insert(cacheKey, cached, null,
                    DateTime.Now.AddSeconds(60),
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return cached;
        }
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            q = Request.QueryString;

            if (!string.IsNullOrEmpty(q["dostatus"]))
            {
                Response.Clear();
                Response.Write("<div style='text-align: center;font-family:'Arial';font-weight: bold;'><br/><br/><p>Your request has been sent successfully </p><br/><br/><button type='button' onclick='window.close();'> Click to close window</button></div>");
                Response.End();
            }

            int.TryParse(q["type"], out type);
            int.TryParse(q["id"], out id);
            int.TryParse(q["woprog_id"], out woprogId);
            int.TryParse(q["poprog_id"], out poprogId);
            int.TryParse(q["quote_id"], out quoteId);
            int.TryParse(q["rev"], out version);

            isCustomer = type == 1;
            isVendor = type == 2;
            isNew = id == 0;
            currentUser = Toolbox.do_handle_authentication(isCustomer ? 10 : 11);

            if (isCustomer && !isNew)
            {
                customer = new NECustomer(id);
            }
            if (isVendor && !isNew)
            {
                vendor = new NEVendor(id);
            }

            // Validation
            if (new[] { woprogId, poprogId, quoteId }.Count(x => x > 0) > 1)
            {
                Toolbox.FriendlyException(Response, "Only one WO/PO/Quote id may be provided", "");
            }
            else if (new[] { woprogId, quoteId }.Any(x => x > 0) && isVendor)
            {
                Toolbox.FriendlyException(Response, "Work Order Id supplied for a vendor.. not proceeding.", "");
            }
            else if (poprogId > 0 && isCustomer)
            {
                Toolbox.FriendlyException(Response, "Only one WO/PO/Quote id may be provided", "");
            }
            else
            {
                if (!new List<int> { 1, 2 }.Contains(type))
                {
                    Toolbox.FriendlyException(Response, "Illegal type provided", "");
                }
                else
                {
                    Title = isCustomer
                        ? isNew ? "Customer Request - Add" : "Customer Request - Update"
                        : isNew ? "Vendor Request - Add" : "Vendor Request - Update";

                    content_title.InnerText = isCustomer
                        ? isNew ? "Request to add customer" : "Request to update customer"
                        : isNew ? "Request to add vendor" : "Request to update vendor";

                    div_regarding.Visible = woprogId > 0 || poprogId > 0 || quoteId > 0;
                    div_ddladdress.Visible = !isNew;

                    if (!isNew)
                    {
                        populateAddresses();
                    }

                    if (isCustomer)
                    {
                        vendorreq.Visible = false;
                        customerreq.Visible = true;
                    }
                    else
                    {
                        vendorreq.Visible = true;
                        customerreq.Visible = false;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (isCustomer)
                {
                    if (id > 0)
                    {
                        custname.Value = customer.Customer_Name;
                    }

                    if (woprogId > 0)
                    {
                        var wo = new NeWOProg(woprogId);
                        content_regarding.InnerText = $"RE: WO# - {wo.OrderNumber}";
                    }
                    else if (quoteId > 0 && version > 0)
                    {
                        content_regarding.InnerText = $"RE: Quote {quoteId} V{version}";
                    }

                    BindCustomerDropdowns();
                    var allEndMarkets = BindEndMarketSegments();
                    var json = new JavaScriptSerializer().Serialize(allEndMarkets);
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "endMarketData",
                        $"var allEndMarkets = {json};",
                        true
                    );
                    string script = $@"
            document.addEventListener('DOMContentLoaded', function () {{
                var ddlInd = document.getElementById('{ddlIndustrialType.ClientID}');
                var ddlEnd = document.getElementById('{ddlEndMarketSegment.ClientID}');

                if (!ddlInd || !ddlEnd) return;

                ddlInd.addEventListener('change', function () {{
                    var selectedId = parseInt(ddlInd.value);
                    ddlEnd.innerHTML = ''; // clear old options

                    var loadingOption = document.createElement('option');
                    loadingOption.text = 'Loading segments...';
                    ddlEnd.add(loadingOption);

                    setTimeout(function () {{
                        ddlEnd.innerHTML = '';

                        if (!selectedId || selectedId === 0) {{
                            var opt = document.createElement('option');
                            opt.text = '-- Select Industrial Type first --';
                            ddlEnd.add(opt);
                            return;
                        }}

                        var filtered = allEndMarkets.filter(function (em) {{
                            return em.IndustrialTypeId === selectedId;
                        }});

                        if (filtered.length === 0) {{
                            var opt = document.createElement('option');
                            opt.text = '-- No segments available --';
                            ddlEnd.add(opt);
                        }} else {{
                            var opt = document.createElement('option');
                            opt.text = '-- Select End Market Segment --';
                            opt.value = '';
                            ddlEnd.add(opt);
                            filtered.forEach(function (em) {{
                                var o = document.createElement('option');
                                o.text = em.Name;
                                o.value = em.Id;
                                ddlEnd.add(o);
                            }});
                        }}
                    }}, 100);
                }});
            }});
        ";

                    ScriptManager.RegisterStartupScript(this, GetType(), "DynamicDropdownScript", script, true);
                    return;
                }

                if (isVendor)
                {
                    if (id > 0)
                    {
                        name.Value = vendor.Name;
                        loadAddressInfo(vendor.Address.id);
                    }

                    if (poprogId > 0)
                    {
                        var po = new NePOProg(poprogId);
                        content_regarding.InnerText = $"RE: PO# - {po.poprog_bvpo}";
                    }
                }
            }
        }

        private void BindCustomerDropdowns()
        {
            ddlNatAccount.DataSource = Toolbox.doSQL_dt("SELECT 0 id, 'Please select a national account manager' name UNION SELECT id, name FROM cust_national_account_mgr WHERE is_active=1");
            ddlNatAccount.DataTextField = "name";
            ddlNatAccount.DataValueField = "id";
            ddlNatAccount.DataBind();

            ddlBDM.DataSource = Toolbox.doSQL_dt("SELECT 0 value, 'Please select a business development manager' label UNION SELECT netsuite_employee_internal_id as value, netsuite_employee_name label FROM netsuite_sales_rep WHERE netsuite_isinactive = 0 AND netsuite_issales_rep = 1");
            ddlBDM.DataTextField = "label";
            ddlBDM.DataValueField = "value";
            ddlBDM.DataBind();

            ddlInsideSales.DataSource = Toolbox.doSQL_dt("SELECT 0 id, 'Please select a inside sales team member' name UNION SELECT id, name FROM cus_inside_sales_team_member WHERE is_active=1");
            ddlInsideSales.DataTextField = "name";
            ddlInsideSales.DataValueField = "id";
            ddlInsideSales.DataBind();

            ddlIndustrialType.DataSource = Toolbox.doSQL_dt("SELECT 0 id, 'Please select an industrial type' name UNION SELECT id, name FROM cus_industrial_type WHERE is_active=1");
            ddlIndustrialType.DataTextField = "name";
            ddlIndustrialType.DataValueField = "id";
            ddlIndustrialType.DataBind();

            BindEndMarketSegments();

            ddlLeadSource.DataSource = Toolbox.doSQL_dt("SELECT 0 id, 'Please select a lead source' name UNION SELECT id, name FROM lead_source WHERE is_active=1");
            ddlLeadSource.DataTextField = "name";
            ddlLeadSource.DataValueField = "id";
            ddlLeadSource.DataBind();

            chkBusinessUnits.DataSource = Toolbox.doSQL_dt("SELECT id, ddl_name FROM business_unit WHERE active='T' AND ddl_name IS NOT NULL");
            chkBusinessUnits.DataTextField = "ddl_name";
            chkBusinessUnits.DataValueField = "id";
            chkBusinessUnits.DataBind();
        }

        private List<object> BindEndMarketSegments(int industrialTypeId = 0)
        {
            var endMarkets = GetEndMarkets();

            ddlEndMarketSegment.Items.Clear();

            if (industrialTypeId == 0)
            {
                ddlEndMarketSegment.Items.Add(new ListItem("-- Select Industrial Type first --", ""));
                return endMarkets.Select(e => new { e.Id, e.Name, e.IndustrialTypeId }).Cast<object>().ToList();
            }

            var filtered = endMarkets.Where(em => em.IndustrialTypeId == industrialTypeId).ToList();

            if (filtered.Count == 0)
            {
                ddlEndMarketSegment.Items.Add(new ListItem("-- No segments available --", ""));
            }
            else
            {
                ddlEndMarketSegment.Items.Add(new ListItem("-- Select End Market Segment --", ""));
                foreach (var em in filtered)
                {
                    ddlEndMarketSegment.Items.Add(new ListItem(em.Name, em.Id.ToString()));
                }
            }

            return endMarkets.Select(e => new { e.Id, e.Name, e.IndustrialTypeId }).Cast<object>().ToList();
        }


        // FIXED: Changed method name to match ASPX
        protected void ddlIndustrialType_Changed(object sender, EventArgs e)
        {
            int indTypeId;
            if (int.TryParse(ddlIndustrialType.SelectedValue, out indTypeId) && indTypeId > 0)
            {
                BindEndMarketSegments(indTypeId);
            }
            else
            {
                BindEndMarketSegments();
            }

            upEnd.Update();
        }

        private void populateAddresses()
        {
            ddlAddress.DataSource = Toolbox.doSQL_dt(
                @"SELECT address_id id, CONCAT('(',address_type,') ', IF(Address_Desc='' OR Address_Desc IS NULL, 'No Description',address_desc)) text 
                  FROM address 
                  WHERE address_table = @v1 AND address_table_id = @v0 
                  ORDER BY address_id",
                new object[] { id, isCustomer ? "Customer" : "Vendor" });
            ddlAddress.DataBind();
        }

        private void loadAddressInfo(int addressId)
        {
            var addressObj = new NEAddress(addressId);
            address.Value = addressObj.Addr1 + "\n" +
                            (addressObj.Addr2 != "" ? addressObj.Addr2 + "\n" : "") +
                            (addressObj.Addr3 != "" ? addressObj.Addr3 + "\n" : "") +
                            (addressObj.Addr4 != "" ? addressObj.Addr4 + "\n" : "") +
                            addressObj.City + "," +
                            addressObj.Prov + " " + addressObj.Postal;
            phone.Value = addressObj.PhoneNumber;
            email.Value = addressObj.Email;
            addressid = addressId;
        }

        protected void ddlAddress_OnSelectedIndexChanged(object _sender, EventArgs _e)
        {
            var addressId = Convert.ToInt32(ddlAddress.SelectedValue);
            loadAddressInfo(addressId);
            addressid = addressId;
        }

        protected void btn_send_OnClick(object _sender, EventArgs _e)
        {
            btn_send.Enabled = false;
            try
            {
                if (CVP == null) CVP = new CustVendRequest();
                if (id == 0) CVP.id = 0;
                CVP.type_id = id;
                CVP.type = type;
                CVP.member_id = currentUser.id;
                CVP.poprog_id = poprogId;
                CVP.woprog_id = woprogId;
                CVP.quote_id = quoteId;
                CVP.address_id = addressid;
                CVP.rev = version;

                string reqtype = Request.Form["reqtype"] ?? "";
                var body = "";

                if (isCustomer)
                {
                    CVP.type_name = custname.Value;
                    CVP.type_contact = custcontact.Value;
                    CVP.JobTitle = jobtitle.Value;
                    CVP.type_email = custemail.Value;
                    CVP.type_phone = custphone.Value;
                    CVP.Mobile = mobile.Value;
                    CVP.type_address = billingAddress.Value;
                    CVP.ShippingAddress = shippingAddress.Value;
                    CVP.request_type = reqtype;
                    CVP.additional_details = request_notes.Value;

                    CVP.natAccount = ddlNatAccount.SelectedItem.Text;
                    CVP.BDM = ddlBDM.SelectedItem.Text;
                    CVP.InsideSales = ddlInsideSales.SelectedItem.Text;
                    CVP.IndustrialType = (ddlIndustrialType.SelectedValue != "0" && !string.IsNullOrEmpty(ddlIndustrialType.SelectedValue))
                                         ? ddlIndustrialType.SelectedItem.Text : "Not Selected";
                    CVP.EndMarketSegment = (ddlEndMarketSegment.SelectedValue != "0" && !string.IsNullOrEmpty(ddlEndMarketSegment.SelectedValue)) ? ddlEndMarketSegment.SelectedItem.Text : "Not Selected";

                    CVP.LeadSource = ddlLeadSource.SelectedItem.Text;
                    CVP.LeadSourceDesc = txtLeadSourceDesc.Value;
                    CVP.BusinessUnits = string.Join(", ", chkBusinessUnits.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Text));

                    body = BuildCustomerEmailBody();
                }
                else
                {
                    CVP.type_name = name.Value;
                    CVP.type_address = address.Value;
                    CVP.type_email = email.Value;
                    CVP.type_contact = contact.Value;
                    CVP.type_phone = phone.Value;
                    CVP.additional_details = request_notes.Value;
                    CVP.request_type = reqtype;

                    if (vendor != null)
                    {
                        CVP.address_id = vendor.Address.id;
                    }
                    else
                    {
                        CVP.address_id = 0;
                    }

                    body = BuildVendorEmailBody();
                }

                var emailObj = new NeEMail
                {
                    To = isCustomer ? currentUser.business_unit.CustomerRequestEmail : currentUser.business_unit.VendorRequestEmail,
                    From = currentUser.NEEmail,
                    CC = currentUser.NEEmail,
                    Subject = isCustomer ? (isNew ? "Customer Creation Request" : "Customer Edit Request")
                                         : (isNew ? "Vendor Creation Request" : "Vendor Edit Request"),
                    Body = body,
                    isHTML = true
                };

                if (add_File.HasFile)
                {
                    var attach = add_File.PostedFile;
                    var filePath = Path.GetTempPath() + Toolbox.do_RandomString(5) + attach.FileName;
                    attach.SaveAs(filePath);
                    emailObj.Attachment = new Attachment(filePath);
                }

                if (isNew)
                {
                    if (isCustomer)
                    {
                        if (custemail.Value != "" && custcontact.Value == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "alert('Please enter a contact name.');", true);
                            btn_send.Enabled = true;
                            return;
                        }
                        else if (custcontact.Value != "" && custemail.Value == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "alert('Please enter an email.');", true);
                            btn_send.Enabled = true;
                            return;
                        }
                    }
                    else
                    {
                        if (email.Value != "" && contact.Value == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "alert('Please enter a contact name.');", true);
                            btn_send.Enabled = true;
                            return;
                        }
                        else if (contact.Value != "" && email.Value == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "alert('Please enter an email.');", true);
                            btn_send.Enabled = true;
                            return;
                        }
                    }
                }

                Updatecust_vend_requests();
                emailObj.Send();
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "location.href = '/modules/request.aspx?dostatus=1';", true);
            }
            catch (Exception ee)
            {
                Toolbox.do_errorLog_errorStack(ee);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "alert('There was an error submitting your request.');", true);
            }
        }
        private string BuildCustomerEmailBody()
        {
            var body = $@"
<div style='font-family:Segoe UI, Arial, sans-serif; font-size:14px; color:#333; padding:20px; background-color:#f9f9f9;'>
    <p style='font-size:16px;'>Hello,</p>
    <p>{currentUser.FullName} from <b>{currentUser.business_unit.ddl_name}</b> has requested that ";

            if (isNew)
            {
                body += " a new customer be created.</p>";
            }
            else
            {
                body += $" the customer <b>{customer.Customer_Name}</b> be updated.</p>";
            }

            var regarding = "";
            if (woprogId > 0)
            {
                regarding = $" WO #{woprogId}";
            }
            else if (quoteId > 0)
            {
                regarding = $" Quote #{quoteId} Ver:{version}";
            }
            else if (poprogId > 0)
            {
                regarding = $" PO #{poprogId}";
            }

            if (regarding != "")
            {
                body += $@"<p style='background:#eef4ff;padding:8px;border-left:4px solid #4a90e2;'><b>Request is regarding:</b> {regarding}</p>";
            }

            body += @"
    <h2 style='border-bottom:2px solid #4a90e2;padding-bottom:5px;color:#4a90e2;'>Details</h2>
    <table style='width:100%; border-collapse:collapse;'>";

            body += $@"
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Request Type</b></td><td style='padding:6px;'>{CVP.request_type}</td></tr>
        <tr><td style='padding:6px;width:200px;background:#f0f0f0;'><b>Company Name</b></td><td style='padding:6px;'>{CVP.type_name}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Contact Name</b></td><td style='padding:6px;'>{CVP.type_contact}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Job Title</b></td><td style='padding:6px;'>{CVP.JobTitle}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Email</b></td><td style='padding:6px;'>{CVP.type_email}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Phone</b></td><td style='padding:6px;'>{CVP.type_phone}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Mobile</b></td><td style='padding:6px;'>{CVP.Mobile}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Billing Address</b></td><td style='padding:6px;'>{CVP.type_address.Replace("\n", "<br/>")}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Shipping Address</b></td><td style='padding:6px;'>{CVP.ShippingAddress.Replace("\n", "<br/>")}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>National Account Manager</b></td><td style='padding:6px;'>{CVP.natAccount}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>BDM</b></td><td style='padding:6px;'>{CVP.BDM}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Inside Sales</b></td><td style='padding:6px;'>{CVP.InsideSales}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Industrial Type</b></td><td style='padding:6px;'>{CVP.IndustrialType}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>End Market Segment</b></td><td style='padding:6px;'>{CVP.EndMarketSegment}</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Lead Source</b></td><td style='padding:6px;'>{CVP.LeadSource} ({CVP.LeadSourceDesc})</td></tr>
        <tr><td style='padding:6px;background:#f0f0f0;'><b>Business Units</b></td><td style='padding:6px;'>{CVP.BusinessUnits}</td></tr>";

            body += @"</table>
    <h2 style='border-bottom:2px solid #4a90e2;padding-bottom:5px;color:#4a90e2;margin-top:20px;'>Supplied Additional Details</h2>
    <div style='padding:10px; background:#fffbe6; border:1px solid #ffe58f;'>" + CVP.additional_details + @"</div>
    <p style='text-align:center; margin-top:30px; font-weight:bold;'>Please respond to all once this request has been fulfilled</p>
</div>";

            return body;
        }

        private string BuildVendorEmailBody()
        {
            var body = $@"<div style='font-family:segoe ui;padding:5px;'>Hello,<br>{currentUser.FullName} from {currentUser.business_unit.ddl_name} has requested that ";

            if (isNew)
            {
                body += " a new vendor be created.";
            }
            else
            {
                body += $" the vendor <b>{vendor.Name}</b> be updated.";
            }

            var regarding = "";
            if (woprogId > 0)
            {
                regarding = $" WO #{woprogId}";
            }
            else if (quoteId > 0)
            {
                regarding = $" Quote #{quoteId} Ver:{version}";
            }
            else if (poprogId > 0)
            {
                regarding = $" PO #{poprogId}";
            }

            if (regarding != "")
            {
                body += $@"<hr/><b>Request is regarding {regarding}</b>";
            }

            body += $@"<hr /><h2>Details</h2>";

            body += $@"
<b>Name:</b> {CVP.type_name}<br/>
<b>Address:</b><br/>{CVP.type_address.Replace("\n", "<br/>")}<br/>
<b>Phone:</b> {CVP.type_phone}<br/>
<b>Email:</b> {CVP.type_email}<br/>
<b>Contact Name:</b> {CVP.type_contact}<br/>";

            body += $@"
<h2>Supplied additional details</h2>
{CVP.additional_details}<br/>
<br/><br/>
<center><b>Please respond to all once this request has been fulfilled</b></center>
</div>";

            return body;
        }

        private int getExisting(int type_id, int type)
        {
            int result;
            var sql = @"SELECT COALESCE((SELECT id FROM cust_vend_requests WHERE type_id = @v0 AND type = @v1 AND type_id != 0 LIMIT 1),0)";
            var paramObjects = new object[] { type_id, type };

            try
            {
                result = Toolbox.doSQL_int(sql, paramObjects);
            }
            catch (Exception ex)
            {
                result = 0;
            }

            return result;
        }

        private bool Updatecust_vend_requests()
        {
            var sql = "";
            var paramObjects = new object[] { };
            CVP.dt = DateTime.Now;
            bool isexisting = getExisting(CVP.type_id, CVP.type) != 0;

            if (!isexisting)
            {
                sql = @"INSERT INTO cust_vend_requests (dt,type,member_id,type_name,type_id,address_id,type_address,type_phone,type_email,type_contact,additional_details,woprog_id,poprog_id,quote_id,rev,request_type) 
                        VALUES (now(),@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14)";
                paramObjects = new object[]
                {
                    CVP.type, CVP.member_id, CVP.type_name, CVP.type_id, CVP.address_id,
                    CVP.type_address, CVP.type_phone, CVP.type_email, CVP.type_contact,
                    CVP.additional_details, CVP.woprog_id, CVP.poprog_id, CVP.quote_id,
                    CVP.rev, CVP.request_type
                };
            }
            else
            {
                sql = @"UPDATE cust_vend_requests 
                        SET dt=now(),type=@v0,member_id=@v1,type_name=@v2,type_id=@v3,address_id=@v4,type_address=@v5,type_phone=@v6,type_email=@v7,type_contact=@v8,additional_details=@v9,woprog_id=@v10,poprog_id=@v11,quote_id=@v12,rev=@v13,request_type=@v14 
                        WHERE type_id=@v3 AND type=@v0 LIMIT 1";
                paramObjects = new object[]
                {
                    CVP.type, CVP.member_id, CVP.type_name, CVP.type_id, CVP.address_id,
                    CVP.type_address, CVP.type_phone, CVP.type_email, CVP.type_contact,
                    CVP.additional_details, CVP.woprog_id, CVP.poprog_id, CVP.quote_id,
                    CVP.rev, CVP.request_type
                };
            }

            try
            {
                Toolbox.doSQL_void(sql, paramObjects);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}