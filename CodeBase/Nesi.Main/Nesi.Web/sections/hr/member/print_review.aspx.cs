using System;
using DevExpress.XtraReports.UI;
using System.Collections.Specialized;
using nesi.core;
using nesi.core.print;

public partial class sections_hr_member_print_review : System.Web.UI.Page
	{
	NeMember current_user;
	Toolbox _tools;
	int rid;
	int locked;
	int mid;
	int mtid;
	int is_worksheet;
	protected void Page_Load(object sender, EventArgs e)
		{
			_tools = new Toolbox();
		//current_user = Toolbox.do_handle_authentication("47", "128");
		current_user = Toolbox.do_handle_authentication(1); 
		var _q	= Request.QueryString;
		rid					= string.IsNullOrEmpty(_q["rid"]) || _q["rid"] == "0" ? 0 : Convert.ToInt32(_q["rid"]);

		if ((rid!=null)&&(rid.ToString() != "0"))
		{
			var review = new NeEmpReview(Convert.ToInt32(rid));
			if (
					!((review.reviewed_by_id == current_user.id) ||
						(current_user.AuthenticatedForPrivilege(144)) ||
					   (NeMember.is_supervisor(review.member_id, current_user.id)) ||
					  ((review.member_id == current_user.id) && (review.status == "Delivered"))))
			{
				Toolbox.FriendlyPopup(Response, "You are not allowed to see this review", "./index.aspx?id=","Not Authorized");
			}
		}

		if ((_q["is_worksheet"] == null)||(_q["is_worksheet"]==""))
		{
			is_worksheet = 99;
		}
		else
		{
			is_worksheet = Convert.ToInt32(_q["is_worksheet"]);
		}
		if ((_q["locked"] == null) || (_q["locked"] == ""))
		{
			locked = 99;
		}
		else
		{
			locked = string.IsNullOrEmpty(_q["locked"]) || _q["locked"] == "0" ? 0 : Convert.ToInt32(_q["locked"]);
		}


		
		mid = string.IsNullOrEmpty(_q["mid"]) || _q["mid"] == "0" ? 0 : Convert.ToInt32(_q["mid"]);
		mtid = string.IsNullOrEmpty(_q["mtid"]) || _q["mtid"] == "0" ? 0 : Convert.ToInt32(_q["mtid"]);
		if(rid == 0)
			{
	//		Toolbox.FriendlyException(Response, "Could not load review data", "/default.aspx");
			}

		var e_info = new emp_review();
		rv.Report = (emp_review)fill_report(e_info);
		rv.DataBind();
		}
	private object fill_report(emp_review report)
		{
		var lblheader     = report.FindControl("lblheader", true) as XRLabel;
		var lblfooterdate = report.FindControl("lblfooterdate", true) as XRLabel;
		var lbldate = report.FindControl("lbldate", true) as XRLabel;
		var lblby = report.FindControl("lblby", true) as XRLabel;
		var lblmt = report.FindControl("lblmt", true) as XRLabel;
		var lblname = report.FindControl("lblname", true) as XRLabel;
		var xrRichText1 = report.FindControl("xrRichText1", true) as XRRichText;
		var xrPictureBox1 = report.FindControl("xrPictureBox1", true) as XRPictureBox;


        if (rid != 0)
		{
			var review = new NeEmpReview(Convert.ToInt32(rid));
			this.Title = "Employee Review for " + review.member.FullName + " - " + review.date.ToString("yyyy-MM-dd");
			if (locked == 99)
			{
				locked = review.locked;
			}
			if (is_worksheet == 99)
			{
				is_worksheet = locked == 0 ? 1 : 0;
			}
		    xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + review.member.business_unit.logo_file;

            lblheader.Text = "Review worksheet for " + review.member.FullName;
			lblfooterdate.Text = "Review Date: " + review.date.ToString("yyyy-MM-dd");
			lbldate.Text = review.date.ToString("yyyy-MM-dd");
			lblby.Text = review.reviewed_by.FullName;
			lblmt.Text = review.mt.name;
			lblname.Text = review.member.FullName;
		}
		else
		{
			if (mtid != 0)
			{
				var mt = new NeMemberType(mtid);
				lblheader.Text = "Review worksheet for " + mt.MemberTypeName;
				lblfooterdate.Text = "Printed Date: " + System.DateTime.Today.ToString("yyyy-MM-dd");
				lbldate.Text = "Not Scheduled";
				lblby.Text = current_user.FullName;
				lblmt.Text = mt.MemberTypeName;
				lblname.Text = "Generic Review";
			    xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + current_user.business_unit.logo_file;
            }
			else if(mid > 0)
			{
				var m = new NeMember(Convert.ToInt32(mid));
				lblheader.Text = "Review worksheet for " + m.FullName;
				lblfooterdate.Text = "Printed Date: " + System.DateTime.Today.ToString("yyyy-MM-dd");
				lbldate.Text = "Not Scheduled";
				lblby.Text = "Note Scheduled";
				lblmt.Text = m.membertype.MemberTypeName;
				lblname.Text = m.FullName;
			    xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + m.business_unit.logo_file;
            }
		}
		report.Parameters["id"].Value = Convert.ToInt32(rid);
		report.Parameters["isworksheet"].Value = is_worksheet.ToString();
		report.Parameters["mid"].Value = mid;
		report.Parameters["mtid"].Value = mtid;

		

		return report;
		}
	}