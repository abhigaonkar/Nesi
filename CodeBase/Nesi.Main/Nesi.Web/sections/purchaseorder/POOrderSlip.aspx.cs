using System;
using System.Data;
using System.Net.Mail;
using System.IO;
using System.Web.UI;
using DevExpress.XtraPrinting.Drawing;
//using nesi.bv;
using nesi.core;
using nesi.core.print;

public partial class sections_purchaseorder_POOrderSlip : System.Web.UI.Page
{
    int poprogid = 0;
    Toolbox _tools = new Toolbox();
    NeMember myMember = new NeMember();
    NePOProg this_po;
    bool TransferFlag = true;
    bool print_preview = true;

    protected void Page_Init(object sender, EventArgs e)
    {
        myMember = Toolbox.do_handle_authentication(1);
        var poid = Request.QueryString["PO"];
        print_preview = Request.QueryString["pp"]=="true";
        poprogid = Convert.ToInt32(poid);
        this_po = new NePOProg(Convert.ToInt32(poprogid));
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        //myMember = new NeMember(589);
        //string poid = "37";


        if (!IsPostBack)
        {
            if ((this_po.poprog_status < 7) &&(!print_preview))
            {
                UpdateStatus();
                this_po = new NePOProg(this_po.poprog_id);
            }

            var company = new NeBusinessUnit(this_po.business_unit_id);
            var sql = "SELECT DISTINCT(po_details_woprog_id) FROM po_details_current WHERE po_details_poprog_id=" + poprogid + " and is_gl_account=false";
            var pmid = "";
            var emailstring = "";
            var emailcheck = "";
            var pms = _tools.getSQL_datatable(@"SELECT DISTINCT(po_details_woprog_id) FROM po_details_current  WHERE po_details_poprog_id=@v0 and is_gl_account = false", new object[] { poprogid.ToString() });
            foreach (DataRow pmrow in pms.Rows)
            {

                try
                {
                    pmid = _tools.getSQL_string(@"Select ifnull((SELECT WOProg_PM_MemberID FROM woprog  WHERE woprog_id =@v0),'') ", new object[] { pmrow[0] });
                    if (pmid != "")
                    {

                        emailcheck = _tools.getSQL_string(@"Select ifnull((SELECT Member_NEEmail FROM member  WHERE member_id =@v0),'') ", new object[] { pmid });
                        if (emailcheck != "")
                        {
                            emailstring += emailcheck + ";";
                        }
                    }
                }
                catch
                {
                }
            }
            var vendnamesel = "SELECT Vendor_Name FROM VENDOR WHERE vendor_id = " + this_po.poprog_vendor_id;
            var wonumsel = "Select ifnull((SELECT woprog_bvwo FROM woprog WHERE woprog_id = " + this_po.poprog_woprog_id + "),'')";
            var vendoremail = new DataTable();
            try
            {
                vendoremail = _tools.getSQL_datatable(@" SELECT member_id id, CONCAT(member_neemail, ' ', member_fullname) email FROM member WHERE member_id = @v1  UNION SELECT 1 id, CONCAT(Address_ContactEmail1, ' ', Address_ContactName1) email FROM Address WHERE Address_ContactEmail1 IS NOT NULL AND Address_ContactEmail1 != '' AND Address_Table = 'Vendor' AND Address_Table_ID = @v0  UNION SELECT 2 id, CONCAT(Address_ContactEmail2, ' ', Address_ContactName2) email FROM Address WHERE Address_ContactEmail2 IS NOT NULL AND Address_ContactEmail2 != '' AND Address_Table = 'Vendor' AND Address_Table_ID = @v0  UNION SELECT 3 id, CONCAT(Address_ContactEmail3, ' ', Address_ContactName2) email FROM Address WHERE Address_ContactEmail3 IS NOT NULL AND Address_ContactEmail3 != '' AND Address_Table = 'Vendor' AND Address_Table_ID = @v0  UNION SELECT contact_id id, CONCAT(contact_email, ' ',Contact_Name) as email FROM contact WHERE contact_cust_id = @v0  AND contact_status='Active' AND contact_type = 'Vendor'", new object[] { this_po.poprog_vendor_id, myMember.id });
            }
            catch
            {
                // ignored
            }
            var trimmedponum = "";
            var trimmedwonum = "";
            trimmedponum = this_po.poprog_bvpo.TrimStart('0');
            try
            {

                trimmedwonum = _tools.getSQL_string(@"Select ifnull((SELECT woprog_bvwo FROM woprog  WHERE woprog_id =@v0),'') ", new object[] { this_po.poprog_woprog_id }).TrimStart('0');
            }
            catch
            {
                // ignored
            }

            txtFromEmailAddress.Text = myMember.NEEmail;
            try
            {
                cbEmailList.DataSource = vendoremail;
                cbEmailList.Value = myMember.id.ToString();
                cbEmailList.DataBind();
            }
            catch
            {
                cbEmailList.ClientEnabled = false;
            }

            txtEmailCC.Text = emailstring;

            if (this_po.poprog_status == 1 && !print_preview)
            {
                txtSubject.Text = "RFQ - " + _tools.getSQL_string(@"SELECT Vendor_Name FROM VENDOR  WHERE vendor_id =@v0", new object[] { this_po.poprog_vendor_id });
                memoBody.Text += "Please review the attached request for quotation note the expected delivery date.\n\n\n";
                memoBody.Text += "We would like to have these items on or before " + Convert.ToDateTime(this_po.poprog_expected_received_date).ToShortDateString() + "\n\n";
            }
            else
            {
                txtSubject.Text = (!print_preview?"Purchase Request" : "Print Preview") + " - PO Number " + company.id.ToString().PadLeft(3, '0') + trimmedponum + " - " + _tools.getSQL_string(@"SELECT Vendor_Name FROM VENDOR  WHERE vendor_id =@v0", new object[] { this_po.poprog_vendor_id });
                if (this_po.poprog_shipping_method == 2)  // if we are picking up the parts.. change the body of the email
                {
                    memoBody.Text += @"
Review the attached purchase order and call or email if the parts are NOT ready for pickup.
** Please reply to this email with an order confirmation, when the order has been placed. **

Also, please make sure your invoices will match our purchase orders.  Payment for matching invoices will be processed at a higher priority than others.

We are expecting to pick these items up on or before " + Convert.ToDateTime(this_po.poprog_expected_received_date).ToShortDateString() + "\n\n";
                }
                memoBody.Text += @"
Review the attached purchase order and call or email if the expected delivery date is unattainable or the costs are incorrect.  Invoices not matching our purchase orders will be processed at a lower priority.
** Please reply to this email with an order confirmation, when the order has been placed. **

We are expecting these items on or before " + Convert.ToDateTime(this_po.poprog_expected_received_date).ToShortDateString() + "\n\n";
            }
            memoBody.Text += "Best Regards,\n\n";

            lblPOStatusDisp.Text = _tools.getSQL_string(@"SELECT status_type FROM poprog_status  WHERE poprog_status_id =@v0", new object[] { this_po.poprog_status.ToString() });

        }

        txtmyemail.Text = myMember.NEEmail;
        bindreport();

    }

    private void bindreport()
    {
        try
        {
            var report = new popheaderdisp();
			report.Name = $"PO - {this_po.poprog_id}";
            report.Parameters[0].Value = poprogid;

            if (this_po.poprog_status >= 7)
            {
                report.Watermark.Text = "Duplicate Copy";
                report.Watermark.ShowBehind = false;
                report.Watermark.ForeColor = System.Drawing.Color.Red;
                report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                report.Watermark.Font = new System.Drawing.Font(report.Watermark.Font.FontFamily, 50);
                report.Watermark.TextTransparency = 175;
            }
            ReportViewer1.Report = report;
            ReportViewer1.DataBind();
        }
        catch (Exception ex)
        {

            //throw new Exception(ex.ToString());
            lblError.Visible = true;
            lblError.Text = ex.ToString();

        }
    }

    protected void UpdateStatus()
    {
        var poprog = new NePOProg(poprogid);

        var company = new NeBusinessUnit(Convert.ToInt32(poprog.business_unit_id.ToString()));

        var TransferMessage = " was ordered.";

			var doBv = company.DSN != "";
				var dsn = company.DSN;
        if (poprog.poprog_status.ToString() == "5")
        {
            //Status 5, apporved for ordering
            //Check to See if PO is Locked
			//if(doBv)
			//	{
			//	var lockinfo = PurchaseOrder.CheckPOLock(dsn, poprog.poprog_bvpo);
			//	if (lockinfo != "")
			//		{
			//		throw new Exception("This PO is Locked By User " + lockinfo + " and cannot be printed at this moment");
			//		}
   //             PurchaseOrder.LockOrder(dsn, poprog.poprog_bvpo, myMember.Initials);
			//	}
            try
            {
				try
					{
					ValidateWO(poprogid.ToString());
					}
				catch (Exception ex1)
					{
					ReportViewer1.Visible = false;
					//if(doBv)
					//	{
					//	var podc = new NEPO_Details_Current();
					//	podc.MoveToBVPORecord(poprogid.ToString(), myMember.id.ToString());
					//	throw new Exception("An associated work order is in an advanced status, and cannot be changed, BV has been updated though.");
					//	}
					throw new Exception("An associated work order is in an advanced status, and cannot be changed.");
					}

				var InValidPartCount = _tools.getSQL_int(@"SELECT pro_check_valid_po_lines(@v0)", new object[] { poprogid });
                if (InValidPartCount > 0)
                    {
                    ReportViewer1.Visible = false;
                    throw new Exception("Parts on this PO either do not exist or are no longer active");
					}

                /*
								try 
								{

								 MoveToBVPORecord();
								}
								catch (Exception ex2)
								{
									if (ex2.ToString().Contains("Possible Vendor Part Number Duplication"))
									{
										throw new Exception(ex2.ToString());
									}
									else if (ex2.ToString().Contains("currently exists in these New Electric"))
									{
										throw new Exception(ex2.ToString());
									}
									else
									{
										throw new Exception("Could not update BV PO "+ex2);
									}
								}
				*/
                //MoveToWOProgDetails(poprog.business_unit_id.ToString(), poprog.poprog_bvpo);
                //This Work Order was Cut and is now being ordered.
                //Change Status and move to BV.
                if (TransferFlag)
                {
                    try
                    {
                        _tools.getSQL_void(@"UPDATE poprog_header SET poprog_status = 3, poprog_order_placed_date = now() WHERE poprog_id = @v0", new object[] { poprogid.ToString() });

                        try
                        {
                            NePOProg.status_log.add(poprogid, myMember.id32, 3);
                        }
                        catch
                        {
                            new Exception("Could not add row to PO status table");
                        }

                        NePOProg.send_email_to_trackers(poprog.poprog_id.ToString(), poprog.business_unit_id.ToString().PadLeft(3, '0') + "-" + poprog.poprog_bvpo.TrimStart('0') + " has been cut for parts you have requested", myMember, "");
                    }
                    catch
                    {
                        throw new Exception("Could not update PO to status 'issued'");
                    }



                    _tools.getSQL_void(@"UPDATE po_details_current                     SET po_details_line_active = 0 WHERE po_details_part_no = 0                    AND po_details_poprog_id = @v0", new object[] { poprogid.ToString() });
                }
                else
                {
                    TransferMessage = TransferMessage + " but due to not all parts transferring it's status remains approved for order.";
                }

                try
                {
                    _tools.getSQL_void(@"INSERT INTO poprog_notes (poprog_id, eventtext, date, member_id) VALUES(@v0, CONCAT('Purchase Order ', @v0,' - ',@v1),now(), @v2)", new object[] { poprogid, poprog.poprog_bvpo + TransferMessage, myMember.id });
                }
                catch
                {
                    throw new Exception("Could not add notes to the PO");
                }



            }
            catch (Exception ex)
            {

                lblError.Visible = true;
                lblError.Text = ex.Message;

            }
            finally
            {
				//if(doBv)
				//	{
				//	PurchaseOrder.UnlockPO(dsn, poprog.poprog_bvpo, myMember.Initials);
				//	}
            }

        }
		//else if(!Toolbox.Contains(poprog.poprog_status, new [] {7,9}) && doBv)
		//	{
		//	var hitBVHistory = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM poprogstatus WHERE poprogstatus_poprog_id = @v0 AND poprogstatus_status_id = 7", new object[] {poprog.poprog_id }) > 0;
		//	if(!hitBVHistory)
		//		{
		//		var podc = new NEPO_Details_Current();
		//		podc.MoveToBVPORecord(poprogid.ToString(), myMember.id.ToString());
		//		}
		//	}
    }


    protected void MoveToBVPORecord()
    {

        var poprog = new NePOProg(poprogid);
        var newinv = new inventory();
        var vprow = new vendor_price_row();
    double oldtotal = 0;
			var bu = new NeBusinessUnit( poprog.business_unit_id);
    var Details = _tools.getSQL_datatable(@" SELECT po_details_id, po_details_part_no, po_details_vendor_part_no, po_details_description, po_details_woprog_id, po_details_qty_ordered, po_details_qty_received, po_details_rec_no, po_details_cost, po_details_tax1, po_details_tax2, po_details_tax3, po_details_tax4, po_details_date_expected, po_details_notes, po_details_vendor_qty_per, is_gl_account FROM po_details_current  WHERE po_details_poprog_id=@v0", new object[] { poprogid.ToString() });
        foreach (DataRow row in Details.Rows)
        {
            var id = "0";
            var oldpartval = "";
            double oldqtyperval = 0;
            var newrealcost = Convert.ToDouble(row["po_details_cost"].ToString()) / Convert.ToDouble(row["po_details_vendor_qty_per"].ToString());
            if (row["po_details_part_no"].ToString() != "0")
                newinv.Load(row["po_details_part_no"].ToString(), bu.warehouse_bu_id);
            if (!newinv.is_exclude && newinv.tag_id != "843" && row["po_details_part_no"].ToString() != "0")
            {
                var priceinfo = _tools.getSQL_datatable(@"SELECT * FROM inventory_price  WHERE master_id =@v0 AND vendor_id =@v1  AND vendor_code =@v2  AND business_unit_id =@v3 ",
                    new object[] { row["po_details_part_no"].ToString(),poprog.poprog_vendor_id,row["po_details_vendor_part_no"],
                     bu.warehouse_bu_id });
                foreach (DataRow row2 in priceinfo.Rows)
                {
                    id = row2["id"].ToString();
                    oldpartval = _tools.value_from(row2["vendor_code"], false);
                    oldqtyperval = Convert.ToDouble(row2["qty"].ToString());
                    oldtotal = Convert.ToDouble(row2["total"].ToString());
                    
                }
                if (oldpartval != row["po_details_vendor_part_no"].ToString()
                  || oldtotal != Convert.ToDouble(row["po_details_cost"].ToString())
                  || oldqtyperval != Convert.ToDouble(row["po_details_vendor_qty_per"].ToString()))
                {
                    //Test to see if Vednor Part Number already exists
                    if (id != "0")
                    {
                        vprow.price_id = id;
                        vprow.master_id = row["po_details_part_no"].ToString();
                        vprow.member_id = myMember.id;
                        vprow.vendor_code = row["po_details_vendor_part_no"].ToString();
                        vprow.business_unit_id =  bu.warehouse_bu_id;
                        vprow.cost = newrealcost;
                        vprow.total = row["po_details_cost"].ToString();
                        vprow.qty = row["po_details_vendor_qty_per"].ToString();
                        vprow.vendor_id = poprog.poprog_vendor_id;
                        vprow.origin = "PO: " + poprog.poprog_bvpo;
                        vprow.save();
                    }
                    else
                    {
                        vprow.price_id = null;
                        vprow.master_id = row["po_details_part_no"].ToString();
                        vprow.member_id = myMember.id;
                        vprow.vendor_code = row["po_details_vendor_part_no"].ToString();
                        vprow.business_unit_id =  bu.warehouse_bu_id;
                        vprow.cost = newrealcost;
                        vprow.total = row["po_details_cost"].ToString();
                        vprow.qty = row["po_details_vendor_qty_per"].ToString();
                        vprow.vendor_id = poprog.poprog_vendor_id;
                        vprow.origin = "PO: " + poprog.poprog_bvpo;
                        try
                        {
                            vprow.save();
                        }
                        catch (Exception exvp)
                        {
                            if (exvp.ToString().Contains("Duplicate Vendor Part Number"))
                            {
                                throw new Exception(string.Format(@"Possible Vendor Part Number Duplication please check part number {0} to make sure it does not use an already existing vendor part number ({1}) for this vendor", row["po_details_part_no"], row["po_details_vendor_part_no"]));
                            }
                            if (exvp.ToString().Contains("Vendor Code not set"))
                            {
                                throw new Exception(string.Format("Master ID {0}, Rec No: {1} - Doesn't have a vendor part number set.", vprow.master_id, row["po_details_rec_no"]));
                            }
                            else
                            {
                                throw new Exception(exvp.ToString());
                            }

                        }

                    }
                }
                else
                {
                    vprow.price_id = id;
                    vprow.master_id = row["po_details_part_no"].ToString();
                    vprow.member_id = myMember.id;
                    vprow.vendor_code = row["po_details_vendor_part_no"].ToString();
                    vprow.business_unit_id =  bu.warehouse_bu_id;
                    vprow.cost = newrealcost;
                    vprow.total = row["po_details_cost"].ToString();
                    vprow.qty = row["po_details_vendor_qty_per"].ToString();
                    vprow.vendor_id = poprog.poprog_vendor_id;
                    vprow.origin = "PO: " + poprog.poprog_bvpo;
                    vprow.save();
                }



                //Update Sell Price

                newinv.Load(row["po_details_part_no"].ToString(),  bu.warehouse_bu_id);
                newinv.get_costs(row["po_details_part_no"].ToString(),  bu.warehouse_bu_id);
                


            }

            try
            {
                var arrdate = row["po_details_date_expected"].ToString();
                var date = Convert.ToDateTime(arrdate);
                arrdate = date.ToString("yyyy-MM-dd");
                PartTrackingEmails(row["po_details_part_no"].ToString(),
                    poprog.business_unit_id.ToString(),
                    row["po_details_woprog_id"].ToString(),
                    Convert.ToDouble(row["po_details_qty_ordered"].ToString()),
                    row["po_details_description"].ToString(),
                    arrdate, row["po_details_notes"].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

    }
    #region Part Tracking Emails
    //Send out Part Tracking Emails
    protected void PartTrackingEmails(string masterid, string _business_unit_id, string woprogid, double qtyord, string partdesc, string arrdate, string notes)
    {
        //get members who are tracking this part

        var mail = new NeEMail();
        var memberstable = _tools.getSQL_datatable(@"SELECT DISTINCT(wo_detail_current_added_by) FROM wo_detail_current WHERE wo_detail_current_qty_committed < wo_detail_current_qty_ordered AND wo_detail_current_track_part = 1 AND wo_detail_current_master_id = @v0  AND business_unit_id =@v1  GROUP BY wo_detail_current_added_by", new object[] { masterid, _business_unit_id });

        var testid = "";
        var GLAccName = "";
        try
        {
            testid = _tools.getSQL_string(@"SELECT gl_te.id FROM gl_te WHERE gl_te.id = @v0 ", new object[] { woprogid });
            GLAccName = _tools.getSQL_string(@"SELECT gl_te.account_no FROM gl_te WHERE gl_te.id = @v0 ", new object[] { woprogid });
        }
        catch { }
        var bvwo = "";
        if (woprogid == "9999999" || woprogid == "9999998" || woprogid == "9999997")
            bvwo = "stock";
        else if (testid != "")
            bvwo = "GL Account-" + GLAccName;
        else
            bvwo = new NeWOProg(Convert.ToInt32(woprogid)).OrderNumber;
        var poprog = new NePOProg(poprogid);
        foreach (DataRow memberrow in memberstable.Rows)
        {
            var informmember = new NeMember(Convert.ToInt32(memberrow[0].ToString()));

            var message = string.Format(@"You requested that part {0} be tracked for you.
{3} of Part {0} have been ordered on Purchase Order {1} for {2}." + System.Environment.NewLine + "Notes: ",
                                                                            masterid,
                                                                            poprog.business_unit_id.ToString().PadLeft(3, '0') + "-" + poprog.poprog_bvpo.TrimStart('0'),
                                                                            bvwo,
                                                                            qtyord,
                                                                            _tools.value_from(notes, true));

            var subject = string.Format(@"{0} {3}. Qty: {2} has been ordered on PO {1}. Expected Arrival Date: {5}. " + System.Environment.NewLine + "Notes: ",
                                                                            masterid,
                                                                            poprog.business_unit_id.ToString().PadLeft(3, '0') + "-" + poprog.poprog_bvpo.TrimStart('0'),
                                                                            qtyord,
                                                                            partdesc,
                                                                            bvwo,
                                                                            arrdate, _tools.value_from(notes, true));

            if (informmember.NEEmail != "")
            {
                mail.To = informmember.NEEmail;
                mail.Subject = subject;
                mail.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
                mail.Body = "";
                try
                {
                    mail.Send();
                }
                catch
                {
                }
            }
        }

    }
    #endregion

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        var temp_po = new NePOProg(Convert.ToInt32(poprogid));
        var report = new popheaderdisp();
        report.Parameters[0].Value = poprogid;
      
        this.ReportViewer1.Report = report;
        this.ReportViewer1.DataBind();
        if (cbEmailList.Text.Split(' ')[0].Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please supply a TO address for this email');", true);
            return;
        }
        var file = new MemoryStream();
        report.CreateDocument(false);
        report.ExportToPdf(file);
        file.Seek(0, SeekOrigin.Begin);

        var filename = (temp_po.poprog_status == 1 && !print_preview)
                            ? "RFQ.pdf"
                            : (!print_preview?"Purchase Order - ":"Print Preview - ") + temp_po.business_unit_id.ToString().PadLeft(3, '0') + "-" + temp_po.poprog_bvpo.TrimStart('0') + ".pdf";
        var m = new NeEMail
        {
            From = txtmyemail.Text,
            To = cbEmailList.Text.Split(' ')[0],
            CC = chksendtome.Checked
                            ? txtmyemail.Text + ";" + txtEmailCC.Text
                            : txtEmailCC.Text,
            Bcc = txtEmailBCC.Text != ""
                            ? txtEmailBCC.Text
                            : "",
            Subject = txtSubject.Text,
            Body = memoBody.Text,
            Attachment = new Attachment(file, filename)
        };
        try
        {
            m.Send();
            temp_po.notes_poprogid = temp_po.poprog_id;
            temp_po.notes_memberid = myMember.id;
            temp_po.eventtext = string.Format("Email sent -- {{To: '{0}', From: '{1}', CC: '{2}', BCC: '{3}', Subject: '{4}', Filename: '{5}'}}", m.To, m.From, m.CC, m.Bcc, m.Subject, filename);
            temp_po.SaveNotes();
            popupemail.ShowOnPageLoad = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successfully emailed " + filename + ".');", true);
        }
        catch (Exception ee)
        {
            Toolbox.FriendlyException(Response, "There were problems sending your email, the message returned from the server was:" + ee.Message, Request.RawUrl);
        }

    }

    protected void sendpartmessage(string part, string bvpo)
    {
        //Send Message concerning part
        var message_ = new NeEMail();
        message_.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
        message_.To = "debug@" + Toolbox.app_setting("DomainForEmail");
        message_.Subject = "Part " + part + " did not transfer";
        message_.Body = "Part " + part + " on PO " + bvpo + " does not exist and was not transferred";
        message_.Send();

    }
    protected void ValidateWO(string poid)
    {
        //Check WO Existance
        DataTable CheckTable;
        try
        {
            // first check for WO rows at all
            CheckTable = _tools.getSQL_datatable(@"SELECT DISTINCT po_details_woprog_id FROM po_details_current  WHERE po_details_poprog_id =@v0 and po_details_woprog_id not in (9999999,9999998,9999997) and po_details_current.is_gl_account = false ", new object[] { poid });
        if (CheckTable.Rows.Count > 0)
            {
// if there are, check for bad status
            var wolist = "";
            var invalidwos = _tools.getSQL_datatable(@" SELECT woprog_bvwo n FROM woprog WHERE 
 woprog_id IN (SELECT po_details_woprog_id FROM po_details_current WHERE po_details_poprog_id = @v0 and po_details_current.is_gl_account = false and po_details_woprog_id not in (9999999,9999998,9999997)  ) 
AND woprog_status IN ('Invoiced', 'Waiting To Be Invoiced', 'Hold', 'Waiting For PO', 'Waiting BM Approval')",
                new object[] {poid});
            foreach (DataRow row in invalidwos.Rows)
                {
                wolist += "WO Status does not allow additional items: " + row[0] + " " + System.Environment.NewLine;
                }
            invalidwos = _tools.getSQL_datatable(@" 
SELECT po_details_woprog_id 
FROM po_details_current 
WHERE po_details_poprog_id = @v0 and po_details_current.is_gl_account = false and po_details_woprog_id not in (9999999,9999998,9999997) and po_details_woprog_id not in ( select woprog_id from woprog)


", new object[] {poid});
            foreach (DataRow row in invalidwos.Rows)
                {
                wolist += "WO Does Not Exist: " + row[0] + " " + System.Environment.NewLine;
                }


            if (wolist != "")
                {
                throw new Exception(wolist);
                }


            }
        }
        catch (Exception ee)
        {
            throw new Exception(ee.Message);
        }

        var woidtotest = "";
        var issues = "";
        foreach (DataRow row in CheckTable.Rows)
        {
            if (_tools.getSQL_int(@"SELECT count(woprog_id) FROM woprog  WHERE woprog_id=@v0", new object[] { row[0].ToString() }) == 0)
            {
              
                    if (row[0].ToString() != "9999999" && row[0].ToString() != "9999998" && row[0].ToString() != "9999997")
                        issues += row[0] + " ";

            }

        }
        if (issues != "")
        {
            throw new Exception("There is An Issue with the Work Orders that this PO is transferring to, check and make sure they Exist: The work order IDs are " + issues);
        }
        //Check WO Status
        var ValidWoForPO = @"SELECT ifnull(COUNT(*),0) FROM woprog WHERE woprog_id > 10000 AND  woprog_id IN
        (SELECT DISTINCT po_details_woprog_id FROM po_details_current WHERE po_details_poprog_id = " + poid + @") and po_details_current.is_gl_account = false 
        AND (woprog_status = 'Invoiced' OR woprog_status = 'Waiting To Be Invoiced' OR woprog_status = 'Hold'
        OR woprog_status = 'Waiting For PO' OR woprog_status = 'Waiting BM Approval')";

        var InvalidCount = _tools.getSQL_int(@"SELECT ifnull(COUNT(*),0) FROM woprog  WHERE woprog_id > 10000 
AND woprog_id IN (SELECT DISTINCT po_details_woprog_id 
FROM po_details_current WHERE po_details_poprog_id =@v0 and po_details_current.is_gl_account = false  ) 
AND (woprog_status = 'Invoiced' OR woprog_status = 'Waiting To Be Invoiced' OR woprog_status = 'Hold' OR woprog_status = 'Waiting For PO' OR woprog_status = 'Waiting BM Approval')", new object[] { poid });
        if (InvalidCount > 0)
        {
            var wolist = "";
            var invalidwos = _tools.getSQL_datatable(@" SELECT woprog_bvwo n FROM woprog WHERE woprog_id > 10000 
AND woprog_id IN (SELECT po_details_woprog_id FROM po_details_current WHERE po_details_poprog_id = @v0 and po_details_current.is_gl_account = false  ) 
AND woprog_status IN ('Invoiced', 'Waiting To Be Invoiced', 'Hold', 'Waiting For PO', 'Waiting BM Approval')", new object[] { poid });
            foreach (DataRow row in invalidwos.Rows)
            {
                wolist += row[0] + " ";
            }
            throw new Exception("There is an issue with the work orders that this PO is transferring to, check " + wolist + " work orders' Status");
        }


    }
}
