using System;
using System.Data;
using System.IO;
using DevExpress.XtraPrinting;
using System.Collections.Specialized;
using nesi.core;
using nesi.core.print;

public partial class sections_hr_member_offer_print_off : System.Web.UI.Page
{
	NeMember current_user;
	NeMemberOffer mo;
	bool can_see_wage;
	bool can_see_all_offers;
	bool can_edit_offers;
   bool isowner = false;
	int jd;
	protected void Page_Load(object sender, EventArgs e)
	{
		var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(1);

		can_see_all_offers = current_user.AuthenticatedForPrivilege(152);
		can_edit_offers = current_user.AuthenticatedForPrivilege(129);
		var _q = Request.QueryString;
		var moid = string.IsNullOrEmpty(_q["moid"]) || _q["moid"] == "0" ? 0 : Convert.ToInt32(_q["moid"]);
		jd = string.IsNullOrEmpty(_q["jd"]) || _q["jd"] == "0" ? 0 : Convert.ToInt32(_q["jd"]);

		if (moid == 0)
		{
			Toolbox.FriendlyException(Response, "Offer not defined", "/default.aspx");
		}
		mo = new NeMemberOffer(Convert.ToInt32(moid));


		#region check credentials
		if (moid != 0) // if we're editing an existing offer
		{
			var user = new NeMember(mo.memberid);
			var comp_id = user.business_unit_id;
		    isowner = NeMember.is_owner(current_user.id, user.business_unit.tax_entity_id);

            if (!mo.isapplicant)
			{



				//				if (user.business_unit_id != current_user.business_unit_id && user.business_unit_id != 0 && !can_see_all_offers)
				if (user.business_unit_id != current_user.business_unit_id && !isowner && !NeMember.is_supervisor(user.id, current_user.id) && (current_user.id != user.id) && !can_see_all_offers)
				{
					Toolbox.FriendlyPopup(Response, "You can only access users from your branch", "./index.aspx?id=", "Not Authorized");
				}
			}


			if (!mo.isapplicant && !isowner && !NeMember.is_supervisor(user.id, current_user.id) && mo.memberid != current_user.id && !can_see_all_offers)  // if the current user is over the offer_user, let them through no matter what
			{
				if (current_user.id != new NeBusinessUnit(mo.business_unit_id).branch_manager.id && (mo.enteredby != current_user.id) && !can_see_all_offers && (mo.memberid != current_user.id) && (mo.reports_to != current_user.id) && (user.reports_to != current_user.id))
				{
					Toolbox.FriendlyPopup(Response, "Sorry, you do not have the credentials to view this page", "./index.aspx?id=", "Not Authorized");
				}
			}

		}


		#endregion

		//	if (!IsPostBack)
		//	{
		btn_view_unsigned.Visible = false;
		btn_view_signed.Visible = false;

		var file_id_unsigned = 0;
		var file_id_signed = 0;
        var showPDF = false;
		if (!IsPostBack)
		{
            //Check the Status
            var offerStatus = mo.status;
            file_id_unsigned =NeMemberOffer.GetUnSignedOfferFileID(moid);
            file_id_signed = NeMemberOffer.GetSignedOfferFileID(moid);
            // add released to create a pdf
            if (mo.status == "Accepted" || mo.status == "Awaiting Start Date" || mo.status == "Previous" || mo.status == "Released" || mo.status == "Approved")
            {
                // Generated or refreshes PDF
               var f = file_id_unsigned == 0? new file_store.fileObj(): new file_store.fileObj(file_id_unsigned);
                if (f.id == 0 || f.dt < mo.last_modified )// if the unsigned is not present or there is a new offer

                {
                    #region save a version into the filestore
                    var stream = new MemoryStream();
                    var e_info = new emp_offer();
                    var eo = (emp_offer)NeMemberOffer.fill_report(e_info, mo.id, 0);
                    e_info.CreateDocument();
                    e_info.PrintingSystem.ExportToPdf(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    var fileLength = Convert.ToInt32(stream.Length);
                    var rawdata = new byte[fileLength];
                    stream.Read(rawdata, 0, fileLength);                   
                    f.page_id = 127;
                    f.folder_id = 4;
                    f.sub_folder_id = mo.id;                  
                    f.name = "Employment Agreement " + mo.id;
                    f.ext = "pdf";
                    f.mime = NeFiles.GetMimeType(f.ext);
                    f.content = rawdata;
                    f.save();
                    #endregion
                    file_id_unsigned = f.id;

                }
            }
            switch (offerStatus)
            {
                case "In Development":
                case "Waiting for Approval":
                  if (file_id_unsigned != 0)
                    {
                        btn_view_unsigned.Visible = true;
                        showPDF = true;
                    }
                    break;
                case "Closed"://check if pdf or else show web page generated offer
                case "Revoked":
                  if (file_id_unsigned != 0)
                    {
                        btn_view_unsigned.Visible = true;
                        showPDF = true;
                    }
                    else  if (file_id_signed != 0)
                    {
                        btn_view_signed.Visible = true;
                        showPDF = false;
                    }
                     
                    break;
                case "Approved":
                case "Released":
                case "Accepted":
                case "Awaiting Start Date":
                case "Previous":
                case "Past":
                  if (file_id_signed != 0)
                    {
                        btn_view_signed.Visible = true;
                        showPDF = false;
                    }
                    break;
                    // Default is not required

            }           

		}

		if (file_id_signed != 0 && showPDF ==false)
		{
			div_pdf_warning.Visible = false;
			rv_toolbar.Visible = false;
			Page.ClientScript.RegisterStartupScript(this.GetType(), "AKey", "window.location='/_tools/get_file/index.aspx?file_id=" + file_id_signed + "';", true);
			return;
		}

		if (file_id_unsigned != 0 && showPDF == true)
		{
			div_pdf_warning.Visible = false;
			rv_toolbar.Visible = false;
			Page.ClientScript.RegisterStartupScript(this.GetType(), "AKey", "window.location='/_tools/get_file/index.aspx?file_id=" + file_id_unsigned + "';", true);
			return;
		}

		if (mo.vendor_id == 0)
		{
			var e_info = new emp_offer();
			rv.Report = (emp_offer)NeMemberOffer.fill_report(e_info, mo.id, jd);
		}
		else
		{
			var e_info = new subcontract_employment();
			rv.Report = (subcontract_employment)NeMemberOffer.fill_report_sub(e_info, mo);
		}
        if (!mo.isapplicant)
        {
            rv.Report.Name = "EA - " + new NeMember(Convert.ToInt32(mo.memberid)).FullName + " " + mo.startdate.ToString("yyyy-MM-dd");
        }
        else
        {
            rv.Report.Name = "EA - " + new NeApplicant(mo.applicantid).firstname + " " +  new NeApplicant(mo.applicantid).lastname + " " + mo.startdate.ToString("yyyy-MM-dd");
        }
		//	}
	}




	protected void Button1_Click(object sender, EventArgs e)//
	{
		var _tools = new Toolbox();
		var file_id = _tools.getSQL_int(@"Select ifnull((Select id from filestore.files  where folder_id=3 and sub_folder_id =@v0),0) ", new object[] { mo.id });
		if (file_id != 0)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "AKey", "window.location='/_tools/get_file/index.aspx?file_id=" + file_id + "';", true);
			return;
		}
	}
	protected void Button2_Click(object sender, EventArgs e)
	{
		var _tools = new Toolbox();

		var file_id = _tools.getSQL_int(@"Select ifnull((Select id from filestore.files  where folder_id=4 and sub_folder_id =@v0),0) ", new object[] { mo.id });
		if (file_id != 0)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "AKey", "window.location='/_tools/get_file/index.aspx?file_id=" + file_id + "';", true);
			return;
		}

	}
}