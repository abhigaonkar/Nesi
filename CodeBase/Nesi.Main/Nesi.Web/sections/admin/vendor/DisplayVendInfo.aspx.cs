using System;
using System.Data;
//using nesi.bv;
using nesi.core;

public partial class sections_admin_vendor_DisplayVendInfo : System.Web.UI.Page
{
//    Toolbox _tools = new Toolbox();
//    string _HTMLDisp = "";
//    NeMember myMember;
//    int _PageAccess = 86;
//    string _MergerPrivilege = "77";
//    string _bvspecpricesourceid;
//    string _bvspecpricewhse;
//    string _bvspecpricecode;
//    string _selling_price;
//    string _starting_date;
//    string _ending_date;
//    string _auto_erase_record;
//    string _ignore_pdm_discount;
//    string _vendor_code;
//    string _bvrvversion;
//    string _bvrvadddate;
//    string _bvrvaddtime;
//    string _bvrvadduserinit;
//    string _bvspecpricepartno;
//    string _dsn;
//    string BVVendTabTable = "";
//    protected void Page_Load(object sender, EventArgs e)
//    {
        
//        myMember = Toolbox.do_handle_authentication(_PageAccess);
//        var MergeVendID = "";
//        var vendid = "";
//        var phonenumber = "";
//        var name = "";
//        var TypeofMerge = "";
//        try
//        {
//            MergeVendID = Request.QueryString["MergeVendor"];
//            TypeofMerge = Request.QueryString["MergeType"];
//        }
//        catch
//        {
//            MergeVendID = "";
//        }
//        if (string.IsNullOrEmpty(MergeVendID))
//        {
//            var vendinfo = new DataTable();
//            try
//            {
//                vendid = Request.QueryString["vendid"];
//                phonenumber = getPhoneNumberForMatch(vendid);
//                vendinfo = getAllVendID(phonenumber);
//                TypeofMerge = "PhoneNumber";
//            }
//            catch
//            {
//            }
//            if (string.IsNullOrEmpty(vendid))
//            {
//                try
//                {
//                    vendid = Request.QueryString["namevendid"];
//                    name = getNameForMatch(vendid);
//                    Session["NameToCheck"] = name;
//                    vendinfo = getAllVendIDName(Session["NameToCheck"].ToString());
//                    TypeofMerge = "VendorName";
//                }
//                catch
//                {
//                }
//            }
//            if (string.IsNullOrEmpty(vendid))
//            {
//                try
//                {
//                    vendid = Request.QueryString["addressvendid"];
//                    name = getAddressForMatch(vendid);
//                    Session["AddressToCheck"] = name;
//                    vendinfo = getAllVendIDAddress(Session["AddressToCheck"].ToString());
//                    TypeofMerge = "VendorAddress";
//                }
//                catch (Exception eex)
//                {
//                    throw new Exception(eex.ToString());
//                }
//            }

//            _HTMLDisp = "<table border='1'>";
//            foreach (DataRow row in vendinfo.Rows)
//            {
//                _HTMLDisp += GenerateInformation(row, TypeofMerge);
//            }
//            _HTMLDisp += "</table>";
//            Vendtable.InnerHtml = _HTMLDisp;
//        }
//        else
//        {
//            if (!myMember.AuthenticatedForPrivilege(Convert.ToInt32(_MergerPrivilege)))
//            {
//                throw new Exception("You Do Not have Permission to Merge Vendors");
//            }
//            MergeVendors(MergeVendID, TypeofMerge);
//        }
//    }
//    protected string getPhoneNumberForMatch(string vendid)
//    {
//        return _tools.getSQL_string(@"SELECT CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast,A.address_type) AS PhoneA FROM address as A, Vendor as C  WHERE A.Address_Table_ID = C.Vendor_ID AND A.Address_Table = 'Vendor' AND C.Vendor_ID =@v0", new object[] { vendid });
//    }
//    protected string getNameForMatch(string vendid)
//    {
//        return _tools.getSQL_string(@"SELECT vendor_name FROM Vendor  WHERE Vendor_ID =@v0", new object[] { vendid }); ;
//    }
//    protected string getAddressForMatch(string vendid)
//    {
//        var sqlAddressFind = "SELECT A.Address_Addr1 ";
//        sqlAddressFind += "FROM address as A, Vendor as C ";
//        sqlAddressFind += "WHERE ";
//        sqlAddressFind += "A.Address_Table_ID = C.Vendor_ID ";
//        sqlAddressFind += "AND A.Address_Table = 'Vendor' ";
//        sqlAddressFind += "AND C.Vendor_ID = @v0";
//        var Addr1 = _tools.getSQL_string(sqlAddressFind, new object[] { vendid });
//        return Addr1;
//    }
//    protected DataTable getAllVendID(string phonenumber)
//    {
//        var VendorSQL = "SELECT vendor_number, Vendor_Name, ";
//        VendorSQL += "Address_Addr1,Address_City, Address_Prov, ";
//        VendorSQL += "Address_PhoneArea, Address_PhoneFirst, Address_PhoneLast, Vendor_ID ";
//        VendorSQL += "FROM address as A, Vendor as C ";
//        VendorSQL += "WHERE CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast,A.address_type) =@v0 ";
//        VendorSQL += "AND A.Address_Table = 'Vendor' ";
//        VendorSQL += "AND A.Address_Table_ID = C.Vendor_ID ";
//        VendorSQL += "AND C.Vendor_Active = 1";
//        var table = _tools.getSQL_datatable(VendorSQL, new object[] { phonenumber });
//        return table;
//    }
//    protected DataTable getAllVendIDName(string name)
//    {
//        var table = _tools.getSQL_datatable(@" SELECT vendor_number, Vendor_Name, Address_Addr1, Address_City, Address_Prov, Address_PhoneArea, Address_PhoneFirst, Address_PhoneLast, Vendor_ID FROM Vendor AS C LEFT JOIN address a ON A.Address_Table = 'Vendor' AND A.Address_Table_ID = C.Vendor_ID  WHERE vendor_name LIKE CONCAT('%',@v0,'%')  AND vendor_active = 1", new object[] { name });

//        return table;
//    }
//    protected DataTable getAllVendIDAddress(string address)
//    {
//        var VendorSQL = "SELECT vendor_number, Vendor_Name, ";
//        VendorSQL += "Address_Addr1,Address_City, Address_Prov, ";
//        VendorSQL += "Address_PhoneArea, Address_PhoneFirst, Address_PhoneLast, Vendor_ID ";
//        VendorSQL += "FROM address as A, Vendor as C ";
//        VendorSQL += "WHERE Address_Addr1 = @v0 ";
//        VendorSQL += "AND A.Address_Table = 'Vendor' ";
//        VendorSQL += "AND A.Address_Table_ID = C.Vendor_ID ";
//        VendorSQL += "AND C.Vendor_Active = 1";
//        var table = _tools.getSQL_datatable(VendorSQL, new object[] { address });
//        return table;
//    }
//    protected string GenerateInformation(DataRow row, string MergeType)
//    {
//        var html = "";
//        html = "<tr>";
//        html += "<td>MySQL ID:" + row[8] + "</td>";
//        html += "<td>" + row[1] + "</td>";
//        html += "<td>BV Vend Num:" + row[0] + "</td>";
//        html += "<td>" + row[5] + "-" + row[6] + "-" + row[7] + "</td>";
//        html += "<td>" + row[2] + "</td>";
//        html += "<td>" + row[3] + "</td>";
//        html += "<td>" + row[4] + "</td></tr>";
//        html += "<tr><td>Used in Inventory Price this many Times:</td>";
//        html += "<td colspan='5'>" + PriceCount(row[0]) + "</td></tr>";
//        html += "<tr><td>Used in Inventory Price History this many Times:</td>";
//        html += "<td colspan='5'>" + PriceHistoryCount(row[0]) + "</td></tr>";
//        html += "<tr><td>Used in Inventory Reference this many Times:</td>";
//        html += "<td colspan='5'>" + ReferenceCount(row[0]) + "</td></tr>";
//        html += "<tr><td colspan='5'>" + UsedInBV(row[0]) + "</td>";
//        // html += "<td>Make <input type=\"submit\" name=\"Button2\" value=\"" + row[0].ToString() + "\" id=\"Button2\" class=\"button\" style=\"color:Black;border-width:0px;border-style:Solid;font-size:11px;font-weight:bold;height:26px;\" /> The True Vendor</td></tr>";
//        html += "<td>Make <a href=\"DisplayVendInfo.aspx?MergeVendor=" + row[8] + "&MergeType=" + MergeType + "\" onClick=\"return confirmation()\">" + row[0] + "</a> The True Vendor</td></tr>";

//        return html;
//    }
//    protected string PriceCount(object vendNum)
//    {
//        var count = "";
//        var vendor_id = _tools.getSQL_string(@"SELECT vendor_id FROM vendor WHERE vendor_number = @v0 ", new object[] { vendNum });
//        var TableCounter = "SELECT COUNT(*) FROM inventory_price WHERE vendor_id = '" + vendor_id + "'";
//        count = _tools.getSQL_string(@"SELECT COUNT(*) FROM inventory_price  WHERE vendor_id =@v0", new object[] { vendor_id });
//        return count;
//    }
//    protected string PriceHistoryCount(object vendNum)
//    {
//        var count = "";
//        var vendor_id = _tools.getSQL_string(@"SELECT vendor_id FROM vendor WHERE vendor_number = @v0 ", new object[] { vendNum });
//        var TableCounter = "SELECT COUNT(*) FROM inventory_price_history WHERE vendor_id = '" + vendor_id + "'";
//        count = _tools.getSQL_string(@"SELECT COUNT(*) FROM inventory_price_history  WHERE vendor_id =@v0", new object[] { vendor_id });
//        return count;
//    }
//    protected string ReferenceCount(object vendnum)
//    {
//        var count = "";
//        var TableCounter = "SELECT COUNT(*) FROM inventory_reference WHERE ref_vendor_number = '" + vendnum + "'";
//        count = _tools.getSQL_string(@"SELECT COUNT(*) FROM inventory_reference  WHERE ref_vendor_number =@v0", new object[] { vendnum });
//        return count;
//    }
//    protected string UsedInBV(object vendnum)
//    {
//        var BVVendTable = "<table>";
//        var DSNList = "SELECT distinct t.dsn dsn, b.name from tax_entity t inner join business_unit b on b.tax_entity_id=t.id WHERE b.enable_timesheet=1 OR b.ID IN (20,21)";
//        var BVVendInfo = "";
//        var BVVendName = "";
//        var DSNTable = _tools.getSQL_datatable(DSNList, null);
//        foreach (DataRow row in DSNTable.Rows)
//        {
//            var dsn = row["dsn"].ToString();
//            var name = row["name"].ToString();
//            BVVendInfo = "SELECT NAME FROM VENDOR WHERE VEN_NO = '" + vendnum + "'";
//            try
//            {
//                BVVendName = _tools.getSQL_string(@"SELECT NAME FROM VENDOR  WHERE VEN_NO =?", dsn, new object[] { vendnum });
//            }
//            catch
//            {
//                BVVendName = "Not Found";
//            }
//            BVVendInfo = "SELECT LAST_DATE FROM VENDOR WHERE VEN_NO = '" + vendnum + "'";
//            try
//            {
//                BVVendName += " Last Date: " + _tools.getSQL_string(@"SELECT LAST_DATE FROM VENDOR  WHERE VEN_NO =?", dsn, new object[] { vendnum });
//            }
//            catch
//            {
//                BVVendName += " No Last Date";
//            }

//            BVVendTable += "<tr><td>" + dsn + "</td>";
//            BVVendTable += "<td>" + name + "</td>";
//            BVVendTable += "<td>" + BVVendName + "</td></tr>";

//            BVVendTabTable += dsn + "\t";
//            BVVendTabTable += name + "\t";
//            BVVendTabTable += BVVendName + "\t\n";
//        }
//        BVVendTable += "</table>";
//        BVVendTabTable += "\n\n\n";
//        //BVVendTable += BVVendTabTable;
//        return BVVendTable;
//    }
//    protected void MergeVendors(string vendorid, string mergetype)
//    {
//        var companyIDS = _tools.getSQL_datatable(@"SELECT b.id, t.DSN FROM  tax_entity t inner join business_unit b on b.tax_entity_id=t.id  WHERE b.ID = 20 OR b.ID = 21", null);
//        var merged_to_vendor = new NEVendor(Convert.ToInt32(vendorid));
//        var MergeVendNum = _tools.getSQL_string(@"Select Vendor_Number FROM vendor  WHERE Vendor_ID =@v0", new object[] { vendorid });
//        UsedInBV(MergeVendNum);
//        var sql = "";
//        var phonematch = getPhoneNumberForMatch(vendorid);
//        //string namematch = getNameForMatch(vendorid);
//        var emailmessage = "";
//        var emailShortBody = "Vendor Number " + MergeVendNum + " has been made the true vendor number and vendor numbers ";


//        var VendorTable = new DataTable();
//        if (mergetype == "PhoneNumber")
//        {
//            VendorTable = getAllVendID(phonematch);
//        }
//        else if (mergetype == "VendorName")
//        {
//            VendorTable = getAllVendIDName(Session["NameToCheck"].ToString());

//        }
//        else if (mergetype == "VendorAddress")
//        {
//            VendorTable = getAllVendIDAddress(Session["AddressToCheck"].ToString());
//        }



//        var tblproblem = new DataTable();
//        var IssueLines = "";
//        var InfoSave = "";

//        //Update inventory_price and inventory_price_history and then set the vendor to
//        //inactive.
//        var InvPrice = new DataTable();
//        var dt1 = new DateTime();
//        var dt2 = new DateTime();
//        var strInvPriceSel = "";
//        var strCheckPriceSel = "";
//        var CheckPrice = new DataTable();

//        foreach (DataRow row in VendorTable.Rows)
//        {
//            if (row[8].ToString() != vendorid)
//            {
//                emailShortBody += row[0] + " has been made inactive ";
//                UsedInBV(row[0].ToString());
//                //Check Vendor for Each Company
//                foreach (DataRow companyrow in companyIDS.Rows)
//                {

//                    //Update inventory Price
//                    strInvPriceSel = "SELECT id, master_id, business_unit_id, vendor_id, vendor_code, cost, edited_dt ";
//                    strInvPriceSel += "FROM inventory_price ";
//                    strInvPriceSel += "WHERE business_unit_id =@v0 ";
//                    strInvPriceSel += " AND vendor_id =@v1 ";
//                    emailmessage += strInvPriceSel + "\n\n\n";
//                    InvPrice = _tools.getSQL_datatable(strInvPriceSel, new object[] { companyrow[0], row["vendor_id"] });
//                    try
//                    {
//                        foreach (DataRow pricerow in InvPrice.Rows)
//                        {
//                            var price_id = pricerow["id"].ToString();
//                            strCheckPriceSel = "SELECT * FROM inventory_price WHERE vendor_code = '" + pricerow[4] + "' AND vendor_id = '" + vendorid + "' AND master_id = " + pricerow[1] + " AND business_unit_id = " + pricerow[2];
//                            emailmessage += strCheckPriceSel + "\n";
//                            CheckPrice = _tools.getSQL_datatable(@"SELECT * FROM inventory_price  WHERE vendor_code =@v0 AND vendor_id =@v1  AND master_id =@v2  AND business_unit_id =@v3 ", new object[] { pricerow[4], vendorid, pricerow[1], pricerow[2] });
//                            if (CheckPrice.Rows.Count > 0)
//                            {
//                                foreach (DataRow checkpricerow in CheckPrice.Rows)
//                                {
//                                    dt1 = Convert.ToDateTime(pricerow[6]);
//                                    dt2 = Convert.ToDateTime(checkpricerow[5]);
//                                    if (dt1 == dt2)
//                                    {
//                                        if (Convert.ToDouble(checkpricerow[7]) <= Convert.ToDouble(pricerow[5]))
//                                        {
//                                            sql = "DELETE FROM inventory_price WHERE id = " + price_id;
//                                            _tools.getSQL_void(sql);
//                                            emailmessage += sql + " Dates The Same\n\n";
//                                        }
//                                        else
//                                        {
//                                            sql = "UPDATE inventory_price SET cost = " + pricerow[5] + ", edited_dt = now() WHERE id = " + checkpricerow[0];
//                                            _tools.getSQL_void(sql);
//                                            emailmessage += sql + " Dates The Same\n";
//                                            sql = "DELETE FROM inventory_price WHERE id = " + price_id;
//                                            _tools.getSQL_void(sql);
//                                            emailmessage += sql + " Dates The Same\n\n";
//                                        }
//                                    }
//                                    else
//                                    {
//                                        if (dt1 > dt2)
//                                        {
//                                            sql = "UPDATE inventory_price SET cost = " + pricerow[5] + ", edited_dt = now() WHERE id = " + checkpricerow[0];
//                                            _tools.getSQL_void(sql);
//                                            emailmessage += sql + " Dates Different\n";
//                                            sql = "DELETE FROM inventory_price WHERE id = " + price_id;
//                                            _tools.getSQL_void(sql);
//                                            emailmessage += sql + " Dates Different\n\n";

//                                        }
//                                        else
//                                        {
//                                            sql = "DELETE FROM inventory_price WHERE id = " + price_id;
//                                            _tools.getSQL_void(sql);
//                                            emailmessage += sql + " Dates Different\n\n";

//                                        }

//                                    }

//                                }

//                            }
//                            else
//                            {
//                                sql = "UPDATE inventory_price SET vendor_id = '" + vendorid + "' WHERE vendor_id = '" + pricerow[3] + "' AND id = " + price_id;
//                                _tools.getSQL_void(sql);
//                                emailmessage += sql + "\n\n";
//                            }


//                        }
//                    }
//                    catch (Exception eeex)
//                    {
//                        IssueLines += eeex.ToString();
//                        emailmessage += "There was an issue with" + row[8] + " " + IssueLines + "\n\n";

//                    }
//                    sql = "UPDATE inventory_price_history SET vendor_id = '" + vendorid + "' WHERE vendor_id = '" + row[0] + "' ";
//                    _tools.getSQL_void(sql);
//                    emailmessage += sql + "\n\n";

//                    sql = "UPDATE inventory_reference SET ref_vendor_number = '" + vendorid + "' WHERE ref_vendor_number = '" + row[0] + "' ";
//                    _tools.getSQL_void(sql);

//                    sql = string.Format(@"UPDATE poprog_header SET poprog_vendor_id = '{0}' where poprog_vendor_id = {1}", vendorid, row[8]);
//                    Toolbox.doSQL_void(@"UPDATE contact SET contact_cust_id = @v0 WHERE contact_cust_id = @v1 AND contact_type = 'Vendor'", new object[] { vendorid, row[8] });
//                    try
//                    {
//                        _tools.getSQL_void(sql);
//                    }
//                    catch { }

//                    sql = "UPDATE vendor SET vendor_active = 0 ";
//                    sql += "WHERE vendor_id = " + row[8];
//                    _tools.getSQL_void(sql);
//                    emailmessage += sql + "\n\n";
//                }




//            }
//        }



//        //Add The New BVs
//        var MergeCompanyNo = _tools.getSQL_string(@"Select business_unit_id FROM vendor  WHERE Vendor_ID =@v0", new object[] { vendorid });
//        var strErrorMessage = "";
//        var erroflag = 0;
//        var vendcompanyid = _tools.getSQL_string(@"SELECT business_unit_id FROM Vendor  WHERE Vendor_ID =@v0", new object[] { vendorid });
//        var strVendorNo = _tools.getSQL_string(@"SELECT Vendor_Number FROM Vendor  WHERE Vendor_ID =@v0", new object[] { vendorid });
//        var strFormDSN = _tools.getSQL_string(@"SELECT distinct t.dsn from tax_entity t inner join business_unit b on b.tax_entity_id=t.id WHERE b.id =@v0", new object[] { MergeCompanyNo });
//        var CompanyList = _tools.getSQL_datatable(@"SELECT distinct t.dsn, b.name from tax_entity t inner join business_unit b on b.tax_entity_id=t.id  WHERE b.id !=@v0 AND enable_timesheet = 1", new object[] { MergeCompanyNo });
//        var strToDSN = "";
//        //string strToCoID = "";
//        var fromVendor = new Vendor(strFormDSN, strVendorNo);
//        foreach (DataRow comprow in CompanyList.Rows)
//        {
//            erroflag = 0;
//            strToDSN = comprow[0].ToString();
//            try
//            {
//                var _exists = _tools.getSQL_int(@"SELECT COUNT(*) FROM VENDOR  WHERE ven_no =?", strFormDSN, new object[] { fromVendor.VendorNumber }) == 0;
//                fromVendor.VendorName = fromVendor.VendorName.Replace("'", "''");
//                fromVendor.VendorAddress.Name = fromVendor.VendorAddress.Name.Replace("'", "''");
//                fromVendor.Save(strToDSN, _exists, "SH");

//            }
//            catch (Exception ex)
//            {
//                strErrorMessage += "Vendor Was not Added to " + comprow[1] + " BV Tables\n";
//                strErrorMessage += " Error Returned Was " + ex + "\n<br>";
//                erroflag = 1;
//            }
//            if (erroflag == 0)
//            {
//                strErrorMessage += "Vendor was Added to " + comprow[1] + " BV Tables\n<br>";
//            }

//        }


//        //Update BV Special Pricing Table.
//        emailmessage += "\n\n\n BV Table Update SQL \n\n\n";
//        foreach (DataRow row in VendorTable.Rows)
//        {
//            if (row[8].ToString() != vendorid)
//            {
//                foreach (DataRow companyrow in companyIDS.Rows)
//                {
//                    var dsn = companyrow[1].ToString();
//                    //if (companyrow[0].ToString() == "8")
//                    // {

//                    //Update Special Pricing
//                    strInvPriceSel = "SELECT VENDOR_CODE, BVSPECPRICECODE, BVSPECPRICEPARTNO, SELLING_PRICE, BVRVMODDATE, BVRVMODTIME, BVRVADDDATE, BVRVADDTIME ";
//                    strInvPriceSel += "FROM SPECIAL_PRICING ";
//                    strInvPriceSel += "WHERE BVSPECPRICESOURCEID = 'V' ";
//                    strInvPriceSel += "AND BVSPECPRICECODE = ?";
//                    emailmessage += companyrow[1] + " " + strInvPriceSel + "\n\n\n";
//                    InvPrice = _tools.getSQL_datatable(strInvPriceSel, dsn, new object[] { row[0] });
//                    foreach (DataRow pricerow in InvPrice.Rows)
//                    {
//                        try
//                        {

//                            strCheckPriceSel = "SELECT VENDOR_CODE, BVSPECPRICECODE, BVSPECPRICEPARTNO, SELLING_PRICE, BVRVMODDATE, BVRVMODTIME, BVRVADDDATE, BVRVADDTIME FROM SPECIAL_PRICING WHERE VENDOR_CODE = '"
//                                + pricerow[0].ToString().Trim()
//                                + "' AND BVSPECPRICEPARTNO = '" + pricerow[2].ToString().Trim() + "' AND BVSPECPRICECODE = '" + MergeVendNum + "' ";
//                            emailmessage += companyrow[1] + " " + strCheckPriceSel + "\n";
//                            CheckPrice = _tools.getSQL_datatable(@"SELECT VENDOR_CODE, BVSPECPRICECODE, BVSPECPRICEPARTNO, SELLING_PRICE, BVRVMODDATE, BVRVMODTIME, BVRVADDDATE, BVRVADDTIME FROM SPECIAL_PRICING  WHERE VENDOR_CODE =? AND BVSPECPRICEPARTNO =?  AND BVSPECPRICECODE =? ", dsn, new object[] { pricerow[0].ToString().Trim(), pricerow[2].ToString().Trim(), MergeVendNum });
//                            if (CheckPrice.Rows.Count == 0)
//                            {
//                                _bvspecpricesourceid = "V";
//                                _bvspecpricewhse = "00";
//                                _bvspecpricecode = MergeVendNum;
//                                _selling_price = pricerow[3].ToString();
//                                _starting_date = "00000000";
//                                _ending_date = "00000000";
//                                _auto_erase_record = "0";
//                                _ignore_pdm_discount = "0";
//                                _vendor_code = pricerow[0].ToString();
//                                _bvrvversion = "7.12";
//                                _bvrvadddate = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day;
//                                _bvrvaddtime = DateTime.Now.Hour + DateTime.Now.Minute.ToString() + DateTime.Now.Second;
//                                _bvrvadduserinit = "SH";
//                                _bvspecpricepartno = pricerow[2].ToString();
//                                _dsn = dsn;
//                                sql = specialpricingadd();
//                                emailmessage += companyrow[1] + " " + sql + "Did not exist was added.\n\n";
//                                sql = "DELETE FROM SPECIAL_PRICING WHERE VENDOR_CODE = '" + pricerow[0].ToString().Trim() + "' AND BVSPECPRICEPARTNO = '" + pricerow[2].ToString().Trim() + "' AND BVSPECPRICECODE = '" + pricerow[1].ToString().Trim() + "'";
//                                emailmessage += companyrow[1] + " " + sql + "Did not exist was added old one deleted.\n\n";
//                                _tools.getSQL_void(@"DELETE FROM SPECIAL_PRICING  WHERE VENDOR_CODE =? AND BVSPECPRICEPARTNO =?  AND BVSPECPRICECODE =? ", dsn, new object[] { pricerow[0].ToString().Trim(), pricerow[2].ToString().Trim(), pricerow[1].ToString().Trim() });
//                            }
//                            else
//                            {
//                                foreach (DataRow checkpricerow in CheckPrice.Rows)
//                                {
//                                    try { dt1 = Convert.ToDateTime(pricerow[4].ToString()); }
//                                    catch
//                                    {
//                                        try { dt1 = Convert.ToDateTime(pricerow[6].ToString()); }
//                                        catch { dt1 = DateTime.Now; }
//                                    }
//                                    try { dt2 = Convert.ToDateTime(pricerow[4].ToString()); }
//                                    catch
//                                    {
//                                        try { dt2 = Convert.ToDateTime(pricerow[6].ToString()); }
//                                        catch { dt2 = DateTime.Now; }
//                                    }
//                                    if (dt1 == dt2)
//                                    {
//                                        if (Convert.ToDouble(checkpricerow[3].ToString()) <= Convert.ToDouble(pricerow[3].ToString()))
//                                        {
//                                            sql = "DELETE FROM SPECIAL_PRICING WHERE VENDOR_CODE = '" + pricerow[0].ToString().Trim() + "' AND BVSPECPRICEPARTNO = '" + pricerow[2].ToString().Trim() + "' AND BVSPECPRICECODE = '" + pricerow[1].ToString().Trim() + "'";
//                                            _tools.getSQL_void(@"DELETE FROM SPECIAL_PRICING  WHERE VENDOR_CODE =? AND BVSPECPRICEPARTNO =?  AND BVSPECPRICECODE =? ", dsn, new object[] { pricerow[0].ToString().Trim(), pricerow[2].ToString().Trim(), pricerow[1].ToString().Trim() });
//                                            emailmessage += dsn + " " + sql + " Dates The Same\n\n";
//                                        }
//                                        else
//                                        {
//                                            sql = @"UPDATE SPECIAL_PRICING SET SELLING_PRICE = " + pricerow[3]
//                                            + ", BVRVMODDATE = '" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day
//                                            + "' WHERE VENDOR_CODE = '" + checkpricerow[0].ToString().Trim() + "' AND BVSPECPRICEPARTNO = '" + checkpricerow[2].ToString().Trim()
//                                            + "' AND BVSPECPRICECODE = '" + checkpricerow[1].ToString().Trim() + "'";
//                                            _tools.getSQL_void(@"UPDATE SPECIAL_PRICING  SET SELLING_PRICE =?, BVRVMODDATE =?   WHERE VENDOR_CODE =? AND BVSPECPRICEPARTNO ? AND BVSPECPRICECODE =? ", dsn, new object[] { pricerow[3].ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), checkpricerow[0].ToString().Trim(), checkpricerow[2].ToString().Trim(), checkpricerow[1].ToString().Trim() });
//                                            sql = @"DELETE FROM SPECIAL_PRICING WHERE VENDOR_CODE = '" + pricerow[0].ToString().Trim()
//                                                + "' AND BVSPECPRICEPARTNO = '" + pricerow[2].ToString().Trim() + "' AND BVSPECPRICECODE = '" + pricerow[1].ToString().Trim() + "' ";
//                                            _tools.getSQL_void(@"DELETE FROM SPECIAL_PRICING  WHERE VENDOR_CODE =? AND BVSPECPRICEPARTNO =?  AND BVSPECPRICECODE =? ", dsn, new object[] { pricerow[0].ToString().Trim(), pricerow[2].ToString().Trim(), pricerow[1].ToString().Trim() });
//                                            emailmessage += dsn + " " + sql + " Same Date\n\n";

//                                        }
//                                    }
//                                    else if (dt1 > dt2)
//                                    {
//                                        sql = "UPDATE SPECIAL_PRICING SET SELLING_PRICE = " + pricerow[3] + ", BVRVMODDATE = '"
//                                            + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day
//                                            + "' WHERE VENDOR_CODE = '" + checkpricerow[0].ToString().Trim()
//                                            + "' AND BVSPECPRICEPARTNO = '" + checkpricerow[2].ToString().Trim()
//                                            + "' AND BVSPECPRICECODE = '" + checkpricerow[1].ToString().Trim() + "'";
//                                        _tools.getSQL_void(@"UPDATE SPECIAL_PRICING  SET SELLING_PRICE =?, BVRVMODDATE =?   WHERE VENDOR_CODE =? AND BVSPECPRICEPARTNO =?  AND BVSPECPRICECODE =? ", dsn, new object[] { pricerow[3].ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), checkpricerow[0].ToString().Trim(), checkpricerow[2].ToString().Trim(), checkpricerow[1].ToString().Trim() });
//                                        sql = "DELETE FROM SPECIAL_PRICING WHERE VENDOR_CODE = '" + pricerow[0].ToString().Trim()
//                                            + "' AND BVSPECPRICEPARTNO = '" + pricerow[2].ToString().Trim()
//                                            + "' AND BVSPECPRICECODE = '" + pricerow[1].ToString().Trim() + "'";
//                                        emailmessage += dsn + " " + sql + " Different Date\n\n";
//                                        _tools.getSQL_void(@"DELETE FROM SPECIAL_PRICING  WHERE VENDOR_CODE =? AND BVSPECPRICEPARTNO =?  AND BVSPECPRICECODE =? ", dsn, new object[] { pricerow[0].ToString().Trim(), pricerow[2].ToString().Trim(), pricerow[1].ToString().Trim() });

//                                    }
//                                    else
//                                    {
//                                        sql = "DELETE FROM SPECIAL_PRICING WHERE VENDOR_CODE = '" + pricerow[0].ToString().Trim()
//                                            + "' AND BVSPECPRICEPARTNO = '" + pricerow[2].ToString().Trim()
//                                            + "' AND BVSPECPRICECODE = '" + pricerow[1].ToString().Trim() + "'";
//                                        emailmessage += dsn + " " + sql + " Different Date\n\n";
//                                        _tools.getSQL_void(@"DELETE FROM SPECIAL_PRICING  WHERE VENDOR_CODE =? AND BVSPECPRICEPARTNO =?  AND BVSPECPRICECODE =? ", dsn, new object[] { pricerow[0].ToString().Trim(), pricerow[2].ToString().Trim(), pricerow[1].ToString().Trim() });

//                                    }

//                                }
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            IssueLines += ex.ToString();
//                            emailmessage += "There was an issue with" + row[8] + " " + IssueLines + "\n\n";
//                        }
//                    }

//                    //}

//                    // if (companyrow[0].ToString() == "8")
//                    //{
//                    var current_notes = "";
//                    try
//                    {
//                        current_notes = _tools.getSQL_string(@"SELECT notes FROM vendor  WHERE ven_no =?", dsn, new object[] { row[0] });
//                    }
//                    catch
//                    {
//                        emailmessage += "ERR- SELECT notes FROM vendor WHERE ven_no = " + row[0] + "\n\n";
//                        current_notes = "";
//                    }
//                    sql = "UPDATE VENDOR SET HOLD = 1, NOTES = '" + current_notes.Replace("'", "''").Trim() + " - MERGED WITH " + MergeVendNum + "' WHERE VEN_NO = '" + row[0] + "'";
//                    try
//                    {
//                        _tools.getSQL_void(@"UPDATE VENDOR  SET HOLD = 1, NOTES =CONCAT(?,' - MERGED WITH ',?) WHERE VEN_NO =?", dsn, new object[] { current_notes.Trim(), MergeVendNum, row[0] });
//                    }
//                    catch (Exception ee)
//                    {
//                        emailmessage += ee + "\n\n";
//                    }
//                    emailmessage += sql + "\n\n";

//                    //Update POs
//                    sql = "UPDATE PURCHASE_ORDER_HEADR SET VEND_NO = '" + MergeVendNum + "' WHERE VEND_NO = '" + row[0] + "'";
//                    try
//                    {
//                        _tools.getSQL_void(@"UPDATE PURCHASE_ORDER_HEADR  SET VEND_NO =?  WHERE VEND_NO =?", dsn, new object[] { MergeVendNum, row[0] });
//                    }
//                    catch (Exception ee)
//                    {
//                        emailmessage += ee + "\n\n";
//                    }
//                    emailmessage += sql + "\n\n";
//                    //}  

//                    // Update ADDRESS - RECORD_TYPE = 'SUPP', ID is CEV_NO
//                    sql = string.Format(@"UPDATE ADDRESS SET CEV_NO = '{1}' WHERE RECORD_TYPE = 'SUPP' AND CEV_NO = '{0}' ", row[0], MergeVendNum);
//                    try
//                    {
//                        //_tools.getSQL_void(@"UPDATE ADDRESS SET CEV_NO = ?  WHERE RECORD_TYPE = 'SUPP' AND CEV_NO = ?  ", dsn , new object[] { MergeVendNum, row[0]  } );
//                    }
//                    catch (Exception ee)
//                    {
//                        emailmessage += ee + "\n\n";
//                    }
//                    emailmessage += sql + "\n\n";
//                    // Update AP_TRANSACTIONS - ID is VEND
//                    sql = string.Format(@"UPDATE AP_TRANSACTIONS SET VEND = '{1}' WHERE VEND = '{0}' ", row[0], MergeVendNum);
//                    try
//                    {
//                        _tools.getSQL_void(@"UPDATE AP_TRANSACTIONS SET VEND = ?  WHERE VEND = ?  ", dsn, new object[] { MergeVendNum, row[0] });
//                    }
//                    catch (Exception ee)
//                    {
//                        emailmessage += ee + "\n\n";
//                    }
//                    emailmessage += sql + "\n\n";
//                    // Update BATCH_PAYABLES - ID is VENDOR
//                    sql = string.Format(@"UPDATE BATCH_PAYABLES SET VENDOR = '{1} WHERE VENDOR = '{0}' ", row[0], MergeVendNum);
//                    try
//                    {
//                        _tools.getSQL_void(@"UPDATE BATCH_PAYABLES SET VENDOR = ?  WHERE VENDOR = ?  ", dsn, new object[] { MergeVendNum, row[0] });
//                    }
//                    catch (Exception ee)
//                    {
//                        emailmessage += ee + "\n\n";
//                    }
//                    emailmessage += sql + "\n\n";
//                    // Update GL_TRANSACTIONS - MF_WHO = 'Vend.', MF_KEY = vendor_id
//                    sql = string.Format(@"UPDATE GL_TRANSACTIONS SET MF_KEY = '{1}' WHERE MF_WHO = 'Vend.' AND MF_KEY = '{0}' ", row[0], MergeVendNum);
//                    try
//                    {
//                        _tools.getSQL_void(@"UPDATE GL_TRANSACTIONS SET MF_KEY = ?  WHERE MF_WHO = 'Vend.' AND MF_KEY = ?  ", dsn, new object[] { MergeVendNum, row[0] });
//                    }
//                    catch (Exception ee)
//                    {
//                        emailmessage += ee + "\n\n";
//                    }
//                    emailmessage += sql + "\n\n";
//                    // Update PURCHASE_HISTORY_HDR - ID = VEND_NO 
//                    var vend_name = Toolbox.do_value_from(Toolbox.doSQL_string(@"SELECT vendor_name 
//FROM vendor WHERE vendor_number_int = @v0 LIMIT 1", MergeVendNum), false);
//                    sql = string.Format(@"UPDATE PURCHASE_HISTORY_HDR SET VEND_NO = '{1}', VEND_NAME = '{2}' WHERE VEND_NO = '{0}' ", row[0], MergeVendNum, vend_name);
//                    try
//                    {
//                        _tools.getSQL_void(@"UPDATE PURCHASE_HISTORY_HDR SET VEND_NO = ? , VEND_NAME = ?  WHERE VEND_NO = ?  ", dsn, new object[] { MergeVendNum, vend_name, row[0] });
//                    }
//                    catch (Exception ee)
//                    {
//                        emailmessage += ee + "\n\n";
//                    }
//                    emailmessage += sql + "\n\n";
//                }
//            }
//        }


//        Vendtable.InnerHtml += strErrorMessage;
//        emailmessage += "\n" + strErrorMessage;
//        InfoSave += emailmessage + "\n" + IssueLines;
//        merged_to_vendor.sync_bvs(myMember);
//        var mail = new NeEMail();
//        mail.To = "ap@newelectric.com";
//        mail.CC = "debug@newelectric.com";
//        mail.Subject = "Vendors Have Been Merged";
//        mail.From = "noreply@newelectric.com";
//        mail.Body = emailShortBody + "\n" + BVVendTabTable;
//        mail.Send();
//        //InfoSave = InfoSave.Replace("'", "");
//        var sqlSave = "INSERT INTO vendor_merge_history (history,dateadded) VALUES(@v0, now())";
//        try
//        {
//            _tools.getSQL_void(sqlSave, new object[] { InfoSave });
//        }
//        catch
//        {
//            try
//            {
//                mail.To = "mhyde@thatsnew.com";
//                mail.Subject = "Error in Info Save";
//                mail.Body = InfoSave;
//                mail.Send();
//            }
//            catch { }
//        }

//    }
//    protected string specialpricingadd()
//    {
//        _tools.getSQL_void(@" INSERT INTO SPECIAL_PRICING ( BVSPECPRICESOURCEID, BVSPECPRICEWHSE, BVSPECPRICECODE, BVSPECPRICEADDRID, BVSPECPRICEADDRTYPE, 
//BVSPECPRICEPARTNO, SELLING_PRICE, BVBREAKQTY01, BVBREAKQTY02, BVBREAKQTY03, BVBREAKQTY04, BVBREAKQTY05, BVBREAKQTY06, BVBREAKQTY07, 
//BVBREAKQTY08, BVBREAKQTY09, BVBREAKQTY10, BVBREAKQTY11, BVBREAKQTY12, BVBREAKQTY13, BVBREAKQTY14, BVBREAKQTY15, BVBREAKPRICE01, 
//BVBREAKPRICE02, BVBREAKPRICE03, BVBREAKPRICE04, BVBREAKPRICE05, BVBREAKPRICE06, BVBREAKPRICE07, BVBREAKPRICE08, BVBREAKPRICE09, 
//BVBREAKPRICE10, BVBREAKPRICE11, BVBREAKPRICE12, BVBREAKPRICE13, BVBREAKPRICE14, BVBREAKPRICE15, USE_LOWEST_PRICE, STARTING_DATE,
//ENDING_DATE, AUTO_ERASE_RECORD, IGNORE_PDM_DISCOUNT,
//RECORD_MODIFIED, VENDOR_CODE, LEAD_TIME, BVRVVERSION, BVRVMODDATE, 
//BVRVMODTIME, BVRVUSERINIT, BVRVADDDATE, BVRVADDTIME, BVRVADDUSERINIT, 
//BVEXCHARKEY1, BVEXCHARKEY2, BVEXCHARFIELD1, BVEXCHARFIELD2, BVEXAMTKEY1,
//BVEXAMTKEY2, BVEXAMTFIELD1, BVEXAMTFIELD2, BVEXPRICEKEY1, BVEXPRICEKEY2, BVEXPRICEFIELD1, 
//BVEXPRICEFIELD2, BVEXCHARFLAGKEY1, BVEXCHARFLAGKEY2, BVEXCHARFLAG1, BVEXCHARFLAG2, BVEXCHARFLAG3,
//BVEXCHARFLAG4, BVEXCHARFLAG5, BVEXCHARFLAG6, BVEXNUMFLAGKEY1, BVEXNUMFLAGKEY2, BVEXNUMFLAG1,
//BVEXNUMFLAG2, BVEXNUMFLAG3, BVEXNUMFLAG4, BVEXNUMFLAG5, BVEXNUMFLAG6 ) VALUES
//( ? , ? , ? , '', '', ? , ? , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
//0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, ? , ? , ? ,
//? , 0, ? , '', ? , '', '', '', ? , ? , ? , '', '', 
//0.00, 0.00, 0.00, 0.00, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 
//'', '', '', '', '', '', '', '', 0, 0, 0, 0, 0, 0, 0, 0, 0 ) ", _dsn,
//new object[] {  _bvspecpricesourceid, _bvspecpricewhse, _bvspecpricecode, _bvspecpricepartno,_selling_price,
//    _starting_date, _ending_date, _auto_erase_record, _ignore_pdm_discount, _vendor_code,
//    _bvrvversion, _bvrvadddate, _bvrvaddtime, _bvrvadduserinit  });

//        return string.Format(@"
//INSERT INTO SPECIAL_PRICING
//	(
//	BVSPECPRICESOURCEID,			
//	BVSPECPRICEWHSE,			
//	BVSPECPRICECODE,			
//	BVSPECPRICEADDRID,				
//	BVSPECPRICEADDRTYPE,		
//	BVSPECPRICEPARTNO,			
//	SELLING_PRICE,
//	BVBREAKQTY01,				
//	BVBREAKQTY02,				
//	BVBREAKQTY03,				
//	BVBREAKQTY04,				
//	BVBREAKQTY05,				
//	BVBREAKQTY06,				
//	BVBREAKQTY07,				
//	BVBREAKQTY08,				
//	BVBREAKQTY09,				
//	BVBREAKQTY10,				
//	BVBREAKQTY11,				
//	BVBREAKQTY12,				
//	BVBREAKQTY13,				
//	BVBREAKQTY14,				
//	BVBREAKQTY15,
//	BVBREAKPRICE01,				
//	BVBREAKPRICE02,				
//	BVBREAKPRICE03,				
//	BVBREAKPRICE04,				
//	BVBREAKPRICE05,				
//	BVBREAKPRICE06,				
//	BVBREAKPRICE07,				
//	BVBREAKPRICE08,				
//	BVBREAKPRICE09,				
//	BVBREAKPRICE10,				
//	BVBREAKPRICE11,				
//	BVBREAKPRICE12,				
//	BVBREAKPRICE13,				
//	BVBREAKPRICE14,				
//	BVBREAKPRICE15,
//	USE_LOWEST_PRICE,			
//	STARTING_DATE,				
//	ENDING_DATE,				
//	AUTO_ERASE_RECORD,			
//	IGNORE_PDM_DISCOUNT,		
//	RECORD_MODIFIED,			
//	VENDOR_CODE,				
//	LEAD_TIME,					
//	BVRVVERSION,				
//	BVRVMODDATE,				
//	BVRVMODTIME,				
//	BVRVUSERINIT,				
//	BVRVADDDATE,				
//	BVRVADDTIME,				
//	BVRVADDUSERINIT,
//	BVEXCHARKEY1,				
//	BVEXCHARKEY2,				
//	BVEXCHARFIELD1,				
//	BVEXCHARFIELD2,				
//	BVEXAMTKEY1,				
//	BVEXAMTKEY2,				
//	BVEXAMTFIELD1,				
//	BVEXAMTFIELD2,
//	BVEXPRICEKEY1,				
//	BVEXPRICEKEY2,				
//	BVEXPRICEFIELD1,
//	BVEXPRICEFIELD2,			
//	BVEXCHARFLAGKEY1,			
//	BVEXCHARFLAGKEY2,			
//	BVEXCHARFLAG1,
//	BVEXCHARFLAG2,				
//	BVEXCHARFLAG3,				
//	BVEXCHARFLAG4,				
//	BVEXCHARFLAG5,				
//	BVEXCHARFLAG6,				
//	BVEXNUMFLAGKEY1,			
//	BVEXNUMFLAGKEY2,
//	BVEXNUMFLAG1,				
//	BVEXNUMFLAG2,				
//	BVEXNUMFLAG3,				
//	BVEXNUMFLAG4,				
//	BVEXNUMFLAG5,				
//	BVEXNUMFLAG6				
//	)
//VALUES
//	(
//	'{0}',		
//	'{1}',		
//	'{2}',		
//	'',			
//	'',			
//	'{13}',			
//	{3},		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,
//	0,		
//	'{4}',		
//	'{5}',		
//	'{6}',		
//	'{7}',		
//	0,		
//	'{8}',		
//	'',			
//	'{9}',		
//	'',			
//	'',			
//	'',			
//	'{10}',		
//	'{11}',		
//	'{12}',		
//	'',			
//	'',			
//	0.00,		
//	0.00,		
//	0.00,		
//	0.00,		
//	0.00000,	
//	0.00000,	
//	0.00000,	
//	0.00000,	
//	0.00000,	
//	'',			
//	'',			
//	'',			
//	'',			
//	'',			
//	'',			
//	'',			
//	'',			
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0,		
//	0			
//	)		
//",
//    _bvspecpricesourceid,       // {0}
//    _bvspecpricewhse,           // {1}
//    _bvspecpricecode,           // {2}
//    _selling_price,             // {3}
//    _starting_date,             // {4}
//    _ending_date,               // {5}
//    _auto_erase_record,         // {6}
//    _ignore_pdm_discount,       // {7}
//    _vendor_code,               // {8}
//    _bvrvversion,               // {9}
//    _bvrvadddate,               // {10}
//    _bvrvaddtime,               // {11}
//    _bvrvadduserinit,           // {12}
//    _bvspecpricepartno          // {13}
//    );

//    }
}
